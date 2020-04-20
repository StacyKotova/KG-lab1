using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.ComponentModel;

namespace WindowsFormsApplication1
{
    abstract class Filter
    {
        protected abstract Color CalculateNewPixelColor(Bitmap sourseImage, int x, int y);

        public virtual Bitmap processImage(Bitmap sourceImage, BackgroundWorker worker)
        {
            Bitmap resultImage = new Bitmap(sourceImage.Width, sourceImage.Height);

            for (int i = 0; i < sourceImage.Width; i++)
            {
                for (int j = 0; j < sourceImage.Height; j++)
                {
                    resultImage.SetPixel(i, j, CalculateNewPixelColor(sourceImage, i, j));
                }
                worker.ReportProgress((int)((float)i / resultImage.Width * 100));

                if (worker.CancellationPending)
                    return sourceImage;
            }
            return resultImage;
        }

        public virtual Bitmap processImage(Bitmap sourceImage)
        {
            Bitmap resultImage = new Bitmap(sourceImage.Width, sourceImage.Height);

            for (int i = 0; i < sourceImage.Width; i++)
            {
                for (int j = 0; j < sourceImage.Height; j++)
                {
                    resultImage.SetPixel(i, j, CalculateNewPixelColor(sourceImage, i, j));
                }
            }
            return resultImage;
        }

        public int Clamp(int value, int min, int max)
        {
            if (value < min)
                return min;
            if (value > max)
                return max;
            return value;
        }
    }

    class InvertFilter : Filter
    {
        protected override Color CalculateNewPixelColor(Bitmap sourseImage, int x, int y)
        {
            Color sourseColor = sourseImage.GetPixel(x, y);
            Color resutlColor = Color.FromArgb(255 - sourseColor.R, 255 - sourseColor.G, 255 - sourseColor.B);
            return resutlColor;
        }
    }

    class BrightnessFilter : Filter
    {
        int k = 0;
        public BrightnessFilter(int _k)
        {
            k = _k;
        }
        protected override Color CalculateNewPixelColor(Bitmap sourseImage, int x, int y)
        {
            Color sourseColor = sourseImage.GetPixel(x, y);
            int a = sourseColor.R + k;
            a = Clamp(a, 0, 255);
            int b = sourseColor.G + k;
            b = Clamp(b, 0, 255);
            int c = sourseColor.B + k;
            c = Clamp(c, 0, 255);
            Color resutlColor = Color.FromArgb(a, b, c);
            return resutlColor;
        }
    }

    class GrayScaleFilter : Filter
    {
        protected override Color CalculateNewPixelColor(Bitmap sourseImage, int x, int y)
        {
            Color sourseColor = sourseImage.GetPixel(x, y);
            int intensity = (int)(0.229 * sourseColor.R + 0.578 * sourseColor.G + 0.144 * sourseColor.B);
            Color resutlColor = Color.FromArgb(intensity, intensity, intensity);
            return resutlColor;
        }
    }

    class SepiaFilter : Filter
    {
        protected override Color CalculateNewPixelColor(Bitmap sourseImage, int x, int y)
        {
            int k = 30;
            Color sourseColor = sourseImage.GetPixel(x, y);
            int intensity = (int)(0.229 * sourseColor.R + 0.578 * sourseColor.G + 0.144 * sourseColor.B);
            int a = intensity + 2 * k;
            a = Clamp(a, 0, 255);
            int b = (int)(intensity + 0.5 * k);
            b = Clamp(b, 0, 255);
            int c = intensity - 1 * k;
            c = Clamp(c, 0, 255);
            Color resutlColor = Color.FromArgb(a, b, c);
            return resutlColor;
        }
    }

    class Glass : Filter
    {
        Random rnd;
        public Glass()
        { rnd = new Random(10000); }
        protected override Color CalculateNewPixelColor(Bitmap sourceImage, int k, int l)
        {
            //Random rnd = new Random(10000);
            double r = rnd.NextDouble();
            int x = k + (int)((r - 0.5) * 20);
            r = rnd.NextDouble();
            int y = l + (int)((r - 0.5) * 20);
            if ((x > sourceImage.Width - 1) || (y > sourceImage.Height - 1) || (x < 0) || (y < 0))
                return sourceImage.GetPixel(k, l);
            else
                return sourceImage.GetPixel(x, y);
        }
    }

    class Transfer : Filter
    {
        protected override Color CalculateNewPixelColor(Bitmap sourceImage, int k, int l)
        {
            int x = k + 50;
            if (x > sourceImage.Width - 1)
                return Color.Transparent;
            else
                return sourceImage.GetPixel(x, l);

        }
    }

    class Turn : Filter
    {
        protected override Color CalculateNewPixelColor(Bitmap sourceImage, int k, int l)
        {
            int x0 = 40;
            int y0 = 40;
            int gradus = 30;
            double radian = gradus * Math.PI / 180;
            int x = (int)((k - x0) * Math.Cos(radian) - (l - y0) * Math.Sin(radian) + x0);
            int y = (int)((k - x0) * Math.Sin(radian) + (l - y0) * Math.Cos(radian) + y0);
            if ((x > sourceImage.Width - 1) || (y > sourceImage.Height - 1) || (x < 0) || (y < 0))
                return Color.Transparent;
            else
                return sourceImage.GetPixel(x, y);

        }
    }

    class wave1 : Filter
    {
        protected override Color CalculateNewPixelColor(Bitmap sourceImage, int k, int l)
        {
            double radian = (2.0 * Math.PI * l) / 60.0;
            int x = (int)(k + 20 * Math.Sin(radian));
            if ((x > sourceImage.Width - 1) || (x < 0))
                //return Color.Transparent;
                return sourceImage.GetPixel(k, l);
            else
                return sourceImage.GetPixel(x, l);

        }
    }

    class wave2 : Filter
    {
        protected override Color CalculateNewPixelColor(Bitmap sourceImage, int k, int l)
        {
            double radian = (2.0 * Math.PI * k) / 30.0;
            int x = (int)(k + 20 * Math.Sin(radian));
            if ((x > sourceImage.Width - 1) || (x < 0))
                //return Color.Transparent;
                return sourceImage.GetPixel(k, l);
            else
                return sourceImage.GetPixel(x, l);

        }
    }
}
