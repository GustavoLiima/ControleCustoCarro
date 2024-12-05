namespace TesteRec.Helpers.Componentes;

public partial class ButtonLoading : ContentPage
{
    public static readonly BindableProperty IsVisibleProperty =
            BindableProperty.Create(nameof(IsVisible), typeof(bool), typeof(ButtonLoading), true);

    public bool IsVisible
    {
        get => (bool)GetValue(IsVisibleProperty);
        set => SetValue(IsVisibleProperty, value);
    }

    public ButtonLoading()
    {
        InitializeComponent();
    }
}