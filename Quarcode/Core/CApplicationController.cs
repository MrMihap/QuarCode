using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quarcode.View;
using Quarcode.Interfaces;
namespace Quarcode.Core
{
  class CApplicationController
  {
    public FimgView viewForm;
    private CPointsMatrix pointsMatrix;
    private IViewInterfaces View;
    public CApplicationController()
    {
      pointsMatrix = new CPointsMatrix(700);
      viewForm = new FimgView();
      View = viewForm as IViewInterfaces;
      View.OnMsgGenerateQuery += RecieveMessage;
    }
    void RecieveMessage(SViewState viewState)
    {
      View.RecieveImg(CImgBuilder.GenQRfromMatrix(this.pointsMatrix, viewState));
    }
  }
}
