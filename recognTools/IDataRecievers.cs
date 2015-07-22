using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Emgu.CV;
using Emgu.CV.Structure;

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
    void Recieve(Image<Bgr, Byte> sourse);
  }
  public interface IRecieveFoundContours
  {
    void Recieve(List<MCvContour> contourList);
  }
  public interface IRecieveCroptedImage
  {
    void Recieve(Image<Bgr, Byte> sourse);
  }
}
