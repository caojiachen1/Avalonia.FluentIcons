using System;
using System.Collections.Generic;

namespace Avalonia.FluentIcons.Gallery.Services;

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
            ["nav.home"] = Z("首页", "Home"),
            ["nav.icons"] = Z("图标浏览", "Icon Browser"),
            ["nav.fonts"] = Z("字体", "Fonts"),
            ["nav.tutorial"] = Z("教程", "Tutorial"),
            ["nav.settings"] = Z("设置", "Settings"),
            ["header.title"] = Z("Avalonia Fluent Icons Gallery", "Avalonia Fluent Icons Gallery"),

            // ── HomePage ──
            ["home.title"] = Z("Avalonia Fluent System Icons", "Fluent System Icons for Avalonia"),
            ["home.subtitle"] = Z("基于微软 Fluent System Icons 的 Avalonia 图标库，从 WPF UI 迁移而来。\n包含 4000+ 图标，支持 Regular 和 Filled 两种风格。", "An Avalonia icon library based on Microsoft Fluent System Icons, migrated from WPF UI.\nIncludes 4000+ icons with Regular and Filled variants."),
            ["home.quick_preview"] = Z("快速预览", "Quick Preview"),
            ["home.get_started"] = Z("快速开始", "Get Started"),
            ["home.step1"] = Z("1. 添加项目引用到 Avalonia.FluentIcons.csproj", "1. Add a project reference to Avalonia.FluentIcons.csproj"),
            ["home.step2"] = Z("2. 在 XAML 文件中添加命名空间", "2. Add the namespace in your XAML file"),
            ["home.step3"] = Z("3. 使用 SymbolIcon 控件", "3. Use the SymbolIcon control"),
            ["home.feat_4000"] = Z("4000+ 图标", "4000+ Icons"),
            ["home.feat_4000_desc"] = Z("完整的 Fluent System Icons 集合，包含 Regular 和 Filled 两种变体。", "Complete Fluent System Icons set with both Regular and Filled variants."),
            ["home.feat_swap"] = Z("轻松切换", "Easy Swap"),
            ["home.feat_swap_desc"] = Z("通过一个 Filled 属性即可在 Regular 和 Filled 风格之间切换。", "Switch between Regular and Filled styles with a single Filled property."),
            ["home.feat_custom"] = Z("高度可定制", "Customizable"),
            ["home.feat_custom_desc"] = Z("通过标准 Avalonia 属性完全控制大小、颜色、粗细和样式。", "Full control over size, color, weight, and style through standard Avalonia properties."),
            ["home.footer"] = Z("前往图标浏览器浏览所有图标，或查看教程了解详细用法。", "Navigate to Icon Browser to explore all available icons, or check the Tutorial for detailed usage."),

            // ── IconBrowserPage ──
            ["browser.search_watermark"] = Z("搜索图标...（如 Home、Settings、Star）", "Search icons... (e.g. Home, Settings, Star)"),
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
            ["browser.copy_projref"] = Z("复制 ProjectReference", "Copy ProjectReference"),

            // ── FontsPage ──
            ["fonts.title"] = Z("字体族", "Font Families"),
            ["fonts.regular_desc"] = Z("FluentSystemIcons-Regular.ttf  |  描边（轮廓）风格", "FluentSystemIcons-Regular.ttf  |  Outline (stroked) style"),
            ["fonts.filled_desc"] = Z("FluentSystemIcons-Filled.ttf  |  实心（填充）风格", "FluentSystemIcons-Filled.ttf  |  Solid (filled) style"),
            ["fonts.color_custom"] = Z("颜色自定义", "Color Customization"),

            // ── TutorialPage ──
            ["tutorial.title"] = Z("教程", "Tutorial"),
            ["tutorial.subtitle"] = Z("在项目中使用的完整指南。", "Complete guide to using Avalonia.FluentIcons in your project."),
            ["tutorial.step1_title"] = Z("添加项目引用", "Add Project Reference"),
            ["tutorial.step1_desc"] = Z("在你的 Avalonia 应用的 .csproj 文件中，添加对 Avalonia.FluentIcons 库的 ProjectReference：", "In your Avalonia application's .csproj file, add a ProjectReference to the Avalonia.FluentIcons library:"),
            ["tutorial.step1_note"] = Z("该库目标框架为 net8.0，需要 Avalonia 11.2.0 或更高版本。", "The library targets net8.0 and requires Avalonia 11.2.0 or later."),
            ["tutorial.step2_title"] = Z("添加 XAML 命名空间", "Add XAML Namespace"),
            ["tutorial.step2_desc"] = Z("在 Window 或 UserControl 顶部添加 XML 命名空间声明：", "Add the XML namespace declaration at the top of your Window or UserControl:"),
            ["tutorial.step3_title"] = Z("使用 SymbolIcon", "Use SymbolIcon"),
            ["tutorial.basic_usage"] = Z("基本用法：", "Basic usage:"),
            ["tutorial.step4_title"] = Z("自定义属性", "Customize Properties"),
            ["tutorial.fontsize_desc"] = Z("FontSize — 控制图标大小：", "FontSize — controls icon size:"),
            ["tutorial.foreground_desc"] = Z("Foreground — 控制图标颜色：", "Foreground — controls icon color:"),
            ["tutorial.step5_title"] = Z("在按钮和其他控件中使用", "Use in Buttons and Other Controls"),
            ["tutorial.step6_title"] = Z("C# 代码用法", "C# Code Usage"),
            ["tutorial.step7_title"] = Z("IconSource 模式（数据驱动）", "IconSource Pattern (Data-Driven)"),
            ["tutorial.step7_desc"] = Z("在数据绑定场景中使用 IconSource 进行延迟图标创建：", "Use IconSource for deferred icon creation in data-binding scenarios:"),
            ["tutorial.available_controls"] = Z("可用控件", "Available Controls"),
            ["tutorial.ctrl_symbolicon_desc"] = Z("支持 Regular/Filled 切换的 Fluent System Icons 图标。", "Fluent System Icons glyph with Regular/Filled toggle."),
            ["tutorial.ctrl_fonticon_desc"] = Z("支持自定义 FontFamily 的任意字体图标。", "Any font glyph with custom FontFamily support."),
            ["tutorial.ctrl_imageicon_desc"] = Z("位图或 SVG 图像作为图标元素。", "Bitmap or SVG image as an icon element."),
            ["tutorial.ctrl_iconsource_desc"] = Z("包装 IconSource 用于绑定中的延迟创建。", "Wraps an IconSource for deferred creation in bindings."),

            // ── SettingsPage ──
            ["settings.title"] = Z("设置", "Settings"),
            ["settings.language_title"] = Z("语言", "Language"),
            ["settings.language_desc"] = Z("选择界面语言", "Select interface language"),
            ["settings.about"] = Z("关于", "About"),
            ["settings.about_desc1"] = Z("本项目是 wpfui (https://github.com/lepoco/wpfui) 的 Avalonia 迁移版本。", "This project is an Avalonia migration of wpfui (https://github.com/lepoco/wpfui)."),
            ["settings.about_desc2"] = Z("wpfui 是一个为 WPF 应用提供 Fluent Design 风格 UI 组件和图标的开源项目。本项目将其中的 Fluent System Icons 图标库迁移到 Avalonia 平台，使 Avalonia 应用也能使用相同的 4000+ 高质量图标，支持 Regular 和 Filled 两种风格。", "wpfui is an open-source project providing Fluent Design UI components and icons for WPF applications. This project migrates the Fluent System Icons library to the Avalonia platform, enabling Avalonia apps to use the same 4000+ high-quality icons with Regular and Filled variants."),
            ["settings.about_desc3"] = Z("迁移内容包括：图标字体资源、SymbolIcon 控件、Symbol 枚举定义，以及配套的 Gallery 浏览应用。", "The migration covers: icon font assets, SymbolIcon control, Symbol enum definitions, and the accompanying Gallery browser app."),
            ["settings.repo_button"] = Z("本项目仓库", "Project Repository"),
            ["settings.wpfui_button"] = Z("原始 wpfui 项目", "Original wpfui Project"),
        };

        var result = new Dictionary<string, Dictionary<string, string>>();
        foreach (var (key, (zh, en)) in raw)
            result[key] = new Dictionary<string, string> { ["zh-CN"] = zh, ["en-US"] = en };
        return result;
    }
}
