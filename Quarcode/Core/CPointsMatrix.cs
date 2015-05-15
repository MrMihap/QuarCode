using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quarcode.Core
{
  public class CPointsMatrix
  {
    public List<Vector> Points;
    public List<Vector> NoisedPoints;
    public List<Vector> BorderPoints;
    int _Width;
    int _Height;
    // 
    /// <summary>
    /// Changing makes matrix up to date
    /// </summary>
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
    /// Changing makes matrix up to date
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
      Width = 800;
      Heigt = 800;
    }
    private void InitMatrix()
    {
      Points = new List<Vector>();
      BorderPoints = new List<Vector>();
      // debug
      mainGexBlock gex = new mainGexBlock(500);
      Points.AddRange(gex.AsArray());
      BorderPoints.AddRange(gex.AsArrayBorder());
      //end debug
    }

    public Vector[] AroundGexAt(int idx)
    {
      Vector[] result = new Vector[6];
      Vector center = VectorAt(idx);
      int[] surround = sixNearest(idx);
      Vector r1;
      Vector r2;
      for (int i = 0; i < 6; i++)
      {
        int idx1 = i, idx2 = i + 1;

        if (i == 5)
        {
          idx1 = 5; idx2 = 0;
        }
        if (surround[idx1] == -1 && surround[idx2] == -1)
        {
          //не найдено окружающи точек
          result[i] = center;
        }
        if (surround[idx1] == -1 && surround[idx2] != -1)
        {
          r1 = VectorAt(surround[idx2]);
          result[i] = new Vector(
           (center.x + r1.x) / 2,
           (center.y + r1.y) / 2
          );
        }
        if (surround[idx1] != -1 && surround[idx2] == -1)
        {
          r1 = VectorAt(surround[idx1]);
          result[i] = new Vector(
           (center.x + r1.x) / 2,
           (center.y + r1.y) / 2
          );
        }
        if (surround[idx1] != -1 && surround[idx2] != -1)
        {
          r1 = VectorAt(surround[idx1]);
          r2 = VectorAt(surround[idx2]);
          result[i] = new Vector(
           (center.x + r1.x + r2.x) / 3,
           (center.y + r1.y + r2.y) / 3
          );
        }
      }
      return result;
    }

    public Vector VectorAt(int i)
    {
      if (i < Points.Count)
        return Points.ElementAt(i);
      else if (i >= Points.Count && i < Points.Count + BorderPoints.Count)
      {
        return BorderPoints.ElementAt(i - Points.Count);
      }
      else
        throw new ArgumentOutOfRangeException();
    }

    public int[] sixNearest(int idx)
    {
      int[] result = new int[6];
      Vector center = VectorAt(idx);
      //ищем ближайшие точки по кругу в секторах по 60 градусов
      //для обхода проблемных мест начальный угол будет -Pi/30;
      for (int i = 0; i < 6; i++)
      {
        double angle = -Math.PI / 30 + i * Math.PI / 3;
        double k1, k2, b1, b2;
        k1 = Math.Tan(angle);
        k2 = Math.Tan(angle + Math.PI / 3);
        b1 = center.y - k1 * center.x;
        b2 = center.y - k2 * center.x;
        int Sign1 = 1;
        int Sign2 = 1;
        if (angle >= Math.PI / 2 && angle < Math.PI * 3 / 2)
          Sign1 = -1;
        if (angle + Math.PI / 3 >= Math.PI / 2 && angle + Math.PI / 3 < Math.PI * 3 / 2)
          Sign2 = -1;
        List<int> candidates = BitweenLines(k1, k2, b1, b2, Sign1, Sign2);
    
        int coolindex = (from x in candidates orderby Vector.Distance(center, this.VectorAt(x)) select x).First();

        
          if (candidates.Count > 0)
            result[i] = (from x in candidates orderby Vector.Distance(center, this.VectorAt(x)) select x).First();

          else
            result[i] = -1;
      }
      return result;
    }

    public int indexAtVector(Vector r)
    {
      throw new NotImplementedException("nod needed now");
      return 0;
    }

    private List<int> BitweenLines(double k1, double k2, double b1, double b2, int sign1, int sign2)
    {
      List<int> result = new List<int>();
      for (int i = 0; i < Points.Count; i++)
      {
        if (sign1 * Points[i].y >= sign1 * (k1 * Points[i].x + b1)
          && sign2 * Points[i].y < sign2 * (k2 * Points[i].x + b2))
          result.Add(i);
      }
      for (int i = 0; i < BorderPoints.Count; i++)
      {
        if (sign1 * BorderPoints[i].y >= sign1 * (k1 * BorderPoints[i].x + b1)
          && sign2 * BorderPoints[i].y < sign2 * (k2 * BorderPoints[i].x + b2))
          result.Add(i + Points.Count);
      }
      return result;
    }
  }
}
