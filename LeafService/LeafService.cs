using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Drawing;
using System.IO;
using Leaf.Logic;

namespace Leaf.Service
{
    public class LeafService : ILeafService
    {
        public Tree[] GetTrees()
        {
            Trees trees = new Trees();
            Tree[] n = trees.GetTrees();
 
            return n;
        }

        public int AddTree(string czRodove, string czDruhove, string ltRodove, string ltDruhove)
        {
            Trees trees = new Trees();
            return trees.AddTree(czRodove, czDruhove, ltRodove, ltDruhove);
        }

        public RecognizedLeaf[] Recognize(string picture, int noAnswers = 3)
        {
            // prijima obrazek jako string zakodovany v base64, ten se rozkoduje do Bitmapy, vypocita deskriptor, a hleda
            //http://stackoverflow.com/questions/6807283/transferring-byte-array-from-soap-service-to-android

            byte[] imageBytes = Convert.FromBase64String(picture);

            MemoryStream ms = new MemoryStream(imageBytes);
            Bitmap img = (Bitmap)Image.FromStream(ms);
            ms.Close();

            //TODO: na tenhle img zavolat rozpozavani

        	var descriptor = ImageProcessing.GetDescriptor(img);

            // odpovedi

            RecognizedLeaf[] leafs = new RecognizedLeaf[1];
            leafs[0] = new RecognizedLeaf("Javor mléč", "Acer platanoides", 0.4d);

        	return leafs;

        	//return this.Evaluate(descriptor);
        }

		private RecognizedLeaf[] Evaluate(double[] desc, double threshold = 2d)
		{
			System.Threading.Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");

			var conn =
				new SqlConnection(
					@"Data Source=leaf.sunstorm.info\SQLEXPRESS;Initial Catalog=Leaf;Persist Security Info=True;User ID=leaf;Password=leaf");

			var cmd = new SqlCommand()
			          	{
			          		Connection = conn
			          	};

			var sb = new StringBuilder(desc.Length);
			sb.Append(desc[0]);

			for (var i = 1; i < desc.Length; i++)
				sb.Append(':').Append(desc[i]);

			var descParam = new SqlParameter()
			                	{
			                		ParameterName = "descriptor",
			                		Value = sb.ToString()
			                	};

			var theshold = new SqlParameter()
			{
				ParameterName = "threshold",
				Value = threshold
			};

			cmd.Parameters.Add(descParam);
			cmd.Parameters.Add(theshold);

			cmd.CommandText =
				"SELECT T1.RodoveCesky + ' ' + T1.DruhoveCesky AS Cesky, T1.RodoveLatinsky + ' ' + T1.DruhoveLatinsky AS Latinsky, T2.Descriptor.Distance(CAST(@descriptor AS dbo.Descriptor)) AS Confidence FROM TREE AS T1 JOIN DESCRIPTOR AS T2 ON T1.ID = T2.TreeID WHERE dbo.IsClose(CAST(@descriptor AS dbo.Descriptor), T2.Descriptor, @Threshold) = 1;";

			conn.Open();
			var reader = cmd.ExecuteReader();
			conn.Close();

			var collection = new List<RecognizedLeaf>();
			while (reader.Read())
			{
				collection.Add(new RecognizedLeaf((string)reader[0], (string)reader[1], (double)reader[2]));
			}

			return collection.ToArray();
		}

    	public bool Learn(Descriptor[] descriptors, string picture, string treeName, SenderInfo senderInfo)
        {
            throw new NotImplementedException();
        }
    }
}
