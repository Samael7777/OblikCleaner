using System.IO;
using System.Xml.Linq;

namespace OblikCleaner
{
    public class Settings
    {
        private const string _settings_file = "settings.xml";
        private XDocument _xSetFile;
        private int _timeout;
        private int _repeats;
        private bool _savelogs;
        private bool _stopservice;

        public bool SaveLogs
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
        public bool StopService
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
        public int timeout
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
        public int repeats
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

        public Settings() //Constructor
        {
            _timeout = 500;
            _repeats = 2;
            _savelogs = true;
            _stopservice = true;
            if (!File.Exists(_settings_file))
            {
                _xSetFile = new XDocument();
                _xSetFile.Add(new XElement("Settings"));
                _xSetFile.Element("Settings").Add(new XElement("Timeout"));
                _xSetFile.Element("Settings").Add(new XElement("Repeats"));
                _xSetFile.Element("Settings").Add(new XElement("SaveLogs"));
                _xSetFile.Element("Settings").Add(new XElement("StopService"));
                _xSetFile.Element("Settings").Element("Timeout").Add(new XAttribute("Value", _timeout.ToString()));
                _xSetFile.Element("Settings").Element("Repeats").Add(new XAttribute("Value", _repeats.ToString()));
                _xSetFile.Element("Settings").Element("SaveLogs").Add(new XAttribute("Value", _savelogs.ToString()));
                _xSetFile.Element("Settings").Element("StopService").Add(new XAttribute("Value", _stopservice.ToString()));
                _xSetFile.Save(_settings_file);
            }
            else
            {
                _xSetFile = XDocument.Load(_settings_file);
            }
        }
    }
}
