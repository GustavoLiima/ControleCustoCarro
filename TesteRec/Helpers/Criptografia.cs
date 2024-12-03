using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TesteRec.Helpers
{
    public class Criptografia
    {
        private static readonly string chave = "jKldAKJV58G684ASD2d"; // Uma chave fixa para a criptografia

        // Método para criptografar uma string
        public string Criptografar(string texto)
        {
            if (string.IsNullOrEmpty(texto))
            {
                throw new ArgumentNullException(nameof(texto), "O texto para criptografia não pode ser nulo ou vazio.");
            }

            var resultado = new StringBuilder();

            for (int i = 0; i < texto.Length; i++)
            {
                char caractere = texto[i];
                char caractereChave = chave[i % chave.Length];
                char criptografado = (char)(caractere + caractereChave);
                resultado.Append(criptografado);
            }

            return Convert.ToBase64String(Encoding.UTF8.GetBytes(resultado.ToString()));
        }

        // Método para descriptografar uma string
        public string Descriptografar(string textoCriptografado)
        {
            if (string.IsNullOrEmpty(textoCriptografado))
            {
                throw new ArgumentNullException(nameof(textoCriptografado), "O texto para descriptografia não pode ser nulo ou vazio.");
            }

            string textoDecodificado = Encoding.UTF8.GetString(Convert.FromBase64String(textoCriptografado));
            var resultado = new StringBuilder();

            for (int i = 0; i < textoDecodificado.Length; i++)
            {
                char caractere = textoDecodificado[i];
                char caractereChave = chave[i % chave.Length];
                char descriptografado = (char)(caractere - caractereChave);
                resultado.Append(descriptografado);
            }

            return resultado.ToString();
        }
    }
}
