﻿using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;

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


    public class AcDrawPolygonPole
    {

        //~~~~~~~~~~~~~~~~~~~~~~~~ Properties~~~~~~~~~~~~~~~~~~~~~//

        Atend.Base.Design.DNode dNode;
        Atend.Base.Design.DPackage PolePackage = new Atend.Base.Design.DPackage();
        Atend.Base.Design.DPackage HalterPackage = new Atend.Base.Design.DPackage();
        List<Atend.Base.Design.DPackage> dPackages = new List<Atend.Base.Design.DPackage>();

        Atend.Base.Design.DPoleInfo _dPoleInfo;
        public Atend.Base.Design.DPoleInfo dPoleInfo
        {
            get { return _dPoleInfo; }
            set { _dPoleInfo = value; }
        }

        bool _UseAccess;
        public bool UseAccess
        {
            get { return _UseAccess; }
            set { _UseAccess = value; }
        }

        int _ProjectCode;
        public int ProjectCode
        {
            get { return _ProjectCode; }
            set { _ProjectCode = value; }
        }

        Atend.Base.Equipment.EPole _ePole;
        public Atend.Base.Equipment.EPole ePole
        {
            get { return _ePole; }
            set { _ePole = value; }
        }

        Atend.Base.Equipment.EPoleTip _ePoleTip;

        public Atend.Base.Equipment.EPoleTip ePoleTip
        {
            get { return _ePoleTip; }
            set { _ePoleTip = value; }
        }

        List<Atend.Base.Equipment.EConsol> _eConsols;
        public List<Atend.Base.Equipment.EConsol> eConsols
        {
            get { return _eConsols; }
            set { _eConsols = value; }
        }

        ArrayList _eConsolUseAccess;
        public ArrayList eConsolUseAccess
        {
            get { return _eConsolUseAccess; }
            set { _eConsolUseAccess = value; }
        }

        ArrayList _eConsolExistance;
        public ArrayList eConsolExistance
        {
            get { return _eConsolExistance; }
            set { _eConsolExistance = value; }
        }

        ArrayList _eConsolProjectCode;
        public ArrayList eConsolProjectCode
        {
            get { return _eConsolProjectCode; }
            set { _eConsolProjectCode = value; }
        }

        ArrayList _eConsolCount;
        public ArrayList eConsolCount
        {
            get { return _eConsolCount; }
            set { _eConsolCount = value; }
        }

        int _Existance;
        public int Existance
        {
            get { return _Existance; }
            set { _Existance = value; }
        }

        private int _HalterExistance;
        public int HalterExistance
        {
            get { return _HalterExistance; }
            set { _HalterExistance = value; }
        }

        private int _HalterProjectCode;
        public int HalterProjectCode
        {
            get { return _HalterProjectCode; }
            set { _HalterProjectCode = value; }
        }

        private Atend.Base.Equipment.EHalter _eHalter;
        public Atend.Base.Equipment.EHalter eHalter
        {
            get { return _eHalter; }
            set { _eHalter = value; }
        }

        int _eHalterCount;
        public int eHalterCount
        {
            get { return _eHalterCount; }
            set { _eHalterCount = value; }
        }

        double _Height;
        public double Height
        {
            get { return _Height; }
            set { _Height = value; }
        }
        //~~~~~~~~~~~~~~~~~~~~~~~~~ class ~~~~~~~~~~~~~~~~~~~~~~~~~//
        class DrawPolePolygonJig : DrawJig
        {

            Point3d CenterPoint = Point3d.Origin;

            public Point3d PoleCenterPoint
            {
                get { return CenterPoint; }
                set { CenterPoint = value; }
            }
            List<Entity> Entities = new List<Entity>();
            int ConsolCount = 0;
            double MyScale = 1;

            public DrawPolePolygonJig(int _ConsolCount, double Scale)
            {
                MyScale = Scale;
                ConsolCount = _ConsolCount;

                Entities.Add(DrawPolePolygon(CenterPoint));
                ////////double StrtX = CenterPoint.X - 50;
                ////////for (int i = 0; i < ConsolCount; i++)
                ////////{
                ////////    Entities.Add(DrawConsol(new Point3d(StrtX + (40 * i), CenterPoint.Y, 0), 10, 10));
                ////////}

                //-------------------
                Entity MyConsolEntity = null;
                switch (ConsolCount)
                {
                    case 1:

                        MyConsolEntity = DrawConsol(new Point3d(CenterPoint.X, CenterPoint.Y, 0), 10, 10);
                        Entities.Add(MyConsolEntity);

                        break;
                    case 2:


                        MyConsolEntity = DrawConsol(new Point3d(CenterPoint.X + 30, CenterPoint.Y, 0), 10, 10);
                        Entities.Add(MyConsolEntity);
                        MyConsolEntity = DrawConsol(new Point3d(CenterPoint.X - 30, CenterPoint.Y, 0), 10, 10);
                        Entities.Add(MyConsolEntity);


                        break;
                    case 3:
                        MyConsolEntity = DrawConsol(new Point3d(CenterPoint.X + 30, CenterPoint.Y, 0), 10, 10);
                        Entities.Add(MyConsolEntity);
                        MyConsolEntity = DrawConsol(new Point3d(CenterPoint.X, CenterPoint.Y, 0), 10, 10);
                        Entities.Add(MyConsolEntity);
                        MyConsolEntity = DrawConsol(new Point3d(CenterPoint.X - 30, CenterPoint.Y, 0), 10, 10);
                        Entities.Add(MyConsolEntity);

                        break;
                    case 4:
                        MyConsolEntity = DrawConsol(new Point3d(CenterPoint.X + 30, CenterPoint.Y, 0), 10, 10);
                        Entities.Add(MyConsolEntity);
                        MyConsolEntity = DrawConsol(new Point3d(CenterPoint.X, CenterPoint.Y + 12, 0), 10, 10);
                        Entities.Add(MyConsolEntity);
                        MyConsolEntity = DrawConsol(new Point3d(CenterPoint.X, CenterPoint.Y - 12, 0), 10, 10);
                        Entities.Add(MyConsolEntity);
                        MyConsolEntity = DrawConsol(new Point3d(CenterPoint.X - 30, CenterPoint.Y, 0), 10, 10);
                        Entities.Add(MyConsolEntity);

                        break;
                }
                //--------------------



            }

            private Entity DrawPolePolygon(Point3d BasePoint)
            {

                double BaseX = BasePoint.X;
                double BaseY = BasePoint.Y;


                Atend.Global.Acad.AcadJigs.MyPolyLine pLine = new Atend.Global.Acad.AcadJigs.MyPolyLine();
                pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX - 70, BaseY), 0, 0, 0);
                pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX - 35, BaseY + 35), 0, 0, 0);
                pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX + 35, BaseY + 35), 0, 0, 0);
                pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX + 70, BaseY), 0, 0, 0);
                pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX + 35, BaseY - 35), 0, 0, 0);
                pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX - 35, BaseY - 35), 0, 0, 0);
                pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX - 70, BaseY), 0, 0, 0);

                Atend.Global.Acad.AcadJigs.SaveExtensionData(pLine, (int)Atend.Control.Enum.ProductType.Pole);
                pLine.Closed = true;
                return pLine;


            }

            private Entity DrawConsol(Point3d BasePoint, double Width, double Height)
            {

                double BaseX = BasePoint.X;
                double BaseY = BasePoint.Y;

                Atend.Global.Acad.AcadJigs.MyPolyLine pLine = new Atend.Global.Acad.AcadJigs.MyPolyLine();
                pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX - (Width / 2), BaseY - (Height / 2)), 0, 0, 0);
                pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX + (Width / 2), BaseY - (Height / 2)), 0, 0, 0);
                pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX + (Width / 2), BaseY + (Height / 2)), 0, 0, 0);
                pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX - (Width / 2), BaseY + (Height / 2)), 0, 0, 0);
                pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX - (Width / 2), BaseY - (Height / 2)), 0, 0, 0);
                pLine.Closed = true;

                Atend.Global.Acad.AcadJigs.SaveExtensionData(pLine, (int)Atend.Control.Enum.ProductType.Consol);
                return pLine;

            }

            protected override SamplerStatus Sampler(JigPrompts prompts)
            {
                //throw new System.Exception("The method or operation is not implemented.");
                JigPromptPointOptions ppo = new JigPromptPointOptions("\nPole Position : ");
                PromptPointResult ppr = prompts.AcquirePoint(ppo);
                if (ppr.Status == PromptStatus.OK)
                {
                    //if (!Atend.Global.Acad.DrawEquips.AcDrawForbidenArea.PointWasInForbidenArea(ppr.Value))
                    //{
                    if (ppr.Value == CenterPoint)
                    {
                        return SamplerStatus.NoChange;
                    }
                    else
                    {
                        CenterPoint = ppr.Value;
                        return SamplerStatus.OK;
                    }
                    //}
                    //else
                    //{
                    //    return SamplerStatus.Cancel;
                    //}
                }
                else
                {
                    return SamplerStatus.Cancel;
                }

            }

            protected override bool WorldDraw(Autodesk.AutoCAD.GraphicsInterface.WorldDraw draw)
            {
                //throw new System.Exception("The method or operation is not implemented.");

                Entities.Clear();
                Entities.Add(DrawPolePolygon(CenterPoint));

                //-------------------
                Entity MyConsolEntity = null;
                switch (ConsolCount)
                {
                    case 1:

                        MyConsolEntity = DrawConsol(new Point3d(CenterPoint.X, CenterPoint.Y, 0), 10, 10);
                        Entities.Add(MyConsolEntity);

                        break;
                    case 2:


                        MyConsolEntity = DrawConsol(new Point3d(CenterPoint.X + 30, CenterPoint.Y, 0), 10, 10);
                        Entities.Add(MyConsolEntity);
                        MyConsolEntity = DrawConsol(new Point3d(CenterPoint.X - 30, CenterPoint.Y, 0), 10, 10);
                        Entities.Add(MyConsolEntity);


                        break;
                    case 3:
                        MyConsolEntity = DrawConsol(new Point3d(CenterPoint.X + 30, CenterPoint.Y, 0), 10, 10);
                        Entities.Add(MyConsolEntity);
                        MyConsolEntity = DrawConsol(new Point3d(CenterPoint.X, CenterPoint.Y, 0), 10, 10);
                        Entities.Add(MyConsolEntity);
                        MyConsolEntity = DrawConsol(new Point3d(CenterPoint.X - 30, CenterPoint.Y, 0), 10, 10);
                        Entities.Add(MyConsolEntity);

                        break;
                    case 4:
                        MyConsolEntity = DrawConsol(new Point3d(CenterPoint.X + 30, CenterPoint.Y, 0), 10, 10);
                        Entities.Add(MyConsolEntity);
                        MyConsolEntity = DrawConsol(new Point3d(CenterPoint.X, CenterPoint.Y + 12, 0), 10, 10);
                        Entities.Add(MyConsolEntity);
                        MyConsolEntity = DrawConsol(new Point3d(CenterPoint.X, CenterPoint.Y - 12, 0), 10, 10);
                        Entities.Add(MyConsolEntity);
                        MyConsolEntity = DrawConsol(new Point3d(CenterPoint.X - 30, CenterPoint.Y, 0), 10, 10);
                        Entities.Add(MyConsolEntity);

                        break;
                }
                //--------------------


                //~~~~~~~~ SCALE ~~~~~~~~~~

                Matrix3d trans1 = Matrix3d.Scaling(MyScale, new Point3d(CenterPoint.X, CenterPoint.Y, 0));
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

            public List<Entity> GetDemo(Point3d CenterPoint, double RadianAngle)
            {
                Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
                Entities.Clear();
                Entities.Add(DrawPolePolygon(CenterPoint));

                //-------------------
                Entity MyConsolEntity = null;
                switch (ConsolCount)
                {
                    case 1:

                        MyConsolEntity = DrawConsol(new Point3d(CenterPoint.X, CenterPoint.Y, 0), 10, 10);
                        Entities.Add(MyConsolEntity);

                        break;
                    case 2:

                        MyConsolEntity = DrawConsol(new Point3d(CenterPoint.X + 30, CenterPoint.Y, 0), 10, 10);
                        Entities.Add(MyConsolEntity);
                        MyConsolEntity = DrawConsol(new Point3d(CenterPoint.X - 30, CenterPoint.Y, 0), 10, 10);
                        Entities.Add(MyConsolEntity);


                        break;
                    case 3:
                        MyConsolEntity = DrawConsol(new Point3d(CenterPoint.X + 30, CenterPoint.Y, 0), 10, 10);
                        Entities.Add(MyConsolEntity);
                        MyConsolEntity = DrawConsol(new Point3d(CenterPoint.X, CenterPoint.Y, 0), 10, 10);
                        Entities.Add(MyConsolEntity);
                        MyConsolEntity = DrawConsol(new Point3d(CenterPoint.X - 30, CenterPoint.Y, 0), 10, 10);
                        Entities.Add(MyConsolEntity);

                        break;
                    case 4:
                        MyConsolEntity = DrawConsol(new Point3d(CenterPoint.X + 30, CenterPoint.Y, 0), 10, 10);
                        Entities.Add(MyConsolEntity);
                        MyConsolEntity = DrawConsol(new Point3d(CenterPoint.X, CenterPoint.Y + 12, 0), 10, 10);
                        Entities.Add(MyConsolEntity);
                        MyConsolEntity = DrawConsol(new Point3d(CenterPoint.X, CenterPoint.Y - 12, 0), 10, 10);
                        Entities.Add(MyConsolEntity);
                        MyConsolEntity = DrawConsol(new Point3d(CenterPoint.X - 30, CenterPoint.Y, 0), 10, 10);
                        Entities.Add(MyConsolEntity);

                        break;
                }
                //~~~~~~~~ SCALE ~~~~~~~~~~

                Matrix3d trans1 = Matrix3d.Scaling(MyScale, new Point3d(CenterPoint.X, CenterPoint.Y, 0));
                foreach (Entity en in Entities)
                {
                    en.TransformBy(trans1);
                }

                Matrix3d trans = Matrix3d.Rotation(RadianAngle, ed.CurrentUserCoordinateSystem.CoordinateSystem3d.Zaxis, new Point3d(CenterPoint.X, CenterPoint.Y, 0));
                foreach (Entity en in Entities)
                {
                    en.TransformBy(trans);
                }

                return Entities;

            }

        }

        public AcDrawPolygonPole()
        {
            eConsolUseAccess = new ArrayList();
            eConsolExistance = new ArrayList();
            eConsolCount = new ArrayList();
            eConsols = new List<Atend.Base.Equipment.EConsol>();
            eConsolProjectCode = new ArrayList();
        }

        private void ResetClass()
        {
            dNode = new Atend.Base.Design.DNode();
            //_dPoleInfo = new Atend.Base.Design.DPoleInfo();
            dPackages = new List<Atend.Base.Design.DPackage>();
            PolePackage = new Atend.Base.Design.DPackage();
            HalterPackage = new Atend.Base.Design.DPackage();

        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //public bool DrawPolePolygon(Point3d CentrePoint, double Angle)
        //{
        //    Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
        //    ResetClass();
        //    //bool conti = true, PoleSaved = false;
        //    ObjectId NewPoleObjectId = ObjectId.Null;
        //    ObjectIdCollection NewConsolObjectIds = new ObjectIdCollection();
        //    //ObjectIdCollection ids = new ObjectIdCollection();
        //    int i = 0;

        //    double MyScale = Atend.Base.Design.DProductProperties.AccessSelectBySoftwareCode((int)Atend.Control.Enum.ProductType.Pole).Scale;
        //    double MyCommentScale = Atend.Base.Design.DProductProperties.AccessSelectBySoftwareCode((int)Atend.Control.Enum.ProductType.Pole).CommentScale;
        //    double MyCommentScaleConsol = Atend.Base.Design.DProductProperties.AccessSelectBySoftwareCode((int)Atend.Control.Enum.ProductType.Consol).CommentScale;


        //    try
        //    {
        //        using (DocumentLock docLock = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.LockDocument())
        //        {
        //            DrawPolePolygonJig polePolygon = new DrawPolePolygonJig(eConsols.Count, MyScale);
        //            List<Entity> entities = polePolygon.GetDemo(CentrePoint, (Math.PI * Angle) / 180);

        //            #region Save data here
        //            if (SavePoleData())
        //            {
        //                //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //                foreach (Entity ent in entities)
        //                {
        //                    object productType = null;
        //                    Entity newEntity = ent;
        //                    Atend.Global.Acad.AcadJigs.MyPolyLine myPoly = ent as Atend.Global.Acad.AcadJigs.MyPolyLine;
        //                    if (myPoly.AdditionalDictionary.ContainsKey("ProductType"))
        //                    {
        //                        myPoly.AdditionalDictionary.TryGetValue("ProductType", out productType);
        //                    }
        //                    else
        //                    {
        //                        return false;
        //                    }
        //                    //ed.WriteMessage("ProductType= " + productType.ToString() + "\n");

        //                    if (Convert.ToInt32(productType) == (int)Atend.Control.Enum.ProductType.Pole)
        //                    {

        //                        NewPoleObjectId = Atend.Global.Acad.UAcad.DrawEntityOnScreen(ent, Atend.Control.Enum.AutoCadLayerName.GENERAL.ToString());

        //                        Atend.Base.Acad.AT_INFO at_info = new Atend.Base.Acad.AT_INFO();
        //                        at_info.ParentCode = "";
        //                        at_info.NodeCode = dNode.Code.ToString();
        //                        at_info.NodeType = Convert.ToInt32(productType);
        //                        at_info.ProductCode = dNode.ProductCode;
        //                        at_info.SelectedObjectId = ent.ObjectId;
        //                        at_info.Insert();

        //                        ObjectId TextOi = Atend.Global.Acad.UAcad.DrawEntityOnScreen(Atend.Global.Acad.UAcad.WriteNote(PolePackage.Number, Atend.Global.Acad.UAcad.CenterOfEntity(Atend.Global.Acad.UAcad.GetEntityByObjectID(ent.ObjectId)), MyCommentScale), Atend.Control.Enum.AutoCadLayerName.GENERAL.ToString());

        //                        Atend.Base.Acad.AT_INFO textInfo = new Atend.Base.Acad.AT_INFO(TextOi);
        //                        textInfo.ParentCode = at_info.NodeCode;
        //                        textInfo.NodeType = (int)Atend.Control.Enum.ProductType.Comment;
        //                        textInfo.NodeCode = "";
        //                        textInfo.ProductCode = 0;
        //                        textInfo.Insert();

        //                        Atend.Base.Acad.AT_SUB polesub = new Atend.Base.Acad.AT_SUB(NewPoleObjectId);
        //                        polesub.SubIdCollection.Add(TextOi);
        //                        polesub.Insert();

        //                        Atend.Base.Acad.AT_SUB textSub = new Atend.Base.Acad.AT_SUB(TextOi);
        //                        textSub.SubIdCollection.Add(NewPoleObjectId);
        //                        textSub.Insert();


        //                        // ed.WriteMessage("Extension was done \n");
        //                    }
        //                    else if (Convert.ToInt32(productType) == (int)Atend.Control.Enum.ProductType.Consol)
        //                    {
        //                        // add extention data
        //                        //ed.WriteMessage("The Entity Is Consol\n");
        //                        //Atend.Base.Design.DPackage package = dPackages[i];
        //                        bool IsWeek = false;
        //                        Atend.Base.Equipment.EConsol eConsol = eConsols[i];  //Atend.Base.Equipment.EConsol.SelectByCodeForDesign(package.ProductCode);


        //                        switch (eConsol.VoltageLevel)
        //                        {
        //                            case 20000:
        //                                IsWeek = false;
        //                                break;
        //                            case 11000:
        //                                IsWeek = false;
        //                                break;
        //                            case 33000:
        //                                IsWeek = false;
        //                                break;
        //                            case 400:
        //                                IsWeek = true;
        //                                break;
        //                        }
        //                        string LayerName;
        //                        if (IsWeek)
        //                        {
        //                            LayerName = Atend.Control.Enum.AutoCadLayerName.LOW_AIR.ToString();
        //                        }
        //                        else
        //                        {
        //                            LayerName = Atend.Control.Enum.AutoCadLayerName.MED_AIR.ToString();
        //                        }



        //                        Atend.Base.Design.DConsol consol = new Atend.Base.Design.DConsol();
        //                        //ed.WriteMessage("ConsolCount= " + dConsolCode.Count.ToString() + "\n");
        //                        consol.Code = dPackages[i].Code;
        //                        consol.LoadCode = 0;
        //                        consol.ProductCode = eConsol.Code;
        //                        consol.ParentCode = dNode.Code;

        //                        //consol.DesignCode = Atend.Control.Common.SelectedDesignCode;
        //                        //ed.WriteMessage("ConsolCode= " + consol.Code.ToString() + "\n");
        //                        //ed.WriteMessage("ParentCode= " + dNode.Code.ToString() + "\n");
        //                        if (consol.AccessInsert())
        //                        {
        //                            ObjectId NewConsolObjectID = Atend.Global.Acad.UAcad.DrawEntityOnScreen(ent, LayerName);
        //                            NewConsolObjectIds.Add(NewConsolObjectID);

        //                            Atend.Base.Acad.AT_INFO at_info = new Atend.Base.Acad.AT_INFO();

        //                            at_info.ParentCode = dNode.Code.ToString();
        //                            at_info.NodeCode = dPackages[i].Code.ToString();
        //                            at_info.NodeType = Convert.ToInt32(productType);
        //                            at_info.ProductCode = dPackages[i].ProductCode;
        //                            at_info.SelectedObjectId = ent.ObjectId;
        //                            at_info.Insert();

        //                            ObjectId TextOi = Atend.Global.Acad.UAcad.DrawEntityOnScreen(Atend.Global.Acad.UAcad.WriteNote(dPackages[i].Number, ((Polyline)Atend.Global.Acad.UAcad.GetEntityByObjectID(ent.ObjectId)).GetPoint3dAt(0), MyCommentScaleConsol), Atend.Control.Enum.AutoCadLayerName.GENERAL.ToString());

        //                            Atend.Base.Acad.AT_INFO textInfo = new Atend.Base.Acad.AT_INFO(TextOi);
        //                            textInfo.ParentCode = at_info.NodeCode;
        //                            textInfo.NodeType = (int)Atend.Control.Enum.ProductType.Comment;
        //                            textInfo.NodeCode = "";
        //                            textInfo.ProductCode = 0;
        //                            textInfo.Insert();

        //                            Atend.Base.Acad.AT_SUB at_sub = new Atend.Base.Acad.AT_SUB();
        //                            at_sub.SelectedObjectId = NewConsolObjectID;
        //                            at_sub.SubIdCollection.Add(NewPoleObjectId);
        //                            at_sub.SubIdCollection.Add(TextOi);
        //                            at_sub.Insert();

        //                            Atend.Base.Acad.AT_SUB textSub = new Atend.Base.Acad.AT_SUB(TextOi);
        //                            textSub.SubIdCollection.Add(NewConsolObjectID);
        //                            textSub.Insert();

        //                        }

        //                        i++;
        //                        // ed.WriteMessage("Extension was done \n");

        //                    }//End of DRaw consol

        //                }// Draw Finished

        //                //insert consols as a sub for pole

        //                foreach (ObjectId obji in NewConsolObjectIds)
        //                {
        //                    Atend.Base.Acad.AT_SUB.AddToAT_SUB(obji, NewPoleObjectId);
        //                }

        //                //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //            }
        //            #endregion
        //        }
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //    return true;
        //}

        public bool DrawPolePolygon(Point3d CentrePoint, double Angle, out ObjectIdCollection NewDrawnNodes)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            ResetClass();
            NewDrawnNodes = new ObjectIdCollection();
            //bool conti = true, PoleSaved = false;
            ObjectId NewPoleObjectId = ObjectId.Null;
            ObjectIdCollection NewConsolObjectIds = new ObjectIdCollection();
            //ObjectIdCollection ids = new ObjectIdCollection();
            int i = 0;

            double MyScale = Atend.Base.Design.DProductProperties.AccessSelectBySoftwareCode((int)Atend.Control.Enum.ProductType.Pole).Scale;
            double MyCommentScale = Atend.Base.Design.DProductProperties.AccessSelectBySoftwareCode((int)Atend.Control.Enum.ProductType.Pole).CommentScale;
            double MyCommentScaleConsol = Atend.Base.Design.DProductProperties.AccessSelectBySoftwareCode((int)Atend.Control.Enum.ProductType.Consol).CommentScale;

            //ed.WriteMessage("^^^^^^ after select ^^^^^^^^\n");
            try
            {
                using (DocumentLock docLock = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.LockDocument())
                {
                    //ed.WriteMessage("1\n");
                    DrawPolePolygonJig polePolygon = new DrawPolePolygonJig(eConsols.Count, MyScale);
                    //ed.WriteMessage("2\n");
                    List<Entity> entities = polePolygon.GetDemo(CentrePoint, (Math.PI * Angle) / 180);
                    //ed.WriteMessage("^^^^^^ Before save entity counter = {0} ^^^^^^^^\n",entities.Count);
                    #region Save data here
                    if (SavePoleData())
                    {
                        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
                        foreach (Entity ent in entities)
                        {
                            object productType = null;
                            Entity newEntity = ent;
                            Atend.Global.Acad.AcadJigs.MyPolyLine myPoly = ent as Atend.Global.Acad.AcadJigs.MyPolyLine;
                            if (myPoly != null)
                            {
                                if (myPoly.AdditionalDictionary.ContainsKey("ProductType"))
                                {
                                    myPoly.AdditionalDictionary.TryGetValue("ProductType", out productType);
                                }
                                else
                                {
                                    return false;
                                }
                            }
                            else
                            {
                                return false;
                            }
                            //ed.WriteMessage(" ^^^^^^ ProductType= " + productType.ToString() + "\n");
                            if (Convert.ToInt32(productType) == (int)Atend.Control.Enum.ProductType.Pole)
                            {
                                NewPoleObjectId = Atend.Global.Acad.UAcad.DrawEntityOnScreen(ent, Atend.Control.Enum.AutoCadLayerName.GENERAL.ToString());

                                Atend.Base.Acad.AT_INFO at_info = new Atend.Base.Acad.AT_INFO(ent.ObjectId);
                                at_info.ParentCode = "";
                                at_info.NodeCode = dNode.Code.ToString();
                                at_info.NodeType = Convert.ToInt32(productType);
                                at_info.ProductCode = dNode.ProductCode;
                                //at_info.SelectedObjectId = ;
                                at_info.Insert();

                                Point3d CommentPosition = Atend.Global.Acad.Global.PoleCommentPosition(Atend.Global.Acad.UAcad.GetEntityByObjectID(ent.ObjectId));
                                if (CommentPosition != Point3d.Origin)
                                {

                                    ObjectId TextOi = Atend.Global.Acad.UAcad.DrawEntityOnScreen(Atend.Global.Acad.UAcad.WriteNote(PolePackage.Number, CommentPosition, MyCommentScale), Atend.Control.Enum.AutoCadLayerName.GENERAL.ToString());

                                    Atend.Base.Acad.AT_INFO textInfo = new Atend.Base.Acad.AT_INFO(TextOi);
                                    textInfo.ParentCode = at_info.NodeCode;
                                    textInfo.NodeType = (int)Atend.Control.Enum.ProductType.Comment;
                                    textInfo.NodeCode = "";
                                    textInfo.ProductCode = 0;
                                    textInfo.Insert();

                                    Atend.Base.Acad.AT_SUB polesub = new Atend.Base.Acad.AT_SUB(NewPoleObjectId);
                                    polesub.SubIdCollection.Add(TextOi);
                                    polesub.Insert();

                                    Atend.Base.Acad.AT_SUB textSub = new Atend.Base.Acad.AT_SUB(TextOi);
                                    textSub.SubIdCollection.Add(NewPoleObjectId);
                                    textSub.Insert();

                                }
                                // ed.WriteMessage("Extension was done \n");
                            }
                            else if (Convert.ToInt32(productType) == (int)Atend.Control.Enum.ProductType.Consol)
                            {
                                // add extention data
                                //ed.WriteMessage("The Entity Is Consol\n");
                                //Atend.Base.Design.DPackage package = dPackages[i];
                                bool IsWeek = false;
                                Atend.Base.Equipment.EConsol eConsol = eConsols[i];  //Atend.Base.Equipment.EConsol.SelectByCodeForDesign(package.ProductCode);

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
                                Atend.Base.Design.DConsol consol = new Atend.Base.Design.DConsol();
                                //ed.WriteMessage("ConsolCount= " + dConsolCode.Count.ToString() + "\n");
                                consol.Code = dPackages[i].Code;
                                consol.LoadCode = 0;
                                consol.ProductCode = eConsol.Code;
                                consol.ParentCode = dNode.Code;

                                //consol.DesignCode = Atend.Control.Common.SelectedDesignCode;
                                //ed.WriteMessage("ConsolCode= " + consol.Code.ToString() + "\n");
                                //ed.WriteMessage("ParentCode= " + dNode.Code.ToString() + "\n");
                                if (consol.AccessInsert())
                                {
                                    ObjectId NewConsolObjectID = Atend.Global.Acad.UAcad.DrawEntityOnScreen(ent, LayerName);
                                    NewConsolObjectIds.Add(NewConsolObjectID);

                                    Atend.Base.Acad.AT_INFO at_info = new Atend.Base.Acad.AT_INFO(ent.ObjectId);

                                    at_info.ParentCode = dNode.Code.ToString();
                                    at_info.NodeCode = dPackages[i].Code.ToString();
                                    at_info.NodeType = Convert.ToInt32(productType);
                                    at_info.ProductCode = dPackages[i].ProductCode;
                                    //at_info.SelectedObjectId =;
                                    at_info.Insert();

                                    ObjectId TextOi = Atend.Global.Acad.UAcad.DrawEntityOnScreen(Atend.Global.Acad.UAcad.WriteNote(dPackages[i].Number, ((Polyline)Atend.Global.Acad.UAcad.GetEntityByObjectID(ent.ObjectId)).GetPoint3dAt(0), MyCommentScaleConsol), Atend.Control.Enum.AutoCadLayerName.GENERAL.ToString());

                                    Atend.Base.Acad.AT_INFO textInfo = new Atend.Base.Acad.AT_INFO(TextOi);
                                    textInfo.ParentCode = at_info.NodeCode;
                                    textInfo.NodeType = (int)Atend.Control.Enum.ProductType.Comment;
                                    textInfo.NodeCode = "";
                                    textInfo.ProductCode = 0;
                                    textInfo.Insert();

                                    Atend.Base.Acad.AT_SUB at_sub = new Atend.Base.Acad.AT_SUB();
                                    at_sub.SelectedObjectId = NewConsolObjectID;
                                    at_sub.SubIdCollection.Add(NewPoleObjectId);
                                    at_sub.SubIdCollection.Add(TextOi);
                                    at_sub.Insert();

                                    Atend.Base.Acad.AT_SUB textSub = new Atend.Base.Acad.AT_SUB(TextOi);
                                    textSub.SubIdCollection.Add(NewConsolObjectID);
                                    textSub.Insert();

                                }

                                i++;
                                // ed.WriteMessage("Extension was done \n");
                            }//End of DRaw consol
                        }// Draw Finished

                        //insert consols as a sub for pole

                        foreach (ObjectId obji in NewConsolObjectIds)
                        {
                            Atend.Base.Acad.AT_SUB.AddToAT_SUB(obji, NewPoleObjectId);
                        }

                        if (eConsols.Count != 0)
                        {
                            NewDrawnNodes = NewConsolObjectIds;
                        }
                        else
                        {
                            NewDrawnNodes.Add(NewPoleObjectId);
                        }
                        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
                    }
                    #endregion
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        public void DrawPolePolygon()
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            ResetClass();
            bool conti = true, PoleSaved = false;
            ObjectId NewPoleObjectId = ObjectId.Null;
            ObjectIdCollection NewConsolObjectIds = new ObjectIdCollection();
            //ObjectIdCollection ids = new ObjectIdCollection();
            int i = 0;

            double MyScale = Atend.Base.Design.DProductProperties.AccessSelectBySoftwareCode((int)Atend.Control.Enum.ProductType.Pole).Scale;
            double MyCommentScale = Atend.Base.Design.DProductProperties.AccessSelectBySoftwareCode((int)Atend.Control.Enum.ProductType.Pole).CommentScale;
            double MyCommentScaleConsol = Atend.Base.Design.DProductProperties.AccessSelectBySoftwareCode((int)Atend.Control.Enum.ProductType.Consol).CommentScale;

            using (DocumentLock docLock = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.LockDocument())
            {


                DrawPolePolygonJig polePolygon = new DrawPolePolygonJig(eConsols.Count, MyScale);

                while (conti)
                {

                    PromptResult pr = ed.Drag(polePolygon);

                    if (pr.Status == PromptStatus.OK)
                    {
                        if (!Atend.Global.Acad.DrawEquips.AcDrawForbidenArea.PointWasInForbidenArea(polePolygon.PoleCenterPoint))
                        {

                            conti = false;
                            List<Entity> entities = polePolygon.GetEntities();
                            #region Save data here
                            if (SavePoleData())
                            {
                                //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
                                foreach (Entity ent in entities)
                                {
                                    object productType = null;
                                    Entity newEntity = ent;
                                    Atend.Global.Acad.AcadJigs.MyPolyLine myPoly = ent as Atend.Global.Acad.AcadJigs.MyPolyLine;
                                    if (myPoly.AdditionalDictionary.ContainsKey("ProductType"))
                                    {
                                        myPoly.AdditionalDictionary.TryGetValue("ProductType", out productType);
                                    }
                                    else
                                    {
                                        return;
                                    }
                                    //ed.WriteMessage("ProductType= " + productType.ToString() + "\n");

                                    if (Convert.ToInt32(productType) == (int)Atend.Control.Enum.ProductType.Pole)
                                    {

                                        NewPoleObjectId = Atend.Global.Acad.UAcad.DrawEntityOnScreen(ent, Atend.Control.Enum.AutoCadLayerName.GENERAL.ToString());

                                        Atend.Base.Acad.AT_INFO at_info = new Atend.Base.Acad.AT_INFO();
                                        at_info.ParentCode = "";
                                        at_info.NodeCode = dNode.Code.ToString();
                                        at_info.NodeType = Convert.ToInt32(productType);
                                        at_info.ProductCode = dNode.ProductCode;
                                        at_info.SelectedObjectId = ent.ObjectId;
                                        at_info.Insert();

                                        Point3d CommentPosition = Atend.Global.Acad.Global.PoleCommentPosition(Atend.Global.Acad.UAcad.GetEntityByObjectID(ent.ObjectId));
                                        if (CommentPosition != Point3d.Origin)
                                        {

                                            ObjectId TextOi = Atend.Global.Acad.UAcad.DrawEntityOnScreen(Atend.Global.Acad.UAcad.WriteNote(PolePackage.Number, CommentPosition, MyCommentScale), Atend.Control.Enum.AutoCadLayerName.GENERAL.ToString());

                                            Atend.Base.Acad.AT_INFO textInfo = new Atend.Base.Acad.AT_INFO(TextOi);
                                            textInfo.ParentCode = at_info.NodeCode;
                                            textInfo.NodeType = (int)Atend.Control.Enum.ProductType.Comment;
                                            textInfo.NodeCode = "";
                                            textInfo.ProductCode = 0;
                                            textInfo.Insert();

                                            Atend.Base.Acad.AT_SUB polesub = new Atend.Base.Acad.AT_SUB(NewPoleObjectId);
                                            polesub.SubIdCollection.Add(TextOi);
                                            polesub.Insert();

                                            Atend.Base.Acad.AT_SUB textSub = new Atend.Base.Acad.AT_SUB(TextOi);
                                            textSub.SubIdCollection.Add(NewPoleObjectId);
                                            textSub.Insert();
                                        }

                                        // ed.WriteMessage("Extension was done \n");
                                    }
                                    else if (Convert.ToInt32(productType) == (int)Atend.Control.Enum.ProductType.Consol)
                                    {
                                        // add extention data
                                        //ed.WriteMessage("The Entity Is Consol\n");
                                        //Atend.Base.Design.DPackage package = dPackages[i];
                                        bool IsWeek = false;
                                        Atend.Base.Equipment.EConsol eConsol = eConsols[i];  //Atend.Base.Equipment.EConsol.SelectByCodeForDesign(package.ProductCode);


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



                                        Atend.Base.Design.DConsol consol = new Atend.Base.Design.DConsol();
                                        //ed.WriteMessage("ConsolCount= " + dConsolCode.Count.ToString() + "\n");
                                        consol.Code = dPackages[i].Code;
                                        consol.LoadCode = 0;
                                        consol.ProductCode = eConsol.Code;
                                        consol.ParentCode = dNode.Code;

                                        //consol.DesignCode = Atend.Control.Common.SelectedDesignCode;
                                        //ed.WriteMessage("ConsolCode= " + consol.Code.ToString() + "\n");
                                        //ed.WriteMessage("ParentCode= " + dNode.Code.ToString() + "\n");
                                        if (consol.AccessInsert())
                                        {
                                            ObjectId NewConsolObjectID = Atend.Global.Acad.UAcad.DrawEntityOnScreen(ent, LayerName);
                                            NewConsolObjectIds.Add(NewConsolObjectID);

                                            Atend.Base.Acad.AT_INFO at_info = new Atend.Base.Acad.AT_INFO();

                                            at_info.ParentCode = dNode.Code.ToString();
                                            at_info.NodeCode = dPackages[i].Code.ToString();
                                            at_info.NodeType = Convert.ToInt32(productType);
                                            at_info.ProductCode = dPackages[i].ProductCode;
                                            at_info.SelectedObjectId = ent.ObjectId;
                                            at_info.Insert();

                                            ObjectId TextOi = Atend.Global.Acad.UAcad.DrawEntityOnScreen(Atend.Global.Acad.UAcad.WriteNote(dPackages[i].Number, ((Polyline)Atend.Global.Acad.UAcad.GetEntityByObjectID(ent.ObjectId)).GetPoint3dAt(0), MyCommentScaleConsol), Atend.Control.Enum.AutoCadLayerName.GENERAL.ToString());

                                            Atend.Base.Acad.AT_INFO textInfo = new Atend.Base.Acad.AT_INFO(TextOi);
                                            textInfo.ParentCode = at_info.NodeCode;
                                            textInfo.NodeType = (int)Atend.Control.Enum.ProductType.Comment;
                                            textInfo.NodeCode = "";
                                            textInfo.ProductCode = 0;
                                            textInfo.Insert();

                                            Atend.Base.Acad.AT_SUB at_sub = new Atend.Base.Acad.AT_SUB();
                                            at_sub.SelectedObjectId = NewConsolObjectID;
                                            at_sub.SubIdCollection.Add(NewPoleObjectId);
                                            at_sub.SubIdCollection.Add(TextOi);
                                            at_sub.Insert();

                                            Atend.Base.Acad.AT_SUB textSub = new Atend.Base.Acad.AT_SUB(TextOi);
                                            textSub.SubIdCollection.Add(NewConsolObjectID);
                                            textSub.Insert();

                                        }

                                        i++;
                                        // ed.WriteMessage("Extension was done \n");

                                    }//End of DRaw consol

                                }// Draw Finished

                                //insert consols as a sub for pole

                                foreach (ObjectId obji in NewConsolObjectIds)
                                {
                                    Atend.Base.Acad.AT_SUB.AddToAT_SUB(obji, NewPoleObjectId);
                                }

                                //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
                            }
                            #endregion
                        }
                    }
                    else
                    {
                        conti = false;
                    }


                }

            }

        }

        public static void RotatePolePolygon(double NewAngleDegree, Guid PoleCode)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            ObjectId PoleOI = Atend.Global.Acad.UAcad.GetPoleByGuid(PoleCode);
            if (PoleOI != ObjectId.Null)
            {
                //ed.WriteMessage("pole oi found \n");
                Entity PoleEntity = Atend.Global.Acad.UAcad.GetEntityByObjectID(PoleOI);
                Point3d CenterPoint = Atend.Global.Acad.UAcad.CenterOfEntity(PoleEntity);
                Atend.Base.Acad.AT_INFO PoleInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(PoleOI);
                double LastAngleDegree = PoleInfo.Angle;
                if (PoleEntity != null)
                {
                    //ed.WriteMessage("pole entity found \n");
                    using (DocumentLock docLock = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.LockDocument())
                    {

                        Database db = Application.DocumentManager.MdiActiveDocument.Database;
                        using (Transaction tr = db.TransactionManager.StartTransaction())
                        {
                            Entity ent = (Entity)tr.GetObject(PoleOI, OpenMode.ForWrite);
                            Polyline LineEntity = ent as Polyline;
                            if (LineEntity != null)
                            {
                                //ed.WriteMessage("LineEntity entity found \n");
                                Matrix3d trans = Matrix3d.Rotation(((PoleInfo.Angle * Math.PI) / 180) * -1, ed.CurrentUserCoordinateSystem.CoordinateSystem3d.Zaxis, CenterPoint);
                                LineEntity.TransformBy(trans);

                                trans = Matrix3d.Rotation(((NewAngleDegree * Math.PI) / 180), ed.CurrentUserCoordinateSystem.CoordinateSystem3d.Zaxis, CenterPoint);
                                LineEntity.TransformBy(trans);

                            }
                            tr.Commit();
                            PoleInfo.Angle = NewAngleDegree;
                            PoleInfo.Insert();


                            Atend.Base.Acad.AT_SUB PoleSubs = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(PoleOI);
                            foreach (ObjectId oi in PoleSubs.SubIdCollection)
                            {
                                Atend.Base.Acad.AT_INFO SubInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oi);
                                switch ((Atend.Control.Enum.ProductType)SubInfo.NodeType)
                                {
                                    case Atend.Control.Enum.ProductType.Consol:
                                        AcDrawConsol.RotateConsol(LastAngleDegree, NewAngleDegree, PoleOI, oi, CenterPoint);
                                        break;
                                    case Atend.Control.Enum.ProductType.Kalamp:
                                        AcDrawKalamp.RotateKalamp(LastAngleDegree, NewAngleDegree, PoleOI, oi, CenterPoint);
                                        break;
                                    case Atend.Control.Enum.ProductType.HeaderCabel:
                                        AcDrawHeaderCabel.RotateHeaderCable(LastAngleDegree, NewAngleDegree, PoleOI, oi, CenterPoint);
                                        break;
                                    case Atend.Control.Enum.ProductType.BankKhazan:
                                        AcDrawKhazan.RotateKhazan(LastAngleDegree, NewAngleDegree, PoleOI, oi, CenterPoint);
                                        break;
                                    case Atend.Control.Enum.ProductType.Rod:
                                        AcDrawRod.RotateRod(LastAngleDegree, NewAngleDegree, PoleOI, oi, CenterPoint);
                                        break;
                                }
                            }




                        }//transaction
                    }

                }
            }
        }

        private bool SavePoleData()
        {

            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;

            //ed.WriteMessage("~~~~~~~~~~~~~ Start Save Pole Data ~~~~~~~~~~~~~~~~~~\n");
            //ed.WriteMessage("~~{0}~~\n",Atend.Control.ConnectionString.AccessCnString);
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
                        if (!ePole.AccessInsert(aTransaction, aConnection, true, true))
                        {
                            throw new System.Exception("ePole.AccessInsert failed");
                        }

                    }
                    //econsol
                    int i = 0;
                    foreach (Atend.Base.Equipment.EConsol SelectedConsol in eConsols)
                    {
                        if (!Convert.ToBoolean(eConsolUseAccess[i]))
                        {
                            if (!SelectedConsol.AccessInsert(aTransaction, aConnection, true, true))
                            {
                                throw new System.Exception("SelectedConsol.AccessInsert failed");
                            }
                        }
                    }///////////////////////////////////////////////////////////////////////////////////
                    if (eHalterCount != 0)
                    {
                        if (!eHalter.AccessInsert(aTransaction, aConnection, true, true))
                        {
                            throw new System.Exception("Halter.AccessInsert failed");
                        }
                    }

                    //ed.WriteMessage("econsols saved \n");
                    dNode = new Atend.Base.Design.DNode();
                    dNode.Number = "N001";
                    dNode.ProductCode = ePole.Code;
                    dNode.Height = Height;
                    if (!dNode.AccessInsert(aTransaction, aConnection))
                    {
                        throw new System.Exception("PoleNode.AccessInsert failed");
                    }

                    if (eHalterCount != 0)
                    {
                        dPoleInfo.NodeCode = dNode.Code;
                        dPoleInfo.HalterType = eHalter.Code;
                        dPoleInfo.HalterCount = eHalterCount;
                    }
                    else
                    {
                        dPoleInfo.NodeCode = dNode.Code;
                    }
                    if (!_dPoleInfo.AcessInsert(aTransaction, aConnection))
                    {
                        throw new System.Exception("_dPoleInfo.AcessInsert failed");
                    }


                    PolePackage = new Atend.Base.Design.DPackage();
                    PolePackage.Count = 1;
                    PolePackage.IsExistance = Existance;
                    PolePackage.NodeCode = dNode.Code;
                    PolePackage.ProductCode = ePole.Code;
                    PolePackage.Type = (int)Atend.Control.Enum.ProductType.Pole;
                    Atend.Control.Common.Counters.PoleCounter++;
                    PolePackage.Number = string.Format("P{2}-{0:00}{1:00}", ePole.Height, Math.Round(ePole.Power / 100, 0), Atend.Control.Common.Counters.PoleCounter);
                    PolePackage.ProjectCode = ProjectCode;
                    if (!PolePackage.AccessInsert(aTransaction, aConnection))
                    {
                        throw new System.Exception("PolePackage.AccessInsert failed");
                    }
                    else
                    {
                        //ed.WriteMessage("PNC:{0} PDC:{1} \n", PolePackage.NodeCode, PolePackage.Code);
                        //dPackages.Add(PolePackage);
                        //فقط کنسول ها را در لیست گذاشته ایم
                    }


                    i = 0;
                    foreach (Atend.Base.Equipment.EConsol consol in eConsols)
                    {

                        Atend.Base.Design.DPackage ConsolPackage = new Atend.Base.Design.DPackage();
                        ConsolPackage.Count = Convert.ToInt32(eConsolCount[i]);
                        ConsolPackage.IsExistance = Convert.ToInt32(eConsolExistance[i]);
                        ConsolPackage.ParentCode = PolePackage.Code;
                        ConsolPackage.ProductCode = consol.Code;
                        ConsolPackage.Type = (int)Atend.Control.Enum.ProductType.Consol;
                        Atend.Control.Common.Counters.ConsolCounter++;
                        ConsolPackage.Number = string.Format("Ins{0:0000}", Atend.Control.Common.Counters.ConsolCounter);
                        ConsolPackage.ProjectCode = Convert.ToInt32(eConsolProjectCode[i]);
                        if (!ConsolPackage.AccessInsert(aTransaction, aConnection))
                        {
                            throw new System.Exception("ConsolPackage.AccessInsert failed");
                        }
                        else
                        {
                            //ed.WriteMessage("NEW CONSOL CODE : {0}  \n", ConsolPackage.Code);
                            dPackages.Add(ConsolPackage);
                        }

                        i++;
                    }


                    if (eHalterCount != 0)
                    {
                        HalterPackage = new Atend.Base.Design.DPackage();
                        HalterPackage.Count = eHalterCount;
                        HalterPackage.IsExistance = HalterExistance;
                        HalterPackage.NodeCode = Guid.Empty;
                        HalterPackage.ProjectCode = HalterProjectCode;
                        HalterPackage.ProductCode = eHalter.Code;
                        HalterPackage.ParentCode = PolePackage.Code;
                        HalterPackage.Type = (int)Atend.Control.Enum.ProductType.Halter;
                        HalterPackage.Number = string.Empty;
                        if (!HalterPackage.AccessInsert(aTransaction, aConnection))
                        {
                            throw new System.Exception("HalterPackage.AccessInsert failed");
                        }
                        else
                        {
                            //dPackages.Add(PolePackage);
                            //فقط کنسول ها را در لیست گذاشته ایم
                        }
                    }
                }
                catch (System.Exception ex1)
                {
                    ed.WriteMessage("ERROR SavePoleData: 02 {0} \n", ex1.Message);
                    aTransaction.Rollback();
                    aConnection.Close();
                    return false;

                }

            }
            catch (System.Exception ex)
            {
                ed.WriteMessage("ERROR SavePoleData: 01 {0} \n", ex.Message);
                aConnection.Close();
                return false;
            }
            aTransaction.Commit();
            aConnection.Close();

            #region Not to Get Green

            Atend.Base.Acad.AcadGlobal.PoleData.UseAccess = true;
            //UseAccess = true;
            for (int i = 0; i < Atend.Base.Acad.AcadGlobal.PoleData.eConsolUseAccess.Count; i++)
            {
                Atend.Base.Acad.AcadGlobal.PoleData.eConsolUseAccess[i] = true;
            }

            #endregion


            //ed.WriteMessage("~~~~~~~~~~~~~ End Save Pole Data ~~~~~~~~~~~~~~~~~~\n");

            return true;

        }

        public bool UpdatePoleData(ObjectId PoleObjectId, ArrayList ConsolCodeForDel, Guid PoleCode)
        {
            //Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            //OleDbTransaction aTransaction;
            //OleDbConnection aConnection = new OleDbConnection(Atend.Control.ConnectionString.AccessCnString);
            //ArrayList AddConsol = new ArrayList();
            //try
            //{
            //    aConnection.Open();
            //    aTransaction = aConnection.BeginTransaction();
            //    try
            //    {
            //        PolePackage = Atend.Base.Design.DPackage.AccessSelectByCode(EXCode);
            //        if (!UseAccess)
            //        {
            //            ed.WriteMessage("Insert EPole\n");
            //            if (!ePole.AccessInsert(aTransaction, aConnection))
            //                throw new System.Exception("ePole.AccessInsert failed");

            //        }
            //        PolePackage.IsExistance = Existance;
            //        PolePackage.ProductCode = ePole.Code;
            //        PolePackage.ProjectCode = ProjectCode;
            //        PolePackage.Number = "";
            //        if (!PolePackage.AccessUpdate(aTransaction, aConnection))
            //        {
            //            throw new System.Exception("PolePackage.AccessInsert(tr) failed");
            //        }

            //        foreach (Atend.Base.Design.DPackage pack in beforDPackage)
            //        {
            //            ed.WriteMessage("Update Pack\n");
            //            if (!pack.AccessUpdate(aTransaction, aConnection))
            //            {
            //                throw new System.Exception("Update dPackage Failed");
            //            }
            //        }
            //        foreach (string gu in ConsolCodeForDel)
            //        {
            //            ed.WriteMessage("DeleteConsol\n");
            //            if (!Atend.Base.Design.DPackage.AccessDelete(new Guid(gu), aTransaction, aConnection))
            //            {
            //                throw new System.Exception("ConsolPackage.AccessDelete failed");

            //            }
            //        }



            //        int i = 0;
            //        foreach (Atend.Base.Equipment.EConsol Consols in eConsols)
            //        {
            //            ed.WriteMessage("UseAcces={0}\n", eConsolUseAccess[i].ToString());
            //            if (!Convert.ToBoolean(eConsolUseAccess[i]))
            //            {
            //                ed.WriteMessage("Consol.AccessInsert\n");
            //                if (!Consols.AccessInsert(aTransaction, aConnection))
            //                {
            //                    throw new System.Exception("eConsols.AccessInsert failed");
            //                }
            //            }
            //            ed.WriteMessage("Consol.Code={0}\n", Consols.Code);
            //            Atend.Base.Acad.AT_INFO poleInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(PoleObjectId);
            //            Atend.Base.Design.DPackage dp = Atend.Base.Design.DPackage.AccessSelectByNodeCode(new Guid(poleInfo.NodeCode));

            //            Atend.Base.Design.DPackage ConsolPackage = new Atend.Base.Design.DPackage();
            //            ConsolPackage.Count = Convert.ToInt32(eConsolCount[i]);
            //            ConsolPackage.IsExistance = Convert.ToInt32(eConsolExistance[i]);
            //            ConsolPackage.ParentCode = dp.Code;
            //            ConsolPackage.ProductCode = Consols.Code;
            //            Atend.Control.Common.Counters.ConsolCounter++;
            //            ConsolPackage.Number = string.Format("Ins{0:0000}", Atend.Control.Common.Counters.ConsolCounter);
            //            ConsolPackage.Type = (int)Atend.Control.Enum.ProductType.Consol;
            //            ConsolPackage.ProjectCode = Convert.ToInt32(eConsolProjectCode[i]);
            //            if (!ConsolPackage.AccessInsert(aTransaction, aConnection))
            //            {
            //                throw new System.Exception("ConsolPackage.AccessInsert failed");
            //            }
            //            ed.WriteMessage("ConsolPackage.Code={0}\n", ConsolPackage.Code);
            //            AddConsol.Add(ConsolPackage.Code);

            //            i++;
            //        }

            //        if (PoleObjectId != ObjectId.Null)
            //        {
            //            Atend.Base.Acad.AT_INFO atinfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(PoleObjectId);
            //            atinfo.ProductCode = ePole.Code;
            //            atinfo.Insert();
            //        }




            //    }
            //    catch (System.Exception ex1)
            //    {
            //        ed.WriteMessage("ERROR UpdatePoleData(Transaction) 01 : {0} \n", ex1.Message);
            //        aTransaction.Rollback();
            //        aConnection.Close();
            //        return false;

            //    }

            //}
            //catch (System.Exception ex)
            //{
            //    ed.WriteMessage("ERROR UpdatePoleData 01 : {0} \n", ex.Message);
            //    aConnection.Close();
            //    return false;
            //}

            //aTransaction.Commit();
            //foreach (string gu in ConsolCodeForDel)
            //{
            //    if (!Atend.Global.Acad.DrawEquips.AcDrawConsol.DeleteConsol(PoleObjectId, gu))
            //        throw new System.Exception("ACDrawConsol.DeleteConsol Failed");
            //}

            //Atend.Global.Acad.DrawEquips.AcDrawConsol.insertConsol(PoleObjectId, AddConsol);
            //aConnection.Close();
            return true;
        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        public void DrawPolePolygonTip(Point3d CentrePoint, double Angle)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            //bool conti = true, PoleSaved = false;
            ObjectId NewPoleObjectId = ObjectId.Null;
            ObjectIdCollection NewConsolObjectIds = new ObjectIdCollection();
            //ObjectIdCollection ids = new ObjectIdCollection();
            int i = 0;

            double MyScale = Atend.Base.Design.DProductProperties.AccessSelectBySoftwareCode((int)Atend.Control.Enum.ProductType.Pole).Scale;
            double MyCommentScale = Atend.Base.Design.DProductProperties.AccessSelectBySoftwareCode((int)Atend.Control.Enum.ProductType.Pole).CommentScale;
            double MyCommentScaleConsol = Atend.Base.Design.DProductProperties.AccessSelectBySoftwareCode((int)Atend.Control.Enum.ProductType.Consol).CommentScale;


            using (DocumentLock docLock = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.LockDocument())
            {


                DrawPolePolygonJig polePolygon = new DrawPolePolygonJig(eConsols.Count, MyScale);
                List<Entity> entities = polePolygon.GetDemo(CentrePoint, (Math.PI * Angle) / 180);

                #region Save data here
                if (SavePoleDataTip())
                {
                    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
                    foreach (Entity ent in entities)
                    {
                        object productType = null;
                        Entity newEntity = ent;
                        Atend.Global.Acad.AcadJigs.MyPolyLine myPoly = ent as Atend.Global.Acad.AcadJigs.MyPolyLine;
                        if (myPoly.AdditionalDictionary.ContainsKey("ProductType"))
                        {
                            myPoly.AdditionalDictionary.TryGetValue("ProductType", out productType);
                        }
                        else
                        {
                            return;
                        }
                        //ed.WriteMessage("ProductType= " + productType.ToString() + "\n");

                        if (Convert.ToInt32(productType) == (int)Atend.Control.Enum.ProductType.Pole)
                        {

                            NewPoleObjectId = Atend.Global.Acad.UAcad.DrawEntityOnScreen(ent, Atend.Control.Enum.AutoCadLayerName.GENERAL.ToString());

                            Atend.Base.Acad.AT_INFO at_info = new Atend.Base.Acad.AT_INFO();
                            at_info.ParentCode = "";
                            at_info.NodeCode = dNode.Code.ToString();
                            at_info.NodeType = (int)Atend.Control.Enum.ProductType.PoleTip;
                            at_info.ProductCode = dNode.ProductCode;
                            at_info.SelectedObjectId = ent.ObjectId;
                            at_info.Insert();

                            Point3d CommentPosition = Atend.Global.Acad.Global.PoleCommentPosition(Atend.Global.Acad.UAcad.GetEntityByObjectID(ent.ObjectId));
                            if (CommentPosition != Point3d.Origin)
                            {

                                ObjectId TextOi = Atend.Global.Acad.UAcad.DrawEntityOnScreen(Atend.Global.Acad.UAcad.WriteNote(PolePackage.Number, CommentPosition, MyCommentScale), Atend.Control.Enum.AutoCadLayerName.GENERAL.ToString());


                                Atend.Base.Acad.AT_INFO textInfo = new Atend.Base.Acad.AT_INFO(TextOi);
                                textInfo.ParentCode = at_info.NodeCode;
                                textInfo.NodeType = (int)Atend.Control.Enum.ProductType.Comment;
                                textInfo.NodeCode = "";
                                textInfo.ProductCode = 0;
                                textInfo.Insert();


                                Atend.Base.Acad.AT_SUB polesub = new Atend.Base.Acad.AT_SUB(NewPoleObjectId);
                                polesub.SubIdCollection.Add(TextOi);
                                polesub.Insert();

                                Atend.Base.Acad.AT_SUB textSub = new Atend.Base.Acad.AT_SUB(TextOi);
                                textSub.SubIdCollection.Add(NewPoleObjectId);
                                textSub.Insert();
                            }

                        }
                        else if (Convert.ToInt32(productType) == (int)Atend.Control.Enum.ProductType.Consol)
                        {
                            // add extention data
                            //ed.WriteMessage("The Entity Is Consol\n");
                            //Atend.Base.Design.DPackage package = dPackages[i];
                            bool IsWeek = false;
                            Atend.Base.Equipment.EConsol eConsol = eConsols[i];  //Atend.Base.Equipment.EConsol.SelectByCodeForDesign(package.ProductCode);


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



                            Atend.Base.Design.DConsol consol = new Atend.Base.Design.DConsol();
                            //ed.WriteMessage("ConsolCount= " + dConsolCode.Count.ToString() + "\n");
                            consol.Code = dPackages[i].Code;
                            consol.LoadCode = 0;
                            consol.ProductCode = eConsol.Code;
                            consol.ParentCode = dNode.Code;

                            //consol.DesignCode = Atend.Control.Common.SelectedDesignCode;
                            //ed.WriteMessage("ConsolCode= " + consol.Code.ToString() + "\n");
                            //ed.WriteMessage("ParentCode= " + dNode.Code.ToString() + "\n");
                            if (consol.AccessInsert())
                            {
                                ObjectId NewConsolObjectID = Atend.Global.Acad.UAcad.DrawEntityOnScreen(ent, LayerName);
                                NewConsolObjectIds.Add(NewConsolObjectID);

                                Atend.Base.Acad.AT_INFO at_info = new Atend.Base.Acad.AT_INFO();

                                at_info.ParentCode = dNode.Code.ToString();
                                at_info.NodeCode = dPackages[i].Code.ToString();
                                at_info.NodeType = Convert.ToInt32(productType);
                                at_info.ProductCode = dPackages[i].ProductCode;
                                at_info.SelectedObjectId = ent.ObjectId;
                                at_info.Insert();

                                ObjectId TextOi = Atend.Global.Acad.UAcad.DrawEntityOnScreen(Atend.Global.Acad.UAcad.WriteNote(dPackages[i].Number, ((Polyline)Atend.Global.Acad.UAcad.GetEntityByObjectID(ent.ObjectId)).GetPoint3dAt(0), MyCommentScaleConsol), Atend.Control.Enum.AutoCadLayerName.GENERAL.ToString());

                                Atend.Base.Acad.AT_INFO textInfo = new Atend.Base.Acad.AT_INFO(TextOi);
                                textInfo.ParentCode = at_info.NodeCode;
                                textInfo.NodeType = (int)Atend.Control.Enum.ProductType.Comment;
                                textInfo.NodeCode = "";
                                textInfo.ProductCode = 0;
                                textInfo.Insert();

                                Atend.Base.Acad.AT_SUB at_sub = new Atend.Base.Acad.AT_SUB();
                                at_sub.SelectedObjectId = NewConsolObjectID;
                                at_sub.SubIdCollection.Add(NewPoleObjectId);
                                at_sub.SubIdCollection.Add(TextOi);
                                at_sub.Insert();

                                Atend.Base.Acad.AT_SUB textSub = new Atend.Base.Acad.AT_SUB(TextOi);
                                textSub.SubIdCollection.Add(NewConsolObjectID);
                                textSub.Insert();



                            }

                            i++;
                            // ed.WriteMessage("Extension was done \n");

                        }//End of DRaw consol

                    }// Draw Finished

                    //insert consols as a sub for pole
                    foreach (ObjectId obji in NewConsolObjectIds)
                    {
                        Atend.Base.Acad.AT_SUB.AddToAT_SUB(obji, NewPoleObjectId);
                    }

                }
                #endregion

            }

        }

        public void DrawPolePolygonTip()
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            bool conti = true, PoleSaved = false;
            ObjectId NewPoleObjectId = ObjectId.Null;
            ObjectIdCollection NewConsolObjectIds = new ObjectIdCollection();
            //ObjectIdCollection ids = new ObjectIdCollection();
            int i = 0;

            double MyScale = Atend.Base.Design.DProductProperties.AccessSelectBySoftwareCode((int)Atend.Control.Enum.ProductType.Pole).Scale;
            double MyCommentScale = Atend.Base.Design.DProductProperties.AccessSelectBySoftwareCode((int)Atend.Control.Enum.ProductType.Pole).CommentScale;
            double MyCommentScaleConsol = Atend.Base.Design.DProductProperties.AccessSelectBySoftwareCode((int)Atend.Control.Enum.ProductType.Consol).CommentScale;


            using (DocumentLock docLock = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.LockDocument())
            {


                DrawPolePolygonJig polePolygon = new DrawPolePolygonJig(eConsols.Count, MyScale);

                while (conti)
                {

                    PromptResult pr = ed.Drag(polePolygon);

                    if (pr.Status == PromptStatus.OK)
                    {

                        conti = false;

                        List<Entity> entities = polePolygon.GetEntities();

                        #region Save data here
                        if (SavePoleDataTip())
                        {
                            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
                            foreach (Entity ent in entities)
                            {
                                object productType = null;
                                Entity newEntity = ent;
                                Atend.Global.Acad.AcadJigs.MyPolyLine myPoly = ent as Atend.Global.Acad.AcadJigs.MyPolyLine;
                                if (myPoly.AdditionalDictionary.ContainsKey("ProductType"))
                                {
                                    myPoly.AdditionalDictionary.TryGetValue("ProductType", out productType);
                                }
                                else
                                {
                                    return;
                                }
                                //ed.WriteMessage("ProductType= " + productType.ToString() + "\n");

                                if (Convert.ToInt32(productType) == (int)Atend.Control.Enum.ProductType.Pole)
                                {

                                    NewPoleObjectId = Atend.Global.Acad.UAcad.DrawEntityOnScreen(ent, Atend.Control.Enum.AutoCadLayerName.GENERAL.ToString());

                                    Atend.Base.Acad.AT_INFO at_info = new Atend.Base.Acad.AT_INFO();
                                    at_info.ParentCode = "";
                                    at_info.NodeCode = dNode.Code.ToString();
                                    at_info.NodeType = (int)Atend.Control.Enum.ProductType.PoleTip;
                                    at_info.ProductCode = dNode.ProductCode;
                                    at_info.SelectedObjectId = ent.ObjectId;
                                    Polyline AngleLine = ent as Polyline;
                                    if (AngleLine != null)
                                    {
                                        Line myLine = new Line(AngleLine.GetPoint3dAt(0), AngleLine.GetPoint3dAt(1));
                                        //ed.WriteMessage("~~~ angle :{0}\n", (180 * myLine.Angle) / Math.PI);
                                        at_info.Angle = (180 * myLine.Angle) / Math.PI;
                                    }
                                    else
                                    {
                                        at_info.Angle = 0;
                                    }

                                    at_info.Insert();

                                    Point3d CommentPosition = Atend.Global.Acad.Global.PoleCommentPosition(Atend.Global.Acad.UAcad.GetEntityByObjectID(ent.ObjectId));
                                    if (CommentPosition != Point3d.Origin)
                                    {

                                        ObjectId TextOi = Atend.Global.Acad.UAcad.DrawEntityOnScreen(Atend.Global.Acad.UAcad.WriteNote(PolePackage.Number, CommentPosition, MyCommentScale), Atend.Control.Enum.AutoCadLayerName.GENERAL.ToString());


                                        Atend.Base.Acad.AT_INFO textInfo = new Atend.Base.Acad.AT_INFO(TextOi);
                                        textInfo.ParentCode = at_info.NodeCode;
                                        textInfo.NodeType = (int)Atend.Control.Enum.ProductType.Comment;
                                        textInfo.NodeCode = "";
                                        textInfo.ProductCode = 0;
                                        textInfo.Insert();


                                        Atend.Base.Acad.AT_SUB polesub = new Atend.Base.Acad.AT_SUB(NewPoleObjectId);
                                        polesub.SubIdCollection.Add(TextOi);
                                        polesub.Insert();

                                        Atend.Base.Acad.AT_SUB textSub = new Atend.Base.Acad.AT_SUB(TextOi);
                                        textSub.SubIdCollection.Add(NewPoleObjectId);
                                        textSub.Insert();
                                    }
                                }
                                else if (Convert.ToInt32(productType) == (int)Atend.Control.Enum.ProductType.Consol)
                                {
                                    // add extention data
                                    //ed.WriteMessage("The Entity Is Consol\n");
                                    //Atend.Base.Design.DPackage package = dPackages[i];
                                    bool IsWeek = false;
                                    Atend.Base.Equipment.EConsol eConsol = eConsols[i];  //Atend.Base.Equipment.EConsol.SelectByCodeForDesign(package.ProductCode);


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



                                    Atend.Base.Design.DConsol consol = new Atend.Base.Design.DConsol();
                                    //ed.WriteMessage("ConsolCount= " + dConsolCode.Count.ToString() + "\n");
                                    consol.Code = dPackages[i].Code;
                                    consol.LoadCode = 0;
                                    consol.ProductCode = eConsol.Code;
                                    consol.ParentCode = dNode.Code;

                                    //consol.DesignCode = Atend.Control.Common.SelectedDesignCode;
                                    //ed.WriteMessage("ConsolCode= " + consol.Code.ToString() + "\n");
                                    //ed.WriteMessage("ParentCode= " + dNode.Code.ToString() + "\n");
                                    if (consol.AccessInsert())
                                    {
                                        ObjectId NewConsolObjectID = Atend.Global.Acad.UAcad.DrawEntityOnScreen(ent, LayerName);
                                        NewConsolObjectIds.Add(NewConsolObjectID);

                                        Atend.Base.Acad.AT_INFO at_info = new Atend.Base.Acad.AT_INFO();

                                        at_info.ParentCode = dNode.Code.ToString();
                                        at_info.NodeCode = dPackages[i].Code.ToString();
                                        at_info.NodeType = Convert.ToInt32(productType);
                                        at_info.ProductCode = dPackages[i].ProductCode;
                                        at_info.SelectedObjectId = ent.ObjectId;
                                        at_info.Insert();

                                        ObjectId TextOi = Atend.Global.Acad.UAcad.DrawEntityOnScreen(Atend.Global.Acad.UAcad.WriteNote(dPackages[i].Number, ((Polyline)Atend.Global.Acad.UAcad.GetEntityByObjectID(ent.ObjectId)).GetPoint3dAt(0), MyCommentScaleConsol), Atend.Control.Enum.AutoCadLayerName.GENERAL.ToString());

                                        Atend.Base.Acad.AT_INFO textInfo = new Atend.Base.Acad.AT_INFO(TextOi);
                                        textInfo.ParentCode = at_info.NodeCode;
                                        textInfo.NodeType = (int)Atend.Control.Enum.ProductType.Comment;
                                        textInfo.NodeCode = "";
                                        textInfo.ProductCode = 0;
                                        textInfo.Insert();

                                        Atend.Base.Acad.AT_SUB at_sub = new Atend.Base.Acad.AT_SUB();
                                        at_sub.SelectedObjectId = NewConsolObjectID;
                                        at_sub.SubIdCollection.Add(NewPoleObjectId);
                                        at_sub.SubIdCollection.Add(TextOi);
                                        at_sub.Insert();

                                        Atend.Base.Acad.AT_SUB textSub = new Atend.Base.Acad.AT_SUB(TextOi);
                                        textSub.SubIdCollection.Add(NewConsolObjectID);
                                        textSub.Insert();



                                    }

                                    i++;
                                    // ed.WriteMessage("Extension was done \n");

                                }//End of DRaw consol

                            }// Draw Finished

                            //insert consols as a sub for pole
                            foreach (ObjectId obji in NewConsolObjectIds)
                            {
                                Atend.Base.Acad.AT_SUB.AddToAT_SUB(obji, NewPoleObjectId);
                            }

                            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
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

        private bool SavePoleDataTip()
        {

            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;

            //ed.WriteMessage("~~~~~~~~~~~~~ Start Save Pole Data ~~~~~~~~~~~~~~~~~~\n");
            //ed.WriteMessage("~~{0}~~\n",Atend.Control.ConnectionString.AccessCnString);
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
                        if (eHalterCount != 0)
                        {
                            if (!eHalter.AccessInsert(aTransaction, aConnection, true, true))
                            {
                                throw new System.Exception("Halter.AccessInsert failed");
                            }
                        }

                        if (!ePole.AccessInsert(aTransaction, aConnection, true, true))
                        {
                            throw new System.Exception("ePole.AccessInsert failed");
                        }

                        if (eHalterCount != 0)
                        {
                            ePoleTip.HalterID = eHalter.Code;
                            ePoleTip.HalterCount = eHalterCount;
                        }
                        else
                        {
                            ePoleTip.HalterID = 0;
                            ePoleTip.HalterCount = 0;
                        }
                        ePoleTip.PoleCode = ePole.Code;
                        if (!ePoleTip.AccessInsert(aTransaction, aConnection, true, true))
                        {
                            throw new System.Exception("ePoleTip.AccessInsert failed");
                        }

                        eConsols.Clear();
                        System.Data.DataTable ConsolTable = Atend.Base.Equipment.EProductPackage.AccessSelectByContainerCodeAndType(aTransaction, aConnection, ePoleTip.Code, (int)Atend.Control.Enum.ProductType.PoleTip);
                        foreach (DataRow dr in ConsolTable.Rows)
                        {
                            //ed.WriteMessage("PoleTipCode : {0} type : {1} code:{2} ||| containerPackageCode:{3} \n", dr["Containercode"], dr["Type"], "", dr["ContainerPackagecode"]);
                            if (Convert.ToInt32(dr["TableType"]) == (int)Atend.Control.Enum.ProductType.Consol)
                            {
                                Atend.Base.Equipment.EConsol EC = Atend.Base.Equipment.EConsol.AccessSelectByCode(aTransaction, aConnection, Convert.ToInt32(dr["ProductCode"]));
                                if (EC.Code != -1)
                                {
                                    eConsols.Add(EC);
                                }
                            }
                        }
                        //ed.WriteMessage("CONSOL count ****{0}\n", eConsols.Count);

                    }
                    dNode = new Atend.Base.Design.DNode();
                    dNode.Number = "N001";
                    dNode.ProductCode = ePoleTip.Code;
                    dNode.Height = Height;
                    if (!dNode.AccessInsert(aTransaction, aConnection))
                    {
                        throw new System.Exception("PoleNode.AccessInsert failed");
                    }

                    dPoleInfo.NodeCode = dNode.Code;
                    if (eHalterCount != 0)
                    {
                        dPoleInfo.HalterType = eHalter.Code;
                        dPoleInfo.HalterCount = eHalterCount;
                    }
                    else
                    {
                        dPoleInfo.HalterType = 0;
                        dPoleInfo.HalterCount = 0;
                    }
                    if (!_dPoleInfo.AcessInsert(aTransaction, aConnection))
                    {
                        throw new System.Exception("_dPoleInfo.AcessInsert failed");
                    }

                    PolePackage = new Atend.Base.Design.DPackage();
                    PolePackage.Count = 1;
                    PolePackage.IsExistance = Existance;
                    PolePackage.NodeCode = dNode.Code;
                    //ed.WriteMessage("POLE****{0}\n", ProjectCode);
                    PolePackage.ProjectCode = ProjectCode;
                    PolePackage.ProductCode = ePoleTip.Code;
                    PolePackage.Type = (int)Atend.Control.Enum.ProductType.PoleTip;
                    Atend.Control.Common.Counters.PoleCounter++;
                    //ed.WriteMessage("power:{0} heigth:{1}\n", ePole.Power, ePole.Height);
                    PolePackage.Number = string.Format("P{2}-{0:00}{1:00}", ePole.Height, Math.Round(ePole.Power / 100, 0), Atend.Control.Common.Counters.PoleCounter);
                    if (!PolePackage.AccessInsert(aTransaction, aConnection))
                    {
                        throw new System.Exception("PolePackage.AccessInsert failed");
                    }
                    else
                    {
                        //dPackages.Add(PolePackage);
                        //فقط کنسول ها را در لیست گذاشته ایم
                    }

                    if (eHalterCount != 0)
                    {
                        HalterPackage = new Atend.Base.Design.DPackage();
                        HalterPackage.Count = eHalterCount;
                        HalterPackage.IsExistance = HalterExistance;
                        HalterPackage.NodeCode = Guid.Empty;
                        HalterPackage.ProjectCode = HalterProjectCode;
                        HalterPackage.ProductCode = eHalter.Code;
                        HalterPackage.ParentCode = PolePackage.Code;
                        HalterPackage.Type = (int)Atend.Control.Enum.ProductType.Halter;
                        HalterPackage.Number = "HALTER"; //string.Empty; string.Format("P{2}-{0:00}{1:00}", Math.Round(ePole.Power / 100, 0), Math.Round(ePole.Height, 0), Atend.Control.Common.Counters.PoleCounter);
                        if (!HalterPackage.AccessInsert(aTransaction, aConnection))
                        {
                            throw new System.Exception("HalterPackage.AccessInsert failed");
                        }
                    }


                    int i = 0;
                    foreach (Atend.Base.Equipment.EConsol consol in eConsols)
                    {

                        Atend.Base.Design.DPackage ConsolPackage = new Atend.Base.Design.DPackage();
                        ConsolPackage.Count = Convert.ToInt32(eConsolCount[i]);
                        ConsolPackage.IsExistance = Convert.ToInt32(eConsolExistance[i]);
                        ConsolPackage.ParentCode = PolePackage.Code;
                        ConsolPackage.ProductCode = consol.Code;
                        Atend.Control.Common.Counters.ConsolCounter++;
                        ConsolPackage.Number = string.Format("Ins{0:0000}", Atend.Control.Common.Counters.ConsolCounter);
                        ConsolPackage.Type = (int)Atend.Control.Enum.ProductType.Consol;
                        ConsolPackage.ProjectCode = Convert.ToInt32(eConsolProjectCode[i]);
                        if (!ConsolPackage.AccessInsert(aTransaction, aConnection))
                        {
                            throw new System.Exception("ConsolPackage.AccessInsert failed");
                        }
                        else
                        {
                            dPackages.Add(ConsolPackage);
                        }
                        i++;

                    }

                }
                catch (System.Exception ex1)
                {
                    ed.WriteMessage("ERROR SavePoleData: 02 {0} \n", ex1.Message);
                    aTransaction.Rollback();
                    aConnection.Close();
                    return false;

                }

            }
            catch (System.Exception ex)
            {
                ed.WriteMessage("ERROR SavePoleData: 01 {0} \n", ex.Message);
                aConnection.Close();
                return false;
            }
            aTransaction.Commit();
            aConnection.Close();

            #region Not to Get Green

            Atend.Base.Acad.AcadGlobal.PolygonPoleData.UseAccess = true;
            //UseAccess = true;
            for (int i = 0; i < Atend.Base.Acad.AcadGlobal.PoleData.eConsolUseAccess.Count; i++)
            {
                Atend.Base.Acad.AcadGlobal.PoleData.eConsolUseAccess[i] = true;
            }

            #endregion


            //ed.WriteMessage("~~~~~~~~~~~~~ End Save Pole Data ~~~~~~~~~~~~~~~~~~\n");

            return true;

        }

        //public bool UpdatePoleDataTip(ObjectId PoleObjectId, ArrayList ConsolCodeForDel, Guid PoleCode)
        //{
        //    Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
        //    OleDbTransaction aTransaction;
        //    OleDbConnection aConnection = new OleDbConnection(Atend.Control.ConnectionString.AccessCnString);
        //    ArrayList AddConsol = new ArrayList();
        //    try
        //    {
        //        aConnection.Open();
        //        aTransaction = aConnection.BeginTransaction();
        //        try
        //        {
        //            PolePackage = Atend.Base.Design.DPackage.AccessSelectByNodeCode(PoleCode);
        //            if (!UseAccess)
        //            {
        //                //ed.WriteMessage("Insert EPole\n");
        //                if (!ePole.AccessInsert(aTransaction, aConnection, true))
        //                    throw new System.Exception("ePole.AccessInsert failed");

        //                ePoleTip.PoleCode = ePole.Code;
        //                if (!ePoleTip.AccessInsert(aTransaction, aConnection))
        //                    throw new System.Exception("ePoleTip.AccessInsert failed");


        //            }
        //            //ed.WriteMessage("PoleTip.Code={0}\n", ePoleTip.Code);
        //            PolePackage.IsExistance = Existance;
        //            PolePackage.ProductCode = ePoleTip.Code;
        //            //ed.WriteMessage("**ProductCode={0}\n", PolePackage.ProductCode);
        //            PolePackage.ProjectCode = ProjectCode;
        //            PolePackage.Number = "";
        //            if (!PolePackage.AccessUpdate(aTransaction, aConnection))
        //            {
        //                throw new System.Exception("PolePackage.AccessInsert(tr) failed");
        //            }

        //            //foreach (Atend.Base.Design.DPackage pack in beforDPackage)
        //            //{
        //            //    ed.WriteMessage("Update Pack\n");
        //            //    if (!pack.AccessUpdate(aTransaction, aConnection))
        //            //    {
        //            //        throw new System.Exception("Update dPackage Failed");
        //            //    }
        //            //}
        //            foreach (string gu in ConsolCodeForDel)
        //            {
        //                //ed.WriteMessage("DeleteConsol\n");
        //                if (!Atend.Base.Design.DPackage.AccessDelete(new Guid(gu), aTransaction, aConnection))
        //                {
        //                    throw new System.Exception("ConsolPackage.AccessDelete failed");

        //                }
        //            }



        //            int i = 0;
        //            foreach (Atend.Base.Equipment.EConsol Consols in eConsols)
        //            {
        //                //ed.WriteMessage("UseAcces={0}\n", eConsolUseAccess[i].ToString());
        //                if (!Convert.ToBoolean(eConsolUseAccess[i]))
        //                {
        //                    //ed.WriteMessage("Consol.AccessInsert\n");
        //                    if (!Consols.AccessInsert(aTransaction, aConnection, true))
        //                    {
        //                        throw new System.Exception("eConsols.AccessInsert failed");
        //                    }
        //                }
        //                //ed.WriteMessage("Consol.Code={0}\n", Consols.Code);
        //                Atend.Base.Acad.AT_INFO poleInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(PoleObjectId);
        //                Atend.Base.Design.DPackage dp = Atend.Base.Design.DPackage.AccessSelectByNodeCode(new Guid(poleInfo.NodeCode));

        //                Atend.Base.Design.DPackage ConsolPackage = new Atend.Base.Design.DPackage();
        //                ConsolPackage.Count = Convert.ToInt32(eConsolCount[i]);
        //                ConsolPackage.IsExistance = Convert.ToInt32(eConsolExistance[i]);
        //                ConsolPackage.ParentCode = dp.Code;
        //                ConsolPackage.ProductCode = Consols.Code;
        //                Atend.Control.Common.Counters.ConsolCounter++;
        //                ConsolPackage.Number = string.Format("Ins{0:0000}", Atend.Control.Common.Counters.ConsolCounter);
        //                ConsolPackage.Type = (int)Atend.Control.Enum.ProductType.Consol;
        //                ConsolPackage.ProjectCode = Convert.ToInt32(eConsolProjectCode[i]);
        //                if (!ConsolPackage.AccessInsert(aTransaction, aConnection))
        //                {
        //                    throw new System.Exception("ConsolPackage.AccessInsert failed");
        //                }
        //                //ed.WriteMessage("ConsolPackage.Code={0}\n", ConsolPackage.Code);
        //                AddConsol.Add(ConsolPackage.Code);

        //                i++;
        //            }

        //            Atend.Base.Acad.AT_INFO poleInfo1 = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(PoleObjectId);
        //            Atend.Base.Design.DNode Node = Atend.Base.Design.DNode.AccessSelectByCode(new Guid(poleInfo1.NodeCode));
        //            dNode.Height = Height;
        //            if (!dNode.AccessUpdate(aTransaction, aConnection))
        //            {
        //                throw new System.Exception("dNODE.AccessUpdate Failed");
        //            }

        //            if (PoleObjectId != ObjectId.Null)
        //            {
        //                Atend.Base.Acad.AT_INFO atinfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(PoleObjectId);
        //                atinfo.ProductCode = ePole.Code;
        //                atinfo.Insert();
        //            }




        //        }
        //        catch (System.Exception ex1)
        //        {
        //            ed.WriteMessage("ERROR UpdatePoleData(Transaction) 01 : {0} \n", ex1.Message);
        //            aTransaction.Rollback();
        //            aConnection.Close();
        //            return false;

        //        }

        //    }
        //    catch (System.Exception ex)
        //    {
        //        ed.WriteMessage("ERROR UpdatePoleData 01 : {0} \n", ex.Message);
        //        aConnection.Close();
        //        return false;
        //    }

        //    aTransaction.Commit();
        //    foreach (string gu in ConsolCodeForDel)
        //    {
        //        if (!Atend.Global.Acad.DrawEquips.AcDrawConsol.DeleteConsol(PoleObjectId, gu))
        //            throw new System.Exception("ACDrawConsol.DeleteConsol Failed");
        //    }

        //    Atend.Global.Acad.DrawEquips.AcDrawConsol.insertConsol(PoleObjectId, AddConsol);
        //    aConnection.Close();
        //    return true;
        //}

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        public static List<Entity> LegendEntity(Point3d CenterPoint, string Text)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            double bassX = CenterPoint.X;
            double bassY = CenterPoint.Y;
            List<Entity> Entities = new List<Entity>();

            Polyline p = new Polyline();
            p.AddVertexAt(p.NumberOfVertices, new Point2d(bassX - 6, bassY), 0, 0, 0);
            p.AddVertexAt(p.NumberOfVertices, new Point2d(bassX - 3, bassY + 3), 0, 0, 0);
            p.AddVertexAt(p.NumberOfVertices, new Point2d(bassX + 3, bassY + 3), 0, 0, 0);
            p.AddVertexAt(p.NumberOfVertices, new Point2d(bassX + 6, bassY), 0, 0, 0);
            p.AddVertexAt(p.NumberOfVertices, new Point2d(bassX + 3, bassY - 3), 0, 0, 0);
            p.AddVertexAt(p.NumberOfVertices, new Point2d(bassX - 3, bassY - 3), 0, 0, 0);
            p.AddVertexAt(p.NumberOfVertices, new Point2d(bassX - 6, bassY), 0, 0, 0);
            Entities.Add(p);

            MText mtext = new MText();
            mtext.Location = new Point3d((bassX - 8), bassY, 0);
            mtext.Contents = Text;
            mtext.Height = 0.5;
            //mtext.Width = 8;

            //TextStyle ts = new TextStyle("Tahoma", "Tahoma", 10, 0, 0, 0, 0, 0, 0, 0, 0, "MyStyle");
            //mtext.tex
            Entities.Add(mtext);


            //ed.WriteMessage("entied add \n");
            return Entities;
        }

    }
}