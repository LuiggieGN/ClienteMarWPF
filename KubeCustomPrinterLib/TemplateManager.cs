using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Xml;
using System.Drawing;
using InterchangeDataLib;

namespace CustomPrinterLib
{
    class AttributeHashtable : Hashtable
    {
        private static Hashtable m_DecodeHAlign = null;
        private static Hashtable m_DecodeBarcodeTextPos = null;
        private static Hashtable m_DecodeBarcodeType = null;
        private static Hashtable m_DecodePrintMode = null;
        private static Hashtable m_DecodeUnderlineType = null;
        private static Hashtable m_DecodeFont = null;
        private static Hashtable m_DecodeCPI = null;

        public AttributeHashtable(XmlAttributeCollection attributes)
            : base(attributes.Count)
        {
            if (m_DecodeHAlign == null)
            {
                m_DecodeHAlign = new Hashtable();
                m_DecodeHAlign.Add("left", HAlignType.Left);
                m_DecodeHAlign.Add("center", HAlignType.Center);
                m_DecodeHAlign.Add("right", HAlignType.Rigth);
            }
            if(m_DecodeBarcodeTextPos == null)
            {
                m_DecodeBarcodeTextPos = new Hashtable();
                m_DecodeBarcodeTextPos.Add("null", BarCodeTextPositionType.Null);
                m_DecodeBarcodeTextPos.Add("down", BarCodeTextPositionType.Down);
                m_DecodeBarcodeTextPos.Add("upper", BarCodeTextPositionType.Upper);
                m_DecodeBarcodeTextPos.Add("upper down", BarCodeTextPositionType.UpperAndDown);
            }
            if(m_DecodeBarcodeType == null)
            {
                m_DecodeBarcodeType = new Hashtable();
                m_DecodeBarcodeType.Add("codabar", BarCodeType.CODABAR);
                m_DecodeBarcodeType.Add("code128", BarCodeType.CODE128);
                m_DecodeBarcodeType.Add("code32", BarCodeType.CODE32);
                m_DecodeBarcodeType.Add("code39", BarCodeType.CODE39);
                m_DecodeBarcodeType.Add("code93", BarCodeType.CODE93);
                m_DecodeBarcodeType.Add("ean13", BarCodeType.EAN13);
                m_DecodeBarcodeType.Add("ean8", BarCodeType.EAN8);
                m_DecodeBarcodeType.Add("itf", BarCodeType.ITF);
                m_DecodeBarcodeType.Add("upc a", BarCodeType.UPC_A);
                m_DecodeBarcodeType.Add("upc e", BarCodeType.UPC_E);
            }
            if(m_DecodePrintMode == null)
            {
                m_DecodePrintMode = new Hashtable();
                m_DecodePrintMode.Add("normal", PrintModeType.Normal);
                m_DecodePrintMode.Add("doubledouble", PrintModeType.DoubleDouble);
                m_DecodePrintMode.Add("doubleheight", PrintModeType.DoubleHeight);
                m_DecodePrintMode.Add("doublewidth", PrintModeType.DoubleWidth);
            }
            if(m_DecodeUnderlineType == null)
            {
                m_DecodeUnderlineType = new Hashtable();
                m_DecodeUnderlineType.Add("null", UnderlineType.Null);
                m_DecodeUnderlineType.Add("single", UnderlineType.Single);
                m_DecodeUnderlineType.Add("double", UnderlineType.Double);
            }
            if(m_DecodeFont == null)
            {
                m_DecodeFont = new Hashtable();
                m_DecodeFont.Add("large", CustomPrinterFontType.FontLarge);
                m_DecodeFont.Add("small", CustomPrinterFontType.FontSmall);
            }
            if (m_DecodeCPI == null)
            {
                m_DecodeCPI = new Hashtable();
                m_DecodeCPI.Add("less", CustomPrinterCPIMode.CPILess);
                m_DecodeCPI.Add("more", CustomPrinterCPIMode.CPIMore);
            }

            foreach (XmlAttribute attr in attributes)
            {
                this.Add(attr.Name.ToLower(), attr.Value);
            }
        }

        private object getDecode(Hashtable decodeTable, string k, string errMsg)
        {
            object v = decodeTable[k.ToLower()];
            if (v == null)
            {
                throw new Exception("Unkown code for " + errMsg);
            }
            return v;
        }

        public Bitmap getBitmap(string key)
        {
            string filename = this[key];
            Bitmap image = new Bitmap(filename);
            return image;
        }

        public byte getByteValue(string key, byte def)
        {
            string parm = this[key];
            return (byte)((parm == null) ? def : byte.Parse(parm));
        }

        public byte getByteValue(string key)
        {
            return getByteValue(key, 0);
        }

        public int getIntValue(string key)
        {
            string parm = this[key];
            return (int)((parm == null) ? 0 : int.Parse(parm));
        }

        public object getMapValue(Object obj)
        {
            object value = null;
            string mapping = this["mapping"];
            string str = this["string"];
            if (mapping != null)
            {
                string[] fieldName = mapping.Split('.');
                value = obj;
                for (int i = 0; i < fieldName.Length; i++)
                {
                    System.Reflection.FieldInfo field = value.GetType().GetField(fieldName[i]);
                    value = field.GetValue(value);
                }
            }
            else if (str != null)
            {
                value = str;
            }
            return value;
        }

        public bool getBoolValue(string key)
        {
            bool ret = false;
            string v = this[key];
            if (v != null)
            {
                if (v.Equals("yes", StringComparison.OrdinalIgnoreCase) || v.Equals("true", StringComparison.OrdinalIgnoreCase))
                {
                    ret = true;
                }
            }
            return ret;
        }

        public HAlignType getHAlign(string key)
        {
            HAlignType align = HAlignType.Left;
            string parm = this[key];
            if(parm != null)
            {
                align = (HAlignType)getDecode(m_DecodeHAlign, parm, "horizontal align.");
            }
            return align;
        }

        public BarCodeTextPositionType getBarcodeTextPos(string key)
        {
            BarCodeTextPositionType pos = BarCodeTextPositionType.Null;
            string parm = this[key];
            if (parm != null)
            {
                pos = (BarCodeTextPositionType)getDecode(m_DecodeBarcodeTextPos, parm, "barcode text position.");
            }
            return pos;
        }

        public BarCodeType getBarcodeType(string key)
        {
            BarCodeType code = BarCodeType.CODE39;
            string parm = this[key];
            if (parm != null)
            {
                code = (BarCodeType)getDecode(m_DecodeBarcodeType, parm, "barcode type.");
            }
            return code;
        }

        public PrintModeType getPrintMode(string key)
        {
            PrintModeType code = PrintModeType.Normal;
            string parm = this[key];
            if (parm != null)
            {
                code = (PrintModeType)getDecode(m_DecodePrintMode, parm, "print mode type.");
            }
            return code;
        }

        public UnderlineType getUnderlineType(string key)
        {
            UnderlineType code = UnderlineType.Null;
            string parm = this[key];
            if (parm != null)
            {
                code = (UnderlineType)getDecode(m_DecodeUnderlineType, parm, "underline type.");
            }
            return code;
        }

        public CustomPrinterFontType getFont(string key)
        {
            CustomPrinterFontType code = CustomPrinterFontType.FontLarge;
            string parm = this[key];
            if (parm != null)
            {
                code = (CustomPrinterFontType)getDecode(m_DecodeFont, parm, "font.");
            }
            return code;
        }

        public CustomPrinterCPIMode getCPI(string key)
        {
            CustomPrinterCPIMode code = CustomPrinterCPIMode.CPILess;
            string parm = this[key];
            if (parm != null)
            {
                code = (CustomPrinterCPIMode)getDecode(m_DecodeCPI, parm, "cpi mode.");
            }
            return code;
        }

        public string this[string key]
        {
            get
            {
                return (string)this[(object)key];
            }
        }
    }

    class TemplateCacheManager : Hashtable
    {
        public TemplateCacheManager()
            : base()
        {
        }

        public void Add(XmlDocument template, string languageKey, string templateKey)
        {
            string key = languageKey + "_" + templateKey;
            Add(key, template);
        }

        public void LoadTemplate(string filename, string languageKey, string templateKey)
        {
            XmlDocument template = new XmlDocument();
            template.Load(filename);
            Add(template, languageKey, templateKey);
        }

        public XmlDocument getTemplate(string languageKey, string templateKey)
        {
            string key = languageKey + "_" + templateKey;
            return (XmlDocument)this[key];
        }
    }

    public class TemplateManager
    {
        private bool m_Init = false;
        private TemplateCacheManager m_Cache = new TemplateCacheManager();
        private LocalizeLabelMapper m_LabelMapper = null;

        public void Init(string filename)
        {
            StringConverterHelper.Init(filename);
            m_Init = true;
        }

        public void LoadTemplate(string filename, string languageKey, string templateKey)
        {
            if (!m_Init)
            {
                throw new Exception("Please call Init before to start load templates");
            }
            m_Cache.LoadTemplate(filename, languageKey, templateKey);
        }

        public byte[] GenerateRawData(string languageKey, string templateKey, object obj)
        {
            CommandPrinterList commandList = GenerateCommandList(languageKey, templateKey, obj);
            int sz = 0;
            foreach (CommandPrinter cmdPrinter in commandList)
            {
                if (cmdPrinter == null)
                {
                    continue;
                }
                sz += cmdPrinter.SizeInByte;
            }
            byte[] buffer = new byte[sz];
            int index = 0;
            foreach (CommandPrinter cmdPrinter in commandList)
            {
                if (cmdPrinter == null)
                {
                    continue;
                }
                Array.Copy(cmdPrinter.GetRawData(), 0, buffer, index, cmdPrinter.SizeInByte);
                index += cmdPrinter.SizeInByte;
            }
            return buffer;
        }

        private CommandPrinterList GenerateCommandList(string languageKey, string templateKey, object obj)
        {
            // Get template
            XmlDocument xmlTemplate = m_Cache.getTemplate(languageKey, templateKey);
            // Generate command list
            CommandPrinterList commandList = new CommandPrinterList();
            AttributeHashtable attributes = new AttributeHashtable(xmlTemplate.FirstChild.Attributes);
            string parmLanguage = attributes["language"];
            string parmPath = attributes["path"];

            m_LabelMapper = LocalizeLabelMapper.getLanguage(parmPath, parmLanguage);
            commandList.AddRange(ParseChild(xmlTemplate.FirstChild, obj, m_LabelMapper));
            return commandList;
        }

        private CommandPrinter[] ParseChild(XmlNode node, Object obj, LocalizeLabelMapper htLabels)
        {
            CommandPrinterList list = new CommandPrinterList();
            foreach (XmlNode command in node.ChildNodes)
            {
                list.AddRange(ParseCommand(command, obj, htLabels));
            }
            return list.ToArray();
        }

        private CommandPrinter[] ParseCommandLoop(XmlNode command, Object master, LocalizeLabelMapper htLabels)
        {
            AttributeHashtable attributes = new AttributeHashtable(command.Attributes);
            string loopField = attributes["on"];
            System.Reflection.FieldInfo field = master.GetType().GetField(loopField);
            IList iList = (IList)field.GetValue(master);
            CommandPrinterList list = new CommandPrinterList();
            foreach (object obj in iList)
            {
                list.AddRange(ParseChild(command, obj, htLabels));
            }
            return list.ToArray();
        }

        private CommandPrinter[] ParseCommand(XmlNode command, Object obj, LocalizeLabelMapper htLabels)
        {
            if (command is XmlComment)
            {
                return new CommandPrinter[]{ null };
            }
            if (!command.Name.Equals("Command", StringComparison.OrdinalIgnoreCase))
            {
                throw new Exception("Invalid node type. Command element is expected");
            }
            AttributeHashtable attributes = new AttributeHashtable(command.Attributes);
            string cmdName = attributes["name"].ToLower();
            if (cmdName.Equals("loop"))
            {
                return ParseCommandLoop(command, obj, htLabels);
            }
            CommandPrinter commandPrinter = null;
            switch (cmdName)
            {
                case "linefeed":
                    commandPrinter = ParseCommandPrintAndFeed(attributes);
                    break;
                case "unitfeed":
                    commandPrinter = ParseCommandPrintAndUnitFeed(attributes);
                    break;
                case "setfont":
                    commandPrinter = ParseCommandSetFont(attributes);
                    break;
                case "setcpimode":
                    commandPrinter = ParseCommandSetCPI(attributes);
                    break;
                case "setleftmargin":
                    commandPrinter = ParseCommandSetLeftMargin(attributes);
                    break;
                case "sethalign":
                    commandPrinter = ParseCommandSetHAlign(attributes);
                    break;
                case "setcharsize":
                    commandPrinter = ParseCommandSetCharSize(attributes);
                    break;
                case "printstring":
                    commandPrinter = ParseCommandPrintString(attributes, obj);
                    break;
                case "printbarcode":
                    commandPrinter = ParseCommandPrintBarcode(attributes, obj);
                    break;
                case "printsmallimage":
                    commandPrinter = ParseCommandPrintSmallImage(attributes);
                    break;
                case "defineimage":
                    commandPrinter = ParseCommandDefineImage(attributes);
                    break;
                case "printimagedefined":
                    commandPrinter = ParseCommandPrintImageDefined(attributes);
                    break;
                case "resetimagedefined":
                    commandPrinter = ParseCommandResetImageDefined(attributes);
                    break;
                case "printlabel":
                    commandPrinter = ParseCommandPrintLabel(attributes, htLabels);
                    break;
                case "printmoney":
                    commandPrinter = ParseCommandPrintMoney(attributes, obj, htLabels);
                    break;
                case "cutpaper":
                    commandPrinter = ParseCommandCutPaper(attributes);
                    break;
                case "settabs":
                    commandPrinter = ParseCommandSetTabs(attributes);
                    break;
                case "newline":
                    commandPrinter = ParseCommandNewLine(attributes);
                    break;
                case "drawline":
                    commandPrinter = ParseCommandDrawLine(attributes);
                    break;
                case "setunderline":
                    commandPrinter = ParseCommandSetUnderline(attributes);
                    break;
                case "setposition":
                    commandPrinter = ParseCommandSetPosition(attributes);
                    break;
                case "definexyunit":
                    commandPrinter = ParseCommandDefineXYUnit(attributes);
                    break;
                case "defineinterline":
                    commandPrinter = ParseCommandDefineInterline(attributes);
                    break;
                case "definedefaultinterline":
                    commandPrinter = ParseCommandDefineDefualtInterline(attributes);
                    break;
                case "definecustomchar":
                    commandPrinter = ParseCommandDefineCustomChar(attributes);
                    break;
                case "setcustomchar":
                    commandPrinter = ParseCommandSetCustomChar(attributes);
                    break;
                case "papertocut":
                    commandPrinter = ParseCommandAlignPaperToCut(attributes);
                    break;
                case "reset":
                    commandPrinter = ParseCommandReset(attributes);
                    break;
            }
            if (commandPrinter == null)
            {
                throw new Exception("Command unkown");
            }
            return new CommandPrinter[]{commandPrinter};
        }

        private CommandPrintAndFeed ParseCommandPrintAndFeed(AttributeHashtable attributes)
        {
            int line = attributes.getIntValue("line");
            CommandPrintAndFeed cmd = new CommandPrintAndFeed();
            cmd.SetLineNumber(line);
            return cmd;
        }

        private CommandPrintAndUnitFeed ParseCommandPrintAndUnitFeed(AttributeHashtable attributes)
        {
            byte unit = attributes.getByteValue("unit");
            CommandPrintAndUnitFeed cmd = new CommandPrintAndUnitFeed(unit);
            return cmd;
        }

        private CommandSetFont ParseCommandSetFont(AttributeHashtable attributes)
        {
            CommandSetFont cmd = new CommandSetFont();
            cmd.SetFont(attributes.getFont("font"));
            return cmd;
        }

        private CommandSetCPI ParseCommandSetCPI(AttributeHashtable attributes)
        {
            CommandSetCPI cmd = new CommandSetCPI();
            cmd.SetCPI(attributes.getCPI("cpi"));
            return cmd;
        }

        private CommandSetLeftMargin ParseCommandSetLeftMargin(AttributeHashtable attributes)
        {
            int margin = attributes.getIntValue("margin");
            CommandSetLeftMargin cmd = new CommandSetLeftMargin();
            cmd.SetMargin(margin);
            return cmd;
        }

        private CommandSetHAlign ParseCommandSetHAlign(AttributeHashtable attributes)
        {
            HAlignType align = attributes.getHAlign("align");
            CommandSetHAlign cmd = new CommandSetHAlign(align);
            return cmd;
        }

        private CommandSetCharSize ParseCommandSetCharSize(AttributeHashtable attributes)
        {
            byte x = attributes.getByteValue("x");
            byte y = attributes.getByteValue("y");
            CommandSetCharSize cmd = new CommandSetCharSize(x, y);
            return cmd;
        }

        private string getFilledString(string s, byte size, bool bOnLeft)
        {
            if (size == 0)
            {
                return s;
            }
            if (s.Length >= size)
            {
                return s.Substring(0, size);
            }
            string fill = new string(' ', size - s.Length);
            if (bOnLeft)
            {
                return fill + s;
            }
            return s + fill;
        }

        private CommandPrintString ParseCommandPrintString(AttributeHashtable attributes, Object obj)
        {
            string valueString = string.Empty;
            object value = attributes.getMapValue(obj);
            bool bNewLine = attributes.getBoolValue("linefeed");
            string formatString = attributes["format"];
            byte fixedLen = attributes.getByteValue("fixedlen");
            bool bFillOnLeft = attributes.getBoolValue("fillonleft");

            if (formatString != null && value != null)
            {
                if (value is DateTime)
                {
                    valueString = ((DateTime)value).ToString(formatString);
                }
                else
                {
                    valueString = ((decimal)value).ToString(formatString);
                }
            }
            else
            {
                valueString = value.ToString();
            }
            valueString = getFilledString(valueString, fixedLen, bFillOnLeft);
            CommandPrintString cmd = new CommandPrintString(valueString, bNewLine);
            return cmd;
        }

        private CommandPrintString /*CommandPrintMoney*/ ParseCommandPrintMoney(AttributeHashtable attributes, Object obj, LocalizeLabelMapper htLabels)
        {
            byte fixedLen = attributes.getByteValue("fixedlen");
            bool bFillOnLeft = attributes.getBoolValue("fillonleft");
            bool bNewLine = attributes.getBoolValue("linefeed");
            bool bShowCurrency = attributes.getBoolValue("showcurrency");
            decimal value = decimal.Parse(attributes.getMapValue(obj).ToString());

            string s = CommandPrintMoney.getFormatString(htLabels, value, bShowCurrency);
            s = getFilledString(s, fixedLen, bFillOnLeft);
            //CommandPrintMoney cmd = new CommandPrintMoney(htLabels, value, bShowCurrency, bNewLine);
            CommandPrintString cmd = new CommandPrintString(s, bNewLine);
            return cmd;
        }

        private CommandPrintBarcode ParseCommandPrintBarcode(AttributeHashtable attributes, Object obj)
        {
            BarCodeType type = attributes.getBarcodeType("barcode");
            object value = attributes.getMapValue(obj);
            byte[] barcode = CommandPrintString.GetBytes(value.ToString(), false);
            byte height = attributes.getByteValue("height", 162);

            CommandPrintBarcode cmd = new CommandPrintBarcode(attributes.getFont("font"), attributes.getBarcodeTextPos("textposition"), height, type, barcode);
            return cmd;
        }

        private CommandPrintSmallImage ParseCommandPrintSmallImage(AttributeHashtable attributes)
        {
            Bitmap image = attributes.getBitmap("filename");
            bool bDoubleDensity = attributes.getBoolValue("doubledensity");
            byte[] raw = ImageRasterHelper.ConvertShortBitmap(image, true, bDoubleDensity);
            CommandPrintSmallImage cmd = new CommandPrintSmallImage(raw);
            return cmd;
        }

        private CommandDefineImage ParseCommandDefineImage(AttributeHashtable attributes)
        {
            Bitmap image = attributes.getBitmap("filename");
            byte[] raw = ImageRasterHelper.ConvertBitmap(image, true);
            CommandDefineImage cmd = new CommandDefineImage(raw);
            return cmd;
        }

        private CommandPrintImageDefined ParseCommandPrintImageDefined(AttributeHashtable attributes)
        {
            PrintModeType mode = attributes.getPrintMode("printmode");
            CommandPrintImageDefined cmd = new CommandPrintImageDefined(mode);
            return cmd;
        }

        private CommandPrintLabel ParseCommandPrintLabel(AttributeHashtable attributes, LocalizeLabelMapper htLabels)
        {
            string label = attributes["labelkey"];
            bool bNewLine = attributes.getBoolValue("linefeed");

            CommandPrintLabel cmd = new CommandPrintLabel(htLabels, label, bNewLine);
            return cmd;
        }

        private CommandCutPaper ParseCommandCutPaper(AttributeHashtable attributes)
        {
            CommandCutPaper cmd = new CommandCutPaper();
            return cmd;
        }

        private CommandNewLine ParseCommandNewLine(AttributeHashtable attributes)
        {
            CommandNewLine cmd = new CommandNewLine();
            return cmd;
        }

        private CommandResetImageDefined ParseCommandResetImageDefined(AttributeHashtable attributes)
        {
            CommandResetImageDefined cmd = new CommandResetImageDefined();
            return cmd;
        }

        private CommandReset ParseCommandReset(AttributeHashtable attributes)
        {
            CommandReset cmd = new CommandReset();
            return cmd;
        }

        private CommandSetTabs ParseCommandSetTabs(AttributeHashtable attributes)
        {
            string parm = attributes["tabs"];
            if (parm == null)
            {
                throw new Exception("tabs attribute must be defined");
            }
            string[] t = parm.Split(',');
            byte[] tabs = new byte[t.Length];
            for (int i = 0; i < t.Length; i++)
            {
                tabs[i] = byte.Parse(t[i]);
            }
            CommandSetTabs cmd = new CommandSetTabs(tabs);
            return cmd;
        }

        private CommandDrawLine ParseCommandDrawLine(AttributeHashtable attributes)
        {
            int w = attributes.getIntValue("width");
            CommandDrawLine cmd = new CommandDrawLine(w);
            return cmd;
        }

        private CommandSetPosition ParseCommandSetPosition(AttributeHashtable attributes)
        {
            int x = attributes.getIntValue("x");
            CommandSetPosition cmd = new CommandSetPosition(x);
            return cmd;
        }

        private CommandDefineXYUnit ParseCommandDefineXYUnit(AttributeHashtable attributes)
        {
            byte x = attributes.getByteValue("x");
            byte y = attributes.getByteValue("y");
            CommandDefineXYUnit cmd = new CommandDefineXYUnit(x, y);
            return cmd;
        }

        private CommandDefineInterline ParseCommandDefineInterline(AttributeHashtable attributes)
        {
            byte n = attributes.getByteValue("y");
            CommandDefineInterline cmd = new CommandDefineInterline(n);
            return cmd;
        }

        private CommandAlignPaperToCut ParseCommandAlignPaperToCut(AttributeHashtable attributes)
        {
            CommandAlignPaperToCut cmd = new CommandAlignPaperToCut();
            return cmd;
        }

        private CommandDefineDefualtInterline ParseCommandDefineDefualtInterline(AttributeHashtable attributes)
        {
            CommandDefineDefualtInterline cmd = new CommandDefineDefualtInterline();
            return cmd;
        }

        private CommandSetUnderline ParseCommandSetUnderline(AttributeHashtable attributes)
        {
            UnderlineType line = attributes.getUnderlineType("value");
            CommandSetUnderline cmd = new CommandSetUnderline(line);
            return cmd;
        }

        private CommandDefineCustomChar ParseCommandDefineCustomChar(AttributeHashtable attributes)
        {
            byte c1 = attributes.getByteValue("startcode");
            byte c2 = attributes.getByteValue("endcode");
            string[] filenames = attributes["filenames"].Split(',');
            Bitmap[] bitmaps = new Bitmap[filenames.Length];
            int bufSize = 0;
            ArrayList bufferList = new ArrayList();
            for (int i = 0; i < bitmaps.Length; i++)
            {
                bitmaps[i] = new Bitmap(filenames[i].Trim());
                byte[] buffer = ImageRasterHelper.ConvertShortBitmap(bitmaps[i], false, false);
                bufferList.Add(buffer);
                bufSize += buffer.Length + 1;
            }
            int idx = 0;
            byte[] raw = new byte[bufSize];
            for (int i = 0; i < bitmaps.Length; i++)
            {
                byte[] buffer = (byte[])bufferList[i];
                raw[idx++] = (byte)bitmaps[i].Width;
                Array.Copy(buffer, 0, raw, idx, buffer.Length);
                idx += buffer.Length;
            }
            CommandDefineCustomChar cmd = new CommandDefineCustomChar(c1, c2, raw);
            return cmd;
        }

        private CommandSetCustomChar ParseCommandSetCustomChar(AttributeHashtable attributes)
        {
            bool flag = attributes.getBoolValue("value");
            CommandSetCustomChar cmd = new CommandSetCustomChar(flag);
            return cmd;
        }
    }
}
