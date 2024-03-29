﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Leaf.ExtesionsColor;
using Leaf.ExtensionsBitmap;
using Leaf.ExtensionsPoint;
using Leaf.Logic;

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
							this.contourPoints.Add(new Point(i-1,j));

						image.SetPixel(i, j, Color.White);
						wasWhite = true;
					}
					else
					{
						if (wasWhite)
						{
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
			var quit = MessageBox.Show("Are you sure you want to quit the application?", "Quit", MessageBoxButtons.YesNo);
			if (quit == DialogResult.Yes) Application.Exit();
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
				}
				catch
				{}
				
			}

			this.LeafPicture.Image = image;
		}

		private void centroidButton_Click(object sender, EventArgs e)
		{
			Point centroid;
			var image = this.threshold;

			centroid = GetCentroid(image);

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

		private Point GetCentroid(Bitmap threshold)
		{
			var centroid = new Point();
			try
			{
				var list = new List<Point>();
				for (var i = 0; i < threshold.Width; i++)
					for (var j = 0; j < threshold.Height; j++)
					{
						if (threshold.GetPixel(i, j) == Color.FromArgb(0, 0, 0))
						{
							if ((i - 1 > 0 && i + 1 < threshold.Width && j - 1 > 0 && j + 1 < threshold.Height) &&
							    !(threshold.GetPixel(i, j + 1) == Color.FromArgb(0, 0, 0) &&
							      threshold.GetPixel(i, j - 1) == Color.FromArgb(0, 0, 0) &&
							      threshold.GetPixel(i + 1, j) == Color.FromArgb(0, 0, 0) &&
							      threshold.GetPixel(i - 1, j) == Color.FromArgb(0, 0, 0) &&
							      threshold.GetPixel(i + 1, j + 1) == Color.FromArgb(0, 0, 0) &&
							      threshold.GetPixel(i + 1, j - 1) == Color.FromArgb(0, 0, 0) &&
							      threshold.GetPixel(i - 1, j + 1) == Color.FromArgb(0, 0, 0) &&
							      threshold.GetPixel(i - 1, j - 1) == Color.FromArgb(0, 0, 0)))
								list.Add(new Point(i, j));
						}
					}

				centroid = ImageProcessing.Centroid(list);
			}
			catch
			{}
			return centroid;
		}

		private void centroidLinesButton_Click(object sender, EventArgs e)
		{
			var image = (Bitmap) this.LeafPicture.Image.Clone();

			this.LeafPicture.Image = ImageProcessing.DrawCentroidLines(image, centroid);
		}
		
        private void buttonHistogram_Click(object sender, EventArgs e)
        {
            var image = (Bitmap)this.LeafPicture.Image;
            var histogram = image.GetHueHistogram();

            histogramaDesenat1.DrawHistogram(histogram);
        }

        private void buttonEdgeDetection_Click(object sender, EventArgs e)
        {
            var ed = new EdgeDetection();
            var edged = ed.Detect(LeafPicture.Image);

            LeafPicture.Image = edged;
        }

		private void getListToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var sb = new StringBuilder();

			foreach (var referncialPoint in ImageProcessing.GetReferncialPoints((Bitmap)this.LeafPicture.Image.Clone(), this.centroid))
			{
				sb.Append(referncialPoint).Append('\n');
			}

			MessageBox.Show(sb.ToString());
		}

		private void drawLinesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var image = (Bitmap)this.LeafPicture.Image.Clone();
			var g = Graphics.FromImage(image);

			foreach (var referncialPoint in ImageProcessing.GetReferncialPoints((Bitmap)this.LeafPicture.Image.Clone(), this.centroid))
			{
				g.DrawLine(new Pen(Color.Blue), this.centroid, referncialPoint);
			}

			this.LeafPicture.Image = image;
		}

		private double[] descriptor;

		private void drawDescriptorHistorgramToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var image = (Bitmap) this.LeafPicture.Image.Clone();

			var time = DateTime.Now;
			var histogram = ImageProcessing.GetDescriptor(image);
			MessageBox.Show((DateTime.Now - time).ToString());

			this.histogramaDesenat1.DrawHistogram(histogram);
			//this.LeafPicture.Image = image;

			this.descriptor = histogram;
		}

		private void StoreTree(string czRodove, string czDruhove, string ltRodove, string ltDruhove)
		{
            Engine.AddTree(czRodove, czDruhove, ltRodove, ltDruhove);
		}

		private void StoreDescriptor(int treeID, double[] desc)
		{
            Engine.AddDescriptor(treeID, desc);
		}

		private void storeDescriptorToolStripMenuItem_Click(object sender, EventArgs e)
		{
			//TODO: form to add tree ID

			StoreDescriptor(1,this.descriptor);
		}

		private void distanceToALLToolStripMenuItem_Click(object sender, EventArgs e)
		{
			System.Threading.Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");

			var desc = this.descriptor;

			var conn =
				new SqlConnection(
					@"Data Source=leaf.sunstorm.info\SQLEXPRESS;Initial Catalog=Leaf;Persist Security Info=True;User ID=leaf;Password=leaf");

			var cmd = new SqlCommand()
			{
				Connection = conn
			};

			var sb = new StringBuilder(desc.Length);
			sb.Append(desc[0]);

			for (var i = 1; i < desc.Length; i++)
				sb.Append(':').Append(desc[i]);

			var descParam = new SqlParameter()
			{
				ParameterName = "descriptor",
				Value = sb.ToString()
			};

			var theshold = new SqlParameter()
			{
				ParameterName = "threshold",
				Value = threshold
			};

			cmd.Parameters.Add(descParam);
			//cmd.Parameters.Add(theshold);

			cmd.CommandText =
				//"SELECT T1.RodoveCesky + ' ' + T1.DruhoveCesky AS Cesky, T1.RodoveLatinsky + ' ' + T1.DruhoveLatinsky AS Latinsky, T2.Descriptor.Distance(CAST(@descriptor AS dbo.Descriptor)) AS Confidence FROM TREE AS T1 JOIN DESCRIPTOR AS T2 ON T1.ID = T2.TreeID WHERE dbo.IsClose(CAST(@descriptor AS dbo.Descriptor), T2.Descriptor, @Threshold) = 1;";
				"SELECT T1.RodoveCesky + ' ' + T1.DruhoveCesky AS Cesky, T1.RodoveLatinsky + ' ' + T1.DruhoveLatinsky AS Latinsky, T2.Descriptor.Distance(CAST(@descriptor AS dbo.Descriptor)) AS Confidence FROM TREE AS T1 JOIN DESCRIPTOR AS T2 ON T1.ID = T2.TreeID";
			conn.Open();
			var reader = cmd.ExecuteReader();
			
			var collection = new StringBuilder();
			while (reader.Read())
			{
				collection.Append((string)reader[0] + " " + (double)reader[2]).Append('\n');
			}

			conn.Close();
			MessageBox.Show(collection.ToString());
		}
	}
}
