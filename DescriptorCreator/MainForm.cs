using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ExtesionsColor;
using ExtensionsBitmap;

namespace DescriptorCreator
{
	public partial class MainForm : Form
	{
		private Image revertPic;
		private IList<Point> contourPoints;

		public MainForm()
		{
			InitializeComponent();

			foreach (var control in this.controlPanel.Controls)
			{
				((Control)control).Enabled = false;
			}

			this.contourPoints = new List<Point>();
		}

		private void openToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var fileDialog = new OpenFileDialog
			{
				Multiselect = false,
				Filter = "Images|*.jpg;*.jpeg;*.png"
			};

			if (fileDialog.ShowDialog() == DialogResult.OK)
			{
				var image = new Bitmap(fileDialog.FileName);
				image = image.ResizePic(400);

				this.LeafPicture.Height = image.Height;
				this.LeafPicture.Width = image.Width;
				this.LeafPicture.Image = image;
				this.revertPic = (Image)image.Clone();
			}
		}

		private Bitmap NormalizeColorsBitmap(Bitmap image, int thresholdMove)
		{
			var tmpImage = image.SetGrayscale();

			var key = ImageProcessing.Huang(tmpImage.Histogram()) - thresholdMove;

			if (key > 255) key = 255;
			if (key < 0) key = 0;

			bool wasWhite = false;

			this.contourPoints = new List<Point>();
			
			for (var j = 0; j < this.LeafPicture.Height; j++)
				for (var i = 0; i < this.LeafPicture.Width; i++)
				{
					if (image.GetPixel(i, j).R > key)
					{
						if (!wasWhite && i > 0)
							//image.SetPixel(i,j-1,Color.Red);
							this.contourPoints.Add(new Point(i-1,j));

						image.SetPixel(i, j, Color.White);
						wasWhite = true;
					}
					else
					{
						if (wasWhite)
						{
							//image.SetPixel(i, j, Color.Red);
							this.contourPoints.Add(new Point(i, j));
							image.SetPixel(i, j, Color.Black);
						}
						else
						{
							image.SetPixel(i, j, Color.Black);

							if (j > 0 && image.GetPixel(i,j-1) == Color.FromArgb(255,255,255))
								contourPoints.Add(new Point(i, j));

							if (j < this.LeafPicture.Height - 1 && image.GetPixel(i, j + 1).R > key)
								contourPoints.Add(new Point(i, j));
						}
						wasWhite = false;
						
					}
				}
			return image;
		}

		private void NormalizeColors_Click(object sender, EventArgs e)
		{
			this.LeafPicture.Image = this.NormalizeColorsBitmap((Bitmap)this.LeafPicture.Image, Int32.Parse(this.NormalizeTreshold.Text));
		}

		private void LeafPicture_MouseMove(object sender, MouseEventArgs e)
		{
			if (this.LeafPicture.Image != null)
				try
				{
					//this.ArgbLabel.Text = ((Bitmap)this.LeafPicture.Image).GetPixel(e.X, e.Y).ToArgb().ToString();
					//this.ArgbLabel.Text += "\n";

					var pixel = ((Bitmap)this.LeafPicture.Image).GetPixel(e.X, e.Y);

					this.ArgbLabel.Text = e.X + ":" + e.Y + " " + pixel.R + ":" + pixel.G + ":" + pixel.B;
				}
				catch (Exception)
				{
					this.ArgbLabel.Text = "Out";
				}
		}

		private void RevertButton_Click(object sender, EventArgs e)
		{
			this.LeafPicture.Image = (Image)this.revertPic.Clone();
		}

		private void LeafPicture_Paint(object sender, PaintEventArgs e)
		{
			if (this.LeafPicture.Image != null)
				foreach (var control in this.controlPanel.Controls)
				{
					((Control)control).Enabled = true;
				}
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}

		private void pathButton_Click(object sender, EventArgs e)
		{
			var image = (Bitmap)this.LeafPicture.Image.Clone();

			for (var i = 0; i < image.Width; i++)
				for (var j = 0; j < image.Height; j++)
					image.SetPixel(i, j, Color.White);

			foreach (var contourPoint in this.contourPoints)
			{
				try
				{
					image.SetPixel(contourPoint.X, contourPoint.Y, Color.Red);

					//image.SetPixel(contourPoint.X, contourPoint.Y+1, Color.Red);
					//image.SetPixel(contourPoint.X, contourPoint.Y-1, Color.Red);
					//image.SetPixel(contourPoint.X+1, contourPoint.Y+1, Color.Red);
					//image.SetPixel(contourPoint.X+1, contourPoint.Y-1, Color.Red);
					//image.SetPixel(contourPoint.X-1, contourPoint.Y+1, Color.Red);
					//image.SetPixel(contourPoint.X-1, contourPoint.Y-1, Color.Red);
					//image.SetPixel(contourPoint.X+1, contourPoint.Y, Color.Red);
					//image.SetPixel(contourPoint.X-1, contourPoint.Y, Color.Red);
				}
				catch
				{}
				
			}

			//var list = new List<MyPoint>();

			//for (var i = 0; i < image.Width; i++)
			//    for (var j = 0; j < image.Height; j++)
			//    {
			//        if (image.GetPixel(i, j) == Color.FromArgb(0,0,0))
			//        {
			//            if ((i-1 > 0 && i+1 < image.Width && j-1 > 0 && j+1 < image.Height)&&
			//                !(image.GetPixel(i, j + 1) == Color.FromArgb(0, 0, 0) &&
			//                image.GetPixel(i, j - 1) == Color.FromArgb(0, 0, 0) &&
			//                image.GetPixel(i + 1, j) == Color.FromArgb(0, 0, 0) &&
			//                image.GetPixel(i - 1, j) == Color.FromArgb(0, 0, 0) &&
			//                image.GetPixel(i + 1, j + 1) == Color.FromArgb(0, 0, 0) &&
			//                image.GetPixel(i + 1, j - 1) == Color.FromArgb(0, 0, 0) &&
			//                image.GetPixel(i - 1, j + 1) == Color.FromArgb(0, 0, 0) &&
			//                image.GetPixel(i - 1, j - 1) == Color.FromArgb(0, 0, 0)))
			//                list.Add(new MyPoint(i, j));
			//        }
			//    }

			//var path = new ConvexPath().ComputePath(list);

			//Graphics g = Graphics.FromImage(image);

			//Vektor firts = path.ElementAt(0);

			//do
			//{
			//    g.DrawLine(new Pen(Color.Red), firts.Bod.SurX, firts.Bod.SurY, firts.Descendant.Bod.SurX, firts.Descendant.Bod.SurY);
			//    firts = firts.Descendant;
			//}
			//while (!firts.Prvy);

			this.LeafPicture.Image = image;
		}

		private void centroidButton_Click(object sender, EventArgs e)
		{
			var a = 0;
			var cx = 0;
			var cy = 0;

			//centroid formula
			for (var i = 0; i < this.contourPoints.Count() - 1; i++)
			{
				var xi = this.contourPoints.ElementAt(i).X;
				var xii = this.contourPoints.ElementAt(i + 1).X;
				var yi = this.contourPoints.ElementAt(i).Y;
				var yii = this.contourPoints.ElementAt(i + 1).Y;

				a += xi*yii - xii*yi;
				cx += (xi + xii)*(xi*yii - xii*yi);
				cy += (yi + yii)*(xi*yii - xii*yi);
			}

			a /= 2;
			cx /= 6*a;
			cy /= 6*a;

			((Bitmap)this.LeafPicture.Image).SetPixel(cx,cy,Color.Black);
		}
	}
}
