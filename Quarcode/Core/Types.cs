﻿using System;
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
    public static Vector Rotate(Vector v, double Angle)
    {
      return new Vector(
        v.x * Math.Cos(Angle) - v.y * Math.Sin(Angle),
        v.x * Math.Sin(Angle) + v.y * Math.Cos(Angle));
    }
    public static Vector operator + (Vector a, Vector b)
    {
      return new Vector(a.x + b.x, a.y + b.y);
    }

  }
  public struct GexBlock
  {
    Vector ParentVector;
    public Vector[] Points;
    public GexBlock(double borderLength)
    {
      ParentVector = new Vector(0, 0);
      Points = new Vector[19];
      init(borderLength);
    }
    public GexBlock(Vector parent, double borderLength)
    {
      ParentVector = parent;
      Points = new Vector[19];
      init(borderLength);
    }
    public GexBlock(Vector parent, Vector[] existsPoints)
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
      DefaultSecondRingNeg.Init(TriangleLength * (Math.Sqrt(3) / 2 + 1),  - TriangleLength * 1 / 2);

      Points = new Vector[19];
      Points[0] = new Vector(0, 0);
      for (int i = 0; i < 6; i++)
      {
        Points[i + 1] = Vector.Rotate(DefaultFirstRing,i *  Math.PI/3);
        Points[7 + 2 * i + 1] = Vector.Rotate(DefaultSecondRingPos, i * Math.PI / 3);
        Points[7 + 2 * i + 0] = Vector.Rotate(DefaultSecondRingNeg, i * Math.PI / 3 );
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
  /// <summary>
  /// contain 6 small gex blocks & LOGO & border
  /// </summary>
  public struct mainGexBlock
  {
    GexBlock[] Gexses;
    public mainGexBlock(double Height)
    {
      double l = Height / (3 * Math.Sqrt(3));
      Gexses = new GexBlock[6];
      Vector Center = new Vector(Height / 2, Height / 2);
      Gexses[0] = new GexBlock(Center, l);
      Vector mainGexRingDefault = new Vector(-l * 1.5, l * Math.Sqrt(3) / 2);
      for (int i = 0; i < 5; i++)
      {
        Gexses[i + 1] = new GexBlock(Center + Vector.Rotate(mainGexRingDefault, - Math.PI / 3 * i), Gexses[0].Points);
      }
    }
    public Vector[] AsArray()
    {
      List<Vector> ResultArray = new List<Vector>();
      for (int i = 0; i < 6; i++)
      {
        ResultArray.AddRange(Gexses[i].AsArray());
      }
      return ResultArray.ToArray();
    }
  }
}
