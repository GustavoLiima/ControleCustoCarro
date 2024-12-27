using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cofauto.Helpers.Componentes
{
    public class MascaraValor : Behavior<Entry>
    {
        private bool _isUpdating;

        protected override void OnAttachedTo(Entry bindable)
        {
            base.OnAttachedTo(bindable);
            bindable.TextChanged += OnTextChanged;
            bindable.Unfocused += OnUnfocused;
            bindable.Keyboard = Keyboard.Numeric;
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.TextChanged -= OnTextChanged;
            bindable.Unfocused -= OnUnfocused;
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (_isUpdating)
                return;

            if (sender is Entry entry)
            {
                _isUpdating = true;

                try
                {
                    // Verifica se o texto excede o MaxLength
                    if (entry.MaxLength > 0 && e.NewTextValue.Length > entry.MaxLength)
                    {
                        // Reverte para o texto antigo para evitar excesso
                        entry.Text = e.OldTextValue;
                        return;
                    }

                    // Remove caracteres não numéricos
                    string numericText = new string(e.NewTextValue.Where(char.IsDigit).ToArray());

                    if (numericText.Length > 0)
                    {
                        // Converte para decimal e formata como moeda
                        decimal value = decimal.Parse(numericText) / 100;
                        entry.Text = value.ToString("N2", CultureInfo.CurrentCulture);

                        // Move o cursor para o final do texto
                        entry.CursorPosition = entry.Text.Length;
                    }
                    else
                    {
                        entry.Text = "0,00"; // Valor padrão para texto vazio
                    }
                }
                catch
                {
                    entry.Text = "0,00"; // Valor padrão em caso de erro
                }
                finally
                {
                    _isUpdating = false;
                }
            }
        }

        private void OnUnfocused(object sender, FocusEventArgs e)
        {
            if (sender is Entry entry && string.IsNullOrWhiteSpace(entry.Text))
            {
                entry.Text = "0,00"; // Define um valor padrão ao desfocar
            }
        }
    }
}
