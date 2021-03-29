
using System;

namespace ClienteMarWPFWin7.UI.ViewModels.ModelObservable.ComboboxModels
{
    public class ComboboxConcepto : ComboboxBase
    {
        #region fields
        private int _clave;
        private int _id;
        private string _tipo;
        private string _tiponombre;
        private string _descripcion;
        private int? _logicakey;
        private bool _estiposistema;
        private bool _estipoanonimo;
        private DateTime _fechacreacion;
        private bool _activo;
        private int _orden;
        #endregion

        public override int Key
        {
            get => _id;
            set
            {
                _id = value; NotifyPropertyChanged(nameof(Key), nameof(Id));
            }

        }

        public override string Value
        {
            get => _tiponombre;
            set
            {
                _tiponombre = value; NotifyPropertyChanged(nameof(Value), nameof(TipoNombre));
            }
        }

        public int Clave
        {
            get
            {
                return _clave;
            }
            set
            {
                _clave = value; NotifyPropertyChanged(nameof(Clave));
            }
        }

        public int Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value; NotifyPropertyChanged(nameof(Key), nameof(Id));
            }
        }

        public string Tipo
        {
            get
            {
                return _tipo;
            }
            set
            {
                _tipo = value; NotifyPropertyChanged(nameof(Tipo));
            }
        }

        public string TipoNombre
        {
            get
            {
                return _tiponombre;
            }
            set
            {
                _tiponombre = value; NotifyPropertyChanged(nameof(Value), nameof(TipoNombre));
            }
        }

        public string Descripcion
        {
            get
            {
                return _descripcion;
            }
            set
            {
                _descripcion = value; NotifyPropertyChanged(nameof(Descripcion));
            }
        }

        public int? LogicaKey
        {
            get
            {
                return _logicakey;
            }
            set
            {
                _logicakey = value; NotifyPropertyChanged(nameof(LogicaKey));
            }
        }

        public bool EsTipoSistema
        {
            get
            {
                return _estiposistema;
            }
            set
            {
                _estiposistema = value; NotifyPropertyChanged(nameof(EsTipoSistema));
            }
        }

        public bool EsTipoAnonimo
        {
            get
            {
                return _estipoanonimo;
            }
            set
            {
                _estipoanonimo = value; NotifyPropertyChanged(nameof(EsTipoAnonimo));
            }
        }

        public DateTime FechaCreacion
        {
            get
            {
                return _fechacreacion;
            }
            set
            {
                _fechacreacion = value; NotifyPropertyChanged(nameof(FechaCreacion));
            }
        }

        public bool Activo
        {
            get
            {
                return _activo;
            }
            set
            {
                _activo = value; NotifyPropertyChanged(nameof(Activo));
            }
        }

        public int Orden
        {
            get
            {
                return _orden;
            }
            set
            {
                _orden = value; NotifyPropertyChanged(nameof(Orden));
            }
        } 


    }// fin de clase

}// fin de namespace
