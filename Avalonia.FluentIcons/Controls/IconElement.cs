// Adapted from WPF UI (https://github.com/lepoco/wpfui)
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.

using System.ComponentModel;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;

namespace Avalonia.FluentIcons;

/// <summary>
/// Represents the base class for an icon UI element.
/// </summary>
[TypeConverter(typeof(IconElementConverter))]
public abstract class IconElement : Panel
{
    /// <summary>Identifies the <see cref="Foreground"/> styled property.</summary>
    public static readonly StyledProperty<IBrush?> ForegroundProperty =
        TextBlock.ForegroundProperty.AddOwner<IconElement>();

    static IconElement()
    {
        FocusableProperty.OverrideDefaultValue<IconElement>(false);
        IsTabStopProperty.OverrideDefaultValue<IconElement>(false);
    }

    /// <inheritdoc cref="TextBlock.Foreground"/>
    [Category("Appearance")]
    public IBrush? Foreground
    {
        get => GetValue(ForegroundProperty);
        set => SetValue(ForegroundProperty, value);
    }

    /// <summary>
    /// Coerces the value of an Icon property, allowing the use of either <see cref="IconElement"/> or <see cref="IconSourceElement"/>.
    /// </summary>
    public static object? Coerce(AvaloniaObject sender, object? baseValue)
    {
        return baseValue switch
        {
            IconSourceElement iconSourceElement => iconSourceElement.CreateIconElement(),
            IconElement or null => baseValue,
            _ => throw new ArgumentException(
                $"Expected either '{typeof(IconSourceElement)}' or '{typeof(IconElement)}' but got '{baseValue.GetType()}'.",
                nameof(baseValue)),
        };
    }
}
