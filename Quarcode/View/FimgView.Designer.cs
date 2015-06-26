namespace Quarcode.View
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
      this.components = new System.ComponentModel.Container();
      this.RandomStringButton = new System.Windows.Forms.Button();
      this.qrImgPictureBox = new System.Windows.Forms.PictureBox();
      this.PicBoxContext = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.SaveToBMPButton = new System.Windows.Forms.ToolStripMenuItem();
      this.ControlsGroupeBox = new System.Windows.Forms.GroupBox();
      this.DrawBorder = new System.Windows.Forms.CheckBox();
      this.ReRand = new System.Windows.Forms.CheckBox();
      this.FillCells = new System.Windows.Forms.CheckBox();
      this.DrawValNum = new System.Windows.Forms.CheckBox();
      this.DrawCellBorder = new System.Windows.Forms.CheckBox();
      this.radiusTrackBar = new System.Windows.Forms.TrackBar();
      this.label2 = new System.Windows.Forms.Label();
      this.label1 = new System.Windows.Forms.Label();
      this.qrMessageTextBox = new System.Windows.Forms.TextBox();
      this.GenerateQRButton = new System.Windows.Forms.Button();
      this.QrImgWrapperPanel = new System.Windows.Forms.Panel();
      ((System.ComponentModel.ISupportInitialize)(this.qrImgPictureBox)).BeginInit();
      this.PicBoxContext.SuspendLayout();
      this.ControlsGroupeBox.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.radiusTrackBar)).BeginInit();
      this.QrImgWrapperPanel.SuspendLayout();
      this.SuspendLayout();
      // 
      // RandomStringButton
      // 
      this.RandomStringButton.Location = new System.Drawing.Point(168, 11);
      this.RandomStringButton.Name = "RandomStringButton";
      this.RandomStringButton.Size = new System.Drawing.Size(75, 23);
      this.RandomStringButton.TabIndex = 0;
      this.RandomStringButton.Text = "Random";
      this.RandomStringButton.UseVisualStyleBackColor = true;
      this.RandomStringButton.Click += new System.EventHandler(this.RandomStringButton_Click);
      // 
      // qrImgPictureBox
      // 
      this.qrImgPictureBox.BackColor = System.Drawing.SystemColors.ButtonHighlight;
      this.qrImgPictureBox.ContextMenuStrip = this.PicBoxContext;
      this.qrImgPictureBox.Location = new System.Drawing.Point(6, 3);
      this.qrImgPictureBox.Name = "qrImgPictureBox";
      this.qrImgPictureBox.Size = new System.Drawing.Size(670, 461);
      this.qrImgPictureBox.TabIndex = 1;
      this.qrImgPictureBox.TabStop = false;
      this.qrImgPictureBox.Paint += new System.Windows.Forms.PaintEventHandler(this.qrImgPictureBox_Paint);
      // 
      // PicBoxContext
      // 
      this.PicBoxContext.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SaveToBMPButton});
      this.PicBoxContext.Name = "PicBoxContext";
      this.PicBoxContext.Size = new System.Drawing.Size(141, 26);
      // 
      // SaveToBMPButton
      // 
      this.SaveToBMPButton.Name = "SaveToBMPButton";
      this.SaveToBMPButton.Size = new System.Drawing.Size(140, 22);
      this.SaveToBMPButton.Text = "Save to BMP";
      this.SaveToBMPButton.Click += new System.EventHandler(this.SaveToBMPButton_Click);
      // 
      // ControlsGroupeBox
      // 
      this.ControlsGroupeBox.Controls.Add(this.DrawBorder);
      this.ControlsGroupeBox.Controls.Add(this.ReRand);
      this.ControlsGroupeBox.Controls.Add(this.FillCells);
      this.ControlsGroupeBox.Controls.Add(this.DrawValNum);
      this.ControlsGroupeBox.Controls.Add(this.DrawCellBorder);
      this.ControlsGroupeBox.Controls.Add(this.radiusTrackBar);
      this.ControlsGroupeBox.Controls.Add(this.label2);
      this.ControlsGroupeBox.Controls.Add(this.label1);
      this.ControlsGroupeBox.Controls.Add(this.qrMessageTextBox);
      this.ControlsGroupeBox.Controls.Add(this.GenerateQRButton);
      this.ControlsGroupeBox.Controls.Add(this.RandomStringButton);
      this.ControlsGroupeBox.Location = new System.Drawing.Point(13, 13);
      this.ControlsGroupeBox.Name = "ControlsGroupeBox";
      this.ControlsGroupeBox.Size = new System.Drawing.Size(681, 68);
      this.ControlsGroupeBox.TabIndex = 2;
      this.ControlsGroupeBox.TabStop = false;
      this.ControlsGroupeBox.Text = "Generate QR";
      // 
      // DrawBorder
      // 
      this.DrawBorder.AutoSize = true;
      this.DrawBorder.Location = new System.Drawing.Point(400, 15);
      this.DrawBorder.Name = "DrawBorder";
      this.DrawBorder.Size = new System.Drawing.Size(85, 17);
      this.DrawBorder.TabIndex = 4;
      this.DrawBorder.Text = "Draw Border";
      this.DrawBorder.UseVisualStyleBackColor = true;
      this.DrawBorder.Click += new System.EventHandler(this.GenerateQRButton_Click);
      // 
      // ReRand
      // 
      this.ReRand.AutoSize = true;
      this.ReRand.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.ReRand.Location = new System.Drawing.Point(607, 40);
      this.ReRand.Name = "ReRand";
      this.ReRand.Size = new System.Drawing.Size(69, 17);
      this.ReRand.TabIndex = 4;
      this.ReRand.Text = "Re Rand";
      this.ReRand.UseVisualStyleBackColor = true;
      this.ReRand.CheckedChanged += new System.EventHandler(this.ReRand_CheckedChanged);
      this.ReRand.Click += new System.EventHandler(this.GenerateQRButton_Click);
      // 
      // FillCells
      // 
      this.FillCells.AutoSize = true;
      this.FillCells.Checked = true;
      this.FillCells.CheckState = System.Windows.Forms.CheckState.Checked;
      this.FillCells.Location = new System.Drawing.Point(400, 38);
      this.FillCells.Name = "FillCells";
      this.FillCells.Size = new System.Drawing.Size(63, 17);
      this.FillCells.TabIndex = 4;
      this.FillCells.Text = "Fill Cells";
      this.FillCells.UseVisualStyleBackColor = true;
      this.FillCells.Click += new System.EventHandler(this.GenerateQRButton_Click);
      // 
      // DrawValNum
      // 
      this.DrawValNum.AutoSize = true;
      this.DrawValNum.Location = new System.Drawing.Point(276, 38);
      this.DrawValNum.Name = "DrawValNum";
      this.DrawValNum.Size = new System.Drawing.Size(94, 17);
      this.DrawValNum.TabIndex = 4;
      this.DrawValNum.Text = "Draw Val Num";
      this.DrawValNum.UseVisualStyleBackColor = true;
      this.DrawValNum.Click += new System.EventHandler(this.GenerateQRButton_Click);
      // 
      // DrawCellBorder
      // 
      this.DrawCellBorder.AutoSize = true;
      this.DrawCellBorder.Checked = true;
      this.DrawCellBorder.CheckState = System.Windows.Forms.CheckState.Checked;
      this.DrawCellBorder.Location = new System.Drawing.Point(276, 15);
      this.DrawCellBorder.Name = "DrawCellBorder";
      this.DrawCellBorder.Size = new System.Drawing.Size(99, 17);
      this.DrawCellBorder.TabIndex = 4;
      this.DrawCellBorder.Text = "DrawCellBorder";
      this.DrawCellBorder.UseVisualStyleBackColor = true;
      this.DrawCellBorder.Click += new System.EventHandler(this.GenerateQRButton_Click);
      // 
      // radiusTrackBar
      // 
      this.radiusTrackBar.AutoSize = false;
      this.radiusTrackBar.Location = new System.Drawing.Point(52, 40);
      this.radiusTrackBar.Maximum = 40;
      this.radiusTrackBar.Minimum = 5;
      this.radiusTrackBar.Name = "radiusTrackBar";
      this.radiusTrackBar.Size = new System.Drawing.Size(118, 20);
      this.radiusTrackBar.TabIndex = 3;
      this.radiusTrackBar.Value = 10;
      this.radiusTrackBar.Scroll += new System.EventHandler(this.radiusTrackBar_Scroll);
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(6, 40);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(40, 13);
      this.label2.TabIndex = 2;
      this.label2.Text = "Radius";
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(6, 16);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(50, 13);
      this.label1.TabIndex = 2;
      this.label1.Text = "Message";
      // 
      // qrMessageTextBox
      // 
      this.qrMessageTextBox.Location = new System.Drawing.Point(62, 13);
      this.qrMessageTextBox.Name = "qrMessageTextBox";
      this.qrMessageTextBox.Size = new System.Drawing.Size(100, 20);
      this.qrMessageTextBox.TabIndex = 1;
      this.qrMessageTextBox.TextChanged += new System.EventHandler(this.qrMessageTextBox_TextChanged);
      // 
      // GenerateQRButton
      // 
      this.GenerateQRButton.Location = new System.Drawing.Point(601, 11);
      this.GenerateQRButton.Name = "GenerateQRButton";
      this.GenerateQRButton.Size = new System.Drawing.Size(75, 23);
      this.GenerateQRButton.TabIndex = 0;
      this.GenerateQRButton.Text = "Generate";
      this.GenerateQRButton.UseVisualStyleBackColor = true;
      this.GenerateQRButton.Click += new System.EventHandler(this.GenerateQRButton_Click);
      // 
      // QrImgWrapperPanel
      // 
      this.QrImgWrapperPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.QrImgWrapperPanel.AutoScroll = true;
      this.QrImgWrapperPanel.Controls.Add(this.qrImgPictureBox);
      this.QrImgWrapperPanel.Location = new System.Drawing.Point(13, 84);
      this.QrImgWrapperPanel.Name = "QrImgWrapperPanel";
      this.QrImgWrapperPanel.Size = new System.Drawing.Size(681, 468);
      this.QrImgWrapperPanel.TabIndex = 3;
      // 
      // FimgView
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(701, 553);
      this.Controls.Add(this.QrImgWrapperPanel);
      this.Controls.Add(this.ControlsGroupeBox);
      this.Name = "FimgView";
      this.Text = "GexQR Generator";
      this.Load += new System.EventHandler(this.FimgView_Load);
      ((System.ComponentModel.ISupportInitialize)(this.qrImgPictureBox)).EndInit();
      this.PicBoxContext.ResumeLayout(false);
      this.ControlsGroupeBox.ResumeLayout(false);
      this.ControlsGroupeBox.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.radiusTrackBar)).EndInit();
      this.QrImgWrapperPanel.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button RandomStringButton;
    private System.Windows.Forms.PictureBox qrImgPictureBox;
    private System.Windows.Forms.GroupBox ControlsGroupeBox;
    private System.Windows.Forms.TextBox qrMessageTextBox;
    private System.Windows.Forms.Button GenerateQRButton;
    private System.Windows.Forms.Panel QrImgWrapperPanel;
    private System.Windows.Forms.ContextMenuStrip PicBoxContext;
    private System.Windows.Forms.ToolStripMenuItem SaveToBMPButton;
    private System.Windows.Forms.TrackBar radiusTrackBar;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.CheckBox DrawBorder;
    private System.Windows.Forms.CheckBox FillCells;
    private System.Windows.Forms.CheckBox DrawValNum;
    private System.Windows.Forms.CheckBox DrawCellBorder;
    private System.Windows.Forms.CheckBox ReRand;
  }
}

