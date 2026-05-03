// -----------------------------------------------------------------------
// <copyright file="Form1.Designer.cs" company="nick-cdev">
// Copyright (c) 2016-2026 nick-cdev (https://github.com)
// Licensed under the AGPL-3.0 license.
// 
// DISCLAIMER: This tool is for educational purposes only. 
// The author is not responsible for account bans or system damage.
// </copyright>
// -----------------------------------------------------------------------
namespace WowAccountManager
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            this.mainTimer = new System.Windows.Forms.Timer(this.components);
            this.DEBUGColorTimer = new System.Windows.Forms.Timer(this.components);
            this.dataSet1 = new System.Data.DataSet();
            this.dataTable1 = new System.Data.DataTable();
            this.dataColumn1 = new System.Data.DataColumn();
            this.dataColumn2 = new System.Data.DataColumn();
            this.dataColumn3 = new System.Data.DataColumn();
            this.dataColumn4 = new System.Data.DataColumn();
            this.dataColumn5 = new System.Data.DataColumn();
            this.dataColumn6 = new System.Data.DataColumn();
            this.spamtimer = new System.Windows.Forms.Timer(this.components);
            this.antiafktimer = new System.Windows.Forms.Timer(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.groupBox25 = new System.Windows.Forms.GroupBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.DebugButton1 = new System.Windows.Forms.Button();
            this.DebugButton2 = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.Switch_button = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.charnameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.serverDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.loginDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.passDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.realmnameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.charpositionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.LocaleBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.WowFolderBox = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataTable1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox25.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainTimer
            // 
            this.mainTimer.Enabled = true;
            this.mainTimer.Interval = 500;
            this.mainTimer.Tick += new System.EventHandler(this.MainTimer_Tick);
            // 
            // DEBUGColorTimer
            // 
            this.DEBUGColorTimer.Interval = 200;
            this.DEBUGColorTimer.Tick += new System.EventHandler(this.DEBUGColorTimer_Tick);
            // 
            // dataSet1
            // 
            this.dataSet1.DataSetName = "NewDataSet";
            this.dataSet1.Tables.AddRange(new System.Data.DataTable[] {
            this.dataTable1});
            // 
            // dataTable1
            // 
            this.dataTable1.Columns.AddRange(new System.Data.DataColumn[] {
            this.dataColumn1,
            this.dataColumn2,
            this.dataColumn3,
            this.dataColumn4,
            this.dataColumn5,
            this.dataColumn6});
            this.dataTable1.TableName = "Table1";
            // 
            // dataColumn1
            // 
            this.dataColumn1.ColumnName = "char_name";
            // 
            // dataColumn2
            // 
            this.dataColumn2.ColumnName = "server";
            // 
            // dataColumn3
            // 
            this.dataColumn3.ColumnName = "login";
            // 
            // dataColumn4
            // 
            this.dataColumn4.ColumnName = "pass";
            // 
            // dataColumn5
            // 
            this.dataColumn5.ColumnName = "realm_name";
            // 
            // dataColumn6
            // 
            this.dataColumn6.ColumnName = "char_position";
            // 
            // spamtimer
            // 
            this.spamtimer.Interval = 50;
            this.spamtimer.Tick += new System.EventHandler(this.SpamTimer_Tick);
            // 
            // antiafktimer
            // 
            this.antiafktimer.Enabled = true;
            this.antiafktimer.Interval = 4000;
            this.antiafktimer.Tick += new System.EventHandler(this.AntiAFKTimer_Tick);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.richTextBox1);
            this.groupBox1.Location = new System.Drawing.Point(6, 124);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(642, 107);
            this.groupBox1.TabIndex = 23;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Wow windows:";
            // 
            // richTextBox1
            // 
            this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox1.Cursor = System.Windows.Forms.Cursors.Default;
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBox1.Location = new System.Drawing.Point(3, 16);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.richTextBox1.Size = new System.Drawing.Size(636, 88);
            this.richTextBox1.TabIndex = 24;
            this.richTextBox1.Text = "";
            this.richTextBox1.WordWrap = false;
            this.richTextBox1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.RichTextBox1_MouseClick);
            // 
            // groupBox25
            // 
            this.groupBox25.Controls.Add(this.listBox1);
            this.groupBox25.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox25.Location = new System.Drawing.Point(0, 369);
            this.groupBox25.Name = "groupBox25";
            this.groupBox25.Size = new System.Drawing.Size(651, 87);
            this.groupBox25.TabIndex = 24;
            this.groupBox25.TabStop = false;
            this.groupBox25.Text = "Log";
            // 
            // listBox1
            // 
            this.listBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(3, 16);
            this.listBox1.Margin = new System.Windows.Forms.Padding(2);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(645, 68);
            this.listBox1.TabIndex = 39;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.textBox5);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.textBox4);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.checkBox2);
            this.groupBox3.Controls.Add(this.textBox1);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Location = new System.Drawing.Point(5, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(241, 111);
            this.groupBox3.TabIndex = 33;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Spam key:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(133, 67);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(54, 13);
            this.label9.TabIndex = 40;
            this.label9.Text = "Windows:";
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(133, 83);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(61, 20);
            this.textBox5.TabIndex = 39;
            this.textBox5.Text = "1,2,3";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(66, 67);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(61, 13);
            this.label8.TabIndex = 38;
            this.label8.Text = "Interval ms:";
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(66, 83);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(61, 20);
            this.textBox4.TabIndex = 37;
            this.textBox4.Text = "1000";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 67);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(33, 13);
            this.label7.TabIndex = 36;
            this.label7.Text = "Keys:";
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox2.Location = new System.Drawing.Point(6, 41);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(130, 17);
            this.checkBox2.TabIndex = 34;
            this.checkBox2.Text = "Enable Spam keys";
            this.checkBox2.UseVisualStyleBackColor = true;
            this.checkBox2.CheckedChanged += new System.EventHandler(this.CheckBox2_CheckedChanged);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(5, 83);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(51, 20);
            this.textBox1.TabIndex = 33;
            this.textBox1.Text = "qr";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(52, 16);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(98, 20);
            this.label3.TabIndex = 32;
            this.label3.Text = "SPAM OFF";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.DebugButton1);
            this.groupBox4.Controls.Add(this.DebugButton2);
            this.groupBox4.Controls.Add(this.label6);
            this.groupBox4.Controls.Add(this.checkBox1);
            this.groupBox4.Location = new System.Drawing.Point(252, 12);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(223, 111);
            this.groupBox4.TabIndex = 34;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "AntiAFK:";
            // 
            // DebugButton1
            // 
            this.DebugButton1.Location = new System.Drawing.Point(72, 64);
            this.DebugButton1.Margin = new System.Windows.Forms.Padding(2);
            this.DebugButton1.Name = "DebugButton1";
            this.DebugButton1.Size = new System.Drawing.Size(85, 19);
            this.DebugButton1.TabIndex = 41;
            this.DebugButton1.Text = "colortimer go";
            this.DebugButton1.UseVisualStyleBackColor = true;
            this.DebugButton1.Click += new System.EventHandler(this.Debug_button1_Click);
            // 
            // DebugButton2
            // 
            this.DebugButton2.AutoSize = true;
            this.DebugButton2.Location = new System.Drawing.Point(74, 86);
            this.DebugButton2.Name = "DebugButton2";
            this.DebugButton2.Size = new System.Drawing.Size(75, 23);
            this.DebugButton2.TabIndex = 40;
            this.DebugButton2.Text = "test";
            this.DebugButton2.UseVisualStyleBackColor = true;
            this.DebugButton2.Click += new System.EventHandler(this.DebugButton2_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label6.Location = new System.Drawing.Point(43, 16);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(123, 20);
            this.label6.TabIndex = 35;
            this.label6.Text = "ANTIAFK OFF";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox1.Location = new System.Drawing.Point(6, 41);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(114, 17);
            this.checkBox1.TabIndex = 33;
            this.checkBox1.Text = "Enable AntiAFK";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.CheckBox1_CheckedChanged);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.Switch_button);
            this.groupBox5.Controls.Add(this.dataGridView1);
            this.groupBox5.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox5.Location = new System.Drawing.Point(0, 231);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(651, 138);
            this.groupBox5.TabIndex = 38;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Accounts";
            // 
            // Switch_button
            // 
            this.Switch_button.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.Switch_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Switch_button.Location = new System.Drawing.Point(3, 13);
            this.Switch_button.Margin = new System.Windows.Forms.Padding(2);
            this.Switch_button.Name = "Switch_button";
            this.Switch_button.Size = new System.Drawing.Size(645, 28);
            this.Switch_button.TabIndex = 32;
            this.Switch_button.Text = "Switch Account";
            this.Switch_button.UseVisualStyleBackColor = true;
            this.Switch_button.Click += new System.EventHandler(this.Switch_button_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.charnameDataGridViewTextBoxColumn,
            this.serverDataGridViewTextBoxColumn,
            this.loginDataGridViewTextBoxColumn,
            this.passDataGridViewTextBoxColumn,
            this.realmnameDataGridViewTextBoxColumn,
            this.charpositionDataGridViewTextBoxColumn});
            this.dataGridView1.DataMember = "Table1";
            this.dataGridView1.DataSource = this.dataSet1;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dataGridView1.Location = new System.Drawing.Point(3, 41);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(2);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(645, 94);
            this.dataGridView1.TabIndex = 31;
            // 
            // charnameDataGridViewTextBoxColumn
            // 
            this.charnameDataGridViewTextBoxColumn.DataPropertyName = "char_name";
            this.charnameDataGridViewTextBoxColumn.HeaderText = "char_name";
            this.charnameDataGridViewTextBoxColumn.Name = "charnameDataGridViewTextBoxColumn";
            // 
            // serverDataGridViewTextBoxColumn
            // 
            this.serverDataGridViewTextBoxColumn.DataPropertyName = "server";
            this.serverDataGridViewTextBoxColumn.HeaderText = "server";
            this.serverDataGridViewTextBoxColumn.Name = "serverDataGridViewTextBoxColumn";
            // 
            // loginDataGridViewTextBoxColumn
            // 
            this.loginDataGridViewTextBoxColumn.DataPropertyName = "login";
            this.loginDataGridViewTextBoxColumn.HeaderText = "login";
            this.loginDataGridViewTextBoxColumn.Name = "loginDataGridViewTextBoxColumn";
            // 
            // passDataGridViewTextBoxColumn
            // 
            this.passDataGridViewTextBoxColumn.DataPropertyName = "pass";
            this.passDataGridViewTextBoxColumn.HeaderText = "pass";
            this.passDataGridViewTextBoxColumn.Name = "passDataGridViewTextBoxColumn";
            // 
            // realmnameDataGridViewTextBoxColumn
            // 
            this.realmnameDataGridViewTextBoxColumn.DataPropertyName = "realm_name";
            this.realmnameDataGridViewTextBoxColumn.HeaderText = "realm_name";
            this.realmnameDataGridViewTextBoxColumn.Name = "realmnameDataGridViewTextBoxColumn";
            // 
            // charpositionDataGridViewTextBoxColumn
            // 
            this.charpositionDataGridViewTextBoxColumn.DataPropertyName = "char_position";
            this.charpositionDataGridViewTextBoxColumn.HeaderText = "char_position";
            this.charpositionDataGridViewTextBoxColumn.Name = "charpositionDataGridViewTextBoxColumn";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.LocaleBox);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.WowFolderBox);
            this.groupBox2.Location = new System.Drawing.Point(481, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(167, 111);
            this.groupBox2.TabIndex = 39;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Settings";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 13);
            this.label2.TabIndex = 40;
            this.label2.Text = "Locale:";
            this.label2.Click += new System.EventHandler(this.Label2_Click);
            // 
            // LocaleBox
            // 
            this.LocaleBox.Location = new System.Drawing.Point(13, 78);
            this.LocaleBox.Name = "LocaleBox";
            this.LocaleBox.Size = new System.Drawing.Size(143, 20);
            this.LocaleBox.TabIndex = 39;
            this.LocaleBox.Text = "enUS";
            this.LocaleBox.TextChanged += new System.EventHandler(this.LocaleBox_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 20);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 13);
            this.label5.TabIndex = 36;
            this.label5.Text = "Wow Folder:";
            // 
            // WowFolderBox
            // 
            this.WowFolderBox.Location = new System.Drawing.Point(13, 35);
            this.WowFolderBox.Margin = new System.Windows.Forms.Padding(2);
            this.WowFolderBox.Name = "WowFolderBox";
            this.WowFolderBox.Size = new System.Drawing.Size(143, 20);
            this.WowFolderBox.TabIndex = 35;
            this.WowFolderBox.Text = "d:\\Wow";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(651, 456);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox25);
            this.Controls.Add(this.groupBox1);
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "WOW Account Manager";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.dataSet1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataTable1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox25.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

#endregion
        private System.Windows.Forms.Timer mainTimer;
        private System.Windows.Forms.Timer DEBUGColorTimer;
        private System.Data.DataSet dataSet1;
        private System.Windows.Forms.Timer spamtimer;
        private System.Windows.Forms.Timer antiafktimer;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.GroupBox groupBox25;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Data.DataTable dataTable1;
        private System.Data.DataColumn dataColumn1;
        private System.Data.DataColumn dataColumn2;
        private System.Data.DataColumn dataColumn3;
        private System.Data.DataColumn dataColumn4;
        private System.Data.DataColumn dataColumn5;
        private System.Data.DataColumn dataColumn6;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button Switch_button;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn charnameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn serverDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn loginDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn passDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn realmnameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn charpositionDataGridViewTextBoxColumn;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox LocaleBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox WowFolderBox;
        private System.Windows.Forms.Button DebugButton1;
        private System.Windows.Forms.Button DebugButton2;
    }
}

