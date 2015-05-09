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

namespace Quarcode
{
  public partial class FimgView : Form, IViewInterfaces
  {
    public event GenerateMsgQueryDelegate OnMsgGenerateQuery;
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
      if (OnMsgGenerateQuery != null) OnMsgGenerateQuery(qrMessageTextBox.Text);
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

  }
}
