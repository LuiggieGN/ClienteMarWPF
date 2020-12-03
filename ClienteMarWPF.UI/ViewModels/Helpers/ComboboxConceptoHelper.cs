

using System.Collections.Generic;
using System.Collections.ObjectModel;

using ClienteMarWPF.Domain.Models.Dtos.EfectivoDtos;

using ClienteMarWPF.UI.ViewModels.ModelObservable.ComboboxModels;

namespace ClienteMarWPF.UI.ViewModels.Helpers
{
    public static class ComboboxConceptoHelper
    {
        public static ObservableCollection<ComboboxConcepto> MapToObservableCollection(this List<TipoAnonimoDTO> anonimos, int tipoIE, bool incluyeEspecificarConcepto)
        {
            var comboOpciones = new ObservableCollection<ComboboxConcepto>();

            if (anonimos == null || anonimos.Count == 0) { return comboOpciones; }

            for (int i = 0; i < anonimos.Count; i++)
            {
                comboOpciones.Add(new ComboboxConcepto()
                {
                    Clave = anonimos[i].Clave,
                    Id = anonimos[i].Id,
                    Tipo = anonimos[i].Tipo,
                    TipoNombre = anonimos[i].TipoNombre,
                    Descripcion = anonimos[i].Descripcion,
                    LogicaKey = anonimos[i].LogicaKey,
                    EsTipoSistema = anonimos[i].EsTipoSistema,
                    EsTipoAnonimo = anonimos[i].EsTipoAnonimo,
                    FechaCreacion = anonimos[i].FechaCreacion,
                    Activo = anonimos[i].Activo,
                    Orden = anonimos[i].Orden,
                    Color = anonimos[i].Clave == 1 ? "#28A745" : "#DC3545"
                });
            }

            if (incluyeEspecificarConcepto)
            {
                if (tipoIE == 1)
                {//Especificar Ingreso
                    comboOpciones.Add(new ComboboxConcepto()
                    {
                        Clave = 1,
                        Id = 0,
                        Tipo = "Especificar Otra Entrada",
                        TipoNombre = "Especificar otro tipo de Entrda",
                        Descripcion = "Para que el usuario registre otro tipo de entrada",
                        LogicaKey = 0,
                        EsTipoSistema = false,
                        EsTipoAnonimo = true,
                        Activo = true,
                        Color = "#28A745"
                    });
                }
                else
                {//Especificar Egreso
                    comboOpciones.Add(new ComboboxConcepto()
                    {
                        Clave = 0,
                        Id = 0,
                        Tipo = "Especificar Otra Salida",
                        TipoNombre = "Especificar otro tipo de Salida",
                        Descripcion = "Para que el usuario registre otro tipo de entrada",
                        LogicaKey = 0,
                        EsTipoSistema = false,
                        EsTipoAnonimo = true,
                        Activo = true,
                        Color = "#DC3545"
                    });
                }

            }//fin de if

            return comboOpciones;

        } // fin de metodo



    } // fin de clase
}
