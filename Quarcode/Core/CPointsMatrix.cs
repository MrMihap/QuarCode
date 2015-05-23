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
    //debug
    public List<Vector> LastSurround = new List<Vector>();
    //debug end
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
      Width = 1700;
      Heigt = 1700;
    }
    public CPointsMatrix(int __Height)
    {
      Width = __Height;
      Heigt = Width;
    }
    private void InitMatrix()
    {
      Points = new List<Vector>();
      BorderPoints = new List<Vector>();
      LogoBorderPoints = new List<Vector>();
      mainGexBlock gex = new mainGexBlock(Heigt - 40);
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
      Vector[] result;
      Vector center = NoisedPoints[idx];
      double L = this.Heigt / (3 * Math.Sqrt(3)) - 10;
      int[] surround = sixNearestForVoronoj(idx);
      Vector r1;
      List<double> kList = new List<double>();
      List<double> bList = new List<double>();
      for (int i = 0; i < surround.Length; i++)
      {
        //int idx1 = i;
        if (surround[i] == -1)
        {
          //не найдено окружающи точек
          //result[i] = center;
        }
        else
        {
          double Eps = 0.001;
          double k1, b1;

          r1 = NoisedPoints[surround[i]];
          // Укорачиваем вектора на попалам
          r1 -= center;
          r1.x /= 2;
          r1.y /= 2;
          // получаем вектора центров отрезков до ближайших точек
          r1 += center;
          //double debug1 = (r1 - center).x;
          //double debug2 = (r2 - center).x;
          if (Math.Abs((r1 - center).x) > Eps)
          {
            if (Math.Abs((r1 - center).y) > Eps)
            {
              // идеальный случай, нет вертикальных прямых
              k1 = (r1.y - center.y) / (r1.x - center.x);
              // Коэф прямых, проходящих через середину между двумя точками
              k1 = -1 / k1;
              b1 = r1.y - k1 * r1.x;
              kList.Add(k1);
              bList.Add(b1);
              //result[i] = SolveSystem(k1, k2, b1, b2);
            }
            else
            {
              // самый плохой случай - перпендикуляр вертикален
              // должен быть исключен правильной генерацией случайных точек
            }
          }
          else
          {
            // случай когда одна из прямых вертикальна
            double k = 0, b = 0;
            // тогда перпендикуляр к ней будет горизонтален
            k = 0;
            b = r1.y - k * r1.x;
            kList.Add(k);
            bList.Add(b);
            //result[i] = new Vector(center.x, k * center.x + b);
          }
        }
      }
      Vector[] Candidates = SolveMultiSystem(kList, bList);
      LastSurround.AddRange(Candidates);
      // необходимо удалить лишние точки, т.е. те, которые вне минимального контура
      result = ExecuteNonVoronojPoints(center, Candidates, kList, bList);
      //result = ExecuteNonVoronojPoints(center, result, kList, bList);
      result = (from v in result orderby Vector.Angle(center - v) select v).ToArray();
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
      GenNoise(l / 10);
    }

    public void GenNoise(double r)
    {
      NoisedPoints = new List<Vector>();
      Random rand = new Random(DateTime.Now.Millisecond);
      Vector shift;
      double distance;
      double Angle;

      for (int i = 0; i < Points.Count; i++)
      {
      //  do
      //  {
      //    distance = r * (rand.Next(5, 100)) / 100.0;
      //    Angle = Math.PI * 2 * rand.Next(5, 600)/ 300.0;
      //    //double distance = r * (i % 5 + 5) / 10.0;
      //    //double Angle = Math.PI * 2 * i % 60 / 60.0;

      //    shift = new Vector(-distance, 0);
      //    shift = Vector.Rotate(shift, Angle);
      //    shift.x += distance;
      //    if ((from p in NoisedPoints where p.x == Points[i].x + shift.x && p.y == Points[i].y + shift.y select p).Count() == 0)
      //    {
      //      NoisedPoints.Add(Points[i] + shift);
      //      break;
      //    }
      //  } while (true);
        distance = r * (rand.Next(5, 10) ) / 10.0;
        Angle = Math.PI * rand.Next(0, 120) / 60.0;

        shift = new Vector(-distance, 0);
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
    public int[] sixNearestForVoronoj(int idx)
    {
      int[] result = new int[6];
      List<int> pre_result = new List<int>();
      Vector center = VectorAt(idx);
      //ищем ближайшие точки по кругу в секторах по 60 градусов
      //для обхода проблемных мест начальный угол будет -Pi/30;
      for (int i = 0; i < 18; i++)
      {
        double angle = -Math.PI / 30 + i * Math.PI / 9;
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
        {
          int min = (from x
                         in candidates
                     where Vector.Distance(center, this.VectorAt(x)) > 0.1
                     orderby Vector.Distance(center, this.VectorAt(x))
                     select x).First();
          if(!pre_result.Contains(min))pre_result.Add(min);
        }
        //else
          //pre_result.Add(-1);
      }
      return pre_result.ToArray();
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
    /// <summary>
    ///Меотд возвращает все возможные пересечения уравнений прямых y = kx + b
    /// </summary>
    /// <param name="k"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    private Vector[] SolveMultiSystem(List<double> k, List<double> b)
    {
      List<Vector> result = new List<Vector>();
      for (int i = 0; i < k.Count; i++)
      {
        for (int j = i; j < k.Count; j++)
        {
          // исключение сравнения с собой
          if (j == i) continue;
          result.Add(SolveSystem(k[i], k[j], b[i], b[j]));
        }
      }

      return result.ToArray();
    }

    private Vector[] ExecuteNonVoronojPoints(Vector center, Vector[] candidates, List<double> k, List<double> b)
    {
      List<Vector> result = new List<Vector>();
      for (int i = 0; i < candidates.Length; i++)
      {
        // счетчик удовлетворений точки полуплоскостям
        int Count = 0;
        for (int j = 0; j < k.Count; j++)
        {
          // центр под прямой
          if (center.y < k[j] * center.x + b[j])
          {
            // тогда если точка выше или на прямой - она удовлетворяет полуплоскости
            if (candidates[i].y >= k[j] * candidates[i].x + b[j] ||
              Math.Abs(candidates[i].y - k[j] * candidates[i].x - b[j]) < 0.0001)
              Count++;
          }
          // центр над прямой
          if (center.y > k[j] * center.x + b[j])
          {
            // тогда если точка ниже или на прямой - она удовлетворяет полуплоскости
            if (candidates[i].y <= k[j] * candidates[i].x + b[j] ||
              Math.Abs(candidates[i].y - k[j] * candidates[i].x - b[j]) < 0.0001)
              Count++;
          }

        }
        if (Count == 2)
        {
          result.Add(candidates[i]);
        }
      }
      return result.ToArray();
    }
  }
}
