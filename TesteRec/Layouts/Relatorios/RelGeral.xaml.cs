using Cofauto.Layouts.Templates;
using System;
using TesteRec.API.Models;
using TesteRec.Db;
using TesteRec.Db.Models;
using TesteRec.Db.Services;

namespace FinaGear.Layouts.Relatorios;

public partial class RelGeral : ContentPage
{
    private ServicoDB servicoService = new ServicoDB();
    private VeiculoModel _VeiculoSelecionado = null;

    public RelGeral()
    {
        InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
        Image_CarroSelecionado.Source = Global.carroSelecionado.Imagem;
        Label_CarroSelecionado.Text = Global.carroSelecionado.NomeVeiculo;
        DatePicker_De.Date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        if (_VeiculoSelecionado == null)
        {
            _VeiculoSelecionado = Global.carroSelecionado;
        }
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        await CarregarRelatorioGeral();
    }

    private async Task CarregarRelatorioGeral()
    {
        double Ganhos = 0;
        double GanhosPorDia = 0;
        double GanhosPorKM = 0;
        double KmMinima = 0;
        double KmMax = 0;
        double KmPercorrida = 0;
        int diferencaDias = Math.Abs((DatePicker_De.Date - DatePicker_Ate.Date).Days);

        double Custos = 0;
        double CustosPorDia = 0;
        double CustosPorKM = 0;

        List<Servico> dadosRelatorio = await servicoService.GetServicosEntreDatas(DatePicker_De.Date, DatePicker_Ate.Date, _VeiculoSelecionado.ID);
        if (dadosRelatorio.Count > 0)
        {

            KmMinima = dadosRelatorio.Min(x => x.Odometro);
            KmMax = dadosRelatorio.Max(x => x.Odometro);
            KmPercorrida = KmMax - KmMinima;

            List<Servico> ganhos = dadosRelatorio.Where(s => s.AcaoSelecionada == 2).ToList();
            dadosRelatorio = dadosRelatorio.Where(s => s.AcaoSelecionada != 2).ToList();


            foreach (var item in ganhos)
            {
                Ganhos += item.ValorReceita;
            }
            GanhosPorDia = Ganhos / diferencaDias;
            GanhosPorKM = Ganhos / KmPercorrida;

            foreach (var item in dadosRelatorio)
            {
                Custos += item.ValorTotal;
                Custos += item.ValorDespesa;
            }

            CustosPorDia = Custos / diferencaDias;
            CustosPorKM = Custos / KmPercorrida;

            Label_GanhoTotal.Text = Ganhos.ToString("N2");
            Label_GanhoPorDia.Text = GanhosPorDia.ToString("N2");
            Label_GanhoPorKm.Text = GanhosPorKM.ToString("N2");
            Label_KmTotal.Text = KmPercorrida + " km";
            Label_KmMediaDiaria.Text = (KmPercorrida / diferencaDias).ToString("N2") + " km";
            Label_CustoTotal.Text = Custos.ToString("N2");
            Label_CustoPorDia.Text = CustosPorDia.ToString("N2");
            Label_CustoPorKm.Text = CustosPorKM.ToString("N2");
            Label_LiquidoPorKm.Text = (GanhosPorKM - CustosPorKM).ToString("N2");
            Label_LiquidoPorDia.Text = (GanhosPorDia - CustosPorDia).ToString("N2");
            Label_LiquidoTotal.Text = (Ganhos - Custos).ToString("N2");
        }
        else
        {
            Label_GanhoTotal.Text = Label_GanhoPorDia.Text = Label_GanhoPorKm.Text = "0,00";
            Label_KmTotal.Text = Label_KmMediaDiaria.Text = "0 km";
            Label_CustoTotal.Text = Label_CustoPorDia.Text = Label_CustoPorKm.Text = "0,00";
            Label_LiquidoTotal.Text = Label_LiquidoPorDia.Text = Label_LiquidoPorKm.Text = "0,00";
        }
    }

    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        //var popup = new PopUp_Veiculos(Global._Veiculos);

        //// Exibe o popup e aguarda o veículo selecionado
        //var veiculoSelecionado = await Navigation.PushModalAsync(popup).ContinueWith(async task =>
        //{
        //    return await popup.ObterVeiculoSelecionadoAsync();
        //});

        //// Exibe o veículo selecionado
        //if (veiculoSelecionado != null)
        //{
        //    _VeiculoSelecionado = veiculoSelecionado.Result;
        //    await CarregarRelatorioGeral();
        //}
    }
}