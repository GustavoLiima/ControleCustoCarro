using CommunityToolkit.Maui.Alerts;
using TesteRec.API.Communic;
using TesteRec.Db;
using TesteRec.Helpers;

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

        Criptografia crip = new Criptografia();
        string SenhaAtual = crip.Criptografar(senhaAtual);

        if (Global._UsuarioSelecionado.senha != SenhaAtual)
        {
            await DisplayAlert("Atenção", "Senha atual incorreta", "OK");
            return;
        }

        HabilitarDesabilitarLoading();

        UsuarioCommunic usuarioCommunic = new UsuarioCommunic();
        var retorno = await usuarioCommunic.AlterarSenhaUsuario(crip.Criptografar(senhaNova));

        HabilitarDesabilitarLoading();

        if(retorno.Sucesso)
        {
            await Toast.Make($"Senha alterada com sucesso").Show();
            await Navigation.PopAsync();
        }
        else
        {
            await Toast.Make(retorno.Mensagem).Show();
        }
    }
    private void HabilitarDesabilitarLoading()
    {
        Frame_LoadingLogin.IsVisible = !Frame_LoadingLogin.IsVisible;
        Button_Salvar.IsVisible = !Button_Salvar.IsVisible;
    }

    private void Button_OlharSenha_Clicked(object sender, EventArgs e)
    {
        if (!Entry_SenhaAtual.IsPassword)
        {
            Entry_SenhaAtual.IsPassword = true;
            Button_OlharSenha.Source = "invisible";
        }
        else
        {
            Entry_SenhaAtual.IsPassword = false;
            Button_OlharSenha.Source = "visible";
        }
    }

    private void Button_OlharSenhaNova_Clicked(object sender, EventArgs e)
    {
        if (!Entry_SenhaNova.IsPassword)
        {
            Entry_SenhaNova.IsPassword = true;
            Button_OlharSenhaNova.Source = "invisible";
        }
        else
        {
            Entry_SenhaNova.IsPassword = false;
            Button_OlharSenhaNova.Source = "visible";
        }
    }

    private void Button_OlharSenhaRepitida_Clicked(object sender, EventArgs e)
    {
        if (!Entry_SenhaNovaRep.IsPassword)
        {
            Entry_SenhaNovaRep.IsPassword = true;
            Button_OlharSenhaRepitida.Source = "invisible";
        }
        else
        {
            Entry_SenhaNovaRep.IsPassword = false;
            Button_OlharSenhaRepitida.Source = "visible";
        }
    }
}