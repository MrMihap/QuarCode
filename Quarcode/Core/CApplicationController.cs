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
namespace Quarcode.Core
{
  public delegate void OnImageReadyDelegate(Bitmap image);
  public class CApplicationController : IDisposable
  {
    public FimgView viewForm;
    public event OnImageReadyDelegate OnImageReady;
    private CPointsMatrix pointsMatrix;
    private IViewInterfaces View;
    public CApplicationController()
    {
      //для начала загружаем конфиг из файла
      try
      {
        XMLConfigoader("config.xml");

        pointsMatrix = new CPointsMatrix(3000);
        viewForm = new FimgView();
        View = viewForm as IViewInterfaces;
        //свяжем интерфейс с контроллером
        View.OnMsgGenerateQuery += RecieveMessage;

      }
      catch(Exception e)
      {

      }
    }
    
    void View_OnMsgGenerateQuery(SViewState viewState)
    {
    }

    public void RecieveMessage(SViewState viewState)
    {
      if (viewState.ReRand)
        pointsMatrix.GenNoise(viewState.radius);
      Bitmap bmp = CImgBuilder.GenBMPQRfromMatrix(this.pointsMatrix, viewState);
      View.RecieveImg(bmp);
      if (OnImageReady != null) OnImageReady(bmp);
    }

    private void XMLConfigoader(string path)
    {
      if(!File.Exists(path)) throw new Exception("config file not found");
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
    }

    void IDisposable.Dispose()
    {
      
    }
  }
}
