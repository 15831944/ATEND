﻿namespace Atend.Design
{
    partial class frmEditDrawBranch02
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmEditDrawBranch02));
            this.label2 = new System.Windows.Forms.Label();
            this.nudCrossSectionArea = new System.Windows.Forms.NumericUpDown();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.gvConductor = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkSectionArea = new System.Windows.Forms.CheckBox();
            this.cboMaterial = new System.Windows.Forms.ComboBox();
            this.chkMaterail = new System.Windows.Forms.CheckBox();
            this.lblSelected = new System.Windows.Forms.Label();
            this.cboIsExist = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.cboProjCode = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.nudCrossSectionArea)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvConductor)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(532, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(120, 13);
            this.label2.TabIndex = 19;
            this.label2.Text = "تعداد سيم انتخاب شده :";
            // 
            // nudCrossSectionArea
            // 
            this.nudCrossSectionArea.Location = new System.Drawing.Point(417, 20);
            this.nudCrossSectionArea.Name = "nudCrossSectionArea";
            this.nudCrossSectionArea.Size = new System.Drawing.Size(57, 21);
            this.nudCrossSectionArea.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(670, 11);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(30, 23);
            this.button1.TabIndex = 18;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(704, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 13);
            this.label1.TabIndex = 17;
            this.label1.Text = "انتخاب سيم :";
            // 
            // btnSearch
            // 
            this.btnSearch.Image = ((System.Drawing.Image)(resources.GetObject("btnSearch.Image")));
            this.btnSearch.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSearch.Location = new System.Drawing.Point(18, 20);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(101, 23);
            this.btnSearch.TabIndex = 4;
            this.btnSearch.Text = "جستجو";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // gvConductor
            // 
            this.gvConductor.AllowUserToAddRows = false;
            this.gvConductor.AllowUserToDeleteRows = false;
            this.gvConductor.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gvConductor.BackgroundColor = System.Drawing.Color.White;
            this.gvConductor.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvConductor.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column6,
            this.Column7,
            this.Column5});
            this.gvConductor.Location = new System.Drawing.Point(14, 126);
            this.gvConductor.MultiSelect = false;
            this.gvConductor.Name = "gvConductor";
            this.gvConductor.ReadOnly = true;
            this.gvConductor.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gvConductor.Size = new System.Drawing.Size(765, 257);
            this.gvConductor.TabIndex = 13;
            this.gvConductor.Click += new System.EventHandler(this.gvConductor_Click);
            // 
            // Column1
            // 
            this.Column1.DataPropertyName = "XCode";
            this.Column1.HeaderText = "Column1";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Visible = false;
            // 
            // Column2
            // 
            this.Column2.DataPropertyName = "Name";
            this.Column2.FillWeight = 70F;
            this.Column2.HeaderText = "نام";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            // 
            // Column3
            // 
            this.Column3.DataPropertyName = "MaterialName";
            this.Column3.FillWeight = 50F;
            this.Column3.HeaderText = "جنس سيم";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            // 
            // Column6
            // 
            this.Column6.DataPropertyName = "AMP";
            this.Column6.FillWeight = 30F;
            this.Column6.HeaderText = "امپدانس";
            this.Column6.Name = "Column6";
            this.Column6.ReadOnly = true;
            // 
            // Column7
            // 
            this.Column7.DataPropertyName = "MaxCurrent";
            this.Column7.FillWeight = 30F;
            this.Column7.HeaderText = "ماکزیمم جریان";
            this.Column7.Name = "Column7";
            this.Column7.ReadOnly = true;
            // 
            // Column5
            // 
            this.Column5.DataPropertyName = "UTS";
            this.Column5.FillWeight = 30F;
            this.Column5.HeaderText = "UTS";
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(95, 389);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 15;
            this.btnOk.Text = "تایید";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(14, 389);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 14;
            this.btnCancel.Text = "انصراف";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnSearch);
            this.groupBox1.Controls.Add(this.nudCrossSectionArea);
            this.groupBox1.Controls.Add(this.chkSectionArea);
            this.groupBox1.Controls.Add(this.cboMaterial);
            this.groupBox1.Controls.Add(this.chkMaterail);
            this.groupBox1.Location = new System.Drawing.Point(9, 51);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(770, 59);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "جستجو بر اساس...";
            // 
            // chkSectionArea
            // 
            this.chkSectionArea.AutoSize = true;
            this.chkSectionArea.Location = new System.Drawing.Point(480, 24);
            this.chkSectionArea.Name = "chkSectionArea";
            this.chkSectionArea.Size = new System.Drawing.Size(86, 17);
            this.chkSectionArea.TabIndex = 3;
            this.chkSectionArea.Text = "سطح مقطع:";
            this.chkSectionArea.UseVisualStyleBackColor = true;
            // 
            // cboMaterial
            // 
            this.cboMaterial.FormattingEnabled = true;
            this.cboMaterial.Location = new System.Drawing.Point(583, 20);
            this.cboMaterial.Name = "cboMaterial";
            this.cboMaterial.Size = new System.Drawing.Size(111, 21);
            this.cboMaterial.TabIndex = 2;
            // 
            // chkMaterail
            // 
            this.chkMaterail.AutoSize = true;
            this.chkMaterail.Location = new System.Drawing.Point(700, 24);
            this.chkMaterail.Name = "chkMaterail";
            this.chkMaterail.Size = new System.Drawing.Size(55, 17);
            this.chkMaterail.TabIndex = 1;
            this.chkMaterail.Text = "جنس:";
            this.chkMaterail.UseVisualStyleBackColor = true;
            // 
            // lblSelected
            // 
            this.lblSelected.AutoSize = true;
            this.lblSelected.Location = new System.Drawing.Point(505, 17);
            this.lblSelected.Name = "lblSelected";
            this.lblSelected.Size = new System.Drawing.Size(19, 13);
            this.lblSelected.TabIndex = 20;
            this.lblSelected.Text = "---";
            // 
            // cboIsExist
            // 
            this.cboIsExist.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboIsExist.FormattingEnabled = true;
            this.cboIsExist.Location = new System.Drawing.Point(195, 389);
            this.cboIsExist.Name = "cboIsExist";
            this.cboIsExist.Size = new System.Drawing.Size(121, 21);
            this.cboIsExist.TabIndex = 22;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(322, 392);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 13);
            this.label3.TabIndex = 23;
            this.label3.Text = " وضعیت تجهیز :";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(702, 394);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(81, 13);
            this.label11.TabIndex = 141;
            this.label11.Text = "شرح دستور کار:";
            // 
            // cboProjCode
            // 
            this.cboProjCode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboProjCode.FormattingEnabled = true;
            this.cboProjCode.Location = new System.Drawing.Point(407, 390);
            this.cboProjCode.Name = "cboProjCode";
            this.cboProjCode.Size = new System.Drawing.Size(289, 21);
            this.cboProjCode.TabIndex = 140;
            // 
            // frmEditDrawBranch02
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(791, 423);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.cboProjCode);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cboIsExist);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.gvConductor);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lblSelected);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmEditDrawBranch02";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ویرایش هادی";
            this.Load += new System.EventHandler(this.frmDrawBranch01_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmDrawBranch01_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.nudCrossSectionArea)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvConductor)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown nudCrossSectionArea;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.DataGridView gvConductor;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkSectionArea;
        private System.Windows.Forms.ComboBox cboMaterial;
        private System.Windows.Forms.CheckBox chkMaterail;
        private System.Windows.Forms.Label lblSelected;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.ComboBox cboIsExist;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox cboProjCode;
    }
}