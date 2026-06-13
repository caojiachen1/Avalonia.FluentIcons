// Adapted from WPF UI (https://github.com/lepoco/wpfui)
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.

using Avalonia.Controls;
using Avalonia.Controls.Documents;
using Avalonia.Layout;
using Avalonia.Media;

namespace Avalonia.FluentIcons;

/// <summary>
/// Represents an icon that uses a glyph from the specified font.
/// </summary>
public class FontIcon : IconElement
{
    /// <summary>
    /// Default icon font size.
    /// </summary>
    public const double DefaultIconFontSize = 14.0;

    /// <summary>Identifies the <see cref="FontFamily"/> styled property.</summary>
    public static readonly StyledProperty<FontFamily> FontFamilyProperty =
        AvaloniaProperty.Register<FontIcon, FontFamily>(nameof(FontFamily), FontFamily.Default);

    /// <summary>Identifies the <see cref="FontSize"/> styled property.</summary>
    public static readonly StyledProperty<double> FontSizeProperty =
        AvaloniaProperty.Register<FontIcon, double>(nameof(FontSize), DefaultIconFontSize, inherits: true);

    /// <summary>Identifies the <see cref="FontStyle"/> styled property.</summary>
    public static readonly StyledProperty<FontStyle> FontStyleProperty =
        AvaloniaProperty.Register<FontIcon, FontStyle>(nameof(FontStyle), FontStyle.Normal);

    /// <summary>Identifies the <see cref="FontWeight"/> styled property.</summary>
    public static readonly StyledProperty<FontWeight> FontWeightProperty =
        AvaloniaProperty.Register<FontIcon, FontWeight>(nameof(FontWeight), FontWeight.Normal);

    /// <summary>Identifies the <see cref="Glyph"/> styled property.</summary>
    public static readonly StyledProperty<string> GlyphProperty =
        AvaloniaProperty.Register<FontIcon, string>(nameof(Glyph), string.Empty);

    /// <inheritdoc cref="Avalonia.Controls.TextBlock.FontFamily"/>
    public FontFamily FontFamily
    {
        get => GetValue(FontFamilyProperty);
        set => SetValue(FontFamilyProperty, value);
    }

    /// <inheritdoc cref="Avalonia.Controls.TextBlock.FontSize"/>
    public double FontSize
    {
        get => GetValue(FontSizeProperty);
        set => SetValue(FontSizeProperty, value);
    }

    /// <inheritdoc cref="Avalonia.Controls.TextBlock.FontStyle"/>
    public FontStyle FontStyle
    {
        get => GetValue(FontStyleProperty);
        set => SetValue(FontStyleProperty, value);
    }

    /// <inheritdoc cref="Avalonia.Controls.TextBlock.FontWeight"/>
    public FontWeight FontWeight
    {
        get => GetValue(FontWeightProperty);
        set => SetValue(FontWeightProperty, value);
    }

    /// <summary>
    /// Gets or sets the character code that identifies the icon glyph.
    /// </summary>
    public string Glyph
    {
        get => GetValue(GlyphProperty);
        set => SetValue(GlyphProperty, value);
    }

    /// <summary>
    /// Gets the internal <see cref="Avalonia.Controls.TextBlock"/> used to render the glyph.
    /// </summary>
    protected TextBlock? TextBlock { get; private set; }

    public FontIcon()
    {
        EnsureTextBlock();
    }

    /// <inheritdoc />
    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == GlyphProperty)
        {
            if (TextBlock != null)
                TextBlock.Text = (string)change.NewValue!;
        }
        else if (change.Property == FontFamilyProperty)
        {
            if (TextBlock != null)
                TextBlock.FontFamily = (FontFamily)change.NewValue!;
        }
        else if (change.Property == FontSizeProperty)
        {
            if (TextBlock != null)
                TextBlock.FontSize = (double)change.NewValue!;
        }
        else if (change.Property == FontStyleProperty)
        {
            if (TextBlock != null)
                TextBlock.FontStyle = (FontStyle)change.NewValue!;
        }
        else if (change.Property == FontWeightProperty)
        {
            if (TextBlock != null)
                TextBlock.FontWeight = (FontWeight)change.NewValue!;
        }
    }

    private void EnsureTextBlock()
    {
        if (TextBlock != null)
            return;

        TextBlock = new TextBlock
        {
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Center,
            TextAlignment = TextAlignment.Center,
            FontFamily = FontFamily,
            FontSize = FontSize,
            FontStyle = FontStyle,
            FontWeight = FontWeight,
            Text = Glyph,
            Focusable = false,
        };

        Children.Add(TextBlock);
    }
}
