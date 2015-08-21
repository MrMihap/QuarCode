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
    public delegate void OnImageRecievedDelegate(Image<Bgr, Byte> source);
    public delegate void OnImageFilteredDelegate(Image<Hsv, Byte> source);
    public delegate void OnContourFoundDelegate(VectorOfVectorOfPoint contourList);
    public delegate void OnHexImageCroptedDelegate(Image<Bgr, Byte> source);

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
      public static void RecieveImage(Image<Bgr, Byte> source)
      {
        // 0. Scale
        int WorkWidth = 1000;
        double reSize = WorkWidth / (double) source.Width;
        source = source.Resize(reSize, Inter.Lanczos4);
        // 1. Recieve
        OnImageRecieved(source.SmoothMedian(9));

        // 2.Filter
        Image<Hsv, Byte> filtredsource = HexDecoder.Filter(source);
        if (OnImageRecieved != null) OnImageFiltered(filtredsource);

        // 3.Find All Contours
        VectorOfVectorOfPoint allContours = HexDecoder.FindAllContours(filtredsource);
        if (OnContourFound != null) OnContourFound(allContours);

        // 4.Filter Contours
        VectorOfVectorOfPoint fltContours = HexDecoder.FilterAllContours(allContours);
        //CvInvoke.DrawContours(source, allContours, -1, new Bgr(Color.Red).MCvScalar, 2);
        if (fltContours.Size < 2) { return;  throw new Exception("не найдены два рапозноваемых контура"); }
        
        // 5.Crop true Orinted HexImage
        Image<Bgr, Byte> CropMatrix = HexDecoder.CropCodeFromImage(source, fltContours);

        if (OnHexImageCropted != null) OnHexImageCropted(CropMatrix);
        // 6.Parse Image for Code
        string result = HexDecoder.TryDecode(CropMatrix);
        if (result != null)
          if (OnHexCodeRecognized != null) OnHexCodeRecognized(result);
        //source.Dispose();
        //filtredsource.Dispose();
        //CropMatrix.Dispose();
      }

      public static void RecieveImageAsync(Image<Bgr, Byte> source)
      {
        throw new NotImplementedException("will be inplemented soon");
      }

    }
}
