﻿using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;

using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.Geometry;
//using Autodesk.AutoCAD.Interop;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.Windows;
using Autodesk.AutoCAD.Colors;

//get from tehran 7/15
namespace Atend.Global.Acad.DrawEquips
{
    public class AcDrawConsol
    {

        bool useAccess;
        public bool UseAccess
        {
            get { return useAccess; }
            set { useAccess = value; }
        }

        int existance;
        public int Existance
        {
            get { return existance; }
            set { existance = value; }
        }

        int projectCode;
        public int ProjectCode
        {
            get { return projectCode; }
            set { projectCode = value; }
        }

        Atend.Base.Equipment.EConsol _eConsol;
        public Atend.Base.Equipment.EConsol eConsol
        {
            get { return _eConsol; }
            set { _eConsol = value; }
        }

        int consolConut;
        public int ConsolConut
        {
            get { return consolConut; }
            set { consolConut = value; }
        }

        ObjectId parentOi;
        public ObjectId ParentOi
        {
            get { return parentOi; }
            set { parentOi = value; }
        }

        Atend.Base.Design.DPackage ConsolPack;


        class DrawConsolJig : DrawJig
        {

            List<Entity> Entities = new List<Entity>();
            public Point3d BasePoint = Point3d.Origin;
            public double Angle = 0;
            public bool TimeToGetAngle = false;
            public bool TimeToGetPosition = true;
            double MyScale = 1;

            public DrawConsolJig(double Scale)
            {
                MyScale = Scale;
            }

            protected override SamplerStatus Sampler(JigPrompts prompts)
            {
                //throw new System.Exception("The method or operation is not implemented.");
                if (TimeToGetPosition == true && TimeToGetAngle == false)
                {
                    JigPromptPointOptions ppo = new JigPromptPointOptions("Consol position ");
                    PromptPointResult ppr = prompts.AcquirePoint(ppo);

                    if (ppr.Status == PromptStatus.OK)
                    {
                        if (BasePoint.Equals(ppr.Value))
                        {
                            return SamplerStatus.NoChange;

                        }
                        else
                        {
                            BasePoint = ppr.Value;
                            return SamplerStatus.OK;
                        }
                    }
                    else
                    {
                        return SamplerStatus.Cancel;
                    }
                }
                else if (TimeToGetPosition == false && TimeToGetAngle == true)
                {
                    JigPromptAngleOptions pao = new JigPromptAngleOptions("consol angle");
                    pao.BasePoint = BasePoint;
                    pao.DefaultValue = 0;
                    pao.UseBasePoint = true;
                    PromptDoubleResult pdr = prompts.AcquireAngle(pao);
                    if (pdr.Status == PromptStatus.OK)
                    {
                        if (Angle.Equals(pdr.Value))
                        {
                            return SamplerStatus.NoChange;
                        }
                        else
                        {
                            Angle = pdr.Value;
                            return SamplerStatus.OK;
                        }
                    }
                    else
                    {
                        return SamplerStatus.OK;
                    }
                }
                return SamplerStatus.NoChange;
            }

            private Entity CreateInsulatorEntity(Point2d BasePoint)
            {

                double BaseX = BasePoint.X;
                double BaseY = BasePoint.Y;

                Atend.Global.Acad.AcadJigs.MyPolyLine pLine = new Atend.Global.Acad.AcadJigs.MyPolyLine();
                pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX - 5, BaseY - 5), 0, 0, 0);
                pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX + 5, BaseY - 5), 0, 0, 0);
                pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX + 5, BaseY + 5), 0, 0, 0);
                pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX - 5, BaseY + 5), 0, 0, 0);
                pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX - 5, BaseY - 5), 0, 0, 0);
                pLine.Closed = true;
                return pLine;

            }

            protected override bool WorldDraw(Autodesk.AutoCAD.GraphicsInterface.WorldDraw draw)
            {
                Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
                //throw new System.Exception("The method or operation is not implemented.");
                Entities.Clear();
                Entities.Add(CreateInsulatorEntity(new Point2d(BasePoint.X, BasePoint.Y)));


                Matrix3d trans1 = Matrix3d.Scaling(MyScale, new Point3d(BasePoint.X, BasePoint.Y, 0));
                foreach (Entity en in Entities)
                {
                    en.TransformBy(trans1);
                }
                if (TimeToGetAngle)
                {
                    Matrix3d trans = Matrix3d.Rotation(Angle, ed.CurrentUserCoordinateSystem.CoordinateSystem3d.Zaxis, new Point3d(BasePoint.X, BasePoint.Y, 0));
                    foreach (Entity en in Entities)
                    {
                        en.TransformBy(trans);
                    }
                }
                foreach (Entity ent in Entities)
                {
                    draw.Geometry.Draw(ent);
                }

                return true;
            }

            public List<Entity> GetEntities()
            {
                return Entities;
            }

        }


        public AcDrawConsol()
        {
            ConsolPack = new Atend.Base.Design.DPackage();
        }

        public void DrawConsol()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            Database db = Application.DocumentManager.MdiActiveDocument.Database;
            ObjectId ParentObjectId = ObjectId.Null;

            double MyScale = Atend.Base.Design.DProductProperties.AccessSelectBySoftwareCode((int)Atend.Control.Enum.ProductType.Consol).Scale;
            double MyCommentScaleConsol = Atend.Base.Design.DProductProperties.AccessSelectBySoftwareCode((int)Atend.Control.Enum.ProductType.Consol).CommentScale;

            using (DocumentLock docLock = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.LockDocument())
            {
                bool conti = true;
                //int i = 0;
                //ed.WriteMessage("SSSSSSSSSSSSSSSSS:{0}\n",MyScale);
                DrawConsolJig _DrawConsolJig = new DrawConsolJig(MyScale);
                System.Data.DataTable Parents = Atend.Global.Acad.UAcad.DetermineParent((int)Atend.Control.Enum.ProductType.Consol);
                while (conti)
                {
                    PromptResult pr;
                    pr = ed.Drag(_DrawConsolJig);
                    if (pr.Status == PromptStatus.OK && _DrawConsolJig.TimeToGetPosition)
                    {
                        //ed.WriteMessage("base point set \n");
                        System.Data.DataTable P = Atend.Global.Acad.Global.PointInsideWhichEntity(_DrawConsolJig.BasePoint);
                        foreach (DataRow dr in P.Rows)
                        {
                            DataRow[] drs = Parents.Select("SoftwareCode=" + dr["Type"].ToString());
                            if (drs.Length > 0)
                            {
                                ParentObjectId = new ObjectId(new IntPtr(int.Parse(dr["ObjectId"].ToString())));
                                break;
                            }
                        }
                        if (ParentObjectId != ObjectId.Null)
                        {
                            Atend.Base.Acad.AT_SUB PoleConsolSub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(ParentObjectId);
                            int consolCounter = 0;
                            foreach (ObjectId oi in PoleConsolSub.SubIdCollection)
                            {
                                Atend.Base.Acad.AT_INFO Consolinfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oi);
                                if (Consolinfo.NodeType == (int)Atend.Control.Enum.ProductType.Consol)
                                {
                                    consolCounter++;
                                }
                            }
                            if (consolCounter < 4)
                            {
                                _DrawConsolJig.TimeToGetAngle = true;
                                _DrawConsolJig.TimeToGetPosition = false;
                            }
                            else
                            {
                                Atend.Global.Acad.Notification notification = new Atend.Global.Acad.Notification();
                                notification.Title = "ترسیم پایه";
                                notification.Msg = "شما مجاز به ترسیم حداکثر چهار کنسول در این پایه می باشید";
                                notification.infoCenterBalloon();

                            }
                        }

                    }
                    else if (pr.Status == PromptStatus.OK && !_DrawConsolJig.TimeToGetPosition)
                    {
                        //pr = ed.Drag(_DrawConsolJig);
                        //if (pr.Status == PromptStatus.OK)
                        //{
                        _DrawConsolJig.TimeToGetAngle = false;
                        _DrawConsolJig.TimeToGetPosition = false;
                        conti = false;

                        #region save data here
                        List<Entity> Entities = _DrawConsolJig.GetEntities();
                        if (ParentObjectId != ObjectId.Null)
                        {
                            ParentOi = ParentObjectId;
                            if (SaveConsolData(true))
                            {
                                ed.WriteMessage("come back from save data \n");
                                foreach (Entity ent in Entities)
                                {
                                    ///////////////////////////////////////////////////////////
                                    bool IsWeek = false;
                                    switch (eConsol.VoltageLevel)
                                    {
                                        case 20000:
                                            IsWeek = false;
                                            break;
                                        case 11000:
                                            IsWeek = false;
                                            break;
                                        case 33000:
                                            IsWeek = false;
                                            break;
                                        case 400:
                                            IsWeek = true;
                                            break;
                                    }
                                    string LayerName;
                                    if (IsWeek)
                                    {
                                        LayerName = Atend.Control.Enum.AutoCadLayerName.LOW_AIR.ToString();
                                    }
                                    else
                                    {
                                        LayerName = Atend.Control.Enum.AutoCadLayerName.MED_AIR.ToString();
                                    }

                                    Atend.Base.Acad.AT_INFO PoleInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(ParentObjectId);
                                    Atend.Base.Design.DConsol consol = new Atend.Base.Design.DConsol();
                                    //ed.WriteMessage("ConsolCount= " + dConsolCode.Count.ToString() + "\n");
                                    consol.Code = ConsolPack.Code;
                                    consol.LoadCode = 0;
                                    consol.ProductCode = eConsol.Code;
                                    consol.ParentCode = new Guid(PoleInfo.NodeCode);
                                    if (consol.AccessInsert())
                                    {
                                        ObjectId NewConsolObjectID = Atend.Global.Acad.UAcad.DrawEntityOnScreen(ent, LayerName);

                                        Atend.Base.Acad.AT_INFO at_info = new Atend.Base.Acad.AT_INFO(NewConsolObjectID);
                                        at_info.ParentCode = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(ParentOi).NodeCode; //ConsolPack.NodeCode.ToString();
                                        at_info.NodeCode = ConsolPack.Code.ToString();
                                        at_info.NodeType = (int)Atend.Control.Enum.ProductType.Consol;
                                        at_info.ProductCode = ConsolPack.ProductCode;
                                        at_info.Insert();


                                        ObjectId TextOi = Atend.Global.Acad.UAcad.DrawEntityOnScreen(Atend.Global.Acad.UAcad.WriteNote(ConsolPack.Number, ((Polyline)Atend.Global.Acad.UAcad.GetEntityByObjectID(ent.ObjectId)).GetPoint3dAt(0), MyCommentScaleConsol), Atend.Control.Enum.AutoCadLayerName.GENERAL.ToString());
                                        Atend.Base.Acad.AT_INFO textInfo = new Atend.Base.Acad.AT_INFO(TextOi);
                                        textInfo.ParentCode = at_info.NodeCode;
                                        textInfo.NodeType = (int)Atend.Control.Enum.ProductType.Comment;
                                        textInfo.NodeCode = "";
                                        textInfo.ProductCode = 0;
                                        textInfo.Insert();


                                        Atend.Base.Acad.AT_SUB at_sub = new Atend.Base.Acad.AT_SUB();
                                        at_sub.SelectedObjectId = NewConsolObjectID;
                                        at_sub.SubIdCollection.Add(ParentObjectId);
                                        at_sub.SubIdCollection.Add(TextOi);
                                        at_sub.Insert();


                                        Atend.Base.Acad.AT_SUB textSub = new Atend.Base.Acad.AT_SUB(TextOi);
                                        textSub.SubIdCollection.Add(NewConsolObjectID);
                                        textSub.Insert();

                                        Atend.Base.Acad.AT_SUB.AddToAT_SUB(NewConsolObjectID, ParentOi);

                                    }
                                    ////////////////////////////////////////////
                                }
                            }
                        }

                        #endregion
                    }
                    else
                    {
                        conti = false;
                    }
                }
            }

        }

        public static bool DeleteConsol(ObjectId PoleOI, String ConsolCode)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            //ed.writeMessage("~~~~~~~~~~~Start Graphic Delete Consol ~~~~~~~~~~~~~~~\n");
            try
            {
                Atend.Base.Acad.AT_SUB poleSub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(PoleOI);
                foreach (ObjectId oi in poleSub.SubIdCollection)
                {

                    Atend.Base.Acad.AT_INFO ConsolInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oi);
                    if (ConsolInfo.ParentCode != "NONE" && ConsolInfo.NodeType == (int)Atend.Control.Enum.ProductType.Consol && ConsolInfo.NodeCode == ConsolCode)
                    {
                        Atend.Base.Acad.AT_SUB ConsolSub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(oi);
                        foreach (ObjectId oii in ConsolSub.SubIdCollection)
                        {
                            Atend.Base.Acad.AT_INFO ConsolSubInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oii);
                            if (ConsolSubInfo.ParentCode != "NONE" && ConsolSubInfo.NodeType == (int)Atend.Control.Enum.ProductType.Conductor)
                            {
                                ObjectId NextConsolOI = UAcad.GetNextConsol(oi, new Guid(ConsolSubInfo.NodeCode));
                                //ed.writeMessage("Graphic Next Consol OI:{0}\n", NextConsolOI);

                                Atend.Base.Acad.AT_SUB NextConsolSub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(NextConsolOI);
                                foreach (ObjectId NextConsolSubOI in NextConsolSub.SubIdCollection)
                                {
                                    Atend.Base.Acad.AT_INFO NextConsolSubInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(NextConsolSubOI);
                                    if (NextConsolSubInfo.ParentCode != "NONE" && NextConsolSubInfo.NodeType == (int)Atend.Control.Enum.ProductType.TensionArc)
                                    {
                                        //ed.writeMessage("Graphic YES IT HAS ARC \n");
                                        if (!Atend.Global.Acad.AcadRemove.DeleteEntityByObjectId(NextConsolSubOI))
                                        {
                                            throw new System.Exception("While delete arc from next consol \n");
                                        }
                                        else
                                        {
                                            Atend.Base.Acad.AT_SUB.RemoveFromAT_SUB(NextConsolSubOI, NextConsolOI);
                                        }
                                    }
                                }
                            }
                            else if (ConsolSubInfo.ParentCode != "NONE" && ConsolSubInfo.NodeType == (int)Atend.Control.Enum.ProductType.Comment)
                            {
                                if (!Atend.Global.Acad.AcadRemove.DeleteEntityByObjectId(oii))
                                {
                                    throw new System.Exception("While delete arc from next consol \n");
                                }
                                else
                                {
                                    Atend.Base.Acad.AT_SUB.RemoveFromAT_SUB(oii, oi);
                                }

                            }
                            else if (ConsolSubInfo.ParentCode != "NONE" && ConsolSubInfo.NodeType == (int)Atend.Control.Enum.ProductType.Comment)
                            {
                                if (!Atend.Global.Acad.AcadRemove.DeleteEntityByObjectId(oii))
                                {
                                    throw new System.Exception("While delete arc from next consol \n");
                                }
                                else
                                {
                                    Atend.Base.Acad.AT_SUB.RemoveFromAT_SUB(oii, oi);
                                }

                            }

                        }
                        foreach (ObjectId oii in ConsolSub.SubIdCollection)
                        {

                            Atend.Base.Acad.AT_INFO ConsolSubInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oii);
                            if (ConsolSubInfo.ParentCode != "NONE" && ConsolSubInfo.NodeType == (int)Atend.Control.Enum.ProductType.Conductor)
                            {
                                Atend.Global.Acad.DrawEquips.AcDrawConductor.DeleteConductor(oii);
                            }
                            if (ConsolSubInfo.ParentCode != "NONE" && ConsolSubInfo.NodeType == (int)Atend.Control.Enum.ProductType.TensionArc)
                            {
                                if (!Atend.Global.Acad.DrawEquips.AcDrawConductor.DeleteConductor(oii))
                                {
                                    throw new System.Exception("While delete tension arc\n");
                                }
                            }

                        }
                        if (!Atend.Global.Acad.AcadRemove.DeleteEntityByObjectId(oi))
                        {
                            throw new System.Exception("while delete consol \n");
                        }
                        else
                        {
                            //ed.writeMessage("**************AT_SUB.RemoveFromAT_SUB\n");
                            Atend.Base.Acad.AT_SUB.RemoveFromAT_SUB(oi, PoleOI);
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                //ed.WriteMessage("~~~~~~~~~~~End Graphic Delete Consol~~~~~~~~~~~~~~~\n");
                ed.WriteMessage("Graphic ERROR DeLeteConsol :" + ex.Message + "\n");
                return false;
            }
            //ed.writeMessage("~~~~~~~~~~~End Graphic Delete Consol~~~~~~~~~~~~~~~\n");
            return true;
        }

        public static bool DeleteConsolData(ObjectId ConsolOI)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;

            OleDbTransaction _Transaction;
            OleDbConnection _Connection = new OleDbConnection(Atend.Control.ConnectionString.AccessCnString);
            try
            {
                _Connection.Open();
                _Transaction = _Connection.BeginTransaction();
                try
                {
                    Atend.Base.Acad.AT_SUB SubGP = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(ConsolOI);
                    foreach (ObjectId oi in SubGP.SubIdCollection)
                    {
                        Atend.Base.Acad.AT_INFO atinfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oi);
                        if (atinfo.ParentCode != "NONE" && (atinfo.NodeType == (int)Atend.Control.Enum.ProductType.Pole || atinfo.NodeType == (int)Atend.Control.Enum.ProductType.PoleTip))
                        {
                            Atend.Base.Acad.AT_SUB poleSub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(oi);
                            foreach (ObjectId oisub in poleSub.SubIdCollection)
                            {
                                Atend.Base.Acad.AT_INFO atinfo2 = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oisub);
                                if (atinfo2.ParentCode != "NONE" && atinfo2.NodeType == (int)Atend.Control.Enum.ProductType.Consol && atinfo2.SelectedObjectId == ConsolOI)
                                {
                                    Atend.Base.Acad.AT_SUB consolSub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(oisub);
                                    foreach (ObjectId oiconsolsub in consolSub.SubIdCollection)
                                    {
                                        Atend.Base.Acad.AT_INFO atinfo4 = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oiconsolsub);
                                        if (atinfo4.ParentCode != "NONE" && atinfo4.NodeType == (int)Atend.Control.Enum.ProductType.Conductor)
                                        {
                                            if (!Atend.Base.Design.DBranch.AccessDelete(new Guid(atinfo4.NodeCode.ToString()), _Transaction, _Connection))
                                            {
                                                throw new System.Exception("Error In Delete dbranch\n");
                                            }

                                            //delete jumper
                                            System.Data.DataTable dt = Atend.Base.Design.DBranch.AccessSelectByLeftOrRightNodeCode(new Guid(atinfo4.NodeCode.ToString()));
                                            foreach (DataRow dr in dt.Rows)
                                            {
                                                if (!Atend.Base.Design.DBranch.AccessDelete(new Guid(dr["Code"].ToString()), _Transaction, _Connection))
                                                {
                                                    throw new System.Exception("Error In Delete dbranch jumper\n");
                                                }
                                            }
                                        }
                                    }
                                    if (!Atend.Base.Design.DConsol.AccessDelete(new Guid(atinfo2.NodeCode.ToString()), _Transaction, _Connection))
                                    {
                                        throw new System.Exception("Error In Delete DConsol\n");
                                    }
                                    if (!Atend.Base.Design.DPackage.AccessDelete(new Guid(atinfo2.NodeCode.ToString()), _Transaction, _Connection))
                                    {
                                        throw new System.Exception("Error In Delete dpackage00\n");
                                    }
                                }
                            }
                        }
                        if (atinfo.ParentCode != "NONE" && (atinfo.NodeType == (int)Atend.Control.Enum.ProductType.Disconnector || atinfo.NodeType == (int)Atend.Control.Enum.ProductType.Breaker || atinfo.NodeType == (int)Atend.Control.Enum.ProductType.CatOut))
                        {
                            if (!Atend.Base.Design.DPackage.AccessDelete(new Guid(atinfo.NodeCode.ToString()), _Transaction, _Connection))
                            {
                                throw new System.Exception("Error In Delete dpackage01\n");
                            }
                        }
                        if (atinfo.ParentCode != "NONE" && atinfo.NodeType == (int)Atend.Control.Enum.ProductType.Terminal)
                        {
                            if (!Atend.Base.Design.DBranch.AccessDelete(new Guid(atinfo.NodeCode.ToString()), _Transaction, _Connection))
                            {
                                throw new System.Exception("Error In Delete dbranch\n");
                            }
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    ed.WriteMessage("Data ERROR Consol(Transaction) : {0} \n", ex.Message);
                    _Transaction.Rollback();
                    return false;
                }
            }
            catch (System.Exception ex1)
            {
                ed.WriteMessage(string.Format("Error in Transaction : {0} \n", ex1.Message));
                _Connection.Close();
                return false;
            }
            _Transaction.Commit();
            _Connection.Close();
            return true;
        }

        public static bool DeleteConsolData(ObjectId ConsolOI, OleDbTransaction _Transaction, OleDbConnection _Connection)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            try
            {
                Atend.Base.Acad.AT_SUB SubGP = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(ConsolOI);
                foreach (ObjectId oi in SubGP.SubIdCollection)
                {
                    Atend.Base.Acad.AT_INFO atinfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oi);
                    if (atinfo.ParentCode != "NONE" && (atinfo.NodeType == (int)Atend.Control.Enum.ProductType.Pole || atinfo.NodeType == (int)Atend.Control.Enum.ProductType.PoleTip))
                    {
                        Atend.Base.Acad.AT_SUB poleSub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(oi);
                        foreach (ObjectId oisub in poleSub.SubIdCollection)
                        {
                            Atend.Base.Acad.AT_INFO atinfo2 = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oisub);
                            if (atinfo2.ParentCode != "NONE" && atinfo2.NodeType == (int)Atend.Control.Enum.ProductType.Consol && atinfo2.SelectedObjectId == ConsolOI)
                            {
                                Atend.Base.Acad.AT_SUB consolSub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(oisub);
                                foreach (ObjectId oiconsolsub in consolSub.SubIdCollection)
                                {
                                    Atend.Base.Acad.AT_INFO atinfo4 = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oiconsolsub);
                                    if (atinfo4.ParentCode != "NONE" && atinfo4.NodeType == (int)Atend.Control.Enum.ProductType.Conductor)
                                    {
                                        if (!Atend.Base.Design.DBranch.AccessDelete(new Guid(atinfo4.NodeCode.ToString()), _Transaction, _Connection))
                                        {
                                            throw new System.Exception("Error In Delete dbranch\n");
                                        }

                                        //delete jumper
                                        System.Data.DataTable dt = Atend.Base.Design.DBranch.AccessSelectByLeftOrRightNodeCode(new Guid(atinfo4.NodeCode.ToString()));
                                        foreach (DataRow dr in dt.Rows)
                                        {
                                            if (!Atend.Base.Design.DBranch.AccessDelete(new Guid(dr["Code"].ToString()), _Transaction, _Connection))
                                            {
                                                throw new System.Exception("Error In Delete dbranch jumper\n");
                                            }
                                        }
                                    }
                                }
                                if (!Atend.Base.Design.DConsol.AccessDelete(new Guid(atinfo2.NodeCode.ToString()), _Transaction, _Connection))
                                {
                                    throw new System.Exception("Error In Delete DConsol\n");
                                }
                                if (!Atend.Base.Design.DPackage.AccessDelete(new Guid(atinfo2.NodeCode.ToString()), _Transaction, _Connection))
                                {
                                    throw new System.Exception("Error In Delete dpackage00\n");
                                }
                            }

                        }


                    }
                    if (atinfo.ParentCode != "NONE" && (atinfo.NodeType == (int)Atend.Control.Enum.ProductType.Disconnector || atinfo.NodeType == (int)Atend.Control.Enum.ProductType.Breaker || atinfo.NodeType == (int)Atend.Control.Enum.ProductType.CatOut))
                    {
                        if (!Atend.Base.Design.DPackage.AccessDelete(new Guid(atinfo.NodeCode.ToString()), _Transaction, _Connection))
                        {
                            throw new System.Exception("Error In Delete dpackage01\n");
                        }
                    }
                    if (atinfo.ParentCode != "NONE" && atinfo.NodeType == (int)Atend.Control.Enum.ProductType.Terminal)
                    {
                        if (!Atend.Base.Design.DBranch.AccessDelete(new Guid(atinfo.NodeCode.ToString()), _Transaction, _Connection))
                        {
                            throw new System.Exception("Error In Delete dbranch\n");
                        }
                    }
                }
            }
            catch (System.Exception ex1)
            {
                ed.WriteMessage(string.Format("Error in Transaction : {0} \n", ex1.Message));
                _Transaction.Rollback();
                return false;
            }
            return true;
        }

        public static bool DeleteConsol(ObjectId ConsolOI)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            try
            {
                Atend.Base.Acad.AT_SUB ConsolSub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(ConsolOI);
                foreach (ObjectId ConsolSubOI in ConsolSub.SubIdCollection)
                {
                    Atend.Base.Acad.AT_INFO ConsolSubInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(ConsolSubOI);
                    if (ConsolSubInfo.ParentCode != "NONE" && ConsolSubInfo.NodeType == (int)Atend.Control.Enum.ProductType.Comment)
                    {
                        if (!Atend.Global.Acad.AcadRemove.DeleteEntityByObjectId(ConsolSubOI))
                        {
                            throw new System.Exception("Error In Delete Comment\n");
                        }

                    }
                    if (ConsolSubInfo.ParentCode != "NONE" && ConsolSubInfo.NodeType == (int)Atend.Control.Enum.ProductType.TensionArc)
                    {
                        if (!Atend.Global.Acad.AcadRemove.DeleteEntityByObjectId(ConsolSubOI))
                        {
                            throw new System.Exception("Error In Delete Arc\n");
                        }

                    }
                    if (ConsolSubInfo.ParentCode != "NONE" && ConsolSubInfo.NodeType == (int)Atend.Control.Enum.ProductType.Conductor)
                    {
                        if (!Atend.Global.Acad.DrawEquips.AcDrawConductor.DeleteConductor(ConsolSubOI))
                        {
                            throw new System.Exception("Error In Delete Conductor\n");
                        }

                    }
                    if (ConsolSubInfo.ParentCode != "NONE" && ConsolSubInfo.NodeType == (int)Atend.Control.Enum.ProductType.Terminal)
                    {
                        if (!Atend.Global.Acad.DrawEquips.AcDrawTerminal.DeleteTerminal(ConsolSubOI))
                        {
                            throw new System.Exception("Error In Delete Terminal\n");
                        }

                    }
                    if (ConsolSubInfo.ParentCode != "NONE" && ConsolSubInfo.NodeType == (int)Atend.Control.Enum.ProductType.Breaker)
                    {
                        ObjectIdCollection CollectionBreaker = Atend.Global.Acad.UAcad.GetGroupSubEntities(ConsolSubOI);
                        foreach (ObjectId collect in CollectionBreaker)
                        {
                            Atend.Base.Acad.AT_INFO at_infoo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(collect);
                            if (at_infoo.ParentCode != "NONE" && at_infoo.NodeType == (int)Atend.Control.Enum.ProductType.Terminal)
                            {
                                if (!Atend.Global.Acad.DrawEquips.AcDrawBreaker.DeleteBreaker(collect))
                                {
                                    throw new System.Exception("Error In Delete Breaker\n");
                                }
                                if (!Atend.Global.Acad.AcadRemove.DeleteEntityByObjectId(collect))
                                {
                                    throw new System.Exception("Error In Delete Breaker2\n");
                                }
                            }
                        }
                    }
                    if (ConsolSubInfo.ParentCode != "NONE" && ConsolSubInfo.NodeType == (int)Atend.Control.Enum.ProductType.Disconnector)
                    {
                        ObjectIdCollection CollectionDisconnector = Atend.Global.Acad.UAcad.GetGroupSubEntities(ConsolSubOI);
                        foreach (ObjectId collect in CollectionDisconnector)
                        {
                            Atend.Base.Acad.AT_INFO at_infoo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(collect);
                            if (at_infoo.ParentCode != "NONE" && at_infoo.NodeType == (int)Atend.Control.Enum.ProductType.Terminal)
                            {
                                if (!Atend.Global.Acad.DrawEquips.AcDrawDisConnector.DeleteDisconnector(collect))
                                {
                                    throw new System.Exception("Error In Delete Disconnector\n");
                                }
                                if (!Atend.Global.Acad.AcadRemove.DeleteEntityByObjectId(collect))
                                {
                                    throw new System.Exception("Error In Delete Disconnector2\n");
                                }
                            }
                        }
                    }
                    if (ConsolSubInfo.ParentCode != "NONE" && ConsolSubInfo.NodeType == (int)Atend.Control.Enum.ProductType.CatOut)
                    {
                        //ed.WriteMessage("catout\n");
                        ObjectIdCollection CollectionDisconnector = Atend.Global.Acad.UAcad.GetGroupSubEntities(ConsolSubOI);
                        foreach (ObjectId collect in CollectionDisconnector)
                        {
                            Atend.Base.Acad.AT_INFO at_infoo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(collect);
                            if (at_infoo.ParentCode != "NONE" && at_infoo.NodeType == (int)Atend.Control.Enum.ProductType.Terminal)
                            {
                                //ed.WriteMessage("1\n");
                                if (!Atend.Global.Acad.DrawEquips.AcDrawCatOut.DeleteCatOut(collect))
                                {
                                    throw new System.Exception("Error In Delete CatOut\n");
                                }
                                if (!Atend.Global.Acad.AcadRemove.DeleteEntityByObjectId(collect))
                                {
                                    throw new System.Exception("Error In Delete CatOut2\n");
                                }
                            }
                        }
                    }
                    if (ConsolSubInfo.ParentCode != "NONE" && (ConsolSubInfo.NodeType == (int)Atend.Control.Enum.ProductType.Pole || ConsolSubInfo.NodeType == (int)Atend.Control.Enum.ProductType.PoleTip))
                    {
                        Atend.Base.Acad.AT_SUB consolSub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(ConsolSubOI);
                        foreach (ObjectId oi in consolSub.SubIdCollection)
                        {
                            Atend.Base.Acad.AT_INFO at_info = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oi);
                            //if (at_info.ParentCode != "NONE" && at_info.NodeType == (int)Atend.Control.Enum.ProductType.TensionArc)
                            //{
                            //    if (!Atend.Global.Acad.AcadRemove.DeleteEntityByObjectId(oi))
                            //    {
                            //        throw new System.Exception("Error In Delete Arc\n");
                            //    }
                            //    else
                            //    {
                            //        Atend.Base.Acad.AT_SUB.RemoveFromAT_SUB(oi, ConsolSubOI);
                            //    }
                            //}
                            if (at_info.ParentCode != "NONE" && at_info.NodeType == (int)Atend.Control.Enum.ProductType.Consol && at_info.SelectedObjectId == ConsolOI)
                            {
                                Atend.Base.Acad.AT_SUB.RemoveFromAT_SUB(oi, ConsolSubOI);
                            }
                        }
                    }
                }

                //+++
                if (!Atend.Global.Acad.AcadRemove.DeleteEntityByObjectId(ConsolOI))
                {
                    throw new System.Exception("GRA while delete conductor \n");
                }

            }
            catch (System.Exception ex)
            {
                ed.WriteMessage("GRA ERROR Consol : {0} \n", ex.Message);
                return false;
            }
            return true;
        }

        public static void insertConsol(ObjectId PoleOI, ArrayList ConsolGuidList)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            Entity PoleEntity = Atend.Global.Acad.UAcad.GetEntityByObjectID(PoleOI);
            Point3d PoleCentre = Atend.Global.Acad.UAcad.CenterOfEntity(PoleEntity);
            ed.WriteMessage("~~~~~~~~~~ ConsolGuidList:{0}\n", ConsolGuidList.Count);
            ed.WriteMessage("~~~~~~~PoleCentre:{0}\n", PoleCentre);
            ed.WriteMessage("PoleOI={0}, arrCount={1}\n", PoleOI.ToString(), ConsolGuidList.Count);
            double MyScale = Atend.Base.Design.DProductProperties.AccessSelectBySoftwareCode((int)Atend.Control.Enum.ProductType.Consol).Scale;
            double MyCommentScale = Atend.Base.Design.DProductProperties.AccessSelectBySoftwareCode((int)Atend.Control.Enum.ProductType.Consol).CommentScale;

            int ConsolCount = ConsolGuidList.Count;
            ObjectIdCollection allSubs = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(PoleOI).SubIdCollection;
            foreach (ObjectId oi in allSubs)
            {
                Atend.Base.Acad.AT_INFO SubInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oi);
                if (SubInfo.ParentCode != "NONE" && SubInfo.NodeType == (int)Atend.Control.Enum.ProductType.Consol)
                    ConsolCount++;
            }
            ed.WriteMessage("CONSO COUNT : {0} \n", ConsolCount);
            Point3dCollection ConsolPoints;
            CalculateConsolPoints(PoleEntity, ConsolCount, out  ConsolPoints);
            ed.WriteMessage("CONSO POINTS COUNT : {0} \n", ConsolPoints.Count);
            if (ConsolPoints.Count >= ConsolGuidList.Count)
            {
                int ConsolCounter = 0;
                for (int i = 0; i < ConsolGuidList.Count; i++)
                {
                    ed.WriteMessage("ConsolGuidList[i]= " + ConsolGuidList[i].ToString() + "\n");
                    Atend.Base.Design.DPackage CurrentConsol =
                    Atend.Base.Design.DPackage.AccessSelectByCode(new Guid(ConsolGuidList[i].ToString()));
                    ed.WriteMessage("CurrentConsol]a= " + CurrentConsol.Code.ToString() + "\n");

                    Atend.Base.Equipment.EConsol CurrentConsolInformation = Atend.Base.Equipment.EConsol.AccessSelectByCode(CurrentConsol.ProductCode);
                    ed.WriteMessage("1" + "\n");
                    bool IsWeek = false;
                    switch (CurrentConsolInformation.VoltageLevel)
                    {
                        case 20000:
                            IsWeek = false;
                            break;
                        case 11000:
                            IsWeek = false;
                            break;
                        case 33000:
                            IsWeek = false;
                            break;
                        case 400:
                            IsWeek = true;
                            break;
                    }
                    string LayerName = "";
                    if (IsWeek)
                    {
                        LayerName = Atend.Control.Enum.AutoCadLayerName.LOW_AIR.ToString();
                    }
                    else
                    {
                        LayerName = Atend.Control.Enum.AutoCadLayerName.MED_AIR.ToString();
                    }

                    //DRAW NEW CONSOL

                    ed.WriteMessage("PPP:{0}\n", ConsolPoints[ConsolCounter]);
                    Entity ent = DrawConsol(ConsolPoints[ConsolCounter], 10, 10, MyScale);
                    ConsolCounter++;
                    ObjectId NewConsol = Atend.Global.Acad.UAcad.DrawEntityOnScreen(ent, LayerName);

                    Atend.Base.Acad.AT_INFO NewConsolInfo = new Atend.Base.Acad.AT_INFO(NewConsol);
                    NewConsolInfo.ParentCode = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(PoleOI).NodeCode;
                    NewConsolInfo.NodeCode = ConsolGuidList[0].ToString();
                    NewConsolInfo.NodeType = (int)Atend.Control.Enum.ProductType.Consol;
                    ed.WriteMessage("CurrentConsol.ProductCode={0}\n", CurrentConsol.ProductCode);
                    NewConsolInfo.ProductCode = CurrentConsol.ProductCode;
                    NewConsolInfo.Insert();

                    Atend.Base.Acad.AT_SUB.AddToAT_SUB(NewConsol, PoleOI);

                    //DRAW CONSOL TEXT 
                    ObjectId TextOi = Atend.Global.Acad.UAcad.DrawEntityOnScreen(Atend.Global.Acad.UAcad.WriteNote(CurrentConsol.Number, ((Polyline)ent).GetPoint3dAt(0), MyCommentScale), Atend.Control.Enum.AutoCadLayerName.GENERAL.ToString());

                    Atend.Base.Acad.AT_INFO textInfo = new Atend.Base.Acad.AT_INFO(TextOi);
                    textInfo.ParentCode = ConsolGuidList[0].ToString();
                    textInfo.NodeType = (int)Atend.Control.Enum.ProductType.Comment;
                    textInfo.NodeCode = "";
                    textInfo.ProductCode = 0;
                    textInfo.Insert();

                    Atend.Base.Acad.AT_SUB TextSub = new Atend.Base.Acad.AT_SUB(TextOi);
                    TextSub.SubIdCollection.Add(PoleOI);
                    TextSub.Insert();

                    Atend.Base.Acad.AT_SUB s = new Atend.Base.Acad.AT_SUB(NewConsol);
                    s.SubIdCollection.Add(PoleOI);
                    s.SubIdCollection.Add(TextOi);
                    s.Insert();
                }
            }
        }

        private static Entity DrawConsol(Point3d BasePoint, double Width, double Height, double Scale)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            double BaseX = BasePoint.X;
            double BaseY = BasePoint.Y;

            Polyline pLine = new Polyline();
            pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX - (Width / 2), BaseY - (Height / 2)), 0, 0, 0);
            pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX + (Width / 2), BaseY - (Height / 2)), 0, 0, 0);
            pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX + (Width / 2), BaseY + (Height / 2)), 0, 0, 0);
            pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX - (Width / 2), BaseY + (Height / 2)), 0, 0, 0);
            pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX - (Width / 2), BaseY - (Height / 2)), 0, 0, 0);
            pLine.Closed = true;


            Matrix3d trans1 = Matrix3d.Scaling(Scale, new Point3d(BasePoint.X, BasePoint.Y, 0));
            pLine.TransformBy(trans1);
            //ed.WriteMessage("~~~~~~~~~CONSOL BasePoint : {0} \n", BasePoint);
            //Matrix3d trans = Matrix3d.Rotation(RadianAngle, ed.CurrentUserCoordinateSystem.CoordinateSystem3d.Zaxis, new Point3d(CenterPoint.X, CenterPoint.Y, 0));
            //foreach (Entity en in Entities)
            //{
            //    en.TransformBy(trans);
            //}


            return pLine;

        }

        public static void RotateConsol(double LastAngleDegree, double NewAngleDegree, ObjectId PoleOI, ObjectId ConsolOI, Point3d CenterPoint)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            using (DocumentLock docLock = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.LockDocument())
            {

                Point3d LastCenterPoint = Atend.Global.Acad.UAcad.CenterOfEntity(Atend.Global.Acad.UAcad.GetEntityByObjectID(PoleOI));
                Point3d newcp = new Point3d(LastCenterPoint.X + .00001, LastCenterPoint.Y + .00001, LastCenterPoint.Z + .00001);
                Matrix3d mat = Matrix3d.Displacement(newcp - LastCenterPoint);

                Database db = Application.DocumentManager.MdiActiveDocument.Database;
                Atend.Global.Acad.AcadMove.LastCenterPoint = Atend.Global.Acad.UAcad.CenterOfEntity(Atend.Global.Acad.UAcad.GetEntityByObjectID(ConsolOI));
                using (Transaction tr = db.TransactionManager.StartTransaction())
                {
                    Entity ent = (Entity)tr.GetObject(ConsolOI, OpenMode.ForWrite);
                    Polyline LineEntity = ent as Polyline;
                    if (LineEntity != null)
                    {
                        //ed.WriteMessage("LineEntity entity found \n");
                        Matrix3d trans = Matrix3d.Rotation(((LastAngleDegree * Math.PI) / 180) * -1, ed.CurrentUserCoordinateSystem.CoordinateSystem3d.Zaxis, CenterPoint);
                        LineEntity.TransformBy(trans);

                        trans = Matrix3d.Rotation(((NewAngleDegree * Math.PI) / 180), ed.CurrentUserCoordinateSystem.CoordinateSystem3d.Zaxis, CenterPoint);
                        LineEntity.TransformBy(trans);

                        Matrix3d m = new Matrix3d();
                        Atend.Global.Acad.AcadMove.ConsolOI = ConsolOI;
                        Atend.Global.Acad.AcadMove.isMoveConsolOnly = true;
                        Atend.Global.Acad.AcadMove.AllowToMove = true;
                        Atend.Global.Acad.AcadMove.MoveConsol(PoleOI, ConsolOI, m);


                        Atend.Global.Acad.AcadMove.EntityInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(PoleOI);
                        Atend.Global.Acad.AcadMove.EntitySub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(PoleOI);

                        Atend.Global.Acad.AcadMove.PoleOI = PoleOI;
                        Atend.Global.Acad.AcadMove.CurrentEntity = Atend.Global.Acad.UAcad.GetEntityByObjectID(PoleOI);
                        Atend.Global.Acad.AcadMove.LastCenterPoint =
                            Atend.Global.Acad.UAcad.CenterOfEntity(Atend.Global.Acad.UAcad.GetEntityByObjectID(PoleOI));
                        Atend.Global.Acad.AcadMove.swBreaker = true;
                        Atend.Global.Acad.AcadMove.swDisconnector = true;
                        Atend.Global.Acad.AcadMove.swCatOut = true;

                        Atend.Global.Acad.AcadMove.MovePole(PoleOI);

                    }
                    tr.Commit();
                }
            }
        }

        public void DrawConsol(Point3d BasePoint, double RadianAngle, ObjectId ParentObjectId, bool UseParentOi)
        {

            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            Point3d PoleCentre = BasePoint;
            double MyScale = Atend.Base.Design.DProductProperties.AccessSelectBySoftwareCode((int)Atend.Control.Enum.ProductType.Consol).Scale;
            double MyCommentScale = Atend.Base.Design.DProductProperties.AccessSelectBySoftwareCode((int)Atend.Control.Enum.ProductType.Consol).CommentScale;
            Entity NewConsolEntity = DrawConsol(BasePoint, 10, 10, MyScale);

            Atend.Base.Equipment.EConsol CurrentConsolInformation = eConsol;
            bool IsWeek = false;
            switch (CurrentConsolInformation.VoltageLevel)
            {
                case 20000:
                    IsWeek = false;
                    break;
                case 11000:
                    IsWeek = false;
                    break;
                case 33000:
                    IsWeek = false;
                    break;
                case 400:
                    IsWeek = true;
                    break;
            }
            string LayerName = "";
            if (IsWeek)
            {
                LayerName = Atend.Control.Enum.AutoCadLayerName.LOW_AIR.ToString();
            }
            else
            {
                LayerName = Atend.Control.Enum.AutoCadLayerName.MED_AIR.ToString();
            }

            //DRAW NEW CONSOL
            Entity ent = DrawConsol(new Point3d(PoleCentre.X, PoleCentre.Y, PoleCentre.Z), 10, 10, MyScale);
            Matrix3d trans = Matrix3d.Rotation(RadianAngle, ed.CurrentUserCoordinateSystem.CoordinateSystem3d.Zaxis, new Point3d(BasePoint.X, BasePoint.Y, 0));
            ent.TransformBy(trans);
            if (SaveConsolData(UseParentOi))
            {
                ObjectId NewConsol = Atend.Global.Acad.UAcad.DrawEntityOnScreen(ent, LayerName);
                Atend.Base.Acad.AT_INFO NewConsolInfo = new Atend.Base.Acad.AT_INFO(NewConsol);
                if (UseParentOi)
                {
                    NewConsolInfo.ParentCode = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(ParentOi).NodeCode;
                }
                else
                {
                    NewConsolInfo.ParentCode = "";
                }
                NewConsolInfo.NodeCode = ConsolPack.Code.ToString();
                NewConsolInfo.NodeType = (int)Atend.Control.Enum.ProductType.Consol;
                //ed.WriteMessage("CurrentConsol.ProductCode={0}\n", CurrentConsol.ProductCode);
                NewConsolInfo.ProductCode = eConsol.ProductCode;
                NewConsolInfo.Insert();



                //DRAW CONSOL TEXT 
                ObjectId TextOi = Atend.Global.Acad.UAcad.DrawEntityOnScreen(Atend.Global.Acad.UAcad.WriteNote(ConsolPack.Number, ((Polyline)ent).GetPoint3dAt(0), MyCommentScale), Atend.Control.Enum.AutoCadLayerName.GENERAL.ToString());

                Atend.Base.Acad.AT_INFO textInfo = new Atend.Base.Acad.AT_INFO(TextOi);
                textInfo.ParentCode = ConsolPack.Code.ToString();
                textInfo.NodeType = (int)Atend.Control.Enum.ProductType.Comment;
                textInfo.NodeCode = "";
                textInfo.ProductCode = 0;
                textInfo.Insert();

                Atend.Base.Acad.AT_SUB TextSub = new Atend.Base.Acad.AT_SUB(TextOi);
                TextSub.SubIdCollection.Add(NewConsol);
                TextSub.Insert();

                Atend.Base.Acad.AT_SUB s = new Atend.Base.Acad.AT_SUB(NewConsol);
                if (UseParentOi)
                {
                    Atend.Base.Acad.AT_SUB.AddToAT_SUB(NewConsol, ParentOi);
                    s.SubIdCollection.Add(ParentOi);
                }
                s.SubIdCollection.Add(TextOi);
                s.Insert();
            }
        }

        private bool SaveConsolData(bool UseParentOi)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            OleDbConnection aConnection = new OleDbConnection(Atend.Control.ConnectionString.AccessCnString);
            OleDbTransaction aTransaction;

            try
            {

                aConnection.Open();
                aTransaction = aConnection.BeginTransaction();
                try
                {

                    if (!UseAccess)
                    {
                        ed.WriteMessage("!UseAccess\n");
                        if (!eConsol.AccessInsert(aTransaction, aConnection, true, true))
                        {
                            throw new System.Exception("eConsol.AccessInsert failed");
                        }
                    }

                    ConsolPack = new Atend.Base.Design.DPackage();
                    ConsolPack.Count = ConsolConut;
                    ConsolPack.IsExistance = Existance;
                    ConsolPack.LoadCode = 0;
                    Atend.Control.Common.Counters.ConsolCounter++;
                    ConsolPack.Number = string.Format("Ins{0:0000}", Atend.Control.Common.Counters.ConsolCounter);
                    if (UseParentOi)
                    {
                        //ConsolPack.ParentCode = new Guid(Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(ParentOi).NodeCode);
                        ConsolPack.ParentCode = Atend.Base.Design.DPackage.AccessSelectByNodeCode(new Guid(Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(ParentOi).NodeCode), aTransaction, aConnection).Code;
                    }
                    else
                    {
                        ConsolPack.ParentCode = new Guid();
                    }
                    //ConsolPack.ProductCode = eConsol.ProductCode;
                    ConsolPack.ProductCode = eConsol.Code;
                    ConsolPack.ProjectCode = ProjectCode;
                    ConsolPack.Type = (int)Atend.Control.Enum.ProductType.Consol;
                    if (!ConsolPack.AccessInsert(aTransaction, aConnection))
                    {
                        throw new System.Exception("ConsolPack.AccessInsert failed");
                    }
                    ed.WriteMessage("ConsolPack.AccessInsert finished\n");
                    //WENT TO
                    //if (!Atend.Base.Equipment.EContainerPackage.SentFromLocalToAccess(eConsol.XCode, (int)Atend.Control.Enum.ProductType.Consol, eConsol.Code, aTransaction, aConnection))
                    //{
                    //    throw new System.Exception("SentFromLocalToAccess failed");
                    //}



                }
                catch (System.Exception ex1)
                {
                    ed.WriteMessage("ERROR SaveConsolData 02 : {0} \n", ex1.Message);
                    aTransaction.Rollback();
                    aConnection.Close();
                    return false;
                }

            }
            catch (System.Exception ex)
            {
                ed.WriteMessage("ERROR SaveConsolData 01 : {0} \n", ex.Message);
                aConnection.Close();
                return false;
            }

            aTransaction.Commit();
            //ed.WriteMessage("aTransaction.Commit \n");
            aConnection.Close();

            #region Not to Get Green

            Atend.Base.Acad.AcadGlobal.ConsolData.UseAccess = true;
            UseAccess = true;

            #endregion

            return true;
        }

        public bool UpdateConsolData(Guid ConsolCode, ObjectId SelectedOBJ)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            OleDbConnection aConnection = new OleDbConnection(Atend.Control.ConnectionString.AccessCnString);
            OleDbTransaction aTransaction;

            try
            {

                aConnection.Open();
                aTransaction = aConnection.BeginTransaction();
                try
                {

                    if (!UseAccess)
                    {
                        ed.WriteMessage("!UseAccess\n");
                        if (!eConsol.AccessInsert(aTransaction, aConnection, true, true))
                        {
                            throw new System.Exception("eConsol.AccessInsert failed");
                        }
                    }

                    ConsolPack = Atend.Base.Design.DPackage.AccessSelectByCode(ConsolCode, aTransaction, aConnection);

                    ConsolPack.Count = ConsolConut;
                    ConsolPack.IsExistance = Existance;
                    ConsolPack.LoadCode = 0;
                    Atend.Control.Common.Counters.ConsolCounter++;
                    ConsolPack.Number = string.Format("Ins{0:0000}", Atend.Control.Common.Counters.ConsolCounter);
                    //if (UseParentOi)
                    //{
                    //    //ConsolPack.ParentCode = new Guid(Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(ParentOi).NodeCode);
                    //    ConsolPack.ParentCode = Atend.Base.Design.DPackage.AccessSelectByNodeCode(new Guid(Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(ParentOi).NodeCode), aTransaction, aConnection).Code;
                    //}
                    //else
                    //{
                    //    ConsolPack.ParentCode = new Guid();
                    //}
                    //ConsolPack.ProductCode = eConsol.ProductCode;
                    ConsolPack.ProductCode = eConsol.Code;
                    ConsolPack.ProjectCode = ProjectCode;
                    ConsolPack.Type = (int)Atend.Control.Enum.ProductType.Consol;
                    if (!ConsolPack.AccessUpdate(aTransaction, aConnection))
                    {
                        throw new System.Exception("ConsolPack.AccessInsert failed");
                    }
                    ed.WriteMessage("ConsolPack.AccessInsert finished\n");
                    //WENT TO
                    //if (!Atend.Base.Equipment.EContainerPackage.SentFromLocalToAccess(eConsol.XCode, (int)Atend.Control.Enum.ProductType.Consol, eConsol.Code, aTransaction, aConnection))
                    //{
                    //    throw new System.Exception("SentFromLocalToAccess failed");
                    //}

                    if (SelectedOBJ != ObjectId.Null)
                    {
                        Atend.Base.Acad.AT_INFO atInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(SelectedOBJ);
                        atInfo.ProductCode = eConsol.Code;
                        atInfo.Insert();
                    }



                }
                catch (System.Exception ex1)
                {
                    ed.WriteMessage("ERROR SaveConsolData 02 : {0} \n", ex1.Message);
                    aTransaction.Rollback();
                    aConnection.Close();
                    return false;
                }

            }
            catch (System.Exception ex)
            {
                ed.WriteMessage("ERROR SaveConsolData 01 : {0} \n", ex.Message);
                aConnection.Close();
                return false;
            }

            aTransaction.Commit();
            ed.WriteMessage("aTransaction.Commit \n");
            aConnection.Close();
            return true;
        }

        private static void CalculateConsolPoints(Entity entity, int ConsolCount, out Point3dCollection Points)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            Point3d StartPoint, EndPoint;
            Points = new Point3dCollection();
            DimInformation(out StartPoint, out EndPoint, entity);
            if (StartPoint != Point3d.Origin && EndPoint != Point3d.Origin)
            {
                //ed.WriteMessage("Start point : {0} End point :{1} \n",StartPoint,EndPoint);
                Vector3d Vect1 = EndPoint - StartPoint, norm1 = Vect1.GetNormal();
                double Length = Vect1.Length / (ConsolCount + 1);
                //ed.WriteMessage("Length:{0}\n", Length);

                for (int i = 1; i <= ConsolCount; i++)
                {
                    Point3d anotherPoint = StartPoint + (norm1 * Length * i);
                    Points.Add(anotherPoint);
                }

                //foreach (Point3d p in Points)
                //{
                //    ed.WriteMessage("NEW POINT : {0} \n",p);
                //}
            }
            else
            {
                ed.WriteMessage("ERRO FINGING POINTS \n");
            }
        }

        private static void DimInformation(out Point3d StartPoint, out Point3d EndPoint, Entity entity)
        {
            StartPoint = Point3d.Origin;
            EndPoint = Point3d.Origin;
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;

            //Ray _Ray;

            Polyline p = entity as Polyline;
            if (p != null)
            {
                switch (p.NumberOfVertices)
                {
                    case 5:
                        //ed.WriteMessage("pole \n");
                        StartPoint = p.GetPoint3dAt(0);
                        EndPoint = p.GetPoint3dAt(2);
                        break;
                    case 7:
                        //ed.WriteMessage("polePOLY \n");
                        StartPoint = p.GetPoint3dAt(0);
                        EndPoint = p.GetPoint3dAt(3);
                        break;
                }
            }
            else
            {
                Circle c = entity as Circle;
                if (c != null)
                {
                    Curve _Curve = entity as Curve;
                    if (_Curve != null)
                    {
                        //ed.WriteMessage("Circle \n");
                        Point3dCollection p3c = new Point3dCollection();
                        Ray _Ray = new Ray();
                        //ed.WriteMessage("Circle : {0} \n", Atend.Global.Acad.UAcad.CenterOfEntity(entity));
                        Point3d po = Atend.Global.Acad.UAcad.CenterOfEntity(entity);
                        _Ray.BasePoint = po;
                        //ed.WriteMessage("0 \n");
                        _Ray.SecondPoint = new Point3d(_Ray.StartPoint.X + 10, _Ray.StartPoint.Y, 0);
                        //ed.WriteMessage("1 \n");
                        _Curve.IntersectWith(_Ray, Intersect.OnBothOperands, p3c, 0, 0);
                        //ed.WriteMessage("2 \n");
                        if (p3c.Count > 0)
                        {
                            StartPoint = p3c[0];

                            _Ray = new Ray();
                            p3c.Clear();
                            _Ray.BasePoint = Atend.Global.Acad.UAcad.CenterOfEntity(_Curve);
                            _Ray.SecondPoint = new Point3d(_Ray.StartPoint.X - 10, _Ray.StartPoint.Y, 0);
                            _Curve.IntersectWith(_Ray, Intersect.OnBothOperands, p3c, 0, 0);
                            if (p3c.Count > 0)
                            {
                                EndPoint = p3c[0];
                            }
                        }

                    }
                }//
            }

        }

        public static void ShowDescription(ObjectId oi, OleDbConnection _Conection)
        {

            int ProductCode = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oi).ProductCode;
            Entity CurrentEntity = Atend.Global.Acad.UAcad.GetEntityByObjectID(oi);
            double CommentScale = Atend.Base.Design.DProductProperties.AccessSelectBySoftwareCode((int)Atend.Control.Enum.ProductType.AirPost).Scale;
            Atend.Base.Equipment.EConsol _EConsol = Atend.Base.Equipment.EConsol.AccessSelectByCode(ProductCode, _Conection);
            if (_EConsol.Code != -1)
            {

                Point3d EntityCenterPoint = Atend.Global.Acad.UAcad.CenterOfEntity(CurrentEntity);
                Entity TextEntity = Atend.Global.Acad.UAcad.WriteNote(_EConsol.Comment, EntityCenterPoint, CommentScale);
                Atend.Global.Acad.UAcad.DrawEntityOnScreen(TextEntity, Atend.Control.Enum.AutoCadLayerName.DESCRIPTION.ToString());


            }
        }


    }
}
