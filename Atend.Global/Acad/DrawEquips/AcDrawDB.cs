﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.OleDb;

using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.Geometry;
//using Autodesk.AutoCAD.Interop;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.Windows;
using Autodesk.AutoCAD.Colors;


namespace Atend.Global.Acad.DrawEquips
{
    public class AcDrawDB
    {
        //~~~~~~~~~~~~~~~~~~~~~Properties~~~~~~~~~~~~~~~~~~~~~~~~~~//
        int _existance;
        public int Existance
        {
            get { return _existance; }
            set { _existance = value; }
        }

        bool _useAccess;
        public bool UseAccess
        {
            get { return _useAccess; }
            set { _useAccess = value; }
        }

        private Atend.Base.Design.DPackage DBPack = new Atend.Base.Design.DPackage();

        private ObjectId selectedObjectId;
        public ObjectId SelectedObjectId
        {
            get { return selectedObjectId; }
            set { selectedObjectId = value; }
        }

        private Atend.Base.Equipment.EDB _eDB;
        public Atend.Base.Equipment.EDB EDB
        {
            get { return _eDB; }
            set { _eDB = value; }
        }

        private int projectCode;
        public int ProjectCode
        {
            get { return projectCode; }
            set { projectCode = value; }
        }

        private List<Atend.Base.Equipment.EDBPhuse> _eDBPhuse;
        public List<Atend.Base.Equipment.EDBPhuse> eDBPhuse
        {
            get { return _eDBPhuse; }
            set { _eDBPhuse = value; }
        }



        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        class DrawDBJig : DrawJig
        {

            Point3d BasePoint = Point3d.Origin;
            public Point3d MyBasePoint
            {
                get { return BasePoint; }
            }

            List<Entity> Entities = new List<Entity>();
            int _feederCount = 0;
            double MyScale = 1;

            public DrawDBJig(int FeederCount, double Scale)
            {
                MyScale = Scale;
                _feederCount = FeederCount;
            }

            private Entity CreateLine(Point3d StartPoint, Point3d EndPoint, int ProductType, int ColorIndex, double Thickness)
            {
                Atend.Global.Acad.AcadJigs.MyLine mLine = new Atend.Global.Acad.AcadJigs.MyLine();
                mLine.StartPoint = StartPoint;
                mLine.EndPoint = EndPoint;

                if (Thickness != 0)
                {
                    mLine.Thickness = Thickness;
                }


                Atend.Global.Acad.AcadJigs.SaveExtensionData(mLine, ProductType);

                mLine.ColorIndex = ColorIndex;

                return mLine;
            }

            private Entity CreateHeaderCable(Point3dCollection P3C)
            {

                Atend.Global.Acad.AcadJigs.MyPolyLine pLine = new Atend.Global.Acad.AcadJigs.MyPolyLine();
                pLine.ColorIndex = 40;
                pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(P3C[0].X, P3C[0].Y), 0, 0, 0);
                pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(P3C[1].X, P3C[1].Y), 0, 0, 0);
                pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(P3C[2].X, P3C[2].Y), 0, 0, 0);
                pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(P3C[0].X, P3C[0].Y), 0, 0, 0);

                Atend.Global.Acad.AcadJigs.SaveExtensionData(pLine, (int)Atend.Control.Enum.ProductType.HeaderCabel);
                pLine.Closed = true;

                return pLine;

            }

            private Entity CreateStreetBoxBox(Point3d CenterPoint, double Width, double Height)
            {

                double BaseX = CenterPoint.X;
                double BaseY = CenterPoint.Y;

                Atend.Global.Acad.AcadJigs.MyPolyLine pLine = new Atend.Global.Acad.AcadJigs.MyPolyLine();
                pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX - (Width / 2), BaseY - (Height / 2)), 0, 0, 0);
                pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX + (Width / 2), BaseY - (Height / 2)), 0, 0, 0);
                pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX + (Width / 2), BaseY + (Height / 2)), 0, 0, 0);
                pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX - (Width / 2), BaseY + (Height / 2)), 0, 0, 0);
                pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX - (Width / 2), BaseY - (Height / 2)), 0, 0, 0);
                pLine.Closed = true;


                Atend.Global.Acad.AcadJigs.SaveExtensionData(pLine, (int)Atend.Control.Enum.ProductType.StreetBox);
                return pLine;

            }

            private void CreateFeeder(Point3d CenterPoint)
            {
                double BaseX = CenterPoint.X;
                double BaseY = CenterPoint.Y;

                //bus
                Entities.Add(CreateLine(
                    new Point3d(BaseX - 8, BaseY, 0),
                    new Point3d(BaseX + 8, BaseY, 0),
                    (int)Atend.Control.Enum.ProductType.Bus,
                    190,
                    0));

                //Additional
                Entities.Add(CreateLine(
                    new Point3d(BaseX, BaseY, 0),
                    new Point3d(BaseX, BaseY - 18, 0),
                    0,
                    0,
                    0));


                Entities.Add(CreateLine(
    new Point3d(BaseX + 2, BaseY - 6, 0),
    new Point3d(BaseX + 2, BaseY - 12, 0),
    (int)Atend.Control.Enum.ProductType.Phuse,
    0,
    0));
                Entities.Add(CreateLine(
                    new Point3d(BaseX + 2, BaseY - 12, 0),
                    new Point3d(BaseX - 2, BaseY - 12, 0),
                    (int)Atend.Control.Enum.ProductType.Phuse,
                    0,
                    0));
                Entities.Add(CreateLine(
                    new Point3d(BaseX - 2, BaseY - 12, 0),
                    new Point3d(BaseX - 2, BaseY - 6, 0),
                    (int)Atend.Control.Enum.ProductType.Phuse,
                    0,
                    0));
                Entities.Add(CreateLine(
                    new Point3d(BaseX + 2, BaseY - 6, 0),
                    new Point3d(BaseX - 2, BaseY - 6, 0),
                    (int)Atend.Control.Enum.ProductType.Phuse,
                    0,
                    0));



                //header cable
                //header cabel
                Point3dCollection Points = new Point3dCollection();
                Points.Add(new Point3d(BaseX, BaseY - 23, 0));
                Points.Add(new Point3d(BaseX - 5, BaseY - 18, 0));
                Points.Add(new Point3d(BaseX + 5, BaseY - 18, 0));
                Points.Add(new Point3d(BaseX, BaseY - 23, 0));
                Entities.Add(CreateHeaderCable(Points));


            }

            protected override SamplerStatus Sampler(JigPrompts prompts)
            {

                JigPromptPointOptions ppo = new JigPromptPointOptions("\nDB Position:");
                PromptPointResult ppr = prompts.AcquirePoint(ppo);

                if (ppr.Status == PromptStatus.OK)
                {

                    if (ppr.Value == BasePoint)
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

            protected override bool WorldDraw(Autodesk.AutoCAD.GraphicsInterface.WorldDraw draw)
            {
                Entities.Clear();
                int AllCounter = 0;
                double ShemshStart = BasePoint.X;
                for (int i = 1; i <= _feederCount; i++)
                {

                    //int FeederCount = 0;
                    //shemshFeeder.TryGetValue(i, out FeederCount);

                    Point3d CP = new Point3d(ShemshStart + (16 * (i - 1)), BasePoint.Y, BasePoint.Z);
                    CreateFeeder(CP);
                    AllCounter++;

                    //ShemshStart +=16// (16 * (FeederCount + 1));

                }
                AllCounter++;
                AllCounter--;
                Entities.Add(CreateStreetBoxBox(new Point3d((BasePoint.X - 8) + ((AllCounter * 16) / 2), (BasePoint.Y + 5) - (30 / 2), BasePoint.Z), (AllCounter * 16), 30));


                //~~~~~~~~ SCALE ~~~~~~~~~~

                Matrix3d trans1 = Matrix3d.Scaling(MyScale, new Point3d(BasePoint.X, BasePoint.Y, 0));
                foreach (Entity en in Entities)
                {
                    en.TransformBy(trans1);
                }

                //~~~~~~~~~~~~~~~~~~~~~~~~~

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

            //List<Entity> Entities = new List<Entity>();
            //Point3d BasePoint = new Point3d();
            //double BaseAngle = 0, NewAngle;
            //public bool GetPoint = true, GetAngle = false;
            //double MyScale = 1;


            //public DrawDBJig(double Scale)
            //{
            //    MyScale = Scale;
            //}

            //protected override SamplerStatus Sampler(JigPrompts prompts)
            //{
            //    JigPromptPointOptions ppo = new JigPromptPointOptions("Select position: \n");
            //    JigPromptAngleOptions pao = new JigPromptAngleOptions("Select angle: \n");
            //    if (GetPoint)
            //    {
            //        PromptPointResult pr = prompts.AcquirePoint(ppo);
            //        if (pr.Status == PromptStatus.OK)
            //        {
            //            if (BasePoint == pr.Value)
            //            {
            //                return SamplerStatus.NoChange;
            //            }
            //            else
            //            {
            //                BasePoint = pr.Value;
            //                return SamplerStatus.OK;
            //            }
            //        }
            //        else
            //        {
            //            return SamplerStatus.Cancel;
            //        }
            //    }
            //    else if (GetAngle)
            //    {
            //        pao.BasePoint = BasePoint;
            //        pao.UseBasePoint = true;
            //        PromptDoubleResult pdr = prompts.AcquireAngle(pao);

            //        if (pdr.Status == PromptStatus.OK)
            //        {
            //            if (BaseAngle == pdr.Value)
            //            {
            //                return SamplerStatus.NoChange;
            //            }
            //            else
            //            {
            //                BaseAngle = pdr.Value - BaseAngle;
            //                return SamplerStatus.OK;
            //            }
            //        }
            //        else
            //        {
            //            return SamplerStatus.Cancel;
            //        }
            //    }
            //    return SamplerStatus.Cancel;
            //}

            //public Entity CreateDBEntity(Point3d BasePoint, double Width, double Height)
            //{

            //    double BaseX = BasePoint.X;
            //    double BaseY = BasePoint.Y;

            //    Atend.Global.Acad.AcadJigs.MyPolyLine pLine = new Atend.Global.Acad.AcadJigs.MyPolyLine();
            //    pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX - (Width / 2), BaseY - (Height / 2)), 0, 0, 0);
            //    pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX + (Width / 2), BaseY - (Height / 2)), 0, 0, 0);
            //    pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX + (Width / 2), BaseY + (Height / 2)), 0, 0, 0);
            //    pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX - (Width / 2), BaseY + (Height / 2)), 0, 0, 0);
            //    pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX - (Width / 2), BaseY - (Height / 2)), 0, 0, 0);
            //    pLine.Closed = true;

            //    Atend.Global.Acad.AcadJigs.SaveExtensionData(pLine, (int)Atend.Control.Enum.ProductType.DB);
            //    return pLine;

            //}

            //private Entity CreateLine(Point3d StartPoint, Point3d EndPoint, int ProductType, int ColorIndex, double Thickness)
            //{
            //    Atend.Global.Acad.AcadJigs.MyLine mLine = new Atend.Global.Acad.AcadJigs.MyLine();
            //    mLine.StartPoint = StartPoint;
            //    mLine.EndPoint = EndPoint;

            //    if (Thickness != 0)
            //    {
            //        mLine.Thickness = Thickness;
            //    }

            //    Atend.Global.Acad.AcadJigs.SaveExtensionData(mLine, ProductType);
            //    mLine.ColorIndex = ColorIndex;

            //    return mLine;
            //}

            //protected override bool WorldDraw(Autodesk.AutoCAD.GraphicsInterface.WorldDraw draw)
            //{



            //    Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            //    NewAngle = 0;
            //    Entities.Clear();
            //    Entities.Add(CreateDBEntity(BasePoint, 15, 15));
            //    Entities.Add(CreateLine(new Point3d(BasePoint.X - 7.5, BasePoint.Y + 5.5, 0), new Point3d(BasePoint.X + 7.5, BasePoint.Y + 5.5, 0), (int)Atend.Control.Enum.ProductType.DB, 0, 1));
            //    Entities.Add(CreateLine(new Point3d(BasePoint.X - 7.5, BasePoint.Y + 3, 0), new Point3d(BasePoint.X + 7.5, BasePoint.Y + 3, 0), (int)Atend.Control.Enum.ProductType.DB, 0, 1));
            //    if (GetPoint)
            //    {
            //        Matrix3d trans1 = Matrix3d.Scaling(MyScale, new Point3d(BasePoint.X, BasePoint.Y, 0));
            //        foreach (Entity en in Entities)
            //        {
            //            en.TransformBy(trans1);
            //        }

            //    }
            //    else if (GetAngle)
            //    {
            //        Matrix3d trans = Matrix3d.Rotation(BaseAngle - NewAngle, ed.CurrentUserCoordinateSystem.CoordinateSystem3d.Zaxis, new Point3d(BasePoint.X, BasePoint.Y, 0));
            //        foreach (Entity en in Entities)
            //        {
            //            en.TransformBy(trans);
            //        }

            //        NewAngle = BaseAngle;
            //        BaseAngle = 0;
            //    }

            //    foreach (Entity ent in Entities)
            //    {
            //        draw.Geometry.Draw(ent);
            //    }


            //    return true;
            //}

            //public List<Entity> GetEntities()
            //{
            //    return Entities;
            //}
        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        public void DrawDB()
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            bool conti = true;

            double MyScale = Atend.Base.Design.DProductProperties.AccessSelectBySoftwareCode((int)Atend.Control.Enum.ProductType.DB).Scale;
            double MyCommentScale = Atend.Base.Design.DProductProperties.AccessSelectBySoftwareCode((int)Atend.Control.Enum.ProductType.DB).CommentScale;
            DrawDBJig _DrawDBJig = new DrawDBJig(eDBPhuse.Count, MyScale);
            PromptResult pr;

            //ed.WriteMessage("start conti \n");
            while (conti)
            {
                pr = ed.Drag(_DrawDBJig);
                if (pr.Status == PromptStatus.OK)
                {

                    conti = false;
                    //ed.WriteMessage("finish jig drawing \n");

                    #region Savedata

                    List<Entity> entities = _DrawDBJig.GetEntities();
                    if (SaveDBData())
                    {
                        ObjectIdCollection OIC = new ObjectIdCollection();
                        Atend.Base.Acad.AT_INFO DBInfo;
                        foreach (Entity ent in entities)
                        {

                            Atend.Global.Acad.AcadJigs.MyPolyLine Header = ent as Atend.Global.Acad.AcadJigs.MyPolyLine;
                            object ProductType = null;
                            if (Header != null)
                            {
                                if (Header.AdditionalDictionary.TryGetValue("ProductType", out ProductType))
                                {
                                }
                            }
                            if (ProductType != null && Convert.ToInt32(ProductType) == (int)Atend.Control.Enum.ProductType.HeaderCabel)
                            {
                                ObjectId oi = Atend.Global.Acad.UAcad.DrawEntityOnScreen(ent, Atend.Control.Enum.AutoCadLayerName.LOW_GROUND.ToString());
                                OIC.Add(oi);
                                DBInfo = new Atend.Base.Acad.AT_INFO(oi);
                                DBInfo.ParentCode = DBPack.Code.ToString();
                                DBInfo.NodeCode = "";//put node code header here
                                DBInfo.NodeType = (int)Atend.Control.Enum.ProductType.HeaderCabel;
                                DBInfo.ProductCode = 0;
                                DBInfo.Angle = 0;
                                DBInfo.Insert();

                            }
                            else
                            {
                                ObjectId oi = Atend.Global.Acad.UAcad.DrawEntityOnScreen(ent, Atend.Control.Enum.AutoCadLayerName.LOW_GROUND.ToString());
                                OIC.Add(oi);
                                DBInfo = new Atend.Base.Acad.AT_INFO(oi);
                                DBInfo.ParentCode = "";
                                DBInfo.NodeCode = DBPack.Code.ToString();
                                DBInfo.NodeType = (int)Atend.Control.Enum.ProductType.DB;
                                DBInfo.ProductCode = DBPack.ProductCode;
                                DBInfo.Angle = 0;
                                DBInfo.Insert();
                            }


                        }

                        ObjectId Goi = Atend.Global.Acad.Global.MakeGroup(DBPack.Code.ToString(), OIC);
                        DBInfo = new Atend.Base.Acad.AT_INFO(Goi);
                        DBInfo.ParentCode = "";
                        DBInfo.NodeCode = DBPack.Code.ToString();
                        DBInfo.NodeType = (int)Atend.Control.Enum.ProductType.DB;
                        DBInfo.ProductCode = DBPack.ProductCode;
                        DBInfo.Angle = 0;
                        DBInfo.Insert();

                    }
                    #endregion

                }
                else
                {
                    conti = false;
                }
            }

        }

        public void DrawDBUpdate()
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            bool conti = true;

            double MyScale = Atend.Base.Design.DProductProperties.AccessSelectBySoftwareCode((int)Atend.Control.Enum.ProductType.DB).Scale;
            double MyCommentScale = Atend.Base.Design.DProductProperties.AccessSelectBySoftwareCode((int)Atend.Control.Enum.ProductType.DB).CommentScale;
            DrawDBJig _DrawDBJig = new DrawDBJig(eDBPhuse.Count, MyScale);
            PromptResult pr;

            //ed.WriteMessage("start conti \n");
            while (conti)
            {
                pr = ed.Drag(_DrawDBJig);
                if (pr.Status == PromptStatus.OK)
                {

                    conti = false;
                    //ed.WriteMessage("finish jig drawing \n");

                    //#region Savedata

                    List<Entity> entities = _DrawDBJig.GetEntities();
                    //if (SaveDBData())
                    //{
                    ObjectIdCollection OIC = new ObjectIdCollection();
                    Atend.Base.Acad.AT_INFO DBInfo;
                    foreach (Entity ent in entities)
                    {

                        Atend.Global.Acad.AcadJigs.MyPolyLine Header = ent as Atend.Global.Acad.AcadJigs.MyPolyLine;
                        object ProductType = null;
                        if (Header != null)
                        {
                            if (Header.AdditionalDictionary.TryGetValue("ProductType", out ProductType))
                            {
                            }
                        }
                        if (ProductType != null && Convert.ToInt32(ProductType) == (int)Atend.Control.Enum.ProductType.HeaderCabel)
                        {
                            ObjectId oi = Atend.Global.Acad.UAcad.DrawEntityOnScreen(ent, Atend.Control.Enum.AutoCadLayerName.LOW_GROUND.ToString());
                            OIC.Add(oi);
                            DBInfo = new Atend.Base.Acad.AT_INFO(oi);
                            DBInfo.ParentCode = DBPack.Code.ToString();
                            DBInfo.NodeCode = "";//put node code header here
                            DBInfo.NodeType = (int)Atend.Control.Enum.ProductType.HeaderCabel;
                            DBInfo.ProductCode = 0;
                            DBInfo.Angle = 0;
                            DBInfo.Insert();

                        }
                        else
                        {
                            ObjectId oi = Atend.Global.Acad.UAcad.DrawEntityOnScreen(ent, Atend.Control.Enum.AutoCadLayerName.LOW_GROUND.ToString());
                            OIC.Add(oi);
                            DBInfo = new Atend.Base.Acad.AT_INFO(oi);
                            DBInfo.ParentCode = "";
                            DBInfo.NodeCode = DBPack.Code.ToString();
                            DBInfo.NodeType = (int)Atend.Control.Enum.ProductType.DB;
                            DBInfo.ProductCode = DBPack.ProductCode;
                            DBInfo.Angle = 0;
                            DBInfo.Insert();
                        }

                    }

                    ObjectId Goi = Atend.Global.Acad.Global.MakeGroup(DBPack.Code.ToString(), OIC);
                    DBInfo = new Atend.Base.Acad.AT_INFO(Goi);
                    DBInfo.ParentCode = "";
                    DBInfo.NodeCode = DBPack.Code.ToString();
                    DBInfo.NodeType = (int)Atend.Control.Enum.ProductType.DB;
                    DBInfo.ProductCode = DBPack.ProductCode;
                    DBInfo.Angle = 0;
                    DBInfo.Insert();

                    //}
                    //#endregion

                }
                else
                {
                    conti = false;
                }
            }
        }

        private bool SaveDBData()
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
                    //Atend.Base.Equipment.EDB db = Atend.Base.Equipment.EDB.AccessSelectByXCode(EDB.XCode);
                    if (!UseAccess)
                    {
                        //if (db.Code == -1)
                        //{
                        if (!EDB.AccessInsert(aTransaction, aConnection, true, true))
                        {
                            throw new System.Exception("eDB.AccessInsert failed");
                        }
                        //ed.WriteMessage("eDB.AccessInsert finished");

                        //WENT TO
                        //if (!Atend.Base.Equipment.EContainerPackage.SentFromLocalToAccess(EDB.XCode, (int)Atend.Control.Enum.ProductType.DB, EDB.Code, aTransaction, aConnection))
                        //{
                        //    throw new System.Exception("SentFromLocalToAccess failed");
                        //}
                        //ed.WriteMessage("SentFromLocalToAccess finished");

                        //if (!Atend.Base.Equipment.EOperation.SentFromLocalToAccess(EDB.XCode, (int)Atend.Control.Enum.ProductType.DB, EDB.Code, aTransaction, aConnection))
                        //{
                        //    throw new System.Exception("operation failed");
                        //}

                        //}

                        foreach (Atend.Base.Equipment.EDBPhuse SelectedDBPhuse in eDBPhuse)
                        {
                            //ed.WriteMessage("@@{0}\n", SelectedStreetBoxPhuse.PhuseXCode);
                            //ed.WriteMessage("########## SelectedStreetBoxPhuse.PhuseXCode:{0}\n", SelectedStreetBoxPhuse.PhuseXCode);
                            Atend.Base.Equipment.EPhuse phuse = Atend.Base.Equipment.EPhuse.SelectByXCode(SelectedDBPhuse.PhuseXCode);
                            //ed.WriteMessage("########## phuse.Code:{0}\n", phuse.Code);
                            if (phuse.Code != -1)
                            {
                                Atend.Base.Equipment.EPhusePole PhuseP = Atend.Base.Equipment.EPhusePole.SelectByXCode(phuse.PhusePoleXCode);
                                if (PhuseP.Code != -1)
                                {

                                    if (!PhuseP.AccessInsert(aTransaction, aConnection, true, true))
                                    {
                                        throw new System.Exception("EPhusePole.AccesInsert failed");
                                    }

                                    phuse.PhusePoleCode = PhuseP.Code;
                                    if (!phuse.AccessInsert(aTransaction, aConnection, true, true))
                                    {
                                        throw new System.Exception("ePhuse.AccesInsert failed");
                                    }

                                    SelectedDBPhuse.PhuseCode = phuse.Code;
                                    SelectedDBPhuse.DBCode = EDB.Code;
                                    //if (sb.Code == -1)
                                    //    SelectedStreetBoxPhuse.StreetBoxCode = eStreetBox.Code;
                                    //else
                                    //    SelectedStreetBoxPhuse.StreetBoxCode = sb.Code;
                                    if (!SelectedDBPhuse.AccessInsert(aTransaction, aConnection, true, true))
                                    {
                                        throw new System.Exception("SelectedDBPhuseInsert failed");
                                    }
                                }
                                else
                                {
                                    throw new System.Exception("EPhusePole.SelectByXCode failed");
                                }
                            }
                            else
                            {
                                throw new System.Exception("EPhuse.SelectByXCode failed");
                            }
                        }



                    }

                    DBPack.Count = 1;
                    DBPack.IsExistance = Existance;
                    DBPack.ProjectCode = projectCode;
                    DBPack.NodeCode = Guid.Empty;
                    //DBPack.Number = EDB.CountorCount.ToString();
                    DBPack.ParentCode = Guid.Empty;
                    DBPack.ProductCode = EDB.Code;
                    DBPack.Number = "DB";
                    //if (db.Code == -1)
                    //    DBPack.ProductCode = EDB.Code;
                    //else
                    //    DBPack.ProductCode = db.Code;
                    DBPack.Type = Convert.ToInt32(Atend.Control.Enum.ProductType.DB);

                    if (!DBPack.AccessInsert(aTransaction, aConnection))
                    {
                        throw new System.Exception("DBPack.AccessInsert failed");
                    }
                    //ed.WriteMessage("DBPack.AccessInsert finished");

                    //if (!UseAccess)
                    //{


                    //if (db.Code == -1)
                    //{
                    //    if (!Atend.Base.Equipment.EContainerPackage.SentFromLocalToAccess(EDB.XCode,
                    //    (int)Atend.Control.Enum.ProductType.DB,
                    //    EDB.Code, aTransaction, aConnection))
                    //    {
                    //        throw new System.Exception("SentFromLocalToAccess failed");
                    //    }
                    //}
                    //else
                    //{
                    //    if (!Atend.Base.Equipment.EContainerPackage.SentFromLocalToAccess(db.XCode,
                    //    (int)Atend.Control.Enum.ProductType.DB,
                    //    db.Code, aTransaction, aConnection))
                    //    {
                    //        throw new System.Exception("SentFromLocalToAccess failed");
                    //    }
                    //}

                    //}


                }
                catch (System.Exception ex1)
                {
                    ed.WriteMessage("ERROR SaveDBData 02 :{0} \n", ex1.Message);
                    aTransaction.Rollback();
                    aConnection.Close();
                    return false;

                }

            }
            catch (System.Exception ex)
            {
                ed.WriteMessage("ERROR SaveDBData 01 :{0} \n", ex.Message);
                aConnection.Close();
                return false;
            }

            aTransaction.Commit();
            aConnection.Close();

            #region Not to Get Green

            Atend.Base.Acad.AcadGlobal.DBData.UseAccess = true;
            UseAccess = true;

            #endregion

            return true;

        }

        public bool UpdateDBData(Guid EXCode)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            OleDbTransaction aTransaction;
            OleDbConnection aConnection = new OleDbConnection(Atend.Control.ConnectionString.AccessCnString);
            try
            {
                aConnection.Open();
                aTransaction = aConnection.BeginTransaction();
                try
                {
                    DBPack = Atend.Base.Design.DPackage.AccessSelectByCode(EXCode);
                    if (!UseAccess)
                    {
                        if (!EDB.AccessInsert(aTransaction, aConnection, true, true))
                        {
                            throw new System.Exception("eDisConnector.AccessInsert failed");
                        }
                        //if (!Atend.Base.Equipment.EOperation.SentFromLocalToAccess(eDisConnector.XCode, (int)Atend.Control.Enum.ProductType.Disconnector, eDisConnector.Code, aTransaction, aConnection))
                        //{
                        //    throw new System.Exception("operation failed");
                        //}

                        foreach (Atend.Base.Equipment.EDBPhuse SelectedDBPhuse in eDBPhuse)
                        {
                            Atend.Base.Equipment.EPhuse phuse = Atend.Base.Equipment.EPhuse.SelectByXCode(SelectedDBPhuse.PhuseXCode);
                            if (phuse.Code != -1)
                            {
                                if (!phuse.AccessInsert(aTransaction, aConnection, true, true))
                                {
                                    throw new System.Exception("ePhuse.AccesInsert failed");
                                }
                                SelectedDBPhuse.PhuseCode = phuse.Code;
                                SelectedDBPhuse.DBCode = EDB.Code;
                                if (!SelectedDBPhuse.AccessInsert(aTransaction, aConnection, true, true))
                                {
                                    throw new System.Exception("SelectedDBPhuseInsert failed");
                                }
                            }
                            else
                            {
                                throw new System.Exception("EPhuse.SelectByXCode failed");
                            }
                        }
                    }
                    DBPack.IsExistance = Existance;
                    DBPack.ProductCode = EDB.Code;
                    DBPack.ProjectCode = ProjectCode;
                    DBPack.Number = "";
                    if (DBPack.AccessUpdate(aTransaction, aConnection))
                    {
                        Atend.Base.Acad.AT_INFO atinfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(selectedObjectId);
                        atinfo.ProductCode = EDB.Code;
                        atinfo.Insert();
                    }
                    else
                    {
                        throw new System.Exception("DBPack.AccessInsert2 failed");
                    }

                }
                catch (System.Exception ex1)
                {
                    ed.WriteMessage("ERROR UpdateDB 01(transaction) : {0} \n", ex1.Message);
                    aTransaction.Rollback();
                    aConnection.Close();
                    return false;

                }

            }
            catch (System.Exception ex)
            {
                ed.WriteMessage("ERROR UpdateDB 01 : {0} \n", ex.Message);
                aConnection.Close();
                return false;
            }

            aTransaction.Commit();
            DeleteDB(selectedObjectId);
            DrawDBUpdate();
            aConnection.Close();
            return true;
        }

        public static bool DeleteDBData(ObjectId DBOI)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            try
            {
                ObjectId id = Atend.Global.Acad.UAcad.GetEntityGroup(DBOI);
                ObjectIdCollection Collection = Atend.Global.Acad.UAcad.GetGroupSubEntities(id);
                foreach (ObjectId collect in Collection)
                {
                    Atend.Base.Acad.AT_INFO atinfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(collect);
                    if (atinfo.ParentCode != "NONE" && atinfo.NodeType == (int)Atend.Control.Enum.ProductType.HeaderCabel)
                    {
                        Atend.Base.Acad.AT_SUB Sub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(collect);
                        foreach (ObjectId oi in Sub.SubIdCollection)
                        {
                            Atend.Base.Acad.AT_INFO atinfo2 = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oi);
                            if (atinfo2.ParentCode != "NONE" && atinfo2.NodeType == (int)Atend.Control.Enum.ProductType.GroundCabel)
                            {
                                if (!Atend.Base.Design.DBranch.AccessDelete(new Guid(atinfo2.NodeCode.ToString())))
                                {
                                    throw new System.Exception("Error In Delete dbranch\n");
                                }
                            }
                        }
                    }
                }

                //delete DB
                Atend.Base.Acad.AT_INFO conductorinfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(DBOI);
                if (!Atend.Base.Design.DPackage.AccessDelete(new Guid(conductorinfo.NodeCode.ToString())))
                {
                    throw new System.Exception("Error In Delete dpackage\n");
                }
            }
            catch (System.Exception ex)
            {
                ed.WriteMessage("Data ERROR DB : {0} \n", ex.Message);
                return false;
            }
            return true;
        }

        public static bool DeleteDB(ObjectId DBOI)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            try
            {
                //Move Ground
                Atend.Base.Acad.AT_SUB Collection1 = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(DBOI);
                foreach (ObjectId obj in Collection1.SubIdCollection)
                {
                    Atend.Base.Acad.AT_INFO at_info02 = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(obj);
                    if (at_info02.ParentCode != "NONE" && at_info02.NodeType == (int)Atend.Control.Enum.ProductType.Ground)
                    {
                        ObjectIdCollection Coll = Atend.Global.Acad.UAcad.GetGroupSubEntities(obj);
                        foreach (ObjectId col in Coll)
                        {
                            Atend.Base.Acad.AT_INFO at_info = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(col);
                            if (at_info.ParentCode != "NONE" && at_info.NodeType == (int)Atend.Control.Enum.ProductType.Ground)
                            {
                                if (!Atend.Global.Acad.AcadRemove.DeleteEntityByObjectId(col))
                                {
                                    throw new System.Exception("Error In Delete ground\n");
                                }
                            }
                        }
                    }
                }

                //Move Cabel
                ObjectId id = Atend.Global.Acad.UAcad.GetEntityGroup(DBOI);
                ObjectIdCollection Collection = Atend.Global.Acad.UAcad.GetGroupSubEntities(id);
                foreach (ObjectId collect in Collection)
                {
                    Atend.Base.Acad.AT_INFO atinfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(collect);
                    if (atinfo.ParentCode != "NONE" && atinfo.NodeType == (int)Atend.Control.Enum.ProductType.HeaderCabel)
                    {
                        Atend.Base.Acad.AT_SUB sub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(collect);
                        foreach (ObjectId objsub in sub.SubIdCollection)
                        {
                            Atend.Base.Acad.AT_INFO at_info1 = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(objsub);
                            if (at_info1.ParentCode != "NONE" && at_info1.NodeType == (int)Atend.Control.Enum.ProductType.GroundCabel)
                            {
                                //Delete Comment 
                                Atend.Base.Acad.AT_SUB subBranch = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(objsub);
                                foreach (ObjectId collect001 in subBranch.SubIdCollection)
                                {
                                    Atend.Base.Acad.AT_INFO atinfo001 = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(collect001);
                                    if (atinfo001.ParentCode != "NONE" && atinfo001.NodeType == (int)Atend.Control.Enum.ProductType.Comment)
                                    {
                                        if (!Atend.Global.Acad.AcadRemove.DeleteEntityByObjectId(collect001))
                                        {
                                            throw new System.Exception("Error In Delete Comment\n");
                                        }
                                    }
                                    if (atinfo001.ParentCode != "NONE" && atinfo001.NodeType == (int)Atend.Control.Enum.ProductType.HeaderCabel && atinfo001.SelectedObjectId != collect)
                                    {
                                        if (!Atend.Global.Acad.AcadRemove.DeleteEntityByObjectId(objsub))
                                        {
                                            throw new System.Exception("Error In Delete groundcabel\n");
                                        }
                                        else
                                        {
                                            Atend.Base.Acad.AT_SUB.RemoveFromAT_SUB(objsub, collect001);
                                        }
                                    }

                                }
                            }
                        }
                    }

                    if (!Atend.Global.Acad.AcadRemove.DeleteEntityByObjectId(collect))
                    {
                        throw new System.Exception("Error In Delete Sub DB\n");
                    }

                }

                if (!Atend.Global.Acad.AcadRemove.DeleteEntityByObjectId(DBOI))
                {
                    throw new System.Exception("GRA while delete DB\n");
                }

            }
            catch (System.Exception ex)
            {
                ed.WriteMessage("GRA ERROR DB: {0} \n", ex.Message);
                return false;
            }
            return true;
        }

        public static void DrawShield()
        {
            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            Dictionary<string, Point2dCollection> MyDic = new Dictionary<string, Point2dCollection>();
            Dictionary<string, ObjectId> MyDic1 = new Dictionary<string, ObjectId>();
            try
            {
                TypedValue[] tvs = new TypedValue[] { new TypedValue((int)DxfCode.LayerName, Atend.Control.Enum.AutoCadLayerName.LOW_GROUND.ToString()) };
                //ed.WriteMessage("a\n");
                SelectionFilter sf = new SelectionFilter(tvs);
                //ed.WriteMessage("b\n");
                PromptSelectionResult psr = ed.SelectAll(sf);
                //ed.WriteMessage("c\n");
                if (psr.Value != null)
                {
                    ObjectId[] ids = psr.Value.GetObjectIds();

                    //ObjectIdCollection OIC = new ObjectIdCollection();
                    //ed.WriteMessage("ff:{0}\n", ids.Length);
                    foreach (ObjectId oi in ids)
                    {
                        //ed.WriteMessage("1\n");
                        Atend.Base.Acad.AT_INFO PostInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oi);
                        if (PostInfo.ParentCode != "NONE" && PostInfo.NodeType == (int)Atend.Control.Enum.ProductType.DB)
                        {
                            //ed.WriteMessage("2\n");
                            Entity ent = Atend.Global.Acad.UAcad.GetEntityByObjectID(oi);
                            Polyline pl = ent as Polyline;
                            Point2dCollection pts = new Point2dCollection(); //p2;
                            if (pl != null)
                            {
                                if (pl.NumberOfVertices == 5)
                                {
                                    pts = new Point2dCollection(5);
                                    for (int i = 0; i < pl.NumberOfVertices; i++)
                                    {
                                        Point2d p = pl.GetPoint2dAt(i);
                                        double a = p.X * 1;
                                        double b = p.Y * 1;
                                        pts.Add(new Point2d(a, b));
                                    }
                                    MyDic.Add(PostInfo.NodeCode, pts);
                                    MyDic1.Add(PostInfo.NodeCode, oi);

                                }
                            }
                        }
                        //ed.WriteMessage("--------------------------------------- \n");
                    }
                    ids = null;
                    //ed.WriteMessage("------------------1--------------------- \n");
                    foreach (string a in MyDic.Keys)
                    {
                        Point2dCollection p = new Point2dCollection();
                        MyDic.TryGetValue(a, out p);
                        Atend.Global.Acad.Global.CreateWhiteBack(p);
                    }
                    //ed.WriteMessage("------------------2--------------------- \n");
                    foreach (string NodeCode in MyDic1.Keys)
                    {
                        ObjectId DBOI = ObjectId.Null;
                        if (MyDic1.TryGetValue(NodeCode, out DBOI))
                        {
                            Atend.Base.Design.DPackage DBPack = Atend.Base.Design.DPackage.AccessSelectByCode(new Guid(NodeCode));
                            if (DBPack.Code != Guid.Empty)
                            {
                                Atend.Base.Base.BEquipStatus ES = Atend.Base.Base.BEquipStatus.SelectByACode(DBPack.IsExistance);
                                if (ES.Name.IndexOf("موجود") != -1)
                                {
                                    //ed.WriteMessage("mojood : {0} \n", DBOI);
                                    ShieldForDB(DBOI, true);
                                }
                                else
                                {
                                    ShieldForDB(DBOI, false);
                                }

                            }
                            else
                            {
                                Atend.Global.Acad.Notification notification = new Atend.Global.Acad.Notification();
                                notification.Title = "بروز خطا";
                                notification.Msg = "اطلاعات یکی از جعبه انشعاب ها در پایگاه داده یافت نشد";
                                notification.infoCenterBalloon();
                                throw new System.Exception("pole was not in DPackage Post");

                            }
                        }
                    }



                    ed.WriteMessage("DB finished \n");
                }
            }
            catch (System.Exception ex)
            {
                ed.WriteMessage("ERROR IN DB :{0} \n", ex.Message);
            }
        }

        private static void ShieldForDB(ObjectId CurrentDBOI, bool IsExist)
        {
            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            //ed.WriteMessage("SHIELD \n");
            try
            {

                Entity ent = Atend.Global.Acad.UAcad.GetEntityByObjectID(CurrentDBOI);
                Polyline pl = ent as Polyline;
                if (pl != null)
                {
                    double Radius = 0;
                    LineSegment3d ls1 = new LineSegment3d(pl.GetPoint3dAt(0), pl.GetPoint3dAt(1));
                    LineSegment3d ls2 = new LineSegment3d(pl.GetPoint3dAt(1), pl.GetPoint3dAt(2));
                    if (ls1.Length < ls2.Length)
                    {
                        Radius = ls1.Length / 3;
                    }
                    else
                    {
                        Radius = ls2.Length / 3;
                    }


                    Point3d centerDB = Atend.Global.Acad.UAcad.CenterOfEntity(ent);
                    Circle c1 = new Circle(centerDB, new Vector3d(0, 0, 1), Radius);
                    Atend.Global.Acad.UAcad.DrawEntityOnScreen(c1, Atend.Control.Enum.AutoCadLayerName.SHELL.ToString());

                    if (IsExist)
                    {

                        Transaction tr = doc.TransactionManager.StartTransaction();
                        using (tr)
                        {
                            DBObject obj = tr.GetObject(CurrentDBOI, OpenMode.ForRead);
                            Curve cur = obj as Curve;
                            if (cur != null && cur.Closed == false)
                            {
                                //ed.WriteMessage("\nLoop must be a closed curve.");
                            }
                            else
                            {
                                BlockTable bt = (BlockTable)tr.GetObject(doc.Database.BlockTableId, OpenMode.ForRead);
                                BlockTableRecord btr = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);
                                Hatch hat = new Hatch();
                                hat.SetDatabaseDefaults();
                                // Firstly make it clear we want a gradient fill
                                hat.HatchObjectType = HatchObjectType.GradientObject;
                                hat.LayerId = Atend.Global.Acad.UAcad.GetLayerById(Atend.Control.Enum.AutoCadLayerName.SHELL.ToString());
                                //Let's use the pre-defined spherical gradient
                                //LINEAR, CYLINDER, INVCYLINDER, SPHERICAL, INVSPHERICAL, HEMISPHERICAL, INVHEMISPHERICAL, CURVED, and INVCURVED. 
                                hat.SetGradient(GradientPatternType.PreDefinedGradient, "LINEAR");
                                // We're defining two colours
                                hat.GradientOneColorMode = false;
                                GradientColor[] gcs = new GradientColor[2];
                                // First colour must have value of 0
                                gcs[0] = new GradientColor(Color.FromRgb(0, 0, 0), 0);
                                // Second colour must have value of 1
                                gcs[1] = new GradientColor(Color.FromRgb(0, 0, 0), 1);
                                hat.SetGradientColors(gcs);
                                // Add the hatch to the model space
                                // and the transaction
                                ObjectId hatId = btr.AppendEntity(hat);
                                tr.AddNewlyCreatedDBObject(hat, true);
                                // Add the hatch loop and complete the hatch
                                ObjectIdCollection ids = new ObjectIdCollection();
                                ids.Add(c1.ObjectId);
                                hat.Associative = true;
                                hat.AppendLoop(HatchLoopTypes.Default, ids);
                                hat.EvaluateHatch(true);
                                tr.Commit();
                            }
                        }
                    }

                }
            }
            catch (System.Exception ex)
            {
                ed.WriteMessage("ERROR IN DB shield:{0} \n", ex.Message);
            }
        }


    }
}
