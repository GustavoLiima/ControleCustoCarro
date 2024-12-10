using Newtonsoft.Json;
using TesteRec.API;
using TesteRec.API.Models;
using TesteRec.Db;
using TesteRec.Db.Services;
using TesteRec.Helpers;
using TesteRec.Layouts.crud;
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
        if(string.IsNullOrEmpty(Entry_Email.Text))
        {
            await DisplayAlert("Atenção", "Preencha o email para continuar", "continuar");
            Entry_Email.Focus();
            return;
        }
        if (string.IsNullOrEmpty(Entry_Senha.Text))
        {
            await DisplayAlert("Atenção", "Preencha a senha para continuar", "continuar");
            Entry_Senha.Focus();
            return;
        }
        DesfocarEntrys();

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



            if (Global._Veiculos.Count == 0)
            {
                await DisplayAlert("Atenção", "Você não tem nenhum veículo cadastrado, cadastre seu primeiro veículo para continuar", "Continuar");
                Application.Current.MainPage = new NavigationPage(new CadastroVeiculo(true));
            }
            else
            {
                VeiculoDB veiculoDB = new VeiculoDB();
                var ret = await veiculoDB.GetVeiculosAsync();
                if (ret.Count == 0)
                {
                    foreach (var item in Global._Veiculos)
                    {
                        await veiculoDB.AddVeiculoAsync(item);
                    }
                }
                Application.Current.MainPage = new AppShell();
            }
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
        DesfocarEntrys();
        Navigation.PushAsync(new InsiraEmail());
    }

    private void Button_Cadastro_Clicked(object sender, EventArgs e)
    {
        DesfocarEntrys();
        Navigation.PushAsync(new CadastroUsuario());
    }

    private void DesfocarEntrys()
    {
        Entry_Email.Unfocus();
        Entry_Senha.Unfocus();
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