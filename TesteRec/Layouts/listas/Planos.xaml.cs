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
                Descricao = "Para pessoas que desejam acompanhar seu veículo",
                Beneficios = "Até 2 veículos\nRegistros ilimitados de reabastecimento, despesas, serviços e percursos\nLembretes ilimitados\nRelatórios e gráficos",
                ValorMensal = "Grátis",
                ValorAnual = "Grátis"
            },
            new Plano
            {
                ID = 1,
                Titulo = "Pro",
                Descricao = "Para uso pessoal e profissional, sem anúncios e com backup em nuvem",
                Beneficios = "Tudo que está no plano gratuito\nSem anúncios\nAté 5 veículos\nBackup em nuvem",
                ValorMensal = "6,90",
                ValorAnual = "66,20"
            },
            new Plano
            {
                ID = 2,
                Titulo = "Pro 10",
                Descricao = "Tudo que está no plano PRO",
                Beneficios = "Tudo que está no plano PRO\nAté 10 veículos\nAté 10 motoristas",
                ValorMensal = "29,90",
                ValorAnual = "286,90"
            },
            new Plano
            {
                ID = 3,
                Titulo = "Pro 15",
                Descricao = "Tudo que está no plano PRO",
                Beneficios = "Tudo que está no plano PRO\nAté 15 veículos\nAté 15 motoristas",
                ValorMensal = "49,90",
                ValorAnual = "478,90"
            },
            new Plano
            {
                ID = 4,
                Titulo = "Pro 20",
                Descricao = "Tudo que está no plano PRO",
                Beneficios = "Tudo que está no plano PRO\nAté 20 veículos\nAté 20 motoristas",
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

        // Limpar seleção (opcional, se não quiser manter o item selecionado)
        ((CollectionView)sender).SelectedItem = null;
        }
    }

    private async void OnAssinarButtonClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is Plano plano)
        {
            // Identifique qual botão foi clicado
            string tipoPlano = button.Text.Contains("Anual") ? "Anual" : "Mensal";

            // Faça algo com o objeto do plano e o tipo do plano
            Global._UsuarioSelecionado.Plano = plano.ID;

            await DisplayAlert("Plano Selecionado",
                $"Você escolheu o plano: {plano.Titulo}\nTipo: {tipoPlano}\nDescrição: {plano.Descricao}",
                "OK");
            await Navigation.PopAsync();
        }
    }
}