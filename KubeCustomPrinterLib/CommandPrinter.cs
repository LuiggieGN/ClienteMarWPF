using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace CustomPrinterLib
{
    public class CommandPrinter
    {
        private byte[] m_Command = null;
        private byte[] m_Params = null;
        private bool m_NeedParam = false;

        public CommandPrinter()
        {
        }

        protected bool NeedParam
        {
            get
            {
                return m_NeedParam;
            }
            set
            {
                m_NeedParam = value;
            }
        }

        protected byte[] Command
        {
            get
            {
                return m_Command;
            }
            set
            {
                m_Command = value;
            }
        }

        protected byte[] Params
        {
            get
            {
                return m_Params;
            }
            set
            {
                m_Params = value;
            }
        }

        public int SizeInByte
        {
            get
            {
                if (m_NeedParam && m_Params == null)
                {
                    throw new Exception("Missed params");
                }
                return m_Command.Length + ((m_Params == null) ? 0 : m_Params.Length);
            }
        }

        public byte[] GetRawData()
        {
            byte[] ret = new byte[SizeInByte];
            Array.Copy(m_Command, 0, ret, 0, m_Command.Length);
            if (m_Params != null)
            {
                Array.Copy(m_Params, 0, ret, m_Command.Length, m_Params.Length);
            }
            return ret;
        }
    }

    class CommandPrintAndFeed : CommandPrinter
    {
        private static byte[] m_CommandPrintAndFeed = new byte[] { 0x1b, 0x64 };

        public CommandPrintAndFeed()
            : base()
        {
            Command = m_CommandPrintAndFeed;
            NeedParam = true;
        }

        public void SetLineNumber(int n)
        {
            byte b = (byte)n;
            Params = new byte[] { b };
        }
    }

    class CommandSetLeftMargin : CommandPrinter
    {
        private static byte[] m_CommandSetLeftMargin = new byte[] { 0x1d, 0x4c };

        public CommandSetLeftMargin()
            : base()
        {
            Command = m_CommandSetLeftMargin;
            NeedParam = true;
        }

        public void SetMargin(int n)
        {
            byte low = (byte)(n & 0x00FF);
            byte high = (byte)((n & 0xFF00) >> 8);
            Params = new byte[] { low, high };
        }
    }

    enum CustomPrinterFontType
    {
        FontLarge = 0, FontSmall = 1
    }

    class CommandSetFont : CommandPrinter
    {
        private static byte[] m_CommandSetFont = new byte[] { 0x1b, 0x4d };

        public CommandSetFont()
            : base()
        {
            Command = m_CommandSetFont;
            NeedParam = true;
        }

        public void SetFont(CustomPrinterFontType font)
        {
            byte b = (byte)font;
            Params = new byte[] { b };
        }
    }

    enum CustomPrinterCPIMode
    {
        CPILess = 0, CPIMore = 1
    }

    class CommandSetCPI : CommandPrinter
    {
        private static byte[] m_CommandSetCPI = new byte[] { 0x1b, 0xc1 };

        public CommandSetCPI()
            : base()
        {
            Command = m_CommandSetCPI;
            NeedParam = true;
        }

        public void SetCPI(CustomPrinterCPIMode cpiMode)
        {
            byte b = (byte)cpiMode;
            Params = new byte[] { b };
        }
    }

    class CommandPrintString : CommandPrinter
    {
        private bool m_AppendNewLine = false;
        private CommandNewLine m_NewLine = new CommandNewLine();

        public CommandPrintString(string text, bool bNewline)
            : base()
        {
            NeedParam = false;
            m_AppendNewLine = bNewline;
            SetText(text);
        }

        public CommandPrintString(string text)
            : this(text, false)
        {
        }

        public bool NewLine
        {
            get
            {
                return m_AppendNewLine;
            }
            set
            {
                m_AppendNewLine = value;
            }
        }

        public static byte[] GetBytes(string s, bool convert)
        {
            if (convert)
            {
                s = StringConverterHelper.Convert(s);
            }
            char[] c = s.ToCharArray();
            byte[] b = new byte[c.Length];
            for(int i = 0; i < c.Length; i++)
            {
                b[i] = (byte)c[i];
            }
            return b;
        }

        public void SetText(string text)
        {
            Command = GetBytes(text, true);
            if (m_AppendNewLine)
            {
                byte[] raw = new byte[SizeInByte + m_NewLine.SizeInByte];
                Array.Copy(GetRawData(), 0, raw, 0, SizeInByte);
                Array.Copy(m_NewLine.GetRawData(), 0, raw, SizeInByte, m_NewLine.SizeInByte);
                Command = raw;
            }
        }

        public void SetText(string text, bool bNewline)
        {
            m_AppendNewLine = bNewline;
            SetText(text);
        }
    }

    class CommandPrintLabel : CommandPrintString
    {
        public CommandPrintLabel(LocalizeLabelMapper htLabels, string labelKey, bool bNewLine)
            : base("")
        {
            NewLine = bNewLine;
            try
            {
                string s = htLabels[labelKey];
                SetText(s);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
                SetText("Label not found");
            }
        }
    }

    class CommandPrintMoney : CommandPrintString
    {
        public CommandPrintMoney(LocalizeLabelMapper htLabels, decimal v, bool bShowCurrency, bool bNewLine)
            : base("")
        {
            string s = getFormatString(htLabels, v, bShowCurrency);
            NewLine = bNewLine;
            SetText(s);
        }

        public static string getFormatString(LocalizeLabelMapper htLabels, decimal v, bool bShowCurrency)
        {
            string s = string.Empty;
            try
            {
                s = (bShowCurrency) ? " " + htLabels["Money"] : string.Empty;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
                s = "Label not found";
            }
            return (v.ToString("#,##0.00") + s);
        }
    }

    class CommandNewLine : CommandPrinter
    {
        private static byte[] m_CommandNewLine = new byte[] { 0x0D, 0x0A };

        public CommandNewLine()
            : base()
        {
            Command = m_CommandNewLine;
        }
    }

    public enum UnderlineType
    {
        Null = 0, Single = 1, Double = 2
    }

    class CommandSetUnderline : CommandPrinter
    {
        private static byte[] m_CommandSetUnderline = new byte[] { 0x1B, 0x2D };

        public CommandSetUnderline(UnderlineType line)
            : base()
        {
            SetUnderline(line);
        }

        public void SetUnderline(UnderlineType line)
        {
            Command = new byte[] { m_CommandSetUnderline[0], m_CommandSetUnderline[1], (byte)line };
        }
    }

    public enum HAlignType
    {
        Left = 0, Center = 1, Rigth = 2
    }

    class CommandSetHAlign : CommandPrinter
    {
        private static byte[] m_CommandSetHAlign = new byte[] { 0x1B, 0x61 };

        public CommandSetHAlign(HAlignType align)
            : base()
        {
            SetAlign(align);
        }

        public void SetAlign(HAlignType align)
        {
            Command = new byte[] { m_CommandSetHAlign[0], m_CommandSetHAlign[1], (byte)align };
        }
    }

    class CommandSetPosition : CommandPrinter
    {
        private static byte[] m_CommandSetPosition = new byte[] { 0x1B, 0x24 };

        public CommandSetPosition(int pos)
            : base()
        {
            SetPosition(pos);
        }

        public void SetPosition(int pos)
        {
            Command = m_CommandSetPosition;
            byte low = (byte)(pos & 0x00FF);
            byte high = (byte)((pos & 0xFF00) >> 8);
            Params = new byte[] { low, high };
        }
    }

    class CommandDefineXYUnit : CommandPrinter
    {
        private static byte[] m_CommandDefineXYUnit = new byte[] { 0x1D, 0x50 };

        public CommandDefineXYUnit()
            : this(0, 0)
        {
        }

        public CommandDefineXYUnit(byte x, byte y)
            : base()
        {
            SetUnit(x, y);
        }

        public void SetUnit(byte x, byte y)
        {
            Command = m_CommandDefineXYUnit;
            Params = new byte[] { x, y };
        }
    }

    class CommandDefineDefualtInterline : CommandPrinter
    {
        private static byte[] m_CommandDefineDefualtInterline = new byte[] { 0x1B, 0x32 };

        public CommandDefineDefualtInterline()
            : base()
        {
            Command = m_CommandDefineDefualtInterline;
        }
    }

    class CommandDefineInterline : CommandPrinter
    {
        private static byte[] m_CommandDefineInterline = new byte[] { 0x1B, 0x33 };

        public CommandDefineInterline()
            : this(64)
        {
        }

        public CommandDefineInterline(byte n)
            : base()
        {
            SetInterline(n);
        }

        public void SetInterline(byte n)
        {
            Command = m_CommandDefineInterline;
            Params = new byte[] { n };
        }
    }

    class CommandPrintAndUnitFeed : CommandPrinter
    {
        private static byte[] m_CommandPrintAndUnitFeed = new byte[] { 0x1B, 0x4A };

        public CommandPrintAndUnitFeed(byte n)
            : base()
        {
            SetFeed(n);
        }

        public void SetFeed(byte n)
        {
            Command = m_CommandPrintAndUnitFeed;
            Params = new byte[] { n };
        }
    }

    class CommandSetCharSize : CommandPrinter
    {
        private static byte[] m_CommandSetCharSize = new byte[] { 0x1D, 0x21 };

        public CommandSetCharSize(byte x, byte y)
            : base()
        {
            SetSize(x, y);
        }

        public void SetSize(byte x, byte y)
        {
            if (x < 1 || x > 8 || y < 1 || y > 8)
            {
                throw new Exception("Invalid char size");
            }
            byte v = (byte)(((x - 1) << 4) | (y - 1));
            Command = m_CommandSetCharSize;
            Params = new byte[] { v };
        }
    }

    class CommandSetTabs : CommandPrinter
    {
        private static byte[] m_CommandSetTabs = new byte[] { 0x1B, 0x44 };

        public CommandSetTabs(byte[] tabs)
            : base()
        {
            SetTabs(tabs);
        }

        public void SetTabs(byte[] tabs)
        {
            Command = m_CommandSetTabs;
            Params = new byte[tabs.Length + 1];
            Array.Copy(tabs, 0, Params, 0, tabs.Length);
            Params[Params.Length - 1] = 0x00;
        }
    }

    public enum BarCodeTextPositionType
    {
        Null = 0, Upper = 1, Down = 2, UpperAndDown = 3
    }

    public enum BarCodeType
    {
        UPC_A = 0, UPC_E = 1, EAN13 = 2, EAN8 = 3, CODE39 = 4, ITF = 5, CODABAR = 6, CODE93 = 7, CODE128 = 8, CODE32 = 20
    }

    class CommandPrintBarcode : CommandPrinter
    {
        private static byte[] m_CommandBarCodeSetFont = new byte[] { 0x1D, 0x66 };
        private static byte[] m_CommandBarCodeSetTextPosition = new byte[] { 0x1D, 0x48 };
        private static byte[] m_CommandBarCodeSetHeight = new byte[] { 0x1D, 0x68 };
        private static byte[] m_CommandBarCodePrint = new byte[] { 0x1D, 0x6B };

        public CommandPrintBarcode(CustomPrinterFontType font, BarCodeTextPositionType position, BarCodeType type, byte[] barcode)
            : base()
        {
            SetAndPrintBarCode(font, position, 162, type, barcode);
        }

        public CommandPrintBarcode(CustomPrinterFontType font, BarCodeTextPositionType position, byte height, BarCodeType type, byte[] barcode)
            : base()
        {
            SetAndPrintBarCode(font, position, height, type, barcode);
        }

        private void SetAndPrintBarCode(CustomPrinterFontType font, BarCodeTextPositionType position, byte height, BarCodeType type, byte[] barcode)
        {
            byte[] buf1 = new byte[] { m_CommandBarCodeSetFont[0], m_CommandBarCodeSetFont[1], (byte)font };
            byte[] buf2 = new byte[] { m_CommandBarCodeSetTextPosition[0], m_CommandBarCodeSetTextPosition[1], (byte)position };
            byte[] buf3 = new byte[] { m_CommandBarCodeSetHeight[0], m_CommandBarCodeSetHeight[1], height };
            Command = new byte[buf1.Length + buf2.Length + buf3.Length + m_CommandBarCodePrint.Length + barcode.Length + 2];
            int idx = 0;
            Array.Copy(buf1, 0, Command, idx, buf1.Length);
            idx += buf1.Length;
            Array.Copy(buf2, 0, Command, idx, buf2.Length);
            idx += buf2.Length;
            Array.Copy(buf3, 0, Command, idx, buf3.Length);
            idx += buf3.Length;
            Array.Copy(m_CommandBarCodePrint, 0, Command, idx, m_CommandBarCodePrint.Length);
            idx += m_CommandBarCodePrint.Length;
            Command[idx++] = (byte)type;
            Array.Copy(barcode, 0, Command, idx, barcode.Length);
            idx += barcode.Length;
            Command[idx] = 0x00;
        }
    }

    class CommandCutPaper : CommandPrinter
    {
        private static byte[] m_CommandCutPaper = new byte[] { 0x1B, 0x69 };

        public CommandCutPaper()
            : base()
        {
            Command = m_CommandCutPaper;
        }
    }

    class CommandAlignPaperToCut : CommandPrinter
    {
        private static byte[] m_CommandAlignPaperToCut = new byte[] { 0x1D, 0xF8 };

        public CommandAlignPaperToCut()
            : base()
        {
            Command = m_CommandAlignPaperToCut;
        }
    }

    class CommandPrintSmallImage : CommandPrinter
    {
        private static byte[] m_CommandPrintImage = new byte[] { 0x1B, 0x2A };

        public CommandPrintSmallImage(byte[] image)
            : base()
        {
            NeedParam = true;
            Command = m_CommandPrintImage;
            SetImage(image);
        }

        public CommandPrintSmallImage()
            : this(null)
        {
        }

        public void SetImage(byte[] image)
        {
            Params = image;
        }
    }

    class CommandDefineImage : CommandPrinter
    {
        private static byte[] m_CommandDefineImage = new byte[] { 0x1D, 0x2A };

        public CommandDefineImage(byte[] imageWithSize)
            : base()
        {
            Command = m_CommandDefineImage;
            Params = imageWithSize;
        }
    }

    public enum PrintModeType
    {
        Normal = 0, DoubleWidth = 1, DoubleHeight = 2, DoubleDouble = 3
    }

    class CommandPrintImageDefined : CommandPrinter
    {
        private static byte[] m_CommandPrintImageDefined = new byte[] { 0x1D, 0x2F };

        public CommandPrintImageDefined(PrintModeType type)
            : base()
        {
            Command = m_CommandPrintImageDefined;
            Params = new byte[] { (byte)type };
        }
    }

    class CommandResetImageDefined : CommandPrinter
    {
        private static byte[] m_CommandResetImageDefined = new byte[] { 0x1D, 0x71 };

        public CommandResetImageDefined()
            : base()
        {
            Command = m_CommandResetImageDefined;
        }
    }

    class CommandReset : CommandPrinter
    {
        private static byte[] m_CommandReset = new byte[] { 0x1B, 0x40 };

        public CommandReset()
            : base()
        {
            Command = m_CommandReset;
        }
    }

    class CommandDrawLine : CommandPrintSmallImage
    {
        public CommandDrawLine(int w)
            : base()
        {
            Bitmap bitmap = new Bitmap(w, 8);
            Graphics graphics = Graphics.FromImage(bitmap);
            graphics.FillRectangle(Brushes.White, 0, 0, w, 8);
            graphics.DrawLine(Pens.Black, 0, 4, w, 4);
            graphics.Dispose();
            Params = ImageRasterHelper.ConvertShortBitmap(bitmap, true, false);
        }
    }

    class CommandDefineCustomChar : CommandPrinter
    {
        private static byte[] m_CommandDefineCustomChar = new byte[] { 0x1B, 0x26, 0x03 };

        public CommandDefineCustomChar(byte startCharCode, byte endCharCode, byte[] charDefinition)
            : base()
        {
            NeedParam = true;
            Command = m_CommandDefineCustomChar;
            Params = new byte[charDefinition.Length + 2];
            Params[0] = startCharCode;
            Params[1] = endCharCode;
            Array.Copy(charDefinition, 0, Params, 2, charDefinition.Length);
        }
    }

    class CommandSetCustomChar : CommandPrinter
    {
        private static byte[] m_CommandSetCustomChar = new byte[] { 0x1B, 0x25 };

        public CommandSetCustomChar(bool flag)
            : base()
        {
            NeedParam = true;
            Command = m_CommandSetCustomChar;
            Params = new byte[]{ (byte)(flag ? 0x01 : 0x00) };
        }
    }

    public class CommandPrinterList : List<CommandPrinter>
    {
    }
}
