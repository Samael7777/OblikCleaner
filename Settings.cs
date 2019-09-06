using System.IO;
using System.Xml.Linq;

namespace OblikCleaner
{
    public static class Settings
    {
        private const string _settings_file = "settings.xml";
        private static XDocument _xSetFile;
        private static int _timeout;
        private static int _repeats;
        private static bool _savelogs;
        private static bool _stopservice;
        private static string _dbpath, _dbserv, _dbuser, _dbpass;

        public static string DBPath
        {
            get
            {
                _dbpath = (string)_xSetFile.Element("Settings").Element("DBPath").Attribute("Value");
                return _dbpath;
            }
            set
            {
                _dbserv = value;
                _xSetFile.Element("Settings").Element("DBPath").Attribute("Value").Value = _dbserv.ToString();
                _xSetFile.Save(_settings_file);
            }
        }
        public static string DBSrvName
        {
            get
            {
                _dbserv = (string)_xSetFile.Element("Settings").Element("DBServ").Attribute("Value");
                return _dbserv;
            }
            set
            {
                _dbserv = value;
                _xSetFile.Element("Settings").Element("DBServ").Attribute("Value").Value = _dbserv.ToString();
                _xSetFile.Save(_settings_file);
            }
        }
        public static string DBUser
        {
            get
            {
                _dbuser = (string)_xSetFile.Element("Settings").Element("DBUser").Attribute("Value");
                return _dbuser;
            }
            set
            {
                _dbuser = value;
                _xSetFile.Element("Settings").Element("DBUser").Attribute("Value").Value = _dbuser.ToString();
                _xSetFile.Save(_settings_file);
            }
        }
        public static string DBPasswd
        {
            get
            {
                _dbpass = (string)_xSetFile.Element("Settings").Element("DBPass").Attribute("Value");
                return _dbpass;
            }
            set
            {
                _dbpass = value;
                _xSetFile.Element("Settings").Element("DBPass").Attribute("Value").Value = _dbpass.ToString();
                _xSetFile.Save(_settings_file);
            }
        }
        public static bool SaveLogs
        {
            get
            {
                _savelogs = (bool)_xSetFile.Element("Settings").Element("SaveLogs").Attribute("Value");
                return _savelogs;
            }
            set
            {
                _savelogs = value;
                _xSetFile.Element("Settings").Element("SaveLogs").Attribute("Value").Value = _savelogs.ToString();
                _xSetFile.Save(_settings_file);
            }
        }
        public static bool StopService
        {
            get
            {
                _stopservice = (bool)_xSetFile.Element("Settings").Element("StopService").Attribute("Value");
                return _stopservice;
            }
            set
            {
                _stopservice = value;
                _xSetFile.Element("Settings").Element("StopService").Attribute("Value").Value = _stopservice.ToString();
                _xSetFile.Save(_settings_file);
            }
        }
        public static int timeout
        {
            get
            {
                _timeout = (int)_xSetFile.Element("Settings").Element("Timeout").Attribute("Value");
                return _timeout;
            }
            set
            {
                _timeout = value;
                _xSetFile.Element("Settings").Element("Timeout").Attribute("Value").Value = _timeout.ToString();
                _xSetFile.Save(_settings_file);
            }
        }
        public static int repeats
        {
            get
            {
                _repeats = (int)_xSetFile.Element("Settings").Element("Repeats").Attribute("Value");
                return _repeats;
            }
            set
            {
                _repeats = value;
                _xSetFile.Element("Settings").Element("Repeats").Attribute("Value").Value = _repeats.ToString();
                _xSetFile.Save(_settings_file);
            }
        }
        public static void GetSettings() //Constructor
        {
            _timeout = 500;
            _repeats = 2;
            _savelogs = true;
            _stopservice = true;
            _dbpath = "";
            _dbserv = "localhost";
            _dbuser = "SYSDBA";
            _dbpass = "masterkey";

            if (!File.Exists(_settings_file))
            {
                _xSetFile = new XDocument();
                _xSetFile.Add(new XElement("Settings"));
                _xSetFile.Element("Settings").Add(new XElement("Timeout"));
                _xSetFile.Element("Settings").Add(new XElement("Repeats"));
                _xSetFile.Element("Settings").Add(new XElement("SaveLogs"));
                _xSetFile.Element("Settings").Add(new XElement("StopService"));
                _xSetFile.Element("Settings").Add(new XElement("DBPath"));
                _xSetFile.Element("Settings").Add(new XElement("DBServ"));
                _xSetFile.Element("Settings").Add(new XElement("DBUser"));
                _xSetFile.Element("Settings").Add(new XElement("DBPass"));
                _xSetFile.Element("Settings").Element("Timeout").Add(new XAttribute("Value", _timeout.ToString()));
                _xSetFile.Element("Settings").Element("Repeats").Add(new XAttribute("Value", _repeats.ToString()));
                _xSetFile.Element("Settings").Element("SaveLogs").Add(new XAttribute("Value", _savelogs.ToString()));
                _xSetFile.Element("Settings").Element("StopService").Add(new XAttribute("Value", _stopservice.ToString()));
                _xSetFile.Element("Settings").Element("DBPath").Add(new XAttribute("Value", _dbpath.ToString()));
                _xSetFile.Element("Settings").Element("DBServ").Add(new XAttribute("Value", _dbserv.ToString()));
                _xSetFile.Element("Settings").Element("DBUser").Add(new XAttribute("Value", _dbuser.ToString()));
                _xSetFile.Element("Settings").Element("DBPass").Add(new XAttribute("Value", _dbpass.ToString()));
                _xSetFile.Save(_settings_file);
            }
            else
            {
                _xSetFile = XDocument.Load(_settings_file);
            }
        }
    }
}
