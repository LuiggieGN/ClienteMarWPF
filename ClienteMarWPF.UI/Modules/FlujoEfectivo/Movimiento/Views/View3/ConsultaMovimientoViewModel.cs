using System;
using System.Linq;
using System.Windows.Input;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Collections.ObjectModel;

using ClienteMarWPF.UI.State.Authenticators;

using ClienteMarWPF.Domain.Models.Dtos.EfectivoDtos;
using ClienteMarWPF.Domain.Services.CajaService;

using ClienteMarWPF.UI.ViewModels;
using ClienteMarWPF.UI.ViewModels.Base;
using ClienteMarWPF.UI.ViewModels.Helpers;
using ClienteMarWPF.UI.ViewModels.Errors;
using ClienteMarWPF.UI.ViewModels.ModelObservable;
using ClienteMarWPF.UI.ViewModels.Commands.FlujoEfectivo.Movimiento.ConsultaView;

namespace ClienteMarWPF.UI.Modules.FlujoEfectivo.Movimiento.Views.View3
{
    public class ConsultaMovimientoViewModel : BaseViewModel, INotifyDataErrorInfo
    {
        #region Fields
        private readonly ToastViewModel _toast;
        private readonly ErroresViewModel _errores;
        private readonly IAuthenticator _aut;
        private readonly ICajaService _cajaService;
        private DateTime? _from;
        private DateTime? _to;
        private PagedDataViewModel<MovimientoObservable> _movimientos;
        #endregion

        #region Propiedades
        public ToastViewModel Toast => _toast;
        public IAuthenticator Aut => _aut;
        public DateTime? From
        {
            get => _from;
            set
            {
                _from = value; NotifyPropertyChanged(nameof(From));
            }
        }
        public DateTime? To
        {
            get => _to;
            set
            {
                _to = value; NotifyPropertyChanged(nameof(To));
            }
        }
        public PagedDataViewModel<MovimientoObservable> Movimientos => _movimientos;
        public bool PagedDataHasRecords => _movimientos?.HasRecords??false;
        public bool NoRecordsWereFound => !PagedDataHasRecords;
        #endregion

        #region Errors
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        public ErroresViewModel Errores => _errores;
        public bool HasErrors => _errores?.HasErrors ?? false;
        public bool CanCreate => !HasErrors;
        #endregion

        #region Commands
        public ICommand ConsultarMovimientosCommand { get; private set; }
        #endregion

        public ConsultaMovimientoViewModel(IAuthenticator aut, ICajaService cajaService)
        {
            _toast = new ToastViewModel();

            _errores = new ErroresViewModel();
            _errores.ErrorsChanged += EnCambioDeErrores;

            _aut = aut;
            _cajaService = cajaService;

            _movimientos = new PagedDataViewModel<MovimientoObservable>
            (
                new List<ICommand>(){
                new PrimeroCommand(this,_cajaService),
                new AnteriorCommand(this,_cajaService),
                new SiguienteCommand(this,_cajaService),
                new UltimoCommand(this,_cajaService)
            });

            ConsultarMovimientosCommand = new ConsultarMovimientosCommand(this, _cajaService);
        }


        #region Metodos Errors Handling
        private void EnCambioDeErrores(object sender, DataErrorsChangedEventArgs e)
        {
            ErrorsChanged?.Invoke(this, e);
            NotifyPropertyChanged(nameof(CanCreate));
        }

        public IEnumerable GetErrors(string propertyName)
        {
            return _errores.GetErrors(propertyName);
        }
        #endregion

        public void PageWasChanged() 
        {
            NotifyPropertyChanged(nameof(Movimientos),
                                  nameof(PagedDataHasRecords),
                                  nameof(NoRecordsWereFound));
        
        }





    } // fin de clase
}
