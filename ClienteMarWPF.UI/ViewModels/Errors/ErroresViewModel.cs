using System;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace ClienteMarWPF.UI.ViewModels.Errors
{
    public class ErroresViewModel: INotifyDataErrorInfo
    {
        private readonly Dictionary<string, List<string>> _errores = new Dictionary<string, List<string>>();

        public bool HasErrors => _errores.Any();

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public IEnumerable GetErrors(string propertyName)
        {
            return _errores.GetValueOrDefault(propertyName, null);
        }

        public void AgregarError(string propiedad, string error)
        {
            if (!_errores.ContainsKey(propiedad))
            {
                _errores.Add(propiedad, new List<string>());
            }
            _errores[propiedad].Add(error);
            
            EnCambioDeErrores(propiedad);
        }

        public void EliminarError(string propiedad)
        {
            if (_errores.ContainsKey(propiedad)) // Se asegura de que la propiedada la cual se atacha el error exista en la coleccion para eliminarla
            {
                if (_errores.Remove(propiedad))
                {
                    EnCambioDeErrores(propiedad);
                }
            }
        }

        private void EnCambioDeErrores(string propiedad)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propiedad));
        }

    }// fin de clase
}
