using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Quarcode.Core
{
  public struct Vector
  {
    public double x;
    public double y;
    public void Init()
    {
      x = 0;
      y = 0;
    }
    public void Init(double _x, double _y)
    {
      x = _x;
      y = _y;
    }

    public Vector(double _x, double _y)
    {
      x = _x;
      y = _y;
    }

    public Vector(Vector v)
    {
      x = v.x;
      y = v.y;
    }

    /// <summary>
    /// Вращает вектор относительно нуля
    /// </summary>
    /// <param name="v">вектор</param>
    /// <param name="Angle">Угол поворота, в радианах</param>
    /// <returns></returns>
    public static Vector Rotate(Vector v, double Angle)
    {
      return new Vector(
        v.x * Math.Cos(Angle) - v.y * Math.Sin(Angle),
        v.x * Math.Sin(Angle) + v.y * Math.Cos(Angle));
    }

    public static Vector[] Rotate(Vector[] v, double Angle)
    {
      Vector[] result = new Vector[v.Length];
      for (int i = 0; i < result.Length; i++)
      {
        result[i] = Vector.Rotate(v[i], Angle);
      }
      return result;
    }

    public static Vector operator +(Vector a, Vector b)
    {
      return new Vector(a.x + b.x, a.y + b.y);
    }

    public static Vector operator -(Vector a, Vector b)
    {
      return new Vector(a.x - b.x, a.y - b.y);
    }
    /// <summary>
    /// Возвращает новый массив, составленный из суммы второго с каждым первым 
    /// </summary>
    /// <param name="a">массив векторов</param>
    /// <param name="b">добавляемый вектор</param>
    /// <returns></returns>
    public static Vector[] operator +(Vector[] a, Vector b)
    {
      Vector[] result = new Vector[a.Length];
      for (int i = 0; i < result.Length; i++)
      {
        result[i] = a[i] + b;
      }
      return result;
    }

    public static double Distance(Vector a, Vector b)
    {
      return Math.Sqrt(Math.Pow(a.x - b.x, 2) + Math.Pow(a.y - b.y, 2));
    }

    public static double Length(Vector a)
    {
      return Distance(a, new Vector());
    }
    /// <summary>
    /// возвращает значения поворота вектора к горизонтальной оси
    /// </summary>
    /// <param name="a">вектор</param>
    /// <returns>значение угла в радианах</returns>
    public static double Angle(Vector a)
    {
      double alpha = 0;
      double SIN, COS;
      double length = Length(a);
      SIN = a.y / length;
      COS = a.x / length;
      alpha = Math.Acos(COS);
      if (SIN < 0)
        alpha += 2 * (Math.PI - alpha);
      return alpha;
    }

    public static PointF[] ToSystemPointsF(Vector[] a)
    {
      PointF[] result = new PointF[a.Length];
      for (int i = 0; i < result.Length; i++)
      {
        result[i].X = (float)a[i].x;
        result[i].Y = (float)a[i].y;
      }
      return result;
    }

    public static PointF ToSystemPointF(Vector a)
    {
      return new PointF((float)a.x, (float)a.y);
    }
  }
}
