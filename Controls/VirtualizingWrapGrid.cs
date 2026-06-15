using System.Collections;
using System.Collections.Specialized;
using Avalonia.Controls;
using Avalonia;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Media;
using FluentIcons.Gallery.Models;
using Avalonia.Styling;
using Avalonia.Threading;
using FluentIcons.Avalonia;
using FluentIcons.Common;

namespace FluentIcons.Gallery.Controls;

public class VirtualizingWrapGrid : Panel
{
    private IBrush _defaultBackground = new SolidColorBrush(Color.FromRgb(51, 51, 51));
    private IBrush _hoverBackground = new SolidColorBrush(Color.Parse("#14FFFFFF"));
    private IBrush _pressedBackground = new SolidColorBrush(Color.FromRgb(39, 39, 39));
    private static readonly IBrush TransparentBrush = Brushes.Transparent;

    private readonly Dictionary<int, Border> _realizedItems = new();
    private int _columns = 1;
    private double _itemWidth;
    private double _itemHeight;
    private const double ItemSize = 100;

    private List<object>? _cachedItemsList;
    private ScrollViewer? _scrollViewer;

    public static readonly StyledProperty<bool> UseSymbolModeProperty =
        AvaloniaProperty.Register<VirtualizingWrapGrid, bool>(nameof(UseSymbolMode));

    public bool UseSymbolMode
    {
        get => GetValue(UseSymbolModeProperty);
        set => SetValue(UseSymbolModeProperty, value);
    }

    public static readonly StyledProperty<IEnumerable?> ItemsSourceProperty =
        AvaloniaProperty.Register<VirtualizingWrapGrid, IEnumerable?>(nameof(ItemsSource));

    public IEnumerable? ItemsSource
    {
        get => GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }

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

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        UpdateThemeBrushes();
        _scrollViewer = FindParentScrollViewer();
        if (_scrollViewer != null)
        {
            _scrollViewer.ScrollChanged += OnScrollChanged;
            Dispatcher.UIThread.Post(RealizeVisibleItems, DispatcherPriority.Loaded);
        }
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromVisualTree(e);
        if (_scrollViewer != null)
        {
            _scrollViewer.ScrollChanged -= OnScrollChanged;
            _scrollViewer = null;
        }
    }

    private void OnScrollChanged(object? sender, ScrollChangedEventArgs e)
    {
        RealizeVisibleItems();
    }

    private void UpdateThemeBrushes()
    {
        bool isDark = ActualThemeVariant == ThemeVariant.Dark ||
                      (ActualThemeVariant == ThemeVariant.Default &&
                       Application.Current?.ActualThemeVariant == ThemeVariant.Dark);

        if (isDark)
        {
            _defaultBackground = new SolidColorBrush(Color.FromRgb(43, 43, 43));
            _hoverBackground = new SolidColorBrush(Color.Parse("#14FFFFFF"));
            _pressedBackground = new SolidColorBrush(Color.Parse("#1FFFFFFF"));
        }
        else
        {
            _defaultBackground = new SolidColorBrush(Color.FromRgb(249, 249, 249));
            _hoverBackground = new SolidColorBrush(Color.Parse("#0A000000"));
            _pressedBackground = new SolidColorBrush(Color.Parse("#14000000"));
        }

        foreach (var (_, card) in _realizedItems)
            card.Background = _defaultBackground;
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == ItemsSourceProperty)
        {
            if (change.OldValue is INotifyCollectionChanged oldCol)
                oldCol.CollectionChanged -= OnCollectionChanged;

            if (change.NewValue is INotifyCollectionChanged newCol)
                newCol.CollectionChanged += OnCollectionChanged;

            InvalidateCache();
            Invalidate();
        }
        else if (change.Property == UseSymbolModeProperty)
        {
            Invalidate();
        }
    }

    private void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        InvalidateCache();

        if (e.Action == NotifyCollectionChangedAction.Reset)
        {
            Invalidate();
            return;
        }

        if (e.Action == NotifyCollectionChangedAction.Remove && e.OldStartingIndex >= 0)
        {
            int removeCount = e.OldItems?.Count ?? 1;
            HandleRemove(e.OldStartingIndex, removeCount);
            return;
        }

        if (e.Action == NotifyCollectionChangedAction.Add && e.NewStartingIndex >= 0)
        {
            HandleAdd(e.NewStartingIndex, e.NewItems?.Count ?? 0);
            return;
        }

        if (e.Action == NotifyCollectionChangedAction.Move)
        {
            Invalidate();
            return;
        }

        Invalidate();
    }

    private void HandleRemove(int startIndex, int count)
    {
        var toRemove = new List<int>();
        foreach (var (index, _) in _realizedItems)
        {
            if (index >= startIndex && index < startIndex + count)
                toRemove.Add(index);
        }
        foreach (var key in toRemove)
        {
            Children.Remove(_realizedItems[key]);
            _realizedItems.Remove(key);
        }

        var reindex = new Dictionary<int, Border>();
        foreach (var (index, card) in _realizedItems)
        {
            int newIndex = index >= startIndex + count ? index - count : index;
            reindex[newIndex] = card;
        }
        _realizedItems.Clear();
        foreach (var (k, v) in reindex)
            _realizedItems[k] = v;

        InvalidateMeasure();
    }

    private void HandleAdd(int startIndex, int count)
    {
        var reindex = new Dictionary<int, Border>();
        var toRemove = new List<int>();
        foreach (var (index, card) in _realizedItems)
        {
            int newIndex = index >= startIndex ? index + count : index;
            reindex[newIndex] = card;
        }
        _realizedItems.Clear();
        foreach (var (k, v) in reindex)
            _realizedItems[k] = v;

        InvalidateMeasure();
    }

    private void InvalidateCache()
    {
        _cachedItemsList = null;
    }

    private void Invalidate()
    {
        ClearRealizedItems();
        InvalidateMeasure();
    }

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
        if (_scrollViewer is null)
        {
            _scrollViewer = FindParentScrollViewer();
            if (_scrollViewer is null) return;
        }

        var items = GetItemsList();
        if (items.Count == 0) return;

        double svHeight = _scrollViewer.Viewport.Height;
        if (svHeight <= 0) return;

        double offset = _scrollViewer.Offset.Y;

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

            var card = CreateCard(items[i]);
            _realizedItems[i] = card;
            Children.Add(card);

            int col = i % _columns;
            int row = i / _columns;
            card.Arrange(new Rect(col * _itemWidth, row * _itemHeight, _itemWidth, _itemHeight));
        }
    }

    private Border CreateCard(object dataContext)
    {
        Control iconControl;

        if (UseSymbolMode && dataContext is IconItem symItem)
        {
            iconControl = new SymbolIcon
            {
                Symbol = symItem.Symbol,
                FontSize = 36,
                HorizontalAlignment = HorizontalAlignment.Center,
            };
        }
        else
        {
            var fluentIcon = new FluentIcon
            {
                IconSize = IconSize.Resizable,
                FontSize = 36,
                HorizontalAlignment = HorizontalAlignment.Center,
            };
            if (dataContext is IconItem iconItem)
                fluentIcon.Icon = iconItem.Icon;
            iconControl = fluentIcon;
        }

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

        var inner = new StackPanel
        {
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Center,
            Spacing = 2,
        };
        inner.Children.Add(iconControl);
        inner.Children.Add(nameBlock);

        var card = new Border
        {
            CornerRadius = new CornerRadius(6),
            Padding = new Thickness(4, 8),
            Margin = new Thickness(3),
            Background = _defaultBackground,
            BorderBrush = TransparentBrush,
            BorderThickness = new Thickness(0),
            DataContext = dataContext,
            Child = inner,
            Cursor = new Cursor(StandardCursorType.Hand),
        };

        ToolTip.SetTip(card, GetName(dataContext));

        bool pressed = false;
        card.PointerEntered += (_, _) => card.Background = pressed ? _pressedBackground : _hoverBackground;
        card.PointerExited += (_, _) => card.Background = _defaultBackground;
        card.PointerPressed += (_, e) =>
        {
            pressed = true;
            card.Background = _pressedBackground;
            SelectedItem = card.DataContext;
            SelectionChanged?.Invoke(this, SelectedItem);
            e.Handled = true;
        };
        card.PointerReleased += (_, _) =>
        {
            pressed = false;
            card.Background = _hoverBackground;
        };
        card.PointerCaptureLost += (_, _) =>
        {
            pressed = false;
            card.Background = _defaultBackground;
        };

        return card;
    }

    private List<object> GetItemsList()
    {
        if (_cachedItemsList != null) return _cachedItemsList;
        if (ItemsSource is null) return s_empty;

        var result = new List<object>();
        foreach (var item in ItemsSource) result.Add(item);
        _cachedItemsList = result;
        return result;
    }

    private static readonly List<object> s_empty = new();

    private static string GetName(object item) =>
        item is IconItem ii ? ii.Name : item?.ToString() ?? "";

    private ScrollViewer? FindParentScrollViewer()
    {
        Control? current = Parent as Control;
        while (current != null)
        {
            if (current is ScrollViewer sv) return sv;
            current = current.Parent as Control;
        }
        return null;
    }

    private void ClearRealizedItems()
    {
        foreach (var (_, card) in _realizedItems)
            Children.Remove(card);
        _realizedItems.Clear();
    }
}
