using FinaGear.Helpers;
using CommunityToolkit.Maui.Alerts;

namespace TesteRec.Layouts.Templates;

public partial class CalculadoraFlex : ContentPage
{
    private TaskCompletionSource<EscolhaCombustivelVM> _escolhaCombustivel;
    public CalculadoraFlex(bool pEstaVindoDoServico)
    {
        InitializeComponent();
        Stack_EscolhaCombustivel.IsVisible = pEstaVindoDoServico;
        _escolhaCombustivel = new TaskCompletionSource<EscolhaCombustivelVM>();
    }

    private void SliderPercentual_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        // Atualiza o valor exibido no Entry
        PercentualEntry.Text = SliderPercentual.Value.ToString("F0");
    }

    private void CalcularButton_Clicked(object sender, EventArgs e)
    {
        AtualizarResultado();
        EtanolEntry.Unfocus();
        GasolinaEntry.Unfocus();
    }

    private void AtualizarResultado()
    {
        if (double.TryParse(GasolinaEntry.Text, out double precoGasolina) &&
            double.TryParse(EtanolEntry.Text, out double precoEtanol) &&
            double.TryParse(PercentualEntry.Text, out double percentual))
        {
            percentual /= 100;

            // Verifica qual combustível compensa
            if (precoEtanol / precoGasolina <= percentual)
            {
                ResultadoLabel.Text = "Compensa mais: Etanol";
            }
            else
            {
                ResultadoLabel.Text = "Compensa mais: Gasolina";
            }

            KmLabel.Text = $"Você andará mais com este combustível.";
        }
        else
        {
            ResultadoLabel.Text = "Insira valores válidos.";
            KmLabel.Text = "";
            EconomiaLabel.Text = "";
        }
    }

    private async void Etanol_Clicked(object sender, EventArgs e)
    {
        if (double.TryParse(EtanolEntry.Text, out double preco))
        {
            _escolhaCombustivel.TrySetResult(new EscolhaCombustivelVM()
            {
                TipoCombustivel = 3,
                Preco = preco
            });
            await Navigation.PopAsync(); // Voltar para a tela anterior
        }
        else
        {
            await Toast.Make("Coloque um valor válido").Show();
            EtanolEntry.Focus();
        }
    }

    public Task<EscolhaCombustivelVM> AguardarSelecaoAsync()
    {
        return _escolhaCombustivel.Task;
    }

    private async void Gasolina_Clicked(object sender, EventArgs e)
    {
        if (double.TryParse(GasolinaEntry.Text, out double preco))
        {
            _escolhaCombustivel.TrySetResult(new EscolhaCombustivelVM()
            {
                TipoCombustivel = 0,
                Preco = preco
            });
            await Navigation.PopAsync(); // Voltar para a tela anterior
        }
        else
        {
            await Toast.Make("Coloque um valor válido").Show();
            GasolinaEntry.Focus();
        }
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        if (Stack_EscolhaCombustivel.IsVisible)
        {
            if (!_escolhaCombustivel.Task.IsCompleted)
            {
                _escolhaCombustivel.TrySetResult(null);
            }
        }
    }
}