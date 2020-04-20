using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace WindowsFormsApplication1
{
    class Operator_Pruitta : Filter
    {
        protected float[,] kernel_Gx = null;
        protected float[,] kernel_Gy = null;

        public Operator_Pruitta()
        {
            kernel_Gy = new float[3, 3] { { -1, -1, -1 }, { 0, 0, 0 }, { 1, 1, 1 } };
            kernel_Gx = new float[3, 3] { { -1, 0, 1 }, { -1, 0, 1 }, { -1, 0, 1 } };
        }
        public Operator_Pruitta(float[,] kernel_1, float[,] kernel_2)
        {
            this.kernel_Gx = kernel_1;
            this.kernel_Gy = kernel_2;
        }
        protected Color _calculateNewPixelColor(Bitmap sourceImage, int x, int y, bool flag)
        {
            float[,] kernel = null;
            if (flag)
                kernel = kernel_Gx;
            else
                kernel = kernel_Gy;
            int radiusX = kernel.GetLength(0) / 2;//ширина/2
            int radiusY = kernel.GetLength(1) / 2;//высота/2
            float resultR = 0;
            float resultB = 0;
            float resultG = 0;
            for (int l = -radiusY; l <= radiusY; l++)
                for (int k = -radiusX; k <= radiusX; k++)
                {
                    int idX = Clamp(x + k, 0, sourceImage.Width - 1);
                    int idY = Clamp(y + l, 0, sourceImage.Height - 1);
                    Color neighborColor = sourceImage.GetPixel(idX, idY);
                    resultR += neighborColor.R * kernel[k + radiusX, l + radiusY];
                    resultG += neighborColor.G * kernel[k + radiusX, l + radiusY];
                    resultB += neighborColor.B * kernel[k + radiusX, l + radiusY];
                }
            return Color.FromArgb(
                Clamp((int)resultR, 0, 255),
                Clamp((int)resultG, 0, 255),
                Clamp((int)resultB, 0, 255)
                );
        }
        protected override Color CalculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            Color neighborColor_x = _calculateNewPixelColor(sourceImage, x, y, true);
            Color neighborColor_y = _calculateNewPixelColor(sourceImage, x, y, false);
            double resultR = Math.Sqrt(Math.Pow(neighborColor_x.R, 2) + Math.Pow(neighborColor_y.R, 2));
            double resultG = Math.Sqrt(Math.Pow(neighborColor_x.G, 2) + Math.Pow(neighborColor_y.G, 2));
            double resultB = Math.Sqrt(Math.Pow(neighborColor_x.B, 2) + Math.Pow(neighborColor_y.B, 2));
            return Color.FromArgb(
                Clamp((int)resultR, 0, 255),
                Clamp((int)resultG, 0, 255),
                Clamp((int)resultB, 0, 255)
                );
        }
    }
}

