using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DescriptorCreator
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
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
				image = this.ResizePic(400, 400, image);

				this.LeafPicture.Image = image;
			}
		}

		public Bitmap ResizePic(int newWidth, int newHeight, Bitmap image)
		{
			var _currentBitmap = image;
			if (newWidth != 0 && newHeight != 0)
			{
				Bitmap temp = (Bitmap)_currentBitmap;
				Bitmap bmap = new Bitmap(newWidth, newHeight, temp.PixelFormat);

				double nWidthFactor = (double)temp.Width / (double)newWidth;
				double nHeightFactor = (double)temp.Height / (double)newHeight;

				double fx, fy, nx, ny;
				int cx, cy, fr_x, fr_y;
				Color color1 = new Color();
				Color color2 = new Color();
				Color color3 = new Color();
				Color color4 = new Color();
				byte nRed, nGreen, nBlue;

				byte bp1, bp2;

				for (int x = 0; x < bmap.Width; ++x)
				{
					for (int y = 0; y < bmap.Height; ++y)
					{

						fr_x = (int)Math.Floor(x * nWidthFactor);
						fr_y = (int)Math.Floor(y * nHeightFactor);
						cx = fr_x + 1;
						if (cx >= temp.Width) cx = fr_x;
						cy = fr_y + 1;
						if (cy >= temp.Height) cy = fr_y;
						fx = x * nWidthFactor - fr_x;
						fy = y * nHeightFactor - fr_y;
						nx = 1.0 - fx;
						ny = 1.0 - fy;

						color1 = temp.GetPixel(fr_x, fr_y);
						color2 = temp.GetPixel(cx, fr_y);
						color3 = temp.GetPixel(fr_x, cy);
						color4 = temp.GetPixel(cx, cy);

						// Blue
						bp1 = (byte)(nx * color1.B + fx * color2.B);

						bp2 = (byte)(nx * color3.B + fx * color4.B);

						nBlue = (byte)(ny * (double)(bp1) + fy * (double)(bp2));

						// Green
						bp1 = (byte)(nx * color1.G + fx * color2.G);

						bp2 = (byte)(nx * color3.G + fx * color4.G);

						nGreen = (byte)(ny * (double)(bp1) + fy * (double)(bp2));

						// Red
						bp1 = (byte)(nx * color1.R + fx * color2.R);

						bp2 = (byte)(nx * color3.R + fx * color4.R);

						nRed = (byte)(ny * (double)(bp1) + fy * (double)(bp2));

						bmap.SetPixel(x, y, System.Drawing.Color.FromArgb
				(255, nRed, nGreen, nBlue));
					}
				}
				_currentBitmap = (Bitmap)bmap.Clone();
			}

			return _currentBitmap;
		}
	}
}
