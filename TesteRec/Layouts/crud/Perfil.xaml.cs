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

    private void OnSalvarClicked(object sender, EventArgs e)
    {

    }
}