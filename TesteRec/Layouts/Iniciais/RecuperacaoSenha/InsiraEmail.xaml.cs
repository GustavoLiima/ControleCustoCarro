using CommunityToolkit.Maui.Alerts;
using TesteRec.API.Communic;

namespace TesteRec.Layouts.Iniciais.RecuperacaoSenha;

public partial class InsiraEmail : ContentPage
{
	public InsiraEmail()
	{
		InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
    }

    private void InverterBotaoLoading()
    {
        Frame_Loading.IsVisible = !Frame_Loading.IsVisible;
        Button_Email.IsVisible = !Button_Email.IsVisible;
    }

    private async void Button_Email_Clicked(object sender, EventArgs e)
    {
        RecuperacaoSenhaCommunic vReq = new RecuperacaoSenhaCommunic();
        if(string.IsNullOrEmpty(Entry_InsiraSeuEmail.Text))
        {
            await Toast.Make($"Insira seu email").Show();
            Entry_InsiraSeuEmail.Focus();
            return;
        }
        InverterBotaoLoading();

        var retorno = await vReq.SolicitarCodigo(Entry_InsiraSeuEmail.Text);
        if (retorno.Sucesso)
        {
            await Navigation.PushAsync(new AtualizaSenha(Entry_InsiraSeuEmail.Text));
        }
        else
        {
            await DisplayAlert("Atenção", retorno.Mensagem, "continuar");
        }
        InverterBotaoLoading();
    }
}