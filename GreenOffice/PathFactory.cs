using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenOffice
{
    public class PathFactory
    {
        public string connString = @"C:\Users\Admin1\Desktop\GreenOffice\settings\connection.txt";
        static string pathString = @"C:\Users\Admin1\Desktop\GreenOffice\settings\";
        public string sourceString = pathString + "source.txt";
        public string portString = pathString + "port.txt";
        public string usernameString = pathString + "username.txt";
        public string passwordString = pathString + "password.txt";
        public string databaseString = pathString + "database.txt";
        public string userString = pathString + "user.txt";
    }
}
