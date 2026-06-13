using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Avalonia.FluentIcons.Gallery.Models;

namespace Avalonia.FluentIcons.Gallery.ViewModels;

public class IconBrowserViewModel : INotifyPropertyChanged
{
    private readonly List<IconItem> _allItems;
    private string _searchText = string.Empty;
    private IconItem _selectedIcon;

    public event PropertyChangedEventHandler? PropertyChanged;

    public IconBrowserViewModel()
    {
        _allItems = LoadIcons();
        FilteredItems = new ObservableCollection<IconItem>(_allItems);
        _selectedIcon = _allItems.Count > 0
            ? _allItems[0]
            : new IconItem { Name = "Empty", Symbol = SymbolRegular.Empty, CodePoint = "0x0" };
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
                ApplyFilter();
            }
        }
    }

    public IconItem SelectedIcon
    {
        get => _selectedIcon;
        set
        {
            if (!ReferenceEquals(_selectedIcon, value))
            {
                _selectedIcon = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(XamlReference));
                OnPropertyChanged(nameof(XamlFilledReference));
                OnPropertyChanged(nameof(CSharpReference));
                OnPropertyChanged(nameof(EnumReference));
            }
        }
    }

    public ObservableCollection<IconItem> FilteredItems { get; }

    public int TotalCount => _allItems.Count;
    public int FilteredCount => FilteredItems.Count;

    public string XamlReference =>
        $"<icons:SymbolIcon Symbol=\"{SelectedIcon.Name}\" />";

    public string XamlFilledReference =>
        $"<icons:SymbolIcon Symbol=\"{SelectedIcon.Name}\" Filled=\"True\" />";

    public string CSharpReference =>
        $"new SymbolIcon(SymbolRegular.{SelectedIcon.Name})";

    public string EnumReference =>
        $"SymbolRegular.{SelectedIcon.Name}";

    private void ApplyFilter()
    {
        FilteredItems.Clear();

        var query = _searchText.Trim();
        IEnumerable<IconItem> filtered = string.IsNullOrEmpty(query)
            ? _allItems
            : _allItems.Where(i => i.Name.Contains(query, StringComparison.OrdinalIgnoreCase));

        foreach (var item in filtered)
            FilteredItems.Add(item);

        OnPropertyChanged(nameof(FilteredCount));

        if (FilteredItems.Count > 0 && !FilteredItems.Contains(SelectedIcon))
            SelectedIcon = FilteredItems[0];
    }

    private static List<IconItem> LoadIcons()
    {
        var items = new List<IconItem>();
        foreach (var value in Enum.GetValues<SymbolRegular>())
        {
            if (value == SymbolRegular.Empty) continue;
            items.Add(new IconItem
            {
                Name = value.ToString(),
                Symbol = value,
                CodePoint = $"0x{(int)value:X4}",
            });
        }
        return items;
    }

    private void OnPropertyChanged([CallerMemberName] string? name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
