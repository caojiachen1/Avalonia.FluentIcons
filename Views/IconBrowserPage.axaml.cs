using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input.Platform;
using Avalonia.Interactivity;
using FluentIcons.Gallery.ViewModels;
using FluentIcons.Common;
using Avalonia.Media;
using Avalonia;

namespace FluentIcons.Gallery;

public partial class IconBrowserPage : UserControl
{
    private readonly IconBrowserViewModel _vm = new();

    public IconBrowserPage()
    {
        InitializeComponent();
        DataContext = _vm;
        UpdateLayout(Bounds.Size);
    }

    // ── Responsive layout ──────────────────────────────────────────────────────
    private bool? _compact = null;

    private void UpdateLayout(Size size)
    {
        if (size.Width >= 600)
        {
            if (_compact == false) return;

            RootGrid.ColumnDefinitions[2].MinWidth = 320;
            Grid.SetRow(Primary, 1);
            Grid.SetRow(Splitter, 1);
            Grid.SetRow(Info, 1);
            Grid.SetRowSpan(Primary, 3);
            Grid.SetRowSpan(Splitter, 3);
            Grid.SetRowSpan(Info, 3);
            Grid.SetColumn(Primary, 0);
            Grid.SetColumn(Splitter, 1);
            Grid.SetColumn(Info, 2);
            Grid.SetColumnSpan(Primary, 1);
            Grid.SetColumnSpan(Splitter, 1);
            Grid.SetColumnSpan(Info, 1);
            Splitter.ResizeDirection = GridResizeDirection.Columns;

            _compact = false;
        }
        else
        {
            if (_compact == true) return;

            RootGrid.ColumnDefinitions[2].MinWidth = 0;
            Grid.SetRow(Primary, 1);
            Grid.SetRow(Splitter, 2);
            Grid.SetRow(Info, 3);
            Grid.SetRowSpan(Primary, 1);
            Grid.SetRowSpan(Splitter, 1);
            Grid.SetRowSpan(Info, 1);
            Grid.SetColumn(Primary, 0);
            Grid.SetColumn(Splitter, 0);
            Grid.SetColumn(Info, 0);
            Grid.SetColumnSpan(Primary, 3);
            Grid.SetColumnSpan(Splitter, 3);
            Grid.SetColumnSpan(Info, 3);
            Splitter.ResizeDirection = GridResizeDirection.Rows;

            _compact = true;
        }
    }

    private void OnSizeChanged(object? sender, SizeChangedEventArgs e) => UpdateLayout(e.NewSize);

    // ── TabStrip handlers ──────────────────────────────────────────────────────

    private void UsesSymbol_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (sender is TabStrip tabs)
        {
            _vm.UsesSymbol = tabs.SelectedIndex != 0;
            _vm.RefreshIcons();
            if (_vm.UsesSymbol && !_vm.Selected.HasSymbol)
                _vm.Selected = IconBrowserViewModel.SourceIcons.FirstOrDefault(x => x.HasSymbol)!;
        }
    }

    private void FlowDirection_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (sender is TabStrip tabs)
        {
            _vm.FlowDirection = (FlowDirection)tabs.SelectedIndex;
        }
    }

    private void IconVariant_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (sender is TabStrip tabs)
        {
            _vm.IconVariant = (IconVariant)tabs.SelectedIndex;
        }
    }
}
