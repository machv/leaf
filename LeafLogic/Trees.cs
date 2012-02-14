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
            Tree[] trees;

            using (SqlConnection conn = new SqlConnection(Database.ConnectionString))
            {
                conn.Open();
                SqlCommand cmdCnt = new SqlCommand("SELECT COUNT(ID) FROM TREE ORDER BY RodoveCesky ASC", conn);
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
                    trees[i].RodoveCzech = (string)reader["RodoveCesky"];
                    trees[i].DruhoveCzech = (string)reader["DruhoveCesky"];
                    trees[i].RodoveLatin = (string)reader["RodoveLatinsky"];
                    trees[i].DruhoveLatin = (string)reader["DruhoveLatinsky"];
                }
            }

            return trees;
        }
    }
}
