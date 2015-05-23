using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Quarcode.Core
{
  /// <summary>
  /// contain 6 small gex blocks & LOGO & border
  /// </summary>
  public struct mainGexBlock
  {
    internalGexBlock[] internalGexses;
    externalGexBlock[] externalGexses;
    externalBorder[] externalBorders;
    internalGexBlock LogoInternal;
    externalGexBlock Logo;
    public mainGexBlock(double Height)
    {
      internalGexses = new internalGexBlock[6];
      externalGexses = new externalGexBlock[6];
      externalBorders = new externalBorder[6];

      double l = Height / (3 * Math.Sqrt(3)) - 10;
      Vector Center = new Vector(Height / 2 + 30, Height / 2 + 30);
      Vector mainGexRingDefault = new Vector(-l * 1.5, l * Math.Sqrt(3) / 2);

      internalGexses[0] = new internalGexBlock(Center, l);
      externalGexses[0] = new externalGexBlock(Center, l, 0);
      externalBorders[5] = new externalBorder(Center + Vector.Rotate(mainGexRingDefault, -Math.PI * 5 / 3), l, 5);
      Logo = new externalGexBlock(Center + Vector.Rotate(mainGexRingDefault, -Math.PI / 3 * 5), l, 0);
      LogoInternal = new internalGexBlock(Center + Vector.Rotate(mainGexRingDefault, (-Math.PI / 3) * 5), l);
      for (int i = 0; i < 5; i++)
      {
        internalGexses[i + 1] = new internalGexBlock(Center + Vector.Rotate(mainGexRingDefault, -Math.PI / 3 * i), internalGexses[0].Points);
        externalGexses[i + 1] = new externalGexBlock(Center + Vector.Rotate(mainGexRingDefault, -Math.PI / 3 * i), l, i + 1);
        externalBorders[i] = new externalBorder(Center + Vector.Rotate(mainGexRingDefault, -Math.PI / 3 * i), l, i);
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
      for (int i = 0; i < externalBorders.Length; i++)
      {
        ResultArray.AddRange(externalBorders[i].AsArray());
      }

      ResultArray.AddRange(LogoInternal.AsArray());
      return ResultArray.ToArray();
    }

    public Vector[] AsArrayLogoBorder()
    {
      List<Vector> ResultArray = new List<Vector>();
      ResultArray.AddRange(Logo.AsArray());
      ResultArray.AddRange(LogoInternal.AsArray());
      return ResultArray.ToArray();
    }
  }
}
