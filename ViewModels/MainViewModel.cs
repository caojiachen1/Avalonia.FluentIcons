using System.ComponentModel;
using System.Runtime.CompilerServices;
using Avalonia.Controls;

namespace FluentIcons.Gallery.ViewModels;

public class MainViewModel : INotifyPropertyChanged
{
    private UserControl? _currentPage;

    public event PropertyChangedEventHandler? PropertyChanged;

    private IconBrowserPage? _iconBrowserPage;
    private TutorialPage? _tutorialPage;
    private SettingsPage? _settingsPage;

    public UserControl? CurrentPage
    {
        get => _currentPage;
        set
        {
            if (_currentPage != value)
            {
                _currentPage = value;
                OnPropertyChanged();
            }
        }
    }

    public void NavigateToIcons()
    {
        _iconBrowserPage ??= new IconBrowserPage();
        CurrentPage = _iconBrowserPage;
    }

    public void NavigateToTutorial()
    {
        _tutorialPage ??= new TutorialPage();
        CurrentPage = _tutorialPage;
    }

    public void NavigateToSettings()
    {
        _settingsPage ??= new SettingsPage();
        CurrentPage = _settingsPage;
    }

    private void OnPropertyChanged([CallerMemberName] string? name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
