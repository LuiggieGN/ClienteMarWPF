using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlujoCustomControl.Helpers
{
    public static class EnumHelper
    {
        public static IEnumerable<T> ObtenerEnumElementos<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }
    }
}
