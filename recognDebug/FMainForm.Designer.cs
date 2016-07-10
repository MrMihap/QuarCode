namespace recognDebug
{
  partial class FMainForm
  {
    /// <summary>
    /// Требуется переменная конструктора.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Освободить все используемые ресурсы.
    /// </summary>
    /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Код, автоматически созданный конструктором форм Windows

    /// <summary>
    /// Обязательный метод для поддержки конструктора - не изменяйте
    /// содержимое данного метода при помощи редактора кода.
    /// </summary>
    private void InitializeComponent()
    {
      this.components = new System.ComponentModel.Container();
      this.RawImageBox = new Emgu.CV.UI.ImageBox();
      this.FilteredImageBox = new Emgu.CV.UI.ImageBox();
      this.CroptedImageBox = new Emgu.CV.UI.ImageBox();
      this.menuStrip1 = new System.Windows.Forms.MenuStrip();
      this.файлToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.connectToCamStream = new System.Windows.Forms.ToolStripMenuItem();
      this.LoadSingleImgButton = new System.Windows.Forms.ToolStripMenuItem();
      this.ExitButton = new System.Windows.Forms.ToolStripMenuItem();
      this.тестToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.DecodeTestButton = new System.Windows.Forms.ToolStripMenuItem();
      this.общийТестToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.secondExitButton = new System.Windows.Forms.ToolStripMenuItem();
      this.label1 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.label3 = new System.Windows.Forms.Label();
      this.LastCodeTextBox = new System.Windows.Forms.TextBox();
      this.label4 = new System.Windows.Forms.Label();
      ((System.ComponentModel.ISupportInitialize)(this.RawImageBox)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.FilteredImageBox)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.CroptedImageBox)).BeginInit();
      this.menuStrip1.SuspendLayout();
      this.SuspendLayout();
      // 
      // RawImageBox
      // 
      this.RawImageBox.Location = new System.Drawing.Point(12, 54);
      this.RawImageBox.Name = "RawImageBox";
      this.RawImageBox.Size = new System.Drawing.Size(513, 309);
      this.RawImageBox.TabIndex = 2;
      this.RawImageBox.TabStop = false;
      // 
      // FilteredImageBox
      // 
      this.FilteredImageBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
      this.FilteredImageBox.Location = new System.Drawing.Point(12, 546);
      this.FilteredImageBox.Name = "FilteredImageBox";
      this.FilteredImageBox.Size = new System.Drawing.Size(513, 137);
      this.FilteredImageBox.TabIndex = 2;
      this.FilteredImageBox.TabStop = false;
      // 
      // CroptedImageBox
      // 
      this.CroptedImageBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.CroptedImageBox.Location = new System.Drawing.Point(531, 54);
      this.CroptedImageBox.Name = "CroptedImageBox";
      this.CroptedImageBox.Size = new System.Drawing.Size(752, 629);
      this.CroptedImageBox.TabIndex = 2;
      this.CroptedImageBox.TabStop = false;
      // 
      // menuStrip1
      // 
      this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.файлToolStripMenuItem,
            this.тестToolStripMenuItem,
            this.secondExitButton});
      this.menuStrip1.Location = new System.Drawing.Point(0, 0);
      this.menuStrip1.Name = "menuStrip1";
      this.menuStrip1.Size = new System.Drawing.Size(1362, 24);
      this.menuStrip1.TabIndex = 3;
      this.menuStrip1.Text = "menuStrip1";
      // 
      // файлToolStripMenuItem
      // 
      this.файлToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.connectToCamStream,
            this.LoadSingleImgButton,
            this.ExitButton});
      this.файлToolStripMenuItem.Name = "файлToolStripMenuItem";
      this.файлToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
      this.файлToolStripMenuItem.Text = "Файл";
      // 
      // connectToCamStream
      // 
      this.connectToCamStream.Name = "connectToCamStream";
      this.connectToCamStream.Size = new System.Drawing.Size(269, 22);
      this.connectToCamStream.Text = "Подключится к камере";
      this.connectToCamStream.Click += new System.EventHandler(this.connectToCamStream_Click);
      // 
      // LoadSingleImgButton
      // 
      this.LoadSingleImgButton.Name = "LoadSingleImgButton";
      this.LoadSingleImgButton.Size = new System.Drawing.Size(269, 22);
      this.LoadSingleImgButton.Text = "Загрузить одиночное изображение";
      this.LoadSingleImgButton.Click += new System.EventHandler(this.LoadSingleImgButton_Click);
      // 
      // ExitButton
      // 
      this.ExitButton.Name = "ExitButton";
      this.ExitButton.Size = new System.Drawing.Size(269, 22);
      this.ExitButton.Text = "Выход";
      this.ExitButton.Click += new System.EventHandler(this.ExitButton_Click);
      // 
      // тестToolStripMenuItem
      // 
      this.тестToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.DecodeTestButton,
            this.общийТестToolStripMenuItem});
      this.тестToolStripMenuItem.Name = "тестToolStripMenuItem";
      this.тестToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
      this.тестToolStripMenuItem.Text = "Тест";
      // 
      // DecodeTestButton
      // 
      this.DecodeTestButton.Name = "DecodeTestButton";
      this.DecodeTestButton.Size = new System.Drawing.Size(169, 22);
      this.DecodeTestButton.Text = "Декодинг данных";
      this.DecodeTestButton.Click += new System.EventHandler(this.DecodeTestButton_Click);
      // 
      // общийТестToolStripMenuItem
      // 
      this.общийТестToolStripMenuItem.Name = "общийТестToolStripMenuItem";
      this.общийТестToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
      this.общийТестToolStripMenuItem.Text = "Общий тест";
      // 
      // secondExitButton
      // 
      this.secondExitButton.Name = "secondExitButton";
      this.secondExitButton.Size = new System.Drawing.Size(53, 20);
      this.secondExitButton.Text = "Выход";
      this.secondExitButton.Click += new System.EventHandler(this.secondExitButton_Click);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(13, 31);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(61, 13);
      this.label1.TabIndex = 4;
      this.label1.Text = "Raw Image";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(13, 530);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(73, 13);
      this.label2.TabIndex = 4;
      this.label2.Text = "Filtered Image";
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(653, 38);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(73, 13);
      this.label3.TabIndex = 5;
      this.label3.Text = "CroptedImage";
      // 
      // LastCodeTextBox
      // 
      this.LastCodeTextBox.Location = new System.Drawing.Point(245, 28);
      this.LastCodeTextBox.Name = "LastCodeTextBox";
      this.LastCodeTextBox.Size = new System.Drawing.Size(149, 20);
      this.LastCodeTextBox.TabIndex = 6;
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(136, 31);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(103, 13);
      this.label4.TabIndex = 7;
      this.label4.Text = "LastReconizedCode";
      // 
      // FMainForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(1362, 711);
      this.Controls.Add(this.label4);
      this.Controls.Add(this.LastCodeTextBox);
      this.Controls.Add(this.label3);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.CroptedImageBox);
      this.Controls.Add(this.FilteredImageBox);
      this.Controls.Add(this.RawImageBox);
      this.Controls.Add(this.menuStrip1);
      this.MainMenuStrip = this.menuStrip1;
      this.Name = "FMainForm";
      this.Load += new System.EventHandler(this.FMainForm_Load);
      ((System.ComponentModel.ISupportInitialize)(this.RawImageBox)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.FilteredImageBox)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.CroptedImageBox)).EndInit();
      this.menuStrip1.ResumeLayout(false);
      this.menuStrip1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private Emgu.CV.UI.ImageBox RawImageBox;
    private Emgu.CV.UI.ImageBox FilteredImageBox;
    private Emgu.CV.UI.ImageBox CroptedImageBox;
    private System.Windows.Forms.MenuStrip menuStrip1;
    private System.Windows.Forms.ToolStripMenuItem файлToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem connectToCamStream;
    private System.Windows.Forms.ToolStripMenuItem LoadSingleImgButton;
    private System.Windows.Forms.ToolStripMenuItem ExitButton;
    private System.Windows.Forms.ToolStripMenuItem secondExitButton;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.TextBox LastCodeTextBox;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.ToolStripMenuItem тестToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem DecodeTestButton;
    private System.Windows.Forms.ToolStripMenuItem общийТестToolStripMenuItem;
  }
}

