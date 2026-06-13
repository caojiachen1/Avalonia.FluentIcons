using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.FluentIcons.Gallery.Services;
using Avalonia.FluentIcons.Gallery.ViewModels;
using FluentAvalonia.UI.Controls;

namespace Avalonia.FluentIcons.Gallery;

public partial class MainWindow : Window
{
    private readonly MainViewModel _vm = new();
    private string? _currentTag;

    public MainWindow()
    {
        InitializeComponent();
        ApplyLanguage();
        LocalizationService.LanguageChanged += ApplyLanguage;

        var navView = this.FindControl<NavigationView>("NavView")!;
        if (navView.MenuItems is IList<object> items && items.Count > 1)
        {
            navView.SelectedItem = items[1];
        }
    }

    private void ApplyLanguage()
    {
        var t = LocalizationService.T;
        this.FindControl<TextBlock>("NavHomeText")!.Text = t("nav.home");
        this.FindControl<TextBlock>("NavIconsText")!.Text = t("nav.icons");
        this.FindControl<TextBlock>("NavFontsText")!.Text = t("nav.fonts");
        this.FindControl<TextBlock>("NavTutorialText")!.Text = t("nav.tutorial");
        this.FindControl<TextBlock>("NavSettingsText")!.Text = t("nav.settings");

        if (_currentTag != null)
        {
            var headerTitle = this.FindControl<TextBlock>("HeaderTitleText")!;
            headerTitle.Text = GetTitleByKey(_currentTag, t);
        }
    }

    private void NavView_SelectionChanged(object? sender, NavigationViewSelectionChangedEventArgs e)
    {
        var contentArea = this.FindControl<ContentControl>("ContentArea")!;
        var headerTitle = this.FindControl<TextBlock>("HeaderTitleText")!;
        var t = LocalizationService.T;

        if (e.SelectedItem is NavigationViewItem item)
        {
            _currentTag = item.Tag?.ToString();
            switch (_currentTag)
            {
                case "home":
                    _vm.NavigateToHome();
                    headerTitle.Text = t("home.title");
                    break;
                case "icons":
                    _vm.NavigateToIcons();
                    headerTitle.Text = t("nav.icons");
                    break;
                case "fonts":
                    _vm.NavigateToFonts();
                    headerTitle.Text = t("fonts.title");
                    break;
                case "tutorial":
                    _vm.NavigateToTutorial();
                    headerTitle.Text = t("tutorial.title");
                    break;
                case "settings":
                    _vm.NavigateToSettings();
                    headerTitle.Text = t("settings.title");
                    break;
            }
            contentArea.Content = _vm.CurrentPage;
        }
    }

    private static string GetTitleByKey(string tag, Func<string, string> t)
    {
        return tag switch
        {
            "home" => t("home.title"),
            "icons" => t("nav.icons"),
            "fonts" => t("fonts.title"),
            "tutorial" => t("tutorial.title"),
            "settings" => t("settings.title"),
            _ => ""
        };
    }
}
