using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;
using System.Data;
namespace recognTools
{
    public delegate void OnHexCodeRecognizedDelegate(string code);
    public delegate void OnImageRecievedDelegate(Image<Bgr, Byte> sourse);
    public delegate void OnImageFilteredDelegate(Image<Bgr, Byte> sourse);
    public delegate void OnContourFoundDelegate(List<MCvContour> contourList);
    public delegate void OnHexImageCroptedDelegate(Image<Bgr, Byte> sourse);

    public static class ImageParser
    {
      public static event OnHexCodeRecognizedDelegate OnHexCodeRecognized;
      public static event OnImageRecievedDelegate OnImageRecieved;
      public static event OnImageFilteredDelegate OnImageFiltered;
      public static event OnContourFoundDelegate  OnContourFound;
      public static event OnHexImageCroptedDelegate OnHexImageCropted; 
      static void AddDevDataReciever(object reciever)
      {
        if (reciever is IRecieveRecognizedCode) OnHexCodeRecognized += (reciever as IRecieveRecognizedCode).Recieve;
        if (reciever is IRecieveRawImage) OnImageRecieved += (reciever as IRecieveRawImage).Recieve;
        if (reciever is IRecieveFilteredImage) OnImageFiltered += (reciever as IRecieveFilteredImage).Recieve;
        if (reciever is IRecieveFoundContours) OnContourFound += (reciever as IRecieveFoundContours).Recieve;
        if (reciever is IRecieveCroptedImage) OnHexImageCropted += (reciever as IRecieveCroptedImage).Recieve;

      }
      static void RecieveImage(Image<Bgr, Byte> sourse)
      {
        string result = Parse(sourse);
        if (result != null)
          if (OnHexCodeRecognized != null) OnHexCodeRecognized(result);
      }

      private static string Parse(Image<Bgr, Byte> sourse)
      {
        return null;
      }

    }
}
