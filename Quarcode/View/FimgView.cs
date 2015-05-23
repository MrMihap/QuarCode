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
    public FimgView()
    {
      InitializeComponent();
      qrImgPictureBox.Image = new Bitmap(1, 1);
    }
    void IViewInterfaces.RecieveImg(Bitmap bmp)
    {

      if (qrImgPictureBox.InvokeRequired)
        qrImgPictureBox.BeginInvoke(new Action<Bitmap>((x) => { qrImgPictureBox.Image = x; qrImgPictureBox.Refresh(); }), bmp);
      else
      {
        qrImgPictureBox.Image = bmp;
        qrImgPictureBox.Refresh();
      }
    }

    private void GenerateQRButton_Click(object sender, EventArgs e)
    {
      viewState.DrawBorder = DrawBorder.Checked;
      viewState.DrawCellBorder = DrawCellBorder.Checked;
      viewState.DrawValNum = DrawValNum.Checked;
      viewState.FillCells = FillCells.Checked;
      if (OnMsgGenerateQuery != null) OnMsgGenerateQuery(viewState);
    }

    private void qrImgPictureBox_Paint(object sender, PaintEventArgs e)
    {
      qrImgPictureBox.Width = qrImgPictureBox.Image.Width;
      qrImgPictureBox.Height = qrImgPictureBox.Image.Height;
    }

    private void SaveToBMPButton_Click(object sender, EventArgs e)
    {
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
  }
}
