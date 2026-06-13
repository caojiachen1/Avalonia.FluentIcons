using Avalonia.Controls;
using Avalonia.FluentIcons.Gallery.Services;

namespace Avalonia.FluentIcons.Gallery;

public partial class HomePage : UserControl
{
    public HomePage()
    {
        InitializeComponent();
        ApplyLanguage();
        LocalizationService.LanguageChanged += ApplyLanguage;
    }

    private void ApplyLanguage()
    {
        var t = LocalizationService.T;
        this.FindControl<TextBlock>("QuickPreviewTitle")!.Text = t("home.quick_preview");
        this.FindControl<TextBlock>("GetStartedTitle")!.Text = t("home.get_started");
        this.FindControl<TextBlock>("Step1Text")!.Text = t("home.step1");
        this.FindControl<TextBlock>("Step2Text")!.Text = t("home.step2");
        this.FindControl<TextBlock>("Step3Text")!.Text = t("home.step3");
        this.FindControl<TextBlock>("Feat4000Title")!.Text = t("home.feat_4000");
        this.FindControl<TextBlock>("Feat4000Desc")!.Text = t("home.feat_4000_desc");
        this.FindControl<TextBlock>("FeatSwapTitle")!.Text = t("home.feat_swap");
        this.FindControl<TextBlock>("FeatSwapDesc")!.Text = t("home.feat_swap_desc");
        this.FindControl<TextBlock>("FeatCustomTitle")!.Text = t("home.feat_custom");
        this.FindControl<TextBlock>("FeatCustomDesc")!.Text = t("home.feat_custom_desc");
        this.FindControl<TextBlock>("FooterText")!.Text = t("home.footer");
    }
}
