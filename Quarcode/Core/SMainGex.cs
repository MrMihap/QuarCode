using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
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
    externalGexBlock LogoExternal;
    public double L;
    public mainGexBlock(double Height)
    {
      internalGexses = new internalGexBlock[6];
      externalGexses = new externalGexBlock[6];
      externalBorders = new externalBorder[6];

      L = Height / ( (3 * Math.Sqrt(3)) + 2/(Math.Sqrt(3) + 1)) ;
      Vector Center = new Vector(Height / 2 + 30, Height / 2 + 30);
      Vector mainGexRingDefault = new Vector(-L * 1.5, L * Math.Sqrt(3) / 2);

      internalGexses[0] = new internalGexBlock(Center, L);
      externalGexses[0] = new externalGexBlock(Center, L, 0);
      externalBorders[5] = new externalBorder(Center + Vector.Rotate(mainGexRingDefault, -Math.PI * 5 / 3), L, 5);
      LogoExternal = new externalGexBlock(Center + Vector.Rotate(mainGexRingDefault, -Math.PI / 3 * 5), L, 6);
      LogoInternal = new internalGexBlock(Center + Vector.Rotate(mainGexRingDefault, (-Math.PI / 3) * 5), L);
      for (int i = 0; i < 5; i++)
      {
        internalGexses[i + 1] = new internalGexBlock(Center + Vector.Rotate(mainGexRingDefault, -Math.PI / 3 * i), internalGexses[0].Points);
        externalGexses[i + 1] = new externalGexBlock(Center + Vector.Rotate(mainGexRingDefault, -Math.PI / 3 * i), L, i + 1);
        externalBorders[i] = new externalBorder(Center + Vector.Rotate(mainGexRingDefault, -Math.PI / 3 * i), L, i);
      }
      ExportToFile("out.txt");
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
      //ResultArray.AddRange(externalBorders[5].AsArray());

     // ResultArray.AddRange(LogoInternal.AsArray());
      return ResultArray.ToArray();
    }
   
    public Vector[] AsArrayLogo()
    {
      List<Vector> ResultArray = new List<Vector>();
      ResultArray.AddRange(LogoInternal.AsArray());
      ResultArray.AddRange(LogoExternal.AsArray());
      return ResultArray.ToArray();
    }

    public void ExportToFile(string Path)
    {
      using (StreamWriter sw = new StreamWriter(Path))
      {
        Vector[] data = this.AsArray();
        int n = data.Length;
        for (int i = 0; i < data.Length; i++)
          sw.WriteLine(i.ToString() + " " + data[i].x + " " + data[i].y);
       
        data = this.AsArrayLogo();
        for (int i = 0; i < data.Length; i++)
          sw.WriteLine((i + n).ToString() + " " + data[i].x + " " + data[i].y);
        sw.WriteLine("*******************************");
        data = this.AsArrayBorder();
        for (int i = 0; i < data.Length; i++)
        {
          sw.WriteLine(data[i].x + " " + data[i].y);
        }
      }
    }
  }
}
