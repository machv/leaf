using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace Leaf.Logic
{
    public class Trees
    {
        public Tree[] GetTrees()
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
                }
            }

            return trees;
        }

        public int AddTree(string czRodove, string czDruhove, string ltRodove, string ltDruhove)
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
                    Value = czDruhove ?? String.Empty
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
                int id = (int)cmd.ExecuteScalar();
                conn.Close();
                
                return id;
            }
        }
    }
}
