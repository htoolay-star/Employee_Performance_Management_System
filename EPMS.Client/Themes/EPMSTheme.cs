using MudBlazor;
using static MudBlazor.CategoryTypes;

namespace EPMS.Client.Themes
{
    public static class EPMSTheme
    {
        public static MudTheme GetTheme()
        {
            return new MudTheme()
            {
                PaletteLight = new PaletteLight()
                {
                    Primary = "#0066CC",
                    PrimaryDarken = "#004499",
                    PrimaryLighten = "#E6F0FF",
                    Secondary = "#9AA0A6",
                    AppbarBackground = Colors.Shades.White,
                    AppbarText = Colors.Shades.Black,
                    Background = Colors.Gray.Lighten5,
                    TextPrimary = Colors.Shades.Black,
                    TextSecondary = Colors.Gray.Darken1,
                    DrawerBackground = Colors.Shades.White,
                    DrawerText = Colors.Shades.Black,
                    Surface = Colors.Shades.White,
                    ActionDefault = Colors.Gray.Default,
                    ActionDisabled = Colors.Gray.Lighten1,
                    ActionDisabledBackground = Colors.Gray.Lighten3,
                    DarkLighten = Colors.Gray.Lighten4,
                    DarkDarken = Colors.Gray.Darken4,
                    Dark = Colors.Gray.Darken3,
                    Info = Colors.Blue.Default,
                    Success = Colors.Green.Default,
                    Warning = Colors.Orange.Default,
                    Error = Colors.Red.Default,
                    TextDisabled = Colors.Gray.Lighten1,
                    LinesDefault = Colors.Gray.Lighten2,
                    LinesInputs = Colors.Gray.Lighten1,
                    TableLines = Colors.Gray.Lighten2,
                    TableHover = Colors.Gray.Lighten4,
                    TableStriped = Colors.Gray.Lighten5,
                    Divider = Colors.Gray.Lighten2,
                    DividerLight = Colors.Gray.Lighten4,
                    HoverOpacity = 0.06,
                    GrayDefault = Colors.Gray.Default,
                    GrayLight = Colors.Gray.Lighten1,
                    GrayLighter = Colors.Gray.Lighten2,
                    GrayDark = Colors.Gray.Darken1,
                    GrayDarker = Colors.Gray.Darken2,
                    OverlayDark = "#000000B3",
                    OverlayLight = "#00000080",
                },

                PaletteDark = new PaletteDark()
                {
                    Primary = "#66B2FF",
                    PrimaryDarken = "#3399FF",
                    PrimaryLighten = "#1A3366",
                    Secondary = "#9AA0A6",
                    AppbarBackground = "#1E1E2E",
                    AppbarText = Colors.Shades.White,
                    Background = "#1E1E2E",
                    TextPrimary = Colors.Shades.White,
                    TextSecondary = Colors.Gray.Lighten1,
                    DrawerBackground = "#1A1A2E",
                    DrawerText = Colors.Shades.White,
                    Surface = "#252540",
                    ActionDefault = Colors.Gray.Lighten2,
                    ActionDisabled = Colors.Gray.Darken1,
                    ActionDisabledBackground = Colors.Gray.Darken4,
                    DarkLighten = Colors.Gray.Darken2,
                    DarkDarken = Colors.Gray.Darken4,
                    Dark = Colors.Gray.Darken3,
                    Info = Colors.Blue.Lighten1,
                    Success = Colors.Green.Lighten1,
                    Warning = Colors.Orange.Lighten1,
                    Error = Colors.Red.Lighten1,
                    TextDisabled = Colors.Gray.Darken1,
                    LinesDefault = Colors.Gray.Darken2,
                    LinesInputs = Colors.Gray.Darken2,
                    TableLines = Colors.Gray.Darken2,
                    TableHover = Colors.Gray.Darken3,
                    TableStriped = Colors.Gray.Darken3,
                    Divider = Colors.Gray.Darken2,
                    DividerLight = Colors.Gray.Darken3,
                    HoverOpacity = 0.1,
                },

                Typography = new Typography()
                {
                    Default = new DefaultTypography()
                    {
                        FontFamily = new[] { "Inter", "Roboto", "Pyidaungsu", "sans-serif" },
                        FontSize = "14px",
                        FontWeight = "400",
                    },
                    Button = new ButtonTypography()
                    {
                        TextTransform = "none",
                        FontWeight = "500",
                        FontSize = "14px"
                    }
                },

                LayoutProperties = new LayoutProperties()
                {
                    DefaultBorderRadius = "8px",
                }
            };
        }
    }
}
