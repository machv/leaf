using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Drawing;
using System.IO;

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
            //http://stackoverflow.com/questions/6807283/transferring-byte-array-from-soap-service-to-android

            byte[] imageBytes = Convert.FromBase64String(picture);

            MemoryStream ms = new MemoryStream(imageBytes);
            Bitmap img = (Bitmap)Image.FromStream(ms);
            ms.Close();


            //na tenhle img zavolat rozpozavani

            return null;
        }

        public bool Learn(Descriptor[] descriptors, string picture, string treeName, SenderInfo senderInfo)
        {
            throw new NotImplementedException();
        }
    }
}
