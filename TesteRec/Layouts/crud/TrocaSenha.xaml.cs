namespace Cofauto.Layouts.crud;

public partial class TrocaSenha : ContentPage
{
	public TrocaSenha()
	{
		InitializeComponent();
	}

    private async void OnSalvarClicked(object sender, EventArgs e)
    {
        string senhaAtual = Entry_SenhaAtual.Text?.Trim();
        string senhaNova = Entry_SenhaNova.Text?.Trim();
        string senhaNovaRep = Entry_SenhaNovaRep.Text?.Trim();

        // Verifica se todos os campos est�o preenchidos
        if (string.IsNullOrEmpty(senhaAtual) || string.IsNullOrEmpty(senhaNova) || string.IsNullOrEmpty(senhaNovaRep))
        {
            await DisplayAlert("Aten��o", "Todos os campos devem ser preenchidos.", "OK");
            return;
        }

        // Valida se a nova senha tem pelo menos 8 caracteres
        if (senhaNova.Length < 8)
        {
            await DisplayAlert("Aten��o", "A nova senha deve ter no m�nimo 8 caracteres.", "OK");
            return;
        }

        // Valida se a nova senha e a repeti��o s�o iguais
        if (!senhaNova.Equals(senhaNovaRep))
        {
            await DisplayAlert("Aten��o", "A nova senha e a repeti��o devem ser iguais.", "OK");
            return;
        }

        // Se todas as valida��es passarem
        await DisplayAlert("Sucesso", "Senha alterada com sucesso!", "OK");

        // Aqui voc� pode adicionar o c�digo para salvar a nova senha
    }
}