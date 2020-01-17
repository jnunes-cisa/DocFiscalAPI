using System.ComponentModel;

namespace Common.Enums
{
    public class Enums
    {
        public enum EnumAmbiente
        {
            [Description("Desenvolvimento")]
            Desenvolvimento,
            [Description("Homologação")]
            Homologacao,
            [Description("Produção")]
            Producao
        }
    }
}
