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
    public List<Vector> LogoPoints;
    public List<CGexPoint> DrawData;
    //debug
    public List<Vector> LastSurround = new List<Vector>();
    //debug end
    int _Width;
    int _Height;
    double L = 0;
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
      }
    }
    /// <summary>
    /// Changing makes matrix up to date
    /// </summary>
    public int Height
    {
      get
      {
        return _Height;
      }
      set
      {
        _Height = value;
        //this.InitMatrix();
      }
    }

    public bool IsInited { get { if (Points.Count > 0) return true; else return false; } }

    public CPointsMatrix()
    {
      Width = 1700;
      Height = 1700;
      InitMatrix();
    }
    /// <summary>
    /// Init this matrix with selected height proportion
    /// </summary>
    /// <param name="height">Height of matrix</param>
    public CPointsMatrix(int height)
    {
      Width = height;
      Height = Width;
      InitMatrix();
    }

    private void InitMatrix()
    {
      Points = new List<Vector>();
      BorderPoints = new List<Vector>();
      LogoPoints = new List<Vector>();
      NoisedPoints = new List<Vector>();
      mainGexBlock gex = new mainGexBlock(Height);
      L = gex.L;
      Points.AddRange(gex.AsArray());
      LogoPoints.AddRange(gex.AsArrayLogo());
      BorderPoints.AddRange(gex.AsArrayBorder());

      NoisedPoints.AddRange(Points);
      NoisedPoints.AddRange(LogoPoints);
      NoisedPoints.AddRange(BorderPoints);

      GenNoise();
      InitDrawData();
      gex.ExportToFile("out2.txt");
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
      double L = this.Height / (3 * Math.Sqrt(3)) - 10;
      int[] surround = sixNearestForVoronoj(idx);
      Vector r1;
      List<double> kList = new List<double>();
      List<double> bList = new List<double>();
      List<double> xList = new List<double>();
      for (int i = 0; i < surround.Length; i++)
      {
        //int idx1 = i;
        if (surround[i] != -1)
        {
          double Eps = L / 2000000;
          double k1, b1;

          r1 = NoisedPoints[surround[i]] + center;
          Vector cdebug3 = NoisedPoints[surround[i]];
          // Укорачиваем вектора на попалам
          r1.x /= 2;
          r1.y /= 2;
          // получаем вектора центров отрезков до ближайших точек
          //double debug1 = (r1 - center).x;
          //double debug2 = (r1 - center).y;
          if(true)// (Math.Abs((r1 - center).x) > Eps && Math.Abs((r1 - center).y) > Eps)
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
          //else if (Math.Abs((r1 - center).y) < Eps)
          //{
          //  xList.Add(r1.x / 2 + center.x / 2);
          //  // самый плохой случай - перпендикуляр вертикален
          //  // должен быть исключен правильной генерацией случайных точек
          //}
          //else if (Math.Abs((r1 - center).x) < Eps)
          //{
          //  // случай когда одна из прямых вертикальна
          //  double k = 0, b = 0;
          //  // тогда перпендикуляр к ней будет горизонтален
          //  k = 0;
          //  b = r1.y - k * r1.x;
          //  kList.Add(k);
          //  bList.Add(b);
          //  //result[i] = new Vector(center.x, k * center.x + b);
          //}
        }
      }
      Vector[] Candidates = SolveMultiSystem(kList, bList);
      LastSurround.Clear();
      LastSurround.AddRange(Candidates);
      // необходимо удалить лишние точки, т.е. те, которые вне минимального контура
      result = ExecuteNonVoronojPoints(center, Candidates, kList, bList, xList);
      result = (from v in result orderby Vector.Angle(center - v) select v).ToArray();
      return result;
    }

    public Vector VectorAt(int i)
    {
      if (i < NoisedPoints.Count && i >= 0)
        return NoisedPoints[i];
      else
        throw new ArgumentOutOfRangeException();
    }

    public void GenNoise()
    {
      GenNoise(L / 10);
    }

    public void GenNoise(int percent)
    {
      GenNoise(L * percent / (100 * (Math.Sqrt(3) + 1)));
    }

    public void GenNoise(double r)
    {
      NoisedPoints = new List<Vector>();
      Random rand = new Random();// (DateTime.Now.Millisecond);
      Vector shift;
      double distance;
      double Angle;

      for (int i = 0; i < Points.Count; i++)
      {
        do
        {
          distance = r * (rand.Next(0, 20)) / 20.0;
          Angle = Math.PI * rand.Next(0, 120) / 60.0;


          shift = new Vector(-distance, 0);
          shift = Vector.Rotate(shift, Angle);
          shift.x += distance;
          Vector neo = Points[i] + shift;
          bool alredyExists = false;
          int identity = (from p in NoisedPoints
                          where
                            Math.Abs((p.x - neo.x)) < L / 200000 &&
                            Math.Abs(p.y - neo.y) < L / 200000
                          select p).Count();
          for (int j = 0; j < NoisedPoints.Count; j++)
          {
            if (Math.Abs(NoisedPoints[j].y - 2499.1284703335459) < 0.000001 )
            {
              //debug

            }
            if (Math.Abs(NoisedPoints[j].y - neo.y) < L / 200000 || Math.Abs(NoisedPoints[j].x - neo.x) < L / 200000)
            {
              alredyExists = true;
              break;
            }
          }
          if (!alredyExists)
          {
            for (int j = 0; j < BorderPoints.Count; j++)
            {
              if (Math.Abs(BorderPoints[j].y - neo.y) < L / 200000 || Math.Abs(BorderPoints[j].x - neo.x) < L / 200000)
              {
                alredyExists = true;
                break;
              }
            }
          }
          if (!alredyExists)
          {
            break;
          }
          else
          {
            continue;
          }
        } while (true);
        //distance = r * (rand.Next(5, 10)) / 10.0;
        //Angle = Math.PI * rand.Next(0, 120) / 60.0;

        //shift = new Vector(-distance, 0);
        //shift = Vector.Rotate(shift, Angle);
        //shift.x += distance;
        NoisedPoints.Add(Points[i] + shift);
      }
      for (int i = 0; i < LogoPoints.Count; i++)
      {
        distance = r * (rand.Next(5, 10)) / 10.0;
        Angle = Math.PI * rand.Next(0, 120) / 60.0;

        shift = new Vector(-distance, 0);
        shift = Vector.Rotate(shift, Angle);
        shift.x += distance;
        NoisedPoints.Add(LogoPoints[i] + shift);
        if (Math.Abs((LogoPoints[i] + shift).y - 2499.1284703335459) < 0.000001)
        {
          //debug

        }
      }
      //NoisedPoints.AddRange(LogoPoints);
      NoisedPoints.AddRange(BorderPoints);
      InitDrawData();

    }

    private void InitDrawData()
    {
      DrawData = new List<CGexPoint>();

      for (int i = 0; i < NoisedPoints.Count; i++)
      {
        if (i >= 0 && i < Points.Count)
        {
          DrawData.Add(new CGexPoint(PointType.UndefinedByte, NoisedPoints[i], AroundVoronojGexAt(i).ToList()));
        }
        if (i >= Points.Count && i < LogoPoints.Count + Points.Count)
        {
          DrawData.Add(new CGexPoint(PointType.Logo, NoisedPoints[i], AroundVoronojGexAt(i).ToList()));
        }
        if (i >= Points.Count + LogoPoints.Count)
        {
          DrawData.Add(new CGexPoint(PointType.Border, NoisedPoints[i], AroundVoronojGexAt(i).ToList()));
        }
      }
      foreach (CGexPoint p in DrawData)
      {
        p.r.y = Height - p.r.y;
        for (int i = 0; i < p.Cell.Count; i++)
        {
          p.Cell[i] = new Vector(p.Cell[i].x, Height - p.Cell[i].y);
        }
      }
    }

    public int[] sixNearest(int idx)
    {
      int[] result = new int[6];
      Vector center = NoisedPoints[idx];
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
                       where Vector.Distance(center, this.NoisedPoints[x]) > 0.1
                       orderby Vector.Distance(center, this.NoisedPoints[x])
                       select x).First();

        else
          result[i] = -1;
      }
      return result;
    }

    public int[] sixNearestForVoronoj(int idx)
    {
      List<int> result = new List<int>();
      Vector center = NoisedPoints[idx];

      List<int> candidates = new List<int>();
      for (int i = 0; i < NoisedPoints.Count; i++)
      {
        candidates.Add(i);
      }
      result.AddRange((from x in candidates
                       where Vector.Distance(center, NoisedPoints[x]) > 0.1
                       orderby Vector.Distance(center, NoisedPoints[x])
                       select x).Take(10));

      return result.ToArray();
    }

    public int indexAtVector(Vector r)
    {
      throw new NotImplementedException("nod needed now");
      return 0;
    }

    private List<int> BitweenLines(double k1, double k2, double b1, double b2, int sign1, int sign2)
    {
      List<int> result = new List<int>();
      for (int i = 0; i < NoisedPoints.Count; i++)
      {
        if (sign1 * NoisedPoints[i].y >= sign1 * (k1 * NoisedPoints[i].x + b1)
          && sign2 * NoisedPoints[i].y < sign2 * (k2 * NoisedPoints[i].x + b2))
          result.Add(i);
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

    private Vector[] ExecuteNonVoronojPoints(Vector center, Vector[] candidates, List<double> k, List<double> b, List<double> xList)
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
        //for (int j = 0; j < xList.Count; j++)
        //{
        //  // центр слева прямой
        //  if (center.x < xList[j])
        //  {
        //    // тогда если точка правее или на прямой - она удовлетворяет полуплоскости
        //    if (candidates[i].x >= xList[j] ||
        //      Math.Abs(candidates[i].x - xList[j]) < 0.0001)
        //      Count++;
        //  }
        //  // центр справа от прямой
        //  if (center.x > xList[j])
        //  {
        //    // тогда если точка левее или на прямой - она удовлетворяет полуплоскости
        //    if (candidates[i].x <= xList[j] ||
        //      Math.Abs(candidates[i].x - xList[j]) < 0.0001)
        //      Count++;
        //  }
        //}
        if (Count == 2)
        {
          result.Add(candidates[i]);
        }
      }
      return result.ToArray();
    }
  }
}
