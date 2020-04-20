using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.ComponentModel;

namespace WindowsFormsApplication1
{
    class Autolevels : Filter
    {
        private byte maxR = 0;
        private byte maxG = 0;
        private byte maxB = 0;

        private byte minR = 255;
        private byte minG = 255;
        private byte minB = 255;

        public override Bitmap processImage(Bitmap sourceImage, BackgroundWorker worker)
        {
            for (int x = 0; x < sourceImage.Width; x++)
                for (int y = 0; y < sourceImage.Height; y++)
                {
                    Color c = sourceImage.GetPixel(x, y);
                    maxR = Math.Max(maxR, c.R);
                    maxG = Math.Max(maxG, c.G);
                    maxB = Math.Max(maxB, c.B);

                    minR = Math.Min(minR, c.R);
                    minG = Math.Min(minG, c.G);
                    minB = Math.Min(minB, c.B);
                }
            return base.processImage(sourceImage, worker);
        }

        protected override Color CalculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            Color sourceColor = sourceImage.GetPixel(x, y);
            int lengthR = maxR - minR > 0 ? maxR - minR : 1;
            int lengthG = maxG - minG > 0 ? maxG - minG : 1;
            int lengthB = maxB - minB > 0 ? maxB - minB : 1;
            byte r = (byte)((sourceColor.R - minR) * 255 / lengthR);
            byte g = (byte)((sourceColor.G - minG) * 255 / lengthG);
            byte b = (byte)((sourceColor.B - minB) * 255 / lengthB);
            return Color.FromArgb(r, g, b);
        }
    }

    /*class GrayWorld: Filter
    {
        //private byte mR = 0;
        //private byte mG = 0;
        //private byte mB = 0;
        //private byte arg = 0;
        int mR = 0;
        int mG = 0;
        int mB = 0;

        double arg = 0;
        int N = 0;

        double MR = 0;
        double MG = 0;
        double MB = 0;

        public override Bitmap processImage(Bitmap sourceImage, BackgroundWorker worker)
        {
            for (int x = 0; x < sourceImage.Width; x++)
                for (int y = 0; y < sourceImage.Height; y++)
                {
                    Color c = sourceImage.GetPixel(x, y);
                    mR = mR + c.R;
                    mG = mG + c.G;
                    mB = mB + c.B;
                    N = N + 1;
                }
            MR = (double)mR / (double)N;
            MG = (double)mG / (double)N;
            MB = (double)mB / (double)N;

            arg = (MR + MG + MB) / 3.0;
            return base.processImage(sourceImage, worker);
        }

        protected override Color CalculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            Color sourceColor = sourceImage.GetPixel(x, y);
            byte r = (byte)((sourceColor.R * arg) / (int)MR);
            byte g = (byte)((sourceColor.G * arg) / (int)MG);
            byte b = (byte)((sourceColor.B * arg) / (int)MB);
            return Color.FromArgb(r, g, b);
        }
    }*/

    class Perfect_reflector : Filter
    {
        Color Color_max;
        protected override Color CalculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            Color sourceColor = sourceImage.GetPixel(x, y);
            Color resultColor = Color.FromArgb(Clamp((int)(((double)sourceColor.R / Color_max.R) * 255), 0, 255),
                                               Clamp((int)(((double)sourceColor.G / Color_max.G) * 255), 0, 255),
                                               Clamp((int)(((double)sourceColor.B / Color_max.B) * 255), 0, 255));
            return resultColor;
        }
        public override Bitmap processImage(Bitmap sourceImage, BackgroundWorker worker)
        {
            Color_max = Max_Color(sourceImage);
            Bitmap resultImage = new Bitmap(sourceImage.Width, sourceImage.Height);
            for (int i = 0; i < sourceImage.Width; i++)
            {
                worker.ReportProgress((int)((float)i / resultImage.Width * 100));//сигнализация BackgroundWorker о текущем прогрессе
                if (worker.CancellationPending)//если отмена
                    return null;
                for (int j = 0; j < sourceImage.Height; j++)
                {
                    resultImage.SetPixel(i, j, CalculateNewPixelColor(sourceImage, i, j));
                }
            }
            return resultImage;
        }

        protected Color Max_Color(Bitmap sourceImage)
        {
            int R_max = 0, G_max = 0, B_max = 0;
            for (int i = 0; i < sourceImage.Width; i++)
            {
                for (int j = 0; j < sourceImage.Height; j++)
                {
                    if (sourceImage.GetPixel(i, j).R > R_max)
                        R_max = sourceImage.GetPixel(i, j).R;
                    if (sourceImage.GetPixel(i, j).G > G_max)
                        G_max = sourceImage.GetPixel(i, j).G;
                    if (sourceImage.GetPixel(i, j).B > B_max)
                        B_max = sourceImage.GetPixel(i, j).B;
                }
            }

            Color resultColor = Color.FromArgb(R_max, G_max, B_max);
            return resultColor;
        }
    }
}
