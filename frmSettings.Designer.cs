namespace OblikCleaner
{
    partial class frmSettings
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSettings));
            this.label1 = new System.Windows.Forms.Label();
            this.tbSrvName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbDBPath = new System.Windows.Forms.TextBox();
            this.btnDBBrowse = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.tbDBUser = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbDBPasswd = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnDef = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.numTimeout = new System.Windows.Forms.NumericUpDown();
            this.numRepeats = new System.Windows.Forms.NumericUpDown();
            this.chkService = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTimeout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRepeats)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Сервер БД:";
            // 
            // tbSrvName
            // 
            this.tbSrvName.Location = new System.Drawing.Point(103, 19);
            this.tbSrvName.Name = "tbSrvName";
            this.tbSrvName.Size = new System.Drawing.Size(133, 20);
            this.tbSrvName.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Путь к БД:";
            // 
            // tbDBPath
            // 
            this.tbDBPath.Location = new System.Drawing.Point(103, 51);
            this.tbDBPath.Name = "tbDBPath";
            this.tbDBPath.Size = new System.Drawing.Size(329, 20);
            this.tbDBPath.TabIndex = 3;
            // 
            // btnDBBrowse
            // 
            this.btnDBBrowse.Location = new System.Drawing.Point(438, 49);
            this.btnDBBrowse.Name = "btnDBBrowse";
            this.btnDBBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnDBBrowse.TabIndex = 4;
            this.btnDBBrowse.Text = "Обзор";
            this.btnDBBrowse.UseVisualStyleBackColor = true;
            this.btnDBBrowse.Click += new System.EventHandler(this.BtnDBBrowse_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 86);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Пользователь:";
            // 
            // tbDBUser
            // 
            this.tbDBUser.Location = new System.Drawing.Point(102, 83);
            this.tbDBUser.Name = "tbDBUser";
            this.tbDBUser.Size = new System.Drawing.Size(133, 20);
            this.tbDBUser.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 118);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(48, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Пароль:";
            // 
            // tbDBPasswd
            // 
            this.tbDBPasswd.Location = new System.Drawing.Point(102, 115);
            this.tbDBPasswd.Name = "tbDBPasswd";
            this.tbDBPasswd.Size = new System.Drawing.Size(133, 20);
            this.tbDBPasswd.TabIndex = 8;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tbSrvName);
            this.groupBox1.Controls.Add(this.tbDBPasswd);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.tbDBUser);
            this.groupBox1.Controls.Add(this.tbDBPath);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.btnDBBrowse);
            this.groupBox1.Location = new System.Drawing.Point(7, 128);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(525, 148);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Настройки связи с БД Облик";
            // 
            // btnDef
            // 
            this.btnDef.Location = new System.Drawing.Point(427, 282);
            this.btnDef.Name = "btnDef";
            this.btnDef.Size = new System.Drawing.Size(101, 23);
            this.btnDef.TabIndex = 11;
            this.btnDef.Text = "По умолчанию";
            this.btnDef.UseVisualStyleBackColor = true;
            this.btnDef.Click += new System.EventHandler(this.BtnDef_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(320, 282);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(101, 23);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(7, 282);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(101, 23);
            this.btnOk.TabIndex = 9;
            this.btnOk.Text = "Сохранить";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.BtnOk_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.numTimeout);
            this.groupBox2.Controls.Add(this.numRepeats);
            this.groupBox2.Controls.Add(this.chkService);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Location = new System.Drawing.Point(7, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(525, 110);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Настройки связи";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 26);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(73, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "Таймаут, мс:";
            // 
            // numTimeout
            // 
            this.numTimeout.Location = new System.Drawing.Point(102, 24);
            this.numTimeout.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numTimeout.Name = "numTimeout";
            this.numTimeout.Size = new System.Drawing.Size(80, 20);
            this.numTimeout.TabIndex = 16;
            this.numTimeout.Value = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            // 
            // numRepeats
            // 
            this.numRepeats.Location = new System.Drawing.Point(102, 49);
            this.numRepeats.Name = "numRepeats";
            this.numRepeats.Size = new System.Drawing.Size(80, 20);
            this.numRepeats.TabIndex = 18;
            this.numRepeats.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // chkService
            // 
            this.chkService.AutoSize = true;
            this.chkService.Checked = true;
            this.chkService.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkService.Location = new System.Drawing.Point(16, 75);
            this.chkService.Name = "chkService";
            this.chkService.Size = new System.Drawing.Size(191, 17);
            this.chkService.TabIndex = 19;
            this.chkService.Text = "Останавливать службы \"Облик\"";
            this.chkService.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(13, 51);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(55, 13);
            this.label6.TabIndex = 17;
            this.label6.Text = "Повторы:";
            // 
            // frmSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(540, 316);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnDef);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmSettings";
            this.Text = "Настройки";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmSettings_FormClosing);
            this.Load += new System.EventHandler(this.FrmSettings_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTimeout)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRepeats)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbSrvName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbDBPath;
        private System.Windows.Forms.Button btnDBBrowse;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbDBUser;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbDBPasswd;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnDef;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown numTimeout;
        private System.Windows.Forms.NumericUpDown numRepeats;
        private System.Windows.Forms.CheckBox chkService;
        private System.Windows.Forms.Label label6;
    }
}