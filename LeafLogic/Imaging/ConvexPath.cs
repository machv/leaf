using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Windows.Forms;

namespace Leaf.Logic
{
	public class ConvexPath
	{
		private IList<MyPoint> zoznam;

		//rovnica priamky prechadzajuca dvoma bodmi ... POZOR ked su dva body presne nad sebou, potom sa tento vypocet zacykli :)
		private double[] Rovnica(MyPoint bod1, MyPoint bod2)
		{
			double tmp1 = (double)(bod1.SurY - bod2.SurY);
			double tmp2 = (double)(bod1.SurX - bod2.SurX);

			double a = (tmp1 / tmp2);
			double b = ((double)bod1.SurY - (double)bod1.SurX * (tmp1 / tmp2));

			double[] tmp = new double[2];
			tmp[0] = a;
			tmp[1] = b;

			return tmp;
		}

		//rozhodovanie ci treti bod lezi pod alebo nad priamkou tvernou dvoma bodmi
		private bool PodCiNad(MyPoint bod, MyPoint p1, MyPoint p2)
		{
			double[] tmp = this.Rovnica(p1, p2);
			double a = tmp[0];
			double b = tmp[1];

			return (bod.SurY < a * bod.SurX + b);
		}

		public List<Vektor> ComputePath(List<MyPoint> zoznam)
		{
			List<Vektor> obal = new List<Vektor>();

			this.zoznam = zoznam;

			List<MyPoint> zaloha = this.zoznam.ToList();

			try
			{
				MyPoint tmp = this.zoznam.ElementAt(0);

				obal.Add(new Vektor(this.zoznam.ElementAt(0), null, null));
				obal.ElementAt(0).Prvy = true;

				if (this.PodCiNad(this.zoznam.ElementAt(1), this.zoznam.ElementAt(0), this.zoznam.ElementAt(2)))
				{
					obal.Add(new Vektor(this.zoznam.ElementAt(1), obal.ElementAt(0), null));
					obal.Add(new Vektor(this.zoznam.ElementAt(2), obal.ElementAt(1), obal.ElementAt(0)));

					obal.ElementAt(0).Descendant = obal.ElementAt(1);
					obal.ElementAt(0).Ancestor = obal.ElementAt(2);
					obal.ElementAt(1).Descendant = obal.ElementAt(2);
				}
				else
				{
					obal.Add(new Vektor(this.zoznam.ElementAt(1), obal.ElementAt(0), null));
					obal.Add(new Vektor(this.zoznam.ElementAt(2), obal.ElementAt(0), obal.ElementAt(1)));

					obal.ElementAt(0).Descendant = obal.ElementAt(2);
					obal.ElementAt(0).Ancestor = obal.ElementAt(1);
					obal.ElementAt(1).Descendant = obal.ElementAt(0);

				}
				//obal.ElementAt(0).Descendant = obal.ElementAt(1);
				//obal.ElementAt(0).Ancestor = obal.ElementAt(2);
				//obal.ElementAt(1).Descendant = obal.ElementAt(2);

				//vyber prvych troch bodov
				this.zoznam.RemoveAt(0);
				this.zoznam.RemoveAt(0);
				this.zoznam.RemoveAt(0);


				//incrementalna metoda
				while (this.zoznam.Count > 0)
				{
					bool neprebehol = true;

					MyPoint p = this.zoznam.ElementAt(0);

					Vektor recentlyAdded = obal.ElementAt(obal.Count - 1);

					obal.Add(new Vektor(p, recentlyAdded, recentlyAdded));
					Vektor aktual = obal.ElementAt(obal.Count - 1);

					//cyklus v smere hodinovych ruciciek
					Vektor vSmere = aktual.Descendant;
					while ((!vSmere.Prvy) && (this.PodCiNad(vSmere.Bod, vSmere.Descendant.Bod, aktual.Bod)))
					{
						neprebehol = false;
						aktual.Descendant = vSmere.Descendant;
						vSmere.Descendant.Ancestor = aktual;
						vSmere = vSmere.Descendant;
					}
					if (neprebehol)
					{
						recentlyAdded.Ancestor.Descendant = aktual;
					}
					neprebehol = true;

					//cyklus proti smeru hodinovych ruciciek
					Vektor protiSmeru = aktual.Ancestor;
					while ((!protiSmeru.Prvy) && (!this.PodCiNad(protiSmeru.Bod, protiSmeru.Ancestor.Bod, aktual.Bod)))
					{
						neprebehol = false;
						aktual.Ancestor = protiSmeru.Ancestor;
						protiSmeru.Ancestor.Descendant = aktual;
						protiSmeru = protiSmeru.Ancestor;
					}

					if (neprebehol)
					{
						recentlyAdded.Descendant = aktual;
					}

					this.zoznam.RemoveAt(0);

				}

			}
			catch 
            {
                throw new InvalidOperationException("Need at least 3 points!");
                //MessageBox.Show("Need at least 3 points!"); 
            }
			finally { this.zoznam = zaloha.ToList(); }

			return obal;
		}
	}

	public class MyPoint : IComparable
	{
		//jeden bod je reprezentovany suradnicami x,y, pretazena metoda na porovnavanie, najskor sa porovnava suradnica x potom y

		private int suxX;
		private int suxY;

		public MyPoint(int x, int y)
		{
			this.suxX = x;
			this.suxY = y;
		}

		public int SurX
		{
			get
			{
				return this.suxX;
			}
			set
			{
				this.suxX = value;
			}
		}

		public int SurY
		{
			get
			{
				return this.suxY;
			}
			set
			{
				this.suxY = value;
			}
		}

		public override string ToString()
		{
			return suxX.ToString() + ":" + suxY.ToString();
		}

		#region IComparable Members

		public int CompareTo(object obj)
		{
			MyPoint tmp = (MyPoint)obj;

			if (this.suxX > tmp.suxX)
			{
				return 1;
			}
			else if (this.suxX == tmp.suxX)
			{
				if (this.suxY > tmp.suxY) { return 1; }
				else if (this.suxY == tmp.suxY) { return 0; }
				else { return -1; }
			}
			else
			{
				return -1;
			}
		}

		#endregion
	}

	public class Vektor
	{
		//struktura bodu konvexneho obalu, kde kazdy bod tvoriaci obal ma nasledovnika a predka

		private Vektor ancestor;
		private Vektor descendant;

		private MyPoint bod;
		private bool prvy = false;


		public Vektor(MyPoint bod, Vektor ans, Vektor des)
		{
			this.bod = bod;

			this.ancestor = ans;
			this.descendant = des;
		}

		public MyPoint Bod
		{
			get { return this.bod; }
			set { this.bod = value; }
		}

		public Vektor Ancestor
		{
			get { return this.ancestor; }
			set { this.ancestor = value; }
		}

		public Vektor Descendant
		{
			get { return this.descendant; }
			set { this.descendant = value; }
		}

		public bool Prvy
		{
			get { return this.prvy; }
			set { this.prvy = value; }
		}
	}
}
