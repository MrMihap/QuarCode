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
    public void IViewInterfaces.RecieveImg(Bitmap bmp)
    {
    }

  }
}
