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
        

        public Visual()
        {
            InitializeComponent();
            OnNewData += Parser_WhenNewData;
            OnCompleted += Parser_WhenCompleted;
            DoubleBuffered = true;


        }

        public void AnimateImage()
        {
            if (!isActive)
            {

                //Begin the animation only once.
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

            //  ListBox.Items.AddRange(result);
            //   ListBox.Items.AddRange(subresult);
            isActive = false;
        }

        private async void MainCompWorker()
        {
            database = new DbManipulations();
            int max = await database.GetMaxCountDB("category");
            LinkToCompanyParser parser = new LinkToCompanyParser();
            loader = new HtmlLoader();
            
            for (int i = 1; i <= max; i++)
            {
                if (!isActive)
                {
                    return;
                }
                string whatTable = "category";
                string link = await database.GetLinkByKey(i, whatTable);
                
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
                    for (int j = 2; j < 100; j++)
                    {
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
        }



        private async void DetailCompWorker()
        {
            database = new DbManipulations();
            DetailCompanyParser detailParser = new DetailCompanyParser();
            string whatTable = "temp";
            int maxKey = await database.GetMaxCountDB(whatTable);
            for (int i = 8; i < maxKey; i++)
            {
                if (!isActive)
                {
                    return;
                }
                string link = await database.GetLinkByKey(i, whatTable);
                int categoryId = await database.GetIdByKey(i);
                loader = new HtmlLoader();
                var source = await loader.GetSource(link);
                var domParser = new HtmlParser();
                var document = await domParser.ParseAsync(source);
                var result = await detailParser.Parse(document, categoryId);

                OnNewData?.Invoke(this, result);

            }
            OnCompleted?.Invoke(this);
            isActive = false;
        }




        private  void parseCategoriesButton_Click(object sender, EventArgs e)
        {
            string link = "https://agrotender.com.ua/kompanii/region_ukraine/t10.html";
            isActive = true;
            Worker(link);
          //  Message("All Category name are collected!");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            isActive = true;
            MainCompWorker();
        }

        private void abortButton_Click(object sender, EventArgs e)
        {
            isActive = false;
            Message("Shit is stoped!");
        }

        private void ParseCompanyesButton_Click(object sender, EventArgs e)
        {
            isActive = true;
            DetailCompWorker();
            
        }
    }
}
