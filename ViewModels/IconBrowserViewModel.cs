using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using FluentIcons.Gallery.Models;
using Avalonia.Input.Platform;
using FluentIcons.Common;
using Avalonia.Media;
using Avalonia;

namespace FluentIcons.Gallery.ViewModels;

public enum IconDisplayMode
{
    Fluent,
    Symbol
}

public class IconBrowserViewModel : INotifyPropertyChanged
{
    public static IReadOnlyList<IconItem> SourceIcons { get; } = LoadSourceIcons();

    private IReadOnlyList<IconItem> _icons = SourceIcons;
    private string _searchText = string.Empty;
    private bool _usesSymbol = false;
    private FlowDirection _flowDirection = FlowDirection.LeftToRight;
    private IconVariant _iconVariant = IconVariant.Regular;
    private IconItem _selected;

    public event PropertyChangedEventHandler? PropertyChanged;

    public IconBrowserViewModel()
    {
        _selected = SourceIcons[0];
        _selected.IsSelected = true;
    }

    public IReadOnlyList<IconItem> Icons
    {
        get => _icons;
        set { if (_icons != value) { _icons = value; OnPropertyChanged(); } }
    }

    public string SearchText
    {
        get => _searchText;
        set
        {
            if (_searchText != value)
            {
                _searchText = value;
                OnPropertyChanged();
                RefreshIcons();
            }
        }
    }

    public bool UsesSymbol
    {
        get => _usesSymbol;
        set
        {
            if (_usesSymbol != value)
            {
                _usesSymbol = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Xaml));
                OnPropertyChanged(nameof(XamlExtension));
                OnPropertyChanged(nameof(CSharp));
                OnPropertyChanged(nameof(CSharpMarkup));
            }
        }
    }

    public FlowDirection FlowDirection
    {
        get => _flowDirection;
        set
        {
            if (_flowDirection != value)
            {
                _flowDirection = value;
                OnPropertyChanged();
            }
        }
    }

    public IconVariant IconVariant
    {
        get => _iconVariant;
        set
        {
            if (_iconVariant != value)
            {
                _iconVariant = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Xaml));
                OnPropertyChanged(nameof(XamlExtension));
                OnPropertyChanged(nameof(CSharp));
                OnPropertyChanged(nameof(CSharpMarkup));
            }
        }
    }

    public IconItem Selected
    {
        get => _selected;
        set
        {
            if (!ReferenceEquals(_selected, value))
            {
                _selected = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Xaml));
                OnPropertyChanged(nameof(XamlExtension));
                OnPropertyChanged(nameof(CSharp));
                OnPropertyChanged(nameof(CSharpMarkup));
            }
        }
    }

    private string Prefix => _usesSymbol ? "Symbol" : "Fluent";
    private string Property => _usesSymbol ? "Symbol" : "Icon";

    public string Xaml => IconVariant == IconVariant.Regular
        ? $"<ic:{Prefix}Icon {Property}=\"{Selected?.Name}\" />"
        : $"<ic:{Prefix}Icon {Property}=\"{Selected?.Name}\" IconVariant=\"{IconVariant}\" />";

    public string XamlExtension => IconVariant == IconVariant.Regular
        ? $"{{icx:{Prefix}Icon {Property}={Selected?.Name}}}"
        : $"{{icx:{Prefix}Icon {Property}={Selected?.Name}, IconVariant={IconVariant}}}";

    public string CSharp => IconVariant == IconVariant.Regular
        ? $"new {Prefix}Icon {{ {Property} = {Property}.{Selected?.Name} }};"
        : $"new {Prefix}Icon {{ {Property} = {Property}.{Selected?.Name}, IconVariant = IconVariant.{IconVariant} }};";

    public string CSharpMarkup => IconVariant == IconVariant.Regular
        ? $"new {Prefix}Icon().{Property}({Property}.{Selected?.Name})"
        : $"new {Prefix}Icon().{Property}({Property}.{Selected?.Name}).IconVariant(IconVariant.{IconVariant})";

    public ICommand SelectIconCommand => new RelayCommand<IconItem>(SelectIcon);
    public ICommand CopyXamlCommand => new RelayCommand(() => CopyToClipboard(Xaml));
    public ICommand CopyXamlExtensionCommand => new RelayCommand(() => CopyToClipboard(XamlExtension));
    public ICommand CopyCSharpCommand => new RelayCommand(() => CopyToClipboard(CSharp));
    public ICommand CopyCSharpMarkupCommand => new RelayCommand(() => CopyToClipboard(CSharpMarkup));

    public string Version => typeof(Icon).Assembly.GetName().Version?.ToString(3) ?? "–";

    public void SelectIcon(IconItem? icon)
    {
        if (icon is null) return;
        Selected.IsSelected = false;
        icon.IsSelected = true;
        Selected = icon;
    }

    public void RefreshIcons()
    {
        if (string.IsNullOrEmpty(SearchText))
        {
            Icons = UsesSymbol
                ? SourceIcons.Where(x => x.HasSymbol).ToList()
                : SourceIcons;
        }
        else
        {
            Icons = SourceIcons
                .Where(x => (!UsesSymbol || x.HasSymbol) &&
                            x.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
    }

    private static async void CopyToClipboard(string text)
    {
        if (string.IsNullOrEmpty(text)) return;
        var clipboard = App.Clipboard;
        if (clipboard != null)
            await ClipboardExtensions.SetTextAsync(clipboard, text);
    }

    private static List<IconItem> LoadSourceIcons()
    {
        var items = new List<IconItem>();
        var iconType = typeof(Icon);
        var symbolNames = new HashSet<string>(Enum.GetNames<Symbol>());

        foreach (var value in Enum.GetValues<Icon>())
        {
            var member = iconType.GetMember(value.ToString());
            if (member.Length > 0 &&
                member[0].GetCustomAttributesData().Any(a =>
                    a.AttributeType.Name == "NonResizableAttribute"))
                continue;

            var name = value.ToString();
            Symbol sym = (Symbol)(-1);
            if (symbolNames.Contains(name))
                Enum.TryParse(name, out sym);

            items.Add(new IconItem
            {
                Name = name,
                Icon = value,
                Symbol = sym,
                CodePoint = $"0x{(int)value:X4}",
            });
        }

        items.Sort((a, b) => string.Compare(a.Name, b.Name, StringComparison.Ordinal));
        return items;
    }

    private void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
