using System.Globalization;

namespace HaarpTech_Licenta.Utils
{
    internal static class GlobalizationUtils
    {
        public static void ConfigureGlobalizationUtils()
        {
            var cultureInfo = new CultureInfo("ro-RO");
            cultureInfo.NumberFormat.CurrencySymbol = "RON";
            cultureInfo.NumberFormat.CurrencyDecimalDigits = 2;

            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
        }
    }
}
