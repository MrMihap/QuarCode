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
using Quarcode.Core;

namespace recognDebug
{
  public partial class FMainForm : Form, IRecieveCroptedImage, IRecieveFilteredImage, IRecieveFoundContours, IRecieveRawImage, IRecieveRecognizedCode
  {
    string dirname = @"../../../testimg/";
    private string currentFileName;

    int CropCount = 0;
    int RecognCount = 0;
    int TotalCount = 0;

    public FMainForm()
    {
      InitializeComponent();
      
    }

    void IRecieveCroptedImage.Recieve(Image<Bgr, Byte> source)
    {

      if (CroptedImageBox.InvokeRequired)
        CroptedImageBox.BeginInvoke(new Action(() => CroptedImageBox.Image = new Image<Bgr, byte>(source.Bitmap)));
      else
        CroptedImageBox.Image = source;
    }

    void IRecieveFilteredImage.Recieve(Image<Hsv, Byte> source)
    {
      if (FilteredImageBox.InvokeRequired)
        FilteredImageBox.BeginInvoke(new Action(() => FilteredImageBox.Image = new Image<Bgr, byte>(source.Bitmap)));
      else
        FilteredImageBox.Image = source;
    }

    void IRecieveFoundContours.Recieve(VectorOfVectorOfPoint contourList)
    {
        CvInvoke.DrawContours(RawImageBox.Image, contourList, -1, new Bgr(Color.Green).MCvScalar, 3);
    }

    void IRecieveRawImage.Recieve(Image<Bgr, Byte> source) 
    {
      if (RawImageBox.InvokeRequired)
        RawImageBox.BeginInvoke(new Action(() => RawImageBox.Image = new Image<Bgr, byte> (source.Bitmap)));
      else
        RawImageBox.Image = source;
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

      //подключение к камере при старте
     // connectToCamStream_Click(this, null);
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
      ImageParser.OnHexCodeRecognized += ImageParser_OnHexCodeRecognized;
      Task fileReadAsync = new Task(ReadFileAsync);
      fileReadAsync.Start();
    }
    void ReadFileAsync()
    {
      int tickCount = Environment.TickCount;
      if (Directory.Exists(dirname))
      {
        foreach (string fileName in Directory.GetFiles(dirname).Where(x => x.Contains("scene") && x.Contains(".jpg")))
        {
          currentFileName = fileName;
          ImageParser.RecieveImage(new Image<Bgr, Byte>(fileName));
          TotalCount++;
        }
      }
      tickCount = Environment.TickCount - tickCount;
      MessageBox.Show("Total time: " + (tickCount / 1000.0).ToString() + "\n Cropted: " + CropCount.ToString() +
        "\n Recogned: " + RecognCount.ToString() + " \n From Total items :" + TotalCount.ToString());
    }

    void ImageParser_OnHexCodeRecognized(string code)
    {
      RecognCount++;
    }

    void ImageParser_OnHexImageCropted(Image<Bgr, byte> source)
    {
      source.ToBitmap().Save(currentFileName.Replace(".jpg", "_passCrop.png"), System.Drawing.Imaging.ImageFormat.Png);
      CropCount++;
    }

    void ImageParser_OnImageFiltered(Image<Hsv, byte> source)
    {
      source.ToBitmap().Save(currentFileName.Replace(".jpg","_pass.png"), System.Drawing.Imaging.ImageFormat.Png);
      //(new Image<Bgr, Byte>(currentFileName)).SmoothMedian(9).ToBitmap().Save(currentFileName.Replace(".jpg", "_pass_blur.png"), System.Drawing.Imaging.ImageFormat.Png);
    }

    private void DecodeTestButton_Click(object sender, EventArgs e)
    {
      const int testCount = 3000;
      int CompareCount = 0;
      for (int i = 0; i < 64; i++)
      {
        byte6 x = new byte6(i);
        List<bool> debug = x.ToList();
        byte6 y = new byte6(debug);
        if (x != y) MessageBox.Show("Error");

      }
     
      for (int i = 0; i < testCount; i++)
      {
        string value = CCoder.genMsg();
        List<bool> array = CCoder.EnCode(value, 72);
        int ticks = Environment.TickCount;
        string result = CCoder.DeCode(array);
        ticks = Environment.TickCount - ticks;
        string subvalue = result.Substring(0, 12);
        if (subvalue.Contains(value)) CompareCount++;
      }
      MessageBox.Show("Test: \nFrom" + testCount.ToString() + " Succes " + CompareCount.ToString());
    }

  }
}
