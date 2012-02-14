using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Leaf.Logic;

namespace Leaf.Service
{
	[ServiceContract]
	public interface ILeafService
	{
        [OperationContract]
        RecognizedLeaf[] Recognize(string picture, int noAnswers);

        [OperationContract]
        Tree[] GetTrees();

        [OperationContract]
        bool Learn(Descriptor[] descriptors, string picture, string treeName, SenderInfo senderInfo);
	}
}
