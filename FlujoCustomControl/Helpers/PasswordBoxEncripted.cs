
using System;
using System.Security;
using System.Windows;



namespace FlujoCustomControl
{
    public static class PasswordBoxEncripted
    {
        public static SecureString GetEncryptedPassword(DependencyObject obj)
        {
            return (SecureString)obj.GetValue(EncryptedPasswordProperty);
        }

        public static void SetEncryptedPassword(DependencyObject obj, SecureString value)
        {
            obj.SetValue(EncryptedPasswordProperty, value);
        }

        public static readonly DependencyProperty EncryptedPasswordProperty =  DependencyProperty.RegisterAttached("EncryptedPassword", typeof(SecureString), typeof(PasswordBoxEncripted));

    }
}
