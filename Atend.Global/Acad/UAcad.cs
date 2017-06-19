﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Colors;

using System.Data.Sql;
using System.Data;
using System.Data.OleDb;

namespace Atend.Global.Acad
{
    public class UAcad
    {
        //Update in tehran 7/14
        public static double CalulateLineLength(Point2d StartPoint, Point2d EndPoint)
        {

            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            double ConductorLength = Math.Sqrt(Math.Pow((EndPoint.X - StartPoint.X), 2) +
                                               Math.Pow((EndPoint.Y - StartPoint.Y), 2));
            return ConductorLength;

        }


        public static Point3d ComputeCenterPoint(Point3d StartPoint, Point3d EndPoint)
        {

            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            Point3d point;

            //ed.WriteMessage("computer point 1 \n");
            double centerX = Math.Abs(StartPoint.X - EndPoint.X) / 2;
            double centerY = Math.Abs(StartPoint.Y - EndPoint.Y) / 2;

            //ed.WriteMessage("computer point going to if \n");
            if ((EndPoint.X - StartPoint.X) > 0 && (EndPoint.Y - StartPoint.Y) > 0)
            {
                //ed.WriteMessage("Part one \n");
                point = new Point3d(StartPoint.X + centerX, StartPoint.Y + centerY, 0);
            }
            else if ((EndPoint.X - StartPoint.X) > 0 && (EndPoint.Y - StartPoint.Y) < 0)
            {
                //ed.WriteMessage("Part two \n");
                point = new Point3d(StartPoint.X + centerX, StartPoint.Y - centerY, 0);
            }
            else if ((EndPoint.X - StartPoint.X) < 0 && (EndPoint.Y - StartPoint.Y) < 0)
            {
                //ed.WriteMessage("Part three \n");
                point = new Point3d(EndPoint.X + centerX, EndPoint.Y + centerY, 0);
            }
            else
            {


                //ed.WriteMessage("Part four \n");
                point = new Point3d(EndPoint.X + centerX, EndPoint.Y - centerY, 0);
            }
            return point;
        }


        public static System.Data.DataTable DetermineSubEquip(int ProductType)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            Atend.Base.Design.DProductProperties productProperties = Atend.Base.Design.DProductProperties.AccessSelectBySoftwareCode(ProductType);
            System.Data.DataTable dtSubequip = Atend.Base.Design.DProductSubEquip.AccessselectByContainerCode(productProperties.Code);
            return dtSubequip;
        }

        //mousavi
        public static System.Data.DataTable DetermineParent(int ProductType)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            Atend.Base.Design.DProductProperties productProperties = Atend.Base.Design.DProductProperties.AccessSelectBySoftwareCode(ProductType);
            //ed.WriteMessage("ProCode ; {0} \n", productProperties.Code);
            System.Data.DataTable dtSubequip = Atend.Base.Design.DProductSubEquip.AccessselectBySubCode(productProperties.Code);

            //ed.WriteMessage("~~~//// ~~~~\n");
            foreach (DataRow dr in dtSubequip.Rows)
            {
                ed.WriteMessage("SC:{0}\n", dr["SoftWareCode"]);
            }

            return dtSubequip;
        }

        //private static Point3d CenterOfRectangle(Polyline Rectangle)
        //{


        //    int NumberOfEdge = Rectangle.NumberOfVertices - 1;
        //    Point3d CenterPoint = Point3d.Origin;

        //    switch (NumberOfEdge)
        //    {
        //        case 3:
        //            break;
        //        case 4:
        //            LineSegment3d LS1 = Rectangle.GetLineSegmentAt(0);

        //            LineSegment3d LS2 = Rectangle.GetLineSegmentAt(1);

        //            LineSegment3d ls3 = new LineSegment3d(LS1.StartPoint, LS2.EndPoint);

        //            CenterPoint = ls3.MidPoint;

        //            break;
        //        case 6:
        //            LineSegment3d LS11 = Rectangle.GetLineSegmentAt(0);

        //            LineSegment3d LS21 = Rectangle.GetLineSegmentAt(2);

        //            LineSegment3d ls31 = new LineSegment3d(LS11.StartPoint, LS21.EndPoint);

        //            CenterPoint = ls31.MidPoint;

        //            break;

        //    }




        //    return CenterPoint;

        //}


        /// <summary>
        /// determine 4,6 edges and circle
        /// </summary>
        /// <param name="CurrentEntity"></param>
        /// <returns></returns>

        public static Point3d CenterOfEntity(Entity CurrentEntity)
        {

            Point3d CenterPoint = Point3d.Origin;
            Polyline Rectangle = CurrentEntity as Polyline;
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            if (Rectangle != null)
            {

                int NumberOfEdge = Rectangle.NumberOfVertices - 1;
                switch (NumberOfEdge)
                {
                    case 3:
                        //ed.writeMessage("Triangle");
                        double X = (Rectangle.GetPoint3dAt(0).X + Rectangle.GetPoint3dAt(1).X + Rectangle.GetPoint3dAt(2).X) / 3;
                        double Y = (Rectangle.GetPoint3dAt(0).Y + Rectangle.GetPoint3dAt(1).Y + Rectangle.GetPoint3dAt(2).Y) / 3;
                        CenterPoint = new Point3d(X, Y, 0);
                        //ed.writeMessage("CenterPoint:{0}\n",CenterPoint);

                        break;
                    case 4:
                        LineSegment3d LS1 = Rectangle.GetLineSegmentAt(0);

                        LineSegment3d LS2 = Rectangle.GetLineSegmentAt(1);

                        LineSegment3d ls3 = new LineSegment3d(LS1.StartPoint, LS2.EndPoint);

                        CenterPoint = ls3.MidPoint;

                        break;
                    case 6:
                        LineSegment3d LS11 = Rectangle.GetLineSegmentAt(0);

                        LineSegment3d LS21 = Rectangle.GetLineSegmentAt(2);

                        LineSegment3d ls31 = new LineSegment3d(LS11.StartPoint, LS21.EndPoint);

                        CenterPoint = ls31.MidPoint;

                        break;

                }
            }
            else
            {
                Circle cir = CurrentEntity as Circle;
                if (cir != null)
                {
                    CenterPoint = cir.Center;
                }
                else
                {
                    Line li = CurrentEntity as Line;
                    if (li != null)
                    {
                        LineSegment3d ls = new LineSegment3d(li.StartPoint, li.EndPoint);
                        CenterPoint = ls.MidPoint;
                    }
                }
            }

            //ed.WriteMessage("CENTER POINT :{0} \n",CenterPoint);
            return CenterPoint;

        }

        public static Entity GetEntityByObjectID(ObjectId SelectedObjectId)
        {
            Entity ent;
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            //ed.WriteMessage(" go to GetEntityByObjectID \n");

            //Database db = HostApplicationServices.WorkingDatabase;
            Database db = Application.DocumentManager.MdiActiveDocument.Database;

            using (Transaction tr = db.TransactionManager.StartTransaction())
            {

                ent = (Entity)tr.GetObject(SelectedObjectId, OpenMode.ForRead);

            }

            return ent;
        }

        public static Entity __GetEntityByObjectID(ObjectId SelectedObjectId, Autodesk.AutoCAD.DatabaseServices.Transaction CadTransaction)
        {
            Entity ent;
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            //ed.WriteMessage(" go to GetEntityByObjectID \n");

            //Database db = HostApplicationServices.WorkingDatabase;
            Database db = Application.DocumentManager.MdiActiveDocument.Database;

            //using (Transaction tr = db.TransactionManager.StartTransaction())
            using (Transaction tr = CadTransaction)
            {
                ent = (Entity)tr.GetObject(SelectedObjectId, OpenMode.ForRead);
            }
            return ent;
        }


        /// <summary>
        /// This methods use for sectioning
        /// </summary>
        /// <param name="CurrentPole"></param>
        /// <returns></returns>
        #region This methods use for sectioning

        public static ObjectId GetPoleByGuid(Guid CurrentPole)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            ObjectId CurrentPoleOi = ObjectId.Null;

            //ed.WriteMessage("I am in GetPoleByGuid \n");

            Database db = Application.DocumentManager.MdiActiveDocument.Database;

            using (DocumentLock dlock = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.LockDocument())
            {

                using (Transaction tr = db.TransactionManager.StartTransaction())
                {


                    BlockTable bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead);

                    BlockTableRecord btr = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForRead);

                    //DBObjectCollection AllObjects = tr.GetAllObjects();
                    //ed.WriteMessage("ALL OBJECTS COUNT {0} \n",AllObjects.Count);

                    //ed.WriteMessage("Current pole {0}",CurrentPole);
                    //ed.WriteMessage("1 \n");
                    foreach (ObjectId oi in btr)
                    {
                        //ed.WriteMessage("2 \n");
                        Atend.Base.Acad.AT_INFO at_info = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oi);

                        //ed.WriteMessage("3 parentcod {0} : productType {1} : nodeGuid {2}  \n", at_info.ParentCode , at_info.NodeType , at_info.NodeCode);
                        if (at_info.ParentCode != "NONE" && (at_info.NodeType == (int)Atend.Control.Enum.ProductType.Pole || at_info.NodeType == (int)Atend.Control.Enum.ProductType.PoleTip) && at_info.NodeCode == CurrentPole.ToString())
                        {
                            CurrentPoleOi = oi;
                            //ed.WriteMessage("Current POLE OI : {0} \n", CurrentPoleOi);
                        }

                    }

                }

                //ed.WriteMessage("I am not in GetPoleByOI {0} \n", CurrentPoleOi);
            }
            return CurrentPoleOi;

        }

        public static System.Data.DataTable GetConsolInfoByObjectId(ObjectId CurrentConsol)
        {

            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            System.Data.DataColumn col1 = new System.Data.DataColumn("ConsolGuid");
            System.Data.DataColumn col2 = new System.Data.DataColumn("PoleGuid");
            System.Data.DataColumn col3 = new System.Data.DataColumn("BranchGuid");
            System.Data.DataColumn col4 = new System.Data.DataColumn("ConsolObjectId");

            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Columns.Add(col1);
            dt.Columns.Add(col2);
            dt.Columns.Add(col3);
            dt.Columns.Add(col4);

            DataSet ds = new DataSet();
            ds.Tables.Add(dt);

            using (DocumentLock dlock = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.LockDocument())
            {

                //ed.WriteMessage("i am in GetConsolInfoByObjectId {0} \n", CurrentConsol);
                if (CurrentConsol != ObjectId.Null)
                {
                    Atend.Base.Acad.AT_INFO CurrentConsolInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(CurrentConsol);
                    //ed.WriteMessage("11 \n");
                    Atend.Base.Acad.AT_SUB CurrentConsolSub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(CurrentConsol);
                    //ed.WriteMessage("12 \n");
                    foreach (ObjectId oi in CurrentConsolSub.SubIdCollection)
                    {

                        //ed.WriteMessage("13 \n");
                        Atend.Base.Acad.AT_INFO at_info = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oi);
                        //ed.WriteMessage("14 \n");
                        if (at_info.ParentCode != "NONE" && at_info.NodeType == (int)Atend.Control.Enum.ProductType.Conductor)
                        {
                            //ed.WriteMessage("15 \n");
                            DataRow dr = dt.NewRow();
                            dr["ConsolGuid"] = CurrentConsolInfo.NodeCode;
                            dr["PoleGuid"] = CurrentConsolInfo.ParentCode;
                            dr["BranchGuid"] = at_info.NodeCode;
                            dr["ConsolObjectId"] = CurrentConsol;
                            dt.Rows.Add(dr);

                        }

                    }

                }

                //foreach (DataRow dr in dt.Rows)
                //{

                //    ed.WriteMessage("\n~~~~START~~~~~~\n");
                //    ed.WriteMessage("\nConsolGuid : {0} , PoleGuid : {1} \n", dr["ConsolGuid"], dr["PoleGuid"]);
                //    ed.WriteMessage("\nBranchGuid : {0} , ConsolObjectId : {1} \n", dr["BranchGuid"], dr["ConsolObjectId"]);
                //    ed.WriteMessage("\n~~~~END~~~~~~\n");
                //}

                //ed.WriteMessage("i am not in GetConsolInfoByObjectId \n");
            }
            return ds.Tables[0];

        }

        public static ObjectId GetNextConsol(ObjectId CurrentConsol, Guid CurrentBranch)
        {

            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            ObjectId NextConsol = ObjectId.Null;

            using (DocumentLock dlock = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.LockDocument())
            {

                //ed.WriteMessage("i am in GetNextConsol \n");
                Atend.Base.Acad.AT_SUB CurrentConsolSub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(CurrentConsol);
                //ed.WriteMessage("21 \n");
                foreach (ObjectId oi in CurrentConsolSub.SubIdCollection)
                {
                    //ed.WriteMessage("22 \n");
                    Atend.Base.Acad.AT_INFO CurrentBranchInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oi);
                    //ed.WriteMessage("23 \n");
                    if (CurrentBranchInfo.ParentCode != "NONE" && CurrentBranchInfo.NodeType == (int)Atend.Control.Enum.ProductType.Conductor && CurrentBranchInfo.NodeCode == CurrentBranch.ToString())
                    {
                        //ed.WriteMessage("24 \n");
                        Atend.Base.Acad.AT_SUB CurrentBranchSub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(oi);
                        //ed.WriteMessage("25 \n");
                        foreach (ObjectId oii in CurrentBranchSub.SubIdCollection)
                        {
                            //ed.WriteMessage("26 \n");
                            if (oii != CurrentConsol)
                            {
                                //ed.WriteMessage("27 \n");
                                Atend.Base.Acad.AT_INFO NextConsolInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oii);
                                //ed.WriteMessage("28 \n");
                                if (NextConsolInfo.ParentCode != "NONE" && NextConsolInfo.NodeType == (int)Atend.Control.Enum.ProductType.Consol)
                                {
                                    //ed.WriteMessage("29 \n");
                                    NextConsol = oii;

                                }

                            }


                        }

                    }

                }
                //ed.WriteMessage("i am not in GetNextConsol \n");
            }
            return NextConsol;

        }

        public static ObjectId GetConsolObjectId(ObjectId CurrentPole, Guid CurrentConsol)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            ObjectId CurrentConsolOi = ObjectId.Null;
            //ed.WriteMessage("i am in GetConsolObjectId {0} : {1} \n", CurrentPole, CurrentConsol);
            using (DocumentLock dlock = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.LockDocument())
            {

                Atend.Base.Acad.AT_SUB CurrentPoleSub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(CurrentPole);
                //ed.WriteMessage("31 \n");
                foreach (ObjectId oi in CurrentPoleSub.SubIdCollection)
                {
                    //ed.WriteMessage("32 \n");
                    Atend.Base.Acad.AT_INFO currentConsolInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oi);
                    //ed.WriteMessage("33 \n");
                    if (currentConsolInfo.ParentCode != "NONE" && currentConsolInfo.NodeType == (int)Atend.Control.Enum.ProductType.Consol && currentConsolInfo.NodeCode == CurrentConsol.ToString())
                    {
                        //ed.WriteMessage("34 \n");
                        CurrentConsolOi = oi;

                    }

                }
                //ed.WriteMessage("i am not in GetConsolObjectId  \n");
            }
            return CurrentConsolOi;

        }

        public static System.Data.DataTable GetConsolConductors(Guid CurrentPole, Guid CurrentConsol)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            //ed.WriteMessage("Pole= " + CurrentPole + "Consol= " + CurrentConsol + "\n");
            ObjectId PoleOI = GetPoleByGuid(CurrentPole);
            //ed.WriteMessage("PoleOI={0}\n", PoleOI);
            ObjectId ConsolOI = GetConsolObjectId(PoleOI, CurrentConsol);
            //ed.WriteMessage("ConsolOI={0}\n", ConsolOI);
            System.Data.DataTable dt = GetConsolInfoByObjectId(ConsolOI);
            //ed.WriteMessage("ROWS={0}\n", dt.Rows.Count);

            return dt;

        }


        public static System.Data.DataTable FillPoleSubList()
        {

            Editor ed = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor;
            //ed.WriteMessage("I Am In FillPoleSubList\n");
            System.Data.DataTable PoleSubList = new System.Data.DataTable();
            PoleSubList.Columns.Add("PoleOI");
            PoleSubList.Columns.Add("PoleGuid");
            PoleSubList.Columns.Add("SubOI");
            PoleSubList.Columns.Add("SubGuid");
            PoleSubList.Columns.Add("Type");
            PoleSubList.Columns.Add("PoleType");
            Database db = Application.DocumentManager.MdiActiveDocument.Database;
            using (DocumentLock dlock = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.LockDocument())
            {
                using (Transaction tr = db.TransactionManager.StartTransaction())
                {
                    BlockTable bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead);
                    BlockTableRecord btr = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForRead);
                    foreach (ObjectId oi in btr)
                    {
                        Atend.Base.Acad.AT_INFO at_info = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oi);
                        if (at_info.ParentCode != "NONE" && (at_info.NodeType == (int)Atend.Control.Enum.ProductType.Pole || at_info.NodeType == (int)Atend.Control.Enum.ProductType.PoleTip))
                        {


                            Atend.Base.Acad.AT_SUB PoleSubs = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(oi);
                            foreach (ObjectId oii in PoleSubs.SubIdCollection)
                            {
                                Atend.Base.Acad.AT_INFO SubInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oii);
                                if (SubInfo.ParentCode != "NONE" && SubInfo.NodeType == (int)Atend.Control.Enum.ProductType.Consol)
                                {
                                    System.Data.DataRow NDR = PoleSubList.NewRow();
                                    NDR["PoleOI"] = oi.ToString().Substring(1, oi.ToString().Length - 2);
                                    NDR["PoleGuid"] = at_info.NodeCode;
                                    NDR["SubOI"] = oii.ToString().Substring(1, oii.ToString().Length - 2);
                                    NDR["SubGuid"] = SubInfo.NodeCode;
                                    NDR["Type"] = SubInfo.NodeType;
                                    NDR["PoleType"] = at_info.NodeType;
                                    PoleSubList.Rows.Add(NDR);
                                    //ed.WriteMessage("ConsolObj={0},ConsolGU={1}\n", oii.ToString().Substring(1, oii.ToString().Length - 2), SubInfo.NodeCode);

                                }

                                if (SubInfo.ParentCode != "NONE" && SubInfo.NodeType == (int)Atend.Control.Enum.ProductType.HeaderCabel)
                                {
                                    System.Data.DataRow NDR = PoleSubList.NewRow();
                                    NDR["PoleOI"] = oi.ToString().Substring(1, oi.ToString().Length - 2);
                                    NDR["PoleGuid"] = at_info.NodeCode;
                                    NDR["SubOI"] = oii.ToString().Substring(1, oii.ToString().Length - 2);
                                    NDR["SubGuid"] = SubInfo.NodeCode;
                                    NDR["Type"] = SubInfo.NodeType;
                                    NDR["PoleType"] = at_info.NodeType;
                                    PoleSubList.Rows.Add(NDR);
                                    //ed.WriteMessage("HeaderCabelObj={0},HeaderCabelGU={1}\n", oii.ToString().Substring(1, oii.ToString().Length - 2), SubInfo.NodeCode);


                                }

                                if (SubInfo.ParentCode != "NONE" && SubInfo.NodeType == (int)Atend.Control.Enum.ProductType.ConnectionPoint)
                                {
                                    System.Data.DataRow NDR = PoleSubList.NewRow();
                                    NDR["PoleOI"] = oi.ToString().Substring(1, oi.ToString().Length - 2);
                                    NDR["PoleGuid"] = at_info.NodeCode;
                                    NDR["SubOI"] = oii.ToString().Substring(1, oii.ToString().Length - 2);
                                    NDR["SubGuid"] = SubInfo.NodeCode;
                                    NDR["Type"] = SubInfo.NodeType;
                                    NDR["PoleType"] = at_info.NodeType;
                                    PoleSubList.Rows.Add(NDR);
                                    //ed.WriteMessage("ConnectionObj={0},ConsolGU={1}\n", oii.ToString().Substring(1, oii.ToString().Length - 2), SubInfo.NodeCode);

                                }

                                if (SubInfo.ParentCode != "NONE" && SubInfo.NodeType == (int)Atend.Control.Enum.ProductType.Kalamp)
                                {
                                    System.Data.DataRow NDR = PoleSubList.NewRow();
                                    NDR["PoleOI"] = oi.ToString().Substring(1, oi.ToString().Length - 2);
                                    NDR["PoleGuid"] = at_info.NodeCode;
                                    NDR["SubOI"] = oii.ToString().Substring(1, oii.ToString().Length - 2);
                                    NDR["SubGuid"] = SubInfo.NodeCode;
                                    NDR["Type"] = SubInfo.NodeType;
                                    NDR["PoleType"] = at_info.NodeType;
                                    PoleSubList.Rows.Add(NDR);
                                    //ed.WriteMessage("KalampObj={0},ConsolGU={1}\n", oii.ToString().Substring(1, oii.ToString().Length - 2), SubInfo.NodeCode);


                                }



                            }


                        }
                    }
                }
            }


            //foreach (DataRow dr in PoleSubList.Rows)
            //{
            //    ed.WriteMessage("~~~~~~~~~~~~~~~\n");
            //    ed.WriteMessage("PoleOI:{0}\nPoleGuid:{1}\nSubOI:{2}\nSubGuid:{3}\nType:{4}\nPoleType:{5}",
            //        dr["PoleOI"], dr["PoleGuid"], dr["SubOI"], dr["SubGuid"], dr["Type"], dr["PoleType"]);
            //}


            //ed.WriteMessage("I Am Not In FillPoleSubList\n");

            return PoleSubList;

        }

        public static System.Data.DataTable FillBranchList()
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            //ed.WriteMessage("I am In FillBranvhList\n");
            System.Data.DataTable BranchList = new System.Data.DataTable();
            BranchList.Columns.Add("Node1Guid");
            BranchList.Columns.Add("Node2Guid");
            BranchList.Columns.Add("BranchGuid");
            BranchList.Columns.Add("Type");
            BranchList.Columns.Add("Angle1");
            BranchList.Columns.Add("Angle2");
            try
            {

                Database db = Application.DocumentManager.MdiActiveDocument.Database;
                using (DocumentLock dlock = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.LockDocument())
                {
                    using (Transaction tr = db.TransactionManager.StartTransaction())
                    {
                        BlockTable bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead);
                        BlockTableRecord btr = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForRead);
                        foreach (ObjectId oi in btr)
                        {
                            Atend.Base.Acad.AT_INFO at_info = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oi);
                            if (at_info.ParentCode != "NONE" && at_info.NodeType == (int)Atend.Control.Enum.ProductType.Conductor)
                            {
                                List<string> Nodes = new List<string>();
                                ObjectIdCollection NodesOI = new ObjectIdCollection();

                                Atend.Base.Acad.AT_SUB PoleSubs = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(oi);
                                foreach (ObjectId oii in PoleSubs.SubIdCollection)
                                {
                                    Atend.Base.Acad.AT_INFO SubInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oii);
                                    if (SubInfo.ParentCode != "NONE" && SubInfo.NodeType == (int)Atend.Control.Enum.ProductType.Consol)
                                    {
                                        Nodes.Add(SubInfo.NodeCode);
                                        NodesOI.Add(oii);
                                    }
                                    if (SubInfo.ParentCode != "NONE" && SubInfo.NodeType == (int)Atend.Control.Enum.ProductType.HeaderCabel)
                                    {
                                        Nodes.Add(SubInfo.NodeCode);
                                        NodesOI.Add(oii);
                                    }
                                    if (SubInfo.ParentCode != "NONE" && SubInfo.NodeType == (int)Atend.Control.Enum.ProductType.ConnectionPoint)
                                    {
                                        Nodes.Add(SubInfo.NodeCode);
                                        NodesOI.Add(oii);
                                    }

                                    if (SubInfo.ParentCode != "NONE" && SubInfo.NodeType == (int)Atend.Control.Enum.ProductType.Kalamp)
                                    {
                                        Nodes.Add(SubInfo.NodeCode);
                                        NodesOI.Add(oii);
                                    }

                                }

                                if (Nodes.Count == 2)
                                {

                                    Line currentConductorEntity = (Line)GetEntityByObjectID(oi);
                                    if (currentConductorEntity != null)
                                    {
                                        System.Data.DataRow NDR = BranchList.NewRow();
                                        NDR["Node1Guid"] = Nodes[0];
                                        NDR["Node2Guid"] = Nodes[1];
                                        NDR["BranchGuid"] = new Guid(at_info.NodeCode); //oi.ToString().Substring(1, oi.ToString().Length - 2);
                                        NDR["Type"] = at_info.NodeType;


                                        Line imaginaryLine = null;
                                        if (currentConductorEntity.StartPoint == CenterOfEntity(GetEntityByObjectID(NodesOI[0])))
                                        {
                                            imaginaryLine = new Line(currentConductorEntity.StartPoint, currentConductorEntity.EndPoint);
                                        }
                                        else
                                        {
                                            imaginaryLine = new Line(currentConductorEntity.EndPoint, currentConductorEntity.StartPoint);
                                        }

                                        if (imaginaryLine != null)
                                        {
                                            if (Math.Ceiling(Math.Round((imaginaryLine.Angle * 180) / Math.PI, 2)) == 360)
                                            {
                                                NDR["Angle1"] = 0;
                                            }
                                            else
                                            {
                                                NDR["Angle1"] = Math.Ceiling(Math.Round((imaginaryLine.Angle * 180) / Math.PI, 2));
                                            }
                                        }

                                        imaginaryLine = null;
                                        if (currentConductorEntity.StartPoint == CenterOfEntity(GetEntityByObjectID(NodesOI[1])))
                                        {
                                            imaginaryLine = new Line(currentConductorEntity.StartPoint, currentConductorEntity.EndPoint);
                                        }
                                        else
                                        {
                                            imaginaryLine = new Line(currentConductorEntity.EndPoint, currentConductorEntity.StartPoint);
                                        }

                                        if (imaginaryLine != null)
                                        {
                                            if (Math.Ceiling(Math.Round((imaginaryLine.Angle * 180) / Math.PI, 2)) == 360)
                                            {
                                                NDR["Angle2"] = 0;
                                            }
                                            else
                                            {
                                                NDR["Angle2"] = Math.Ceiling(Math.Round((imaginaryLine.Angle * 180) / Math.PI, 2));
                                            }
                                        }
                                        BranchList.Rows.Add(NDR);


                                    }//if (currentConductorEntity != null)
                                }
                            }


                            if (at_info.ParentCode != "NONE" && at_info.NodeType == (int)Atend.Control.Enum.ProductType.SelfKeeper)
                            {
                                //ed.WriteMessage("SELF KEEPER \n");
                                List<string> Nodes = new List<string>();
                                ObjectIdCollection NodesOI = new ObjectIdCollection();

                                Atend.Base.Acad.AT_SUB PoleSubs = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(oi);
                                foreach (ObjectId oii in PoleSubs.SubIdCollection)
                                {
                                    Atend.Base.Acad.AT_INFO SubInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oii);
                                    if (SubInfo.ParentCode != "NONE" && SubInfo.NodeType == (int)Atend.Control.Enum.ProductType.Consol)
                                    {
                                        Nodes.Add(SubInfo.NodeCode);
                                        NodesOI.Add(oii);
                                    }
                                    if (SubInfo.ParentCode != "NONE" && SubInfo.NodeType == (int)Atend.Control.Enum.ProductType.HeaderCabel)
                                    {
                                        Nodes.Add(SubInfo.NodeCode);
                                        NodesOI.Add(oii);

                                    }
                                    if (SubInfo.ParentCode != "NONE" && SubInfo.NodeType == (int)Atend.Control.Enum.ProductType.ConnectionPoint)
                                    {
                                        Nodes.Add(SubInfo.NodeCode);
                                        NodesOI.Add(oii);

                                    }
                                    if (SubInfo.ParentCode != "NONE" && SubInfo.NodeType == (int)Atend.Control.Enum.ProductType.Kalamp)
                                    {
                                        Nodes.Add(SubInfo.NodeCode);
                                        NodesOI.Add(oii);

                                    }
                                }

                                if (Nodes.Count == 2)
                                {
                                    //System.Data.DataRow NDR = BranchList.NewRow();
                                    //NDR["Node1Guid"] = Nodes[0];
                                    //NDR["Node2Guid"] = Nodes[1];
                                    //NDR["BranchGuid"] = new Guid(at_info.NodeCode);  //oi.ToString().Substring(1, oi.ToString().Length - 2);
                                    //NDR["Type"] = at_info.NodeType;


                                    Line currentConductorEntity = (Line)GetEntityByObjectID(oi);
                                    if (currentConductorEntity != null)
                                    {
                                        System.Data.DataRow NDR = BranchList.NewRow();
                                        NDR["Node1Guid"] = Nodes[0];
                                        NDR["Node2Guid"] = Nodes[1];
                                        NDR["BranchGuid"] = new Guid(at_info.NodeCode); //oi.ToString().Substring(1, oi.ToString().Length - 2);
                                        NDR["Type"] = at_info.NodeType;


                                        Line imaginaryLine = null;
                                        //ed.WriteMessage("start:{0}\nCenter:{1}\n", currentConductorEntity.StartPoint, CenterOfEntity(GetEntityByObjectID(NodesOI[0])));
                                        if (currentConductorEntity.StartPoint == CenterOfEntity(GetEntityByObjectID(NodesOI[0])))
                                        {
                                            imaginaryLine = new Line(currentConductorEntity.StartPoint, currentConductorEntity.EndPoint);
                                        }
                                        else
                                        {
                                            imaginaryLine = new Line(currentConductorEntity.EndPoint, currentConductorEntity.StartPoint);
                                        }

                                        if (imaginaryLine != null)
                                        {
                                            NDR["Angle1"] = (imaginaryLine.Angle * 180) / Math.PI;
                                        }

                                        imaginaryLine = null;
                                        //ed.WriteMessage("start:{0}\nCenter:{1}\n", currentConductorEntity.StartPoint, CenterOfEntity(GetEntityByObjectID(NodesOI[1])));
                                        if (currentConductorEntity.StartPoint == CenterOfEntity(GetEntityByObjectID(NodesOI[1])))
                                        {
                                            imaginaryLine = new Line(currentConductorEntity.StartPoint, currentConductorEntity.EndPoint);
                                        }
                                        else
                                        {
                                            imaginaryLine = new Line(currentConductorEntity.EndPoint, currentConductorEntity.StartPoint);
                                        }

                                        if (imaginaryLine != null)
                                        {
                                            NDR["Angle2"] = (imaginaryLine.Angle * 180) / Math.PI;
                                        }
                                        BranchList.Rows.Add(NDR);


                                    }//if (currentConductorEntity != null)

                                }
                            }/////////////////////////////////////
                        }
                    }
                }
                //foreach (DataRow dr in BranchList.Rows)
                //{
                //    ed.WriteMessage("BranchType={0}\n", dr["Type"].ToString());
                //}

            }
            catch (System.Exception ex)
            {
                ed.WriteMessage("Error : {0} \n", ex.Message);
            }
            //ed.WriteMessage("ROWS count : {0} \n", BranchList.Rows.Count);
            foreach (DataRow dr in BranchList.Rows)
            {
                //ed.WriteMessage("-------------***-------------\n");
                //ed.WriteMessage("Node1Guid:{0}\nNode2Guid:{1}\nBranchGuid:{2}\nType:{3}\nAngle1:{4}\nAngle2:{5}\n",
                //dr["Node1Guid"], dr["Node2Guid"], dr["BranchGuid"], dr["Type"], dr["Angle1"], dr["Angle2"]);
            }



            //ed.WriteMessage("I Am Not In FillBranchList\n");
            return BranchList;
        }

        public static Guid GetNextNode(Guid CurrentNode, Guid CurrentBranch, System.Data.DataTable BranchList)
        {
            Guid NextNode = Guid.Empty;

            DataRow[] drs = BranchList.Select(string.Format("BranchGuid='{0}'", CurrentBranch));

            if (drs.Length == 1)
            {
                if (new Guid(drs[0]["Node1Guid"].ToString()) == CurrentNode)
                {
                    NextNode = new Guid(drs[0]["Node2Guid"].ToString());
                }
                else
                {
                    NextNode = new Guid(drs[0]["Node1Guid"].ToString());
                }
            }

            return NextNode;
        }

        public static System.Data.DataTable GetNodeBranches(Guid CurrentNode, int BranchType, System.Data.DataTable BranchList)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            //ed.WriteMessage("I Am in The GetNodeBranches\n");
            System.Data.DataTable NodeBranchesList = new System.Data.DataTable();
            NodeBranchesList.Columns.Add("BranchGuid");
            //ed.WriteMessage("Node1Guid={0} Type={1}\n", CurrentNode, BranchType);
            DataRow[] drs = BranchList.Select(string.Format("Node1Guid='{0}' And Type={1}", CurrentNode, BranchType));
            foreach (DataRow dr in drs)
            {
                //ed.WriteMessage("I Am In The First For\n");
                //ed.WriteMessage("dr[BranchGuid]={0}\n", dr["BranchGuid"].ToString());
                DataRow NROW = NodeBranchesList.NewRow();
                NROW["BranchGuid"] = new Guid(dr["BranchGuid"].ToString());
                NodeBranchesList.Rows.Add(NROW);
            }

            drs = BranchList.Select(string.Format("Node2Guid='{0}' And Type={1}", CurrentNode, BranchType));
            foreach (DataRow dr in drs)
            {
                //ed.WriteMessage("I Am In The Second For\n");

                DataRow NROW = NodeBranchesList.NewRow();
                NROW["BranchGuid"] = new Guid(dr["BranchGuid"].ToString());
                NodeBranchesList.Rows.Add(NROW);
            }

            //ed.WriteMessage(" GetNodeBranches : NodeBranchesList={0}\n", NodeBranchesList.Rows.Count);
            //ed.WriteMessage("FiNish GetNodeBranches\n");
            return NodeBranchesList;

        }


        #endregion



        /// <summary>
        /// This methods use for numbering
        /// </summary>
        /// <param name="SectionedTable"></param>
        /// <param name="SectionCount"></param>
        #region This methods use for numbering

        //public static Entity WriteNote(string Text, Point3d Position)
        //{
        //    Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
        //    DBText dbText = new DBText();
        //    using (DocumentLock dlock = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.LockDocument())
        //    {
        //        dbText.Position = Position;
        //        dbText.TextString = Text;
        //        dbText.Height = 10;





        //    }
        //    return dbText;
        //}

        public static Entity WriteNote(string Text, Point3d Position, double Scale)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            DBText dbText = new DBText();
            using (DocumentLock dlock = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.LockDocument())
            {

                dbText.Position = Position;
                //if (Text == string.Empty)
                //{
                //    dbText.TextString = "-";
                //}
                //else
                //{
                dbText.TextString = Text;
                //}
                dbText.Height = 5;

                Entity ent = dbText;
                //ed.WriteMessage("******SCALE : {0} :{1} \n", Scale, Position);
                Matrix3d trans1 = Matrix3d.Scaling(Scale, Position);
                ent.TransformBy(trans1);


            }
            return dbText;
        }


        //public static Entity WriteNote(string Text, Point3d LineStartPoint, Point3d LineEndPoint)
        //{
        //    Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
        //    DBText dbText = new DBText();
        //    dbText.Position = new LineSegment3d(LineStartPoint, LineEndPoint).MidPoint;
        //    dbText.TextString = Text;
        //    dbText.Height = 10;
        //    //ed.WriteMessage(string.Format("Angle is : {0} \n", ((Math.Atan((conductorInformation.Point1.Y - conductorInformation.Point2.Y) / (conductorInformation.Point1.X - conductorInformation.Point2.X))) * Math.PI) / 180));
        //    dbText.Rotation = new Point2d(LineStartPoint.X, LineStartPoint.Y).GetVectorTo(new Point2d(LineEndPoint.X, LineEndPoint.Y)).Angle;
        //    return dbText;
        //}


        public static Entity WriteNote(string Text, Point3d LineStartPoint, Point3d LineEndPoint, double Scale)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            DBText dbText = new DBText();
            dbText.Position = new LineSegment3d(LineStartPoint, LineEndPoint).MidPoint;
            dbText.TextString = Text;
            dbText.Height = 5;
            //ed.WriteMessage(string.Format("Angle is : {0} \n", ((Math.Atan((conductorInformation.Point1.Y - conductorInformation.Point2.Y) / (conductorInformation.Point1.X - conductorInformation.Point2.X))) * Math.PI) / 180));
            dbText.Rotation = new Point2d(LineStartPoint.X, LineStartPoint.Y).GetVectorTo(new Point2d(LineEndPoint.X, LineEndPoint.Y)).Angle;


            Entity ent = dbText;
            //ed.WriteMessage("******SCALE : {0} :{1} \n", Scale, dbText.Position);
            Matrix3d trans1 = Matrix3d.Scaling(Scale, dbText.Position);
            ent.TransformBy(trans1);


            return dbText;
        }


        public static void ChangeEntityText(ObjectId SelectedTextObjectID, string Text)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            using (DocumentLock dlock = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.LockDocument())
            {


                //Database db = HostApplicationServices.WorkingDatabase;
                Database db = Application.DocumentManager.MdiActiveDocument.Database;

                using (Transaction tr = db.TransactionManager.StartTransaction())
                {

                    DBText dbtext = (DBText)tr.GetObject(SelectedTextObjectID, OpenMode.ForWrite);

                    if (dbtext != null)
                    {
                        dbtext.TextString = Text;
                    }

                    tr.Commit();

                    ed.Regen();

                }
            }

        }

        public static void ChangeBranchText(ObjectId SelectedBranchObjectID, string Text)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            using (DocumentLock dlock = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.LockDocument())
            {

                //Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;

                //Database db = HostApplicationServices.WorkingDatabase;
                Database db = Application.DocumentManager.MdiActiveDocument.Database;

                Atend.Base.Acad.AT_SUB BranchSub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(SelectedBranchObjectID);
                foreach (ObjectId oi in BranchSub.SubIdCollection)
                {

                    Atend.Base.Acad.AT_INFO commentInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oi);
                    if (commentInfo.ParentCode != "NONE" && commentInfo.NodeType == (int)Atend.Control.Enum.ProductType.Comment)
                    {
                        //ed.WriteMessage("COmmen twas found.\n");
                        using (Transaction tr = db.TransactionManager.StartTransaction())
                        {

                            DBText dbtext = (DBText)tr.GetObject(oi, OpenMode.ForWrite);

                            if (dbtext != null)
                            {
                                dbtext.TextString = Text;
                            }

                            tr.Commit();

                            ed.Regen();

                        }


                    }

                }

            }

        }

        public static void ChangeMText(ObjectId RodObjectID, string Text)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            using (DocumentLock dlock = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.LockDocument())
            {

                //Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;

                //Database db = HostApplicationServices.WorkingDatabase;
                Database db = Application.DocumentManager.MdiActiveDocument.Database;

                Atend.Base.Acad.AT_INFO RodInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(RodObjectID);
                if (RodInfo.ParentCode != "NONE" && RodInfo.NodeType == (int)Atend.Control.Enum.ProductType.Rod)
                {
                    ObjectId GOI = Atend.Global.Acad.UAcad.GetEntityGroup(RodObjectID);
                    if (GOI != ObjectId.Null)
                    {
                        Atend.Base.Acad.AT_SUB RodGroupSub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(GOI);
                        foreach (ObjectId oi in RodGroupSub.SubIdCollection)
                        {

                            Atend.Base.Acad.AT_INFO commentInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oi);
                            if (commentInfo.ParentCode != "NONE" && commentInfo.NodeType == (int)Atend.Control.Enum.ProductType.Comment)
                            {
                                //ed.WriteMessage("COmmen twas found.\n");
                                using (Transaction tr = db.TransactionManager.StartTransaction())
                                {

                                    MText dbtext = tr.GetObject(oi, OpenMode.ForWrite) as MText;

                                    if (dbtext != null)
                                    {
                                        dbtext.Contents = Text;
                                    }

                                    tr.Commit();

                                    ed.Regen();

                                }


                            }


                        }
                    }
                }


            }

        }

        public static ObjectId GetLayerById(string LayerName)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            using (DocumentLock dlock = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.LockDocument())
            {

                Database db = Application.DocumentManager.MdiActiveDocument.Database;

                using (DocumentLock dLock = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.LockDocument())
                {

                    using (Transaction tr = db.TransactionManager.StartTransaction())
                    {

                        LayerTable lt = (LayerTable)tr.GetObject(db.LayerTableId, OpenMode.ForWrite);


                        if (lt.Has(LayerName))
                        {

                            return lt[LayerName];

                        }
                        else
                        {

                            ObjectId newLayer;

                            LayerTableRecord ltr = new LayerTableRecord();

                            ltr.Name = Convert.ToString(LayerName);

                            ltr.Color = Color.FromColorIndex(ColorMethod.ByAci, Convert.ToInt16(Enum.Parse(typeof(Atend.Control.Enum.AutoCadLayerName), LayerName)));

                            //ed.WriteMessage("Layers Setting Done. \n");

                            newLayer = lt.Add(ltr);

                            tr.AddNewlyCreatedDBObject(ltr, true);

                            tr.Commit();

                            return newLayer;
                        }

                    }

                }
            }
        }

        //public static ObjectId DrawEntityOnScreen(Entity entity)
        //{

        //    ObjectId oi;


        //    Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
        //    //ed.WriteMessage("I aM in DrawEntityOnScreen\n");
        //    //Database db = HostApplicationServices.WorkingDatabase;
        //    Database db = Application.DocumentManager.MdiActiveDocument.Database;
        //    using (DocumentLock dl = Application.DocumentManager.MdiActiveDocument.LockDocument())
        //    {


        //        using (Transaction tr = db.TransactionManager.StartTransaction())
        //        {

        //            BlockTable bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForWrite);

        //            BlockTableRecord btr = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);

        //            oi = btr.AppendEntity(entity);

        //            tr.AddNewlyCreatedDBObject(entity, true);

        //            tr.Commit();


        //        }
        //    }
        //    //ed.WriteMessage("End Of DrawEntityOnScreen\n");

        //    return oi;

        //}

        public static ObjectId DrawEntityOnScreen(Entity entity, string LayerName)
        {

            ObjectId oi;
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            //ed.WriteMessage("I aM in DrawEntityOnScreen\n");
            //Database db = HostApplicationServices.WorkingDatabase;
            Database db = Application.DocumentManager.MdiActiveDocument.Database;
            using (DocumentLock dl = Application.DocumentManager.MdiActiveDocument.LockDocument())
            {
                using (Transaction tr = db.TransactionManager.StartTransaction())
                {

                    BlockTable bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForWrite);
                    BlockTableRecord btr = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);
                    entity.LayerId = GetLayerById(LayerName);
                    oi = btr.AppendEntity(entity);
                    tr.AddNewlyCreatedDBObject(entity, true);
                    tr.Commit();
                }
            }
            //ed.WriteMessage("End Of DrawEntityOnScreen\n");

            return oi;

        }

        //public static ObjectId DrawEntityOnScreen(Entity entity, string LayerName ,Transaction _transaction) 
        //{

        //    ObjectId oi;
        //    Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
        //    //ed.WriteMessage("I aM in DrawEntityOnScreen\n");
        //    //Database db = HostApplicationServices.WorkingDatabase;
        //    Database db = Application.DocumentManager.MdiActiveDocument.Database;
        //    using (DocumentLock dl = Application.DocumentManager.MdiActiveDocument.LockDocument())
        //    {
        //        using (Transaction tr = _transaction)
        //        {

        //            BlockTable bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForWrite);
        //            BlockTableRecord btr = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);
        //            entity.LayerId = GetLayerById(LayerName);
        //            oi = btr.AppendEntity(entity);
        //            tr.AddNewlyCreatedDBObject(entity, true);
        //            //tr.Commit();
        //        }
        //    }
        //    //ed.WriteMessage("End Of DrawEntityOnScreen\n");

        //    return oi;

        //}


        public static void PoleNumbering()
        {
            //////////Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            //////////using (DocumentLock dlock = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.LockDocument())
            //////////{

            //////////    int PoleCounter = 1;

            //////////    System.Data.DataTable SectionsList =
            //////////    Atend.Base.Design.DSection.selectByDesignCode(Atend.Control.Common.SelectedDesignCode);
            //////////    //ed.WriteMessage("a\n");
            //////////    foreach (DataRow dr in SectionsList.Rows)
            //////////    {
            //////////        //ed.WriteMessage("12\n");
            //////////        System.Data.DataTable PolesList =
            //////////            Atend.Base.Design.DPoleSection.SelectBySectionCodeDesignCodeProductType(
            //////////            new Guid(dr["Code"].ToString()), Atend.Control.Common.SelectedDesignCode,
            //////////            (int)Atend.Control.Enum.ProductType.Pole);

            //////////        //ed.WriteMessage("Section Guid : {0} \n", dr["Code"].ToString());
            //////////        //ed.WriteMessage("Selected Design Code : {0} \n", Atend.Control.Common.SelectedDesignCode);
            //////////        //ed.WriteMessage("Pole Type : {0} \n", (int)Atend.Control.Enum.ProductType.Pole);

            //////////        foreach (DataRow pdr in PolesList.Rows)
            //////////        {
            //////////            //put on screen

            //////////            //ed.WriteMessage("Guid="+pdr["ProductCode"].ToString()+"\n");
            //////////            ObjectId Poleoi = GetPoleByGuid(new Guid(pdr["ProductCode"].ToString()));

            //////////            string PoleNumber = string.Format("P:{0}", PoleCounter);
            //////////            bool HasComment = false;
            //////////            Atend.Base.Acad.AT_SUB poleSub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(Poleoi);
            //////////            foreach (ObjectId oi in poleSub.SubIdCollection)
            //////////            {
            //////////                Atend.Base.Acad.AT_INFO subInfo =
            //////////                    Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oi);
            //////////                if (subInfo.ParentCode != "NONE" &&
            //////////                    subInfo.NodeType == (int)Atend.Control.Enum.ProductType.Comment)
            //////////                {
            //////////                    HasComment = true;

            //////////                    ChangeEntityText(oi, PoleNumber);
            //////////                }
            //////////            }

            //////////            if (HasComment)
            //////////            {
            //////////            }
            //////////            else
            //////////            {
            //////////                //Polyline pl = (Polyline)GetEntityByObjectID(Poleoi);
            //////////                Point3d CenterPoint = CenterOfEntity(GetEntityByObjectID(Poleoi));
            //////////                Point3d NewCenter = new Point3d(CenterPoint.X + 50, CenterPoint.Y + 50, 0);

            //////////                Entity textEntity = WriteNote(PoleNumber, NewCenter);
            //////////                ObjectId newText =
            //////////                DrawEntityOnScreen(textEntity, Atend.Control.Enum.AutoCadLayerName.GENERAL.ToString());
            //////////                Atend.Base.Acad.AT_INFO text_info = new Atend.Base.Acad.AT_INFO();
            //////////                text_info.ParentCode = pdr["ProductCode"].ToString();
            //////////                text_info.NodeCode = "";
            //////////                text_info.NodeType = (int)Atend.Control.Enum.ProductType.Comment;
            //////////                text_info.ProductCode = 0;
            //////////                text_info.SelectedObjectId = newText;
            //////////                text_info.Insert();

            //////////                Atend.Base.Acad.AT_SUB.AddToAT_SUB(newText, Poleoi);

            //////////            }




            //////////            // put in database

            //////////            Atend.Base.Design.DNode CurrentNode = Atend.Base.Design.DNode.SelectByDesignCodeAndCode(
            //////////                Atend.Control.Common.SelectedDesignCode, new Guid(pdr["ProductCode"].ToString()));

            //////////            CurrentNode.Number = PoleNumber;
            //////////            CurrentNode.AccessUpdate();


            //////////            PoleCounter++;


            //////////        }

            //////////    }
            //////////}

        }

        #endregion


        /// <summary>
        /// This methods use for Inside Point
        /// </summary>
        /// <param name="SectionedTable"></param>
        /// <param name="SectionCount"></param>
        #region This methods use for inside point
        //private static int INSIDE = 0;
        //private static int OUTSIDE = 1;

        //private static double MIN(double x, double y)
        //{
        //    if (x < y)
        //        return x;
        //    else
        //        return y;
        //}

        //private static double MAX(double x, double y)
        //{

        //    if (x > y)
        //        return x;
        //    else
        //        return y;

        //}

        //public static int InsidePolygon(Point3dCollection P3C, Point3d point)
        //{
        //    //ed.WriteMessage("I Am In InsidePolygon\n");
        //    int counter = 0;
        //    int N;
        //    double xinters;
        //    Point3d p1, p2;

        //    p1 = P3C[0];
        //    N = P3C.Count - 1;
        //    //ed.WriteMessage("N= "+N.ToString()+"\n");

        //    for (int i = 1; i <= N; i++)
        //    {
        //        //ed.WriteMessage("i= "+i.ToString()+"\n");
        //        p2 = P3C[i - ((int)(i / N) * i)];
        //        //ed.WriteMessage("{0} REMAIL", (N - ((int)(i / N) * i)));
        //        //ed.WriteMessage("p2= "+p2+"\n");
        //        if (point.Y > MIN(p1.Y, p2.Y))
        //        {
        //            //ed.WriteMessage("I AM IN The First IF\n");
        //            //ed.WriteMessage("Point.y= "+point.Y.ToString()+"\n");
        //            //ed.WriteMessage("p1.Y= "+p1.Y.ToString()+"\n");
        //            //ed.WriteMessage("p2.Y= "+p2.Y.ToString()+"\n");
        //            if (point.Y <= MAX(p1.Y, p2.Y))
        //            {
        //                //ed.WriteMessage("I AM IN The Second IF\n");
        //                //ed.WriteMessage("point.X= "+point.X.ToString()+"\n");
        //                //ed.WriteMessage("p1.X= "+p1.X.ToString()+"\n");
        //                //ed.WriteMessage("p2.X= "+p2.X.ToString()+"\n");
        //                if (point.X <= MAX(p1.X, p2.X))
        //                {
        //                    //ed.WriteMessage("I Am In The Third If\n");
        //                    //ed.WriteMessage("p1.Y= " + p1.Y.ToString() + "\n");
        //                    //ed.WriteMessage("p2.Y= "+p2.Y.ToString()+"\n");

        //                    if (p1.Y != p2.Y)
        //                    {
        //                        //ed.WriteMessage("I Am In The Forth IF\n");

        //                        xinters = (point.Y - p1.Y) * (p2.X - p1.X) / (p2.Y - p1.Y) + p1.X;
        //                        //ed.WriteMessage("Xinters= "+xinters.ToString()+"\n");
        //                        if (p1.X == p2.X || point.X <= xinters)
        //                        {
        //                            //ed.WriteMessage("I Am In The Fifth If\n");
        //                            counter++;
        //                            //ed.WriteMessage("Counter= "+counter.ToString()+"\n");
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        p1 = p2;
        //    }
        //    if (counter - (counter * (int)(counter / 2)) == 0)
        //        return OUTSIDE;
        //    else
        //        return INSIDE;

        //}
        #endregion

        public static Point3dCollection ConvertEntityToPoint3dCollection(Entity polyLIneEntity)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            Polyline p = polyLIneEntity as Polyline;
            Point3dCollection pc = new Point3dCollection();

            if (p != null)
            {



                for (int i = 0; i < p.NumberOfVertices; i++)
                {
                    pc.Add(p.GetPointAtParameter(i));
                    //ed.WriteMessage("{0} \n", p.GetPointAtParameter(i));
                }
                //ed.WriteMessage("{0}.:0O0:. \n", InsidePolygon(pc, ppr.Value));

            }

            return pc;
        }

        public static System.Data.DataTable GetPoleConductors(Guid CurrentPole)//Guid CurrentPole)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            ////Guid CurrentPole = new Guid(ed.GetString("Pole guid :").StringResult);

            System.Data.DataColumn col1 = new System.Data.DataColumn("PoleGuid");
            System.Data.DataColumn col2 = new System.Data.DataColumn("ConsolGuid");
            System.Data.DataColumn col3 = new System.Data.DataColumn("BranchGuid");
            System.Data.DataColumn col4 = new System.Data.DataColumn("Type");
            System.Data.DataColumn col5 = new System.Data.DataColumn("Angle");
            System.Data.DataTable ConductorList = new System.Data.DataTable();
            ConductorList.Columns.Add(col1);
            ConductorList.Columns.Add(col2);
            ConductorList.Columns.Add(col3);
            ConductorList.Columns.Add(col4);
            ConductorList.Columns.Add(col5);


            ObjectId PoleOI = GetPoleByGuid(CurrentPole);
            if (PoleOI != null)
            {

                Atend.Base.Acad.AT_SUB PoleSub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(PoleOI);

                foreach (ObjectId oi in PoleSub.SubIdCollection)
                {
                    Atend.Base.Acad.AT_INFO subInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oi);
                    if (subInfo.ParentCode != "NONE" && subInfo.NodeType == (int)Atend.Control.Enum.ProductType.Consol)
                    {

                        Atend.Base.Acad.AT_SUB ConsolSub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(oi);
                        foreach (ObjectId oii in ConsolSub.SubIdCollection)
                        {

                            Atend.Base.Acad.AT_INFO consolSubInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oii);
                            if (consolSubInfo.ParentCode != "NONE" && consolSubInfo.NodeType == (int)Atend.Control.Enum.ProductType.Conductor)
                            {

                                Polyline ConsolPoly = (Polyline)GetEntityByObjectID(oi);
                                if (ConsolPoly != null)
                                {
                                    Point3d ConsolCenterPoint = CenterOfEntity(ConsolPoly);

                                    Line currentConductorEntity = (Line)GetEntityByObjectID(oii);
                                    if (currentConductorEntity != null)
                                    {

                                        Line imaginaryLine;
                                        if (currentConductorEntity.StartPoint == ConsolCenterPoint)
                                        {
                                            imaginaryLine = new Line(currentConductorEntity.StartPoint, currentConductorEntity.EndPoint);
                                        }
                                        else
                                        {
                                            imaginaryLine = new Line(currentConductorEntity.EndPoint, currentConductorEntity.StartPoint);
                                        }

                                        //add to conductorList

                                        DataRow nr = ConductorList.NewRow();
                                        nr["PoleGuid"] = CurrentPole.ToString();
                                        nr["ConsolGuid"] = subInfo.NodeCode;
                                        nr["BranchGuid"] = consolSubInfo.NodeCode;
                                        nr["Type"] = (int)Atend.Control.Enum.ProductType.Conductor;
                                        nr["Angle"] = imaginaryLine.Angle;//(180 * )/ Math.PI;
                                        ConductorList.Rows.Add(nr);


                                    }

                                }

                            }

                        }
                    }

                    if (subInfo.ParentCode != "NONE" && subInfo.NodeType == (int)Atend.Control.Enum.ProductType.ConnectionPoint)
                    {
                        Atend.Base.Acad.AT_SUB ConnectionPointSub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(oi);
                        foreach (ObjectId oii in ConnectionPointSub.SubIdCollection)
                        {

                            Atend.Base.Acad.AT_INFO ConnectionPointSubInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oii);
                            if (ConnectionPointSubInfo.ParentCode != "NONE" && ConnectionPointSubInfo.NodeType == (int)Atend.Control.Enum.ProductType.SelfKeeper)
                            {

                                Circle ConnectionPointCircle = (Circle)GetEntityByObjectID(oi);
                                if (ConnectionPointCircle != null)
                                {
                                    Point3d ConnectionPointCenterPoint = ConnectionPointCircle.Center;

                                    Line currentConductorEntity = (Line)GetEntityByObjectID(oii);
                                    if (currentConductorEntity != null)
                                    {

                                        Line imaginaryLine;
                                        if (currentConductorEntity.StartPoint == ConnectionPointCenterPoint)
                                        {
                                            imaginaryLine = new Line(currentConductorEntity.StartPoint, currentConductorEntity.EndPoint);
                                        }
                                        else
                                        {
                                            imaginaryLine = new Line(currentConductorEntity.EndPoint, currentConductorEntity.StartPoint);
                                        }

                                        //add to conductorList

                                        DataRow nr = ConductorList.NewRow();
                                        nr["PoleGuid"] = CurrentPole.ToString();
                                        nr["ConsolGuid"] = "";
                                        nr["BranchGuid"] = ConnectionPointSubInfo.NodeCode;
                                        nr["Type"] = (int)Atend.Control.Enum.ProductType.SelfKeeper;
                                        nr["Angle"] = imaginaryLine.Angle;
                                        ConductorList.Rows.Add(nr);


                                    }

                                }

                            }

                        }
                    }

                }

            }
            else
            {
                //ed.WriteMessage("\nThis pole was not exist in drawing \n");
            }

            //foreach (DataRow dr in ConductorList.Rows)
            //{
            //    ed.WriteMessage("Pole:{0} Consol:{1} type:{2} Angle:{3} branch:{4} \n",dr["PoleGuid"],dr["ConsolGuid"],dr["Type"],dr["Angle"], dr["BranchGuid"]);
            //}
            //ed.WriteMessage("ConductorList.Count= "+ConductorList.Rows.Count+"\n");
            return ConductorList;

        }

        public void LoadBackGround()
        {
            string ReferenzName = "Blasy_1";
            string ReferenzPath = Atend.Control.Common.DesignFullAddress + @"\grid.dwg";
            Autodesk.AutoCAD.ApplicationServices.Document dwg = Application.DocumentManager.MdiActiveDocument;
            Database db = dwg.Database;
            Autodesk.AutoCAD.DatabaseServices.TransactionManager tm = dwg.TransactionManager;
            Transaction bTrans = tm.StartTransaction();
            Editor ed = dwg.Editor;
            try
            {
                ObjectId myX = db.AttachXref(ReferenzPath, ReferenzName);
                Autodesk.AutoCAD.DatabaseServices.BlockReference myBl = new BlockReference(new Autodesk.AutoCAD.Geometry.Point3d(0, 0, 0), myX);
                BlockTable bt = (BlockTable)bTrans.GetObject(db.BlockTableId, OpenMode.ForRead, false);
                BlockTableRecord mSpace = (BlockTableRecord)bTrans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite, false);
                mSpace.AppendEntity(myBl);
                bTrans.AddNewlyCreatedDBObject(myBl, true);
                bTrans.Commit();
            }
            catch (System.Exception e)
            {
                string tmp = e.Message;
                ed.WriteMessage(e + "\n");
                bTrans.Abort();
            }
        }

        public static void DrawLeader(string Comment, Point3d Position)
        {

        }


        public static void RotateBy2Point(ObjectIdCollection OIC, Point3d CenterPoint, Point3d StartPoint, Point3d EndPoint)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            LineSegment3d ls1 = new LineSegment3d(StartPoint, EndPoint);
            Point3d BasePoint = ls1.MidPoint;
            Database db = Application.DocumentManager.MdiActiveDocument.Database;

            double Angle = new Line(StartPoint, EndPoint).Angle;
            //ed.WriteMessage("start:{0} end:{1} angle:{2}\n", StartPoint, EndPoint, Angle);
            Matrix3d trans = Matrix3d.Rotation(Angle, ed.CurrentUserCoordinateSystem.CoordinateSystem3d.Zaxis, CenterPoint);
            using (DocumentLock dlock = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.LockDocument())
            {
                using (Transaction tr = db.TransactionManager.StartTransaction())
                {
                    foreach (ObjectId oi in OIC)
                    {
                        Entity ent = tr.GetObject(oi, OpenMode.ForWrite) as Entity;
                        if (ent != null)
                        {
                            ent.TransformBy(trans);
                        }
                    }
                    tr.Commit();
                }
            }
        }

        public static void WriteLoadForPole(ArrayList PoleList)
        {
            //////////////////Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            //////////////////foreach (object obj in PoleList)
            //////////////////{
            //////////////////    //ed.WriteMessage("in foreach : {0} \n", new Guid(obj.ToString()));
            //////////////////    Atend.Base.Design.DNode PoleIndo = Atend.Base.Design.DNode.SelectByDesignCodeAndCode(Atend.Control.Common.SelectedDesignCode, new Guid(obj.ToString()));
            //////////////////    if (PoleIndo.LoadCode != 0)
            //////////////////    {
            //////////////////        //ed.WriteMessage("Pole load : {0}", PoleIndo.LoadCode);
            //////////////////        Atend.Base.Calculating.CLoadFactor PoleLoad = Atend.Base.Calculating.CLoadFactor.AccessSelectByCode(PoleIndo.LoadCode);
            //////////////////        //ed.WriteMessage("1 \n");
            //////////////////        ObjectId CurrentPoleOI = GetPoleByGuid(new Guid(obj.ToString()));
            //////////////////        //ed.WriteMessage("2 \n");
            //////////////////        Atend.Base.Acad.AT_INFO PoleCadInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(CurrentPoleOI);
            //////////////////        //ed.WriteMessage("3 \n");
            //////////////////        if (PoleCadInfo.ParentCode != "NONE" && PoleCadInfo.NodeType == (int)Atend.Control.Enum.ProductType.Pole)
            //////////////////        {
            //////////////////            //ed.WriteMessage("4 \n");
            //////////////////            Atend.Base.Acad.AT_SUB PoleSub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(CurrentPoleOI);
            //////////////////            //ed.WriteMessage("5 \n");
            //////////////////            bool CommentIsExist = false;
            //////////////////            //ed.WriteMessage("6 \n");

            //////////////////            foreach (ObjectId PoleSubOI in PoleSub.SubIdCollection)
            //////////////////            {
            //////////////////                //ed.WriteMessage("7 \n");
            //////////////////                Atend.Base.Acad.AT_INFO PoleSubInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(PoleSubOI);
            //////////////////                //ed.WriteMessage("8 \n");
            //////////////////                if (PoleSubInfo.ParentCode != "NONE" && PoleSubInfo.NodeType == (int)Atend.Control.Enum.ProductType.LoadComment)
            //////////////////                {
            //////////////////                    //ed.WriteMessage("9 \n");
            //////////////////                    CommentIsExist = true;
            //////////////////                    //ed.WriteMessage("10 \n");
            //////////////////                    ChangeEntityText(PoleSubOI, PoleLoad.Name);
            //////////////////                }
            //////////////////            }
            //////////////////            //ed.WriteMessage("11 \n");
            //////////////////            if (!CommentIsExist)
            //////////////////            {
            //////////////////                //ed.WriteMessage("12 \n");
            //////////////////                Polyline PolePoly = (Polyline)GetEntityByObjectID(CurrentPoleOI);
            //////////////////                //ed.WriteMessage("13 \n");
            //////////////////                if (PolePoly != null)
            //////////////////                {
            //////////////////                    //ed.WriteMessage("14 \n");
            //////////////////                    Point3d PoleCenter = CenterOfEntity(PolePoly);

            //////////////////                    string TextComment = "NONE";
            //////////////////                    if (PoleLoad.Name == string.Empty)
            //////////////////                    {
            //////////////////                        TextComment = "NONE";
            //////////////////                    }
            //////////////////                    else
            //////////////////                    {
            //////////////////                        TextComment = PoleLoad.Name;
            //////////////////                    }

            //////////////////                    //ed.WriteMessage("15 {0} ,{1} \n", TextComment, PoleCenter);
            //////////////////                    Entity TextEntity = WriteNote(TextComment, PoleCenter);
            //////////////////                    //ed.WriteMessage("16 \n");
            //////////////////                    ObjectId NewTextOI = DrawEntityOnScreen(TextEntity, Atend.Control.Enum.AutoCadLayerName.GENERAL.ToString());
            //////////////////                    //ed.WriteMessage("17 \n");
            //////////////////                    Atend.Base.Acad.AT_SUB.AddToAT_SUB(NewTextOI, CurrentPoleOI);
            //////////////////                    //ed.WriteMessage("18 \n");
            //////////////////                    Atend.Base.Acad.AT_INFO TextInfo = new Atend.Base.Acad.AT_INFO(NewTextOI);
            //////////////////                    //ed.WriteMessage("19 \n");
            //////////////////                    TextInfo.ParentCode = PoleCadInfo.NodeCode;
            //////////////////                    TextInfo.NodeCode = "";
            //////////////////                    TextInfo.NodeType = (int)Atend.Control.Enum.ProductType.LoadComment;
            //////////////////                    TextInfo.ProductCode = 0;
            //////////////////                    TextInfo.Insert();

            //////////////////                }
            //////////////////            }
            //////////////////        }
            //////////////////    }
            //////////////////}

        }

        public static bool IsInsideCurve(Curve cur, Point3d testPt)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            //ed.WriteMessage("PPOI:{0}\n", testPt);
            try
            {
                if (!cur.Closed)

                    // Cannot be inside

                    return false;
                //ed.WriteMessage("~~It was close\n");


                Polyline2d poly2d = cur as Polyline2d;

                if (poly2d != null &&

                    poly2d.PolyType != Poly2dType.SimplePoly)

                    // Not supported

                    return false;
                //ed.WriteMessage("~~It was suported Polyline2d\n");


                Point3d ptOnCurve =

                  cur.GetClosestPointTo(testPt, false);



                if (Tolerance.Equals(testPt, ptOnCurve))

                    return true;


                //ed.WriteMessage("~~It was not on shape \n");
                // Check it's planar



                Plane plane = cur.GetPlane();

                if (!cur.IsPlanar)

                    return false;

                //ed.WriteMessage("~~It was planar  \n");

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

                //ed.WriteMessage("~~It found intersectedpoints\n");

                ray.Dispose();


                int numberOfInters =

                  intersectionPoints.Count;

                if (numberOfInters == 0)

                    // Must be outside

                    return false;

                //ed.WriteMessage("~~It must be outside  \n");

                int nGlancingHits = 0;

                double epsilon = 2e-6; // (trust me on this)

                for (int i = 0; i < numberOfInters; i++)
                {

                    // Get the first point, and get its parameter

                    Point3d hitPt = intersectionPoints[i];

                    double hitParam = cur.GetParameterAtPoint(hitPt);
                    double inParam = hitParam - epsilon;
                    double outParam = hitParam + epsilon;
                    IncidenceType inIncidence = CurveIncidence(cur, inParam, testVector, normal);

                    IncidenceType outIncidence = CurveIncidence(cur, outParam, testVector, normal);

                    if ((inIncidence == IncidenceType.ToRight && outIncidence == IncidenceType.ToLeft) || (inIncidence == IncidenceType.ToLeft && outIncidence == IncidenceType.ToRight))
                    {
                        nGlancingHits++;
                    }
                }
                return ((numberOfInters + nGlancingHits) % 2 == 1);
            }
            catch (System.Exception ex)
            {
                ed.WriteMessage("ERROR : {0} \n", ex.Message);
                return false;
            }
            return true;

        }

        private static IncidenceType CurveIncidence(Curve cur, double param, Vector3d dir, Vector3d normal)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
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

        private enum IncidenceType
        {

            ToLeft = 0,

            ToRight = 1,

            ToFront = 2,

            Unknown

        };

        public static bool ChangeKeyStatus(ObjectId KeyObjectdId, bool IsClosed)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            //Database db = HostApplicationServices.WorkingDatabase;
            Database db = Application.DocumentManager.MdiActiveDocument.Database;
            double MyScale = Atend.Base.Design.DProductProperties.AccessSelectBySoftwareCode((int)Atend.Control.Enum.ProductType.MiddleJackPanel).Scale;
            bool Done = false;

            //ed.WriteMessage("KeyStatus :{0} \n", IsClosed);
            switch (IsClosed)
            {
                case true:

                    //ed.WriteMessage("~~~~~~~~2\n");
                    try
                    {
                        using (DocumentLock dl = Application.DocumentManager.MdiActiveDocument.LockDocument())
                        {
                            using (Transaction tr = db.TransactionManager.StartTransaction())
                            {
                                Line KL = tr.GetObject(KeyObjectdId, OpenMode.ForWrite, true) as Line;
                                if (KL != null)
                                {
                                    //ed.WriteMessage("12\n");
                                    double StartX = KL.StartPoint.X;
                                    double EndY = KL.EndPoint.Y;

                                    //need scale
                                    KL.EndPoint = new Point3d(StartX, EndY, 0);

                                }
                                tr.Commit();
                                Done = true;
                            }
                        }
                    }
                    catch (System.Exception ex)
                    {
                        ed.WriteMessage("Change Key Status : {0}\n", ex.Message);
                        Done = false;
                    }

                    break;
                case false:

                    //ed.WriteMessage("~~~~~~~3\n");

                    try
                    {
                        using (DocumentLock dl = Application.DocumentManager.MdiActiveDocument.LockDocument())
                        {
                            using (Transaction tr = db.TransactionManager.StartTransaction())
                            {
                                Line KL = tr.GetObject(KeyObjectdId, OpenMode.ForWrite, true) as Line;
                                if (KL != null)
                                {
                                    //ed.WriteMessage("22\n");
                                    if (KL.StartPoint.X == KL.EndPoint.X)
                                    {
                                        //ed.WriteMessage("12\n");
                                        double StartX = KL.StartPoint.X;
                                        double EndY = KL.EndPoint.Y;

                                        Point3d TempPoint = new Point3d(StartX - 5, EndY, 0);
                                        TempPoint = TempPoint.ScaleBy(MyScale, KL.EndPoint);
                                        KL.EndPoint = TempPoint; // new Point3d(StartX - 5, EndY, 0);

                                    }

                                }
                                tr.Commit();
                                Done = true;
                            }
                        }
                    }
                    catch (System.Exception ex)
                    {
                        ed.WriteMessage("Change Key Status : {0}\n", ex.Message);
                        Done = false;
                    }

                    break;
            }
            return Done;
        }

        public static ObjectId GetEntityGroup(ObjectId CurrentEntityOI)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            //Database db = HostApplicationServices.WorkingDatabase;
            Database db = Application.DocumentManager.MdiActiveDocument.Database;
            ObjectId CurrentGroupOI = ObjectId.Null;

            using (DocumentLock dlock = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.LockDocument())
            {

                using (Transaction tr = db.TransactionManager.StartTransaction())
                {
                    DBDictionary dict = (DBDictionary)tr.GetObject(db.GroupDictionaryId, OpenMode.ForRead, true);
                    foreach (DBDictionaryEntry dbdic in dict)
                    {
                        //ed.WriteMessage("dictionary list {0} . \n", dbdic.Key);
                        ObjectId groupId = dict.GetAt(dbdic.Key);
                        Group group = (Group)tr.GetObject(groupId, OpenMode.ForRead, true);
                        ObjectId[] objs = group.GetAllEntityIds();
                        foreach (ObjectId id in objs)
                        {

                            //ed.WriteMessage("sub entity id {0} \n", id);
                            if (id == CurrentEntityOI)
                            {
                                CurrentGroupOI = groupId;
                            }

                        }

                    }

                    //tr.Commit();
                }
            }
            return CurrentGroupOI;
        }

        public static ObjectIdCollection GetGroupSubEntities(ObjectId GroupOI)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            Database db = Application.DocumentManager.MdiActiveDocument.Database;
            ObjectIdCollection OIC = new ObjectIdCollection();

            using (DocumentLock dlock = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.LockDocument())
            {

                using (Transaction tr = db.TransactionManager.StartTransaction())
                {
                    DBDictionary dict = (DBDictionary)tr.GetObject(db.GroupDictionaryId, OpenMode.ForRead, true);
                    //foreach (DBDictionaryEntry dbdic in dict)
                    //{
                    //ed.WriteMessage("dictionary list {0} . \n", dbdic.Key);
                    //ObjectId groupId = dict.GetAt(dbdic.Key);
                    //if (groupId == GroupOI)
                    //{
                    Group group = tr.GetObject(GroupOI, OpenMode.ForRead, true) as Group;
                    if (group != null)
                    {
                        ObjectId[] objs = group.GetAllEntityIds();
                        foreach (ObjectId id in objs)
                        {

                            OIC.Add(id);
                        }

                    }
                }
            }
            return OIC;

        }

        public static DataRow GetBranchInfoByGuid(Guid brGuid)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            //ed.WriteMessage("I Am In GetBranchInfoByGuid\n");
            Atend.Base.Design.DBranch branch = Atend.Base.Design.DBranch.AccessSelectByCode(brGuid);
            System.Data.DataTable dtBranch = new System.Data.DataTable();
            dtBranch.Columns.Add("Code");
            dtBranch.Columns.Add("Lenght");
            dtBranch.Columns.Add("CondCode");
            dtBranch.Columns.Add("CondProductType");

            DataRow dr = dtBranch.NewRow();
            dr["Code"] = branch.Code.ToString();
            dr["Lenght"] = branch.Lenght.ToString();
            dr["CondCode"] = branch.ProductCode.ToString();
            dr["CondProductType"] = branch.ProductType.ToString();
            dtBranch.Rows.Add(dr);


            //ed.WriteMessage("************Lenght= " + dtBranch.Rows[0]["Lenght"].ToString() + "\n");
            ed.WriteMessage("###CondCode={0}\n", dtBranch.Rows[0]["CondCode"].ToString());
            //ed.WriteMessage("@@@@@ProductType={0}\n", dtBranch.Rows[0]["CondProductType"].ToString());
            return dtBranch.Rows[0];
            //System.Data.DataTable dtConductor = Atend.Base.Equipment.EConductor.SelectForCalculate(brGuid);
            //return dtConductor.Rows[0];

        }

        public static DataRow GetCondInfo(int CondCode, int CondProductType)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            System.Data.DataTable dtCond = new System.Data.DataTable();
            dtCond.Columns.Add("Resistance");
            dtCond.Columns.Add("Reactance");
            dtCond.Columns.Add("MaxCurrent");
            dtCond.Columns.Add("Capacitance");
            dtCond.Columns.Add("MaxCurrent1Second");
            DataRow dr = dtCond.NewRow();
            ed.WriteMessage("**I Am In GetCondInfo\n");
            switch (((Atend.Control.Enum.ProductType)(CondProductType)))
            {
                case Atend.Control.Enum.ProductType.Terminal:
                case Atend.Control.Enum.ProductType.Jumper:
                case Atend.Control.Enum.ProductType.Conductor:
                    Atend.Base.Equipment.EConductorTip condTip = Atend.Base.Equipment.EConductorTip.AccessSelectByCode(CondCode);
                    Atend.Base.Equipment.EConductor cond = Atend.Base.Equipment.EConductor.AccessSelectByCode(condTip.PhaseProductCode);
                    dr["Resistance"] = cond.Resistance.ToString();
                    dr["Reactance"] = cond.Reactance.ToString();
                    dr["MaxCurrent"] = cond.MaxCurrent.ToString();
                    dr["Capacitance"] = cond.Capacitance.ToString();
                    dr["MaxCurrent1Second"] = cond.MaxCurrent1Second.ToString();
                    //ed.WriteMessage("@@@@@@@I AM IN GetCondInfo Re= " + cond.Resistance + "\n");
                    dtCond.Rows.Add(dr);
                    break;
                case Atend.Control.Enum.ProductType.SelfKeeper:
                    Atend.Base.Equipment.ESelfKeeperTip SelfTip = Atend.Base.Equipment.ESelfKeeperTip.AccessSelectByCode(CondCode);
                    Atend.Base.Equipment.ESelfKeeper SelfKeeper = Atend.Base.Equipment.ESelfKeeper.AccessSelectByCode(SelfTip.PhaseProductCode);
                    dr["Resistance"] = SelfKeeper.Resistance.ToString();
                    dr["Reactance"] = SelfKeeper.Reactance.ToString();
                    dr["MaxCurrent"] = SelfKeeper.MaxCurrent.ToString();
                    dr["Capacitance"] = SelfKeeper.Capacitance.ToString();
                    dr["MaxCurrent1Second"] = SelfKeeper.MaxCurrent1Second.ToString();

                    dtCond.Rows.Add(dr);
                    break;
                case Atend.Control.Enum.ProductType.GroundCabel:
                    ed.WriteMessage("ProducType:{0}\n", CondProductType);
                    Atend.Base.Equipment.EGroundCabelTip GroundTip = Atend.Base.Equipment.EGroundCabelTip.AccessSelectByCode(CondCode);
                    Atend.Base.Equipment.EGroundCabel Ground = Atend.Base.Equipment.EGroundCabel.AccessSelectByCode(GroundTip.PhaseProductCode);
                    ed.WriteMessage("********************Ground.Resistance:{0}\n", Ground.Resistance);

                    dr["Resistance"] = Ground.Resistance.ToString();
                    dr["Reactance"] = Ground.Reactance.ToString();
                    dr["MaxCurrent"] = Ground.MaxCurrent.ToString();
                    dr["Capacitance"] = Ground.Capacitance.ToString();
                    dr["MaxCurrent1Second"] = Ground.MaxCurrent1Second.ToString();

                    dtCond.Rows.Add(dr);
                    break;
                case Atend.Control.Enum.ProductType.CatOut:
                    ed.WriteMessage("ProducType:\n", CondProductType);
                    //Atend.Base.Equipment.EGroundCabelTip GroundTip = Atend.Base.Equipment.EGroundCabelTip.AccessSelectByCode(CondCode);
                    //Atend.Base.Equipment.EGroundCabel Ground = Atend.Base.Equipment.EGroundCabel.AccessSelectByCode(GroundTip.PhaseProductCode);
                    //ed.WriteMessage("Ground.Resistance:\n", Ground.Resistance);

                    dr["Resistance"] = 0;
                    dr["Reactance"] = 0;
                    dr["MaxCurrent"] = 0;
                    dr["Capacitance"] = 0;
                    dr["MaxCurrent1Second"] = "0";

                    dtCond.Rows.Add(dr);
                    break;

                case Atend.Control.Enum.ProductType.Breaker:
                    ed.WriteMessage("ProducType:\n", CondProductType);
                    //Atend.Base.Equipment.EGroundCabelTip GroundTip = Atend.Base.Equipment.EGroundCabelTip.AccessSelectByCode(CondCode);
                    //Atend.Base.Equipment.EGroundCabel Ground = Atend.Base.Equipment.EGroundCabel.AccessSelectByCode(GroundTip.PhaseProductCode);
                    //ed.WriteMessage("Ground.Resistance:\n", Ground.Resistance);

                    dr["Resistance"] = 0;
                    dr["Reactance"] = 0;
                    dr["MaxCurrent"] = 0;
                    dr["Capacitance"] = 0;
                    dr["MaxCurrent1Second"] = "0";

                    dtCond.Rows.Add(dr);
                    break;
                case Atend.Control.Enum.ProductType.Disconnector:
                    ed.WriteMessage("ProducType:\n", CondProductType);
                    //Atend.Base.Equipment.EGroundCabelTip GroundTip = Atend.Base.Equipment.EGroundCabelTip.AccessSelectByCode(CondCode);
                    //Atend.Base.Equipment.EGroundCabel Ground = Atend.Base.Equipment.EGroundCabel.AccessSelectByCode(GroundTip.PhaseProductCode);
                    //ed.WriteMessage("Ground.Resistance:\n", Ground.Resistance);

                    dr["Resistance"] = 0;
                    dr["Reactance"] = 0;
                    dr["MaxCurrent"] = 0;
                    dr["Capacitance"] = 0;
                    dr["MaxCurrent1Second"] = "0";

                    dtCond.Rows.Add(dr);
                    break;
                default:
                    Atend.Base.Equipment.EConductor cond2 = Atend.Base.Equipment.EConductor.AccessSelectByCode(CondCode);
                    break;
            }
            //ed.writeMessage("Cond[Resistence=" + dtCond.Rows[0]["Resistance"].ToString()+"\n");
            return dtCond.Rows[0];

        }

        public static DataRow GetCondInfo(Guid CondCode, int CondProductType)
        {

            //فقط برای سطح مقطع
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            System.Data.DataTable dtCond = new System.Data.DataTable();
            dtCond.Columns.Add("Resistance");
            dtCond.Columns.Add("Reactance");
            dtCond.Columns.Add("MaxCurrent");
            dtCond.Columns.Add("Capacitance");
            dtCond.Columns.Add("MaxCurrent1Second");
            DataRow dr = dtCond.NewRow();
            //ed.writeMessage("**I Am In GetCondInfo\n");
            switch (((Atend.Control.Enum.ProductType)(CondProductType)))
            {
                case Atend.Control.Enum.ProductType.Terminal:
                case Atend.Control.Enum.ProductType.Jumper:
                case Atend.Control.Enum.ProductType.Conductor:
                    //Atend.Base.Equipment.EConductorTip condTip = Atend.Base.Equipment.EConductorTip.SelectByXCode(CondCode);
                    //Atend.Base.Equipment.EConductor cond = Atend.Base.Equipment.EConductor.SelectByXCode(condTip.PhaseProductXCode);
                    Atend.Base.Equipment.EConductor cond = Atend.Base.Equipment.EConductor.SelectByXCode(CondCode);

                    dr["Resistance"] = cond.Resistance.ToString();
                    dr["Reactance"] = cond.Reactance.ToString();
                    dr["MaxCurrent"] = cond.MaxCurrent.ToString();
                    dr["Capacitance"] = cond.Capacitance.ToString();
                    dr["MaxCurrent1Second"] = cond.MaxCurrent1Second.ToString();
                    ed.WriteMessage("@@@@@@@I AM IN GetCondInfo Re= " + cond.Resistance + "\n");
                    dtCond.Rows.Add(dr);
                    break;
                case Atend.Control.Enum.ProductType.SelfKeeper:
                    //Atend.Base.Equipment.ESelfKeeperTip SelfTip = Atend.Base.Equipment.ESelfKeeperTip.SelectByXCode(CondCode);
                    //Atend.Base.Equipment.ESelfKeeper SelfKeeper = Atend.Base.Equipment.ESelfKeeper.SelectByXCode(SelfTip.PhaseProductxCode);
                    Atend.Base.Equipment.ESelfKeeper SelfKeeper = Atend.Base.Equipment.ESelfKeeper.SelectByXCode(CondCode);

                    dr["Resistance"] = SelfKeeper.Resistance.ToString();
                    dr["Reactance"] = SelfKeeper.Reactance.ToString();
                    dr["MaxCurrent"] = SelfKeeper.MaxCurrent.ToString();
                    dr["Capacitance"] = SelfKeeper.Capacitance.ToString();
                    dtCond.Rows.Add(dr);
                    break;
                case Atend.Control.Enum.ProductType.GroundCabel:
                    ed.WriteMessage("ProducType:\n", CondProductType);
                    //Atend.Base.Equipment.EGroundCabelTip GroundTip = Atend.Base.Equipment.EGroundCabelTip.AccessSelectByCode(CondCode);
                    //Atend.Base.Equipment.EGroundCabel Ground = Atend.Base.Equipment.EGroundCabel.AccessSelectByCode(GroundTip.PhaseProductCode);
                    Atend.Base.Equipment.EGroundCabel Ground = Atend.Base.Equipment.EGroundCabel.SelectByXCode(CondCode);
                    ed.WriteMessage("Ground.Resistance:\n", Ground.Resistance);

                    dr["Resistance"] = Ground.Resistance.ToString();
                    dr["Reactance"] = Ground.Reactance.ToString();
                    dr["MaxCurrent"] = Ground.MaxCurrent.ToString();
                    dr["Capacitance"] = Ground.Capacitance.ToString();
                    dr["MaxCurrent1Second"] = "1";

                    dtCond.Rows.Add(dr);
                    break;
                case Atend.Control.Enum.ProductType.CatOut:
                    ed.WriteMessage("ProducType:\n", CondProductType);
                    //Atend.Base.Equipment.EGroundCabelTip GroundTip = Atend.Base.Equipment.EGroundCabelTip.AccessSelectByCode(CondCode);
                    //Atend.Base.Equipment.EGroundCabel Ground = Atend.Base.Equipment.EGroundCabel.AccessSelectByCode(GroundTip.PhaseProductCode);
                    //ed.WriteMessage("Ground.Resistance:\n", Ground.Resistance);

                    dr["Resistance"] = 0;
                    dr["Reactance"] = 0;
                    dr["MaxCurrent"] = 0;
                    dr["Capacitance"] = 0;
                    dr["MaxCurrent1Second"] = "0";

                    dtCond.Rows.Add(dr);
                    break;

                case Atend.Control.Enum.ProductType.Breaker:
                    ed.WriteMessage("ProducType:\n", CondProductType);
                    //Atend.Base.Equipment.EGroundCabelTip GroundTip = Atend.Base.Equipment.EGroundCabelTip.AccessSelectByCode(CondCode);
                    //Atend.Base.Equipment.EGroundCabel Ground = Atend.Base.Equipment.EGroundCabel.AccessSelectByCode(GroundTip.PhaseProductCode);
                    //ed.WriteMessage("Ground.Resistance:\n", Ground.Resistance);

                    dr["Resistance"] = 0;
                    dr["Reactance"] = 0;
                    dr["MaxCurrent"] = 0;
                    dr["Capacitance"] = 0;
                    dr["MaxCurrent1Second"] = "0";

                    dtCond.Rows.Add(dr);
                    break;
                case Atend.Control.Enum.ProductType.Disconnector:
                    ed.WriteMessage("ProducType:\n", CondProductType);
                    //Atend.Base.Equipment.EGroundCabelTip GroundTip = Atend.Base.Equipment.EGroundCabelTip.AccessSelectByCode(CondCode);
                    //Atend.Base.Equipment.EGroundCabel Ground = Atend.Base.Equipment.EGroundCabel.AccessSelectByCode(GroundTip.PhaseProductCode);
                    //ed.WriteMessage("Ground.Resistance:\n", Ground.Resistance);

                    dr["Resistance"] = 0;
                    dr["Reactance"] = 0;
                    dr["MaxCurrent"] = 0;
                    dr["Capacitance"] = 0;
                    dr["MaxCurrent1Second"] = "0";

                    dtCond.Rows.Add(dr);
                    break;

                default:
                    Atend.Base.Equipment.EConductor cond2 = Atend.Base.Equipment.EConductor.SelectByXCode(CondCode);
                    break;
            }
            //ed.writeMessage("Cond[Resistence=" + dtCond.Rows[0]["Resistance"].ToString()+"\n");
            return dtCond.Rows[0];

        }

        public static int GetNodeLoadByGuid(Guid guid, out DataRow drLoad, out System.Data.DataTable dtPkgLoad)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            System.Data.DataTable dt = new System.Data.DataTable();
            System.Data.DataColumn dcCode = new System.Data.DataColumn("Code");
            System.Data.DataColumn dcDesignCode = new System.Data.DataColumn("DesignCode");
            System.Data.DataColumn dcP = new System.Data.DataColumn("P");
            System.Data.DataColumn dcQ = new System.Data.DataColumn("Q");
            System.Data.DataColumn dcPF = new System.Data.DataColumn("PF");
            System.Data.DataColumn dcI2 = new System.Data.DataColumn("I2");
            System.Data.DataColumn dcPF2 = new System.Data.DataColumn("PF2");
            System.Data.DataColumn dcR = new System.Data.DataColumn("R");
            System.Data.DataColumn dcX = new System.Data.DataColumn("X");
            System.Data.DataColumn dcModeNumber = new System.Data.DataColumn("ModeNumber");
            System.Data.DataColumn dcTypeCode = new System.Data.DataColumn("TypeCode");
            System.Data.DataColumn dcName = new System.Data.DataColumn("Name");
            dt.Columns.Add(dcCode);
            dt.Columns.Add(dcDesignCode);
            dt.Columns.Add(dcP);
            dt.Columns.Add(dcQ);
            dt.Columns.Add(dcPF);
            dt.Columns.Add(dcI2);
            dt.Columns.Add(dcPF2);
            dt.Columns.Add(dcR);
            dt.Columns.Add(dcX);
            dt.Columns.Add(dcTypeCode);
            dt.Columns.Add(dcModeNumber);
            dt.Columns.Add(dcName);


            System.Data.DataTable dtPack = new System.Data.DataTable();
            System.Data.DataColumn dcCode1 = new System.Data.DataColumn("Code");
            System.Data.DataColumn dcAmper = new System.Data.DataColumn("Amper");
            System.Data.DataColumn dcPhaseCount = new System.Data.DataColumn("PhaseCount");
            System.Data.DataColumn dcFActorPower = new System.Data.DataColumn("FactorPower");
            System.Data.DataColumn dcFactorConcurency = new System.Data.DataColumn("FactorConcurency");
            System.Data.DataColumn dcName1 = new System.Data.DataColumn("Name");
            System.Data.DataColumn dcVoltage = new System.Data.DataColumn("Voltage");
            System.Data.DataColumn dcCount = new System.Data.DataColumn("Count");
            dtPack.Columns.Add(dcCode1);
            dtPack.Columns.Add(dcAmper);
            dtPack.Columns.Add(dcPhaseCount);
            dtPack.Columns.Add(dcFActorPower);
            dtPack.Columns.Add(dcFactorConcurency);
            dtPack.Columns.Add(dcName1);
            dtPack.Columns.Add(dcVoltage);
            dtPack.Columns.Add(dcCount);
            //ed.writeMessage("Buil DatTAble\n");
            try
            {
                //ed.WriteMessage("ConsolGuid= " + guid + "\n");
                Atend.Base.Design.DPackage consol = Atend.Base.Design.DPackage.AccessSelectByCode(guid);

                //ed.WriteMessage("DPackage.LoadCode={0}\n", consol.LoadCode);
                Atend.Base.Calculating.CLoadFactor loadFactor = Atend.Base.Calculating.CLoadFactor.AccessSelectByCode(consol.LoadCode);
                //ed.WriteMessage("Consol.LAodCode= " + consol.LoadCode + "\n");
                if (consol.LoadCode == 0)
                {
                    //ed.writeMessage("I Am In The IF\n");
                    drLoad = null;
                    dtPkgLoad = null;
                    return 0;
                }
                else
                {
                    dtPkgLoad = Atend.Base.Calculating.CPackageLoad.AccessSelectByLoadFactorCode(consol.LoadCode);
                    //ed.writeMessage("dtPAc.Rows>count= "+dtPack.Rows.Count+"\n");
                    if (loadFactor.ModeNumber.IndexOf('4') != -1)
                    {
                        System.Data.DataTable LoadPDT = Atend.Base.Calculating.CPackageLoad.AccessSelectByLoadFactorCode(loadFactor.Code);
                        //ed.writeMessage("LoadPDT.Rows.Count=  "+LoadPDT.Rows.Count+"\n");
                        for (int i = 0; i < LoadPDT.Rows.Count; i++)
                        {
                            //ed.writeMessage(LoadPDT.Rows[i][2].ToString());
                            Atend.Base.Calculating.CDloadFactor DL = Atend.Base.Calculating.CDloadFactor.AccessSelectByCode(Convert.ToInt32(LoadPDT.Rows[i]["DLaodFactorCode"].ToString()));
                            DataRow drpak = dtPack.NewRow();
                            drpak["Code"] = DL.Code;
                            drpak["Name"] = DL.Name;
                            drpak["Amper"] = DL.Amper;
                            drpak["PhaseCount"] = DL.PhaseCount;
                            drpak["FactorPower"] = DL.FactorPower;
                            drpak["FactorConcurency"] = DL.FactorConcurency;
                            drpak["Voltage"] = DL.Voltage;
                            drpak["Count"] = LoadPDT.Rows[i]["Count"];
                            dtPack.Rows.Add(drpak);

                        }
                        dtPkgLoad = dtPack;
                    }


                    DataRow dr = dt.NewRow();
                    dr["Code"] = loadFactor.Code;
                    dr["DesignCode"] = loadFactor.DesignCode;
                    dr["P"] = loadFactor.P;
                    dr["Q"] = loadFactor.Q;
                    dr["PF"] = loadFactor.PF;
                    dr["I2"] = loadFactor.I2;
                    dr["PF2"] = loadFactor.PF2;
                    dr["R"] = loadFactor.R;
                    dr["X"] = loadFactor.X;
                    dr["ModeNumber"] = loadFactor.ModeNumber;
                    dr["TypeCode"] = loadFactor.TypeCode;
                    dr["Name"] = loadFactor.Name;
                    dt.Rows.Add(dr);
                    drLoad = dt.Rows[0];
                    //ed.writeMessage("AddLoad\n");
                }
                //ed.writeMessage("Finish Get Node Load\n");
                return 0;
            }
            catch (System.Exception ex)
            {
                ed.WriteMessage("Error In GetNodeLoadByGuid= " + ex.Message + "\n");
                drLoad = null;
                dtPkgLoad = null;
                return 1;
            }
        }

        public static int GetNextConsol(int CurrentConsolOI, int CurrentBranchOI)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            ObjectId _CurrentConsolOI = new ObjectId(new IntPtr(Convert.ToInt32(CurrentConsolOI)));
            ObjectId _CurrentBranchOI = new ObjectId(new IntPtr(Convert.ToInt32(CurrentBranchOI)));
            Int32 NextConsol = 0;

            using (DocumentLock dlock = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.LockDocument())
            {
                if (_CurrentConsolOI != null && _CurrentBranchOI != null)
                {
                    Atend.Base.Acad.AT_INFO BranchInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(_CurrentBranchOI);
                    if (BranchInfo.ParentCode != "NONE")
                    {
                        if (BranchInfo.NodeType == (int)Atend.Control.Enum.ProductType.Jumper)
                        {
                            #region Branch was jumper
                            ObjectId NextPoleOI = ObjectId.Null;
                            ObjectId NCOI = ObjectId.Null;
                            Atend.Base.Acad.AT_SUB JumperSub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(_CurrentBranchOI);
                            foreach (ObjectId oi in JumperSub.SubIdCollection)
                            {
                                Atend.Base.Acad.AT_SUB ConductorSub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(oi);
                                foreach (ObjectId oii in ConductorSub.SubIdCollection)
                                {
                                    if (oii == _CurrentConsolOI)
                                    {

                                        NCOI = GetNextConsol(_CurrentConsolOI, new Guid(Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oi).NodeCode));
                                        if (NCOI != null)
                                        {
                                            Atend.Base.Acad.AT_SUB NextConsolSub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(NCOI);
                                            foreach (ObjectId oiii in NextConsolSub.SubIdCollection)
                                            {
                                                Atend.Base.Acad.AT_INFO NextConsolSubInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oiii);
                                                if (NextConsolSubInfo.ParentCode != "NONE" && NextConsolSubInfo.NodeType == (int)Atend.Control.Enum.ProductType.Pole)
                                                {
                                                    NextPoleOI = oiii;
                                                }
                                            }
                                        }

                                    }

                                }
                            }
                            //
                            ObjectIdCollection Consols = new ObjectIdCollection();
                            if (NextPoleOI != null && NCOI != null)
                            {

                                Atend.Base.Acad.AT_SUB JumperSub1 = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(_CurrentBranchOI);
                                foreach (ObjectId oi in JumperSub1.SubIdCollection)
                                {
                                    Atend.Base.Acad.AT_INFO JumperSubInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oi);
                                    if (JumperSubInfo.ParentCode != "NONE" && JumperSubInfo.NodeType == (int)Atend.Control.Enum.ProductType.Conductor)
                                    {
                                        Atend.Base.Acad.AT_SUB ConductorSub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(oi);
                                        foreach (ObjectId oii in ConductorSub.SubIdCollection)
                                        {
                                            Atend.Base.Acad.AT_INFO ConsolInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oii);
                                            if (ConsolInfo.ParentCode != "NONE" && ConsolInfo.NodeType == (int)Atend.Control.Enum.ProductType.Consol)
                                            {
                                                Consols.Add(oii);
                                            }
                                        }
                                    }
                                }

                                foreach (ObjectId oi in Consols)
                                {
                                    if (oi != NCOI)
                                    {
                                        Atend.Base.Acad.AT_SUB ConsolSub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(oi);
                                        foreach (ObjectId oii in ConsolSub.SubIdCollection)
                                        {
                                            Atend.Base.Acad.AT_INFO ConsolSubInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oii);
                                            if (ConsolSubInfo.ParentCode != "NONE" && ConsolSubInfo.NodeType == (int)Atend.Control.Enum.ProductType.Pole)
                                            {
                                                if (oii == NextPoleOI)
                                                {
                                                    NextConsol = Convert.ToInt32(oi.ToString().Substring(1, oi.ToString().Length - 2));
                                                }
                                            }
                                        }
                                    }
                                }

                            }
                            #endregion
                        }

                        if (BranchInfo.NodeType == (int)Atend.Control.Enum.ProductType.Conductor)
                        {
                            #region Branch was Conductor

                            Atend.Base.Acad.AT_SUB ConductorSub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(_CurrentBranchOI);
                            foreach (ObjectId oi in ConductorSub.SubIdCollection)
                            {
                                if (oi != _CurrentConsolOI)
                                {
                                    Atend.Base.Acad.AT_INFO ConductorSubInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oi);
                                    if (ConductorSubInfo.ParentCode != "NONE" && ConductorSubInfo.NodeType == (int)Atend.Control.Enum.ProductType.Consol)
                                    {

                                        NextConsol = Convert.ToInt32(oi.ToString().Substring(1, oi.ToString().Length - 2));
                                    }
                                }
                            }

                            #endregion
                        }


                    }
                }
            }

            //ed.writeMessage("NEXT CONSOL:{0}\n", NextConsol);
            return NextConsol;

        }

        public static System.Data.DataTable GetConsolInfoByObjectId(int CurrentConsolOI)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            ObjectId _CurrentConsolOI = new ObjectId(new IntPtr(Convert.ToInt32(CurrentConsolOI)));
            System.Data.DataColumn col1 = new System.Data.DataColumn("ConsolGuid");
            System.Data.DataColumn col2 = new System.Data.DataColumn("PoleGuid");
            System.Data.DataColumn col3 = new System.Data.DataColumn("BranchGuid");
            //keep objectid as integer
            System.Data.DataColumn col4 = new System.Data.DataColumn("ConsolObjectId");
            System.Data.DataColumn col5 = new System.Data.DataColumn("BranchObjectId");

            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Columns.Add(col1);
            dt.Columns.Add(col2);
            dt.Columns.Add(col3);
            dt.Columns.Add(col4);
            dt.Columns.Add(col5);


            DataSet ds = new DataSet();
            ds.Tables.Add(dt);

            using (DocumentLock dlock = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.LockDocument())
            {

                //ed.writeMessage("i am in GetConsolInfoByObjectId {0} \n", _CurrentConsolOI);
                if (_CurrentConsolOI != ObjectId.Null)
                {
                    Atend.Base.Acad.AT_INFO CurrentConsolInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(_CurrentConsolOI);
                    //ed.WriteMessage("11 \n");
                    Atend.Base.Acad.AT_SUB CurrentConsolSub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(_CurrentConsolOI);
                    // ed.WriteMessage("12 \n");
                    foreach (ObjectId oi in CurrentConsolSub.SubIdCollection)
                    {

                        //ed.WriteMessage("13 \n");
                        Atend.Base.Acad.AT_INFO at_info = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oi);
                        //ed.WriteMessage("14 \n");
                        if (at_info.ParentCode != "NONE" && at_info.NodeType == (int)Atend.Control.Enum.ProductType.Conductor)
                        {
                            //ed.WriteMessage("15 \n");
                            DataRow dr = dt.NewRow();
                            dr["ConsolGuid"] = CurrentConsolInfo.NodeCode;
                            dr["PoleGuid"] = CurrentConsolInfo.ParentCode;
                            dr["BranchGuid"] = at_info.NodeCode;
                            dr["ConsolObjectId"] = Convert.ToInt32(_CurrentConsolOI.ToString().Substring(1, _CurrentConsolOI.ToString().Length - 2));
                            dr["BranchObjectId"] = Convert.ToInt32(oi.ToString().Substring(1, oi.ToString().Length - 2));
                            dt.Rows.Add(dr);


                            Atend.Base.Acad.AT_SUB ConductorSub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(oi);
                            foreach (ObjectId oii in ConductorSub.SubIdCollection)
                            {

                                Atend.Base.Acad.AT_INFO JumperInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oii);
                                if (JumperInfo.ParentCode != "NONE" && JumperInfo.NodeType == (int)Atend.Control.Enum.ProductType.Jumper)
                                {
                                    dr = dt.NewRow();
                                    dr["ConsolGuid"] = CurrentConsolInfo.NodeCode;
                                    dr["PoleGuid"] = CurrentConsolInfo.ParentCode;
                                    dr["BranchGuid"] = "JUMPER";
                                    dr["ConsolObjectId"] = Convert.ToInt32(_CurrentConsolOI.ToString().Substring(1, _CurrentConsolOI.ToString().Length - 2));
                                    dr["BranchObjectId"] = Convert.ToInt32(oii.ToString().Substring(1, oii.ToString().Length - 2));
                                    dt.Rows.Add(dr);
                                }

                            }

                        }

                    }

                }

                //foreach (DataRow dr in dt.Rows)
                //{

                //    ed.WriteMessage("\n~~~~START~~~~~~\n");
                //    ed.WriteMessage("\nConsolGuid : {0} , PoleGuid : {1} \n", dr["ConsolGuid"], dr["PoleGuid"]);
                //    ed.WriteMessage("\nBranchGuid : {0} , ConsolObjectId : {1} \n", dr["BranchGuid"], dr["ConsolObjectId"]);
                //    ed.WriteMessage("\nBranchObjectId : {0} \n", dr["BranchObjectid"]);
                //    ed.WriteMessage("\n~~~~END~~~~~~\n");
                //}

                //ed.writeMessage("i am not in GetConsolInfoByObjectId \n");
            }
            return ds.Tables[0];

        }

        public static void CreateWipeout(Point2dCollection Points)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Transaction tr = db.TransactionManager.StartTransaction();
            using (DocumentLock dlock = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.LockDocument())
            {
                try
                {
                    using (tr)
                    {

                        BlockTable bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead, false, true);
                        BlockTableRecord btr = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite, false, true);
                        Point2dCollection pts = Points;
                        Wipeout wo = new Wipeout();
                        wo.SetDatabaseDefaults(db);
                        wo.SetFrom(pts, new Vector3d(0.0, 0.0, 0.1));
                        btr.AppendEntity(wo);
                        tr.AddNewlyCreatedDBObject(wo, true);
                        tr.Commit();
                    }
                }
                catch (System.Exception ex)
                {
                    ed.WriteMessage("ERROR Wipeout : {0} \n", ex.Message);
                }
            }
        }

        public static Point3d CenterOfGroup(ObjectId GroupOI)
        {
            Point3d CenterPoint = Point3d.Origin;
            if (GroupOI != null)
            {
                Atend.Base.Acad.AT_INFO GroupInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(GroupOI);
                ObjectIdCollection OIC = GetGroupSubEntities(GroupOI);
                switch ((Atend.Control.Enum.ProductType)GroupInfo.NodeType)
                {
                    case Atend.Control.Enum.ProductType.Transformer:
                        #region transaformer
                        if (OIC.Count > 0)
                        {
                            ObjectIdCollection Nodes = new ObjectIdCollection();
                            foreach (ObjectId oi in OIC)
                            {
                                Atend.Base.Acad.AT_INFO subInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oi);
                                if (subInfo.ParentCode != "NONE" && subInfo.NodeType == (int)Atend.Control.Enum.ProductType.HeaderCabel)
                                {
                                    Nodes.Add(oi);
                                }
                                if (subInfo.ParentCode != "NONE" && subInfo.NodeType == (int)Atend.Control.Enum.ProductType.KablSho)
                                {
                                    Nodes.Add(oi);
                                }
                            }
                            if (Nodes.Count == 2)
                            {
                                LineSegment3d ls = new LineSegment3d(Atend.Global.Acad.UAcad.CenterOfEntity(Atend.Global.Acad.UAcad.GetEntityByObjectID(Nodes[0])), Atend.Global.Acad.UAcad.CenterOfEntity(Atend.Global.Acad.UAcad.GetEntityByObjectID(Nodes[1])));
                                CenterPoint = ls.MidPoint;
                            }
                        }
                        #endregion
                        break;

                    case Atend.Control.Enum.ProductType.MiddleJackPanel:
                        #region MiddleJackPanel
                        if (OIC.Count > 0)
                        {
                            ObjectIdCollection Nodes = new ObjectIdCollection();
                            foreach (ObjectId oi in OIC)
                            {
                                Atend.Base.Acad.AT_INFO subInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oi);
                                if (subInfo.ParentCode != "NONE" && subInfo.NodeType == (int)Atend.Control.Enum.ProductType.Cell)
                                {
                                    Nodes.Add(oi);
                                }
                            }
                            if (Nodes.Count > 0)
                            {
                                LineSegment3d ls = new LineSegment3d(Atend.Global.Acad.UAcad.CenterOfEntity(Atend.Global.Acad.UAcad.GetEntityByObjectID(Nodes[0])), Atend.Global.Acad.UAcad.CenterOfEntity(Atend.Global.Acad.UAcad.GetEntityByObjectID(Nodes[Nodes.Count - 1])));
                                CenterPoint = ls.MidPoint;
                            }
                        }

                        #endregion
                        break;

                    case Atend.Control.Enum.ProductType.WeekJackPanel:
                        #region WeekJackPanel
                        if (OIC.Count > 0)
                        {
                            ObjectIdCollection Nodes = new ObjectIdCollection();
                            foreach (ObjectId oi in OIC)
                            {
                                Atend.Base.Acad.AT_INFO subInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oi);
                                if (subInfo.ParentCode != "NONE" && subInfo.NodeType == (int)Atend.Control.Enum.ProductType.HeaderCabel)
                                {
                                    Nodes.Add(oi);
                                }
                            }
                            if (Nodes.Count > 0)
                            {
                                LineSegment3d ls = new LineSegment3d(Atend.Global.Acad.UAcad.CenterOfEntity(Atend.Global.Acad.UAcad.GetEntityByObjectID(Nodes[1])), Atend.Global.Acad.UAcad.CenterOfEntity(Atend.Global.Acad.UAcad.GetEntityByObjectID(Nodes[Nodes.Count - 1])));
                                CenterPoint = ls.MidPoint;
                            }
                        }

                        #endregion
                        break;
                }


            }
            return CenterPoint;
        }

        //for status report by selected area
        public static System.Data.DataTable GetEquipsOfAnErea()
        {
            System.Data.DataTable dt = new System.Data.DataTable();

            System.Data.DataColumn dc1 = new System.Data.DataColumn("id");
            System.Data.DataColumn dc2 = new System.Data.DataColumn("type");

            dt.Columns.Add(dc1);
            dt.Columns.Add(dc2);


            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            PromptSelectionOptions pso = new PromptSelectionOptions();
            pso.MessageForAdding = "انتخاب منطقه مورد نظر";
            PromptSelectionResult psr = ed.GetSelection(pso);
            if (psr.Status == PromptStatus.OK)
            {
                ObjectId[] OIS = psr.Value.GetObjectIds();
                foreach (ObjectId oi in OIS)
                {
                    //ed.WriteMessage("~~~@@@~~~:{0}\n", oi);
                    Atend.Base.Acad.AT_INFO info = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oi);
                    if (info.ParentCode != "NONE" && info.NodeCode != string.Empty)
                    {
                        dt.Rows.Add(info.NodeCode, info.NodeType);
                    }
                }
            }

            frmTest t = new frmTest();
            t.dataGridView1.DataSource = dt;
            t.ShowDialog();

            return dt;
        }

        //2Electrical
        public static System.Data.DataTable GetNodeInfoByObjectId(int CurrentNodeOI)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            ObjectId _CurrentConsolOI = new ObjectId(new IntPtr(Convert.ToInt32(CurrentNodeOI)));
            System.Data.DataColumn col1 = new System.Data.DataColumn("ConsolGuid");
            System.Data.DataColumn col2 = new System.Data.DataColumn("PoleGuid");
            System.Data.DataColumn col3 = new System.Data.DataColumn("BranchGuid");
            //keep objectid as integer
            System.Data.DataColumn col4 = new System.Data.DataColumn("ConsolObjectId");
            System.Data.DataColumn col5 = new System.Data.DataColumn("BranchObjectId");
            System.Data.DataColumn col6 = new System.Data.DataColumn("BranchType");

            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Columns.Add(col1);
            dt.Columns.Add(col2);
            dt.Columns.Add(col3);
            dt.Columns.Add(col4);
            dt.Columns.Add(col5);
            dt.Columns.Add(col6);

            DataSet ds = new DataSet();
            ds.Tables.Add(dt);

            using (DocumentLock dlock = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.LockDocument())
            {


                #region Conductor
                if (_CurrentConsolOI != ObjectId.Null)
                {
                    Atend.Base.Acad.AT_INFO CurrentConsolInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(_CurrentConsolOI);
                    Atend.Base.Acad.AT_SUB CurrentConsolSub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(_CurrentConsolOI);
                    foreach (ObjectId oi in CurrentConsolSub.SubIdCollection)
                    {

                        Atend.Base.Acad.AT_INFO at_info = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oi);
                        if (at_info.ParentCode != "NONE" && at_info.NodeType == (int)Atend.Control.Enum.ProductType.Conductor)
                        {
                            DataRow dr = dt.NewRow();
                            dr["ConsolGuid"] = CurrentConsolInfo.NodeCode;
                            dr["PoleGuid"] = CurrentConsolInfo.ParentCode;
                            dr["BranchGuid"] = at_info.NodeCode;
                            dr["ConsolObjectId"] = Convert.ToInt32(_CurrentConsolOI.ToString().Substring(1, _CurrentConsolOI.ToString().Length - 2));
                            dr["BranchObjectId"] = Convert.ToInt32(oi.ToString().Substring(1, oi.ToString().Length - 2));
                            dt.Rows.Add(dr);

                            Atend.Base.Acad.AT_SUB ConductorSub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(oi);
                            foreach (ObjectId oii in ConductorSub.SubIdCollection)
                            {

                                Atend.Base.Acad.AT_INFO JumperInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oii);
                                if (JumperInfo.ParentCode != "NONE" && JumperInfo.NodeType == (int)Atend.Control.Enum.ProductType.Jumper)
                                {
                                    dr = dt.NewRow();
                                    dr["ConsolGuid"] = CurrentConsolInfo.NodeCode;
                                    dr["PoleGuid"] = CurrentConsolInfo.ParentCode;
                                    dr["BranchGuid"] = "JUMPER";
                                    dr["ConsolObjectId"] = Convert.ToInt32(_CurrentConsolOI.ToString().Substring(1, _CurrentConsolOI.ToString().Length - 2));
                                    dr["BranchObjectId"] = Convert.ToInt32(oii.ToString().Substring(1, oii.ToString().Length - 2));
                                    dt.Rows.Add(dr);
                                }

                            }

                        }

                    }

                }
                #endregion


                #region Cable
                if (_CurrentConsolOI != ObjectId.Null)
                {
                    Atend.Base.Acad.AT_INFO CurrentConsolInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(_CurrentConsolOI);
                    //ed.WriteMessage("11 \n");
                    Atend.Base.Acad.AT_SUB CurrentConsolSub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(_CurrentConsolOI);
                    // ed.WriteMessage("12 \n");
                    foreach (ObjectId oi in CurrentConsolSub.SubIdCollection)
                    {

                        //ed.WriteMessage("13 \n");
                        Atend.Base.Acad.AT_INFO at_info = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oi);
                        //ed.WriteMessage("14 \n");
                        if (at_info.ParentCode != "NONE" && at_info.NodeType == (int)Atend.Control.Enum.ProductType.GroundCabel)
                        {
                            //ed.WriteMessage("15 \n");
                            DataRow dr = dt.NewRow();
                            dr["ConsolGuid"] = CurrentConsolInfo.NodeCode;
                            dr["PoleGuid"] = CurrentConsolInfo.ParentCode;
                            dr["BranchGuid"] = at_info.NodeCode;
                            dr["ConsolObjectId"] = Convert.ToInt32(_CurrentConsolOI.ToString().Substring(1, _CurrentConsolOI.ToString().Length - 2));
                            dr["BranchObjectId"] = Convert.ToInt32(oi.ToString().Substring(1, oi.ToString().Length - 2));

                            dt.Rows.Add(dr);


                            Atend.Base.Acad.AT_SUB ConductorSub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(oi);
                            foreach (ObjectId oii in ConductorSub.SubIdCollection)
                            {

                                Atend.Base.Acad.AT_INFO JumperInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oii);
                                if (JumperInfo.ParentCode != "NONE" && JumperInfo.NodeType == (int)Atend.Control.Enum.ProductType.Jumper)
                                {
                                    dr = dt.NewRow();
                                    dr["ConsolGuid"] = CurrentConsolInfo.NodeCode;
                                    dr["PoleGuid"] = CurrentConsolInfo.ParentCode;
                                    dr["BranchGuid"] = "JUMPER";
                                    dr["ConsolObjectId"] = Convert.ToInt32(_CurrentConsolOI.ToString().Substring(1, _CurrentConsolOI.ToString().Length - 2));
                                    dr["BranchObjectId"] = Convert.ToInt32(oii.ToString().Substring(1, oii.ToString().Length - 2));
                                    dt.Rows.Add(dr);
                                }

                            }

                        }

                    }

                }
                #endregion


                #region SelfKeeper
                if (_CurrentConsolOI != ObjectId.Null)
                {
                    Atend.Base.Acad.AT_INFO CurrentConsolInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(_CurrentConsolOI);
                    //ed.WriteMessage("11 \n");
                    Atend.Base.Acad.AT_SUB CurrentConsolSub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(_CurrentConsolOI);
                    // ed.WriteMessage("12 \n");
                    foreach (ObjectId oi in CurrentConsolSub.SubIdCollection)
                    {

                        //ed.WriteMessage("13 \n");
                        Atend.Base.Acad.AT_INFO at_info = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oi);
                        //ed.WriteMessage("14 \n");
                        if (at_info.ParentCode != "NONE" && at_info.NodeType == (int)Atend.Control.Enum.ProductType.SelfKeeper)
                        {
                            //ed.WriteMessage("15 \n");
                            DataRow dr = dt.NewRow();
                            dr["ConsolGuid"] = CurrentConsolInfo.NodeCode;
                            dr["PoleGuid"] = CurrentConsolInfo.ParentCode;
                            dr["BranchGuid"] = at_info.NodeCode;
                            dr["ConsolObjectId"] = Convert.ToInt32(_CurrentConsolOI.ToString().Substring(1, _CurrentConsolOI.ToString().Length - 2));
                            dr["BranchObjectId"] = Convert.ToInt32(oi.ToString().Substring(1, oi.ToString().Length - 2));

                            dt.Rows.Add(dr);

                            Atend.Base.Acad.AT_SUB ConductorSub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(oi);
                            foreach (ObjectId oii in ConductorSub.SubIdCollection)
                            {

                                Atend.Base.Acad.AT_INFO JumperInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oii);
                                if (JumperInfo.ParentCode != "NONE" && JumperInfo.NodeType == (int)Atend.Control.Enum.ProductType.Jumper)
                                {
                                    dr = dt.NewRow();
                                    dr["ConsolGuid"] = CurrentConsolInfo.NodeCode;
                                    dr["PoleGuid"] = CurrentConsolInfo.ParentCode;
                                    dr["BranchGuid"] = "JUMPER";
                                    dr["ConsolObjectId"] = Convert.ToInt32(_CurrentConsolOI.ToString().Substring(1, _CurrentConsolOI.ToString().Length - 2));
                                    dr["BranchObjectId"] = Convert.ToInt32(oii.ToString().Substring(1, oii.ToString().Length - 2));
                                    dt.Rows.Add(dr);
                                }

                            }

                        }

                    }

                }
                #endregion


                foreach (DataRow dr in dt.Rows)
                {

                    //ed.WriteMessage("\n~~~~START~~~~~~\n");
                    //ed.WriteMessage("\nConsolGuid : {0} , PoleGuid : {1} \n", dr["ConsolGuid"], dr["PoleGuid"]);
                    //ed.WriteMessage("\nBranchGuid : {0} , ConsolObjectId : {1} \n", dr["BranchGuid"], dr["ConsolObjectId"]);
                    //ed.WriteMessage("\nBranchObjectId : {0} \n", dr["BranchObjectid"]);
                    //ed.WriteMessage("\n~~~~END~~~~~~\n");
                }

            }
            return ds.Tables[0];

        }

        //2Electrical 02 With terminal
        public static System.Data.DataTable GetNodeInfoByObjectId02(int CurrentNodeOI)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            ObjectId _CurrentConsolOI = new ObjectId(new IntPtr(Convert.ToInt32(CurrentNodeOI)));
            System.Data.DataColumn col1 = new System.Data.DataColumn("ConsolGuid");
            System.Data.DataColumn col2 = new System.Data.DataColumn("PoleGuid");
            System.Data.DataColumn col3 = new System.Data.DataColumn("BranchGuid");
            //keep objectid as integer
            System.Data.DataColumn col4 = new System.Data.DataColumn("ConsolObjectId");
            System.Data.DataColumn col5 = new System.Data.DataColumn("BranchObjectId");
            System.Data.DataColumn col6 = new System.Data.DataColumn("BranchType");

            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Columns.Add(col1);
            dt.Columns.Add(col2);
            dt.Columns.Add(col3);
            dt.Columns.Add(col4);
            dt.Columns.Add(col5);
            dt.Columns.Add(col6);

            DataSet ds = new DataSet();
            ds.Tables.Add(dt);

            using (DocumentLock dlock = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.LockDocument())
            {

                if (_CurrentConsolOI != ObjectId.Null)
                {

                    {
                        Atend.Base.Acad.AT_INFO CurrentConsolInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(_CurrentConsolOI);
                        Atend.Base.Acad.AT_SUB CurrentConsolSub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(_CurrentConsolOI);

                        //#region Conductor
                        //if (_CurrentConsolOI != ObjectId.Null)
                        //{
                        //Atend.Base.Acad.AT_INFO CurrentConsolInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(_CurrentConsolOI);
                        //Atend.Base.Acad.AT_SUB CurrentConsolSub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(_CurrentConsolOI);
                        foreach (ObjectId oi in CurrentConsolSub.SubIdCollection)
                        {

                            Atend.Base.Acad.AT_INFO at_info = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oi);

                            #region conductor
                            if (at_info.ParentCode != "NONE" && at_info.NodeType == (int)Atend.Control.Enum.ProductType.Conductor)
                            {
                                DataRow dr = dt.NewRow();
                                dr["ConsolGuid"] = CurrentConsolInfo.NodeCode;
                                dr["PoleGuid"] = CurrentConsolInfo.ParentCode;
                                dr["BranchGuid"] = at_info.NodeCode;

                                dr["ConsolObjectId"] = Convert.ToInt32(_CurrentConsolOI.ToString().Substring(1, _CurrentConsolOI.ToString().Length - 2));
                                dr["BranchObjectId"] = Convert.ToInt32(oi.ToString().Substring(1, oi.ToString().Length - 2));
                                dr["BranchType"] = (int)Atend.Control.Enum.ProductType.Conductor;

                                dt.Rows.Add(dr);

                                Atend.Base.Acad.AT_SUB ConductorSub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(oi);
                                foreach (ObjectId oii in ConductorSub.SubIdCollection)
                                {

                                    Atend.Base.Acad.AT_INFO JumperInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oii);
                                    if (JumperInfo.ParentCode != "NONE" && JumperInfo.NodeType == (int)Atend.Control.Enum.ProductType.Jumper)
                                    {
                                        dr = dt.NewRow();
                                        dr["ConsolGuid"] = CurrentConsolInfo.NodeCode;
                                        dr["PoleGuid"] = CurrentConsolInfo.ParentCode;
                                        //dr["BranchGuid"] = "JUMPER";
                                        dr["BranchGuid"] = JumperInfo.NodeCode;

                                        dr["ConsolObjectId"] = Convert.ToInt32(_CurrentConsolOI.ToString().Substring(1, _CurrentConsolOI.ToString().Length - 2));
                                        dr["BranchObjectId"] = Convert.ToInt32(oii.ToString().Substring(1, oii.ToString().Length - 2));
                                        dr["BranchType"] = (int)Atend.Control.Enum.ProductType.Jumper;

                                        dt.Rows.Add(dr);
                                    }

                                }

                            }
                            #endregion

                            #region Breaker
                            if (at_info.ParentCode != "NONE" && at_info.NodeType == (int)Atend.Control.Enum.ProductType.Breaker)
                            {

                                Atend.Base.Design.DKeyStatus _DKeyStatus = Atend.Base.Design.DKeyStatus.SelectByNodeCode(new Guid(at_info.NodeCode));
                                if (_DKeyStatus.Code != Guid.Empty)
                                {
                                    //ed.WriteMessage("breaker found \n"); 
                                    if (_DKeyStatus.IsClosed)
                                    {
                                        //ed.WriteMessage("breaker was closed \n");
                                        DataRow dr = dt.NewRow();
                                        dr["ConsolGuid"] = CurrentConsolInfo.NodeCode;
                                        dr["PoleGuid"] = CurrentConsolInfo.ParentCode;
                                        dr["BranchGuid"] = at_info.NodeCode;

                                        dr["ConsolObjectId"] = Convert.ToInt32(_CurrentConsolOI.ToString().Substring(1, _CurrentConsolOI.ToString().Length - 2));
                                        dr["BranchObjectId"] = Convert.ToInt32(oi.ToString().Substring(1, oi.ToString().Length - 2));
                                        dr["BranchType"] = (int)Atend.Control.Enum.ProductType.Breaker;

                                        dt.Rows.Add(dr);
                                    }
                                }
                            }
                            #endregion

                            #region Disconnector
                            if (at_info.ParentCode != "NONE" && at_info.NodeType == (int)Atend.Control.Enum.ProductType.Disconnector)
                            {

                                Atend.Base.Design.DKeyStatus _DKeyStatus = Atend.Base.Design.DKeyStatus.SelectByNodeCode(new Guid(at_info.NodeCode));
                                if (_DKeyStatus.Code != Guid.Empty)
                                {
                                    //ed.WriteMessage("breaker found \n");
                                    if (_DKeyStatus.IsClosed)
                                    {
                                        //ed.WriteMessage("breaker was closed \n");
                                        DataRow dr = dt.NewRow();
                                        dr["ConsolGuid"] = CurrentConsolInfo.NodeCode;
                                        dr["PoleGuid"] = CurrentConsolInfo.ParentCode;
                                        dr["BranchGuid"] = at_info.NodeCode;

                                        dr["ConsolObjectId"] = Convert.ToInt32(_CurrentConsolOI.ToString().Substring(1, _CurrentConsolOI.ToString().Length - 2));
                                        dr["BranchObjectId"] = Convert.ToInt32(oi.ToString().Substring(1, oi.ToString().Length - 2));
                                        dr["BranchType"] = (int)Atend.Control.Enum.ProductType.Disconnector;

                                        dt.Rows.Add(dr);
                                    }
                                }
                            }
                            #endregion

                            #region Catout
                            if (at_info.ParentCode != "NONE" && at_info.NodeType == (int)Atend.Control.Enum.ProductType.CatOut)
                            {

                                Atend.Base.Design.DKeyStatus _DKeyStatus = Atend.Base.Design.DKeyStatus.SelectByNodeCode(new Guid(at_info.NodeCode));
                                if (_DKeyStatus.Code != Guid.Empty)
                                {
                                    //ed.WriteMessage("breaker found \n");
                                    if (_DKeyStatus.IsClosed)
                                    {
                                        //ed.WriteMessage("breaker was closed \n");
                                        DataRow dr = dt.NewRow();
                                        dr["ConsolGuid"] = CurrentConsolInfo.NodeCode;
                                        dr["PoleGuid"] = CurrentConsolInfo.ParentCode;
                                        dr["BranchGuid"] = at_info.NodeCode;

                                        dr["ConsolObjectId"] = Convert.ToInt32(_CurrentConsolOI.ToString().Substring(1, _CurrentConsolOI.ToString().Length - 2));
                                        dr["BranchObjectId"] = Convert.ToInt32(oi.ToString().Substring(1, oi.ToString().Length - 2));
                                        dr["BranchType"] = (int)Atend.Control.Enum.ProductType.CatOut;

                                        dt.Rows.Add(dr);
                                    }
                                }
                            }
                            #endregion

                            #region Cable
                            if (at_info.ParentCode != "NONE" && at_info.NodeType == (int)Atend.Control.Enum.ProductType.GroundCabel)
                            {
                                DataRow dr = dt.NewRow();
                                dr["ConsolGuid"] = CurrentConsolInfo.NodeCode;
                                dr["PoleGuid"] = CurrentConsolInfo.ParentCode;
                                ed.WriteMessage("***************BranchGuid in Cabel ={0}\n", at_info.NodeCode.ToString());
                                dr["BranchGuid"] = at_info.NodeCode;

                                dr["ConsolObjectId"] = Convert.ToInt32(_CurrentConsolOI.ToString().Substring(1, _CurrentConsolOI.ToString().Length - 2));
                                dr["BranchObjectId"] = Convert.ToInt32(oi.ToString().Substring(1, oi.ToString().Length - 2));
                                dr["BranchType"] = (int)Atend.Control.Enum.ProductType.GroundCabel;

                                dt.Rows.Add(dr);


                                Atend.Base.Acad.AT_SUB ConductorSub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(oi);
                                foreach (ObjectId oii in ConductorSub.SubIdCollection)
                                {
                                    Atend.Base.Acad.AT_INFO JumperInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oii);
                                    if (JumperInfo.ParentCode != "NONE" && JumperInfo.NodeType == (int)Atend.Control.Enum.ProductType.Jumper)
                                    {
                                        dr = dt.NewRow();
                                        dr["ConsolGuid"] = CurrentConsolInfo.NodeCode;
                                        dr["PoleGuid"] = CurrentConsolInfo.ParentCode;
                                        //dr["BranchGuid"] = "JUMPER";
                                        dr["BranchGuid"] = JumperInfo.NodeCode;

                                        dr["ConsolObjectId"] = Convert.ToInt32(_CurrentConsolOI.ToString().Substring(1, _CurrentConsolOI.ToString().Length - 2));
                                        dr["BranchObjectId"] = Convert.ToInt32(oii.ToString().Substring(1, oii.ToString().Length - 2));
                                        dr["BranchType"] = (int)Atend.Control.Enum.ProductType.Jumper;
                                        dt.Rows.Add(dr);
                                    }

                                }
                            }
                            #endregion

                            #region SElfkeeper
                            if (at_info.ParentCode != "NONE" && at_info.NodeType == (int)Atend.Control.Enum.ProductType.SelfKeeper)
                            {
                                //ed.WriteMessage("15 \n");
                                DataRow dr = dt.NewRow();
                                dr["ConsolGuid"] = CurrentConsolInfo.NodeCode;
                                dr["PoleGuid"] = CurrentConsolInfo.ParentCode;
                                dr["BranchGuid"] = at_info.NodeCode;

                                dr["ConsolObjectId"] = Convert.ToInt32(_CurrentConsolOI.ToString().Substring(1, _CurrentConsolOI.ToString().Length - 2));
                                dr["BranchObjectId"] = Convert.ToInt32(oi.ToString().Substring(1, oi.ToString().Length - 2));
                                dr["BranchType"] = (int)Atend.Control.Enum.ProductType.SelfKeeper;

                                dt.Rows.Add(dr);

                                Atend.Base.Acad.AT_SUB ConductorSub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(oi);
                                foreach (ObjectId oii in ConductorSub.SubIdCollection)
                                {

                                    Atend.Base.Acad.AT_INFO JumperInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oii);
                                    if (JumperInfo.ParentCode != "NONE" && JumperInfo.NodeType == (int)Atend.Control.Enum.ProductType.Jumper)
                                    {
                                        dr = dt.NewRow();
                                        dr["ConsolGuid"] = CurrentConsolInfo.NodeCode;
                                        dr["PoleGuid"] = CurrentConsolInfo.ParentCode;
                                        //dr["BranchGuid"] = "JUMPER";
                                        dr["BranchGuid"] = JumperInfo.NodeCode;

                                        dr["ConsolObjectId"] = Convert.ToInt32(_CurrentConsolOI.ToString().Substring(1, _CurrentConsolOI.ToString().Length - 2));
                                        dr["BranchObjectId"] = Convert.ToInt32(oii.ToString().Substring(1, oii.ToString().Length - 2));
                                        dr["BranchType"] = (int)Atend.Control.Enum.ProductType.Jumper;

                                        dt.Rows.Add(dr);
                                    }

                                }

                            }//
                            #endregion

                            #region Terminal
                            if (at_info.ParentCode != "NONE" && at_info.NodeType == (int)Atend.Control.Enum.ProductType.Terminal)
                            {
                                DataRow dr = dt.NewRow();
                                dr["ConsolGuid"] = CurrentConsolInfo.NodeCode;
                                dr["PoleGuid"] = CurrentConsolInfo.ParentCode;
                                dr["BranchGuid"] = at_info.NodeCode;

                                dr["ConsolObjectId"] = Convert.ToInt32(_CurrentConsolOI.ToString().Substring(1, _CurrentConsolOI.ToString().Length - 2));
                                dr["BranchObjectId"] = Convert.ToInt32(oi.ToString().Substring(1, oi.ToString().Length - 2));
                                dr["BranchType"] = (int)Atend.Control.Enum.ProductType.Terminal;

                                dt.Rows.Add(dr);

                                Atend.Base.Acad.AT_SUB ConductorSub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(oi);
                                foreach (ObjectId oii in ConductorSub.SubIdCollection)
                                {

                                    Atend.Base.Acad.AT_INFO JumperInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oii);
                                    if (JumperInfo.ParentCode != "NONE" && JumperInfo.NodeType == (int)Atend.Control.Enum.ProductType.Jumper)
                                    {
                                        dr = dt.NewRow();
                                        dr["ConsolGuid"] = CurrentConsolInfo.NodeCode;
                                        dr["PoleGuid"] = CurrentConsolInfo.ParentCode;
                                        //dr["BranchGuid"] = "JUMPER";
                                        dr["BranchGuid"] = JumperInfo.NodeCode;

                                        dr["ConsolObjectId"] = Convert.ToInt32(_CurrentConsolOI.ToString().Substring(1, _CurrentConsolOI.ToString().Length - 2));
                                        dr["BranchObjectId"] = Convert.ToInt32(oii.ToString().Substring(1, oii.ToString().Length - 2));
                                        dr["BranchType"] = (int)Atend.Control.Enum.ProductType.Jumper;

                                        dt.Rows.Add(dr);
                                    }

                                }

                            }//
                            #endregion
                        }

                        //}
                        //#endregion


                        //#region Breaker
                        //if (_CurrentConsolOI != ObjectId.Null)
                        //{
                        //Atend.Base.Acad.AT_INFO CurrentConsolInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(_CurrentConsolOI);
                        //Atend.Base.Acad.AT_SUB CurrentConsolSub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(_CurrentConsolOI);
                        //foreach (ObjectId oi in CurrentConsolSub.SubIdCollection)
                        //{

                        //    Atend.Base.Acad.AT_INFO at_info = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oi);
                        //    if (at_info.ParentCode != "NONE" && at_info.NodeType == (int)Atend.Control.Enum.ProductType.Breaker)
                        //    {

                        //        Atend.Base.Design.DKeyStatus _DKeyStatus = Atend.Base.Design.DKeyStatus.SelectByNodeCode(new Guid(at_info.NodeCode));
                        //        if (_DKeyStatus.Code != Guid.Empty)
                        //        {
                        //            //ed.WriteMessage("breaker found \n"); 
                        //            if (_DKeyStatus.IsClosed)
                        //            {
                        //                //ed.WriteMessage("breaker was closed \n");
                        //                DataRow dr = dt.NewRow();
                        //                dr["ConsolGuid"] = CurrentConsolInfo.NodeCode;
                        //                dr["PoleGuid"] = CurrentConsolInfo.ParentCode;
                        //                dr["BranchGuid"] = at_info.NodeCode;

                        //                dr["ConsolObjectId"] = Convert.ToInt32(_CurrentConsolOI.ToString().Substring(1, _CurrentConsolOI.ToString().Length - 2));
                        //                dr["BranchObjectId"] = Convert.ToInt32(oi.ToString().Substring(1, oi.ToString().Length - 2));
                        //                dr["BranchType"] = (int)Atend.Control.Enum.ProductType.Breaker;

                        //                dt.Rows.Add(dr);
                        //            }
                        //        }
                        //    }

                        //}

                        //}
                        //#endregion


                        //#region Disconnector
                        ////if (_CurrentConsolOI != ObjectId.Null)
                        ////{
                        ////Atend.Base.Acad.AT_INFO CurrentConsolInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(_CurrentConsolOI);
                        ////Atend.Base.Acad.AT_SUB CurrentConsolSub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(_CurrentConsolOI);
                        //foreach (ObjectId oi in CurrentConsolSub.SubIdCollection)
                        //{

                        //    Atend.Base.Acad.AT_INFO at_info = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oi);
                        //    if (at_info.ParentCode != "NONE" && at_info.NodeType == (int)Atend.Control.Enum.ProductType.Disconnector)
                        //    {

                        //        Atend.Base.Design.DKeyStatus _DKeyStatus = Atend.Base.Design.DKeyStatus.SelectByNodeCode(new Guid(at_info.NodeCode));
                        //        if (_DKeyStatus.Code != Guid.Empty)
                        //        {
                        //            //ed.WriteMessage("breaker found \n");
                        //            if (_DKeyStatus.IsClosed)
                        //            {
                        //                //ed.WriteMessage("breaker was closed \n");
                        //                DataRow dr = dt.NewRow();
                        //                dr["ConsolGuid"] = CurrentConsolInfo.NodeCode;
                        //                dr["PoleGuid"] = CurrentConsolInfo.ParentCode;
                        //                dr["BranchGuid"] = at_info.NodeCode;

                        //                dr["ConsolObjectId"] = Convert.ToInt32(_CurrentConsolOI.ToString().Substring(1, _CurrentConsolOI.ToString().Length - 2));
                        //                dr["BranchObjectId"] = Convert.ToInt32(oi.ToString().Substring(1, oi.ToString().Length - 2));
                        //                dr["BranchType"] = (int)Atend.Control.Enum.ProductType.Disconnector;

                        //                dt.Rows.Add(dr);
                        //            }
                        //        }
                        //    }

                        //}

                        ////}
                        //#endregion


                        // #region Catout
                        //if (_CurrentConsolOI != ObjectId.Null)
                        //{
                        //Atend.Base.Acad.AT_INFO CurrentConsolInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(_CurrentConsolOI);
                        //Atend.Base.Acad.AT_SUB CurrentConsolSub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(_CurrentConsolOI);
                        //foreach (ObjectId oi in CurrentConsolSub.SubIdCollection)
                        //{

                        //    Atend.Base.Acad.AT_INFO at_info = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oi);
                        //    if (at_info.ParentCode != "NONE" && at_info.NodeType == (int)Atend.Control.Enum.ProductType.CatOut)
                        //    {

                        //        Atend.Base.Design.DKeyStatus _DKeyStatus = Atend.Base.Design.DKeyStatus.SelectByNodeCode(new Guid(at_info.NodeCode));
                        //        if (_DKeyStatus.Code != Guid.Empty)
                        //        {
                        //            //ed.WriteMessage("breaker found \n");
                        //            if (_DKeyStatus.IsClosed)
                        //            {
                        //                //ed.WriteMessage("breaker was closed \n");
                        //                DataRow dr = dt.NewRow();
                        //                dr["ConsolGuid"] = CurrentConsolInfo.NodeCode;
                        //                dr["PoleGuid"] = CurrentConsolInfo.ParentCode;
                        //                dr["BranchGuid"] = at_info.NodeCode;

                        //                dr["ConsolObjectId"] = Convert.ToInt32(_CurrentConsolOI.ToString().Substring(1, _CurrentConsolOI.ToString().Length - 2));
                        //                dr["BranchObjectId"] = Convert.ToInt32(oi.ToString().Substring(1, oi.ToString().Length - 2));
                        //                dr["BranchType"] = (int)Atend.Control.Enum.ProductType.CatOut;

                        //                dt.Rows.Add(dr);
                        //            }
                        //        }
                        //    }

                        //}

                        //}
                        // #endregion


                        //#region Cable
                        ////if (_CurrentConsolOI != ObjectId.Null)
                        ////{
                        ////Atend.Base.Acad.AT_INFO CurrentConsolInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(_CurrentConsolOI);
                        ////Atend.Base.Acad.AT_SUB CurrentConsolSub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(_CurrentConsolOI);
                        //foreach (ObjectId oi in CurrentConsolSub.SubIdCollection)
                        //{

                        //    Atend.Base.Acad.AT_INFO at_info = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oi);
                        //    if (at_info.ParentCode != "NONE" && at_info.NodeType == (int)Atend.Control.Enum.ProductType.GroundCabel)
                        //    {
                        //        DataRow dr = dt.NewRow();
                        //        dr["ConsolGuid"] = CurrentConsolInfo.NodeCode;
                        //        dr["PoleGuid"] = CurrentConsolInfo.ParentCode;
                        //        dr["BranchGuid"] = at_info.NodeCode;

                        //        dr["ConsolObjectId"] = Convert.ToInt32(_CurrentConsolOI.ToString().Substring(1, _CurrentConsolOI.ToString().Length - 2));
                        //        dr["BranchObjectId"] = Convert.ToInt32(oi.ToString().Substring(1, oi.ToString().Length - 2));
                        //        dr["BranchType"] = (int)Atend.Control.Enum.ProductType.GroundCabel;

                        //        dt.Rows.Add(dr);


                        //        Atend.Base.Acad.AT_SUB ConductorSub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(oi);
                        //        foreach (ObjectId oii in ConductorSub.SubIdCollection)
                        //        {
                        //            Atend.Base.Acad.AT_INFO JumperInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oii);
                        //            if (JumperInfo.ParentCode != "NONE" && JumperInfo.NodeType == (int)Atend.Control.Enum.ProductType.Jumper)
                        //            {
                        //                dr = dt.NewRow();
                        //                dr["ConsolGuid"] = CurrentConsolInfo.NodeCode;
                        //                dr["PoleGuid"] = CurrentConsolInfo.ParentCode;
                        //                dr["BranchGuid"] = "JUMPER";

                        //                dr["ConsolObjectId"] = Convert.ToInt32(_CurrentConsolOI.ToString().Substring(1, _CurrentConsolOI.ToString().Length - 2));
                        //                dr["BranchObjectId"] = Convert.ToInt32(oii.ToString().Substring(1, oii.ToString().Length - 2));
                        //                dr["BranchType"] = (int)Atend.Control.Enum.ProductType.GroundCabel;
                        //                dt.Rows.Add(dr);
                        //            }

                        //        }
                        //    }

                        //}

                        ////}
                        //#endregion


                        //#region SelfKeeper
                        ////if (_CurrentConsolOI != ObjectId.Null)
                        ////{
                        ////Atend.Base.Acad.AT_INFO CurrentConsolInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(_CurrentConsolOI);
                        ////Atend.Base.Acad.AT_SUB CurrentConsolSub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(_CurrentConsolOI);
                        //foreach (ObjectId oi in CurrentConsolSub.SubIdCollection)
                        //{

                        //    //ed.WriteMessage("13 \n");
                        //    Atend.Base.Acad.AT_INFO at_info = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oi);
                        //    //ed.WriteMessage("14 \n");
                        //    if (at_info.ParentCode != "NONE" && at_info.NodeType == (int)Atend.Control.Enum.ProductType.SelfKeeper)
                        //    {
                        //        //ed.WriteMessage("15 \n");
                        //        DataRow dr = dt.NewRow();
                        //        dr["ConsolGuid"] = CurrentConsolInfo.NodeCode;
                        //        dr["PoleGuid"] = CurrentConsolInfo.ParentCode;
                        //        dr["BranchGuid"] = at_info.NodeCode;

                        //        dr["ConsolObjectId"] = Convert.ToInt32(_CurrentConsolOI.ToString().Substring(1, _CurrentConsolOI.ToString().Length - 2));
                        //        dr["BranchObjectId"] = Convert.ToInt32(oi.ToString().Substring(1, oi.ToString().Length - 2));
                        //        dr["BranchType"] = (int)Atend.Control.Enum.ProductType.SelfKeeper;

                        //        dt.Rows.Add(dr);

                        //        Atend.Base.Acad.AT_SUB ConductorSub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(oi);
                        //        foreach (ObjectId oii in ConductorSub.SubIdCollection)
                        //        {

                        //            Atend.Base.Acad.AT_INFO JumperInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oii);
                        //            if (JumperInfo.ParentCode != "NONE" && JumperInfo.NodeType == (int)Atend.Control.Enum.ProductType.Jumper)
                        //            {
                        //                dr = dt.NewRow();
                        //                dr["ConsolGuid"] = CurrentConsolInfo.NodeCode;
                        //                dr["PoleGuid"] = CurrentConsolInfo.ParentCode;
                        //                dr["BranchGuid"] = "JUMPER";

                        //                dr["ConsolObjectId"] = Convert.ToInt32(_CurrentConsolOI.ToString().Substring(1, _CurrentConsolOI.ToString().Length - 2));
                        //                dr["BranchObjectId"] = Convert.ToInt32(oii.ToString().Substring(1, oii.ToString().Length - 2));
                        //                dr["BranchType"] = (int)Atend.Control.Enum.ProductType.SelfKeeper;

                        //                dt.Rows.Add(dr);
                        //            }

                        //        }

                        //    }//

                        //}

                        ////}
                        //#endregion


                        //#region Terminal
                        ////if (_CurrentConsolOI != ObjectId.Null)
                        ////{
                        ////Atend.Base.Acad.AT_INFO CurrentConsolInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(_CurrentConsolOI);
                        ////Atend.Base.Acad.AT_SUB CurrentConsolSub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(_CurrentConsolOI);
                        //foreach (ObjectId oi in CurrentConsolSub.SubIdCollection)
                        //{

                        //    Atend.Base.Acad.AT_INFO at_info = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oi);
                        //    if (at_info.ParentCode != "NONE" && at_info.NodeType == (int)Atend.Control.Enum.ProductType.Terminal)
                        //    {
                        //        DataRow dr = dt.NewRow();
                        //        dr["ConsolGuid"] = CurrentConsolInfo.NodeCode;
                        //        dr["PoleGuid"] = CurrentConsolInfo.ParentCode;
                        //        dr["BranchGuid"] = at_info.NodeCode;

                        //        dr["ConsolObjectId"] = Convert.ToInt32(_CurrentConsolOI.ToString().Substring(1, _CurrentConsolOI.ToString().Length - 2));
                        //        dr["BranchObjectId"] = Convert.ToInt32(oi.ToString().Substring(1, oi.ToString().Length - 2));
                        //        dr["BranchType"] = (int)Atend.Control.Enum.ProductType.Terminal;

                        //        dt.Rows.Add(dr);

                        //        Atend.Base.Acad.AT_SUB ConductorSub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(oi);
                        //        foreach (ObjectId oii in ConductorSub.SubIdCollection)
                        //        {

                        //            Atend.Base.Acad.AT_INFO JumperInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oii);
                        //            if (JumperInfo.ParentCode != "NONE" && JumperInfo.NodeType == (int)Atend.Control.Enum.ProductType.Jumper)
                        //            {
                        //                dr = dt.NewRow();
                        //                dr["ConsolGuid"] = CurrentConsolInfo.NodeCode;
                        //                dr["PoleGuid"] = CurrentConsolInfo.ParentCode;
                        //                dr["BranchGuid"] = "JUMPER";

                        //                dr["ConsolObjectId"] = Convert.ToInt32(_CurrentConsolOI.ToString().Substring(1, _CurrentConsolOI.ToString().Length - 2));
                        //                dr["BranchObjectId"] = Convert.ToInt32(oii.ToString().Substring(1, oii.ToString().Length - 2));
                        //                dr["BranchType"] = (int)Atend.Control.Enum.ProductType.Terminal;

                        //                dt.Rows.Add(dr);
                        //            }

                        //        }

                        //    }//

                        //}

                        ////}
                        //#endregion

                    }


                    #region MiddleJackPanelCell
                    System.Data.DataTable MiddleJackCells = null;
                    Dictionary<ObjectId, Atend.Base.Acad.AT_INFO> ALL_AT_INFO = new Dictionary<ObjectId, Atend.Base.Acad.AT_INFO>();
                    //Dictionary<ObjectId, Atend.Base.Acad.AT_SUB> ALL_AT_SUB = new Dictionary<ObjectId, Atend.Base.Acad.AT_SUB>();

                    ObjectId GOI = Atend.Global.Acad.UAcad.GetEntityGroup(_CurrentConsolOI);
                    if (GOI != ObjectId.Null)
                    {
                        //ed.WriteMessage("group found \n");
                        //now get middlejackpanel info
                        Atend.Base.Acad.AT_INFO MJPInfo = null;
                        if (ALL_AT_INFO.ContainsKey(GOI))
                        {
                            ALL_AT_INFO.TryGetValue(GOI, out MJPInfo);
                        }
                        else
                        {
                            MJPInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(GOI);
                            ALL_AT_INFO.Add(GOI, MJPInfo);
                        }
                        if (MJPInfo != null)
                        {
                            if (MJPInfo.ParentCode != "NONE" && MJPInfo.NodeType == (int)Atend.Control.Enum.ProductType.MiddleJackPanel)
                            {
                                MiddleJackCells = Atend.Base.Design.DCellStatus.AccessSelectByJackPanelCode(new Guid(MJPInfo.NodeCode));
                                System.Data.DataColumn c1 = new System.Data.DataColumn("HeaderOI");
                                System.Data.DataColumn c2 = new System.Data.DataColumn("IsEnterance");
                                System.Data.DataColumn c3 = new System.Data.DataColumn("Order");
                                System.Data.DataColumn c4 = new System.Data.DataColumn("IsVisited");
                                MiddleJackCells.Columns.Add(c1);
                                MiddleJackCells.Columns.Add(c2);
                                MiddleJackCells.Columns.Add(c3);
                                MiddleJackCells.Columns.Add(c4);
                                if (MiddleJackCells.Rows.Count > 0)
                                {
                                    //bring all sub of middle jackpanel entities
                                    ObjectIdCollection OIC = Atend.Global.Acad.UAcad.GetGroupSubEntities(GOI);
                                    int CellCounter = 1;
                                    foreach (ObjectId oi in OIC)
                                    {
                                        Atend.Base.Acad.AT_INFO CellInfo = null;
                                        if (ALL_AT_INFO.ContainsKey(oi))
                                        {
                                            ALL_AT_INFO.TryGetValue(oi, out CellInfo);
                                        }
                                        else
                                        {
                                            CellInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oi);
                                            ALL_AT_INFO.Add(oi, CellInfo);
                                        }
                                        if (CellInfo != null)
                                        {
                                            if (CellInfo.ParentCode != "NONE" && CellInfo.NodeType == (int)Atend.Control.Enum.ProductType.Cell)
                                            {
                                                System.Data.DataRow[] mycell = MiddleJackCells.Select(string.Format("CellCode = '{0}'", CellInfo.NodeCode));
                                                if (mycell.Length > 0)
                                                {
                                                    ObjectId hoi = ObjectId.Null;
                                                    mycell[0]["Order"] = CellCounter;
                                                    mycell[0]["IsVisited"] = 0;
                                                    CellCounter++;
                                                    Atend.Base.Acad.AT_SUB CellSub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(oi);
                                                    foreach (ObjectId oii in CellSub.SubIdCollection)
                                                    {
                                                        Atend.Base.Acad.AT_INFO hinfo = null;
                                                        if (ALL_AT_INFO.ContainsKey(oii))
                                                        {
                                                            ALL_AT_INFO.TryGetValue(oii, out hinfo);
                                                        }
                                                        else
                                                        {
                                                            hinfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oii);
                                                            ALL_AT_INFO.Add(oii, hinfo);
                                                        }
                                                        if (hinfo != null)
                                                        {
                                                            if (hinfo.ParentCode != "NONE" && hinfo.NodeType == (int)Atend.Control.Enum.ProductType.HeaderCabel)
                                                            {
                                                                hoi = oii;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            //اطلاعات سرکابل یافت نشد
                                                        }
                                                    }
                                                    if (hoi != ObjectId.Null)
                                                    {
                                                        mycell[0]["HeaderOI"] = hoi.ToString().Substring(1, hoi.ToString().Length - 2);
                                                        if (hoi == _CurrentConsolOI)
                                                        {
                                                            mycell[0]["IsEnterance"] = 1;
                                                        }
                                                        else
                                                        {
                                                            mycell[0]["IsEnterance"] = 0;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        mycell[0]["HeaderOI"] = -1;
                                                        mycell[0]["IsEnterance"] = 0;
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            //اطلاعات سلول یافت نشد
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            //اطلاعات گروه تابلو فشار متوسط یافت نشد
                        }
                    }//


                    //Atend.Global.frmTest _frmTest = new frmTest();
                    //_frmTest.dataGridView1.DataSource = MiddleJackCells;
                    //_frmTest.ShowDialog();

                    if (MiddleJackCells != null)
                    {
                        //ed.WriteMessage("MiddleJackCells found \n");
                        int OrderIntrance = -1;
                        DataRow[] drs = MiddleJackCells.Select(string.Format("IsEnterance = {0} ", 1));
                        if (drs.Length > 0)
                        {
                            OrderIntrance = Convert.ToInt32(drs[0]["Order"]);
                        }
                        //ed.WriteMessage("ORDER : {0} \n", OrderIntrance);
                        if (OrderIntrance != -1)
                        {
                            //ed.WriteMessage("ORDER : {0} \n", OrderIntrance);
                            for (int counter = OrderIntrance; counter < MiddleJackCells.Rows.Count; counter++)
                            {
                                int r = GetNextMiddleCell(MiddleJackCells, OrderIntrance, false);
                                //ed.WriteMessage("Intrance : {0} ans:{1} FALSE\n", OrderIntrance, r);
                                if (r != -1)
                                {
                                    Atend.Base.Acad.AT_INFO CurrentConsolInfo = null;
                                    if (ALL_AT_INFO.ContainsKey(_CurrentConsolOI))
                                    {
                                        ALL_AT_INFO.TryGetValue(_CurrentConsolOI, out CurrentConsolInfo);
                                    }
                                    else
                                    {
                                        CurrentConsolInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(_CurrentConsolOI);
                                        ALL_AT_INFO.Add(_CurrentConsolOI, CurrentConsolInfo);
                                    }
                                    if (CurrentConsolInfo != null)
                                    {
                                        DataRow dr = dt.NewRow();
                                        dr["ConsolGuid"] = CurrentConsolInfo.NodeCode;
                                        dr["PoleGuid"] = CurrentConsolInfo.ParentCode;
                                        dr["BranchGuid"] = "BUS";

                                        dr["ConsolObjectId"] = Convert.ToInt32(_CurrentConsolOI.ToString().Substring(1, _CurrentConsolOI.ToString().Length - 2));
                                        dr["BranchObjectId"] = -1;
                                        dr["BranchType"] = (int)Atend.Control.Enum.ProductType.Bus;
                                        dt.Rows.Add(dr);
                                    }
                                    //ed.WriteMessage("Intrance : {0} ans:{1} FALSE\n", OrderIntrance, r);
                                }
                                //ed.WriteMessage("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\n");
                            }

                            //ed.WriteMessage("SSSSSSSSSSSS \n");
                            for (int counter = OrderIntrance; counter > 1; counter--)
                            {
                                int r = GetNextMiddleCell(MiddleJackCells, OrderIntrance, true);

                                if (r != -1)
                                {
                                    Atend.Base.Acad.AT_INFO CurrentConsolInfo = null;
                                    if (ALL_AT_INFO.ContainsKey(_CurrentConsolOI))
                                    {
                                        ALL_AT_INFO.TryGetValue(_CurrentConsolOI, out CurrentConsolInfo);
                                    }
                                    else
                                    {
                                        CurrentConsolInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(_CurrentConsolOI);
                                        ALL_AT_INFO.Add(_CurrentConsolOI, CurrentConsolInfo);
                                    }
                                    if (CurrentConsolInfo != null)
                                    {
                                        DataRow dr = dt.NewRow();
                                        dr["ConsolGuid"] = CurrentConsolInfo.NodeCode;
                                        dr["PoleGuid"] = CurrentConsolInfo.ParentCode;
                                        dr["BranchGuid"] = "BUS";

                                        dr["ConsolObjectId"] = Convert.ToInt32(_CurrentConsolOI.ToString().Substring(1, _CurrentConsolOI.ToString().Length - 2));
                                        dr["BranchObjectId"] = -1;
                                        dr["BranchType"] = (int)Atend.Control.Enum.ProductType.Bus;
                                        dt.Rows.Add(dr);

                                    }
                                    //ed.WriteMessage("Intrance : {0} ans:{1} TRUE\n", OrderIntrance, r);
                                }
                                //ed.WriteMessage("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\n");
                            }


                        }
                    }
                    #endregion

                    //frmTest _frmTest = new frmTest();
                    //_frmTest.dataGridView1.DataSource = dt;
                    //_frmTest.ShowDialog();


                    //Atend2Electrical.ElecTree t = new Atend2Electrical.ElecTree();
                    //Atend.Global.frmTest ft = new frmTest();
                    //ft.dataGridView1.DataSource = t.dt
                    //ft.ShowDialog();

                    //////////ed.WriteMessage("\n~~~~START~~~~~~\n");
                    //////////foreach (DataRow dr in dt.Rows)
                    //////////{
                    //////////    ed.WriteMessage("\nConsolGuid : {0} , PoleGuid : {1} \n", dr["ConsolGuid"], dr["PoleGuid"]);
                    //////////    ed.WriteMessage("\nBranchGuid : {0} , ConsolObjectId : {1} \n", dr["BranchGuid"], dr["ConsolObjectId"]);
                    //////////    ed.WriteMessage("\nBranchObjectId : {0} \n", dr["BranchObjectid"]);
                    //////////}
                    //////////ed.WriteMessage("\n~~~~END~~~~~~\n");
                }
            }
            return ds.Tables[0];

        }

        private static int GetNextMiddleCell(System.Data.DataTable MiddleJackpanelCells, int CurrentOrder, bool UpDirection)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            int CellOrder = -1;
            DataRow[] currentCell = MiddleJackpanelCells.Select(string.Format("Order = {0}", CurrentOrder));
            DataRow[] OtherCell = null;
            int OtherCellCounter = 0;
            ed.WriteMessage("START : CurrentOrder:{0} \n", CurrentOrder);
            if (UpDirection)
            {
                OtherCellCounter = CurrentOrder - 1;
                OtherCell = MiddleJackpanelCells.Select(string.Format("Order = {0}", OtherCellCounter));
                if (currentCell.Length > 0)
                {
                    if (Convert.ToInt32(currentCell[0]["HeaderOI"]) != -1 && Convert.ToBoolean(currentCell[0]["IsClosed"]) != true)
                    {
                        CellOrder = -1;
                    }

                    bool conti = true;
                    while (conti)
                    {
                        if (OtherCell.Length > 0)
                        {
                            ed.WriteMessage("badi darad \n");
                            if (Convert.ToInt32(OtherCell[0]["IsVisited"]) == 1)
                            {
                                ed.WriteMessage("cell {0} visited \n", OtherCellCounter);
                                OtherCellCounter--;
                                OtherCell = MiddleJackpanelCells.Select(string.Format("Order = {0}", OtherCellCounter));
                            }
                            else
                            {
                                ed.WriteMessage("cell {0} not visited \n", OtherCellCounter);
                                conti = false;
                            }

                            if (conti == false)
                            {
                                if (Convert.ToInt32(OtherCell[0]["HeaderOI"]) != -1)
                                {
                                    ed.WriteMessage("header darad \n");
                                    if (Convert.ToBoolean(OtherCell[0]["IsClosed"]) == true)
                                    {
                                        ed.WriteMessage("header cloed \n");
                                        OtherCell[0]["IsVisited"] = 1;
                                        CellOrder = Convert.ToInt32(OtherCell[0]["Order"]);
                                    }
                                    else
                                    {
                                        ed.WriteMessage("header not cloed \n");
                                        OtherCell[0]["IsVisited"] = 1;
                                        CellOrder = -1;
                                    }
                                }
                                else
                                {
                                    ed.WriteMessage("header nadarad \n");
                                    if (Convert.ToBoolean(OtherCell[0]["IsClosed"]) == true)
                                    {
                                        ed.WriteMessage("header cloed \n");
                                        CellOrder = GetNextMiddleCell(MiddleJackpanelCells, Convert.ToInt32(OtherCell[0]["Order"]), UpDirection);
                                    }
                                    else
                                    {
                                        ed.WriteMessage("header not cloed \n");
                                        CellOrder = -1;
                                    }

                                }
                            }
                        }
                        else
                        {
                            conti = false;
                            CellOrder = -1;
                        }
                    }
                }
            }
            else
            {
                OtherCellCounter = CurrentOrder + 1;
                OtherCell = MiddleJackpanelCells.Select(string.Format("Order = {0}", OtherCellCounter));
                if (currentCell.Length > 0)
                {
                    if (Convert.ToInt32(currentCell[0]["HeaderOI"]) != -1 && Convert.ToBoolean(currentCell[0]["IsClosed"]) != true)
                    {
                        CellOrder = -1;
                    }

                    bool conti = true;
                    while (conti)
                    {
                        if (OtherCell.Length > 0)
                        {
                            //ed.WriteMessage("cell badi darad \n");
                            if (Convert.ToInt32(OtherCell[0]["IsVisited"]) == 1)
                            {
                                //ed.WriteMessage("cell {0} visited \n", OtherCellCounter);
                                //System.Windows.Forms.MessageBox.Show("1");
                                OtherCellCounter++;
                                OtherCell = MiddleJackpanelCells.Select(string.Format("Order = {0}", OtherCellCounter));
                            }
                            else
                            {
                                //ed.WriteMessage("cell {0} not visited \n", OtherCellCounter);
                                //System.Windows.Forms.MessageBox.Show("2");
                                conti = false;
                            }

                            if (conti == false)
                            {
                                if (Convert.ToInt32(OtherCell[0]["HeaderOI"]) != -1)
                                {
                                    //ed.WriteMessage("Header darad \n");
                                    //System.Windows.Forms.MessageBox.Show("3");
                                    if (Convert.ToBoolean(OtherCell[0]["IsClosed"]) == true)
                                    {
                                        //ed.WriteMessage("Header closed \n");
                                        //System.Windows.Forms.MessageBox.Show("4");
                                        OtherCell[0]["IsVisited"] = 1;
                                        CellOrder = Convert.ToInt32(OtherCell[0]["Order"]);
                                    }
                                    else
                                    {
                                        //ed.WriteMessage("Header not closed \n");
                                        //System.Windows.Forms.MessageBox.Show("5");
                                        OtherCell[0]["IsVisited"] = 1;
                                        CellOrder = -1;
                                    }
                                }
                                else
                                {
                                    //ed.WriteMessage("Header nadarad \n");
                                    //System.Windows.Forms.MessageBox.Show("6");
                                    if (Convert.ToBoolean(OtherCell[0]["IsClosed"]) == true)
                                    {
                                        //ed.WriteMessage("Header closed \n");
                                        //System.Windows.Forms.MessageBox.Show("7");
                                        CellOrder = GetNextMiddleCell(MiddleJackpanelCells, Convert.ToInt32(OtherCell[0]["Order"]), UpDirection);
                                    }
                                    else
                                    {
                                        //ed.WriteMessage("Header not cloed \n");
                                        //System.Windows.Forms.MessageBox.Show("8");
                                        CellOrder = -1;
                                    }

                                }
                            }
                        }
                        else
                        {
                            conti = false;
                            CellOrder = -1;
                        }
                    }
                }
            }//down
            return CellOrder;
        }

        //2Electrical
        public static Guid GetNextNodeBranch(int CurrentConsolOI, int CurrentBranchOI)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            ObjectId _CurrentConsolOI = new ObjectId(new IntPtr(Convert.ToInt32(CurrentConsolOI)));
            ObjectId _CurrentBranchOI = new ObjectId(new IntPtr(Convert.ToInt32(CurrentBranchOI)));
            Int32 NextConsol = 0;
            Guid BranchGuid = Guid.Empty;

            //ed.WriteMessage("** -- ** COI:{0} , BOI:{1} \n", _CurrentConsolOI, _CurrentBranchOI);
            using (DocumentLock dlock = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.LockDocument())
            {
                if (_CurrentConsolOI != null && _CurrentBranchOI != null)
                {
                    Atend.Base.Acad.AT_INFO BranchInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(_CurrentBranchOI);
                    if (BranchInfo.ParentCode != "NONE")
                    {
                        if (BranchInfo.NodeType == (int)Atend.Control.Enum.ProductType.Jumper)
                        {
                            #region Branch was jumper
                            ObjectId NextPoleOI = ObjectId.Null;
                            ObjectId NCOI = ObjectId.Null;
                            Atend.Base.Acad.AT_SUB JumperSub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(_CurrentBranchOI);
                            foreach (ObjectId oi in JumperSub.SubIdCollection)
                            {
                                bool CanGetBranchGuid = true;
                                Atend.Base.Acad.AT_SUB ConductorSub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(oi);
                                foreach (ObjectId oii in ConductorSub.SubIdCollection)
                                {
                                    if (oii == _CurrentConsolOI)
                                    {
                                        CanGetBranchGuid = false;
                                    }

                                }
                                if (CanGetBranchGuid)
                                    BranchGuid = new Guid(Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oi).NodeCode);
                            }
                            #endregion
                        }

                    }
                }
            }

            //ed.writeMessage("NEXT CONSOL:{0}\n", NextConsol);
            return BranchGuid;

        }

        //2Electrical
        public static int GetNextNode(int CurrentNodeOI, int CurrentBranchOI)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            ObjectId _CurrentConsolOI = new ObjectId(new IntPtr(Convert.ToInt32(CurrentNodeOI)));
            ObjectId _CurrentBranchOI = new ObjectId(new IntPtr(Convert.ToInt32(CurrentBranchOI)));
            Int32 NextConsol = 0;

            using (DocumentLock dlock = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.LockDocument())
            {
                if (_CurrentConsolOI != null && _CurrentBranchOI != null)
                {
                    Atend.Base.Acad.AT_INFO BranchInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(_CurrentBranchOI);
                    if (BranchInfo.ParentCode != "NONE")
                    {

                        if (BranchInfo.NodeType == (int)Atend.Control.Enum.ProductType.Jumper)
                        {
                            #region Branch was jumper
                            ObjectId NextPoleOI = ObjectId.Null;
                            ObjectId NCOI = ObjectId.Null;
                            Atend.Base.Acad.AT_SUB JumperSub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(_CurrentBranchOI);
                            foreach (ObjectId oi in JumperSub.SubIdCollection)
                            {
                                Atend.Base.Acad.AT_SUB ConductorSub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(oi);
                                foreach (ObjectId oii in ConductorSub.SubIdCollection)
                                {
                                    if (oii == _CurrentConsolOI)
                                    {

                                        NCOI = new ObjectId(new IntPtr(GetNextNode(Convert.ToInt32(_CurrentConsolOI.ToString().Substring(1, _CurrentConsolOI.ToString().Length - 2)), Convert.ToInt32(oi.ToString().Substring(1, oi.ToString().Length - 2)))));
                                        if (NCOI != null)
                                        {
                                            Atend.Base.Acad.AT_SUB NextConsolSub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(NCOI);
                                            foreach (ObjectId oiii in NextConsolSub.SubIdCollection)
                                            {
                                                Atend.Base.Acad.AT_INFO NextConsolSubInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oiii);
                                                if (NextConsolSubInfo.ParentCode != "NONE" && NextConsolSubInfo.NodeType == (int)Atend.Control.Enum.ProductType.Pole)
                                                {
                                                    NextPoleOI = oiii;
                                                }
                                            }
                                        }

                                    }

                                }
                            }
                            //
                            ObjectIdCollection Consols = new ObjectIdCollection();
                            if (NextPoleOI != null && NCOI != null)
                            {

                                Atend.Base.Acad.AT_SUB JumperSub1 = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(_CurrentBranchOI);
                                foreach (ObjectId oi in JumperSub1.SubIdCollection)
                                {
                                    Atend.Base.Acad.AT_INFO JumperSubInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oi);
                                    if (JumperSubInfo.ParentCode != "NONE" && JumperSubInfo.NodeType == (int)Atend.Control.Enum.ProductType.Conductor)
                                    {
                                        Atend.Base.Acad.AT_SUB ConductorSub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(oi);
                                        foreach (ObjectId oii in ConductorSub.SubIdCollection)
                                        {
                                            Atend.Base.Acad.AT_INFO ConsolInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oii);
                                            if (ConsolInfo.ParentCode != "NONE" && (ConsolInfo.NodeType == (int)Atend.Control.Enum.ProductType.Consol || ConsolInfo.NodeType == (int)Atend.Control.Enum.ProductType.HeaderCabel || ConsolInfo.NodeType == (int)Atend.Control.Enum.ProductType.Kalamp))
                                            {
                                                Consols.Add(oii);
                                            }
                                        }
                                    }
                                    else if (JumperSubInfo.ParentCode != "NONE" && JumperSubInfo.NodeType == (int)Atend.Control.Enum.ProductType.SelfKeeper)
                                    {
                                        Atend.Base.Acad.AT_SUB ConductorSub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(oi);
                                        foreach (ObjectId oii in ConductorSub.SubIdCollection)
                                        {
                                            Atend.Base.Acad.AT_INFO ConsolInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oii);
                                            if (ConsolInfo.ParentCode != "NONE" && (ConsolInfo.NodeType == (int)Atend.Control.Enum.ProductType.Consol || ConsolInfo.NodeType == (int)Atend.Control.Enum.ProductType.HeaderCabel || ConsolInfo.NodeType == (int)Atend.Control.Enum.ProductType.Kalamp))
                                            {
                                                Consols.Add(oii);
                                            }
                                        }
                                    }
                                    else if (JumperSubInfo.ParentCode != "NONE" && JumperSubInfo.NodeType == (int)Atend.Control.Enum.ProductType.GroundCabel)
                                    {
                                        Atend.Base.Acad.AT_SUB ConductorSub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(oi);
                                        foreach (ObjectId oii in ConductorSub.SubIdCollection)
                                        {
                                            Atend.Base.Acad.AT_INFO ConsolInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oii);
                                            if (ConsolInfo.ParentCode != "NONE" && (ConsolInfo.NodeType == (int)Atend.Control.Enum.ProductType.Consol || ConsolInfo.NodeType == (int)Atend.Control.Enum.ProductType.HeaderCabel || ConsolInfo.NodeType == (int)Atend.Control.Enum.ProductType.Kalamp))
                                            {
                                                Consols.Add(oii);
                                            }
                                        }
                                    }

                                }

                                foreach (ObjectId oi in Consols)
                                {
                                    if (oi != NCOI)
                                    {
                                        //Atend.Base.Acad.AT_SUB ConsolSub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(oi);
                                        //foreach (ObjectId oii in ConsolSub.SubIdCollection)
                                        //{
                                        //Atend.Base.Acad.AT_INFO ConsolSubInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oii);
                                        //if (ConsolSubInfo.ParentCode != "NONE" && ConsolSubInfo.NodeType == (int)Atend.Control.Enum.ProductType.Pole)
                                        //{
                                        //if (oii == NextPoleOI)
                                        //{
                                        NextConsol = Convert.ToInt32(oi.ToString().Substring(1, oi.ToString().Length - 2));
                                        //}
                                        // }
                                        //}
                                    }
                                }

                            }
                            #endregion
                        }

                        if (BranchInfo.NodeType == (int)Atend.Control.Enum.ProductType.Conductor)
                        {
                            #region Branch was Conductor

                            Atend.Base.Acad.AT_SUB ConductorSub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(_CurrentBranchOI);
                            foreach (ObjectId oi in ConductorSub.SubIdCollection)
                            {
                                if (oi != _CurrentConsolOI)
                                {
                                    Atend.Base.Acad.AT_INFO ConductorSubInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oi);
                                    if (ConductorSubInfo.ParentCode != "NONE" && ConductorSubInfo.NodeType == (int)Atend.Control.Enum.ProductType.Consol)
                                    {

                                        NextConsol = Convert.ToInt32(oi.ToString().Substring(1, oi.ToString().Length - 2));
                                    }
                                }
                            }

                            #endregion
                        }

                        if (BranchInfo.NodeType == (int)Atend.Control.Enum.ProductType.SelfKeeper)
                        {
                            #region Branch was SelfKeeper

                            Atend.Base.Acad.AT_SUB ConductorSub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(_CurrentBranchOI);
                            foreach (ObjectId oi in ConductorSub.SubIdCollection)
                            {
                                if (oi != _CurrentConsolOI)
                                {
                                    Atend.Base.Acad.AT_INFO ConductorSubInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oi);
                                    if (ConductorSubInfo.ParentCode != "NONE" && ConductorSubInfo.NodeType == (int)Atend.Control.Enum.ProductType.HeaderCabel)
                                    {

                                        NextConsol = Convert.ToInt32(oi.ToString().Substring(1, oi.ToString().Length - 2));
                                    }
                                    else if (ConductorSubInfo.ParentCode != "NONE" && ConductorSubInfo.NodeType == (int)Atend.Control.Enum.ProductType.Kalamp)
                                    {

                                        NextConsol = Convert.ToInt32(oi.ToString().Substring(1, oi.ToString().Length - 2));
                                    }
                                    else if (ConductorSubInfo.ParentCode != "NONE" && ConductorSubInfo.NodeType == (int)Atend.Control.Enum.ProductType.KablSho)
                                    {

                                        NextConsol = Convert.ToInt32(oi.ToString().Substring(1, oi.ToString().Length - 2));
                                    }
                                }
                            }

                            #endregion
                        }

                        if (BranchInfo.NodeType == (int)Atend.Control.Enum.ProductType.GroundCabel)
                        {
                            #region Branch was MiddleCabel

                            Atend.Base.Acad.AT_SUB ConductorSub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(_CurrentBranchOI);
                            foreach (ObjectId oi in ConductorSub.SubIdCollection)
                            {
                                if (oi != _CurrentConsolOI)
                                {
                                    Atend.Base.Acad.AT_INFO ConductorSubInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oi);
                                    if (ConductorSubInfo.ParentCode != "NONE" && ConductorSubInfo.NodeType == (int)Atend.Control.Enum.ProductType.HeaderCabel)
                                    {

                                        NextConsol = Convert.ToInt32(oi.ToString().Substring(1, oi.ToString().Length - 2));
                                    }
                                    else if (ConductorSubInfo.ParentCode != "NONE" && ConductorSubInfo.NodeType == (int)Atend.Control.Enum.ProductType.Kalamp)
                                    {

                                        NextConsol = Convert.ToInt32(oi.ToString().Substring(1, oi.ToString().Length - 2));
                                    }
                                    else if (ConductorSubInfo.ParentCode != "NONE" && ConductorSubInfo.NodeType == (int)Atend.Control.Enum.ProductType.KablSho)
                                    {

                                        NextConsol = Convert.ToInt32(oi.ToString().Substring(1, oi.ToString().Length - 2));
                                    }
                                }
                            }

                            #endregion
                        }

                    }
                }
            }

            //ed.writeMessage("NEXT CONSOL:{0}\n", NextConsol);
            return NextConsol;

        }

        //2Electrical 02 with terminal
        public static int GetNextNode02(int CurrentNodeOI, int CurrentBranchOI)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            ed.WriteMessage("  *** NOI:{0} , BOI:{1} \n", CurrentNodeOI, CurrentBranchOI);
            ObjectId _CurrentConsolOI = new ObjectId(new IntPtr(Convert.ToInt32(CurrentNodeOI)));
            ObjectId _CurrentBranchOI = new ObjectId(new IntPtr(Convert.ToInt32(CurrentBranchOI)));
            Int32 NextConsol = 0;

            using (DocumentLock dlock = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.LockDocument())
            {
                if (_CurrentConsolOI != null && _CurrentBranchOI != null)
                {
                    Atend.Base.Acad.AT_INFO BranchInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(_CurrentBranchOI);
                    if (BranchInfo.ParentCode != "NONE")
                    {


                        if (BranchInfo.NodeType == (int)Atend.Control.Enum.ProductType.Jumper)
                        {



                            #region Branch was jumper new
                            ObjectId NextPoleOI = ObjectId.Null;
                            ObjectId NCOI = ObjectId.Null;
                            ObjectId CurrentPoleOI = ObjectId.Null;

                            Atend.Base.Acad.AT_SUB CurrentNodeSub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(_CurrentConsolOI);
                            foreach (ObjectId oi in CurrentNodeSub.SubIdCollection)
                            {
                                Atend.Base.Acad.AT_INFO NodeInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oi);
                                if (NodeInfo.ParentCode != "NONE" && (NodeInfo.NodeType == (int)Atend.Control.Enum.ProductType.Pole || NodeInfo.NodeType == (int)Atend.Control.Enum.ProductType.Pole))
                                {
                                    CurrentPoleOI = oi;
                                }
                            }


                            ////////Atend.Base.Acad.AT_SUB JumperSub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(_CurrentBranchOI);
                            ////////foreach (ObjectId oi in JumperSub.SubIdCollection)
                            ////////{
                            ////////    Atend.Base.Acad.AT_SUB ConductorSub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(oi);
                            ////////    foreach (ObjectId oii in ConductorSub.SubIdCollection)
                            ////////    {






                            ////////        //if (oii == _CurrentConsolOI)
                            ////////        //{

                            ////////        //    NCOI = new ObjectId(GetNextNode(Convert.ToInt32(_CurrentConsolOI.ToString().Substring(1, _CurrentConsolOI.ToString().Length - 2)), Convert.ToInt32(oi.ToString().Substring(1, oi.ToString().Length - 2))));
                            ////////        //    if (NCOI != null)
                            ////////        //    {
                            ////////        //        Atend.Base.Acad.AT_SUB NextConsolSub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(NCOI);
                            ////////        //        foreach (ObjectId oiii in NextConsolSub.SubIdCollection)
                            ////////        //        {
                            ////////        //            Atend.Base.Acad.AT_INFO NextConsolSubInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oiii);
                            ////////        //            if (NextConsolSubInfo.ParentCode != "NONE" && (NextConsolSubInfo.NodeType == (int)Atend.Control.Enum.ProductType.Pole || NextConsolSubInfo.NodeType == (int)Atend.Control.Enum.ProductType.PoleTip))
                            ////////        //            {
                            ////////        //                NextPoleOI = oiii;
                            ////////        //            }
                            ////////        //        }
                            ////////        //    }

                            ////////        //}

                            ////////    }
                            ////////}
                            //
                            ObjectIdCollection Consols = new ObjectIdCollection();
                            //if (NextPoleOI != null && NCOI != null)
                            //{

                            Atend.Base.Acad.AT_SUB JumperSub1 = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(_CurrentBranchOI);
                            foreach (ObjectId oi in JumperSub1.SubIdCollection)
                            {
                                //Atend.Base.Acad.AT_INFO JumperSubInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oi);
                                //if (JumperSubInfo.ParentCode != "NONE" && JumperSubInfo.NodeType == (int)Atend.Control.Enum.ProductType.Conductor)
                                //{
                                Atend.Base.Acad.AT_SUB ConductorSub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(oi);
                                foreach (ObjectId oii in ConductorSub.SubIdCollection)
                                {
                                    Atend.Base.Acad.AT_INFO ConsolInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oii);
                                    if (ConsolInfo.ParentCode != "NONE" && (ConsolInfo.NodeType == (int)Atend.Control.Enum.ProductType.Consol || ConsolInfo.NodeType == (int)Atend.Control.Enum.ProductType.HeaderCabel || ConsolInfo.NodeType == (int)Atend.Control.Enum.ProductType.Kalamp))
                                    {
                                        //Consols.Add(oii);
                                        //file pole here
                                        if (oii != _CurrentConsolOI)
                                        {
                                            Atend.Base.Acad.AT_SUB CurrentNodeSub1 = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(oii);
                                            foreach (ObjectId oi1 in CurrentNodeSub1.SubIdCollection)
                                            {

                                                Atend.Base.Acad.AT_INFO NodeInfo1 = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oi1);
                                                if (NodeInfo1.ParentCode != "NONE" && (NodeInfo1.NodeType == (int)Atend.Control.Enum.ProductType.Pole || NodeInfo1.NodeType == (int)Atend.Control.Enum.ProductType.Pole))
                                                {
                                                    if (oi1 == CurrentPoleOI)
                                                    {
                                                        NextConsol = Convert.ToInt32(oii.ToString().Substring(1, oii.ToString().Length - 2));
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                //}
                                ////else if (JumperSubInfo.ParentCode != "NONE" && JumperSubInfo.NodeType == (int)Atend.Control.Enum.ProductType.SelfKeeper)
                                ////{
                                ////    Atend.Base.Acad.AT_SUB ConductorSub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(oi);
                                ////    foreach (ObjectId oii in ConductorSub.SubIdCollection)
                                ////    {
                                ////        Atend.Base.Acad.AT_INFO ConsolInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oii);
                                ////        if (ConsolInfo.ParentCode != "NONE" && (ConsolInfo.NodeType == (int)Atend.Control.Enum.ProductType.Consol || ConsolInfo.NodeType == (int)Atend.Control.Enum.ProductType.HeaderCabel || ConsolInfo.NodeType == (int)Atend.Control.Enum.ProductType.Kalamp))
                                ////        {
                                ////            Consols.Add(oii);
                                ////        }
                                ////    }
                                ////}
                                ////else if (JumperSubInfo.ParentCode != "NONE" && JumperSubInfo.NodeType == (int)Atend.Control.Enum.ProductType.GroundCabel)
                                ////{
                                ////    Atend.Base.Acad.AT_SUB ConductorSub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(oi);
                                ////    foreach (ObjectId oii in ConductorSub.SubIdCollection)
                                ////    {
                                ////        Atend.Base.Acad.AT_INFO ConsolInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oii);
                                ////        if (ConsolInfo.ParentCode != "NONE" && (ConsolInfo.NodeType == (int)Atend.Control.Enum.ProductType.Consol || ConsolInfo.NodeType == (int)Atend.Control.Enum.ProductType.HeaderCabel || ConsolInfo.NodeType == (int)Atend.Control.Enum.ProductType.Kalamp))
                                ////        {
                                ////            Consols.Add(oii);
                                ////        }
                                ////    }
                                ////}

                            }

                            //////////foreach (ObjectId oi in Consols)
                            //////////{
                            //////////    ed.WriteMessage("Oi={0},NCOI={1}\n", oi.ToString(), NCOI.ToString());
                            //////////    if (oi != NCOI)
                            //////////    {
                            //////////        //Atend.Base.Acad.AT_SUB ConsolSub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(oi);
                            //////////        //foreach (ObjectId oii in ConsolSub.SubIdCollection)
                            //////////        //{
                            //////////        //Atend.Base.Acad.AT_INFO ConsolSubInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oii);
                            //////////        //if (ConsolSubInfo.ParentCode != "NONE" && ConsolSubInfo.NodeType == (int)Atend.Control.Enum.ProductType.Pole)
                            //////////        //{
                            //////////        //if (oii == NextPoleOI)
                            //////////        //{
                            //////////        NextConsol = Convert.ToInt32(oi.ToString().Substring(1, oi.ToString().Length - 2));
                            //////////        //}
                            //////////        // }
                            //////////        //}
                            //////////    }
                            //////////}

                            //}
                            #endregion




                            //////#region Branch was jumper
                            //////ObjectId NextPoleOI = ObjectId.Null;
                            //////ObjectId NCOI = ObjectId.Null;
                            //////Atend.Base.Acad.AT_SUB JumperSub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(_CurrentBranchOI);
                            //////foreach (ObjectId oi in JumperSub.SubIdCollection)
                            //////{
                            //////    Atend.Base.Acad.AT_SUB ConductorSub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(oi);
                            //////    foreach (ObjectId oii in ConductorSub.SubIdCollection)
                            //////    {
                            //////        if (oii == _CurrentConsolOI)
                            //////        {

                            //////            NCOI = new ObjectId(GetNextNode(Convert.ToInt32(_CurrentConsolOI.ToString().Substring(1, _CurrentConsolOI.ToString().Length - 2)), Convert.ToInt32(oi.ToString().Substring(1, oi.ToString().Length - 2))));
                            //////            if (NCOI != null)
                            //////            {
                            //////                Atend.Base.Acad.AT_SUB NextConsolSub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(NCOI);
                            //////                foreach (ObjectId oiii in NextConsolSub.SubIdCollection)
                            //////                {
                            //////                    Atend.Base.Acad.AT_INFO NextConsolSubInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oiii);
                            //////                    if (NextConsolSubInfo.ParentCode != "NONE" && (NextConsolSubInfo.NodeType == (int)Atend.Control.Enum.ProductType.Pole || NextConsolSubInfo.NodeType == (int)Atend.Control.Enum.ProductType.PoleTip))
                            //////                    {
                            //////                        NextPoleOI = oiii;
                            //////                    }
                            //////                }
                            //////            }

                            //////        }

                            //////    }
                            //////}
                            ////////
                            //////ObjectIdCollection Consols = new ObjectIdCollection();
                            //////if (NextPoleOI != null && NCOI != null)
                            //////{

                            //////    Atend.Base.Acad.AT_SUB JumperSub1 = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(_CurrentBranchOI);
                            //////    foreach (ObjectId oi in JumperSub1.SubIdCollection)
                            //////    {
                            //////        Atend.Base.Acad.AT_INFO JumperSubInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oi);
                            //////        if (JumperSubInfo.ParentCode != "NONE" && JumperSubInfo.NodeType == (int)Atend.Control.Enum.ProductType.Conductor)
                            //////        {
                            //////            Atend.Base.Acad.AT_SUB ConductorSub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(oi);
                            //////            foreach (ObjectId oii in ConductorSub.SubIdCollection)
                            //////            {
                            //////                Atend.Base.Acad.AT_INFO ConsolInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oii);
                            //////                if (ConsolInfo.ParentCode != "NONE" && (ConsolInfo.NodeType == (int)Atend.Control.Enum.ProductType.Consol || ConsolInfo.NodeType == (int)Atend.Control.Enum.ProductType.HeaderCabel || ConsolInfo.NodeType == (int)Atend.Control.Enum.ProductType.Kalamp))
                            //////                {
                            //////                    Consols.Add(oii);
                            //////                }
                            //////            }
                            //////        }
                            //////        else if (JumperSubInfo.ParentCode != "NONE" && JumperSubInfo.NodeType == (int)Atend.Control.Enum.ProductType.SelfKeeper)
                            //////        {
                            //////            Atend.Base.Acad.AT_SUB ConductorSub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(oi);
                            //////            foreach (ObjectId oii in ConductorSub.SubIdCollection)
                            //////            {
                            //////                Atend.Base.Acad.AT_INFO ConsolInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oii);
                            //////                if (ConsolInfo.ParentCode != "NONE" && (ConsolInfo.NodeType == (int)Atend.Control.Enum.ProductType.Consol || ConsolInfo.NodeType == (int)Atend.Control.Enum.ProductType.HeaderCabel || ConsolInfo.NodeType == (int)Atend.Control.Enum.ProductType.Kalamp))
                            //////                {
                            //////                    Consols.Add(oii);
                            //////                }
                            //////            }
                            //////        }
                            //////        else if (JumperSubInfo.ParentCode != "NONE" && JumperSubInfo.NodeType == (int)Atend.Control.Enum.ProductType.GroundCabel)
                            //////        {
                            //////            Atend.Base.Acad.AT_SUB ConductorSub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(oi);
                            //////            foreach (ObjectId oii in ConductorSub.SubIdCollection)
                            //////            {
                            //////                Atend.Base.Acad.AT_INFO ConsolInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oii);
                            //////                if (ConsolInfo.ParentCode != "NONE" && (ConsolInfo.NodeType == (int)Atend.Control.Enum.ProductType.Consol || ConsolInfo.NodeType == (int)Atend.Control.Enum.ProductType.HeaderCabel || ConsolInfo.NodeType == (int)Atend.Control.Enum.ProductType.Kalamp))
                            //////                {
                            //////                    Consols.Add(oii);
                            //////                }
                            //////            }
                            //////        }

                            //////    }

                            //////    foreach (ObjectId oi in Consols)
                            //////    {
                            //////        ed.WriteMessage("Oi={0},NCOI={1}\n",oi.ToString(),NCOI.ToString());
                            //////        if (oi != NCOI)
                            //////        {
                            //////            //Atend.Base.Acad.AT_SUB ConsolSub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(oi);
                            //////            //foreach (ObjectId oii in ConsolSub.SubIdCollection)
                            //////            //{
                            //////            //Atend.Base.Acad.AT_INFO ConsolSubInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oii);
                            //////            //if (ConsolSubInfo.ParentCode != "NONE" && ConsolSubInfo.NodeType == (int)Atend.Control.Enum.ProductType.Pole)
                            //////            //{
                            //////            //if (oii == NextPoleOI)
                            //////            //{
                            //////            NextConsol = Convert.ToInt32(oi.ToString().Substring(1, oi.ToString().Length - 2));
                            //////            //}
                            //////            // }
                            //////            //}
                            //////        }
                            //////    }

                            //////}
                            //////#endregion
                        }

                        if (BranchInfo.NodeType == (int)Atend.Control.Enum.ProductType.Conductor)
                        {
                            #region Branch was Conductor

                            Atend.Base.Acad.AT_SUB ConductorSub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(_CurrentBranchOI);
                            foreach (ObjectId oi in ConductorSub.SubIdCollection)
                            {
                                if (oi != _CurrentConsolOI)
                                {
                                    Atend.Base.Acad.AT_INFO ConductorSubInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oi);
                                    if (ConductorSubInfo.ParentCode != "NONE" && ConductorSubInfo.NodeType == (int)Atend.Control.Enum.ProductType.Consol)
                                    {

                                        NextConsol = Convert.ToInt32(oi.ToString().Substring(1, oi.ToString().Length - 2));
                                    }
                                }
                            }

                            #endregion
                        }

                        if (BranchInfo.NodeType == (int)Atend.Control.Enum.ProductType.SelfKeeper)
                        {
                            #region Branch was SelfKeeper

                            Atend.Base.Acad.AT_SUB ConductorSub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(_CurrentBranchOI);
                            foreach (ObjectId oi in ConductorSub.SubIdCollection)
                            {
                                if (oi != _CurrentConsolOI)
                                {
                                    Atend.Base.Acad.AT_INFO ConductorSubInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oi);
                                    if (ConductorSubInfo.ParentCode != "NONE" && ConductorSubInfo.NodeType == (int)Atend.Control.Enum.ProductType.HeaderCabel)
                                    {

                                        NextConsol = Convert.ToInt32(oi.ToString().Substring(1, oi.ToString().Length - 2));
                                    }
                                    else if (ConductorSubInfo.ParentCode != "NONE" && ConductorSubInfo.NodeType == (int)Atend.Control.Enum.ProductType.Kalamp)
                                    {

                                        NextConsol = Convert.ToInt32(oi.ToString().Substring(1, oi.ToString().Length - 2));
                                    }
                                    else if (ConductorSubInfo.ParentCode != "NONE" && ConductorSubInfo.NodeType == (int)Atend.Control.Enum.ProductType.KablSho)
                                    {

                                        NextConsol = Convert.ToInt32(oi.ToString().Substring(1, oi.ToString().Length - 2));
                                    }
                                }
                            }

                            #endregion
                        }

                        if (BranchInfo.NodeType == (int)Atend.Control.Enum.ProductType.GroundCabel)
                        {
                            #region Branch was MiddleCabel

                            Atend.Base.Acad.AT_SUB ConductorSub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(_CurrentBranchOI);
                            foreach (ObjectId oi in ConductorSub.SubIdCollection)
                            {
                                if (oi != _CurrentConsolOI)
                                {
                                    Atend.Base.Acad.AT_INFO ConductorSubInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oi);
                                    if (ConductorSubInfo.ParentCode != "NONE" && ConductorSubInfo.NodeType == (int)Atend.Control.Enum.ProductType.HeaderCabel)
                                    {

                                        NextConsol = Convert.ToInt32(oi.ToString().Substring(1, oi.ToString().Length - 2));
                                    }
                                    else if (ConductorSubInfo.ParentCode != "NONE" && ConductorSubInfo.NodeType == (int)Atend.Control.Enum.ProductType.Kalamp)
                                    {

                                        NextConsol = Convert.ToInt32(oi.ToString().Substring(1, oi.ToString().Length - 2));
                                    }
                                    else if (ConductorSubInfo.ParentCode != "NONE" && ConductorSubInfo.NodeType == (int)Atend.Control.Enum.ProductType.KablSho)
                                    {

                                        NextConsol = Convert.ToInt32(oi.ToString().Substring(1, oi.ToString().Length - 2));
                                    }
                                }
                            }

                            #endregion
                        }

                        if (BranchInfo.NodeType == (int)Atend.Control.Enum.ProductType.Terminal)
                        {
                            #region Branch was Terminal
                            System.Data.DataTable Parents = Atend.Global.Acad.UAcad.DetermineParent((int)Atend.Control.Enum.ProductType.Terminal);

                            Atend.Base.Acad.AT_SUB ConductorSub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(_CurrentBranchOI);
                            foreach (ObjectId oi in ConductorSub.SubIdCollection)
                            {
                                if (oi != _CurrentConsolOI)
                                {
                                    Atend.Base.Acad.AT_INFO ConductorSubInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oi);
                                    if (ConductorSubInfo.ParentCode != "NONE")
                                    {
                                        DataRow[] drs = Parents.Select(string.Format("SoftwareCode={0}", ConductorSubInfo.NodeType));
                                        if (drs.Length != 0)
                                        {
                                            NextConsol = Convert.ToInt32(oi.ToString().Substring(1, oi.ToString().Length - 2));
                                        }
                                        //&& ConductorSubInfo.NodeType == (int)Atend.Control.Enum.ProductType.Consol
                                    }
                                }
                            }

                            #endregion
                        }

                        if (BranchInfo.NodeType == (int)Atend.Control.Enum.ProductType.Breaker)
                        {
                            #region Branch was Breaker
                            System.Data.DataTable Parents = Atend.Global.Acad.UAcad.DetermineParent((int)Atend.Control.Enum.ProductType.Breaker);

                            Atend.Base.Acad.AT_SUB ConductorSub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(_CurrentBranchOI);
                            foreach (ObjectId oi in ConductorSub.SubIdCollection)
                            {
                                if (oi != _CurrentConsolOI)
                                {
                                    Atend.Base.Acad.AT_INFO ConductorSubInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oi);
                                    if (ConductorSubInfo.ParentCode != "NONE")
                                    {
                                        DataRow[] drs = Parents.Select(string.Format("SoftwareCode={0}", ConductorSubInfo.NodeType));
                                        if (drs.Length != 0)
                                        {
                                            NextConsol = Convert.ToInt32(oi.ToString().Substring(1, oi.ToString().Length - 2));
                                        }
                                        //&& ConductorSubInfo.NodeType == (int)Atend.Control.Enum.ProductType.Consol
                                    }
                                }
                            }

                            #endregion
                        }

                        if (BranchInfo.NodeType == (int)Atend.Control.Enum.ProductType.Disconnector)
                        {
                            #region Branch was Disconnector
                            System.Data.DataTable Parents = Atend.Global.Acad.UAcad.DetermineParent((int)Atend.Control.Enum.ProductType.Disconnector);

                            Atend.Base.Acad.AT_SUB ConductorSub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(_CurrentBranchOI);
                            foreach (ObjectId oi in ConductorSub.SubIdCollection)
                            {
                                if (oi != _CurrentConsolOI)
                                {
                                    Atend.Base.Acad.AT_INFO ConductorSubInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oi);
                                    if (ConductorSubInfo.ParentCode != "NONE")
                                    {
                                        DataRow[] drs = Parents.Select(string.Format("SoftwareCode={0}", ConductorSubInfo.NodeType));
                                        if (drs.Length != 0)
                                        {
                                            NextConsol = Convert.ToInt32(oi.ToString().Substring(1, oi.ToString().Length - 2));
                                        }
                                        //&& ConductorSubInfo.NodeType == (int)Atend.Control.Enum.ProductType.Consol
                                    }
                                }
                            }

                            #endregion
                        }

                        if (BranchInfo.NodeType == (int)Atend.Control.Enum.ProductType.CatOut)
                        {
                            #region Branch was Catout
                            System.Data.DataTable Parents = Atend.Global.Acad.UAcad.DetermineParent((int)Atend.Control.Enum.ProductType.CatOut);

                            Atend.Base.Acad.AT_SUB ConductorSub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(_CurrentBranchOI);
                            foreach (ObjectId oi in ConductorSub.SubIdCollection)
                            {
                                if (oi != _CurrentConsolOI)
                                {
                                    Atend.Base.Acad.AT_INFO ConductorSubInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oi);
                                    if (ConductorSubInfo.ParentCode != "NONE")
                                    {
                                        DataRow[] drs = Parents.Select(string.Format("SoftwareCode={0}", ConductorSubInfo.NodeType));
                                        if (drs.Length != 0)
                                        {
                                            NextConsol = Convert.ToInt32(oi.ToString().Substring(1, oi.ToString().Length - 2));
                                        }
                                        //&& ConductorSubInfo.NodeType == (int)Atend.Control.Enum.ProductType.Consol
                                    }
                                }
                            }

                            #endregion
                        }


                    }
                }
            }

            //ed.writeMessage("NEXT CONSOL:{0}\n", NextConsol);
            return NextConsol;

        }

        //HATAMI
        public static ObjectId GetBranchByGuid(Guid CurrentBranch)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            ObjectId CurrentPoleOi = ObjectId.Null;

            ed.WriteMessage("I am in GetBranchByGuid \n");

            Database db = Application.DocumentManager.MdiActiveDocument.Database;

            using (DocumentLock dlock = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.LockDocument())
            {

                using (Transaction tr = db.TransactionManager.StartTransaction())
                {


                    BlockTable bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead);

                    BlockTableRecord btr = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForRead);

                    //DBObjectCollection AllObjects = tr.GetAllObjects();
                    //ed.WriteMessage("ALL OBJECTS COUNT {0} \n",AllObjects.Count);

                    //ed.WriteMessage("Current pole {0}",CurrentPole);
                    //ed.WriteMessage("1 \n");
                    foreach (ObjectId oi in btr)
                    {
                        //ed.WriteMessage("2 \n");
                        Atend.Base.Acad.AT_INFO at_info = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oi);

                        //ed.WriteMessage("3 parentcod {0} : productType {1} : nodeGuid {2}  \n", at_info.ParentCode , at_info.NodeType , at_info.NodeCode);
                        if (at_info.ParentCode != "NONE" && (at_info.NodeType == (int)Atend.Control.Enum.ProductType.Conductor || at_info.NodeType == (int)Atend.Control.Enum.ProductType.SelfKeeper || at_info.NodeType == (int)Atend.Control.Enum.ProductType.GroundCabel) && at_info.NodeCode == CurrentBranch.ToString())
                        {
                            CurrentPoleOi = oi;
                            ed.WriteMessage("Current Branch OI : {0} \n", CurrentPoleOi);
                            return CurrentPoleOi;
                        }

                    }

                }

                //ed.WriteMessage("I am not in GetPoleByOI {0} \n", CurrentPoleOi);
            }
            return CurrentPoleOi;

        }





        public static bool Convertor()
        {

            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            OleDbTransaction OldTransaction = null;
            OleDbConnection OldConnection = new OleDbConnection(Atend.Control.ConnectionString.AccessCnString);

            OleDbTransaction NewTransaction = null;
            OleDbConnection NewConnection = new OleDbConnection(Atend.Control.ConnectionString.NewAccessconnecion);

            Atend.Control.Common.dtConvertor = new System.Data.DataTable();
            Atend.Control.Common.dtConvertor.Columns.Add("ProductType");
            Atend.Control.Common.dtConvertor.Columns.Add("OldProductCode");
            Atend.Control.Common.dtConvertor.Columns.Add("NewProductCode");
            Atend.Control.Common.dtConvertor.Columns.Add("GroupObjID");

            try
            {
                OldConnection.Open();
                NewConnection.Open();
                try
                {
                    OldTransaction = OldConnection.BeginTransaction();
                    NewTransaction = NewConnection.BeginTransaction();
                    //int ServerCode = FindCode(xCodeProduct, Type);

                    System.Data.DataTable dtEntity = Atend.Global.Acad.Global.ReadAllEntity();
                    int NewCode = 0;
                    foreach (DataRow drEntity in dtEntity.Rows)
                    {
                        #region Switch
                        switch ((Atend.Control.Enum.ProductType)Convert.ToInt32(drEntity["ProductCode"].ToString()))
                        {

                            //    case Atend.Control.Enum.ProductType.AirPost:
                            //        ed.WriteMessage("Sub Is : AirPost \n");
                            //        Atend.Base.Equipment.EAirPost SelectedEquipEAirPost = Atend.Base.Equipment.EAirPost.SelectByXCodeForDesign(xCodeProduct, LocalConnection, LocalTransaction);
                            //        if (SelectedEquipEAirPost.Code != -1)
                            //        {
                            //            if (SelectedEquipEAirPost.Code == 0)
                            //            {
                            //                if (SelectedEquipEAirPost.ServerInsert(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                {
                            //                    NewCode = SelectedEquipEAirPost.Code;
                            //                    if (!SelectedEquipEAirPost.UpdateX(LocalTransaction, LocalConnection))
                            //                    {
                            //                        throw new System.Exception("UpdateX Airpost Failed");
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    throw new System.Exception("ServerInsert failed" + Type.ToString());
                            //                }
                            //            }
                            //            else
                            //            {
                            //                Atend.Base.Equipment.EAirPost ServerSelectedEquipAirPost = Atend.Base.Equipment.EAirPost.ServerSelectByCode(SelectedEquipEAirPost.Code, ServerConnection, ServerTransaction);
                            //                if (ServerSelectedEquipAirPost.Code != -1)
                            //                {
                            //                    if (SelectedEquipEAirPost.ServerUpdate(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                    {
                            //                        NewCode = SelectedEquipEAirPost.Code;
                            //                    }
                            //                    else
                            //                    {
                            //                        throw new System.Exception("ServerUpdate failed" + Type.ToString());
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    throw new System.Exception("airpost:There Is No Row On Server For Update " + Type.ToString());
                            //                }
                            //            }
                            //        }
                            //        else
                            //        {
                            //            ed.WriteMessage(Type.ToString() + " does not exist \n");
                            //        }
                            //        break;

                            case Atend.Control.Enum.ProductType.Arm:
                                ed.WriteMessage("Sub Is : Arm \n");
                                Atend.Base.Equipment.EArm SelectedEquipEArm = Atend.Base.Equipment.EArm.AccessSelectByCodeForConvertor(Convert.ToInt32(drEntity["ProductCode"].ToString()), OldTransaction, OldConnection);
                                if (SelectedEquipEArm.Code != -1)
                                {

                                    if (drEntity["GroupObjID"].ToString() != "0")
                                    {
                                        DataRow[] dr = Atend.Control.Common.dtConvertor.Select(string.Format("GroupObjID={0} And ProductType={1}", drEntity["GroupObjId"].ToString(), drEntity["ProductType"].ToString()));
                                        if (dr.Length == 0)
                                        {
                                            DataRow[] drMem = Atend.Control.Common.dtConvertor.Select(string.Format("OldProductCode={0} And ProductType={1}", drEntity["OldProductCode"].ToString(), drEntity["ProductType"].ToString()));
                                            if (drMem.Length == 0)
                                            {
                                                if (!SelectedEquipEArm.AccessInsert(OldTransaction, OldConnection, NewTransaction, NewConnection, true, true))
                                                    throw new System.Exception("Exception In AccessInsert in Arm");
                                                else
                                                {
                                                    DataRow drNew = Atend.Control.Common.dtConvertor.NewRow();
                                                    drNew["ProuctType"] = drEntity["ProductType"].ToString();
                                                    drNew["OldProductCode"] = drEntity["ProductCode"].ToString();
                                                    drNew["NewProductCode"] = SelectedEquipEArm.Code;
                                                    drNew["GroupObjID"] = drEntity["GroupObjID"].ToString();
                                                    Atend.Control.Common.dtConvertor.Rows.Add(drNew);
                                                    NewCode = SelectedEquipEArm.Code; ;
                                                }
                                            }
                                            else
                                            {
                                                NewCode = Convert.ToInt32(drMem[0]["NewProductCode"].ToString());
                                            }
                                        }
                                    }
                                    else
                                    {
                                        DataRow[] drMem = Atend.Control.Common.dtConvertor.Select(string.Format("OldProductCode={0} And ProductType={1}", drEntity["OldProductCode"].ToString(), drEntity["ProductType"].ToString()));
                                        if (drMem.Length == 0)
                                        {
                                            if (!SelectedEquipEArm.AccessInsert(OldTransaction, OldConnection, NewTransaction, NewConnection, true, true))
                                                throw new System.Exception("Exception In AccessInsert in Arm");
                                            else
                                            {
                                                DataRow drNew = Atend.Control.Common.dtConvertor.NewRow();
                                                drNew["ProuctType"] = drEntity["ProductType"].ToString();
                                                drNew["OldProductCode"] = drEntity["ProductCode"].ToString();
                                                drNew["NewProductCode"] = SelectedEquipEArm.Code;
                                                drNew["GroupObjID"] = drEntity["GroupObjID"].ToString();
                                                Atend.Control.Common.dtConvertor.Rows.Add(drNew);
                                                NewCode = SelectedEquipEArm.Code; ;
                                            }
                                        }
                                        else
                                        {
                                            NewCode = Convert.ToInt32(drMem[0]["NewProductCode"].ToString());
                                        }
                                    }

                                    //Atend.Base.Acad.AT_INFO AT = Atend.Base.Acad.AT_INFO.__SelectBySelectedObjectId((ObjectId)dr["ObjID"].ToString());
                                    //AT.ProductCode=

                                }
                                else
                                {
                                    throw new System.Exception("Lack Of Data In Arm");
                                }


                                break;

                            //    case Atend.Control.Enum.ProductType.AuoKey3p:
                            //        ed.WriteMessage("Sub Is : AuoKey3p \n");

                            //        Atend.Base.Equipment.EAutoKey_3p SelectedEquipEAutoKey_3p = Atend.Base.Equipment.EAutoKey_3p.SelectByXCodeForDesign(xCodeProduct, LocalConnection, LocalTransaction);
                            //        if (SelectedEquipEAutoKey_3p.Code != -1)
                            //        {
                            //            //Atend.Base.Equipment.EAutoKey_3p answer = Atend.Base.Equipment.EAutoKey_3p.AccessSelectByXCode(SelectedEquipEAutoKey_3p.XCode, _transaction, _connection);
                            //            //if (answer.Code != -1)
                            //            //{
                            //            //    NewCode = answer.Code;
                            //            //}
                            //            //else
                            //            //{
                            //            if (SelectedEquipEAutoKey_3p.Code == 0)
                            //            {//confirm
                            //                if (SelectedEquipEAutoKey_3p.ServerInsert(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                {
                            //                    NewCode = SelectedEquipEAutoKey_3p.Code;
                            //                    if (!SelectedEquipEAutoKey_3p.UpdateX(LocalTransaction, LocalConnection))
                            //                    {
                            //                        throw new System.Exception("UpdateX AuoKey3p Failed");
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    throw new System.Exception("ServerInsert failed" + Type.ToString());
                            //                }
                            //            }
                            //            else
                            //            {
                            //                Atend.Base.Equipment.EAutoKey_3p ServerSelectedAutoKey3p = Atend.Base.Equipment.EAutoKey_3p.ServerSelectByCode(SelectedEquipEAutoKey_3p.Code, ServerConnection, ServerTransaction);
                            //                if (ServerSelectedAutoKey3p.Code != -1)
                            //                {
                            //                    if (SelectedEquipEAutoKey_3p.ServerUpdate(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                    {
                            //                        NewCode = SelectedEquipEAutoKey_3p.Code;
                            //                    }
                            //                    else
                            //                    {
                            //                        throw new System.Exception("ServerUpdate failed" + Type.ToString());
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    throw new System.Exception("AuoKey3p:There Is No Row On Server For Update " + Type.ToString());
                            //                }

                            //            }
                            //            //}
                            //        }
                            //        else
                            //        {
                            //            ed.WriteMessage(Type.ToString() + " does not exist \n");
                            //        }
                            //        break;
                            //    case Atend.Control.Enum.ProductType.Breaker:
                            //        ed.WriteMessage("Sub Is : Breaker \n");
                            //        Atend.Base.Equipment.EBreaker SelectedEquipEBreaker = Atend.Base.Equipment.EBreaker.SelectByXCodeForDesign(xCodeProduct, LocalConnection, LocalTransaction);
                            //        if (SelectedEquipEBreaker.Code != -1)
                            //        {
                            //            //Atend.Base.Equipment.EBreaker answer = Atend.Base.Equipment.EBreaker.AccessSelectByXCode(SelectedEquipEBreaker.XCode, _transaction, _connection);
                            //            //if (answer.Code != -1)
                            //            //{
                            //            //    NewCode = answer.Code;
                            //            //}
                            //            //else
                            //            //{
                            //            if (SelectedEquipEBreaker.Code == 0)
                            //            {//confirm
                            //                if (SelectedEquipEBreaker.ServerInsert(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                {
                            //                    NewCode = SelectedEquipEBreaker.Code;
                            //                    if (!SelectedEquipEBreaker.UpdateX(LocalTransaction, LocalConnection))
                            //                    {
                            //                        throw new System.Exception("UpdateX Breaker Failed");
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    throw new System.Exception("AccessInsert failed" + Type.ToString());
                            //                }
                            //            }
                            //            else
                            //            {
                            //                Atend.Base.Equipment.EBreaker ServerSelectedBreaker = Atend.Base.Equipment.EBreaker.ServerSelectByCode(SelectedEquipEBreaker.Code, ServerConnection, ServerTransaction);
                            //                if (ServerSelectedBreaker.Code != -1)
                            //                {
                            //                    if (SelectedEquipEBreaker.ServerUpdate(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                    {
                            //                        NewCode = SelectedEquipEBreaker.Code;
                            //                    }
                            //                    else
                            //                    {
                            //                        throw new System.Exception("ServerUpdate failed" + Type.ToString());
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    throw new System.Exception("Breaker:There Is No Row On Server For Update " + Type.ToString());
                            //                }
                            //            }
                            //            //}
                            //        }
                            //        else
                            //        {
                            //            ed.WriteMessage(Type.ToString() + " does not exist \n");
                            //        }
                            //        break;
                            //    case Atend.Control.Enum.ProductType.Bus:
                            //        ed.WriteMessage("Sub Is : Bus \n");
                            //        Atend.Base.Equipment.EBus SelectedEquipEBus = Atend.Base.Equipment.EBus.SelectByXCodeForDesign(xCodeProduct, LocalConnection, LocalTransaction);
                            //        if (SelectedEquipEBus.Code != -1)
                            //        {
                            //            //Atend.Base.Equipment.EBus answer = Atend.Base.Equipment.EBus.AccessSelectByXCode(SelectedEquipEBus.XCode, _transaction, _connection);
                            //            //if (answer.Code != -1)
                            //            //{
                            //            //    NewCode = answer.Code;
                            //            //}
                            //            //else
                            //            //{
                            //            if (SelectedEquipEBus.Code == 0)
                            //            {//confirm
                            //                if (SelectedEquipEBus.ServerInsert(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                {
                            //                    NewCode = SelectedEquipEBus.Code;
                            //                    if (!SelectedEquipEBus.UpdateX(LocalTransaction, LocalConnection))
                            //                    {
                            //                        throw new System.Exception("UpdateX Bus Failed");
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    throw new System.Exception("ServerInsert failed" + Type.ToString());
                            //                }
                            //            }
                            //            else
                            //            {
                            //                Atend.Base.Equipment.EBus ServerSelectedBus = Atend.Base.Equipment.EBus.ServerSelectByCode(SelectedEquipEBus.Code, ServerConnection, ServerTransaction);
                            //                if (ServerSelectedBus.Code != -1)
                            //                {
                            //                    if (SelectedEquipEBus.ServerUpdate(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                    {
                            //                        NewCode = SelectedEquipEBus.Code;
                            //                    }
                            //                    else
                            //                    {
                            //                        throw new System.Exception("ServerUpdate failed" + Type.ToString());
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    throw new System.Exception("Bus:There Is No Row On Server For Update " + Type.ToString());
                            //                }
                            //            }
                            //            //}
                            //        }
                            //        else
                            //        {
                            //            ed.WriteMessage(Type.ToString() + " does not exist \n");
                            //        }
                            //        break;
                            //    case Atend.Control.Enum.ProductType.CatOut:
                            //        ed.WriteMessage("ShareOnServer Sub Is : CatOut \n");
                            //        #region Catout
                            //        Atend.Base.Equipment.ECatOut SelectedEquipECatOut = Atend.Base.Equipment.ECatOut.SelectByXCodeForDesign(xCodeProduct, LocalConnection, LocalTransaction);
                            //        if (SelectedEquipECatOut.Code != -1)
                            //        {
                            //            //Atend.Base.Equipment.ECatOut answer = Atend.Base.Equipment.ECatOut.AccessSelectByXCode(SelectedEquipECatOut.XCode, _transaction, _connection);
                            //            //if (answer.Code != -1)
                            //            //{
                            //            //    NewCode = answer.Code;
                            //            //}
                            //            //else
                            //            //{
                            //            if (SelectedEquipECatOut.Code == 0)
                            //            {//confirm
                            //                if (SelectedEquipECatOut.ServerInsert(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                {
                            //                    NewCode = SelectedEquipECatOut.Code;
                            //                    if (!SelectedEquipECatOut.UpdateX(LocalTransaction, LocalConnection))
                            //                    {
                            //                        throw new System.Exception("UpdateXCatOut Failed ");
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    throw new System.Exception("ServerInsert failed" + Type.ToString());
                            //                }
                            //            }
                            //            else
                            //            {
                            //                Atend.Base.Equipment.ECatOut ServerSelectedCatout = Atend.Base.Equipment.ECatOut.ServerSelectByCode(SelectedEquipECatOut.Code, ServerConnection, ServerTransaction);
                            //                if (ServerSelectedCatout.Code != -1)
                            //                {
                            //                    if (SelectedEquipECatOut.ServerUpdate(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                    {
                            //                        NewCode = SelectedEquipECatOut.Code;
                            //                    }
                            //                    else
                            //                    {
                            //                        throw new System.Exception("ServerUpdate failed" + Type.ToString());
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    throw new System.Exception("CatOut:There Is No Row On Server For Update " + Type.ToString());
                            //                }
                            //            }
                            //            //}
                            //        }
                            //        else
                            //        {
                            //            ed.WriteMessage(Type.ToString() + " does not exist \n");
                            //        }
                            //        #endregion
                            //        break;
                            //    case Atend.Control.Enum.ProductType.Cell:
                            //        ed.WriteMessage("Sub Is : Cell \n");
                            //        Atend.Base.Equipment.ECell SelectedEquipECell = Atend.Base.Equipment.ECell.SelectByXCodeForDesign(xCodeProduct, LocalConnection, LocalTransaction);
                            //        if (SelectedEquipECell.Code != -1)
                            //        {
                            //            //Atend.Base.Equipment.ECell answer = Atend.Base.Equipment.ECell.AccessSelectByXCode(SelectedEquipECell.XCode, _transaction, _connection);
                            //            //if (answer.Code != -1)
                            //            //{
                            //            //    NewCode = answer.Code;
                            //            //}
                            //            //else
                            //            //{
                            //            if (SelectedEquipECell.Code == 0)
                            //            {//confirm
                            //                if (SelectedEquipECell.ServerInsert(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                {
                            //                    NewCode = SelectedEquipECell.Code;
                            //                    if (!SelectedEquipECell.UpdateX(LocalTransaction, LocalConnection))
                            //                    {
                            //                        throw new System.Exception("UpdateX   Cell Failed");
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    throw new System.Exception("ServerInsert failed" + Type.ToString());
                            //                }
                            //            }
                            //            else
                            //            {
                            //                Atend.Base.Equipment.ECell serverSelectedCell = Atend.Base.Equipment.ECell.ServerSelectByCode(SelectedEquipECell.Code, ServerConnection, ServerTransaction);
                            //                if (serverSelectedCell.Code != -1)
                            //                {
                            //                    if (SelectedEquipECell.ServerUpdate(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                    {
                            //                        NewCode = SelectedEquipECell.Code;
                            //                    }
                            //                    else
                            //                    {
                            //                        throw new System.Exception("ServerUpdate failed" + Type.ToString());
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    throw new System.Exception("Cell:There Is No Row On Server For Update " + Type.ToString());
                            //                }
                            //            }
                            //            //}
                            //        }
                            //        else
                            //        {
                            //            ed.WriteMessage(Type.ToString() + " does not exist \n");
                            //        }
                            //        break;
                            //    case Atend.Control.Enum.ProductType.Conductor:
                            //        ed.WriteMessage("Sub Is : Conductor \n");
                            //        Atend.Base.Equipment.EConductor SelectedEquipEConductor = Atend.Base.Equipment.EConductor.SelectByXCodeForDesign(xCodeProduct, LocalConnection, LocalTransaction);
                            //        if (SelectedEquipEConductor.Code != -1)
                            //        {
                            //            //Atend.Base.Equipment.EConductor answer = Atend.Base.Equipment.EConductor.AccessSelectByXCode(SelectedEquipEConductor.XCode, _transaction, _connection);
                            //            //if (answer.Code != -1)
                            //            //{
                            //            //    NewCode = answer.Code;
                            //            //}
                            //            //else
                            //            //{
                            //            if (SelectedEquipEConductor.Code == 0)
                            //            {//confirm
                            //                if (SelectedEquipEConductor.ServerInsert(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                {
                            //                    NewCode = SelectedEquipEConductor.Code;
                            //                    if (!SelectedEquipEConductor.UpdateX(LocalTransaction, LocalConnection))
                            //                    {
                            //                        throw new System.Exception("UpdateX    Conductor     Failed");
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    throw new System.Exception("ServerInsert failed" + Type.ToString());
                            //                }
                            //            }
                            //            else
                            //            {
                            //                Atend.Base.Equipment.EConductor ServerSelectedConductor = Atend.Base.Equipment.EConductor.ServerSelectByCode(SelectedEquipEConductor.Code, ServerConnection, ServerTransaction);
                            //                if (ServerSelectedConductor.Code != -1)
                            //                {
                            //                    if (SelectedEquipEConductor.ServerUpdate(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                    {
                            //                        NewCode = SelectedEquipEConductor.Code;
                            //                    }
                            //                    else
                            //                    {
                            //                        throw new System.Exception("ServerUpdate failed" + Type.ToString());
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    throw new System.Exception("Conductor:There Is No Row On Server For Update " + Type.ToString());
                            //                }
                            //            }
                            //            //}
                            //        }
                            //        else
                            //        {
                            //            ed.WriteMessage(Type.ToString() + " does not exist \n");
                            //        }
                            //        break;

                            //    case Atend.Control.Enum.ProductType.ConductorTip:
                            //        ed.WriteMessage("Sub Is : ConductorTip \n");

                            //        Atend.Base.Equipment.EConductorTip SelectedequipEConductorTip = Atend.Base.Equipment.EConductorTip.SelectByXCodeForDesign(xCodeProduct, LocalConnection, LocalTransaction);




                            //        if (SelectedequipEConductorTip.Code != -1)
                            //        {

                            //            Atend.Base.Equipment.EConductor SelectedPhaseConductor = Atend.Base.Equipment.EConductor.SelectByXCodeForDesign(SelectedequipEConductorTip.PhaseProductXCode, LocalConnection, LocalTransaction);
                            //            Atend.Base.Equipment.EConductor SelectedNeutralConductor = Atend.Base.Equipment.EConductor.SelectByXCodeForDesign(SelectedequipEConductorTip.NeutralProductXCode, LocalConnection, LocalTransaction);
                            //            Atend.Base.Equipment.EConductor SelectedNightConductor = Atend.Base.Equipment.EConductor.SelectByXCodeForDesign(SelectedequipEConductorTip.NightProductXCode, LocalConnection, LocalTransaction);

                            //            if (SelectedPhaseConductor.Code != -1)
                            //            {
                            //                if (SelectedPhaseConductor.Code == 0)
                            //                {
                            //                    if (SelectedPhaseConductor.ServerInsert(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                    {
                            //                        NewCode = SelectedPhaseConductor.Code;
                            //                        if (!SelectedPhaseConductor.UpdateX(LocalTransaction, LocalConnection))
                            //                            throw new System.Exception("SelectedPhaseConductor Updatex Failed");
                            //                    }
                            //                    else
                            //                    {
                            //                        throw new System.Exception("Conductor ServerInsert failed in CondTip");
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    Atend.Base.Equipment.EConductor ServerCond = Atend.Base.Equipment.EConductor.ServerSelectByCode(SelectedPhaseConductor.Code, ServerConnection, ServerTransaction);
                            //                    if (ServerCond.Code != -1)
                            //                    {
                            //                        if (SelectedPhaseConductor.ServerUpdate(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                        {
                            //                            NewCode = SelectedPhaseConductor.Code;
                            //                        }
                            //                        else
                            //                        {
                            //                            throw new System.Exception("PhaseCond ServerUpdate Failed in CondTip");
                            //                        }
                            //                    }
                            //                    else
                            //                    {
                            //                        throw new System.Exception("There Is No Row On Server in CondPhase in CondTip");
                            //                    }
                            //                }

                            //            }
                            //            else
                            //            {
                            //                throw new System.Exception("Lack Of data In Phase Conductor in Conductor Tip");
                            //            }




                            //            if (SelectedNightConductor.Code != -1)
                            //            {
                            //                if (SelectedNightConductor.Code == 0)
                            //                {
                            //                    if (SelectedNightConductor.ServerInsert(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                    {
                            //                        NewCode = SelectedNightConductor.Code;
                            //                        if (!SelectedNightConductor.UpdateX(LocalTransaction, LocalConnection))
                            //                            throw new System.Exception("SelectedNightConductor Updatex Failed");
                            //                    }
                            //                    else
                            //                    {
                            //                        throw new System.Exception("NightConductor ServerInsert failed in CondTip");
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    Atend.Base.Equipment.EConductor ServerNightCond = Atend.Base.Equipment.EConductor.ServerSelectByCode(SelectedNightConductor.Code, ServerConnection, ServerTransaction);
                            //                    if (ServerNightCond.Code != -1)
                            //                    {
                            //                        if (ServerNightCond.ServerUpdate(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                        {
                            //                            NewCode = ServerNightCond.Code;
                            //                        }
                            //                        else
                            //                        {
                            //                            throw new System.Exception("NightCond ServerUpdate Failed in CondTip");
                            //                        }
                            //                    }
                            //                    else
                            //                    {
                            //                        throw new System.Exception("There Is No Row On Server in CondNight in CondTip");
                            //                    }
                            //                }

                            //            }
                            //            else
                            //            {
                            //                throw new System.Exception("Lack Of data In Night Conductor in Conductor Tip");
                            //            }




                            //            if (SelectedNeutralConductor.Code != -1)
                            //            {
                            //                if (SelectedNeutralConductor.Code == 0)
                            //                {
                            //                    if (SelectedNeutralConductor.ServerInsert(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                    {
                            //                        NewCode = SelectedNeutralConductor.Code;
                            //                        if (!SelectedNeutralConductor.UpdateX(LocalTransaction, LocalConnection))
                            //                            throw new System.Exception("SelectedNeutralConductor Updatex Failed");
                            //                    }
                            //                    else
                            //                    {
                            //                        throw new System.Exception("NeutralConductor ServerInsert failed in CondTip");
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    Atend.Base.Equipment.EConductor ServerNeutralCond = Atend.Base.Equipment.EConductor.ServerSelectByCode(SelectedNeutralConductor.Code, ServerConnection, ServerTransaction);
                            //                    if (ServerNeutralCond.Code != -1)
                            //                    {
                            //                        if (SelectedNeutralConductor.ServerUpdate(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                        {
                            //                            NewCode = SelectedNeutralConductor.Code;
                            //                        }
                            //                        else
                            //                        {
                            //                            throw new System.Exception("NeutralCond ServerUpdate Failed in CondTip");
                            //                        }
                            //                    }
                            //                    else
                            //                    {
                            //                        throw new System.Exception("There Is No Row On Server in CondNeutral in CondTip");
                            //                    }
                            //                }

                            //            }
                            //            else
                            //            {
                            //                throw new System.Exception("Lack Of data In PNetral Conductor in Conductor Tip");
                            //            }


                            //            SelectedequipEConductorTip.PhaseProductCode = SelectedPhaseConductor.Code;
                            //            SelectedequipEConductorTip.NeutralProductCode = SelectedNeutralConductor.Code;
                            //            SelectedequipEConductorTip.NightProductCode = SelectedNightConductor.Code;
                            //            ed.WriteMessage("**ShareOnServer:SelectedequipEConductorTip.Code={0}\n", SelectedequipEConductorTip.Code);
                            //            if (SelectedequipEConductorTip.Code == 0)
                            //            {
                            //                if (SelectedequipEConductorTip.ServerInsert(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                {
                            //                    NewCode = SelectedequipEConductorTip.Code;
                            //                    if (!SelectedequipEConductorTip.UpdateX(LocalTransaction, LocalConnection))
                            //                    {
                            //                        throw new System.Exception("UpdateX    ConductorTip     Failed");
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    throw new System.Exception("ServerInsert failed" + Type.ToString());
                            //                }
                            //            }
                            //            else
                            //            {
                            //                Atend.Base.Equipment.EConductorTip ServerSelectedConductorTip = Atend.Base.Equipment.EConductorTip.ServerSelectByCode(SelectedequipEConductorTip.Code, ServerConnection, ServerTransaction);
                            //                if (ServerSelectedConductorTip.Code != -1)
                            //                {
                            //                    if (SelectedequipEConductorTip.ServerUpdate(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                    {
                            //                        NewCode = SelectedequipEConductorTip.Code;
                            //                    }
                            //                    else
                            //                    {
                            //                        throw new System.Exception("ServerUpdate failed" + Type.ToString());
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    throw new System.Exception("Conductor:There Is No Row On Server For Update " + Type.ToString());
                            //                }
                            //            }
                            //            //}
                            //        }
                            //        else
                            //        {
                            //            ed.WriteMessage(Type.ToString() + " does not exist \n");
                            //        }
                            //        break;
                            //    case Atend.Control.Enum.ProductType.Consol:
                            //        ed.WriteMessage("Sub Is : Consol \n");
                            //        //ed.WriteMessage("**XCODE={0}\n", dr["XCode"].ToString());
                            //        Atend.Base.Equipment.EConsol SelectedEquipEConsol = Atend.Base.Equipment.EConsol.SelectByXCodeForDesign(xCodeProduct, LocalConnection, LocalTransaction);
                            //        if (SelectedEquipEConsol.Code != -1)
                            //        {
                            //            //Atend.Base.Equipment.EConsol answer = Atend.Base.Equipment.EConsol.AccessSelectByXCode(SelectedEquipEConsol.XCode, _transaction, _connection);
                            //            //if (answer.Code != -1)
                            //            //{
                            //            //    NewCode = answer.Code;
                            //            //}
                            //            //else
                            //            //{
                            //            if (SelectedEquipEConsol.Code == 0)
                            //            {//confirm
                            //                if (SelectedEquipEConsol.ServerInsert(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                {
                            //                    NewCode = SelectedEquipEConsol.Code;
                            //                    if (!SelectedEquipEConsol.UpdateX(LocalTransaction, LocalConnection))
                            //                    {
                            //                        throw new System.Exception("UpdateX   Consol      Failed");
                            //                    }

                            //                }
                            //                else
                            //                {
                            //                    throw new System.Exception("ServerInsert failed" + Type.ToString());
                            //                }
                            //            }
                            //            else
                            //            {
                            //                Atend.Base.Equipment.EConsol ServerSelectedConsol = Atend.Base.Equipment.EConsol.ServerSelectByCode(SelectedEquipEConsol.Code, ServerConnection, ServerTransaction);
                            //                if (ServerSelectedConsol.Code != -1)
                            //                {
                            //                    if (SelectedEquipEConsol.ServerUpdate(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                    {
                            //                        NewCode = SelectedEquipEConsol.Code;
                            //                    }
                            //                    else
                            //                    {
                            //                        throw new System.Exception("ServerUpdate failed" + Type.ToString());
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    throw new System.Exception("Consol:There Is No Row On Server For Update " + Type.ToString());
                            //                }
                            //            }
                            //            // }
                            //        }
                            //        else
                            //        {
                            //            ed.WriteMessage(Type.ToString() + " does not exist \n");
                            //        }
                            //        break;
                            //    case Atend.Control.Enum.ProductType.Countor:
                            //        ed.WriteMessage("Sub Is : Countor \n");
                            //        Atend.Base.Equipment.ECountor SelectedEquipECountor = Atend.Base.Equipment.ECountor.SelectByXCodeForDesign(xCodeProduct, LocalConnection, LocalTransaction);
                            //        if (SelectedEquipECountor.Code != -1)
                            //        {
                            //            //Atend.Base.Equipment.ECountor answer = Atend.Base.Equipment.ECountor.AccessSelectByXCode(SelectedEquipECountor.XCode, _transaction, _connection);
                            //            //if (answer.Code != -1)
                            //            //{
                            //            //    NewCode = answer.Code;
                            //            //}
                            //            //else
                            //            //{
                            //            if (SelectedEquipECountor.Code == 0)
                            //            {//confirm
                            //                if (SelectedEquipECountor.ServerInsert(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                {
                            //                    NewCode = SelectedEquipECountor.Code;
                            //                    if (!SelectedEquipECountor.UpdateX(LocalTransaction, LocalConnection))
                            //                    {
                            //                        throw new System.Exception(" UpdateX  Countor  Failed");
                            //                    }

                            //                }
                            //                else
                            //                {
                            //                    throw new System.Exception("ServerInsert failed" + Type.ToString());
                            //                }
                            //            }
                            //            else
                            //            {
                            //                Atend.Base.Equipment.ECountor ServerSelectedCountor = Atend.Base.Equipment.ECountor.ServerSelectByCode(SelectedEquipECountor.Code, ServerConnection, ServerTransaction);
                            //                if (ServerSelectedCountor.Code != -1)
                            //                {
                            //                    if (SelectedEquipECountor.ServerUpdate(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                    {
                            //                        NewCode = SelectedEquipECountor.Code;
                            //                    }
                            //                    else
                            //                    {
                            //                        throw new System.Exception("ServerUpdate failed" + Type.ToString());
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    throw new System.Exception("Countor:There Is No Row On Server For Update " + Type.ToString());
                            //                }
                            //            }


                            //            // }
                            //        }
                            //        else
                            //        {
                            //            ed.WriteMessage(Type.ToString() + " does not exist \n");
                            //        }
                            //        break;
                            //    case Atend.Control.Enum.ProductType.CT:
                            //        ed.WriteMessage("Sub Is : CT \n");
                            //        Atend.Base.Equipment.ECT SelectedEquipECT = Atend.Base.Equipment.ECT.SelectByXCodeForDesign(xCodeProduct, LocalConnection, LocalTransaction);
                            //        if (SelectedEquipECT.Code != -1)
                            //        {
                            //            //Atend.Base.Equipment.ECT answer = Atend.Base.Equipment.ECT.AccessSelectByXCode(SelectedEquipECT.XCode, _transaction, _connection);
                            //            //if (answer.Code != -1)
                            //            //{
                            //            //    NewCode = answer.Code;
                            //            //}
                            //            //else
                            //            //{
                            //            if (SelectedEquipECT.Code == 0)
                            //            {//confirm
                            //                if (SelectedEquipECT.ServerInsert(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                {
                            //                    NewCode = SelectedEquipECT.Code;
                            //                    if (!SelectedEquipECT.UpdateX(LocalTransaction, LocalConnection))
                            //                    {
                            //                        throw new System.Exception(" UpdateX  CT  Failed");
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    throw new System.Exception("ServerInsert failed" + Type.ToString());
                            //                }
                            //            }
                            //            else
                            //            {
                            //                Atend.Base.Equipment.ECT ServerSelectedCT = Atend.Base.Equipment.ECT.ServerSelectByCode(SelectedEquipECT.Code, ServerConnection, ServerTransaction);
                            //                if (ServerSelectedCT.Code != -1)
                            //                {
                            //                    if (SelectedEquipECT.ServerUpdate(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                    {
                            //                        NewCode = SelectedEquipECT.Code;
                            //                    }
                            //                    else
                            //                    {
                            //                        throw new System.Exception("ServerUpdate failed" + Type.ToString());
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    throw new System.Exception("CT:There Is No Row On Server For Update " + Type.ToString());
                            //                }
                            //            }

                            //            //}
                            //        }
                            //        else
                            //        {
                            //            ed.WriteMessage(Type.ToString() + " does not exist \n");
                            //        }
                            //        break;
                            //    case Atend.Control.Enum.ProductType.DB:
                            //        ed.WriteMessage("Sub Is : DB 3\n");
                            //        Atend.Base.Equipment.EDB SelectedEquipEDB = Atend.Base.Equipment.EDB.SelectByXCodeForDesign(xCodeProduct, LocalConnection, LocalTransaction);
                            //        ed.WriteMessage("SelectedEquipEDB:{0}\n", SelectedEquipEDB.Code);
                            //        if (SelectedEquipEDB.Code != -1)
                            //        {

                            //            if (SelectedEquipEDB.Code == 0)
                            //            {
                            //                ed.WriteMessage("GO FOR DB INSERT\n");
                            //                if (SelectedEquipEDB.ServerInsert(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                {
                            //                    NewCode = SelectedEquipEDB.Code;
                            //                    ed.WriteMessage("DB.Code={0}\n", NewCode);
                            //                    if (!SelectedEquipEDB.UpdateX(LocalTransaction, LocalConnection))
                            //                    {
                            //                        throw new System.Exception(" UpdateX  DB  Failed");
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    throw new System.Exception("ServerInsert failed" + Type.ToString());

                            //                }
                            //            }
                            //            else
                            //            {
                            //                Atend.Base.Equipment.EDB ServerSelectedDB = Atend.Base.Equipment.EDB.ServerSelectByCode(SelectedEquipEDB.Code, ServerConnection, ServerTransaction);
                            //                ed.WriteMessage("ServerSelectedDB={0}\n", ServerSelectedDB.Code);
                            //                if (ServerSelectedDB.Code != -1)
                            //                {
                            //                    ed.WriteMessage("GO For Update\n");
                            //                    if (SelectedEquipEDB.ServerUpdate(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                    {
                            //                        NewCode = SelectedEquipEDB.Code;
                            //                    }
                            //                    else
                            //                    {
                            //                        throw new System.Exception("ServerUpdate failed" + Type.ToString());

                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    throw new System.Exception("DB:There Is No Row On Server For Update " + Type.ToString());
                            //                }
                            //            }

                            //            ed.WriteMessage("&&&&&&&&&&&&&&&&&&&&&&&&&&&&&\n");
                            //            DataTable DBPhuses = Atend.Base.Equipment.EDBPhuse.SelectByDBXCode(xCodeProduct, LocalTransaction, LocalConnection);
                            //            ed.WriteMessage("DBPHUSES Rows.Count={0}\n", DBPhuses.Rows.Count);
                            //            if (!Atend.Base.Equipment.EDBPhuse.Delete(ServerTransaction, ServerConnection, SelectedEquipEDB.Code))
                            //            {
                            //                throw new System.Exception("Error In Delete DbPhuse In DB ");
                            //            }
                            //            foreach (DataRow DBPhuse in DBPhuses.Rows)
                            //            {
                            //                Atend.Base.Equipment.EPhuse _EPhuse = Atend.Base.Equipment.EPhuse.SelectByXCodeForDesign(new Guid(DBPhuse["PhuseXcode"].ToString()), LocalConnection, LocalTransaction);
                            //                ed.WriteMessage("Phuse.Code={0}\n", _EPhuse.Code);
                            //                Atend.Base.Equipment.EPhusePole _EphusePole = Atend.Base.Equipment.EPhusePole.SelectByXCodeForDesign(_EPhuse.PhusePoleXCode, LocalConnection, LocalTransaction);
                            //                if (_EphusePole.Code != -1)
                            //                {
                            //                    if (_EphusePole.Code == 0)
                            //                    {
                            //                        if (!_EphusePole.ServerInsert(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                        {
                            //                            throw new System.Exception("ServerInsert failed" + Type.ToString() + ":PhusePole");
                            //                        }
                            //                        else
                            //                        {

                            //                            if (!_EphusePole.UpdateX(LocalTransaction, LocalConnection))
                            //                            {
                            //                                throw new System.Exception(" UpdateX  EPhusePole  Failed");
                            //                            }

                            //                        }
                            //                    }
                            //                    else
                            //                    {
                            //                        Atend.Base.Equipment.EPhusePole ServerSelectedPhusePole = Atend.Base.Equipment.EPhusePole.ServerSelectByCode(_EphusePole.Code, ServerConnection, ServerTransaction);
                            //                        if (ServerSelectedPhusePole.Code != -1)
                            //                        {
                            //                            if (!_EphusePole.ServerUpdate(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                            {
                            //                                throw new System.Exception("ServerUpdate failed" + Type.ToString() + ":PhusePole");
                            //                            }
                            //                        }
                            //                        else
                            //                        {
                            //                            throw new System.Exception("There Is No Row on Server For update " + Type.ToString());
                            //                        }
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    throw new System.Exception("Lack Of Data In PhusePole inDB");
                            //                }



                            //                _EPhuse.PhusePoleCode = _EphusePole.Code;
                            //                if (_EPhuse.Code != -1)
                            //                {
                            //                    if (_EPhuse.Code == 0)
                            //                    {
                            //                        if (!_EPhuse.ServerInsert(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                        {
                            //                            throw new System.Exception("ServerInsert failed" + Type.ToString() + ":Phuse");
                            //                        }
                            //                        else
                            //                        {

                            //                            if (!_EPhuse.UpdateX(LocalTransaction, LocalConnection))
                            //                            {
                            //                                throw new System.Exception(" UpdateX  EPhuse  Failed");
                            //                            }

                            //                        }
                            //                    }
                            //                    else
                            //                    {
                            //                        Atend.Base.Equipment.EPhuse ServerSelectedPhuse = Atend.Base.Equipment.EPhuse.ServerSelectByCode(_EPhuse.Code, ServerConnection, ServerTransaction);
                            //                        if (ServerSelectedPhuse.Code != -1)
                            //                        {
                            //                            if (!_EPhuse.ServerUpdate(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                            {
                            //                                throw new System.Exception("ServerUpdate failed" + Type.ToString() + ":Phuse");
                            //                            }
                            //                        }
                            //                        else
                            //                        {
                            //                            throw new System.Exception("There Is No Row on Server For update " + Type.ToString());
                            //                        }
                            //                    }

                            //                    Atend.Base.Equipment.EDBPhuse _edbPhuse = Atend.Base.Equipment.EDBPhuse.SelectByXCodeForDesign(new Guid(DBPhuse["XCode"].ToString()), LocalConnection, LocalTransaction);
                            //                    ed.WriteMessage("dbPhuse.Code={0}\n", _edbPhuse.Code);
                            //                    if (_edbPhuse.Code != -1)
                            //                    {
                            //                        _edbPhuse.DBCode = SelectedEquipEDB.Code;
                            //                        _edbPhuse.PhuseCode = _EPhuse.Code;
                            //                        //if (_edbPhuse.Code == 0)
                            //                        //{
                            //                        ed.WriteMessage("go for Insert dbPhuse\n");
                            //                        if (!_edbPhuse.ServerInsert(ServerTransaction, ServerConnection, false, false, LocalTransaction, LocalConnection))
                            //                        {
                            //                            throw new System.Exception("ServerInsert failed" + Type.ToString() + ":DBPhuse");
                            //                        }
                            //                        else
                            //                        {

                            //                            if (!_edbPhuse.UpdateX(LocalTransaction, LocalConnection))
                            //                            {
                            //                                throw new System.Exception(" UpdateX  dbPhuse  Failed");
                            //                            }
                            //                        }
                            //                        //}
                            //                        //else
                            //                        //{
                            //                        //    Atend.Base.Equipment.EDBPhuse ServerSelectedDBPhuse = Atend.Base.Equipment.EDBPhuse.ServerSelectByCode(_edbPhuse.Code, ServerConnection, ServerTransaction);
                            //                        //    ed.WriteMessage("ServerSelectedDBPhuse.Code={0}\n", ServerSelectedDBPhuse.Code);
                            //                        //    if (ServerSelectedDBPhuse.Code != -1)
                            //                        //    {
                            //                        //        if (!_edbPhuse.ServerUpdate(ServerTransaction, ServerConnection, false, false, LocalTransaction, LocalConnection))
                            //                        //        {
                            //                        //            throw new System.Exception("ServerUpdate failed" + Type.ToString() + ":DBPhuse");
                            //                        //        }
                            //                        //    }
                            //                        //    else
                            //                        //    {
                            //                        //        throw new System.Exception("DBPhuse:There Is No Row on Server For update " + Type.ToString());
                            //                        //    }
                            //                        //}
                            //                    }
                            //                    else
                            //                    {
                            //                        throw new System.Exception("Lack of Data in edbPhuse");
                            //                    }

                            //                }
                            //                else
                            //                {
                            //                    throw new System.Exception("Lack Of Data in DBPhuse");
                            //                }

                            //            }
                            //        }
                            //        else
                            //        {
                            //            throw new System.Exception("AccessInsert failed" + Type.ToString());
                            //        }


                            //        break;
                            //    case Atend.Control.Enum.ProductType.Disconnector:
                            //        ed.WriteMessage("Sub Is : Disconnector \n");
                            //        Atend.Base.Equipment.EDisconnector SelectedEquipEDisconnector = Atend.Base.Equipment.EDisconnector.SelectByXCodeForDesign(xCodeProduct, LocalConnection, LocalTransaction);
                            //        if (SelectedEquipEDisconnector.Code != -1)
                            //        {
                            //            //Atend.Base.Equipment.EDisconnector answer = Atend.Base.Equipment.EDisconnector.AccessSelectByXCode(SelectedEquipEDisconnector.XCode, _transaction, _connection);
                            //            //if (answer.Code != -1)
                            //            //{
                            //            //    NewCode = answer.Code;
                            //            //}
                            //            //else
                            //            //{
                            //            if (SelectedEquipEDisconnector.Code == 0)
                            //            {//confirm
                            //                if (SelectedEquipEDisconnector.ServerInsert(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                {
                            //                    NewCode = SelectedEquipEDisconnector.Code;
                            //                    if (!SelectedEquipEDisconnector.UpdateX(LocalTransaction, LocalConnection))
                            //                    {
                            //                        throw new System.Exception(" UpdateX Disconnector   Failed");
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    throw new System.Exception("ServerInsert failed" + Type.ToString());
                            //                }
                            //            }
                            //            else
                            //            {
                            //                Atend.Base.Equipment.EDisconnector ServerSelectedDisconnector = Atend.Base.Equipment.EDisconnector.ServerSelectByCode(SelectedEquipEDisconnector.Code, ServerConnection, ServerTransaction);
                            //                if (ServerSelectedDisconnector.Code != -1)
                            //                {
                            //                    if (SelectedEquipEDisconnector.ServerUpdate(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                    {
                            //                        NewCode = SelectedEquipEDisconnector.Code;
                            //                    }
                            //                    else
                            //                    {
                            //                        throw new System.Exception("ServerUpdate failed" + Type.ToString());
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    throw new System.Exception("Disconnector:There Is No Row on Server For update " + Type.ToString());
                            //                }
                            //            }
                            //            //}
                            //        }
                            //        else
                            //        {
                            //            ed.WriteMessage(Type.ToString() + " does not exist \n");
                            //        }
                            //        break;

                            //    case Atend.Control.Enum.ProductType.Floor:
                            //        ed.WriteMessage("Sub Is : Floor \n");
                            //        Atend.Base.Equipment.EFloor SelectedEquipEFloor = Atend.Base.Equipment.EFloor.SelectByXCodeForDesign(xCodeProduct, LocalConnection, LocalTransaction);
                            //        if (SelectedEquipEFloor.Code != -1)
                            //        {
                            //            //Atend.Base.Equipment.EFloor answer = Atend.Base.Equipment.EFloor.AccessSelectByXCode(SelectedEquipEFloor.XCode, _transaction, _connection);
                            //            //if (answer.Code != -1)
                            //            //{
                            //            //    NewCode = answer.Code;
                            //            //}
                            //            //else
                            //            //{
                            //            if (SelectedEquipEFloor.Code == 0)
                            //            {//confirm
                            //                if (SelectedEquipEFloor.ServerInsert(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                {
                            //                    NewCode = SelectedEquipEFloor.Code;
                            //                    if (!SelectedEquipEFloor.UpdateX(LocalTransaction, LocalConnection))
                            //                    {
                            //                        throw new System.Exception(" UpdateX   Floor Failed");
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    throw new System.Exception("ServerInsert failed" + Type.ToString());
                            //                }
                            //            }
                            //            else
                            //            {
                            //                Atend.Base.Equipment.EFloor ServerSelectedFloor = Atend.Base.Equipment.EFloor.ServerSelectByCode(SelectedEquipEFloor.Code, ServerConnection, ServerTransaction);
                            //                if (ServerSelectedFloor.Code != -1)
                            //                {
                            //                    if (SelectedEquipEFloor.ServerUpdate(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                    {
                            //                        NewCode = SelectedEquipEFloor.Code;
                            //                    }
                            //                    else
                            //                    {
                            //                        throw new System.Exception("ServerUpdate failed" + Type.ToString());
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    throw new System.Exception("Floor:There Is No Row on Server For update " + Type.ToString());
                            //                }
                            //            }
                            //            //}
                            //        }
                            //        else
                            //        {
                            //            ed.WriteMessage(Type.ToString() + " does not exist \n");
                            //        }
                            //        break;


                            //    case Atend.Control.Enum.ProductType.Ground:
                            //        ed.WriteMessage("Sub Is : Ground \n");
                            //        Atend.Base.Equipment.EGround SelectedEquipEGround = Atend.Base.Equipment.EGround.SelectByXCodeForDesign(xCodeProduct, LocalConnection, LocalTransaction);
                            //        if (SelectedEquipEGround.Code != -1)
                            //        {
                            //            //Atend.Base.Equipment.EGround answer = Atend.Base.Equipment.EGround.AccessSelectByXCode(SelectedEquipEGround.XCode, _transaction, _connection);
                            //            //if (answer.Code != -1)
                            //            //{
                            //            //    NewCode = answer.Code;
                            //            //}
                            //            //else
                            //            //{
                            //            if (SelectedEquipEGround.Code == 0)
                            //            {
                            //                if (SelectedEquipEGround.ServerInsert(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                {
                            //                    NewCode = SelectedEquipEGround.Code;
                            //                    if (!SelectedEquipEGround.UpdateX(LocalTransaction, LocalConnection))
                            //                    {
                            //                        throw new System.Exception(" UpdateX  Ground  Failed");
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    throw new System.Exception("ServerInsert failed" + Type.ToString());
                            //                }
                            //            }
                            //            else
                            //            {

                            //                Atend.Base.Equipment.EGround ServerSelectedGround = Atend.Base.Equipment.EGround.ServerSelectByCode(SelectedEquipEGround.Code, ServerConnection, ServerTransaction);
                            //                if (ServerSelectedGround.Code != -1)
                            //                {
                            //                    if (SelectedEquipEGround.ServerUpdate(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                    {
                            //                        NewCode = SelectedEquipEGround.Code;
                            //                    }
                            //                    else
                            //                    {
                            //                        throw new System.Exception("ServerUpdate failed" + Type.ToString());
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    throw new System.Exception("Ground:There Is No Row on Server For update " + Type.ToString());
                            //                }
                            //            }
                            //            //}
                            //        }
                            //        else
                            //        {
                            //            ed.WriteMessage(Type.ToString() + " does not exist \n");
                            //        }
                            //        break;


                            //    case Atend.Control.Enum.ProductType.GroundCabel:
                            //        //ed.WriteMessage("Sub Is : MiddleCabel :={0}\n", dr["XCode"].ToString());
                            //        Atend.Base.Equipment.EGroundCabel SelectedEquipEMiddleGroundCabel = Atend.Base.Equipment.EGroundCabel.SelectByXCodeForDeign(xCodeProduct, LocalConnection, LocalTransaction);
                            //        if (SelectedEquipEMiddleGroundCabel.Code != -1)
                            //        {
                            //            //Atend.Base.Equipment.EGroundCabel answer = Atend.Base.Equipment.EGroundCabel.AccessSelectByXCode(SelectedEquipEMiddleGroundCabel.XCode, _transaction, _connection);
                            //            //if (answer.Code != -1)
                            //            //{
                            //            //    NewCode = answer.Code;
                            //            //}
                            //            //else
                            //            //{
                            //            if (SelectedEquipEMiddleGroundCabel.Code == 0)
                            //            {//confirm
                            //                if (SelectedEquipEMiddleGroundCabel.ServerInsert(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                {
                            //                    NewCode = SelectedEquipEMiddleGroundCabel.Code;
                            //                    if (!SelectedEquipEMiddleGroundCabel.UpdateX(LocalTransaction, LocalConnection))
                            //                    {
                            //                        throw new System.Exception(" UpdateX  GroundCabel  Failed");
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    throw new System.Exception("ServerInsert failed" + Type.ToString());
                            //                }
                            //            }
                            //            else
                            //            {
                            //                Atend.Base.Equipment.EGroundCabel ServerSelectedGroundCabel = Atend.Base.Equipment.EGroundCabel.ServerSelectByCode(SelectedEquipEMiddleGroundCabel.Code, ServerConnection, ServerTransaction);
                            //                if (ServerSelectedGroundCabel.Code != -1)
                            //                {
                            //                    if (SelectedEquipEMiddleGroundCabel.ServerUpdate(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                    {
                            //                        NewCode = SelectedEquipEMiddleGroundCabel.Code;
                            //                    }
                            //                    else
                            //                    {
                            //                        throw new System.Exception("ServerUpDate failed" + Type.ToString());
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    throw new System.Exception("GroundCabel:There Is No Row on Server For update " + Type.ToString());
                            //                }
                            //            }
                            //            //}
                            //        }
                            //        else
                            //        {
                            //            ed.WriteMessage(Type.ToString() + " does not exist \n");
                            //        }
                            //        break;


                            //    case Atend.Control.Enum.ProductType.GroundCabelTip:
                            //        ed.WriteMessage("Sub Is : MiddleCabel :={0}\n", xCodeProduct);
                            //        Atend.Base.Equipment.EGroundCabelTip SelectedEquipEMiddleGroundCabelTip = Atend.Base.Equipment.EGroundCabelTip.SelectByXCodeForDesign(xCodeProduct, LocalConnection, LocalTransaction);
                            //        if (SelectedEquipEMiddleGroundCabelTip.Code != -1)
                            //        {

                            //            Atend.Base.Equipment.EGroundCabel SelectedMidGroundPhase = Atend.Base.Equipment.EGroundCabel.SelectByXCode(LocalTransaction, LocalConnection, SelectedEquipEMiddleGroundCabelTip.PhaseProductXCode);
                            //            Atend.Base.Equipment.EGroundCabel SeletedMidGroundNeutral = Atend.Base.Equipment.EGroundCabel.SelectByXCode(LocalTransaction, LocalConnection, SelectedEquipEMiddleGroundCabelTip.NeutralProductXCode);
                            //            if (SelectedMidGroundPhase.Code != -1)
                            //            {
                            //                if (SelectedMidGroundPhase.Code == 0)
                            //                {
                            //                    if (SelectedMidGroundPhase.ServerInsert(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                    {
                            //                        NewCode = SelectedMidGroundPhase.Code;
                            //                        if (!SelectedMidGroundPhase.UpdateX(LocalTransaction, LocalConnection))
                            //                            throw new System.Exception("SelectedMidGroundPhase Updatex Failed");
                            //                    }
                            //                    else
                            //                    {
                            //                        throw new System.Exception("SelectedMidGroundPhase ServerInser Failed");
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    Atend.Base.Equipment.EGroundCabel ServerMidGroundPhase = Atend.Base.Equipment.EGroundCabel.ServerSelectByCode(SelectedMidGroundPhase.Code, ServerConnection, ServerTransaction);
                            //                    if (ServerMidGroundPhase.Code != -1)
                            //                    {
                            //                        if (SelectedMidGroundPhase.ServerUpdate(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                        {
                            //                            NewCode = SelectedMidGroundPhase.Code;
                            //                        }
                            //                        else
                            //                        {
                            //                            throw new System.Exception("SelectedMidGroundPhase Server Update Failed");
                            //                        }
                            //                    }
                            //                    else
                            //                    {
                            //                        throw new System.Exception("There Is No Row On Server For Update SelectedMidGroundPhase");
                            //                    }
                            //                }
                            //            }
                            //            else
                            //            {
                            //                throw new System.Exception("Lack Of Data SelectedMidGroundPhase");
                            //            }





                            //            if (SeletedMidGroundNeutral.Code != -1)
                            //            {
                            //                if (SeletedMidGroundNeutral.Code == 0)
                            //                {
                            //                    if (SeletedMidGroundNeutral.ServerInsert(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                    {
                            //                        NewCode = SeletedMidGroundNeutral.Code;
                            //                        if (!SeletedMidGroundNeutral.UpdateX(LocalTransaction, LocalConnection))
                            //                            throw new System.Exception("SeletedMidGroundNeutral Updatex Failed");
                            //                    }
                            //                    else
                            //                    {
                            //                        throw new System.Exception("SeletedMidGroundNeutral ServerInser Failed");
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    Atend.Base.Equipment.EGroundCabel ServerMidGroundNeutral = Atend.Base.Equipment.EGroundCabel.ServerSelectByCode(SeletedMidGroundNeutral.Code, ServerConnection, ServerTransaction);
                            //                    if (ServerMidGroundNeutral.Code != -1)
                            //                    {
                            //                        if (SeletedMidGroundNeutral.ServerUpdate(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                        {
                            //                            NewCode = SeletedMidGroundNeutral.Code;
                            //                        }
                            //                        else
                            //                        {
                            //                            throw new System.Exception("SeletedMidGroundNeutral Server Update Failed");
                            //                        }
                            //                    }
                            //                    else
                            //                    {
                            //                        throw new System.Exception("There Is No Row On Server For Update SeletedMidGroundNeutral");
                            //                    }
                            //                }
                            //            }
                            //            else
                            //            {
                            //                throw new System.Exception("Lack Of Data SeletedMidGroundNeutral");
                            //            }



                            //            SelectedEquipEMiddleGroundCabelTip.PhaseProductCode = SelectedMidGroundPhase.Code;
                            //            SelectedEquipEMiddleGroundCabelTip.NeutralProductCode = SeletedMidGroundNeutral.Code;

                            //            if (SelectedEquipEMiddleGroundCabelTip.Code == 0)
                            //            {//confirm
                            //                if (SelectedEquipEMiddleGroundCabelTip.ServerInsert(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                {
                            //                    NewCode = SelectedEquipEMiddleGroundCabelTip.Code;
                            //                    if (!SelectedEquipEMiddleGroundCabelTip.UpdateX(LocalTransaction, LocalConnection))
                            //                    {
                            //                        throw new System.Exception(" UpdateX  GroundCabel  Failed");
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    throw new System.Exception("ServerInsert failed" + Type.ToString());
                            //                }
                            //            }
                            //            else
                            //            {
                            //                Atend.Base.Equipment.EGroundCabel ServerSelectedGroundCabel = Atend.Base.Equipment.EGroundCabel.ServerSelectByCode(SelectedEquipEMiddleGroundCabelTip.Code, ServerConnection, ServerTransaction);
                            //                if (ServerSelectedGroundCabel.Code != -1)
                            //                {
                            //                    if (SelectedEquipEMiddleGroundCabelTip.ServerUpdate(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                    {
                            //                        NewCode = SelectedEquipEMiddleGroundCabelTip.Code;
                            //                    }
                            //                    else
                            //                    {
                            //                        throw new System.Exception("ServerUpDate failed" + Type.ToString());
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    throw new System.Exception("GroundCabel:There Is No Row on Server For update " + Type.ToString());
                            //                }
                            //            }
                            //            //}
                            //        }
                            //        else
                            //        {
                            //            ed.WriteMessage(Type.ToString() + " does not exist \n");
                            //        }
                            //        break;


                            //    case Atend.Control.Enum.ProductType.GroundPost:
                            //        ed.WriteMessage("Sub Is : GroundPost \n");
                            //        Atend.Base.Equipment.EGroundPost SelectedEquipEGroundPost = Atend.Base.Equipment.EGroundPost.SelectByXCodeForDesign(xCodeProduct, LocalConnection, LocalTransaction);
                            //        if (SelectedEquipEGroundPost.Code != -1)
                            //        {
                            //            if (SelectedEquipEGroundPost.Code == 0)
                            //            {//confirm
                            //                if (SelectedEquipEGroundPost.ServerInsert(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                {
                            //                    NewCode = SelectedEquipEGroundPost.Code;
                            //                    if (!SelectedEquipEGroundPost.UpdateX(LocalTransaction, LocalConnection))
                            //                    {
                            //                        throw new System.Exception(" UpdateX  GroundPost  Failed");
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    throw new System.Exception("ServerInsert failed" + Type.ToString());
                            //                }
                            //            }
                            //            else
                            //            {
                            //                Atend.Base.Equipment.EGroundPost SereverSelectedGroundPost = Atend.Base.Equipment.EGroundPost.ServerSelectByCode(SelectedEquipEGroundPost.Code, ServerConnection, ServerTransaction);
                            //                if (SereverSelectedGroundPost.Code != -1)
                            //                {
                            //                    if (SelectedEquipEGroundPost.ServerUpdate(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                    {
                            //                        NewCode = SelectedEquipEGroundPost.Code;
                            //                    }
                            //                    else
                            //                    {
                            //                        throw new System.Exception("ServerUpdate failed" + Type.ToString());
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    throw new System.Exception("GroundPost:There Is No Row on Server For update " + Type.ToString());
                            //                }
                            //            }
                            //        }
                            //        else
                            //        {
                            //            ed.WriteMessage(Type.ToString() + " does not exist \n");
                            //        }
                            //        break;



                            //    case Atend.Control.Enum.ProductType.Halter:
                            //        ed.WriteMessage("Sub Is : Halter \n");
                            //        Atend.Base.Equipment.EHalter SelectedEquipEHalter = Atend.Base.Equipment.EHalter.SelectByXCodeForDesign(xCodeProduct, LocalConnection, LocalTransaction);
                            //        if (SelectedEquipEHalter.Code != -1)
                            //        {
                            //            //Atend.Base.Equipment.EHalter answer = Atend.Base.Equipment.EHalter.AccessSelectByXCode(SelectedEquipEHalter.XCode, _transaction, _connection);
                            //            //if (answer.Code != -1)
                            //            //{
                            //            //    NewCode = answer.Code;
                            //            //}
                            //            //else
                            //            //{
                            //            if (SelectedEquipEHalter.Code == 0)
                            //            {//confirm
                            //                if (SelectedEquipEHalter.ServerInsert(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                {
                            //                    NewCode = SelectedEquipEHalter.Code;
                            //                    if (!SelectedEquipEHalter.UpdateX(LocalTransaction, LocalConnection))
                            //                    {
                            //                        throw new System.Exception(" UpdateX  Halter  Failed");
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    throw new System.Exception("ServerInsert failed" + Type.ToString());
                            //                }
                            //            }
                            //            else
                            //            {
                            //                Atend.Base.Equipment.EHalter ServerSelectedHalter = Atend.Base.Equipment.EHalter.ServerSelectByCode(SelectedEquipEHalter.Code, ServerConnection, ServerTransaction);
                            //                if (ServerSelectedHalter.Code != -1)
                            //                {
                            //                    if (SelectedEquipEHalter.ServerUpdate(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                    {
                            //                        NewCode = SelectedEquipEHalter.Code;
                            //                    }
                            //                    else
                            //                    {
                            //                        throw new System.Exception("ServerUpdate failed" + Type.ToString());
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    throw new System.Exception("Halter:There Is No Row on Server For update " + Type.ToString());
                            //                }
                            //            }
                            //            //}
                            //        }
                            //        else
                            //        {
                            //            ed.WriteMessage(Type.ToString() + " does not exist \n");
                            //        }
                            //        break;

                            //    //case Atend.Control.Enum.ProductType.Halter:
                            //    //    Atend.Base.Equipment. SelectedEquip = Atend.Base.Equipment.EBreaker.SelectByXCode(XCode);
                            //    //    if (SelectedEquip.Code != -1)
                            //    //    {
                            //    //        if (SelectedEquip.AccessInsert(_transaction, _connection))
                            //    //        {
                            //    //            NewCode = SelectedEquip.Code;
                            //    //        }
                            //    //        else
                            //    //        {
                            //    //            throw new Exception("{0}.AccessInsert failed", ((Atend.Control.Enum.ProductType)Convert.ToInt32(dr["TableType"])).ToString());
                            //    //        }
                            //    //    }
                            //    //    break;
                            //    case Atend.Control.Enum.ProductType.HeaderCabel:
                            //        ed.WriteMessage("Sub Is : HeaderCabel \n");
                            //        Atend.Base.Equipment.EHeaderCabel SelectedEquipEHeaderCabel = Atend.Base.Equipment.EHeaderCabel.SelectByXCodeForDesign(xCodeProduct, LocalConnection, LocalTransaction);
                            //        if (SelectedEquipEHeaderCabel.Code != -1)
                            //        {
                            //            //Atend.Base.Equipment.EHeaderCabel answer = Atend.Base.Equipment.EHeaderCabel.AccessSelectByXCode(SelectedEquipEHeaderCabel.XCode, _transaction, _connection);
                            //            //if (answer.Code != -1)
                            //            //{
                            //            //    NewCode = answer.Code;
                            //            //}
                            //            //else
                            //            //{
                            //            if (SelectedEquipEHeaderCabel.Code == 0)
                            //            {//confirm
                            //                if (SelectedEquipEHeaderCabel.ServerInsert(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                {
                            //                    NewCode = SelectedEquipEHeaderCabel.Code;
                            //                    if (!SelectedEquipEHeaderCabel.UpdateX(LocalTransaction, LocalConnection))
                            //                    {
                            //                        throw new System.Exception(" UpdateX  HeaderCabel  Failed");
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    throw new System.Exception("ServerInsert failed" + Type.ToString());
                            //                }
                            //            }
                            //            else
                            //            {
                            //                Atend.Base.Equipment.EHeaderCabel ServerSelectedHeaderCabel = Atend.Base.Equipment.EHeaderCabel.ServerSelectByCode(SelectedEquipEHeaderCabel.Code, ServerConnection, ServerTransaction);
                            //                if (ServerSelectedHeaderCabel.Code != -1)
                            //                {
                            //                    if (SelectedEquipEHeaderCabel.ServerUpdate(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                    {
                            //                        NewCode = SelectedEquipEHeaderCabel.Code;
                            //                    }
                            //                    else
                            //                    {
                            //                        throw new System.Exception("ServerUpdate failed" + Type.ToString());
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    throw new System.Exception("HeaderCabel:There Is No Row on Server For update " + Type.ToString());
                            //                }
                            //            }
                            //            //}
                            //        }
                            //        else
                            //        {
                            //            ed.WriteMessage(Type.ToString() + " does not exist \n");
                            //        }
                            //        break;
                            //    case Atend.Control.Enum.ProductType.Insulator:
                            //        ed.WriteMessage("Sub Is : Insulator \n");
                            //        Atend.Base.Equipment.EInsulator SelectedEquipEInsulator = Atend.Base.Equipment.EInsulator.SelectByXCodeForDesign(xCodeProduct, LocalConnection, LocalTransaction);
                            //        if (SelectedEquipEInsulator.Code != -1)
                            //        {
                            //            //Atend.Base.Equipment.EInsulator answer = Atend.Base.Equipment.EInsulator.AccessSelectByXCode(SelectedEquipEInsulator.XCode, _transaction, _connection);
                            //            //if (answer.Code != -1)
                            //            //{
                            //            //    NewCode = answer.Code;
                            //            //}
                            //            //else
                            //            //{
                            //            if (SelectedEquipEInsulator.Code == 0)
                            //            {//confirm
                            //                if (SelectedEquipEInsulator.ServerInsert(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                {
                            //                    NewCode = SelectedEquipEInsulator.Code;
                            //                    if (!SelectedEquipEInsulator.UpdateX(LocalTransaction, LocalConnection))
                            //                    {
                            //                        throw new System.Exception(" UpdateX    Failed");
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    throw new System.Exception("ServerInsert failed" + Type.ToString());
                            //                }
                            //            }
                            //            else
                            //            {
                            //                Atend.Base.Equipment.EInsulator ServerSelectedInsulator = Atend.Base.Equipment.EInsulator.ServerSelectByCode(SelectedEquipEInsulator.Code, ServerConnection, ServerTransaction);
                            //                if (ServerSelectedInsulator.Code != -1)
                            //                {
                            //                    if (SelectedEquipEInsulator.ServerUpdate(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                    {
                            //                        NewCode = SelectedEquipEInsulator.Code;
                            //                    }
                            //                    else
                            //                    {
                            //                        throw new System.Exception("Server Update failed" + Type.ToString());
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    throw new System.Exception("Insulator:There Is No Row on Server For update " + Type.ToString());
                            //                }
                            //            }
                            //            //}
                            //        }
                            //        else
                            //        {
                            //            ed.WriteMessage(Type.ToString() + " does not exist \n");
                            //        }
                            //        break;


                            //    case Atend.Control.Enum.ProductType.InsulatorChain:
                            //        ed.WriteMessage("Sub Is : InsulatorChain \n");
                            //        Atend.Base.Equipment.EInsulatorChain SelectedEquipEInsulatorChain = Atend.Base.Equipment.EInsulatorChain.SelectByXCodeForDesign(xCodeProduct, LocalConnection, LocalTransaction);
                            //        if (SelectedEquipEInsulatorChain.Code != -1)
                            //        {
                            //            //Atend.Base.Equipment.EInsulatorChain answer = Atend.Base.Equipment.EInsulatorChain.AccessSelectByXCode(SelectedEquipEInsulatorChain.XCode, _transaction, _connection);
                            //            //if (answer.Code != -1)
                            //            //{
                            //            //    NewCode = answer.Code;
                            //            //}
                            //            //else
                            //            //{
                            //            if (SelectedEquipEInsulatorChain.Code == 0)
                            //            {//confirm
                            //                if (SelectedEquipEInsulatorChain.ServerInsert(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                {
                            //                    NewCode = SelectedEquipEInsulatorChain.Code;
                            //                    if (!SelectedEquipEInsulatorChain.UpdateX(LocalTransaction, LocalConnection))
                            //                    {
                            //                        throw new System.Exception(" UpdateX  InsulatorChain  Failed");
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    throw new System.Exception("ServerInsert failed" + Type.ToString());
                            //                }
                            //            }
                            //            else
                            //            {
                            //                Atend.Base.Equipment.EInsulatorChain ServerSelectedInsulatorChain = Atend.Base.Equipment.EInsulatorChain.ServerSelectByCode(SelectedEquipEInsulatorChain.Code, ServerConnection, ServerTransaction);
                            //                if (ServerSelectedInsulatorChain.Code != -1)
                            //                {

                            //                    if (SelectedEquipEInsulatorChain.ServerUpdate(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                    {
                            //                        NewCode = SelectedEquipEInsulatorChain.Code;
                            //                    }
                            //                    else
                            //                    {
                            //                        throw new System.Exception("ServerUpdate failed" + Type.ToString());
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    throw new System.Exception("InsulatorChain:There Is No Row on Server For update " + Type.ToString());
                            //                }
                            //            }
                            //            //}
                            //        }
                            //        else
                            //        {
                            //            ed.WriteMessage(Type.ToString() + " does not exist \n");
                            //        }
                            //        break;

                            //    case Atend.Control.Enum.ProductType.InsulatorPipe:
                            //        ed.WriteMessage("Sub Is : InsulatorPipe \n");
                            //        Atend.Base.Equipment.EInsulatorPipe SelectedEquipEInsulatorPipe = Atend.Base.Equipment.EInsulatorPipe.SelectByXCodeForDesign(xCodeProduct, LocalConnection, LocalTransaction);
                            //        if (SelectedEquipEInsulatorPipe.Code != -1)
                            //        {
                            //            //Atend.Base.Equipment.EInsulatorPipe answer = Atend.Base.Equipment.EInsulatorPipe.AccessSelectByXCode(SelectedEquipEInsulatorPipe.XCode, _transaction, _connection);
                            //            //if (answer.Code != -1)
                            //            //{
                            //            //    NewCode = answer.Code;
                            //            //}
                            //            //else
                            //            //{
                            //            if (SelectedEquipEInsulatorPipe.Code == 0)
                            //            {//confirm
                            //                if (SelectedEquipEInsulatorPipe.ServerInsert(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                {
                            //                    NewCode = SelectedEquipEInsulatorPipe.Code;
                            //                    if (!SelectedEquipEInsulatorPipe.UpdateX(LocalTransaction, LocalConnection))
                            //                    {
                            //                        throw new System.Exception(" UpdateX  InsulatorPipe  Failed");
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    throw new System.Exception("ServerInsert failed" + Type.ToString());
                            //                }
                            //            }
                            //            else
                            //            {
                            //                Atend.Base.Equipment.EInsulatorPipe ServerSelectedInsulatorPipe = Atend.Base.Equipment.EInsulatorPipe.ServerSelectByCode(SelectedEquipEInsulatorPipe.Code, ServerConnection, ServerTransaction);
                            //                if (ServerSelectedInsulatorPipe.Code != 0)
                            //                {
                            //                    if (SelectedEquipEInsulatorPipe.ServerUpdate(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                    {
                            //                        NewCode = SelectedEquipEInsulatorPipe.Code;
                            //                    }
                            //                    else
                            //                    {
                            //                        throw new System.Exception("ServerUpdate failed" + Type.ToString());
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    throw new System.Exception("InsulatorPipe:There Is No Row OnServer For Update " + Type.ToString());
                            //                }
                            //            }
                            //            //}
                            //        }
                            //        else
                            //        {
                            //            ed.WriteMessage(Type.ToString() + " does not exist \n");
                            //        }
                            //        break;


                            //    case Atend.Control.Enum.ProductType.MiddleJackPanel:
                            //        ////ed.WriteMessage("AAA : {0} \n", new Guid(dr["XCode"].ToString()));
                            //        ed.WriteMessage("Sub Is : MiddleJackPanel \n");
                            //        Atend.Base.Equipment.EJAckPanel SelectedEquipEJAckPanel = Atend.Base.Equipment.EJAckPanel.SelectByXCodeForDesign(xCodeProduct, LocalConnection, LocalTransaction);
                            //        if (SelectedEquipEJAckPanel.Code != -1)
                            //        {
                            //            //Atend.Base.Equipment.EJAckPanel answer = Atend.Base.Equipment.EJAckPanel.AccessSelectByXCode(SelectedEquipEJAckPanel.XCode, _transaction, _connection);
                            //            //if (answer.Code != -1)
                            //            //{
                            //            //    NewCode = answer.Code;
                            //            //}
                            //            //else
                            //            //{

                            //            Atend.Base.Equipment.EBus _eBus = Atend.Base.Equipment.EBus.SelectByXCodeForDesign(SelectedEquipEJAckPanel.MasterProductXCode, LocalConnection, LocalTransaction);
                            //            if (_eBus.Code != -1)
                            //            {
                            //                if (_eBus.Code == 0)
                            //                {
                            //                    if (!_eBus.ServerInsert(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                    {
                            //                        throw new System.Exception("ServerInsert failed" + Type.ToString() + ":Bus");
                            //                    }
                            //                    else
                            //                    {

                            //                        if (!_eBus.UpdateX(LocalTransaction, LocalConnection))
                            //                        {
                            //                            throw new System.Exception(" UpdateX  eBus  Failed");
                            //                        }



                            //                    }
                            //                }
                            //                else
                            //                {

                            //                    Atend.Base.Equipment.EBus SeverSelectedBus = Atend.Base.Equipment.EBus.ServerSelectByCode(_eBus.Code, ServerConnection, ServerTransaction);
                            //                    if (SeverSelectedBus.Code != -1)
                            //                    {
                            //                        if (!_eBus.ServerUpdate(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                        {
                            //                            throw new System.Exception("ServerUpdate failed" + Type.ToString() + ":Bus");
                            //                        }
                            //                    }
                            //                    else
                            //                    {
                            //                        throw new System.Exception("MiddleJackPanel:There is No row On server For Update " + Type.ToString());
                            //                    }
                            //                }
                            //            }
                            //            else
                            //            {
                            //                throw new System.Exception("Lack of data in MiddleJackPanel:Bus");
                            //            }

                            //            SelectedEquipEJAckPanel.MasterProductCode = _eBus.Code;
                            //            if (SelectedEquipEJAckPanel.Code == 0)
                            //            {
                            //                //confirm
                            //                if (SelectedEquipEJAckPanel.ServerInsert(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                {
                            //                    NewCode = SelectedEquipEJAckPanel.Code;

                            //                    if (!SelectedEquipEJAckPanel.UpdateX(LocalTransaction, LocalConnection))
                            //                    {
                            //                        throw new System.Exception(" UpdateX    Failed");
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    throw new System.Exception("Serevr Insert Failed in MJackPanel");
                            //                }
                            //            }
                            //            else
                            //            {

                            //                Atend.Base.Equipment.EJAckPanel ServerSelectedJackPanel = Atend.Base.Equipment.EJAckPanel.ServerSelectByCode(SelectedEquipEJAckPanel.Code, ServerConnection, ServerTransaction);
                            //                if (ServerSelectedJackPanel.Code != -1)
                            //                {
                            //                    if (SelectedEquipEJAckPanel.ServerUpdate(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                    {
                            //                        NewCode = SelectedEquipEJAckPanel.Code;
                            //                    }
                            //                    else
                            //                    {
                            //                        throw new System.Exception("Serevr Update Failed in MJackPanel");
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    throw new System.Exception("There is No row On server For Update " + Type.ToString());
                            //                }
                            //            }

                            //            //if (SelectedEquipEJAckPanel.ServerInsert(_transaction, _connection, true, false))
                            //            //{
                            //            //    NewCode = SelectedEquipEJAckPanel.Code;
                            //            DataTable _JPS = Atend.Base.Equipment.EJackPanelCell.SelectByJackPanelXCode(LocalConnection, LocalTransaction, SelectedEquipEJAckPanel.XCode);
                            //            if (!Atend.Base.Equipment.EJackPanelCell.Delete(ServerTransaction, ServerConnection, SelectedEquipEJAckPanel.Code))
                            //            {
                            //                throw new System.Exception("Error in Delete JackPanelCell in Share Onserver");
                            //            }

                            //            foreach (DataRow _JPSdr in _JPS.Rows)
                            //            {
                            //                Atend.Base.Equipment.ECell _eCell = Atend.Base.Equipment.ECell.SelectByXCodeForDesign(new Guid(_JPSdr["ProductXCode"].ToString()), LocalConnection, LocalTransaction);
                            //                if (_eCell.Code != -1)
                            //                {
                            //                    if (_eCell.Code == 0)
                            //                    {
                            //                        if (!_eCell.ServerInsert(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                        {
                            //                            throw new System.Exception("Server Insert failed" + Type.ToString() + ":Cell");
                            //                        }
                            //                        else
                            //                        {

                            //                            if (!_eCell.UpdateX(LocalTransaction, LocalConnection))
                            //                            {
                            //                                throw new System.Exception(" UpdateX Cell   Failed");
                            //                            }

                            //                        }
                            //                    }
                            //                    else
                            //                    {
                            //                        Atend.Base.Equipment.ECell ServerSelectedCell = Atend.Base.Equipment.ECell.ServerSelectByCode(_eCell.Code, ServerConnection, ServerTransaction);
                            //                        if (ServerSelectedCell.Code != -1)
                            //                        {
                            //                            if (!_eCell.ServerUpdate(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                            {
                            //                                throw new System.Exception("ServerUpdate failed" + Type.ToString() + ":Cell");
                            //                            }
                            //                        }
                            //                        else
                            //                        {
                            //                            throw new System.Exception("cellINjackpanel:There is No row On server For Update " + Type.ToString());
                            //                        }
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    throw new System.Exception("Lack of data in MiddleJackPanel:Cell");
                            //                }
                            //                Atend.Base.Equipment.EJackPanelCell _EJC = Atend.Base.Equipment.EJackPanelCell.SelectByXCode(new Guid(_JPSdr["XCode"].ToString()), LocalConnection, LocalTransaction);

                            //                if (_EJC.Code != -1)
                            //                {
                            //                    _EJC.ProductCode = _eCell.Code;
                            //                    _EJC.JackPanelCode = SelectedEquipEJAckPanel.Code;
                            //                    //if (_EJC.Code == 0)
                            //                    //{
                            //                    if (!_EJC.ServerInsert(ServerTransaction, ServerConnection, false, false, LocalTransaction, LocalConnection))
                            //                    {
                            //                        throw new System.Exception("ServerInsert failed" + Type.ToString() + ":JackPanelCell");
                            //                    }
                            //                    else
                            //                    {
                            //                        if (!_EJC.UpdateX(LocalConnection, LocalTransaction))
                            //                        {
                            //                            throw new System.Exception(" UpdateX    Failed");
                            //                        }
                            //                    }
                            //                    //}
                            //                    //else
                            //                    //{
                            //                    //    Atend.Base.Equipment.EJackPanelCell ServerSelectedJackPanelCell = Atend.Base.Equipment.EJackPanelCell.ServerSelectByCode(_EJC.Code, ServerConnection, ServerTransaction);
                            //                    //    if (ServerSelectedJackPanelCell.Code != -1)
                            //                    //    {
                            //                    //        if (!_EJC.ServerUpdate(ServerTransaction, ServerConnection, false, false, LocalTransaction, LocalConnection))
                            //                    //        {
                            //                    //            throw new System.Exception("Server Update failed" + Type.ToString() + ":JackPanelCell");
                            //                    //        }
                            //                    //    }
                            //                    //    else
                            //                    //    {
                            //                    //        throw new System.Exception("JackPanelCell:There Is no Row On server For Update " + Type.ToString());
                            //                    //    }
                            //                    //}
                            //                }
                            //                else
                            //                {
                            //                    throw new System.Exception("Lack of data in MiddleJackPanel:JackPanelCell");
                            //                }
                            //            }
                            //        }
                            //        else
                            //        {
                            //            throw new System.Exception("Lack Of DAta in MidJackPanel");
                            //        }
                            //        //}
                            //        //}
                            //        break;


                            //    case Atend.Control.Enum.ProductType.WeekJackPanel:
                            //        ed.WriteMessage("Sub Is : WeekJackPanel \n");
                            //        Atend.Base.Equipment.EJackPanelWeek SelectedEquipWeekJackPanel = Atend.Base.Equipment.EJackPanelWeek.SelectByXCodeForDesign(xCodeProduct, LocalConnection, LocalTransaction);
                            //        if (SelectedEquipWeekJackPanel.Code != -1)
                            //        {


                            //            Atend.Base.Equipment.EBus _eBus = Atend.Base.Equipment.EBus.SelectByXCodeForDesign(SelectedEquipWeekJackPanel.BusXCode, LocalConnection, LocalTransaction);
                            //            if (_eBus.Code != -1)
                            //            {
                            //                if (_eBus.Code == 0)
                            //                {
                            //                    if (!_eBus.ServerInsert(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                    {
                            //                        throw new System.Exception("Server Insert failed" + Type.ToString() + ":Bus");
                            //                    }
                            //                    else
                            //                    {
                            //                        if (!_eBus.UpdateX(LocalTransaction, LocalConnection))
                            //                        {
                            //                            throw new System.Exception(" UpdateX  Bus  Failed");
                            //                        }
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    Atend.Base.Equipment.EBus ServerSelectedBus = Atend.Base.Equipment.EBus.ServerSelectByCode(_eBus.Code, ServerConnection, ServerTransaction);
                            //                    if (ServerSelectedBus.Code != -1)
                            //                    {
                            //                        if (!_eBus.ServerUpdate(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                        {
                            //                            throw new System.Exception("Server Update failed" + Type.ToString() + ":Bus");
                            //                        }
                            //                    }
                            //                    else
                            //                    {
                            //                        throw new System.Exception("WeekJackPanel:There is No row On server For Update " + Type.ToString());
                            //                    }
                            //                }
                            //            }
                            //            else
                            //            {
                            //                throw new System.Exception("Lack of data in WeekJackPanel:Bus");
                            //            }
                            //            SelectedEquipWeekJackPanel.BusCode = _eBus.Code;

                            //            Atend.Base.Equipment.EAutoKey_3p _Key = Atend.Base.Equipment.EAutoKey_3p.SelectByXCodeForDesign(SelectedEquipWeekJackPanel.AutoKey3pXCode, LocalConnection, LocalTransaction);
                            //            //ed.WriteMessage("1\n");

                            //            if (_Key.Code != -1)
                            //            {
                            //                //ed.WriteMessage("2\n");
                            //                if (_Key.Code == 0)
                            //                {
                            //                    if (!_Key.ServerInsert(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                    {
                            //                        //ed.WriteMessage("3\n");
                            //                        throw new System.Exception("Server Insert failed" + Type.ToString() + ":EAutoKey_3p");
                            //                    }
                            //                    else
                            //                    {
                            //                        if (!_Key.UpdateX(LocalTransaction, LocalConnection))
                            //                        {
                            //                            throw new System.Exception(" UpdateX  Key  Failed");
                            //                        }
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    Atend.Base.Equipment.EAutoKey_3p ServerSelectedAutokeyAuto3p = Atend.Base.Equipment.EAutoKey_3p.ServerSelectByCode(_Key.Code, ServerConnection, ServerTransaction);
                            //                    if (ServerSelectedAutokeyAuto3p.Code != -1)
                            //                    {
                            //                        if (!_Key.ServerUpdate(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                        {
                            //                            //ed.WriteMessage("3\n");
                            //                            throw new System.Exception("Server Update failed" + Type.ToString() + ":EAutoKey_3p");
                            //                        }
                            //                    }

                            //                    else
                            //                    {
                            //                        throw new System.Exception("AutoKey_3pINpanel:There is No row On server For Update " + Type.ToString());
                            //                    }
                            //                }
                            //                //ed.WriteMessage("4\n");
                            //            }
                            //            else
                            //            {
                            //                throw new System.Exception("Lack of data in WeekJackPanel:EAutoKey_3p");
                            //            }
                            //            //ed.WriteMessage("insert passed \n");
                            //            SelectedEquipWeekJackPanel.AutoKey3pCode = _Key.Code;
                            //            if (SelectedEquipWeekJackPanel.Code == 0)
                            //            {
                            //                if (SelectedEquipWeekJackPanel.ServerInsert(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                {
                            //                    NewCode = SelectedEquipWeekJackPanel.Code;
                            //                    if (!SelectedEquipWeekJackPanel.UpdateX(LocalTransaction, LocalConnection))
                            //                    {
                            //                        throw new System.Exception(" UpdateX  WeekJackPanel  Failed");
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    throw new System.Exception("Server Insert Failed " + Type.ToString());
                            //                }
                            //            }
                            //            else
                            //            {
                            //                //Atend.Base.Equipment.EJackPanelWeek ServerSelectedWeekJackPanel=Atend.Base.Equipment.EJackPanelWeek.ServerSelectByXCode
                            //                if (SelectedEquipWeekJackPanel.ServerUpdate(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                {
                            //                    NewCode = SelectedEquipWeekJackPanel.Code;
                            //                }
                            //                else
                            //                {
                            //                    throw new System.Exception("ServerUpdate failed " + Type.ToString());
                            //                }
                            //            }
                            //            //if (SelectedEquipWeekJackPanel.ServerInsert(_transaction, _connection, true, false))
                            //            //{
                            //            //    NewCode = SelectedEquipWeekJackPanel.Code;

                            //            DataTable _WJS = Atend.Base.Equipment.EJackPanelWeekCell.SelectByJackPanelWeekXCode(LocalTransaction, LocalConnection, SelectedEquipWeekJackPanel.XCode);

                            //            if (!Atend.Base.Equipment.EJackPanelWeekCell.Delete(ServerTransaction, ServerConnection, SelectedEquipWeekJackPanel.Code))
                            //            {
                            //                throw new System.Exception("Error in Delete EJackPanelWeekCell in Share On Server");
                            //            }

                            //            foreach (DataRow _WJSdr in _WJS.Rows)
                            //            {
                            //                //ed.WriteMessage("go for cells \n");
                            //                Atend.Base.Equipment.EJackPanelWeekCell _WJC = Atend.Base.Equipment.EJackPanelWeekCell.SelectByXCodeForDesign(new Guid(_WJSdr["XCode"].ToString()), LocalConnection, LocalTransaction);
                            //                if (_WJC.Code != -1)
                            //                {
                            //                    _WJC.JackPanelWeekCode = SelectedEquipWeekJackPanel.Code;
                            //                    //if (_WJC.Code == 0)
                            //                    //{
                            //                    if (!_WJC.ServerInsert(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                    {
                            //                        throw new System.Exception("Server Insert failed" + Type.ToString() + ":WeekJackPanelCell");
                            //                    }
                            //                    else
                            //                    {
                            //                        //if (!)
                            //                        //{
                            //                        //    throw new System.Exception("UpdateX      Failed");
                            //                        //}
                            //                    }
                            //                    //}
                            //                    //else
                            //                    //{
                            //                    //    Atend.Base.Equipment.EJackPanelWeekCell ServerSelectedJackPanelWeekCell = Atend.Base.Equipment.EJackPanelWeekCell.ServerSelectByCode(_WJC.Code, ServerConnection, ServerTransaction);
                            //                    //    if (ServerSelectedJackPanelWeekCell.Code != -1)
                            //                    //    {
                            //                    //        if (!_WJC.ServerUpdate(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                    //        {
                            //                    //            throw new System.Exception("Server Update failed" + Type.ToString() + ":WeekJackPanelCell");
                            //                    //        }
                            //                    //    }
                            //                    //    else
                            //                    //    {
                            //                    //        throw new System.Exception("JackPanelWeekCellINpanel:There is No row On server For Update " + Type.ToString());
                            //                    //    }
                            //                    //}
                            //                }
                            //                else
                            //                {
                            //                    throw new System.Exception("Lack of data in WeekJackPanel:WeekJackPanelCell");
                            //                }
                            //            }

                            //        }
                            //        else
                            //        {
                            //            throw new System.Exception("Lack Of Data In Week JackPanel");
                            //        }
                            //        //}
                            //        break;


                            //    case Atend.Control.Enum.ProductType.KablSho:
                            //        ed.WriteMessage("Sub Is : KablSho \n");
                            //        Atend.Base.Equipment.EKablsho SelectedEquipEKablsho = Atend.Base.Equipment.EKablsho.SelectByXCodeForDesign(xCodeProduct, LocalConnection, LocalTransaction);
                            //        if (SelectedEquipEKablsho.Code != -1)
                            //        {
                            //            //Atend.Base.Equipment.EKablsho answer = Atend.Base.Equipment.EKablsho.AccessSelectByXCode(SelectedEquipEKablsho.XCode, _transaction, _connection);
                            //            //if (answer.Code != -1)
                            //            //{
                            //            //    NewCode = answer.Code;
                            //            //}
                            //            //else
                            //            //{
                            //            if (SelectedEquipEKablsho.Code == 0)
                            //            {//confirm
                            //                if (SelectedEquipEKablsho.ServerInsert(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                {
                            //                    NewCode = SelectedEquipEKablsho.Code;
                            //                    if (!SelectedEquipEKablsho.UpdateX(LocalTransaction, LocalConnection))
                            //                    {
                            //                        throw new System.Exception("UpdateX   Kablsho   Failed");
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    throw new System.Exception("Server Insert failed" + Type.ToString());
                            //                }
                            //            }
                            //            else
                            //            {
                            //                Atend.Base.Equipment.EKablsho ServerSelectedKablsho = Atend.Base.Equipment.EKablsho.ServerSelectByCode(SelectedEquipEKablsho.Code, ServerConnection, ServerTransaction);
                            //                if (ServerSelectedKablsho.Code != -1)
                            //                {
                            //                    if (SelectedEquipEKablsho.ServerUpdate(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                    {
                            //                        NewCode = SelectedEquipEKablsho.Code;
                            //                    }
                            //                    else
                            //                    {
                            //                        throw new System.Exception("Server Update failed" + Type.ToString());
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    throw new System.Exception("KablSho:There is No row On server For Update " + Type.ToString());
                            //                }
                            //            }
                            //            //}
                            //        }
                            //        else
                            //        {
                            //            ed.WriteMessage(Type.ToString() + " does not exist \n");
                            //        }
                            //        break;
                            //    case Atend.Control.Enum.ProductType.Kalamp:
                            //        ed.WriteMessage("Sub Is : Kalamp \n");
                            //        Atend.Base.Equipment.EClamp SelectedEquipEClamp = Atend.Base.Equipment.EClamp.SelectByXCodeForDesign(xCodeProduct, LocalConnection, LocalTransaction);
                            //        if (SelectedEquipEClamp.Code != -1)
                            //        {
                            //            //Atend.Base.Equipment.EClamp answer = Atend.Base.Equipment.EClamp.AccessSelectByXCode(SelectedEquipEClamp.XCode, _transaction, _connection);
                            //            //if (answer.Code != -1)
                            //            //{
                            //            //    NewCode = answer.Code;
                            //            //}
                            //            //else
                            //            //{
                            //            if (SelectedEquipEClamp.Code == 0)
                            //            {//confirm
                            //                if (SelectedEquipEClamp.ServerInsert(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                {
                            //                    NewCode = SelectedEquipEClamp.Code;
                            //                    if (!SelectedEquipEClamp.UpdateX(LocalTransaction, LocalConnection))
                            //                    {
                            //                        throw new System.Exception("UpdateX   Calamp   Failed");
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    throw new System.Exception("Server Insert failed" + Type.ToString());
                            //                }
                            //            }
                            //            else
                            //            {
                            //                Atend.Base.Equipment.EClamp ServerSelectedCalamp = Atend.Base.Equipment.EClamp.ServerSelectByCode(SelectedEquipEClamp.Code, ServerConnection, ServerTransaction);
                            //                if (ServerSelectedCalamp.Code != -1)
                            //                {
                            //                    if (SelectedEquipEClamp.ServerUpdate(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                    {
                            //                        NewCode = SelectedEquipEClamp.Code;
                            //                    }
                            //                    else
                            //                    {
                            //                        throw new System.Exception("Server Update failed" + Type.ToString());
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    throw new System.Exception("Kalamp:There is No row On server For Update " + Type.ToString());
                            //                }
                            //            }
                            //            //}
                            //        }
                            //        else
                            //        {
                            //            ed.WriteMessage(Type.ToString() + " does not exist \n");
                            //        }
                            //        break;
                            //    case Atend.Control.Enum.ProductType.Khazan:
                            //        ed.WriteMessage("Sub Is : Khazan \n");
                            //        Atend.Base.Equipment.EKhazan SelectedEquipEKhazan = Atend.Base.Equipment.EKhazan.SelectByXCodeForDesign(xCodeProduct, LocalConnection, LocalTransaction);
                            //        if (SelectedEquipEKhazan.Code != -1)
                            //        {
                            //            //Atend.Base.Equipment.EKhazan answer = Atend.Base.Equipment.EKhazan.AccessSelectByXCode(SelectedEquipEKhazan.XCode, _transaction, _connection);
                            //            //if (answer.Code != -1)
                            //            //{
                            //            //    NewCode = answer.Code;
                            //            //}
                            //            //else
                            //            //{
                            //            if (SelectedEquipEKhazan.Code == 0)
                            //            {//confirm
                            //                if (SelectedEquipEKhazan.ServerInsert(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                {
                            //                    NewCode = SelectedEquipEKhazan.Code;
                            //                    if (!SelectedEquipEKhazan.UpdateX(LocalTransaction, LocalConnection))
                            //                    {
                            //                        throw new System.Exception("UpdateX  Khazan    Failed");
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    throw new System.Exception("Server Insert failed" + Type.ToString());
                            //                }
                            //            }
                            //            else
                            //            {
                            //                Atend.Base.Equipment.EKhazan ServerSelectedKhazan = Atend.Base.Equipment.EKhazan.ServerSelectByCode(SelectedEquipEKhazan.Code, ServerConnection, ServerTransaction);
                            //                if (ServerSelectedKhazan.Code != -1)
                            //                {
                            //                    if (SelectedEquipEKhazan.ServerUpdate(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                    {
                            //                        NewCode = SelectedEquipEKhazan.Code;
                            //                    }
                            //                    else
                            //                    {
                            //                        throw new System.Exception("Server Update failed" + Type.ToString());
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    throw new System.Exception("Khazan:There is No row On server For Update " + Type.ToString());
                            //                }
                            //            }
                            //            //}
                            //        }
                            //        else
                            //        {
                            //            ed.WriteMessage(Type.ToString() + " does not exist \n");
                            //        }
                            //        break;


                            //    case Atend.Control.Enum.ProductType.BankKhazan:
                            //        ed.WriteMessage("Sub Is : Khazan \n");
                            //        Atend.Base.Equipment.EKhazanTip SelectedEquipEKhazanTip = Atend.Base.Equipment.EKhazanTip.SelectByXCodeForDesign(xCodeProduct, LocalConnection, LocalTransaction);
                            //        if (SelectedEquipEKhazanTip.Code != -1)
                            //        {
                            //            //Atend.Base.Equipment.EKhazan answer = Atend.Base.Equipment.EKhazan.AccessSelectByXCode(SelectedEquipEKhazan.XCode, _transaction, _connection);
                            //            //if (answer.Code != -1)
                            //            //{
                            //            //    NewCode = answer.Code;
                            //            //}
                            //            //else
                            //            //{
                            //            if (SelectedEquipEKhazanTip.Code == 0)
                            //            {//confirm
                            //                if (SelectedEquipEKhazanTip.ServerInsert(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                {
                            //                    NewCode = SelectedEquipEKhazanTip.Code;
                            //                    if (!SelectedEquipEKhazanTip.UpdateX(LocalTransaction, LocalConnection))
                            //                    {
                            //                        throw new System.Exception("UpdateX  Khazan    Failed");
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    throw new System.Exception("Server Insert failed" + Type.ToString());
                            //                }
                            //            }
                            //            else
                            //            {
                            //                Atend.Base.Equipment.EKhazanTip ServerSelectedKhazanTip = Atend.Base.Equipment.EKhazanTip.ServerSelectByCode(SelectedEquipEKhazanTip.Code, ServerConnection, ServerTransaction);
                            //                if (ServerSelectedKhazanTip.Code != -1)
                            //                {
                            //                    if (SelectedEquipEKhazanTip.ServerUpdate(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                    {
                            //                        NewCode = SelectedEquipEKhazanTip.Code;
                            //                    }
                            //                    else
                            //                    {
                            //                        throw new System.Exception("Server Update failed" + Type.ToString());
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    throw new System.Exception("Khazan:There is No row On server For Update " + Type.ToString());
                            //                }
                            //            }
                            //            //}
                            //        }
                            //        else
                            //        {
                            //            ed.WriteMessage(Type.ToString() + " does not exist \n");
                            //        }
                            //        break;




                            //    case Atend.Control.Enum.ProductType.Light:
                            //        ed.WriteMessage("Sub Is : Light \n");
                            //        Atend.Base.Equipment.ELight SelectedEquipELight = Atend.Base.Equipment.ELight.SelectByXCodeForDesign(xCodeProduct, LocalConnection, LocalTransaction);
                            //        if (SelectedEquipELight.Code != -1)
                            //        {
                            //            //Atend.Base.Equipment.ELight answer = Atend.Base.Equipment.ELight.AccessSelectByXCode(SelectedEquipELight.XCode, _transaction, _connection);
                            //            //if (answer.Code != -1)
                            //            //{
                            //            //    NewCode = answer.Code;
                            //            //}
                            //            //else
                            //            //{
                            //            if (SelectedEquipELight.Code == 0)
                            //            {//confirm
                            //                if (SelectedEquipELight.ServerInsert(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                {
                            //                    NewCode = SelectedEquipELight.Code;
                            //                    if (!SelectedEquipELight.UpdateX(LocalTransaction, LocalConnection))
                            //                    {
                            //                        throw new System.Exception("UpdateX  Light  Failed");
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    throw new System.Exception("Server Insert failed" + Type.ToString());
                            //                }
                            //            }
                            //            else
                            //            {
                            //                Atend.Base.Equipment.ELight ServerSelectedLight = Atend.Base.Equipment.ELight.ServerSelectByCode(SelectedEquipELight.Code, ServerConnection, ServerTransaction);
                            //                if (ServerSelectedLight.Code != -1)
                            //                {
                            //                    if (SelectedEquipELight.ServerUpdate(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                    {
                            //                        NewCode = SelectedEquipELight.Code;
                            //                    }
                            //                    else
                            //                    {
                            //                        throw new System.Exception("Server Update failed" + Type.ToString());
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    throw new System.Exception("Light:There is No row On server For Update " + Type.ToString());
                            //                }
                            //            }
                            //            //}
                            //        }
                            //        else
                            //        {
                            //            ed.WriteMessage(Type.ToString() + " does not exist \n");
                            //        }
                            //        break;

                            //    case Atend.Control.Enum.ProductType.Mafsal:
                            //        ed.WriteMessage("Sub Is : Mafsal \n");
                            //        Atend.Base.Equipment.EMafsal SelectedEquipEMafsal = Atend.Base.Equipment.EMafsal.SelectByXCodeForDesign(xCodeProduct, LocalConnection, LocalTransaction);
                            //        if (SelectedEquipEMafsal.Code != -1)
                            //        {
                            //            //Atend.Base.Equipment.EMafsal answer = Atend.Base.Equipment.EMafsal.AccessSelectByXCode(SelectedEquipEMafsal.XCode, _transaction, _connection);
                            //            //if (answer.Code != -1)
                            //            //{
                            //            //    NewCode = answer.Code;
                            //            //}
                            //            //else
                            //            //{
                            //            if (SelectedEquipEMafsal.Code == 0)
                            //            {//confirm
                            //                if (SelectedEquipEMafsal.ServerInsert(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                {
                            //                    NewCode = SelectedEquipEMafsal.Code;
                            //                    if (!SelectedEquipEMafsal.UpdateX(LocalTransaction, LocalConnection))
                            //                    {
                            //                        throw new System.Exception("UpdateX  Mafsal Failed");
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    throw new System.Exception("Server Insert failed" + Type.ToString());
                            //                }
                            //            }
                            //            else
                            //            {
                            //                Atend.Base.Equipment.EMafsal ServerSelectedMafsal = Atend.Base.Equipment.EMafsal.ServerSelectByCode(SelectedEquipEMafsal.Code, ServerConnection, ServerTransaction);
                            //                if (ServerSelectedMafsal.Code != -1)
                            //                {
                            //                    if (SelectedEquipEMafsal.ServerUpdate(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                    {
                            //                        NewCode = SelectedEquipEMafsal.Code;
                            //                    }
                            //                    else
                            //                    {
                            //                        throw new System.Exception("Server Update failed" + Type.ToString());
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    throw new System.Exception("Mafsal:There is No row On server For Update " + Type.ToString());
                            //                }
                            //            }
                            //            //}
                            //        }
                            //        else
                            //        {
                            //            ed.WriteMessage(Type.ToString() + " does not exist \n");
                            //        }
                            //        break;

                            //    case Atend.Control.Enum.ProductType.MeasuredJackPanel:
                            //        ed.WriteMessage("Sub Is : MeasuredJackPanel \n");
                            //        Atend.Base.Equipment.EMeasuredJackPanel SelectedEquipEMeasuredJackPanel = Atend.Base.Equipment.EMeasuredJackPanel.SelectByXCodeForDesign(xCodeProduct, LocalConnection, LocalTransaction);
                            //        if (SelectedEquipEMeasuredJackPanel.Code != -1)
                            //        {
                            //            //Atend.Base.Equipment.EMeasuredJackPanel answer = Atend.Base.Equipment.EMeasuredJackPanel.AccessSelectByXCode(SelectedEquipEMeasuredJackPanel.XCode, _transaction, _connection);
                            //            //if (answer.Code != -1)
                            //            //{
                            //            //    NewCode = answer.Code;
                            //            //}
                            //            //else
                            //            //{
                            //            if (SelectedEquipEMeasuredJackPanel.Code == 0)
                            //            {//confirm
                            //                if (SelectedEquipEMeasuredJackPanel.ServerInsert(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                {
                            //                    NewCode = SelectedEquipEMeasuredJackPanel.Code;
                            //                    if (!SelectedEquipEMeasuredJackPanel.UpdateX(LocalTransaction, LocalConnection))
                            //                    {
                            //                        throw new System.Exception("UpdateX MeasureJackPAnel  Failed");
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    throw new System.Exception("Server Insert failed" + Type.ToString());
                            //                }
                            //            }
                            //            else
                            //            {
                            //                Atend.Base.Equipment.EMeasuredJackPanel ServerSelectedMeasureJackpanel = Atend.Base.Equipment.EMeasuredJackPanel.ServerSelectByCode(SelectedEquipEMeasuredJackPanel.Code, ServerConnection, ServerTransaction);
                            //                if (ServerSelectedMeasureJackpanel.Code != -1)
                            //                {
                            //                    if (SelectedEquipEMeasuredJackPanel.ServerUpdate(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                    {
                            //                        NewCode = SelectedEquipEMeasuredJackPanel.Code;
                            //                    }
                            //                    else
                            //                    {
                            //                        throw new System.Exception("Server Update failed" + Type.ToString());
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    throw new System.Exception("MeasuredJackPanel:There is No row On server For Update " + Type.ToString());
                            //                }

                            //            }
                            //            //}
                            //        }
                            //        else
                            //        {
                            //            ed.WriteMessage(Type.ToString() + " does not exist \n");
                            //        }
                            //        break;


                            //    case Atend.Control.Enum.ProductType.MiniatureKey:
                            //        ed.WriteMessage("Sub Is : MiniatureKey \n");
                            //        Atend.Base.Equipment.EMiniatorKey SelectedEquipEMiniatorKey = Atend.Base.Equipment.EMiniatorKey.SelectByXCodeForDesign(xCodeProduct, LocalConnection, LocalTransaction);
                            //        if (SelectedEquipEMiniatorKey.Code != -1)
                            //        {
                            //            //Atend.Base.Equipment.EMiniatorKey answer = Atend.Base.Equipment.EMiniatorKey.AccessSelectByXCode(SelectedEquipEMiniatorKey.XCode, _transaction, _connection);
                            //            //if (answer.Code != -1)
                            //            //{
                            //            //    NewCode = answer.Code;
                            //            //}
                            //            //else
                            //            //{
                            //            if (SelectedEquipEMiniatorKey.Code == 0)
                            //            {//confirm
                            //                if (SelectedEquipEMiniatorKey.ServerInsert(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                {
                            //                    NewCode = SelectedEquipEMiniatorKey.Code;
                            //                    if (!SelectedEquipEMiniatorKey.UpdateX(LocalTransaction, LocalConnection))
                            //                    {
                            //                        throw new System.Exception("UpdateX  MiniatorKey Failed");
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    throw new System.Exception("Server Insert failed" + Type.ToString());
                            //                }
                            //            }
                            //            else
                            //            {
                            //                Atend.Base.Equipment.EMiniatorKey ServerSelectedMiniatorKey = Atend.Base.Equipment.EMiniatorKey.ServerSelectByCode(SelectedEquipEMiniatorKey.Code, ServerConnection, ServerTransaction);
                            //                if (ServerSelectedMiniatorKey.Code != -1)
                            //                {
                            //                    if (SelectedEquipEMiniatorKey.ServerUpdate(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                    {
                            //                        NewCode = SelectedEquipEMiniatorKey.Code;
                            //                    }
                            //                    else
                            //                    {
                            //                        throw new System.Exception("Server Update failed" + Type.ToString());
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    throw new System.Exception("MiniatureKey:There is No row On server For Update " + Type.ToString());
                            //                }
                            //            }
                            //            //}
                            //        }
                            //        else
                            //        {
                            //            ed.WriteMessage(Type.ToString() + " does not exist \n");
                            //        }
                            //        break;
                            //    case Atend.Control.Enum.ProductType.PhotoCell:
                            //        ed.WriteMessage("Sub Is : PhotoCell \n");
                            //        Atend.Base.Equipment.EPhotoCell SelectedEquipEPhotoCell = Atend.Base.Equipment.EPhotoCell.SelectByXCodeForDesign(xCodeProduct, LocalConnection, LocalTransaction);
                            //        if (SelectedEquipEPhotoCell.Code != -1)
                            //        {
                            //            //Atend.Base.Equipment.EPhotoCell answer = Atend.Base.Equipment.EPhotoCell.AccessSelectByXCode(SelectedEquipEPhotoCell.XCode, _transaction, _connection);
                            //            //if (answer.Code != -1)
                            //            //{
                            //            //    NewCode = answer.Code;
                            //            //}
                            //            //else
                            //            //{
                            //            if (SelectedEquipEPhotoCell.Code == 0)
                            //            {//confirm
                            //                if (SelectedEquipEPhotoCell.ServerInsert(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                {
                            //                    NewCode = SelectedEquipEPhotoCell.Code;
                            //                    if (!SelectedEquipEPhotoCell.UpdateX(LocalTransaction, LocalConnection))
                            //                    {
                            //                        throw new System.Exception("UpdateX  PhotoCell Failed");
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    throw new System.Exception("Server Insert failed" + Type.ToString());
                            //                }
                            //            }
                            //            else
                            //            {
                            //                Atend.Base.Equipment.EPhotoCell ServerSelectedPhotoCell = Atend.Base.Equipment.EPhotoCell.ServerSelectByCode(SelectedEquipEPhotoCell.Code, ServerConnection, ServerTransaction);
                            //                if (ServerSelectedPhotoCell.Code != -1)
                            //                {
                            //                    if (SelectedEquipEPhotoCell.ServerUpdate(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                    {
                            //                        NewCode = SelectedEquipEPhotoCell.Code;
                            //                    }
                            //                    else
                            //                    {
                            //                        throw new System.Exception("Server Update failed" + Type.ToString());
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    throw new System.Exception("PhotoCell:There is No row On server For Update " + Type.ToString());
                            //                }
                            //            }
                            //            //}
                            //        }
                            //        else
                            //        {
                            //            ed.WriteMessage(Type.ToString() + " does not exist \n");
                            //        }
                            //        break;
                            //    case Atend.Control.Enum.ProductType.Phuse:
                            //        ed.WriteMessage("Sub Is : Phuse \n");
                            //        Atend.Base.Equipment.EPhuse SelectedEquipEPhuse = Atend.Base.Equipment.EPhuse.SelectByXCodeForDesign(xCodeProduct, LocalConnection, LocalTransaction);
                            //        if (SelectedEquipEPhuse.Code != -1)
                            //        {
                            //            //Atend.Base.Equipment.EPhuse answer = Atend.Base.Equipment.EPhuse.AccessSelectByXCode(SelectedEquipEPhuse.XCode, _transaction, _connection);
                            //            //if (answer.Code != -1)
                            //            //{
                            //            //    NewCode = answer.Code;
                            //            //}
                            //            //else
                            //            //{
                            //            Atend.Base.Equipment.EPhusePole _EPhusePole = Atend.Base.Equipment.EPhusePole.SelectByXCodeForDesign(SelectedEquipEPhuse.PhusePoleXCode, LocalConnection, LocalTransaction);
                            //            if (_EPhusePole.Code != -1)
                            //            {
                            //                if (_EPhusePole.Code == 0)
                            //                {
                            //                    if (!_EPhusePole.ServerInsert(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                    {
                            //                        throw new System.Exception("Server Insert failed" + Type.ToString() + ":PhusePole");
                            //                    }
                            //                    else
                            //                    {
                            //                        if (!_EPhusePole.UpdateX(LocalTransaction, LocalConnection))
                            //                        {
                            //                            throw new System.Exception("UpdateX  PhusePole Failed");
                            //                        }
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    Atend.Base.Equipment.EPhusePole ServerSelectedPhusePole = Atend.Base.Equipment.EPhusePole.ServerSelectByCode(_EPhusePole.Code, ServerConnection, ServerTransaction);
                            //                    if (ServerSelectedPhusePole.Code != -1)
                            //                    {
                            //                        if (!_EPhusePole.ServerUpdate(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                        {
                            //                            throw new System.Exception("Server Update phusePole Failed");
                            //                        }
                            //                    }
                            //                    else
                            //                    {
                            //                        throw new System.Exception("PhusepoleINphuse:There is No row On server For Update " + Type.ToString());
                            //                    }
                            //                }
                            //            }
                            //            else
                            //            {
                            //                throw new System.Exception("Lack of data in Phuse:PhusePole");
                            //            }
                            //            ed.WriteMessage("PhusePoleCode:{0}\n", _EPhusePole.Code);
                            //            SelectedEquipEPhuse.PhusePoleCode = _EPhusePole.Code;
                            //            if (SelectedEquipEPhuse.Code == 0)
                            //            {//confirm
                            //                if (SelectedEquipEPhuse.ServerInsert(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                {
                            //                    NewCode = SelectedEquipEPhuse.Code;
                            //                    if (!SelectedEquipEPhuse.UpdateX(LocalTransaction, LocalConnection))
                            //                    {
                            //                        throw new System.Exception("UpdateX Phuse  Failed");
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    throw new System.Exception("Server Insert failed" + Type.ToString());
                            //                }
                            //            }
                            //            else
                            //            {
                            //                Atend.Base.Equipment.EPhuse ServerSelectedPhuse = Atend.Base.Equipment.EPhuse.ServerSelectByCode(SelectedEquipEPhuse.Code, ServerConnection, ServerTransaction);
                            //                if (ServerSelectedPhuse.Code != -1)
                            //                {
                            //                    if (!SelectedEquipEPhuse.ServerUpdate(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                    {
                            //                        throw new System.Exception("Server Update Failed in Phuse");
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    throw new System.Exception("Phuse:There Is No Row on Server For Upadte " + Type.ToString());
                            //                }

                            //            }


                            //            //}
                            //        }
                            //        else
                            //        {
                            //            ed.WriteMessage(Type.ToString() + " does not exist \n");
                            //        }
                            //        break;
                            //    case Atend.Control.Enum.ProductType.PhuseKey:
                            //        ed.WriteMessage("Sub Is : PhuseKey \n");
                            //        Atend.Base.Equipment.EPhuseKey SelectedEquipEPhuseKey = Atend.Base.Equipment.EPhuseKey.SelectByXCodeForDesign(xCodeProduct, LocalConnection, LocalTransaction);
                            //        ed.WriteMessage("PhuseKey.Code={0}\n", SelectedEquipEPhuseKey.Code);
                            //        if (SelectedEquipEPhuseKey.Code != -1)
                            //        {

                            //            if (SelectedEquipEPhuseKey.Code == 0)
                            //            {//confirm
                            //                if (SelectedEquipEPhuseKey.ServerInsert(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                {
                            //                    NewCode = SelectedEquipEPhuseKey.Code;
                            //                    if (!SelectedEquipEPhuseKey.UpdateX(LocalTransaction, LocalConnection))
                            //                    {
                            //                        throw new System.Exception("UpdateX PhuseKey  Failed");
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    throw new System.Exception("Server Insert failed" + Type.ToString());
                            //                }
                            //            }
                            //            else
                            //            {
                            //                Atend.Base.Equipment.EPhuseKey ServerSelectedPhuseKey = Atend.Base.Equipment.EPhuseKey.ServerSelectByCode(SelectedEquipEPhuseKey.Code, ServerConnection, ServerTransaction);
                            //                if (ServerSelectedPhuseKey.Code != -1)
                            //                {
                            //                    if (SelectedEquipEPhuseKey.ServerUpdate(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                    {
                            //                        NewCode = SelectedEquipEPhuseKey.Code;
                            //                    }
                            //                    else
                            //                    {
                            //                        throw new System.Exception("Server Update failed" + Type.ToString());
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    throw new System.Exception("PhuseKey:There Is No Row on Server For Upadte " + Type.ToString());
                            //                }

                            //            }
                            //            //}
                            //        }
                            //        else
                            //        {
                            //            ed.WriteMessage(Type.ToString() + " does not exist \n");
                            //        }
                            //        break;
                            //    case Atend.Control.Enum.ProductType.PhusePole:
                            //        ed.WriteMessage("Sub Is : PhusePole \n");
                            //        Atend.Base.Equipment.EPhusePole SelectedEquipEPhusePole = Atend.Base.Equipment.EPhusePole.SelectByXCodeForDesign(xCodeProduct, LocalConnection, LocalTransaction);
                            //        if (SelectedEquipEPhusePole.Code != -1)
                            //        {
                            //            //Atend.Base.Equipment.EPhusePole answer = Atend.Base.Equipment.EPhusePole.AccessSelectByXCode(SelectedEquipEPhusePole.XCode, _transaction, _connection);
                            //            //if (answer.Code != -1)
                            //            //{
                            //            //    NewCode = answer.Code;
                            //            //}
                            //            //else
                            //            //{
                            //            if (SelectedEquipEPhusePole.Code == 0)
                            //            {//confirm
                            //                if (SelectedEquipEPhusePole.ServerInsert(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                {
                            //                    NewCode = SelectedEquipEPhusePole.Code;
                            //                    if (!SelectedEquipEPhusePole.UpdateX(LocalTransaction, LocalConnection))
                            //                    {
                            //                        throw new System.Exception("UpdateX  PhusePole Failed");
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    throw new System.Exception("Server Insert failed" + Type.ToString());
                            //                }
                            //            }
                            //            else
                            //            {
                            //                Atend.Base.Equipment.EPhusePole ServerSelectedPhusePole = Atend.Base.Equipment.EPhusePole.ServerSelectByCode(SelectedEquipEPhusePole.Code, ServerConnection, ServerTransaction);
                            //                if (ServerSelectedPhusePole.Code != -1)
                            //                {
                            //                    if (SelectedEquipEPhusePole.ServerUpdate(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                    {
                            //                        NewCode = SelectedEquipEPhusePole.Code;
                            //                    }
                            //                    else
                            //                    {
                            //                        throw new System.Exception("Server Update failed" + Type.ToString());
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    throw new System.Exception("PhusePole:There Is No Row on Server For Upadte " + Type.ToString());
                            //                }
                            //            }
                            //            //}
                            //        }
                            //        else
                            //        {
                            //            ed.WriteMessage(Type.ToString() + " does not exist \n");
                            //        }
                            //        break;
                            case Atend.Control.Enum.ProductType.Pole:
                                ed.WriteMessage("Sub Is : Pole \n");
                                Atend.Base.Equipment.EPole SelectedEquipEPole = Atend.Base.Equipment.EPole.AccessSelectByCode(Convert.ToInt32(drEntity["ProductCode"].ToString())/*, OldTransaction, OldConnection*/);
                                if (SelectedEquipEPole.Code != -1)
                                {

                                    if (drEntity["GroupObjID"].ToString() != "0")
                                    {
                                        DataRow[] dr = Atend.Control.Common.dtConvertor.Select(string.Format("GroupObjID={0} And ProductType={1}", drEntity["GroupObjId"].ToString(), drEntity["ProductType"].ToString()));
                                        if (dr.Length == 0)
                                        {
                                            DataRow[] drMem = Atend.Control.Common.dtConvertor.Select(string.Format("OldProductCode={0} And ProductType={1}", drEntity["OldProductCode"].ToString(), drEntity["ProductType"].ToString()));
                                            if (drMem.Length == 0)
                                            {
                                                if (!SelectedEquipEPole.AccessInsert(OldTransaction, OldConnection, NewTransaction, NewConnection, true, true))
                                                    throw new System.Exception("Exception In AccessInsert in Arm");
                                                else
                                                {
                                                    DataRow drNew = Atend.Control.Common.dtConvertor.NewRow();
                                                    drNew["ProuctType"] = drEntity["ProductType"].ToString();
                                                    drNew["OldProductCode"] = drEntity["ProductCode"].ToString();
                                                    drNew["NewProductCode"] = SelectedEquipEPole.Code;
                                                    drNew["GroupObjID"] = drEntity["GroupObjID"].ToString();
                                                    Atend.Control.Common.dtConvertor.Rows.Add(drNew);
                                                    NewCode = SelectedEquipEPole.Code; ;
                                                }
                                            }
                                            else
                                            {
                                                NewCode = Convert.ToInt32(drMem[0]["NewProductCode"].ToString());
                                            }
                                        }
                                    }
                                    else
                                    {
                                        DataRow[] drMem = Atend.Control.Common.dtConvertor.Select(string.Format("OldProductCode={0} And ProductType={1}", drEntity["OldProductCode"].ToString(), drEntity["ProductType"].ToString()));
                                        if (drMem.Length == 0)
                                        {
                                            if (!SelectedEquipEPole.AccessInsert(OldTransaction, OldConnection, NewTransaction, NewConnection, true, true))
                                                throw new System.Exception("Exception In AccessInsert in Arm");
                                            else
                                            {
                                                DataRow drNew = Atend.Control.Common.dtConvertor.NewRow();
                                                drNew["ProuctType"] = drEntity["ProductType"].ToString();
                                                drNew["OldProductCode"] = drEntity["ProductCode"].ToString();
                                                drNew["NewProductCode"] = SelectedEquipEPole.Code;
                                                drNew["GroupObjID"] = drEntity["GroupObjID"].ToString();
                                                Atend.Control.Common.dtConvertor.Rows.Add(drNew);
                                                NewCode = SelectedEquipEPole.Code; ;
                                            }
                                        }
                                        else
                                        {
                                            NewCode = Convert.ToInt32(drMem[0]["NewProductCode"].ToString());
                                        }
                                    }

                                    Atend.Base.Acad.AT_INFO AT = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId((ObjectId)drEntity["ObjID"]);
                                    AT.ProductCode = NewCode;
                                    AT.Insert();

                                    Atend.Base.Design.DNode Node = Atend.Base.Design.DNode.AccessSelectByCode(new Guid(drEntity["Guid"].ToString()), OldConnection);
                                    Atend.Base.Design.DPackage Package = Atend.Base.Design.DPackage.AccessSelectByNodeCode(new Guid(drEntity["Guid"].ToString()), OldTransaction, OldConnection);
                                    if (Node.Code != Guid.Empty && Package.Code != Guid.Empty)
                                    {
                                        if (!Node.AccessInsert(NewTransaction, NewConnection))
                                            throw new System.Exception("Node.Access Insert Failed in Pole");

                                        if (!Package.AccessInsert(NewTransaction, NewConnection))
                                            throw new System.Exception("Package.Access Insert Failed Pole");
                                    }
                                    else
                                    {
                                        throw new System.Exception("Lack Of Data In dNode && Dpackage pole");
                                    }
                                }
                                else
                                {
                                    throw new System.Exception("Lack Of Data In Pole");
                                }
                                break;
                            case Atend.Control.Enum.ProductType.PoleTip:
                                ed.WriteMessage("Sub Is : PoleTip,XCodeProduct={0} \n", drEntity["Guid"].ToString());
                                Atend.Base.Equipment.EPoleTip SelectedEquipEPoleTip = Atend.Base.Equipment.EPoleTip.AccessSelectByCodeForConvertor(Convert.ToInt32(drEntity["ProductCode"].ToString()), OldTransaction, OldConnection);
                                ed.WriteMessage("SelectedEquipEPoleTip.code={0}\n", SelectedEquipEPoleTip.Code);
                                if (SelectedEquipEPoleTip.Code != -1)
                                {
                                    //    Atend.Base.Equipment.EPole _ePole = Atend.Base.Equipment.EPole.AccessSelectByCodeForConvertor(SelectedEquipEPoleTip.PoleCode, OldTransaction, OldConnection);
                                    //    ed.WriteMessage("ePole.Code={0}\n", _ePole.Code);
                                    //    if (_ePole.Code != -1)
                                    //    {
                                    //        if (_ePole.Code == 0)
                                    //        {
                                    //            if (_ePole.ServerInsert(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                                    //            {
                                    //                NewCode = _ePole.Code;
                                    //                if (!_ePole.UpdateX(LocalTransaction, LocalConnection))
                                    //                    throw new System.Exception("epole.Updatex Failed");
                                    //            }
                                    //            else
                                    //            {
                                    //                throw new System.Exception("ServerInsert failed" + Convert.ToInt32(Type.ToString()) + ":POle");

                                    //            }

                                    //        }
                                    //        else
                                    //        {
                                    //            Atend.Base.Equipment.EPole _ServerPole = Atend.Base.Equipment.EPole.ServerSelectByCode(_ePole.Code, ServerConnection, ServerTransaction);
                                    //            if (!_ePole.ServerUpdate(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                                    //                throw new System.Exception("_epole serverUpdate Failed in PoleTip");
                                    //        }
                                    //    }
                                    //    else
                                    //    {
                                    //        throw new System.Exception("Lack of data in poleTip:Pole");
                                    //    }

                                    //    Atend.Base.Equipment.EHalter _eHalter = Atend.Base.Equipment.EHalter.SelectByXCodeForDesign(SelectedEquipEPoleTip.HalterXID, LocalConnection, LocalTransaction);
                                    //    if (_eHalter.Code != -1)
                                    //    {

                                    //        if (_eHalter.Code == 0)
                                    //        {

                                    //            if (_eHalter.ServerInsert(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                                    //            {
                                    //                NewCode = _eHalter.Code;
                                    //                if (!_eHalter.UpdateX(LocalTransaction, LocalConnection))
                                    //                    throw new System.Exception("eHalter Updatex Failed in PoleTip");
                                    //            }
                                    //            else
                                    //            {
                                    //                throw new System.Exception("ServerInsert failed" + Convert.ToInt32(Type).ToString() + ":Halter");

                                    //            }
                                    //        }
                                    //        else
                                    //        {
                                    //            Atend.Base.Equipment.EHalter ServerHalter = Atend.Base.Equipment.EHalter.ServerSelectByCode(_eHalter.Code, ServerConnection, ServerTransaction);
                                    //            if (_eHalter.Code != -1)
                                    //            {
                                    //                if (_eHalter.ServerUpdate(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                                    //                {
                                    //                    NewCode = _eHalter.Code;
                                    //                }
                                    //                else
                                    //                {
                                    //                    throw new System.Exception("Ehalter ServerUpdate Failed in Poletip");
                                    //                }
                                    //            }
                                    //        }
                                    //    }
                                    //    else
                                    //    {
                                    //        throw new System.Exception("Lack of data in poleTip:Halter");
                                    //    }

                                    //    SelectedEquipEPoleTip.PoleCode = _ePole.Code;
                                    //    SelectedEquipEPoleTip.HalterID = _eHalter.Code;

                                    //    if (SelectedEquipEPoleTip.Code == 0)
                                    //    {
                                    //        if (SelectedEquipEPoleTip.ServerInsert(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                                    //        {
                                    //            NewCode = SelectedEquipEPoleTip.Code;
                                    //            if (!SelectedEquipEPoleTip.UpdateX(LocalTransaction, LocalConnection))
                                    //                throw new System.Exception("SelectedEquipEPoleTip Updatex Failed in Poletip");
                                    //        }
                                    //        else
                                    //        {
                                    //            throw new System.Exception("ServerInsert failed" + Convert.ToInt32(Type).ToString());
                                    //        }
                                    //    }
                                    //    else
                                    //    {
                                    //        Atend.Base.Equipment.EPoleTip ServerPoleTip = Atend.Base.Equipment.EPoleTip.ServerSelectByCode(SelectedEquipEPoleTip.Code, ServerConnection, ServerTransaction);
                                    //        if (ServerPoleTip.Code != -1)
                                    //        {
                                    //            if (SelectedEquipEPoleTip.ServerUpdate(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                                    //            {
                                    //                NewCode = SelectedEquipEPoleTip.Code;
                                    //            }
                                    //            else
                                    //            {
                                    //                throw new System.Exception("SelectedEquipEPoleTip Server Update in PoleTip");
                                    //            }
                                    //        }
                                    //    }


                                    //}
                                }
                                else
                                {
                                    throw new System.Exception("Lack Of Data In PoleTip");
                                }
                                break;
                            //    case Atend.Control.Enum.ProductType.Prop:
                            //        ed.WriteMessage("Sub Is : Prop \n");
                            //        Atend.Base.Equipment.EProp SelectedEquipEProp = Atend.Base.Equipment.EProp.SelectByXCodeForDesign(xCodeProduct, LocalConnection, LocalTransaction);
                            //        if (SelectedEquipEProp.Code != -1)
                            //        {
                            //            //Atend.Base.Equipment.EProp answer = Atend.Base.Equipment.EProp.AccessSelectByXCode(SelectedEquipEProp.XCode, _transaction, _connection);
                            //            //if (answer.Code != -1)
                            //            //{
                            //            //    NewCode = answer.Code;
                            //            //}
                            //            //else
                            //            //{
                            //            if (SelectedEquipEProp.Code == 0)
                            //            {//confirm
                            //                if (SelectedEquipEProp.ServerInsert(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                {
                            //                    NewCode = SelectedEquipEProp.Code;
                            //                    if (!SelectedEquipEProp.UpdateX(LocalTransaction, LocalConnection))
                            //                    {
                            //                        throw new System.Exception("UpdateX  Prop Failed");
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    throw new System.Exception("container error in Server Insert failed" + Type.ToString());
                            //                }
                            //            }
                            //            else
                            //            {
                            //                Atend.Base.Equipment.EProp ServerSelectedProp = Atend.Base.Equipment.EProp.ServerSelectByCode(SelectedEquipEProp.Code, ServerConnection, ServerTransaction);
                            //                if (ServerSelectedProp.Code != -1)
                            //                {
                            //                    if (SelectedEquipEProp.ServerUpdate(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                    {
                            //                        NewCode = SelectedEquipEProp.Code;
                            //                    }
                            //                    else
                            //                    {
                            //                        throw new System.Exception("container error in Server Update failed" + Type.ToString());
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    throw new System.Exception("Prop:There Is No Row on Server For Upadte " + Type.ToString());
                            //                }
                            //            }
                            //            //}
                            //        }
                            //        else
                            //        {
                            //            ed.WriteMessage(Type.ToString() + " does not exist \n");
                            //        }
                            //        break;

                            //    case Atend.Control.Enum.ProductType.PT:
                            //        ed.WriteMessage("Sub Is : PT \n");
                            //        Atend.Base.Equipment.EPT SelectedEquipEPT = Atend.Base.Equipment.EPT.SelectByXCodeForDesign(xCodeProduct, LocalConnection, LocalTransaction);
                            //        if (SelectedEquipEPT.Code != -1)
                            //        {
                            //            //Atend.Base.Equipment.EPT answer = Atend.Base.Equipment.EPT.AccessSelectByXCode(SelectedEquipEPT.XCode, _transaction, _connection);
                            //            //if (answer.Code != -1)
                            //            //{
                            //            //    NewCode = answer.Code;
                            //            //}
                            //            //else
                            //            //{
                            //            if (SelectedEquipEPT.Code == 0)
                            //            {//confirm
                            //                if (SelectedEquipEPT.ServerInsert(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                {
                            //                    NewCode = SelectedEquipEPT.Code;
                            //                    if (!SelectedEquipEPT.UpdateX(LocalTransaction, LocalConnection))
                            //                    {
                            //                        throw new System.Exception("UpdateX PT  Failed");
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    throw new System.Exception("Server Insert failed" + Type.ToString());
                            //                }
                            //            }
                            //            else
                            //            {
                            //                Atend.Base.Equipment.EPT ServerSelectedPT = Atend.Base.Equipment.EPT.ServerSelectByCode(SelectedEquipEPT.Code, ServerConnection, ServerTransaction);
                            //                if (ServerSelectedPT.Code != -1)
                            //                {
                            //                    if (SelectedEquipEPT.ServerUpdate(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                    {
                            //                        NewCode = SelectedEquipEPT.Code;
                            //                    }
                            //                    else
                            //                    {
                            //                        throw new System.Exception("Server Update failed" + Type.ToString());
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    throw new System.Exception("PT:There Is No Row on Server For Upadte " + Type.ToString());
                            //                }
                            //            }
                            //            //}
                            //        }
                            //        else
                            //        {
                            //            ed.WriteMessage(Type.ToString() + " does not exist \n");
                            //        }
                            //        break;



                            //    case Atend.Control.Enum.ProductType.Ramp:
                            //        ed.WriteMessage("Sub Is : Ramp \n");
                            //        Atend.Base.Equipment.ERamp SelectedEquipERamp = Atend.Base.Equipment.ERamp.SelectByXCodeForDesign(xCodeProduct, LocalConnection, LocalTransaction);
                            //        if (SelectedEquipERamp.Code != -1)
                            //        {
                            //            //Atend.Base.Equipment.ERamp answer = Atend.Base.Equipment.ERamp.AccessSelectByXCode(SelectedEquipERamp.XCode, _transaction, _connection);
                            //            //if (answer.Code != -1)
                            //            //{
                            //            //    NewCode = answer.Code;
                            //            //}
                            //            //else
                            //            //{
                            //            if (SelectedEquipERamp.Code == 0)
                            //            {//confirm
                            //                if (SelectedEquipERamp.ServerInsert(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                {
                            //                    NewCode = SelectedEquipERamp.Code;
                            //                    if (!SelectedEquipERamp.UpdateX(LocalTransaction, LocalConnection))
                            //                    {
                            //                        throw new System.Exception("UpdateX Ramp  Failed");
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    throw new System.Exception("Server Insert failed" + Type.ToString());
                            //                }
                            //            }
                            //            else
                            //            {
                            //                Atend.Base.Equipment.ERamp ServerSelectedRamp = Atend.Base.Equipment.ERamp.ServerSelectByCode(SelectedEquipERamp.Code, ServerConnection, ServerTransaction);
                            //                if (ServerSelectedRamp.Code != -1)
                            //                {
                            //                    if (SelectedEquipERamp.ServerUpdate(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                    {
                            //                        NewCode = SelectedEquipERamp.Code;
                            //                    }
                            //                    else
                            //                    {
                            //                        throw new System.Exception("Server Update failed" + Type.ToString());
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    throw new System.Exception("Ramp:There Is No Row on Server For Upadte " + Type.ToString());
                            //                }
                            //            }
                            //            //}
                            //        }
                            //        else
                            //        {
                            //            ed.WriteMessage(Type.ToString() + " does not exist \n");
                            //        }
                            //        break;


                            //    case Atend.Control.Enum.ProductType.ReCloser:
                            //        ed.WriteMessage("Sub Is : ReCloser \n");
                            //        Atend.Base.Equipment.EReCloser SelectedEquipEReCloser = Atend.Base.Equipment.EReCloser.SelectByXCodeForDesign(xCodeProduct, LocalConnection, LocalTransaction);
                            //        if (SelectedEquipEReCloser.Code != -1)
                            //        {
                            //            //Atend.Base.Equipment.EReCloser answer = Atend.Base.Equipment.EReCloser.AccessSelectByXCode(SelectedEquipEReCloser.XCode, _transaction, _connection);
                            //            //if (answer.Code != -1)
                            //            //{
                            //            //    NewCode = answer.Code;
                            //            //}
                            //            //else
                            //            //{
                            //            if (SelectedEquipEReCloser.Code == 0)
                            //            {//confirm
                            //                if (SelectedEquipEReCloser.ServerInsert(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                {
                            //                    NewCode = SelectedEquipEReCloser.Code;
                            //                    if (!SelectedEquipEReCloser.UpdateX(LocalTransaction, LocalConnection))
                            //                    {
                            //                        throw new System.Exception("UpdateX ReCloser  Failed");
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    throw new System.Exception("ServerInsert failed" + Type.ToString());
                            //                }
                            //            }
                            //            else
                            //            {
                            //                Atend.Base.Equipment.EReCloser ServerSelectedReCloser = Atend.Base.Equipment.EReCloser.ServerSelectByCode(SelectedEquipEReCloser.Code, ServerConnection, ServerTransaction);
                            //                if (ServerSelectedReCloser.Code != -1)
                            //                {
                            //                    if (SelectedEquipEReCloser.ServerUpdate(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                    {
                            //                        NewCode = SelectedEquipEReCloser.Code;
                            //                    }
                            //                    else
                            //                    {
                            //                        throw new System.Exception("ServerUpdate failed" + Type.ToString());
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    throw new System.Exception("ReCloser:There Is No Row on Server For Upadte " + Type.ToString());
                            //                }
                            //            }
                            //            //}
                            //        }
                            //        else
                            //        {
                            //            ed.WriteMessage(Type.ToString() + " does not exist \n");
                            //        }
                            //        break;

                            //    case Atend.Control.Enum.ProductType.Rod:
                            //        ed.WriteMessage("Sub Is : Rod \n");
                            //        Atend.Base.Equipment.ERod SelectedEquipERod = Atend.Base.Equipment.ERod.SelectByXCodeForDesign(xCodeProduct, LocalConnection, LocalTransaction);
                            //        if (SelectedEquipERod.Code != -1)
                            //        {
                            //            //Atend.Base.Equipment.ERod answer = Atend.Base.Equipment.ERod.AccessSelectByXCode(SelectedEquipERod.XCode, _transaction, _connection);
                            //            //if (answer.Code != -1)
                            //            //{
                            //            //    NewCode = answer.Code;
                            //            //}
                            //            //else
                            //            //{
                            //            if (SelectedEquipERod.Code == 0)
                            //            {//confirm
                            //                if (SelectedEquipERod.ServerInsert(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                {
                            //                    NewCode = SelectedEquipERod.Code;
                            //                    if (!SelectedEquipERod.UpdateX(LocalTransaction, LocalConnection))
                            //                    {
                            //                        throw new System.Exception("UpdateX Rod  Failed");
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    throw new System.Exception("ServerInsert failed" + Type.ToString());
                            //                }
                            //            }
                            //            else
                            //            {
                            //                Atend.Base.Equipment.ERod ServerSelectedRod = Atend.Base.Equipment.ERod.ServerSelectByCode(SelectedEquipERod.Code, ServerConnection, ServerTransaction);
                            //                if (ServerSelectedRod.Code != -1)
                            //                {
                            //                    if (SelectedEquipERod.ServerUpdate(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                    {
                            //                        NewCode = SelectedEquipERod.Code;
                            //                    }
                            //                    else
                            //                    {
                            //                        throw new System.Exception("ServerUpdate failed" + Type.ToString());
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    throw new System.Exception("Rod:There Is No Row on Server For Upadte " + Type.ToString());
                            //                }
                            //            }
                            //            //}
                            //        }
                            //        else
                            //        {
                            //            ed.WriteMessage(Type.ToString() + " does not exist \n");
                            //        }
                            //        break;

                            //    case Atend.Control.Enum.ProductType.SectionLizer:
                            //        ed.WriteMessage("Sub Is : SectionLizer \n");
                            //        Atend.Base.Equipment.ESectionLizer SelectedEquipESectionLizer = Atend.Base.Equipment.ESectionLizer.SelectByXCodeForDesign(xCodeProduct, LocalConnection, LocalTransaction);
                            //        if (SelectedEquipESectionLizer.Code != -1)
                            //        {
                            //            //Atend.Base.Equipment.ESectionLizer answer = Atend.Base.Equipment.ESectionLizer.AccessSelectByXCode(SelectedEquipESectionLizer.XCode, _transaction, _connection);
                            //            //if (answer.Code != -1)
                            //            //{
                            //            //    NewCode = answer.Code;
                            //            //}
                            //            //else
                            //            //{
                            //            if (SelectedEquipESectionLizer.Code == 0)
                            //            {//confirm
                            //                if (SelectedEquipESectionLizer.ServerInsert(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                {
                            //                    NewCode = SelectedEquipESectionLizer.Code;
                            //                    if (!SelectedEquipESectionLizer.UpdateX(LocalTransaction, LocalConnection))
                            //                    {
                            //                        throw new System.Exception("UpdateX   Failed");
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    throw new System.Exception("Server Insert failed" + Type.ToString());
                            //                }
                            //            }
                            //            else
                            //            {
                            //                Atend.Base.Equipment.ESectionLizer ServerSelectedSectionLizer = Atend.Base.Equipment.ESectionLizer.ServerSelectByCode(SelectedEquipESectionLizer.Code, ServerConnection, ServerTransaction);
                            //                if (ServerSelectedSectionLizer.Code != -1)
                            //                {
                            //                    if (SelectedEquipESectionLizer.ServerUpdate(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                    {
                            //                        NewCode = SelectedEquipESectionLizer.Code;
                            //                    }
                            //                    else
                            //                    {
                            //                        throw new System.Exception("Server Update failed" + Type.ToString());
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    throw new System.Exception("SectionLizer:There Is No Row on Server For Upadte " + Type.ToString());
                            //                }
                            //            }
                            //            //}
                            //        }
                            //        else
                            //        {
                            //            ed.WriteMessage(Type.ToString() + " does not exist \n");
                            //        }
                            //        break;

                            //    case Atend.Control.Enum.ProductType.SelfKeeper:
                            //        ed.WriteMessage("Sub Is : SelfKeeper \n");
                            //        Atend.Base.Equipment.ESelfKeeper SelectedEquipESelfKeeper = Atend.Base.Equipment.ESelfKeeper.SelectByXCodeForDesign(xCodeProduct, LocalConnection, LocalTransaction);
                            //        if (SelectedEquipESelfKeeper.Code != -1)
                            //        {
                            //            //Atend.Base.Equipment.ESelfKeeper answer = Atend.Base.Equipment.ESelfKeeper.AccessSelectByXCode(SelectedEquipESelfKeeper.XCode, _transaction, _connection);
                            //            //if (answer.Code != -1)
                            //            //{
                            //            //    NewCode = answer.Code;
                            //            //}
                            //            //else
                            //            //{
                            //            if (SelectedEquipESelfKeeper.Code == 0)
                            //            {
                            //                if (SelectedEquipESelfKeeper.ServerInsert(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                {
                            //                    NewCode = SelectedEquipESelfKeeper.Code;
                            //                    if (!SelectedEquipESelfKeeper.UpdateX(LocalTransaction, LocalConnection))
                            //                    {
                            //                        throw new System.Exception("UpdateX SelfKeeper  Failed");
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    throw new System.Exception("Server Insert failed" + Type.ToString());
                            //                }
                            //            }
                            //            else
                            //            {
                            //                Atend.Base.Equipment.ESelfKeeper ServerSelectedSelfKeeper = Atend.Base.Equipment.ESelfKeeper.ServerSelectByCode(SelectedEquipESelfKeeper.Code, ServerConnection, ServerTransaction);
                            //                if (ServerSelectedSelfKeeper.Code != -1)
                            //                {
                            //                    if (SelectedEquipESelfKeeper.ServerUpdate(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                    {
                            //                        NewCode = SelectedEquipESelfKeeper.Code;

                            //                    }
                            //                    else
                            //                    {
                            //                        throw new System.Exception("Server Update failed" + Type.ToString());
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    throw new System.Exception("SelfKeeper:There Is No Row on Server For Upadte " + Type.ToString());
                            //                }
                            //            }
                            //            //}
                            //        }
                            //        else
                            //        {
                            //            ed.WriteMessage(Type.ToString() + " does not exist \n");
                            //        }
                            //        break;


                            //    case Atend.Control.Enum.ProductType.SelfKeeperTip:
                            //        ed.WriteMessage("Sub Is : SelfkeeperTip \n");

                            //        Atend.Base.Equipment.ESelfKeeperTip SelectedequipESelfKeeperTip = Atend.Base.Equipment.ESelfKeeperTip.SelectByXCodeForDesign(LocalTransaction, LocalConnection, xCodeProduct);




                            //        if (SelectedequipESelfKeeperTip.Code != -1)
                            //        {

                            //            Atend.Base.Equipment.ESelfKeeper SelectedPhaseSelfKeeper = Atend.Base.Equipment.ESelfKeeper.SelectByXCodeForDesign(SelectedequipESelfKeeperTip.PhaseProductxCode, LocalConnection, LocalTransaction);
                            //            Atend.Base.Equipment.ESelfKeeper SelectedNeutralSelfKeeper = Atend.Base.Equipment.ESelfKeeper.SelectByXCodeForDesign(SelectedequipESelfKeeperTip.NeutralProductxCode, LocalConnection, LocalTransaction);
                            //            Atend.Base.Equipment.ESelfKeeper SelectedNightSelfKeeper = Atend.Base.Equipment.ESelfKeeper.SelectByXCodeForDesign(SelectedequipESelfKeeperTip.NightProductxCode, LocalConnection, LocalTransaction);

                            //            if (SelectedPhaseSelfKeeper.Code != -1)
                            //            {
                            //                if (SelectedPhaseSelfKeeper.Code == 0)
                            //                {
                            //                    if (SelectedPhaseSelfKeeper.ServerInsert(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                    {
                            //                        NewCode = SelectedPhaseSelfKeeper.Code;
                            //                        if (!SelectedPhaseSelfKeeper.UpdateX(LocalTransaction, LocalConnection))
                            //                            throw new System.Exception("SelectedPhaseSelfKeeper Updatex Failed");
                            //                    }
                            //                    else
                            //                    {
                            //                        throw new System.Exception("SelfKeeper ServerInsert failed in SelfTip");
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    Atend.Base.Equipment.ESelfKeeper ServerSelf = Atend.Base.Equipment.ESelfKeeper.ServerSelectByCode(SelectedPhaseSelfKeeper.Code, ServerConnection, ServerTransaction);
                            //                    if (ServerSelf.Code != -1)
                            //                    {
                            //                        if (SelectedPhaseSelfKeeper.ServerUpdate(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                        {
                            //                            NewCode = SelectedPhaseSelfKeeper.Code;
                            //                        }
                            //                        else
                            //                        {
                            //                            throw new System.Exception("PhaseSelf ServerUpdate Failed in SelfTip");
                            //                        }
                            //                    }
                            //                    else
                            //                    {
                            //                        throw new System.Exception("There Is No Row On Server in SelfPhase in SelfTip");
                            //                    }
                            //                }

                            //            }
                            //            else
                            //            {
                            //                throw new System.Exception("Lack Of data In Phase Self in Self Tip");
                            //            }




                            //            if (SelectedNightSelfKeeper.Code != -1)
                            //            {
                            //                if (SelectedNightSelfKeeper.Code == 0)
                            //                {
                            //                    if (SelectedNightSelfKeeper.ServerInsert(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                    {
                            //                        NewCode = SelectedNightSelfKeeper.Code;
                            //                        if (!SelectedNightSelfKeeper.UpdateX(LocalTransaction, LocalConnection))
                            //                            throw new System.Exception("SelectedNightSelfKeeper Updatex Failed");
                            //                    }
                            //                    else
                            //                    {
                            //                        throw new System.Exception("NightSelf ServerInsert failed in SelfTip");
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    Atend.Base.Equipment.ESelfKeeper ServerNightSelf = Atend.Base.Equipment.ESelfKeeper.ServerSelectByCode(SelectedNightSelfKeeper.Code, ServerConnection, ServerTransaction);
                            //                    if (ServerNightSelf.Code != -1)
                            //                    {
                            //                        if (SelectedNightSelfKeeper.ServerUpdate(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                        {
                            //                            NewCode = SelectedNightSelfKeeper.Code;
                            //                        }
                            //                        else
                            //                        {
                            //                            throw new System.Exception("NightCond ServerUpdate Failed in CondTip");
                            //                        }
                            //                    }
                            //                    else
                            //                    {
                            //                        throw new System.Exception("There Is No Row On Server in CondNight in CondTip");
                            //                    }
                            //                }

                            //            }
                            //            else
                            //            {
                            //                throw new System.Exception("Lack Of data In Night Conductor in Conductor Tip");
                            //            }




                            //            if (SelectedNeutralSelfKeeper.Code != -1)
                            //            {
                            //                if (SelectedNeutralSelfKeeper.Code == 0)
                            //                {
                            //                    if (SelectedNeutralSelfKeeper.ServerInsert(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                    {
                            //                        NewCode = SelectedNeutralSelfKeeper.Code;
                            //                        if (!SelectedNeutralSelfKeeper.UpdateX(LocalTransaction, LocalConnection))
                            //                            throw new System.Exception("SelectedNeutralSelfKeeper Updatex Failed");
                            //                    }
                            //                    else
                            //                    {
                            //                        throw new System.Exception("NeutralConductor ServerInsert failed in CondTip");
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    Atend.Base.Equipment.ESelfKeeper ServerNeutralSelf = Atend.Base.Equipment.ESelfKeeper.ServerSelectByCode(SelectedNeutralSelfKeeper.Code, ServerConnection, ServerTransaction);
                            //                    if (ServerNeutralSelf.Code != -1)
                            //                    {
                            //                        if (SelectedNeutralSelfKeeper.ServerUpdate(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                        {
                            //                            NewCode = SelectedNeutralSelfKeeper.Code;
                            //                        }
                            //                        else
                            //                        {
                            //                            throw new System.Exception("NeutralCond ServerUpdate Failed in CondTip");
                            //                        }
                            //                    }
                            //                    else
                            //                    {
                            //                        throw new System.Exception("There Is No Row On Server in CondNeutral in CondTip");
                            //                    }
                            //                }

                            //            }
                            //            else
                            //            {
                            //                throw new System.Exception("Lack Of data In PNetral Conductor in Conductor Tip");
                            //            }


                            //            SelectedequipESelfKeeperTip.PhaseProductCode = SelectedPhaseSelfKeeper.Code;
                            //            SelectedequipESelfKeeperTip.NeutralProductCode = SelectedNeutralSelfKeeper.Code;
                            //            SelectedequipESelfKeeperTip.NightProductCode = SelectedNightSelfKeeper.Code;
                            //            if (SelectedequipESelfKeeperTip.Code == 0)
                            //            {
                            //                if (SelectedequipESelfKeeperTip.ServerInsert(ServerTransaction, ServerConnection))//, true, true, LocalTransaction, LocalConnection))
                            //                {
                            //                    NewCode = SelectedequipESelfKeeperTip.Code;
                            //                    if (!SelectedequipESelfKeeperTip.UpdateX(LocalTransaction, LocalConnection))
                            //                    {
                            //                        throw new System.Exception("UpdateX    ConductorTip     Failed");
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    throw new System.Exception("ServerInsert failed" + Type.ToString());
                            //                }
                            //            }
                            //            else
                            //            {
                            //                //Atend.Base.Equipment.EConductor ServerSelectedSelfKeeper = Atend.Base.Equipment.EConductor.ServerSelectByCode(SelectedequipESelfKeeperTip.Code, ServerConnection, ServerTransaction);
                            //                Atend.Base.Equipment.ESelfKeeperTip ServerSelectedSelfKeeper = Atend.Base.Equipment.ESelfKeeperTip.ServerSelectByCode(SelectedequipESelfKeeperTip.Code, ServerConnection, ServerTransaction);
                            //                if (ServerSelectedSelfKeeper.Code != -1)
                            //                {
                            //                    if (SelectedequipESelfKeeperTip.ServerUpdate(ServerTransaction, ServerConnection))//, true, false, LocalTransaction, LocalConnection))
                            //                    {
                            //                        NewCode = SelectedequipESelfKeeperTip.Code;
                            //                    }
                            //                    else
                            //                    {
                            //                        throw new System.Exception("ServerUpdate failed" + Type.ToString());
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    throw new System.Exception("Conductor:There Is No Row On Server For Update " + Type.ToString());
                            //                }
                            //            }
                            //            //}
                            //        }
                            //        else
                            //        {
                            //            ed.WriteMessage(Type.ToString() + " does not exist \n");
                            //        }
                            //        break;




                            //    //case Atend.Control.Enum.ProductType.SelfKeeperTip:
                            //    //    ed.WriteMessage("Sub Is : SelfKeeperTip \n");
                            //    //    Atend.Base.Equipment.ESelfKeeperTip SelectedEquipESelfKeeperTip = Atend.Base.Equipment.ESelfKeeperTip.SelectByXCode(new Guid(dr["XCode"].ToString()));
                            //    //    if (SelectedEquipESelfKeeperTip.Code != -1)
                            //    //    {
                            //    //        //Atend.Base.Equipment.ESelfKeeperTip answer = Atend.Base.Equipment.ESelfKeeperTip.AccessSelectByXCode(SelectedEquipESelfKeeperTip.XCode, _transaction, _connection);
                            //    //        //if (answer.Code != -1)
                            //    //        //{
                            //    //        //    NewCode = answer.Code;
                            //    //        //}
                            //    //        //else
                            //    //        //{
                            //    //        if (SelectedEquipESelfKeeperTip.AccessInsert(_transaction, _connection))
                            //    //        {
                            //    //            NewCode = SelectedEquipESelfKeeperTip.Code;
                            //    //        }
                            //    //        else
                            //    //        {
                            //    //            throw new System.Exception("AccessInsert failed" + Convert.ToInt32(dr["TableType"]).ToString());
                            //    //        }
                            //    //        //}
                            //    //    }
                            //    //    break;

                            //    case Atend.Control.Enum.ProductType.StreetBox:
                            //        ed.WriteMessage("Sub Is : StreetBox \n");
                            //        Atend.Base.Equipment.EStreetBox SelectedEquipEStreetBox = Atend.Base.Equipment.EStreetBox.SelectByXCodeForDesign(xCodeProduct, LocalConnection, LocalTransaction);
                            //        if (SelectedEquipEStreetBox.Code != -1)
                            //        {
                            //            //Atend.Base.Equipment.EStreetBox answer = Atend.Base.Equipment.EStreetBox.AccessSelectByXCode(SelectedEquipEStreetBox.XCode, _transaction, _connection);
                            //            //if (answer.Code != -1)
                            //            //{
                            //            //    NewCode = answer.Code;
                            //            //}
                            //            //else
                            //            //{
                            //            if (SelectedEquipEStreetBox.Code == 0)
                            //            {//confirm
                            //                if (SelectedEquipEStreetBox.ServerInsert(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                {
                            //                    NewCode = SelectedEquipEStreetBox.Code;
                            //                    if (!SelectedEquipEStreetBox.UpdateX(LocalTransaction, LocalConnection))
                            //                    {
                            //                        throw new System.Exception("UpdateX StreetBox  Failed");
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    throw new System.Exception("Server InSert Failed " + Type.ToString());
                            //                }
                            //            }
                            //            else
                            //            {
                            //                Atend.Base.Equipment.EStreetBox ServerSelectedStreetBox = Atend.Base.Equipment.EStreetBox.ServerSelectByCode(SelectedEquipEStreetBox.Code, ServerConnection, ServerTransaction);
                            //                if (ServerSelectedStreetBox.Code != -1)
                            //                {
                            //                    if (SelectedEquipEStreetBox.ServerUpdate(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                    {
                            //                        NewCode = SelectedEquipEStreetBox.Code;
                            //                    }
                            //                    else
                            //                    {
                            //                        throw new System.Exception("Server Update Failed " + Type.ToString());
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    throw new System.Exception("StreetBox:There is no row on server for update " + Type.ToString());
                            //                }

                            //            }
                            //            //if (SelectedEquipEStreetBox.ServerInsert(_transaction, _connection, true, false))
                            //            //{
                            //            //    NewCode = SelectedEquipEStreetBox.Code;
                            //            DataTable StreetBoxPhuses = Atend.Base.Equipment.EStreetBoxPhuse.SelectByStreetBoxXCode(xCodeProduct, LocalTransaction, LocalConnection);
                            //            ed.WriteMessage("StrretBoxPhuse={0}\n", StreetBoxPhuses.Rows.Count);

                            //            if (!Atend.Base.Equipment.EStreetBoxPhuse.Delete(ServerTransaction, ServerConnection, SelectedEquipEStreetBox.Code))
                            //            {
                            //                throw new System.Exception("Error In Delete StreetBoxPhuse");
                            //            }

                            //            foreach (DataRow StreetBoxPhuse in StreetBoxPhuses.Rows)
                            //            {
                            //                //ed.WriteMessage("110\n");
                            //                Atend.Base.Equipment.EPhuse _EPhuse = Atend.Base.Equipment.EPhuse.SelectByXCodeForDesign(new Guid(StreetBoxPhuse["PhuseXCode"].ToString()), LocalConnection, LocalTransaction);
                            //                ed.WriteMessage("_EPhuse.Code={0}\n", _EPhuse.Code);
                            //                if (_EPhuse.Code != -1)
                            //                {
                            //                    //ed.WriteMessage("111\n");
                            //                    Atend.Base.Equipment.EPhusePole _EPhusePole = Atend.Base.Equipment.EPhusePole.SelectByXCodeForDesign(_EPhuse.PhusePoleXCode, LocalConnection, LocalTransaction);
                            //                    ed.WriteMessage("_EPhusePole.Code={0}\n", _EPhusePole.Code);
                            //                    if (_EPhusePole.Code != -1)
                            //                    {
                            //                        //ed.WriteMessage("112\n");
                            //                        if (_EPhusePole.Code == 0)
                            //                        {
                            //                            ed.WriteMessage("_EPhusePole\n");
                            //                            if (!_EPhusePole.ServerInsert(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                            {
                            //                                throw new System.Exception("ServerInsert failed" + Type.ToString() + ":PhusePole");
                            //                            }
                            //                            else
                            //                            {
                            //                                if (!_EPhusePole.UpdateX(LocalTransaction, LocalConnection))
                            //                                {
                            //                                    throw new System.Exception("UpdateX PhusePole  Failed");
                            //                                }
                            //                            }
                            //                        }
                            //                        else
                            //                        {
                            //                            Atend.Base.Equipment.EPhusePole ServerSelectedPhusePole = Atend.Base.Equipment.EPhusePole.ServerSelectByCode(_EPhusePole.Code, ServerConnection, ServerTransaction);
                            //                            ed.WriteMessage("ServerSelectedPhusePole={0}\n", ServerSelectedPhusePole.Code);
                            //                            if (ServerSelectedPhusePole.Code != -1)
                            //                            {
                            //                                if (!_EPhusePole.ServerUpdate(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                                {
                            //                                    throw new System.Exception("ServerUpdate failed" + Type.ToString() + ":PhusePole");
                            //                                }
                            //                            }
                            //                            else
                            //                            {
                            //                                throw new System.Exception("phusePoleINStreetBox:There Is No Row on Server For Update PhusePole in StreetBox");
                            //                            }
                            //                        }
                            //                    }
                            //                    else
                            //                    {
                            //                        throw new System.Exception("Lack of data in StreetBox:Phuse:PhusePole");
                            //                    }
                            //                    //ed.WriteMessage("streetbox was :{0} \n", _EPhusePole.Code);
                            //                    _EPhuse.PhusePoleCode = _EPhusePole.Code;
                            //                    if (_EPhuse.Code == 0)
                            //                    {
                            //                        if (!_EPhuse.ServerInsert(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                        {
                            //                            throw new System.Exception("ServerInsert failed" + Type.ToString() + ":Phuse");
                            //                        }
                            //                        else
                            //                        {
                            //                            if (!_EPhuse.UpdateX(LocalTransaction, LocalConnection))
                            //                            {
                            //                                throw new System.Exception("UpdateX Phuse  Failed");
                            //                            }
                            //                        }
                            //                    }
                            //                    else
                            //                    {

                            //                        Atend.Base.Equipment.EPhuse ServerSelectedPhuse = Atend.Base.Equipment.EPhuse.ServerSelectByCode(_EPhuse.Code, ServerConnection, ServerTransaction);
                            //                        if (ServerSelectedPhuse.Code != -1)
                            //                        {
                            //                            if (!_EPhuse.ServerUpdate(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                            {
                            //                                throw new System.Exception("ServerUpdate failed" + Type.ToString() + ":Phuse");
                            //                            }
                            //                        }
                            //                        else
                            //                        {
                            //                            throw new System.Exception("PhuseINStreetBox:There is no row on server for update " + Type.ToString());
                            //                        }
                            //                    }

                            //                    Atend.Base.Equipment.EStreetBoxPhuse _EStreetBoxPhuse = Atend.Base.Equipment.EStreetBoxPhuse.SelectByXCodeForDesign(new Guid(StreetBoxPhuse["Xcode"].ToString()), LocalConnection, LocalTransaction);
                            //                    if (_EStreetBoxPhuse.Code != -1)
                            //                    {
                            //                        _EStreetBoxPhuse.PhuseCode = _EPhuse.Code;
                            //                        _EStreetBoxPhuse.StreetBoxCode = SelectedEquipEStreetBox.Code;
                            //                        //if (_EStreetBoxPhuse.Code == 0)
                            //                        //{//confirm
                            //                        if (!_EStreetBoxPhuse.ServerInsert(ServerTransaction, ServerConnection, true, false, LocalTransaction, LocalConnection))
                            //                        {
                            //                            throw new System.Exception("ServerInsert failed" + Type.ToString() + ":StreetBoxPhuse");
                            //                        }
                            //                        else
                            //                        {
                            //                            if (!_EStreetBoxPhuse.UpdateX(LocalTransaction, LocalConnection))
                            //                            {
                            //                                throw new System.Exception("UpdateX StreetBoxPhuse  Failed");
                            //                            }
                            //                        }
                            //                        //}
                            //                        //else
                            //                        //{
                            //                        //    Atend.Base.Equipment.EStreetBoxPhuse ServerSelectedStreetBoxPhuse = Atend.Base.Equipment.EStreetBoxPhuse.ServerSelectByCode(_EStreetBoxPhuse.Code, ServerConnection, ServerTransaction);
                            //                        //    if (ServerSelectedStreetBoxPhuse.Code != -1)
                            //                        //    {

                            //                        //        if (!_EStreetBoxPhuse.ServerUpdate(ServerTransaction, ServerConnection, true, false, LocalTransaction, LocalConnection))
                            //                        //        {
                            //                        //            throw new System.Exception("Server Update failed" + Type.ToString() + ":StreetBoxPhuse");
                            //                        //        }
                            //                        //    }
                            //                        //    else
                            //                        //    {
                            //                        //        throw new System.Exception("StreetBoxPhuseINStreetBox:There is no row on server for update " + Type.ToString());
                            //                        //    }
                            //                        //}
                            //                    }
                            //                    else
                            //                    {
                            //                        throw new System.Exception("Lack of data in StreetBox:StreetBoxPhuse");
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    throw new System.Exception("Lack of data in StreetBox:Phuse");
                            //                }
                            //            }
                            //        }
                            //        else
                            //        {
                            //            throw new System.Exception("Lack Of DAta in StreetBox");
                            //        }
                            //        //}
                            //        break;
                            //    case Atend.Control.Enum.ProductType.Transformer:
                            //        ed.WriteMessage("Sub Is : Transformer \n");
                            //        Atend.Base.Equipment.ETransformer SelectedEquipETransformer = Atend.Base.Equipment.ETransformer.SelectByXCodeForDesign(xCodeProduct, LocalConnection, LocalTransaction);
                            //        if (SelectedEquipETransformer.Code != -1)
                            //        {
                            //            //Atend.Base.Equipment.ETransformer answer = Atend.Base.Equipment.ETransformer.AccessSelectByXCode(SelectedEquipETransformer.XCode, _transaction, _connection);
                            //            //if (answer.Code != -1)
                            //            //{
                            //            //    NewCode = answer.Code;
                            //            //}
                            //            //else
                            //            //{
                            //            if (SelectedEquipETransformer.Code == 0)
                            //            {//confirm
                            //                if (SelectedEquipETransformer.ServerInsert(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                {
                            //                    NewCode = SelectedEquipETransformer.Code;
                            //                    if (!SelectedEquipETransformer.UpdateX(LocalTransaction, LocalConnection))
                            //                    {
                            //                        throw new System.Exception("UpdateX   Failed");
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    throw new System.Exception("ServerInsert failed" + Type.ToString());
                            //                }
                            //            }
                            //            else
                            //            {
                            //                Atend.Base.Equipment.ETransformer ServerSelectedTransform = Atend.Base.Equipment.ETransformer.ServerSelectByCode(SelectedEquipETransformer.Code, ServerConnection, ServerTransaction);
                            //                if (ServerSelectedTransform.Code != -1)
                            //                {
                            //                    if (SelectedEquipETransformer.ServerUpdate(ServerTransaction, ServerConnection, true, true, LocalTransaction, LocalConnection))
                            //                    {
                            //                        NewCode = SelectedEquipETransformer.Code;
                            //                    }
                            //                    else
                            //                    {
                            //                        throw new System.Exception("ServerUpdate failed" + Type.ToString());
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    throw new System.Exception("Transformer:There is no row on server for update " + Type.ToString());
                            //                }
                            //            }
                            //            //}
                            //        }
                            //        else
                            //        {
                            //            ed.WriteMessage(Type.ToString() + " does not exist \n");
                            //        }
                            //        break;

                        }
                        #endregion
                    }

                }
                catch (System.Exception ex)
                {
                    ed.WriteMessage("ERROR Convertor catch 1: {0} \n", ex.Message);
                    OldTransaction.Rollback();
                    OldConnection.Close();

                    NewTransaction.Rollback();
                    NewConnection.Close();
                    return false;
                }

                OldTransaction.Commit();
                OldConnection.Close();

                NewTransaction.Commit();
                NewConnection.Close();
            }
            catch (System.Exception ex1)
            {
                ed.WriteMessage("ERROR Convertor catch 2: {0} \n", ex1.Message);
                OldConnection.Close();
                NewConnection.Close();
                return false;
            }

            return true;
        }





    }
}
