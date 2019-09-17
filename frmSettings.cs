using System;
using System.Windows.Forms;

namespace OblikCleaner
{
    public partial class frmSettings : Form
    {
        private int save = 2;
        public frmSettings()
        {
            InitializeComponent();
        }

        private void SaveSettings()
        {
            Settings.DBPasswd = tbDBPasswd.Text;
            Settings.DBPath = tbDBPath.Text;
            Settings.DBSrvName = tbSrvName.Text;
            Settings.DBUser = tbDBUser.Text;
            Settings.timeout = (int)numTimeout.Value;
            Settings.repeats = (int)numRepeats.Value;
            Settings.StopService = chkService.Checked;
        }
        private void FrmSettings_Load(object sender, EventArgs e)
        {
            tbSrvName.Text = Settings.DBSrvName;
            tbDBPath.Text = Settings.DBPath;
            tbDBUser.Text = Settings.DBUser;
            tbDBPasswd.Text = Settings.DBPasswd;
            numRepeats.Value = Settings.repeats;
            numTimeout.Value = Settings.timeout;
            chkService.Checked = Settings.StopService;
        }

        private void BtnDBBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Interbase DB files(*.gdb)|*.gdb|Firebird DB files (*.fdb)|*.fdb|All files(*.*)|*.*";
            ofd.CheckFileExists = true;
            ofd.CheckPathExists = true;
            ofd.Title = "Открыть базу данных Облик";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                tbDBPath.Text = ofd.FileName;
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            save = 0;
            this.Close();
        }

        private void BtnDef_Click(object sender, EventArgs e)
        {
            tbDBPasswd.Text = "masterkey";
            tbDBUser.Text = "SYSDBA";
            tbSrvName.Text = "localhost";
            tbDBPath.Text = "OASYS_DB.GDB";
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            save = 1;
            this.Close();
        }

        private void FrmSettings_FormClosing(object sender, FormClosingEventArgs e)
        {
            switch (save)
            {
                case 0:
                    break;
                case 1:
                    SaveSettings();
                    break;
                default:
                    DialogResult res = MessageBox.Show("Сохранить настройки?", "Настройки", MessageBoxButtons.YesNo);
                    if (res == DialogResult.Yes)
                    {
                        SaveSettings();
                    }
                    break;
            }
        }
    }
}
