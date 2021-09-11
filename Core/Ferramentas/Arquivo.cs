using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Core.Ferramentas
{
    public class Arquivo
    {
        public String Nome { get; set; }
        public Byte[] Conteudo { get; set; }
        public Object Anexo { get; set; }

        public Arquivo() { }

        public override string ToString()
        {
            if (this.Conteudo == null || this.Conteudo.Count() <= 0)
                return String.Empty;

            var objISOencoding = System.Text.Encoding.GetEncoding("iso-8859-1");
            return objISOencoding.GetString(this.Conteudo);
        }

        public static byte[] TratarString(byte[] conteudo)
        {
            var strConteudo = Encoding.GetEncoding("iso-8859-1").GetString(conteudo);
            strConteudo = ReplaceCaracters(strConteudo);
            strConteudo = strConteudo.Replace("(", " ").Replace(")", " ").Replace("{", " ").Replace("}", " ");
            strConteudo = Regex.Replace(strConteudo, "[^0-9a-zA-Z\\.\\,\\/\\x20\\/\\x1F\\-\\r\\n]+", " ");

            return Encoding.GetEncoding("iso-8859-1").GetBytes(strConteudo);
        }

        public static byte[] TratarStringComPontoVirgula(byte[] conteudo)
        {
            var strConteudo = Encoding.GetEncoding("iso-8859-1").GetString(conteudo);
            strConteudo = strConteudo.Replace("(", " ").Replace(")", " ").Replace("{", " ").Replace("}", " ").Replace("\"", "");
            strConteudo = Regex.Replace(strConteudo, "[^0-9a-záàâãéèêíïóôõöúçA-ZÁÀÂÃÉÈÍÏÓÔÕÖÚÇ\\.\\;\\,\\/\\x20\\/\\x1F\\-\\r\\n]+", " ");

            return Encoding.GetEncoding("iso-8859-1").GetBytes(strConteudo);
        }

        public static byte[] TratarStringISO88591(byte[] conteudo)
        {
            var strConteudo = Encoding.GetEncoding("iso-8859-1").GetString(conteudo);
            MatchCollection caracterEspecial = Regex.Matches(strConteudo, @"[!#$%^&*]");

            foreach (Match m in caracterEspecial)
            {
                strConteudo = strConteudo.Replace(m.Value, " ");
            }

            return Encoding.GetEncoding("iso-8859-1").GetBytes(strConteudo);
        }

        public static string ReplaceCaracters(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] > 255)
                    sb.Append(text[i]);
                else
                    sb.Append(tratarString[text[i]]);
            }

            return sb.ToString();
        }

        private static readonly char[] tratarString = TratarCaracters();

        private static char[] TratarCaracters()
        {
            char[] accents = new char[256];

            for (int i = 0; i < 256; i++)
                accents[i] = (char)i;

            accents[(byte)'á'] = accents[(byte)'à'] = accents[(byte)'ã'] = accents[(byte)'â'] = accents[(byte)'ä'] = 'a';
            accents[(byte)'Á'] = accents[(byte)'À'] = accents[(byte)'Ã'] = accents[(byte)'Â'] = accents[(byte)'Ä'] = 'A';

            accents[(byte)'é'] = accents[(byte)'è'] = accents[(byte)'ê'] = accents[(byte)'ë'] = 'e';
            accents[(byte)'É'] = accents[(byte)'È'] = accents[(byte)'Ê'] = accents[(byte)'Ë'] = 'E';

            accents[(byte)'í'] = accents[(byte)'ì'] = accents[(byte)'î'] = accents[(byte)'ï'] = 'i';
            accents[(byte)'Í'] = accents[(byte)'Ì'] = accents[(byte)'Î'] = accents[(byte)'Ï'] = 'I';

            accents[(byte)'ó'] = accents[(byte)'ò'] = accents[(byte)'ô'] = accents[(byte)'õ'] = accents[(byte)'ö'] = 'o';
            accents[(byte)'Ó'] = accents[(byte)'Ò'] = accents[(byte)'Ô'] = accents[(byte)'Õ'] = accents[(byte)'Ö'] = 'O';

            accents[(byte)'ú'] = accents[(byte)'ù'] = accents[(byte)'û'] = accents[(byte)'ü'] = 'u';
            accents[(byte)'Ú'] = accents[(byte)'Ù'] = accents[(byte)'Û'] = accents[(byte)'Ü'] = 'U';

            accents[(byte)'ç'] = 'c';
            accents[(byte)'Ç'] = 'C';

            accents[(byte)'ñ'] = 'n';
            accents[(byte)'Ñ'] = 'N';

            accents[(byte)'ÿ'] = accents[(byte)'ý'] = 'y';
            accents[(byte)'Ý'] = 'Y';

            return accents;
        }

    }
}
