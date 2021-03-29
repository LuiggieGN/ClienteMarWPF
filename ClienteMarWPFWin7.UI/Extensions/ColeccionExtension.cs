using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClienteMarWPFWin7.UI.Extensions
{
    public static class ColeccionExtension
    {
        public static TValue GetValueOrDefault<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key)
          => dict.TryGetValue(key, out var value) ? value : default(TValue);
    }

 



}
