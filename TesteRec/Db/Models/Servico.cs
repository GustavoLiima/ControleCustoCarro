﻿using SQLite;
using System.ComponentModel.DataAnnotations.Schema;
using TesteRec.Enum;
using TesteRec.Model;

namespace TesteRec.Db.Models
{
    public class Servico
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int AcaoSelecionada { get; set; }
        public DateTime Data { get; set; }
        public TimeSpan Hora { get; set; }
        public double Odometro { get; set; }
        public int Combustivel { get; set; }
        public double Preco { get; set; }
        public double ValorTotal { get; set; }
        public double Litros { get; set; }
        public string? Motorista { get; set; }
        public int FormaPagamento { get; set; }
        public string Descricao { get; set; }
        public int TipoDespesa { get; set; }
        public int TipoServico { get; set; }
        [NotMapped]
        public string DescricaoReceita
        {
            get
            {
                switch ((EMenuSelecionado)AcaoSelecionada)
                {
                    case EMenuSelecionado.Receita:
                        return DbTipoReceita._tipoReceitas.Find(x => x.Id == Receita).Descricao;
                    case EMenuSelecionado.Despesa:
                        return DbDespesa._listaDespesa.Find(x => x.Id == TipoDespesa).Descricao;
                    case EMenuSelecionado.Abastecimento:
                        return new Layouts.listas.TiposCombustivel()._tipoCombustivel.Find(x => x.Id == Combustivel).Descricao;
                    default:
                        return null;
                }
            }
        }
        [NotMapped]
        public TipoReceita ReceitaModelo
        {
            get
            {
                return DbTipoReceita._tipoReceitas.Find(x => x.Id == Receita);
            }
        }
        [NotMapped]
        public Despesa DespesaModelo
        {
            get
            {
                return DbDespesa._listaDespesa.Find(x => x.Id == TipoDespesa);
            }
        }
        [NotMapped]
        public Pagamentos FormaPagamentoModelo
        {
            get
            {
                return new Layouts.listas.FormasPagamento()._tiposPagamentos.Find(x => x.Id == FormaPagamento);
            }
        }
        public int Receita { get; set; }
        public double ValorReceita { get; set; }

        public double ValorDespesa { get; set; }

        //Lembrete
        public bool LembreteFoiServico { get; set; }
        public bool ApenasUmaVez { get; set; }
        public bool LembrarEmKm { get; set; }
        public bool LembrarEmData { get; set; }
        public double LembreteKilometragem { get; set; }
        public DateTime DataLembrete { get; set; }
    }
}
