﻿using System;
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

  public interface IRecieveRecognizedCode
  {
    void Recieve (string code);
  }
  public interface IRecieveRawImage
  {
    void Recieve(Image<Bgr, Byte> sourse);
  }
  public interface IRecieveFilteredImage
  {
    void Recieve(Image<Hsv, Byte> sourse);
  }
  public interface IRecieveFoundContours
  {
    void Recieve(VectorOfVectorOfPoint contourList);
  }
  public interface IRecieveCroptedImage
  {
    void Recieve(Image<Bgr, Byte> sourse);
  }
}