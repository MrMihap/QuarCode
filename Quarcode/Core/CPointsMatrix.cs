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
    public List<Vector> BorderPoints;
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
      BorderPoints = new List<Vector>();
      // debug
      mainGexBlock gex = new mainGexBlock(500);
      Points.AddRange(gex.AsArray());
      BorderPoints.AddRange(gex.AsArrayBorder());
      //end debug
    } 
    Vector VectorAt(int i)
    {
      if (i < Points.Count)
        return Points.ElementAt(i);
      else
        throw new ArgumentOutOfRangeException();
    }
    public int[] sixNearest(int idx)
    {
      return new int[0];
    }
    int indexAtVector(Vector r)
    {
      throw new NotImplementedException("nod needed now");
      return 0;
    }

    List<int> BitweenLines(double k1, double k2, double b1, double b2)
    {
      List<int> result = new List<int>();
      for (int i = 0; i < Points.Count; i++)
      {
        if (Points[i].y >= k1 * Points[i].x + b1 && Points[i].y < k2 * Points[i].x + b2)
        result.Add(i);
      }
      for (int i = 0; i < BorderPoints.Count; i++)
      {
        if (BorderPoints[i].y >= k1 * BorderPoints[i].x + b1 && BorderPoints[i].y < k2 * BorderPoints[i].x + b2)
          result.Add(i + Points.Count);
      }
      return result;
    }
  }
}
