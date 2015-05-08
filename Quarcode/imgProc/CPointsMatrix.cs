using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quarcode.Core;
namespace Quarcode.imgProc
{
  public class CPointsMatrix
  {
    List<Vector> Points;
    int _Width;
    int _Height;
    int Width
    {
      get
      {
        return _Width;
      }
      set
      {
        _Width = value;
        this.InitMatrix();
      }
    }
    int Heigt
    {
      get
      {
        return _Height;
      }
      set
      {
        _Height = value;
        this.InitMatrix();
      }
    }
    public CPointsMatrix()
    {
      Points = new List<Vector>();

      this.InitMatrix();
    }
    void InitMatrix()
    {

    }
  }
}
