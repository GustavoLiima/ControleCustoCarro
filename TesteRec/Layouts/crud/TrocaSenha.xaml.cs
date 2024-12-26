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

        // Verifica se todos os campos estão preenchidos
        if (string.IsNullOrEmpty(senhaAtual) || string.IsNullOrEmpty(senhaNova) || string.IsNullOrEmpty(senhaNovaRep))
        {
            await DisplayAlert("Atenção", "Todos os campos devem ser preenchidos.", "OK");
            return;
        }

        // Valida se a nova senha tem pelo menos 8 caracteres
        if (senhaNova.Length < 8)
        {
            await DisplayAlert("Atenção", "A nova senha deve ter no mínimo 8 caracteres.", "OK");
            return;
        }

        // Valida se a nova senha e a repetição são iguais
        if (!senhaNova.Equals(senhaNovaRep))
        {
            await DisplayAlert("Atenção", "A nova senha e a repetição devem ser iguais.", "OK");
            return;
        }

        // Se todas as validações passarem
        await DisplayAlert("Sucesso", "Senha alterada com sucesso!", "OK");

        // Aqui você pode adicionar o código para salvar a nova senha
    }
}