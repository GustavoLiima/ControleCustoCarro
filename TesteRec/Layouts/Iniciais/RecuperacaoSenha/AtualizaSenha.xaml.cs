namespace TesteRec.Layouts.Iniciais.RecuperacaoSenha;

public partial class AtualizaSenha : ContentPage
{
    private bool isNewPasswordVisible = false;
    private bool isConfirmPasswordVisible = false;

    public AtualizaSenha()
    {
        InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
    }

    private void TogglePasswordVisibility_NewPassword(object sender, EventArgs e)
    {
        isNewPasswordVisible = !isNewPasswordVisible;
        Entry_NewPassword.IsPassword = !isNewPasswordVisible;
        ((ImageButton)sender).Source = isNewPasswordVisible ? "visible" : "invisible";
    }

    private void TogglePasswordVisibility_ConfirmPassword(object sender, EventArgs e)
    {
        isConfirmPasswordVisible = !isConfirmPasswordVisible;
        Entry_ConfirmPassword.IsPassword = !isConfirmPasswordVisible;
        ((ImageButton)sender).Source = isConfirmPasswordVisible ? "visible" : "invisible";
    }

    private async void Button_Avancar_Clicked(object sender, EventArgs e)
    {
        // Valida��es
        if (string.IsNullOrWhiteSpace(Entry_Code.Text))
        {
            await DisplayAlert("Erro", "O c�digo n�o pode estar vazio.", "OK");
            Entry_Code.Focus();
            return;
        }

        if (string.IsNullOrWhiteSpace(Entry_NewPassword.Text))
        {
            await DisplayAlert("Erro", "A nova senha n�o pode estar vazia.", "OK");
            Entry_NewPassword.Focus();
            return;
        }

        if (Entry_NewPassword.Text.Length < 8)
        {
            await DisplayAlert("Erro", "A senha deve ter pelo menos 8 caracteres.", "OK");
            Entry_NewPassword.Focus();
            return;
        }

        if (string.IsNullOrWhiteSpace(Entry_ConfirmPassword.Text))
        {
            await DisplayAlert("Erro", "A confirma��o da senha n�o pode estar vazia.", "OK");
            Entry_ConfirmPassword.Focus();
            return;
        }

        if (Entry_NewPassword.Text != Entry_ConfirmPassword.Text)
        {
            await DisplayAlert("Erro", "As senhas n�o coincidem.", "OK");
            Entry_ConfirmPassword.Focus();
            return;
        }

        // Caso passe pelas valida��es
        await DisplayAlert("Sucesso", "Senha atualizada com sucesso!", "OK");
        Application.Current.MainPage = new NavigationPage(new PaginaLogin());

        // Navegar para pr�xima p�gina, se necess�rio.
    }
}
