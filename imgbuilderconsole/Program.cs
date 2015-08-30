using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Resources;
using System.Drawing;

using Quarcode.Core;
namespace imgbuilderconsole
{
  class Program
  {
    static int Main(string[] args)
    {
      Controller ctrl = new Controller();
      return ctrl.processRequest(args);
    }
  }
  class Controller
  {
    string saveFilePath = @"img\";
    string saveFileName = "qr.png";
    public int processRequest(string[] args)
    {
      if (args.Length == 0 || (args.Length == 1 && args[0].Equals("-h")))
      {
        Console.Write(resourses.help);

      }
      if (args.Length >= 2)
      {
        CApplicationController controller = new CApplicationController();
        SViewState viewState = new SViewState();
        viewState.DrawCellBorder = true;
        viewState.DrawQRBorder = false;
        viewState.DrawValNum = false;
        viewState.FillCells = true;
        viewState.Message = args[0];
        viewState.radius = 30;
        saveFilePath = args[1];
        if (args.Length == 3)
          saveFileName = args[2] + ".png";
        controller.OnImageReady += controller_OnImageReady;
        controller.RecieveMessage(viewState);
        
      }
      Console.ReadKey();
      return 1;
    }

    void controller_OnImageReady(System.Drawing.Bitmap image)
    {
      Console.WriteLine("build success");
      CImgBuilder.saveToFile(image, @saveFilePath + @saveFileName);
    }
  }

}
