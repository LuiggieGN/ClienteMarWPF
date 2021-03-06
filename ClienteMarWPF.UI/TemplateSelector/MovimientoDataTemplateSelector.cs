﻿using System.Windows;
using System.Windows.Controls;
using ClienteMarWPF.UI.ViewModels.ModelObservable;

namespace ClienteMarWPF.UI.TemplateSelector
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
