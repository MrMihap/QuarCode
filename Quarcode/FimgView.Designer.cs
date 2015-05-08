namespace Quarcode
{
  partial class FimgView
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
      this.RandomStringButton = new System.Windows.Forms.Button();
      this.qrImgPictureBox = new System.Windows.Forms.PictureBox();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.qrMessageTextBox = new System.Windows.Forms.TextBox();
      this.GenerateQRButton = new System.Windows.Forms.Button();
      this.QrImgWrapperPanel = new System.Windows.Forms.Panel();
      ((System.ComponentModel.ISupportInitialize)(this.qrImgPictureBox)).BeginInit();
      this.groupBox1.SuspendLayout();
      this.QrImgWrapperPanel.SuspendLayout();
      this.SuspendLayout();
      // 
      // RandomStringButton
      // 
      this.RandomStringButton.Location = new System.Drawing.Point(6, 19);
      this.RandomStringButton.Name = "RandomStringButton";
      this.RandomStringButton.Size = new System.Drawing.Size(75, 23);
      this.RandomStringButton.TabIndex = 0;
      this.RandomStringButton.Text = "Random";
      this.RandomStringButton.UseVisualStyleBackColor = true;
      // 
      // qrImgPictureBox
      // 
      this.qrImgPictureBox.BackColor = System.Drawing.SystemColors.ButtonHighlight;
      this.qrImgPictureBox.Location = new System.Drawing.Point(6, 3);
      this.qrImgPictureBox.Name = "qrImgPictureBox";
      this.qrImgPictureBox.Size = new System.Drawing.Size(526, 406);
      this.qrImgPictureBox.TabIndex = 1;
      this.qrImgPictureBox.TabStop = false;
      this.qrImgPictureBox.Paint += new System.Windows.Forms.PaintEventHandler(this.qrImgPictureBox_Paint);
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this.qrMessageTextBox);
      this.groupBox1.Controls.Add(this.GenerateQRButton);
      this.groupBox1.Controls.Add(this.RandomStringButton);
      this.groupBox1.Location = new System.Drawing.Point(13, 13);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(351, 51);
      this.groupBox1.TabIndex = 2;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Generate QR";
      // 
      // qrMessageTextBox
      // 
      this.qrMessageTextBox.Location = new System.Drawing.Point(88, 21);
      this.qrMessageTextBox.Name = "qrMessageTextBox";
      this.qrMessageTextBox.Size = new System.Drawing.Size(100, 20);
      this.qrMessageTextBox.TabIndex = 1;
      // 
      // GenerateQRButton
      // 
      this.GenerateQRButton.Location = new System.Drawing.Point(194, 19);
      this.GenerateQRButton.Name = "GenerateQRButton";
      this.GenerateQRButton.Size = new System.Drawing.Size(75, 23);
      this.GenerateQRButton.TabIndex = 0;
      this.GenerateQRButton.Text = "Generate";
      this.GenerateQRButton.UseVisualStyleBackColor = true;
      this.GenerateQRButton.Click += new System.EventHandler(this.GenerateQRButton_Click);
      // 
      // QrImgWrapperPanel
      // 
      this.QrImgWrapperPanel.AutoScroll = true;
      this.QrImgWrapperPanel.Controls.Add(this.qrImgPictureBox);
      this.QrImgWrapperPanel.Location = new System.Drawing.Point(13, 70);
      this.QrImgWrapperPanel.Name = "QrImgWrapperPanel";
      this.QrImgWrapperPanel.Size = new System.Drawing.Size(535, 412);
      this.QrImgWrapperPanel.TabIndex = 3;
      // 
      // FimgView
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(555, 483);
      this.Controls.Add(this.QrImgWrapperPanel);
      this.Controls.Add(this.groupBox1);
      this.Name = "FimgView";
      this.Text = "GexQR Generator";
      ((System.ComponentModel.ISupportInitialize)(this.qrImgPictureBox)).EndInit();
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.QrImgWrapperPanel.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button RandomStringButton;
    private System.Windows.Forms.PictureBox qrImgPictureBox;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.TextBox qrMessageTextBox;
    private System.Windows.Forms.Button GenerateQRButton;
    private System.Windows.Forms.Panel QrImgWrapperPanel;
  }
}

