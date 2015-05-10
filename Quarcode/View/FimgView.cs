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

    private void RandomStringButton_Click(object sender, EventArgs e)
    {
      qrMessageTextBox.Text = genMsg();
    }
    String genMsg()
    {
      // lowChars 97-122
      // highChars 65 - 90
      // nums 48 - 57
      String k = "";
      Random rnd = new Random();
      int l = 0;
      l = rnd.Next(0, 3);

      for (int i = 0; i < 12; i++)
      {
        switch (l)
        {
          case 0: k = k + (char)(rnd.Next(48, 58)); break;
          case 1: k = k + (char)(rnd.Next(97, 123)); break;
          case 2: k = k + (char)(rnd.Next(65, 91)); break;
        }
        l = rnd.Next(0, 3);
      }
      return k;
    }
  }
}
