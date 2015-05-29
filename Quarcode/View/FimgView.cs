using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Quarcode.Core;
using Quarcode.Interfaces;
  
namespace Quarcode.View
{
  public partial class FimgView : Form, IViewInterfaces
  {
    public event GenerateMsgQueryDelegate OnMsgGenerateQuery;
    private SViewState viewState = new SViewState();
    private volatile Bitmap sourseImage = null;
    public FimgView()
    {
      InitializeComponent();
      qrImgPictureBox.Image = new Bitmap(1, 1);
      radiusTrackBar.Value = 30;
    }
    void IViewInterfaces.RecieveImg(Bitmap bmp)
    {
      sourseImage = bmp;
      Size s = new System.Drawing.Size();
      s.Height = 700;
      s.Width = s.Height;
      Bitmap ScaledImage = new Bitmap(bmp, s);
      if (qrImgPictureBox.InvokeRequired)
        qrImgPictureBox.BeginInvoke(new Action<Bitmap>((x) => { qrImgPictureBox.Image = x; qrImgPictureBox.Refresh(); }), ScaledImage);
      else
      {
        qrImgPictureBox.Image = ScaledImage;
        qrImgPictureBox.Refresh();
      }
    }

    private void GenerateQRButton_Click(object sender, EventArgs e)
    {
      if (qrMessageTextBox.Text.Length == 0)
      {
        qrMessageTextBox.Text = CCoder.genMsg();
      }
      viewState.DrawQRBorder = DrawBorder.Checked;
      viewState.DrawCellBorder = DrawCellBorder.Checked;
      viewState.DrawValNum = DrawValNum.Checked;
      viewState.FillCells = FillCells.Checked;

      viewState.Message = qrMessageTextBox.Text;
      viewState.ReRand = ReRand.Checked;
      if (OnMsgGenerateQuery != null) OnMsgGenerateQuery(viewState);
    }

    private void qrImgPictureBox_Paint(object sender, PaintEventArgs e)
    {
      qrImgPictureBox.Width = qrImgPictureBox.Image.Width;
      qrImgPictureBox.Height = qrImgPictureBox.Image.Height;
    }

    private void SaveToBMPButton_Click(object sender, EventArgs e)
    {

      SaveFileDialog dialog = new SaveFileDialog();
      dialog.Filter = "png files (*.png)|*.png";
      dialog.Title = "Export in png format"; 
      switch (dialog.ShowDialog())
      {
        case System.Windows.Forms.DialogResult.OK:
          CImgBuilder.saveToFile((Bitmap)qrImgPictureBox.Image, dialog.FileName);
          break;
      }

      MessageBox.Show("functinal will be implemented soon");
    }

    private void RandomStringButton_Click(object sender, EventArgs e)
    {
      qrMessageTextBox.Text = CCoder.genMsg();
    }

    private void qrMessageTextBox_TextChanged(object sender, EventArgs e)
    {
      viewState.Message = qrMessageTextBox.Text;
    }

    private void radiusTrackBar_Scroll(object sender, EventArgs e)
    {
      viewState.radius = radiusTrackBar.Value;
    }

    private void FimgView_Load(object sender, EventArgs e)
    {

    }

    private void ReRand_CheckedChanged(object sender, EventArgs e)
    {
      viewState.radius = radiusTrackBar.Value;
    }
  }
}
