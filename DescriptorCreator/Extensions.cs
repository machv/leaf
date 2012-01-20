using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Leaf.Base;

namespace ExtesionsColor
{
    public static class ExtensionsColor
    {
        public static bool AreSimilar(this Color source, Color actual, int treshold)
        {
            return Math.Abs(source.ToArgb() - actual.ToArgb()) < treshold ? true : false;
        }

        public static Hsv ToHsv(this Color color)
        {
            return Hsv.FromColor(color);
        }
    }

}

namespace ExtensionsBitmap
{
    public static class ExtensionsBitmap
    {
        public static int[] Histogram(this Bitmap image)
        {
            var array = new int[256];

            for (var i = 0; i < image.Width; i++)
                for (var j = 0; j < image.Height; j++)
                    array[image.GetPixel(i, j).R]++;

            return array;
        }

        public static Bitmap SetGrayscale(this Bitmap image)
        {
            Bitmap temp = (Bitmap)image;
            Bitmap bmap = (Bitmap)temp.Clone();
            Color c;
            for (int i = 0; i < bmap.Width; i++)
            {
                for (int j = 0; j < bmap.Height; j++)
                {
                    c = bmap.GetPixel(i, j);
                    byte gray = (byte)(.299 * c.R + .587 * c.G + .114 * c.B);

                    bmap.SetPixel(i, j, Color.FromArgb(gray, gray, gray));
                }
            }
            return (Bitmap)bmap.Clone();
        }

        public static Bitmap ResizePic(this Bitmap image, int longerSide)
        {
            var _currentBitmap = image;
            int newHeight;
            int newWidth;

            if (image.Height > image.Width)
            {
                newHeight = longerSide;
                newWidth = (int)(((float)((float)image.Width / (float)image.Height)) * (float)longerSide);
            }
            else
            {
                newWidth = longerSide;
                newHeight = (int)(((float)((float)image.Height / (float)image.Width)) * (float)longerSide);
            }

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

        public static long[] GetHueHistogram(this Bitmap image)
        {
            long[] hist = new long[361];

            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    Color c = image.GetPixel(i, j);
                    Hsv hsv = Hsv.FromColor(c);
                    hist[Convert.ToInt32(hsv.Hue)] += 1;
                }
            }

            return hist;
        }
    }
}

namespace ExtensionsPoint
{
	public static class ExtensionsPoint
	{
		public static double Distance(this Point source, Point destination)
		{
			return Math.Sqrt(Math.Abs(source.X - destination.X) ^ 2 + Math.Abs(source.Y - destination.Y) ^ 2);
		}

		public static bool RedAround(this Point source, Bitmap image)
		{
			var pixels = new List<Color>();

			try
			{
				pixels.Add(image.GetPixel(source.X,source.Y+1));
			}
			catch {}
			try
			{
				pixels.Add(image.GetPixel(source.X,source.Y-1));
			}
			catch {}
			try
			{
				pixels.Add(image.GetPixel(source.X+1,source.Y));
			}
			catch {}
			try
			{
				pixels.Add(image.GetPixel(source.X + 1, source.Y+1));
			}
			catch { }
			try
			{
				pixels.Add(image.GetPixel(source.X + 1, source.Y - 1));
			}
			catch { }
			try
			{
				pixels.Add(image.GetPixel(source.X - 1, source.Y));
			}
			catch { }
			try
			{
				pixels.Add(image.GetPixel(source.X - 1, source.Y+1));
			}
			catch { }
			try
			{
				pixels.Add(image.GetPixel(source.X - 1, source.Y-1));
			}
			catch { }

			return pixels.Where(p => (p.ToArgb() == Color.Red.ToArgb())).Count() > 0;
		}
	}
}
