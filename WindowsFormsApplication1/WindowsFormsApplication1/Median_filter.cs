using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.ComponentModel;

namespace WindowsFormsApplication1
{
    class Median_filter : Filter
    {

        int rad = 0;
        public Median_filter(int _rad)
        {
            rad = _rad;
        }

        protected override Color CalculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            return Color.FromArgb(0, 0, 0);
        }

        public override Bitmap processImage(Bitmap sourceImage, BackgroundWorker worker)
        {
            Bitmap resultImage = new Bitmap(sourceImage.Width, sourceImage.Height);

            for (int i = rad; i < resultImage.Width - rad; i++)
            {
                worker.ReportProgress((int)((float)i / resultImage.Width * 100));//сигнализация BackgroundWorker о текущем прогрессе
                if (worker.CancellationPending)//если отмена
                    return null;
                for (int j = rad + 1; j < resultImage.Height - rad; j++)
                {
                    median_filter(sourceImage, ref resultImage, i, j, rad);
                }

            }
            return resultImage;
        }
        public void median_filter(Bitmap sourseImage, ref Bitmap resultImage, int x, int y, int rad)
        {

            int n;
            int cR_, cB_, cG_;
            int k = 0;

            n = (2 * rad + 1) * (2 * rad + 1);

            int[] cR = new int[n];
            int[] cB = new int[n];
            int[] cG = new int[n];

            //for (int i = 0; i < n; i++)
            //{
            //    cR[i] = 0;
            //    cG[i] = 0;
            //    cB[i] = 0;
            //}

            for (int i = x - rad; i < x + rad + 1; i++)
            {
                for (int j = y - rad; j < y + rad + 1; j++)
                {
                    {
                        System.Drawing.Color c = sourseImage.GetPixel(i, j);
                        cR[k] = System.Convert.ToInt32(c.R);
                        cG[k] = System.Convert.ToInt32(c.G);
                        cB[k] = System.Convert.ToInt32(c.B);
                        k++;

                    }
                }
            }

            Array.Sort(cR);
            Array.Sort(cG);
            Array.Sort(cB);

            int n_ = (int)((double)n / 2.0);

            cR_ = cR[n_];
            cG_ = cG[n_];
            cB_ = cB[n_];

            for (int i = x - rad; i < x + rad + 1; i++)
            {
                for (int j = y - rad; j < y + rad + 1; j++)
                {
                    resultImage.SetPixel(i, j, System.Drawing.Color.FromArgb(cR_, cG_, cB_));
                }
            }

        }
    }
}
