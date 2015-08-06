using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;

using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;

using recognTools;

namespace recognDebug
{
  public partial class FMainForm : Form, IRecieveCroptedImage, IRecieveFilteredImage, IRecieveFoundContours, IRecieveRawImage, IRecieveRecognizedCode
  {
    string dirname = @"../../../testimg/";
    private string currentFileName;

    public FMainForm()
    {
      InitializeComponent();
      
    }

    void IRecieveCroptedImage.Recieve(Image<Bgr, Byte> sourse)
    {
      
    }

    void IRecieveFilteredImage.Recieve(Image<Hsv, Byte> sourse)
    {
      if (FilteredImageBox.InvokeRequired)
        FilteredImageBox.BeginInvoke(new Action(() => FilteredImageBox.Image = sourse));
      else
        FilteredImageBox.Image = sourse;
    }

    void IRecieveFoundContours.Recieve(VectorOfVectorOfPoint contourList)
    {
      //Bitmap bmp = RawImageBox.Image.Bitmap;
      //using (Graphics gr = Graphics.FromImage(bmp))
      //{
        //gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

        #region contour drawing
        CvInvoke.DrawContours(RawImageBox.Image, contourList, -1, new Bgr(Color.Green).MCvScalar, 1);
       
        #endregion


      //  Brush br = new SolidBrush(Color.Red);
      //  Pen pen = new Pen(br, 0.5f);
        
      //}
      //bmp = FilteredImageBox.Image.Bitmap; 

      //using (Graphics gr = Graphics.FromImage(FilteredImageBox.Image.Bitmap))
      //{
      //  Brush br = new SolidBrush(Color.Red);
      //  Pen pen = new Pen(br, 2.0f);
        
      //}
    }

    void IRecieveRawImage.Recieve(Image<Bgr, Byte> sourse) 
    {
      if (RawImageBox.InvokeRequired)
        RawImageBox.BeginInvoke(new Action(() => RawImageBox.Image = sourse));
      else
        RawImageBox.Image = sourse;
    }

    void IRecieveRecognizedCode.Recieve(string code) 
    {
      if (LastCodeTextBox.InvokeRequired)
        LastCodeTextBox.BeginInvoke(new Action(() => LastCodeTextBox.Text = code));
      else
        LastCodeTextBox.Text = code;
    }

    private void FMainForm_Load(object sender, EventArgs e)
    {
      ImageParser.AddDevDataReciever(this as object);
      RawImageBox.SizeMode = PictureBoxSizeMode.StretchImage;
      FilteredImageBox.SizeMode = PictureBoxSizeMode.StretchImage;
    }

    private void ExitButton_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private void secondExitButton_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private void LoadSingleImgButton_Click(object sender, EventArgs e)
    {
      OpenFileDialog dialog = new OpenFileDialog();
      switch (dialog.ShowDialog())
      {
        case System.Windows.Forms.DialogResult.OK:
          ImageParser.RecieveImage(new Image<Bgr, Byte>(dialog.FileName));
          break;
        case System.Windows.Forms.DialogResult.Cancel:
          MessageBox.Show("Отменено пользователем");
          break;
      }
    }

    private void connectToCamStream_Click(object sender, EventArgs e)
    {
      //debug mode
      //packet processing
      ImageParser.OnImageFiltered += ImageParser_OnImageFiltered;
      if (Directory.Exists(dirname)) 
      {
        foreach (string fileName in Directory.GetFiles(dirname))
        {
          currentFileName = fileName;
          ImageParser.RecieveImage(new Image<Bgr, Byte>(fileName));
        }
      }
    }

    void ImageParser_OnImageFiltered(Image<Hsv, byte> sourse)
    {
      sourse.ToBitmap().Save(currentFileName.Replace(".jpg","_pass.png"), System.Drawing.Imaging.ImageFormat.Png);
    }
  }
}
