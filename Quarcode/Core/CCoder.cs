using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Security.Cryptography;
namespace Quarcode.Core
{
  public static class CCoder
  {

    private static Random rand = new Random();

    public static List<bool> EnCode(String Message, int ResultLength)
    {
      // lowChars 97-122
      // highChars 65 - 90
      // nums 48 - 57
      if (ResultLength < 72) throw new IndexOutOfRangeException("too low target array");
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
      for (int i = 0; i < bin_msg.Length; i++) tmp.Add(bin_msg[i]);

      //укладка хэш функции
      string md5 = GetMd5Sum(Message);
      for (int i = 0; i < 10; i++)
      {
        x = (byte)md5[i];

        if (char.IsLower(md5[i])) x = (byte)(x - (byte)97);
        else
        {
          if (char.IsUpper(md5[i])) x = (byte)(x - (byte)36); //x-65+26
          else if (char.IsDigit(md5[i])) x = (byte)(x + (byte)4); //x-48+26+26
        }
        for (int j = 0; j <= 5; j++)
          tmp.Add(Convert.ToBoolean(x & (int)Math.Pow(2, j)));
      }
      //нехватающие байты

      for (int i = 0; i < 4; i++)
      {
        tmp.Add(true);
      }
      return tmp;

    }

    public static string DeCode(List<bool> array)
    {
      String result = null;
      int cap = 0;
      for (int i = 0; i < array.Count / 6; i++)
      {
        cap = 0;
        for (int j = 0; j < 6; j++)
        {
          if (array[i * 6 + j]) cap += (int)Math.Pow((double)2, (double)j);
        }
        if (cap >= 0 && cap <= 25) { result += (char)(cap + 97); continue; }
        if (cap >= 26 && cap <= 51) { result += (char)(cap + 39); continue; }
        if (cap >= 52 && cap <= 61) { result += (char)(cap - 4); continue; }

      }
      return result;
    }

    public static Color GetColorFor(PointType pointType)
    {
      switch (pointType)
      {
        case PointType.ByteTrue:
          return ByteTrueColors[rand.Next(0, ByteTrueColors.Count)];
        case PointType.ByteFalse:
          return ByteFalseColors[rand.Next(0, ByteFalseColors.Count)];
        case PointType.UndefinedByte:
          return ByteUndefColors[rand.Next(0, ByteUndefColors.Count)];
        case PointType.Border:
          return BorderColors[rand.Next(0, BorderColors.Count)];
        case PointType.Logo:
          return LogoCellColors[rand.Next(0, LogoCellColors.Count)];
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

    public static string GetMd5Sum(string str)
    {
      MD5 md5 = MD5CryptoServiceProvider.Create();
      byte[] dataMd5 = md5.ComputeHash(Encoding.Default.GetBytes(str));
      StringBuilder sb = new StringBuilder();
      for (int i = 0; i < dataMd5.Length; i++)
        sb.AppendFormat("{0:x2}", dataMd5[i]);
      return sb.ToString();
    }
    
    #region Colors
    public static List<Color> ByteTrueColors = new List<Color>();
    public static List<Color> ByteFalseColors = new List<Color>();
    public static List<Color> ByteUndefColors = new List<Color>();
    //public static List<Color> LogoColors = new List<Color>();
    public static List<Color> LogoCellColors = new List<Color>();
    public static List<Color> LogoBorderColors = new List<Color>();
    public static List<Color> BorderColors = new List<Color>();
    public static List<Color> BackgroundColors = new List<Color>();
    #endregion
  }
}
