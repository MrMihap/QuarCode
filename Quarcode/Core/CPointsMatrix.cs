using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quarcode.Core
{
  public class  CPointsMatrix
  {
    List<Vector> Points;
    int _Width;
    int _Height;
    // Сводка:
    // Changing make matrix uptodate
    public int Width
    {
      get
      {
        return _Width;
      }
      set
      {
        _Width = value;
        this.InitMatrix();
      }
    }
    // Changing make matrix uptodate
    public int Heigt
    {
      get
      {
        return _Height;
      }
      set
      {
        _Height = value;
        this.InitMatrix();
      }
    }
    public CPointsMatrix()
    {
      Width = 600;
      Heigt = 600;
    }
    void InitMatrix()
    {
      // TODO calc Algorithm
    }
  }
}
