using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ShowsHub.Views;
public partial class RegisterView : UserControl
{
    public RegisterView()
    {
        InitializeComponent();
    }
    private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
}
