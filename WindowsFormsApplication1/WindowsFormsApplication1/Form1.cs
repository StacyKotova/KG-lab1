using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{

    public partial class Form1 : Form
    {
        int rad;
        Bitmap image;
        List<Bitmap> ListBm = new List<Bitmap>();
        public float[,] kernel = new float[3, 3];

        public Form1()
        {
            InitializeComponent();

            kernel[0, 0] = 0.0f; kernel[0, 1] = 1.0f; kernel[0, 2] = 0.0f;
            kernel[1, 0] = 1.0f; kernel[1, 1] = 1.0f; kernel[1, 2] = 1.0f;
            kernel[2, 0] = 0.0f; kernel[2, 1] = 1.0f; kernel[2, 2] = 0.0f;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Image files|*.png;*.jpg;*bmp|All files(*.*)|*.*";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                image = new Bitmap(dialog.FileName);
            }
            pictureBox1.Image = image;
            pictureBox1.Refresh();
        }

        private void инверсияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (image != null) {
                InvertFilter filter = new InvertFilter();
                /*Bitmap resultImage = filter.processImage(image);
                pictureBox1.Image = resultImage;
                pictureBox1.Refresh();*/
                backgroundWorker1.RunWorkerAsync(filter);
                ListBm.Add(image);
            }
              
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            Bitmap newImage = ((Filter)e.Argument).processImage(image, backgroundWorker1);
            if (backgroundWorker1.CancellationPending != true)
                image = newImage;
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!e.Cancelled)
            {
                pictureBox1.Image = image;
                pictureBox1.Refresh();
            }
            progressBar1.Value = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            backgroundWorker1.CancelAsync();
        }

        private void полутоновоеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (image != null)
            {
                GrayScaleFilter filter = new GrayScaleFilter();
                backgroundWorker1.RunWorkerAsync(filter);
                ListBm.Add(image);
            }
        }

        private void сепияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (image != null)
            {
                SepiaFilter filter = new SepiaFilter();
                backgroundWorker1.RunWorkerAsync(filter);
                ListBm.Add(image);
            }
        }

        private void яркостьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (image != null)
            {
                Form4 form4 = new Form4();
                form4.ShowDialog();
                int k = System.Convert.ToInt32(form4.numericUpDown1.Value);
                BrightnessFilter filter = new BrightnessFilter(k);
                backgroundWorker1.RunWorkerAsync(filter);
                ListBm.Add(image);
            }
        }

        private void размытиеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (image != null)
            {
                BlurFilter filter = new BlurFilter();
                backgroundWorker1.RunWorkerAsync(filter);
                ListBm.Add(image);
            }
        }

        private void размытиеПоГауссуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (image != null)
            {
                GaussFilter filter = new GaussFilter();
                backgroundWorker1.RunWorkerAsync(filter);
                ListBm.Add(image);
            }
        }

        private void резкостьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (image != null)
            {
                Sharpness1Filter filter = new Sharpness1Filter();
                backgroundWorker1.RunWorkerAsync(filter);
                ListBm.Add(image);
            }
        }

        private void фильтСобеляToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (image != null)
            {
                SobelFilter filter = new SobelFilter();
                backgroundWorker1.RunWorkerAsync(filter);
                ListBm.Add(image);
            }
        }

        private void резкость2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (image != null)
            {
                Sharpness2Filter filter = new Sharpness2Filter();
                backgroundWorker1.RunWorkerAsync(filter);
                ListBm.Add(image);
            }
        }

        private void dilationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (image != null)
            {
                Dilation filter = new Dilation(kernel);
                backgroundWorker1.RunWorkerAsync(filter);
                ListBm.Add(image);
            }
        }

        private void erosionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (image != null)
            {
                Erosion filter = new Erosion(kernel);
                backgroundWorker1.RunWorkerAsync(filter);
                ListBm.Add(image);
            }
        }

        private void openingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (image != null)
            {
                Opening filter = new Opening(kernel);
                backgroundWorker1.RunWorkerAsync(filter);
                ListBm.Add(image);
            }
        }

        private void closingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (image != null)
            {
                Closing filter = new Closing(kernel);
                backgroundWorker1.RunWorkerAsync(filter);
                ListBm.Add(image);
            }
        }

        private void topHatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (image != null)
            {
                TopHat filter = new TopHat(kernel);
                backgroundWorker1.RunWorkerAsync(filter);
                ListBm.Add(image);
            }
        }

        private void blackHatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (image != null)
            {
                BlackHat filter = new BlackHat(kernel);
                backgroundWorker1.RunWorkerAsync(filter);
                ListBm.Add(image);
            }
        }

        private void gradToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (image != null)
            {
                Grad filter = new Grad();
                backgroundWorker1.RunWorkerAsync(filter);
                ListBm.Add(image);
            }
        }

        /*private void серыйМирToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (image != null)
            {
                GrayWorld filter = new GrayWorld();
                backgroundWorker1.RunWorkerAsync(filter);
                ListBm.Add(image);
            }
        }*/

        private void автоуровниToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (image != null)
            {
                Autolevels filter = new Autolevels();
                backgroundWorker1.RunWorkerAsync(filter);
                ListBm.Add(image);
            }
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null) //если в pictureBox есть изображение
            {
                //создание диалогового окна "Сохранить как..", для сохранения изображения
                SaveFileDialog savedialog = new SaveFileDialog();
                savedialog.Title = "Сохранить картинку как...";
                //отображать ли предупреждение, если пользователь указывает имя уже существующего файла
                savedialog.OverwritePrompt = true;
                //отображать ли предупреждение, если пользователь указывает несуществующий путь
                savedialog.CheckPathExists = true;
                //список форматов файла, отображаемый в поле "Тип файла"
                savedialog.Filter = "Image Files(*.BMP)|*.BMP|Image Files(*.JPG)|*.JPG|Image Files(*.GIF)|*.GIF|Image Files(*.PNG)|*.PNG|All files (*.*)|*.*";
                //отображается ли кнопка "Справка" в диалоговом окне
                savedialog.ShowHelp = true;
                if (savedialog.ShowDialog() == DialogResult.OK) //если в диалоговом окне нажата кнопка "ОК"
                {
                    try
                    {
                        image.Save(savedialog.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                    }
                    catch
                    {
                        MessageBox.Show("Невозможно сохранить изображение", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void закрытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void эффектСтеклаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (image != null)
            {
                Glass filter = new Glass();
                backgroundWorker1.RunWorkerAsync(filter);
                ListBm.Add(image);
            }
        }

        private void операторЩарраToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (image != null)
            {
                Operator_Shcharra filter = new Operator_Shcharra();
                backgroundWorker1.RunWorkerAsync(filter);
                ListBm.Add(image);
            }
        }

        private void переносToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (image != null)
            {
                Transfer filter = new Transfer();
                backgroundWorker1.RunWorkerAsync(filter);
                ListBm.Add(image);
            }
        }

        private void идеальныйОтражательToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (image != null)
            {
                Perfect_reflector filter = new Perfect_reflector();
                backgroundWorker1.RunWorkerAsync(filter);
                ListBm.Add(image);
            }
        }

        private void линейноеРастяжениеГистограммыToolStripMenuItem_Click(object sender, EventArgs e)
        {
                if (image != null)
            {
                Histogram_linear_stretch filter = new Histogram_linear_stretch();
                backgroundWorker1.RunWorkerAsync(filter);
                ListBm.Add(image);
            }
        }

        private void отменаToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (ListBm.Count > 0)
            {
                image = ListBm.Last();
                pictureBox1.Image = image;
                ListBm.Remove(ListBm.Last());
            }
        }

        private void поворотToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (image != null)
            {
                Turn filter = new Turn();
                backgroundWorker1.RunWorkerAsync(filter);
                ListBm.Add(image);
            }
        }

        private void волны1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (image != null)
            {
                wave1 filter = new wave1();
                backgroundWorker1.RunWorkerAsync(filter);
                ListBm.Add(image);
            }
        }

        private void волны2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (image != null)
            {
                wave2 filter = new wave2();
                backgroundWorker1.RunWorkerAsync(filter);
                ListBm.Add(image);
            }
        }

        private void операторToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (image != null)
            {
                Operator_Pruitta filter = new Operator_Pruitta();
                backgroundWorker1.RunWorkerAsync(filter);
                ListBm.Add(image);
            }
        }

       /* private void button2_Click(object sender, EventArgs e)
        {
            rad = System.Convert.ToInt32(numericUpDown1.Value);
            Median_filter filter = new Median_filter(rad);
            backgroundWorker1.RunWorkerAsync(filter);//запускает выполнение фоновой опрерации
            ListBm.Add(image);
            button2.Visible = false;
            numericUpDown1.Visible = false;
        }*/

        private void медианныйФильтрToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (image != null)
            {
                //button2.Visible = true;
                //numericUpDown1.Visible = true;


                Form3 form3 = new Form3();
                form3.ShowDialog();

                rad = System.Convert.ToInt32(form3.numericUpDown1.Value);
                Median_filter filter = new Median_filter(rad);
                backgroundWorker1.RunWorkerAsync(filter);
                ListBm.Add(image);
            }
        }

        private void тестToolStripMenuItem_Click(object sender, EventArgs e)
        {

            Form2 form2 = new Form2();
            form2.ShowDialog();

            kernel[0, 0] = System.Convert.ToSingle(form2.numericUpDown1.Value);
            kernel[1, 0] = System.Convert.ToSingle(form2.numericUpDown2.Value);
            kernel[2, 0] = System.Convert.ToSingle(form2.numericUpDown3.Value);
            kernel[0, 1] = System.Convert.ToSingle(form2.numericUpDown4.Value);
            kernel[1, 1] = System.Convert.ToSingle(form2.numericUpDown5.Value);
            kernel[2, 1] = System.Convert.ToSingle(form2.numericUpDown6.Value);
            kernel[0, 2] = System.Convert.ToSingle(form2.numericUpDown7.Value);
            kernel[1, 2] = System.Convert.ToSingle(form2.numericUpDown8.Value);
            kernel[2, 2] = System.Convert.ToSingle(form2.numericUpDown9.Value);
        }
       
    }
}
