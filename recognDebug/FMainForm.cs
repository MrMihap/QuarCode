using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;

using recognTools;

namespace recognDebug
{
  public partial class FMainForm : Form, IRecieveCroptedImage, IRecieveFilteredImage, IRecieveFoundContours, IRecieveRawImage, IRecieveRecognizedCode
  {
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

    void IRecieveFoundContours.Recieve(List<List<Point>> contourList)
    {
      Bitmap bmp = RawImageBox.Image.Bitmap;
      using (Graphics gr = Graphics.FromImage(bmp))
      {
        //gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
       
        Brush br = new SolidBrush(Color.Red);
        Pen pen = new Pen(br, 0.5f);
        foreach (List<Point> con in contourList)
        {
          gr.DrawLines(pen, con.ToArray());
          gr.DrawLine(pen, con[con.Count - 1], con[0]);
        }
      }
      bmp = FilteredImageBox.Image.Bitmap; 

      using (Graphics gr = Graphics.FromImage(FilteredImageBox.Image.Bitmap))
      {
        Brush br = new SolidBrush(Color.Red);
        Pen pen = new Pen(br, 2.0f);
        foreach (List<Point> con in contourList)
        {
          gr.DrawLines(pen, con.ToArray());
        }
      }
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
  }
}
