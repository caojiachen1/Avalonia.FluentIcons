// Adapted from WPF UI (https://github.com/lepoco/wpfui)
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.

using Avalonia.Controls;
using Avalonia.Media;

namespace Avalonia.FluentIcons;

/// <summary>
/// Represents an icon that uses an <see cref="Avalonia.Controls.Image"/> as its content.
/// </summary>
public class ImageIcon : IconElement
{
    /// <summary>Identifies the <see cref="Source"/> styled property.</summary>
    public static readonly StyledProperty<IImage?> SourceProperty =
        AvaloniaProperty.Register<ImageIcon, IImage?>(nameof(Source));

    /// <summary>
    /// Gets or sets the image source.
    /// </summary>
    public IImage? Source
    {
        get => GetValue(SourceProperty);
        set => SetValue(SourceProperty, value);
    }

    private Image? _image;

    public ImageIcon()
    {
        EnsureImage();
    }

    /// <inheritdoc />
    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == SourceProperty && _image != null)
        {
            _image.Source = (IImage?)change.NewValue;
        }
    }

    private void EnsureImage()
    {
        if (_image != null)
            return;

        _image = new Image
        {
            Source = Source,
            Stretch = Stretch.Uniform,
        };

        Children.Add(_image);
    }
}
