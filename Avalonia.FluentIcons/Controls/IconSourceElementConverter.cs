// Adapted from WPF UI (https://github.com/lepoco/wpfui)
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.

using System.Globalization;
using Avalonia.Data.Converters;

namespace Avalonia.FluentIcons;

/// <summary>
/// Converts an <see cref="IconSourceElement"/> to an <see cref="IconElement"/>.
/// </summary>
public class IconSourceElementConverter : IValueConverter
{
    /// <summary>
    /// Converts a value to an <see cref="IconElement"/>.
    /// </summary>
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is IconSourceElement iconSourceElement)
        {
            return iconSourceElement.CreateIconElement();
        }

        return value;
    }

    /// <summary>
    /// Converts back is not supported.
    /// </summary>
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
