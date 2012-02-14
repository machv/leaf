using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Leaf.Service
{
    [DataContractAttribute]
    public class RecognizedLeaf
    {
        [DataMemberAttribute]
        public string Name { get; set; }

        [DataMemberAttribute]
        public string LatinName { get; set; }

        [DataMemberAttribute]
        public double Confidency { get; set; }

        public RecognizedLeaf(string name, string latinName, double confidency)
        {
            Name = name;
            LatinName = latinName;
            Confidency = confidency;
        }
    }
}
