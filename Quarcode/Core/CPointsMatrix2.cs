using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;

namespace Quarcode.Core
{
  public class CPointsMatrix2
  {
    public readonly List<PointF> bitPoints = new List<PointF>();
    public readonly List<PointF> borderPoints = new List<PointF>();

    public CPointsMatrix2()
    {
      bitPoints.Clear();
      borderPoints.Clear();
      LoadPoints(@"Core\bitPoints.txt");
    }
    public void LoadPoints(string Path)
    {
      int PointCount = 0;
      using (StreamReader sr = new StreamReader(Path))
      {
        while (!sr.EndOfStream)
        {
          string line = sr.ReadLine();
          string[] words = line.Split(' ');
          if (words.Length != 2) throw new FileLoadException("wrong Points File");
          double x = 0, y = 0;
          double.TryParse(words[0], out x);
          double.TryParse(words[1], out y);
          PointF p = new PointF((float)x, (float)y);
          if (PointCount > 60)  
            bitPoints.Add(p);
          else
            borderPoints.Add(p);
          PointCount++;
        }
      }
    }
  }
}
