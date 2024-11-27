using TesteRec.Layouts.Iniciais.RecuperacaoSenha;

namespace TesteRec.Layouts.Iniciais;

public partial class PaginaLogin : ContentPage
{
    private bool _senhaVisivel = false;
	public PaginaLogin()
	{
		InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
    }

    private void Button_Login_Clicked(object sender, EventArgs e)
    {
        Application.Current.MainPage = new AppShell();
    }

    private void EsqueciMinhaSenha_Tapped(object sender, TappedEventArgs e)
    {
        Navigation.PushAsync(new InsiraEmail());
    }

    private void Button_Cadastro_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new CadastroUsuario());
    }

    private void Button_OlharSenha_Clicked(object sender, EventArgs e)
    {
        if(_senhaVisivel)
        {
            Entry_Senha.IsPassword = true;
            Button_OlharSenha.Source = "invisible";
        }
        else
        {
            Entry_Senha.IsPassword = false;
            Button_OlharSenha.Source = "visible";
        }
        _senhaVisivel = !_senhaVisivel;
    }
}