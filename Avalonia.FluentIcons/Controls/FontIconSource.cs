// Adapted from WPF UI (https://github.com/lepoco/wpfui)
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.

using Avalonia.Media;

namespace Avalonia.FluentIcons;

/// <summary>
/// Represents an icon source that uses a glyph from the specified font.
/// </summary>
public class FontIconSource : IconSource
{
    /// <summary>Identifies the <see cref="FontFamily"/> styled property.</summary>
    public static readonly StyledProperty<FontFamily> FontFamilyProperty =
        AvaloniaProperty.Register<FontIconSource, FontFamily>(nameof(FontFamily), FontFamily.Default);

    /// <summary>Identifies the <see cref="FontSize"/> styled property.</summary>
    public static readonly StyledProperty<double> FontSizeProperty =
        AvaloniaProperty.Register<FontIconSource, double>(nameof(FontSize), FontIcon.DefaultIconFontSize);

    /// <summary>Identifies the <see cref="FontStyle"/> styled property.</summary>
    public static readonly StyledProperty<FontStyle> FontStyleProperty =
        AvaloniaProperty.Register<FontIconSource, FontStyle>(nameof(FontStyle), FontStyle.Normal);

    /// <summary>Identifies the <see cref="FontWeight"/> styled property.</summary>
    public static readonly StyledProperty<FontWeight> FontWeightProperty =
        AvaloniaProperty.Register<FontIconSource, FontWeight>(nameof(FontWeight), FontWeight.Normal);

    /// <summary>Identifies the <see cref="Glyph"/> styled property.</summary>
    public static readonly StyledProperty<string> GlyphProperty =
        AvaloniaProperty.Register<FontIconSource, string>(nameof(Glyph), string.Empty);

    public FontFamily FontFamily
    {
        get => GetValue(FontFamilyProperty);
        set => SetValue(FontFamilyProperty, value);
    }

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

    public string Glyph
    {
        get => GetValue(GlyphProperty);
        set => SetValue(GlyphProperty, value);
    }

    /// <inheritdoc />
    public override IconElement CreateIconElement()
    {
        var fontIcon = new FontIcon { Glyph = Glyph };

        if (!Equals(FontFamily, FontFamily.Default))
        {
            fontIcon.FontFamily = FontFamily;
        }

        if (!FontSize.Equals(FontIcon.DefaultIconFontSize))
        {
            fontIcon.FontSize = FontSize;
        }

        if (FontWeight != FontWeight.Normal)
        {
            fontIcon.FontWeight = FontWeight;
        }

        if (FontStyle != FontStyle.Normal)
        {
            fontIcon.FontStyle = FontStyle;
        }

        if (Foreground != null)
        {
            fontIcon.Foreground = Foreground;
        }

        return fontIcon;
    }
}
