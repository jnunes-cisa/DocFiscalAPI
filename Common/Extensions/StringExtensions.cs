using System.Text;

namespace Common.Extensions
{

    public static class StringExtensions
    {
        public static string ToProperCase(this string str)
        {
            var chars = str.ToCharArray();
            if (chars.Length > 0)
            {
                chars[0] = char.ToUpper(chars[0]);
                var builder = new StringBuilder();
                for (var i = 0; i < chars.Length; i++)
                {
                    if ((chars[i]) == '-')
                    {
                        i = i + 1;
                        builder.Append(char.ToUpper(chars[i]));
                    }
                    else
                    {
                        builder.Append(chars[i]);
                    }
                }
                return builder.ToString();
            }
            return str;
        }

        public static string RemoveAcentosCaracteresEspeciais(string str)
        {

            string[] acentos = new string[] { "�", "�", "�", "�", "�", "�", "�", "�", "�", "�", "�", "�", "�", "�", "�", "�", "�", "�", "�", "�", "�", "�", "�", "�", "�", "�", "�", "�", "�", "�", "�", "�", "�", "�", "�", "�", "�", "�", "�", "�", "�", "�", "�", "�", "�", "�", "�", "�", "�", "�", "�" };
            string[] semAcento = new string[] { "c", "C", "a", "e", "i", "o", "u", "y", "A", "E", "I", "O", "U", "Y", "a", "e", "i", "o", "u", "A", "E", "I", "O", "U", "a", "o", "n", "a", "e", "i", "o", "u", "y", "A", "E", "I", "O", "U", "A", "O", "N", "a", "e", "i", "o", "u", "A", "E", "I", "O", "U" };

            for (int i = 0; i < acentos.Length; i++)
            {
                str = str.Replace(acentos[i], semAcento[i]);
            }

            string[] caracteresEspeciais = { "\\.", ",", "-", ":", "\\(", "\\)", "�", "\\|", "\\\\", "�" };

            for (int i = 0; i < caracteresEspeciais.Length; i++)
            {
                str = str.Replace(caracteresEspeciais[i], "");
            }

            return str;
        }

    }

}