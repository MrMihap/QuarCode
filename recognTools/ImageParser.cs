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
    public delegate void OnHexCodeRecognizedDelegate(string code);
    public delegate void OnImageRecievedDelegate(Image<Bgr, Byte> sourse);
    public delegate void OnImageFilteredDelegate(Image<Hsv, Byte> sourse);
    public delegate void OnContourFoundDelegate(VectorOfVectorOfPoint contourList);
    public delegate void OnHexImageCroptedDelegate(Image<Bgr, Byte> sourse);

    public static class ImageParser
    {
      public static event OnHexCodeRecognizedDelegate OnHexCodeRecognized;
      public static event OnImageRecievedDelegate OnImageRecieved;
      public static event OnImageFilteredDelegate OnImageFiltered;
      public static event OnContourFoundDelegate  OnContourFound;
      public static event OnHexImageCroptedDelegate OnHexImageCropted; 
      public static void AddDevDataReciever(object reciever)
      {
        if (reciever is IRecieveRecognizedCode) OnHexCodeRecognized += (reciever as IRecieveRecognizedCode).Recieve;
        if (reciever is IRecieveRawImage) OnImageRecieved += (reciever as IRecieveRawImage).Recieve;
        if (reciever is IRecieveFilteredImage) OnImageFiltered += (reciever as IRecieveFilteredImage).Recieve;
        if (reciever is IRecieveFoundContours) OnContourFound += (reciever as IRecieveFoundContours).Recieve;
        if (reciever is IRecieveCroptedImage) OnHexImageCropted += (reciever as IRecieveCroptedImage).Recieve;
      }
      public static void RecieveImage(Image<Bgr, Byte> sourse)
      {
        // 0. Scale
        sourse.Resize(0.5, Inter.Linear);
        // 1. Recieve
        OnImageRecieved(sourse.SmoothMedian(9));

        // 2.Filter
        Image<Hsv, Byte> filtredSourse = HexDecoder.Filter(sourse);
        if (OnImageRecieved != null) OnImageFiltered(filtredSourse);

        // 3.Find All Contours
        VectorOfVectorOfPoint allContours = HexDecoder.FindAllContours(filtredSourse);
        if (OnContourFound != null) OnContourFound(allContours);

        // 4.Filter Contours
        allContours = HexDecoder.FilterAllContours(allContours);        
        CvInvoke.DrawContours(sourse,allContours, -1, new Bgr(Color.Red).MCvScalar, 2);

        // 5.Crop true Orinted HexImage
        

        // 6.Parse Image for Code
        string result = HexDecoder.TryDecode(sourse);
        if (result != null)
          if (OnHexCodeRecognized != null) OnHexCodeRecognized(result);

      }

    }
}
