using gir.net.Application.Interfaces.Services;
using gir.net.Configurations;
using gir.net.Domain.Entities;
using gir.net.Domain.Exceptions;
using gir.net.Infra;
using gir.net.Infra.Permissions.Preconditions;
using NetCord.Rest;
using NetCord.Services.ApplicationCommands;

namespace gir.net.Modules.Filter;

[SlashCommand("filter", "Manage filter")]
public class ManageFilterModule(IFilterService filterService) : GIRBaseCommandModule
{
    [RequirePermission<GIRContext>(PermissionLevel.Moderator)]
    [SubSlashCommand("add", "Add a word to the filter")]
    public async Task<InteractionMessageProperties> AddWord(
        [SlashCommandParameter(Name = "word", Description = "Word to add")] string word,
        [SlashCommandParameter(Name = "bypass", Description = "Permission level that bypasses word")]
        PermissionLevel bypassLevel,
        [SlashCommandParameter(Name = "notify", Description = "Ping moderators when this filter is triggered")]
        bool notify)
    {
        var filterWord = new FilterWord()
        {
            Phrase = word.ToLower(),
            BypassLevel = (int)bypassLevel,
            Notify = notify
        };

        try
        {
            await filterService.AddFilterWordAsync(filterWord);
        }
        catch (DuplicateFilerWordException)
        {
            return ErrorResponse($"'{word}' is already filtered.");
        }

        return SuccessResponse($"Added new word to the filter!", $"This filter {(filterWord.Notify ? "will" : "will not")} ping for reports, level {(PermissionLevel)filterWord.BypassLevel} can bypass it, and the phrase is `{filterWord.Phrase}`");
    }
}