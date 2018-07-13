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
            this.parseCategoriesButton = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.ParseCompanyesButton = new System.Windows.Forms.Button();
            this.abortButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.progressTextLabel = new System.Windows.Forms.Label();
            this.progresLabel = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.timeElapsedLabel = new System.Windows.Forms.Label();
            this.timeLeftLabel = new System.Windows.Forms.Label();
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // DbManipButton
            // 
            this.DbManipButton.AutoSize = true;
            this.DbManipButton.Location = new System.Drawing.Point(9, 12);
            this.DbManipButton.Name = "DbManipButton";
            this.DbManipButton.Size = new System.Drawing.Size(144, 13);
            this.DbManipButton.TabIndex = 0;
            this.DbManipButton.Text = "Manipulations with DataBase";
            // 
            // labelParse
            // 
            this.labelParse.AutoSize = true;
            this.labelParse.Location = new System.Drawing.Point(260, 12);
            this.labelParse.Name = "labelParse";
            this.labelParse.Size = new System.Drawing.Size(62, 13);
            this.labelParse.TabIndex = 1;
            this.labelParse.Text = "Let\'s Parse!";
            // 
            // ListBox
            // 
            this.ListBox.FormattingEnabled = true;
            this.ListBox.HorizontalScrollbar = true;
            this.ListBox.Location = new System.Drawing.Point(434, 12);
            this.ListBox.Name = "ListBox";
            this.ListBox.Size = new System.Drawing.Size(842, 420);
            this.ListBox.TabIndex = 2;
            // 
            // CreateDBButton
            // 
            this.CreateDBButton.Location = new System.Drawing.Point(12, 37);
            this.CreateDBButton.Name = "CreateDBButton";
            this.CreateDBButton.Size = new System.Drawing.Size(123, 41);
            this.CreateDBButton.TabIndex = 3;
            this.CreateDBButton.Text = "Create DataBase";
            this.CreateDBButton.UseVisualStyleBackColor = true;
            this.CreateDBButton.Click += new System.EventHandler(this.CreateDBButton_Click);
            // 
            // clearDataButton
            // 
            this.clearDataButton.Location = new System.Drawing.Point(12, 84);
            this.clearDataButton.Name = "clearDataButton";
            this.clearDataButton.Size = new System.Drawing.Size(123, 41);
            this.clearDataButton.TabIndex = 4;
            this.clearDataButton.Text = "Erase All Data";
            this.clearDataButton.UseVisualStyleBackColor = true;
            this.clearDataButton.Click += new System.EventHandler(this.clearDataButton_Click);
            // 
            // dropDbButton
            // 
            this.dropDbButton.Location = new System.Drawing.Point(12, 131);
            this.dropDbButton.Name = "dropDbButton";
            this.dropDbButton.Size = new System.Drawing.Size(123, 41);
            this.dropDbButton.TabIndex = 5;
            this.dropDbButton.Text = "Delete DataBase";
            this.dropDbButton.UseVisualStyleBackColor = true;
            this.dropDbButton.Click += new System.EventHandler(this.dropDbButton_Click);
            // 
            // parseCategoriesButton
            // 
            this.parseCategoriesButton.Location = new System.Drawing.Point(226, 37);
            this.parseCategoriesButton.Name = "parseCategoriesButton";
            this.parseCategoriesButton.Size = new System.Drawing.Size(141, 41);
            this.parseCategoriesButton.TabIndex = 6;
            this.parseCategoriesButton.Text = "Get all Categories Name";
            this.parseCategoriesButton.UseVisualStyleBackColor = true;
            this.parseCategoriesButton.Click += new System.EventHandler(this.parseCategoriesButton_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(226, 84);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(141, 41);
            this.button2.TabIndex = 7;
            this.button2.Text = "Get All Links";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // ParseCompanyesButton
            // 
            this.ParseCompanyesButton.Location = new System.Drawing.Point(226, 131);
            this.ParseCompanyesButton.Name = "ParseCompanyesButton";
            this.ParseCompanyesButton.Size = new System.Drawing.Size(141, 41);
            this.ParseCompanyesButton.TabIndex = 8;
            this.ParseCompanyesButton.Text = "Parse Companyes";
            this.ParseCompanyesButton.UseVisualStyleBackColor = true;
            this.ParseCompanyesButton.Click += new System.EventHandler(this.ParseCompanyesButton_Click);
            // 
            // abortButton
            // 
            this.abortButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.abortButton.Location = new System.Drawing.Point(183, 178);
            this.abortButton.Name = "abortButton";
            this.abortButton.Size = new System.Drawing.Size(106, 41);
            this.abortButton.TabIndex = 9;
            this.abortButton.Text = "STOP!";
            this.abortButton.UseVisualStyleBackColor = true;
            this.abortButton.Click += new System.EventHandler(this.abortButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(193, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 15);
            this.label1.TabIndex = 10;
            this.label1.Text = "1-->";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(193, 96);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 15);
            this.label2.TabIndex = 11;
            this.label2.Text = "2-->";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(193, 143);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 15);
            this.label3.TabIndex = 12;
            this.label3.Text = "3-->";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(305, 178);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(106, 41);
            this.button1.TabIndex = 13;
            this.button1.Text = "Check Log for Errors";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
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
            // progresLabel
            // 
            this.progresLabel.AutoSize = true;
            this.progresLabel.BackColor = System.Drawing.SystemColors.ControlLight;
            this.progresLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.progresLabel.Location = new System.Drawing.Point(810, 455);
            this.progresLabel.Name = "progresLabel";
            this.progresLabel.Size = new System.Drawing.Size(59, 15);
            this.progresLabel.TabIndex = 16;
            this.progresLabel.Text = "Progress:";
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
            // timer2
            // 
            this.timer2.Interval = 1000;
            // 
            // Visual
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1288, 486);
            this.Controls.Add(this.timeLeftLabel);
            this.Controls.Add(this.timeElapsedLabel);
            this.Controls.Add(this.progresLabel);
            this.Controls.Add(this.progressTextLabel);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.abortButton);
            this.Controls.Add(this.ParseCompanyesButton);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.parseCategoriesButton);
            this.Controls.Add(this.dropDbButton);
            this.Controls.Add(this.clearDataButton);
            this.Controls.Add(this.CreateDBButton);
            this.Controls.Add(this.ListBox);
            this.Controls.Add(this.labelParse);
            this.Controls.Add(this.DbManipButton);
            this.Name = "Visual";
            this.Text = "Visual";
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
        private System.Windows.Forms.Button parseCategoriesButton;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button ParseCompanyesButton;
        private System.Windows.Forms.Button abortButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label progressTextLabel;
        private System.Windows.Forms.Label progresLabel;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label timeElapsedLabel;
        private System.Windows.Forms.Label timeLeftLabel;
        private System.Windows.Forms.Timer timer2;
    }
}