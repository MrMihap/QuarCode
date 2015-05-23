using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quarcode.Core
{
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

  public struct externalBorder
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
    public externalBorder(double borderLength, int position)
    {
      ParentVector = new Vector(0, 0);
      _Points = new List<Vector>();
      init(borderLength, position);
    }
    public externalBorder(Vector parent, double borderLength, int position)
    {
      ParentVector = parent;
      _Points = new List<Vector>();
      init(borderLength, position);
    }
    public externalBorder(Vector parent, Vector[] existsPoints)
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
        Vector defaultsmallRing1 = defaultsmallRing2 + new Vector(TriangleLength * 1 / 2, -TriangleLength * Math.Sqrt(3) / 2);
        Vector defaultbigRing1 = new Vector(-borderLength - TriangleLength * Math.Sqrt(3) / 2, -TriangleLength * 1 / 2);
        Vector defaultbigRing2 = new Vector(-borderLength - TriangleLength * Math.Sqrt(3) / 2, +TriangleLength * 1 / 2);
        List<Vector> extPoints = new List<Vector>();

        for (int i = 0; i < 3; i++)
        {
          extPoints.Add(Vector.Rotate(defaultsmallRing1, (1 - i) * Math.PI / 3));
          extPoints.Add(Vector.Rotate(defaultsmallRing2, (1 - i) * Math.PI / 3));
          if (i < 2)
          {
            extPoints.Add(Vector.Rotate(defaultbigRing1, (1 - i) * Math.PI / 3));
            extPoints.Add(Vector.Rotate(defaultbigRing2, (1 - i) * Math.PI / 3));
          }
        }
        _Points.AddRange(Vector.Rotate(extPoints.ToArray(), Math.PI * (5 - position) / 3));
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

}
