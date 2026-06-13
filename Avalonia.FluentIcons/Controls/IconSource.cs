// Adapted from WPF UI (https://github.com/lepoco/wpfui)
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.

using Avalonia.Media;

namespace Avalonia.FluentIcons;

/// <summary>
/// Represents the base class for an icon source.
/// </summary>
public abstract class IconSource : AvaloniaObject
{
    /// <summary>Identifies the <see cref="Foreground"/> styled property.</summary>
    public static readonly StyledProperty<IBrush?> ForegroundProperty =
        AvaloniaProperty.Register<IconSource, IBrush?>(nameof(Foreground));

    /// <summary>
    /// Gets or sets the foreground brush.
    /// </summary>
    public IBrush? Foreground
    {
        get => GetValue(ForegroundProperty);
        set => SetValue(ForegroundProperty, value);
    }

    /// <summary>
    /// Creates an <see cref="IconElement"/> from this source.
    /// </summary>
    public abstract IconElement CreateIconElement();
}
