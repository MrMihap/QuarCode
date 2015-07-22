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
      sourse = sourse.SmoothGaussian(3);
      // 2.To HSV
      Image<Hsv, Byte> filteredimage = sourse.Convert<Hsv, Byte>();

      Hsv min = new Hsv();
      Hsv max = new Hsv();
      min.Hue = 0;
      min.Satuation = 0;
      min.Value = 0;

      max.Hue = 179;
      max.Satuation = 55;
      max.Value = 105;


      return filteredimage.InRange(min, max).Canny(3, 6).Convert<Hsv, Byte>();
    }
    public static List<List<Point>> FindAllContours(Image<Hsv, Byte> sourse)
    {
      List<List<Point>> result = new List<List<Point>>();
      sourse.Canny(3, 6);
      double cannyThreshold = 180.0;
      double cannyThresholdLinking = 120.0;
      UMat uimage = new UMat();
      CvInvoke.CvtColor(sourse, uimage, ColorConversion.Bgr2Gray);
      UMat cannyEdges = new UMat();
      CvInvoke.Canny(uimage, cannyEdges, cannyThreshold, cannyThresholdLinking);
      using (VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint())
      {
        CvInvoke.FindContours(cannyEdges, contours, null, RetrType.List, ChainApproxMethod.LinkRuns);
        int count = contours.Size;
        for (int i = 0; i < count; i++)
        {
          using (VectorOfPoint contour = contours[i])
          using (VectorOfPoint approxContour = new VectorOfPoint())
          {
            CvInvoke.ApproxPolyDP(contour, approxContour, CvInvoke.ArcLength(contour, true) * 0.05, true);
            if (CvInvoke.ContourArea(approxContour, false) > 350) //only consider contours with area greater than 250
            {
              result.Add(approxContour.ToArray().ToList());
            }
          }
        }

      }


      return result;
    }

  }
}
