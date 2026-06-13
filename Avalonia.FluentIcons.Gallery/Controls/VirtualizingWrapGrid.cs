using System.Collections;
using System.Collections.Specialized;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.FluentIcons;
using Avalonia.FluentIcons.Gallery.Models;

namespace Avalonia.FluentIcons.Gallery.Controls;

/// <summary>
/// A virtualizing panel that arranges items in a wrapping grid.
/// Only creates visual elements for items within the visible viewport.
/// Each item is rendered as a lightweight Border + TextBlock using the Fluent System Icons font.
/// </summary>
public class VirtualizingWrapGrid : Panel
{
    private const string FontUriRegular =
        "avares://Avalonia.FluentIcons/Assets/Fonts/FluentSystemIcons-Regular.ttf#FluentSystemIcons-Regular";
    private const string FontUriFilled =
        "avares://Avalonia.FluentIcons/Assets/Fonts/FluentSystemIcons-Filled.ttf#FluentSystemIcons-Filled";

    private static FontFamily? _regularFont;
    private static FontFamily? _filledFont;

    private static FontFamily RegularFont => _regularFont ??= FontFamily.Parse(FontUriRegular);
    private static FontFamily FilledFont => _filledFont ??= FontFamily.Parse(FontUriFilled);

    private static readonly IBrush DefaultBackground = new SolidColorBrush(Color.FromRgb(51, 51, 51));
    private static readonly IBrush HoverBackground = new SolidColorBrush(Color.Parse("#14FFFFFF"));
    private static readonly IBrush PressedBackground = new SolidColorBrush(Color.FromRgb(39, 39, 39));
    private static readonly IBrush TransparentBrush = Brushes.Transparent;

    private readonly Dictionary<int, Border> _realizedItems = new();
    private int _columns = 1;
    private double _itemWidth;
    private double _itemHeight;
    private const double ItemSize = 100;

    // ── ItemsSource ──────────────────────────────────────────────
    public static readonly StyledProperty<IEnumerable?> ItemsSourceProperty =
        AvaloniaProperty.Register<VirtualizingWrapGrid, IEnumerable?>(nameof(ItemsSource));

    public IEnumerable? ItemsSource
    {
        get => GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }

    // ── SelectedItem ─────────────────────────────────────────────
    public static readonly DirectProperty<VirtualizingWrapGrid, object?> SelectedItemProperty =
        AvaloniaProperty.RegisterDirect<VirtualizingWrapGrid, object?>(
            nameof(SelectedItem), o => o.SelectedItem, (o, v) => o.SelectedItem = v);

    private object? _selectedItem;
    public object? SelectedItem
    {
        get => _selectedItem;
        set
        {
            if (!ReferenceEquals(_selectedItem, value))
                SetAndRaise(SelectedItemProperty, ref _selectedItem, value);
        }
    }

    public event EventHandler<object?>? SelectionChanged;

    // ── Lifecycle ────────────────────────────────────────────────

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == ItemsSourceProperty)
        {
            if (change.OldValue is INotifyCollectionChanged oldCol)
                oldCol.CollectionChanged -= OnCollectionChanged;

            if (change.NewValue is INotifyCollectionChanged newCol)
                newCol.CollectionChanged += OnCollectionChanged;

            Invalidate();
        }
    }

    private void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        Invalidate();
    }

    private void Invalidate()
    {
        ClearRealizedItems();
        InvalidateMeasure();
    }

    // ── Layout ───────────────────────────────────────────────────

    protected override Size MeasureOverride(Size availableSize)
    {
        var items = GetItemsList();
        if (items.Count == 0 || availableSize.Width <= 0)
            return default;

        _columns = Math.Max(1, (int)(availableSize.Width / ItemSize));
        _itemWidth = availableSize.Width / _columns;
        _itemHeight = _itemWidth;
        int totalRows = (items.Count + _columns - 1) / _columns;
        double totalHeight = totalRows * _itemHeight;

        return new Size(availableSize.Width, totalHeight);
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        if (_columns < 1) _columns = 1;

        foreach (var (index, card) in _realizedItems)
        {
            int col = index % _columns;
            int row = index / _columns;
            double x = col * _itemWidth;
            double y = row * _itemHeight;
            card.Arrange(new Rect(x, y, _itemWidth, _itemHeight));
        }

        RealizeVisibleItems();
        return finalSize;
    }

    private void RealizeVisibleItems()
    {
        if (Parent is not ScrollViewer sv) return;

        var items = GetItemsList();
        if (items.Count == 0) return;

        double svHeight = sv.Viewport.Height;
        double offset = sv.Offset.Y;

        int firstRow = Math.Max(0, (int)(offset / _itemHeight) - 1);
        int lastRow = (int)((offset + svHeight) / _itemHeight) + 1;
        int totalRows = (items.Count + _columns - 1) / _columns;
        lastRow = Math.Min(lastRow, totalRows - 1);

        int firstIndex = firstRow * _columns;
        int lastIndex = Math.Min((lastRow + 1) * _columns - 1, items.Count - 1);

        var toRemove = _realizedItems.Keys.Where(k => k < firstIndex || k > lastIndex).ToList();
        foreach (var key in toRemove)
        {
            Children.Remove(_realizedItems[key]);
            _realizedItems.Remove(key);
        }

        for (int i = firstIndex; i <= lastIndex; i++)
        {
            if (_realizedItems.ContainsKey(i)) continue;
            if (i >= items.Count) break;

            var card = CreateCard(i, items[i]);
            _realizedItems[i] = card;
            Children.Add(card);

            int col = i % _columns;
            int row = i / _columns;
            card.Arrange(new Rect(col * _itemWidth, row * _itemHeight, _itemWidth, _itemHeight));
        }
    }

    // ── Card Factory ─────────────────────────────────────────────

    private Border CreateCard(int index, object dataContext)
    {
        var textBlock = new TextBlock
        {
            FontFamily = RegularFont,
            FontSize = 36,
            HorizontalAlignment = HorizontalAlignment.Center,
            TextAlignment = TextAlignment.Center,
        };

        var nameBlock = new TextBlock
        {
            Text = GetName(dataContext),
            FontSize = 9,
            TextTrimming = TextTrimming.CharacterEllipsis,
            HorizontalAlignment = HorizontalAlignment.Center,
            MaxWidth = 90,
            Opacity = 0.65,
            TextAlignment = TextAlignment.Center,
            Margin = new Thickness(0, 4, 0, 0),
        };

        if (dataContext is IconItem iconItem)
            textBlock.Text = GetGlyph(iconItem.Symbol);

        var inner = new StackPanel
        {
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Center,
            Spacing = 2,
        };
        inner.Children.Add(textBlock);
        inner.Children.Add(nameBlock);

        var card = new Border
        {
            CornerRadius = new CornerRadius(6),
            Padding = new Thickness(4, 8),
            Margin = new Thickness(3),
            Background = DefaultBackground,
            BorderBrush = TransparentBrush,
            BorderThickness = new Thickness(0),
            DataContext = dataContext,
            Child = inner,
            Cursor = new Cursor(StandardCursorType.Hand),
        };

        ToolTip.SetTip(card, GetName(dataContext));

        card.Tag = textBlock; // stash reference for quick font swapping

        bool pressed = false;
        card.PointerEntered += (_, _) => card.Background = pressed ? PressedBackground : HoverBackground;
        card.PointerExited += (_, _) => card.Background = DefaultBackground;
        card.PointerPressed += (_, e) =>
        {
            pressed = true;
            card.Background = PressedBackground;
            SelectedItem = card.DataContext;
            SelectionChanged?.Invoke(this, SelectedItem);
            e.Handled = true;
        };
        card.PointerReleased += (_, _) =>
        {
            pressed = false;
            card.Background = HoverBackground;
        };
        card.PointerCaptureLost += (_, _) =>
        {
            pressed = false;
            card.Background = DefaultBackground;
        };

        return card;
    }

    // ── Helpers ──────────────────────────────────────────────────

    private List<object> GetItemsList()
    {
        if (ItemsSource is null) return s_empty;
        if (ItemsSource is List<object> list) return list;

        var result = new List<object>();
        foreach (var item in ItemsSource) result.Add(item);
        return result;
    }

    private static readonly List<object> s_empty = new();

    private static string GetName(object item) =>
        item is IconItem ii ? ii.Name : item?.ToString() ?? "";

    private static string GetGlyph(SymbolRegular symbol) =>
        symbol.GetString();

    private void ClearRealizedItems()
    {
        foreach (var (_, card) in _realizedItems)
            Children.Remove(card);
        _realizedItems.Clear();
    }
}
