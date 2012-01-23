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

        public RecognizedLeaf Recognize(string picture, int noAnswers = 3)
        {
			// prijima obrazek jako string zakodovany v base64, ten se rozkoduje do Bitmapy, vypocita deskriptor, a hleda
            throw new NotImplementedException();
        }

        public bool Learn(Descriptor[] descriptors, string picture, string treeName, SenderInfo senderInfo)
        {
            throw new NotImplementedException();
        }
    }
}
