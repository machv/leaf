using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Leaf.ExtensionsBitmap;
using Leaf.ExtensionsPoint;

namespace Leaf.Logic
{
	public class ImageProcessing
	{
		private static object o = new object();

		public static int Huang(int[] data)
		{
			// Implements Huang's fuzzy thresholding method 
			// Uses Shannon's entropy function (one can also use Yager's entropy function) 
			// Huang L.-K. and Wang M.-J.J. (1995) "Image Thresholding by Minimizing  
			// the Measures of Fuzziness" Pattern Recognition, 28(1): 41-51
			// M. Emre Celebi  06.15.2007
			// Ported to ImageJ plugin by G. Landini from E Celebi's fourier_0.8 routines
			int threshold = -1;
			int ih, it;
			int first_bin;
			int last_bin;
			double sum_pix;
			double num_pix;
			double term;
			double ent;  // entropy 
			double min_ent; // min entropy 
			double mu_x;

			/* Determine the first non-zero bin */
			first_bin = 0;
			for (ih = 0; ih < 256; ih++)
			{
				if (data[ih] != 0)
				{
					first_bin = ih;
					break;
				}
			}

			/* Determine the last non-zero bin */
			last_bin = 255;
			for (ih = 255; ih >= first_bin; ih--)
			{
				if (data[ih] != 0)
				{
					last_bin = ih;
					break;
				}
			}
			term = 1.0 / (double)(last_bin - first_bin);
			double[] mu_0 = new double[256];
			sum_pix = num_pix = 0;
			for (ih = first_bin; ih < 256; ih++)
			{
				sum_pix += (double)ih * data[ih];
				num_pix += data[ih];
				/* NUM_PIX cannot be zero ! */
				mu_0[ih] = sum_pix / num_pix;
			}

			double[] mu_1 = new double[256];
			sum_pix = num_pix = 0;
			for (ih = last_bin; ih > 0; ih--)
			{
				sum_pix += (double)ih * data[ih];
				num_pix += data[ih];
				/* NUM_PIX cannot be zero ! */
				mu_1[ih - 1] = sum_pix / (double)num_pix;
			}

			/* Determine the threshold that minimizes the fuzzy entropy */
			threshold = -1;
			min_ent = Double.MaxValue;
			for (it = 0; it < 256; it++)
			{
				ent = 0.0;
				for (ih = 0; ih <= it; ih++)
				{
					/* Equation (4) in Ref. 1 */
					mu_x = 1.0 / (1.0 + term * Math.Abs(ih - mu_0[it]));
					if (!((mu_x < 1e-06) || (mu_x > 0.999999)))
					{
						/* Equation (6) & (8) in Ref. 1 */
						ent += data[ih] * (-mu_x * Math.Log(mu_x) - (1.0 - mu_x) * Math.Log(1.0 - mu_x));
					}
				}

				for (ih = it + 1; ih < 256; ih++)
				{
					/* Equation (4) in Ref. 1 */
					mu_x = 1.0 / (1.0 + term * Math.Abs(ih - mu_1[it]));
					if (!((mu_x < 1e-06) || (mu_x > 0.999999)))
					{
						/* Equation (6) & (8) in Ref. 1 */
						ent += data[ih] * (-mu_x * Math.Log(mu_x) - (1.0 - mu_x) * Math.Log(1.0 - mu_x));
					}
				}
				/* No need to divide by NUM_ROWS * NUM_COLS * LOG(2) ! */
				if (ent < min_ent)
				{
					min_ent = ent;
					threshold = it;
				}
			}
			return threshold;
		}

		public static Point Centroid(IList<Point> contourPoints)
		{
			var a = 0F;
			var cx = 0F;
			var cy = 0F;

			//contourPoints = OrderClockwise(contourPoints);

			var convex = new ConvexPath().ComputePath(contourPoints.Select(contourPoint => new MyPoint(contourPoint.X, contourPoint.Y)).ToList());
			//var convex = new ConvexPath().ComputePath(contourPoints.Select(contourPoint => new MyPoint(contourPoint.X, contourPoint.Y)).ToList());

			var first = convex[0];
			contourPoints.Clear();
			do
			{
				contourPoints.Add(new Point(first.Bod.SurX,first.Bod.SurY));
			    first = first.Descendant;
			}
			while (!first.Prvy);
			
			//centroid formula
			for (var i = 0; i < contourPoints.Count() - 1; i++)
			{
				var xi = contourPoints.ElementAt(i).X;
				var xii = contourPoints.ElementAt(i + 1).X;
				var yi = contourPoints.ElementAt(i).Y;
				var yii = contourPoints.ElementAt(i + 1).Y;

				a += xi * yii - xii * yi;
				cx += (xi + xii) * (xi * yii - xii * yi);
				cy += (yi + yii) * (xi * yii - xii * yi);
			}

			a /= 2;
			cx /= 6 * a;
			cy /= 6 * a;

			return new Point(Convert.ToInt32(cx), Convert.ToInt32(cy));
		}

		private static Point GetCentroid(Bitmap threshold)
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
			{ }
			return centroid;
		}

		private static Point IntersectionPoint(Bitmap image, Point point, Point centroidPoint, bool closest)
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

			lock (o)
			{
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
							if (image.GetPixel(point.X - 1, point.Y - 1).ToArgb() == Color.ForestGreen.ToArgb())
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
			}
			return point;
		}

		public static Image DrawCentroidLines(Bitmap image, Point centroid)
		{
			const int STEP = 20; //divide PI/2 into *step* pieces

			var g = Graphics.FromImage(image);

			var refPoints = new List<Point>();

			//round 1
#if true
			#region round1
			for (var i = 1; i <= STEP; i++)
			{

				var tan = Math.Tan(Math.PI / 4 - i * Math.PI / 4 / STEP + Math.PI / 4);

				{
					var x = tan != 0 ? Convert.ToInt32(centroid.Y / tan) : 0;
					if (x < (image.Width - centroid.X))
					{
						g.DrawLine(new Pen(Color.ForestGreen), centroid, new Point(x + centroid.X, 0));

						var p = IntersectionPoint(image, new Point(x + centroid.X, 0), centroid, false);
						refPoints.Add(p);

						g.DrawLine(new Pen(Color.White), centroid, new Point(x + centroid.X, 0));
						g.DrawLine(new Pen(Color.Blue), centroid, p);
					}
					else
					{
						var y = Convert.ToInt32(tan * (image.Width - centroid.X));
						g.DrawLine(new Pen(Color.ForestGreen), centroid, new Point(image.Width - 1, y));
						var p = IntersectionPoint(image, new Point(image.Width - 1, y), centroid, false);
						refPoints.Add(p);

						g.DrawLine(new Pen(Color.White), centroid, new Point(image.Width - 1, y));
						g.DrawLine(new Pen(Color.Blue), centroid, p);
					}
				}

			}
			#endregion
#endif

			//round 2
#if true
			#region round2
			for (var i = 1; i <= STEP; i++)
			{
				var tan = Math.Tan(Math.PI / 4 - i * Math.PI / 4 / STEP + 0 * Math.PI / 4);

				{
					var x = tan != 0 ? Convert.ToInt32(centroid.Y / tan) : 0;
					if (x !=0 && x < image.Width - centroid.X)
					{
						g.DrawLine(new Pen(Color.ForestGreen), centroid, new Point(x + centroid.X, 0));

						var p = IntersectionPoint(image, new Point(x + centroid.X, 0), centroid, false);
						refPoints.Add(p);

						g.DrawLine(new Pen(Color.White), centroid, new Point(x + centroid.X, 0));
						g.DrawLine(new Pen(Color.Blue), centroid, p);
					}
					else
					{
						var y = Convert.ToInt32(tan * (image.Width - centroid.X));

						if (tan == 0F)
							y = centroid.Y;

						g.DrawLine(new Pen(Color.ForestGreen), centroid, new Point(image.Width - 1, y));
						var p = IntersectionPoint(image, new Point(image.Width - 1, y), centroid, false);
						refPoints.Add(p);

						g.DrawLine(new Pen(Color.White), centroid, new Point(image.Width - 1, y));
						g.DrawLine(new Pen(Color.Blue), centroid, p);
					}
				}
			}
			#endregion 
#endif

			//round 3
#if true
			#region round3
			for (var i = 1; i <= STEP; i++)
			{
				var tan = Math.Tan(Math.PI / 4 - i * Math.PI / 4 / STEP + -1 * Math.PI / 4);

				{
					var x = tan != 0 ? Convert.ToInt32((centroid.Y - image.Height + 1) / tan) : 0;
					if (x < image.Width - centroid.X)
					{
						g.DrawLine(new Pen(Color.ForestGreen), centroid, new Point(x + centroid.X, image.Height - 1));

						var p = IntersectionPoint(image, new Point(x + centroid.X, image.Height - 1), centroid, false);
						refPoints.Add(p);

						g.DrawLine(new Pen(Color.White), centroid, new Point(x + centroid.X, image.Height - 1));
						g.DrawLine(new Pen(Color.Blue), centroid, p);
					}
					else
					{
						var y = centroid.Y - Convert.ToInt32(tan * (image.Width - centroid.X));
						g.DrawLine(new Pen(Color.ForestGreen), centroid, new Point(image.Width - 1, y));
						var p = IntersectionPoint(image, new Point(image.Width - 1, y), centroid, false);
						refPoints.Add(p);

						g.DrawLine(new Pen(Color.White), centroid, new Point(image.Width - 1, y));
						g.DrawLine(new Pen(Color.Blue), centroid, p);
					}
				}
			}
			#endregion
#endif

			//round 4
#if true
			#region round4
			for (var i = 1; i <= STEP; i++)
			{
				var tan = Math.Tan(Math.PI / 4 - i * Math.PI / 4 / STEP + -2 * Math.PI / 4);

				{
					var x = tan != 0 ? Convert.ToInt32((centroid.Y - image.Height + 1) / tan) : 0;
					if (x < image.Width - centroid.X)
					{
						g.DrawLine(new Pen(Color.ForestGreen), centroid, new Point(x + centroid.X, image.Height - 1));

						var p = IntersectionPoint(image, new Point(x + centroid.X, image.Height - 1), centroid, false);
						refPoints.Add(p);

						g.DrawLine(new Pen(Color.White), centroid, new Point(x + centroid.X, image.Height - 1));
						g.DrawLine(new Pen(Color.Blue), centroid, p);
					}
					else
					{
						var y = centroid.Y - Convert.ToInt32(tan * (image.Width - centroid.X));
						g.DrawLine(new Pen(Color.ForestGreen), centroid, new Point(image.Width - 1, y));
						var p = IntersectionPoint(image, new Point(image.Width - 1, y), centroid, false);
						refPoints.Add(p);

						g.DrawLine(new Pen(Color.White), centroid, new Point(image.Width - 1, y));
						g.DrawLine(new Pen(Color.Blue), centroid, p);
					}
				}
			}

			#endregion
#endif

			//round 5
#if true
			#region round5
			for (var i = 1; i <= STEP; i++)
			{
				var tan = Math.Tan(Math.PI / 4 - i * Math.PI / 4 / STEP + -3 * Math.PI / 4);

				{
					var x = tan != 0 ? Convert.ToInt32((centroid.Y - image.Height + 1) / tan) : 0;
					if (x + centroid.X > 0)
					{
						g.DrawLine(new Pen(Color.ForestGreen), centroid, new Point(centroid.X + x, image.Height - 1));

						var p = IntersectionPoint(image, new Point(centroid.X + x, image.Height - 1), centroid, false);
						refPoints.Add(p);

						g.DrawLine(new Pen(Color.White), centroid, new Point(x + centroid.X, image.Height - 1));
						g.DrawLine(new Pen(Color.Blue), centroid, p);
					}
					else
					{
						var y = centroid.Y - Convert.ToInt32(tan * (-centroid.X));
						g.DrawLine(new Pen(Color.ForestGreen), centroid, new Point(0, y));
						var p = IntersectionPoint(image, new Point(0, y), centroid, false);
						refPoints.Add(p);

						g.DrawLine(new Pen(Color.White), centroid, new Point(0, y));
						g.DrawLine(new Pen(Color.Blue), centroid, p);
					}
				}
			}
			#endregion
#endif

			//round 6
#if true
			#region round6
			for (var i = 1; i <= STEP; i++)
			{
				var tan = Math.Tan(Math.PI / 4 - i * Math.PI / 4 / STEP + -4 * Math.PI / 4);

				{
					int x;
					try
					{
						x = tan != 0 ? Convert.ToInt32((centroid.Y - image.Height + 1) / tan) : 0;
					}
					catch (Exception)
					{
						x = 0;
						tan = 0F;
					}

					if (x != 0 && x + centroid.X > 0)
					{
						g.DrawLine(new Pen(Color.ForestGreen), centroid, new Point(centroid.X + x, image.Height - 1));

						var p = IntersectionPoint(image, new Point(centroid.X + x, image.Height - 1), centroid, false);
						refPoints.Add(p);

						g.DrawLine(new Pen(Color.White), centroid, new Point(x + centroid.X, image.Height - 1));
						g.DrawLine(new Pen(Color.Blue), centroid, p);
					}
					else
					{
						var y = centroid.Y - Convert.ToInt32(tan * (-centroid.X));

						if (tan == 0F)
							y = centroid.Y;

						g.DrawLine(new Pen(Color.ForestGreen), centroid, new Point(0, y));
						var p = IntersectionPoint(image, new Point(0, y), centroid, false);
						refPoints.Add(p);

						g.DrawLine(new Pen(Color.White), centroid, new Point(0, y));
						g.DrawLine(new Pen(Color.Blue), centroid, p);
					}
				}
			}
			#endregion
#endif

			//round 7
#if true
			#region rouond7
			for (var i = 1; i <= STEP; i++)
			{
				var tan = Math.Tan(Math.PI / 4 - i * Math.PI / 4 / STEP + -5 * Math.PI / 4);

				{
					var x = tan != 0 ? Convert.ToInt32((centroid.Y) / tan) : 0;
					if (x + centroid.X > 0)
					{
						g.DrawLine(new Pen(Color.ForestGreen), centroid, new Point(centroid.X + x, 0));

						var p = IntersectionPoint(image, new Point(centroid.X + x, 0), centroid, false);
						refPoints.Add(p);

						g.DrawLine(new Pen(Color.White), centroid, new Point(x + centroid.X, 0));
						g.DrawLine(new Pen(Color.Blue), centroid, p);
					}
					else
					{
						var y = centroid.Y - Convert.ToInt32(tan * (-centroid.X));
						g.DrawLine(new Pen(Color.ForestGreen), centroid, new Point(0, y));
						var p = IntersectionPoint(image, new Point(0, y), centroid, false);
						refPoints.Add(p);

						g.DrawLine(new Pen(Color.White), centroid, new Point(0, y));
						g.DrawLine(new Pen(Color.Blue), centroid, p);
					}
				}
			}
			#endregion
#endif

			//round 8
#if true
			#region round8
			for (var i = 1; i <= STEP; i++)
			{
				var tan = Math.Tan(Math.PI / 4 - i * Math.PI / 4 / STEP + -6 * Math.PI / 4);

				{
					var x = tan != 0 ? Convert.ToInt32((centroid.Y) / tan) : 0;
					if (x + centroid.X > 0)
					{
						g.DrawLine(new Pen(Color.ForestGreen), centroid, new Point(centroid.X + x, 0));

						var p = IntersectionPoint(image, new Point(centroid.X + x, 0), centroid, false);
						refPoints.Add(p);

						g.DrawLine(new Pen(Color.White), centroid, new Point(x + centroid.X, 0));
						g.DrawLine(new Pen(Color.Blue), centroid, p);
					}
					else
					{
						var y = centroid.Y - Convert.ToInt32(tan * (-centroid.X));
						g.DrawLine(new Pen(Color.ForestGreen), centroid, new Point(0, y));
						var p = IntersectionPoint(image, new Point(0, y), centroid, false);
						refPoints.Add(p);

						g.DrawLine(new Pen(Color.White), centroid, new Point(0, y));
						g.DrawLine(new Pen(Color.Blue), centroid, p);
					}
				}
			}
			#endregion 
#endif

			return image;
		}

		public static IList<Point> GetReferncialPoints(Bitmap image, Point centroid)
		{
			const int STEP = 20;

			var t1 = Task.Factory.StartNew(() => GetReferencialPointsRound1(image, centroid, STEP));
			var t2 = Task.Factory.StartNew(() => GetReferencialPointsRound2(image, centroid, STEP));
			var t3 = Task.Factory.StartNew(() => GetReferencialPointsRound3(image, centroid, STEP));
			var t4 = Task.Factory.StartNew(() => GetReferencialPointsRound4(image, centroid, STEP));
			var t5 = Task.Factory.StartNew(() => GetReferencialPointsRound5(image, centroid, STEP));
			var t6 = Task.Factory.StartNew(() => GetReferencialPointsRound6(image, centroid, STEP));
			var t7 = Task.Factory.StartNew(() => GetReferencialPointsRound7(image, centroid, STEP));
			var t8 = Task.Factory.StartNew(() => GetReferencialPointsRound8(image, centroid, STEP));

			return t1.Result.Concat(t2.Result.Concat(t3.Result.Concat(t4.Result.Concat(t5.Result.Concat(t6.Result.Concat(t7.Result.Concat(t8.Result))))))).ToList();
		}

		private static IList<Point> GetReferencialPointsRound1(Bitmap image, Point centroid, int step)
		{
			Graphics g;
			lock (o)
				g = Graphics.FromImage(image);

			var refPoints = new List<Point>();

			for (var i = 1; i <= step; i++)
			{

				var tan = Math.Tan(Math.PI / 4 - i * Math.PI / 4 / step + Math.PI / 4);

				lock(o)
				{
					var x = tan != 0 ? Convert.ToInt32(centroid.Y / tan) : 0;
					if (x < (image.Width - centroid.X))
					{
						g.DrawLine(new Pen(Color.ForestGreen), centroid, new Point(x + centroid.X, 0));

						var p = IntersectionPoint(image, new Point(x + centroid.X, 0), centroid, false);
						refPoints.Add(p);

						g.DrawLine(new Pen(Color.White), centroid, new Point(x + centroid.X, 0));
					}
					else
					{
						var y = Convert.ToInt32(tan * (image.Width - centroid.X));

						g.DrawLine(new Pen(Color.ForestGreen), centroid, new Point(image.Width - 1, y));

						var p = IntersectionPoint(image, new Point(image.Width - 1, y), centroid, false);
						refPoints.Add(p);

						g.DrawLine(new Pen(Color.White), centroid, new Point(image.Width - 1, y));
					}
				}

			}

			return refPoints;
		}

		private static IList<Point> GetReferencialPointsRound2(Bitmap image, Point centroid, int step)
		{
			Graphics g;
			lock (o)
				g = Graphics.FromImage(image);

			var refPoints = new List<Point>();

			for (var i = 1; i <= step; i++)
			{
				var tan = Math.Tan(Math.PI / 4 - i * Math.PI / 4 / step + 0 * Math.PI / 4);

				lock(o)
				{
					var x = tan != 0 ? Convert.ToInt32(centroid.Y / tan) : 0;
					if (x != 0 && x < image.Width - centroid.X)
					{
						g.DrawLine(new Pen(Color.ForestGreen), centroid, new Point(x + centroid.X, 0));

						var p = IntersectionPoint(image, new Point(x + centroid.X, 0), centroid, false);
						refPoints.Add(p);

						g.DrawLine(new Pen(Color.White), centroid, new Point(x + centroid.X, 0));
					}
					else
					{
						var y = Convert.ToInt32(tan * (image.Width - centroid.X));

						if (tan == 0F)
							y = centroid.Y;

						g.DrawLine(new Pen(Color.ForestGreen), centroid, new Point(image.Width - 1, y));

						var p = IntersectionPoint(image, new Point(image.Width - 1, y), centroid, false);
						refPoints.Add(p);

						g.DrawLine(new Pen(Color.White), centroid, new Point(image.Width - 1, y));
					}
				}
			}

			return refPoints;
		}

		private static IList<Point> GetReferencialPointsRound3(Bitmap image, Point centroid, int step)
		{
			Graphics g;
			lock (o)
				g = Graphics.FromImage(image);
			var refPoints = new List<Point>();

			for (var i = 1; i <= step; i++)
			{
				var tan = Math.Tan(Math.PI / 4 - i * Math.PI / 4 / step + -1 * Math.PI / 4);

				lock(o)
				{
					var x = tan != 0 ? Convert.ToInt32((centroid.Y - image.Height + 1) / tan) : 0;
					if (x < image.Width - centroid.X)
					{
						g.DrawLine(new Pen(Color.ForestGreen), centroid, new Point(x + centroid.X, image.Height - 1));

						var p = IntersectionPoint(image, new Point(x + centroid.X, image.Height - 1), centroid, false);
						refPoints.Add(p);

						g.DrawLine(new Pen(Color.White), centroid, new Point(x + centroid.X, image.Height - 1));
					}
					else
					{
						var y = centroid.Y - Convert.ToInt32(tan * (image.Width - centroid.X));

						g.DrawLine(new Pen(Color.ForestGreen), centroid, new Point(image.Width - 1, y));

						var p = IntersectionPoint(image, new Point(image.Width - 1, y), centroid, false);
						refPoints.Add(p);

						g.DrawLine(new Pen(Color.White), centroid, new Point(image.Width - 1, y));
					}
				}
			}

			return refPoints;
		}

		private static IList<Point> GetReferencialPointsRound4(Bitmap image, Point centroid, int step)
		{
			Graphics g;
			lock (o)
				g = Graphics.FromImage(image);
			var refPoints = new List<Point>();

			for (var i = 1; i <= step; i++)
			{
				var tan = Math.Tan(Math.PI / 4 - i * Math.PI / 4 / step + -2 * Math.PI / 4);

				lock(o)
				{
					var x = tan != 0 ? Convert.ToInt32((centroid.Y - image.Height + 1) / tan) : 0;
					if (x < image.Width - centroid.X)
					{
						g.DrawLine(new Pen(Color.ForestGreen), centroid, new Point(x + centroid.X, image.Height - 1));

						var p = IntersectionPoint(image, new Point(x + centroid.X, image.Height - 1), centroid, false);
						refPoints.Add(p);

						g.DrawLine(new Pen(Color.White), centroid, new Point(x + centroid.X, image.Height - 1));
					}
					else
					{
						var y = centroid.Y - Convert.ToInt32(tan * (image.Width - centroid.X));

						g.DrawLine(new Pen(Color.ForestGreen), centroid, new Point(image.Width - 1, y));

						var p = IntersectionPoint(image, new Point(image.Width - 1, y), centroid, false);
						refPoints.Add(p);

						g.DrawLine(new Pen(Color.White), centroid, new Point(image.Width - 1, y));
					}
				}
			}

			return refPoints;
		}

		private static IList<Point> GetReferencialPointsRound5(Bitmap image, Point centroid, int step)
		{
			Graphics g;
			lock (o)
				g = Graphics.FromImage(image);
			var refPoints = new List<Point>();

			for (var i = 1; i <= step; i++)
			{
				var tan = Math.Tan(Math.PI / 4 - i * Math.PI / 4 / step + -3 * Math.PI / 4);

				lock(o)
				{
					var x = tan != 0 ? Convert.ToInt32((centroid.Y - image.Height + 1) / tan) : 0;
					if (x + centroid.X > 0)
					{

						g.DrawLine(new Pen(Color.ForestGreen), centroid, new Point(centroid.X + x, image.Height - 1));

						var p = IntersectionPoint(image, new Point(centroid.X + x, image.Height - 1), centroid, false);
						refPoints.Add(p);

						g.DrawLine(new Pen(Color.White), centroid, new Point(centroid.X + x, image.Height - 1));
					}
					else
					{
						var y = centroid.Y - Convert.ToInt32(tan * (-centroid.X));

						g.DrawLine(new Pen(Color.ForestGreen), centroid, new Point(0, y));

						var p = IntersectionPoint(image, new Point(0, y), centroid, false);
						refPoints.Add(p);

						g.DrawLine(new Pen(Color.White), centroid, new Point(0, y));
					}
				}
			}

			return refPoints;
		}

		private static IList<Point> GetReferencialPointsRound6(Bitmap image, Point centroid, int step)
		{
			Graphics g;
			lock (o)
				g = Graphics.FromImage(image);
			var refPoints = new List<Point>();

			for (var i = 1; i <= step; i++)
			{
				var tan = Math.Tan(Math.PI / 4 - i * Math.PI / 4 / step + -4 * Math.PI / 4);

				lock (o)
				{
					int x;
					try
					{
						x = tan != 0 ? Convert.ToInt32((centroid.Y - image.Height + 1) / tan) : 0;
					}
					catch (Exception)
					{
						x = 0;
						tan = 0F;
					}

					if (x != 0 && x + centroid.X > 0)
					{
						g.DrawLine(new Pen(Color.ForestGreen), centroid, new Point(centroid.X + x, image.Height - 1));

						var p = IntersectionPoint(image, new Point(centroid.X + x, image.Height - 1), centroid, false);
						refPoints.Add(p);

						g.DrawLine(new Pen(Color.White), centroid, new Point(centroid.X + x, image.Height - 1));
					}
					else
					{
						var y = centroid.Y - Convert.ToInt32(tan * (-centroid.X));

						if (tan == 0F)
							y = centroid.Y;

						g.DrawLine(new Pen(Color.ForestGreen), centroid, new Point(0, y));

						var p = IntersectionPoint(image, new Point(0, y), centroid, false);
						refPoints.Add(p);

						g.DrawLine(new Pen(Color.White), centroid, new Point(0, y));
					}
				}
			}

			return refPoints;
		}

		private static IList<Point> GetReferencialPointsRound7(Bitmap image, Point centroid, int step)
		{
			Graphics g;
			lock (o)
				g = Graphics.FromImage(image);

			var refPoints = new List<Point>();

			for (var i = 1; i <= step; i++)
			{
				var tan = Math.Tan(Math.PI / 4 - i * Math.PI / 4 / step + -5 * Math.PI / 4);

				lock(o)
				{
					var x = tan != 0 ? Convert.ToInt32((centroid.Y) / tan) : 0;
					if (x + centroid.X > 0)
					{
						g.DrawLine(new Pen(Color.ForestGreen), centroid, new Point(centroid.X + x, 0));

						var p = IntersectionPoint(image, new Point(centroid.X + x, 0), centroid, false);
						refPoints.Add(p);

						g.DrawLine(new Pen(Color.White), centroid, new Point(centroid.X + x, 0));
					}
					else
					{
						var y = centroid.Y - Convert.ToInt32(tan * (-centroid.X));

						g.DrawLine(new Pen(Color.ForestGreen), centroid, new Point(0, y));

						var p = IntersectionPoint(image, new Point(0, y), centroid, false);
						refPoints.Add(p);

						g.DrawLine(new Pen(Color.White), centroid, new Point(0, y));
					}
				}
			}

			return refPoints;
		}

		private static IList<Point> GetReferencialPointsRound8(Bitmap image, Point centroid, int step)
		{
			Graphics g;
			lock (o)
				g = Graphics.FromImage(image);

			var refPoints = new List<Point>();

			for (var i = 1; i <= step; i++)
			{
				var tan = Math.Tan(Math.PI / 4 - i * Math.PI / 4 / step + -6 * Math.PI / 4);

				lock(o)
				{
					var x = tan != 0 ? Convert.ToInt32((centroid.Y) / tan) : 0;
					if (x + centroid.X > 0)
					{
						g.DrawLine(new Pen(Color.ForestGreen), centroid, new Point(centroid.X + x, 0));

						var p = IntersectionPoint(image, new Point(centroid.X + x, 0), centroid, false);
						refPoints.Add(p);

						g.DrawLine(new Pen(Color.White), centroid, new Point(centroid.X + x, 0));
					}
					else
					{
						var y = centroid.Y - Convert.ToInt32(tan * (-centroid.X));

						g.DrawLine(new Pen(Color.ForestGreen), centroid, new Point(0, y));

						var p = IntersectionPoint(image, new Point(0, y), centroid, false);
						refPoints.Add(p);

						g.DrawLine(new Pen(Color.White), centroid, new Point(0, y));
					}
				}
			}

			return refPoints;
		}

		public static double[] GetDescriptor(IList<Point> referencialPoints, Point centroid)
		{
			var normalizationDistance = Distance(referencialPoints.Last(), centroid);

			var descriptor = new double[160];

			for (var i =0; i < referencialPoints.Count; i++)
			{
				descriptor[i] = normalizationDistance != 0
				                	? Distance(referencialPoints[i], centroid)/normalizationDistance
				                	: Distance(referencialPoints[i], centroid);
			}

			return descriptor;
		}

		public static double[] GetDescriptor(Bitmap image)
		{
			var contourPoints = ContourPoints(image, 0);

			var tmp = (Bitmap) image.Clone();

			var t1 = Task.Factory.StartNew(() => GetCentroid(tmp));
			var t2 = Task.Factory.StartNew(() => DrawContour(image, contourPoints));

			var centroid = t1.Result;
			image = t2.Result;

			return GetDescriptor(GetReferncialPoints(image, centroid), centroid);
		}

		private static Bitmap DrawContour(Bitmap threshold, IEnumerable<Point> contourPoints)
		{
			for (var i = 0; i < threshold.Width; i++)
				for (var j = 0; j < threshold.Height; j++)
					threshold.SetPixel(i, j, Color.White);

			foreach (var contourPoint in contourPoints)
			{
				try
				{
					threshold.SetPixel(contourPoint.X, contourPoint.Y, Color.Red);
				}
				catch
				{ }

			}

			return threshold;
		}

		private static double Distance(Point refPoint, Point centroid)
		{
			return Math.Sqrt((Math.Abs(refPoint.X - centroid.X) ^ 2) + (Math.Abs(refPoint.Y - centroid.Y) ^ 2));
		}

		private static IEnumerable<Point> ContourPoints(Bitmap image, int thresholdMove)
		{
			var tmpImage = image.SetGrayscale();

			var key = ImageProcessing.Huang(tmpImage.Histogram()) - thresholdMove;

			if (key > 255) key = 255;
			if (key < 0) key = 0;

			bool wasWhite = false;

			var contourPoints = new List<Point>();

			for (var j = 0; j < image.Height; j++)
				for (var i = 0; i < image.Width; i++)
				{
					if (image.GetPixel(i, j).R > key)
					{
						if (!wasWhite && i > 0)
							contourPoints.Add(new Point(i - 1, j));

						image.SetPixel(i, j, Color.White);
						wasWhite = true;
					}
					else
					{
						if (wasWhite)
						{
							contourPoints.Add(new Point(i, j));
							image.SetPixel(i, j, Color.Black);
						}
						else
						{
							image.SetPixel(i, j, Color.Black);

							if (j > 0 && image.GetPixel(i, j - 1) == Color.FromArgb(255, 255, 255))
								contourPoints.Add(new Point(i, j));

							if (j < image.Height - 1 && image.GetPixel(i, j + 1).R > key)
								contourPoints.Add(new Point(i, j));
						}
						wasWhite = false;

					}
				}

			return contourPoints;
		}
	}
}
