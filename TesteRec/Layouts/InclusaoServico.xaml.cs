using Cofauto.Db.Services;
using Cofauto.Layouts.Templates;
using Microsoft.Maui;
using Newtonsoft.Json;
using System.Collections.Generic;
using TesteRec.Db;
using TesteRec.Db.Models;
using TesteRec.Db.Services;
using TesteRec.Enum;
using TesteRec.Layouts.listas;
using TesteRec.Layouts.Templates;
using TesteRec.Model;

namespace TesteRec.Layouts;

public partial class InclusaoServico : ContentPage
{
    EMenuSelecionado _menuSelecionado;
    private Pagamentos _pagamentoSelecionado = new Pagamentos();
    private Model.TiposCombustivel _combustivelSelecionado = new Model.TiposCombustivel();
    private Despesa _despesaSelecionada = new Despesa();
    private Db.Models.TipoReceita _receitaSelecionada = new Db.Models.TipoReceita();
    private Db.Models.TipoServico _servicoSelecionado = new Db.Models.TipoServico();
    private bool _ApenasUmaVez = true;


    private List<Db.Models.TipoServico> _tiposServico = null;
    private List<Despesa> _tiposDespesa = null;

    private Servico _servico;

    public InclusaoServico(EMenuSelecionado pMenu)
    {
        InitializeComponent();
        _menuSelecionado = pMenu;
        CarregarInformacoesIniciais(pMenu);
        Shell.SetTitleColor(this, Color.FromHex("#ecf0f1"));
    }

    public InclusaoServico(Servico pServico)
    {
        InitializeComponent();
        _menuSelecionado = (EMenuSelecionado)pServico.AcaoSelecionada;
        CarregarInformacoesIniciais(_menuSelecionado);
        _servico = pServico;
        Shell.SetTitleColor(this, Color.FromHex("#ecf0f1"));
        CarregarDadosTela();
    }

    private async void CarregarDadosTela()
    {
        if (_servico.AcaoSelecionada != (int)EMenuSelecionado.Lembrete)
        {
            datePickerServico.Date = _servico.Data;
            timePickerServico.Time = _servico.Hora;
            entryOdometro.Text = _servico.Odometro.ToString();
            entryMotorista.Text = _servico.Motorista;
            Editor_Observacao.Text = _servico.Descricao;
        }
        switch (_servico.AcaoSelecionada)
        {
            case (int)EMenuSelecionado.Lembrete:
                Entry_OdometroLembrete.Text = _servico.LembreteKilometragem.ToString();
                DatePicker_Lembrete.Date = _servico.DataLembrete;
                CheckBox_Data.IsChecked = _servico.LembrarEmData;
                CheckBox_Kilometragem.IsChecked = _servico.LembrarEmKm;
                if(_servico.LembreteFoiServico)
                {
                    var servicoSelecionado = DbTipoServico._tipoServicos.Find(x => x.Id == _servico.TipoServico);
                    if(servicoSelecionado != null)
                    {
                        Label_TipoServico.Text = servicoSelecionado.Descricao;
                        _servicoSelecionado = servicoSelecionado;
                    }
                }
                else
                {
                    Button_Despesa_Clicked(null, null);
                    var despesaSelecionada = DbDespesa._listaDespesa.Find(x => x.Id == _servico.TipoDespesa);
                    if(despesaSelecionada != null)
                    {
                        Label_TipoDespesa.Text = despesaSelecionada.Descricao;
                        _despesaSelecionada = despesaSelecionada;
                    }
                }
                break;
            case (int)EMenuSelecionado.Checklist:
                break;
            case (int)EMenuSelecionado.Receita:
                entryValor.Text = _servico.ValorReceita.ToString();
                _receitaSelecionada = _servico.ReceitaModelo;
                Label_TipoReceita.Text = _receitaSelecionada.Descricao;
                Image_TipoReceita.Source = _receitaSelecionada.Imagem;
                break;
            case (int)EMenuSelecionado.Percurso:
                break;
            case (int)EMenuSelecionado.Serviço:
                _pagamentoSelecionado = _servico.FormaPagamentoModelo;
                if (_pagamentoSelecionado != null)
                {
                    Label_TipoPagamento.Text = _pagamentoSelecionado.Descricao;
                    Image_FormaPagamento.Source = _pagamentoSelecionado.Imagem;
                }
                _servicoSelecionado = _servico.TipoServicoModelo;
                Entry_ValorDespesa.Text = _servico.ValorDespesa.ToString();


                ServicoDB db = new ServicoDB();
                var resultado = await db.GetTipoServicoPorIDAsync(_servico.Id);
                if (resultado.Count > 0)
                {
                    CarregarTiposServicos();
                    foreach (var item in resultado)
                    {
                        var item2 = _tiposServico.FirstOrDefault(x => x.Descricao == item.Descricao);

                        if (item2 != null)
                        {
                            // Atualiza as propriedades de item2 com base em item1
                            item2.IsSelected = item.IsSelected;
                            item2.Valor = item.Valor;
                            item2.EnviadoServidor = item.EnviadoServidor;
                        }
                    }

                    ListView_ServicosSelecionados.ItemsSource = resultado;
                    ListView_ServicosSelecionados.IsVisible = true;
                    double ValorTotal = 0;
                    foreach (var item in resultado)
                    {
                        ValorTotal += item.Valor;
                    }
                    Label_ValorTotalServicos.Text = ValorTotal.ToString("N2");
                    Stack_ValoresTotaisServicos.IsVisible = true;
                }

                //Image_TipoServico.Source = _servicoSelecionado.Imagem;
                break;
            case (int)EMenuSelecionado.Despesa:
                Entry_ValorDespesa.Text = _servico.ValorDespesa.ToString();
                _despesaSelecionada = _servico.DespesaModelo;
                _pagamentoSelecionado = _servico.FormaPagamentoModelo;
                if(_pagamentoSelecionado != null)
                {
                    Label_TipoPagamento.Text = _pagamentoSelecionado.Descricao;
                    Image_FormaPagamento.Source = _pagamentoSelecionado.Imagem;
                }

                DespesaDB dbd = new DespesaDB();
                var result = await dbd.GetTipoDespesaPorIDAsync(_servico.Id);
                if (result.Count > 0)
                {
                    CarregarTiposDespesa();
                    foreach (var item in result)
                    {
                        var item2 = _tiposDespesa.FirstOrDefault(x => x.Descricao == item.Descricao);

                        if (item2 != null)
                        {
                            // Atualiza as propriedades de item2 com base em item1
                            item2.IsSelected = item.IsSelected;
                            item2.Valor = item.Valor;
                            item2.EnviadoServidor = item.EnviadoServidor;
                        }
                    }

                    ListView_DespesasSelecionados.ItemsSource = result;
                    ListView_DespesasSelecionados.IsVisible = true;
                    double ValorTotal = 0;
                    foreach (var item in result)
                    {
                        ValorTotal += item.Valor;
                    }
                    Label_ValorTotalDespesas.Text = ValorTotal.ToString("N2");
                    Stack_ValoresTotaisDespesas.IsVisible = true;
                }
                break;
            case (int)EMenuSelecionado.Abastecimento:
                entryPreco.Text = _servico.Preco.ToString();
                entryValorTotal.Text = _servico.ValorTotal.ToString();
                entryLitros.Text = _servico.Litros.ToString();
                _pagamentoSelecionado = _servico.FormaPagamentoModelo;
                if(_pagamentoSelecionado != null)
                {
                    Label_TipoPagamento.Text = _pagamentoSelecionado.Descricao;
                    Image_FormaPagamento.Source = _pagamentoSelecionado.Imagem;
                }
                _combustivelSelecionado = _servico.CombustivelModelo;
                Label_Combustivel.Text = _combustivelSelecionado.Descricao.ToString();
                Image_TipoCombustivel.Source = _combustivelSelecionado.Imagem.ToString();
                break;
            default:
                break;
        }
    }

    private async void CarregarInformacoesIniciais(EMenuSelecionado pMenu)
    {
        timePickerServico.Time = DateTime.Now.TimeOfDay;
        entryOdometro.Text = Global.carroSelecionado.Kilometragem.ToString();
        switch (pMenu)
        {
            case EMenuSelecionado.Lembrete:
                Shell.SetBackgroundColor(this, Color.FromHex("#d35400"));
                Button_Gravar.BackgroundColor = Color.FromHex("#d35400");
                Title = "Lembrete";
                Entry_OdometroLembrete.Text = Global.carroSelecionado.Kilometragem.ToString();
                Grid_BotoesServicoDespesa.IsVisible = true;
                Grid_Servico.IsVisible = true;
                //Grid_BotoesPeriodicidade.IsVisible = true;
                Label_InformacoesPeriodicidade.IsVisible = true;
                Grid_InformacoesPeriodicidade.IsVisible = true;
                break;
            case EMenuSelecionado.Checklist:
                Shell.SetBackgroundColor(this, Color.FromHex("#8e44ad"));
                Button_Gravar.BackgroundColor = Color.FromHex("#8e44ad");
                Title = "Checklist";
                break;
            case EMenuSelecionado.Receita:
                Shell.SetBackgroundColor(this, Color.FromHex("#16a085"));
                Button_Gravar.BackgroundColor = Color.FromHex("#16a085");
                Title = "Receita";
                Grid_Valor.IsVisible = true;
                Grid_Receita.IsVisible = true;
                Grid_DataHorario.IsVisible = true;
                Grid_Odometro.IsVisible = true;
                Grid_Motorista.IsVisible = true;
                break;
            case EMenuSelecionado.Percurso:
                Shell.SetBackgroundColor(this, Color.FromHex("#7f8c8d"));
                Button_Gravar.BackgroundColor = Color.FromHex("#7f8c8d");
                Title = "Percurso";
                Grid_DataHorario.IsVisible = true;
                Grid_Odometro.IsVisible = true;
                Grid_Motorista.IsVisible = true;
                break;
            case EMenuSelecionado.Serviço:
                Shell.SetBackgroundColor(this, Color.FromHex("#9b59b6"));
                Button_Gravar.BackgroundColor = Color.FromHex("#9b59b6");
                Grid_Servico.IsVisible = true;
                Title = "Serviço";
                Grid_TipoPagamento.IsVisible = true;
                Grid_DataHorario.IsVisible = true;
                Grid_Odometro.IsVisible = true;
                Grid_Motorista.IsVisible = true;
                break;
            case EMenuSelecionado.Despesa:
                Shell.SetBackgroundColor(this, Color.FromHex("#c0392b"));
                Button_Gravar.BackgroundColor = Color.FromHex("#c0392b");
                Title = "Despesa";
                Grid_Despesa.IsVisible = true;
                Grid_TipoPagamento.IsVisible = true;
                Grid_DataHorario.IsVisible = true;
                Grid_Odometro.IsVisible = true;
                Grid_Motorista.IsVisible = true;
                var despesaService = new ServicoDB();
                Servico retDesp = await despesaService.GetUltimoAbastecimento();
                if (retDesp != null)
                {
                    if(retDesp.FormaPagamentoModelo != null)
                    {
                        _pagamentoSelecionado = retDesp.FormaPagamentoModelo;
                        Label_TipoPagamento.Text = _pagamentoSelecionado.Descricao;
                        Image_FormaPagamento.Source = _pagamentoSelecionado.Imagem;
                    }
                }
                break;
            case EMenuSelecionado.Abastecimento:
                Shell.SetBackgroundColor(this, Color.FromHex("#f39c12"));
                Title = "Abastecimento";
                Button_CalculadoraFlex.IsVisible = Grid_ValoresCombustivel.IsVisible = Grid_Combustivel.IsVisible = true;
                Grid_TipoPagamento.IsVisible = true;
                Grid_DataHorario.IsVisible = true;
                Grid_Odometro.IsVisible = true;
                Grid_Motorista.IsVisible = true;
                var servicoService = new ServicoDB();
                Servico obj = await servicoService.GetUltimoAbastecimento();
                if (obj != null)
                {
                    _combustivelSelecionado = obj.CombustivelModelo;
                    Label_Combustivel.Text = _combustivelSelecionado.Descricao;
                    Image_TipoCombustivel.Source = _combustivelSelecionado.Imagem;
                    if (obj.FormaPagamentoModelo != null)
                    {
                        _pagamentoSelecionado = obj.FormaPagamentoModelo;
                        Label_TipoPagamento.Text = _pagamentoSelecionado.Descricao;
                        Image_FormaPagamento.Source = _pagamentoSelecionado.Imagem;
                    }
                }
                break;
            default:
                break;
        }

        // Verifica se a página está contida em uma NavigationPage
        if (Parent is NavigationPage navigationPage)
        {
            navigationPage.BarBackgroundColor = Color.FromArgb("#f1c40f"); // Defina a cor desejada
        }

        listas.TiposCombustivel tipo = new listas.TiposCombustivel();
        CollectionView_TipoCombustivel.ItemsSource = tipo._tipoCombustivel;
        FormasPagamento formaPagamento = new FormasPagamento();
        CollectionView_TipoPagamento.ItemsSource = formaPagamento._tiposPagamentos;
        CollectionView_TipoDespesa.ItemsSource = DbDespesa._listaDespesa;
        CollectionView_TipoServico.ItemsSource = DbTipoServico._tipoServicos;
        CollectionView_TipoReceita.ItemsSource = DbTipoReceita._tipoReceitas;
    }

    private async void OnSalvarServicoClicked(object sender, EventArgs e)
    {
        var servicoService = new ServicoDB();
        var despesaService = new DespesaDB();
        if (_menuSelecionado != EMenuSelecionado.Lembrete)
        {
            if (string.IsNullOrEmpty(entryOdometro.Text))
            {
                await DisplayAlert("Atenção", "É necessário colocar o valor do odômetro", "continuar");
                entryValor.Focus();
                return;
            }
            entryOdometro.Text = entryOdometro.Text.Replace(".", "").Replace(",", "");
        }
        DateTime dataComHora = datePickerServico.Date;
        dataComHora.AddHours(timePickerServico.Time.Hours);
        dataComHora.AddMinutes(timePickerServico.Time.Minutes);

        switch (_menuSelecionado)
        {
            case EMenuSelecionado.Lembrete:
                if(Grid_Servico.IsVisible && _servicoSelecionado.Id == 0)
                {
                    await DisplayAlert("Atenção", "É necessário selecionar um serviço para salvar o lembrete", "Continuar");
                    return;
                }
                if(!Grid_Servico.IsVisible && _despesaSelecionada.Id == 0)
                {
                    await DisplayAlert("Atenção", "É necessário selecionar uma despesa para salvar o lembrete", "Continuar");
                    return;
                }
                if (CheckBox_Kilometragem.IsChecked)
                {
                    if (string.IsNullOrEmpty(Entry_OdometroLembrete.Text))
                    {
                        await DisplayAlert("Atenção", "É necessário colocar com quantos kilometros deseja receber o alerta", "continuar");
                        Entry_OdometroLembrete.Focus();
                        return;
                    }
                }
                Servico objAddServico = new Servico
                {
                    AcaoSelecionada = (int)_menuSelecionado,
                    TipoDespesa = _despesaSelecionada.Id,
                    TipoServico = _servicoSelecionado.Id,
                    LembreteFoiServico = Grid_Servico.IsVisible,
                    ApenasUmaVez = _ApenasUmaVez,
                    LembrarEmKm = CheckBox_Kilometragem.IsChecked,
                    LembrarEmData = CheckBox_Data.IsChecked,
                    DataLembrete = DatePicker_Lembrete.Date,
                    Descricao = Editor_Observacao.Text
                };
                if (CheckBox_Kilometragem.IsChecked && string.IsNullOrEmpty(Entry_OdometroLembrete.Text))
                {
                    await DisplayAlert("Atenção", "Preencha daqui quantos kilometros deseja ser alertado", "Continuar");
                    Entry_OdometroLembrete.Focus();
                    return;
                }
                else if (CheckBox_Kilometragem.IsChecked && !string.IsNullOrEmpty(Entry_OdometroLembrete.Text))
                {
                    double.TryParse(Entry_OdometroLembrete.Text, out double valor);
                    if(valor < Global.carroSelecionado.Kilometragem)
                    {
                        await DisplayAlert("Atenção", $"É necessário colocar uma kilometragem maior do que a do veículo atual \nA kilometragem atual do {Global.carroSelecionado.NomeVeiculo} é de {Global.carroSelecionado.Kilometragem}", "Continuar");
                        return;
                    }
                    objAddServico.LembreteKilometragem = valor;
                }
                await servicoService.AddServicoAsync(objAddServico);
                break;
            case EMenuSelecionado.Checklist:
                break;
            case EMenuSelecionado.Receita:
                if (string.IsNullOrEmpty(entryValor.Text))
                {
                    await DisplayAlert("Atenção", "É necessário colocar o valor recebido", "continuar");
                    entryValor.Focus();
                    return;
                }
                if(_receitaSelecionada.Id == 0)
                {
                    await DisplayAlert("Atenção", "É necessário selecionar um tipo de receita", "continuar");
                    return;
                }
                Servico objAdd = new Servico
                {
                    AcaoSelecionada = (int)_menuSelecionado,
                    Data = dataComHora,
                    Hora = timePickerServico.Time,
                    Odometro = double.Parse(entryOdometro.Text),
                    ValorReceita = double.Parse(entryValor.Text),
                    Receita = _receitaSelecionada.Id,
                    Motorista = entryMotorista.Text,
                    Descricao = Editor_Observacao.Text,
                };
                if (_servico != null)
                {
                    objAdd.Id = _servico.Id;
                }
                await servicoService.AddServicoAsync(objAdd);
                await AtualizarKMVeiculo(objAdd);
                break;
            case EMenuSelecionado.Percurso:
                break;
            case EMenuSelecionado.Serviço:
                if (!ListView_ServicosSelecionados.IsVisible)
                {
                    await DisplayAlert("Atenção", "É necessário selecionar no mínimo 1 serviço", "continuar");
                    return;
                }

                Servico objAddServ = new Servico()
                {
                    AcaoSelecionada = (int)_menuSelecionado,
                    Data = dataComHora,
                    Hora = timePickerServico.Time,
                    Odometro = double.Parse(entryOdometro.Text),
                    Motorista = entryMotorista.Text,
                    Descricao = Editor_Observacao.Text,
                    ValorDespesa = double.Parse(Label_ValorTotalServicos.Text)
                };
                if (_pagamentoSelecionado != null)
                {
                    objAddServ.FormaPagamento = _pagamentoSelecionado.Id;
                }
                if (_servico != null)
                {
                    objAddServ.Id = _servico.Id;
                }
                var descricaoServicos = _tiposServico.Where(x => x.IsSelected).Select(s => s.Descricao).ToList();
                objAddServ.DescricaoServico = string.Join(" \n", descricaoServicos);
                int IdServico = await servicoService.AddServicoAsync(objAddServ);
                foreach (var item in _tiposServico.Where(x => x.IsSelected))
                {
                    Db.Models.TipoServico objAd = new Db.Models.TipoServico()
                    {
                        Id = item.Id,
                        Descricao = item.Descricao,
                        EnviadoServidor = item.EnviadoServidor,
                        IdServico = IdServico,
                        IsSelected = item.IsSelected,
                        Valor = item.Valor
                    };
                    await servicoService.AddTipoServicoAsync(objAd);
                }
                await AtualizarKMVeiculo(objAddServ);
                break;
            case EMenuSelecionado.Despesa:
                if (!ListView_DespesasSelecionados.IsVisible)
                {
                    await DisplayAlert("Atenção", "É necessário selecionar no mínimo uma despesa", "continuar");
                    return;
                }
                Servico objAddDesp = new Servico()
                {
                    AcaoSelecionada = (int)_menuSelecionado,
                    Data = dataComHora,
                    Hora = timePickerServico.Time,
                    Odometro = double.Parse(entryOdometro.Text),
                    Motorista = entryMotorista.Text,
                    Descricao = Editor_Observacao.Text,
                    ValorDespesa = double.Parse(Label_ValorTotalDespesas.Text)
                };
                if(_pagamentoSelecionado != null)
                {
                    objAddDesp.FormaPagamento = _pagamentoSelecionado.Id;
                }
                if (_servico != null)
                {
                    objAddDesp.Id = _servico.Id;
                }
                var descricaoDespesa = _tiposDespesa.Where(x => x.IsSelected).Select(s => s.Descricao).ToList();
                objAddDesp.DescricaoDespesa = string.Join(" \n", descricaoDespesa);
                int IdServ = await servicoService.AddServicoAsync(objAddDesp);

                foreach (var item in _tiposDespesa.Where(x => x.IsSelected))
                {
                    Despesa objAd = new Despesa()
                    {
                        Id = item.Id,
                        Descricao = item.Descricao,
                        EnviadoServidor = item.EnviadoServidor,
                        IdServico = IdServ,
                        IsSelected = item.IsSelected,
                        Valor = item.Valor
                    };
                    await despesaService.AddDespesaAsync(objAd);
                }

                await AtualizarKMVeiculo(objAddDesp);
                break;
            case EMenuSelecionado.Abastecimento:
                if (string.IsNullOrEmpty(entryPreco.Text))
                {
                    await DisplayAlert("Atenção", "Preencha o valor do preço do combustível", "continuar");
                    entryPreco.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(entryValorTotal.Text))
                {
                    await DisplayAlert("Atenção", "Preencha o valor total do abastecimento", "continuar");
                    entryValorTotal.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(entryLitros.Text))
                {
                    await DisplayAlert("Atenção", "Preencha a quantidade de litros", "continuar");
                    entryLitros.Focus();
                    return;
                }
                Servico objAddAbast = new Servico()
                {
                    AcaoSelecionada = (int)_menuSelecionado,
                    Data = dataComHora,
                    Hora = timePickerServico.Time,
                    Odometro = double.Parse(entryOdometro.Text),
                    Combustivel = _combustivelSelecionado.Id,
                    Preco = double.Parse(entryPreco.Text),
                    ValorTotal = double.Parse(entryValorTotal.Text),
                    Litros = double.Parse(entryLitros.Text),
                    Motorista = entryMotorista.Text,
                    FormaPagamento = _pagamentoSelecionado.Id,
                    Descricao = Editor_Observacao.Text
                };
                if (_servico != null)
                {
                    objAddAbast.Id = _servico.Id;
                }
                await servicoService.AddServicoAsync(objAddAbast);
                await AtualizarKMVeiculo(objAddAbast);
                break;
            default:
                break;
        }

        await Navigation.PopAsync();
    }

    private static async Task AtualizarKMVeiculo(Servico objAdd)
    {
        if (objAdd.Odometro > Global.carroSelecionado.Kilometragem)
        {
            Global.carroSelecionado.Kilometragem = (int)objAdd.Odometro;
            await SecureStorage.Default.SetAsync("veiculoSelecionado", JsonConvert.SerializeObject(Global.carroSelecionado));
            await new VeiculoDB().UpdateVeiculoAsync(Global.carroSelecionado);
        }
    }

    // Quando a data é selecionada, foca no próximo campo (TimePicker)
    private void OnDatePickerUnfocused(object sender, FocusEventArgs e)
    {
        timePickerServico.Focus();
    }

    // Quando a hora é selecionada, foca no próximo campo (Odômetro)
    private void OnTimePickerUnfocused(object sender, FocusEventArgs e)
    {
        entryOdometro.Focus();
    }

    // Quando o campo de odômetro é completado, foca no próximo campo (Combustível)
    private void OnOdometroCompleted(object sender, EventArgs e)
    {
        //entryCombustivel.Focus();
    }

    // Quando o campo de combustível é completado, foca no próximo campo (Preço)
    private void OnCombustivelCompleted(object sender, EventArgs e)
    {
        entryPreco.Focus();
    }

    // Quando o campo de preço é completado, foca no próximo campo (Valor Total)
    private void OnPrecoCompleted(object sender, EventArgs e)
    {
        entryValorTotal.Focus();
    }

    // Quando o campo de valor total é completado, foca no próximo campo (Litros)
    private void OnValorTotalCompleted(object sender, EventArgs e)
    {
        entryLitros.Focus();
    }

    // Quando o campo de litros é completado, foca no próximo campo (Motorista)
    private void OnLitrosCompleted(object sender, EventArgs e)
    {
        entryMotorista.Focus();
    }

    // Quando o campo de motorista é completado, ativa o botão de salvar
    private void OnMotoristaCompleted(object sender, EventArgs e)
    {
        OnSalvarServicoClicked(sender, e);
    }

    private void OnDatePickerFocused(object sender, FocusEventArgs e)
    {

    }

    private void OnTimePickerFocused(object sender, FocusEventArgs e)
    {

    }

    private void entryPreco_Unfocused(object sender, FocusEventArgs e)
    {
        double valGas = 0;
        double litros = 0;

        if (!string.IsNullOrEmpty(entryPreco.Text))
        {
            valGas = double.Parse(entryPreco.Text);
        }

        if (!string.IsNullOrEmpty(entryLitros.Text))
        {
            litros = double.Parse(entryLitros.Text);
            if (litros > 0 && valGas > 0)
            {
                entryValorTotal.Text = (valGas * litros).ToString("N2");
                return;
            }
        }

    }

    private void entryValorTotal_Unfocused(object sender, FocusEventArgs e)
    {
        double valTotal = 0;
        double valGas = 0;
        double litros = 0;

        if (!string.IsNullOrEmpty(entryValorTotal.Text))
        {
            valTotal = double.Parse(entryValorTotal.Text);
        }

        if (!string.IsNullOrEmpty(entryPreco.Text))
        {
            valGas = double.Parse(entryPreco.Text);
            if (valGas > 0 && valTotal > 0)
            {
                entryLitros.Text = (valTotal / valGas).ToString("N2");
                return;
            }
        }

        if (!string.IsNullOrEmpty(entryLitros.Text))
        {
            litros = double.Parse(entryLitros.Text);
            if (litros > 0 && valTotal > 0)
            {
                entryPreco.Text = (valTotal / litros).ToString("N2");
                return;
            }
        }
    }

    private void entryLitros_Unfocused(object sender, FocusEventArgs e)
    {
        double valTotal = 0;
        double valGas = 0;
        double litros = 0;

        if (!string.IsNullOrEmpty(entryLitros.Text))
        {
            litros = double.Parse(entryLitros.Text);

        }

        if (!string.IsNullOrEmpty(entryValorTotal.Text))
        {
            valTotal = double.Parse(entryValorTotal.Text);
            if (litros > 0 && valTotal > 0)
            {
                entryPreco.Text = (valTotal / litros).ToString("N2");
                return;
            }
        }

        if (!string.IsNullOrEmpty(entryPreco.Text))
        {
            valGas = double.Parse(entryPreco.Text);
            if (valGas > 0 && litros > 0)
            {
                entryValorTotal.Text = (valGas * litros).ToString("N2");
                return;
            }
        }
    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        PopupTipoCombustivel.IsVisible = true;
    }

    private void OnCancelPopupClicked(object sender, EventArgs e)
    {
        PopupTipoPagamento.IsVisible = PopupTipoCombustivel.IsVisible = false;
        PopupTipoDespesa.IsVisible = false;
        PopupTipoServico.IsVisible = false;
        PopupTipoReceita.IsVisible = false;
    }

    private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        Model.TiposCombustivel selectedItem = e.CurrentSelection.FirstOrDefault() as Model.TiposCombustivel;

        if (selectedItem != null)
        {
            Image_TipoCombustivel.Source = selectedItem.Imagem;
            Label_Combustivel.Text = selectedItem.Descricao;
            _combustivelSelecionado = selectedItem;
            PopupTipoCombustivel.IsVisible = false;
            ((CollectionView)sender).SelectedItem = null;
        }
    }

    private void TapGestureRecognizerTipoPagamento_Tapped(object sender, TappedEventArgs e)
    {
        PopupTipoPagamento.IsVisible = true;
    }

    private void OnSelectionChangedTipoPagamento(object sender, SelectionChangedEventArgs e)
    {
        Pagamentos selectedItem = e.CurrentSelection.FirstOrDefault() as Pagamentos;

        if (selectedItem != null)
        {
            Image_FormaPagamento.Source = selectedItem.Imagem;
            Label_TipoPagamento.Text = selectedItem.Descricao;
            _pagamentoSelecionado = selectedItem;
            PopupTipoPagamento.IsVisible = false;
            ((CollectionView)sender).SelectedItem = null;
        }
    }

    private async void TapGestureRecognizerTipoDespesa_Tapped(object sender, TappedEventArgs e)
    {

        if (_menuSelecionado == EMenuSelecionado.Lembrete)
        {
            PopupTipoDespesa.IsVisible = true;
        }
        else
        {
            CarregarTiposDespesa();

            var popup = new PopUpDespesas(_tiposDespesa);

            // Exibe o popup e aguarda o veículo selecionado
            var despesasSelecionado = Navigation.PushAsync(popup);

            var resultado = await popup.ObterDespesasSelecionadoAsync();


            // Exibe o veículo selecionado
            if (resultado != null)
            {
                if (resultado.Count > 0)
                {
                    ListView_DespesasSelecionados.ItemsSource = resultado;
                    ListView_DespesasSelecionados.IsVisible = true;
                    double ValorTotal = 0;
                    foreach (var item in resultado)
                    {
                        ValorTotal += item.Valor;
                    }
                    Label_ValorTotalDespesas.Text = ValorTotal.ToString("N2");
                    Stack_ValoresTotaisDespesas.IsVisible = true;
                }
                else
                {
                    ListView_DespesasSelecionados.ItemsSource = null;
                    ListView_DespesasSelecionados.IsVisible = false;
                    Label_ValorTotalDespesas.IsVisible = false;
                }
            }
        }
    }

    private void OnSelectionChangedTipoDespesa(object sender, SelectionChangedEventArgs e)
    {
        Despesa selectedItem = e.CurrentSelection.FirstOrDefault() as Despesa;

        if (selectedItem != null)
        {
            //Image_TipoDespesa.Source = selectedItem.Imagem;
            Label_TipoDespesa.Text = selectedItem.Descricao;
            PopupTipoDespesa.IsVisible = false;
            _despesaSelecionada = selectedItem;
            ((CollectionView)sender).SelectedItem = null;
        }
    }

    private void OnSelectionChangedTipoServico(object sender, SelectionChangedEventArgs e)
    {
        Db.Models.TipoServico selectedItem = e.CurrentSelection.FirstOrDefault() as Db.Models.TipoServico;

        if (selectedItem != null)
        {
            //Image_TipoDespesa.Source = selectedItem.Imagem;
            Label_TipoServico.Text = selectedItem.Descricao;
            _servicoSelecionado = selectedItem;
            PopupTipoServico.IsVisible = false;
            ((CollectionView)sender).SelectedItem = null;
        }
    }

    private async void TapGestureRecognizerTipoServico_Tapped(object sender, TappedEventArgs e)
    {
        if (_menuSelecionado == EMenuSelecionado.Lembrete)
        {
            PopupTipoServico.IsVisible = true;
        }
        else
        {
            CarregarTiposServicos();

            var popup = new PopUp_Servicos(_tiposServico);

            // Exibe o popup e aguarda o veículo selecionado
            var veiculoSelecionado = Navigation.PushAsync(popup);

            var resultado = await popup.ObterServicoSelecionadoAsync();


            // Exibe o veículo selecionado
            if (resultado != null)
            {
                if (resultado.Count > 0)
                {
                    ListView_ServicosSelecionados.ItemsSource = resultado;
                    ListView_ServicosSelecionados.IsVisible = true;
                    double ValorTotal = 0;
                    foreach (var item in resultado)
                    {
                        ValorTotal += item.Valor;
                    }
                    Label_ValorTotalServicos.Text = ValorTotal.ToString("N2");
                    Stack_ValoresTotaisServicos.IsVisible = true;
                }
                else
                {
                    ListView_ServicosSelecionados.ItemsSource = null;
                    ListView_ServicosSelecionados.IsVisible = false;
                    Label_ValorTotalServicos.IsVisible = false;
                }
            }
        }
    }

    private void CarregarTiposServicos()
    {
        if (_tiposServico == null)
        {
            _tiposServico = new List<Db.Models.TipoServico>();
            foreach (var item in DbTipoServico._tipoServicos.ToList())
            {
                Db.Models.TipoServico objAdd = new Db.Models.TipoServico()
                {
                    Id = item.Id,
                    Valor = item.Valor,
                    Descricao = item.Descricao,
                    EnviadoServidor = item.EnviadoServidor,
                    IdServico = item.IdServico,
                    IsSelected = item.IsSelected
                };
                _tiposServico.Add(objAdd);
            }
        }
    }

    private void CarregarTiposDespesa()
    {
        if (_tiposDespesa == null)
        {
            _tiposDespesa = new List<Despesa>();
            foreach (var item in DbDespesa._listaDespesa.ToList())
            {
                Despesa objAdd = new Despesa()
                {
                    Id = item.Id,
                    Valor = item.Valor,
                    Descricao = item.Descricao,
                    EnviadoServidor = item.EnviadoServidor,
                    IdServico = item.IdServico,
                    IsSelected = item.IsSelected
                };
                _tiposDespesa.Add(objAdd);
            }
        }
    }

    private void OnSelectionChangedTipoReceita(object sender, SelectionChangedEventArgs e)
    {
        Db.Models.TipoReceita selectedItem = e.CurrentSelection.FirstOrDefault() as Db.Models.TipoReceita;

        if (selectedItem != null)
        {
            //Image_TipoDespesa.Source = selectedItem.Imagem;
            Label_TipoReceita.Text = selectedItem.Descricao;
            PopupTipoReceita.IsVisible = false;
            _receitaSelecionada = selectedItem;
            ((CollectionView)sender).SelectedItem = null;
        }
    }

    private void TapGestureRecognizerTipoReceita_Tapped(object sender, TappedEventArgs e)
    {
        PopupTipoReceita.IsVisible = true;
    }

    private void Button_Servico_Clicked(object sender, EventArgs e)
    {
        Button_Despesa.BackgroundColor = Color.FromHex("#ffffff");
        Button_Despesa.TextColor = Color.FromHex("#2c3e50");
        Button_Servico.BackgroundColor = Color.FromHex("#d35400");
        Button_Servico.TextColor = Color.FromHex("#ffffff");
        Grid_Despesa.IsVisible = false;
        Grid_Servico.IsVisible = true;
    }

    private void Button_Despesa_Clicked(object sender, EventArgs e)
    {
        Button_Despesa.BackgroundColor = Color.FromHex("#d35400");
        Button_Despesa.TextColor = Color.FromHex("#ffffff");
        Button_Servico.BackgroundColor = Color.FromHex("#ffffff");
        Button_Servico.TextColor = Color.FromHex("#2c3e50");
        Grid_Despesa.IsVisible = true;
        Grid_Servico.IsVisible = false;
    }

    private void Button_ApenasUmaVez_Clicked(object sender, EventArgs e)
    {
        _ApenasUmaVez = true;
        Button_RepetirACada.BackgroundColor = Color.FromHex("#ffffff");
        Button_RepetirACada.TextColor = Color.FromHex("#2c3e50");
        Button_ApenasUmaVez.BackgroundColor = Color.FromHex("#d35400");
        Button_ApenasUmaVez.TextColor = Color.FromHex("#ffffff");
    }

    private void Button_RepetirACada_Clicked(object sender, EventArgs e)
    {
        _ApenasUmaVez = false;
        Button_RepetirACada.BackgroundColor = Color.FromHex("#d35400");
        Button_RepetirACada.TextColor = Color.FromHex("#ffffff");
        Button_ApenasUmaVez.BackgroundColor = Color.FromHex("#ffffff");
        Button_ApenasUmaVez.TextColor = Color.FromHex("#2c3e50");
    }

    private void CheckBox_Kilometragem_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (CheckBox_Kilometragem != null && CheckBox_Data != null)
        {
            if (!CheckBox_Kilometragem.IsChecked && !CheckBox_Data.IsChecked)
            {
                CheckBox_Data.IsChecked = true;
            }
        }
    }

    private void CheckBox_Data_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (CheckBox_Kilometragem != null && CheckBox_Data != null)
        {
            if (!CheckBox_Kilometragem.IsChecked && !CheckBox_Data.IsChecked)
            {
                CheckBox_Kilometragem.IsChecked = true;
            }
        }
    }

    private async void Button_CalculadoraFlex_Clicked(object sender, EventArgs e)
    {
        var selecionarPage = new CalculadoraFlex(true);

        var result = Navigation.PushAsync(selecionarPage);
        // Aguardamos o retorno de uma variável selecionada
        var resultado = await selecionarPage.AguardarSelecaoAsync();

        if (resultado != null)
        {
            // Use o valor retornado
            entryPreco.Text = resultado.Preco.ToString("N2");
            listas.TiposCombustivel tiposCombustivel = new listas.TiposCombustivel();
            _combustivelSelecionado = tiposCombustivel._tipoCombustivel.Find(x => x.Id == resultado.TipoCombustivel);
            Label_Combustivel.Text = _combustivelSelecionado.Descricao;
            Image_TipoCombustivel.Source = _combustivelSelecionado.Imagem;
        }
    }

    private void Editor_Observacao_TextChanged(object sender, TextChangedEventArgs e)
    {
        ScrollView_TelaPrincipal.ScrollToAsync(0, ScrollView_TelaPrincipal.ContentSize.Height, true);
    }
}