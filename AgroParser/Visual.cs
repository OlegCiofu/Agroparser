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
using AgroParser.ParserWorkers;

using System.Windows.Forms;

namespace AgroParser
{
    public partial class Visual : Form
    {
        event Action<object, string[]> OnNewData;
        DbManipulations database;
        DbChecker dbChecker;
        bool isActive;
        Bitmap animatedImage = new Bitmap($"{Application.StartupPath}\\tenor.gif");
        int counter;
        double timeLeft;
        double timeTotal;
        double percent;
        int curentValueProgress;

        public Visual()
        {
            InitializeComponent();
            OnNewData += Parser_WhenNewData;
            DoubleBuffered = true;
            progressBar1.Visible = false;
            progressTextLabel.Visible = false;
            timeElapsedLabel.Visible = false;
            timeLeftLabel.Visible = false;
            panel3.Visible = false;
            abortButton.Enabled = false;
            // progresLabel.Visible = false;
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
            e.Graphics.DrawImage(animatedImage, 27, 191, 382, 283);
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
            database = new DbManipulations();
            database.CreateDB();
        }

        private void clearDataButton_Click(object sender, EventArgs e)
        {
            database = new DbManipulations();
            database.EraseData();
        }

        private void dropDbButton_Click(object sender, EventArgs e)
        {
            database = new DbManipulations();
            database.DropDB();
        }

        private void Parser_WhenNewData(object arg1, string[] arg2)
        {
            ListBox.Items.AddRange(arg2);
            ListBox.TopIndex = ListBox.Items.Count - 1;
        }
        
        public void prepareProcessUpdate(int max)
        {
            progressBarInfo.Refresh();
            progressBarInfo.Maximum = max;
            progressBarInfo.Value = curentValueProgress;
            percent = (double)(((double)progressBarInfo.Value / (double)progressBarInfo.Maximum) * 100);
            percent = Math.Round(percent, 2);
            progressInfoLabel.Text = $"{percent.ToString("00.00")} %";
        }

        public void totalProcessUpdate(int max)
        {
            progressBar1.Refresh();
            progressBar1.Maximum = max;
            progressBar1.Value = curentValueProgress;
            percent = (double)(((double)progressBar1.Value / (double)progressBar1.Maximum) * 100);
            percent = Math.Round(percent, 2);
            progresLabel.Text = $"{percent.ToString("00.00")} %";
        }
        
        private void abortButton_Click(object sender, EventArgs e)
        {
            isActive = false;
            dbChecker.storeCounterToDb(counter);
            FinishProcessMonitoring();
            startAllButton.Enabled = true;
            abortButton.Enabled = false;
            Message("Parsing aborted!");
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

        public async void ShowTotalParsed()
        {
            dbChecker = new DbChecker();
            (int links, int companyes, int phones, int faxes, int emails) = await dbChecker.GetParsedData();
            ParsedLinksBox.Text = links.ToString();
            CompParsedBox.Text = companyes.ToString();
            PhonesParsedBox.Text = phones.ToString();
            FaxParsedBox.Text = faxes.ToString();
            EmailParsedBox.Text = emails.ToString();
        }

        public void InitProcessMonitoring()
        {
            progressBar1.Value = 0;
            timer1.Start();
            timer1.Tick += new EventHandler(timerTick);
            counter = 0;
            timeTotal = 0;
            timeLeft = 0;
            percent = 0;
        }

        public void ActivateVisibilityMonitor()
        {
            progressBar1.Visible = true;
            progresLabel.Visible = true;
            timeElapsedLabel.Text = $"Time elapsed: 00:00:00";
            timeLeftLabel.Text = $"Time remains: 00:00:00";
            timeRemainsInfoLabel.Text = $"Time remains: 00:00:00";
            timeElapsedLabel.Visible = true;
            timeLeftLabel.Visible = true;
            progressTextLabel.Visible = true;

        }
        
        public void FinishProcessMonitoring()
        {
            if (timer1.Enabled==true)
                timer1.Stop();
            timer1.Enabled = false;
            timeElapsedLabel.Visible = false;
            timeLeftLabel.Visible = false;
            progressBar1.Visible = false;
            progressTextLabel.Visible = false;
            progresLabel.Visible = false;
            timer1.Tick -= new EventHandler(timerTick);
        }

        private void timerTick(Object myObject, EventArgs myEventArgs)
        {

            counter++;
            int seconds = counter-counter/60*60;
            int minutes = counter/60 - (counter/60)/60*60;
            int hours = counter/(60*60);
            timeElapsedLabel.Text = $"Time elapsed: {hours.ToString("00")}:{minutes.ToString("00")}:{seconds.ToString("00")}";
            timeTotal = (counter * 100) / percent;
            timeLeft = timeTotal - counter;
           // timeLeft-=2;
            if (timeLeft >= 1)
            {
                int secondsLeft = Convert.ToInt32((int)timeLeft - (int)timeLeft / 60 * 60);
                int minutesLeft = Convert.ToInt32((int)timeLeft / 60 - ((int)timeLeft / 60) / 60 * 60);
                int hoursLeft = Convert.ToInt32((int)timeLeft / (60 * 60));
                timeRemainsInfoLabel.Text = $"Time remains: {hoursLeft.ToString("00")}:{minutesLeft.ToString("00")}:{secondsLeft.ToString("00")}";
                timeLeftLabel.Text = $"Time remains: {hoursLeft.ToString("00")}:{minutesLeft.ToString("00")}:{secondsLeft.ToString("00")}";
            }
        }

        private async void startAllButton_Click(object sender, EventArgs e)
        {
            dbChecker = new DbChecker();
            bool firstStep = dbChecker.CheckIntProcessFromDb("step_1");
            bool secondStep = dbChecker.CheckIntProcessFromDb("step_2");
            startAllButton.Enabled = false;
            if (!firstStep)
            {
                string link = "https://agrotender.com.ua/kompanii/region_ukraine/t10.html";
                isActive = true;
                firstStep = await Worker(link);
            }
                
            if (firstStep)
            {
                if (!secondStep)
                {
                    panel3.Visible = true;
                    isActive = true;
                    InitProcessMonitoring();
                    abortButton.Enabled = false;
                    secondStep = await MainCompWorker();
                }
                if (secondStep)
                {
                    panel3.Visible = false;
                    isActive = true;
                    InitProcessMonitoring();
                    abortButton.Enabled = true;
                    abortButton.Text = "Pause";
                    DetailCompWorker();

                }
            }

        }

        public async Task<bool> Worker(string link)
        {
            CategoryParser categoryParser = new CategoryParser();
            SubCategoryParser subCatParser = new SubCategoryParser();
            var result = await categoryParser.Parse(1, $"{link}");
            var subresult = await subCatParser.Parse(1, $"{link}");
            OnNewData?.Invoke(this, result);
            OnNewData?.Invoke(this, subresult);
            dbChecker.PutIntProcessToDb("step_1", 0, true);
            isActive = false;
            return true;
        }

        public async Task<bool> MainCompWorker()
        {
            dbChecker = new DbChecker();
            int max = await dbChecker.GetMaxCountDB("category");
            LinkToCompanyParser parser = new LinkToCompanyParser();
            curentValueProgress = 0;
            ActivateVisibilityMonitor();

            for (int i = 1; i <= max; i++)
            {
                if (!isActive)
                {
                    return true;
                }

                (string link, int categoryId) = await dbChecker.GetLinkAndIdByKey(i, "category");

                if (link == "0" || link == "")
                {
                    continue;
                }

                for (int j = 1; j <= 100; j++)
                {
                    curentValueProgress++;
                    prepareProcessUpdate((max - 7) * 100);

                    if (!isActive)
                    {
                        return true;
                    }
                    var result = await parser.Parse(j, link, i);
                    if (result == null)
                    {
                        curentValueProgress += (100 - j);
                        prepareProcessUpdate((max - 7) * 100);
                        break;
                    }

                    ShowTotalParsed();
                    OnNewData?.Invoke(this, result);
                    Console.WriteLine($"Цикл страницы {j} завершен");

                }
            }

            dbChecker.PutIntProcessToDb("step_2", 0, true);
            dbChecker.PutIntProcessToDb("internal-string-time-elapsed", 0, false);
            isActive = false;
            FinishProcessMonitoring();
            return true;
        }

        public async void DetailCompWorker()
        {
            ActivateVisibilityMonitor();
            dbChecker = new DbChecker();
            curentValueProgress = 0;
            DetailCompanyParser detailParser = new DetailCompanyParser();
            string whatTable = "temp";
            int maxKey = await dbChecker.GetMaxCountDB(whatTable);
            int linkId = await dbChecker.GetMaxCountDB();
            if (linkId == 0)
                linkId = 1;

            for (int i = linkId+1; i < maxKey; i++)
            {
                curentValueProgress = i;
                totalProcessUpdate(maxKey);

                if (!isActive)
                {
                    return;
                }

                (string link, int categoryId) = await dbChecker.GetLinkAndIdByKey(i, whatTable);
                var result = await detailParser.Parse(0, link, categoryId);
                dbChecker.markCompanyAsParsed(i);
                ShowTotalParsed();
                OnNewData?.Invoke(this, result);
            }
            FinishProcessMonitoring();
            isActive = false;
            startAllButton.Enabled = true;
            abortButton.Enabled = false;
            Message("All work is done!");
        }
    }
}
