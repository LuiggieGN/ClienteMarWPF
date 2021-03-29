using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MAR.Config
{
    internal static partial class Settings
    {
        internal static string AdminVersion = @"2020-1201";
        internal static string ClientWinVersion = @"9.7.1.37";
        internal static string ClientWebVersion = @"2020-12";


        internal static ReleaseNote[] AdminCambios = {
            new ReleaseNote {Descripcion=@"Creacion de archivos CSV para interfase contable desde la pagina Cuentas.", Mostrar=true, Version=@"2017-121" },
            new ReleaseNote {Descripcion=@"Mejora en proceso de pago de ganadores, utilizando un SP en vez de EF.", Mostrar=false, Version=@"2017-121" },
            new ReleaseNote {Descripcion=@"Mejora para Admin, evita introduccion de hora y fechas erroneas en el cambio de hora de cierre", Mostrar=false, Version=@"2017-122" },
            new ReleaseNote {Descripcion=@"Mejora para Admin, evita Re-Inicio de ventas para dia anterior", Mostrar=false, Version=@"2017-123" },
            new ReleaseNote {Descripcion=@"Mejora para Admin, Agrega Opcion para dia NO LABORABLE en la pagina de Premios", Mostrar=true, Version=@"2017-123" },
            new ReleaseNote {Descripcion=@"Mejora para Admin, Agrega Loterias Nuevas Pega4 Real", Mostrar=false, Version=@"2017-124" },
            new ReleaseNote {Descripcion=@"Bingo Mejorado", Mostrar=false, Version=@"2018-081" },
            new ReleaseNote {Descripcion=@"Agrega correcion de premios de un dia cerrado", Mostrar=false, Version=@"2019-059" },
            new ReleaseNote {Descripcion=@"Bloquea Limite por dinero si ya existe venta", Mostrar=false, Version=@"2019-060" },
            new ReleaseNote {Descripcion=@"Agrega Flujo Efectivo", Mostrar=false, Version=@"2019-090" },
            new ReleaseNote {Descripcion=@"Mejora Flujo Efectivo Dashboard", Mostrar=false, Version=@"2019-096" },
            new ReleaseNote {Descripcion=@"Mejora Flujo Agrega funcion para transferencias a bancas", Mostrar=false, Version=@"2019-097" },
                      new ReleaseNote {Descripcion=@"Mejora Flujo Agrega filtros para usuarios riferos", Mostrar=false, Version=@"2019-0923" },
                      new ReleaseNote {Descripcion=@"Agrega mantenimiento de api externa en bancas", Mostrar=false, Version=@"2019-1008" },
                      new ReleaseNote {Descripcion=@"Agrega Consolidacion de Pagos al cerrar dia tarde", Mostrar=false, Version=@"2019-1010" },
                      new ReleaseNote {Descripcion=@"Agrega Reporte admin para Cinco Minutos", Mostrar=false, Version=@"2019-1016" },
                      new ReleaseNote {Descripcion=@"Valida si esta conectado a hacienda", Mostrar=false, Version=@"2019-1028" },
                      new ReleaseNote {Descripcion=@"Agrega reporte de cinco minutos automatizado", Mostrar=false, Version=@"2019-1114" },
                      new ReleaseNote {Descripcion=@"Incluye Sorteo La Primera Y reportes de Camion", Mostrar=false, Version=@"2019-1212" },
                      new ReleaseNote {Descripcion=@"Cambia codigo de entrar premios en super pale", Mostrar=false, Version=@"2019-1220" },
                      new ReleaseNote {Descripcion=@"Mejora proceso de cierre de dias", Mostrar=false, Version=@"2020-0113" },
                      new ReleaseNote {Descripcion=@"Busca tickets ganadores adecuadamente cuando dias no han cerrado.", Mostrar=false, Version=@"2020-0116" },
                      new ReleaseNote {Descripcion=@"Inicio y cierre de Recargas. Agrega Rifero Especial.", Mostrar=false, Version=@"2020-0218" },
                      new ReleaseNote {Descripcion=@"Agrega nuevo permiso para central especial.", Mostrar=false, Version=@"2020-0301" },
                      new ReleaseNote {Descripcion=@"Agrega Limites y Comisiones Camion.", Mostrar=false, Version=@"2020-0527" },
                      new ReleaseNote {Descripcion=@"Agrega Admin Nuevo", Mostrar=false, Version=@"2020-0612" },
                      new ReleaseNote {Descripcion=@"Agrega Bulk de consolidado al cierre de dias", Mostrar=false, Version=@"2020-1201" }
        };

        internal static ReleaseNote[] ClienteWinCambios = {
              new ReleaseNote {Descripcion=@"Mejora para Windows, mejora el tamaño del cliente para resoluciones bajas", Mostrar=false, Version=@"9.2.0.3" },
               new ReleaseNote {Descripcion=@"Arreglo para Windows, vuelve a mostrar los tabs antes de log in", Mostrar=false, Version=@"9.2.0.4" },
               new ReleaseNote {Descripcion=@"Agrega Bingo para Windows", Mostrar=false, Version=@"9.2.0.5" },
               new ReleaseNote {Descripcion=@"Agrega Auto logout en base a un valor desde la db", Mostrar=false, Version=@"9.2.0.7" },
               new ReleaseNote {Descripcion=@"Agrega Flujo Efectivo", Mostrar=false, Version=@"9.5.0.0" },
               new ReleaseNote {Descripcion=@"Inicia Cinco minutos", Mostrar=false, Version=@"9.5.0.1"},
                  new ReleaseNote {Descripcion=@"Aumenta MaxReceivedMessageSize", Mostrar=false, Version=@"9.5.0.2"},
                  new ReleaseNote {Descripcion=@"Mejora reporte de productos en el punto de venta", Mostrar=false, Version=@"9.5.0.3"},
                  new ReleaseNote {Descripcion=@"Mejora Cambio de monitor cinco minutos", Mostrar=false, Version=@"9.5.0.7"},
                  new ReleaseNote {Descripcion=@"Mejora Pagos Cinco Minutos", Mostrar=false, Version=@"9.5.0.8"},
                  new ReleaseNote {Descripcion=@"Valida Si esta conectado a hacienda", Mostrar=false, Version=@"9.5.0.11"},
                  new ReleaseNote {Descripcion=@"Agrega Anulacion de Camion Millonario cliente wpf", Mostrar=false, Version=@"9.5.0.12"},
                  new ReleaseNote {Descripcion=@"Incluye Sorteo La primera", Mostrar=false, Version=@"9.6.0.0" },
                  new ReleaseNote {Descripcion=@"Arrega impresion hacienda y la primera", Mostrar=false, Version=@"9.7.0.0" },
                  new ReleaseNote {Descripcion=@"Valida pago contra balance si tiene flujo", Mostrar=false, Version=@"9.7.0.1" },
                  new ReleaseNote {Descripcion=@"Inicio y cierre de Recargas.", Mostrar=false, Version=@"9.7.0.2" },
                  new ReleaseNote {Descripcion=@"Agrega clave modulo de configuracion.", Mostrar=false, Version=@"9.7.0.10" },
                  new ReleaseNote {Descripcion=@"Agrega Limite camion millonario", Mostrar=false, Version=@"9.7.0.11" },
                  new ReleaseNote {Descripcion=@"Cambios admin nuevo", Mostrar=false, Version=@"9.7.0.12" },
                  new ReleaseNote {Descripcion=@"Agrega Reload Sorteos Windows", Mostrar=false, Version=@"9.7.0.13" }  ,
                  new ReleaseNote {Descripcion=@"Agrega Reload WPF Multi sorteos", Mostrar=false, Version=@"9.7.1.35" }
        };

        internal static ReleaseNote[] ClienteWebCambios = {
                 new ReleaseNote {Descripcion=@"Mejora para Andriod, evita introduccion de '-' en la Jugada", Mostrar=false, Version=@"2017-121" },
                 new ReleaseNote {Descripcion=@"Introduccion del Bingo", Mostrar=false, Version=@"2017-125" },
                 new ReleaseNote {Descripcion=@"Bingo Mejorado", Mostrar=false, Version=@"2018-081" },
                 new ReleaseNote {Descripcion=@"Mejora confirmacion de ticket", Mostrar=false, Version=@"2018-082" },
                 new ReleaseNote {Descripcion=@"Corrige error en reporte de venta con la Comision", Mostrar=false, Version=@"2018-083" },
                 new ReleaseNote {Descripcion=@"Filtra Sorteos no disponibles", Mostrar=false, Version=@"2019-090" },
                 new ReleaseNote {Descripcion=@"Mejora en print venta", Mostrar=false, Version=@"2020-04" },
                 new ReleaseNote {Descripcion=@"Incluye Super a la Carta", Mostrar=false, Version=@"2020-12" }
        };

    }
}
