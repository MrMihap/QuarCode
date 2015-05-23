using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Svg;
namespace Quarcode.Core
{
  static class CImgBuilder
  {
    public static Bitmap GenQRfromMatrix(CPointsMatrix matrix, SViewState viewState)
    {
      matrix.GenNoise();
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
          Random rand = new Random();
          Color rndclr = Color.Black;
          //Рисуем черный бордер
          if (viewState.DrawBorder)
            gr.FillPolygon(new SolidBrush(rndclr), Vector.ToSystemPointsF(matrix.BorderPoints.ToArray()));
          for (int i = 0; i < matrix.Points.Count; i++)
          {
#if DEBUG
            List<int> drawlist = new List<int>();
            //drawlist.Add(0);
            //drawlist.Add(57);
            drawlist.Add(48);

            if (!drawlist.Contains(i)) continue;
#endif
            switch (rand.Next() % 7)
            {
              case 0:
                rndclr = Color.DeepSkyBlue;
                break;
              case 1:
                rndclr = Color.DeepPink;
                break;
              case 2:
                rndclr = Color.Aquamarine;
                break;
              case 3:
                rndclr = Color.Brown;
                break;
              case 4:
                rndclr = Color.SlateGray;
                break;
              case 5:
                rndclr = Color.RoyalBlue;
                break;
              case 6:
                rndclr = Color.Orchid;
                break;

            }
            //Получаем список окружающих точек
            Vector[] aroundgex = matrix.AroundVoronojGexAt(i);
            // Заливаем поле по окружающим точкам
            if (viewState.FillCells)
              gr.FillPolygon(new SolidBrush(rndclr), Vector.ToSystemPointsF(aroundgex));
            // Отрисовываем границу по окружающим точкам
            if (aroundgex.Length > 2 && viewState.DrawCellBorder)
              gr.DrawPolygon(new Pen(new SolidBrush(Color.Red)), Vector.ToSystemPointsF(aroundgex));
            if (false)
              for (int ii = 0; ii < matrix.LastSurround.Count; ii++)
              {
                gr.DrawLine(new Pen(new SolidBrush(Color.Red), 3),
                  (int)matrix.LastSurround[ii].x,
                  (int)matrix.LastSurround[ii].y,
                  (int)matrix.LastSurround[ii].x + 2,
                  (int)matrix.LastSurround[ii].y + 2);
              }
            for (int j = 0; j < aroundgex.Length; j++)
            {
              // Ставим точку

              if (false)
                gr.DrawLine(new Pen(new SolidBrush(Color.Green), 3),
                  (int)aroundgex[j].x,
                  (int)aroundgex[j].y,
                  (int)aroundgex[j].x + 2,
                  (int)aroundgex[j].y + 2);
              //номер в округе гекса
              if (false)
                gr.DrawString(j.ToString(),
                 new Font("Sans Serif", 10f),
                 new SolidBrush(rndclr),
                 (int)aroundgex[j].x + 2,
                 (int)aroundgex[j].y + 2);
            }
          }
          if (viewState.DrawValNum)
            for (int i = 0; i < matrix.Points.Count; i++)
            {
              //Отрисовка центральных точек данных
              //gr.DrawLine(innerPointsPen,
              //  (int)matrix.Points[i].x,
              //  (int)matrix.Points[i].y,
              //  (int)matrix.Points[i].x + 2,
              //  (int)matrix.Points[i].y + 2);

              gr.DrawString(i.ToString(),
                new Font("Sans Serif", 10f),
                new SolidBrush(Color.Black),
               (int)matrix.Points[i].x,
               (int)(int)matrix.Points[i].y);
              // Отрисовка сдвинутых точек

              gr.DrawLine(innerPointsPen,
               (int)matrix.NoisedPoints[i].x,
               (int)matrix.NoisedPoints[i].y,
               (int)matrix.NoisedPoints[i].x + 2,
               (int)matrix.NoisedPoints[i].y + 2);
            }
          if (false)
            for (int i = 0; i < matrix.BorderPoints.Count; i++)
            {
              //Отрисовка центральных точек границы
              gr.DrawLine(borderPointsPen,
                (int)matrix.BorderPoints[i].x,
                (int)(int)matrix.BorderPoints[i].y,
                (int)matrix.BorderPoints[i].x + 2,
                (int)(int)matrix.BorderPoints[i].y + 2);
              gr.DrawString(i.ToString(),
                new Font("Sans Serif", 16f),
                new SolidBrush(Color.Red),
               (int)matrix.BorderPoints[i].x,
               (int)(int)matrix.BorderPoints[i].y);
            }

          //Отрисовка места под логотип
          //gr.FillPolygon(new SolidBrush(Color.WhiteSmoke), Vector.ToSystemPointsF(matrix.LogoBorderPoints.ToArray()));
#if DEBUG
          //DEBUG
          // вывод на экран окружения конкретной точки
          if (false)
            for (int i = 0; i < matrix.Points.Count; i++)
            {
              if (i != 5) continue;
              int[] points = matrix.sixNearest(i);
              for (int jj = 0; jj < points.Length; jj++)
              {
                try
                {
                  gr.DrawLine(new Pen(new SolidBrush(Color.Green)),
                    (int)matrix.VectorAt(points[jj]).x + 2,
                    (int)(int)matrix.VectorAt(points[jj]).y + 2,
                    (int)matrix.VectorAt(points[jj]).x + 4,
                    (int)(int)matrix.VectorAt(points[jj]).y + 4);
                  gr.DrawString(jj.ToString(),
                    new Font("Sans Serif", 16f),
                    new SolidBrush(Color.Green),
                   (int)matrix.VectorAt(points[jj]).x + 5,
                   (int)(int)matrix.VectorAt(points[jj]).y - 5);

                }
                catch (Exception e)
                {
                  // do nothing
                }
              }
            }
          //END DEBUG
#endif

        }
      }
      return bmp;
    }
  }
}
