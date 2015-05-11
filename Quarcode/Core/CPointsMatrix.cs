using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quarcode.Core
{
  public class  CPointsMatrix
  {
    public List<Vector> Points;
    int _Width;
    int _Height;
    // Changing make matrix uptodate
    public int Width
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
    /// <summary>
    /// Changing make matrix uptodate
    /// </summary>
    public int Heigt
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

    public bool IsInited { get { if (Points.Count > 0) return true; else return false; } }
    public CPointsMatrix()
    {
      Width = 600;
      Heigt = 600;
    }
    void InitMatrix()
    {
      Points = new List<Vector>();
      // debug
      mainGexBlock gex = new mainGexBlock(400);
      Points.AddRange(gex.AsArray());
      //end debug

    }
    Vector VectorAt(int i)
    {
      if (i < Points.Count)
        return Points.ElementAt(i);
      else
        throw new ArgumentOutOfRangeException();
    }
    int indexAtVector(Vector r)
    {
      return 0;
    }
  }
}
