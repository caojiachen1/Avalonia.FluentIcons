using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using FluentIcons.Gallery.Services;

namespace FluentIcons.Gallery;

public partial class SettingsPage : UserControl
{
    public SettingsPage()
    {
        InitializeComponent();
        SyncComboWithService();
        ApplyLanguage();
        LocalizationService.LanguageChanged += ApplyLanguage;
    }

    private void SyncComboWithService()
    {
        var combo = this.FindControl<ComboBox>("LanguageCombo")!;
        var lang = LocalizationService.CurrentLanguage;
        for (int i = 0; i < combo.Items.Count; i++)
        {
            if (combo.Items[i] is ComboBoxItem item && item.Tag?.ToString() == lang)
            {
                combo.SelectedIndex = i;
                break;
            }
        }
    }

    private void ApplyLanguage()
    {
        var t = LocalizationService.T;
        this.FindControl<TextBlock>("LangTitle")!.Text = t("settings.language_title");
        this.FindControl<TextBlock>("LangDesc")!.Text = t("settings.language_desc");
        this.FindControl<TextBlock>("AboutTitle")!.Text = t("settings.about");
        this.FindControl<TextBlock>("AboutDesc1")!.Text = t("settings.about_desc1");
        this.FindControl<TextBlock>("RepoButtonText")!.Text = t("settings.repo_button");
    }

    private void LanguageCombo_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (this.FindControl<ComboBox>("LanguageCombo")?.SelectedItem is ComboBoxItem item)
        {
            LocalizationService.CurrentLanguage = item.Tag?.ToString() ?? "zh-CN";
        }
    }

    private void RepoButton_Click(object? sender, RoutedEventArgs e)
    {
        OpenUrl("https://github.com/davidxuang/FluentIcons");
    }

    private static void OpenUrl(string url)
    {
        try
        {
            var psi = new System.Diagnostics.ProcessStartInfo(url) { UseShellExecute = true };
            System.Diagnostics.Process.Start(psi);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Failed to open URL: {ex.Message}");
        }
    }
}
