using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OblikCleaner
{
    public partial class frmSetDB : Form
    {
        public frmSetDB()
        {
            InitializeComponent();
        }

        private void FrmSetDB_Load(object sender, EventArgs e)
        {
            tbSrvName.Text = Settings.DBSrvName;
            tbDBPath.Text = Settings.DBPath;
            tbDBUser.Text = Settings.DBUser;
            tbDBPasswd.Text = Settings.DBPasswd;
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
            Settings.DBPasswd = tbDBPasswd.Text;
            Settings.DBPath = tbDBPath.Text;
            Settings.DBSrvName = tbSrvName.Text;
            Settings.DBUser = tbDBUser.Text;
            this.Close();
        }
    }
}
