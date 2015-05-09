using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quarcode.Core
{
  public struct Vector
  {
    double x;
    double y;
    void Init()
    {
      x = 0;
      y = 0;
    }
    void Init(double _x, double _y)
    {
      x = _x;
      y = _y;
    }
  }
  public struct GexBlock
  {
    Vector ParentVector;
    Vector[] Points = new Vector[19];
  }
}
