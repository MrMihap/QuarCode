using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Security.Cryptography;
namespace Quarcode.Core
{
  public class byte6
  {
    public byte value;
    public byte6()
    {
    }
    public byte6(int num)
    {
      value = (byte)num;
    }
    public byte6(byte num)
    {
      value = num;
    }
    /// <summary>
    /// Builds 6 bit byte from List of bool
    /// </summary>
    /// <param name="array">source list of bool values</param>
    public byte6(List<bool> array)
    {
      if (array.Count != 6) throw new Exception("invalid array param");
      value = 0;
      for (int i = 0; i < 6; i++)
      {
        value += array[5 - i] ? (byte)Math.Pow(2, i) : (byte)0;
      }
    }
    public List<bool> ToList()
    {
      List<bool> Result = new List<bool>();
      byte subvalue = value;
      for (int i = 0; i < 6; i++)
      {
        Result.Add(subvalue % 2 == 1 ? true : false);
        subvalue /= 2;
      }
      Result.Reverse();
      return Result;
    }
    public static bool operator == (byte6 left, byte6 right)
    {
      if(left.value == right.value) return true;
      return false;
    }
    public static bool operator !=(byte6 left, byte6 right)
    {
      if (left.value == right.value) return false;
      return true;
    }
    public override bool Equals(object obj)
    {
      if (obj is byte6)
        if ((obj as byte6).value == this.value)
          return true;
        else
          return false;
      return base.Equals(obj);
    }
    public override int GetHashCode()
    {
      return value;
    }
  }

  public static class CCoder
  {
    private static Random rand = new Random();
    private static bool IsDictInited = false; 

    public static List<bool> EnCode(String Message, int ResultLength = 72)
    {
      InitCharBytes();
      if (ResultLength < 72) throw new IndexOutOfRangeException("too low target array");

      char[] bytearray = Message.ToCharArray();
      List<Boolean> Result = new List<Boolean>();
      for (int i = 0; i < 12; i++)
      {
        Result.AddRange(ByteChars[bytearray[i]].ToList());
      }
      //укладка хэш функции
      string md5 = GetMd5Sum(Message);
      bytearray = md5.ToCharArray();

      for (int i = 0; i < 10; i++)
      {
        byte6 currentByte;
        if (ByteChars.Keys.Contains(bytearray[i]))
        {
          currentByte = ByteChars[bytearray[i]];
        }
        else
        {
          throw new FormatException("Invalid Symbol in Message");
        }
        List<bool> debug = currentByte.ToList();
        Result.AddRange(debug);
      }
      //нехватающие байты

      for (int i = 0; i < 4; i++)
      {
        Result.Add(true);
      }
      return Result;

    }

    public static string DeCode(List<bool> array)
    {
      
      InitCharBytes();
      char[] result = new char[22];
      //for (int i = 0; i < 22; i++)
      //{
      //  List<bool> debug = array.GetRange(i * 6, 6);
      //  byte6 debug2 = new byte6(debug);
      //  char Litera;
      //  if (CharBytes.ContainsKey(debug2))
      //    Litera = CharBytes[debug2];
      //  else
      //    Litera = '!';
      //  result[i] = Litera;

      //}
      return new string(result);
      
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

    private static Dictionary<byte6, char> CharBytes = new Dictionary<byte6, char>();
    private static Dictionary<char, byte6> ByteChars = new Dictionary<char, byte6>();
    
    private static void InitCharBytes()
    {
      if (IsDictInited)
        return;
      else
        IsDictInited = true;
      byte lowNum = 48;
      byte lowTitle = 97;
      byte lowLittle = 65;

      byte hightNum = 58;
      byte hightTitle = 123;
      byte hightLittle = 91;

      CharBytes.Clear();
      ByteChars.Clear();

      for (byte i = lowNum; i < hightNum; i++)
      {
        CharBytes.Add(new byte6(CharBytes.Count), (char)i);
      }
      for (byte i = lowLittle; i < hightLittle; i++)
      {
        CharBytes.Add(new byte6(CharBytes.Count), (char)i);
      }
      for (byte i = lowTitle; i < hightTitle; i++)
      {
        CharBytes.Add(new byte6(CharBytes.Count), (char)i);
      }
      foreach (byte6 key in CharBytes.Keys)
      {
        ByteChars.Add(CharBytes[key], key);
      }
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
