using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ClienteMarWPF.UI.Views.AttachedProperties
{
    public class SoloNumerosBehaviour
    {

        public static readonly DependencyProperty HabilitadoProperty =
                DependencyProperty.RegisterAttached("Habilitado", typeof(bool),
                typeof(SoloNumerosBehaviour), new UIPropertyMetadata(false, OnValueChanged));

        public static bool GetHabilitado(Control o) { return (bool)o.GetValue(HabilitadoProperty); }
        public static void SetHabilitado(Control o, bool value) { o.SetValue(HabilitadoProperty, value); }

        private static void OnValueChanged(
              DependencyObject dependencyObject,
              DependencyPropertyChangedEventArgs e)
        {
            var uiElement = dependencyObject as Control;
            if (uiElement == null) return;
            if (e.NewValue is bool && (bool)e.NewValue)
            {
                uiElement.PreviewTextInput += OnTextInput;
                uiElement.PreviewKeyDown += OnPreviewKeyDown;
                DataObject.AddPastingHandler(uiElement, OnPaste);
            }

            else
            {
                uiElement.PreviewTextInput -= OnTextInput;
                uiElement.PreviewKeyDown -= OnPreviewKeyDown;
                DataObject.RemovePastingHandler(uiElement, OnPaste);
            }
        }

        private static void OnTextInput(object sender, TextCompositionEventArgs e)
        {
            if (e.Text.Any(c => !char.IsDigit(c))) { e.Handled = true; }
        }

        private static void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space) e.Handled = true;
        }

        private static void OnPaste(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(DataFormats.Text))
            {
                var text = Convert.ToString(e.DataObject.GetData(DataFormats.Text)).Trim();
                if (text.Any(c => !char.IsDigit(c))) { e.CancelCommand(); }
            }
            else
            {
                e.CancelCommand();
            }
        }






    }// fin de clase
}
