using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace CustomPrinterLib
{
    public class CharConvert
    {
        public char OrigChar;
        public char[] InternationalChar;

        public CharConvert()
        {
        }

        public CharConvert(char origChar, char[] internationalChar)
        {
            OrigChar = origChar;
            InternationalChar = internationalChar;
        }

        public string ApplyReplace(string s)
        {
            return s.Replace(OrigChar.ToString(), new string(InternationalChar));
        }

        public override string ToString()
        {
            string s = "Convert: " + OrigChar + " = ";
            for (int i = 0; i < InternationalChar.Length; i++)
            {
                s += (((byte)InternationalChar[i]).ToString() + " ");
            }
            return s;
        }
    }

    public class CharMapper : List<CharConvert>
    {
        public CharMapper()
            : base()
        {
        }

        public void Add(char origChar, char[] internationalChar)
        {
            base.Add(new CharConvert(origChar, internationalChar));
        }

        public new void Add(CharConvert obj)
        {
            base.Add(obj);
        }

        public string ApplyReplace(string s)
        {
            string ret = s;
            foreach (CharConvert conv in this)
            {
                ret = conv.ApplyReplace(ret);
            }
            return ret;
        }

        public static CharMapper getCharMapper(string filename)
        {
            CharMapper obj;
            XmlSerializer serializer = new XmlSerializer(typeof(CharMapper));
            FileStream stream = new FileStream(filename, FileMode.Open);
            obj = (CharMapper)serializer.Deserialize(stream);
            stream.Close();
            return obj;
        }

        public void Save(string filename)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(CharMapper));
            StreamWriter stream = new StreamWriter(filename);
            serializer.Serialize(stream, this);
            stream.Close();
        }
    }

    public class StringConverterHelper
    {
        private static CharMapper m_Mapper = null;
        public static void Create()
        {
            CharMapper map = new CharMapper();
            map.Add('ñ', new char[] { (char)0x1B, (char)0x52, (char)0x07, (char)0x7C });
            map.Add('Ñ', new char[] { (char)0x1B, (char)0x52, (char)0x07, (char)0x5C });
            map.Add('è', new char[] { (char)0x1B, (char)0x52, (char)0x06, (char)0x7D });
            map.Add('é', new char[] { (char)0x1B, (char)0x52, (char)0x06, (char)0x5D });
            map.Add('ì', new char[] { (char)0x1B, (char)0x52, (char)0x06, (char)0x7E });
            map.Add('í', new char[] { (char)0x1B, (char)0x74, (char)0x02, (char)0xA1 });
            map.Add('ù', new char[] { (char)0x1B, (char)0x52, (char)0x06, (char)0x60 });
            map.Add('ú', new char[] { (char)0x1B, (char)0x74, (char)0x02, (char)0xA3 });
            map.Add('à', new char[] { (char)0x1B, (char)0x52, (char)0x06, (char)0x7B });
            map.Add('á', new char[] { (char)0x1B, (char)0x74, (char)0x02, (char)0xA0 });
            map.Add('ò', new char[] { (char)0x1B, (char)0x52, (char)0x06, (char)0x7C });
            map.Add('ó', new char[] { (char)0x1B, (char)0x74, (char)0x02, (char)0xA2 });
            map.Add('È', new char[] { (char)0x1B, (char)0x74, (char)0x02, (char)0xD4 });
            map.Add('É', new char[] { (char)0x1B, (char)0x52, (char)0x09, (char)0x40 });
            map.Add('Ì', new char[] { (char)0x1B, (char)0x74, (char)0x02, (char)0xDE });
            map.Add('Í', new char[] { (char)0x1B, (char)0x74, (char)0x02, (char)0xD6 });
            map.Add('Ù', new char[] { (char)0x1B, (char)0x74, (char)0x02, (char)0xEB });
            map.Add('Ú', new char[] { (char)0x1B, (char)0x74, (char)0x02, (char)0xE9 });
            map.Add('À', new char[] { (char)0x1B, (char)0x74, (char)0x02, (char)0xB7 });
            map.Add('Á', new char[] { (char)0x1B, (char)0x74, (char)0x02, (char)0xB5 });
            map.Add('Ò', new char[] { (char)0x1B, (char)0x74, (char)0x02, (char)0xE3 });
            map.Add('Ó', new char[] { (char)0x1B, (char)0x74, (char)0x02, (char)0xE0 });
            map.Add('°', new char[] { (char)0x1B, (char)0x52, (char)0x06, (char)0x5B });
            map.Save("AInternationalChars.xml");
        }

        public static void Init(string filename)
        {
            if (m_Mapper == null)
            {
                m_Mapper = CharMapper.getCharMapper(filename);
            }
        }

        public static string Convert(string s)
        {
            return m_Mapper.ApplyReplace(s);
        }
    }
}
