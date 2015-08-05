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
      const int grayMax = 80;
      const double blueProp = 1.8;
      const double greeProp = 2.5;
      const double redProp = 2.5;
      const byte zeroLevel = 0;
      const byte treshhold = 55;
      const byte colorDif = 16;
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
          if (b >= zeroLevel && g >= zeroLevel && r >= zeroLevel)
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
      filteredimage = filteredimage.SmoothMedian(9);
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

      //Blue field
      maskHsvBlue = sourse.InRange(blueVal_min, blueVal_max);
      maskHsvBlue.Erode(3).Dilate(3);

      double cannyThreshold = 1.0;
      double cannyThresholdLinking = 500.0;


      Image<Gray, Byte> cannyBlack = maskHsvBlack.Canny(cannyThreshold, cannyThresholdLinking);
      VectorOfVectorOfPoint borders = new VectorOfVectorOfPoint();//list of all borders
      int minimumArea = sourse.Height * sourse.Width/80;
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
              if (approxContour.Size <= 12)
              {
                borders.Push(contour);
                //Black_boxList.Add(CvInvoke.MinAreaRect(approxContour)); 
              }
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
      using (VectorOfVectorOfPoint contours = sourse)
      {
        for (int i = 0; i < contours.Size && !ready; i++)
        {
          using (VectorOfPoint contour = contours[i])
          {
            for (int j = i + 1; j < contours.Size && !ready; j++)
            {

              if (0.35 * CvInvoke.ContourArea(contours[j]) > CvInvoke.ContourArea(contours[i]) && 0.29 * CvInvoke.ContourArea(contours[j]) < CvInvoke.ContourArea(contours[i]))
              {
                result.Push(contours[j]);
                result.Push(contours[i]);
                ready = !ready;
              }
            }
          }
        }
      }
      return result;
    }
  }
}
