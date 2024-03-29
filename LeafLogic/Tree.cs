﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Leaf.Logic
{
    public class Tree
    {
        public int ID { get; set; }
        public string RodoveCzech { get; set; }
        public string DruhoveCzech { get; set; }
        public string RodoveLatin { get; set; }
        public string DruhoveLatin { get; set; }
        public bool Verified { get; set; }
        public double Confidence { get; set; }

        public string NameCzech
        {
            get
            {
                return RodoveCzech + " " + DruhoveCzech;
            }
        }

        public string NameLatin
        {
            get
            {
                return RodoveLatin + " " + DruhoveLatin;
            }
        }
	}
}
