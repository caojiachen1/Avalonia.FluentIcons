using Avalonia.Controls;
using Avalonia.FluentIcons.Gallery.Services;

namespace Avalonia.FluentIcons.Gallery;

public partial class TutorialPage : UserControl
{
    public TutorialPage()
    {
        InitializeComponent();
        ApplyLanguage();
        LocalizationService.LanguageChanged += ApplyLanguage;
    }

    private void ApplyLanguage()
    {
        var t = LocalizationService.T;
        this.FindControl<TextBlock>("Step1Title")!.Text = t("tutorial.step1_title");
        this.FindControl<TextBlock>("Step1Desc")!.Text = t("tutorial.step1_desc");
        this.FindControl<TextBlock>("Step1Note")!.Text = t("tutorial.step1_note");
        this.FindControl<TextBlock>("Step2Title")!.Text = t("tutorial.step2_title");
        this.FindControl<TextBlock>("Step2Desc")!.Text = t("tutorial.step2_desc");
        this.FindControl<TextBlock>("Step3Title")!.Text = t("tutorial.step3_title");
        this.FindControl<TextBlock>("BasicUsageLabel")!.Text = t("tutorial.basic_usage");
        this.FindControl<TextBlock>("Step4Title")!.Text = t("tutorial.step4_title");
        this.FindControl<TextBlock>("FontsizeDesc")!.Text = t("tutorial.fontsize_desc");
        this.FindControl<TextBlock>("ForegroundDesc")!.Text = t("tutorial.foreground_desc");
        this.FindControl<TextBlock>("Step5Title")!.Text = t("tutorial.step5_title");
        this.FindControl<TextBlock>("Step6Title")!.Text = t("tutorial.step6_title");
        this.FindControl<TextBlock>("Step7Title")!.Text = t("tutorial.step7_title");
        this.FindControl<TextBlock>("Step7Desc")!.Text = t("tutorial.step7_desc");
        this.FindControl<TextBlock>("AvailControlsTitle")!.Text = t("tutorial.available_controls");
        this.FindControl<TextBlock>("CtrlSymbolIconDesc")!.Text = t("tutorial.ctrl_symbolicon_desc");
        this.FindControl<TextBlock>("CtrlFontIconDesc")!.Text = t("tutorial.ctrl_fonticon_desc");
        this.FindControl<TextBlock>("CtrlImageIconDesc")!.Text = t("tutorial.ctrl_imageicon_desc");
        this.FindControl<TextBlock>("CtrlIconSourceDesc")!.Text = t("tutorial.ctrl_iconsource_desc");
    }
}
