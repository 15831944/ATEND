﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Atend.Base;
using System.Collections;
using Autodesk.AutoCAD.EditorInput;

namespace Atend.Equipment
{
    public partial class frmMafsal02 : Form
    {
        public int productCode = -1;
        Guid SelectedMafsalCode = Guid.Empty;
        public bool IsDefault = false;
        int Code = -1;

        DataColumn TypeName = new DataColumn("Name", typeof(string));
        DataColumn TypeCode = new DataColumn("Code", typeof(int));
        DataTable TypeTbl = new DataTable();
        bool ForceToClose = false;

        public frmMafsal02()
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
            Atend.Control.Common.selectedProductCode = -1;

            TypeTbl.Columns.Add(TypeCode);
            TypeTbl.Columns.Add(TypeName);

            DataRow dr1 = TypeTbl.NewRow();
            dr1["Name"] = "خشک به روغنی";
            dr1["Code"] = 1;

            DataRow dr2 = TypeTbl.NewRow();
            dr2["Name"] = "چدنی";
            dr2["Code"] = 2;

            DataRow dr3 = TypeTbl.NewRow();
            dr3["Name"] = "حرارتی";
            dr3["Code"] = 3;


            TypeTbl.Rows.Clear();
            TypeTbl.Rows.Add(dr1);
            TypeTbl.Rows.Add(dr2);
            TypeTbl.Rows.Add(dr3);
        }

        private void Reset()
        {
            cboCrossSectionArea.SelectedIndex = 0;
            //productCode = -1;
            SelectedMafsalCode = Guid.Empty;
            txtComment.Text = string.Empty;
            cboType.SelectedIndex = 0;
            txtName.Text = string.Empty;
            IsDefault = false;
            tsbIsDefault.Checked = false;
            productCode = -1;
            Code = -1;
            Atend.Control.Common.selectedProductCode = -1;
            txtBackUpName.Text = string.Empty;
            txtCode.Text = string.Empty;

            ClearCheckAndGrid(tvOperation, gvOperation);
            ClearCheckAndGrid(tvEquipment, gvSelectedEquipment);
        }

        private void ClearCheckAndGrid(TreeView treeView, DataGridView dataGridView)
        {
            foreach (TreeNode rootNode in treeView.Nodes)
            {
                rootNode.Checked = false;
                rootNode.BackColor = Color.White;
                foreach (TreeNode childNode in rootNode.Nodes)
                {
                    childNode.Checked = false;
                }
            }



            for (int i = dataGridView.Rows.Count - 1; i >= 0; i--)
            {
                dataGridView.Rows.RemoveAt(i);
            }
            dataGridView.Rows.Clear();
            dataGridView.Refresh();
        }

        private bool CheckStatuseOfAccessChangeDefault()
        {
            Autodesk.AutoCAD.EditorInput.Editor ed = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor;
            //ed.WriteMessage("AccessChangeDefault={0}\n", Atend.Control.Common.AccessChangeDefault);
            if (!Atend.Control.Common.AccessChangeDefault)
            {
                if (SelectedMafsalCode == Guid.Empty && IsDefault)
                {
                    MessageBox.Show("کاربر گرامی شما اجازه ثبت تجهیز به صورت پیش فرض ندارید", "خطا");
                    return false;
                }
                else
                {
                    Atend.Base.Equipment.EMafsal Mafsal = Atend.Base.Equipment.EMafsal.SelectByXCode(SelectedMafsalCode);
                    if (Mafsal.IsDefault || IsDefault)
                    {
                        MessageBox.Show("کاربر گرامی شما اجازه ویرایش  تجهیز به صورت پیش فرض ندارید", "خطا");
                        return false;
                    }
                }
            }
            return true;
        }

        private bool Validation()
        {
            //if (Atend.Control.Common.selectedProductCode == -1)
            //{
            //    MessageBox.Show("لطفا ابتدا یک کالا را از پشتیبان انتخاب کنید", "خطا");

            //    return false;
            //}
            if (string.IsNullOrEmpty(txtName.Text))
            {
                MessageBox.Show("لطفا نام را مشخص نمایید", "خطا");
                txtName.Focus();
                return false;
            }
            if (Atend.Base.Equipment.EMafsal.SearchByName(txtName.Text) == true && SelectedMafsalCode == Guid.Empty)
            {
                MessageBox.Show("نام قبلا استفاده شده است", "خطا");
                txtName.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(cboCrossSectionArea.Text))
            {
                MessageBox.Show("لطفا سطح مقطع را مشخص نمایید", "خطا");
                cboCrossSectionArea.Focus();

                return false;
            }
            if (string.IsNullOrEmpty(cboVoltage.Text))
            {
                MessageBox.Show("لطفا ولتاژ را مشخص نمایید", "خطا");
                cboCrossSectionArea.Focus();

                return false;
            }
            if (string.IsNullOrEmpty(cboType.Text))
            {
                MessageBox.Show("لطفا نوع را مشخص نمایید", "خطا");
                cboType.Focus();

                return false;
            }

            Atend.Base.Equipment.EMafsal mafsal = Atend.Base.Equipment.EMafsal.CheckForExist(Convert.ToDouble(cboCrossSectionArea.Text), Convert.ToByte(cboType.SelectedValue), Convert.ToInt32(cboVoltage.Text));
            if (mafsal.Code != -1 && SelectedMafsalCode == Guid.Empty)
            {
                if (MessageBox.Show("مفصل با مشخصات داده شده موجود میباشد\n\n مفصل با مشخصات فوق  : " + mafsal.Name + "\n\n" + "آیا مایل به ادامه  ثبت می باشید؟", "خطا", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    cboCrossSectionArea.Focus();
                    return false;
                }
            }

            if (!Atend.Global.Utility.UBinding.CheckGridValidation(gvOperation, 3))
            {
                MessageBox.Show("لطفا تعداد آماده سازی را با فرمت مناسب وارد نمایید", "خطا");
                gvOperation.Focus();
                return false;
            }
            if (!Atend.Global.Utility.UBinding.CheckGridValidation(gvSelectedEquipment, 2))
            {
                MessageBox.Show("لطفا تعداد تجهیزات جانبی را با فرمت مناسب وارد نمایید", "خطا");
                gvSelectedEquipment.Focus();
                return false;
            }

            for (int j = 0; j < gvSelectedEquipment.Rows.Count; j++)
            {
                Atend.Base.Equipment.EProductPackage _EProductPackage = new Atend.Base.Equipment.EProductPackage();
                _EProductPackage.XCode = new Guid(gvSelectedEquipment.Rows[j].Cells[0].Value.ToString());
                _EProductPackage.Count = Convert.ToInt32(gvSelectedEquipment.Rows[j].Cells[2].Value.ToString());
                _EProductPackage.TableType = Convert.ToInt16(gvSelectedEquipment.Rows[j].Cells[3].Value.ToString());

                if (Atend.Base.Equipment.EContainerPackage.FindLoopNode(SelectedMafsalCode, Convert.ToInt32(Atend.Control.Enum.ProductType.Mafsal), _EProductPackage.XCode, _EProductPackage.TableType))
                {
                    MessageBox.Show(string.Format("تجهیز '{0}' در زیر تجهیزات موجود می باشد", txtName.Text), "خطا");
                    gvSelectedEquipment.Focus();
                    return false;
                }

            }

            return CheckStatuseOfAccessChangeDefault();
            //return true;
        }

        private void Save()
        {
            txtName.Focus();
            Atend.Base.Equipment.EMafsal mafsal = new Atend.Base.Equipment.EMafsal();
            mafsal.CrossSectionArea = Convert.ToDouble(cboCrossSectionArea.Text);
            mafsal.ProductCode = Atend.Control.Common.selectedProductCode;
            mafsal.Type = Convert.ToByte(cboType.SelectedValue);
            mafsal.Comment = txtComment.Text;
            mafsal.Voltage = Convert.ToInt32(cboVoltage.Text);
            mafsal.Name = txtName.Text;
            mafsal.IsDefault = IsDefault;
            mafsal.Code = Code;

            //Equipment
            ArrayList EPackageProduct = new ArrayList();
            for (int j = 0; j < gvSelectedEquipment.Rows.Count; j++)
            {
                Atend.Base.Equipment.EProductPackage _EProductPackage = new Atend.Base.Equipment.EProductPackage();
                _EProductPackage.XCode = new Guid(gvSelectedEquipment.Rows[j].Cells[0].Value.ToString());
                _EProductPackage.Count = Convert.ToInt32(gvSelectedEquipment.Rows[j].Cells[2].Value.ToString());
                _EProductPackage.TableType = Convert.ToInt16(gvSelectedEquipment.Rows[j].Cells[3].Value.ToString());
                EPackageProduct.Add(_EProductPackage);
            }
            mafsal.EquipmentList = EPackageProduct;

            //Operation
            ArrayList EOperation = new ArrayList();
            for (int i = 0; i < gvOperation.Rows.Count; i++)
            {
                Atend.Base.Equipment.EOperation _EOperation = new Atend.Base.Equipment.EOperation();
                _EOperation.ProductID = Convert.ToInt32(gvOperation.Rows[i].Cells[0].Value);
                _EOperation.Count = Convert.ToDouble(gvOperation.Rows[i].Cells[3].Value);
                EOperation.Add(_EOperation);
            }
            mafsal.OperationList = EOperation;
            if (SelectedMafsalCode == Guid.Empty)
            {
                if (mafsal.InsertX())
                    Reset();
                else
                    MessageBox.Show("امکان ثبت اطلاعات نمی باشد", "خطا");
            }
            else
            {
                mafsal.XCode = SelectedMafsalCode;
                if (mafsal.UpdateX())
                    Reset();
                else
                    MessageBox.Show("امکان به روز رسانی اطلاعات نمی باشد", "خطا");
            }
        }

        private void Delete()
        {
            string name = string.Empty;
            if (!Atend.Global.Utility.UBinding.ExistInSubEquip(SelectedMafsalCode, out name))
            {
                MessageBox.Show(string.Format("حذف بدلیل وجود در تجهیزات جانبی امکانپذیر نمی باشد\n تجهیز موردنظر زیر تجهیز '{0}' میباشد ", name), "خطا");

                return;
            }
            //Atend.Base.Equipment.EProductPackage _ProductPackage = Atend.Base.Equipment.EProductPackage.SelectByXCode(SelectedMafsalCode);
            //if (_ProductPackage.Code != -1)
            //{
            //    MessageBox.Show("حذف  بدلیل وجود در تجهیزات جانبی امکانپذیر نمی باشد ", "خطا");
            //    return;
            //}

            if (MessageBox.Show("آیا از حذف کردن اطلاعات اطمینان دارید؟", "حذف", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                if (SelectedMafsalCode != Guid.Empty)
                {
                    if (Atend.Base.Equipment.EMafsal.DeleteX(SelectedMafsalCode))
                        Reset();
                    else
                        MessageBox.Show("امکان حذف کردن اطلاعات نمی باشد", "خطا");
                }
                else
                    MessageBox.Show("لطفاً گزینه مورد نظر را انتخاب نمایید", "خطا");
            }
        }

        public void BindDataToOwnControl(Guid XCode)
        {
            Atend.Base.Equipment.EMafsal mafsal = Atend.Base.Equipment.EMafsal.SelectByXCode(XCode);
            //Atend.Base.Base.BProduct product = Atend.Base.Base.BProduct.Select_ByCode(reCloser.ProductCode);
            Atend.Control.Common.selectedProductCode = mafsal.ProductCode;
            SelectProduct();

            SelectedMafsalCode = XCode;
            txtName.Text = mafsal.Name;
            cboCrossSectionArea.Text = mafsal.CrossSectionArea.ToString();
            cboVoltage.Text = mafsal.Voltage.ToString();
            cboType.SelectedValue = mafsal.Type;
            txtComment.Text = mafsal.Comment;
            tsbIsDefault.Checked = mafsal.IsDefault;
            Code = mafsal.Code;
            BindTreeViwAndGrid();

        }

        private void BindTreeViwAndGrid()
        {
            //EQUIPMENT
            ClearCheckAndGrid(tvEquipment, gvSelectedEquipment);
            gvSelectedEquipment.Refresh();
            for (int i = 0; i < Atend.Base.Equipment.EMafsal.nodeKeysEPackageX.Count; i++)
            {
                string s = Atend.Base.Equipment.EMafsal.nodeKeysEPackageX[i].ToString();
                foreach (TreeNode rootnode in tvEquipment.Nodes)
                {
                    foreach (TreeNode chileNode in rootnode.Nodes)
                    {
                        if (Atend.Base.Equipment.EMafsal.nodeTypeEPackageX[i].ToString() == chileNode.Name.ToString())
                        {
                            if (chileNode.Tag.ToString() == s)
                            {
                                rootnode.BackColor = Color.FromArgb(203, 214, 235);
                                chileNode.Checked = true;
                                gvSelectedEquipment.Rows.Add();
                                gvSelectedEquipment.Rows[gvSelectedEquipment.Rows.Count - 1].Cells[0].Value = chileNode.Tag;
                                gvSelectedEquipment.Rows[gvSelectedEquipment.Rows.Count - 1].Cells[1].Value = chileNode.Text;
                                gvSelectedEquipment.Rows[gvSelectedEquipment.Rows.Count - 1].Cells[2].Value = Atend.Base.Equipment.EMafsal.nodeCountEPackageX[i].ToString();
                                gvSelectedEquipment.Rows[gvSelectedEquipment.Rows.Count - 1].Cells[3].Value = chileNode.Name;
                            }
                        }
                    }
                }
            }
            //************
            //Operation
            ClearCheckAndGrid(tvOperation, gvOperation);
            gvOperation.Refresh();
            for (int i = 0; i < Atend.Base.Equipment.EMafsal.nodeKeys.Count; i++)
            {
                Atend.Base.Equipment.EOperation Operation = ((Atend.Base.Equipment.EOperation)Atend.Base.Equipment.EMafsal.nodeKeys[i]);
                string s = Operation.ProductID.ToString();

                foreach (TreeNode rootnode in tvOperation.Nodes)
                {
                    if (rootnode.Tag.ToString() == s)
                    {
                        rootnode.Checked = true;
                        gvOperation.Rows.Add();
                        gvOperation.Rows[gvOperation.Rows.Count - 1].Cells[0].Value = rootnode.Tag;
                        gvOperation.Rows[gvOperation.Rows.Count - 1].Cells[1].Value = rootnode.Text;
                        //gvSelectedProduct.Rows[gvSelectedProduct.Rows.Count - 1].Cells[2].Value = Atend.Base.Equipment.EPole.nodeCount[i].ToString();
                        Atend.Base.Base.BUnit Unit = Atend.Base.Base.BUnit.Select_ByProductID(Convert.ToInt32(rootnode.Tag.ToString()));
                        gvOperation.Rows[gvOperation.Rows.Count - 1].Cells[2].Value = Unit.Name;
                        gvOperation.Rows[gvOperation.Rows.Count - 1].Cells[3].Value = Operation.Count;
                    }

                }
            }
        }

        private void BindDataToTypeComboBox()
        {
            cboType.DisplayMember = "Name";
            cboType.ValueMember = "Code";
            cboType.DataSource = TypeTbl;
        }

        private void frmBreaker_Load(object sender, EventArgs e)
        {
            if (ForceToClose)
                this.Close();

            BindDataToTypeComboBox();

            //Atend.Base.Base.BProduct product = Atend.Base.Base.BProduct.Select_ById(productCode);
            //txtName.Text = product.Name;

            if (cboType.Items.Count > 0)
            {
                cboType.SelectedIndex = 0;// cboType.Items.Count - 1;
            }
            if (cboCrossSectionArea.Items.Count > 0)
            {
                cboCrossSectionArea.SelectedIndex = 0;
            }
            if (cboVoltage.Items.Count > 0)
            {
                cboVoltage.SelectedIndex = 0;
            }

            BindDataToTreeView();
        }

        private void BindDataToTreeView()
        {
            Atend.Global.Utility.UBinding.BindDataToTreeViewX(tvEquipment);


            DataTable ProductType = Atend.Base.Base.BProduct.SelectByTypeX(Convert.ToInt32(Atend.Control.Enum.ProductType.Operation));
            foreach (DataRow dr in ProductType.Rows)
            {
                TreeNode node = new TreeNode();
                node.Text = dr["Name"].ToString();
                node.Tag = dr["ID"].ToString();
                tvOperation.Nodes.Add(node);
            }
        }

        private void جدیدToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Reset();
        }

        private void ذخیرهسازیToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Validation())
                Save();
        }

        private void ذخیرهسازیوخروجToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Validation())
            {
                Save();
                Close();
            }
        }

        private void حذفToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CheckStatuseOfAccessChangeDefault())
                Delete();
        }

        private void btnNewBreakerType_Click(object sender, EventArgs e)
        {
            //frmMafsalType frmMafsalType = new frmMafsalType();
            // frmMafsalType.ShowDialog();
            //BindDataToTypeComboBox();
        }

        private void جستجوToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmMafsalSearch02 frmMafsalSearch = new frmMafsalSearch02(this);
            Autodesk.AutoCAD.ApplicationServices.Application.ShowModalDialog(frmMafsalSearch);
        }

        private void خروجToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void SelectProduct()
        {
            Atend.Base.Base.BProduct product = Atend.Base.Base.BProduct.Select_ByIdX(Atend.Control.Common.selectedProductCode);
            txtName.Text = product.Name;
            txtCode.Text = product.Code.ToString();
            txtBackUpName.Text = product.Name;
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            Atend.Base.frmProductSearch frmproductSearch = new Atend.Base.frmProductSearch(Atend.Control.Enum.ProductType.Mafsal);

            Autodesk.AutoCAD.ApplicationServices.Application.ShowModalDialog(frmproductSearch);
            if (Atend.Control.Common.selectedProductCode != -1)
            {
                SelectProduct();
            }
        }

        private void toolStripLabel6_Click(object sender, EventArgs e)
        {
            Reset();
        }

        private void toolStripLabel7_Click(object sender, EventArgs e)
        {
            if (Validation())
                Save();
        }

        private void toolStripLabel8_Click(object sender, EventArgs e)
        {
            Delete();
        }

        private void toolStripLabel9_Click(object sender, EventArgs e)
        {
            frmMafsalSearch02 frmMafsalSearch = new frmMafsalSearch02(this);
            Autodesk.AutoCAD.ApplicationServices.Application.ShowModalDialog(frmMafsalSearch);
        }

        private void toolStripLabel10_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void tsbIsDefault_Click(object sender, EventArgs e)
        {
            if (IsDefault)
            {
                IsDefault = false;
                tsbIsDefault.Checked = false;
            }
            else
            {
                IsDefault = true;
                tsbIsDefault.Checked = true;
            }
        }

        private void tsbShare_Click(object sender, EventArgs e)
        {

            if (SelectedMafsalCode != Guid.Empty)
            {
                if (Atend.Base.Equipment.EContainerPackage.ShareOnServer(Convert.ToInt32(Atend.Control.Enum.ProductType.Mafsal), SelectedMafsalCode))
                {
                    Atend.Base.Equipment.EMafsal  Mafsal = Atend.Base.Equipment.EMafsal.SelectByXCode(SelectedMafsalCode);
                    Code = Mafsal.Code;
                    MessageBox.Show("به اشتراک گذاری با موفقیت انجام شد");
                }
                else
                {
                    MessageBox.Show("خطا در به اشتراک گذاری .");
                }
            }
            else
            {
                MessageBox.Show("لطفا تجهیز مورد نظر را انتخاب کنید");
            }
            //if (SelectedMafsalCode != Guid.Empty)
            //{
            //    if (Atend.Base.Equipment.EMafsal.ShareOnServer(SelectedMafsalCode))
            //    {
            //        Atend.Base.Equipment.EMafsal m1 = Atend.Base.Equipment.EMafsal.SelectByXCode(SelectedMafsalCode);
            //        Code = m1.Code;
            //        MessageBox.Show("به اشتراک گذاری با موفقیت انجام شد");
            //    }
            //    else
            //        MessageBox.Show("خطا در به اشتراک گذاری . لطفاً دوباره سعی کنید");
            //}
            //else
            //    MessageBox.Show("لطفا تجهیز مورد نظر را انتخاب کنید");
        }

        private void txtOperationName_TextChanged(object sender, EventArgs e)
        {
            SearchOperation();
        }

        private void SearchOperation()
        {
            tvOperation.Nodes.Clear();
            DataTable ProductType = Atend.Base.Base.BProduct.SelectByNameTypeX(Convert.ToInt32(Atend.Control.Enum.ProductType.Operation), txtOperationName.Text);

            foreach (DataRow dr in ProductType.Rows)
            {
                TreeNode node = new TreeNode();
                node.Text = dr["Name"].ToString();
                node.Tag = dr["ID"].ToString();
                tvOperation.Nodes.Add(node);
            }
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            //gvOperation.Rows.Clear();
            for (int i = 0; i < tvOperation.Nodes.Count; i++)
            {

                if (tvOperation.Nodes[i].Checked)
                {
                    bool sw = false;
                    for (int j = 0; j < gvOperation.Rows.Count; j++)
                    {
                        if (gvOperation.Rows[j].Cells[0].Value.ToString() == tvOperation.Nodes[i].Tag.ToString())
                            sw = true;
                    }

                    if (!sw)
                    {
                        gvOperation.Rows.Add();
                        gvOperation.Rows[gvOperation.Rows.Count - 1].Cells[0].Value = tvOperation.Nodes[i].Tag.ToString();
                        gvOperation.Rows[gvOperation.Rows.Count - 1].Cells[1].Value = tvOperation.Nodes[i].Text.ToString();
                        Atend.Base.Base.BUnit Unit = Atend.Base.Base.BUnit.Select_ByProductID(Convert.ToInt32(tvOperation.Nodes[i].Tag.ToString()));
                        gvOperation.Rows[gvOperation.Rows.Count - 1].Cells[2].Value = Unit.Name;
                        gvOperation.Rows[gvOperation.Rows.Count - 1].Cells[3].Value = 1;
                    }
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (gvOperation.Rows.Count > 0)
            {
                foreach (TreeNode rootnode in tvOperation.Nodes)
                {
                    if (rootnode.Tag.ToString() == gvOperation.Rows[gvOperation.CurrentRow.Index].Cells[0].Value.ToString())
                    {
                        rootnode.Checked = false;
                    }
                }

                gvOperation.Rows.RemoveAt(gvOperation.CurrentRow.Index);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //Equipment
            Boolean canAdd = true;
            for (int i = 0; i < tvEquipment.Nodes.Count; i++)
            {
                for (int j = 0; j < tvEquipment.Nodes[i].Nodes.Count; j++)
                {
                    if (tvEquipment.Nodes[i].Nodes[j].Checked)
                    {
                        canAdd = true;
                        for (int k = 0; k < gvSelectedEquipment.Rows.Count; k++)
                        {
                            if ((gvSelectedEquipment.Rows[k].Cells[0].Value.ToString() == tvEquipment.Nodes[i].Nodes[j].Tag.ToString()) && (Convert.ToInt32(gvSelectedEquipment.Rows[k].Cells[3].Value.ToString()) == Convert.ToInt32(tvEquipment.Nodes[i].Nodes[j].Name.ToString())))
                            {
                                canAdd = false;
                                //ed.WriteMessage("No Allow To Add Row\n");
                            }
                        }
                        if (canAdd)
                        {
                            //ed.WriteMessage("AddRow\n");
                            gvSelectedEquipment.Rows.Add();
                            gvSelectedEquipment.Rows[gvSelectedEquipment.Rows.Count - 1].Cells[0].Value = tvEquipment.Nodes[i].Nodes[j].Tag.ToString();
                            gvSelectedEquipment.Rows[gvSelectedEquipment.Rows.Count - 1].Cells[1].Value = tvEquipment.Nodes[i].Nodes[j].Text.ToString();
                            gvSelectedEquipment.Rows[gvSelectedEquipment.Rows.Count - 1].Cells[3].Value = tvEquipment.Nodes[i].Nodes[j].Name.ToString();
                            gvSelectedEquipment.Rows[gvSelectedEquipment.Rows.Count - 1].Cells[2].Value = 1;

                        }

                    }
                    else
                    {
                        for (int k = 0; k < gvSelectedEquipment.Rows.Count; k++)
                        {
                            if ((gvSelectedEquipment.Rows[k].Cells[0].Value.ToString() == tvEquipment.Nodes[i].Nodes[j].Tag.ToString()) && (Convert.ToInt32(gvSelectedEquipment.Rows[k].Cells[3].Value.ToString()) == Convert.ToInt32(tvEquipment.Nodes[i].Nodes[j].Name.ToString())))
                            {
                                //ed.WriteMessage("Name To Delete" + gvSelectedEquipment.Rows[k].Cells[1].Value.ToString() + "\n");
                                gvSelectedEquipment.Rows.RemoveAt(k);

                            }
                        }

                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (gvSelectedEquipment.Rows.Count > 0)
            {
                foreach (TreeNode rootnode in tvEquipment.Nodes)
                {
                    foreach (TreeNode childNode in rootnode.Nodes)
                    {
                        if (childNode.Tag.ToString() == gvSelectedEquipment.Rows[gvSelectedEquipment.CurrentRow.Index].Cells[0].Value.ToString())
                        {
                            childNode.Checked = false;
                        }
                    }
                }

                gvSelectedEquipment.Rows.RemoveAt(gvSelectedEquipment.CurrentRow.Index);
            }
        }


    }
}