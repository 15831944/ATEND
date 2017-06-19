﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Autodesk.AutoCAD.EditorInput;

namespace Atend.Equipment
{
    public partial class frmPhuseKeySearch02 : Form
    {
        frmPhuseKey02 frmphuseKey;
        bool ForceToClose = false;

        public frmPhuseKeySearch02(frmPhuseKey02 _PhuseKey)
        {
            Editor ed = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor;
            ed.WriteMessage("\nchecking.....\n");
            if (!Atend.Global.Acad.DrawEquips.Dicision.IsHere())
            {
                if (!Atend.Global.Acad.DrawEquips.Dicision.IsThere())
                {
                    //System.Diagnostics.Process[] prs = System.Diagnostics.Process.GetProcesses();
                    //foreach (System.Diagnostics.Process pr in prs)
                    //{
                    //    if (pr.ProcessName == "acad")
                    //    {
                    //        pr.CloseMainWindow();
                    //    }
                    //}
                    Atend.Global.Acad.Notification notification = new Atend.Global.Acad.Notification();
                    notification.Title = "شناسایی قفل";
                    notification.Msg = "لطفا وضعیت قفل را بررسی نمایید ";
                    notification.infoCenterBalloon();

                    ForceToClose = true;

                }
            }

            InitializeComponent();
            frmphuseKey = _PhuseKey;
        }

        private void Search()
        {
            gvProduct.AutoGenerateColumns = false;
            gvProduct.DataSource = Atend.Base.Equipment.EPhuseKey.SearchLocal(tstName.Text);
            ShowNumberColumn(gvProduct);
        }

        private void ShowNumberColumn(DataGridView _DataGridView)
        {
            //DataGridViewTextBoxColumn tx=new DataGridViewTextBoxColumn()
            //DataGridViewColumn c = new DataGridViewColumn();
            //c.Index = 0;
            //c.Name = "COLNO";
            //c.HeaderText = "ردیف";
            //_DataGridView.Columns.Add("COLNO", "ردیف");
            int counter = 1;
            //_DataGridView.Width = 10;

            foreach (DataGridViewRow gr in _DataGridView.Rows)
            {
                gr.Cells["COLNO"].Value = counter.ToString();
                counter++;
            }
            _DataGridView.EndEdit();
            _DataGridView.Refresh();
        }

        private void toolStripLabel2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void frmPhotokey02_Load(object sender, EventArgs e)
        {
            if (ForceToClose)
                this.Close();

            Search();
        }

        private void tstName_TextChanged(object sender, EventArgs e)
        {
            Search();
        }

        private void tsbSearch_Click(object sender, EventArgs e)
        {
            Search();
        }

        private void gvProduct_DoubleClick(object sender, EventArgs e)
        {
            if (gvProduct.Rows.Count > 0)
            {
                frmphuseKey.BindDataToOwnControl(new Guid(gvProduct.Rows[gvProduct.CurrentRow.Index].Cells[0].Value.ToString()));
                Close();
            }
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void tstName_Click(object sender, EventArgs e)
        {

        }

        private void toolStripLabel2_Click_1(object sender, EventArgs e)
        {
            Close();
        }
    }
}