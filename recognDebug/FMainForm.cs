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
using System.Media;

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
      if (CroptedImageBox.InvokeRequired)
        CroptedImageBox.BeginInvoke(new Action(() => CroptedImageBox.Image = sourse));
      else
        CroptedImageBox.Image = sourse;
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
        CvInvoke.DrawContours(RawImageBox.Image, contourList, -1, new Bgr(Color.Green).MCvScalar, 3);
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
      RawImageBox.SizeMode = PictureBoxSizeMode.Zoom;
      FilteredImageBox.SizeMode = PictureBoxSizeMode.Zoom;
      CroptedImageBox.SizeMode = PictureBoxSizeMode.Zoom;
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
      ImageParser.OnHexImageCropted += ImageParser_OnHexImageCropted;
      if (Directory.Exists(dirname))
      {
        foreach (string fileName in Directory.GetFiles(dirname).Where(d=> !d.Contains("pass")))
        {
          currentFileName = fileName;
          ImageParser.RecieveImage(new Image<Bgr, Byte>(fileName));
          SystemSounds.Beep.Play();

        }
      }
    }

    void ImageParser_OnHexImageCropted(Image<Bgr, byte> sourse)
    {
      sourse.ToBitmap().Save(currentFileName.Replace(".jpg", "_passCrop.png"), System.Drawing.Imaging.ImageFormat.Png);
      
    }

    void ImageParser_OnImageFiltered(Image<Hsv, byte> sourse)
    {
      //sourse.ToBitmap().Save(currentFileName.Replace(".jpg","_pass.png"), System.Drawing.Imaging.ImageFormat.Png);
      //(new Image<Bgr, Byte>(currentFileName)).SmoothMedian(9).ToBitmap().Save(currentFileName.Replace(".jpg", "_pass_blur.png"), System.Drawing.Imaging.ImageFormat.Png);
    }
  }
}
