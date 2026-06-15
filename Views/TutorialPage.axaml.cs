using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using FluentIcons.Gallery.Services;

namespace FluentIcons.Gallery;

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
        GettingStartedTitle!.Text = t("tutorial.getting_started");
        GettingStartedDesc!.Text = t("tutorial.getting_started_desc");
        InstallTitle!.Text = t("tutorial.install_title");
        InstallDesc!.Text = t("tutorial.install_desc");
        InstallCmd!.Text = t("tutorial.install_cmd");
        XamlUsageTitle!.Text = t("tutorial.xaml_usage_title");
        XamlUsageDesc!.Text = t("tutorial.xaml_usage_desc");
        XamlBasicTitle!.Text = t("tutorial.xaml_basic_title");
        XamlBasicDesc!.Text = t("tutorial.xaml_basic_desc");
        VariantsTitle!.Text = t("tutorial.variants_title");
        VariantsDesc!.Text = t("tutorial.variants_desc");
        VariantRegular!.Text = t("tutorial.variant_regular");
        VariantFilled!.Text = t("tutorial.variant_filled");
        VariantColor!.Text = t("tutorial.variant_color");
        VariantLight!.Text = t("tutorial.variant_light");
        SizingTitle!.Text = t("tutorial.sizing_title");
        SizingDesc!.Text = t("tutorial.sizing_desc");
        CSharpTitle!.Text = t("tutorial.csharp_usage_title");
        CSharpDesc!.Text = t("tutorial.csharp_usage_desc");
        SearchTipsTitle!.Text = t("tutorial.search_tips_title");
        SearchTipsDesc!.Text = t("tutorial.search_tips_desc");
        Tip1!.Text = t("tutorial.tip1");
        Tip2!.Text = t("tutorial.tip2");
        Tip3!.Text = t("tutorial.tip3");
        Tip4!.Text = t("tutorial.tip4");
        MoreInfoTitle!.Text = t("tutorial.more_info_title");
        MoreInfoDesc!.Text = t("tutorial.more_info_desc");
        RepoButtonText!.Text = t("tutorial.repo_button");
    }

    private void RepoButton_Click(object? sender, RoutedEventArgs e)
    {
        try
        {
            var psi = new System.Diagnostics.ProcessStartInfo("https://github.com/davidxuang/FluentIcons") { UseShellExecute = true };
            System.Diagnostics.Process.Start(psi);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Failed to open URL: {ex.Message}");
        }
    }
}
