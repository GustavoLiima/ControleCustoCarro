using Cofauto.Helpers.ViewModel;
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
    private Menu _RelatorioSelecionado;

    public RelGeral()
    {
        InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
        Image_CarroSelecionado.Source = Global.carroSelecionado.Imagem;
        Label_CarroSelecionado.Text = Global.carroSelecionado.NomeVeiculo;
        DatePicker_De.Date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        CollectionView_PopUpInclusoes.ItemsSource = DbMenu._menusRelatorios;
        _RelatorioSelecionado = DbMenu._menusRelatorios[0];
        if (_VeiculoSelecionado == null)
        {
            _VeiculoSelecionado = Global.carroSelecionado;
        }
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        await CarregarRelatorio();
    }

    private async Task CarregarRelatorio()
    {
        OcultarTodosRelatorios();
        ActivityIndicator_Loading.IsVisible = true;
        if (_RelatorioSelecionado.Descricao == "Geral")
        {
            await CarregarRelatorioGeral();
            ActivityIndicator_Loading.IsVisible = false;
            Stack_Geral.IsVisible = true;
        }
        if (_RelatorioSelecionado.Descricao == "Abastecimento")
        {
            await CarregarRelatorioAbastecimento();
        }
        if (_RelatorioSelecionado.Descricao == "Serviço")
        {
            await CarregarRelatorioServico();
            ActivityIndicator_Loading.IsVisible = false;
            Stack_Servico.IsVisible = true;
        }
        if (_RelatorioSelecionado.Descricao == "Gastos")
        {
            await CarregarRelatorioDespesas();
            ActivityIndicator_Loading.IsVisible = false;
            Stack_Despesas.IsVisible = true;
        }
        if (_RelatorioSelecionado.Descricao == "Ganhos")
        {
            await CarregarRelatorioGanhos();
            ActivityIndicator_Loading.IsVisible = false;
            Stack_Ganho.IsVisible = true;
        }
    }

    private void OcultarTodosRelatorios()
    {
        Stack_Geral.IsVisible = false;
        Stack_Ganho.IsVisible = false;
        Stack_Despesas.IsVisible = false;
        Stack_Servico.IsVisible = false;
    }

    private async Task CarregarRelatorioGanhos()
    {
        Label_GanhoDetalhado.IsVisible = false;
        ListView_RelGanho.IsVisible = false;
        double Ganho = 0;
        double GanhoPorDia = 0;
        double GanhoPorKM = 0;
        double KmMinima = 0;
        double KmMax = 0;
        double KmPercorrida = 0;
        List<ModeloBaseRelatorio> listaObjetos = new List<ModeloBaseRelatorio>();


        List<Servico> dadosRelatorio = await servicoService.GetServicosEntreDatasGanhos(DatePicker_De.Date, DatePicker_Ate.Date, _VeiculoSelecionado.ID);
        if (dadosRelatorio.Count > 0)
        {
            KmMinima = dadosRelatorio.Min(x => x.Odometro);
            KmMax = dadosRelatorio.Max(x => x.Odometro);
            KmPercorrida = KmMax - KmMinima;

            foreach (var item in dadosRelatorio)
            {
                Ganho += item.ValorReceita;
                listaObjetos.Add(new ModeloBaseRelatorio()
                {
                    Descricao = item.DescricaoReceita,
                    Valor = item.ValorReceita,
                    Data = item.Data,
                    Hora = item.Hora
                });
            }

            int diferencaDias = Math.Abs((DatePicker_De.Date - DatePicker_Ate.Date).Days);

            GanhoPorDia = Ganho / diferencaDias;
            GanhoPorKM = Ganho / KmPercorrida;

            if (listaObjetos.Count > 0)
            {
                ListView_RelGanho.ItemsSource = listaObjetos;
                ListView_RelGanho.IsVisible = true;
                Label_GanhoDetalhado.IsVisible = true;
            }
            Label_GanhoPorKmGanho.Text = GanhoPorKM.ToString("N2");
            Label_GanhoPorDiaGanho.Text = GanhoPorDia.ToString("N2");
            Label_GanhoTotalGanho.Text = Ganho.ToString("N2");
        }
        else
        {
            Label_GanhoPorKmGanho.Text = "0,00";
            Label_GanhoPorDiaGanho.Text = "0,00";
            Label_GanhoTotalGanho.Text = "0,00";
        }
    }

    private async Task CarregarRelatorioDespesas()
    {
        Label_CustoDetalhado.IsVisible = false;
        ListView_RelDespesa.IsVisible = false;
        double Custos = 0;
        double CustosPorDia = 0;
        double CustosPorKM = 0;
        double KmMinima = 0;
        double KmMax = 0;
        double KmPercorrida = 0;
        List<ModeloBaseRelatorio> listaObjetos = new List<ModeloBaseRelatorio>();


        List<Servico> dadosRelatorio = await servicoService.GetServicosEntreDatasDespesas(DatePicker_De.Date, DatePicker_Ate.Date, _VeiculoSelecionado.ID);
        if (dadosRelatorio.Count > 0)
        {
            KmMinima = dadosRelatorio.Min(x => x.Odometro);
            KmMax = dadosRelatorio.Max(x => x.Odometro);
            KmPercorrida = KmMax - KmMinima;

            foreach (var item in dadosRelatorio)
            {
                Custos += item.ValorDespesa;
                listaObjetos.Add(new ModeloBaseRelatorio()
                {
                    Descricao = item.DescricaoReceita,
                    Valor = item.ValorDespesa,
                    Data = item.Data,
                    Hora = item.Hora
                });
            }
            
            int diferencaDias = Math.Abs((DatePicker_De.Date - DatePicker_Ate.Date).Days);

            CustosPorDia = Custos / diferencaDias;
            CustosPorKM = Custos / KmPercorrida;

            if(listaObjetos.Count > 0)
            {
                ListView_RelDespesa.ItemsSource = listaObjetos;
                ListView_RelDespesa.IsVisible = true;
                Label_CustoDetalhado.IsVisible = true;
            }
            Label_CustoPorKmDespesa.Text = CustosPorKM.ToString("N2");
            Label_CustoPorDiaDespesa.Text = CustosPorDia.ToString("N2");
            Label_CustoTotalDespesa.Text = Custos.ToString("N2");
        }
        else
        {
            Label_CustoPorKmDespesa.Text = "0,00";
            Label_CustoPorDiaDespesa.Text = "0,00";
            Label_CustoTotalDespesa.Text = "0,00";
        }

    }

    private async Task CarregarRelatorioServico()
    {
        Label_ServicoDetalhado.IsVisible = false;
        ListView_RelServico.IsVisible = false;
        double Servico = 0;
        double ServicoPorDia = 0;
        double ServicoPorKM = 0;
        double KmMinima = 0;
        double KmMax = 0;
        double KmPercorrida = 0;
        List<ModeloBaseRelatorio> listaObjetos = new List<ModeloBaseRelatorio>();


        List<Servico> dadosRelatorio = await servicoService.GetServicosEntreDatasServico(DatePicker_De.Date, DatePicker_Ate.Date, _VeiculoSelecionado.ID);
        if (dadosRelatorio.Count > 0)
        {
            KmMinima = dadosRelatorio.Min(x => x.Odometro);
            KmMax = dadosRelatorio.Max(x => x.Odometro);
            KmPercorrida = KmMax - KmMinima;

            foreach (var item in dadosRelatorio)
            {
                Servico += item.ValorDespesa;
                listaObjetos.Add(new ModeloBaseRelatorio()
                {
                    Descricao = item.DescricaoReceita,
                    Valor = item.ValorDespesa,
                    Data = item.Data,
                    Hora = item.Hora
                });
            }

            int diferencaDias = Math.Abs((DatePicker_De.Date - DatePicker_Ate.Date).Days);

            ServicoPorDia = Servico / diferencaDias;
            ServicoPorKM = Servico / KmPercorrida;

            if (listaObjetos.Count > 0)
            {
                ListView_RelServico.ItemsSource = listaObjetos;
                ListView_RelServico.IsVisible = true;
                Label_ServicoDetalhado.IsVisible = true;
            }
            Label_ServicoPorKmServico.Text = ServicoPorKM.ToString("N2");
            Label_ServicoPorDiaServico.Text = ServicoPorDia.ToString("N2");
            Label_ServicoTotalServico.Text = Servico.ToString("N2");
        }
        else
        {
            Label_ServicoPorKmServico.Text = "0,00";
            Label_ServicoPorDiaServico.Text = "0,00";
            Label_ServicoTotalServico.Text = "0,00";
        }
    }

    private Task CarregarRelatorioAbastecimento()
    {
        throw new NotImplementedException();
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

        var popup = new PopUp_Veiculos(Global._Veiculos);

        // Exibe o popup e aguarda o veículo selecionado
        var veiculoSelecionado = Navigation.PushAsync(popup);

        var resultado = await popup.ObterVeiculoSelecionadoAsync();


        // Exibe o veículo selecionado
        if (resultado != null)
        {
            _VeiculoSelecionado = resultado;
            Label_CarroSelecionado.Text = _VeiculoSelecionado.NomeVeiculo;
            Image_CarroSelecionado.Source = _VeiculoSelecionado.Imagem;
            await CarregarRelatorio();
        }
    }

    private void TapGestureRecognizer_Tapped_1(object sender, TappedEventArgs e)
    {
        PopupServiceSelection.IsVisible = true;
    }

    private void OnCancelPopupClicked(object sender, EventArgs e)
    {
        PopupServiceSelection.IsVisible = false;
    }

    private async void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        Menu? selectedItem = e.CurrentSelection.FirstOrDefault() as Menu;

        if (selectedItem != null)
        {
            _RelatorioSelecionado = selectedItem;
            ((CollectionView)sender).SelectedItem = null;
            Label_Categoria.Text = selectedItem.Descricao;
            Image_Categoria.Source = selectedItem.Imagem;
            PopupServiceSelection.IsVisible = false;
            await CarregarRelatorio();
        }
    }
}