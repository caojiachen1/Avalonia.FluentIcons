using System.ComponentModel;
using System.Runtime.CompilerServices;
using FluentIcons.Common;

namespace FluentIcons.Gallery.Models;

public class IconItem : INotifyPropertyChanged
{
    public string Name { get; init; } = string.Empty;
    public Icon Icon { get; init; }
    public Symbol Symbol { get; init; }
    public string CodePoint { get; init; } = string.Empty;

    private bool _isSelected;
    public bool IsSelected
    {
        get => _isSelected;
        set
        {
            if (_isSelected != value)
            {
                _isSelected = value;
                OnPropertyChanged();
            }
        }
    }

    public Symbol SymbolValue => Symbol;
    public bool HasSymbol => Enum.IsDefined(Symbol);

    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
