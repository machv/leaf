using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Leaf.Logic
{
    public class Database
    {
        public static string ConnectionString
        {
            get
            {
                if (ConfigurationManager.ConnectionStrings["Leaf"] == null)
                {
                    throw new MissingFieldException("Missing definition of Leaf connection string.");
                }

                return ConfigurationManager.ConnectionStrings["Leaf"].ConnectionString;
            }
        }
    }
}
