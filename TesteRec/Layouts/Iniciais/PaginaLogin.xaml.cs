using Newtonsoft.Json;
using TesteRec.API;
using TesteRec.API.Models;
using TesteRec.Db;
using TesteRec.Helpers;
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

    private void HabilitarDesabilitarLoading()
    {
        Frame_LoadingLogin.IsVisible = !Frame_LoadingLogin.IsVisible;
        Button_Login.IsVisible = !Button_Login.IsVisible;
    }

    private async void Button_Login_Clicked(object sender, EventArgs e)
    {
        HabilitarDesabilitarLoading();
        TokenService tokenService = new TokenService();
        Criptografia crip = new Criptografia();
        TokenVM tokenVM = new TokenVM()
        {
            user = Entry_Email.Text,
            password = crip.Criptografar(Entry_Senha.Text)
        };
        ApiResponse<string> vRet = await tokenService.GetTokenAsync(tokenVM);
        if (vRet.Sucesso)
        {
            await SecureStorage.Default.SetAsync("login", JsonConvert.SerializeObject(tokenVM));
            Global._login = tokenVM;
            Application.Current.MainPage = new AppShell();
            HabilitarDesabilitarLoading();
        }
        else
        {
            HabilitarDesabilitarLoading();
            await DisplayAlert("Atenção", vRet.Mensagem, "continuar");
        }
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