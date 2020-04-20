using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.ComponentModel;

namespace WindowsFormsApplication1
{
    class Histogram_linear_stretch : Filter
    {
        Color Color_max, Color_min;
        protected override Color CalculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            Color sourceColor = sourceImage.GetPixel(x, y);
            Color resultColor = Color.FromArgb(Clamp((int)(((double)(sourceColor.R - Color_min.R) / (double)(Color_max.R - Color_min.R)) * 255), 0, 255),
                                               Clamp((int)(((double)(sourceColor.G - Color_min.G) / (double)(Color_max.G - Color_min.G)) * 255), 0, 255),
                                               Clamp((int)(((double)(sourceColor.B - Color_min.B) / (double)(Color_max.B - Color_min.B)) * 255), 0, 255));
            return resultColor;
        }
        public override Bitmap processImage(Bitmap sourceImage, BackgroundWorker worker)
        {
            Max_Color(sourceImage);
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

        protected void Max_Color(Bitmap sourceImage)
        {
            int R_max = 0, G_max = 0, B_max = 0, R_min = 255, G_min = 255, B_min = 255;
            for (int i = 0; i < sourceImage.Width; i++)
            {
                for (int j = 0; j < sourceImage.Height; j++)
                {
                    if (sourceImage.GetPixel(i, j).R > R_max)
                        R_max = sourceImage.GetPixel(i, j).R;
                    if (sourceImage.GetPixel(i, j).R < R_min)
                        R_min = sourceImage.GetPixel(i, j).R;

                    if (sourceImage.GetPixel(i, j).G > G_max)
                        G_max = sourceImage.GetPixel(i, j).G;
                    if (sourceImage.GetPixel(i, j).G < G_min)
                        G_min = sourceImage.GetPixel(i, j).G;

                    if (sourceImage.GetPixel(i, j).B > B_max)
                        B_max = sourceImage.GetPixel(i, j).B;
                    if (sourceImage.GetPixel(i, j).B < B_min)
                        B_min = sourceImage.GetPixel(i, j).B;
                }
            }

            Color_max = Color.FromArgb(R_max, G_max, B_max);
            Color_min = Color.FromArgb(R_min, G_min, B_min);
        }
    }
}
