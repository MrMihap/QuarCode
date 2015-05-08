using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Quarcode.Core
{
  static class CImgBuilder
  {
    public static Bitmap GenQRfromMatrix(CPointsMatrix matrix)
    {
      Bitmap bmp = new Bitmap(matrix.Width, matrix.Heigt);
      using (Graphics gr = Graphics.FromImage(bmp))
      {
        Brush redline = new SolidBrush(Color.Red);
        Pen fatPen = new Pen(redline, 5);
        gr.DrawLine(fatPen, 0, 0, bmp.Width, bmp.Height);
        gr.DrawLine(fatPen, 0, bmp.Height, bmp.Width, 0);
      }
      return bmp;
    }
  }
}
