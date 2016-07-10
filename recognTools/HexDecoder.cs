using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.CV.CvEnum;

using Quarcode.Core;
namespace recognTools
{
  class HexDecoder
  {

    /// <summary>
    /// Пытается найти валидируемый код на обрезанном по контору гекс кода изображении
    /// </summary>
    /// <param name="source">Cropted Hex Image</param>
    /// <returns> возвращает прочитанный валидированный код или null</returns>
    public static string TryDecode(Image<Bgr, Byte> source)
    {
      int threshold = 130;
      int r = 0, g = 0, b = 0;
      source.SmoothMedian(5);
      CPointsMatrix matrixold = new CPointsMatrix(source.Height);
      CPointsMatrix2 matrix= new CPointsMatrix2();
      byte[, ,] dst = source.Data;
      List<bool> bitlist = new List<bool>();

      // K.O.S.T.J.L methods here

      // выраниваем гекс по центру
      PointF[] data = Vector.ToSystemPointsF(matrixold.Points.ToArray());
      for (int i = 0; i < data.Length; i++)
      {
        data[i].Y = source.Height - data[i].Y;
      }
      PointF shift = new PointF(data[0].X - source.Width / 2, data[0].Y - source.Height / 2);

      for (int i = 0; i < data.Length; i++)
      {
        data[i].X -= shift.X  - 10;
        data[i].Y -= shift.Y;
      }
      // end
      for (int i = 0; i < 136; i++)
      {

        source.Draw(new Ellipse(data[i], new SizeF(3, 3), 1.5f), new Bgr(Color.Red), 1);
        source.Draw(i.ToString(), new Point((int)data[i].X, (int)data[i].Y), FontFace.HersheyDuplex, 0.4, new Bgr(Color.Black), thickness: 1);
        int ii = (int)data[i].Y;
        int jj = (int)data[i].X;
        b = 0;
        g = 0;
        r = 0;
        for (int ishift = -2; ishift <= 2; ishift++)
        {
          for (int jshift = -2; jshift <= 2; jshift++)
          {
            b += dst[ii + ishift, jj + jshift, 0];
            g += dst[ii + ishift, jj + jshift, 1];
            r += dst[ii + ishift, jj + jshift, 2];
          }
        }
        b /= 25;
        g /= 25;
        r /= 25;
        
        //debug
        if (i == 11)
        {
        }
        if (b > threshold && g > threshold && r > threshold)
          bitlist.Add(false);
        else
          bitlist.Add(true);
        if (b > threshold && g > threshold && r > threshold)
          source.Draw(new Ellipse(data[i], new SizeF(1, 1), 1.5f), new Bgr(Color.Black), 1);
        else
          source.Draw(new Ellipse(data[i], new SizeF(1, 1), 1.5f), new Bgr(Color.Chocolate), 1);
      }
      string fullResult = CCoder.DeCode(bitlist);
      string messageCandidate = fullResult.Substring(0, 12);
      string md5Part = fullResult.Substring(12, 10);
      string md5Calc = CCoder.GetMd5Sum(messageCandidate).Substring(0, 10);

      if (md5Calc.Equals(md5Part))
        return messageCandidate;
      else
        return null;
    }

    public static Image<Hsv, Byte> Filter(Image<Bgr, Byte> source)
    {
      // 1.Gauss 
      //source.SmoothGaussian(7);
      //source = source.SmoothMedian(9);
      // 2.Color filter
      byte[, ,] data = source.Data;

      Image<Bgr, Byte> TMP = new Image<Bgr, byte>(source.Width, source.Height);
      //source.Dispose();
      byte[, ,] dst = TMP.Data;

      bool IsBlack;
      //const double blueProp = 1.8;
      //const double greeProp = 2.5;
      //const double redProp = 2.5;
      //const byte treshhold = 15;
      const int grayMax = 110; // превышение этого уровня по любому каналу означет отсутствие черного
      const byte zeroLevel = 70;//уровень выше которого цвет проверяется на отклонение от серого
      const byte colorDif = 30; // максимальное отклонение между цветами
      for (int i = TMP.Rows - 1; i >= 0; i--)
      {
        for (int j = TMP.Cols - 1; j >= 0; j--)
        {
          IsBlack = true;
          // 0 - blue
          // 1 - green
          // 2 - red
          //new great, god like code
          //debug
          if (i == 847 && j == 1580)
          {
            //debug here
          }

          byte b = data[i, j, 0];
          byte g = data[i, j, 1];
          byte r = data[i, j, 2];
          if (b > grayMax)
            IsBlack = false;
          if (g > grayMax)
            IsBlack = false;
          if (r > grayMax)
            IsBlack = false;
          if (b >= zeroLevel || g >= zeroLevel || r >= zeroLevel)
          {
            //too many blue
            if (b > r + colorDif || b > g + colorDif)
              IsBlack = false;
            //too many green
            if (g > r + colorDif || g > b + colorDif)
              IsBlack = false;
            //too many red
            if (r > b + colorDif || r > g + colorDif)
              IsBlack = false;
          }
          if (IsBlack)
          {
            dst[i, j, 0] = dst[i, j, 1] = dst[i, j, 2] = 0;
          }
          else
          {
            dst[i, j, 0] = dst[i, j, 1] = dst[i, j, 2] = 255;
          }
        }
      }

      // 3.To HSV
      Image<Bgr, Byte> filteredimage = new Image<Bgr, byte>(TMP.ToBitmap());
      filteredimage.Data = dst;
      filteredimage = filteredimage.SmoothMedian(5);
      return filteredimage.Convert<Hsv, Byte>();
    }

    public static VectorOfVectorOfPoint FindAllContours(Image<Hsv, Byte> source)
    {
      Image<Gray, Byte> maskHsvBlack;

      Hsv blueVal_min = new Hsv(0, 50, 125); Hsv blueVal_max = new Hsv(359.9, 255, 255);
      Hsv blackVal_min = new Hsv(0, 0, 100); Hsv blackVal_max = new Hsv(360, 255, 255);

      // borders
      maskHsvBlack = source.InRange(blackVal_min, blackVal_max);
      maskHsvBlack.Erode(3).Dilate(3);
      maskHsvBlack = maskHsvBlack.Not();

      double cannyThreshold = 1.0;
      double cannyThresholdLinking = 500.0;


      Image<Gray, Byte> cannyBlack = maskHsvBlack.Canny(cannyThreshold, cannyThresholdLinking);
      VectorOfVectorOfPoint borders = new VectorOfVectorOfPoint();//list of all borders
      int minimumArea = source.Height * source.Width / 80;
      using (VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint())
      {
        CvInvoke.FindContours(cannyBlack, contours, null, RetrType.List, ChainApproxMethod.ChainApproxSimple);
        //cannyBlack = resultImg.Convert<Gray, Byte>();
        for (int i = 0; i < contours.Size; i++)
        {
          using (VectorOfPoint contour = contours[i])
          using (VectorOfPoint approxContour = new VectorOfPoint())
          {
            CvInvoke.ApproxPolyDP(contour, approxContour, CvInvoke.ArcLength(contour, true) * 0.05, true);
            if (CvInvoke.ContourArea(approxContour, false) > minimumArea) //only consider contours with area greater than 250
            {
              //if (approxContour.Size <= 45)
              //{
              borders.Push(contour);
              //Black_boxList.Add(CvInvoke.MinAreaRect(approxContour)); 
              //}
            }
          }
        }
      }
      return borders;
    }

    public static VectorOfVectorOfPoint FilterAllContours(VectorOfVectorOfPoint source)
    {
      VectorOfVectorOfPoint result = new VectorOfVectorOfPoint();
      bool ready = false;
      VectorOfVectorOfPoint contours = source;
      double[] Areas = new double[contours.Size];
      for (int i = 0; i < contours.Size; i++)
      {
        Areas[i] = CvInvoke.ContourArea(contours[i]);
      }
      for (int i = 0; i < contours.Size && !ready; i++)
      {
        using (VectorOfPoint contour = contours[i])
        {
          for (int j = 1; j < contours.Size && !ready; j++)
          {
            if (Math.Abs(Areas[i] / Areas[j] - 0.33) < 0.1)
            {
              result.Push(contours[j]);
              result.Push(contours[i]);
              ready = !ready;
            }
          }
        }
      }
      return result;
    }

    public static Image<Bgr, Byte> CropCodeFromImage(Image<Bgr, Byte> source, VectorOfVectorOfPoint Contours)
    {
      Image<Bgr, Byte> CroptedImage;
      int idMain = 0;
      int idBlue = 1;
      if (CvInvoke.ContourArea(Contours[0]) < CvInvoke.ContourArea(Contours[1]))
      {
        idMain = 1;
        idBlue = 0;
      }
      Point[] mainPoints = Contours[idMain].ToArray();
      Point[] subPoints = Contours[idBlue].ToArray();
      Image<Bgr, Byte> RotatedImage;
      // 0.Вырежем прямоугольник описанный вокруг основного контура(в целях оптимизации)
      Point[] contourRectangle = new Point[4];
      int bottom = mainPoints.Min(x => x.Y);
      contourRectangle[0] = mainPoints.Where(p => p.Y == bottom).First();

      int left = mainPoints.Min(x => x.X);
      contourRectangle[1] = mainPoints.Where(p => p.X == left).First();

      int top = mainPoints.Max(x => x.Y);
      contourRectangle[2] = mainPoints.Where(p => p.Y == top).First();

      int right = mainPoints.Max(x => x.X);
      contourRectangle[3] = mainPoints.Where(p => p.X == right).First();

      Rectangle box = new Rectangle(left, bottom, right - left, top - bottom);
      CroptedImage = source;//.GetSubRect(box);
      // 1.Ищем угол поворота относительно вертикали для вертикального выравнивания контуров
      double Angle = 0;
      // 1.1.найдем две точки : самую левую основного контура и самую левую синего  контура
      Point mainLeft = (from p in mainPoints orderby p.X select p).FirstOrDefault();
      Point mainBottom = (from p in mainPoints orderby p.Y descending select p).FirstOrDefault();
      Point subLeft = (from p in subPoints orderby p.X select p).FirstOrDefault();
      Point center = new Point(CroptedImage.Width / 2, CroptedImage.Height / 2);
      // 1.2 Ищем угол поворота, который поставит одну над второй
      if (mainLeft.X != subLeft.X)
      {
        double tg = -(mainLeft.Y - subLeft.Y) / (double)(mainLeft.X - subLeft.X);

        Angle = Math.Atan(tg) + Math.PI / 2;
      }
      #region вертикальное выравнивание
      // случай перевернутого изображения
      mainLeft.X -= center.X;
      mainLeft.Y -= center.Y;
      // умножение на матрицу поворота
      Point old = new Point(mainLeft.X, mainLeft.Y);
      mainLeft.X = (int)(old.X * Math.Cos(Angle) - old.Y * Math.Sin(Angle));
      mainLeft.Y = (int)(+old.X * Math.Sin(Angle) + old.Y * Math.Cos(Angle));
      // обратный переход в координаты отрезанного изображения 
      mainLeft.X += center.X;
      mainLeft.Y += center.Y;
      // случай перевернутого изображения
      subLeft.X -= center.X;
      subLeft.Y -= center.Y;
      // умножение на матрицу поворота
      old = new Point(subLeft.X, subLeft.Y);
      subLeft.X = (int)(old.X * Math.Cos(Angle) - old.Y * Math.Sin(Angle));
      subLeft.Y = (int)(+old.X * Math.Sin(Angle) + old.Y * Math.Cos(Angle));
      // обратный переход в координаты отрезанного изображения 
      subLeft.X += center.X;
      subLeft.Y += center.Y;
      if (mainLeft.Y > subLeft.Y)
        Angle -= Math.PI;
      #endregion
      // 1.3 Поворачиваем основное изображение на этот угол
      /*DEBUG*/
      //source.Draw(contourRectangle, new Bgr(Color.Blue), 3);
      RotatedImage = new Image<Bgr, byte>(CroptedImage.Rotate(Angle * 180 / Math.PI, new Bgr(Color.White), false).Bitmap);

      // 2 Поворачиваем контур на этот угол
      RotateContour(mainPoints, Angle, center,
        new Point(center.X + (RotatedImage.Width - CroptedImage.Width) / 2, center.Y + (RotatedImage.Height - CroptedImage.Height) / 2));

      // 3.Вырезаем основной контур(повернутый) из обрезанного и повернутого изображения
      bottom = mainPoints.Min(x => x.Y);
      top = mainPoints.Max(x => x.Y);
      left = mainPoints.Min(x => x.X);
      right = mainPoints.Max(x => x.X);
      box = new Rectangle(left, bottom, right - left, top - bottom);
      VectorOfVectorOfPoint contoursRoteated = new VectorOfVectorOfPoint();
      contoursRoteated.Push(new VectorOfPoint());
      contoursRoteated.Push(new VectorOfPoint());
      contoursRoteated[0].Push(mainPoints);
      contoursRoteated[1].Push(mainPoints);
      // При некорректном контуре падает
      if (left < 0 || right < 0 || top > RotatedImage.Height || right > RotatedImage.Width)
      {
        RotatedImage.DrawPolyline(mainPoints, true, new Bgr(Color.Red), 2);
        return RotatedImage;
      }
      else
      {
        return CropImage(RotatedImage, contoursRoteated);
        CroptedImage = RotatedImage.GetSubRect(box);
      }
      for (int i = 0; i < mainPoints.Length; i++)
      {
        mainPoints[i].X -= left;
        mainPoints[i].Y -= bottom;
        if (mainPoints[i].X > box.Width) mainPoints[i].X = box.Width;
        if (mainPoints[i].Y > box.Height) mainPoints[i].Y = box.Height;
      }

      return CropImage(CroptedImage, contoursRoteated);

      VectorOfPoint ApproxRect = new VectorOfPoint();
      Contours[idMain].Clear();
      Contours[idMain].Push(mainPoints);
      // Approximation 
      CvInvoke.ApproxPolyDP(Contours[idMain], ApproxRect, CvInvoke.ArcLength(Contours[idMain], true) * 0.15, true);

      MCvMoments moments = CvInvoke.Moments(ApproxRect);
      Point Code_gravityCenter = new Point((int)(moments.M10 / moments.M00), (int)(moments.M01 / moments.M00));

      //corners
      Point[] corners_mainBox = ApproxRect.ToArray();

      //sort corners
      List<Point> leftl = new List<Point>();
      List<Point> rightl = new List<Point>();
      for (int i = 0; i < corners_mainBox.Length; i++)
      {
        if (corners_mainBox[i].X < Code_gravityCenter.X)
          leftl.Add(corners_mainBox[i]);
        else
          rightl.Add(corners_mainBox[i]);
      }


      //matrix with starting Points
      // top-left top-right bottom-right bottom-left
      PointF[] corners = new PointF[4];
      corners[0] = leftl[0].Y < leftl[1].Y ? leftl[0] : leftl[1]; //top-left
      corners[1] = rightl[0].Y < rightl[1].Y ? rightl[0] : rightl[1]; //top-right
      corners[2] = rightl[0].Y > rightl[1].Y ? rightl[0] : rightl[1]; //bottom-right
      corners[3] = leftl[0].Y > leftl[1].Y ? leftl[0] : leftl[1]; //bottom-left
      //create matrixes
      box = CvInvoke.BoundingRectangle(ApproxRect);
      PointF[] targetPF = {
                            new PointF(0,0),
                            new PointF(box.Width, 0),
                            new PointF(box.Width, box.Width),
                            new PointF(0,box.Width),
                          }; //matrix with destination Points

      //rotate crop
      Image<Bgr, Byte> newcroppimg = CroptedImage.Clone();

      //transformation matrix 
      Mat TransformMat = CvInvoke.GetPerspectiveTransform(corners, targetPF);

      Size ROI = box.Size;
      //transformation
      CvInvoke.WarpPerspective(CroptedImage, newcroppimg, TransformMat, ROI);

      return newcroppimg;
    }

    /// <summary>
    /// Альтернативная функция вырезания
    /// </summary>
    /// <param name="source"></param>
    /// <param name="contours"></param>
    /// <returns></returns>
    public static Image<Bgr, Byte> CropImage(Image<Bgr, Byte> source, VectorOfVectorOfPoint contours)
    {
      //additional reordering contours
      VectorOfPoint code_contour;
      VectorOfPoint blue_contour;

      if (contours.Size > 2)
      {
        code_contour = contours[1];
        blue_contour = contours[2];
      }
      else
      {
        code_contour = contours[0];
        blue_contour = contours[1];
      }

      // Approximation 
      CvInvoke.ApproxPolyDP(code_contour, code_contour, CvInvoke.ArcLength(code_contour, true) * 0.10, true);
      CvInvoke.ApproxPolyDP(blue_contour, blue_contour, CvInvoke.ArcLength(blue_contour, true) * 0.10, true);

      // Centers of contours

      MCvMoments moments = CvInvoke.Moments(code_contour);
      Point Code_gravityCenter = new Point((int)(moments.M10 / moments.M00), (int)(moments.M01 / moments.M00));
      moments = CvInvoke.Moments(blue_contour);
      Point Blue_gravityCenter = new Point((int)(moments.M10 / moments.M00), (int)(moments.M01 / moments.M00));


      //corners
      Point[] corners_mainBox = code_contour.ToArray();

      //sort corners
      List<Point> left = new List<Point>();
      List<Point> right = new List<Point>();
      for (int i = 0; i < corners_mainBox.Length; i++)
      {
        if (corners_mainBox[i].X < Code_gravityCenter.X)
          left.Add(corners_mainBox[i]);
        else
          right.Add(corners_mainBox[i]);
      }


      PointF[] corners = new PointF[4]; //matrix with starting Points
      // top-left top-right bottom-right bottom-left
      corners[0] = left[0].Y < left[1].Y ? left[0] : left[1]; //top-left
      corners[1] = right[0].Y < right[1].Y ? right[0] : right[1]; //top-right
      corners[2] = right[0].Y > right[1].Y ? right[0] : right[1]; //bottom-right
      corners[3] = left[0].Y > left[1].Y ? left[0] : left[1]; //bottom-left

      //create matrixes
      Rectangle box = CvInvoke.BoundingRectangle(code_contour);
      PointF[] targetPF = {
                            new PointF(0,0),
                            new PointF(box.Width, 0),
                            new PointF(box.Width, box.Width),
                            new PointF(0,box.Width),
                          }; //matrix with destination Points

      //rotate crop
      Image<Bgr, Byte> newcroppimg = new Image<Bgr, byte>(source.GetSubRect(box).Bitmap);

      //crop image
      Mat TransformMat = CvInvoke.GetPerspectiveTransform(corners, targetPF); //transformation matrix itself
      //Image<Bgr, Byte> newcroppimg = new Image<Bgr, Byte>((int)targetPF[2].X, (int)targetPF[2].Y);

      Size ROI = CvInvoke.BoundingRectangle(code_contour).Size;
      CvInvoke.WarpPerspective(source, newcroppimg, TransformMat, ROI); //transformation itself

      return newcroppimg;
    }

    private static void RotateContour(Point[] src, double Angle, Point firstC, Point secondC)
    {
      for (int i = 0; i < src.Length; i++)
      {
        // переход в систему координат центра 
        src[i].X -= firstC.X;
        src[i].Y -= firstC.Y;
        // умножение на матрицу поворота
        Point old = new Point(src[i].X, src[i].Y);
        src[i].X = (int)(old.X * Math.Cos(Angle) - old.Y * Math.Sin(Angle));
        src[i].Y = (int)(+old.X * Math.Sin(Angle) + old.Y * Math.Cos(Angle));
        // обратный переход в координаты нового центра 
        src[i].X += secondC.X;
        src[i].Y += secondC.Y;

      }
    }
  }
}
