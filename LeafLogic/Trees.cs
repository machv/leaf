using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace Leaf.Logic
{
    class Trees
    {
        public Tree[] GetTrees()
        {
            return null;

            using (SqlConnection conn = new SqlConnection(
                @"Data Source=leaf.sunstorm.info\SQLEXPRESS;Initial Catalog=Leaf;Persist Security Info=True;User ID=leaf;Password=leaf"))
            {

                var cmd = new SqlCommand()
                {
                    Connection = conn
                };
                /*
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
                                */
                // cmd.Parameters.Add(czD);
                // cmd.Parameters.Add(czR);
                // cmd.Parameters.Add(ltR);
                // cmd.Parameters.Add(ltD);

                cmd.CommandText =
                    "INSERT INTO TREE (RodoveCesky, DruhoveCesky, RodoveLatinsky, DruhoveLatinsky, Verified) VALUES (@czR, @czD, @ltR, @ltD, 1)";

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
    }
}
