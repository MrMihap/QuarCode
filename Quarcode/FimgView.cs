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
    }
    void IViewInterfaces.RecieveImg(Bitmap bmp)
    {
      
      if (qrImgPictureBox.InvokeRequired)
        qrImgPictureBox.BeginInvoke(new Action<Bitmap>((x) => qrImgPictureBox.Image = x), bmp);
      else
        qrImgPictureBox.Image = bmp;
    }

    private void GenerateQRButton_Click(object sender, EventArgs e)
    {
      if (OnMsgGenerateQuery != null) OnMsgGenerateQuery(qrMessageTextBox.Text);
    }

  }
}
