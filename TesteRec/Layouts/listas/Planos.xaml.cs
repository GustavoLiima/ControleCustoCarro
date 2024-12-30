using Cofauto.Db.Models;
using System.Collections.ObjectModel;
using TesteRec.Db;

namespace Cofauto.Layouts.listas;

public partial class Planos : ContentPage
{
    public ObservableCollection<Plano> _Planos { get; set; }
    public Planos()
	{
		InitializeComponent();
        _Planos = new ObservableCollection<Plano>
        {
            new Plano
            {
                ID = 0,
                Titulo = "Gratuito",
                Descricao = "Para pessoas que desejam acompanhar seu ve�culo",
                Beneficios = "At� 2 ve�culos\nRegistros ilimitados de reabastecimento, despesas, servi�os e percursos\nLembretes ilimitados\nRelat�rios e gr�ficos",
                ValorMensal = "Gr�tis",
                ValorAnual = "Gr�tis"
            },
            new Plano
            {
                ID = 1,
                Titulo = "Pro",
                Descricao = "Para uso pessoal e profissional, sem an�ncios e com backup em nuvem",
                Beneficios = "Tudo que est� no plano gratuito\nSem an�ncios\nAt� 5 ve�culos\nBackup em nuvem",
                ValorMensal = "6,90",
                ValorAnual = "66,20"
            },
            new Plano
            {
                ID = 2,
                Titulo = "Pro 10",
                Descricao = "Tudo que est� no plano PRO",
                Beneficios = "Tudo que est� no plano PRO\nAt� 10 ve�culos\nAt� 10 motoristas",
                ValorMensal = "29,90",
                ValorAnual = "286,90"
            },
            new Plano
            {
                ID = 3,
                Titulo = "Pro 15",
                Descricao = "Tudo que est� no plano PRO",
                Beneficios = "Tudo que est� no plano PRO\nAt� 15 ve�culos\nAt� 15 motoristas",
                ValorMensal = "49,90",
                ValorAnual = "478,90"
            },
            new Plano
            {
                ID = 4,
                Titulo = "Pro 20",
                Descricao = "Tudo que est� no plano PRO",
                Beneficios = "Tudo que est� no plano PRO\nAt� 20 ve�culos\nAt� 20 motoristas",
                ValorMensal = "79,90",
                ValorAnual = "766,90"
            }
        };
        _Planos[Global._UsuarioSelecionado.Plano].PlanoAtual = true;
        CollectionView_Planos.ItemsSource = _Planos;
    }

    private void OnPlanoSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection != null && e.CurrentSelection.Count > 0)
        {
            // Obtenha o objeto selecionado
            var selectedPlano = e.CurrentSelection[0] as Plano;

            if (selectedPlano != null)
            {
            }

        // Limpar sele��o (opcional, se n�o quiser manter o item selecionado)
        ((CollectionView)sender).SelectedItem = null;
        }
    }

    private async void OnAssinarButtonClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is Plano plano)
        {
            // Identifique qual bot�o foi clicado
            string tipoPlano = button.Text.Contains("Anual") ? "Anual" : "Mensal";

            // Fa�a algo com o objeto do plano e o tipo do plano
            Global._UsuarioSelecionado.Plano = plano.ID;

            await DisplayAlert("Plano Selecionado",
                $"Voc� escolheu o plano: {plano.Titulo}\nTipo: {tipoPlano}\nDescri��o: {plano.Descricao}",
                "OK");
            await Navigation.PopAsync();
        }
    }
}