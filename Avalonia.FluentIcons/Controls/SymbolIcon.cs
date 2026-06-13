// Adapted from WPF UI (https://github.com/lepoco/wpfui)
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.

using Avalonia.Media;

namespace Avalonia.FluentIcons;

/// <summary>
/// Represents a text element containing an icon glyph rendered from the Fluent System Icons font.
/// </summary>
public class SymbolIcon : FontIcon
{
    private const string FontUriRegular = "avares://Avalonia.FluentIcons/Assets/Fonts/FluentSystemIcons-Regular.ttf#FluentSystemIcons-Regular";
    private const string FontUriFilled = "avares://Avalonia.FluentIcons/Assets/Fonts/FluentSystemIcons-Filled.ttf#FluentSystemIcons-Filled";

    private static FontFamily? s_regularFamily;
    private static FontFamily? s_filledFamily;

    private static FontFamily RegularFamily => s_regularFamily ??= FontFamily.Parse(FontUriRegular);
    private static FontFamily FilledFamily => s_filledFamily ??= FontFamily.Parse(FontUriFilled);

    /// <summary>Identifies the <see cref="Symbol"/> styled property.</summary>
    public static readonly StyledProperty<SymbolRegular> SymbolProperty =
        AvaloniaProperty.Register<SymbolIcon, SymbolRegular>(nameof(Symbol), SymbolRegular.Empty);

    /// <summary>Identifies the <see cref="Filled"/> styled property.</summary>
    public static readonly StyledProperty<bool> FilledProperty =
        AvaloniaProperty.Register<SymbolIcon, bool>(nameof(Filled), false);

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

    public SymbolIcon() { }

    public SymbolIcon(SymbolRegular symbol, double fontSize = DefaultIconFontSize, bool filled = false)
    {
        Symbol = symbol;
        Filled = filled;
        FontSize = fontSize;
    }

    /// <inheritdoc />
    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == SymbolProperty || change.Property == FilledProperty)
        {
            UpdateFontReference();
            UpdateGlyph();
        }
    }

    private void UpdateGlyph()
    {
        if (TextBlock is null)
            return;

        if (Filled)
        {
            TextBlock.Text = Symbol.Swap().GetString();
        }
        else
        {
            TextBlock.Text = Symbol.GetString();
        }
    }

    private void UpdateFontReference()
    {
        if (TextBlock is null)
            return;

        TextBlock.FontFamily = Filled ? FilledFamily : RegularFamily;
    }
}
