using TesteRec.Db;

namespace FinaGear.Layouts.Relatorios;

public partial class RelGeral : ContentPage
{
	public RelGeral()
	{
		InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
		Image_CarroSelecionado.Source = Global.carroSelecionado.Imagem;
		Label_CarroSelecionado.Text = Global.carroSelecionado.NomeVeiculo;
    }
}