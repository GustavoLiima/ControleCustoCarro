using CommunityToolkit.Maui.Alerts;
using TesteRec.API.Communic;
using TesteRec.API.Models;
using TesteRec.Db;

namespace Cofauto.Layouts.crud;

public partial class Perfil : ContentPage
{
	public Perfil()
	{
		InitializeComponent();
        NomeEntry.Text = Global._UsuarioSelecionado.nome;
        SobrenomeEntry.Text = Global._UsuarioSelecionado.sobrenome;
        TelefoneEntry.Text = Global._UsuarioSelecionado.telefone;
        EmailEntry.Text = Global._UsuarioSelecionado.email;
        NumeroCnhEntry.Text = Global._UsuarioSelecionado.numeroCnh;
        CategoriaCnhEntry.Text = Global._UsuarioSelecionado.categoriaCnh;
    }

    private async void OnSalvarClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(NomeEntry.Text))
        {
            await DisplayAlert("Erro", "O nome não pode estar vazio.", "OK");
            NomeEntry.Focus();
            return;
        }

        if (string.IsNullOrWhiteSpace(SobrenomeEntry.Text))
        {
            await DisplayAlert("Erro", "O sobrenome não pode estar vazio.", "OK");
            SobrenomeEntry.Focus();
            return;
        }

        if (string.IsNullOrWhiteSpace(EmailEntry.Text))
        {
            await DisplayAlert("Erro", "O e-mail não pode estar vazio.", "OK");
            EmailEntry.Focus();
            return;
        }

        UsuarioVM obj = new UsuarioVM()
        {
            cd_usuario = Global._UsuarioSelecionado.cd_usuario,
            nome = NomeEntry.Text,
            sobrenome = SobrenomeEntry.Text,
            email = EmailEntry.Text,
            senha = Global._UsuarioSelecionado.senha,
            categoriaCnh = NumeroCnhEntry.Text,
            numeroCnh = NumeroCnhEntry.Text,
            vencimentoCnh = VencimentoCnhPicker.Date
        };

        HabilitarDesabilitarLoading();
        UsuarioCommunic com = new UsuarioCommunic();
        var retorno = await com.AtualizarUsuario(obj);
        if(retorno.Sucesso)
        {
            await Toast.Make("Usuário atualizado com sucesso!").Show();
            Global._UsuarioSelecionado = retorno.Valor;
        }
        else
        {
            await DisplayAlert("Atenção", "Verifique sua conexão e tente novamente", "confirmar");
        }
        HabilitarDesabilitarLoading();

    }

    private void HabilitarDesabilitarLoading()
    {
        Frame_LoadingLogin.IsVisible = !Frame_LoadingLogin.IsVisible;
        Button_Salvar.IsVisible = !Button_Salvar.IsVisible;
    }

    private async void OnTrocarSenhaClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new TrocaSenha());
    }
}