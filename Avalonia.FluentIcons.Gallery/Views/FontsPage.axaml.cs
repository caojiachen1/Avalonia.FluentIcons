using Avalonia.Controls;
using Avalonia.FluentIcons.Gallery.Services;

namespace Avalonia.FluentIcons.Gallery;

public partial class FontsPage : UserControl
{
    public FontsPage()
    {
        InitializeComponent();
        ApplyLanguage();
        LocalizationService.LanguageChanged += ApplyLanguage;
    }

    private void ApplyLanguage()
    {
        var t = LocalizationService.T;
        this.FindControl<TextBlock>("RegularDesc")!.Text = t("fonts.regular_desc");
        this.FindControl<TextBlock>("FilledDesc")!.Text = t("fonts.filled_desc");
        this.FindControl<TextBlock>("ColorCustomTitle")!.Text = t("fonts.color_custom");
    }
}
