using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace LeafService
{
    public class LeafService : ILeafService
    {
        public string SayHello()
        {
            return "Hello world!";
        }

        public RecognizedLeaf Recognize(Descriptor[] descriptors, int noAnswers = 3)
        {
            throw new NotImplementedException();
        }

        public bool Learn(Descriptor[] descriptors, byte picture, string treeName, SenderInfo senderInfo)
        {
            throw new NotImplementedException();
        }
    }
}
