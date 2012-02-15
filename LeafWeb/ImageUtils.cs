using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Drawing;
using Leaf.ExtensionsPoint;
using Leaf.ExtensionsBitmap;
using Leaf.ExtesionsColor;
using Leaf.Logic;

namespace Leaf.Web
{
    public class ImageUtils
    {
        public static System.Drawing.Bitmap BitmapFromBytes(byte[] b)
        {
            MemoryStream ms = new MemoryStream(b);
            Bitmap img = (Bitmap)System.Drawing.Image.FromStream(ms);
            ms.Close();

            return img;
        }

        public static byte[] ImageToBytes(System.Drawing.Image img)
        {
            MemoryStream stream = new MemoryStream();
            img.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
            stream.Position = 0;
            byte[] data = new byte[stream.Length];
            stream.Read(data, 0, (int)stream.Length);

            return data;
        }

        public static System.Drawing.Image Resize(System.Drawing.Image img, int NewWidth, int MaxHeight, bool OnlyResizeIfWider)
        {
            // Prevent using images internal thumbnail
            img.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipNone);
            img.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipNone);

            if (OnlyResizeIfWider)
            {
                if (img.Width <= NewWidth)
                {
                    NewWidth = img.Width;
                }
            }

            int NewHeight = img.Height * NewWidth / img.Width;
            if (NewHeight > MaxHeight)
            {
                // Resize with height instead
                NewWidth = img.Width * MaxHeight / img.Height;
                NewHeight = MaxHeight;
            }

            System.Drawing.Image NewImage = img.GetThumbnailImage(NewWidth, NewHeight, null, IntPtr.Zero);

            // Clear handle to original file so that we can overwrite it if necessary
            img.Dispose();

            return NewImage;
        }

        public static Bitmap GenerateDescriptorPreview(Bitmap image)
        {
            DescriptorPreview descriptor = new DescriptorPreview(image);
            return descriptor.GeneratePreview();
        }

        public class DescriptorPreview
        {
            private List<Point> ContourPoints;
            private Bitmap image;
            protected Bitmap threshold;
            protected Point centroid;

            public DescriptorPreview(Bitmap Image)
            {
                image = Image;
            }

            public Bitmap GeneratePreview()
            {
                Normalize(0);
                Countour();
                Centroid();
                //Lines();

                return image;
            }

            protected void Lines()
            {
                var image2 = (Bitmap)image.Clone();

                image = (Bitmap)ImageProcessing.DrawCentroidLines(image2, centroid);
            }

            protected Point GetCentroid()
            {
                var centroid = new Point();
                var list = new List<Point>();
                
                try
                {
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
                catch (Exception e)
                { }
                return centroid;
            }

            public void Normalize(int thresholdMove = 0)
            {
                var tmpImage = image.SetGrayscale();

                var key = ImageProcessing.Huang(tmpImage.Histogram()) - thresholdMove;

                if (key > 255) key = 255;
                if (key < 0) key = 0;

                bool wasWhite = false;

                ContourPoints = new List<Point>();

                for (var j = 0; j < image.Height; j++)
                    for (var i = 0; i < image.Width; i++)
                    {
                        if (image.GetPixel(i, j).R > key)
                        {
                            if (!wasWhite && i > 0)
                                ContourPoints.Add(new Point(i - 1, j));

                            image.SetPixel(i, j, Color.White);
                            wasWhite = true;
                        }
                        else
                        {
                            if (wasWhite)
                            {
                                ContourPoints.Add(new Point(i, j));
                                image.SetPixel(i, j, Color.Black);
                            }
                            else
                            {
                                image.SetPixel(i, j, Color.Black);

                                if (j > 0 && image.GetPixel(i, j - 1) == Color.FromArgb(255, 255, 255))
                                    ContourPoints.Add(new Point(i, j));

                                if (j < image.Height - 1 && image.GetPixel(i, j + 1).R > key)
                                    ContourPoints.Add(new Point(i, j));
                            }
                            wasWhite = false;
                        }
                    }
                threshold = (Bitmap)image.Clone();
            }

            public void Countour()
            {
                for (var i = 0; i < image.Width; i++)
                    for (var j = 0; j < image.Height; j++)
                        image.SetPixel(i, j, Color.White);

                foreach (var contourPoint in ContourPoints)
                {
                    try
                    {
                        image.SetPixel(contourPoint.X, contourPoint.Y, Color.Red);
                    }
                    catch
                    { }
                }
            }

            protected void Centroid()
            {
                //Point centroid;
                //var image = threshold;
                Point centroid = GetCentroid();

                var cx = centroid.X >= 0 ? centroid.X : 0;
                var cy = centroid.Y >= 0 ? centroid.Y : 0;

                //image = (Bitmap)this.LeafPicture.Image.Clone();

                image.SetPixel(cx, cy, Color.Black);
                image.SetPixel(cx + 1, cy, Color.Black);
                image.SetPixel(cx, cy + 1, Color.Black);
                image.SetPixel(cx + 1, cy + 1, Color.Black);

                //this.LeafPicture.Image = (Image)image.Clone();
            }

        }
    }
}