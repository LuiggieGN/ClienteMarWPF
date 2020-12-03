﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace ClienteMarWPF.UI.Views.AttachedProperties
{
    public class SeleccionaTodoBehaviour
    {

        public static bool GetHabilitado(FrameworkElement frameworkElement)
        {
            return (bool)frameworkElement.GetValue(HabilitadoProperty);
        }

        public static void SetHabilitado(FrameworkElement frameworkElement, bool value)
        {
            frameworkElement.SetValue(HabilitadoProperty, value);
        }

        public static readonly DependencyProperty HabilitadoProperty =
                 DependencyProperty.RegisterAttached("Habilitado",
                    typeof(bool), typeof(SeleccionaTodoBehaviour),
                    new FrameworkPropertyMetadata(false, OnEnableChanged));

        private static void OnEnableChanged
                   (DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var frameworkElement = d as FrameworkElement;
            if (frameworkElement == null) return;

            if (e.NewValue is bool == false) return;

            if ((bool)e.NewValue)
            {
                frameworkElement.GotFocus += SelectAll;
                frameworkElement.PreviewMouseDown += IgnoreMouseButton;
            }
            else
            {
                frameworkElement.GotFocus -= SelectAll;
                frameworkElement.PreviewMouseDown -= IgnoreMouseButton;
            }
        }

        private static void SelectAll(object sender, RoutedEventArgs e)
        {
            var frameworkElement = e.OriginalSource as FrameworkElement;
            if (frameworkElement is TextBox)
                ((TextBoxBase)frameworkElement).SelectAll();
            else if (frameworkElement is PasswordBox)
                ((PasswordBox)frameworkElement).SelectAll();
        }

        private static void IgnoreMouseButton
                (object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var frameworkElement = sender as FrameworkElement;
            if (frameworkElement == null || frameworkElement.IsKeyboardFocusWithin) return;
            e.Handled = true;
            frameworkElement.Focus();
        }






    }// fin de clase
}
