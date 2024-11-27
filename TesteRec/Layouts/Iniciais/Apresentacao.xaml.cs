namespace TesteRec.Layouts.Iniciais;

public partial class Apresentacao : ContentPage
{
	public Apresentacao()
	{
		InitializeComponent();
	}

    private void Button_Clicked(object sender, EventArgs e)
    {
        Application.Current.MainPage = new NavigationPage(new PaginaLogin());
    }
}