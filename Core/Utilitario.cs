using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace Core
{
    public static class Utilitario
    {
        const string SPCHAR = "\"!@#$%¨&*()_+-=¹²³£¢¬[]{}´`~^,.;/<>:?ªº°§ ";

        public static Double ObterNumero(string strValor)
        {
            double dblConvertido = 0;

            strValor = strValor.Trim();

            if (string.IsNullOrEmpty(strValor))
                return 0;

            if (!double.TryParse(strValor, out dblConvertido))
                throw new Exception("Valor não é um numérico válido: [" + strValor + "]");

            return dblConvertido;
        }

        public static Decimal ObterDecimal(string strValor, int intDecimais)
        {
            decimal decConvertido = 0;
            string strConvertido = strValor;

            string strNumero = string.Empty;
            string strDecimal = string.Empty;


            if (string.IsNullOrEmpty(strValor)) return 0;
            if (strValor.Length < intDecimais) return 0;

            strNumero = strValor.Substring(0, strValor.Length - intDecimais);
            strDecimal = strValor.Substring(strValor.Length - intDecimais, intDecimais);

            strConvertido = strNumero + "," + strDecimal;

            if (!decimal.TryParse(strConvertido, out decConvertido))
                throw new Exception("Valor não é um decimal válido: [" + strValor + "]");

            return decConvertido;
        }

        public static DateTime ObterDataHora(string strValor, string strFormato)
        {
            int intDia = 0, intMes = 0, intAno = 0, intHora = 0, intMinuto = 0, intSegundo = 0;
            string strDia = "0", strMes = "0", strAno = "0", strHora = "0", strMinuto = "0", strSegundo = "0";

            if (strValor.Trim().Count() != strFormato.Trim().Count())
                throw new Exception("Data não está no formato correto: [" + strValor + "]");

            if (strFormato.IndexOf("DD") >= 0)
                strDia = strValor.Substring(strFormato.IndexOf("DD"), 2);

            if (strFormato.IndexOf("MM") >= 0)
                strMes = strValor.Substring(strFormato.IndexOf("MM"), 2);

            if (strFormato.IndexOf("AA") >= 0)
            {
                if (strFormato.LastIndexOf("AA") > strFormato.IndexOf("AA"))
                    strAno = strValor.Substring(strFormato.IndexOf("AA"), 4);
                else if (strFormato.StartsWith("AA"))
                    strAno = strValor.Substring(strFormato.IndexOf("AA"), 2);
                else
                    strAno = strValor.Substring(strFormato.IndexOf("AA", 2));
            }

            if (strFormato.IndexOf("HH") >= 0)
                strHora = strValor.Substring(strFormato.IndexOf("HH"), 2);

            if (strFormato.IndexOf("II") >= 0)
                strMinuto = strValor.Substring(strFormato.IndexOf("II"), 2);

            if (strFormato.IndexOf("SS") >= 0)
                strSegundo = strValor.Substring(strFormato.IndexOf("SS"), 2);

            if (!int.TryParse(strDia, out intDia))
                throw new Exception("Dia da data é inválido (DD): [" + strValor + "]");

            if (!int.TryParse(strMes, out intMes))
                throw new Exception("Mês da data é inválido (MM): [" + strValor + "]");

            if (!int.TryParse(strAno, out intAno))
                throw new Exception("Ano da data é inválido (AA ou AAAA): [" + strValor + "]");

            if (!int.TryParse(strHora, out intHora))
                throw new Exception("Hora da data é inválida (HH): [" + strValor + "]");

            if (!int.TryParse(strMinuto, out intMinuto))
                throw new Exception("Minuto da data é inválido (II): [" + strValor + "]");

            if (!int.TryParse(strSegundo, out intSegundo))
                throw new Exception("Segundo da data é inválido (SS): [" + strValor + "]");

            //Caso 2 dígitos, somar 2000
            if (strAno.Length <= 2)
                intAno += 2000;

            try
            {
                return new DateTime(intAno, intMes, intDia, intHora, intMinuto, intSegundo);
            }
            catch
            {
                throw new Exception("Data informada está em formato incorreto: [" + strValor + "]");
            }
        }


        public static string DeNumero(double dblValor, int intTamanho, bool booPreencherComZeros)
        {
            string strValor = string.Empty;

            if (dblValor.ToString().Length > intTamanho)
                throw new Exception("Valor é maior que o tamanho solicitado [" + dblValor.ToString() + "].");

            strValor = strValor.PadLeft(100, (booPreencherComZeros ? '0' : ' ')) + dblValor.ToString();
            strValor = strValor.Substring(strValor.Length - intTamanho, intTamanho);

            return strValor;
        }

        public static string DeNumero(string strValor, int intTamanho, bool booPreencherComZeros)
        {
            strValor = strValor.PadLeft(30, (booPreencherComZeros ? '0' : ' '));
            strValor = strValor.Substring(strValor.Length - intTamanho, intTamanho);

            return strValor;
        }

        public static string DeDecimal(decimal decValor, int intTamanho, int intDecimais, bool booPreencherComZeros)
        {
            string strValor = string.Empty;
            string strNumero = string.Empty;
            string strDecimal = string.Empty;

            if (decValor.ToString().Split(',').Length == 2)
            {
                strNumero = decValor.ToString().Split(',')[0];
                strDecimal = decValor.ToString().Split(',')[1];

                strDecimal = strDecimal.PadRight(30, '0');
                strDecimal = strDecimal.Substring(0, intDecimais);
                strValor = strNumero + strDecimal;
            }
            else if (decValor.ToString().Split('.').Length == 2)
            {
                strNumero = decValor.ToString().Split('.')[0];
                strDecimal = decValor.ToString().Split('.')[1];

                strDecimal = strDecimal.PadRight(30, '0');
                strDecimal = strDecimal.Substring(0, intDecimais);
                strValor = strNumero + strDecimal;
            }
            else
                strValor = decValor.ToString();

            strValor = strValor.Replace(".", "");
            strValor = strValor.Replace(",", "");

            strValor = strValor.PadLeft(30, (booPreencherComZeros ? '0' : ' '));
            strValor = strValor.Substring(strValor.Length - intTamanho, intTamanho);

            return strValor;
        }

        public static string DeTexto(string strValor, int intTamanho)
        {
            strValor = strValor == null ? string.Empty : strValor;
            return strValor.PadRight(320 + (intTamanho * 2), ' ').Substring(0, intTamanho);
        }

        public static string DeCEP(string strValor, int intTamanho)
        {
            strValor = strValor == null ? string.Empty : strValor;
            strValor = strValor.Trim().Replace("-", string.Empty);
            strValor = "00000000" + strValor;
            strValor = strValor.Substring(strValor.Length - 8, 8);
            strValor = strValor.Substring(0, 5) + "-" + strValor.Substring(5, 3);

            return strValor.PadRight(320, ' ').Substring(0, intTamanho);
        }

        public static string DeDataHora(DateTime datValor, string strFormato)
        {
            string strValor = string.Empty;
            if (datValor <= DateTime.MinValue)
                return string.Empty.PadLeft(strFormato.Length, ' ');
            return datValor.ToString(strFormato);
        }

        public static string SomenteTexto(string strValor, int intTamanho)
        {
            strValor = strValor == null ? string.Empty : strValor;
            foreach (char chrItem in SPCHAR.ToCharArray())
                strValor = strValor.Replace(chrItem.ToString(), string.Empty);
            return strValor.PadRight(320, ' ').Substring(0, intTamanho);
        }


        public static string RecuperarCampo(string strLinha, int intPosicao, int intTamanho)
        {
            return RecuperarCampo(strLinha, intPosicao, intTamanho, false, 0, string.Empty);
        }

        public static string RecuperarCampo(string strLinha, int intPosicao, int intTamanho, bool booErroSeVazio, int intLinha, string strTextoSeVazio)
        {
            if ((intPosicao + intTamanho) > strLinha.Length)
                throw new Exception("Linha " + intLinha.ToString() + ": Offset solicitado supera o tamanho ta linha.");

            string strValor = strLinha.Substring(intPosicao, intTamanho).Trim();

            if (string.IsNullOrEmpty(strValor) && booErroSeVazio)
                throw new Exception(string.Format("Linha {0}: {1} é obrigatório e está inválido na posição {2}->{3} [{4}].",
                    intLinha.ToString(),
                    strTextoSeVazio,
                    intPosicao.ToString(),
                    (intPosicao + intTamanho).ToString(),
                    intTamanho.ToString())
                );

            return strValor;
        }

        public static string RecuperarCampo(string strLinha, int intPosicao, int intTamanho, bool booErroSeVazio, int intLinha, string strTextoSeVazio, string strValorSeVazio)
        {
            string strValor = RecuperarCampo(strLinha, intPosicao, intTamanho, booErroSeVazio, intLinha, strTextoSeVazio);

            if (string.IsNullOrEmpty(strValor))
                return strValorSeVazio;
            return strValor;
        }

        public static string RecuperarNumeros(string strValor)
        {
            if (String.IsNullOrEmpty(strValor))
                return string.Empty;

            var objRegex = new Regex("\\d+");
            var strNumero = string.Empty;

            foreach (Match objMatch in objRegex.Matches(strValor))
            {
                if (!objMatch.Success)
                    continue;
                strNumero += objMatch.Value;
            }

            return strNumero;
        }

        public static string Filler(int intTamanho)
        {
            string strValor = string.Empty;
            strValor = strValor.PadLeft(intTamanho, ' ');
            return strValor;
        }

        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }

            foreach (T item in items)
            {
                var values = new object[props.Length];
                for (int i = 0; i < props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = props[i].GetValue(item, null);
                }

                dataTable.Rows.Add(values);
            }

            //put a breakpoint here and check datatable
            return dataTable;
        }

        public static DataTable ToDataTable<T>(List<T> items, List<string> listaNomesColunas)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var nomes in listaNomesColunas)
            {
                dataTable.Columns.Add(nomes);
            }

            foreach (T item in items)
            {
                var values = new object[props.Length];
                for (int i = 0; i < props.Length; i++)
                {
                    values[i] = props[i].GetValue(item, null);
                }

                dataTable.Rows.Add(values);
            }

            return dataTable;
        }

        private static bool IsIndex(List<int> indexIgnore, int indexAtual)
        {
            bool isIndex = false;
            indexIgnore.ForEach(i =>
            {
                if (i == indexAtual)
                    isIndex = true;
            });
            return isIndex;
        }

        public static string TratarString(string conteudo)
        {
            conteudo =
                conteudo.Replace("(", " ").Replace(")", " ").Replace("{", " ").Replace("}", " ");
            conteudo = Regex.Replace(conteudo, "[^0-9a-zA-Z\\.\\,\\/\\x20\\-\\r\\n]+", " ");

            return conteudo;
        }

        public static decimal ConverterDecimalCultura(this string valor)
        {
            var cultura = Thread.CurrentThread.CurrentCulture;
            var separador = cultura.NumberFormat.CurrencyDecimalSeparator;

            valor = valor ?? "0";
            valor = valor.Replace(",", separador);
            valor = valor.Replace(".", separador);

            return Convert.ToDecimal(valor);
        }

        public static byte[] ReadFully(Stream input)
        {
            var buffer = new byte[16 * 1024];
            using (var ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        public static string RemoverAcentos(this string text)
        {
            StringBuilder sbReturn = new StringBuilder();
            var arrayText = text.Normalize(NormalizationForm.FormD).ToCharArray();
            foreach (char letter in arrayText)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(letter) != UnicodeCategory.NonSpacingMark)
                    sbReturn.Append(letter);
            }
            return sbReturn.ToString();
        }

        public static string RemoverMascaraTelefone(this string telefone)
        {
            return telefone.Replace("-", string.Empty).Replace("(", string.Empty).Replace(")", string.Empty).Replace(" ", string.Empty).Trim();
        }

        public static string TratarEspecial(string conteudo)
        {
            conteudo =
                conteudo.Replace("(", " ").Replace(")", " ").Replace("{", " ").Replace("}", " ");
            conteudo = Regex.Replace(conteudo, "[!@#$%^&*.,]", " ");

            return conteudo;
        }

        public static byte[] Criptografar(byte[] entrada, string chave)
        {

            object controleLock = new object();
            var bytesChave = CalculaHash(chave);
            byte[] IV = new byte[8] { 1, 2, 3, 4, 5, 6, 7, 8 };
            lock (controleLock)
            {
                var cryptoProvider = new TripleDESCryptoServiceProvider();

                cryptoProvider.Mode = CipherMode.ECB;
                cryptoProvider.Key = bytesChave;

                var transformador = cryptoProvider.CreateEncryptor(bytesChave, IV);

                return transformador.TransformFinalBlock(entrada, 0, entrada.Length);
            }
        }

        public static byte[] Descriptografar(byte[] entrada, string chave)
        {
            object controleLock = new object();
            var bytesChave = CalculaHash(chave);
            byte[] IV = new byte[8] { 1, 2, 3, 4, 5, 6, 7, 8 };
            lock (controleLock)
            {
                var cryptoProvider = new TripleDESCryptoServiceProvider
                {
                    Mode = CipherMode.ECB,
                    Key = bytesChave
                };

                var transformador = cryptoProvider.CreateDecryptor(bytesChave, IV);

                return transformador.TransformFinalBlock(entrada, 0, entrada.Length);
            }
        }

        public static byte[] CalculaHash(string texto)
        {
            return CalculaHash(Encoding.ASCII.GetBytes(texto));
        }

        public static byte[] CalculaHash(byte[] bytes)
        {
            MD5CryptoServiceProvider md5Provider = new MD5CryptoServiceProvider();
            return md5Provider.ComputeHash(bytes);
        }

        public static string DeNumeroIntelbras(string strValor, int intTamanho, bool booPreencherComZeros)
        {
            strValor = strValor.PadLeft(30, (booPreencherComZeros ? '0' : ' '));
            strValor = strValor.Substring(strValor.Length - intTamanho, intTamanho);
            strValor = strValor.Remove(0, 1);
            strValor = strValor.Insert(0, " ");

            return strValor;
        }
    }

    static class DateTimeExtensions
    {
        static GregorianCalendar _gc = new GregorianCalendar();
        public static int GetWeekOfMonth(this DateTime time)
        {
            DateTime first = new DateTime(time.Year, time.Month, 1);
            return time.GetWeekOfYear() - first.GetWeekOfYear() + 1;
        }

        static int GetWeekOfYear(this DateTime time)
        {
            return _gc.GetWeekOfYear(time, CalendarWeekRule.FirstDay, DayOfWeek.Sunday);
        }
    }

    public static class UtilSql
    {
        public static string SqlString(string field)
        {
            return field != null ? $"'{field}'" : "NULL";
        }

        public static string SqlBitConverter(byte[] field)
        {
            return field != null ? $"0x{BitConverter.ToString(field).Replace("-", "")}" : "NULL";
        }

        public static string SqlDateTime(DateTime? field)
        {
            return field != null ? $"'{field:yyyy-MM-dd HH:mm:ss.fff}'" : "NULL";
        }

        public static int SqlBool(bool? field)
        {
            return field == true ? 1 : 0;
        }

        public static string SqlDecimal(decimal? field)
        {
            string Decimal = field.ToString();
            var Decimalreplace = String.Empty;

            if (string.IsNullOrEmpty(Decimal) != true)
            {
                Decimalreplace = Decimal.Replace(",", ".");
            }

            return string.IsNullOrEmpty(Decimal) == true ? "NULL" : $"{Decimalreplace}";
        }

    }
}
