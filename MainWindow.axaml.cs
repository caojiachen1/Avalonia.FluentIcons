using Avalonia.Controls;
using Avalonia.Interactivity;
using FluentIcons.Gallery.Services;
using FluentIcons.Gallery.ViewModels;
using FluentAvalonia.UI.Controls;

namespace FluentIcons.Gallery;

public partial class MainWindow : Window
{
    private readonly MainViewModel _vm = new();
    private string? _currentTag;

    public MainWindow()
    {
        InitializeComponent();
        ApplyLanguage();
        LocalizationService.LanguageChanged += ApplyLanguage;

        var navView = this.FindControl<FANavigationView>("NavView")!;
        if (navView.MenuItems is IList<object> items && items.Count > 0)
        {
            navView.SelectedItem = items[0];
        }
    }

    private void ApplyLanguage()
    {
        var t = LocalizationService.T;
        this.FindControl<TextBlock>("NavIconsText")!.Text = t("nav.icons");
        this.FindControl<TextBlock>("NavTutorialText")!.Text = t("nav.tutorial");
        this.FindControl<TextBlock>("NavSettingsText")!.Text = t("nav.settings");

        if (_currentTag != null)
        {
            var headerTitle = this.FindControl<TextBlock>("HeaderTitleText")!;
            headerTitle.Text = GetTitleByKey(_currentTag, t);
        }
    }

    private void NavView_SelectionChanged(object? sender, FANavigationViewSelectionChangedEventArgs e)
    {
        var contentArea = this.FindControl<ContentControl>("ContentArea")!;
        var headerTitle = this.FindControl<TextBlock>("HeaderTitleText")!;
        var t = LocalizationService.T;

        if (e.SelectedItem is FANavigationViewItem item)
        {
            _currentTag = item.Tag?.ToString();
            switch (_currentTag)
            {
                case "icons":
                    _vm.NavigateToIcons();
                    headerTitle.Text = t("nav.icons");
                    break;
                case "tutorial":
                    _vm.NavigateToTutorial();
                    headerTitle.Text = t("nav.tutorial");
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
            "icons" => t("nav.icons"),
            "tutorial" => t("nav.tutorial"),
            "settings" => t("settings.title"),
            _ => ""
        };
    }
}
