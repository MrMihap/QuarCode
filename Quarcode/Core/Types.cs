using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    public static Vector operator + (Vector a, Vector b)
    {
      return new Vector(a.x + b.x, a.y + b.y);
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
  }

  public struct externalGexBlock
  {
    public Vector ParentVector;
    private List<Vector> _Points;
    public Vector[] Points
    {
      get
      {
        if (_Points == null) return null;
        return _Points.ToArray();
      }
    }
    public externalGexBlock(double borderLength, int position)
    {
      ParentVector = new Vector(0, 0);
      _Points = new List<Vector>();
      init(borderLength, position);
    }
    public externalGexBlock(Vector parent, double borderLength, int position)
    {
      ParentVector = parent;
      _Points = new List<Vector>();
      init(borderLength, position);
    }
    public externalGexBlock(Vector parent, Vector[] existsPoints)
    {
      ParentVector = parent;
      _Points = new List<Vector>();
      existsPoints.CopyTo(Points, 0);
    }
    public void init(double borderLength, int position)
    {
      double TriangleLength = borderLength / (Math.Sqrt(3) + 1);
      // Zero 
      if (position == 0)
      {
        //all points
        Vector defaultRing = new Vector(borderLength, 0);
        for (int i = 0; i < 6; i++)
        {
          _Points.Add(Vector.Rotate(defaultRing, i * Math.PI / 3));
        }
      }
      else if (position < 6)
      {
        int PointsCount = 3;
        Vector defaultRing = new Vector(-borderLength, 0);
        if (position == 5)
        {
          defaultRing = Vector.Rotate(defaultRing, -Math.PI / 3);
          PointsCount = 4;
        }
        Vector[] extPoints = new Vector[PointsCount];

        for (int i = 0; i < PointsCount; i++)
        {
          extPoints[i] = Vector.Rotate(defaultRing, (i - 1) * Math.PI / 3);
        }
        _Points.AddRange(Vector.Rotate(extPoints, Math.PI * (1 - position) / 3));
      }

    }
    public Vector[] AsArray()
    {
      Vector[] result = _Points.ToArray();
      for (int i = 0; i < result.Length; i++)
      {
        result[i] += ParentVector;
      }
      return result;
    }

  }
  public struct internalGexBlock
  {
    public Vector ParentVector;
    public Vector[] Points;
    public internalGexBlock(double borderLength)
    {
      ParentVector = new Vector(0, 0);
      Points = new Vector[19];
      init(borderLength);
    }
    public internalGexBlock(Vector parent, double borderLength)
    {
      ParentVector = parent;
      Points = new Vector[19];
      init(borderLength);
    }
    public internalGexBlock(Vector parent, Vector[] existsPoints)
    {
      ParentVector = parent;
      Points = new Vector[19];
      existsPoints.CopyTo(Points, 0);
    }
    public void init(double borderLength)
    {
      double TriangleLength = borderLength / (Math.Sqrt(3) + 1);

      Vector DefaultFirstRing = new Vector();
      Vector DefaultSecondRingPos = new Vector();
      Vector DefaultSecondRingNeg = new Vector();

      DefaultFirstRing.Init(TriangleLength * Math.Sqrt(3) / 2, TriangleLength * 1 / 2);
      DefaultSecondRingPos.Init(TriangleLength * (Math.Sqrt(3) / 2 + 1), TriangleLength * 1 / 2);
      DefaultSecondRingNeg.Init(TriangleLength * (Math.Sqrt(3) / 2 + 1), -TriangleLength * 1 / 2);

      Points = new Vector[19];
      Points[0] = new Vector(0, 0);
      for (int i = 0; i < 6; i++)
      {
        Points[i + 1] = Vector.Rotate(DefaultFirstRing, i * Math.PI / 3);
        Points[7 + 2 * i + 1] = Vector.Rotate(DefaultSecondRingPos, i * Math.PI / 3);
        Points[7 + 2 * i + 0] = Vector.Rotate(DefaultSecondRingNeg, i * Math.PI / 3);
      }
    }
    public Vector[] AsArray()
    {
      Vector[] result = new Vector[19];
      for (int i = 0; i < 19; i++)
      {
        result[i] = Points[i] + ParentVector;
      }
      return result;
    }
  }
  public struct extrenalBorder
  {
    public Vector ParentVector;
    private List<Vector> _Points;
    public Vector[] Points
    {
      get
      {
        if (_Points == null) return null;
        return _Points.ToArray();
      }
    }
    public extrenalBorder(double borderLength, int position)
    {
      ParentVector = new Vector(0, 0);
      _Points = new List<Vector>();
      init(borderLength, position);
    }
    public extrenalBorder(Vector parent, double borderLength, int position)
    {
      ParentVector = parent;
      _Points = new List<Vector>();
      init(borderLength, position);
    }
    public extrenalBorder(Vector parent, Vector[] existsPoints)
    {
      ParentVector = parent;
      _Points = new List<Vector>();
      existsPoints.CopyTo(Points, 0);
    }
    public void init(double borderLength, int position)
    {
      double TriangleLength = borderLength / (Math.Sqrt(3) + 1);
    
     if (position < 6)
      {
        int PointsCount = 10;
        Vector defaultsmallRing2 = new Vector(-borderLength, -TriangleLength);
        Vector defaultsmallRing1 = defaultsmallRing2 + new Vector(TriangleLength * 1 / 2, - TriangleLength * Math.Sqrt(3) / 2);
        Vector defaultbigRing1 = new Vector(-borderLength - TriangleLength * Math.Sqrt(3) / 2, - TriangleLength * 1 / 2);
        Vector defaultbigRing2 = new Vector(-borderLength - TriangleLength * Math.Sqrt(3) / 2, + TriangleLength * 1 / 2);
        Vector[] extPoints = new Vector[PointsCount];

        for (int i = 0; i < 3; i++)
        {
          extPoints[2 * i + 0] =  Vector.Rotate(defaultsmallRing1, (1 - i) * Math.PI / 3);
          extPoints[2 * i + 1] =  Vector.Rotate(defaultsmallRing2, (1 - i) * Math.PI / 3);
        }
        for (int i = 0; i < 2; i++)
        {
          extPoints[6 + 2 * i + 0] = Vector.Rotate(defaultbigRing1, (1 - i) * Math.PI / 3);
          extPoints[6 + 2 * i + 1] = Vector.Rotate(defaultbigRing2, (1 - i) * Math.PI / 3);
        }
        _Points.AddRange(Vector.Rotate(extPoints, Math.PI * (5 - position) / 3));
      }

    }
    public Vector[] AsArray()
    {
      Vector[] result = _Points.ToArray();
      for (int i = 0; i < result.Length; i++)
      {
        result[i] += ParentVector;
      }
      return result;
    }
  }

  /// <summary>
  /// contain 6 small gex blocks & LOGO & border
  /// </summary>
  public struct mainGexBlock
  {
    internalGexBlock[] internalGexses;
    externalGexBlock[] externalGexses;
    extrenalBorder[] externalBorders;
    public mainGexBlock(double Height)
    {
      internalGexses = new internalGexBlock[6];
      externalGexses = new externalGexBlock[6];
      externalBorders = new extrenalBorder[5];

      double l = Height / (3 * Math.Sqrt(3)) - 10;
      Vector Center = new Vector(Height / 2 + 30, Height / 2 + 30);
      Vector mainGexRingDefault = new Vector(-l * 1.5, l * Math.Sqrt(3) / 2);

      internalGexses[0] = new internalGexBlock(Center, l);
      externalGexses[0] = new externalGexBlock(Center, l, 0);
      for (int i = 0; i < 5; i++)
      {
        internalGexses[i + 1] = new internalGexBlock(Center + Vector.Rotate(mainGexRingDefault, -Math.PI / 3 * i), internalGexses[0].Points);
        externalGexses[i + 1] = new externalGexBlock(Center + Vector.Rotate(mainGexRingDefault, -Math.PI / 3 * i), l, i + 1);
        externalBorders[i] = new extrenalBorder(Center + Vector.Rotate(mainGexRingDefault, -Math.PI / 3 * i), l, i);
      }
    }
    public Vector[] AsArray()
    {
      List<Vector> ResultArray = new List<Vector>();
      for (int i = 0; i < 6; i++)
      {
        ResultArray.AddRange(internalGexses[i].AsArray());
        ResultArray.AddRange(externalGexses[i].AsArray());
      }
     
      return ResultArray.ToArray();
    }
    public Vector[] AsArrayBorder()
    {
      List<Vector> ResultArray = new List<Vector>();
     
      for (int i = 0; i < 5; i++)
      {
        ResultArray.AddRange(externalBorders[i].AsArray());
      }
      return ResultArray.ToArray();
    }
  }
}
