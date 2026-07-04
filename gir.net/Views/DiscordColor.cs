using Color = System.Drawing.Color;

namespace gir.net.Views;

/// <summary>
/// discord.py <c>Colour</c> values from
/// https://github.com/Rapptz/discord.py/blob/master/discord/colour.py
/// </summary>
public static class DiscordColor
{
    public const int DefaultValue = 0x000000;
    public const int TealValue = 0x1ABC9C;
    public const int DarkTealValue = 0x11806A;
    public const int BrandGreenValue = 0x57F287;
    public const int GreenValue = 0x2ECC71;
    public const int DarkGreenValue = 0x1F8B4C;
    public const int BlueValue = 0x3498DB;
    public const int DarkBlueValue = 0x206694;
    public const int PurpleValue = 0x9B59B6;
    public const int DarkPurpleValue = 0x71368A;
    public const int MagentaValue = 0xE91E63;
    public const int DarkMagentaValue = 0xAD1457;
    public const int GoldValue = 0xF1C40F;
    public const int DarkGoldValue = 0xC27C0E;
    public const int OrangeValue = 0xE67E22;
    public const int DarkOrangeValue = 0xA84300;
    public const int BrandRedValue = 0xED4245;
    public const int RedValue = 0xE74C3C;
    public const int DarkRedValue = 0x992D22;
    public const int LighterGreyValue = 0x95A5A6;
    public const int DarkGreyValue = 0x607D8B;
    public const int LightGreyValue = 0x979C9F;
    public const int DarkerGreyValue = 0x546E7A;
    public const int OgBlurpleValue = 0x7289DA;
    public const int BlurpleValue = 0x5865F2;
    public const int GreypleValue = 0x99AAB5;
    public const int FuchsiaValue = 0xEB459E;
    public const int YellowValue = 0xFEE75C;
    public const int PinkValue = 0xEB459F;

    public static Color Default => FromValue(DefaultValue);
    public static Color Teal => FromValue(TealValue);
    public static Color DarkTeal => FromValue(DarkTealValue);
    public static Color BrandGreen => FromValue(BrandGreenValue);
    public static Color Green => FromValue(GreenValue);
    public static Color DarkGreen => FromValue(DarkGreenValue);
    public static Color Blue => FromValue(BlueValue);
    public static Color DarkBlue => FromValue(DarkBlueValue);
    public static Color Purple => FromValue(PurpleValue);
    public static Color DarkPurple => FromValue(DarkPurpleValue);
    public static Color Magenta => FromValue(MagentaValue);
    public static Color DarkMagenta => FromValue(DarkMagentaValue);
    public static Color Gold => FromValue(GoldValue);
    public static Color DarkGold => FromValue(DarkGoldValue);
    public static Color Orange => FromValue(OrangeValue);
    public static Color DarkOrange => FromValue(DarkOrangeValue);
    public static Color BrandRed => FromValue(BrandRedValue);
    public static Color Red => FromValue(RedValue);
    public static Color DarkRed => FromValue(DarkRedValue);
    public static Color LighterGrey => FromValue(LighterGreyValue);
    public static Color DarkGrey => FromValue(DarkGreyValue);
    public static Color LightGrey => FromValue(LightGreyValue);
    public static Color DarkerGrey => FromValue(DarkerGreyValue);
    public static Color OgBlurple => FromValue(OgBlurpleValue);
    public static Color Blurple => FromValue(BlurpleValue);
    public static Color Greyple => FromValue(GreypleValue);
    public static Color Fuchsia => FromValue(FuchsiaValue);
    public static Color Yellow => FromValue(YellowValue);
    public static Color Pink => FromValue(PinkValue);

    public static Color FromValue(int value)
    {
        var r = (value >> 16) & 0xFF;
        var g = (value >> 8) & 0xFF;
        var b = value & 0xFF;
        return Color.FromArgb(0xFF, r, g, b);
    }
}
