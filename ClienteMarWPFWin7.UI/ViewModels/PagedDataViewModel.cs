
using System.Windows.Input;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using ClienteMarWPFWin7.UI.ViewModels.Base;

namespace ClienteMarWPFWin7.UI.ViewModels  
{
    public class PagedDataViewModel<T> : BaseViewModel where T : BaseViewModel
    {
        #region Commands
        public ICommand PrimeroCommand { get; private set; }
        public ICommand AnteriorCommand { get; private set; }
        public ICommand SiguienteCommand { get; private set; }
        public ICommand UltimoCommand { get; private set; }
        #endregion

        #region Fields
        private int _paginaSize;
        private int _paginaNo;
        private int _totalPaginas;
        private int _totalRecords;
        private bool _ordenAscendiendo;
        private string _ordenColumna;
        private List<T> _vistaPaginada;
        #endregion

        #region Properties
        public int PaginaSize
        {
            get => _paginaSize;
            set
            {
                _paginaSize = value; NotifyPropertyChanged(nameof(PaginaSize));
            }
        }
        public int PaginaNo
        {
            get => _paginaNo;
            set
            {
                _paginaNo = value; NotifyPropertyChanged(nameof(PaginaNo));
            }
        }
        public int TotalPaginas
        {
            get => _totalPaginas;
            set
            {
                _totalPaginas = value; NotifyPropertyChanged(nameof(TotalPaginas));
            }
        }
        public int TotalRecords
        {
            get => _totalRecords;
            set
            {
                _totalRecords = value; NotifyPropertyChanged(nameof(TotalRecords),
                                                             nameof(HasRecords));
            }
        }
        public bool OrdenAscendiendo
        {
            get => _ordenAscendiendo;
            set
            {
                _ordenAscendiendo = value; NotifyPropertyChanged(nameof(OrdenAscendiendo));
            }
        }
        public string OrdenColumna
        {
            get => _ordenColumna;
            set
            {
                _ordenColumna = value; NotifyPropertyChanged(nameof(OrdenColumna));
            }
        }
        public List<T> VistaPaginada
        {
            get => _vistaPaginada;
            set
            {
                _vistaPaginada = value; NotifyPropertyChanged(nameof(VistaPaginada));
            }
        }
        public bool HasRecords => TotalRecords > 0;
        #endregion

        public PagedDataViewModel(List<ICommand> comandos)
        {
            _paginaSize = 25;
            _paginaNo = 1;
            _totalPaginas = 0;
            _totalRecords = 0;
            _ordenAscendiendo = false;
            _ordenColumna = string.Empty;

            PrimeroCommand = comandos[0];
            AnteriorCommand = comandos[1];
            SiguienteCommand = comandos[2];
            UltimoCommand = comandos[3];
        }



    }
}
