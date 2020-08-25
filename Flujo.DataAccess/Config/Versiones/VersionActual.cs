using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MAR.Config
{
    internal static partial class Settings
    {
        internal static string AdminVersion = @"2018-081";
        internal static string ClientWinVersion = @"9.2.0.5";
        internal static string ClientWebVersion = @"2018-082";
        

        internal static ReleaseNote[] AdminCambios = {
            new ReleaseNote {Descripcion=@"Creacion de archivos CSV para interfase contable desde la pagina Cuentas.", Mostrar=true, Version=@"2017-121" },
            new ReleaseNote {Descripcion=@"Mejora en proceso de pago de ganadores, utilizando un SP en vez de EF.", Mostrar=false, Version=@"2017-121" },
            new ReleaseNote {Descripcion=@"Mejora para Admin, evita introduccion de hora y fechas erroneas en el cambio de hora de cierre", Mostrar=false, Version=@"2017-122" },
            new ReleaseNote {Descripcion=@"Mejora para Admin, evita Re-Inicio de ventas para dia anterior", Mostrar=false, Version=@"2017-123" },
            new ReleaseNote {Descripcion=@"Mejora para Admin, Agrega Opcion para dia NO LABORABLE en la pagina de Premios", Mostrar=true, Version=@"2017-123" },
            new ReleaseNote {Descripcion=@"Mejora para Admin, Agrega Loterias Nuevas Pega4 Real", Mostrar=false, Version=@"2017-124" },
            new ReleaseNote {Descripcion=@"Bingo Mejorado", Mostrar=false, Version=@"2018-081" }

        };

        internal static ReleaseNote[] ClienteWinCambios = {
              new ReleaseNote {Descripcion=@"Mejora para Windows, mejora el tamaño del cliente para resoluciones bajas", Mostrar=false, Version=@"9.2.0.3" },
               new ReleaseNote {Descripcion=@"Arreglo para Windows, vuelve a mostrar los tabs antes de log in", Mostrar=false, Version=@"9.2.0.4" },
               new ReleaseNote {Descripcion=@"Agrega Bingo para Windows", Mostrar=false, Version=@"9.2.0.5" }
        };

        internal static ReleaseNote[] ClienteWebCambios = {
                 new ReleaseNote {Descripcion=@"Mejora para Andriod, evita introduccion de '-' en la Jugada", Mostrar=false, Version=@"2017-121" },
                 new ReleaseNote {Descripcion=@"Introduccion del Bingo", Mostrar=false, Version=@"2017-125" },
                 new ReleaseNote {Descripcion=@"Bingo Mejorado", Mostrar=false, Version=@"2018-081" },
                 new ReleaseNote {Descripcion=@"Mejora confirmacion de ticket", Mostrar=false, Version=@"2018-082" }
        };

    }
}
