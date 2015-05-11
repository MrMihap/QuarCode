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
      if (!matrix.IsInited)
        using (Graphics gr = Graphics.FromImage(bmp))
        {
          Brush redline = new SolidBrush(Color.Red);
          Pen fatPen = new Pen(redline, 5);
          gr.DrawLine(fatPen, 0, 0, bmp.Width, bmp.Height);
          gr.DrawLine(fatPen, 0, bmp.Height, bmp.Width, 0);
        }
      else
      {
        using (Graphics gr = Graphics.FromImage(bmp))
        {
          Brush redline = new SolidBrush(Color.Blue);
          Pen fatPen = new Pen(redline, 2);
          for (int i = 0; i < matrix.Points.Count; i++)
          {
            gr.DrawLine(fatPen, 
              (int)matrix.Points[i].x,
              (int)matrix.Heigt - (int)matrix.Points[i].y, 
              (int)matrix.Points[i].x + 1,
              (int)matrix.Heigt - (int)matrix.Points[i].y + 1);
            gr.DrawString(i.ToString(),
              new Font("Sans Serif", 10f),
              new SolidBrush(Color.Black),
             (int)matrix.Points[i].x,
             (int)matrix.Heigt - (int)matrix.Points[i].y);
          }
        }
      }
      return bmp;
    }
  }
}
