namespace AgroParser
{
    partial class Visual
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.DbManipButton = new System.Windows.Forms.Label();
            this.labelParse = new System.Windows.Forms.Label();
            this.ListBox = new System.Windows.Forms.ListBox();
            this.CreateDBButton = new System.Windows.Forms.Button();
            this.clearDataButton = new System.Windows.Forms.Button();
            this.dropDbButton = new System.Windows.Forms.Button();
            this.abortButton = new System.Windows.Forms.Button();
            this.errorCheckButon = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.progressTextLabel = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.timeElapsedLabel = new System.Windows.Forms.Label();
            this.timeLeftLabel = new System.Windows.Forms.Label();
            this.progresLabel = new System.Windows.Forms.Label();
            this.CompParsedBox = new System.Windows.Forms.TextBox();
            this.totalParsedLabel = new System.Windows.Forms.Label();
            this.TotalCompParsed = new System.Windows.Forms.Label();
            this.TotalParsedPhones = new System.Windows.Forms.Label();
            this.PhonesParsedBox = new System.Windows.Forms.TextBox();
            this.EmailsParsedLabels = new System.Windows.Forms.Label();
            this.EmailParsedBox = new System.Windows.Forms.TextBox();
            this.FaxParsedLabel = new System.Windows.Forms.Label();
            this.FaxParsedBox = new System.Windows.Forms.TextBox();
            this.ParsedLinksLabel = new System.Windows.Forms.Label();
            this.ParsedLinksBox = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.startAllButton = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.progressInfoLabel = new System.Windows.Forms.Label();
            this.timeRemainsInfoLabel = new System.Windows.Forms.Label();
            this.progressBarInfo = new System.Windows.Forms.ProgressBar();
            this.infoLabel = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // DbManipButton
            // 
            this.DbManipButton.AutoSize = true;
            this.DbManipButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.DbManipButton.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.DbManipButton.Location = new System.Drawing.Point(30, 18);
            this.DbManipButton.Name = "DbManipButton";
            this.DbManipButton.Size = new System.Drawing.Size(81, 17);
            this.DbManipButton.TabIndex = 0;
            this.DbManipButton.Text = "DATABASE";
            // 
            // labelParse
            // 
            this.labelParse.AutoSize = true;
            this.labelParse.Location = new System.Drawing.Point(109, 18);
            this.labelParse.Name = "labelParse";
            this.labelParse.Size = new System.Drawing.Size(62, 13);
            this.labelParse.TabIndex = 1;
            this.labelParse.Text = "Let\'s Parse!";
            // 
            // ListBox
            // 
            this.ListBox.FormattingEnabled = true;
            this.ListBox.HorizontalScrollbar = true;
            this.ListBox.Location = new System.Drawing.Point(443, 10);
            this.ListBox.Name = "ListBox";
            this.ListBox.Size = new System.Drawing.Size(701, 420);
            this.ListBox.TabIndex = 2;
            // 
            // CreateDBButton
            // 
            this.CreateDBButton.Location = new System.Drawing.Point(23, 47);
            this.CreateDBButton.Name = "CreateDBButton";
            this.CreateDBButton.Size = new System.Drawing.Size(97, 41);
            this.CreateDBButton.TabIndex = 3;
            this.CreateDBButton.Text = "Create DataBase";
            this.CreateDBButton.UseVisualStyleBackColor = true;
            this.CreateDBButton.Click += new System.EventHandler(this.CreateDBButton_Click);
            // 
            // clearDataButton
            // 
            this.clearDataButton.Location = new System.Drawing.Point(23, 94);
            this.clearDataButton.Name = "clearDataButton";
            this.clearDataButton.Size = new System.Drawing.Size(97, 41);
            this.clearDataButton.TabIndex = 4;
            this.clearDataButton.Text = "Erase All Data";
            this.clearDataButton.UseVisualStyleBackColor = true;
            this.clearDataButton.Click += new System.EventHandler(this.clearDataButton_Click);
            // 
            // dropDbButton
            // 
            this.dropDbButton.Location = new System.Drawing.Point(23, 141);
            this.dropDbButton.Name = "dropDbButton";
            this.dropDbButton.Size = new System.Drawing.Size(97, 41);
            this.dropDbButton.TabIndex = 5;
            this.dropDbButton.Text = "Delete DataBase";
            this.dropDbButton.UseVisualStyleBackColor = true;
            this.dropDbButton.Click += new System.EventHandler(this.dropDbButton_Click);
            // 
            // abortButton
            // 
            this.abortButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.abortButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.abortButton.Location = new System.Drawing.Point(153, 40);
            this.abortButton.Name = "abortButton";
            this.abortButton.Size = new System.Drawing.Size(104, 41);
            this.abortButton.TabIndex = 9;
            this.abortButton.Text = "STOP!";
            this.abortButton.UseVisualStyleBackColor = true;
            this.abortButton.Click += new System.EventHandler(this.abortButton_Click);
            // 
            // errorCheckButon
            // 
            this.errorCheckButon.Location = new System.Drawing.Point(1170, 289);
            this.errorCheckButon.Name = "errorCheckButon";
            this.errorCheckButon.Size = new System.Drawing.Size(106, 41);
            this.errorCheckButon.TabIndex = 13;
            this.errorCheckButon.Text = "Check Log for Errors";
            this.errorCheckButon.UseVisualStyleBackColor = true;
            this.errorCheckButon.Click += new System.EventHandler(this.button1_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(515, 451);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(629, 23);
            this.progressBar1.TabIndex = 14;
            // 
            // progressTextLabel
            // 
            this.progressTextLabel.AutoSize = true;
            this.progressTextLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.progressTextLabel.Location = new System.Drawing.Point(440, 453);
            this.progressTextLabel.Name = "progressTextLabel";
            this.progressTextLabel.Size = new System.Drawing.Size(69, 17);
            this.progressTextLabel.TabIndex = 15;
            this.progressTextLabel.Text = "Progress:";
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            // 
            // timeElapsedLabel
            // 
            this.timeElapsedLabel.AutoSize = true;
            this.timeElapsedLabel.Location = new System.Drawing.Point(1161, 445);
            this.timeElapsedLabel.Name = "timeElapsedLabel";
            this.timeElapsedLabel.Size = new System.Drawing.Size(35, 13);
            this.timeElapsedLabel.TabIndex = 17;
            this.timeElapsedLabel.Text = "label4";
            // 
            // timeLeftLabel
            // 
            this.timeLeftLabel.AutoSize = true;
            this.timeLeftLabel.Location = new System.Drawing.Point(1161, 464);
            this.timeLeftLabel.Name = "timeLeftLabel";
            this.timeLeftLabel.Size = new System.Drawing.Size(35, 13);
            this.timeLeftLabel.TabIndex = 18;
            this.timeLeftLabel.Text = "label4";
            // 
            // progresLabel
            // 
            this.progresLabel.AutoSize = true;
            this.progresLabel.BackColor = System.Drawing.SystemColors.ControlLight;
            this.progresLabel.Location = new System.Drawing.Point(804, 455);
            this.progresLabel.Name = "progresLabel";
            this.progresLabel.Size = new System.Drawing.Size(0, 13);
            this.progresLabel.TabIndex = 19;
            // 
            // CompParsedBox
            // 
            this.CompParsedBox.Location = new System.Drawing.Point(1230, 126);
            this.CompParsedBox.Name = "CompParsedBox";
            this.CompParsedBox.Size = new System.Drawing.Size(46, 20);
            this.CompParsedBox.TabIndex = 20;
            // 
            // totalParsedLabel
            // 
            this.totalParsedLabel.AutoSize = true;
            this.totalParsedLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.totalParsedLabel.Location = new System.Drawing.Point(1182, 35);
            this.totalParsedLabel.Name = "totalParsedLabel";
            this.totalParsedLabel.Size = new System.Drawing.Size(76, 15);
            this.totalParsedLabel.TabIndex = 21;
            this.totalParsedLabel.Text = "Total Parsed";
            // 
            // TotalCompParsed
            // 
            this.TotalCompParsed.AutoSize = true;
            this.TotalCompParsed.Location = new System.Drawing.Point(1155, 129);
            this.TotalCompParsed.Name = "TotalCompParsed";
            this.TotalCompParsed.Size = new System.Drawing.Size(62, 13);
            this.TotalCompParsed.TabIndex = 22;
            this.TotalCompParsed.Text = "Companyes";
            // 
            // TotalParsedPhones
            // 
            this.TotalParsedPhones.AutoSize = true;
            this.TotalParsedPhones.Location = new System.Drawing.Point(1155, 163);
            this.TotalParsedPhones.Name = "TotalParsedPhones";
            this.TotalParsedPhones.Size = new System.Drawing.Size(43, 13);
            this.TotalParsedPhones.TabIndex = 24;
            this.TotalParsedPhones.Text = "Phones";
            // 
            // PhonesParsedBox
            // 
            this.PhonesParsedBox.Location = new System.Drawing.Point(1230, 160);
            this.PhonesParsedBox.Name = "PhonesParsedBox";
            this.PhonesParsedBox.Size = new System.Drawing.Size(46, 20);
            this.PhonesParsedBox.TabIndex = 23;
            // 
            // EmailsParsedLabels
            // 
            this.EmailsParsedLabels.AutoSize = true;
            this.EmailsParsedLabels.Location = new System.Drawing.Point(1155, 199);
            this.EmailsParsedLabels.Name = "EmailsParsedLabels";
            this.EmailsParsedLabels.Size = new System.Drawing.Size(37, 13);
            this.EmailsParsedLabels.TabIndex = 26;
            this.EmailsParsedLabels.Text = "Emails";
            // 
            // EmailParsedBox
            // 
            this.EmailParsedBox.Location = new System.Drawing.Point(1230, 196);
            this.EmailParsedBox.Name = "EmailParsedBox";
            this.EmailParsedBox.Size = new System.Drawing.Size(46, 20);
            this.EmailParsedBox.TabIndex = 25;
            // 
            // FaxParsedLabel
            // 
            this.FaxParsedLabel.AutoSize = true;
            this.FaxParsedLabel.Location = new System.Drawing.Point(1155, 235);
            this.FaxParsedLabel.Name = "FaxParsedLabel";
            this.FaxParsedLabel.Size = new System.Drawing.Size(69, 13);
            this.FaxParsedLabel.TabIndex = 28;
            this.FaxParsedLabel.Text = "Fax Numbers";
            // 
            // FaxParsedBox
            // 
            this.FaxParsedBox.Location = new System.Drawing.Point(1230, 232);
            this.FaxParsedBox.Name = "FaxParsedBox";
            this.FaxParsedBox.Size = new System.Drawing.Size(46, 20);
            this.FaxParsedBox.TabIndex = 27;
            // 
            // ParsedLinksLabel
            // 
            this.ParsedLinksLabel.AutoSize = true;
            this.ParsedLinksLabel.Location = new System.Drawing.Point(1155, 98);
            this.ParsedLinksLabel.Name = "ParsedLinksLabel";
            this.ParsedLinksLabel.Size = new System.Drawing.Size(32, 13);
            this.ParsedLinksLabel.TabIndex = 30;
            this.ParsedLinksLabel.Text = "Links";
            // 
            // ParsedLinksBox
            // 
            this.ParsedLinksBox.Location = new System.Drawing.Point(1230, 95);
            this.ParsedLinksBox.Name = "ParsedLinksBox";
            this.ParsedLinksBox.Size = new System.Drawing.Size(46, 20);
            this.ParsedLinksBox.TabIndex = 29;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.CreateDBButton);
            this.panel1.Controls.Add(this.clearDataButton);
            this.panel1.Controls.Add(this.dropDbButton);
            this.panel1.Controls.Add(this.DbManipButton);
            this.panel1.Location = new System.Drawing.Point(10, 10);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(148, 202);
            this.panel1.TabIndex = 31;
            // 
            // startAllButton
            // 
            this.startAllButton.Location = new System.Drawing.Point(16, 40);
            this.startAllButton.Name = "startAllButton";
            this.startAllButton.Size = new System.Drawing.Size(104, 41);
            this.startAllButton.TabIndex = 32;
            this.startAllButton.Text = "START";
            this.startAllButton.UseVisualStyleBackColor = true;
            this.startAllButton.Click += new System.EventHandler(this.startAllButton_Click);
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.labelParse);
            this.panel2.Controls.Add(this.startAllButton);
            this.panel2.Controls.Add(this.abortButton);
            this.panel2.Location = new System.Drawing.Point(164, 10);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(273, 100);
            this.panel2.TabIndex = 33;
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.progressInfoLabel);
            this.panel3.Controls.Add(this.timeRemainsInfoLabel);
            this.panel3.Controls.Add(this.progressBarInfo);
            this.panel3.Controls.Add(this.infoLabel);
            this.panel3.Location = new System.Drawing.Point(165, 117);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(272, 95);
            this.panel3.TabIndex = 34;
            // 
            // progressInfoLabel
            // 
            this.progressInfoLabel.AutoSize = true;
            this.progressInfoLabel.Location = new System.Drawing.Point(38, 69);
            this.progressInfoLabel.Name = "progressInfoLabel";
            this.progressInfoLabel.Size = new System.Drawing.Size(35, 13);
            this.progressInfoLabel.TabIndex = 3;
            this.progressInfoLabel.Text = "label1";
            // 
            // timeRemainsInfoLabel
            // 
            this.timeRemainsInfoLabel.AutoSize = true;
            this.timeRemainsInfoLabel.Location = new System.Drawing.Point(140, 70);
            this.timeRemainsInfoLabel.Name = "timeRemainsInfoLabel";
            this.timeRemainsInfoLabel.Size = new System.Drawing.Size(35, 13);
            this.timeRemainsInfoLabel.TabIndex = 2;
            this.timeRemainsInfoLabel.Text = "label1";
            // 
            // progressBarInfo
            // 
            this.progressBarInfo.Location = new System.Drawing.Point(24, 45);
            this.progressBarInfo.Name = "progressBarInfo";
            this.progressBarInfo.Size = new System.Drawing.Size(226, 17);
            this.progressBarInfo.TabIndex = 1;
            // 
            // infoLabel
            // 
            this.infoLabel.AutoSize = true;
            this.infoLabel.Location = new System.Drawing.Point(23, 11);
            this.infoLabel.Name = "infoLabel";
            this.infoLabel.Size = new System.Drawing.Size(229, 26);
            this.infoLabel.TabIndex = 0;
            this.infoLabel.Text = "We are preparing for parsing. Please, wait.\n Button Stop/Pause will be able in fe" +
    "w minutes.";
            this.infoLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // Visual
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1288, 486);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.ParsedLinksLabel);
            this.Controls.Add(this.ParsedLinksBox);
            this.Controls.Add(this.FaxParsedLabel);
            this.Controls.Add(this.FaxParsedBox);
            this.Controls.Add(this.EmailsParsedLabels);
            this.Controls.Add(this.EmailParsedBox);
            this.Controls.Add(this.TotalParsedPhones);
            this.Controls.Add(this.PhonesParsedBox);
            this.Controls.Add(this.TotalCompParsed);
            this.Controls.Add(this.totalParsedLabel);
            this.Controls.Add(this.CompParsedBox);
            this.Controls.Add(this.progresLabel);
            this.Controls.Add(this.timeLeftLabel);
            this.Controls.Add(this.timeElapsedLabel);
            this.Controls.Add(this.progressTextLabel);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.errorCheckButon);
            this.Controls.Add(this.ListBox);
            this.Name = "Visual";
            this.Text = "Visual";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label DbManipButton;
        private System.Windows.Forms.Label labelParse;
        private System.Windows.Forms.ListBox ListBox;
        private System.Windows.Forms.Button CreateDBButton;
        private System.Windows.Forms.Button clearDataButton;
        private System.Windows.Forms.Button dropDbButton;
        private System.Windows.Forms.Button abortButton;
        private System.Windows.Forms.Button errorCheckButon;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label progressTextLabel;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label timeElapsedLabel;
        private System.Windows.Forms.Label timeLeftLabel;
        private System.Windows.Forms.Label progresLabel;
        private System.Windows.Forms.TextBox CompParsedBox;
        private System.Windows.Forms.Label totalParsedLabel;
        private System.Windows.Forms.Label TotalCompParsed;
        private System.Windows.Forms.Label TotalParsedPhones;
        private System.Windows.Forms.TextBox PhonesParsedBox;
        private System.Windows.Forms.Label EmailsParsedLabels;
        private System.Windows.Forms.TextBox EmailParsedBox;
        private System.Windows.Forms.Label FaxParsedLabel;
        private System.Windows.Forms.TextBox FaxParsedBox;
        private System.Windows.Forms.Label ParsedLinksLabel;
        private System.Windows.Forms.TextBox ParsedLinksBox;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button startAllButton;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label infoLabel;
        private System.Windows.Forms.Label timeRemainsInfoLabel;
        private System.Windows.Forms.ProgressBar progressBarInfo;
        private System.Windows.Forms.Label progressInfoLabel;
    }
}