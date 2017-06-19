﻿using System;
using System.Collections.Generic;
using System.Text;

using Autodesk.AutoCAD.EditorInput;
//using Autodesk.AutoCAD.GraphicsInterface;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.Geometry;
//using Autodesk.AutoCAD.Interop;
using Autodesk.AutoCAD.ApplicationServices;
//using Autodesk.Windows;
using Autodesk.AutoCAD.Colors;
using Autodesk.AutoCAD.AcInfoCenterConn;
using Autodesk.AutoCAD.Windows;
//using Autodesk.AutoCAD.Customization;
using Autodesk.Windows;


using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using acadApp = Autodesk.AutoCAD.ApplicationServices.Application;

////using Autodesk.AutoCAD.BoundaryRepresentation;

////using BrFace =

////  Autodesk.AutoCAD.BoundaryRepresentation.Face;

////using BrException =

////  Autodesk.AutoCAD.BoundaryRepresentation.Exception;


using System.Collections;
using System.IO;
using System.Diagnostics;
using System.Data;
using System.Data.SqlClient;

[assembly: CommandClass(typeof(Atend.Acad.AcadCommands))]
namespace Atend
{
    public class Menu 
    {
        public static Autodesk.AutoCAD.Windows.PaletteSet ProductPalette;

        Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;

        public void Initialize()
        {

            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            Atend.Control.Common.fullPath = Environment.CurrentDirectory;
            ed.WriteMessage("fullpath:{0}\n", Atend.Control.Common.fullPath);

        }


        public void ActiveDatabaseEvent()
        {
            //Database db = HostApplicationServices.WorkingDatabase;
            Database db = Application.DocumentManager.MdiActiveDocument.Database;
            db.ObjectErased += new ObjectErasedEventHandler(db_ObjectErased);

        }

        public void DeactiveDatabaseEvent()
        {
            //Database db = HostApplicationServices.WorkingDatabase;
            Database db = Application.DocumentManager.MdiActiveDocument.Database;
            db.ObjectErased -= new ObjectErasedEventHandler(db_ObjectErased);
        }

        //private static Point3d ComputeCenterPoint()
        //{

        //    Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
        //    Point3d point;
        //    //ed.writeMessage("computer point 1 \n");
        //    double centerX = Math.Abs(conductorInformation.Point1.X - conductorInformation.Point2.X) / 2;
        //    double centerY = Math.Abs(conductorInformation.Point1.Y - conductorInformation.Point2.Y) / 2;

        //    //ed.writeMessage("computer point going to if \n");
        //    if ((conductorInformation.Point2.X - conductorInformation.Point1.X) > 0 && (conductorInformation.Point2.Y - conductorInformation.Point1.Y) > 0)
        //    {
        //        //ed.writeMessage("Part one \n");
        //        point = new Point3d(conductorInformation.Point1.X + centerX, conductorInformation.Point1.Y + centerY, 0);
        //    }
        //    else if ((conductorInformation.Point2.X - conductorInformation.Point1.X) > 0 && (conductorInformation.Point2.Y - conductorInformation.Point1.Y) < 0)
        //    {
        //        //ed.writeMessage("Part two \n");
        //        point = new Point3d(conductorInformation.Point1.X + centerX, conductorInformation.Point1.Y - centerY, 0);
        //    }
        //    else if ((conductorInformation.Point2.X - conductorInformation.Point1.X) < 0 && (conductorInformation.Point2.Y - conductorInformation.Point1.Y) < 0)
        //    {
        //        //ed.writeMessage("Part three \n");
        //        point = new Point3d(conductorInformation.Point2.X + centerX, conductorInformation.Point2.Y + centerY, 0);
        //    }
        //    else
        //    {
        //        //ed.writeMessage("Part four \n");
        //        point = new Point3d(conductorInformation.Point2.X + centerX, conductorInformation.Point2.Y - centerY, 0);
        //    }
        //    return point;
        //}

        //private static void WriteNote(string Text, Point3d Position)
        //{
        //    Database db = HostApplicationServices.WorkingDatabase;
        //    Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;

        //    using (Transaction trans = Application.DocumentManager.MdiActiveDocument.TransactionManager.StartTransaction())
        //    {
        //        BlockTable bt = (BlockTable)trans.GetObject(db.BlockTableId, OpenMode.ForRead);
        //        BlockTableRecord ms = (BlockTableRecord)trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);

        //        DBText dbText = new DBText();
        //        dbText.Position = Position;
        //        dbText.TextString = Text;
        //        dbText.Height = 0.2;
        //        //ed.writeMessage(string.Format("Angle is : {0} \n", ((Math.Atan((conductorInformation.Point1.Y - conductorInformation.Point2.Y) / (conductorInformation.Point1.X - conductorInformation.Point2.X))) * Math.PI) / 180));
        //        dbText.Rotation = ((Math.Atan((conductorInformation.Point1.Y - conductorInformation.Point2.Y) / (conductorInformation.Point1.X - conductorInformation.Point2.X))) * Math.PI) / 180;
        //        ms.AppendEntity(dbText);
        //        trans.AddNewlyCreatedDBObject(dbText, true);

        //        trans.Commit();
        //    }
        //}

        public void CreatePalleteOne()
        {
            //Autodesk.AutoCAD.Windows.PaletteSet ps = Atend.Control.Common.ps;
            Atend.Control.Common.ps = new Autodesk.AutoCAD.Windows.PaletteSet("َAtend PaletteSet");
            Atend.Control.Common.ps.Style = Autodesk.AutoCAD.Windows.PaletteSetStyles.ShowTabForSingle |
                Autodesk.AutoCAD.Windows.PaletteSetStyles.NameEditable |
                //Autodesk.AutoCAD.Windows.PaletteSetStyles.ShowPropertiesMenu |
                Autodesk.AutoCAD.Windows.PaletteSetStyles.ShowAutoHideButton |
                Autodesk.AutoCAD.Windows.PaletteSetStyles.ShowCloseButton;
            Atend.Control.Common.ps.Dock = Autodesk.AutoCAD.Windows.DockSides.Left;
            Atend.Control.Common.ps.Add("مدیریت تجهیزات", new ucProduct02());
            //ps.Add("مدیریت تجهیزات", new ucProduct());
            Atend.Control.Common.ps.Visible = true;

        }

        //public static Autodesk.AutoCAD.Windows.PaletteSet ps = null;

        //public static void ShowEquipmentPallete()
        //{

        //    if (ps == null)
        //    {
        //        ps = new Autodesk.AutoCAD.Windows.PaletteSet("َAtend PaletteSet");
        //        ps.Style = Autodesk.AutoCAD.Windows.PaletteSetStyles.ShowTabForSingle |
        //            Autodesk.AutoCAD.Windows.PaletteSetStyles.NameEditable |
        //            Autodesk.AutoCAD.Windows.PaletteSetStyles.ShowAutoHideButton |
        //            Autodesk.AutoCAD.Windows.PaletteSetStyles.ShowCloseButton;
        //        ps.Dock = Autodesk.AutoCAD.Windows.DockSides.Left;
        //        ps.Add("مدیریت تجهیزات", new ucProduct02());
        //        ps.Visible = true;
        //    }
        //    else
        //    {
        //        //ps.Visible = true;
        //    }



        //}

        public void ControlPaletteSet()
        {

        }

        private void CreateContext()
        {
            Autodesk.AutoCAD.Windows.ContextMenuExtension contextMenu1 = new Autodesk.AutoCAD.Windows.ContextMenuExtension();
            contextMenu1.Title = "محاسبات";
            Autodesk.AutoCAD.Windows.MenuItem menuitem1 = new Autodesk.AutoCAD.Windows.MenuItem("بارگذاری");
            menuitem1.Click += new EventHandler(menuItem2_Click);
            contextMenu1.MenuItems.Add(menuitem1);
            Application.AddDefaultContextMenuExtension(contextMenu1);
        }

        void ed_SelectionAdded(object sender, SelectionAddedEventArgs e)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            Autodesk.AutoCAD.EditorInput.SelectionSet ss = e.Selection;
            if (ss.Count > 0)
            {
                //ed.writeMessage(ss[0].ObjectId.ToString() + "\n");
            }
        }

        void menuitem1_Click(object sender, EventArgs e)
        {
            // throw new System.Exception("The method or operation is not implemented.");
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            //ed.writeMessage("context menu clicked  \n");

            Editor editor = Application.DocumentManager.MdiActiveDocument.Editor;
            PromptEntityResult promptEntity = editor.GetEntity("Select an entity: ");
            if (promptEntity.Status == PromptStatus.OK)
            {
                ed = Application.DocumentManager.MdiActiveDocument.Editor;
                //ed.writeMessage(promptEntity.ObjectId.ToString() + "\n");
            }
        }

        void menuItem2_Click(object sender, EventArgs e)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            PromptSelectionOptions promptSelectionOption = new PromptSelectionOptions();
            promptSelectionOption.MessageForAdding = "Select a bolck : ";
            PromptSelectionResult pSelectionResult;
            pSelectionResult = ed.GetSelection(promptSelectionOption);
            ////ed.writeMessage(pSelectionResult.Status.ToString());
            if (pSelectionResult.Status == PromptStatus.OK)
            {
                SelectionSet ss = pSelectionResult.Value;
                ObjectId[] o = ss.GetObjectIds();
                //ed.writeMessage(o[0].OldId.ToString() + "\n");
                //ed.writeMessage("block seleced \n");

                //after select
                //Atend.Control.Common.AutoCadId = o[0].OldId;
                //Atend.Design.frmLoad frmload = new Atend.Design.frmLoad();
                //frmload.ShowDialog();

            }
        }

        public void Terminate()
        {
            CustomObjectSnapMode.Deactivate("_Quarter");
        }

        //public void ImportBlocks()
        //{
        //    DocumentCollection dm = Application.DocumentManager;
        //    Editor ed = dm.MdiActiveDocument.Editor;
        //    Database destDb = dm.MdiActiveDocument.Database;
        //    Database sourceDb = new Database(false, true);
        //    try
        //    {

        //        // Get name of DWG from which to copy blocks
        //        // Read the DWG into a side database
        //        // Create a variable to store the list of block identifiers

        //        //findtab("ID_Equipments");

        //        System.Reflection.Module m = System.Reflection.Assembly.GetExecutingAssembly().GetModules()[0];
        //        string fullPath = m.FullyQualifiedName;
        //        try
        //        {
        //            fullPath = fullPath.Substring(0, fullPath.LastIndexOf('\\'));
        //        }
        //        catch
        //        {
        //        }

        //        fullPath = fullPath + "\\atend.dwg";
        //        Application.DocumentManager.MdiActiveDocument.Editor.WriteMessage(fullPath + "\n");


        //        //sourceFileName = ed.GetString("\nEnter the name of the source drawing: ");
        //        sourceDb.ReadDwgFile(fullPath, System.IO.FileShare.Read, true, "2009");
        //        ObjectIdCollection blockIds = new ObjectIdCollection();
        //        Autodesk.AutoCAD.DatabaseServices.TransactionManager tm = sourceDb.TransactionManager;
        //        using (Transaction myT = tm.StartTransaction())
        //        {
        //            // Open the block table
        //            // Check each block in the block table
        //            // No need to commit the transaction

        //            BlockTable bt = (BlockTable)tm.GetObject(sourceDb.BlockTableId, OpenMode.ForRead, false);
        //            foreach (ObjectId btrId in bt)
        //            {
        //                BlockTableRecord btr = (BlockTableRecord)tm.GetObject(btrId, OpenMode.ForWrite, false);
        //                if (!btr.IsAnonymous && !btr.IsLayout)
        //                {
        //                    // Only add named & non-layout blocks to the copy list
        //                    // Attribute Definition
        //                    AttributeDefinition text = new AttributeDefinition(new Point3d(0, 2, 0), "NoName", "Name:", "Enter Name", destDb.Textstyle);
        //                    text.ColorIndex = 0;
        //                    btr.AppendEntity(text);
        //                    myT.AddNewlyCreatedDBObject(text, true);
        //                    blockIds.Add(btrId);
        //                }
        //                btr.Dispose();
        //            }
        //            bt.Dispose();
        //            myT.Dispose();
        //        }
        //        // Copy blocks from source to destination database
        //        IdMapping mapping = new IdMapping();
        //        sourceDb.WblockCloneObjects(blockIds, destDb.BlockTableId, mapping, DuplicateRecordCloning.Replace, false);
        //        //ed.writeMessage("\nCopied " + blockIds.Count.ToString() + " block definitions from Source file to the current drawing.\n");
        //    }
        //    catch (Autodesk.AutoCAD.Runtime.Exception ex)
        //    {
        //        //ed.writeMessage("\nError during copy: " + ex.Message + "\n");
        //    }
        //    sourceDb.Dispose();
        //}



        //void rEditbutton_Click(object sender, System.Windows.RoutedEventArgs e)
        //{
        //    PromptEntityOptions entity = new PromptEntityOptions("لظفاً تجهیز مورد نظر را انتخاب نمایید");
        //    entity.AllowNone = false;

        //    PromptEntityResult result = ed.GetEntity(entity);
        //    //ed.writeMessage(result.Status.ToString());
        //    if (result.Status == PromptStatus.OK)
        //    {
        //        Atend.Control.Common.AutoCadId = Convert.ToInt64(result.ObjectId.ToString().Substring(1, result.ObjectId.ToString().Length - 2));
        //        if (Atend.Control.Common.SelectedDesignCode == 0)
        //        {
        //            System.Windows.Forms.MessageBox.Show("اطفاً ابتدا طرح مورد نظر را انتخاب نمایید");
        //            return;
        //        }
        //        else
        //        {
        //            Atend.Base.Design.DNode node = Atend.Base.Design.DNode.SelectByAutocadCodeDesignCode(
        //                Atend.Control.Common.AutoCadId, Atend.Control.Common.SelectedDesignCode);
        //            //ed.writeMessage("AutocadId = " + Atend.Control.Common.AutoCadId.ToString() + "\n");
        //            //ed.writeMessage("DesignCode = " + Atend.Control.Common.SelectedDesignCode.ToString() + "\n");
        //            if (node.Code != Guid.Empty)
        //            {
        //                //ed.writeMessage("ProductBlockCode = " + node.ProductBlockCode.ToString() + "\n");


        //                Atend.Base.Base.BProductBlock productBlock = Atend.Base.Base.BProductBlock.SelectByCode(
        //                    node.ProductBlockCode);
        //                //ed.writeMessage("productBlock.Type = " + productBlock.Type.ToString() + "\n");
        //                //ed.writeMessage("Atend.Control.Enum.ProductType.Pole = " + Atend.Control.Enum.ProductType.Pole.ToString() + "\n");
        //                if (productBlock.Type == (int)Atend.Control.Enum.ProductType.Pole)
        //                {
        //                    Point3d p = new Point3d(node.X, node.Y, 0);
        //                    Design.frmEditDrawPole01 frmdrawPole = new Atend.Design.frmEditDrawPole01(p);
        //                    frmdrawPole.ShowDialog();
        //                }
        //            }
        //            else
        //            {
        //                //ed.writeMessage("node Not Found \n");
        //                Atend.Base.Design.DBranch branch = Atend.Base.Design.DBranch.SelectByDesignCodeAutocadCode(
        //                    Atend.Control.Common.SelectedDesignCode, Atend.Control.Common.AutoCadId);


        //            }
        //        }
        //    }
        //    else
        //    {
        //        //ed.writeMessage("Entity not found \n");
        //    }
        //}

        void rd_ItemSelected(object sender, EventArgs e)
        {
            //throw new System.Exception("The method or operation is not implemented.");
            RibbonItem ribbonItem = sender as RibbonItem;
            Application.DocumentManager.MdiActiveDocument.Editor.WriteMessage(ribbonItem.Id + "\n");

        }

        //public static void btnProductBindBlock_Click(object sender, EventArgs e)
        //{
        //    Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
        //    DocumentLock dlock = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.LockDocument();
        //    Atend.Base.frmProdutBindBlock _frmProductBindBlock = new Atend.Base.frmProdutBindBlock();
        //    Application.ShowModalDialog(_frmProductBindBlock);
        //    dlock.Dispose();

        //}

        //public static void btnCounductor_Click(object sender, EventArgs e)
        //{
        //    Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
        //    Atend.Control.Common.EquipmentType = Atend.Control.Enum.EquipmentType.Branch;
        //    //ed.writeMessage("ترسیم سیم" + "\n");
        //    PromptEntityOptions promptEntity1 = new PromptEntityOptions("لطفا گره اول را انتخاب کنید :");
        //    PromptEntityResult entityResult;
        //    promptEntity1.AllowNone = true;
        //    try
        //    {
        //        entityResult = ed.GetEntity(promptEntity1);
        //        if (entityResult.Status == PromptStatus.OK)
        //        {
        //            //ed.writeMessage(string.Format("شماره شناسایی گره انتخاب شده: {0} \n", entityResult.ObjectId.ToString()));

        //            conductorInformation.Point1 = entityResult.PickedPoint;
        //            //ed.writeMessage("get point : " + conductorInformation.Point1.ToString() + "\n");
        //            conductorInformation.AutocadId1 = entityResult.ObjectId.ToString().Substring(1, entityResult.ObjectId.ToString().Length - 2);
        //            //ed.writeMessage("get autoId : " + conductorInformation.AutocadId1.ToString() + "\n");

        //            PromptEntityOptions promptEntity2 = new PromptEntityOptions("لطفا گره دوم را انتخاب کنید :");
        //            promptEntity1.AllowNone = true;
        //            entityResult = ed.GetEntity(promptEntity2);
        //            if (entityResult.Status == PromptStatus.OK)
        //            {
        //                //ed.writeMessage(string.Format("شماره شناسایی گره انتخاب شده: {0} \n", entityResult.ObjectId.ToString()));
        //                //ed.writeMessage(string.Format("{0},{1} \n", entityResult.PickedPoint.X, entityResult.PickedPoint.Y));


        //                conductorInformation.Point2 = entityResult.PickedPoint;
        //                //ed.writeMessage("get point : " + conductorInformation.Point2.ToString() + "\n");
        //                conductorInformation.AutocadId2 = entityResult.ObjectId.ToString().Substring(1, entityResult.ObjectId.ToString().Length - 2);
        //                //ed.writeMessage("get autoId : " + conductorInformation.AutocadId2.ToString() + "\n");


        //                Atend.Design.frmDrawBranch01 fDrawBranch = new Atend.Design.frmDrawBranch01();
        //                if (fDrawBranch.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        //                {
        //                    using (DocumentLock docLock = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.LockDocument())
        //                    {
        //                        //ادامه عملیات ترسیم سیم
        //                        //ed.writeMessage("go to draw conductor \n");
        //                        Atend.Menu.drawLine("Line01");
        //                    }
        //                }
        //                else
        //                {
        //                    //قطع عملیات ترسیم سیم

        //                }
        //            }
        //            else
        //            {
        //                //endtityresult is not OK
        //            }
        //        }
        //        else
        //        {
        //            //endtityresult is not OK
        //        }
        //    }
        //    catch
        //    {
        //    }
        //}

        //public static void btnPole_Click(object sender, EventArgs e)
        //{
        //    Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
        //    PromptPointOptions p = new PromptPointOptions("لطفا نقطه کاشت پایه راانتخاب نمایید:");
        //    PromptPointResult r = ed.GetPoint(p);
        //    if (r.Status == PromptStatus.OK)
        //    {
        //        //ed.writeMessage(r.Value.ToString()+"\n");
        //        Design.frmDrawPole01 frmdrawPole = new Atend.Design.frmDrawPole01(r.Value);
        //        frmdrawPole.ShowDialog();

        //    }


        //}

        //public static void btnProductBindBlock1_Click(object sender, EventArgs e)
        //{
        //    DocumentLock dlock = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.LockDocument();
        //    Atend.Design.frmDesignSearch _frmdesignSearch = new Atend.Design.frmDesignSearch();
        //    Application.ShowModalDialog(_frmdesignSearch);
        //    dlock.Dispose();
        //}

        //public static void btnProductBindBlock2_Click(object sender, EventArgs e)
        //{
        //    //DocumentLock dlock = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.LockDocument();
        //    Atend.Design.frmDesignSave _frmdesignSave = new Atend.Design.frmDesignSave();
        //    Application.ShowModalDialog(_frmdesignSave);
        //    //dlock.Dispose();
        //}

        //public static void btnDefineUser_Click(object sender, EventArgs e)
        //{
        //    Atend.Design.frmUser FrmDesign = new Atend.Design.frmUser();
        //    FrmDesign.ShowDialog();
        //}

        //public static void btnWeatherCondition_Click(object sender, EventArgs e)
        //{
        //    Atend.Design.frmWeather FrmWeather = new Atend.Design.frmWeather();
        //    Application.ShowModalDialog(FrmWeather);
        //}

        /// <summary>
        /// tarsim block dar safhe
        /// 
        /// </summary>
        /// <param name="EquipName"></param>

        //public static void drawOnScreen(String EquipName)
        //{
        //    Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
        //    Database db = HostApplicationServices.WorkingDatabase;
        //    Transaction tr = db.TransactionManager.StartTransaction();

        //    BlockTable bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead);
        //    BlockTableRecord btr = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);

        //    ObjectId bID;
        //    ////ed.writeMessage("Go for find equipment Name" + " \n");
        //    if (bt.Has(EquipName))
        //    {
        //        bID = bt[EquipName];
        //        //ed.writeMessage("بلاک مورد نظر یافت شد" + " \n");
        //        try
        //        {
        //            Autodesk.AutoCAD.EditorInput.PromptPointOptions pPointOption = new PromptPointOptions("محل قرار گیری تجهیز را مشخص نمایید: " + "\n");
        //            PromptPointResult pPointResult;
        //            pPointResult = ed.GetPoint(pPointOption);
        //            if (pPointResult.Status == PromptStatus.OK)
        //            {
        //                //MText _text = new MText();
        //                //_text.Text = "";

        //                BlockReference br = new BlockReference(pPointResult.Value, bID);
        //                //ed.writeMessage("BLOCK REFERENCE CREATED \n");



        //                btr.AppendEntity(br);
        //                //ed.writeMessage("BLOCK APPENDED\n");

        //                tr.AddNewlyCreatedDBObject(br, true);
        //                //ed.writeMessage("TRANSACTION \n");

        //                //after change
        //                //Atend.Control.Common.AutoCadId = Convert.ToInt64(br.Id.ToString().Substring(1, br.Id.ToString().Length - 2));
        //                //ed.writeMessage("get cad id " + br.Id.ToString().Substring(1, br.Id.ToString().Length - 2) + " \n");
        //                point1 = new Point2d(pPointResult.Value.X, pPointResult.Value.Y);

        //                tr.Commit();


        //                // add XData To blockreference
        //                AddXData(br);

        //                // end of XData to add 

        //                if (!db_ObjectAppended())
        //                {
        //                    //ed.writeMessage("کاربر محترم امکان ثبت تجهیز مورد نظر وجود ندارد" + "\n");
        //                }
        //            }
        //        }
        //        catch (System.Exception ex1)
        //        {
        //            //ed.writeMessage(ex1.Message + "\n");
        //            //ed.writeMessage("Error Occured in Block Table Record opening \n");
        //        }
        //    }
        //}

        //public static void drawOnScreen(String EquipName, Point3d myPoint)
        //{
        //    Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
        //    Database db = HostApplicationServices.WorkingDatabase;
        //    Transaction tr = db.TransactionManager.StartTransaction();

        //    BlockTable bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead);
        //    BlockTableRecord btr = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);

        //    ObjectId bID;


        //    switch (Atend.Control.Common.EquipmentType)
        //    {
        //        case Atend.Control.Enum.EquipmentType.Branch:
        //            break;
        //        case Atend.Control.Enum.EquipmentType.Jumper:
        //            break;
        //        case Atend.Control.Enum.EquipmentType.Node:
        //            //ed.writeMessage("i am in node switch in draw on screen \n");
        //            if (bt.Has(EquipName))
        //            {
        //                bID = bt[EquipName];
        //                //ed.writeMessage("بلاک مورد نظر یافت شد" + " \n");
        //                try
        //                {
        //                    using (DocumentLock docLock = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.LockDocument())
        //                    {

        //                        BlockReference br = new BlockReference(myPoint, bID);
        //                        if (Atend.Control.Common.SelectedProductType == Atend.Control.Enum.ProductType.Pole)
        //                        {

        //                            //if (dPoleInfo.PoleType.ToString().IndexOf('2') != -1)
        //                            //{
        //                            //    br.Rotation = -90;
        //                            //}
        //                        }
        //                        //ed.writeMessage("BLOCK REFERENCE CREATED " + myPoint.X.ToString() + "---" + myPoint.Y.ToString() + " \n");

        //                        btr.AppendEntity(br);
        //                        //ed.writeMessage("BLOCK APPENDED\n");

        //                        tr.AddNewlyCreatedDBObject(br, true);
        //                        //ed.writeMessage("TRANSACTION \n");


        //                        //after change
        //                        //Atend.Control.Common.AutoCadId = Convert.ToInt64(br.Id.ToString().Substring(1, br.Id.ToString().Length - 2));
        //                        nodeInformation.AutoCadId = br.Id.ToString().Substring(1, br.Id.ToString().Length - 2);
        //                        //ed.writeMessage("The Number Of Created Equip Is : " + br.Id.ToString().Substring(1, br.Id.ToString().Length - 2) + " \n");

        //                        tr.Commit();
        //                        //Atend.Base.Base.BProductBlock ProductBlock = Atend.Base.Base.BProductBlock.SelectByCode((int)nodeInformation.ProductBlockId);
        //                        //Atend.Base.Base.BProduct product = Atend.Base.Base.BProduct.Select_ByCode(ProductBlock.ProductID);

        //                        WriteNote(Comment, new Point3d(myPoint.X + 2, myPoint.Y + 2, 0));
        //                        if (!db_ObjectAppended())
        //                        {
        //                            //ed.writeMessage("کاربر محترم امکان ثبت تجهیز مورد نظر وجود ندارد" + "\n");
        //                        }
        //                    }
        //                }
        //                catch (System.Exception ex1)
        //                {
        //                    //ed.writeMessage("Error Occured In Block Reference Creation " + ex1.Message + " \n");
        //                }
        //            }
        //            break;
        //        case Atend.Control.Enum.EquipmentType.Other:
        //            break;
        //    }
        //}

        //public static void drawOnScreen(String EquipName, Point3d myPoint, Atend.Base.Design.DPoleInfo myPoleInfo)
        //{
        //    Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
        //    Database db = HostApplicationServices.WorkingDatabase;
        //    Transaction tr = db.TransactionManager.StartTransaction();

        //    BlockTable bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead);
        //    BlockTableRecord btr = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);

        //    ObjectId bID;


        //    switch (Atend.Control.Common.EquipmentType)
        //    {
        //        case Atend.Control.Enum.EquipmentType.Branch:
        //            break;
        //        case Atend.Control.Enum.EquipmentType.Jumper:
        //            break;
        //        case Atend.Control.Enum.EquipmentType.Node:
        //            if (bt.Has(nodeInformation.ProductBlockName))
        //            {
        //                bID = bt[EquipName];
        //                //ed.writeMessage("بلاک مورد نظر یافت شد" + " \n");
        //                try
        //                {
        //                    using (DocumentLock docLock = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.LockDocument())
        //                    {
        //                        BlockReference br = new BlockReference(myPoint, bID);
        //                        if (Atend.Control.Common.SelectedProductType == Atend.Control.Enum.ProductType.Pole)
        //                        {
        //                            if (myPoleInfo.PoleType.ToString().IndexOf('2') != -1)
        //                            {
        //                                br.Rotation = -90;
        //                            }
        //                        }
        //                        //ed.writeMessage("BLOCK REFERENCE CREATED " + myPoint.X.ToString() + "---" + myPoint.Y.ToString() + " \n");

        //                        btr.AppendEntity(br);
        //                        //ed.writeMessage("BLOCK APPENDED\n");

        //                        tr.AddNewlyCreatedDBObject(br, true);
        //                        //ed.writeMessage("TRANSACTION \n");

        //                        //after change
        //                        //Atend.Control.Common.AutoCadId = Convert.ToInt64(br.Id.ToString().Substring(1, br.Id.ToString().Length - 2));
        //                        nodeInformation.AutoCadId = br.Id.ToString().Substring(1, br.Id.ToString().Length - 2);
        //                        //ed.writeMessage("The Number Of Created Equip Is : " + br.Id.ToString().Substring(1, br.Id.ToString().Length - 2) + " \n");

        //                        tr.Commit();
        //                        //Atend.Base.Base.BProductBlock ProductBlock = Atend.Base.Base.BProductBlock.SelectByCode((int)nodeInformation.ProductBlockId);
        //                        //Atend.Base.Base.BProduct product = Atend.Base.Base.BProduct.Select_ByCode(ProductBlock.ProductID);

        //                        WriteNote(Comment, new Point3d(myPoint.X + 2, myPoint.Y + 2, 0));
        //                        if (!db_ObjectAppended())
        //                        {
        //                            //ed.writeMessage("کاربر محترم امکان ثبت تجهیز مورد نظر وجود ندارد" + "\n");
        //                        }
        //                    }
        //                }
        //                catch (System.Exception ex1)
        //                {
        //                    ed.WriteMessage("Error Occured In Block Reference Creation " + ex1.Message + " \n");
        //                }
        //            }
        //            break;
        //        case Atend.Control.Enum.EquipmentType.Other:
        //            break;
        //    }
        //}

        static bool db_ObjectAppended()
        {
            //Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            //ed.WriteMessage("i am in db_objectAppended \n");

            //switch (Atend.Control.Common.EquipmentType)
            //{
            //    //////case Atend.Control.Enum.EquipmentType.Branch:
            //    //////    Atend.Base.Design.CoductorTransaction conductorTransaction = new Atend.Base.Design.CoductorTransaction();
            //    //////    conductorTransaction.conductorInformation = conductorInformation;
            //    //////    if (conductorTransaction.Insert())
            //    //////    {
            //    //////        //ثبت هادی به درستی انجام شد
            //    //////        ed.WriteMessage(string.Format("هادی به  درستی ثبت شد"));
            //    //////    }
            //    //////    else
            //    //////    {
            //    //////        //ثبت هادب به درستی انجام نشد
            //    //////        ed.WriteMessage(string.Format("هادی به  درستی ثبت نشد"));
            //    //////    }
            //    //////    break;
            //    case Atend.Control.Enum.EquipmentType.Jumper:
            //        Atend.Base.Design.JumperTransaction jumperTransaction = new Atend.Base.Design.JumperTransaction();
            //        break;
            //    case Atend.Control.Enum.EquipmentType.Node:
            //        ed.WriteMessage("i am in node switch \n");

            //        Atend.Base.Design.NodeTransaction nodeTransaction = new Atend.Base.Design.NodeTransaction();
            //        ed.WriteMessage("i am going to nodeTransaction \n");
            //        if (nodeTransaction.Insert())
            //        {
            //            //اطلاعات با موفقیت ثبت شد

            //        }
            //        else
            //        {
            //            // خطا در زمان ثبت اطلاعات
            //        }
            //        break;
            //}
            return true;

        }

        static void db_ObjectErased(object sender, ObjectErasedEventArgs e)
        {
            ////////Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            ////////try
            ////////{
            ////////    if (e.Erased)
            ////////    {
            ////////        //after change
            ////////        //Atend.Control.Common.AutoCadId = Convert.ToInt64(e.DBObject.ObjectId.ToString().Substring(1, e.DBObject.Id.ToString().Length - 2));
            ////////        ed.WriteMessage("~~~~~~~ Delete Process Start ~~~~~~~ \n");


            ////////        //after change
            ////////        //ed.WriteMessage("ٍErased Object ID : " + Atend.Control.Common.AutoCadId.ToString() + "\n");

            ////////        //after change
            ////////        //Atend.Control.Common.EquipmentType = DetermineEquipmentType(Atend.Control.Common.AutoCadId, Atend.Control.Common.SelectedDesignCode);

            ////////        if (Atend.Control.Common.EquipmentType != Atend.Control.Enum.EquipmentType.Other)
            ////////        {
            ////////            switch (Atend.Control.Common.EquipmentType)
            ////////            {
            ////////                case Atend.Control.Enum.EquipmentType.Branch:
            ////////                    if (Atend.Base.Design.CoductorTransaction.Delete())
            ////////                    {
            ////////                        ed.WriteMessage("All Branch information removed done \n");
            ////////                    }
            ////////                    else
            ////////                    {
            ////////                        throw new System.Exception("while deleting branch information ");
            ////////                    }
            ////////                    break;
            ////////                case Atend.Control.Enum.EquipmentType.Node:
            ////////                    if (Atend.Base.Design.NodeTransaction.Delete())
            ////////                    {
            ////////                        ed.WriteMessage("All Node Information deleted successfully \n");
            ////////                    }
            ////////                    else
            ////////                    {
            ////////                        throw new System.Exception("while deleting node information");
            ////////                    }
            ////////                    break;
            ////////            }
            ////////        }
            ////////        ed.WriteMessage("~~~~~~~ Delete Process Ended ~~~~~~~\n");

            ////////    }
            ////////    else
            ////////    {
            ////////        // Error occured while object was wrased from autocad database
            ////////    }
            ////////}
            ////////catch (System.Exception ex1)
            ////////{
            ////////    ed.WriteMessage("Error Deleting Record " + ex1.Message + " \n");
            ////////}
        }

        //public static Atend.Control.Enum.EquipmentType DetermineEquipmentType(long AutoCadId, int DesignCode)
        //{
        //    Atend.Control.Enum.EquipmentType returnEquipmentType;
        //    Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;

        //    ed.WriteMessage("i am Determining Equipment Type \n");
        //    if (Atend.Base.Design.DNode.IsExist(Atend.Control.Common.SelectedAutocadObjectGuid, Atend.Control.Common.SelectedDesignCode))
        //    {
        //        returnEquipmentType = Atend.Control.Enum.EquipmentType.Node;
        //    }
        //    else if (Atend.Base.Design.DBranch.IsExist(Atend.Control.Common.SelectedDesignCode, Atend.Control.Common.SelectedAutocadObjectGuid))
        //    {
        //        returnEquipmentType = Atend.Control.Enum.EquipmentType.Branch;
        //    }
        //    else
        //    {
        //        returnEquipmentType = Atend.Control.Enum.EquipmentType.Other;
        //    }
        //    ed.WriteMessage(string.Format("Deleted Equipment Type : {0} \n", returnEquipmentType));
        //    return returnEquipmentType;
        //}

        public void CreateComplexLinetype()
        {

            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;


            Transaction tr = db.TransactionManager.StartTransaction();

            using (tr)
            {

                TextStyleTable tt = (TextStyleTable)tr.GetObject(db.TextStyleTableId, OpenMode.ForRead);

                // Get the linetype table from the drawing

                LinetypeTable lt = (LinetypeTable)tr.GetObject(db.LinetypeTableId, OpenMode.ForWrite);

                // Create our new linetype table record...

                LinetypeTableRecord ltr = new LinetypeTableRecord();

                // ... and set its properties

                ltr.Name = "COLD_WATER_SUPPLY";
                ltr.AsciiDescription = "Cold water supply ---- CW ---- CW ---- CW ----";

                ltr.PatternLength = 0.9;
                ltr.NumDashes = 3;

                // Dash #1

                ltr.SetDashLengthAt(0, 0.5);

                // Dash #2

                ltr.SetDashLengthAt(1, -0.2);

                ltr.SetShapeStyleAt(1, tt["Standard"]);

                ltr.SetShapeNumberAt(1, 0);

                ltr.SetShapeOffsetAt(1, new Vector2d(-0.1, -0.05));

                ltr.SetShapeScaleAt(1, 0.1);

                ltr.SetShapeIsUcsOrientedAt(1, false);

                ltr.SetShapeRotationAt(1, 0);

                ltr.SetTextAt(1, "[]");

                // Dash #3

                ltr.SetDashLengthAt(2, -0.2);

                // Add the new linetype to the linetype table

                ObjectId ltId = lt.Add(ltr);

                tr.AddNewlyCreatedDBObject(ltr, true);

                tr.Commit();
            }


        }

        public void CreateComplexLinetype1()
        {

            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;
            Transaction tr = db.TransactionManager.StartTransaction();

            using (tr)
            {

                TextStyleTable tt = (TextStyleTable)tr.GetObject(db.TextStyleTableId, OpenMode.ForRead);
                LinetypeTable lt = (LinetypeTable)tr.GetObject(db.LinetypeTableId, OpenMode.ForWrite);
                LinetypeTableRecord ltr = new LinetypeTableRecord();

                ltr.Name = "line02";
                ltr.AsciiDescription = "کابل فشارضعیف و فشارمتوسط پیشنهادی ---  ---  ---  ---";
                ltr.PatternLength = 1.0;
                ltr.NumDashes = 3;
                // dash #1
                ltr.SetDashLengthAt(0, 0.5);
                // dash #2
                ltr.SetDashLengthAt(1, -0.2);
                ltr.SetShapeStyleAt(1, tt["standard"]);
                ltr.SetShapeNumberAt(1, 0);
                ltr.SetShapeOffsetAt(1, new Vector2d(-0.1, -0.05));
                ltr.SetShapeScaleAt(1, 0.1);
                ltr.SetShapeIsUcsOrientedAt(1, false);
                ltr.SetShapeRotationAt(1, 0);
                ltr.SetTextAt(1, "A");
                // dash #3
                ltr.SetDashLengthAt(2, -0.2);
                // Add the new linetype to the linetype table
                ObjectId ltId = lt.Add(ltr);
                tr.AddNewlyCreatedDBObject(ltr, true);
                tr.Commit();
            }

        }

        public void line01()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;
            Transaction tr = db.TransactionManager.StartTransaction();

            using (tr)
            {

                TextStyleTable tt = (TextStyleTable)tr.GetObject(db.TextStyleTableId, OpenMode.ForRead);
                LinetypeTable lt = (LinetypeTable)tr.GetObject(db.LinetypeTableId, OpenMode.ForWrite);
                LinetypeTableRecord ltr = new LinetypeTableRecord();

                ltr.Name = "line01";
                ltr.AsciiDescription = "کابل فشارضعیف وفشارمتوسط موجود";
                ltr.PatternLength = 0.9;
                ltr.NumDashes = 3;
                // dash #1
                ltr.SetDashLengthAt(0, 0.5);
                // dash #2
                ltr.SetDashLengthAt(1, -0.2);
                ltr.SetShapeStyleAt(1, tt["standard"]);
                ltr.SetShapeNumberAt(1, 0);
                ltr.SetShapeOffsetAt(1, new Vector2d(-0.1, -0.05));
                ltr.SetShapeScaleAt(1, 0.1);
                ltr.SetShapeIsUcsOrientedAt(1, false);
                ltr.SetShapeRotationAt(1, 0);
                //ltr.SetTextAt(1, "");
                // dash #3
                ltr.SetDashLengthAt(2, -0.2);
                //Add the new linetype to the linetype table
                ObjectId ltId = lt.Add(ltr);
                tr.AddNewlyCreatedDBObject(ltr, true);
                tr.Commit();
            }
        }

        public void line02()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;
            Transaction tr = db.TransactionManager.StartTransaction();

            using (tr)
            {

                TextStyleTable tt = (TextStyleTable)tr.GetObject(db.TextStyleTableId, OpenMode.ForRead);
                LinetypeTable lt = (LinetypeTable)tr.GetObject(db.LinetypeTableId, OpenMode.ForWrite);
                LinetypeTableRecord ltr = new LinetypeTableRecord();

                ltr.Name = "line02";
                ltr.AsciiDescription = "کابل فشارضعیف وفشارمتوسط پیشنهادی";
                ltr.PatternLength = 0.9;
                ltr.NumDashes = 3;
                // dash #1
                ltr.SetDashLengthAt(0, 0.5);
                // dash #2
                ltr.SetDashLengthAt(1, -0.2);
                ltr.SetShapeStyleAt(1, tt["standard"]);
                ltr.SetShapeNumberAt(1, 0);
                ltr.SetShapeOffsetAt(1, new Vector2d(-0.1, -0.05));
                ltr.SetShapeScaleAt(1, 0.1);
                ltr.SetShapeIsUcsOrientedAt(1, false);
                ltr.SetShapeRotationAt(1, 0);
                ltr.SetTextAt(1, " ");
                // dash #3
                ltr.SetDashLengthAt(2, -0.2);
                // Add the new linetype to the linetype table
                ObjectId ltId = lt.Add(ltr);
                tr.AddNewlyCreatedDBObject(ltr, true);
                tr.Commit();
            }
        }

        public void line03()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;
            Transaction tr = db.TransactionManager.StartTransaction();

            using (tr)
            {

                TextStyleTable tt = (TextStyleTable)tr.GetObject(db.TextStyleTableId, OpenMode.ForRead);
                LinetypeTable lt = (LinetypeTable)tr.GetObject(db.LinetypeTableId, OpenMode.ForWrite);
                LinetypeTableRecord ltr = new LinetypeTableRecord();

                ltr.Name = "line03";
                ltr.AsciiDescription = "کابل برگردان فشارضعیف و فشارمتوسط";
                ltr.PatternLength = 0.9;
                ltr.NumDashes = 3;
                // dash #1
                ltr.SetDashLengthAt(0, 0.5);
                // dash #2
                ltr.SetDashLengthAt(1, -0.2);
                ltr.SetShapeStyleAt(1, tt["standard"]);
                ltr.SetShapeNumberAt(1, 0);
                ltr.SetShapeOffsetAt(1, new Vector2d(-0.1, -0.05));
                ltr.SetShapeScaleAt(1, 0.1);
                ltr.SetShapeIsUcsOrientedAt(1, false);
                ltr.SetShapeRotationAt(1, 0);
                ltr.SetTextAt(1, ".");
                // dash #3
                ltr.SetDashLengthAt(2, -0.2);
                // Add the new linetype to the linetype table
                ObjectId ltId = lt.Add(ltr);
                tr.AddNewlyCreatedDBObject(ltr, true);
                tr.Commit();
            }
        }

        public void line04()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;
            Transaction tr = db.TransactionManager.StartTransaction();

            using (tr)
            {

                TextStyleTable tt = (TextStyleTable)tr.GetObject(db.TextStyleTableId, OpenMode.ForRead);
                LinetypeTable lt = (LinetypeTable)tr.GetObject(db.LinetypeTableId, OpenMode.ForWrite);
                LinetypeTableRecord ltr = new LinetypeTableRecord();

                ltr.Name = "line04";
                ltr.AsciiDescription = "خط هوائی فشارضعیف و فشارمتوسط تکمدار موجود";
                ltr.PatternLength = 1.0;
                ltr.NumDashes = 3;
                // dash #1
                ltr.SetDashLengthAt(0, 0.5);
                // dash #2
                ltr.SetDashLengthAt(1, -0.2);
                ltr.SetShapeStyleAt(1, tt["standard"]);
                ltr.SetShapeNumberAt(1, 0);
                ltr.SetShapeOffsetAt(1, new Vector2d(-0.1, -0.05));
                ltr.SetShapeScaleAt(1, 0.1);
                ltr.SetShapeIsUcsOrientedAt(1, false);
                ltr.SetShapeRotationAt(1, 0);
                ltr.SetTextAt(1, "A");
                // dash #3
                ltr.SetDashLengthAt(2, -0.2);
                // Add the new linetype to the linetype table
                ObjectId ltId = lt.Add(ltr);
                tr.AddNewlyCreatedDBObject(ltr, true);
                tr.Commit();
            }
        }

        public void line05()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;
            Transaction tr = db.TransactionManager.StartTransaction();

            using (tr)
            {

                TextStyleTable tt = (TextStyleTable)tr.GetObject(db.TextStyleTableId, OpenMode.ForRead);
                LinetypeTable lt = (LinetypeTable)tr.GetObject(db.LinetypeTableId, OpenMode.ForWrite);
                LinetypeTableRecord ltr = new LinetypeTableRecord();

                ltr.Name = "line05";
                ltr.AsciiDescription = "خط هوائی فشارضعیف وفشارمتوسط تکمدار پیشنهادی";
                ltr.PatternLength = 1.0;
                ltr.NumDashes = 3;
                // dash #1
                ltr.SetDashLengthAt(0, 0.5);
                // dash #2
                ltr.SetDashLengthAt(1, -0.2);
                ltr.SetShapeStyleAt(1, tt["standard"]);
                ltr.SetShapeNumberAt(1, 0);
                ltr.SetShapeOffsetAt(1, new Vector2d(-0.1, -0.05));
                ltr.SetShapeScaleAt(1, 0.1);
                ltr.SetShapeIsUcsOrientedAt(1, false);
                ltr.SetShapeRotationAt(1, 0);
                ltr.SetTextAt(1, "A");
                // dash #3
                ltr.SetDashLengthAt(2, -0.2);
                // Add the new linetype to the linetype table
                ObjectId ltId = lt.Add(ltr);
                tr.AddNewlyCreatedDBObject(ltr, true);
                tr.Commit();
            }
        }

        //public static Point3d myGetEntity()
        //{
        //    Editor ed = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor;

        //    PromptEntityOptions promptEntity = new PromptEntityOptions("لطفا گره مورد نظر را انتخاب کنید :");
        //    PromptEntityResult entityResult = null;
        //    promptEntity.AllowNone = false;
        //    //ed.writeMessage("Enter to draw get point \n");
        //    bool AskAgain = true;

        //    while (AskAgain)
        //    {
        //        try
        //        {
        //            entityResult = ed.GetEntity(promptEntity);
        //            if (entityResult.Status == PromptStatus.OK)
        //            {

        //                //ed.writeMessage(string.Format("شماره شناسایی گره انتخاب شده: {0} \n", entityResult.ObjectId.ToString()));
        //                AskAgain = false;
        //            }
        //            else
        //            {
        //                AskAgain = true;
        //            }
        //        }
        //        catch
        //        {
        //        }
        //    }





        //    if (id1 == null)
        //    {

        //        id1 = entityResult.ObjectId.ToString().Substring(1, entityResult.ObjectId.ToString().Length - 2);
        //        //ed.writeMessage("first selected is : " + id1 + "\n");
        //    }
        //    else
        //    {
        //        id2 = entityResult.ObjectId.ToString().Substring(1, entityResult.ObjectId.ToString().Length - 2);
        //        //ed.writeMessage("second selected is : " + id2 + "\n");
        //    }




        //    return entityResult.PickedPoint;

        //}

        // For draw conductor we will come here

        //public static void drawLine(string lineTypeName)
        //{

        //    ////////Document doc = Application.DocumentManager.MdiActiveDocument;
        //    ////////Database db = doc.Database;
        //    ////////Editor ed = doc.Editor;
        //    ////////Transaction tr = db.TransactionManager.StartTransaction();
        //    ////////ed.WriteMessage("Enter to DrawLine \n");

        //    ////////// Create a line with this linetype
        //    ////////BlockTable bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead);
        //    ////////BlockTableRecord btr = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);
        //    ////////AttributeReference attRef = new AttributeReference();
        //    //////////////////////////////////////////
        //    ////////ed.WriteMessage(string.Format("point1 : {0} point2: {1}", conductorInformation.Point1.X.ToString(), conductorInformation.Point2.X.ToString()));
        //    ////////Line ln = new Line(conductorInformation.Point1, conductorInformation.Point2);
        //    ////////ObjectId ltId = ObjectId.Null;
        //    ////////LinetypeTable ltt = (LinetypeTable)tr.GetObject(db.LinetypeTableId, OpenMode.ForRead);
        //    ////////if (ltt.Has(lineTypeName))
        //    ////////{
        //    ////////    ltId = ltt[lineTypeName];

        //    ////////    ln.SetDatabaseDefaults(db);
        //    ////////    ln.LinetypeId = ltId;
        //    ////////    ObjectId objectID = btr.AppendEntity(ln);

        //    ////////    tr.AddNewlyCreatedDBObject(ln, true);
        //    ////////    Atend.Control.Common.AutoCadId = Convert.ToInt64(objectID.ToString().Substring(1, objectID.ToString().Length - 2));
        //    ////////    ed.WriteMessage("get cad id " + objectID.ToString().Substring(1, objectID.ToString().Length - 2) + " \n");

        //    ////////    tr.Commit();
        //    ////////    //double centerX = Math.Abs(conductorInformation.Point1.X - conductorInformation.Point2.X) / 2;
        //    ////////    //double centerY = Math.Abs(conductorInformation.Point1.Y - conductorInformation.Point2.Y) / 2;
        //    ////////    //ed.WriteMessage(string.Format("comment on coundoctur X: {0} Y: {1} \n", centerX, centerY));
        //    ////////    WriteNote("Branch Comment Here", ComputeCenterPoint());
        //    ////////    db_ObjectAppended();
        //    ////////}
        //    ////////else
        //    ////////{
        //    ////////    ed.WriteMessage("\n Line Type Which you seleced was not found. \n");
        //    ////////}
        //}

        //public static void drawLine(string lineTypeName, Atend.Control.Common.CounductorInformation _ConductorInformation)
        //{


        //    ////////Document doc = Application.DocumentManager.MdiActiveDocument;
        //    ////////Database db = doc.Database;
        //    ////////Editor ed = doc.Editor;
        //    ////////Transaction tr = db.TransactionManager.StartTransaction();
        //    ////////ed.WriteMessage("Enter to DrawLine \n");
        //    ////////// Create a test line with this linetype
        //    ////////BlockTable bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead);
        //    ////////BlockTableRecord btr = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);
        //    ////////conductorInformation = _ConductorInformation;

        //    //////////////////////////////////////////

        //    ////////Line ln = new Line(_ConductorInformation.Point1, _ConductorInformation.Point2);

        //    ////////ObjectId ltId = ObjectId.Null;
        //    ////////LinetypeTable ltt = (LinetypeTable)tr.GetObject(db.LinetypeTableId, OpenMode.ForRead);
        //    ////////if (ltt.Has(lineTypeName))
        //    ////////{
        //    ////////    ltId = ltt[lineTypeName];

        //    ////////    ln.SetDatabaseDefaults(db);
        //    ////////    ln.LinetypeId = ltId;
        //    ////////    ObjectId objectID = btr.AppendEntity(ln);
        //    ////////    tr.AddNewlyCreatedDBObject(ln, true);
        //    ////////    tr.Commit();

        //    ////////    Atend.Control.Common.AutoCadId = Convert.ToInt64(objectID.ToString().Substring(1, objectID.ToString().Length - 2));
        //    ////////    newid = objectID.ToString().Substring(1, objectID.ToString().Length - 2);
        //    ////////    ed.WriteMessage("New Line ID : " + newid + "\n");

        //    ////////    points.Add(_ConductorInformation.Point1);
        //    ////////    points.Add(_ConductorInformation.Point2);

        //    ////////    if (!db_ObjectAppended())
        //    ////////    {
        //    ////////        ed.WriteMessage("کاربر  محترم امکان ثبت اطلاعات ترسیم سیم موجود نمی باشد" + "\n");
        //    ////////    }
        //    ////////}
        //    ////////else
        //    ////////{
        //    ////////    ed.WriteMessage("\n Line Type Which you seleced was not found. \n");
        //    ////////}

        //}

        //public static void drawCurve()
        //{
        //    Editor ed = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor;
        //    //ed.writeMessage("\n");

        //    Autodesk.AutoCAD.ApplicationServices.Document doc = Application.DocumentManager.MdiActiveDocument;
        //    doc.SendStringToExecute("_PL ", true, false, true);


        //}

        //public static void AddXData(BlockReference blockReference)
        //{

        //    Database db = HostApplicationServices.WorkingDatabase;
        //    using (Transaction tr = db.TransactionManager.StartTransaction())
        //    {
        //        Xrecord xrec = new Xrecord();
        //        xrec.Data = new ResultBuffer(
        //            new TypedValue((int)DxfCode.Text, "Name"));
        //        blockReference.CreateExtensionDictionary();
        //        DBDictionary brExtDict = (DBDictionary)tr.GetObject(blockReference.ExtensionDictionary, OpenMode.ForWrite, false);
        //        brExtDict.SetAt("ObjectData", xrec);
        //        tr.AddNewlyCreatedDBObject(xrec, true);
        //    }

        //}

    }


    public class Commands
    {



        [CommandMethod("SaveActiveDrawing")]
        public static void SaveActiveDrawing()
        {
            Document acDoc = Application.DocumentManager.MdiActiveDocument;
            string strDWGName = acDoc.Name;
            object obj = Application.GetSystemVariable("DWGTITLED");
            // Check to see if the drawing has been named
            if (System.Convert.ToInt16(obj) == 0)
            {
                // If the drawing is using a default name (Drawing1, Drawing2, etc)
                // then provide a new name
                strDWGName = "c:\\MyDrawing.dwg";
            }
            // Save the active drawing
            acDoc.Database.SaveAs(strDWGName, true, DwgVersion.Current,
            acDoc.Database.SecurityParameters);
        }



        //[CommandMethod("LL")]
        //public void LoadLinetypes()
        //{
        //    Document doc = Application.DocumentManager.MdiActiveDocument;
        //    Database db = doc.Database;
        //    Editor ed = doc.Editor;
        //    const string filename = "acad.lin";
        //    try
        //    {
        //        string path = HostApplicationServices.Current.FindFile(filename, db, FindFileHint.EmbeddedImageFile);
        //        ed.WriteMessage("Path New={0}\n",path);
        //        db.LoadLineTypeFile("*", path);
        //    }
        //    catch (Autodesk.AutoCAD.Runtime.Exception ex)
        //    {
        //        if (ex.ErrorStatus == ErrorStatus.FilerError)

        //            ed.WriteMessage("\nCould not find file \"{0}\".", filename);
        //        else if (ex.ErrorStatus == ErrorStatus.DuplicateRecordName)
        //            ed.WriteMessage("\nCannot load already defined linetypes.");
        //        else
        //            ed.WriteMessage("\nException: {0}", ex.Message);
        //    }
        //}





        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

        [CommandMethod("CTWS")]
        static public void CreateTableWithStyleAndWhatStyle()
        {

            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;
            PromptPointResult pr = ed.GetPoint("\nEnter table insertion point: ");
            if (pr.Status == PromptStatus.OK)
            {
                Transaction tr = doc.TransactionManager.StartTransaction();
                using (tr)
                {
                    // First let us create our custom style,
                    //  if it doesn't exist
                    const string styleName = "Garish Table Style";
                    ObjectId tsId = ObjectId.Null;
                    DBDictionary sd = (DBDictionary)tr.GetObject(db.TableStyleDictionaryId, OpenMode.ForRead);
                    // Use the style if it already exists
                    if (sd.Contains(styleName))
                    {
                        tsId = sd.GetAt(styleName);
                    }
                    else
                    {
                        // Otherwise we have to create it
                        TableStyle ts = new TableStyle();
                        // Make the header area red
                        ts.SetBackgroundColor(Color.FromColorIndex(ColorMethod.ByAci, 1), (int)(RowType.TitleRow | RowType.HeaderRow));
                        // And the data area yellow
                        ts.SetBackgroundColor(Color.FromColorIndex(ColorMethod.ByAci, 2), (int)RowType.DataRow);
                        // With magenta text everywhere (yeuch :-)
                        ts.SetColor(Color.FromColorIndex(ColorMethod.ByAci, 6), (int)(RowType.TitleRow | RowType.HeaderRow | RowType.DataRow));
                        // And now with cyan outer grid-lines
                        ts.SetGridColor(Color.FromColorIndex(ColorMethod.ByAci, 4), (int)GridLineType.OuterGridLines, (int)(RowType.TitleRow | RowType.HeaderRow | RowType.DataRow));
                        // And bright green inner grid-lines
                        ts.SetGridColor(Color.FromColorIndex(ColorMethod.ByAci, 3), (int)GridLineType.InnerGridLines, (int)(RowType.TitleRow | RowType.HeaderRow | RowType.DataRow));
                        // And we'll make the grid-lines nice and chunky
                        ts.SetGridLineWeight(LineWeight.LineWeight000, (int)GridLineType.AllGridLines, (int)(RowType.TitleRow | RowType.HeaderRow | RowType.DataRow));
                        // Add our table style to the dictionary
                        //  and to the transaction
                        tsId = ts.PostTableStyleToDatabase(db, styleName);
                        tr.AddNewlyCreatedDBObject(ts, true);
                    }
                    BlockTable bt = (BlockTable)tr.GetObject(doc.Database.BlockTableId, OpenMode.ForRead);
                    Table tb = new Table();
                    tb.NumRows = 6;
                    tb.NumColumns = 3;
                    tb.SetRowHeight(3);
                    tb.SetColumnWidth(15);
                    tb.Position = pr.Value;
                    // Use our table style
                    if (tsId == ObjectId.Null)
                        // This should not happen, unless the
                        //  above logic changes
                        tb.TableStyle = db.Tablestyle;
                    else
                        tb.TableStyle = tsId;
                    // Create a 2-dimensional array
                    // of our table contents
                    string[,] str = new string[6, 3];
                    str[0, 0] = "Material Properties Table";
                    str[1, 0] = "Part No.";
                    str[1, 1] = "Name";
                    str[1, 2] = "Material";
                    str[2, 0] = "1876-1";
                    str[2, 1] = "Flange";
                    str[2, 2] = "Perspex";
                    str[3, 0] = "0985-4";
                    str[3, 1] = "Bolt";
                    str[3, 2] = "Steel";
                    str[4, 0] = "3476-K";
                    str[4, 1] = "Tile";
                    str[4, 2] = "Ceramic";
                    str[5, 0] = "8734-3";
                    str[5, 1] = "Kean";
                    str[5, 2] = "Mostly water";
                    // Use a nested loop to add and format each cell
                    for (int i = 0; i < 6; i++)
                    {
                        if (i == 0)
                        {
                            // This is for the title
                            tb.SetTextHeight(0, 0, 1);
                            tb.SetTextString(0, 0, str[0, 0]);
                            tb.SetAlignment(0, 0, CellAlignment.MiddleRight);
                        }

                        else
                        {

                            // These are the header and data rows



                            for (int j = 0; j < 3; j++)
                            {

                                tb.SetTextHeight(i, j, 1);

                                tb.SetTextString(i, j, str[i, j]);

                                tb.SetAlignment(i, j, CellAlignment.MiddleCenter);

                            }

                        }

                    }

                    tb.GenerateLayout();



                    BlockTableRecord btr =

                      (BlockTableRecord)tr.GetObject(

                        bt[BlockTableRecord.ModelSpace],

                        OpenMode.ForWrite

                      );

                    btr.AppendEntity(tb);

                    tr.AddNewlyCreatedDBObject(tb, true);

                    tr.Commit();

                }

            }

        }




        [CommandMethod("CRT")]
        static public void CreateTable()
        {

            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;
            PromptPointResult pr = ed.GetPoint("\nEnter table insertion point: ");
            if (pr.Status == PromptStatus.OK)
            {

                Table tb = new Table();
                //tb.TableStyle = db.Tablestyle;
                tb.NumRows = 5;
                tb.NumColumns = 3;
                tb.SetRowHeight(3);
                tb.SetColumnWidth(15);
                tb.Position = pr.Value;
                // Create a 2-dimensional array
                // of our table contents
                string[,] str = new string[5, 3];
                str[0, 0] = "Part No.";
                str[0, 1] = "Name ";
                str[0, 2] = "Material ";
                str[1, 0] = "1876-1";
                str[1, 1] = "Flange";
                str[1, 2] = "Perspex";
                str[2, 0] = "0985-4";
                str[2, 1] = "Bolt";
                str[2, 2] = "Steel";
                str[3, 0] = "3476-K";
                str[3, 1] = "Tile";
                str[3, 2] = "Ceramic";
                str[4, 0] = "8734-3";
                str[4, 1] = "Kean";
                str[4, 2] = "Mostly water";
                // Use a nested loop to add and format each cell
                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        tb.SetTextHeight(i, j, 1);
                        tb.SetTextString(i, j, str[i, j]);
                        tb.SetAlignment(i, j, CellAlignment.MiddleRight);
                    }
                }
                tb.GenerateLayout();
                Transaction tr = doc.TransactionManager.StartTransaction();
                using (tr)
                {
                    BlockTable bt = (BlockTable)tr.GetObject(doc.Database.BlockTableId, OpenMode.ForRead);
                    BlockTableRecord btr = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);
                    btr.AppendEntity(tb);
                    tr.AddNewlyCreatedDBObject(tb, true);
                    tr.Commit();
                }
            }
        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public class PointMonitorTooltips
        {

            [CommandMethod("SM")]
            public static void StartMonitor()
            {

                Editor ed =

                  Application.DocumentManager.MdiActiveDocument.Editor;

                ed.PointMonitor +=

                  new PointMonitorEventHandler(ed_PointMonitor);

            }

            [CommandMethod("XM")]
            public static void StopMonitor()
            {

                Editor ed =

                  Application.DocumentManager.MdiActiveDocument.Editor;

                ed.TurnForcedPickOn();

                ed.PointMonitor -=

                  new PointMonitorEventHandler(ed_PointMonitor);

            }

            static void ed_PointMonitor(object sender, PointMonitorEventArgs e)
            {
                Editor ed = (Editor)sender;
                Document doc = ed.Document;
                try
                {
                    FullSubentityPath[] paths = e.Context.GetPickedEntities();
                    string curveInfo = "";
                    Transaction tr = doc.TransactionManager.StartTransaction();
                    using (tr)
                    {
                        foreach (FullSubentityPath path in paths)
                        {
                            ObjectId[] ids = path.GetObjectIds();
                            if (ids.Length > 0)
                            {
                                ObjectId id = ids[ids.GetUpperBound(0)];
                                DBObject obj = tr.GetObject(id, OpenMode.ForRead);
                                if (obj != null)
                                {
                                    Curve cv = obj as Curve;
                                    if (cv != null)
                                    {
                                        double length = cv.GetDistanceAtParameter(cv.EndParam) - cv.GetDistanceAtParameter(cv.StartParam);
                                        curveInfo += obj.GetType().Name + "'s length: " + string.Format("{0:F}", length) + "\n";
                                    }
                                }
                            }
                        }
                        tr.Commit();
                    }
                    if (curveInfo != "")
                        e.AppendToolTipText(curveInfo);
                }
                catch
                {

                    // Not sure what we might get here, but not real action

                    // needed (worth adding an Exception parameter and a

                    // breakpoint, in case things need investigating).

                }

            }

        }
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


        [CommandMethod("aa")]
        public static void a()
        {
        }

        [CommandMethod("OpenDrawing")]

        public static void OpenDrawing()
        {

            string strFileName = @"D:\ATEND TEMP FILE\4\4.DWG";

            DocumentCollection acDocMgr = Application.DocumentManager;



            if (File.Exists(strFileName))
            {

                acDocMgr.Open(strFileName, false);

            }

            else
            {

                acDocMgr.MdiActiveDocument.Editor.WriteMessage("File " + strFileName +

                                                               " does not exist.");

            }

        }


        public class Fns
        {
            enum IncidenceType
            {
                ToLeft = 0,
                ToRight = 1,
                ToFront = 2,
                Unknown
            };

            static IncidenceType CurveIncidence(
              Curve cur,
              double param,
              Vector3d dir,
              Vector3d normal
            )
            {
                Vector3d deriv1 =
                  cur.GetFirstDerivative(param);
                if (deriv1.IsParallelTo(dir))
                {
                    // Need second degree analysis

                    Vector3d deriv2 =
                      cur.GetSecondDerivative(param);
                    if (deriv2.IsZeroLength() ||
                        deriv2.IsParallelTo(dir))
                        return IncidenceType.ToFront;
                    else
                        if (deriv2.CrossProduct(dir).
                            DotProduct(normal) < 0)
                            return IncidenceType.ToRight;
                        else
                            return IncidenceType.ToLeft;
                }

                if (deriv1.CrossProduct(dir).
                    DotProduct(normal) < 0)
                    return IncidenceType.ToLeft;
                else
                    return IncidenceType.ToRight;
            }

            static public bool IsInsideCurve(
              Curve cur,
              Point3d testPt
            )
            {
                if (!cur.Closed)
                    // Cannot be inside
                    return false;

                Polyline2d poly2d = cur as Polyline2d;
                if (poly2d != null &&
                    poly2d.PolyType != Poly2dType.SimplePoly)
                    // Not supported
                    return false;

                Point3d ptOnCurve =
                  cur.GetClosestPointTo(testPt, false);

                if (Tolerance.Equals(testPt, ptOnCurve))
                    return true;

                // Check it's planar

                Plane plane = cur.GetPlane();
                if (!cur.IsPlanar)
                    return false;

                // Make the test ray from the plane

                Vector3d normal = plane.Normal;
                Vector3d testVector =
                  normal.GetPerpendicularVector();
                Ray ray = new Ray();
                ray.BasePoint = testPt;
                ray.UnitDir = testVector;

                Point3dCollection intersectionPoints =
                  new Point3dCollection();

                // Fire the ray at the curve
                cur.IntersectWith(
                  ray,
                  Intersect.OnBothOperands,
                  intersectionPoints,
                  0, 0
                );

                ray.Dispose();
                int numberOfInters =
                  intersectionPoints.Count;
                if (numberOfInters == 0)
                    // Must be outside
                    return false;

                int nGlancingHits = 0;
                double epsilon = 2e-6; // (trust me on this)
                for (int i = 0; i < numberOfInters; i++)
                {
                    // Get the first point, and get its parameter
                    Point3d hitPt = intersectionPoints[i];
                    double hitParam =
                      cur.GetParameterAtPoint(hitPt);
                    double inParam = hitParam - epsilon;
                    double outParam = hitParam + epsilon;

                    IncidenceType inIncidence =
                      CurveIncidence(cur, inParam, testVector, normal);
                    IncidenceType outIncidence =
                      CurveIncidence(cur, outParam, testVector, normal);

                    if ((inIncidence == IncidenceType.ToRight &&
                        outIncidence == IncidenceType.ToLeft) ||
                        (inIncidence == IncidenceType.ToLeft &&
                        outIncidence == IncidenceType.ToRight))
                        nGlancingHits++;
                }
                return ((numberOfInters + nGlancingHits) % 2 == 1);
            }
        }

        // Get a vector in a random direction

        public Vector3d randomUnitVector(PlanarEntity pl)
        {
            // Create our random number generator

            System.Random ran = new System.Random();

            // First we get the absolute value
            // of our x and y coordinates

            double x = ran.NextDouble();
            double y = ran.NextDouble();

            // Then we negate them, half of the time

            if (ran.NextDouble() < 0.5)
                x = -x;
            if (ran.NextDouble() < 0.5)
                y = -y;

            // Create a 2D vector and return it as
            //  3D on our plane

            Vector2d v2 = new Vector2d(x, y);
            return new Vector3d(pl, v2);
        }

        // Allow tracing in 3 colours, depending on
        // whether the vector was accepted, rejected,
        // or superseded by a better one

        enum TraceType
        {
            Accepted = 0,
            Rejected = 1,
            Superseded = 2
        };

        void TraceSegment(
          Point3d start,
          Point3d end,
          TraceType type
        )
        {
            Editor ed =
              Application.DocumentManager.MdiActiveDocument.Editor;
            int vecCol = 0;
            switch (type)
            {
                case TraceType.Accepted:
                    vecCol = 3;
                    break;
                case TraceType.Rejected:
                    vecCol = 1;
                    break;
                case TraceType.Superseded:
                    vecCol = 2;
                    break;
            }
            Matrix3d trans =
              ed.CurrentUserCoordinateSystem.Inverse();
            ed.DrawVector(
              start.TransformBy(trans),
              end.TransformBy(trans),
              vecCol,
              false
            );
        }

        // Test whether a segment goes outside our boundary

        bool TestSegment(
          Curve cur,
          Point3d start,
          Vector3d vec
        )
        {
            // Test 10 points along the segment...

            // (This is inefficient, but it's not a problem for
            //  this application. Some of the redundant overhead
            //  of firing rays for each iteration could be factored
            //  out, among other enhancements, I expect.)

            bool result = true;
            for (int i = 1; i < 10; i++)
            {
                Point3d test = start + (vec * 0.1 * i);

                // Call into our IsInsideCurve library function,
                // "and"-ing the results

                result &=
                  Fns.IsInsideCurve(cur, test);
                if (!result)
                    break;
            }
            return result;
        }

        // For a particular boundary, get the next vertex on the
        // curve, found by firing a ray in a random direction

        Point3d nextBoundaryPoint(
          Curve cur,
          Point3d start,
          bool trace
        )
        {
            // Create and define our ray

            Ray ray = new Ray();
            ray.BasePoint = start;
            ray.UnitDir =
              randomUnitVector(cur.GetPlane());

            // Get the intersection points until we
            // have at least 2 returned
            // (will usually happen straightaway)

            Point3dCollection pts =
              new Point3dCollection();
            do
            {
                cur.IntersectWith(
                  ray,
                  Intersect.OnBothOperands,
                  pts,
                  0, 0
                );
                if (pts.Count < 2)
                {
                    ray.UnitDir =
                      randomUnitVector(cur.GetPlane());
                }
            }
            while (pts.Count < 2);

            ray.Dispose();
            // For each of the intersection points - which
            // are points elsewhere on the boundary - let's
            // check to make sure we don't have to leave the
            // area to reach them

            bool first = true;
            double nextLen = 0.0;
            Point3d nextPt = start;

            foreach (Point3d pt in pts)
            {
                // Get the distance between this intersection
                // and the last accepted point - both points
                // are on our ray

                Vector3d vec = pt - start;
                double len = vec.Length;

                // If the vector length is positive and either
                // the first to be a candidate or closer than
                // the previous one (we generally select the
                // closest non-zero option) then check it out
                // further

                if (len > Tolerance.Global.EqualVector &&
                    (first || len < nextLen))
                {
                    // Run our tests to make sure the segment is
                    // inside our boundary

                    if (TestSegment(cur, start, vec))
                    {
                        // Draw the previous segment before overwriting

                        if (trace)
                            TraceSegment(
                              start,
                              nextPt,
                              TraceType.Superseded
                            );

                        nextLen = len;
                        nextPt = pt;
                        first = false;
                    }
                    else
                        // This segment has been rejected,
                        // as it goes outside
                        if (trace)
                            TraceSegment(
                              start,
                              pt,
                              TraceType.Rejected
                            );
                }
            }

            // Draw our accepted segment and return it

            if (nextLen > Tolerance.Global.EqualVector)
            {
                if (trace)
                    TraceSegment(
                      start,
                      nextPt,
                      TraceType.Accepted
                    );
                return nextPt;
            }
            else
            {
                // If we didn't find a good segment, throw an
                // exception to be handled by the calling function
                ////////////throw new Exception(
                ////////////  ErrorStatus.PointNotOnEntity,
                ////////////  "Could not find another intersection point."
                ////////////);
            }
            return nextPt;
        }

        [CommandMethod("BOUNCE")]
        public void BounceHatch()
        {
            Document doc =
              Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;
            bool doTrace = false;



            // Get various bits of user input

            PromptEntityOptions peo =
              new PromptEntityOptions(
                "\nSelect point on closed loop: "
              );
            PromptEntityResult per =
              ed.GetEntity(peo);

            if (per.Status != PromptStatus.OK)
                return;

            PromptIntegerOptions pio =
              new PromptIntegerOptions(
                "\nEnter number of segments: "
                );
            pio.DefaultValue = 500;

            PromptIntegerResult pir =
              ed.GetInteger(pio);

            if (pir.Status != PromptStatus.OK)
                return;

            PromptKeywordOptions pko =
              new PromptKeywordOptions(
                "\nDisplay segment trace: "
              );

            pko.Keywords.Add("Yes");
            pko.Keywords.Add("No");
            pko.Keywords.Default = "Yes";

            PromptResult pkr =
              ed.GetKeywords(pko);

            if (pkr.Status != PromptStatus.OK)
                return;

            Transaction tr =
              db.TransactionManager.StartTransaction();
            using (tr)
            {
                // Check the selected object - make sure it's
                // a closed loop (could do some more checks here)

                DBObject obj =
                  tr.GetObject(per.ObjectId, OpenMode.ForRead);
                Curve cur = obj as Curve;
                if (cur == null)
                    ed.WriteMessage("\nThis is not a curve.");
                else
                {
                    if (!cur.Closed)
                        ed.WriteMessage("\nLoop is not closed.");
                    else
                    {
                        // Extract parameters from our user-input...

                        // A flag for our vector tracing
                        doTrace = (pkr.StringResult == "Yes");
                        // The number of segments
                        int numBounces = pir.Value;
                        // The first vertex of our path
                        Point3d latest =
                          per.PickedPoint.
                            TransformBy(ed.CurrentUserCoordinateSystem).
                              OrthoProject(cur.GetPlane());

                        ed.WriteMessage("latest:{0}", latest);
                        // Create our polyline path, adding   the
                        // initial vertex

                        Polyline path = new Polyline();
                        path.Normal = cur.GetPlane().Normal;

                        path.AddVertexAt(
                          0,
                          latest.Convert2d(cur.GetPlane()),
                          0.0, 0.0, 0.0
                        );

                        // For each segment, get the next vertex
                        // and add it to the path

                        int i = 1;
                        while (i <= numBounces)
                        {
                            try
                            {
                                Point3d next =
                                  nextBoundaryPoint(cur, latest, doTrace);
                                path.AddVertexAt(
                                  i++,
                                  next.Convert2d(cur.GetPlane()),
                                  0.0, 0.0, 0.0
                                );
                                latest = next;
                            }
                            catch //(Exception ex)
                            {
                                // If there's an exception we know about
                                // then ignore it and allow the loop to
                                // continue (we probably did not increment
                                // i in this case, as it will fail on
                                // nextBoundaryPoint)

                                ////////if (ex.ErrorStatus !=
                                ////////    ErrorStatus.PointNotOnEntity)
                                ////////    throw ex;
                            }
                        }

                        // Open the modelspace

                        BlockTable bt =
                          (BlockTable)
                            tr.GetObject(
                              db.BlockTableId,
                              OpenMode.ForRead
                            );
                        BlockTableRecord btr =
                          (BlockTableRecord)
                            tr.GetObject(
                              bt[BlockTableRecord.ModelSpace],
                              OpenMode.ForWrite
                            );

                        // We need to transform the path polyline so
                        // that it's over our boundary

                        path.TransformBy(
                          Matrix3d.Displacement(
                            cur.StartPoint - Point3d.Origin
                          )
                        );

                        // Add our path to the modelspace

                        btr.AppendEntity(path);
                        tr.AddNewlyCreatedDBObject(path, true);
                    }
                }

                // Commit, whether we added a path or not.

                tr.Commit();

                // If we're tracing, pause for user input
                // before regenerating the graphics

                if (doTrace)
                {
                    pko =
                      new PromptKeywordOptions(
                        "\nPress return to clear trace vectors: "
                      );
                    pko.AllowNone = true;
                    pko.AllowArbitraryInput = true;
                    pkr = ed.GetKeywords(pko);
                    ed.Regen();
                }
            }
        }



        //[CommandMethod("SEWP")]
        //public void SelectEntitiesWithProperties()
        //{
        //    Document doc = Application.DocumentManager.MdiActiveDocument;
        //    Editor ed = doc.Editor;
        //    // Build a conditional filter list so that only
        //    // entities with the specified properties are
        //    // selected
        //    try
        //    {
        //        using (DocumentLock dlock = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.LockDocument())
        //        {
        //            TypedValue[] tvs = new TypedValue[] { 
        //        new TypedValue((int)DxfCode.LayerName, "POST")
        //        //new TypedValue((int)DxfCode.Start, "Polyline")
        //            };

        //            ed.WriteMessage("1\n");

        //            SelectionFilter sf = new SelectionFilter(tvs);
        //            ed.WriteMessage("2\n");
        //            PromptSelectionResult psr = ed.SelectAll(sf);
        //            ed.WriteMessage("3\n");

        //            //for (int i = 0; i < psr.Value.Count - 1; i++)
        //            //{
        //            //    ed.WriteMessage("\ntype {0} entit", psr.Value[i].GetType());
        //            //}

        //            ObjectId[] ids = psr.Value.GetObjectIds();
        //            foreach (ObjectId oi in ids)
        //            {
        //                ed.WriteMessage("\ntype {0} entit", Atend.Global.Acad.UAcad.GetEntityByObjectID(oi).GetType());

        //                Atend.Base.Acad.AT_INFO PostInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oi);
        //                if (PostInfo.ParentCode != "NONE" && PostInfo.NodeType == (int)Atend.Control.Enum.ProductType.GroundPost)
        //                {


        //                    Polyline pl = Atend.Global.Acad.UAcad.GetEntityByObjectID(oi) as Polyline;
        //                    if (pl != null)
        //                    {
        //                        Point2dCollection points = new Point2dCollection();

        //                        for (int i = 0; i <= pl.NumberOfVertices - 1; i++)
        //                        {
        //                            ed.WriteMessage("{0}\n",pl.GetPoint2dAt(i));
        //                            points.Add(pl.GetPoint2dAt(i));
        //                        }

        //                        CreateWipeout(points);
        //                    }


        //                }


        //            }


        //            ed.WriteMessage("\nFound {0} entit", psr.Value.Count);
        //        }
        //    }
        //    catch (System.Exception ex)
        //    {
        //        ed.WriteMessage("@@@@@  ERROR :{0} \n", ex.Message);
        //    }
        //}

        [CommandMethod("CW")]

        public void CreateWipeout()
        {

            Document doc = Application.DocumentManager.MdiActiveDocument;

            Database db = doc.Database;

            Transaction tr = db.TransactionManager.StartTransaction();

            using (tr)
            {

                BlockTable bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead, false);

                BlockTableRecord btr = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite, false);

                Point2dCollection pts = new Point2dCollection(5);

                pts.Add(new Point2d(0.0, 0.0));

                pts.Add(new Point2d(100.0, 0.0));

                pts.Add(new Point2d(100.0, 100.0));

                pts.Add(new Point2d(0.0, 100.0));

                pts.Add(new Point2d(0.0, 0.0));

                Wipeout wo = new Wipeout();

                wo.SetDatabaseDefaults(db);

                wo.SetFrom(pts, new Vector3d(0.0, 0.0, 0.1));

                btr.AppendEntity(wo);

                tr.AddNewlyCreatedDBObject(wo, true);

                tr.Commit();

            }

        }





        //public ObjectId CreateWhiteBack(ObjectId oi)
        //{
        //    Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
        //    Document doc = Application.DocumentManager.MdiActiveDocument;
        //    Database db = doc.Database;
        //    Transaction tr = db.TransactionManager.StartTransaction();
        //    ObjectId hatId = ObjectId.Null;

        //    Entity ent = Atend.Global.Acad.UAcad.GetEntityByObjectID(oi);
        //    Polyline pl = ent as Polyline;

        //    Point2dCollection pts = new Point2dCollection(); //p2;
        //    for (int i = 0; i < pl.NumberOfVertices; i++)
        //    {
        //        Point2d p = pl.GetPoint2dAt(i);
        //        double a = p.X * 1;
        //        double b = p.Y * 1;
        //        pts.Add(new Point2d(a, b));
        //    }

        //    try
        //    {
        //        BlockTable bt = (BlockTable)tr.GetObject(doc.Database.BlockTableId, OpenMode.ForRead);
        //        BlockTableRecord btr = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);

        //        Wipeout wo = new Wipeout();
        //        wo.SetFrom(pts, new Vector3d(0.0, 0.0, 0.1));
        //        wo.LayerId = Atend.Global.Acad.UAcad.GetLayerById(Atend.Control.Enum.AutoCadLayerName.SHELL.ToString());
        //        Point2dCollection p2dc = wo.GetClipBoundary();
        //        btr.AppendEntity(wo);
        //        tr.AddNewlyCreatedDBObject(wo, true);
        //        tr.Commit();

        //    }
        //    catch (System.Exception ex)
        //    {
        //        ed.WriteMessage("ERROR Wipeout : {0} \n", ex.Message);
        //    }
        //    return hatId;
        //}



        //public static void CreateWipeout(Point2dCollection Points)
        //{

        //    Document doc = Application.DocumentManager.MdiActiveDocument;

        //    Database db = doc.Database;

        //    Transaction tr = db.TransactionManager.StartTransaction();
        //    using (DocumentLock dlock = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.LockDocument())
        //    {

        //        using (tr)
        //        {

        //            BlockTable bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead, false , true);

        //            BlockTableRecord btr = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite, false, true);

        //            Point2dCollection pts = Points;

        //            Wipeout wo = new Wipeout();

        //            wo.SetDatabaseDefaults(db);

        //            wo.SetFrom(pts, new Vector3d(0.0, 0.0, 0.1));

        //            //Point2dCollection p2dc = wo.GetClipBoundary();


        //            btr.AppendEntity(wo);

        //            tr.AddNewlyCreatedDBObject(wo, true);

        //            tr.Commit();

        //        }
        //    }
        //}

        [CommandMethod("TEST003")]
        public void ReadFromRegionTable()
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            //Database db = HostApplicationServices.WorkingDatabase;
            Database db = Application.DocumentManager.MdiActiveDocument.Database;
            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                RegAppTable rat = (RegAppTable)tr.GetObject(db.RegAppTableId, OpenMode.ForRead);
                foreach (ObjectId obid in rat)
                {
                    DBObject dbObject = tr.GetObject(obid, OpenMode.ForRead);
                    RegAppTableRecord rr = dbObject as RegAppTableRecord;
                    if (rr != null)
                    {
                        ed.WriteMessage("RegioApplicationTableRecord {0} \n", rr.Name);
                        //DBObject dbo = tr.GetObject(rr.ObjectId, OpenMode.ForRead);
                        ResultBuffer rb = rr.XData;
                        if (rb != null)
                        {
                            foreach (TypedValue tv in rb)
                            {
                                ed.WriteMessage("TypedValue : {0} , Value : {1} \n", tv.TypeCode, tv.Value);
                            }
                        }
                        else
                        {
                            ed.WriteMessage("No XData found \n");
                        }
                    }
                }
                if (rat.Has(kRegAppName))
                {
                    DBObject obj = tr.GetObject(rat[kRegAppName], OpenMode.ForRead);
                    ResultBuffer rb = obj.XData;
                    if (rb != null)
                    {
                        foreach (TypedValue tv in rb)
                        {
                            ed.WriteMessage("TypedValue : {0} , Value : {1} \n", tv.TypeCode, tv.Value);
                        }
                    }
                }
                else
                {
                    ed.WriteMessage("RegioApplicationTable {0} Not Found \n", kRegAppName);
                }
            }
        }

        [CommandMethod("TEST004")]
        public void CreateMyRegionTable()
        {

            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            //Database db = HostApplicationServices.WorkingDatabase;
            Database db = Application.DocumentManager.MdiActiveDocument.Database;
            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                RegAppTable rat = (RegAppTable)tr.GetObject(db.RegAppTableId, OpenMode.ForRead);

                if (!rat.Has("AT_COUNTER"))
                {
                    rat.UpgradeOpen();
                    ed.WriteMessage("table not found");
                    RegAppTableRecord ratr = new RegAppTableRecord();
                    ratr.Name = "AT_COUNTER";
                    rat.Add(ratr);
                    tr.AddNewlyCreatedDBObject(ratr, true);

                    ratr.XData = new ResultBuffer(
                        new TypedValue((int)DxfCode.Text, "SALAM")
                        );


                }
                else
                {
                    ed.WriteMessage("table found");
                }
                tr.Commit();
                //foreach (ObjectId obid in rat)
                //{
                //    DBObject dbObject = tr.GetObject(obid, OpenMode.ForRead);
                //    RegAppTableRecord rr = dbObject as RegAppTableRecord;
                //    if (rr != null)
                //    {
                //        ed.WriteMessage("RegioApplicationTableRecord {0} \n", rr.Name);
                //        //DBObject dbo = tr.GetObject(rr.ObjectId, OpenMode.ForRead);
                //        ResultBuffer rb = rr.XData;
                //        if (rb != null)
                //        {
                //            foreach (TypedValue tv in rb)
                //            {
                //                ed.WriteMessage("TypedValue : {0} , Value : {1} \n", tv.TypeCode, tv.Value);
                //            }
                //        }
                //        else
                //        {
                //            ed.WriteMessage("No XData found \n");
                //        }
                //    }
                //}
                //if (rat.Has(kRegAppName))
                //{
                //    DBObject obj = tr.GetObject(rat[kRegAppName], OpenMode.ForRead);
                //    ResultBuffer rb = obj.XData;
                //    if (rb != null)
                //    {
                //        foreach (TypedValue tv in rb)
                //        {
                //            ed.WriteMessage("TypedValue : {0} , Value : {1} \n", tv.TypeCode, tv.Value);
                //        }
                //    }
                //}
                //else
                //{
                //    ed.WriteMessage("RegioApplicationTable {0} Not Found \n", kRegAppName);
                //}
            }//using

        }

        [CommandMethod("AllDict")]
        public void ReadAllDicts()
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            Database db = Application.DocumentManager.MdiActiveDocument.Database;
            using (Transaction tr = db.TransactionManager.StartTransaction())
            {

                DBDictionary dbdic = (DBDictionary)tr.GetObject(db.NamedObjectsDictionaryId, OpenMode.ForRead);
                ed.WriteMessage("DIC Coount : {0} \n", dbdic.Count);


                //foreach (DBDictionaryEntry ent in dbdic)
                //{
                //    ed.WriteMessage("Key:{0}\n",ent.Key);
                //}


                //dbdic.UpgradeOpen();
                //DBDictionary newDict = new DBDictionary();
                //dbdic.SetAt("AT_COUNTER", newDict);
                //tr.AddNewlyCreatedDBObject(newDict, true);



                //tr.Commit();

                foreach (DBDictionaryEntry dbr in dbdic)
                {
                    ed.WriteMessage("DIC Name : {0} \n", dbr.Key);

                    //~~~~~

                    if (dbdic.Contains(dbr.Key))
                    {

                        ObjectId recId = dbdic.GetAt(dbr.Key);

                        DBObject obj = tr.GetObject(recId, OpenMode.ForRead);

                        Xrecord xrec = obj as Xrecord;

                        if (xrec == null)
                        {
                            ed.WriteMessage("\nDictionary contains non-xrecords.\n");
                        }
                        else
                        {
                            ed.WriteMessage("\nDictionary contains xrecords.\n");

                            //Xrecord xrec = (Xrecord)tr.GetObject(dbdic.GetAt(de.Key), OpenMode.ForRead);
                            foreach (TypedValue tv in xrec.Data)
                            {

                                ed.WriteMessage("Value : {0} , Type : {1} \n", tv.Value, tv.TypeCode);

                            }


                        }


                        //DBDictionary mydic = (DBDictionary)tr.GetObject(dbdic.GetAt(dbr.Key),OpenMode.ForRead);

                        //if (mydic.GetAt("KEy1") != ObjectId.Null)
                        //{
                        //    ed.WriteMessage("Key 1 found \n");
                        //}

                    }
                }
                tr.Commit();
            }

        }

        [CommandMethod("Loadback")]
        public void LoadBack()
        {
            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            doc.SendStringToExecute(@"_Xattach 'F:\TEHRAN FILES\1SHAHRAK_Absabad20.dwg' ", true, false, false);

        }


        [CommandMethod("SXD")]
        static public void SetXData()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            PromptEntityOptions opt = new PromptEntityOptions("\nSelect entity: ");
            PromptEntityResult res = ed.GetEntity(opt);
            if (res.Status == PromptStatus.OK)
            {
                Transaction tr = doc.TransactionManager.StartTransaction();
                using (tr)
                {
                    DBObject obj = tr.GetObject(res.ObjectId, OpenMode.ForWrite);
                    AddRegAppTableRecord("PARISA");
                    ResultBuffer rb = new ResultBuffer(
                                    new TypedValue(1001, "PARISA"),
                                    new TypedValue(1000, "CODE HERE ....")
                                    );
                    obj.XData = rb;
                    rb.Dispose();
                    tr.Commit();
                }
            }
        }

        //~~~~~~~~~~~~~~~ load xref ~~~~~~~~~~~~~~~



        //-----------   MONITOR ~~~~~~~~~~~~~~~~

        [CommandMethod("SM")]
        public static void StartMonitor()
        {

            Editor ed =

              Application.DocumentManager.MdiActiveDocument.Editor;

            ed.PointMonitor +=

              new PointMonitorEventHandler(ed_PointMonitor);

        }

        // XREF binden


        [CommandMethod("XM")]
        public static void StopMonitor()
        {

            Editor ed =

              Application.DocumentManager.MdiActiveDocument.Editor;

            ed.PointMonitor -=

              new PointMonitorEventHandler(ed_PointMonitor);

        }

        static void ed_PointMonitor(object sender, PointMonitorEventArgs e)
        {

            Editor ed = (Editor)sender;
            Document doc = ed.Document;
            //ed.WriteMessage("1\n");
            if (!e.Context.PointComputed)
                return;
            try
            {

                // Determine the size of the selection aperture
                short pickbox = (short)Application.GetSystemVariable("PICKBOX");
                Point2d extents = e.Context.DrawContext.Viewport.GetNumPixelsInUnitSquare(e.Context.ComputedPoint);
                double boxWidth = pickbox / extents.X;
                Vector3d vec = new Vector3d(boxWidth / 2, boxWidth / 2, 0.0);
                // Do a crossing selection using a centred box the
                // size of the aperture
                PromptSelectionResult pse = ed.SelectCrossingWindow(e.Context.ComputedPoint - vec, e.Context.ComputedPoint + vec);
                if (pse.Status != PromptStatus.OK || pse.Value.Count <= 0)
                    return;
                // Go through the results of the selection
                // and detect the curves
                string curveInfo = "";
                Transaction tr = doc.TransactionManager.StartTransaction();
                using (tr)
                {
                    // Open each object, one by one
                    ObjectId[] ids = pse.Value.GetObjectIds();
                    foreach (ObjectId id in ids)
                    {
                        DBObject obj = tr.GetObject(id, OpenMode.ForRead);
                        if (obj != null)
                        {
                            // If it's a curve, get its length
                            Curve cv = obj as Curve;
                            if (cv != null)
                            {
                                double length = cv.GetDistanceAtParameter(cv.EndParam);
                                // Then add a message including the object
                                // type and its length
                                curveInfo += obj.GetType().Name + "'s length: " + string.Format("{0:F}", length) + "\n";
                            }
                        }
                    }
                    // Cheaper than aborting
                    tr.Commit();
                }
                // Add the tooltip of the lengths of the curves detected
                if (curveInfo != "")
                    e.AppendToolTipText(curveInfo);
            }
            catch
            {

                // Not sure what we might get here, but not real action

                // needed (worth adding an Exception parameter and a

                // breakpoint, in case things need investigating).

            }

        }

        //----------------------------
        // Maintain a list of temporary objects that require removal

        ObjectIdCollection _ids = new ObjectIdCollection();

        static bool _placeOnCurrentLayer = true;

        [CommandMethod("XOFFSETLAYER")]

        static public void XrefOffsetLayer()
        {

            Document doc =

              Application.DocumentManager.MdiActiveDocument;

            Database db = doc.Database;

            Editor ed = doc.Editor;



            PromptKeywordOptions pko =

              new PromptKeywordOptions(

                "\nEnter layer option for offset objects"

              );

            const string option1 = "Current";

            const string option2 = "Source";



            pko.AllowNone = true;

            pko.Keywords.Add(option1);

            pko.Keywords.Add(option2);

            pko.Keywords.Default =

              (_placeOnCurrentLayer ? option1 : option2);



            PromptResult pkr =

              ed.GetKeywords(pko);



            if (pkr.Status == PromptStatus.OK)

                _placeOnCurrentLayer =

                  (pkr.StringResult == option1);

        }

        [CommandMethod("XOFFSETCPLAYS")]

        static public void XrefOffsetCopyLayers()
        {

            Document doc =

              Application.DocumentManager.MdiActiveDocument;

            Database db = doc.Database;

            Editor ed = doc.Editor;



            Transaction tr =

              doc.TransactionManager.StartTransaction();

            using (tr)
            {

                BlockTableRecord btr =

                  (BlockTableRecord)tr.GetObject(

                    db.CurrentSpaceId,

                    OpenMode.ForRead

                  );



                // We will collect the layers used by the various entities



                List<string> layerNames =

                  new List<string>();



                // And store a list of the entities to come back and update



                ObjectIdCollection entsToUpdate =

                  new ObjectIdCollection();



                // Loop through the contents of the active space, and look

                // for entities that are on dependent layers (i.e. ones

                // that come from attached xrefs)



                foreach (ObjectId entId in btr)
                {

                    Entity ent =

                      (Entity)tr.GetObject(entId, OpenMode.ForRead);



                    // Check the dependent status of the entity's layer



                    LayerTableRecord ltr =

                      (LayerTableRecord)tr.GetObject(

                        ent.LayerId,

                        OpenMode.ForRead

                      );



                    if (ltr.IsDependent && !(ent is BlockReference))
                    {

                        // Add it to our list and flag the entity for updating



                        string layName = ltr.Name;

                        if (!layerNames.Contains(layName))
                        {

                            layerNames.Add(layName);

                        }

                        entsToUpdate.Add(ent.ObjectId);

                    }

                }

                // Sorting the list will allow us to minimise the number

                // of external drawings we need to load (many layers

                // will be from the same xref)



                layerNames.Sort();



                // Get the xref graph, which allows us to get at the

                // names of the xrefed drawings more easily



                XrefGraph xg = db.GetHostDwgXrefGraph(false);



                // We're going to store a list of our xrefed databases

                // for later disposal



                List<Database> xrefs =

                  new List<Database>();



                // Collect a list of the layers we want to clone across



                ObjectIdCollection laysToClone =

                  new ObjectIdCollection();



                // Loop through the list of layers, only loading xrefs

                // in when they haven't been already



                string currentXrefName = "";

                foreach (string layName in layerNames)
                {

                    Database xdb = null;



                    // Make sure we have our mangled layer name



                    if (layName.Contains("|"))
                    {

                        // Split it up, so we know the xref name

                        // and the root layer name



                        int sepIdx = layName.IndexOf("|");

                        string xrefName =

                          layName.Substring(0, sepIdx);

                        string rootName =

                          layName.Substring(sepIdx + 1);



                        // If the xref is the same as the last loaded,

                        // this saves us some effort



                        if (xrefName == currentXrefName)
                        {

                            xdb = xrefs[xrefs.Count - 1];

                        }

                        else
                        {

                            // Otherwise we get the node for our xref,

                            // so we can get its filename



                            XrefGraphNode xgn =

                              xg.GetXrefNode(xrefName);



                            if (xgn != null)
                            {

                                // Create an xrefed database, loading our

                                // drawing into it



                                xdb = new Database(false, true);

                                xdb.ReadDwgFile(

                                  xgn.Database.Filename,

                                  System.IO.FileShare.Read,

                                  true,

                                  null

                                );



                                // Add it to the list for later disposal

                                // (we do this after the clone operation)



                                xrefs.Add(xdb);

                            }

                            xgn.Dispose();

                        }



                        if (xdb != null)
                        {

                            // Start a transaction in our loaded database

                            // to get at the layer name



                            Transaction tr2 =

                              xdb.TransactionManager.StartTransaction();

                            using (tr2)
                            {

                                // Open the layer table



                                LayerTable lt2 =

                                  (LayerTable)tr2.GetObject(

                                    xdb.LayerTableId,

                                    OpenMode.ForRead

                                  );



                                // Add our layer (which we get via its

                                // unmangled name) to the list to clone



                                if (lt2.Has(rootName))
                                {

                                    laysToClone.Add(lt2[rootName]);

                                }



                                // Committing is cheaper



                                tr2.Commit();

                            }

                        }

                    }

                }



                // If we have layers to clone, do so



                if (laysToClone.Count > 0)
                {

                    // We use wblockCloneObjects to clone between DWGs



                    IdMapping idMap = new IdMapping();

                    db.WblockCloneObjects(

                      laysToClone,

                      db.LayerTableId,

                      idMap,

                      DuplicateRecordCloning.Ignore,

                      false

                    );



                    // Dispose each of our xrefed databases



                    foreach (Database xdb in xrefs)
                    {

                        xdb.Dispose();

                    }



                    // Open the resultant layer table, so we can check

                    // for the existance of our new layers



                    LayerTable lt =

                      (LayerTable)tr.GetObject(

                        db.LayerTableId,

                        OpenMode.ForRead

                      );



                    // Loop through the entities to update



                    foreach (ObjectId id in entsToUpdate)
                    {

                        // Open them each for write, and then check their

                        // current layer



                        Entity ent =

                          (Entity)tr.GetObject(id, OpenMode.ForWrite);

                        LayerTableRecord ltr =

                          (LayerTableRecord)tr.GetObject(

                            ent.LayerId,

                            OpenMode.ForRead

                          );



                        // We split the name once again (could use a function

                        // for this, but hey)



                        string layName = ltr.Name;

                        int sepIdx = layName.IndexOf("|");

                        string xrefName =

                          layName.Substring(0, sepIdx);

                        string rootName =

                          layName.Substring(sepIdx + 1);



                        // If we now have the layer in our database, use it



                        if (lt.Has(rootName))

                            ent.LayerId = lt[rootName];

                    }

                }

                tr.Commit();

            }

        }

        public Commands()
        {
            DocumentCollection dm =

    Application.DocumentManager;



            // Remove any temporary objects at the end of the command



            dm.DocumentLockModeWillChange +=

              delegate(

                object sender, DocumentLockModeWillChangeEventArgs e

              )
              {

                  if (_ids.Count > 0)
                  {

                      Transaction tr =

                        e.Document.TransactionManager.StartTransaction();

                      using (tr)
                      {

                          foreach (ObjectId id in _ids)
                          {

                              DBObject obj =

                                tr.GetObject(id, OpenMode.ForWrite, true);

                              obj.Erase();

                          }

                          tr.Commit();

                      }

                      _ids.Clear();



                      // Launch a command to bring across our layers



                      if (!_placeOnCurrentLayer)
                      {

                          e.Document.SendStringToExecute(

                            "_.XOFFSETCPLAYS ", false, false, false

                          );

                      }

                  }

              };



            // When a document is created, make sure we handle the

            // important events it fires



            dm.DocumentCreated +=

              delegate(

                object sender, DocumentCollectionEventArgs e

              )
              {

                  e.Document.CommandWillStart +=

                    new CommandEventHandler(OnCommandWillStart);

                  e.Document.CommandEnded +=

                    new CommandEventHandler(OnCommandFinished);

                  e.Document.CommandCancelled +=

                    new CommandEventHandler(OnCommandFinished);

                  e.Document.CommandFailed +=

                    new CommandEventHandler(OnCommandFinished);

              };



            // Do the same for any documents existing on application

            // initialization



            foreach (Document doc in dm)
            {

                doc.CommandWillStart +=

                  new CommandEventHandler(OnCommandWillStart);

                doc.CommandEnded +=

                  new CommandEventHandler(OnCommandFinished);

                doc.CommandCancelled +=

                  new CommandEventHandler(OnCommandFinished);

                doc.CommandFailed +=

                  new CommandEventHandler(OnCommandFinished);

            }
        }

        // When the OFFSET command starts, let's add our selection

        // manipulating event-handler

        void OnCommandWillStart(object sender, CommandEventArgs e)
        {

            if (e.GlobalCommandName == "OFFSET")
            {

                Document doc = (Document)sender;

                doc.Editor.PromptForEntityEnding +=

                  new PromptForEntityEndingEventHandler(

                    OnPromptForEntityEnding

                  );

            }

        }

        // And when the command ends, remove it

        void OnCommandFinished(object sender, CommandEventArgs e)
        {

            if (e.GlobalCommandName == "OFFSET")
            {

                Document doc = (Document)sender;

                doc.Editor.PromptForEntityEnding -=

                  new PromptForEntityEndingEventHandler(

                    OnPromptForEntityEnding

                  );

            }

        }

        // Here's where the heavy lifting happens...

        void OnPromptForEntityEnding(

          object sender, PromptForEntityEndingEventArgs e

        )
        {

            if (e.Result.Status == PromptStatus.OK)
            {

                Editor ed = sender as Editor;

                ObjectId objId = e.Result.ObjectId;

                Database db = objId.Database;



                Transaction tr =

                  db.TransactionManager.StartTransaction();

                using (tr)
                {

                    // First get the currently selected object

                    // and check whether it's a block reference



                    BlockReference br =

                      tr.GetObject(objId, OpenMode.ForRead)

                        as BlockReference;

                    if (br != null)
                    {

                        // If so, we check whether the block table record

                        // to which it refers is actually from an XRef



                        ObjectId btrId = br.BlockTableRecord;

                        BlockTableRecord btr =

                          tr.GetObject(btrId, OpenMode.ForRead)

                            as BlockTableRecord;

                        if (btr != null)
                        {

                            if (btr.IsFromExternalReference)
                            {

                                // If so, then we programmatically select the object

                                // underneath the pick-point already used



                                PromptNestedEntityOptions pneo =

                                  new PromptNestedEntityOptions("");

                                pneo.NonInteractivePickPoint =

                                  e.Result.PickedPoint;

                                pneo.UseNonInteractivePickPoint = true;



                                PromptNestedEntityResult pner =

                                  ed.GetNestedEntity(pneo);



                                if (pner.Status == PromptStatus.OK)
                                {

                                    try
                                    {

                                        ObjectId selId = pner.ObjectId;



                                        // Let's look at this programmatically-selected

                                        // object, to see what it is



                                        DBObject obj =

                                          tr.GetObject(selId, OpenMode.ForRead);



                                        // If it's a polyline vertex, we need to go one

                                        // level up to the polyline itself



                                        if (obj is PolylineVertex3d || obj is Vertex2d)

                                            selId = obj.OwnerId;



                                        // We don't want to do anything at all for

                                        // textual stuff, let's also make sure we

                                        // are dealing with an entity (should always

                                        // be the case)



                                        if (obj is MText || obj is DBText ||

                                          !(obj is Entity))

                                            return;



                                        // Now let's get the name of the layer, to use

                                        // later



                                        Entity ent = (Entity)obj;

                                        LayerTableRecord ltr =

                                          (LayerTableRecord)tr.GetObject(

                                            ent.LayerId,

                                            OpenMode.ForRead

                                          );

                                        string layName = ltr.Name;



                                        // Clone the selected object



                                        object o = ent.Clone();

                                        Entity clone = o as Entity;



                                        // We need to manipulate the clone to make sure

                                        // it works



                                        if (clone != null)
                                        {

                                            // Setting the properties from the block

                                            // reference helps certain entities get the

                                            // right references (and allows them to be

                                            // offset properly)



                                            clone.SetPropertiesFrom(br);



                                            // But we then need to get the layer

                                            // information from the database to set the

                                            // right layer (at least) on the new entity



                                            if (_placeOnCurrentLayer)
                                            {

                                                clone.LayerId = db.Clayer;

                                            }

                                            else
                                            {

                                                LayerTable lt =

                                                  (LayerTable)tr.GetObject(

                                                    db.LayerTableId,

                                                    OpenMode.ForRead

                                                  );

                                                if (lt.Has(layName))

                                                    clone.LayerId = lt[layName];

                                            }



                                            // Now we need to transform the entity for

                                            // each of its Xref block reference containers

                                            // If we don't do this then entities in nested

                                            // Xrefs may end up in the wrong place



                                            ObjectId[] conts =

                                              pner.GetContainers();

                                            foreach (ObjectId contId in conts)
                                            {

                                                BlockReference cont =

                                                  tr.GetObject(contId, OpenMode.ForRead)

                                                    as BlockReference;

                                                if (cont != null)

                                                    clone.TransformBy(cont.BlockTransform);

                                            }



                                            // Let's add the cloned entity to the current

                                            // space



                                            BlockTableRecord space =

                                              tr.GetObject(

                                                db.CurrentSpaceId,

                                                OpenMode.ForWrite

                                              ) as BlockTableRecord;

                                            if (space == null)
                                            {

                                                clone.Dispose();

                                                return;

                                            }



                                            ObjectId cloneId = space.AppendEntity(clone);

                                            tr.AddNewlyCreatedDBObject(clone, true);



                                            // Now let's flush the graphics, to help our

                                            // clone get displayed



                                            tr.TransactionManager.QueueForGraphicsFlush();



                                            // And we add our cloned entity to the list

                                            // for deletion



                                            _ids.Add(cloneId);



                                            // Created a non-graphical selection of our

                                            // newly created object and replace it with

                                            // the selection of the container Xref



                                            SelectedObject so =

                                              new SelectedObject(

                                                cloneId,

                                                SelectionMethod.NonGraphical,

                                                -1

                                              );



                                            e.ReplaceSelectedObject(so);

                                        }

                                    }

                                    catch
                                    {

                                        // A number of innocuous things could go wrong

                                        // so let's not worry about the details



                                        // In the worst case we are simply not trying

                                        // to replace the entity, so OFFSET will just

                                        // reject the selected Xref

                                    }

                                }

                            }

                        }

                    }

                    tr.Commit();

                }

            }

        }

        //------------------------------

        [CommandMethod("sS")]
        static public void ss()
        {


            //Atend.Calculating.frmTest t=new Atend.Calculating.frmTest();
            //t.Show();
            //Atend.Global.Acad.UAcad.FillPoleSubList();

            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            //ed.WriteMessage("####" + Atend.Global.Acad.DrawEquips.Dicision.IsHere() + "\n");
            //ed.WriteMessage("####" + Atend.Global.Acad.DrawEquips.Dicision.IsThere() + "\n");


            //Atend.Global.Calculation.Mechanical.AutoPoleInstallation _AutoPoleInstallation = new Atend.Global.Calculation.Mechanical.AutoPoleInstallation();
            //_AutoPoleInstallation.SpanCount = 4;
            //_AutoPoleInstallation.CalculateSpanPoints(new Point3d(0, 0, 0), new Point3d(30, 30, 0));
            //foreach (Point3d p in _AutoPoleInstallation.SpanPoints)
            //{
            //    ed.WriteMessage("~~~~" + p.ToString() + "\n");
            //}


            //PromptPointOptions ppo1 = new PromptPointOptions("a");
            //PromptPointResult ppr1 = ed.GetPoint(ppo1);

            //PromptPointOptions ppo2 = new PromptPointOptions("b");
            //PromptPointResult ppr2 = ed.GetPoint(ppo2);

            //Vector3d v = ppr2.Value - ppr1.Value;

            //Ellipse e = new Ellipse(ppr1.Value, v.GetNormal(), Vector3d.XAxis, 1, 0, 6.28318530717958647692);
            ////Ellipse e = new Ellipse();
            //Atend.Global.Acad.UAcad.DrawEntityOnScreen(e, Atend.Control.Enum.AutoCadLayerName.GENERAL.ToString());

            //PromptStringOptions ps = new PromptStringOptions("sss");
            //PromptResult pr = ed.GetString(ps);
            //Atend.Global.Acad.DrawEquips.AcDrawPole.DeletePoleData(new Guid(pr.StringResult));


        }

        [CommandMethod("AS")]
        static public void addScale()
        {

            Document doc =
              Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;


            PromptStringOptions ps = new PromptStringOptions("id");
            PromptResult pr = ed.GetString(ps);

            PromptStringOptions ps1 = new PromptStringOptions("id");
            PromptResult pr1 = ed.GetString(ps1);


            //Atend.Global.Acad.UAcad.GetNodeInfoByObjectId(Convert.ToInt32(pr.StringResult));
            ed.WriteMessage("ans:{0}\n", Atend.Global.Acad.UAcad.GetNextNodeBranch(Convert.ToInt32(pr.StringResult), Convert.ToInt32(pr1.StringResult)));
            //Autodesk.AutoCAD.DatabaseServices.Database.

            //Arc a=new Arc(

            //Atend.Base.Acad.AT_COUNTER.ReadAll();
            //ed.WriteMessage("Pole:{0}", Atend.Control.Common.Counters.PoleCounter);
            //ed.WriteMessage("Consol:{0}", Atend.Control.Common.Counters.ConsolCounter);
            //ed.WriteMessage("Clamp:{0}", Atend.Control.Common.Counters.ClampCounter);
            //ed.WriteMessage("HeaderCabel:{0}\n", Atend.Control.Common.Counters.HeadercableCounter);
            //Atend.Base.Acad.AT_COUNTER.ReadAll();
            //ed.WriteMessage("Pole:{0}", Atend.Control.Common.Counters.PoleCounter);
            //ed.WriteMessage("Consol:{0}", Atend.Control.Common.Counters.ConsolCounter);
            //ed.WriteMessage("Clamp:{0}", Atend.Control.Common.Counters.ClampCounter);
            //ed.WriteMessage("HeaderCabel:{0}\n", Atend.Control.Common.Counters.HeadercableCounter);

            //Atend.Control.Common.Counters.PoleCounter = 10;
            //Atend.Control.Common.Counters.HeadercableCounter = 11;
            //Atend.Control.Common.Counters.ConsolCounter = 12;
            //Atend.Control.Common.Counters.ClampCounter = 13;


            //Atend.Base.Acad.AT_COUNTER.SaveAll();
            //Atend.Base.Acad.AT_COUNTER.ReadAll();

            //ed.WriteMessage("Pole:{0}", Atend.Control.Common.Counters.PoleCounter);
            //ed.WriteMessage("Consol:{0}", Atend.Control.Common.Counters.ConsolCounter);
            //ed.WriteMessage("Clamp:{0}", Atend.Control.Common.Counters.ClampCounter);
            //ed.WriteMessage("HeaderCabel:{0}\n", Atend.Control.Common.Counters.HeadercableCounter);

            //Dictionary<Guid, Atend.Base.Equipment.EJackPanelCell> a = new Dictionary<Guid, Atend.Base.Equipment.EJackPanelCell>();
            //a.Add(Guid.NewGuid(),new Atend.Base.Equipment.EJackPanelCell());
            //a.Add(Guid.NewGuid(),new Atend.Base.Equipment.EJackPanelCell());
            //a.Add(Guid.NewGuid(),new Atend.Base.Equipment.EJackPanelCell());
            //foreach (Guid g in a.Keys)
            //{

            //    ed.WriteMessage(g.ToString()+"\n");
            //}



            ////////try
            ////////{
            ////////    ObjectContextManager cm =
            ////////      db.ObjectContextManager;
            ////////    if (cm != null)
            ////////    {
            ////////        // Now get the Annotation Scaling context collection
            ////////        // (named ACDB_ANNOTATIONSCALES_COLLECTION)
            ////////        ObjectContextCollection occ =
            ////////          cm.GetContextCollection("ACDB_ANNOTATIONSCALES");
            ////////        if (occ != null)
            ////////        {
            ////////            // Create a brand new scale context
            ////////            AnnotationScale asc = new AnnotationScale();
            ////////            asc.Name = "MyScale 1:28";
            ////////            asc.PaperUnits = 1;
            ////////            asc.DrawingUnits = 28;
            ////////            // Add it to the drawing's context collection
            ////////            occ.AddContext(asc);
            ////////        }
            ////////    }
            ////////}
            ////////catch (System.Exception ex)
            ////////{
            ////////    ed.WriteMessage(ex.ToString());
            ////////}
        }

        [CommandMethod("ADS")]
        static public void addScale01()
        {
            Document doc =
              Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;
            try
            {
                ObjectContextManager ocm =
                  db.ObjectContextManager;
                if (ocm != null)
                {
                    // Now get the Annotation Scaling context collection
                    // (named ACDB_ANNOTATIONSCALES_COLLECTION)
                    ObjectContextCollection occ =
                      ocm.GetContextCollection("ACDB_ANNOTATIONSCALES");

                    if (occ != null)
                    {
                        // Create a brand new scale context
                        AnnotationScale asc = new AnnotationScale();
                        asc.Name = "MyScale 1:28";
                        asc.PaperUnits = 1;
                        asc.DrawingUnits = 28;
                        // Add it to the drawing's context collection
                        occ.AddContext(asc);
                    }
                }
            }
            catch (System.Exception ex)
            {
                ed.WriteMessage(ex.ToString());
            }
        }

        [CommandMethod("ATS")]
        static public void attachScale()
        {
            Document doc =
              Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;
            ObjectContextManager ocm =
              db.ObjectContextManager;
            ObjectContextCollection occ =
              ocm.GetContextCollection("ACDB_ANNOTATIONSCALES");

            Transaction tr =
              doc.TransactionManager.StartTransaction();
            using (tr)
            {
                PromptEntityOptions opts =
                  new PromptEntityOptions("\nSelect entity: ");
                opts.SetRejectMessage(
                  "\nEntity must support annotation scaling."
                );
                //opts.AddAllowedClass(typeof(DBText), false);
                //opts.AddAllowedClass(typeof(MText), false);
                //opts.AddAllowedClass(typeof(Dimension), false);
                //opts.AddAllowedClass(typeof(Leader), false);
                //opts.AddAllowedClass(typeof(Table), false);
                //opts.AddAllowedClass(typeof(Hatch), false);

                PromptEntityResult per = ed.GetEntity(opts);
                if (per.ObjectId != ObjectId.Null)
                {
                    DBObject obj =
                      tr.GetObject(per.ObjectId, OpenMode.ForRead);
                    if (obj != null)
                    {
                        obj.UpgradeOpen();
                        obj.Annotative = AnnotativeStates.True;
                        obj.AddContext(occ.GetContext("1:1"));
                        obj.AddContext(occ.GetContext("1:2"));
                        obj.AddContext(occ.GetContext("1:10"));
                        ObjectContext oc = occ.GetContext("MyScale 1:28");
                        if (oc != null)
                        {
                            obj.AddContext(oc);
                        }
                    }
                }
                tr.Commit();
            }
        }

        [CommandMethod("SPE")]
        public void SetPropertiesOnEntity()
        {

            Document doc =

              Application.DocumentManager.MdiActiveDocument;

            Database db = doc.Database;

            Editor ed = doc.Editor;

            PromptEntityResult per =

              ed.GetEntity(

                "\nSelect entity to modify: "

              );

            if (per.Status != PromptStatus.OK)

                return;

            Transaction tr =

              db.TransactionManager.StartTransaction();

            using (tr)
            {

                Entity ent =

                  (Entity)

                    tr.GetObject(

                      per.ObjectId,

                      OpenMode.ForRead

                    );

                ColorDialog cd = new ColorDialog();

                cd.Color = ent.Color;

                System.Windows.Forms.DialogResult dr;

                dr = cd.ShowDialog();

                if (dr != System.Windows.Forms.DialogResult.OK)

                    return;

                LinetypeDialog ltd = new LinetypeDialog();

                ltd.Linetype = ent.LinetypeId;

                dr = ltd.ShowDialog();

                if (dr != System.Windows.Forms.DialogResult.OK)

                    return;

                LineWeightDialog lwd = new LineWeightDialog();

                lwd.LineWeight = ent.LineWeight;

                dr = lwd.ShowDialog();

                if (dr != System.Windows.Forms.DialogResult.OK)

                    return;

                ent.UpgradeOpen();

                if (ent.Color != cd.Color)

                    ent.Color = cd.Color;

                if (ent.LinetypeId != ltd.Linetype)

                    ent.LinetypeId = ltd.Linetype;

                if (ent.LineWeight != lwd.LineWeight)

                    ent.LineWeight = lwd.LineWeight;

                tr.Commit();

            }

        }


        [CommandMethod("SOB")]
        public void polenumber()
        {
            //Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            //PromptStringOptions pso = new PromptStringOptions("\nPoleOI:");
            //PromptResult pr = ed.GetString(pso);
            //Atend.Acad.AcadRemove.DeleteGroundPost(new ObjectId(Convert.ToInt32(pr.StringResult)));

            Atend.Global.Calculation.General.General.GetDesignPoles();

            //PromptStringOptions pso = new PromptStringOptions("\nPoleGuid:");
            //PromptResult pr = ed.GetString(pso);
            //Guid ID = new Guid(pr.StringResult);

            //PromptStringOptions pso1 = new PromptStringOptions("\nCatOutProductcode:");
            //PromptResult pr1 = ed.GetString(pso);

            //int catout = Convert.ToInt32(pr1.StringResult);

            //Atend.Global.Desig.NodeTransaction.DeleteCatOut(ID);
            //---------------------------------------------------
            //PromptStringOptions pso = new PromptStringOptions("\nPostNumber:");
            //PromptResult pr = ed.GetString(pso);

            //Atend.Base.Design.NodeTransaction.InsertCatOut(ID , catout);

            //foreach (Atend.Base.Acad.AcadGlobal.PostEquips p in Atend.Base.Acad.AcadGlobal.PostEquipInserted)
            //{
            //    ed.WriteMessage("ParentCode:{0},ProductCode:{1},ProductType:{2},CodeGuid:{3}\n",
            //        p.ParentCode, p.ProductCode, p.ProductType, p.CodeGuid);
            //    ed.WriteMessage("-------------------------------------------------------\n");
            //}

            //-------------------------------------------------------


            //PromptStringOptions pso = new PromptStringOptions("\nConsol oi:");
            //PromptResult pr = ed.GetString(pso);
            ////Atend.Acad.UAcad.GetConsolInfoByObjectId(Convert.ToInt32(pr.StringResult));
            //PromptResult pr1 = ed.GetString(pso);
            //pso.Message = "\nBranch oi:";
            //Atend.Acad.UAcad.GetNextConsol(Convert.ToInt32(pr.StringResult), Convert.ToInt32(pr1.StringResult));

            //----------------------------------------------------------

            //Atend.Calculating.frmTest t = new Atend.Calculating.frmTest();
            //Application.ShowModalDialog(t);

            //Atend.Acad.UAcad.GetPoleConductors();\


            //Atend.Base.Design.NodeTransaction.DeletePole();

            //Atend.Base.Design.NodeTransaction a = new Atend.Base.Design.NodeTransaction();
            //a.f();


            //PromptEntityOptions peo = new PromptEntityOptions("Emtity:");
            //PromptEntityResult per = ed.GetEntity(peo);

            //ObjectId GroupId = Atend.Acad.UAcad.GetEntityGroup(per.ObjectId);
            //if (GroupId != ObjectId.Null)
            //{
            //    Atend.Base.Acad.AT_SUB GroupSub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(GroupId);
            //    foreach (ObjectId oi in GroupSub.SubIdCollection)
            //    {
            //        Atend.Base.Acad.AT_INFO info = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oi);
            //        if (info.ParentCode != "NONE")
            //        {
            //            ed.WriteMessage("OI:{0} , TYpe:{1} \n", oi,info.NodeType);
            //        }

            //    }
            //}


            //PromptStringOptions pso = new PromptStringOptions("\nConsolcode:");
            //PromptResult pr = ed.GetString(pso);

            //Atend.Acad.AcadRemove.DeleteConsol(per.ObjectId, pr.StringResult);


            //Atend.Global.Utility.UReport Report = new Atend.Global.Utility.UReport();
            //System.Data.DataTable Table = Report.CreateExcelStatus();

            //Dictionary<string, string> Dic = new Dictionary<string, string>();
            //Dic.Add("Code", "كد كالا");
            //Dic.Add("Name", "نام كالا");
            //Dic.Add("Count", "تعداد");
            //Dic.Add("Unit", "واحد كالا");

            //for (int i = 0; i < Table.Rows.Count; i++)
            //{
            //    if (Table.Rows[i]["Count"].ToString() == "1")
            //        Table.Rows[i]["Count"] = "عدد";

            //    else
            //        if (Table.Rows[i]["Count"].ToString() == "3")
            //            Table.Rows[i]["Count"] = "كيلو گرم";
            //        else
            //            if (Table.Rows[i]["Count"].ToString() == "2")
            //                Table.Rows[i]["Count"] = "متر";


            //}

            //Atend.Global.Utility.UReport.CreateExcelReaport("صورت وضعيت" + DateTime.Now.ToShortDateString(), Table, Dic, 1);

        }

        //~~~~~~~~~~~~~~~~~~~~ MAIN METHODS ~~~~~~~~~~~~~~~~~~~//

        [CommandMethod("at_cu")]
        public void ShowCutout()
        {
            Base.frmProductSearch frmproductSearch = new Atend.Base.frmProductSearch(Atend.Control.Enum.ProductType.CatOut);
            Application.ShowModalDialog(frmproductSearch);
        }

        [CommandMethod("at_ph")]
        public void ShowPhuse()
        {
            Base.frmProductSearch frmproductSearch = new Atend.Base.frmProductSearch(Atend.Control.Enum.ProductType.Phuse);
            Application.ShowModalDialog(frmproductSearch);
        }

        [CommandMethod("at_rd")]
        public void ShowRod()
        {
            Base.frmProductSearch frmproductSearch = new Atend.Base.frmProductSearch(Atend.Control.Enum.ProductType.Rod);
            Application.ShowModalDialog(frmproductSearch);
        }

        [CommandMethod("at_k3")]
        public void ShowAutoKey()
        {
            Base.frmProductSearch frmproductSearch = new Atend.Base.frmProductSearch(Atend.Control.Enum.ProductType.AuoKey3p);
            Application.ShowModalDialog(frmproductSearch);
        }

        [CommandMethod("at_ds")]
        public void ShowDisconnector()
        {
            Base.frmProductSearch frmproductSearch = new Atend.Base.frmProductSearch(Atend.Control.Enum.ProductType.Disconnector);
            Application.ShowModalDialog(frmproductSearch);
        }

        [CommandMethod("at_br")]
        public void ShowBreaker()
        {
            Base.frmProductSearch frmproductSearch = new Atend.Base.frmProductSearch(Atend.Control.Enum.ProductType.Breaker);
            Application.ShowModalDialog(frmproductSearch);
        }

        //[CommandMethod("Blockentity")]
        //public void BlockInBlock()
        //{
        //    Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
        //    PromptEntityOptions peo = new PromptEntityOptions("Entity Option:");
        //    PromptEntityResult per = ed.GetEntity(peo);
        //    if (per.Status == PromptStatus.OK)
        //    {
        //        Database db = HostApplicationServices.WorkingDatabase;
        //        using (Transaction tr = db.TransactionManager.StartTransaction())
        //        {
        //            Entity ent = (Entity)tr.GetObject(per.ObjectId, OpenMode.ForWrite);

        //        }
        //    }
        //}

        [CommandMethod("at_ct")]
        public void ShowCT()
        {
            Base.frmProductSearch frmproductSearch = new Atend.Base.frmProductSearch(Atend.Control.Enum.ProductType.CT);
            Application.ShowModalDialog(frmproductSearch);
        }

        [CommandMethod("at_pt")]
        public void ShowPT()
        {
            Base.frmProductSearch frmproductSearch = new Atend.Base.frmProductSearch(Atend.Control.Enum.ProductType.PT);
            Application.ShowModalDialog(frmproductSearch);
        }

        [CommandMethod("at_ce")]
        public void ShowPhotocell()
        {
            Base.frmProductSearch frmproductSearch = new Atend.Base.frmProductSearch(Atend.Control.Enum.ProductType.PhotoCell);
            Application.ShowModalDialog(frmproductSearch);
        }

        [CommandMethod("at_co")]
        public void ShowCountor()
        {
            Base.frmProductSearch frmproductSearch = new Atend.Base.frmProductSearch(Atend.Control.Enum.ProductType.Countor);
            Application.ShowModalDialog(frmproductSearch);
        }

        [CommandMethod("at_bu")]
        public void ShowBus()
        {
            Base.frmProductSearch frmproductSearch = new Atend.Base.frmProductSearch(Atend.Control.Enum.ProductType.Bus);
            Application.ShowModalDialog(frmproductSearch);
        }

        [CommandMethod("at_hc")]
        public void ShowHeaderCabel()
        {
            Base.frmProductSearch frmproductSearch = new Atend.Base.frmProductSearch(Atend.Control.Enum.ProductType.HeaderCabel);
            Application.ShowModalDialog(frmproductSearch);
        }

        //[CommandMethod("at_dp")]
        //public void ShowDistributedPost()
        //{
        //    Base.frmProductSearch frmproductSearch = new Atend.Base.frmProductSearch(Atend.Control.Enum.ProductType.DistributedPost);
        //    Application.ShowModalDialog(frmproductSearch);
        //}
        //[CommandMethod("at_gp")]
        //public void ShowGroundPost()
        //{
        //    Base.frmProductSearch frmproductSearch = new Atend.Base.frmProductSearch(Atend.Control.Enum.ProductType.GroundPost);
        //    Application.ShowModalDialog(frmproductSearch);
        //}
        [CommandMethod("at_tr")]
        public void ShowTransformer()
        {
            Base.frmProductSearch frmproductSearch = new Atend.Base.frmProductSearch(Atend.Control.Enum.ProductType.Transformer);
            Application.ShowModalDialog(frmproductSearch);
        }

        [CommandMethod("at_jp")]
        public void ShowJackPanel()
        {
            Base.frmProductSearch frmproductSearch = new Atend.Base.frmProductSearch(Atend.Control.Enum.ProductType.MiddleJackPanel);
            Application.ShowModalDialog(frmproductSearch);
        }

        [CommandMethod("at_cd")]
        public void ShowConductor()
        {
            Base.frmProductSearch frmproductSearch = new Atend.Base.frmProductSearch(Atend.Control.Enum.ProductType.Conductor);
            Application.ShowModalDialog(frmproductSearch);
        }

        //[CommandMethod("at_to")]
        //public void ShowTower()
        //{
        //    Base.frmProductSearch frmproductSearch = new Atend.Base.frmProductSearch(Atend.Control.Enum.ProductType.Tower);
        //    Application.ShowModalDialog(frmproductSearch);
        //}

        [CommandMethod("at_po")]
        public void ShowPole()
        {
            Base.frmProductSearch frmproductSearch = new Atend.Base.frmProductSearch(Atend.Control.Enum.ProductType.Pole);
            Application.ShowModalDialog(frmproductSearch);
        }

        [CommandMethod("at_in")]
        public void ShowInsulator()
        {
            Base.frmProductSearch frmproductSearch = new Atend.Base.frmProductSearch(Atend.Control.Enum.ProductType.Insulator);
            Application.ShowModalDialog(frmproductSearch);
        }

        [CommandMethod("at_ca")]
        public void ShowCabel()
        {
            Base.frmProductSearch frmproductSearch = new Atend.Base.frmProductSearch(Atend.Control.Enum.ProductType.GroundCabel);
            Application.ShowModalDialog(frmproductSearch);
        }

        [CommandMethod("at_sb")]
        public void ShowStreetBox()
        {
            Base.frmProductSearch frmproductSearch = new Atend.Base.frmProductSearch(Atend.Control.Enum.ProductType.StreetBox);
            Application.ShowModalDialog(frmproductSearch);
        }

        [CommandMethod("at_db")]
        public void ShowDB()
        {
            Base.frmProductSearch frmproductSearch = new Atend.Base.frmProductSearch(Atend.Control.Enum.ProductType.DB);
            Application.ShowModalDialog(frmproductSearch);
        }

        //[CommandMethod("SHOWP")]
        //public void showpalette()
        //{
        //    Autodesk.AutoCAD.Windows.PaletteSet ps;
        //    ps = new Autodesk.AutoCAD.Windows.PaletteSet("مدیریت تجهیزات");
        //    ps.Style = Autodesk.AutoCAD.Windows.PaletteSetStyles.NameEditable |
        //        Autodesk.AutoCAD.Windows.PaletteSetStyles.ShowPropertiesMenu |
        //        Autodesk.AutoCAD.Windows.PaletteSetStyles.ShowAutoHideButton |
        //        Autodesk.AutoCAD.Windows.PaletteSetStyles.ShowCloseButton;
        //    ps.Size = new System.Drawing.Size(210, 541);
        //    ps.MinimumSize = new System.Drawing.Size(125, 541);
        //    ps.Add("ProductPalette", new ucProduct());
        //    ps.Visible = true;

        //}

        //[CommandMethod("SHOWPRODUCT")]
        //public void ShowProduct()
        //{
        //    Autodesk.AutoCAD.Windows.PaletteSet ps;
        //    ps = new Autodesk.AutoCAD.Windows.PaletteSet("ترسیم تجهیزات");
        //    ps.Style = Autodesk.AutoCAD.Windows.PaletteSetStyles.NameEditable |
        //        Autodesk.AutoCAD.Windows.PaletteSetStyles.ShowPropertiesMenu |
        //        Autodesk.AutoCAD.Windows.PaletteSetStyles.ShowAutoHideButton |
        //        Autodesk.AutoCAD.Windows.PaletteSetStyles.ShowCloseButton;
        //    ps.Size = new System.Drawing.Size(210, 541);
        //    ps.MinimumSize = new System.Drawing.Size(125, 541);
        //    ps.Add("ProductListPalette1", new uCustomePanel());
        //    ps.Visible = true;
        //}

        public void EnableRibbonButton(string TabId, string ButtonId)
        {
            Autodesk.Windows.RibbonControl rcontrol = Autodesk.AutoCAD.Ribbon.RibbonServices.RibbonPaletteSet.RibbonControl;

            //rcontrol.Tabs["ID_Setting"].Panels[0].Source.Rows[1].Items["btnDefineUser"].IsEnabled = true;

            foreach (Autodesk.Windows.RibbonTab rTab in rcontrol.Tabs)
            {
                if (rTab.Id == TabId)//"ID_Setting"
                {
                    foreach (Autodesk.Windows.RibbonPanel rPanel in rTab.Panels)
                    {
                        Autodesk.Windows.RibbonPanelSource rPanelSource = rPanel.Source;
                        foreach (Autodesk.Windows.RibbonRow rRow in rPanelSource.Rows)
                        {
                            foreach (RibbonItem rItem in rRow.Items)
                            {
                                RibbonButton rButton = rItem as RibbonButton;
                                if (rButton != null)
                                {
                                    if (rButton.Id == ButtonId)//"btnDefineUser"
                                    {
                                        rButton.IsEnabled = true;
                                        Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
                                        ed.WriteMessage("Active\n");
                                    }
                                }
                            }
                        }
                    }

                }
            }
        }

        [CommandMethod("AtLogIn")]
        public void logintoAtend()
        {
            /*
            try
            {
                Atend.Design.frmInterface inf = new Atend.Design.frmInterface();


                Atend.Global.Utility.UInterface inter = new Atend.Global.Utility.UInterface();
                string utl = inter.GetInterface();
                string pi = utl.Substring(0, 16);
                string hs = utl.Substring(16, 10);
                using (RegistryKey Key = Registry.LocalMachine.OpenSubKey(@"Software\Autodesk\AutoCAD\R17.2\ACAD-7001:409\Applications\ATEND", false))
                {
                    if (Key != null)
                    {
                        if (Key.GetValue("InterfaceDT").ToString() != "" && Key.GetValue("InterfaceDU").ToString() != "")
                        {
                            int du = Convert.ToInt32(Key.GetValue("InterfaceDU").ToString());
                            string t = Key.GetValue("InterfaceDT").ToString();
                            DateTime _date = Convert.ToDateTime(string.Format(@"{0}/{1}/{2}", t.Substring(0, 4), t.Substring(4, 2), t.Substring(6, 2)));

                            //string next = Key.GetValue("InterfaceDN").ToString();
                            //DateTime _dateN = Convert.ToDateTime(string.Format(@"{0}/{1}/{2}", next.Substring(0, 4), next.Substring(4, 2), next.Substring(6, 2)));
                            
                            DateTime nowDate = DateTime.Now;
                            DateTime lastDate = _date.AddMonths(du);
                            if (nowDate > lastDate)
                            {
                                Atend.Global.Acad.Notification notification = new Atend.Global.Acad.Notification();
                                notification.Title = "خطا";
                                notification.Msg = "اعتبار نرم افزار به پایان رسیده است";
                                notification.infoCenterBalloon();
                            }
                            else
                            {
                                if (Key.GetValue("InterfacePI").ToString() == "" || Key.GetValue("InterfaceHS").ToString() == ""
                                    || Key.GetValue("InterfacePI").ToString() != pi || Key.GetValue("InterfaceHS").ToString() != hs)
                                {
                                    inf.ShowDialog();
                                }
                                else
                                    inf._ISACTIVE = 1;
                            }
                        }
                        else //not set date in registry
                        {
                            inf.ShowDialog();
                        }
                    }

                    //if (Key != null)
                    //{
                    
                    //}

                    if (inf._ISACTIVE == 1)
                    {
                        //Atend.Menu myMenu = new Menu();
                        //myMenu.Initialize();
                        Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
                        DocManForUI MyDocManForUI = new DocManForUI();
                        MyDocManForUI.Do();
                        Menu m = new Menu();
                        if (Atend.Control.ConnectionString.ConnectionValidation(Atend.Control.ConnectionString.LocalcnString))
                        {
                            if (Atend.Control.Common.userCode == 0)
                            {
                                Atend.Design.frmLogin FrmLogin = new Atend.Design.frmLogin();
                                if (Application.ShowModalDialog(FrmLogin) == System.Windows.Forms.DialogResult.OK)
                                {
                                    //ed.WriteMessage("OK\n");
                                    //active user access
                                    if (!Atend.Control.Common.IsClassicView)
                                    {
                                        Atend.Acad.Ribbon.DesignRibbon();
                                        EnableRibbonButton("ID_Setting", "btnShowAccess");
                                        //EnableRibbonButton("ID_Setting", "btnRemarkSetting");
                                        EnableRibbonButton("ID_Setting", "btnBSetting");
                                        foreach (System.Data.DataRow dr in Atend.Control.Common.AccessList.Rows)
                                        {
                                            switch (Convert.ToInt32(dr["IdAccess"]))
                                            {
                                                case 1:
                                                    //ed.WriteMessage("1\n");
                                                    Atend.Acad.Ribbon.EquipmentRibbon();
                                                    break;
                                                case 2:
                                                    //ed.WriteMessage("2\n");
                                                    //Atend.Menu.ShowEquipmentPallete();
                                                    break;
                                                case 3:
                                                    //ed.WriteMessage("3\n");
                                                    //Atend.Menu.ShowEquipmentPallete();
                                                    //ed.WriteMessage("You  Have Access To ChngeDefault\n");
                                                    Atend.Control.Common.AccessChangeDefault = true;
                                                    break;
                                                case 4:
                                                    //ed.WriteMessage("4\n");
                                                    Atend.Acad.Ribbon.MechanicalCalculationRibbon();
                                                    break;
                                                case 5:
                                                    //ed.WriteMessage("5\n");
                                                    Atend.Acad.Ribbon.ReportingRibbon();
                                                    break;
                                                case 6:
                                                    // ed.WriteMessage("6\n");
                                                    break;
                                                case 7:
                                                    //ed.WriteMessage("7\n");
                                                    break;
                                                case 8:
                                                    // ed.WriteMessage("8\n");
                                                    break;
                                                case 9:
                                                    //ed.WriteMessage("9\n");
                                                    Atend.Acad.Ribbon.ManagementRibbon();
                                                    EnableRibbonButton("ID_DataManagement", "btnDefineUser");
                                                    break;
                                                case 10:
                                                    ed.WriteMessage("10\n");
                                                    EnableRibbonButton("ID_DataManagement", "btnRemarkSetting");
                                                    Atend.Control.Common.AccessProductPallet = true;
                                                    m.CreatePalleteOne();
                                                    break;
                                            }
                                        }
                                    }
                                    else if (Atend.Control.Common.IsClassicView)
                                    {
                                        //start tool bar

                                        AcadApplication acadApp = (AcadApplication)Application.AcadApplication;
                                        AcadToolbar toolbar = acadApp.MenuGroups.Item(0).Toolbars.Add("ToolBar");
                                        //GetResPath path = new GetResPath();

                                        AcadToolbarItem tbBut = toolbar.AddToolbarButton(0, "first", "first", "first\n", false);
                                        //tbBut.SetBitmaps(path.GetresPath() + "first.bmp", path.GetresPath() + "first.bmp");

                                        tbBut = toolbar.AddToolbarButton(1, "second", "second", "second\n", false);
                                        //tbBut.SetBitmaps(path.GetresPath() + "second.bmp", path.GetresPath() + "second.bmp");
                                        toolbar.Dock(Autodesk.AutoCAD.Interop.Common.AcToolbarDockStatus.acToolbarDockRight);


                                    }
                                }
                            }
                            else
                            {
                                Atend.Global.Acad.Notification notification = new Atend.Global.Acad.Notification();
                                notification.Title = "ورود";
                                notification.Msg = "کاربر قبلاً به سیستم وارد شده";
                                notification.infoCenterBalloon();
                            }
                        }
                        else
                        {
                            Atend.Global.Acad.Notification notification = new Atend.Global.Acad.Notification();
                            notification.Title = "پایگاه داده محلی";
                            notification.Msg = "اتصال به پایگاه داده محلی برقرار نشد";
                            notification.infoCenterBalloon();
                        }
                    }
                    
                }
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("ERROR : " + ex.Message);
            }
            */


            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            DocManForUI MyDocManForUI = new DocManForUI();
            MyDocManForUI.Do();
            Menu m = new Menu();
            if (Atend.Control.ConnectionString.ConnectionValidation(Atend.Control.ConnectionString.LocalcnString))
            {
                if (Atend.Control.Common.userCode == 0)
                {
                    Atend.Design.frmLogin FrmLogin = new Atend.Design.frmLogin();
                    if (Application.ShowModalDialog(FrmLogin) == System.Windows.Forms.DialogResult.OK)
                    {
                        if (!Atend.Control.Common.IsClassicView)
                        {
                            Atend.Acad.Ribbon.DesignRibbon();
                            EnableRibbonButton("ID_Setting", "btnShowAccess");
                            //EnableRibbonButton("ID_Setting", "btnRemarkSetting");
                            EnableRibbonButton("ID_Setting", "btnBSetting");
                            foreach (System.Data.DataRow dr in Atend.Control.Common.AccessList.Rows)
                            {
                                switch (Convert.ToInt32(dr["IdAccess"]))
                                {
                                    case 1:
                                        //ed.WriteMessage("1\n");
                                        Atend.Acad.Ribbon.EquipmentRibbon();
                                        break;
                                    case 2:
                                        //ed.WriteMessage("2\n");
                                        //Atend.Menu.ShowEquipmentPallete();
                                        break;
                                    case 3:
                                        //ed.WriteMessage("3\n");
                                        //Atend.Menu.ShowEquipmentPallete();
                                        //ed.WriteMessage("You  Have Access To ChngeDefault\n");
                                        Atend.Control.Common.AccessChangeDefault = true;
                                        break;
                                    case 4:
                                        //ed.WriteMessage("4\n");
                                        Atend.Acad.Ribbon.MechanicalCalculationRibbon();
                                        break;
                                    case 5:
                                        //ed.WriteMessage("5\n");
                                        Atend.Acad.Ribbon.ReportingRibbon();
                                        break;
                                    case 6:
                                        // ed.WriteMessage("6\n");
                                        break;
                                    case 7:
                                        //ed.WriteMessage("7\n");
                                        break;
                                    case 8:
                                        // ed.WriteMessage("8\n");
                                        break;
                                    case 9:
                                        //ed.WriteMessage("9\n");
                                        Atend.Acad.Ribbon.ManagementRibbon();
                                        EnableRibbonButton("ID_DataManagement", "btnDefineUser");
                                        break;
                                    case 10:
                                        //ed.WriteMessage("10\n");
                                        EnableRibbonButton("ID_DataManagement", "btnRemarkSetting");
                                        Atend.Control.Common.AccessProductPallet = true;
                                        m.CreatePalleteOne();
                                        break;
                                }
                            }
                        }
                        //else if (Atend.Control.Common.IsClassicView)
                        //{
                        //    //start tool bar

                        //    AcadApplication acadApp = (AcadApplication)Application.AcadApplication;
                        //    AcadToolbar toolbar = acadApp.MenuGroups.Item(0).Toolbars.Add("ToolBar");
                        //    //GetResPath path = new GetResPath();

                        //    AcadToolbarItem tbBut = toolbar.AddToolbarButton(0, "first", "first", "first\n", false);
                        //    //tbBut.SetBitmaps(path.GetresPath() + "first.bmp", path.GetresPath() + "first.bmp");

                        //    tbBut = toolbar.AddToolbarButton(1, "second", "second", "second\n", false);
                        //    //tbBut.SetBitmaps(path.GetresPath() + "second.bmp", path.GetresPath() + "second.bmp");
                        //    toolbar.Dock(Autodesk.AutoCAD.Interop.Common.AcToolbarDockStatus.acToolbarDockRight);


                        //}
                    }
                }
                else
                {
                    Atend.Global.Acad.Notification notification = new Atend.Global.Acad.Notification();
                    notification.Title = "ورود";
                    notification.Msg = "کاربر قبلاً به سیستم وارد شده";
                    notification.infoCenterBalloon();
                }
            }
            else
            {
                Atend.Global.Acad.Notification notification = new Atend.Global.Acad.Notification();
                notification.Title = "پایگاه داده محلی";
                notification.Msg = "اتصال به پایگاه داده محلی برقرار نشد";
                notification.infoCenterBalloon();
            }

        }

        //////[CommandMethod("AtOpen", CommandFlags.Session)]
        //////public void OpenDesign()
        //////{
        //////    if (Atend.Control.Common.userCode != 0)
        //////    {
        //////        Atend.Design.frmOpenDesign02 frmOpenDesign = new Atend.Design.frmOpenDesign02();
        //////        if (Application.ShowModalDialog(frmOpenDesign) == System.Windows.Forms.DialogResult.OK)
        //////        {
        //////        }
        //////    }
        //////}

        //[CommandMethod("AtNew")]
        //public void NewDesign()
        //{
        //    if (Atend.Control.Common.userCode != 0)
        //    {
        //        Atend.Design.frmNewDesign02 frmNewDesin = new Atend.Design.frmNewDesign02();
        //        if (Autodesk.AutoCAD.ApplicationServices.Application.ShowModalDialog(frmNewDesin) == System.Windows.Forms.DialogResult.OK)
        //        {
        //            //DocManForUI MyDocManForUI = new DocManForUI();
        //            //MyDocManForUI.Do();

        //        }
        //    }
        //}

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//

        // Define some constants we'll use to

        // store our XData

        // AppName is our RDS (TTIF, for

        // "Through The InterFace") plus an indicator

        // what it's for (ROTation)

        const string kRegAppName = "TTIF_ROT";

        const int kAppCode = 1001;

        const int kRotCode = 1040;

        class RotateJig : EntityJig
        {

            // Declare some internal state

            double m_baseAngle, m_deltaAngle;

            Point3d m_rotationPoint;

            Matrix3d m_ucs;

            // Constructor sets the state and clones

            // the entity passed in

            // (adequate for simple entities)

            public RotateJig(

              Entity ent,

              Point3d rotationPoint,

              double baseAngle,

              Matrix3d ucs)

                : base(ent.Clone() as Entity)
            {

                m_rotationPoint = rotationPoint;

                m_baseAngle = baseAngle;

                m_ucs = ucs;

            }

            protected override SamplerStatus Sampler(

              JigPrompts jp

            )
            {

                // We acquire a single angular value

                JigPromptAngleOptions jo =

                  new JigPromptAngleOptions(

                    "\nAngle of rotation: "

                  );

                jo.BasePoint = m_rotationPoint;

                jo.UseBasePoint = true;

                PromptDoubleResult pdr =

                  jp.AcquireAngle(jo);

                if (pdr.Status == PromptStatus.OK)
                {

                    // Check if it has changed or not

                    // (reduces flicker)

                    if (m_deltaAngle == pdr.Value)
                    {

                        return SamplerStatus.NoChange;

                    }

                    else
                    {

                        // Set the change in angle to

                        // the new value

                        m_deltaAngle = pdr.Value;

                        return SamplerStatus.OK;

                    }

                }

                return SamplerStatus.Cancel;

            }

            protected override bool Update()
            {

                // We rotate the polyline by the change

                // minus the base angle

                Matrix3d trans =

                  Matrix3d.Rotation(

                    m_deltaAngle - m_baseAngle,

                    m_ucs.CoordinateSystem3d.Zaxis,

                    m_rotationPoint);

                Entity.TransformBy(trans);

                // The base becomes the previous delta

                // and the delta gets set to zero

                m_baseAngle = m_deltaAngle;

                m_deltaAngle = 0.0;

                return true;

            }

            public Entity GetEntity()
            {

                return Entity;

            }

            public double GetRotation()
            {

                // The overall rotation is the

                // base plus the delta

                return m_baseAngle + m_deltaAngle;

            }

        }

        [CommandMethod("ROT")]

        public void RotateEntity()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            Database db = doc.Database;
            ed.WriteMessage("1\n");
            // First we prompt for the entity to rotate
            PromptEntityOptions peo = new PromptEntityOptions("\nSelect entity to rotate: ");
            PromptEntityResult per = ed.GetEntity(peo);
            ed.WriteMessage("2\n");
            if (per.Status == PromptStatus.OK)
            {
                Transaction tr = db.TransactionManager.StartTransaction();
                using (tr)
                {
                    ed.WriteMessage("3\n");
                    DBObject obj = tr.GetObject(per.ObjectId, OpenMode.ForRead);
                    Entity ent = obj as Entity;

                    // Use the origin as the default center
                    Point3d rotationPoint = Point3d.Origin;
                    // If the entity is a polyline,
                    // assume it is rectangular and then
                    // set the rotation point as its center
                    ed.WriteMessage("4\n");
                    Polyline pl = obj as Polyline;
                    if (pl != null)
                    {
                        ed.WriteMessage("5\n");
                        LineSegment3d ps0 = pl.GetLineSegmentAt(0);
                        LineSegment3d ps1 = pl.GetLineSegmentAt(1);
                        Vector3d vec = ((ps0.EndPoint - ps0.StartPoint) / 2.0) + ((ps1.EndPoint - ps1.StartPoint) / 2.0);
                        rotationPoint = pl.StartPoint + vec;
                        ed.WriteMessage("6\n");
                    }
                    ed.WriteMessage("7\n");
                    // Get the base rotation angle stored with the
                    // entity, if there was one (default is 0.0)
                    double baseAngle = GetStoredRotation(obj);
                    if (ent != null)
                    {
                        ed.WriteMessage("8\n");
                        // Get the current UCS, to pass to the Jig
                        Matrix3d ucs = ed.CurrentUserCoordinateSystem;
                        // Create our jig object
                        RotateJig jig = new RotateJig(ent, rotationPoint, baseAngle, ucs);
                        PromptResult res = ed.Drag(jig);
                        ed.WriteMessage("9\n");
                        if (res.Status == PromptStatus.OK)
                        {
                            ed.WriteMessage("10\n");
                            // Get the overall rotation angle
                            // and dispose of the temp clone
                            double newAngle = jig.GetRotation();
                            jig.GetEntity().Dispose();
                            // Rotate the original entity
                            Matrix3d trans = Matrix3d.Rotation(newAngle - baseAngle, ucs.CoordinateSystem3d.Zaxis,
                                    rotationPoint);
                            ed.WriteMessage("11\n");
                            ent.UpgradeOpen();
                            ent.TransformBy(trans);
                            // Store the new rotation as XData
                            SetStoredRotation(ent, newAngle);
                            ed.WriteMessage("12\n");
                        }
                    }
                    ed.WriteMessage("13\n");
                    tr.Commit();
                }
            }
        }

        // Helper function to create a RegApp

        static void AddRegAppTableRecord(string regAppName)
        {

            Document doc = Application.DocumentManager.MdiActiveDocument;

            Editor ed = doc.Editor;

            Database db = doc.Database;

            Transaction tr = db.TransactionManager.StartTransaction();

            using (tr)
            {

                RegAppTable rat =

                  (RegAppTable)tr.GetObject(

                    db.RegAppTableId,

                    OpenMode.ForRead,

                    false

                  );

                if (!rat.Has(regAppName))
                {

                    rat.UpgradeOpen();

                    RegAppTableRecord ratr =

                      new RegAppTableRecord();

                    ratr.Name = regAppName;

                    rat.Add(ratr);

                    tr.AddNewlyCreatedDBObject(ratr, true);

                    ResultBuffer rb = new ResultBuffer(
           new TypedValue(1001, "PARISA"),
           new TypedValue(1000, "CODE HERE ....")
           );
                    ratr.XData = rb;


                    rb.Dispose();

                }

                tr.Commit();

            }

        }

        // Store our rotation angle as XData

        private void SetStoredRotation(

          DBObject obj, double rotation)
        {

            AddRegAppTableRecord(kRegAppName);

            ResultBuffer rb = obj.XData;

            if (rb == null)
            {

                rb =

                  new ResultBuffer(

                    new TypedValue(kAppCode, kRegAppName),

                    new TypedValue(kRotCode, rotation)

                  );

            }

            else
            {

                // We can simply add our values - no need

                // to remove the previous ones, the new ones

                // are the ones that get stored

                rb.Add(new TypedValue(kAppCode, kRegAppName));

                rb.Add(new TypedValue(kRotCode, rotation));

            }

            obj.XData = rb;

            rb.Dispose();

        }

        // Retrieve the existing rotation angle from XData

        private double GetStoredRotation(DBObject obj)
        {

            double ret = 0.0;

            ResultBuffer rb = obj.XData;

            if (rb != null)
            {

                // If we find our group code, it means that on

                // the next iteration, we'll get our rotation

                bool bReadyForRot = false;

                foreach (TypedValue tv in rb)
                {

                    if (bReadyForRot)
                    {

                        if (tv.TypeCode == kRotCode)

                            ret = (double)tv.Value;

                        bReadyForRot = false;

                    }

                    if (tv.TypeCode == kAppCode)

                        bReadyForRot = true;

                }

                rb.Dispose();

            }

            return ret;

        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        [CommandMethod("GFH")]
        static public void GradientFillHatch()
        {

            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;

            // Ask the user to select a hatch boundary

            PromptEntityOptions opt = new PromptEntityOptions("\nSelect boundary: ");
            opt.SetRejectMessage("\nObject must be a curve.");
            opt.AddAllowedClass(typeof(Curve), false);
            PromptEntityResult res = ed.GetEntity(opt);
            if (res.Status == PromptStatus.OK)
            {
                Transaction tr = doc.TransactionManager.StartTransaction();
                using (tr)
                {
                    // Check the entity is a closed curve
                    DBObject obj = tr.GetObject(res.ObjectId, OpenMode.ForRead);
                    Curve cur = obj as Curve;
                    if (cur != null && cur.Closed == false)
                    {
                        ed.WriteMessage("\nLoop must be a closed curve.");
                    }
                    else
                    {
                        // We'll add the hatch to the model space
                        BlockTable bt = (BlockTable)tr.GetObject(doc.Database.BlockTableId, OpenMode.ForRead);
                        BlockTableRecord btr = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);
                        Hatch hat = new Hatch();
                        hat.SetDatabaseDefaults();
                        // Firstly make it clear we want a gradient fill
                        hat.HatchObjectType = HatchObjectType.GradientObject;
                        //Let's use the pre-defined spherical gradient
                        //LINEAR, CYLINDER, INVCYLINDER, SPHERICAL, INVSPHERICAL, HEMISPHERICAL, INVHEMISPHERICAL, CURVED, and INVCURVED. 
                        hat.SetGradient(GradientPatternType.PreDefinedGradient, "LINEAR");
                        // We're defining two colours
                        hat.GradientOneColorMode = false;
                        GradientColor[] gcs = new GradientColor[2];
                        // First colour must have value of 0
                        gcs[0] = new GradientColor(Color.FromRgb(0, 0, 255), 0);
                        // Second colour must have value of 1
                        gcs[1] = new GradientColor(Color.FromRgb(47, 165, 208), 1);
                        hat.SetGradientColors(gcs);
                        // Add the hatch to the model space
                        // and the transaction
                        ObjectId hatId = btr.AppendEntity(hat);
                        tr.AddNewlyCreatedDBObject(hat, true);
                        // Add the hatch loop and complete the hatch
                        ObjectIdCollection ids = new ObjectIdCollection();
                        ids.Add(res.ObjectId);
                        hat.Associative = true;
                        hat.AppendLoop(HatchLoopTypes.Default, ids);
                        hat.EvaluateHatch(true);
                        tr.Commit();
                    }
                }
            }
        }

        [CommandMethod("AddRegion")]
        public static void AddRegion()
        {


            LineSegment3d ls = new LineSegment3d();



            // Get the current document and database

            Document acDoc = Application.DocumentManager.MdiActiveDocument;

            Database acCurDb = acDoc.Database;

            // Start a transaction

            using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
            {

                // Open the Block table for read

                BlockTable acBlkTbl;

                acBlkTbl = acTrans.GetObject(acCurDb.BlockTableId,

                                             OpenMode.ForRead) as BlockTable;



                // Open the Block table record Model space for write

                BlockTableRecord acBlkTblRec;

                acBlkTblRec = acTrans.GetObject(acBlkTbl[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

                // Create an in memory circle

                using (Circle acCirc = new Circle())
                {

                    acCirc.SetDatabaseDefaults();

                    acCirc.Center = new Point3d(2, 2, 0);

                    acCirc.Radius = 5;

                    // Adds the circle to an object array

                    DBObjectCollection acDBObjColl = new DBObjectCollection();

                    acDBObjColl.Add(acCirc);

                    // Calculate the regions based on each closed loop

                    Line myLine = new Line(new Point3d(0, 0, 0), new Point3d(100, 100, 0));

                    Point3dCollection P3C = new Point3dCollection();

                    DBObjectCollection myRegionColl = new DBObjectCollection();

                    myRegionColl = Region.CreateFromCurves(acDBObjColl);

                    Region acRegion = myRegionColl[0] as Region;

                    acRegion.ColorIndex = 140;

                    acRegion.IntersectWith(myLine, Intersect.OnBothOperands, P3C, 0, 0);

                    Application.ShowAlertDialog("Count: " + P3C.Count.ToString() + P3C[0].ToString());

                    // Add the new object to the block table record and the transaction

                    acBlkTblRec.AppendEntity(acRegion);

                    acTrans.AddNewlyCreatedDBObject(acRegion, true);

                    // Dispose of the in memory circle not appended to the database

                }

                // Save the new object to the database

                acTrans.Commit();

            }

        }

    }//end of command


    #region MyCode

    //public class Commands001
    //{

    //    [CommandMethod("OSS")]

    //    static public void OffscreenSnapshot()
    //    {

    //        CreateSphere();

    //        SnapshotToFile(

    //          "c:\\sphere-Wireframe2D.png",

    //          VisualStyleType.Wireframe2D

    //        );

    //        SnapshotToFile(

    //          "c:\\sphere-Hidden.png",

    //          VisualStyleType.Hidden

    //        );

    //        SnapshotToFile(

    //          "c:\\sphere-Basic.png",

    //          VisualStyleType.Basic

    //        );

    //        SnapshotToFile(

    //          "c:\\sphere-ColorChange.png",

    //          VisualStyleType.ColorChange

    //        );

    //        SnapshotToFile(

    //          "c:\\sphere-Conceptual.png",

    //          VisualStyleType.Conceptual

    //        );

    //        SnapshotToFile(

    //          "c:\\sphere-Flat.png",

    //          VisualStyleType.Flat

    //        );

    //        SnapshotToFile(

    //          "c:\\sphere-Gouraud.png",

    //          VisualStyleType.Gouraud

    //        );

    //        SnapshotToFile(

    //          "c:\\sphere-Realistic.png",

    //          VisualStyleType.Realistic

    //        );

    //    }

    //    static public void CreateSphere()
    //    {

    //        Document doc =

    //          Application.DocumentManager.MdiActiveDocument;

    //        Database db = doc.Database;

    //        Editor ed = doc.Editor;

    //        Transaction tr =

    //          doc.TransactionManager.StartTransaction();

    //        using (tr)
    //        {

    //            BlockTable bt =

    //              (BlockTable)tr.GetObject(

    //                db.BlockTableId,

    //                OpenMode.ForRead

    //              );

    //            BlockTableRecord btr =

    //              (BlockTableRecord)tr.GetObject(

    //                bt[BlockTableRecord.ModelSpace],

    //                OpenMode.ForWrite

    //              );

    //            Solid3d sol = new Solid3d();

    //            sol.CreateSphere(10.0);

    //            const string matname =

    //              "Sitework.Paving - Surfacing.Riverstone.Mortared";

    //            DBDictionary matdict =

    //              (DBDictionary)tr.GetObject(

    //                db.MaterialDictionaryId,

    //                OpenMode.ForRead

    //              );

    //            if (matdict.Contains(matname))
    //            {

    //                sol.Material = matname;

    //            }

    //            else
    //            {

    //                ed.WriteMessage(

    //                  "\nMaterial (" + matname + ") not found" +

    //                  " - sphere will be rendered without it.",

    //                  matname

    //                );

    //            }

    //            btr.AppendEntity(sol);

    //            tr.AddNewlyCreatedDBObject(sol, true);

    //            tr.Commit();

    //        }

    //        AcadApplication acadApp =

    //          (AcadApplication)Application.AcadApplication;

    //        acadApp.ZoomExtents();

    //    }

    //    static public void SnapshotToFile(

    //      string filename,

    //      VisualStyleType vst

    //    )
    //    {

    //        Document doc =

    //          Application.DocumentManager.MdiActiveDocument;

    //        Editor ed = doc.Editor;

    //        Database db = doc.Database;
    //        Autodesk.AutoCAD.Windows.ToolPalette.ToolPaletteManager.Manager gsm = doc.GraphicsManager;

    //        // Get some AutoCAD system variables

    //        int vpn =

    //          System.Convert.ToInt32(

    //            Application.GetSystemVariable("CVPORT")

    //          );

    //        // Get AutoCAD's GS view for this document...

    //        View gsv =

    //          doc.GraphicsManager.GetGsView(vpn, true);

    //        // ... but create a new one for the actual snapshot

    //        using (View view = new View())
    //        {

    //            // Set the view to be just like the one

    //            // in the AutoCAD editor

    //            view.Viewport = gsv.Viewport;

    //            view.SetView(

    //              gsv.Position,

    //              gsv.Target,

    //              gsv.UpVector,

    //              gsv.FieldWidth,

    //              gsv.FieldHeight

    //            );

    //            // Set the visual style to the one passed in

    //            view.VisualStyle = new VisualStyle(vst);

    //            Device dev =

    //              gsm.CreateAutoCADOffScreenDevice();

    //            using (dev)
    //            {

    //                dev.OnSize(gsm.DisplaySize);

    //                // Set the render type and the background color

    //                dev.DeviceRenderType = RendererType.Default;

    //                dev.BackgroundColor = Color.White;

    //                // Add the view to the device and update it

    //                dev.Add(view);

    //                dev.Update();

    //                using (Model model = gsm.CreateAutoCADModel())
    //                {

    //                    Transaction tr =

    //                      db.TransactionManager.StartTransaction();

    //                    using (tr)
    //                    {

    //                        // Add the modelspace to the view

    //                        // It's a container but also a drawable

    //                        BlockTable bt =

    //                          (BlockTable)tr.GetObject(

    //                            db.BlockTableId,

    //                            OpenMode.ForRead

    //                          );

    //                        BlockTableRecord btr =

    //                          (BlockTableRecord)tr.GetObject(

    //                            bt[BlockTableRecord.ModelSpace],

    //                            OpenMode.ForRead

    //                          );

    //                        view.Add(btr, model);

    //                        tr.Commit();

    //                    }

    //                    // Take the snapshot

    //                    Rectangle rect = view.Viewport;

    //                    using (Bitmap bitmap = view.GetSnapshot(rect))
    //                    {

    //                        bitmap.Save(filename);

    //                        ed.WriteMessage(

    //                          "\nSnapshot image saved to: " +

    //                          filename

    //                        );

    //                        // Clean up

    //                        view.EraseAll();

    //                        dev.Erase(view);

    //                    }

    //                }

    //            }

    //        }

    //    }

    //}

    #endregion



    public class DocManForUI
    {

        Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;

        Document m_docMan;

        DocumentCollection myDocuments;

        Database db = Application.DocumentManager.MdiActiveDocument.Database;

        string ActivatedCommand = "";

        public DocManForUI()
        {
            m_docMan = Application.DocumentManager.MdiActiveDocument;
            myDocuments = Application.DocumentManager;
        }

        public void Do()
        {

            try
            {

                m_docMan.CommandWillStart += new CommandEventHandler(m_docMan_CommandWillStart);
                m_docMan.CommandEnded += new CommandEventHandler(m_docMan_CommandEnded);

                myDocuments.DocumentCreated += new DocumentCollectionEventHandler(myDocuments_DocumentCreated);
                myDocuments.DocumentActivated += new DocumentCollectionEventHandler(myDocuments_DocumentActivated);
                myDocuments.DocumentLockModeChanged += new DocumentLockModeChangedEventHandler(myDocuments_DocumentLockModeChanged);
                myDocuments.DocumentCreated += new DocumentCollectionEventHandler(myDocuments_DocumentCreated);
                //myDocuments.DocumentActivated+=new DocumentCollectionEventHandler(myDocuments_DocumentActivated);


                ed.DraggingEnded += new DraggingEndedEventHandler(ed_DraggingEnded);
                ed.SelectionAdded += new SelectionAddedEventHandler(ed_SelectionAdded);
                ed.PointMonitor += new PointMonitorEventHandler(ed_PointMonitor);
                ed.PromptedForPoint += new PromptPointResultEventHandler(ed_PromptedForPoint);
                ed.Dragging += new DraggingEventHandler(ed_Dragging);


            }
            catch (System.Exception ex)
            {
                ed.WriteMessage(string.Format("Error DocManForUI.Do : {0} \n", ex.Message));
            }
        }

        //~~~~~~~~~~~~ editor events ~~~~~~~~~~~~~~~~~//

        void ed_Dragging(object sender, DraggingEventArgs e)
        {
            //throw new System.Exception("The method or operation is not implemented.");
            //ed.WriteMessage(" dragging {0} \n",e.Prompt);
        }

        void ed_PromptedForPoint(object sender, PromptPointResultEventArgs e)
        {
            //throw new System.Exception("The method or operation is not implemented.");
            //ed.WriteMessage("Point picked \n");
        }

        void ed_PointMonitor(object sender, PointMonitorEventArgs e)
        {
            string ToolTipStrig = "";
            FullSubentityPath[] paths = e.Context.GetPickedEntities();
            foreach (FullSubentityPath path in paths)
            {
                ObjectId[] ids = path.GetObjectIds();
                if (ids.Length > 0)
                {
                    ObjectId id = ids[ids.GetUpperBound(0)];
                    Atend.Base.Acad.AT_INFO EntityInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(id);
                    if (EntityInfo.ParentCode != "NONE")
                    {
                        switch ((Atend.Control.Enum.ProductType)EntityInfo.NodeType)
                        {
                            case Atend.Control.Enum.ProductType.Pole:
                                ToolTipStrig = "پایه";
                                break;
                            case Atend.Control.Enum.ProductType.Conductor:
                                ToolTipStrig = "سیم";
                                break;
                            case Atend.Control.Enum.ProductType.Consol:
                                ToolTipStrig = "دسته مقره";
                                break;
                        }
                    }
                }
            }
            e.AppendToolTipText(ToolTipStrig);
        }

        void ed_SelectionAdded(object sender, SelectionAddedEventArgs e)
        {
            //throw new System.Exception("The method or operation is not implemented.");
            //////if (e.AddedObjects.Count == 1)
            //////{
            //////    ed.WriteMessage(string.Format("Selected id : {0} \n", Convert.ToInt64(e.AddedObjects[0].ObjectId.ToString().Substring(1, e.AddedObjects[0].ObjectId.ToString().Length - 2))));

            //////    //Database db = HostApplicationServices.WorkingDatabase;
            //////    //using (Transaction tr = db.TransactionManager.StartTransaction())
            //////    //{

            //////    //    BlockTable bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForWrite);
            //////    //    BlockTableRecord btr = (BlockTableRecord)tr.GetObject(e.AddedObjects[0].ObjectId, OpenMode.ForWrite);

            //////    //    //foreach (ObjectId id in btr)
            //////    //    //{
            //////    //    //    id.o
            //////    //    //}

            //////    //}
            //////}

        }

        void ed_DraggingEnded(object sender, DraggingEndedEventArgs e)
        {
            //throw new System.Exception("The method or operation is not implemented.");
            //ed.WriteMessage(string.Format("Drop point is : X : {0} , Y : {1} offSet : {2} \n",e.PickPoint.X , e.PickPoint.Y , e.Offset.ToString()));
            //Atend.AcadDrawNode acadDrawNode = new AcadDrawNode();
            //if (acadDrawNode.Update())
            //{
            //    // all modified done success fully
            //}
            //else
            //{
            //    // some failure while updating node information
            //}

            //if (!Application.DocumentManager.MdiActiveDocument.Editor.IsDragging)
            //{
            //    //find right line if it has
            //    Atend.Base.Design.DNode dNode = Atend.Base.Design.DNode.SelectByAutocadCodeDesignCode(Atend.Control.Common.AutoCadId, Atend.Control.Common.SelectedDesignCode);
            //    ed.WriteMessage("Dnode done 000111000 \n");
            //    if (dNode.Code != null)
            //    {
            //        ed.WriteMessage("Select Entity Dnode.Code is : " + dNode.Code.ToString() + "\n");

            //        Atend.Base.Design.DBranch RightdBranch = Atend.Base.Design.DBranch.SelectByLeftNodeCode(dNode.Code);
            //        ed.WriteMessage("DBranch leftNodeCode done 000111000 \n");

            //        Atend.Base.Design.DBranch LeftdBranch = Atend.Base.Design.DBranch.SelectByRigthNodeCode(dNode.Code);
            //        ed.WriteMessage("DBranch RigthNodeCode done 000111000 \n");

            //        if (RightdBranch.Code != null)
            //        {
            //            ed.WriteMessage("the AutocadId of rigth branch is : " + RightdBranch.AutocadCode + "\n");
            //        }

            //        if (LeftdBranch.Code != null)
            //        {
            //            ed.WriteMessage("the AutocadId of Left branch is : " + LeftdBranch.AutocadCode + "\n");
            //        }


            //    }
            //}
        }

        //~~~~~~~~~~~~ database events ~~~~~~~~~~~~~~~~~//

        Atend.Base.Acad.AT_INFO SelectedEntityInfo = new Atend.Base.Acad.AT_INFO();

        Atend.Base.Acad.AT_SUB SelectedEntitySub = new Atend.Base.Acad.AT_SUB();

        void db_ObjectOpenedForModify(object sender, ObjectEventArgs e)
        {

            //ed.WriteMessage("i am in db_ObjectOpenedForModify : {0} \n", ActivatedCommand);
            try
            {

                //ed.WriteMessage("@@@@@@@@@ :{0}\n", ActivatedCommand);
                switch (ActivatedCommand)
                {
                    case "DROPGEOM":
                        #region DROPGEOM
                        ObjectId GOI = Atend.Global.Acad.UAcad.GetEntityGroup(e.DBObject.ObjectId);
                        if (GOI == ObjectId.Null)
                        {
                            Atend.Base.Acad.AT_INFO CurrentInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(e.DBObject.ObjectId);
                            //ed.WriteMessage("________:{0}\n", CurrentInfo.NodeType);
                            if (CurrentInfo.ParentCode != "NONE")
                            {

                                //Atend.Acad.AcadMove.EntityInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(e.DBObject.ObjectId);
                                //Atend.Acad.AcadMove.EntitySub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(e.DBObject.ObjectId);

                                Atend.Global.Acad.AcadMove.AllowToMove = true;
                                //ed.WriteMessage("AllowToMove = true\n");
                                switch ((Atend.Control.Enum.ProductType)CurrentInfo.NodeType)
                                {
                                    case Atend.Control.Enum.ProductType.Pole:

                                        //ed.WriteMessage("POLE \n");
                                        Atend.Global.Acad.AcadMove.EntityInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(e.DBObject.ObjectId);
                                        Atend.Global.Acad.AcadMove.EntitySub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(e.DBObject.ObjectId);

                                        Atend.Global.Acad.AcadMove.PoleOI = e.DBObject.ObjectId;
                                        Atend.Global.Acad.AcadMove.CurrentEntity = Atend.Global.Acad.UAcad.GetEntityByObjectID(e.DBObject.ObjectId);
                                        Atend.Global.Acad.AcadMove.LastCenterPoint =
                                            Atend.Global.Acad.UAcad.CenterOfEntity(Atend.Global.Acad.UAcad.GetEntityByObjectID(e.DBObject.ObjectId));
                                        Atend.Global.Acad.AcadMove.swBreaker = true;
                                        Atend.Global.Acad.AcadMove.swDisconnector = true;
                                        Atend.Global.Acad.AcadMove.swCatOut = true;
                                        //ed.WriteMessage("~~~~~~~~~Atend.Global.Acad.AcadMove.LastCenterPoint : {0} \n", Atend.Global.Acad.AcadMove.LastCenterPoint);
                                        break;

                                    case Atend.Control.Enum.ProductType.PoleTip:

                                        //ed.WriteMessage("POLETIP \n");
                                        Atend.Global.Acad.AcadMove.EntityInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(e.DBObject.ObjectId);
                                        Atend.Global.Acad.AcadMove.EntitySub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(e.DBObject.ObjectId);

                                        Atend.Global.Acad.AcadMove.PoleTipOI = e.DBObject.ObjectId;
                                        Atend.Global.Acad.AcadMove.CurrentEntity = Atend.Global.Acad.UAcad.GetEntityByObjectID(e.DBObject.ObjectId);
                                        Atend.Global.Acad.AcadMove.LastCenterPoint =
                                            Atend.Global.Acad.UAcad.CenterOfEntity(Atend.Global.Acad.UAcad.GetEntityByObjectID(e.DBObject.ObjectId));
                                        Atend.Global.Acad.AcadMove.swBreaker = true;
                                        Atend.Global.Acad.AcadMove.swDisconnector = true;
                                        Atend.Global.Acad.AcadMove.swCatOut = true;
                                        //ed.WriteMessage("~~~~~~~~~Atend.Global.Acad.AcadMove.LastCenterPoint : {0} \n", Atend.Global.Acad.AcadMove.LastCenterPoint);
                                        break;

                                    case Atend.Control.Enum.ProductType.Consol:
                                        //ed.WriteMessage("Consol \n");
                                        Atend.Global.Acad.AcadMove.EntityInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(e.DBObject.ObjectId);
                                        Atend.Global.Acad.AcadMove.EntitySub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(e.DBObject.ObjectId);


                                        Atend.Global.Acad.AcadMove.ConsolOI = e.DBObject.ObjectId;
                                        Atend.Global.Acad.AcadMove.CurrentEntity = Atend.Global.Acad.UAcad.GetEntityByObjectID(e.DBObject.ObjectId);

                                        Polyline lastPoly = Atend.Global.Acad.UAcad.GetEntityByObjectID(e.DBObject.ObjectId) as Polyline;
                                        Entity CurrentPoleEntity = null;
                                        if (lastPoly != null)
                                        {
                                            Polyline pl = new Polyline();
                                            if (lastPoly != null)
                                            {
                                                for (int i = 0; i < lastPoly.NumberOfVertices; i++)
                                                {

                                                    //ed.WriteMessage("last poly : {0} \n", lastPoly.GetPoint2dAt(i));
                                                    pl.AddVertexAt(pl.NumberOfVertices, lastPoly.GetPoint2dAt(i), 0, 0, 0);

                                                }
                                                CurrentPoleEntity = pl;
                                            }

                                        }
                                        else
                                        {
                                            Circle lastPoly1 = Atend.Global.Acad.UAcad.GetEntityByObjectID(e.DBObject.ObjectId) as Circle;
                                            if (lastPoly1 != null)
                                            {
                                                Circle pl = new Circle();
                                                if (lastPoly1 != null)
                                                {
                                                    pl.Center = lastPoly1.Center;
                                                    pl.Radius = lastPoly1.Radius;
                                                    CurrentPoleEntity = pl;
                                                }
                                            }

                                        }
                                        Atend.Global.Acad.AcadMove.CurrentEntity = CurrentPoleEntity;
                                        //ed.WriteMessage("Atend.Global.Acad.AcadMove.ConsolOI : {0}\n", Atend.Global.Acad.AcadMove.ConsolOI);
                                        Atend.Global.Acad.AcadMove.LastCenterPoint =
                                            Atend.Global.Acad.UAcad.CenterOfEntity(Atend.Global.Acad.UAcad.GetEntityByObjectID(e.DBObject.ObjectId));
                                        //ed.WriteMessage("~~~~~~~~~Atend.Global.Acad.AcadMove.LastCenterPoint : {0} \n", Atend.Global.Acad.AcadMove.LastCenterPoint);
                                        break;


                                    case Atend.Control.Enum.ProductType.ConsolElse: //BankKhazan
                                        //ed.WriteMessage("AAAAAAAAEntitySub.SubIdCollection :{0}\n", Atend.Global.Acad.AcadMove.EntitySub.SubIdCollection.Count);
                                        Atend.Global.Acad.AcadMove.EntityInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(e.DBObject.ObjectId);
                                        Atend.Global.Acad.AcadMove.EntitySub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(e.DBObject.ObjectId);

                                        Atend.Global.Acad.AcadMove.LastCenterPoint =
                                            Atend.Global.Acad.UAcad.CenterOfEntity(Atend.Global.Acad.UAcad.GetEntityByObjectID(e.DBObject.ObjectId));

                                        Polyline lastPoly11 = Atend.Global.Acad.UAcad.GetEntityByObjectID(e.DBObject.ObjectId) as Polyline;
                                        Entity CurrentPoleEntity1 = null;
                                        if (lastPoly11 != null)
                                        {
                                            //ed.WriteMessage("~~~~ Consol was see as poly line ~~~~~\n");
                                            Polyline pl = new Polyline();
                                            if (lastPoly11 != null)
                                            {
                                                for (int i = 0; i < lastPoly11.NumberOfVertices; i++)
                                                {

                                                    //ed.WriteMessage("last poly : {0} \n", lastPoly11.GetPoint2dAt(i));
                                                    pl.AddVertexAt(pl.NumberOfVertices, lastPoly11.GetPoint2dAt(i), 0, 0, 0);

                                                }
                                                CurrentPoleEntity1 = pl;
                                            }

                                            Atend.Global.Acad.AcadMove.CurrentEntity = CurrentPoleEntity1;// Atend.Global.Acad.UAcad.GetEntityByObjectID(e.DBObject.ObjectId);

                                            Atend.Global.Acad.AcadMove.BankKhazanOI = e.DBObject.ObjectId;
                                            //ed.WriteMessage("\n~~~~~~~~~~~~~~~~~~\n");

                                        }

                                        break;


                                    /*case Atend.Control.Enum.ProductType.Rod:
                                        ed.WriteMessage("rod \n");
                                        ed.WriteMessage("XXXXXXXXXXXXXXXXXXXX\n");
                                        Atend.Global.Acad.AcadMove.EntityInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(e.DBObject.ObjectId);
                                        Atend.Global.Acad.AcadMove.EntitySub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(e.DBObject.ObjectId);

                                        Atend.Global.Acad.AcadMove.LastCenterPoint =
                                            Atend.Global.Acad.UAcad.CenterOfEntity(Atend.Global.Acad.UAcad.GetEntityByObjectID(e.DBObject.ObjectId));

                                        Polyline lastPoly12 = Atend.Global.Acad.UAcad.GetEntityByObjectID(e.DBObject.ObjectId) as Polyline;
                                        Entity CurrentPoleEntity2 = null;
                                        if (lastPoly12 != null)
                                        {
                                            ed.WriteMessage("~~~~ Consol was see as poly line ~~~~~\n");
                                            Polyline pl1 = new Polyline();
                                            if (lastPoly12 != null)
                                            {
                                                for (int i = 0; i < lastPoly12.NumberOfVertices; i++)
                                                {

                                                    ed.WriteMessage("last poly : {0} \n", lastPoly12.GetPoint2dAt(i));
                                                    pl1.AddVertexAt(pl1.NumberOfVertices, lastPoly12.GetPoint2dAt(i), 0, 0, 0);

                                                }
                                                CurrentPoleEntity2 = pl1;
                                            }

                                            Atend.Global.Acad.AcadMove.CurrentEntity = CurrentPoleEntity2;

                                            Atend.Global.Acad.AcadMove.RodOI = e.DBObject.ObjectId;
                                            ed.WriteMessage("\n~~~~~~~~~~~~~~~~~~\n");

                                        }

                                        break;
                                       */

                                    case Atend.Control.Enum.ProductType.GroundPost:
                                        //ed.WriteMessage("11GGGGGGGEntitySub.SubIdCollection :{0}\n", Atend.Global.Acad.AcadMove.EntitySub.SubIdCollection.Count);
                                        Atend.Global.Acad.AcadMove.EntityInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(e.DBObject.ObjectId);
                                        Atend.Global.Acad.AcadMove.EntitySub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(e.DBObject.ObjectId);

                                        Atend.Global.Acad.AcadMove.LastCenterPoint =
                                            Atend.Global.Acad.UAcad.CenterOfEntity(Atend.Global.Acad.UAcad.GetEntityByObjectID(e.DBObject.ObjectId));


                                        Polyline lastPolyGP = Atend.Global.Acad.UAcad.GetEntityByObjectID(e.DBObject.ObjectId) as Polyline;
                                        Entity CurrentPoleEntityGP = null;
                                        if (lastPolyGP != null)
                                        {
                                            //ed.WriteMessage("~~~~ Consol was see as poly line ~~~~~\n");
                                            Polyline pGP = new Polyline();
                                            if (lastPolyGP != null)
                                            {
                                                for (int i = 0; i < lastPolyGP.NumberOfVertices; i++)
                                                {

                                                    //ed.WriteMessage("last poly : {0} \n", lastPolyGP.GetPoint2dAt(i));
                                                    pGP.AddVertexAt(pGP.NumberOfVertices, lastPolyGP.GetPoint2dAt(i), 0, 0, 0);

                                                }
                                                CurrentPoleEntityGP = pGP;
                                            }

                                            Atend.Global.Acad.AcadMove.CurrentEntity = CurrentPoleEntityGP;// Atend.Global.Acad.UAcad.GetEntityByObjectID(e.DBObject.ObjectId);

                                            Atend.Global.Acad.AcadMove.GroundPostOI = e.DBObject.ObjectId;
                                            //ed.WriteMessage("\n~~~~~~~~~~~~~~~~~~\n");

                                        }
                                        //Atend.Global.Acad.AcadMove.CurrentEntity = Atend.Global.Acad.UAcad.GetEntityByObjectID(e.DBObject.ObjectId);
                                        //Atend.Global.Acad.AcadMove.BankKhazanOI = e.DBObject.ObjectId;

                                        break;

                                    case Atend.Control.Enum.ProductType.AirPost:
                                        //ed.WriteMessage("11aaaaaaaaairEntitySub.SubIdCollection :{0}\n", Atend.Global.Acad.AcadMove.EntitySub.SubIdCollection.Count);
                                        Atend.Global.Acad.AcadMove.EntityInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(e.DBObject.ObjectId);
                                        Atend.Global.Acad.AcadMove.EntitySub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(e.DBObject.ObjectId);

                                        Atend.Global.Acad.AcadMove.LastCenterPoint =
                                            Atend.Global.Acad.UAcad.CenterOfEntity(Atend.Global.Acad.UAcad.GetEntityByObjectID(e.DBObject.ObjectId));

                                        Polyline lastPolyAir = Atend.Global.Acad.UAcad.GetEntityByObjectID(e.DBObject.ObjectId) as Polyline;
                                        Entity CurrentPoleEntityAir = null;
                                        if (lastPolyAir != null)
                                        {
                                            Polyline pAir = new Polyline();
                                            if (lastPolyAir != null)
                                            {
                                                for (int i = 0; i < lastPolyAir.NumberOfVertices; i++)
                                                {
                                                    pAir.AddVertexAt(pAir.NumberOfVertices, lastPolyAir.GetPoint2dAt(i), 0, 0, 0);

                                                }
                                                CurrentPoleEntityAir = pAir;
                                            }
                                            Atend.Global.Acad.AcadMove.CurrentEntity = CurrentPoleEntityAir;
                                            Atend.Global.Acad.AcadMove.AirPostOI = e.DBObject.ObjectId;

                                        }

                                        break;

                                    case Atend.Control.Enum.ProductType.StreetBox:
                                        //ed.WriteMessage("11ssssssssstreetboxEntitySub.SubIdCollection :{0}\n", Atend.Global.Acad.AcadMove.EntitySub.SubIdCollection.Count);
                                        Atend.Global.Acad.AcadMove.EntityInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(e.DBObject.ObjectId);
                                        Atend.Global.Acad.AcadMove.EntitySub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(e.DBObject.ObjectId);

                                        Atend.Global.Acad.AcadMove.LastCenterPoint =
                                            Atend.Global.Acad.UAcad.CenterOfEntity(Atend.Global.Acad.UAcad.GetEntityByObjectID(e.DBObject.ObjectId));

                                        Polyline lastPolystreet = Atend.Global.Acad.UAcad.GetEntityByObjectID(e.DBObject.ObjectId) as Polyline;
                                        Entity CurrentPoleEntitystreet = null;
                                        if (lastPolystreet != null)
                                        {
                                            Polyline pstreet = new Polyline();
                                            if (lastPolystreet != null)
                                            {
                                                for (int i = 0; i < lastPolystreet.NumberOfVertices; i++)
                                                {
                                                    pstreet.AddVertexAt(pstreet.NumberOfVertices, lastPolystreet.GetPoint2dAt(i), 0, 0, 0);

                                                }
                                                CurrentPoleEntitystreet = pstreet;
                                            }
                                            Atend.Global.Acad.AcadMove.CurrentEntity = CurrentPoleEntitystreet;
                                            Atend.Global.Acad.AcadMove.StreetBoxOI = e.DBObject.ObjectId;

                                        }
                                        break;

                                    case Atend.Control.Enum.ProductType.DB:
                                        Atend.Global.Acad.AcadMove.EntityInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(e.DBObject.ObjectId);
                                        Atend.Global.Acad.AcadMove.EntitySub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(e.DBObject.ObjectId);

                                        Atend.Global.Acad.AcadMove.LastCenterPoint =
                                            Atend.Global.Acad.UAcad.CenterOfEntity(Atend.Global.Acad.UAcad.GetEntityByObjectID(e.DBObject.ObjectId));

                                        Polyline lastPolyDB = Atend.Global.Acad.UAcad.GetEntityByObjectID(e.DBObject.ObjectId) as Polyline;
                                        Entity CurrentPoleEntityDB = null;
                                        if (lastPolyDB != null)
                                        {
                                            Polyline pstreet = new Polyline();
                                            if (lastPolyDB != null)
                                            {
                                                for (int i = 0; i < lastPolyDB.NumberOfVertices; i++)
                                                {
                                                    pstreet.AddVertexAt(pstreet.NumberOfVertices, lastPolyDB.GetPoint2dAt(i), 0, 0, 0);

                                                }
                                                CurrentPoleEntityDB = pstreet;
                                            }
                                            Atend.Global.Acad.AcadMove.CurrentEntity = CurrentPoleEntityDB;
                                            Atend.Global.Acad.AcadMove.DBOI = e.DBObject.ObjectId;

                                        }
                                        break;

                                    case Atend.Control.Enum.ProductType.HeaderCabel:
                                        //ed.WriteMessage("11HHHederCabelEntitySub.SubIdCollection :{0}\n", Atend.Global.Acad.AcadMove.EntitySub.SubIdCollection.Count);
                                        Atend.Global.Acad.AcadMove.EntityInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(e.DBObject.ObjectId);
                                        Atend.Global.Acad.AcadMove.EntitySub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(e.DBObject.ObjectId);

                                        Atend.Global.Acad.AcadMove.LastCenterPoint =
                                            Atend.Global.Acad.UAcad.CenterOfEntity(Atend.Global.Acad.UAcad.GetEntityByObjectID(e.DBObject.ObjectId));

                                        Polyline lastPolyheader = Atend.Global.Acad.UAcad.GetEntityByObjectID(e.DBObject.ObjectId) as Polyline;
                                        Entity CurrentPoleEntityheader = null;
                                        if (lastPolyheader != null)
                                        {
                                            Polyline pheader = new Polyline();
                                            if (lastPolyheader != null)
                                            {
                                                for (int i = 0; i < lastPolyheader.NumberOfVertices; i++)
                                                {
                                                    pheader.AddVertexAt(pheader.NumberOfVertices, lastPolyheader.GetPoint2dAt(i), 0, 0, 0);

                                                }
                                                CurrentPoleEntityheader = pheader;
                                            }
                                            Atend.Global.Acad.AcadMove.CurrentEntity = CurrentPoleEntityheader;
                                            Atend.Global.Acad.AcadMove.HeaderCabelOI = e.DBObject.ObjectId;

                                        }

                                        break;

                                    case Atend.Control.Enum.ProductType.Mafsal:
                                        //ed.WriteMessage("11MMMafsalEntitySub.SubIdCollection :{0}\n", Atend.Global.Acad.AcadMove.EntitySub.SubIdCollection.Count);
                                        Atend.Global.Acad.AcadMove.EntityInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(e.DBObject.ObjectId);
                                        Atend.Global.Acad.AcadMove.EntitySub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(e.DBObject.ObjectId);

                                        Atend.Global.Acad.AcadMove.LastCenterPoint =
                                            Atend.Global.Acad.UAcad.CenterOfEntity(Atend.Global.Acad.UAcad.GetEntityByObjectID(e.DBObject.ObjectId));

                                        Atend.Global.Acad.AcadMove.CurrentEntity = Atend.Global.Acad.UAcad.GetEntityByObjectID(e.DBObject.ObjectId); //CurrentPoleEntitymafsal;
                                        Atend.Global.Acad.AcadMove.MafsalOI = e.DBObject.ObjectId;


                                        break;



                                    case Atend.Control.Enum.ProductType.Kalamp:

                                        //ed.WriteMessage("Kalamp \n");
                                        Atend.Global.Acad.AcadMove.EntityInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(e.DBObject.ObjectId);
                                        Atend.Global.Acad.AcadMove.EntitySub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(e.DBObject.ObjectId);

                                        Atend.Global.Acad.AcadMove.LastCenterPoint =
                                            Atend.Global.Acad.UAcad.CenterOfEntity(Atend.Global.Acad.UAcad.GetEntityByObjectID(e.DBObject.ObjectId));

                                        Polyline lastPolykalamp = Atend.Global.Acad.UAcad.GetEntityByObjectID(e.DBObject.ObjectId) as Polyline;
                                        Entity CurrentPoleEntitykalamp = null;
                                        if (lastPolykalamp != null)
                                        {
                                            Polyline pkalamp = new Polyline();
                                            if (lastPolykalamp != null)
                                            {
                                                for (int i = 0; i < lastPolykalamp.NumberOfVertices; i++)
                                                {
                                                    pkalamp.AddVertexAt(pkalamp.NumberOfVertices, lastPolykalamp.GetPoint2dAt(i), 0, 0, 0);

                                                }
                                                CurrentPoleEntitykalamp = pkalamp;
                                            }
                                            Atend.Global.Acad.AcadMove.CurrentEntity = CurrentPoleEntitykalamp;
                                            Atend.Global.Acad.AcadMove.KalampOI = e.DBObject.ObjectId;

                                        }
                                        break;

                                    case Atend.Control.Enum.ProductType.KablSho:
                                        Atend.Global.Acad.AcadMove.EntityInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(e.DBObject.ObjectId);
                                        Atend.Global.Acad.AcadMove.EntitySub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(e.DBObject.ObjectId);


                                        Atend.Global.Acad.AcadMove.LastCenterPoint =
                                            Atend.Global.Acad.UAcad.CenterOfEntity(Atend.Global.Acad.UAcad.GetEntityByObjectID(e.DBObject.ObjectId));

                                        Polyline lastPolykablsho = Atend.Global.Acad.UAcad.GetEntityByObjectID(e.DBObject.ObjectId) as Polyline;
                                        Entity CurrentPoleEntitykablsho = null;
                                        if (lastPolykablsho != null)
                                        {
                                            Polyline pkablsho = new Polyline();
                                            if (lastPolykablsho != null)
                                            {
                                                for (int i = 0; i < lastPolykablsho.NumberOfVertices; i++)
                                                {
                                                    pkablsho.AddVertexAt(pkablsho.NumberOfVertices, lastPolykablsho.GetPoint2dAt(i), 0, 0, 0);

                                                }
                                                CurrentPoleEntitykablsho = pkablsho;
                                            }
                                            Atend.Global.Acad.AcadMove.CurrentEntity = CurrentPoleEntitykablsho;
                                            Atend.Global.Acad.AcadMove.KablShoOI = e.DBObject.ObjectId;

                                        }
                                        break;

                                    case Atend.Control.Enum.ProductType.Light:
                                        Atend.Global.Acad.AcadMove.EntityInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(e.DBObject.ObjectId);
                                        Atend.Global.Acad.AcadMove.EntitySub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(e.DBObject.ObjectId);

                                        Atend.Global.Acad.AcadMove.LightOI = e.DBObject.ObjectId;
                                        Atend.Global.Acad.AcadMove.CurrentEntity = Atend.Global.Acad.UAcad.GetEntityByObjectID(e.DBObject.ObjectId);
                                        Atend.Global.Acad.AcadMove.LastCenterPoint =
                                            Atend.Global.Acad.UAcad.CenterOfEntity(Atend.Global.Acad.UAcad.GetEntityByObjectID(e.DBObject.ObjectId));
                                        break;

                                    case Atend.Control.Enum.ProductType.MeasuredJackPanel:
                                        Atend.Global.Acad.AcadMove.EntityInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(e.DBObject.ObjectId);
                                        Atend.Global.Acad.AcadMove.EntitySub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(e.DBObject.ObjectId);
                                        Atend.Global.Acad.AcadMove.MeasuredJackPanelOI = e.DBObject.ObjectId;
                                        Atend.Global.Acad.AcadMove.CurrentEntity = Atend.Global.Acad.UAcad.GetEntityByObjectID(e.DBObject.ObjectId);
                                        Atend.Global.Acad.AcadMove.LastCenterPoint =
                                            Atend.Global.Acad.UAcad.CenterOfEntity(Atend.Global.Acad.UAcad.GetEntityByObjectID(e.DBObject.ObjectId));
                                        break;

                                    case Atend.Control.Enum.ProductType.Ground:
                                        Atend.Global.Acad.AcadMove.EntityInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(e.DBObject.ObjectId);
                                        Atend.Global.Acad.AcadMove.EntitySub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(e.DBObject.ObjectId);

                                        Atend.Global.Acad.AcadMove.GroundOI = e.DBObject.ObjectId;
                                        Atend.Global.Acad.AcadMove.CurrentEntity = Atend.Global.Acad.UAcad.GetEntityByObjectID(e.DBObject.ObjectId);
                                        Atend.Global.Acad.AcadMove.LastCenterPoint =
                                            Atend.Global.Acad.UAcad.CenterOfEntity(Atend.Global.Acad.UAcad.GetEntityByObjectID(e.DBObject.ObjectId));
                                        break;

                                    case Atend.Control.Enum.ProductType.Transformer:
                                        //ed.WriteMessage("TRANSSSSSSSSSSFORMER\n");
                                        //Atend.Global.Acad.AcadMove.EntityInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(e.DBObject.ObjectId);
                                        //Atend.Global.Acad.AcadMove.EntitySub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(e.DBObject.ObjectId);
                                        //Atend.Global.Acad.AcadMove.ProductType = Convert.ToInt32(Atend.Control.Enum.ProductType.Transformer);
                                        //Atend.Global.Acad.AcadMove.TransformerOI = e.DBObject.ObjectId;
                                        //Atend.Global.Acad.AcadMove.CurrentEntity = Atend.Global.Acad.UAcad.GetEntityByObjectID(e.DBObject.ObjectId);
                                        //Atend.Global.Acad.AcadMove.LastCenterPoint =
                                        //    Atend.Global.Acad.UAcad.CenterOfEntity(Atend.Global.Acad.UAcad.GetEntityByObjectID(e.DBObject.ObjectId));

                                        //ObjectIdCollection Collection = Atend.Global.Acad.UAcad.GetGroupSubEntities(Atend.Global.Acad.UAcad.GetEntityGroup(e.DBObject.ObjectId));
                                        //foreach (ObjectId collect in Collection)
                                        //{
                                        //    Atend.Base.Acad.AT_INFO Info = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(collect);
                                        //    if (Info.ParentCode != "NONE" && Info.NodeType == (int)Atend.Control.Enum.ProductType.KablSho)
                                        //    {
                                        //        Atend.Global.Acad.AcadMove.LastCenterPointKL = Atend.Global.Acad.UAcad.CenterOfEntity(Atend.Global.Acad.UAcad.GetEntityByObjectID(collect));
                                        //    }
                                        //}
                                        break;


                                    ////case Atend.Control.Enum.ProductType.GroundCabel:
                                    ////    ed.WriteMessage("Cabbbbbblllllll++++++++++++++++++\n");
                                    ////    //////////////////
                                    ////    Atend.Global.Acad.AcadMove.EntityInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(e.DBObject.ObjectId);
                                    ////    Atend.Global.Acad.AcadMove.EntitySub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(e.DBObject.ObjectId);
                                    ////    Atend.Global.Acad.AcadMove.CabelOI = e.DBObject.ObjectId;
                                    ////    Atend.Global.Acad.AcadMove.CurrentEntity = Atend.Global.Acad.UAcad.GetEntityByObjectID(e.DBObject.ObjectId);
                                    ////    Atend.Global.Acad.AcadMove.LastCenterPoint =
                                    ////        Atend.Global.Acad.UAcad.CenterOfEntity(Atend.Global.Acad.UAcad.GetEntityByObjectID(e.DBObject.ObjectId));
                                    ////    //////////////////

                                    ////    ed.WriteMessage("end cable\n");
                                    ////    break;
                                }

                            }
                            else
                            {
                                Atend.Global.Acad.AcadMove.AllowToMove = false;
                                //ed.WriteMessage("OBJECT WAS NOT FOUND \n");
                            }
                        }
                        else
                        {
                            Atend.Base.Acad.AT_INFO CurrentInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(GOI);
                            if (CurrentInfo.ParentCode != "NONE")
                            {
                                Atend.Global.Acad.AcadMove.AllowToMove = true;
                                switch ((Atend.Control.Enum.ProductType)CurrentInfo.NodeType)
                                {
                                    case Atend.Control.Enum.ProductType.Transformer:
                                        //ed.WriteMessage("TRANSSSSSSSSSSFORMER\n");
                                        Atend.Global.Acad.AcadMove.EntityInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(e.DBObject.ObjectId);
                                        Atend.Global.Acad.AcadMove.EntitySub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(e.DBObject.ObjectId);
                                        Atend.Global.Acad.AcadMove.ProductType = Convert.ToInt32(Atend.Control.Enum.ProductType.Transformer);
                                        Atend.Global.Acad.AcadMove.TransformerOI = e.DBObject.ObjectId;
                                        Atend.Global.Acad.AcadMove.CurrentEntity = Atend.Global.Acad.UAcad.GetEntityByObjectID(e.DBObject.ObjectId);
                                        Atend.Global.Acad.AcadMove.LastCenterPoint =
                                            Atend.Global.Acad.UAcad.CenterOfEntity(Atend.Global.Acad.UAcad.GetEntityByObjectID(e.DBObject.ObjectId));

                                        ObjectIdCollection Collection = Atend.Global.Acad.UAcad.GetGroupSubEntities(Atend.Global.Acad.UAcad.GetEntityGroup(e.DBObject.ObjectId));
                                        foreach (ObjectId collect in Collection)
                                        {
                                            Atend.Base.Acad.AT_INFO Info = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(collect);
                                            if (Info.ParentCode != "NONE" && Info.NodeType == (int)Atend.Control.Enum.ProductType.KablSho)
                                            {
                                                Atend.Global.Acad.AcadMove.LastCenterPointKL = Atend.Global.Acad.UAcad.CenterOfEntity(Atend.Global.Acad.UAcad.GetEntityByObjectID(collect));
                                            }
                                        }
                                        break;

                                    case Atend.Control.Enum.ProductType.MiddleJackPanel:
                                        //ed.WriteMessage("MMMMMMMMMMMMiddleJackPanel\n");
                                        Atend.Global.Acad.AcadMove.EntityInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(e.DBObject.ObjectId);
                                        Atend.Global.Acad.AcadMove.EntitySub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(e.DBObject.ObjectId);
                                        Atend.Global.Acad.AcadMove.ProductType = Convert.ToInt32(Atend.Control.Enum.ProductType.Transformer);
                                        Atend.Global.Acad.AcadMove.MiddleJackPanelOI = e.DBObject.ObjectId;
                                        Atend.Global.Acad.AcadMove.CurrentEntity = Atend.Global.Acad.UAcad.GetEntityByObjectID(e.DBObject.ObjectId);
                                        Atend.Global.Acad.AcadMove.LastCenterPoint =
                                            Atend.Global.Acad.UAcad.CenterOfEntity(Atend.Global.Acad.UAcad.GetEntityByObjectID(e.DBObject.ObjectId));

                                        ObjectIdCollection Collection2 = Atend.Global.Acad.UAcad.GetGroupSubEntities(Atend.Global.Acad.UAcad.GetEntityGroup(e.DBObject.ObjectId));
                                        foreach (ObjectId collect2 in Collection2)
                                        {
                                            Atend.Base.Acad.AT_INFO Info = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(collect2);
                                            if (Info.ParentCode != "NONE" && Info.NodeType == (int)Atend.Control.Enum.ProductType.HeaderCabel)
                                            {
                                                Atend.Global.Acad.AcadMove.LastCenterPointKL = Atend.Global.Acad.UAcad.CenterOfEntity(Atend.Global.Acad.UAcad.GetEntityByObjectID(collect2));
                                            }
                                        }
                                        break;

                                    case Atend.Control.Enum.ProductType.WeekJackPanel:
                                        //ed.WriteMessage("WWWWWWWWWWWeekJackPanel\n");
                                        Atend.Global.Acad.AcadMove.EntityInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(e.DBObject.ObjectId);
                                        Atend.Global.Acad.AcadMove.EntitySub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(e.DBObject.ObjectId);
                                        Atend.Global.Acad.AcadMove.ProductType = Convert.ToInt32(Atend.Control.Enum.ProductType.Transformer);
                                        Atend.Global.Acad.AcadMove.WeekJackPanelOI = e.DBObject.ObjectId;
                                        Atend.Global.Acad.AcadMove.CurrentEntity = Atend.Global.Acad.UAcad.GetEntityByObjectID(e.DBObject.ObjectId);
                                        Atend.Global.Acad.AcadMove.LastCenterPoint =
                                            Atend.Global.Acad.UAcad.CenterOfEntity(Atend.Global.Acad.UAcad.GetEntityByObjectID(e.DBObject.ObjectId));

                                        ObjectIdCollection Collection3 = Atend.Global.Acad.UAcad.GetGroupSubEntities(Atend.Global.Acad.UAcad.GetEntityGroup(e.DBObject.ObjectId));
                                        foreach (ObjectId collect3 in Collection3)
                                        {
                                            Atend.Base.Acad.AT_INFO Info = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(collect3);
                                            if (Info.ParentCode != "NONE" && Info.NodeType == (int)Atend.Control.Enum.ProductType.HeaderCabel)
                                            {
                                                Atend.Global.Acad.AcadMove.LastCenterPointKL = Atend.Global.Acad.UAcad.CenterOfEntity(Atend.Global.Acad.UAcad.GetEntityByObjectID(collect3));
                                            }
                                        }
                                        break;
                                }
                            }
                        }

                        #endregion
                        break;
                    case "MOVE":
                        ed.WriteMessage("MOVEEEEEEEEEEEEEEEE\n");
                        break;
                    case "ERASE":
                        Atend.Base.Acad.AT_INFO CurrentInfo1 = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(e.DBObject.ObjectId);
                        if (CurrentInfo1.ParentCode != "NONE")
                        {
                            Atend.Global.Acad.Notification notification = new Atend.Global.Acad.Notification();
                            notification.Title = "خطا در نرم افزار";
                            notification.Msg = "لطفا جهت حذف تجهیزات از دکمه 'حذف تجهیز' استفاده نمایید";
                            notification.infoCenterBalloon();
                        }
                        #region ERASE


                        //    Atend.Base.Acad.AT_INFO at_info = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(e.DBObject.ObjectId);
                        //    string S = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Database.Filename;
                        //    if (System.IO.File.Exists(S.Replace("DWG", "MDB")))
                        //    {
                        //        if (Atend.Control.Common.DesignName != "" && Atend.Control.Common.userCode != 0)
                        //        {
                        //            switch (((Atend.Control.Enum.ProductType)at_info.NodeType))
                        //            {
                        //                case Atend.Control.Enum.ProductType.Pole:
                        //                    db.ObjectOpenedForModify -= new ObjectEventHandler(db_ObjectOpenedForModify);
                        //                    if (Atend.Global.Acad.DrawEquips.AcDrawPole.DeletePoleData(e.DBObject.ObjectId))
                        //                    {
                        //                        if (!Atend.Global.Acad.DrawEquips.AcDrawPole.DeletePole(e.DBObject.ObjectId))
                        //                        {
                        //                            ed.WriteMessage("Error in delete draw\n");
                        //                        }
                        //                    }
                        //                    else
                        //                    {
                        //                        ed.WriteMessage("Error in delete data\n");
                        //                    }
                        //                    break;
                        //                case Atend.Control.Enum.ProductType.PoleTip:
                        //                    db.ObjectOpenedForModify -= new ObjectEventHandler(db_ObjectOpenedForModify);
                        //                    if (Atend.Global.Acad.DrawEquips.AcDrawPole.DeletePoleData(e.DBObject.ObjectId))
                        //                    {
                        //                        if (!Atend.Global.Acad.DrawEquips.AcDrawPole.DeletePole(e.DBObject.ObjectId))
                        //                        {
                        //                            ed.WriteMessage("Error in delete draw\n");
                        //                        }
                        //                    }
                        //                    else
                        //                    {
                        //                        ed.WriteMessage("Error in delete data\n");
                        //                    }
                        //                    break;

                        //                case Atend.Control.Enum.ProductType.Conductor:
                        //                    db.ObjectOpenedForModify -= new ObjectEventHandler(db_ObjectOpenedForModify);
                        //                    if (Atend.Global.Acad.DrawEquips.AcDrawConductor.DeleteConductorData(e.DBObject.ObjectId))
                        //                    {
                        //                        if (!Atend.Global.Acad.DrawEquips.AcDrawConductor.DeleteConductor(e.DBObject.ObjectId))
                        //                        {
                        //                            ed.WriteMessage("Error in delete draw\n");
                        //                        }
                        //                    }
                        //                    else
                        //                    {
                        //                        ed.WriteMessage("Error in delete data\n");
                        //                    }
                        //                    break;

                        //                case Atend.Control.Enum.ProductType.GroundCabel:
                        //                    db.ObjectOpenedForModify -= new ObjectEventHandler(db_ObjectOpenedForModify);
                        //                    if (Atend.Global.Acad.DrawEquips.AcDrawGroundCabel.DeleteGroundCabelData(e.DBObject.ObjectId))
                        //                    {
                        //                        if (!Atend.Global.Acad.DrawEquips.AcDrawGroundCabel.DeleteGroundCabel(e.DBObject.ObjectId))
                        //                        {
                        //                            ed.WriteMessage("Error in delete draw\n");
                        //                        }
                        //                    }
                        //                    else
                        //                    {
                        //                        ed.WriteMessage("Error in delete data\n");
                        //                    }
                        //                    break;

                        //                case Atend.Control.Enum.ProductType.SelfKeeper:
                        //                    db.ObjectOpenedForModify -= new ObjectEventHandler(db_ObjectOpenedForModify);
                        //                    if (Atend.Global.Acad.DrawEquips.AcDrawSelfKeeper.DeleteSelfKeeperData(e.DBObject.ObjectId))
                        //                    {
                        //                        if (!Atend.Global.Acad.DrawEquips.AcDrawSelfKeeper.DeleteSelfKeeper(e.DBObject.ObjectId))
                        //                        {
                        //                            ed.WriteMessage("Error in delete draw\n");
                        //                        }
                        //                    }
                        //                    else
                        //                    {
                        //                        ed.WriteMessage("Error in delete data\n");
                        //                    }
                        //                    break;

                        //                case Atend.Control.Enum.ProductType.Disconnector:
                        //                    db.ObjectOpenedForModify -= new ObjectEventHandler(db_ObjectOpenedForModify);
                        //                    if (Atend.Global.Acad.DrawEquips.AcDrawDisConnector.DeleteDisconnectorData(e.DBObject.ObjectId))
                        //                    {
                        //                        if (!Atend.Global.Acad.DrawEquips.AcDrawDisConnector.DeleteDisconnector(e.DBObject.ObjectId))
                        //                        {
                        //                            ed.WriteMessage("Error in delete draw\n");
                        //                        }
                        //                    }
                        //                    else
                        //                    {
                        //                        ed.WriteMessage("Error in delete data\n");
                        //                    }
                        //                    break;

                        //                case Atend.Control.Enum.ProductType.Breaker:
                        //                    db.ObjectOpenedForModify -= new ObjectEventHandler(db_ObjectOpenedForModify);
                        //                    if (Atend.Global.Acad.DrawEquips.AcDrawBreaker.DeleteBreakerData(e.DBObject.ObjectId))
                        //                    {
                        //                        if (!Atend.Global.Acad.DrawEquips.AcDrawBreaker.DeleteBreaker(e.DBObject.ObjectId))
                        //                        {
                        //                            ed.WriteMessage("Error in delete draw\n");
                        //                        }
                        //                    }
                        //                    else
                        //                    {
                        //                        ed.WriteMessage("Error in delete data\n");
                        //                    }
                        //                    break;

                        //                case Atend.Control.Enum.ProductType.CatOut:
                        //                    db.ObjectOpenedForModify -= new ObjectEventHandler(db_ObjectOpenedForModify);
                        //                    if (Atend.Global.Acad.DrawEquips.AcDrawCatOut.DeleteCatOutData(e.DBObject.ObjectId))
                        //                    {
                        //                        if (!Atend.Global.Acad.DrawEquips.AcDrawCatOut.DeleteCatOut(e.DBObject.ObjectId))
                        //                        {
                        //                            ed.WriteMessage("Error in delete draw\n");
                        //                        }
                        //                    }
                        //                    else
                        //                    {
                        //                        ed.WriteMessage("Error in delete data\n");
                        //                    }
                        //                    break;

                        //                case Atend.Control.Enum.ProductType.Jumper:
                        //                    db.ObjectOpenedForModify -= new ObjectEventHandler(db_ObjectOpenedForModify);
                        //                    if (Atend.Global.Acad.DrawEquips.AcDrawJumper.DeleteJumperData(e.DBObject.ObjectId))
                        //                    {
                        //                        if (!Atend.Global.Acad.DrawEquips.AcDrawJumper.DeleteJumper(e.DBObject.ObjectId))
                        //                        {
                        //                            ed.WriteMessage("Error in delete draw\n");
                        //                        }
                        //                    }
                        //                    else
                        //                    {
                        //                        ed.WriteMessage("Error in delete data\n");
                        //                    }
                        //                    break;

                        //                case Atend.Control.Enum.ProductType.ConsolElse:  //for Khazan And Rod
                        //                    db.ObjectOpenedForModify -= new ObjectEventHandler(db_ObjectOpenedForModify);
                        //                    ObjectId id = Atend.Global.Acad.UAcad.GetEntityGroup(e.DBObject.ObjectId);
                        //                    ObjectIdCollection Collection = Atend.Global.Acad.UAcad.GetGroupSubEntities(id);
                        //                    Atend.Base.Acad.AT_INFO atinfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(Collection[1]);
                        //                    if (atinfo.ParentCode != "NONE" && atinfo.NodeType == (int)Atend.Control.Enum.ProductType.BankKhazan)
                        //                    {
                        //                        if (Atend.Global.Acad.DrawEquips.AcDrawKhazan.DeleteKhazanData(e.DBObject.ObjectId))
                        //                        {
                        //                            if (!Atend.Global.Acad.DrawEquips.AcDrawKhazan.DeleteKhazan(e.DBObject.ObjectId))
                        //                            {
                        //                                ed.WriteMessage("Error in delete draw\n");
                        //                            }
                        //                        }
                        //                        else
                        //                        {
                        //                            ed.WriteMessage("Error in delete data\n");
                        //                        }
                        //                    }
                        //                    else
                        //                    {
                        //                        ed.WriteMessage("i am in ROD\n");
                        //                        if (Atend.Global.Acad.DrawEquips.AcDrawRod.DeleteRodData(e.DBObject.ObjectId))
                        //                        {
                        //                            if (!Atend.Global.Acad.DrawEquips.AcDrawRod.DeleteRod(e.DBObject.ObjectId))
                        //                            {
                        //                                ed.WriteMessage("Error in delete draw\n");
                        //                            }
                        //                        }
                        //                        else
                        //                        {
                        //                            ed.WriteMessage("Error in delete data\n");
                        //                        }
                        //                    }
                        //                    break;

                        //                case Atend.Control.Enum.ProductType.GroundPost:
                        //                    db.ObjectOpenedForModify -= new ObjectEventHandler(db_ObjectOpenedForModify);
                        //                    if (Atend.Global.Acad.DrawEquips.AcDrawGroundPost.DeleteGroundPostData(e.DBObject.ObjectId))
                        //                    {
                        //                        if (!Atend.Global.Acad.DrawEquips.AcDrawGroundPost.DeleteGroundPost(e.DBObject.ObjectId))
                        //                        {
                        //                            ed.WriteMessage("Error in delete draw\n");
                        //                        }
                        //                    }
                        //                    else
                        //                    {
                        //                        ed.WriteMessage("Error in delete data\n");
                        //                    }
                        //                    break;

                        //                case Atend.Control.Enum.ProductType.AirPost:
                        //                    db.ObjectOpenedForModify -= new ObjectEventHandler(db_ObjectOpenedForModify);
                        //                    if (Atend.Global.Acad.DrawEquips.AcDrawAirPost.DeleteAirPostData(e.DBObject.ObjectId))
                        //                    {
                        //                        if (!Atend.Global.Acad.DrawEquips.AcDrawAirPost.DeleteAirPost(e.DBObject.ObjectId))
                        //                        {
                        //                            ed.WriteMessage("Error in delete draw\n");
                        //                        }
                        //                    }
                        //                    else
                        //                    {
                        //                        ed.WriteMessage("Error in delete data\n");
                        //                    }
                        //                    break;

                        //                case Atend.Control.Enum.ProductType.HeaderCabel:
                        //                    db.ObjectOpenedForModify -= new ObjectEventHandler(db_ObjectOpenedForModify);
                        //                    if (Atend.Global.Acad.DrawEquips.AcDrawHeaderCabel.DeleteHeaderCabelData(e.DBObject.ObjectId))
                        //                    {
                        //                        if (!Atend.Global.Acad.DrawEquips.AcDrawHeaderCabel.DeleteHeaderCabel(e.DBObject.ObjectId))
                        //                        {
                        //                            ed.WriteMessage("Error in delete draw\n");
                        //                        }
                        //                    }
                        //                    else
                        //                    {
                        //                        ed.WriteMessage("Error in delete data\n");
                        //                    }
                        //                    break;

                        //                case Atend.Control.Enum.ProductType.Kalamp:
                        //                    db.ObjectOpenedForModify -= new ObjectEventHandler(db_ObjectOpenedForModify);
                        //                    if (Atend.Global.Acad.DrawEquips.AcDrawKalamp.DeleteKalampData(e.DBObject.ObjectId))
                        //                    {
                        //                        if (!Atend.Global.Acad.DrawEquips.AcDrawKalamp.DeleteKalamp(e.DBObject.ObjectId))
                        //                        {
                        //                            ed.WriteMessage("Error in delete draw\n");
                        //                        }
                        //                    }
                        //                    else
                        //                    {
                        //                        ed.WriteMessage("Error in delete data\n");
                        //                    }
                        //                    break;

                        //                case Atend.Control.Enum.ProductType.KablSho:
                        //                    db.ObjectOpenedForModify -= new ObjectEventHandler(db_ObjectOpenedForModify);
                        //                    if (Atend.Global.Acad.DrawEquips.AcDrawKablsho.DeleteKablshoData(e.DBObject.ObjectId))
                        //                    {
                        //                        if (!Atend.Global.Acad.DrawEquips.AcDrawKablsho.DeleteKablsho(e.DBObject.ObjectId))
                        //                        {
                        //                            ed.WriteMessage("Error in delete draw\n");
                        //                        }
                        //                    }
                        //                    else
                        //                    {
                        //                        ed.WriteMessage("Error in delete data\n");
                        //                    }
                        //                    break;

                        //                case Atend.Control.Enum.ProductType.StreetBox:
                        //                    db.ObjectOpenedForModify -= new ObjectEventHandler(db_ObjectOpenedForModify);
                        //                    if (Atend.Global.Acad.DrawEquips.AcDrawStreetBox.DeleteStreetBoxData(e.DBObject.ObjectId))
                        //                    {
                        //                        if (!Atend.Global.Acad.DrawEquips.AcDrawStreetBox.DeleteStreetBox(e.DBObject.ObjectId))
                        //                        {
                        //                            ed.WriteMessage("Error in delete draw\n");
                        //                        }
                        //                    }
                        //                    else
                        //                    {
                        //                        ed.WriteMessage("Error in delete data\n");
                        //                    }
                        //                    break;

                        //                case Atend.Control.Enum.ProductType.DB:
                        //                    db.ObjectOpenedForModify -= new ObjectEventHandler(db_ObjectOpenedForModify);
                        //                    if (Atend.Global.Acad.DrawEquips.AcDrawDB.DeleteDBData(e.DBObject.ObjectId))
                        //                    {
                        //                        if (!Atend.Global.Acad.DrawEquips.AcDrawDB.DeleteDB(e.DBObject.ObjectId))
                        //                        {
                        //                            ed.WriteMessage("Error in delete draw\n");
                        //                        }
                        //                    }
                        //                    else
                        //                    {
                        //                        ed.WriteMessage("Error in delete data\n");
                        //                    }
                        //                    break;

                        //                case Atend.Control.Enum.ProductType.Ground:
                        //                    db.ObjectOpenedForModify -= new ObjectEventHandler(db_ObjectOpenedForModify);
                        //                    if (Atend.Global.Acad.DrawEquips.AcDrawGround.DeleteGroundData(e.DBObject.ObjectId))
                        //                    {
                        //                        if (!Atend.Global.Acad.DrawEquips.AcDrawGround.DeleteGround(e.DBObject.ObjectId))
                        //                        {
                        //                            ed.WriteMessage("Error in delete draw\n");
                        //                        }
                        //                    }
                        //                    else
                        //                    {
                        //                        ed.WriteMessage("Error in delete data\n");
                        //                    }
                        //                    break;

                        //                case Atend.Control.Enum.ProductType.Light:
                        //                    db.ObjectOpenedForModify -= new ObjectEventHandler(db_ObjectOpenedForModify);
                        //                    if (Atend.Global.Acad.DrawEquips.AcDrawLight.DeleteLightData(e.DBObject.ObjectId))
                        //                    {
                        //                        if (!Atend.Global.Acad.DrawEquips.AcDrawLight.DeleteLight(e.DBObject.ObjectId))
                        //                        {
                        //                            ed.WriteMessage("Error in delete draw\n");
                        //                        }
                        //                    }
                        //                    else
                        //                    {
                        //                        ed.WriteMessage("Error in delete data\n");
                        //                    }
                        //                    break;

                        //                case Atend.Control.Enum.ProductType.Consol:
                        //                    db.ObjectOpenedForModify -= new ObjectEventHandler(db_ObjectOpenedForModify);
                        //                    if (Atend.Global.Acad.DrawEquips.AcDrawConsol.DeleteConsolData(e.DBObject.ObjectId))
                        //                    {
                        //                        if (!Atend.Global.Acad.DrawEquips.AcDrawConsol.DeleteConsol(e.DBObject.ObjectId))
                        //                        {
                        //                            ed.WriteMessage("Error in delete draw\n");
                        //                        }
                        //                    }
                        //                    else
                        //                    {
                        //                        ed.WriteMessage("Error in delete data\n");
                        //                    }
                        //                    break;

                        //                case Atend.Control.Enum.ProductType.MeasuredJackPanel:
                        //                    db.ObjectOpenedForModify -= new ObjectEventHandler(db_ObjectOpenedForModify);
                        //                    if (Atend.Global.Acad.DrawEquips.AcDrawMeasuredJackPanel.DeleteMeasuredJackPanelData(e.DBObject.ObjectId))
                        //                    {
                        //                        if (!Atend.Global.Acad.DrawEquips.AcDrawMeasuredJackPanel.DeleteMeasuredJackPanel(e.DBObject.ObjectId))
                        //                        {
                        //                            ed.WriteMessage("Error in delete draw\n");
                        //                        }
                        //                    }
                        //                    else
                        //                    {
                        //                        ed.WriteMessage("Error in delete data\n");
                        //                    }
                        //                    break;

                        //            }
                        //        }
                        //    }
                        #endregion
                        break;
                }
            }
            catch (System.Exception ex)
            {
                Atend.Global.Acad.AcadMove.AllowToMove = false;
                ed.WriteMessage("Error db_ObjectOpenedForModify : {0} \n", ex.Message);
            }

        }

        //void db_ObjectModified(object sender, ObjectEventArgs e)
        //{

        //}

        void m_docMan_CommandWillStart(object sender, CommandEventArgs e)
        {
            ActivatedCommand = e.GlobalCommandName;
            switch (e.GlobalCommandName)
            {
                case "ERASE":
                    //db.ObjectModified += new ObjectEventHandler(db_ObjectModified);
                    db.ObjectOpenedForModify += new ObjectEventHandler(db_ObjectOpenedForModify);

                    break;
                case "DROPGEOM":
                    //ed.WriteMessage("DROPGEOM will start \n");
                    //db.ObjectModified += new ObjectEventHandler(db_ObjectModified);
                    //ed.WriteMessage("ObjectModified activated \n");
                    db.ObjectOpenedForModify += new ObjectEventHandler(db_ObjectOpenedForModify);
                    //ed.WriteMessage("ObjectOpenedForModify activated \n");
                    break;
                //case "MOVE":
                //    ed.WriteMessage("MOVE will start \n");
                //    db.ObjectModified += new ObjectEventHandler(db_ObjectModified);
                //    db.ObjectOpenedForModify += new ObjectEventHandler(db_ObjectOpenedForModify);
                //    break;
                case "QSAVE":
                    string S = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Database.Filename;
                    if (System.IO.File.Exists(S.Replace("DWG", "MDB")))
                    {
                        if (Atend.Control.Common.DesignName != "" && Atend.Control.Common.userCode != 0)
                        {
                            Atend.Base.Acad.AT_COUNTER.SaveAll();
                        }
                    }
                    else
                    {
                        ed.WriteMessage("MDB WAS NOT EXIST\n");
                    }
                    break;


            }

        }

        void m_docMan_CommandEnded(object sender, CommandEventArgs e)
        {
            try
            {
                //db.ObjectModified -= new ObjectEventHandler(db_ObjectModified);
                db.ObjectOpenedForModify -= new ObjectEventHandler(db_ObjectOpenedForModify);
                //ed.WriteMessage("Handle Erased \n");


                ObjectId GroupID = Atend.Global.Acad.UAcad.GetEntityGroup(Atend.Global.Acad.AcadMove.EntityInfo.SelectedObjectId);
                if (GroupID == ObjectId.Null)
                {
                    switch (e.GlobalCommandName)
                    {
                        case "DROPGEOM":
                            #region Drop
                            //ed.WriteMessage("### {0}\n", Atend.Global.Acad.AcadMove.EntityInfo.NodeType);
                            //ed.WriteMessage("&&&&&&&&&&DROPGEOM command ended {0} \n", Atend.Global.Acad.AcadMove.EntityInfo.NodeType);
                            switch ((Atend.Control.Enum.ProductType)Atend.Global.Acad.AcadMove.EntityInfo.NodeType)
                            {
                                case Atend.Control.Enum.ProductType.Consol:
                                    //ed.WriteMessage("go for consol 1 \n");
                                    //ed.WriteMessage("PoleOI : {0} , PoleCode : {1} \n",
                                    //    Atend.Acad.UAcad.GetPoleByGuid(new Guid(Atend.Global.Acad.AcadMove.EntityInfo.ParentCode)),
                                    //    Atend.Global.Acad.AcadMove.EntityInfo.ParentCode);
                                    //ed.WriteMessage("Consol oi : {0} \n", Atend.Global.Acad.AcadMove.ConsolOI);
                                    ObjectId Pole = Atend.Global.Acad.UAcad.GetPoleByGuid(new Guid(Atend.Global.Acad.AcadMove.EntityInfo.ParentCode));
                                    Matrix3d m = new Matrix3d();
                                    Atend.Global.Acad.AcadMove.MoveConsol(Pole, Atend.Global.Acad.AcadMove.ConsolOI, m);
                                    //ed.WriteMessage("go for consol 2 \n");
                                    break;

                                case Atend.Control.Enum.ProductType.Pole:
                                    ed.WriteMessage("go for pole  \n");
                                    //PoleOI = Atend.Acad.UAcad.GetPoleByGuid(new Guid(Atend.Global.Acad.AcadMove.EntityInfo.ParentCode));
                                    Atend.Global.Acad.AcadMove.MovePole(Atend.Global.Acad.AcadMove.PoleOI);

                                    break;

                                case Atend.Control.Enum.ProductType.PoleTip:
                                    ed.WriteMessage("go for poleTip  \n");
                                    Atend.Global.Acad.AcadMove.MovePole(Atend.Global.Acad.AcadMove.PoleTipOI);
                                    break;

                                case Atend.Control.Enum.ProductType.ConsolElse:  //for Khazan And Rod
                                    //ed.WriteMessage("go for khazan or rod1 \n");
                                    ObjectId id = Atend.Global.Acad.UAcad.GetEntityGroup(Atend.Global.Acad.AcadMove.BankKhazanOI);
                                    ObjectIdCollection Collection = Atend.Global.Acad.UAcad.GetGroupSubEntities(id);
                                    Atend.Base.Acad.AT_INFO at_info = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(Collection[1]);
                                    if (at_info.ParentCode != "NONE" && at_info.NodeType == (int)Atend.Control.Enum.ProductType.BankKhazan)
                                        Atend.Global.Acad.AcadMove.MoveKhazan(Atend.Global.Acad.AcadMove.BankKhazanOI);
                                    else
                                        Atend.Global.Acad.AcadMove.MoveRod(Atend.Global.Acad.AcadMove.BankKhazanOI);

                                    //ed.WriteMessage("go for khazan or rod2 \n");

                                    break;

                                /*  case Atend.Control.Enum.ProductType.Rod:
                                    ed.WriteMessage("go for rod 1 \n");
                                    Atend.Global.Acad.AcadMove.MoveRod(Atend.Global.Acad.AcadMove.RodOI);
                                    ed.WriteMessage("go for rod 2 \n");
                                    break;*/
                                case Atend.Control.Enum.ProductType.GroundPost:
                                    //ed.WriteMessage("go for GroundPost1 \n");
                                    //ed.WriteMessage("22GGGGGGGEntitySub.SubIdCollection :{0},{1}\n", Atend.Global.Acad.AcadMove.EntitySub.SubIdCollection.Count, Atend.Global.Acad.AcadMove.GroundPostOI);
                                    Atend.Global.Acad.AcadMove.MoveGroundPost(Atend.Global.Acad.AcadMove.GroundPostOI);
                                    //ed.WriteMessage("go for GroundPost2 \n");
                                    break;

                                case Atend.Control.Enum.ProductType.AirPost:
                                    //ed.WriteMessage("go for AirPost1 \n");
                                    //ed.WriteMessage("22aaaairEntitySub.SubIdCollection :{0},{1}\n", Atend.Global.Acad.AcadMove.EntitySub.SubIdCollection.Count, Atend.Global.Acad.AcadMove.AirPostOI);
                                    Atend.Global.Acad.AcadMove.MoveAirPost(Atend.Global.Acad.AcadMove.AirPostOI);
                                    //ed.WriteMessage("go for AirPost2 \n");
                                    break;

                                case Atend.Control.Enum.ProductType.StreetBox:
                                    //ed.WriteMessage("go for StreetBox1 \n");
                                    //ed.WriteMessage("22sssssstreetboxEntitySub.SubIdCollection :{0},{1}\n", Atend.Global.Acad.AcadMove.EntitySub.SubIdCollection.Count, Atend.Global.Acad.AcadMove.StreetBoxOI);
                                    Atend.Global.Acad.AcadMove.MoveStreetBox(Atend.Global.Acad.AcadMove.StreetBoxOI);
                                    //ed.WriteMessage("go for streetbox2 \n");
                                    break;

                                case Atend.Control.Enum.ProductType.DB:
                                    Atend.Global.Acad.AcadMove.MoveDB(Atend.Global.Acad.AcadMove.DBOI);
                                    break;

                                case Atend.Control.Enum.ProductType.HeaderCabel:
                                    //ed.WriteMessage("go for HeaderCabel1 \n");
                                    Atend.Global.Acad.AcadMove.MoveHeaderCabelANDKablSho(Atend.Global.Acad.AcadMove.HeaderCabelOI);
                                    //ed.WriteMessage("go for HeaderCabel2 \n");
                                    break;

                                case Atend.Control.Enum.ProductType.Mafsal:
                                    //ed.WriteMessage("go for Mafsal1 \n");
                                    Atend.Global.Acad.AcadMove.MoveMafsal(Atend.Global.Acad.AcadMove.MafsalOI);
                                    //ed.WriteMessage("go for Mafsal2 \n");
                                    break;

                                case Atend.Control.Enum.ProductType.Kalamp:
                                    //PoleOI = Atend.Acad.UAcad.GetPoleByGuid(new Guid(Atend.Global.Acad.AcadMove.EntityInfo.ParentCode));
                                    Atend.Global.Acad.AcadMove.MoveKalamp(Atend.Global.Acad.AcadMove.KalampOI);

                                    break;

                                case Atend.Control.Enum.ProductType.KablSho:
                                    Atend.Global.Acad.AcadMove.MoveHeaderCabelANDKablSho(Atend.Global.Acad.AcadMove.KablShoOI);
                                    break;

                                case Atend.Control.Enum.ProductType.Light:
                                    Atend.Global.Acad.AcadMove.MoveLight(Atend.Global.Acad.AcadMove.LightOI);
                                    break;

                                case Atend.Control.Enum.ProductType.MeasuredJackPanel:
                                    Atend.Global.Acad.AcadMove.MoveMeasuredJackPanel(Atend.Global.Acad.AcadMove.MeasuredJackPanelOI);
                                    break;

                                case Atend.Control.Enum.ProductType.Ground:
                                    Atend.Global.Acad.AcadMove.MoveGround(Atend.Global.Acad.AcadMove.GroundOI);
                                    break;


                                ////case Atend.Control.Enum.ProductType.GroundCabel:
                                ////    ed.WriteMessage("Cabbbbbblllllll****************\n");
                                ////    Atend.Global.Acad.AcadMove.MoveCabel(Atend.Global.Acad.AcadMove.CabelOI);
                                ////    break;

                            }
                            #endregion
                            break;
                        case "MOVE":
                            ed.WriteMessage("MOVE command ended \n");
                            break;
                        case "SAVE":
                            //ed.WriteMessage("SAVE command ended \n");
                            //FileStream fs;
                            //File.Copy(txtFileName.Text, "c:\\" + Atend.Control.Common.SelectedDesignCode + ".dwg", true);
                            //fs = File.Open("c:\\" + Atend.Control.Common.SelectedDesignCode + ".dwg", FileMode.Open);
                            //BinaryReader br = new BinaryReader(fs);
                            //Atend.Base.Design.DDesignFile dDesignFile = new Atend.Base.Design.DDesignFile();
                            //dDesignFile.DesignCode = Atend.Control.Common.SelectedDesignCode;
                            //dDesignFile.FileSize = Convert.ToInt64(br.BaseStream.Length);
                            //dDesignFile.File = br.ReadBytes((Int32)br.BaseStream.Length);
                            //if (!dDesignFile.Insert())
                            //    MessageBox.Show("امکان ثبت اطلاعات نمی باشد", "خطا");
                            break;
                        case "QSAVE":
                            string S = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Database.Filename;
                            if (System.IO.File.Exists(S.Replace("DWG", "MDB")))
                            {
                                if (Atend.Control.Common.DesignName != "" && Atend.Control.Common.userCode != 0)
                                {
                                    Directory.CreateDirectory(string.Format(@"{0}\{1}", Atend.Control.Common.DesignFullAddress, "Data"));
                                    File.Copy(string.Format(@"{0}\{1}", Atend.Control.Common.DesignFullAddress, Atend.Control.Common.DesignName), string.Format(@"{0}\{1}\{2}", Atend.Control.Common.DesignFullAddress, "Data", Atend.Control.Common.DesignName), true);
                                    File.Copy(string.Format(@"{0}\{1}", Atend.Control.Common.DesignFullAddress, Atend.Control.Common.DesignName.Replace(".DWG", ".MDB")), string.Format(@"{0}\{1}\{2}", Atend.Control.Common.DesignFullAddress, "Data", Atend.Control.Common.DesignName.Replace("DWG", "MDB")), true);
                                    Atend.Global.Acad.DrawinOperation dOp = new Atend.Global.Acad.DrawinOperation();
                                    dOp.AddFileToAtendFile(string.Format(@"{0}\{1}", Atend.Control.Common.DesignFullAddress, Atend.Control.Common.DesignName.Replace("DWG", "ATNX")), string.Format(@"{0}\{1}\{2}", Atend.Control.Common.DesignFullAddress, "Data", Atend.Control.Common.DesignName));
                                    dOp.AddFileToAtendFile(string.Format(@"{0}\{1}", Atend.Control.Common.DesignFullAddress, Atend.Control.Common.DesignName.Replace("DWG", "ATNX")), string.Format(@"{0}\{1}\{2}", Atend.Control.Common.DesignFullAddress, "Data", Atend.Control.Common.DesignName.Replace("DWG", "MDB")));
                                    #region copy background

                                    ed.WriteMessage("DB:{0} \n", Atend.Control.Common.DesignBackGroundName);
                                    dOp.AddFileToAtendFile(string.Format(@"{0}\{1}", Atend.Control.Common.DesignFullAddress, Atend.Control.Common.DesignName.Replace("DWG", "ATNX")), Atend.Control.Common.DesignBackGroundName);
                                    #endregion
                                    ActivatedCommand = "";
                                }
                            }
                            else
                            {
                                ed.WriteMessage("MDB WAS NOT EXIST FOR QSAVE\n");
                            }

                            break;

                    }
                }
                else
                {
                    Atend.Global.Acad.AcadMove.EntityInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(GroupID);
                    //ed.WriteMessage(">>>>  >>> ProductType : {0} \n", Atend.Global.Acad.AcadMove.ProductType);
                    //ed.WriteMessage("<<<<  <<< .EntityInfo.NodeType : {0} \n", Atend.Global.Acad.AcadMove.EntityInfo.NodeType);
                    switch (e.GlobalCommandName)
                    {
                        //Atend.Global.Acad.AcadMove.EntityInfo.NodeType

                        case "DROPGEOM":
                            switch ((Atend.Control.Enum.ProductType)Atend.Global.Acad.AcadMove.EntityInfo.NodeType)
                            {
                                case Atend.Control.Enum.ProductType.Transformer:
                                    //ed.WriteMessage("Transformer found \n");
                                    Atend.Global.Acad.AcadMove.MoveTransformer(Atend.Global.Acad.AcadMove.TransformerOI);
                                    break;

                                case Atend.Control.Enum.ProductType.MiddleJackPanel:
                                    //ed.WriteMessage("MiddleJackPanel found \n");
                                    Atend.Global.Acad.AcadMove.MoveJackPanel(Atend.Global.Acad.AcadMove.MiddleJackPanelOI);
                                    break;

                                case Atend.Control.Enum.ProductType.WeekJackPanel:
                                    //ed.WriteMessage("WeekJackPanel found \n");
                                    Atend.Global.Acad.AcadMove.MoveJackPanel(Atend.Global.Acad.AcadMove.WeekJackPanelOI);
                                    break;
                            }
                            break;
                    }
                }




            }
            catch (System.Exception ex1)
            {

                ed.WriteMessage("CommandEnded : {0} \n", ex1.Message);

            }

            Atend.Global.Acad.AcadMove.AllowToMove = false;
            Atend.Global.Acad.AcadMove.ProductType = -1;
        }

        //~~~~~~~~~~~~ editor events ~~~~~~~~~~~~~~~~~//

        void myDocuments_DocumentLockModeChanged(object sender, DocumentLockModeChangedEventArgs e)
        {
            //throw new System.Exception("The method or operation is not implemented.");

            if (e.GlobalCommandName == "ERASE")
            {
                //e.Veto();

            }

        }

        void myDocuments_DocumentCreated(object sender, DocumentCollectionEventArgs e)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            try
            {
                Atend.Base.Design.DDesignProfile _DDesignProfile = Atend.Base.Design.DDesignProfile.AccessSelect();
                if (_DDesignProfile.DesignId != 0)
                {
                    Atend.Control.Common.DesignId = _DDesignProfile.DesignId;
                }
                else
                {
                    _DDesignProfile.DesignId = Atend.Control.Common.DesignId;
                    _DDesignProfile.Edition = Atend.Control.Common.Edition;
                }
                if (!_DDesignProfile.AccessUpdate())
                {
                    throw new System.Exception("DesignProfile update failed");
                }
            }
            catch
            {
            }
        }

        void myDocuments_DocumentActivated(object sender, DocumentCollectionEventArgs e)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            try
            {
                DocManForUI MyDocManForUI = new DocManForUI();
                MyDocManForUI.Do();
                Atend.Base.Acad.AT_COUNTER.ReadAll();
                string S = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Database.Filename;
                Atend.Control.Common.DesignFullAddress = S.Substring(0, S.LastIndexOf(@"\"));
                Atend.Control.Common.AccessPath = S.Replace("DWG", "MDB");
                Atend.Control.Common.DesignName = S.Substring(S.LastIndexOf(@"\") + 1, S.Length - (S.LastIndexOf(@"\") + 1));
                //ed.WriteMessage("DI:{0} \n",Atend.Control.Common.DesignId);
            }
            catch (System.Exception ex)
            {
                //ed.WriteMessage("ERROR myDocuments_DocumentActivated : {0} \n", ex.Message);
            }

        }


    }







}