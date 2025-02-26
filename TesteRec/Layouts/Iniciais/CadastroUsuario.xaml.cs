using Newtonsoft.Json;
using TesteRec.API.Communic;
using TesteRec.API.Models;
using TesteRec.Db;
using TesteRec.Helpers;
using TesteRec.Layouts.crud;

namespace TesteRec.Layouts.Iniciais;

public partial class CadastroUsuario : ContentPage
{
	public CadastroUsuario()
	{
		InitializeComponent();
		NavigationPage.SetHasNavigationBar(this, false);
	}

    private bool isPasswordVisible = false;
    private bool isReconfirmPasswordVisible = false;

    private void TogglePasswordVisibility_Senha(object sender, EventArgs e)
    {
        isPasswordVisible = !isPasswordVisible;
        Entry_Senha.IsPassword = !isPasswordVisible;
        ((ImageButton)sender).Source = isPasswordVisible ? "visible" : "invisible";
    }

    private void TogglePasswordVisibility_ReconfirmacaoSenha(object sender, EventArgs e)
    {
        isReconfirmPasswordVisible = !isReconfirmPasswordVisible;
        Entry_ReconfirmacaoSenha.IsPassword = !isReconfirmPasswordVisible;
        ((ImageButton)sender).Source = isReconfirmPasswordVisible ? "visible" : "invisible";
    }

    private void InverterVisibilidade()
    {
        Frame_LoadingCadastro.IsVisible = !Frame_LoadingCadastro.IsVisible;
        Button_Cadastrar.IsVisible = !Button_Cadastrar.IsVisible;
    }

    private async void Button_Avancar_Clicked(object sender, EventArgs e)
    {
        // Valida��es
        if (string.IsNullOrWhiteSpace(Entry_Nome.Text))
        {
            await DisplayAlert("Erro", "O nome n�o pode estar vazio.", "OK");
            Entry_Nome.Focus();
            return;
        }

        if (string.IsNullOrWhiteSpace(Entry_Sobrenome.Text))
        {
            await DisplayAlert("Erro", "O sobrenome n�o pode estar vazio.", "OK");
            Entry_Sobrenome.Focus();
            return;
        }

        if (string.IsNullOrWhiteSpace(Entry_Email.Text))
        {
            await DisplayAlert("Erro", "O e-mail n�o pode estar vazio.", "OK");
            Entry_Email.Focus();
            return;
        }

        if (string.IsNullOrWhiteSpace(Entry_Senha.Text))
        {
            await DisplayAlert("Erro", "A senha n�o pode estar vazia.", "OK");
            Entry_Senha.Focus();
            return;
        }

        if (Entry_Senha.Text.Length < 8)
        {
            await DisplayAlert("Erro", "A senha deve ter pelo menos 8 caracteres.", "OK");
            Entry_Senha.Focus();
            return;
        }

        if (string.IsNullOrWhiteSpace(Entry_ReconfirmacaoSenha.Text))
        {
            await DisplayAlert("Erro", "A confirma��o da senha n�o pode estar vazia.", "OK");
            Entry_ReconfirmacaoSenha.Focus();
            return;
        }

        if (Entry_Senha.Text != Entry_ReconfirmacaoSenha.Text)
        {
            await DisplayAlert("Erro", "As senhas n�o coincidem.", "OK");
            Entry_ReconfirmacaoSenha.Focus();
            return;
        }

        InverterVisibilidade();

        UsuarioCommunic chamada = new UsuarioCommunic();
        Criptografia crip = new Criptografia();
        UsuarioVM obj = new UsuarioVM()
        {
            nome = Entry_Nome.Text,
            sobrenome = Entry_Sobrenome.Text,
            email = Entry_Email.Text,
            senha = crip.Criptografar(Entry_Senha.Text),
        };

        ApiResponse<UsuarioVM> vRet = await chamada.CadastrarUsuario(obj);

        if (vRet.Sucesso)
        {
            Global._UsuarioSelecionado = vRet.Valor;
            await SecureStorage.Default.SetAsync("usuario", JsonConvert.SerializeObject(Global._UsuarioSelecionado));
            await DisplayAlert("Aten��o", "Usu�rio cadastro! Agora voc� ir� fazer a configura��o do seu primeiro ve�culo!", "Continuar");
            Application.Current.MainPage = new NavigationPage(new CadastroVeiculo(true));
        }
        else
        {
            await DisplayAlert("Aten��o", vRet.Mensagem, "Continuar");
        }
        InverterVisibilidade();
    }

    private async void Button_Voltar_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}