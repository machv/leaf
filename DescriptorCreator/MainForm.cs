﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ExtesionsColor;
using ExtensionsBitmap;
using ExtensionsPoint;

namespace DescriptorCreator
{
	public partial class MainForm : Form
	{
		private Image revertPic;
		private Bitmap threshold;
		private IList<Point> contourPoints;
		private Point centroid;

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
			this.threshold = image;

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
			var centroid = new Point();
			var image = this.threshold;

			try
			{
				var list = new List<Point>();
				for (var i = 0; i < image.Width; i++)
					for (var j = 0; j < image.Height; j++)
					{
						if (image.GetPixel(i, j) == Color.FromArgb(0, 0, 0))
						{
							if ((i - 1 > 0 && i + 1 < image.Width && j - 1 > 0 && j + 1 < image.Height) &&
								!(image.GetPixel(i, j + 1) == Color.FromArgb(0, 0, 0) &&
								image.GetPixel(i, j - 1) == Color.FromArgb(0, 0, 0) &&
								image.GetPixel(i + 1, j) == Color.FromArgb(0, 0, 0) &&
								image.GetPixel(i - 1, j) == Color.FromArgb(0, 0, 0) &&
								image.GetPixel(i + 1, j + 1) == Color.FromArgb(0, 0, 0) &&
								image.GetPixel(i + 1, j - 1) == Color.FromArgb(0, 0, 0) &&
								image.GetPixel(i - 1, j + 1) == Color.FromArgb(0, 0, 0) &&
								image.GetPixel(i - 1, j - 1) == Color.FromArgb(0, 0, 0)))
								list.Add(new Point(i, j));
						}
					}

				centroid = ImageProcessing.Centroid(list);
			}
			catch
			{}

			var cx = centroid.X >= 0 ? centroid.X : 0;
			var cy = centroid.Y >= 0 ? centroid.Y : 0;

			this.centroid = centroid;

			image = (Bitmap)this.LeafPicture.Image.Clone();

			image.SetPixel(cx,cy,Color.Black);
			image.SetPixel(cx+1, cy, Color.Black);
			image.SetPixel(cx, cy+1, Color.Black);
			image.SetPixel(cx+1, cy+1, Color.Black);

			this.LeafPicture.Image = (Image)image.Clone();
		}

		private void centroidLinesButton_Click(object sender, EventArgs e)
		{
			var image = (Bitmap) this.LeafPicture.Image;

			const int step = 20; //divide PI/2 into *step* pieces
			const double EPSILON = 0.01;
			//var y = this.centroid.Y;
			//var x = image.Width - centroid.X;

			var g = Graphics.FromImage(image);

			var refPoints = new List<Point>();

			//var ftan = Math.Tan(Math.PI / 4 - 1 * Math.PI / 4 / step);
			//round 1
			for (var i = 1; i <= step; i++)
			{

				var tan = Math.Tan(Math.PI/4 - i*Math.PI/4/step + Math.PI/4);
				var points = this.contourPoints.Where(p => ((p.X >= centroid.X)
					&& (Math.Abs((double)(centroid.Y - p.Y) / (p.X - centroid.X) - tan) < /*(2 / (ftan + 1 - tan)) +*/ EPSILON)));

				if (points.Count() > 0)
				{
					points = (from c in points
							  orderby c.Distance(centroid) descending
							  select c);

					g.DrawLine(new Pen(Color.Blue), centroid, points.First());
					refPoints.Add(points.First());
				}
				else
				{
					//var tg = Math.Tan(Math.PI/4 - i*Math.PI/4/step);
					var x = tan != 0 ? Convert.ToInt32(centroid.Y / tan) : 0;
					if (x < image.Width - centroid.X)
					{
						g.DrawLine(new Pen(Color.ForestGreen), centroid, new Point(x + centroid.X, 0));
						
						var p = this.IntersectionPoint(image,new Point(x+centroid.X,0), centroid, false);
						refPoints.Add(p);

						g.DrawLine(new Pen(Color.White), centroid, new Point(x + centroid.X, 0));
						g.DrawLine(new Pen(Color.Blue), centroid, p);
					}
					else
					{
						var y = Convert.ToInt32(tan * (image.Width - centroid.X));
						g.DrawLine(new Pen(Color.ForestGreen), centroid, new Point(image.Width - 1, y));
						var p = this.IntersectionPoint(image, new Point(image.Width-1, y), centroid, false);
						refPoints.Add(p);

						g.DrawLine(new Pen(Color.White), centroid, new Point(image.Width - 1, y));
						g.DrawLine(new Pen(Color.Blue), centroid, p);
						//image.SetPixel(image.Width - 1, y, Color.Yellow);
					}
				}

			}

			//round 2
			for (var i = 1; i <= step; i++)
			{
				var tan = Math.Tan(Math.PI / 4 - i * Math.PI / 4 / step + 0*Math.PI / 4);
				var points = this.contourPoints.Where(p => ((p.X >= centroid.X)
					&& (Math.Abs((double)(centroid.Y - p.Y) / (p.X - centroid.X) - tan) < /*(2 / (ftan + 1 - tan)) +*/ EPSILON)));

				if (points.Count() > 0)
				{
					points = (from c in points
							  orderby c.Distance(centroid) descending
							  select c);

					g.DrawLine(new Pen(Color.Blue), centroid, points.First());
					refPoints.Add(points.First());
				}
				else
				{
					var x = tan != 0 ? Convert.ToInt32(centroid.Y / tan) : 0;
					if (x < image.Width - centroid.X)
					{
						g.DrawLine(new Pen(Color.ForestGreen), centroid, new Point(x + centroid.X, 0));

						var p = this.IntersectionPoint(image, new Point(x + centroid.X, 0), centroid, false);
						refPoints.Add(p);

						g.DrawLine(new Pen(Color.White), centroid, new Point(x + centroid.X, 0));
						g.DrawLine(new Pen(Color.Blue), centroid, p);
					}
					else
					{
						var y = Convert.ToInt32(tan * (image.Width - centroid.X));
						g.DrawLine(new Pen(Color.ForestGreen), centroid, new Point(image.Width - 1, y));
						var p = this.IntersectionPoint(image, new Point(image.Width - 1, y), centroid, false);
						refPoints.Add(p);

						g.DrawLine(new Pen(Color.White), centroid, new Point(image.Width - 1, y));
						g.DrawLine(new Pen(Color.Blue), centroid, p);
					}
				}
			}

			//round 3
			for (var i = 1; i <= step; i++)
			{
				var tan = Math.Tan(Math.PI / 4 - i * Math.PI / 4 / step + -1 * Math.PI / 4);
				var points = this.contourPoints.Where(p => ((p.X >= centroid.X)
					&& (Math.Abs((double)(centroid.Y - p.Y) / (p.X - centroid.X) - tan) < /*(2 / (ftan + 1 - tan)) +*/ EPSILON)));

				if (points.Count() > 0)
				{
					points = (from c in points
							  orderby c.Distance(centroid) descending
							  select c);

					g.DrawLine(new Pen(Color.Blue), centroid, points.First());
					refPoints.Add(points.First());
				}
				else
				{
					var x = tan != 0 ? Convert.ToInt32((centroid.Y - image.Height + 1) / tan) : 0;
					if (x < image.Width - centroid.X)
					{
						g.DrawLine(new Pen(Color.ForestGreen), centroid, new Point(x + centroid.X, image.Height - 1));

						var p = this.IntersectionPoint(image, new Point(x + centroid.X, image.Height - 1), centroid, false);
						refPoints.Add(p);

						g.DrawLine(new Pen(Color.White), centroid, new Point(x + centroid.X, image.Height - 1));
						g.DrawLine(new Pen(Color.Blue), centroid, p);
					}
					else
					{
						var y = centroid.Y - Convert.ToInt32(tan * (image.Width - centroid.X));
						g.DrawLine(new Pen(Color.ForestGreen), centroid, new Point(image.Width - 1, y));
						var p = this.IntersectionPoint(image, new Point(image.Width - 1, y), centroid, false);
						refPoints.Add(p);

						g.DrawLine(new Pen(Color.White), centroid, new Point(image.Width - 1, y));
						g.DrawLine(new Pen(Color.Blue), centroid, p);
					}
				}
			}

			//round 4
			for (var i = 1; i <= step; i++)
			{
				var tan = Math.Tan(Math.PI / 4 - i * Math.PI / 4 / step + -2 * Math.PI / 4);
				var points = this.contourPoints.Where(p => ((p.X >= centroid.X)
					&& (Math.Abs((double)(centroid.Y - p.Y) / (p.X - centroid.X) - tan) < /*(2 / (ftan + 1 - tan)) +*/ EPSILON)));

				if (points.Count() > 0)
				{
					points = (from c in points
					          orderby c.Distance(centroid) descending
					          select c);

					g.DrawLine(new Pen(Color.Blue), centroid, points.First());
					refPoints.Add(points.First());
				}
				else
				{
					var x = tan != 0 ? Convert.ToInt32((centroid.Y - image.Height + 1) / tan) : 0;
					if (x < image.Width - centroid.X)
					{
						g.DrawLine(new Pen(Color.ForestGreen), centroid, new Point(x + centroid.X, image.Height - 1));

						var p = this.IntersectionPoint(image, new Point(x + centroid.X, image.Height - 1), centroid, false);
						refPoints.Add(p);

						g.DrawLine(new Pen(Color.White), centroid, new Point(x + centroid.X, image.Height - 1));
						g.DrawLine(new Pen(Color.Blue), centroid, p);
					}
					else
					{
						var y = centroid.Y - Convert.ToInt32(tan * (image.Width - centroid.X));
						g.DrawLine(new Pen(Color.ForestGreen), centroid, new Point(image.Width - 1, y));
						var p = this.IntersectionPoint(image, new Point(image.Width - 1, y), centroid, false);
						refPoints.Add(p);

						g.DrawLine(new Pen(Color.White), centroid, new Point(image.Width - 1, y));
						g.DrawLine(new Pen(Color.Blue), centroid, p);
					}
				}
			}

			//for (var i = 0; i <= Math.PI / 2 / step; i++)
			//{
			//    var points = this.contourPoints.Where(p => ((p.X > centroid.X)
			//        && (Math.Abs((double)(centroid.Y - p.Y) / (p.X - centroid.X) - Math.Tan(Math.PI - i * step + Math.PI)) < /*3 / step * Math.PI / i **/ EPSILON)));

			//    if (points.Count() > 0)
			//    {
			//        points = i%2 == 0
			//                    ? (from c in points
			//                       orderby c.Distance(centroid) descending
			//                       select c)
			//                    : (from c in points
			//                       orderby c.Distance(centroid) ascending
			//                       select c);

			//        g.DrawLine(new Pen(Color.Blue), centroid, points.First());

			//    }
			//}

			this.LeafPicture.Image = image;
		}

		private Point IntersectionPoint(Bitmap image, Point point, Point centroidPoint, bool closest)
		{
			const int UP = 1;
			const int DOWN = 1 << 1;
			const int RIGHT = 1 << 2;
			const int LEFT = 1 << 3;
			const int UP_LEFT = UP | LEFT;
			const int DOWN_LEFT = DOWN | LEFT;
			const int UP_RIGHT = UP | RIGHT;
			const int DOWN_RIGHT = DOWN | RIGHT;

			var direction = 0;

			var deltaX = point.X - centroidPoint.X;
			var deltaY = point.Y - centroidPoint.Y;

			direction |= deltaX > 0 ? LEFT : deltaX < 0 ? RIGHT : 0;
			direction |= deltaY > 0 ? UP : deltaY < 0 ? DOWN : 0;

			while (!point.RedAround(image) && point != centroidPoint)
			{
				switch (direction)
				{
					case UP:
						point.Y--;
						break;
					case DOWN:
						point.Y++;
						break;
					case RIGHT:
						point.X++;
						break;
					case LEFT:
						point.X--;
						break;
					case UP_LEFT:
						if (image.GetPixel(point.X-1,point.Y-1).ToArgb() == Color.ForestGreen.ToArgb())
						{
							point.X--;
							point.Y--;
						}
						else if (image.GetPixel(point.X - 1, point.Y).ToArgb() == Color.ForestGreen.ToArgb())
						{
							point.X--;
						}
						else
						{
							point.Y--;
						}
						break;
					case DOWN_LEFT:
						if (image.GetPixel(point.X - 1, point.Y + 1).ToArgb() == Color.ForestGreen.ToArgb())
						{
							point.X--;
							point.Y++;
						}
						else if (image.GetPixel(point.X - 1, point.Y).ToArgb() == Color.ForestGreen.ToArgb())
						{
							point.X--;
						}
						else
						{
							point.Y++;
						}
						break;
					case UP_RIGHT:
						if (image.GetPixel(point.X + 1, point.Y - 1).ToArgb() == Color.ForestGreen.ToArgb())
						{
							point.X++;
							point.Y--;
						}
						else if (image.GetPixel(point.X + 1, point.Y).ToArgb() == Color.ForestGreen.ToArgb())
						{
							point.X++;
						}
						else
						{
							point.Y--;
						}
						break;
					case DOWN_RIGHT:
						if (image.GetPixel(point.X + 1, point.Y + 1).ToArgb() == Color.ForestGreen.ToArgb())
						{
							point.X++;
							point.Y++;
						}
						else if (image.GetPixel(point.X + 1, point.Y).ToArgb() == Color.ForestGreen.ToArgb())
						{
							point.X++;
						}
						else
						{
							point.Y++;
						}
						break;
				}
			}

			return point;
		}
	}
}
