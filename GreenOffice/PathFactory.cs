using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenOffice
{
    public class PathFactory
    {
        public string connString = @"C:\Users\Mikołaj Stanulewicz\Desktop\GreenOffice\settings\connection.txt"; //this is the combined string used to connect to DB
        static string pathString = @"C:\Users\Mikołaj Stanulewicz\Desktop\GreenOffice\settings\"; //this sets path to pathFactory files, it's just easier to change in needed
        public string sourceString = pathString + "source.txt"; //source so like: 127.0.0.1
        public string portString = pathString + "port.txt"; // port should look like this: 3306
        public string usernameString = pathString + "username.txt"; //username for the database: root
        public string passwordString = pathString + "password.txt"; //password, at default there isn't one
        public string databaseString = pathString + "database.txt"; //name of the database in this case: greenoffice
        public string userString = pathString + "user.txt"; //user is still saved in pathFactory it's a remnant, doesn't do anything
    }
}
