using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace CustomPrinterLib
{
    public class ImageRasterHelper
    {
        private static bool RGBEgual(Color c1, Color c2)
        {
            return (c1.R == c2.R && c1.G == c2.G && c1.B == c2.B);
        }

        private static bool RGBGreatEgual(Color c1, int R, int G, int B)
        {
            return (c1.R >= R && c1.G >= G && c1.B >= B);
        }

        /// <summary>
        /// Convert a Bitmap in a raster for max 24 pixels in height.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static byte[] ConvertShortBitmap(Bitmap bitmap, bool bIncludeSize, bool bDoubleDensity)
        {
            int ySize = (bitmap.Height <= 8) ? 1 : 3;
            int baseIndex = ((bIncludeSize) ? 3 : 0);
            int nBytes = ySize * bitmap.Width;
            byte[] raw = new byte[nBytes + baseIndex];
            for (int i = 0; i < raw.Length; raw[i++] = 0) ;
            if (bIncludeSize)
            {
                raw[0] = (byte)((ySize == 1) ? ((!bDoubleDensity) ? 0 : 1) : ((!bDoubleDensity) ? 32 : 33));
                raw[1] = (byte)(bitmap.Width & 0x00FF);
                raw[2] = (byte)((bitmap.Width & 0xFF00) >> 8);
            }
            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    if (y == 24)
                    {
                        break;
                    }
                    Color color = bitmap.GetPixel(x, y);
                    if (RGBGreatEgual(color, 255, 255, 128))
                    {
                        continue;
                    }
                    int idx = (ySize * x) + y / 8;
                    byte mask = (byte)(0x80 >> (y % 8));
                    raw[idx + baseIndex] |= mask;
                }
            }
            return raw;
        }

        /// <summary>
        /// Convert a Bitmap in a raster for max ... pixels.
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="bIncludeSize"></param>
        /// <returns></returns>
        public static byte[] ConvertBitmap(Bitmap bitmap, bool bIncludeSize)
        {
            int baseIndex = ((bIncludeSize) ? 2 : 0);
            int xSize = (bitmap.Width / 8);
            if (xSize * 8 != bitmap.Width)
            {
                xSize++;
            }
            int ySize = (bitmap.Height / 8);
            if (ySize * 8 != bitmap.Height)
            {
                ySize++;
            }
            if (xSize < 1 || xSize > 255 || ySize < 1 || ySize > 48 || xSize * ySize > 1536)
            {
                throw new Exception("Incorrect size");
            }
            byte[] raw = new byte[xSize * ySize * 8 + ((bIncludeSize) ? 2 : 0)];
            for (int i = 0; i < raw.Length; raw[i++] = 0) ;

            if (bIncludeSize)
            {
                raw[0] = (byte)(xSize & 0x00FF);
                raw[1] = (byte)(ySize & 0x00FF);
            }
            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    Color color = bitmap.GetPixel(x, y);
                    if (RGBGreatEgual(color, 255, 255, 128))
                    {
                        continue;
                    }
                    int idx = (ySize * x) + y / 8;
                    byte mask = (byte)(0x80 >> (y % 8));
                    raw[idx + baseIndex] |= mask;
                }
            }
            return raw;
        }
    }
}
