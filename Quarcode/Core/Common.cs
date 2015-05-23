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
    public bool DrawBorder;
    public bool FillCells;
  }
  public enum PointType
  {
    ByteTrue,
    ByteFalse,
    Logo,
    Border,
    LogoAdditional
  }
}
