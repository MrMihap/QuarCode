using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quarcode.View;
using Quarcode.Interfaces;
using System.Xml;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
namespace Quarcode.Core
{
  public delegate void OnImageReadyDelegate(Bitmap image);
  public class CApplicationController : IDisposable
  {
    public FimgView viewForm;
    public event OnImageReadyDelegate OnImageReady;
    private CPointsMatrix pointsMatrix;
    private IViewInterfaces View;
    private bool IsConfigLoaded = false;
    public CApplicationController()
    {
      //для начала загружаем конфиг из файла
      try
      {
        XMLConfigoader(Path.GetDirectoryName(Application.ExecutablePath) + "/config.xml");
        CLogger.WriteLine("succes cfg load / parsing ");
        pointsMatrix = new CPointsMatrix(3000);

        if (!IsRunningOnMono())
        {
          viewForm = new FimgView();
          View = viewForm as IViewInterfaces;
          //свяжем интерфейс с контроллером
          View.OnMsgGenerateQuery += RecieveMessage;
        }
      }
      catch(Exception e)
      {
        CLogger.WriteLine("Application Controller Ctor Fail: " + e.Message + ", " + e.InnerException, true);
        string searchPath = Path.GetDirectoryName(Application.ExecutablePath);
        if (File.Exists(searchPath + "/config.xml"))
          CLogger.WriteLine("Exist chek test pass succses", true);
        else
        {
          CLogger.WriteLine("Exist chek test faild", true);
          CLogger.WriteLine("App current directory is: " + searchPath, true);
          CLogger.WriteLine("Begin file scan in app directory:", true);
          foreach (string name in Directory.GetFiles(searchPath))
          {
            CLogger.WriteLine(name, true);
          }
          CLogger.WriteLine("File scan end", true);
        }
      }
    }

      
    public void RecieveMessage(SViewState viewState)
    {
      if (!IsConfigLoaded)
      {
        CLogger.WriteLine("config not loaded, app failed", true);
        return;
      }
      if (viewState.ReRand)
        pointsMatrix.GenNoise(viewState.radius);

      Bitmap bmp = CImgBuilder.GenBMPQRfromMatrix(this.pointsMatrix, viewState);
      if(View != null) View.RecieveImg(bmp);
      if (OnImageReady != null) OnImageReady(bmp);
    }

    private void XMLConfigoader(string path)
    {
      if(!File.Exists(path))
        CLogger.WriteLine("config file not found", true);
      XmlDocument doc = new XmlDocument();
      doc.Load(path);

      foreach (XmlElement elem in doc.GetElementsByTagName("color"))
      {
        switch (elem.GetAttribute("type"))
        {
          case "emptyByte":
            CCoder.ByteFalseColors.Add(Common.createColor(elem));
            break;
          case "valueByte":
            CCoder.ByteTrueColors.Add(Common.createColor(elem));
            break;
          case "undefByte":
            CCoder.ByteUndefColors.Add(Common.createColor(elem));
            break;
          case "basicBackground":
            CCoder.BackgroundColors.Add(Common.createColor(elem));
            break;
          case "logoBackground":
            CCoder.LogoCellColors.Add(Common.createColor(elem));
            break;
          case "valueCellBorder":
            CCoder.BorderColors.Add(Common.createColor(elem));
            break;
          case "logoCellBorder":
            CCoder.LogoBorderColors.Add(Common.createColor(elem));
            break;
        }
      }
      IsConfigLoaded = true;
    }

    void IDisposable.Dispose()
    {
      
    }
    public static bool IsRunningOnMono()
    {
      return Type.GetType("Mono.Runtime") != null;
    }
  }
}
