using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.FluentIcons.Gallery.Services;
using Avalonia.FluentIcons.Gallery.ViewModels;

namespace Avalonia.FluentIcons.Gallery;

public partial class IconBrowserPage : UserControl
{
    private readonly IconBrowserViewModel _vm = new();

    public IconBrowserPage()
    {
        InitializeComponent();
        DataContext = _vm;
        ApplyLanguage();
        LocalizationService.LanguageChanged += ApplyLanguage;
    }

    private void ApplyLanguage()
    {
        var t = LocalizationService.T;
        SearchBox!.Watermark = t("browser.search_watermark");
        BtnClear!.Content = t("browser.clear");
        StatusShowing!.Text = t("browser.showing");
        StatusOf!.Text = t("browser.of");
        StatusIcons!.Text = t("browser.icons_suffix");
        FilledVariantText!.Text = t("browser.filled_variant");
        CodePointLabel!.Text = t("browser.code_point");
        XamlRegularLabel!.Text = t("browser.xaml_regular");
        XamlFilledLabel!.Text = t("browser.xaml_filled");
        BtnCopyXaml!.Content = t("browser.copy_xaml");
        BtnCopyXamlFilled!.Content = t("browser.copy_xaml_filled");
        CSharpLabel!.Text = t("browser.csharp_code");
        BtnCopyCSharp!.Content = t("browser.copy_csharp");
        EnumLabel!.Text = t("browser.enum_value");
        BtnCopyEnum!.Content = t("browser.copy_enum");
        HowToImportLabel!.Text = t("browser.how_to_import");
        AddCsprojText!.Text = t("browser.add_csproj");
        BtnCopyProj!.Content = t("browser.copy_projref");
    }

    private void ClearSearch_Click(object? sender, RoutedEventArgs e)
    {
        _vm.SearchText = string.Empty;
    }

    private async void CopyXaml_Click(object? sender, RoutedEventArgs e)
    {
        await CopyToClipboard(_vm.XamlReference, BtnCopyXaml);
    }

    private async void CopyXamlFilled_Click(object? sender, RoutedEventArgs e)
    {
        await CopyToClipboard(_vm.XamlFilledReference, BtnCopyXamlFilled);
    }

    private async void CopyCSharp_Click(object? sender, RoutedEventArgs e)
    {
        await CopyToClipboard(_vm.CSharpReference, BtnCopyCSharp);
    }

    private async void CopyEnum_Click(object? sender, RoutedEventArgs e)
    {
        await CopyToClipboard(_vm.EnumReference, BtnCopyEnum);
    }

    private async void CopyProjectRef_Click(object? sender, RoutedEventArgs e)
    {
        await CopyToClipboard(
            "<ProjectReference Include=\"..\\Avalonia.FluentIcons\\Avalonia.FluentIcons.csproj\" />",
            BtnCopyProj);
    }

    private async Task CopyToClipboard(string text, Button? btn = null)
    {
        if (string.IsNullOrEmpty(text)) return;

        var topLevel = TopLevel.GetTopLevel(this);
        if (topLevel?.Clipboard != null)
        {
            await topLevel.Clipboard.SetTextAsync(text);
        }

        if (btn != null)
        {
            var original = btn.Content?.ToString();
            btn.Content = "Copied!";
            await Task.Delay(1200);
            btn.Content = original;
        }
    }
}
