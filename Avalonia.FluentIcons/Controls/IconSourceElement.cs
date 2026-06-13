// Adapted from WPF UI (https://github.com/lepoco/wpfui)
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.

using Avalonia.Controls;

namespace Avalonia.FluentIcons;

/// <summary>
/// Represents an icon that uses an <see cref="IconSource"/> as its content.
/// </summary>
public class IconSourceElement : IconElement
{
    /// <summary>Identifies the <see cref="IconSource"/> styled property.</summary>
    public static readonly StyledProperty<IconSource?> IconSourceProperty =
        AvaloniaProperty.Register<IconSourceElement, IconSource?>(nameof(IconSource));

    /// <summary>
    /// Gets or sets the <see cref="IconSource"/>.
    /// </summary>
    public IconSource? IconSource
    {
        get => GetValue(IconSourceProperty);
        set => SetValue(IconSourceProperty, value);
    }

    /// <summary>
    /// Creates an <see cref="IconElement"/> from the current <see cref="IconSource"/>.
    /// </summary>
    public IconElement? CreateIconElement()
    {
        return IconSource?.CreateIconElement();
    }
}
