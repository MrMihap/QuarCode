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
          Brush blueline = new SolidBrush(Color.Red);
          Pen innerPointsPen = new Pen(redline, 3);
          Pen borderPointsPen = new Pen(blueline, 3);

          for (int i = 0; i < matrix.Points.Count; i++)
          {
            gr.DrawLine(innerPointsPen,
              (int)matrix.Points[i].x,
              (int)matrix.Heigt - (int)matrix.Points[i].y,
              (int)matrix.Points[i].x + 2,
              (int)matrix.Heigt - (int)matrix.Points[i].y + 2);
            gr.DrawString(i.ToString(),
              new Font("Sans Serif", 10f),
              new SolidBrush(Color.Black),
             (int)matrix.Points[i].x,
             (int)matrix.Heigt - (int)matrix.Points[i].y);
          }
          for (int i = 0; i < matrix.BorderPoints.Count; i++)
          {
            gr.DrawLine(borderPointsPen,
              (int)matrix.BorderPoints[i].x,
              (int)matrix.Heigt - (int)matrix.BorderPoints[i].y,
              (int)matrix.BorderPoints[i].x + 2,
              (int)matrix.Heigt - (int)matrix.BorderPoints[i].y + 2);
            gr.DrawString(i.ToString(),
              new Font("Sans Serif", 16f),
              new SolidBrush(Color.Red),
             (int)matrix.BorderPoints[i].x,
             (int)matrix.Heigt - (int)matrix.BorderPoints[i].y);
          }
          //DEBUG
          int[] points = matrix.sixNearest(0);

          //END DEBUG

          for (int i = 0; i < points.Length; i++)
          {
            gr.DrawLine(borderPointsPen,
              (int)matrix.VectorAt(points[i]).x,
              (int)matrix.Heigt - (int)matrix.VectorAt(points[i]).y,
              (int)matrix.VectorAt(points[i]).x + 2,
              (int)matrix.Heigt - (int)matrix.VectorAt(points[i]).y + 2);
            gr.DrawString(i.ToString(),
              new Font("Sans Serif", 16f),
              new SolidBrush(Color.Red),
             (int)matrix.VectorAt(points[i]).x,
             (int)matrix.Heigt - (int)matrix.VectorAt(points[i]).y);
           
          }
        }
      }
      return bmp;
    }
  }
}
