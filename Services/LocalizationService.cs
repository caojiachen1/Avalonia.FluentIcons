using System;
using System.Collections.Generic;

namespace FluentIcons.Gallery.Services;

public static class LocalizationService
{
    private static string _currentLanguage = "zh-CN";

    public static event Action? LanguageChanged;

    public static string CurrentLanguage
    {
        get => _currentLanguage;
        set
        {
            if (_currentLanguage != value)
            {
                _currentLanguage = value;
                LanguageChanged?.Invoke();
            }
        }
    }

    public static string T(string key)
    {
        if (_translations.TryGetValue(key, out var langMap) &&
            langMap.TryGetValue(_currentLanguage, out var text))
            return text;
        return key;
    }

    private static (string, string) Z(string zh, string en) => (zh, en);

    private static readonly Dictionary<string, Dictionary<string, string>> _translations = Build();

    private static Dictionary<string, Dictionary<string, string>> Build()
    {
        var raw = new Dictionary<string, (string Zh, string En)>
        {
            // ── MainWindow nav ──
            ["nav.icons"] = Z("图标浏览", "Icon Browser"),
            ["nav.tutorial"] = Z("使用教程", "Tutorial"),
            ["nav.settings"] = Z("设置", "Settings"),

            // ── IconBrowserPage ──
            ["browser.search_watermark"] = Z("搜索图标...（如 Home, Settings, Star）", "Search icons... (e.g. Home, Settings, Star)"),
            ["browser.clear"] = Z("清除", "Clear"),
            ["browser.showing"] = Z("显示 ", "Showing "),
            ["browser.of"] = Z(" / ", " of "),
            ["browser.icons_suffix"] = Z(" 个图标", " icons"),
            ["browser.filled_variant"] = Z("Filled 变体", "Filled variant"),
            ["browser.code_point"] = Z("编码：", "Code Point:"),
            ["browser.xaml_regular"] = Z("XAML（Regular）", "XAML (Regular)"),
            ["browser.xaml_filled"] = Z("XAML（Filled）", "XAML (Filled)"),
            ["browser.copy_xaml"] = Z("复制 XAML", "Copy XAML"),
            ["browser.copy_xaml_filled"] = Z("复制 XAML（Filled）", "Copy XAML (Filled)"),
            ["browser.csharp_code"] = Z("C# 代码", "C# Code"),
            ["browser.copy_csharp"] = Z("复制 C#", "Copy C#"),
            ["browser.enum_value"] = Z("枚举值", "Enum Value"),
            ["browser.copy_enum"] = Z("复制枚举", "Copy Enum"),
            ["browser.how_to_import"] = Z("如何引入", "How to import"),
            ["browser.add_csproj"] = Z("在 .csproj 中添加：", "Add this to your .csproj:"),
            ["browser.copy_packageref"] = Z("复制 PackageReference", "Copy PackageReference"),
            ["browser.switch_to_symbol"] = Z("切换到 SymbolIcon", "Switch to SymbolIcon"),
            ["browser.switch_to_fluent"] = Z("切换到 FluentIcon", "Switch to FluentIcon"),

            // ── TutorialPage ──
            ["tutorial.title"] = Z("使用教程", "Tutorial"),
            ["tutorial.getting_started"] = Z("快速开始", "Getting Started"),
            ["tutorial.getting_started_desc"] = Z("FluentIcons.Avalonia 是一个基于微软 Fluent UI System Icons 的 Avalonia 图标库，提供数千个高质量图标，支持多种风格。", "FluentIcons.Avalonia is an Avalonia icon library based on Microsoft Fluent UI System Icons, providing thousands of high-quality icons with multiple style variants."),
            ["tutorial.install_title"] = Z("第一步：安装 NuGet 包", "Step 1: Install NuGet Package"),
            ["tutorial.install_desc"] = Z("在项目中通过 NuGet 安装 FluentIcons.Avalonia：", "Install FluentIcons.Avalonia via NuGet in your project:"),
            ["tutorial.install_cmd"] = Z("或使用 dotnet CLI：", "Or use the dotnet CLI:"),
            ["tutorial.xaml_usage_title"] = Z("第二步：在 XAML 中使用图标", "Step 2: Use Icons in XAML"),
            ["tutorial.xaml_usage_desc"] = Z("首先在 XAML 文件顶部添加命名空间引用：", "First, add the namespace reference at the top of your XAML file:"),
            ["tutorial.xaml_basic_title"] = Z("基础用法", "Basic Usage"),
            ["tutorial.xaml_basic_desc"] = Z("使用 FluentIcon 控件显示图标，通过 Icon 属性指定图标名称：", "Use the FluentIcon control to display icons, specifying the icon name via the Icon property:"),
            ["tutorial.variants_title"] = Z("图标风格变体", "Icon Style Variants"),
            ["tutorial.variants_desc"] = Z("FluentIcons 支持四种风格变体，通过 IconVariant 属性切换：", "FluentIcons supports four style variants, switchable via the IconVariant property:"),
            ["tutorial.variant_regular"] = Z("Regular（默认）— 线条风格，适合常规 UI", "Regular (Default) — Outlined style, suitable for standard UI"),
            ["tutorial.variant_filled"] = Z("Filled — 填充风格，适合强调和选中状态", "Filled — Solid style, suitable for emphasis and selected states"),
            ["tutorial.variant_color"] = Z("Color — 彩色风格，适合特殊装饰场景", "Color — Multicolor style, suitable for decorative scenarios"),
            ["tutorial.variant_light"] = Z("Light — 轻量风格，更细的线条", "Light — Lightweight style with thinner strokes"),
            ["tutorial.sizing_title"] = Z("调整图标大小", "Adjusting Icon Size"),
            ["tutorial.sizing_desc"] = Z("通过 FontSize 属性控制图标大小，图标会自动缩放：", "Control icon size via the FontSize property — the icon scales automatically:"),
            ["tutorial.csharp_usage_title"] = Z("在 C# 代码中使用", "Using in C# Code"),
            ["tutorial.csharp_usage_desc"] = Z("除了 XAML，也可以在 C# 代码中动态创建图标：", "Besides XAML, you can also create icons dynamically in C# code:"),
            ["tutorial.search_tips_title"] = Z("图标搜索技巧", "Icon Search Tips"),
            ["tutorial.search_tips_desc"] = Z("在图标浏览器中使用以下技巧快速找到所需图标：", "Use these tips in the Icon Browser to quickly find the icons you need:"),
            ["tutorial.tip1"] = Z("• 使用英文关键词搜索，如 Home、Settings、Star", "• Search with English keywords, e.g. Home, Settings, Star"),
            ["tutorial.tip2"] = Z("• 图标名称采用 PascalCase 命名，如 ArrowLeft、CircleSmall", "• Icon names use PascalCase, e.g. ArrowLeft, CircleSmall"),
            ["tutorial.tip3"] = Z("• 点击任意图标卡片查看详细引用信息和代码片段", "• Click any icon card to view detailed reference info and code snippets"),
            ["tutorial.tip4"] = Z("• 使用复制按钮快速获取 XAML、C# 或枚举引用代码", "• Use copy buttons to quickly get XAML, C#, or enum reference code"),
            ["tutorial.more_info_title"] = Z("更多信息", "More Information"),
            ["tutorial.more_info_desc"] = Z("访问 FluentIcons.Avalonia 仓库获取更多文档和示例：", "Visit the FluentIcons.Avalonia repository for more documentation and examples:"),
            ["tutorial.repo_button"] = Z("GitHub 仓库", "GitHub Repository"),

            // ── SettingsPage ──
            ["settings.title"] = Z("设置", "Settings"),
            ["settings.language_title"] = Z("语言", "Language"),
            ["settings.language_desc"] = Z("选择界面语言", "Select interface language"),
            ["settings.about"] = Z("关于", "About"),
            ["settings.about_desc1"] = Z("本 Gallery 使用 FluentIcons.Avalonia (https://github.com/davidxuang/FluentIcons) —— 一个基于微软 Fluent UI System Icons 的 Avalonia 图标库，支持 Regular、Filled、Color、Light 四种风格，包含数千个图标。", "This Gallery uses FluentIcons.Avalonia (https://github.com/davidxuang/FluentIcons) — an Avalonia icon library based on Microsoft Fluent UI System Icons, supporting Regular, Filled, Color, and Light variants with thousands of icons."),
            ["settings.repo_button"] = Z("FluentIcons.Avalonia 仓库", "FluentIcons.Avalonia Repository"),
        };

        var result = new Dictionary<string, Dictionary<string, string>>();
        foreach (var (key, (zh, en)) in raw)
            result[key] = new Dictionary<string, string> { ["zh-CN"] = zh, ["en-US"] = en };
        return result;
    }
}
