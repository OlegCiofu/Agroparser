using AgroParser.Core;
using AngleSharp.Dom.Html;
using AngleSharp.Parser.Html;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;

namespace AgroParser
{
    public partial class Visual : Form
    {
        Core.DbManipulations database;
        bool isActive;
        HtmlLoader loader;
        public event Action<object, string[]> OnNewData;
        
        public event Action<object> OnCompleted;
        Bitmap animatedImage = new Bitmap($"{System.Windows.Forms.Application.StartupPath}\\giphy.gif");
        int counter;
        double timeLeft;
        double percent;



        public Visual()
        {
            InitializeComponent();
            OnNewData += Parser_WhenNewData;
            OnCompleted += Parser_WhenCompleted;
            DoubleBuffered = true;
            progressBar1.Visible = false;
            progressTextLabel.Visible = false;
            timeElapsedLabel.Visible = false;
            timeLeftLabel.Visible = false;
            progresLabel.Visible = false;


        }

        public void AnimateImage()
        {
            if (!isActive)
            {
                ImageAnimator.Animate(animatedImage, new EventHandler(OnFramesChanged));
            }
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            //Begin the animation.
            AnimateImage();

            //Get the next frame ready for rendering.
            ImageAnimator.UpdateFrames();

            //Draw the next frame in the animation.
            e.Graphics.DrawImage(animatedImage, new Point(12, 242));
        }

        private void OnFramesChanged(object sender, EventArgs e)
        {
            Invalidate();
        }

        public void Message(string info)
        {
            MessageBox.Show($"{info}", "AgroParser", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void CreateDBButton_Click(object sender, EventArgs e)
        {
            database = new Core.DbManipulations();
            database.CreateDB();
        }

        private void clearDataButton_Click(object sender, EventArgs e)
        {
            database = new Core.DbManipulations();
            database.EraseData();
        }

        private void dropDbButton_Click(object sender, EventArgs e)
        {
            database = new Core.DbManipulations();
            database.DropDB();
        }

        private void Parser_WhenNewData(object arg1, string[] arg2)
        {
            ListBox.Items.AddRange(arg2);
            ListBox.TopIndex = ListBox.Items.Count - 1;
        }

        private void Parser_WhenCompleted(object obj)
        {
            MessageBox.Show("All works good!", "AgroParser", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async void Worker(string link)
        {
            CategoryParser categoryParser = new CategoryParser();
            SubCategoryParser subCatParser = new SubCategoryParser();
            loader = new HtmlLoader();
            var source = await loader.GetSource(1, $"{link}");
            var domParser = new HtmlParser();
            var document = await domParser.ParseAsync(source);
            var result = await categoryParser.Parse(document);
            var subresult = await subCatParser.Parse(document);
            OnNewData?.Invoke(this, result);
            OnNewData?.Invoke(this, subresult);
            OnCompleted?.Invoke(this);
            isActive = false;
        }

        private async void MainCompWorker()
        {
            database = new DbManipulations();
            int max = await database.GetMaxCountDB("category");
            LinkToCompanyParser parser = new LinkToCompanyParser();
            loader = new HtmlLoader();
            int curentValueProgress = 0;
            
            for (int i = 1; i <= max; i++)
            {
                if (!isActive)
                {
                    return;
                }
                string whatTable = "category";
                (string link, int categoryId) = await database.GetLinkAndIdByKey(i, whatTable);

                if (link == "0"|| link == "")
                {
                    continue;
                }
                else
                {

                    var source = await loader.GetSource(link);
                    Console.WriteLine($"Рубрика {i}, страница 1");
                    var domParser = new HtmlParser();
                    var document = await domParser.ParseAsync(source);
                    var result = await parser.Parse(document, i);
                    OnNewData?.Invoke(this, result);
                    Console.WriteLine("Цикл базовго урл завершен");

                    for (int j = 2; j <= 100; j++)
                    {
                        curentValueProgress++;
                        progressBar1.Refresh();
                        progressBar1.Maximum = max * 100;
                        progressBar1.Value = curentValueProgress;
                        percent = (double)(((double)progressBar1.Value / (double)progressBar1.Maximum) * 100);
                        percent = Math.Round(percent, 2);
                        progresLabel.Text = $"{percent.ToString()} %";
                        if (!isActive)
                        {
                            return;
                        }
                        Console.WriteLine($"Рубрика {i}, страница {j}");
                        source = await loader.GetSource(j, link);
                        if (source == "404")
                            break;
                        document = await domParser.ParseAsync(source);
                        result = await parser.Parse(document, i);
                        OnNewData?.Invoke(this, result);
                        Console.WriteLine($"Цикл страницы {j} завершен");
                    }
                }
            }
            OnCompleted?.Invoke(this);
            isActive = false;
            progressBar1.Visible = false;
            progressTextLabel.Visible = false;
            progresLabel.Visible = false;
        }
        
        private async void DetailCompWorker()
        {
            database = new DbManipulations();
            DetailCompanyParser detailParser = new DetailCompanyParser();
            string whatTable = "temp";
            int maxKey = await database.GetMaxCountDB(whatTable);
            for (int i = 8; i < maxKey; i++)
            {
                progressBar1.Refresh();
                progressBar1.Maximum = maxKey-8;
                progressBar1.Value = i-8;
                percent = (double)(((double)progressBar1.Value / (double)progressBar1.Maximum) * 100);
                percent = Math.Round(percent, 2);
                progresLabel.Text = $"{percent.ToString()} %";
                timeLeft = (counter * 100) / percent;


                if (!isActive)
                {
                    return;
                }
                (string link, int categoryId) = await database.GetLinkAndIdByKey(i, whatTable);
               // string link = await database.GetLinkByKey(i, whatTable);
               // int categoryId = await database.GetIdByKey(i);
                loader = new HtmlLoader();
                var source = await loader.GetSource(link);
                var domParser = new HtmlParser();
                var document = await domParser.ParseAsync(source);
                var result = await detailParser.Parse(document, categoryId);

                OnNewData?.Invoke(this, result);

            }
            OnCompleted?.Invoke(this);
            isActive = false;
            progressBar1.Visible = false;
            progressTextLabel.Visible = false;
            progresLabel.Visible = false;
        }
        
        private  void parseCategoriesButton_Click(object sender, EventArgs e)
        {
            string link = "https://agrotender.com.ua/kompanii/region_ukraine/t10.html";
            isActive = true;
            Worker(link);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            isActive = true;
            progressBar1.Value = 0;
            progressBar1.Visible = true;
            progressTextLabel.Visible = true;
            progresLabel.Visible = true;
            MainCompWorker();
        }

        private void abortButton_Click(object sender, EventArgs e)
        {
            isActive = false;
            finishTimeElapse();
            Message("Parsing aborted!");
            progressBar1.Value = 0;
            progressBar1.Visible = false;
            progressTextLabel.Visible = false;
            progresLabel.Visible = false;
            
        }

        private void ParseCompanyesButton_Click(object sender, EventArgs e)
        {
            isActive = true;
            initTimeElapse();
            progressBar1.Value = 0;
            progressBar1.Visible = true;
            progressTextLabel.Visible = true;
            progresLabel.Visible = true;
            DetailCompWorker();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (var fs = new FileStream($"{Application.StartupPath}\\log.txt", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var rs = new StreamReader(fs, Encoding.Default))
            {
                string log = rs.ReadToEnd();
                if (log.Contains("Error"))
                {

                    int count = (log.Length - log.Replace("Error", "").Length) / 5;
                    MessageBox.Show($"There is {count} Errors in log.txt", "AgroParser", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                    MessageBox.Show("There is no Errors in log.txt", "AgroParser", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void initTimeElapse()
        {
            timeElapsedLabel.Text = $"Time elapsed: 00:00:00";
            timeLeftLabel.Text = $"Time remains: 00:00:00";
            timeElapsedLabel.Visible = true;
            timeLeftLabel.Visible = true;
            timer1.Start();
            timer1.Tick += new EventHandler(timerTick);
            counter = 0;
        }





        public void finishTimeElapse()
        {
            if (timer1.Enabled==true)
                timer1.Stop();
            timer1.Enabled = false;
            timeElapsedLabel.Visible = false;
            timeLeftLabel.Visible = false;

        }

        private void timerTick(Object myObject, EventArgs myEventArgs)
        {
            counter++;
            int seconds = counter-counter/60*60;
            int minutes = counter/60 - (counter/60)/60*60;
            int hours = counter/(60*60);
            timeElapsedLabel.Text = $"Time elapsed: {hours.ToString("00")}:{minutes.ToString("00")}:{seconds.ToString("00")}";

            timeLeft--;
            if (timeLeft >= 1)
            {
                int secondsLeft = Convert.ToInt32((int)timeLeft - (int)timeLeft / 60 * 60);
                int minutesLeft = Convert.ToInt32((int)timeLeft / 60 - ((int)timeLeft / 60) / 60 * 60);
                int hoursLeft = Convert.ToInt32((int)timeLeft / (60 * 60));
                timeLeftLabel.Text = $"Time remains: {hoursLeft.ToString("00")}:{minutesLeft.ToString("00")}:{secondsLeft.ToString("00")}";
            }




        }

    }
}
