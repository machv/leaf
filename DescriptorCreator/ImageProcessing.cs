using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DescriptorCreator
{
	public class ImageProcessing
	{
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
	}
}
