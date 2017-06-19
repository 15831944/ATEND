﻿namespace Atend.Design
{
    partial class frmEditSingleBranch02
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmEditSingleBranch02));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.nudCrossSectionArea = new System.Windows.Forms.NumericUpDown();
            this.chkSectionArea = new System.Windows.Forms.CheckBox();
            this.cboMaterial = new System.Windows.Forms.ComboBox();
            this.chkMaterail = new System.Windows.Forms.CheckBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.txtLenght = new System.Windows.Forms.TextBox();
            this.cboIsExist = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.gvConductor = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.XCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CrossSectionArea = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IsSql = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label11 = new System.Windows.Forms.Label();
            this.cboProjCode = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudCrossSectionArea)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvConductor)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(712, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 15;
            this.label1.Text = "طول سیم:";
            // 
            // btnSearch
            // 
            this.btnSearch.Image = ((System.Drawing.Image)(resources.GetObject("btnSearch.Image")));
            this.btnSearch.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSearch.Location = new System.Drawing.Point(12, 18);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(101, 23);
            this.btnSearch.TabIndex = 4;
            this.btnSearch.Text = "جستجو";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Image = global::Atend.ResourceImage._16_button_ok;
            this.btnOk.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOk.Location = new System.Drawing.Point(99, 461);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 13;
            this.btnOk.Text = "تایید";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click_1);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnSearch);
            this.groupBox1.Controls.Add(this.nudCrossSectionArea);
            this.groupBox1.Controls.Add(this.chkSectionArea);
            this.groupBox1.Controls.Add(this.cboMaterial);
            this.groupBox1.Controls.Add(this.chkMaterail);
            this.groupBox1.Location = new System.Drawing.Point(6, 10);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(779, 50);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "جستجو بر اساس...";
            // 
            // nudCrossSectionArea
            // 
            this.nudCrossSectionArea.Location = new System.Drawing.Point(394, 19);
            this.nudCrossSectionArea.Name = "nudCrossSectionArea";
            this.nudCrossSectionArea.Size = new System.Drawing.Size(57, 21);
            this.nudCrossSectionArea.TabIndex = 1;
            // 
            // chkSectionArea
            // 
            this.chkSectionArea.AutoSize = true;
            this.chkSectionArea.Location = new System.Drawing.Point(457, 21);
            this.chkSectionArea.Name = "chkSectionArea";
            this.chkSectionArea.Size = new System.Drawing.Size(86, 17);
            this.chkSectionArea.TabIndex = 3;
            this.chkSectionArea.Text = "سطح مقطع:";
            this.chkSectionArea.UseVisualStyleBackColor = true;
            // 
            // cboMaterial
            // 
            this.cboMaterial.FormattingEnabled = true;
            this.cboMaterial.Location = new System.Drawing.Point(560, 19);
            this.cboMaterial.Name = "cboMaterial";
            this.cboMaterial.Size = new System.Drawing.Size(111, 21);
            this.cboMaterial.TabIndex = 2;
            // 
            // chkMaterail
            // 
            this.chkMaterail.AutoSize = true;
            this.chkMaterail.Location = new System.Drawing.Point(677, 21);
            this.chkMaterail.Name = "chkMaterail";
            this.chkMaterail.Size = new System.Drawing.Size(55, 17);
            this.chkMaterail.TabIndex = 1;
            this.chkMaterail.Text = "جنس:";
            this.chkMaterail.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Image = global::Atend.ResourceImage._16button_cancel;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(18, 461);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 12;
            this.btnCancel.Text = "انصراف";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // txtLenght
            // 
            this.txtLenght.Location = new System.Drawing.Point(585, 20);
            this.txtLenght.Name = "txtLenght";
            this.txtLenght.Size = new System.Drawing.Size(100, 21);
            this.txtLenght.TabIndex = 16;
            // 
            // cboIsExist
            // 
            this.cboIsExist.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboIsExist.FormattingEnabled = true;
            this.cboIsExist.Location = new System.Drawing.Point(12, 19);
            this.cboIsExist.Name = "cboIsExist";
            this.cboIsExist.Size = new System.Drawing.Size(156, 21);
            this.cboIsExist.TabIndex = 18;
            this.cboIsExist.SelectedIndexChanged += new System.EventHandler(this.cboIsExist_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(174, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 13);
            this.label2.TabIndex = 19;
            this.label2.Text = " وضعیت تجهیز:";
            // 
            // gvConductor
            // 
            this.gvConductor.AllowUserToAddRows = false;
            this.gvConductor.AllowUserToDeleteRows = false;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(243)))), ((int)(((byte)(249)))));
            this.gvConductor.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle4;
            this.gvConductor.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gvConductor.BackgroundColor = System.Drawing.Color.White;
            this.gvConductor.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvConductor.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.XCode,
            this.dataGridViewTextBoxColumn2,
            this.Column3,
            this.Column4,
            this.Column1,
            this.Column2,
            this.CrossSectionArea,
            this.IsSql});
            this.gvConductor.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(214)))), ((int)(((byte)(214)))));
            this.gvConductor.Location = new System.Drawing.Point(6, 65);
            this.gvConductor.MultiSelect = false;
            this.gvConductor.Name = "gvConductor";
            this.gvConductor.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gvConductor.Size = new System.Drawing.Size(779, 280);
            this.gvConductor.TabIndex = 20;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "CondCode";
            this.dataGridViewTextBoxColumn1.HeaderText = "Code";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.Visible = false;
            // 
            // XCode
            // 
            this.XCode.DataPropertyName = "CondXCode";
            this.XCode.HeaderText = "XCode";
            this.XCode.Name = "XCode";
            this.XCode.Visible = false;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "CondName";
            this.dataGridViewTextBoxColumn2.FillWeight = 70F;
            this.dataGridViewTextBoxColumn2.HeaderText = "نام تيپ";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // Column3
            // 
            this.Column3.DataPropertyName = "MaterialName";
            this.Column3.FillWeight = 50F;
            this.Column3.HeaderText = "جنس سيم";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            // 
            // Column4
            // 
            this.Column4.DataPropertyName = "AMP";
            this.Column4.FillWeight = 30F;
            this.Column4.HeaderText = "امپدانس";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            // 
            // Column1
            // 
            this.Column1.DataPropertyName = "MaxCurrent";
            this.Column1.FillWeight = 30F;
            this.Column1.HeaderText = "ماكزيمم جريان";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            // 
            // Column2
            // 
            this.Column2.DataPropertyName = "UTS";
            this.Column2.FillWeight = 30F;
            this.Column2.HeaderText = "UTS";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            // 
            // CrossSectionArea
            // 
            this.CrossSectionArea.DataPropertyName = "CrossSectionArea";
            this.CrossSectionArea.FillWeight = 40F;
            this.CrossSectionArea.HeaderText = "سطح مقطع";
            this.CrossSectionArea.Name = "CrossSectionArea";
            // 
            // IsSql
            // 
            this.IsSql.DataPropertyName = "IsSql";
            this.IsSql.HeaderText = "IsSql";
            this.IsSql.Name = "IsSql";
            this.IsSql.Visible = false;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(687, 23);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(81, 13);
            this.label11.TabIndex = 161;
            this.label11.Text = "شرح دستور کار:";
            // 
            // cboProjCode
            // 
            this.cboProjCode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboProjCode.FormattingEnabled = true;
            this.cboProjCode.Location = new System.Drawing.Point(271, 19);
            this.cboProjCode.Name = "cboProjCode";
            this.cboProjCode.Size = new System.Drawing.Size(414, 21);
            this.cboProjCode.TabIndex = 160;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cboProjCode);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.cboIsExist);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(6, 405);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(779, 50);
            this.groupBox2.TabIndex = 162;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "مشخصات دستورکار";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.txtLenght);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Location = new System.Drawing.Point(6, 351);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(779, 50);
            this.groupBox3.TabIndex = 163;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "مشخصات سیم";
            // 
            // frmEditSingleBranch02
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(790, 487);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.gvConductor);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnCancel);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmEditSingleBranch02";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ویرایش هادی";
            this.Load += new System.EventHandler(this.frmDrawBranch01_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudCrossSectionArea)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvConductor)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.NumericUpDown nudCrossSectionArea;
        private System.Windows.Forms.CheckBox chkSectionArea;
        private System.Windows.Forms.ComboBox cboMaterial;
        private System.Windows.Forms.CheckBox chkMaterail;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox txtLenght;
        private System.Windows.Forms.ComboBox cboIsExist;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView gvConductor;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn XCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn CrossSectionArea;
        private System.Windows.Forms.DataGridViewTextBoxColumn IsSql;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox cboProjCode;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
    }
}