using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace DescriptorCreator
{
    public struct Hsv
    {
        private double _hue;
        private double _sat;
        private double _val;

        public double Hue
        {
            get { return _hue; }
            set { _hue = value % 360; }
        }

        public double Saturation
        {
            get { return _sat; }
            set
            {
                if (value >= 0 && value <= 1)
                    _sat = value;
            }
        }

        public double Value
        {
            get { return _val; }
            set
            {
                if (value >= 0 && value <= 1)
                    _val = value;
            }
        }

        public static Hsv FromColor(Color color)
        {
            int max = Math.Max(color.R, Math.Max(color.G, color.B));
            int min = Math.Min(color.R, Math.Min(color.G, color.B));
            Hsv hsv = new Hsv();
            hsv.Hue = color.GetHue();
            hsv.Saturation = (max == 0) ? 0 : 1d - (1d * min / max);
            hsv.Value = max / 255d;
            return hsv;
        }

        public Color ToColor()
        {
            return Hsv.ToColor(this);
        }

        public static Color ToColor(Hsv color)
        {
            Color ret = Color.Transparent;
            double f = color.Hue / 60 - Math.Floor(color.Hue / 60);
            double value = color.Value * 255;
            int v = Convert.ToInt32(value);
            int p = Convert.ToInt32(value * (1 - color.Saturation));
            int q = Convert.ToInt32(value * (1 - f * color.Saturation));
            int t = Convert.ToInt32(value * (1 - (1 - f) * color.Saturation));

            switch (Convert.ToInt32(Math.Floor(color.Hue / 60)) % 6)
            {
                default:
                    ret = Color.FromArgb(255, v, p, q);
                    break;
                case 0:
                    ret = Color.FromArgb(255, v, t, p);
                    break;
                case 1:
                    ret = Color.FromArgb(255, q, v, p);
                    break;
                case 2:
                    ret = Color.FromArgb(255, p, v, t);
                    break;
                case 3:
                    ret = Color.FromArgb(255, p, q, v);
                    break;
                case 4:
                    ret = Color.FromArgb(255, t, p, v);
                    break;
            }
            return ret;
        }
    }
}
