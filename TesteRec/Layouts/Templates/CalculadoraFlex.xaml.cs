namespace TesteRec.Layouts.Templates;

public partial class CalculadoraFlex : ContentPage
{
	public CalculadoraFlex()
	{
		InitializeComponent();
	}

    private void SliderPercentual_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        // Atualiza o valor exibido no Entry
        PercentualEntry.Text = SliderPercentual.Value.ToString("F0");
    }

    private void CalcularButton_Clicked(object sender, EventArgs e)
    {
        AtualizarResultado();
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

}