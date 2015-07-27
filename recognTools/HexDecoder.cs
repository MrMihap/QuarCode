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
      sourse = sourse.SmoothGaussian(9);
      // 2.To HSV
      Image<Hsv, Byte> filteredimage = sourse.Convert<Hsv, Byte>();
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
             if (CvInvoke.ContourArea(approxContour, false) > 250) //only consider contours with area greater than 250
             {
               if (approxContour.Size == 4)
               {
                 borders.Push(contour);
                 //Black_boxList.Add(CvInvoke.MinAreaRect(approxContour)); 
               }
             }
           }
         }
       }
      
      Image<Gray, Byte> cannyBlue = maskHsvBlue.Canny(cannyThreshold, cannyThresholdLinking);
      //VectorOfVectorOfPoint blueborders = new VectorOfVectorOfPoint();//list of blue borders
      using (VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint())
      {
        CvInvoke.FindContours(cannyBlue, contours, null, RetrType.List, ChainApproxMethod.ChainApproxSimple);

        for (int i = 0; i < contours.Size; i++)
        {
          using (VectorOfPoint contour = contours[i])
          using (VectorOfPoint approxContour = new VectorOfPoint())
          {
            CvInvoke.ApproxPolyDP(contour, approxContour, CvInvoke.ArcLength(contour, true) * 0.05, true);
            if (CvInvoke.ContourArea(approxContour, false) > 150) //only consider contours with area greater than 250
            {
              if (approxContour.Size >= 4)
              {
                //Blue_boxList.Add(CvInvoke.MinAreaRect(approxContour));
                borders.Push(contour);
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
            for (int j = i + 1; j < contours.Size  && !ready; j++)
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
