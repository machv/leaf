using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Drawing;

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
    }
}