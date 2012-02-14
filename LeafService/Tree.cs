using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Leaf.Service
{
    public class Tree
    {
        public int ID { get; set; }
        public string RodoveCzech { get; set; }
        public string DruhoveCzech { get; set; }
        public string RodoveLatin { get; set; }
        public string DruhoveLatin { get; set; }
        public bool Verified { get; set; }
    }
}
