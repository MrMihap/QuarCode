using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Quarcode.Core;

namespace Quarcode
{
  public interface IViewInterfaces
  {
    event GenerateMsgQueryDelegate OnMsgGenerateQuery;
    void RecieveImg(Bitmap bmp);
    
  }
}
