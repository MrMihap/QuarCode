﻿using System;
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

              gr.FillPolygon(new SolidBrush(rndclr), Vector.ToSystemPointsF(aroundgex));
              gr.DrawLine(new Pen(new SolidBrush(Color.Green), 3),
                (int)aroundgex[j].x,
                (int)(int)aroundgex[j].y,
                (int)aroundgex[j].x + 2,
                (int)(int)aroundgex[j].y + 2);
            }
          }
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
          for (int i = 0; i < matrix.BorderPoints.Count; i++)
          {
#if DEBUG
            if (i != 0) continue;
#endif
            Vector[] aroundgex = matrix.AroundGexAt(i);
            for (int j = 0; j < 6; j++)
            {
              gr.DrawLine(borderPointsPen,
                (int)aroundgex[j].x,
                (int)matrix.Heigt - (int)aroundgex[j].y,
                (int)aroundgex[j].x + 2,
                (int)matrix.Heigt - (int)aroundgex[j].y + 2);
            }
          }
          //DEBUG
          for (int i = 0; i < matrix.Points.Count; i++)
          {
#if DEBUG
            if (i != 0) continue;
#endif
            int[] points = matrix.sixNearest(i);
            for (int jj = 0; jj < points.Length; jj++)
            {
              gr.DrawLine(new Pen(new SolidBrush(Color.Green)),
                (int)matrix.VectorAt(points[jj]).x + 2,
                (int)matrix.Heigt - (int)matrix.VectorAt(points[jj]).y + 2,
                (int)matrix.VectorAt(points[jj]).x + 4,
                (int)matrix.Heigt - (int)matrix.VectorAt(points[jj]).y + 4);
              gr.DrawString(jj.ToString(),
                new Font("Sans Serif", 16f),
                new SolidBrush(Color.Green),
               (int)matrix.VectorAt(points[jj]).x + 5,
               (int)matrix.Heigt - (int)matrix.VectorAt(points[jj]).y - 5);
            }
          }
          //END DEBUG

        }
      }
      return bmp;
    }
  }
}
