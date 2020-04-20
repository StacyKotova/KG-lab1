using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.ComponentModel;

namespace WindowsFormsApplication1
{
    
    class Dilation : MatrixFilter
    {
            public Dilation()
            {
                kernel = new float[3, 3];
                kernel[0, 0] = 0.0f; kernel[0, 1] = 1.0f; kernel[0, 2] = 0.0f;
                kernel[1, 0] = 1.0f; kernel[1, 1] = 1.0f; kernel[1, 2] = 1.0f;
                kernel[2, 0] = 0.0f; kernel[2, 1] = 1.0f; kernel[2, 2] = 0.0f;
            }

        public Dilation(float[,] kernel)
        {
            this.kernel = kernel;
        }

            protected override System.Drawing.Color CalculateNewPixelColor(System.Drawing.Bitmap sourceImage, int x, int y)
            {
                // определяем радиус действия фильтра по оси X
                int radiusX = kernel.GetLength(0) / 2;

                // определяем радиус действия фильтра по оси Y
                int radiusY = kernel.GetLength(1) / 2;

                Color resultColor = Color.Black;

                byte max = 0;
                for (int l = -radiusY; l <= radiusY; l++)
                    for (int k = -radiusX; k <= radiusX; k++)
                    {
                        int idX = Clamp(x + k, 0, sourceImage.Width - 1);
                        int idY = Clamp(y + l, 0, sourceImage.Height - 1);
                        Color color = sourceImage.GetPixel(idX, idY);
                        int intensity = color.R;
                        if (color.R != color.G || color.R != color.B || color.G != color.B)
                        {
                            intensity = (int)(0.36 * color.R + 0.53 * color.G + 0.11 * color.R);
                        }
                        if (kernel[k + radiusX, l + radiusY] > 0 && intensity > max)
                        {
                            max = (byte)intensity;
                            resultColor = color;
                        }
                    }
                return resultColor;
            }
        }

        class Erosion : MatrixFilter
    {
            public Erosion()
            {
                kernel = new float[3, 3];
                kernel[0, 0] = 0.0f; kernel[0, 1] = 1.0f; kernel[0, 2] = 0.0f;
                kernel[1, 0] = 1.0f; kernel[1, 1] = 1.0f; kernel[1, 2] = 1.0f;
                kernel[2, 0] = 0.0f; kernel[2, 1] = 1.0f; kernel[2, 2] = 0.0f;
            }

        public Erosion(float[,] kernel)
        {
            this.kernel = kernel;
        }

            protected override System.Drawing.Color CalculateNewPixelColor(System.Drawing.Bitmap sourceImage, int x, int y)
            {
                // определяем радиус действия фильтра по оси X
                int radiusX = kernel.GetLength(0) / 2;

                // определяем радиус действия фильтра по оси Y
                int radiusY = kernel.GetLength(1) / 2;

                Color resultColor = Color.White;

                byte min = 255;
                for (int l = -radiusY; l <= radiusY; l++)
                    for (int k = -radiusX; k <= radiusX; k++)
                    {
                        int idX = Clamp(x + k, 0, sourceImage.Width - 1);
                        int idY = Clamp(y + l, 0, sourceImage.Height - 1);
                        Color color = sourceImage.GetPixel(idX, idY);
                        int intensity = color.R;
                        if (color.R != color.G || color.R != color.B || color.G != color.B)
                        {
                            intensity = (int)(0.36 * color.R + 0.53 * color.G + 0.11 * color.R);
                        }
                        if (kernel[k + radiusX, l + radiusY] > 0 && intensity < min)
                        {
                            min = (byte)intensity;
                            resultColor = color;
                        }
                    }
                return resultColor;
            }
        }

        class Opening : MatrixFilter
    {
            public Opening()
            {
                this.kernel = null;
            }

        public Opening(float[,] kernel)
        {
            this.kernel = kernel;
        }

            public override Bitmap processImage(Bitmap sourceImage, BackgroundWorker worker)
            {
                Dilation dilation;
                Erosion erosion;
                if (kernel != null)
                {
                    dilation = new Dilation(this.kernel);
                    erosion = new Erosion(this.kernel);
                }
                else
                {
                    dilation = new Dilation();
                    erosion = new Erosion();
                }
                return dilation.processImage(erosion.processImage(sourceImage, worker), worker);
            }
            protected override Color CalculateNewPixelColor(Bitmap sourceImage, int x, int y)
            {
                throw new NotImplementedException();
            }
        }

        class Closing : MatrixFilter
    {
            public Closing()
            {
                this.kernel = null;
            }

        public Closing(float[,] kernel)
        {
            this.kernel = kernel;
        }

            public override Bitmap processImage(Bitmap sourceImage, BackgroundWorker worker)
            {
                Dilation dilation;
                Erosion erosion;
                if (kernel != null)
                {
                    dilation = new Dilation(this.kernel);
                    erosion = new Erosion(this.kernel);
                }
                else
                {
                    dilation = new Dilation();
                    erosion = new Erosion();
                }
                return erosion.processImage(dilation.processImage(sourceImage, worker), worker);
            }
            protected override Color CalculateNewPixelColor(Bitmap sourceImage, int x, int y)
            {
                throw new NotImplementedException();
            }
        }

        class Grad : Filter
        {
            public override Bitmap processImage(Bitmap sourceImage, BackgroundWorker worker)
            {
                Dilation dilation = new Dilation();
                Erosion erosion = new Erosion();
                Subtraction subtraction = new Subtraction(dilation.processImage(sourceImage, worker));
                return subtraction.processImage(erosion.processImage(sourceImage, worker), worker);
            }
            protected override Color CalculateNewPixelColor(Bitmap sourceImage, int x, int y)
            {
                throw new NotImplementedException();
            }
        }

        class TopHat : MatrixFilter
    {
            public TopHat()
            {
                this.kernel = null;
            }

        public TopHat(float[,] kernel)
        {
            this.kernel = kernel;
        }

            public override Bitmap processImage(Bitmap sourceImage, BackgroundWorker worker)
            {
                Opening opening;
                if (this.kernel == null)
                {
                    opening = new Opening();
                }
                else
                {
                    opening = new Opening(this.kernel);
                }
                Subtraction subtraction = new Subtraction(sourceImage);
                return subtraction.processImage(opening.processImage(sourceImage, worker), worker);
            }
            protected override Color CalculateNewPixelColor(Bitmap sourceImage, int x, int y)
            {
                throw new NotImplementedException();
            }
        }

        class BlackHat : MatrixFilter
    {
            public BlackHat()
            {
                this.kernel = null;
            }

        public BlackHat(float[,] kernel)
        {
            this.kernel = kernel;
        }

            public override Bitmap processImage(Bitmap sourceImage, BackgroundWorker worker)
            {
                Closing closing;
                if (this.kernel == null)
                {
                    closing = new Closing();
                }
                else
                {
                    closing = new Closing(this.kernel);
                }
                Subtraction subtraction = new Subtraction(closing.processImage(sourceImage, worker));
                return subtraction.processImage(sourceImage, worker);
            }
            protected override Color CalculateNewPixelColor(Bitmap sourceImage, int x, int y)
            {
                throw new NotImplementedException();
            }
        }

        class Subtraction : Filter
        {
            Bitmap mMinuedImage = null;
            public Subtraction(Bitmap minuendImage)
            {
                mMinuedImage = minuendImage;
            }
            protected override Color CalculateNewPixelColor(Bitmap sourceImage, int x, int y)
            {
                Color minuedColor = mMinuedImage.GetPixel(x, y);
                Color subtrahendColor = sourceImage.GetPixel(x, y);
                return Color.FromArgb(Clamp(minuedColor.R - subtrahendColor.R, 0, 255),
                                      Clamp(minuedColor.G - subtrahendColor.G, 0, 255),
                                      Clamp(minuedColor.B - subtrahendColor.B, 0, 255));
            }
        }
 }