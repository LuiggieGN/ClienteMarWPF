using System.Text.RegularExpressions;

namespace ClienteMarWPF.UI.ViewModels.Helpers
{
    public static class InputHelper
    {
        public static bool InputIsBlank(string input) 
        {
           if(input == null || input == string.Empty) { return true; }

            var nuevo = Regex.Replace(input, @"\s+", "").Trim();

            return string.IsNullOrEmpty(nuevo);
        }

    }
}
