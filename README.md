## Avalonia.FluentIcons

Avalonia Fluent System Icons library ported from [WPF UI](https://github.com/lepoco/wpfui). Maintains consistent naming and usage patterns with the original project, while fully adapting to Avalonia's property system and control framework.

### Project Structure

```
Avalonia.FluentIcons/
├── Avalonia.FluentIcons.csproj          # Project file (net8.0, Avalonia 11.2+)
├── Assets/Fonts/
│   ├── FluentSystemIcons-Regular.ttf    # Regular (stroke) font
│   └── FluentSystemIcons-Filled.ttf     # Filled (solid) font
├── Controls/
│   ├── IIconControl.cs                  # Icon control interface
│   ├── IconElement.cs                   # Icon base class (inherits from Panel)
│   ├── FontIcon.cs                      # Font icon control
│   ├── SymbolIcon.cs                    # Fluent symbol icon control (most commonly used)
│   ├── ImageIcon.cs                     # Image icon control
│   ├── IconSourceElement.cs             # IconSource wrapper element
│   ├── IconSource.cs                    # IconSource base class
│   ├── FontIconSource.cs                # Font icon data source
│   ├── SymbolIconSource.cs              # Symbol icon data source
│   ├── IconElementConverter.cs          # Symbol→SymbolIcon type converter
│   ├── IconSourceElementConverter.cs    # IconSourceElement→IconElement value converter
│   ├── SymbolGlyph.cs                   # Icon name parsing utility
│   ├── SymbolRegular.cs                 # Regular icon enum (~4000+ icons)
│   └── SymbolFilled.cs                  # Filled icon enum (~4000+ icons)
├── Extensions/
│   └── SymbolExtensions.cs              # Enum extension methods (Swap, GetString)
└── Resources/
    └── Fonts.axaml                      # Font resource dictionary (optional reference)
```

### Quick Start

#### 1. Add Project Reference

Add a project reference in your Avalonia application's `.csproj`:

```xml
<ItemGroup>
  <ProjectReference Include="..\Avalonia.FluentIcons\Avalonia.FluentIcons.csproj" />
</ItemGroup>
```

#### 2. Use in XAML

Add the namespace at the top of your XAML file:

```xml
<Window xmlns:icons="using:Avalonia.FluentIcons" ...>
```

Then use the `SymbolIcon` control:

```xml
<!-- Basic usage: display a Regular style icon -->
<icons:SymbolIcon Symbol="Home24" />

<!-- Display a Filled style icon -->
<icons:SymbolIcon Symbol="Home24" Filled="True" />

<!-- Custom size -->
<icons:SymbolIcon Symbol="Home24" FontSize="24" />

<!-- Custom color -->
<icons:SymbolIcon Symbol="Home24" Foreground="Red" />

<!-- Combined usage -->
<icons:SymbolIcon Symbol="Settings20" FontSize="20" Filled="True" Foreground="#60CDFF" />
```

#### 3. Use in C# Code

```csharp
using Avalonia.FluentIcons;

// Create icon
var icon = new SymbolIcon(SymbolRegular.Home24);

// Create Filled icon
var filledIcon = new SymbolIcon(SymbolRegular.Home24, fontSize: 20, filled: true);

// Dynamically modify properties
icon.Symbol = SymbolRegular.Settings20;
icon.Filled = true;
icon.FontSize = 16;
```

### Control Details

#### SymbolIcon — Symbol Icon (Recommended)

`SymbolIcon` is the most commonly used icon control, rendering icons from the embedded Fluent System Icons font.

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `Symbol` | `SymbolRegular` | `Empty` | Icon to display |
| `Filled` | `bool` | `false` | Whether to use filled variant |
| `FontSize` | `double` | `14.0` | Icon size |
| `Foreground` | `IBrush` | Inherited | Icon color |
| `FontWeight` | `FontWeight` | `Normal` | Font weight |
| `FontStyle` | `FontStyle` | `Normal` | Font style |

```xml
<!-- Use in a Button -->
<Button>
    <StackPanel Orientation="Horizontal" Spacing="8">
        <icons:SymbolIcon Symbol="Add20" />
        <TextBlock Text="Add" />
    </StackPanel>
</Button>

<!-- Use in a ListBox item -->
<ListBox>
    <ListBoxItem>
        <StackPanel Orientation="Horizontal" Spacing="12">
            <icons:SymbolIcon Symbol="DocumentEdit24" FontSize="20" />
            <TextBlock Text="Edit document" VerticalAlignment="Center" />
        </StackPanel>
    </ListBoxItem>
</ListBox>
```

#### FontIcon — Custom Font Icon

`FontIcon` is used to render characters from any font. Use it when you need to use fonts other than Fluent System Icons (e.g., Segoe Fluent Icons).

```xml
<icons:FontIcon
    Glyph="&#xE710;"
    FontFamily="Segoe Fluent Icons"
    FontSize="16"
    Foreground="Blue" />
```

#### ImageIcon — Image Icon

`ImageIcon` is used to display bitmap/SVG images as icons.

```xml
<icons:ImageIcon Source="/Assets/my-icon.png" />
```

#### IconSource System — Deferred Icon Creation

`IconSource` is a data layer abstraction for deferred creation of `IconElement` in data binding scenarios.

```xml
<!-- Define icon data via SymbolIconSource -->
<icons:SymbolIconSource x:Key="HomeIconSource" Symbol="Home24" Filled="True" />

<!-- Convert to visual icon via IconSourceElement -->
<icons:IconSourceElement IconSource="{StaticResource HomeIconSource}" />
```

In C#:

```csharp
var source = new SymbolIconSource
{
    Symbol = SymbolRegular.Home24,
    Filled = true,
    FontSize = 20
};

IconElement icon = source.CreateIconElement();
```

#### IIconControl Interface

If your custom control needs to support icons, simply implement the `IIconControl` interface:

```csharp
public class MyCustomControl : ContentControl, IIconControl
{
    public static readonly StyledProperty<IconElement?> IconProperty =
        AvaloniaProperty.Register<MyCustomControl, IconElement?>(nameof(Icon));

    public IconElement? Icon
    {
        get => GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }
}
```

### Enums and Utility Methods

#### SymbolRegular / SymbolFilled Enums

Contains 4000+ Fluent System Icons definitions, with names in the format `{icon_name}{size}`:

```csharp
SymbolRegular.Home24
SymbolRegular.Settings20
SymbolRegular.DocumentAdd28
SymbolFilled.Home24
SymbolFilled.Settings20
```

#### SymbolGlyph — Name Parsing

```csharp
// Look up icon by name
var icon = SymbolGlyph.Parse("Home24");           // → SymbolRegular.Home24
var filled = SymbolGlyph.ParseFilled("Home24");   // → SymbolFilled.Home24

// Returns default icon BorderNone24 when name doesn't exist (Release mode)
```

#### SymbolExtensions — Extension Methods

```csharp
// Regular ↔ Filled swap
SymbolRegular regular = SymbolFilled.Home24.Swap();    // → SymbolRegular.Home24
SymbolFilled filled = SymbolRegular.Home24.Swap();     // → SymbolFilled.Home24

// Enum value → Unicode string (for font rendering)
string glyph = SymbolRegular.Home24.GetString();       // → Corresponding Unicode character
string glyph2 = SymbolFilled.Home24.GetString();       // → Corresponding Unicode character
```

### Optional: Import Font Resource Dictionary

If you need to reference fonts as resource keys in XAML (e.g., for `FontIcon`), merge the resource dictionary in `App.axaml`:

```xml
<Application xmlns="https://github.com/avaloniaui"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml">
    <Application.Resources>
        <ResourceDictionary>
            <ResourceDictionary.MergedDictionaries>
                <ResourceInclude Source="avares://Avalonia.FluentIcons/Resources/Fonts.axaml" />
            </ResourceDictionary.MergedDictionaries>
        </ResourceDictionary>
    </Application.Resources>
</Application>
```

After merging, you can use it like this:

```xml
<TextBlock
    Text="&#xE100;"
    FontFamily="{StaticResource FluentSystemIcons}"
    FontSize="20" />

<TextBlock
    Text="&#xE100;"
    FontFamily="{StaticResource FluentSystemIconsFilled}"
    FontSize="20" />
```

### Migration Guide from WPF UI

If you're migrating a WPF UI project to Avalonia, here are the key mappings:

| WPF UI | Avalonia.FluentIcons | Description |
|--------|---------------------|-------------|
| `Wpf.Ui.Controls` | `Avalonia.FluentIcons` | Namespace |
| `DependencyProperty` | `StyledProperty<T>` / `AvaloniaProperty` | Property system |
| `FrameworkElement` | `Panel` | IconElement base class |
| `System.Windows.Media.Brush` | `Avalonia.Media.IBrush` | Brush type |
| `System.Windows.Media.ImageSource` | `Avalonia.Media.IImage` | Image type |
| `System.Windows.Media.FontFamily` | `Avalonia.Media.FontFamily` | Font type |
| `pack://application:,,,/...` | `avares://...` | Resource URI |
| `SetResourceReference(...)` | Direct assignment of static FontFamily | Font reference method |

**XAML usage is completely identical:**

```xml
<!-- WPF UI -->
<ui:SymbolIcon Symbol="Home24" Filled="True" />

<!-- Avalonia.FluentIcons -->
<icons:SymbolIcon Symbol="Home24" Filled="True" />
```

**C# usage is completely identical:**

```csharp
// WPF UI
var icon = new Wpf.Ui.Controls.SymbolIcon(Wpf.Ui.Controls.SymbolRegular.Home24);

// Avalonia.FluentIcons
var icon = new Avalonia.FluentIcons.SymbolIcon(Avalonia.FluentIcons.SymbolRegular.Home24);
```

### License

Font files are from [Microsoft Fluent UI System Icons](https://github.com/microsoft/fluentui-system-icons), MIT License.  
Control code adapted from [WPF UI](https://github.com/lepoco/wpfui), MIT License.