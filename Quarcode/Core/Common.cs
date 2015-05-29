using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
}
