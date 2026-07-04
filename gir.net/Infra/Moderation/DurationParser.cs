using System.Text.RegularExpressions;

namespace gir.net.Infra.Moderation;

public static partial class DurationParser
{
    [GeneratedRegex(
        @"(\d+)\s*(s|sec|secs|second|seconds|m|min|mins|minute|minutes|h|hr|hrs|hour|hours|d|day|days|w|week|weeks)",
        RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)]
    private static partial Regex DurationTokenPattern();

    public static bool TryParse(string input, out TimeSpan duration)
    {
        duration = TimeSpan.Zero;
        if (string.IsNullOrWhiteSpace(input))
            return false;

        var matches = DurationTokenPattern().Matches(input.Trim());
        if (matches.Count == 0)
            return false;

        foreach (Match match in matches)
        {
            var amount = int.Parse(match.Groups[1].Value);
            if (amount <= 0)
                return false;

            duration += ParseUnit(amount, match.Groups[2].Value);
        }

        return duration > TimeSpan.Zero;
    }

    public static string Format(TimeSpan duration)
    {
        if (duration.TotalSeconds < 60)
        {
            var seconds = (int)Math.Round(duration.TotalSeconds);
            return seconds == 1 ? "1 second" : $"{seconds} seconds";
        }

        if (duration.TotalMinutes < 60)
        {
            var minutes = (int)Math.Round(duration.TotalMinutes);
            return minutes == 1 ? "1 minute" : $"{minutes} minutes";
        }

        if (duration.TotalHours < 24)
        {
            var hours = (int)Math.Round(duration.TotalHours);
            return hours == 1 ? "1 hour" : $"{hours} hours";
        }

        var days = (int)Math.Round(duration.TotalDays);
        return days == 1 ? "1 day" : $"{days} days";
    }

    private static TimeSpan ParseUnit(int amount, string unit) =>
        unit.ToLowerInvariant() switch
        {
            "s" or "sec" or "secs" or "second" or "seconds" => TimeSpan.FromSeconds(amount),
            "m" or "min" or "mins" or "minute" or "minutes" => TimeSpan.FromMinutes(amount),
            "h" or "hr" or "hrs" or "hour" or "hours" => TimeSpan.FromHours(amount),
            "d" or "day" or "days" => TimeSpan.FromDays(amount),
            "w" or "week" or "weeks" => TimeSpan.FromDays(amount * 7),
            _ => TimeSpan.Zero,
        };
}
