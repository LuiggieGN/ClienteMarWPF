using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace CustomPrinterLib
{
    /// <summary>
    /// Enumeration for label key
    /// </summary>
    //public enum LabelKeyType
    //{
    //    Null = 0, Money = 1, TotalStake = 2, TotalPrice = 3, MaxReturn = 4, PlaceDate = 5, PlaceTime = 6, User = 7, TerminalID = 8
    //}

    public class LocalizeLabelItem
    {
        public string key;
        public string value;

        public LocalizeLabelItem()
        {
            key = string.Empty;
            value = string.Empty;
        }

        public LocalizeLabelItem(string k, string v)
        {
            key = k;
            value = v;
        }
    }

    public class LocalizeLabelMapper : List<LocalizeLabelItem>
    {
        private string m_Language = null;

        public LocalizeLabelMapper()
            : base()
        {
        }

        [XmlIgnore]
        public string Language
        {
            get
            {
                return m_Language;
            }
        }

        public void Add(string key, string value)
        {
            base.Add(new LocalizeLabelItem(key, value));
        }

        public new void Add(LocalizeLabelItem obj)
        {
            base.Add(obj);
        }

        private int getIndex(string key)
        {
            int r = -1;
            for (int i = 0; i < Count; i++)
            {
                if (this[i].key.Equals(key))
                {
                    r = i;
                    break;
                }
            }
            return r;
        }

        public string this[string key]
        {
            get
            {
                int idx = getIndex(key);
                if (idx == -1)
                {
                    return null;
                }
                return this[idx].value;
            }
        }

        public static LocalizeLabelMapper getLanguage(string path, string language)
        {
            string filename = ((path != null) ? path + "\\" : string.Empty ) + language + ".xml";
            LocalizeLabelMapper cl = Load(filename);
            cl.m_Language = language;
            return cl;
        }

        public void saveLanguage(string path, string language)
        {
            m_Language = language;
            Save(path + "\\" + language + ".xml");
        }

        private static LocalizeLabelMapper Load(string filename)
        {
            LocalizeLabelMapper obj;
            XmlSerializer serializer = new XmlSerializer(typeof(LocalizeLabelMapper));
            FileStream stream = new FileStream(filename, FileMode.Open);
            obj = (LocalizeLabelMapper)serializer.Deserialize(stream);
            stream.Close();
            return obj;
        }

        public void Save(string filename)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(LocalizeLabelMapper));
            StreamWriter stream = new StreamWriter(filename);
            serializer.Serialize(stream, this);
            stream.Close();
        }
    }
}
