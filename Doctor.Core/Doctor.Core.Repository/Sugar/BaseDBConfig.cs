using System;
using System.Collections.Generic;
using System.Text;

namespace Doctor.Core.Repository
{
    public class BaseDBConfig
    {
        public static string ConnectionString { get { return "Data Source=localhost;Initial Catalog=DoctorCoreDB;Integrated Security=True";  } set { } }
    }
}
