using System.Windows;
using System.Windows.Controls;
using ClienteMarWPFWin7.UI.ViewModels.ModelObservable;

namespace ClienteMarWPFWin7.UI.TemplateSelector
{
    public class MovimientoDataTemplateSelector : DataTemplateSelector
    {

        public DataTemplate CuadreTemplate { get; set; }
        public DataTemplate NoCuadreTemplate { get; set; } 

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            return ((MovimientoObservable)item).IsCuadre ? CuadreTemplate : NoCuadreTemplate;
        }
    }
}
