using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;

namespace Quarcode.Core
{
  public static class CImgBuilder
  {
    public static Bitmap GenBMPQRfromMatrix(CPointsMatrix matrix, SViewState viewState)
    {
      Bitmap bmp;
      try
      {
        bmp = new Bitmap(matrix.Width, matrix.Height);
      }
      catch
      {
        return new Bitmap(1, 1);
      }
      using (Graphics gr = Graphics.FromImage(bmp))
      {

        gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        gr.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
        gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

        gr.FillRectangle(new SolidBrush(CCoder.GetColorFor(PointType.Logo)), 0, 0, matrix.Width, matrix.Height);

        DrawBorderBackground(gr, matrix, viewState);
        DrawBytes(gr, matrix, viewState);
        DrawLogo(gr, matrix, viewState);
        DrawPoints(gr, matrix, viewState);
        //DrawLogoPoints(gr, matrix, viewState);

      
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
      Bitmap fullimage = new Bitmap(@"img\HexFrame.png");
      using (Graphics gr = Graphics.FromImage(fullimage))
      {
        gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
        double resize = (596.0 - 16 * 2.0 ) / (bmp.Width);
        Bitmap resizedBmp = new Bitmap(bmp, new Size((int)(bmp.Width * resize), (int)(bmp.Height * resize)));
        gr.DrawImage(resizedBmp, new Point(16, 16));
      }
      return fullimage;
    }

    private static void DrawBytes(Graphics gr, CPointsMatrix matrix, SViewState viewState)
    {
      // Значащие биты
      List<CGexPoint> ValueBits = matrix.DrawData.Where(
       x => x.pointType == PointType.ByteTrue ||
         x.pointType == PointType.ByteFalse ||
         x.pointType == PointType.UndefinedByte).ToList();
      List<bool> databitsvalue = CCoder.EnCode(viewState.Message, ValueBits.Count);

      for (int i = 0; i < ValueBits.Count; i++)
      {
        if (ValueBits[i].Cell.Count > 2)
        {
          ValueBits[i].pointType = databitsvalue[i] ? PointType.ByteTrue : PointType.ByteFalse;
          if (viewState.FillCells)
            gr.FillPolygon(new SolidBrush(CCoder.GetColorFor(matrix.DrawData[i].pointType)),
              Vector.ToSystemPointsF(ValueBits[i].Cell.ToArray()));
          if (viewState.DrawCellBorder)
            gr.DrawPolygon(new Pen(new SolidBrush(CCoder.GetColorFor(PointType.Border)), matrix.Height / (350f)),
              Vector.ToSystemPointsF(ValueBits[i].Cell.ToArray()));
        }
      }
    }

    private static void DrawLogo(Graphics gr, CPointsMatrix matrix, SViewState viewState)
    {
      // Логотип
      List<CGexPoint> logoBits = matrix.DrawData.Where(x => x.pointType == PointType.Logo).ToList();

      for (int i = 0; i < logoBits.Count; i++)
      {
        if (logoBits[i].Cell.Count > 2)
        {
          if (viewState.FillCells)
            gr.FillPolygon(
              new SolidBrush(CCoder.GetColorFor(logoBits[i].pointType)),
              Vector.ToSystemPointsF(logoBits[i].Cell.ToArray()));
          if (viewState.DrawCellBorder)
            gr.DrawPolygon(
              new Pen(new SolidBrush(Color.Gray), matrix.Height / (350f)),
              Vector.ToSystemPointsF(logoBits[i].Cell.ToArray()));
        }
      }
    }

    private static void DrawBorderBackground(Graphics gr, CPointsMatrix matrix, SViewState viewState)
    {
      //Рисуем черный бордер
      Vector[] border = (from p in matrix.DrawData where p.pointType == PointType.Border select p.r).ToArray();
      if (viewState.DrawQRBorder)
        gr.FillPolygon(new SolidBrush(CCoder.GetColorFor(PointType.Border)), Vector.ToSystemPointsF(border));
    }

    private static void DrawPoints(Graphics gr, CPointsMatrix matrix, SViewState viewState)
    {
      Brush redline = new SolidBrush(Color.Red);
      Pen innerPointsPen = new Pen(redline, matrix.Height / 350f);
      if (viewState.DrawValNum)
        for (int i = 0; i < matrix.DrawData.Count; i++)
        {
          //Отрисовка центральных точек данных
          //gr.DrawLine(innerPointsPen,
          //  (int)matrix.Points[i].x,
          //  (int)matrix.Points[i].y,
          //  (int)matrix.Points[i].x + 2,
          //  (int)matrix.Points[i].y + 2);

          gr.DrawString(i.ToString(),
            new Font("Sans Serif", matrix.Height / 70f),
            new SolidBrush(Color.Black),
           (int)matrix.DrawData[i].r.x,
           (int)(int)matrix.DrawData[i].r.y);
          // Отрисовка сдвинутых точек

          gr.DrawLine(innerPointsPen,
           (int)matrix.DrawData[i].r.x,
           (int)matrix.DrawData[i].r.y,
           (int)matrix.DrawData[i].r.x + matrix.Height / 350f,
           (int)matrix.DrawData[i].r.y + matrix.Height / 350f);
        }
    }

    private static void DrawLogoPoints(Graphics gr, CPointsMatrix matrix, SViewState viewState)
    {
      //Brush redline = new SolidBrush(Color.Red);
      //Pen innerPointsPen = new Pen(redline, 3);
      //if (viewState.DrawValNum)
      //  for (int i = matrix.Points.Count; i < matrix.Points.Count + matrix.LogoPoints.Count; i++)
      //  {
      //    //Отрисовка центральных точек данных
      //    gr.DrawString(i.ToString(),
      //    new Font("Sans Serif", 10f),
      //    new SolidBrush(Color.Black),
      //   (int)matrix.NoisedPoints[i].x,
      //   (int)matrix.NoisedPoints[i].y);
      //    // Отрисовка сдвинутых точек

      //    gr.DrawLine(innerPointsPen,
      //     (int)matrix.NoisedPoints[i].x,
      //     (int)matrix.NoisedPoints[i].y,
      //     (int)matrix.NoisedPoints[i].x + 2,
      //     (int)matrix.NoisedPoints[i].y + 2);
      //  }
    }

    private static void DrawBorderPoints(Graphics gr, CPointsMatrix matrix, SViewState viewState)
    {
      Brush redline = new SolidBrush(Color.Red);
      Pen innerPointsPen = new Pen(redline, 3);
      if (viewState.DrawValNum)
        for (int i = matrix.Points.Count; i < matrix.Points.Count + matrix.LogoPoints.Count; i++)
        {
          //Отрисовка центральных точек данных
          gr.DrawString(i.ToString(),
          new Font("Sans Serif", 10f),
          new SolidBrush(Color.Black),
         (int)matrix.NoisedPoints[i].x,
         (int)matrix.NoisedPoints[i].y);
          // Отрисовка сдвинутых точек

          gr.DrawLine(innerPointsPen,
           (int)matrix.NoisedPoints[i].x,
           (int)matrix.NoisedPoints[i].y,
           (int)matrix.NoisedPoints[i].x + 2,
           (int)matrix.NoisedPoints[i].y + 2);
        }
    }

    public static void saveToFile(Bitmap img, string filepath)
    {
      using (Graphics gr = Graphics.FromImage(img))
      {
        gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        gr.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
        gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

        img.Save(filepath, ImageFormat.Png);

      }
    }
  }

}
