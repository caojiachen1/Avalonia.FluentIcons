// Adapted from WPF UI (https://github.com/lepoco/wpfui)
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.

using Avalonia.Media;

namespace Avalonia.FluentIcons;

/// <summary>
/// Represents an icon source that uses a Fluent System Icons symbol glyph.
/// </summary>
public class SymbolIconSource : IconSource
{
    /// <summary>Identifies the <see cref="FontSize"/> styled property.</summary>
    public static readonly StyledProperty<double> FontSizeProperty =
        AvaloniaProperty.Register<SymbolIconSource, double>(nameof(FontSize), FontIcon.DefaultIconFontSize);

    /// <summary>Identifies the <see cref="FontStyle"/> styled property.</summary>
    public static readonly StyledProperty<FontStyle> FontStyleProperty =
        AvaloniaProperty.Register<SymbolIconSource, FontStyle>(nameof(FontStyle), FontStyle.Normal);

    /// <summary>Identifies the <see cref="FontWeight"/> styled property.</summary>
    public static readonly StyledProperty<FontWeight> FontWeightProperty =
        AvaloniaProperty.Register<SymbolIconSource, FontWeight>(nameof(FontWeight), FontWeight.Normal);

    /// <summary>Identifies the <see cref="Symbol"/> styled property.</summary>
    public static readonly StyledProperty<SymbolRegular> SymbolProperty =
        AvaloniaProperty.Register<SymbolIconSource, SymbolRegular>(nameof(Symbol), SymbolRegular.Empty);

    /// <summary>Identifies the <see cref="Filled"/> styled property.</summary>
    public static readonly StyledProperty<bool> FilledProperty =
        AvaloniaProperty.Register<SymbolIconSource, bool>(nameof(Filled), false);

    public double FontSize
    {
        get => GetValue(FontSizeProperty);
        set => SetValue(FontSizeProperty, value);
    }

    public FontStyle FontStyle
    {
        get => GetValue(FontStyleProperty);
        set => SetValue(FontStyleProperty, value);
    }

    public FontWeight FontWeight
    {
        get => GetValue(FontWeightProperty);
        set => SetValue(FontWeightProperty, value);
    }

    /// <summary>
    /// Gets or sets displayed <see cref="SymbolRegular"/>.
    /// </summary>
    public SymbolRegular Symbol
    {
        get => GetValue(SymbolProperty);
        set => SetValue(SymbolProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether or not we should use the filled variant.
    /// </summary>
    public bool Filled
    {
        get => GetValue(FilledProperty);
        set => SetValue(FilledProperty, value);
    }

    /// <inheritdoc />
    public override IconElement CreateIconElement()
    {
        var symbolIcon = new SymbolIcon(Symbol, FontSize, Filled);

        if (FontWeight != FontWeight.Normal)
        {
            symbolIcon.FontWeight = FontWeight;
        }

        if (FontStyle != FontStyle.Normal)
        {
            symbolIcon.FontStyle = FontStyle;
        }

        if (Foreground != null)
        {
            symbolIcon.Foreground = Foreground;
        }

        return symbolIcon;
    }
}
