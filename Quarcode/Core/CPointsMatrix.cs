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
    public List<Vector> LogoBorderPoints;
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
      Width = 700;
      Heigt = 700;
    }

    private void InitMatrix()
    {
      Points = new List<Vector>();
      BorderPoints = new List<Vector>();
      LogoBorderPoints = new List<Vector>();
      mainGexBlock gex = new mainGexBlock(500);
      Points.AddRange(gex.AsArray());
      BorderPoints.AddRange(gex.AsArrayBorder());
      LogoBorderPoints.AddRange(gex.AsArrayLogoBorder());
      GenNoise();
    }

    public Vector[] AroundAverageGexAt(int idx)
    {
      Vector[] result = new Vector[6];
      Vector center = NoisedPoints[idx];
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
          r1 = NoisedPoints[surround[idx2]];
          result[i] = new Vector(
           (center.x + r1.x) / 2,
           (center.y + r1.y) / 2
          );
        }
        if (surround[idx1] != -1 && surround[idx2] == -1)
        {
          r1 = NoisedPoints[surround[idx1]];
          result[i] = new Vector(
           (center.x + r1.x) / 2,
           (center.y + r1.y) / 2
          );
        }
        if (surround[idx1] != -1 && surround[idx2] != -1)
        {
          r1 = NoisedPoints[surround[idx1]];
          r2 = NoisedPoints[surround[idx2]];
          result[i] = new Vector(
           (center.x + r1.x + r2.x) / 3,
           (center.y + r1.y + r2.y) / 3
          );
        }
      }
      return result;
    }
    public Vector[] AroundVoronojGexAt(int idx)
    {
      Vector[] result = new Vector[6];
      Vector center = NoisedPoints[idx];
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
        if (surround[idx1] == -1 || surround[idx2] == -1)
        {
          //не найдено окружающи точек
          result[i] = center;
        }
        else
        {
          double Eps = 0.00001;
          double k1, k2;
          double b1, b2;

          r1 = NoisedPoints[surround[idx1]];
          r2 = NoisedPoints[surround[idx2]];
          // Укорачиваем вектора на попалам
          r1 -= center;
          r2 -= center;
          r1.x /= 2;
          r2.x /= 2;
          r1.y /= 2;
          r2.y /= 2;
          // получаем вектора центров отрезков до ближайших точек
          r1 += center;
          r2 += center;
          double debug1 = (r1 - center).x;
          double debug2 = (r2 - center).x;
          if (Math.Abs((r1 - center).x) > Eps && Math.Abs((r2 - center).x) > Eps)
          {
            // идеальный случай, нет вертикальных прямых
            k1 = (r1.y - center.y) / (r1.x - center.x);
            k2 = (r2.y - center.y) / (r2.x - center.x);
            // Коэф прямых, проходящих через середину между двумя точками
            k1 = -1 / k1;
            k2 = -1 / k2;
            b1 = r1.y - k1 * r1.x;
            b2 = r2.y - k2 * r2.x;

            result[i] = SolveSystem(k1, k2, b1, b2);
          }
          else
          {
            result[i] = center;
          }
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

    public void GenNoise()
    {
      double l = _Height / (3 * Math.Sqrt(3)) - 10;
      GenNoise(l / 7);
    }

    public void GenNoise(double r)
    {
      NoisedPoints = new List<Vector>();
      Random rand = new Random(DateTime.Now.Millisecond);
      for (int i = 0; i < Points.Count; i++)
      {
        double distance = r * (rand.Next() % 5 + 5) / 10.0;
        double Angle = Math.PI * 2 * rand.Next() % 60 / 60.0;
        Vector shift = new Vector(-distance, 0);
        shift = Vector.Rotate(shift, Angle);
        shift.x += distance;
        NoisedPoints.Add(Points[i] + shift);
      }
      for (int i = 0; i < BorderPoints.Count; i++)
      {
        NoisedPoints.Add(BorderPoints[i]);
      }
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

        if (candidates.Count > 0)
          result[i] = (from x
                         in candidates
                       where Vector.Distance(center, this.VectorAt(x)) > 0.1
                       orderby Vector.Distance(center, this.VectorAt(x))
                       select x).First();

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
    /// <summary>
    /// Меотд возвращает решение системы из двух линейных уравнений
    /// Комментарии излишни
    /// </summary>
    /// <param name="k1"></param>
    /// <param name="k2"></param>
    /// <param name="b1"></param>
    /// <param name="b2"></param>
    /// <returns></returns>
    private Vector SolveSystem(double k1, double k2, double b1, double b2)
    {
      Vector result = new Vector();
      result.x = (b2 - b1) / (k1 - k2);
      result.y = k1 * result.x + b1;
      return result;
    }
  }
}
