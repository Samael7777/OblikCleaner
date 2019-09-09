namespace OblikCleaner
{
    partial class frmMain
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.dgCounters = new System.Windows.Forms.DataGridView();
            this.cmCounters = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmGetData = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.cmSelAll = new System.Windows.Forms.ToolStripMenuItem();
            this.cmSelNone = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.cmAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.cmDel = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.numTimeout = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.numRepeats = new System.Windows.Forms.NumericUpDown();
            this.btnDel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnGetdata = new System.Windows.Forms.Button();
            this.btnDelDG = new System.Windows.Forms.Button();
            this.log = new System.Windows.Forms.ListBox();
            this.chkService = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnGetDB = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnSvcStart = new System.Windows.Forms.Button();
            this.btnSvcStop = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnSetBD = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.btnCleanLog = new System.Windows.Forms.Button();
            this.chkSaveLogs = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgCounters)).BeginInit();
            this.cmCounters.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTimeout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRepeats)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgCounters
            // 
            this.dgCounters.AllowUserToAddRows = false;
            this.dgCounters.AllowUserToDeleteRows = false;
            this.dgCounters.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgCounters.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Sunken;
            this.dgCounters.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgCounters.ContextMenuStrip = this.cmCounters;
            this.dgCounters.Location = new System.Drawing.Point(18, 11);
            this.dgCounters.Name = "dgCounters";
            this.dgCounters.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgCounters.Size = new System.Drawing.Size(611, 339);
            this.dgCounters.TabIndex = 0;
            this.dgCounters.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgCounters_CellValueChanged);
            // 
            // cmCounters
            // 
            this.cmCounters.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmGetData,
            this.toolStripSeparator2,
            this.cmSelAll,
            this.cmSelNone,
            this.toolStripSeparator1,
            this.cmAdd,
            this.cmDel});
            this.cmCounters.Name = "cmCounters";
            this.cmCounters.Size = new System.Drawing.Size(188, 126);
            // 
            // cmGetData
            // 
            this.cmGetData.Name = "cmGetData";
            this.cmGetData.Size = new System.Drawing.Size(187, 22);
            this.cmGetData.Text = "Получить данные";
            this.cmGetData.Click += new System.EventHandler(this.CmGetData_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(184, 6);
            // 
            // cmSelAll
            // 
            this.cmSelAll.Name = "cmSelAll";
            this.cmSelAll.Size = new System.Drawing.Size(187, 22);
            this.cmSelAll.Text = "Выделить все";
            this.cmSelAll.Click += new System.EventHandler(this.CmSelAll_Click);
            // 
            // cmSelNone
            // 
            this.cmSelNone.Name = "cmSelNone";
            this.cmSelNone.Size = new System.Drawing.Size(187, 22);
            this.cmSelNone.Text = "Снять выделение";
            this.cmSelNone.Click += new System.EventHandler(this.CmSelNone_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(184, 6);
            // 
            // cmAdd
            // 
            this.cmAdd.Name = "cmAdd";
            this.cmAdd.Size = new System.Drawing.Size(187, 22);
            this.cmAdd.Text = "Добавить счетчик";
            this.cmAdd.Click += new System.EventHandler(this.CmAdd_Click);
            // 
            // cmDel
            // 
            this.cmDel.Name = "cmDel";
            this.cmDel.Size = new System.Drawing.Size(187, 22);
            this.cmDel.Text = "Удалить счетчик(и)";
            this.cmDel.Click += new System.EventHandler(this.CmDel_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Таймаут, мс:";
            // 
            // numTimeout
            // 
            this.numTimeout.Location = new System.Drawing.Point(85, 22);
            this.numTimeout.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numTimeout.Name = "numTimeout";
            this.numTimeout.Size = new System.Drawing.Size(80, 20);
            this.numTimeout.TabIndex = 2;
            this.numTimeout.Value = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.numTimeout.ValueChanged += new System.EventHandler(this.NumTimeout_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Повторы:";
            // 
            // numRepeats
            // 
            this.numRepeats.Location = new System.Drawing.Point(85, 51);
            this.numRepeats.Name = "numRepeats";
            this.numRepeats.Size = new System.Drawing.Size(80, 20);
            this.numRepeats.TabIndex = 4;
            this.numRepeats.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numRepeats.ValueChanged += new System.EventHandler(this.NumRepeats_ValueChanged);
            // 
            // btnDel
            // 
            this.btnDel.Location = new System.Drawing.Point(6, 47);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(125, 23);
            this.btnDel.TabIndex = 5;
            this.btnDel.Text = "Удалить выбранное";
            this.btnDel.UseVisualStyleBackColor = true;
            this.btnDel.Click += new System.EventHandler(this.BtnDel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(6, 103);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(125, 23);
            this.btnSave.TabIndex = 9;
            this.btnSave.Text = "Сохранить список";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(6, 19);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(125, 23);
            this.btnAdd.TabIndex = 10;
            this.btnAdd.Text = "Добавить счетчик";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.BtnAdd_Click);
            // 
            // btnGetdata
            // 
            this.btnGetdata.Location = new System.Drawing.Point(6, 19);
            this.btnGetdata.Name = "btnGetdata";
            this.btnGetdata.Size = new System.Drawing.Size(125, 23);
            this.btnGetdata.TabIndex = 11;
            this.btnGetdata.Text = "Получить данные";
            this.btnGetdata.UseVisualStyleBackColor = true;
            this.btnGetdata.Click += new System.EventHandler(this.BtnGetdata_Click);
            // 
            // btnDelDG
            // 
            this.btnDelDG.Location = new System.Drawing.Point(6, 48);
            this.btnDelDG.Name = "btnDelDG";
            this.btnDelDG.Size = new System.Drawing.Size(125, 23);
            this.btnDelDG.TabIndex = 12;
            this.btnDelDG.Text = "Очистить СГ";
            this.btnDelDG.UseVisualStyleBackColor = true;
            this.btnDelDG.Click += new System.EventHandler(this.BtnDelDG_Click);
            // 
            // log
            // 
            this.log.FormattingEnabled = true;
            this.log.Location = new System.Drawing.Point(6, 19);
            this.log.Name = "log";
            this.log.Size = new System.Drawing.Size(605, 95);
            this.log.TabIndex = 13;
            // 
            // chkService
            // 
            this.chkService.AutoSize = true;
            this.chkService.Checked = true;
            this.chkService.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkService.Location = new System.Drawing.Point(9, 81);
            this.chkService.Name = "chkService";
            this.chkService.Size = new System.Drawing.Size(191, 17);
            this.chkService.TabIndex = 14;
            this.chkService.Text = "Останавливать службы \"Облик\"";
            this.chkService.UseVisualStyleBackColor = true;
            this.chkService.CheckedChanged += new System.EventHandler(this.ChkService_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnGetDB);
            this.groupBox1.Controls.Add(this.btnAdd);
            this.groupBox1.Controls.Add(this.btnSave);
            this.groupBox1.Controls.Add(this.btnDel);
            this.groupBox1.Location = new System.Drawing.Point(12, 356);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(138, 137);
            this.groupBox1.TabIndex = 15;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Управление списком";
            // 
            // btnGetDB
            // 
            this.btnGetDB.Location = new System.Drawing.Point(6, 75);
            this.btnGetDB.Name = "btnGetDB";
            this.btnGetDB.Size = new System.Drawing.Size(125, 23);
            this.btnGetDB.TabIndex = 19;
            this.btnGetDB.Text = "Получить из БД";
            this.btnGetDB.UseVisualStyleBackColor = true;
            this.btnGetDB.Click += new System.EventHandler(this.BtnGetDB_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnSvcStart);
            this.groupBox2.Controls.Add(this.btnSvcStop);
            this.groupBox2.Controls.Add(this.btnGetdata);
            this.groupBox2.Controls.Add(this.btnDelDG);
            this.groupBox2.Location = new System.Drawing.Point(156, 356);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(261, 137);
            this.groupBox2.TabIndex = 16;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Управление опросом";
            // 
            // btnSvcStart
            // 
            this.btnSvcStart.Location = new System.Drawing.Point(137, 48);
            this.btnSvcStart.Name = "btnSvcStart";
            this.btnSvcStart.Size = new System.Drawing.Size(118, 23);
            this.btnSvcStart.TabIndex = 18;
            this.btnSvcStart.Text = "Запуск служб";
            this.btnSvcStart.UseVisualStyleBackColor = true;
            // 
            // btnSvcStop
            // 
            this.btnSvcStop.Location = new System.Drawing.Point(137, 19);
            this.btnSvcStop.Name = "btnSvcStop";
            this.btnSvcStop.Size = new System.Drawing.Size(118, 23);
            this.btnSvcStop.TabIndex = 17;
            this.btnSvcStop.Text = "Остановка служб";
            this.btnSvcStop.UseVisualStyleBackColor = true;
            this.btnSvcStop.Click += new System.EventHandler(this.BtnSvcStop_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnSetBD);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.numTimeout);
            this.groupBox3.Controls.Add(this.numRepeats);
            this.groupBox3.Controls.Add(this.chkService);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Location = new System.Drawing.Point(423, 356);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(206, 137);
            this.groupBox3.TabIndex = 17;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Настройки";
            // 
            // btnSetBD
            // 
            this.btnSetBD.Location = new System.Drawing.Point(9, 104);
            this.btnSetBD.Name = "btnSetBD";
            this.btnSetBD.Size = new System.Drawing.Size(125, 23);
            this.btnSetBD.TabIndex = 19;
            this.btnSetBD.Text = "Настройка БД Облик";
            this.btnSetBD.UseVisualStyleBackColor = true;
            this.btnSetBD.Click += new System.EventHandler(this.BtnSetBD_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.btnCleanLog);
            this.groupBox4.Controls.Add(this.chkSaveLogs);
            this.groupBox4.Controls.Add(this.log);
            this.groupBox4.Location = new System.Drawing.Point(12, 499);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(617, 146);
            this.groupBox4.TabIndex = 18;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Логгирование";
            // 
            // btnCleanLog
            // 
            this.btnCleanLog.Location = new System.Drawing.Point(516, 116);
            this.btnCleanLog.Name = "btnCleanLog";
            this.btnCleanLog.Size = new System.Drawing.Size(95, 23);
            this.btnCleanLog.TabIndex = 16;
            this.btnCleanLog.Text = "Очистить лог";
            this.btnCleanLog.UseVisualStyleBackColor = true;
            this.btnCleanLog.Click += new System.EventHandler(this.BtnCleanLog_Click);
            // 
            // chkSaveLogs
            // 
            this.chkSaveLogs.AutoSize = true;
            this.chkSaveLogs.Location = new System.Drawing.Point(6, 120);
            this.chkSaveLogs.Name = "chkSaveLogs";
            this.chkSaveLogs.Size = new System.Drawing.Size(117, 17);
            this.chkSaveLogs.TabIndex = 15;
            this.chkSaveLogs.Text = "Сохранять в файл";
            this.chkSaveLogs.UseVisualStyleBackColor = true;
            this.chkSaveLogs.CheckedChanged += new System.EventHandler(this.ChkSaveLogs_CheckedChanged);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(645, 653);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.dgCounters);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmMain";
            this.Text = "Oblic Cleaner v.0.1a";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmMain_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.dgCounters)).EndInit();
            this.cmCounters.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numTimeout)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRepeats)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.DataGridView dgCounters;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numTimeout;
        private System.Windows.Forms.ContextMenuStrip cmCounters;
        private System.Windows.Forms.ToolStripMenuItem cmGetData;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem cmSelAll;
        private System.Windows.Forms.ToolStripMenuItem cmSelNone;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem cmAdd;
        private System.Windows.Forms.ToolStripMenuItem cmDel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numRepeats;
        private System.Windows.Forms.Button btnDel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnGetdata;
        private System.Windows.Forms.Button btnDelDG;
        private System.Windows.Forms.ListBox log;
        private System.Windows.Forms.CheckBox chkService;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.CheckBox chkSaveLogs;
        private System.Windows.Forms.Button btnCleanLog;
        private System.Windows.Forms.Button btnSvcStart;
        private System.Windows.Forms.Button btnSvcStop;
        private System.Windows.Forms.Button btnGetDB;
        private System.Windows.Forms.Button btnSetBD;
    }
}

