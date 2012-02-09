using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace LeafService
{
	[ServiceContract]
	public interface ILeafService
	{
		[OperationContract]
		string SayHello();

        [OperationContract]
        RecognizedLeaf[] Recognize(string picture, int noAnswers);

        [OperationContract]
        bool Learn(Descriptor[] descriptors, string picture, string treeName, SenderInfo senderInfo);
	}
}
