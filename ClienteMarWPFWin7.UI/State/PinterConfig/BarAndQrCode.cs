using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace ClienteMarWPFWin7.UI.State.PinterConfig
{
    public class BarAndQrCode
    {

        public static Image GetBarCode(string barcodeValue)
        {
            Zen.Barcode.Code128BarcodeDraw barCode = Zen.Barcode.BarcodeDrawFactory.Code128WithChecksum;
            return barCode.Draw(barcodeValue, 50);
        }


        public static Image GetQrCode(string qrcodeValue)
        {
            Zen.Barcode.CodeQrBarcodeDraw qrCode = Zen.Barcode.BarcodeDrawFactory.CodeQr;
            return qrCode.Draw(qrcodeValue, 50);
        }


    }

    public enum TypeGenerateCode
    {
        QrCode = 1,
        BarCode = 2
    }
}
