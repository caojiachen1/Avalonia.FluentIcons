// Adapted from WPF UI (https://github.com/lepoco/wpfui)
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.

using System.Globalization;

namespace Avalonia.FluentIcons;

/// <summary>
/// Tries to convert <see cref="SymbolRegular"/> and <see cref="SymbolFilled"/> to <see cref="SymbolIcon"/>.
/// </summary>
public class IconElementConverter : System.ComponentModel.TypeConverter
{
    public override bool CanConvertFrom(System.ComponentModel.ITypeDescriptorContext? context, Type sourceType)
    {
        if (sourceType == typeof(SymbolRegular))
        {
            return true;
        }

        if (sourceType == typeof(SymbolFilled))
        {
            return true;
        }

        return false;
    }

    public override bool CanConvertTo(System.ComponentModel.ITypeDescriptorContext? context, Type? destinationType) => false;

    public override object? ConvertFrom(
        System.ComponentModel.ITypeDescriptorContext? context,
        CultureInfo? culture,
        object? value
    ) =>
        value switch
        {
            SymbolRegular symbolRegular => new SymbolIcon(symbolRegular),
            SymbolFilled symbolFilled => new SymbolIcon(symbolFilled.Swap(), filled: true),
            _ => null,
        };

    public override object ConvertTo(
        System.ComponentModel.ITypeDescriptorContext? context,
        CultureInfo? culture,
        object? value,
        Type destinationType
    )
    {
        throw GetConvertFromException(value);
    }
}
