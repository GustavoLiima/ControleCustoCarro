using TesteRec.API.Communic;
using TesteRec.API.Models;
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

    private async void Button_Avancar_Clicked(object sender, EventArgs e)
    {
        // Validações
        if (string.IsNullOrWhiteSpace(Entry_Nome.Text))
        {
            await DisplayAlert("Erro", "O nome não pode estar vazio.", "OK");
            Entry_Nome.Focus();
            return;
        }

        if (string.IsNullOrWhiteSpace(Entry_Sobrenome.Text))
        {
            await DisplayAlert("Erro", "O sobrenome não pode estar vazio.", "OK");
            Entry_Sobrenome.Focus();
            return;
        }

        if (string.IsNullOrWhiteSpace(Entry_Email.Text))
        {
            await DisplayAlert("Erro", "O e-mail não pode estar vazio.", "OK");
            Entry_Email.Focus();
            return;
        }

        if (string.IsNullOrWhiteSpace(Entry_Senha.Text))
        {
            await DisplayAlert("Erro", "A senha não pode estar vazia.", "OK");
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
            await DisplayAlert("Erro", "A confirmação da senha não pode estar vazia.", "OK");
            Entry_ReconfirmacaoSenha.Focus();
            return;
        }

        if (Entry_Senha.Text != Entry_ReconfirmacaoSenha.Text)
        {
            await DisplayAlert("Erro", "As senhas não coincidem.", "OK");
            Entry_ReconfirmacaoSenha.Focus();
            return;
        }

        UsuarioCommunic chamada = new UsuarioCommunic();
        UsuarioVM obj = new UsuarioVM()
        {
            nome = Entry_Nome.Text,
            sobrenome = Entry_Sobrenome.Text,
            email = Entry_Email.Text,
            senha = Entry_Senha.Text,
            categoriaCnh = "B",
            numeroCnh = "1234",
            vencimentoCnh = DateTime.MinValue
        };

        ApiResponse<bool> vRet = await chamada.CadastrarUsuario(obj);

        if (vRet.Sucesso)
        {
            await DisplayAlert("Atenção", "Deu certo!", "Continuar");
            Application.Current.MainPage = new NavigationPage(new CadastroVeiculo(true));
        }
        else
        {
            await DisplayAlert("Atenção", vRet.Mensagem, "Continuar");
        }
    }

    private void Button_Voltar_Clicked(object sender, EventArgs e)
    {
        Navigation.PopAsync();
    }
}