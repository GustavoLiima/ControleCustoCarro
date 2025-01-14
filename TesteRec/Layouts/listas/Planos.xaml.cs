using Cofauto.Db.Models;
using Plugin.InAppBilling;
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
                ValorMensal = "7,30",
                ValorAnual = "39,42",
                IdPlanoMensal = "plano_pro",
                IdPlanoSemestral = "plano_pro_semestral"
            },
            new Plano
            {
                ID = 2,
                Titulo = "Pro 10",
                Descricao = "Tudo que está no plano PRO",
                Beneficios = "Tudo que está no plano PRO\nAté 10 veículos\nAté 10 motoristas",
                ValorMensal = "34,90",
                ValorAnual = "188,46",
                IdPlanoMensal = "plano_pro10",
                IdPlanoSemestral = "plano_pro10_semestral"
            },
            new Plano
            {
                ID = 3,
                Titulo = "Pro 15",
                Descricao = "Tudo que está no plano PRO",
                Beneficios = "Tudo que está no plano PRO\nAté 15 veículos\nAté 15 motoristas",
                ValorMensal = "49,90",
                ValorAnual = "269,46",
                IdPlanoMensal = "plano_pro15",
                IdPlanoSemestral = "plano_pro15_semestral"
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

            bool Assinou = false;

            if(tipoPlano == "Anual")
            {
                Assinou = await SolicitarPlano(plano.IdPlanoSemestral);
            }
            else
            {
                Assinou = await SolicitarPlano(plano.IdPlanoMensal);
            }

            // Faça algo com o objeto do plano e o tipo do plano
            if(Assinou)
            {
                Global._UsuarioSelecionado.Plano = plano.ID;

                await DisplayAlert("Plano Selecionado",
                    $"Você escolheu o plano: {plano.Titulo}\nTipo: {tipoPlano}\nDescrição: {plano.Descricao}",
                    "OK");
                await Navigation.PopAsync();
            }
        }
    }

    private async Task<bool> SolicitarPlano(string pIdSolicitarPlano)
    {
        bool retorno = false;
        try
        {
            // Inicializa o serviço de faturamento
            bool isBillingInitialized = await SubscriptionService.Instance.InitializeBillingAsync();
            if (!isBillingInitialized)
            {
                await DisplayAlert("Erro", "Não foi possível conectar ao serviço de faturamento.", "OK");
                return false;
            }

            // Realiza a assinatura
            var purchase = await CrossInAppBilling.Current.PurchaseAsync(pIdSolicitarPlano, ItemType.Subscription);

            if (purchase != null && purchase.State == PurchaseState.Purchased)
            {
                // Reconhece a compra para evitar reembolso
                await SubscriptionService.Instance.FinalizePurchaseAsync(new[] { purchase.PurchaseToken });
                retorno = true;
                await DisplayAlert("Sucesso", "Assinatura realizada e reconhecida com sucesso!", "OK");
            }
            else
            {
                await DisplayAlert("Falha", "A assinatura não foi concluída.", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", $"Ocorreu um erro: {ex.Message}", "OK");
        }
        finally
        {
            // Desconecta o serviço de faturamento após a operação
            await SubscriptionService.Instance.DisconnectBillingAsync();
        }
        return retorno;
    }
}