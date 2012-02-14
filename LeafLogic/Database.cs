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
                return ConfigurationManager.ConnectionStrings["Leaf"].ConnectionString;
            }
        }
    }
}
