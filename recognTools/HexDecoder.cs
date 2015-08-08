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
namespace recognTools
{
  class HexDecoder
  {

    /// <summary>
    /// Пытается найти валидируемый код на обрезанном по контору гекс кода изображении
    /// </summary>
    /// <param name="sourse">Cropted Hex Image</param>
    /// <returns> возвращает прочитанный валидированный код или null</returns>
    public static string TryDecode(Image<Bgr, Byte> sourse)
    {
      return null;
    }
    public static Image<Hsv, Byte> Filter(Image<Bgr, Byte> sourse)
    {
      // 1.Gauss 
      //sourse.SmoothGaussian(7);
      sourse = sourse.SmoothMedian(9);
      // 2.Color filter
      byte[, ,] data = sourse.Data;

      Image<Bgr, Byte> TMP = new Image<Bgr, byte>(sourse.Width, sourse.Height);
      byte[, ,] dst = TMP.Data;

      bool IsBlack;
      //const double blueProp = 1.8;
      //const double greeProp = 2.5;
      //const double redProp = 2.5;
      //const byte treshhold = 15;
      const int grayMax = 110; // превышение этого уровня по любому каналу означет отсутствие черного
      const byte zeroLevel = 70;//уровень выше которого цвет проверяется на отклонение от серого
      const byte colorDif = 35; // максимальное отклонение между цветами
      for (int i = sourse.Rows - 1; i >= 0; i--)
      {
        for (int j = sourse.Cols - 1; j >= 0; j--)
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
          /*if (IsBlack)
          {
              int debug = 0;
          }*/
          if (b >= zeroLevel || g >= zeroLevel || r >= zeroLevel)
          {
            //IsBlack = false;

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
      Image<Bgr, Byte> filteredimage = new Image<Bgr, byte>(sourse.ToBitmap());
      filteredimage.Data = dst;
      //filteredimage = filteredimage.SmoothMedian(9);
      return filteredimage.Convert<Hsv, Byte>();
    }

    public static VectorOfVectorOfPoint FindAllContours(Image<Hsv, Byte> sourse)
    {
      Image<Gray, Byte> maskHsvBlack;
      Image<Gray, Byte> maskHsvBlue;

      Hsv blueVal_min = new Hsv(0, 50, 125); Hsv blueVal_max = new Hsv(359.9, 255, 255);
      Hsv blackVal_min = new Hsv(0, 0, 100); Hsv blackVal_max = new Hsv(360, 255, 255);

      // borders
      maskHsvBlack = sourse.InRange(blackVal_min, blackVal_max);
      maskHsvBlack.Erode(3).Dilate(3);
      maskHsvBlack = maskHsvBlack.Not();

      double cannyThreshold = 1.0;
      double cannyThresholdLinking = 500.0;


      Image<Gray, Byte> cannyBlack = maskHsvBlack.Canny(cannyThreshold, cannyThresholdLinking);
      VectorOfVectorOfPoint borders = new VectorOfVectorOfPoint();//list of all borders
      int minimumArea = sourse.Height * sourse.Width / 80;
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

      VectorOfVectorOfPoint result = borders;



      return result;
    }

    public static VectorOfVectorOfPoint FilterAllContours(VectorOfVectorOfPoint sourse)
    {
      VectorOfVectorOfPoint result = new VectorOfVectorOfPoint();
      bool ready = false;
      VectorOfVectorOfPoint contours = sourse;
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

            if (Math.Abs(Areas[i] / Areas[j] - 0.33) < 0.075)
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

    public static Image<Bgr, Byte> CropCodeFromImage(Image<Bgr, Byte> sourse, VectorOfVectorOfPoint Contours)
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
      int bottom = mainPoints.Min(x => x.Y);
      int top = mainPoints.Max(x => x.Y);
      int left = mainPoints.Min(x => x.X);
      int right = mainPoints.Max(x => x.X);
      Rectangle box = new Rectangle(left, bottom, right - left, top - bottom);
      CroptedImage = sourse.GetSubRect(box);
      // 1.Ищем угол поворота относительно вертикали для вертикального выравнивания контуров
      double Angle = 0;
      // 1.1.найдем две точки : самую левую основного контура и самую левую синего  контура
      Point mainLeft = (from p in mainPoints orderby p.X  select p).FirstOrDefault();
      Point subLeft = (from p in subPoints orderby p.X select p).FirstOrDefault();
      Point center = new Point(sourse.Width/2, sourse.Height / 2);
      // 1.2 Ищем угол поворота, который поставит одну над второй
      if (mainLeft.X != subLeft.X)
      {
        double tg = (mainLeft.Y - subLeft.Y) / (double)(mainLeft.X - subLeft.X);

        Angle = Math.Atan(tg) * 180 / Math.PI;
        // случай перевернутого изображения
        if (mainLeft.Y > subLeft.Y) Angle += 180;
      }
      // 1.3 Поворачиваем основное изображение на этот угол
      RotatedImage = CroptedImage.Rotate(Angle, new Bgr(Color.White));
      // 2 Поворачиваем контур на этот угол

      for (int i = 0; i < Contours[idMain].Size; i++)
      {
        // переход в систему координат центра отрезанного изображения из основного
        mainPoints[i].X -= (RotatedImage.Width / 2 + left);
        mainPoints[i].Y -= (RotatedImage.Height / 2 + bottom);
        // умножение на матрицу поворота
        Point old = mainPoints[i];
        mainPoints[i].X = (int)(old.X * Math.Cos(Angle) - old.Y * Math.Sin(Angle));
        mainPoints[i].Y = (int)(-old.X * Math.Sin(Angle) + old.Y * Math.Cos(Angle));
        // обратный переход в координаты отрезанного изображения 
        mainPoints[i].X += (RotatedImage.Width / 2 + left) - left;
        mainPoints[i].Y += (RotatedImage.Height / 2 + bottom) -bottom;
      }
      // 3.Вырезаем основной контур(повернутый) из обрезанного и повернутого изображения
      bottom = mainPoints.Min(x => x.Y);
      top = mainPoints.Max(x => x.Y);
      left = mainPoints.Min(x => x.X);
      right = mainPoints.Max(x => x.X);
      box = new Rectangle(left, bottom, right - left, top - bottom);
      // При некорректном контуре падает
      //CroptedImage = RotatedImage.GetSubRect(box);

      //Раскоментировать при победе над багами
      //return CroptedImage;
      RotatedImage.DrawPolyline(mainPoints, true, new Bgr(Color.Red), 2);
      return RotatedImage;
    }

  }
}
