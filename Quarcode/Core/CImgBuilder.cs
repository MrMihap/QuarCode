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
      Brush redline = new SolidBrush(Color.Red);
      matrix.GenNoise();
      Bitmap bmp = new Bitmap(matrix.Width, matrix.Heigt);
      if (!matrix.IsInited)
        using (Graphics gr = Graphics.FromImage(bmp))
        {
          Pen fatPen = new Pen(redline, 5);
          gr.DrawLine(fatPen, 0, 0, bmp.Width, bmp.Height);
          gr.DrawLine(fatPen, 0, bmp.Height, bmp.Width, 0);
        }
      else
      {
        using (Graphics gr = Graphics.FromImage(bmp))
        {
          gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
          
          DrawBorderBackground(gr, matrix, viewState);
          DrawBytes(gr, matrix, viewState);
          //DrawLogo(gr, matrix, viewState);
          DrawPoints(gr, matrix, viewState);

//#if DEBUG
          Brush blueline = new SolidBrush(Color.Red);
          Pen borderPointsPen = new Pen(blueline, 3);
          if (true)
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
          if (false)
            for (int i = 0; i < matrix.LogoPoints.Count; i++)
            {
              //Отрисовка центральных точек ЛОГО
              gr.DrawLine(borderPointsPen,
                (int)matrix.LogoPoints[i].x,
                (int)(int)matrix.LogoPoints[i].y,
                (int)matrix.LogoPoints[i].x + 2,
                (int)(int)matrix.LogoPoints[i].y + 2);
              gr.DrawString(i.ToString(),
                new Font("Calibri", 16f),
                new SolidBrush(Color.Red),
               (int)matrix.LogoPoints[i].x,
               (int)(int)matrix.LogoPoints[i].y);
            }
//#endif
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
    private static void DrawBytes(Graphics gr, CPointsMatrix matrix, SViewState viewState)
    {
      Random rand = new Random();
      //Рисуем черный бордер
      if (viewState.DrawBorder)
        gr.FillPolygon(new SolidBrush(CCoder.GetColorFor(PointType.Border)), Vector.ToSystemPointsF(matrix.BorderPoints.ToArray()));
      for (int i = 0; i < matrix.Points.Count; i++)
      {
#if DEBUG
            List<int> drawlist = new List<int>();
            //drawlist.Add(0);
            //drawlist.Add(57);
            drawlist.Add(48);

            //if (!drawlist.Contains(i)) continue;
#endif

        //Получаем список окружающих точек
        Vector[] aroundgex = matrix.AroundVoronojGexAt(i);
        // Заливаем поле по окружающим точкам
        if (viewState.FillCells)
          gr.FillPolygon(new SolidBrush(CCoder.GetColorFor(PointType.ByteTrue)), Vector.ToSystemPointsF(aroundgex));
        // Отрисовываем границу по окружающим точкам
        if (aroundgex.Length > 2 && viewState.DrawCellBorder)
          gr.DrawPolygon(new Pen(new SolidBrush(CCoder.GetColorFor(PointType.Border))), Vector.ToSystemPointsF(aroundgex));


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
             new SolidBrush(Color.Black),
             (int)aroundgex[j].x + 2,
             (int)aroundgex[j].y + 2);
        }
      }
    }

    private static void DrawLogo(Graphics gr, CPointsMatrix matrix, SViewState viewState)
    {
           //Рисуем черный бордер
      if (viewState.DrawBorder)
        gr.FillPolygon(new SolidBrush(CCoder.GetColorFor(PointType.Border)), Vector.ToSystemPointsF(matrix.BorderPoints.ToArray()));
      for (int i = matrix.Points.Count; i < matrix.Points.Count + matrix.LogoPoints.Count; i++)
      {
        //Получаем список окружающих точек
        Vector[] aroundgex = matrix.AroundVoronojGexAt(i);
        // Заливаем поле по окружающим точкам
        if (viewState.FillCells)
          gr.FillPolygon(new SolidBrush(CCoder.GetColorFor(PointType.Logo)), Vector.ToSystemPointsF(aroundgex));
        // Отрисовываем границу по окружающим точкам
        if (aroundgex.Length > 2 && viewState.DrawCellBorder)
          gr.DrawPolygon(new Pen(new SolidBrush(CCoder.GetColorFor(PointType.Border))), Vector.ToSystemPointsF(aroundgex));


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
             new SolidBrush(Color.Black),
             (int)aroundgex[j].x + 2,
             (int)aroundgex[j].y + 2);
        }
      }
    }

    private static void DrawBorderBackground(Graphics gr, CPointsMatrix matrix, SViewState viewState)
  {
      Random rand = new Random();
      //Рисуем черный бордер
      if (viewState.DrawBorder)
        gr.FillPolygon(new SolidBrush(CCoder.GetColorFor(PointType.Border)), Vector.ToSystemPointsF(matrix.BorderPoints.ToArray()));
  }

    private static void DrawPoints(Graphics gr, CPointsMatrix matrix, SViewState viewState)
    {
      Brush redline = new SolidBrush(Color.Red);
      Pen innerPointsPen = new Pen(redline, 3);
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
    }
  }
}
