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
      #region Manual Point Add
      bitPoints.Clear();
      bitPoints.Add(new PointF(316.7989f, 434.8057f));
      bitPoints.Add(new PointF(258.6298f, 401.2213f));
      bitPoints.Add(new PointF(316.7989f, 367.6369f));
      bitPoints.Add(new PointF(374.9680f, 401.2213f));
      bitPoints.Add(new PointF(374.9680f, 468.3901f));
      bitPoints.Add(new PointF(316.7989f, 501.9744f));
      bitPoints.Add(new PointF(258.6298f, 468.3901f));
      bitPoints.Add(new PointF(188.8339f, 468.3901f));
      bitPoints.Add(new PointF(224.1768f, 528.3177f));
      bitPoints.Add(new PointF(281.9010f, 562.4156f));
      bitPoints.Add(new PointF(351.6968f, 562.4156f));
      bitPoints.Add(new PointF(409.8659f, 528.8313f));
      bitPoints.Add(new PointF(444.7638f, 468.3901f));
      bitPoints.Add(new PointF(444.7638f, 401.2213f));
      bitPoints.Add(new PointF(409.8659f, 340.7801f));
      bitPoints.Add(new PointF(351.6968f, 307.1957f));
      bitPoints.Add(new PointF(281.9010f, 307.1957f));
      bitPoints.Add(new PointF(223.7318f, 273.6114f));
      bitPoints.Add(new PointF(223.7318f, 340.7801f));
      bitPoints.Add(new PointF(188.8339f, 401.2213f));
      bitPoints.Add(new PointF(409.8659f, 273.6114f));
      bitPoints.Add(new PointF(223.7318f, 596.0000f));
      bitPoints.Add(new PointF(130.6648f, 434.8057f));
      bitPoints.Add(new PointF(502.9330f, 434.8057f));
      bitPoints.Add(new PointF(409.8659f, 596.0000f));
      bitPoints.Add(new PointF(130.6648f, 757.1943f));
      bitPoints.Add(new PointF(223.7318f, 918.3886f));
      bitPoints.Add(new PointF(409.8659f, 918.3886f));
      bitPoints.Add(new PointF(502.9330f, 1079.5830f));
      bitPoints.Add(new PointF(689.0670f, 1079.5830f));
      bitPoints.Add(new PointF(782.1341f, 918.3886f));
      bitPoints.Add(new PointF(968.2682f, 918.3886f));
      bitPoints.Add(new PointF(1061.3352f, 757.1943f));
      bitPoints.Add(new PointF(502.9330f, 112.4170f));
      bitPoints.Add(new PointF(689.0670f, 112.4170f));
      bitPoints.Add(new PointF(782.1341f, 273.6114f));
      bitPoints.Add(new PointF(968.2682f, 273.6114f));
      bitPoints.Add(new PointF(1061.3352f, 434.8057f));
      bitPoints.Add(new PointF(968.2682f, 596.0000f));
      bitPoints.Add(new PointF(875.2011f, 757.1943f));
      bitPoints.Add(new PointF(689.0670f, 757.1943f));
      bitPoints.Add(new PointF(596.0000f, 918.3886f));
      bitPoints.Add(new PointF(502.9330f, 757.1943f));
      bitPoints.Add(new PointF(316.7989f, 757.1943f));
      bitPoints.Add(new PointF(596.0000f, 596.0000f));
      bitPoints.Add(new PointF(782.1341f, 596.0000f));
      bitPoints.Add(new PointF(596.0000f, 273.6114f));
      bitPoints.Add(new PointF(875.2011f, 434.8057f));
      bitPoints.Add(new PointF(689.0670f, 434.8057f));
      bitPoints.Add(new PointF(502.9330f, 179.5858f));
      bitPoints.Add(new PointF(537.8309f, 240.0270f));
      bitPoints.Add(new PointF(468.0350f, 240.0270f));
      bitPoints.Add(new PointF(689.0670f, 179.5858f));
      bitPoints.Add(new PointF(723.9650f, 240.0270f));
      bitPoints.Add(new PointF(654.1691f, 240.0270f));
      bitPoints.Add(new PointF(630.8979f, 146.0014f));
      bitPoints.Add(new PointF(561.1021f, 146.0014f));
      bitPoints.Add(new PointF(596.0000f, 206.4426f));
      bitPoints.Add(new PointF(723.9650f, 307.1957f));
      bitPoints.Add(new PointF(654.1691f, 307.1957f));
      bitPoints.Add(new PointF(689.0670f, 367.6369f));
      bitPoints.Add(new PointF(537.8309f, 307.1957f));
      bitPoints.Add(new PointF(468.0350f, 307.1957f));
      bitPoints.Add(new PointF(502.9330f, 367.6369f));
      bitPoints.Add(new PointF(596.0000f, 340.7801f));
      bitPoints.Add(new PointF(630.8979f, 401.2213f));
      bitPoints.Add(new PointF(561.1021f, 401.2213f));
      bitPoints.Add(new PointF(782.1341f, 340.7801f));
      bitPoints.Add(new PointF(817.0320f, 401.2213f));
      bitPoints.Add(new PointF(747.2362f, 401.2213f));
      bitPoints.Add(new PointF(968.2682f, 340.7801f));
      bitPoints.Add(new PointF(1003.1661f, 401.2213f));
      bitPoints.Add(new PointF(933.3702f, 401.2213f));
      bitPoints.Add(new PointF(910.0990f, 307.1957f));
      bitPoints.Add(new PointF(840.3032f, 307.1957f));
      bitPoints.Add(new PointF(875.2011f, 367.6369f));
      bitPoints.Add(new PointF(1003.1661f, 468.3901f));
      bitPoints.Add(new PointF(933.3702f, 468.3901f));
      bitPoints.Add(new PointF(968.2682f, 528.8313f));
      bitPoints.Add(new PointF(817.0320f, 468.3901f));
      bitPoints.Add(new PointF(747.2362f, 468.3901f));
      bitPoints.Add(new PointF(782.1341f, 528.8313f));
      bitPoints.Add(new PointF(875.2011f, 501.9744f));
      bitPoints.Add(new PointF(910.0990f, 562.4156f));
      bitPoints.Add(new PointF(840.3032f, 562.4156f));
      bitPoints.Add(new PointF(782.1341f, 663.1687f));
      bitPoints.Add(new PointF(817.0320f, 723.6099f));
      bitPoints.Add(new PointF(747.2362f, 723.6099f));
      bitPoints.Add(new PointF(968.2682f, 663.1687f));
      bitPoints.Add(new PointF(1003.1661f, 723.6099f));
      bitPoints.Add(new PointF(933.3702f, 723.6099f));
      bitPoints.Add(new PointF(910.0990f, 629.5844f));
      bitPoints.Add(new PointF(840.3032f, 629.5844f));
      bitPoints.Add(new PointF(875.2011f, 690.0256f));
      bitPoints.Add(new PointF(1003.1661f, 790.7787f));
      bitPoints.Add(new PointF(933.3702f, 790.7787f));
      bitPoints.Add(new PointF(968.2682f, 851.2199f));
      bitPoints.Add(new PointF(817.0320f, 790.7787f));
      bitPoints.Add(new PointF(747.2362f, 790.7787f));
      bitPoints.Add(new PointF(782.1341f, 851.2199f));
      bitPoints.Add(new PointF(875.2011f, 824.3631f));
      bitPoints.Add(new PointF(910.0990f, 884.8043f));
      bitPoints.Add(new PointF(840.3032f, 884.8043f));
      bitPoints.Add(new PointF(502.9330f, 501.9744f));
      bitPoints.Add(new PointF(537.8309f, 562.4156f));
      bitPoints.Add(new PointF(468.0350f, 562.4156f));
      bitPoints.Add(new PointF(689.0670f, 501.9744f));
      bitPoints.Add(new PointF(723.9650f, 562.4156f));
      bitPoints.Add(new PointF(654.1691f, 562.4156f));
      bitPoints.Add(new PointF(630.8979f, 468.3901f));
      bitPoints.Add(new PointF(561.1021f, 468.3901f));
      bitPoints.Add(new PointF(596.0000f, 528.8313f));
      bitPoints.Add(new PointF(723.9650f, 629.5844f));
      bitPoints.Add(new PointF(654.1691f, 629.5844f));
      bitPoints.Add(new PointF(689.0670f, 690.0256f));
      bitPoints.Add(new PointF(537.8309f, 629.5844f));
      bitPoints.Add(new PointF(468.0350f, 629.5844f));
      bitPoints.Add(new PointF(502.9330f, 690.0256f));
      bitPoints.Add(new PointF(596.0000f, 663.1687f));
      bitPoints.Add(new PointF(630.8979f, 723.6099f));
      bitPoints.Add(new PointF(561.1021f, 723.6099f));
      bitPoints.Add(new PointF(223.7318f, 663.1687f));
      bitPoints.Add(new PointF(258.6298f, 723.6099f));
      bitPoints.Add(new PointF(188.8339f, 723.6099f));
      bitPoints.Add(new PointF(409.8659f, 663.1687f));
      bitPoints.Add(new PointF(444.7638f, 723.6099f));
      bitPoints.Add(new PointF(374.9680f, 723.6099f));
      bitPoints.Add(new PointF(351.6968f, 629.5844f));
      bitPoints.Add(new PointF(281.9010f, 629.5844f));
      bitPoints.Add(new PointF(316.7989f, 690.0256f));
      bitPoints.Add(new PointF(444.7638f, 790.7787f));
      bitPoints.Add(new PointF(374.9680f, 790.7787f));
      bitPoints.Add(new PointF(409.8659f, 851.2199f));
      bitPoints.Add(new PointF(258.6298f, 790.7787f));
      bitPoints.Add(new PointF(188.8339f, 790.7787f));
      bitPoints.Add(new PointF(223.7318f, 851.2199f));
      bitPoints.Add(new PointF(316.7989f, 824.3631f));
      bitPoints.Add(new PointF(351.6968f, 884.8043f));
      bitPoints.Add(new PointF(281.9010f, 884.8043f));
      bitPoints.Add(new PointF(502.9330f, 824.3631f));
      bitPoints.Add(new PointF(537.8309f, 884.8043f));
      bitPoints.Add(new PointF(468.0350f, 884.8043f));
      bitPoints.Add(new PointF(689.0670f, 824.3631f));
      bitPoints.Add(new PointF(723.9650f, 884.8043f));
      bitPoints.Add(new PointF(654.1691f, 884.8043f));
      bitPoints.Add(new PointF(630.8979f, 790.7787f));
      bitPoints.Add(new PointF(561.1021f, 790.7787f));
      bitPoints.Add(new PointF(596.0000f, 851.2199f));
      bitPoints.Add(new PointF(723.9650f, 951.9730f));
      bitPoints.Add(new PointF(654.1691f, 951.9730f));
      bitPoints.Add(new PointF(689.0670f, 1012.4142f));
      bitPoints.Add(new PointF(537.8309f, 951.9730f));
      bitPoints.Add(new PointF(468.0350f, 951.9730f));
      bitPoints.Add(new PointF(502.9330f, 1012.4142f));
      bitPoints.Add(new PointF(596.0000f, 985.5574f));
      bitPoints.Add(new PointF(630.8979f, 1045.9986f));
      bitPoints.Add(new PointF(561.1021f, 1045.9986f));
      #endregion
      return;
      //bitPoints.Clear();
      //int PointCount = 0;
      //using (StreamReader sr = new StreamReader(Path))
      //{
      //  while (!sr.EndOfStream)
      //  {
      //    string line = sr.ReadLine();
      //    string[] words = line.Split(' ');
      //    if (words.Length != 2) throw new FileLoadException("wrong Points File");
      //    double x = 0, y = 0;
      //    if (!double.TryParse(words[0], out x))
      //    {
      //      words[0] = words[0].Replace(".", ",");
      //      double.TryParse(words[0], out x);
      //    }
      //    if (!double.TryParse(words[1], out y))
      //    {
      //      words[1] = words[1].Replace(".", ",");
      //      double.TryParse(words[1], out y);
      //    }
      //    PointF p = new PointF((float)x, (float)y);
      //    //if (PointCount > 60)  
      //    bitPoints.Add(p);
      //    //else
      //    //  borderPoints.Add(p);
      //    PointCount++;
      //  }
      //}
    }
  }
}
