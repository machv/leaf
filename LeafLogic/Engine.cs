using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Globalization;

namespace Leaf.Logic
{
    public class Engine
    {
        public static Tree[] GetTrees()
        {
            Tree[] trees; // = new Tree[3];

            using (SqlConnection conn = new SqlConnection(Database.ConnectionString))
            {
                conn.Open();
                SqlCommand cmdCnt = new SqlCommand("SELECT COUNT(ID) FROM TREE", conn);
                int rows = (int)cmdCnt.ExecuteScalar();

                trees = new Tree[rows];

                SqlCommand cmd = new SqlCommand("SELECT ID, Verified, RodoveCesky, DruhoveCesky, RodoveLatinsky, DruhoveLatinsky FROM TREE ORDER BY RodoveCesky ASC", conn);
                SqlDataReader reader = cmd.ExecuteReader();

                int i = 0;
                while (reader.Read())
                {
                    trees[i] = new Tree();
                    trees[i].ID = (int)reader["ID"];
                    trees[i].Verified = (bool)reader.GetBoolean(1);
                    trees[i].RodoveCzech = reader["RodoveCesky"] != DBNull.Value ? (string)reader["RodoveCesky"] : "";
                    trees[i].DruhoveCzech = reader["DruhoveCesky"] != DBNull.Value ? (string)reader["DruhoveCesky"] : "";
                    trees[i].RodoveLatin = reader["RodoveLatinsky"] != DBNull.Value ? (string)reader["RodoveLatinsky"] : "";
                    trees[i].DruhoveLatin = reader["DruhoveLatinsky"] != DBNull.Value ? (string)reader["DruhoveLatinsky"] : "";
                    
                    i++;
                }
            }

            return trees;
        }

        public static int AddTree(string czRodove, string czDruhove, string ltRodove, string ltDruhove)
        {
            using (SqlConnection conn = new SqlConnection(Database.ConnectionString))
            {
                var cmd = new SqlCommand()
                {
                    Connection = conn
                };

                var czR = new SqlParameter()
                {
                    ParameterName = "czR",
                    Value = czRodove ?? String.Empty
                };

                var czD = new SqlParameter()
                {
                    ParameterName = "czD",
                    Value = czDruhove ?? String.Empty
                };

                var ltR = new SqlParameter()
                {
                    ParameterName = "ltR",
                    Value = ltRodove ?? String.Empty
                };

                var ltD = new SqlParameter()
                {
                    ParameterName = "ltD",
                    Value = ltDruhove ?? String.Empty
                };

                cmd.Parameters.Add(czD);
                cmd.Parameters.Add(czR);
                cmd.Parameters.Add(ltR);
                cmd.Parameters.Add(ltD);

                cmd.CommandText = "INSERT INTO TREE (RodoveCesky, DruhoveCesky, RodoveLatinsky, DruhoveLatinsky, Verified) VALUES (@czR, @czD, @ltR, @ltD, 1); select scope_identity()";

                conn.Open();
                int id = (int)(decimal)cmd.ExecuteScalar();
                conn.Close();

                return id;
            }
        }

        public static int AddDescriptor(int treeID, double[] descriptor)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");

            using (SqlConnection conn = new SqlConnection(Database.ConnectionString))
            {
                var cmd = new SqlCommand()
                {
                    Connection = conn
                };

                var sb = new StringBuilder(descriptor.Length);
                sb.Append(descriptor[0]);

                for (var i = 1; i < descriptor.Length; i++)
                    sb.Append(':').Append(descriptor[i]);

                var descParam = new SqlParameter()
                {
                    ParameterName = "descriptor",
                    Value = sb.ToString()
                };

                var tree = new SqlParameter()
                {
                    ParameterName = "id",
                    Value = treeID
                };

                cmd.Parameters.Add(descParam);
                cmd.Parameters.Add(tree);

                cmd.CommandText =
                    "INSERT INTO DESCRIPTOR (TreeID, Descriptor) VALUES (@id, CAST(@descriptor AS dbo.Descriptor));  select scope_identity()";

                conn.Open();
                int id = (int)(decimal)cmd.ExecuteScalar();
                conn.Close();

                return id;
            }
        }

        public static Tree[] MatchDescriptor(double[] desc, int limitResults = 3, double threshold = 2d)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");

            var collection = new List<Tree>();
            using (SqlConnection conn = new SqlConnection(Database.ConnectionString))
            {
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

                string limit = limitResults > 0 ? "TOP " + limitResults.ToString() : "";

                cmd.CommandText =
                    "SELECT " + limit + " T1.ID as ID, T1.RodoveCesky AS RodoveCesky, T1.DruhoveCesky AS DruhoveCesky, T1.RodoveLatinsky AS RodoveLatinsky, T1.DruhoveLatinsky AS DruhoveLatinsky, T2.Descriptor.Distance(CAST(@descriptor AS dbo.Descriptor)) AS Confidence FROM TREE AS T1 JOIN DESCRIPTOR AS T2 ON T1.ID = T2.TreeID WHERE dbo.IsClose(CAST(@descriptor AS dbo.Descriptor), T2.Descriptor, @Threshold) = 1 ORDER BY Confidence ASC;";

                conn.Open();
                var reader = cmd.ExecuteReader();
                
                while (reader.Read())
                {
                    Tree tree = new Tree();
                    tree.Confidence = (double)reader["Confidence"];
                    tree.DruhoveCzech = (string)reader["DruhoveCesky"];
                    tree.DruhoveLatin = (string)reader["DruhoveLatinsky"];
                    tree.ID = (int)reader["ID"];
                    tree.RodoveCzech = (string)reader["RodoveCesky"];
                    tree.RodoveLatin = (string)reader["RodoveLatinsky"];
                    collection.Add(tree);
                }

                conn.Close();
            }

            return collection.ToArray();
        }
    }
}
