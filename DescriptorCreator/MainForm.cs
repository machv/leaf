using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ExtensionsColor;
using ExtensionsBitmap;

namespace ExtensionsColor
{
	public partial class MainForm : Form
	{
		private Image revertPic;

		public MainForm()
		{
			InitializeComponent();

			foreach (var control in this.controlPanel.Controls)
			{
				((Control) control).Enabled = false;
			}
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

		private Bitmap NormalizeColorsBitmap(Bitmap image, int treshold)
		{
			var tmpImage = image.SetGrayscale();

			var key = ImageProcessing.Huang(tmpImage.Histogram());

			for (var i = 0; i < this.LeafPicture.Width; i++)
				for (var j = 0; j < this.LeafPicture.Height; j++)
					image.SetPixel(i, j, image.GetPixel(i, j).R > key ? Color.White : Color.Black);

			return image;
		}

		private void NormalizeColors_Click(object sender, EventArgs e)
		{
			this.LeafPicture.Image = this.NormalizeColorsBitmap((Bitmap)this.LeafPicture.Image, Int32.Parse(this.NormalizeTreshold.Value.ToString()));
		}

		private void LeafPicture_MouseMove(object sender, MouseEventArgs e)
		{
			if (this.LeafPicture.Image != null)
				try
				{
					//this.ArgbLabel.Text = ((Bitmap)this.LeafPicture.Image).GetPixel(e.X, e.Y).ToArgb().ToString();
					//this.ArgbLabel.Text += "\n";

					var pixel = ((Bitmap) this.LeafPicture.Image).GetPixel(e.X, e.Y);

					this.ArgbLabel.Text = e.X + ":" + e.Y + " " + pixel.R + ":" + pixel.G + ":" + pixel.B;
				}
				catch (Exception)
				{
					this.ArgbLabel.Text = "Out";
				}
		}

		private void RevertButton_Click(object sender, EventArgs e)
		{
			this.LeafPicture.Image = (Image) this.revertPic.Clone();
		}

		private void LeafPicture_Paint(object sender, PaintEventArgs e)
		{
			if (this.LeafPicture.Image != null)
				foreach (var control in this.controlPanel.Controls)
				{
					((Control) control).Enabled = true;
				}
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}
	}
}
