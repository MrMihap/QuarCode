using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
namespace Quarcode.Core
{
  public static class CCoder
  {
 
    private static Random rand = new Random();

    public static List<bool> EnCode(String Message)
    {
      // lowChars 97-122
      // highChars 65 - 90
      // nums 48 - 57
      Boolean[] bin_msg = new Boolean[72];
      Byte x;
      List<Boolean> tmp = new List<Boolean>();

      for (int i = 0; i < 12; i++)
      {
        x = (byte)Message[i];

        if (char.IsLower(Message[i])) x = (byte)(x - (byte)97);
        else
        {
          if (char.IsUpper(Message[i])) x = (byte)(x - (byte)36); //x-65+26
          else if (char.IsDigit(Message[i])) x = (byte)(x + (byte)4); //x-48+26+26
        }
        for (int j = 0; j <= 5; j++) bin_msg[i * 6 + 5 - j] = Convert.ToBoolean(x & (int)Math.Pow(2, j));
      }
      for (int i = 0; i < 72; i++) tmp.Add(bin_msg[i]);
      return tmp;

    }

    public static Color GetColorFor(PointType pointType) 
    {
      switch (pointType)
      {
        case PointType.ByteTrue:
          return ByteTrueColors[rand.Next(0, ByteTrueColors.Length)];
        case PointType.ByteFalse:
          return ByteFalseColors[rand.Next(0, ByteFalseColors.Length)];
        case PointType.Border:
          return BorderColors[rand.Next(0, BorderColors.Length)];
        case PointType.Logo:
          return LogoColors[rand.Next(0, LogoColors.Length)];
        case PointType.LogoAdditional:
          return LogoAddColors[rand.Next(0, LogoAddColors.Length)];
      }
      return Color.White;
    }
    
    public static String genMsg()
    {
      // lowChars 97-122
      // highChars 65 - 90
      // nums 48 - 57
      String k = "";
      Random rnd = new Random();
      int l = 0;
      l = rnd.Next(0, 3);

      for (int i = 0; i < 12; i++)
      {
        switch (l)
        {
          case 0: k = k + (char)(rnd.Next(48, 58)); break;
          case 1: k = k + (char)(rnd.Next(97, 123)); break;
          case 2: k = k + (char)(rnd.Next(65, 91)); break;
        }
        l = rnd.Next(0, 3);
      }
      return k;
    }

    #region Colors
    private static Color[] ByteTrueColors = new Color[] { 
      Color.Red, 
      Color.Orange,
      Color.Yellow,
      Color.Green,
      Color.SkyBlue,
      Color.Blue,
      Color.AliceBlue 
      };
    private static Color[] ByteFalseColors = new Color[] { 
      Color.White
      };
    private static Color[] LogoColors = new Color[] { 
      Color.Yellow
      };
    private static Color[] LogoAddColors = new Color[] { 
      Color.Yellow
      };
    private static Color[] BorderColors = new Color[] { 
      Color.Black
      };
    #endregion
  }
}
