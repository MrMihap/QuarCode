using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
namespace Quarcode.Core
{
  public static class CLogger
  {
    public static string eventPath = Path.GetDirectoryName(Application.ExecutablePath) + "/events.log";
    public static string errorPath = Path.GetDirectoryName(Application.ExecutablePath) + "/errors.log";
    public static void WriteLine(string msg, bool IsError = false)
    {
      StreamWriter sw = new StreamWriter((IsError) ? errorPath : eventPath, true);
      sw.WriteLine(DateTime.Now.ToString() + ": " + msg);
      sw.Close();
    }
  }
}
