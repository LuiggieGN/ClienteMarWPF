using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Collections;
using System.IO.Ports;

namespace CustomPrinterLib
{
    public enum PrinterPaperStatusType
    {
        Paper = 0, NoPaper = 1, PoorPaper = 2
    }

    public enum PrinterStatusType
    {
        OK = 0, PrintBreak = 1, Error = 2
    }

    public class CustomPrinterEngine
    {
        private string m_PrinterName = null;
        private object m_DataToPrint = null;
        private bool m_IsPrinted = false;
        private string m_Portname;
        private int m_BaudRate;
        private Parity m_Parity;
        private int m_DataBits;
        private StopBits m_StopBits;
        private bool bRs232 = false;

        public CustomPrinterEngine(string priterName)
        {
            m_PrinterName = priterName;
            bRs232 = false;
        }

        public CustomPrinterEngine(string portname, int baudRate, Parity parity, int dataBits, StopBits stopBits)
        {
            m_Portname = portname;
            m_BaudRate = baudRate;
            m_Parity = parity;
            m_DataBits = dataBits;
            m_StopBits = stopBits;
            bRs232 = true;
        }

        public object DataToPrint
        {
            get
            {
                return m_DataToPrint;
            }
            set
            {
                m_DataToPrint = value;
                m_IsPrinted = false;
            }
        }

        public bool IsPrinted
        {
            get
            {
                return m_IsPrinted;
            }
        }

        private PrinterPaperStatusType CheckPaperStatus(byte[] buffer)
        {
            if (buffer.Length == 0)
            {
                return PrinterPaperStatusType.Paper;
            }
            PrinterPaperStatusType status = PrinterPaperStatusType.Paper;
            if (buffer[0] != 0x10)
            {
                throw new Exception("Error in read pinter status");
            }
            if (buffer[1] != 0x0F)
            {
                throw new Exception("Error in read pinter status");
            }
            bool flag = (buffer[2] & 0x01) == 0x01;
            if (!flag)
            {
                status = PrinterPaperStatusType.Paper;
            }
            else
            {
                status = PrinterPaperStatusType.NoPaper;
            }
            if (!flag)
            {
                flag = (buffer[2] & 0x04) == 0x04;
                if (flag)
                {
                    status = PrinterPaperStatusType.PoorPaper;
                }
            }
            return status;
        }

        private PrinterPaperStatusType getPaperStatus(SerialPort serial)
        {
            int n = 0;
            // Query printer
            byte[] b = new byte[] { 0x10, 0x04, 0x14 };
            serial.Write(b, 0, b.Length);
            while ((n = serial.BytesToRead) != 6)
            {
                System.Threading.Thread.Sleep(5);
            }
            b = new byte[n];
            serial.Read(b, 0, n);
            return CheckPaperStatus(b);
        }

        public PrinterPaperStatusType getPaperStatus()
        {
            if (!bRs232)
            {
                throw new Exception("This command works only via RS232");
            }
            SerialPort serial = null;
            try
            {
                serial = new SerialPort(m_Portname, m_BaudRate, m_Parity, m_DataBits, m_StopBits);
                serial.Open();
                return getPaperStatus(serial);
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine(ex);
                throw ex;
            }
            finally
            {
                if (serial != null)
                {
                    serial.Close();
                    serial.Dispose();
                    serial = null;
                }
            }
        }

        private PrinterStatusType CheckStatus(byte[] buffer)
        {
            if (buffer.Length == 0)
            {
                return PrinterStatusType.OK;
            }
            PrinterStatusType status = PrinterStatusType.OK;
            if ((buffer[0] & 0x20) == 0x20)
            {
                status = PrinterStatusType.PrintBreak;
            }
            else if ((buffer[0] & 0x40) == 0x40)
            {
                status = PrinterStatusType.Error;
            }
                return status;
        }

        private PrinterStatusType getPrintStatus(SerialPort serial)
        {
            int n = 0;
            // Query printer
            byte[] b = new byte[] { 0x10, 0x04, 0x2 };
            serial.Write(b, 0, b.Length);
            while ((n = serial.BytesToRead) != 1)
            {
                System.Threading.Thread.Sleep(5);
            }
            b = new byte[n];
            serial.Read(b, 0, n);
            return CheckStatus(b);
        }

        public PrinterStatusType getPrintStatus()
        {
            if (!bRs232)
            {
                throw new Exception("This command works only via RS232");
            }
            SerialPort serial = null;
            try
            {
                serial = new SerialPort(m_Portname, m_BaudRate, m_Parity, m_DataBits, m_StopBits);
                serial.Open();
                return getPrintStatus(serial);
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine(ex);
                throw ex;
            }
            finally
            {
                if (serial != null)
                {
                    serial.Close();
                    serial.Dispose();
                    serial = null;
                }
            }
        }

        public void Print(TemplateManager templateManager, string languageKey, string templateKey)
        {
            Print(templateManager, languageKey, templateKey, 0);
        }

        public void Print(TemplateManager templateManager, string languageKey, string templateKey, int msPrintWait)
        {
            if (m_DataToPrint == null)
            {
                throw new Exception("Nothing to print!");
            }
            // Generate command list for the template and transforme command list in byte sequence
            byte[] buffer = templateManager.GenerateRawData(languageKey, templateKey, m_DataToPrint);
            // Sending buffer to printer
            if (bRs232)
            {
                // Send via RS232
                SerialPort serial = null;
                try
                {
                    serial = new SerialPort(m_Portname, m_BaudRate, m_Parity, m_DataBits, m_StopBits);
                    serial.Open();
                    // Check for paper and print
                    if (getPaperStatus(serial) != PrinterPaperStatusType.NoPaper)
                    {
                        // Send via RS232
                        serial.Write(buffer, 0, buffer.Length);
                        int a = serial.BytesToRead;
                        // Wait for print
                        System.Threading.Thread.Sleep(msPrintWait);
                        // Query printer
                        if(getPrintStatus(serial) == PrinterStatusType.OK)
                        {
                            // Set printed flag
                            m_IsPrinted = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Out.WriteLine(ex);
                    throw ex;
                }
                finally
                {
                    if (serial != null)
                    {
                        serial.Close();
                        serial.Dispose();
                        serial = null;
                    }
                }
            }
            else
            {
                // Send via USB
                IntPtr ptr = Marshal.AllocHGlobal(buffer.Length);
                Marshal.Copy(buffer, 0, ptr, buffer.Length);
                RawPrinterHelper.SendBytesToPrinter(m_PrinterName, ptr, buffer.Length);
                Marshal.FreeHGlobal(ptr);
                // Set printed flag
                m_IsPrinted = true;
            }
        }
    }
}
