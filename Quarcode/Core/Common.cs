using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Xml;
namespace Quarcode.Core
{
  public struct SViewState
  {
    public int radius;
    public string Message;
    public bool DrawCellBorder;
    public bool DrawValNum;
    public bool DrawQRBorder;
    public bool FillCells;
    public bool ReRand;
  }

  public enum PointType
  {
    ByteTrue,
    ByteFalse,
    UndefinedByte,
    Logo,
    Border,
    LogoAdditional
  }

  public class CGexPoint
  {
    public PointType pointType;

    public Vector r;

    public List<Vector> Cell;

    public CGexPoint(PointType type, Vector center, List<Vector> Surround)
    {
      pointType = type;
      r = center;
      Cell = Surround;
    }

    public void SetType(PointType type)
    {
      this.pointType = type;
    }
  }

  public static class Common
  {
    public static Color createColor(XmlElement elem)
    {
      int r = 0, g = 0, b = 0, a = 255;

      int.TryParse(elem.GetAttribute("r"), out r);
      int.TryParse(elem.GetAttribute("g"), out g);
      int.TryParse(elem.GetAttribute("b"), out b);
      int.TryParse(elem.GetAttribute("a"), out a);
      Color result = Color.FromArgb(a, r, g, b);
      return result;
    }
  }
}
