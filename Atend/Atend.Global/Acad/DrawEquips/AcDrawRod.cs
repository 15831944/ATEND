using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.GraphicsInterface;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.Geometry;
//using Autodesk.AutoCAD.Interop;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.Windows;
using Autodesk.AutoCAD.Colors;
using System.Data.OleDb;

namespace Atend.Global.Acad.DrawEquips
{



    public class AcDrawRod
    {
        //~~~~~~~~~~~~~~~~~~~~~~~~~~Properties~~~~~~~~~~~~~~~~~~~~~~~~`
        bool _useAccess;
        public bool UseAccess
        {
            get { return _useAccess; }
            set { _useAccess = value; }
        }

        int _existance;
        public int Existance
        {
            get { return _existance; }
            set { _existance = value; }
        }

        int _projectCode;
        public int ProjectCode
        {
            get { return _projectCode; }
            set { _projectCode = value; }
        }

        Atend.Base.Equipment.ERod _eRod;

        public Atend.Base.Equipment.ERod ERod
        {
            get { return _eRod; }
            set { _eRod = value; }
        }

        private ObjectId selectedObjectId;
        public ObjectId SelectedObjectId
        {
            get { return selectedObjectId; }
            set { selectedObjectId = value; }
        }

        Atend.Base.Design.DPackage RodPack;
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~CLass~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public class DrawRodJig : DrawJig
        {

            public Point3d CenterPoint01 = Point3d.Origin, CenterPoint02 = Point3d.Origin;
            List<Entity> Entitied = new List<Entity>();
            //Point3dCollection p3c;
            Entity ContainerEntity = null;
            public bool PartOneIsActive = true;
            Matrix3d m_ucs;
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            double MyScale = 1;

            public DrawRodJig(Entity Container, double Scale)
            {
                //p3c = ConvertEntityToPoint3dCollection(Container);
                MyScale = Scale;
                ContainerEntity = Container;

                CenterPoint02 = new Point3d(CenterPoint01.X + 40, CenterPoint01.Y, CenterPoint01.Z);
                Entitied.Add(DrawConsol(CenterPoint01, 10, 10));
                Entitied.Add(DrawLine(CenterPoint01, CenterPoint02));
                Entitied.Add(DrawRod(CenterPoint02, 6, 6));
                Entitied.Add(DrawTriangle(CenterPoint02));

                m_ucs = ed.CurrentUserCoordinateSystem;

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


                Atend.Global.Acad.AcadJigs.SaveExtensionData(pLine, (int)Atend.Control.Enum.ProductType.ConsolElse);
                return pLine;

            }

            private Entity DrawLine(Point3d StartPoint, Point3d EndPoint)
            {

                Atend.Global.Acad.AcadJigs.MyLine l = new Atend.Global.Acad.AcadJigs.MyLine();
                l.StartPoint = StartPoint;
                l.EndPoint = EndPoint;

                Atend.Global.Acad.AcadJigs.SaveExtensionData(l, (int)Atend.Control.Enum.ProductType.Connection);
                return l;

            }

            private Entity DrawTriangle(Point3d BasePoint)
            {

                double BaseX = BasePoint.X;
                double BaseY = BasePoint.Y;

                Atend.Global.Acad.AcadJigs.MyPolyLine _MyPolyline = new Atend.Global.Acad.AcadJigs.MyPolyLine();
                _MyPolyline.AddVertexAt(_MyPolyline.NumberOfVertices, new Point2d(BaseX - 3, BaseY + 3), 0, 0, 0);
                _MyPolyline.AddVertexAt(_MyPolyline.NumberOfVertices, new Point2d(BaseX + 3, BaseY), 0, 0, 0);
                _MyPolyline.AddVertexAt(_MyPolyline.NumberOfVertices, new Point2d(BaseX - 3, BaseY - 3), 0, 0, 0);
                _MyPolyline.AddVertexAt(_MyPolyline.NumberOfVertices, new Point2d(BaseX - 3, BaseY + 3), 0, 0, 0);
                _MyPolyline.Closed = true;


                Atend.Global.Acad.AcadJigs.SaveExtensionData(_MyPolyline, (int)Atend.Control.Enum.ProductType.Else);
                return _MyPolyline;

            }

            private Entity DrawRod(Point3d BasePoint, double Width, double Height)
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


                Atend.Global.Acad.AcadJigs.SaveExtensionData(pLine, (int)Atend.Control.Enum.ProductType.Rod);
                return pLine;

            }

            protected override SamplerStatus Sampler(JigPrompts prompts)
            {
                JigPromptPointOptions ppo = new JigPromptPointOptions();
                PromptPointResult pr;

                if (PartOneIsActive)
                {

                    ppo.Message = "\nPart One Position : ";
                    pr = prompts.AcquirePoint(ppo);
                    if (pr.Status == PromptStatus.OK)
                    {

                        if (pr.Value == CenterPoint01)
                        {
                            return SamplerStatus.NoChange;
                        }
                        else
                        {
                            if (Atend.Global.Acad.UAcad.IsInsideCurve((Curve)ContainerEntity, pr.Value) == true)
                            {
                                CenterPoint01 = pr.Value;
                                return SamplerStatus.OK;
                            }
                            else
                            {
                                return SamplerStatus.NoChange;
                            }
                        }
                    }
                    else
                    {
                        return SamplerStatus.Cancel;
                    }


                }
                else
                {
                    ppo.Message = "\nPart Two Position : ";
                    pr = prompts.AcquirePoint(ppo);
                    if (pr.Status == PromptStatus.OK)
                    {
                        if (pr.Value == CenterPoint02)
                        {
                            return SamplerStatus.NoChange;
                        }
                        else
                        {
                            CenterPoint02 = pr.Value;
                            return SamplerStatus.OK;
                        }
                    }
                    else
                    {
                        return SamplerStatus.Cancel;
                    }

                }
            }

            protected override bool WorldDraw(Autodesk.AutoCAD.GraphicsInterface.WorldDraw draw)
            {
                //throw new System.Exception("The method or operation is not implemented.");

                Entitied.Clear();
                Entity enti = null;

                if (PartOneIsActive)
                {
                    CenterPoint02 = new Point3d(CenterPoint01.X + 40, CenterPoint01.Y, CenterPoint01.Z);
                    Matrix3d trans1 = Matrix3d.Scaling(MyScale, new Point3d(CenterPoint01.X, CenterPoint01.Y, 0));

                    CenterPoint02 = CenterPoint02.ScaleBy(MyScale, CenterPoint01);
                    Matrix3d trans2 = Matrix3d.Scaling(MyScale, new Point3d(CenterPoint02.X, CenterPoint02.Y, 0));

                    enti = DrawConsol(CenterPoint01, 10, 10);
                    enti.TransformBy(trans1);
                    Entitied.Add(enti);

                    Entitied.Add(DrawLine(CenterPoint01, CenterPoint02));

                    enti = DrawRod(CenterPoint02, 6, 6);
                    enti.TransformBy(trans2);
                    Entitied.Add(enti);

                    enti = DrawTriangle(CenterPoint02);
                    enti.TransformBy(trans2);
                    Entitied.Add(enti);

                }
                else
                {
                    Line l = new Line(CenterPoint01, CenterPoint02);
                    double newAngle = l.Angle;
                    Matrix3d trans1 = Matrix3d.Scaling(MyScale, new Point3d(CenterPoint01.X, CenterPoint01.Y, 0));
                    Matrix3d trans2 = Matrix3d.Scaling(MyScale, new Point3d(CenterPoint02.X, CenterPoint02.Y, 0));
                    Matrix3d trans3 = Matrix3d.Rotation(newAngle, ed.CurrentUserCoordinateSystem.CoordinateSystem3d.Zaxis, CenterPoint02);
                    //NewAngle - BaseAngle

                    enti = DrawConsol(CenterPoint01, 10, 10);
                    enti.TransformBy(trans1);
                    Entitied.Add(enti);

                    Entitied.Add(DrawLine(CenterPoint01, CenterPoint02));

                    enti = DrawRod(CenterPoint02, 6, 6);
                    enti.TransformBy(trans2);
                    enti.TransformBy(trans3);
                    Entitied.Add(enti);

                    enti = DrawTriangle(CenterPoint02);
                    enti.TransformBy(trans2);
                    enti.TransformBy(trans3);
                    Entitied.Add(enti);

                }

                ////////~~~~~~~~ SCALE ~~~~~~~~~~

                //////Matrix3d trans1 = Matrix3d.Scaling(MyScale, new Point3d(CenterPoint01.X, CenterPoint01.Y, 0));
                //////foreach (Entity en in Entitied)
                //////{
                //////    en.TransformBy(trans1);
                //////}

                ////////~~~~~~~~~~~~~~~~~~~~~~~~~


                foreach (Entity ent in Entitied)
                {
                    draw.Geometry.Draw(ent);
                }


                return true;
            }

            public List<Entity> GetEntities()
            {
                return Entitied;
            }

        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~Method~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        public AcDrawRod()
        {
            RodPack = new Atend.Base.Design.DPackage();
        }

        public void DrawRod()
        {

            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            bool conti = true;
            ObjectIdCollection NewDrawnCollection = new ObjectIdCollection();
            //ObjectId ConsolElseOI = ObjectId.Null, ConnectionOI = ObjectId.Null, RodOI = ObjectId.Null, RodTriangeOI = ObjectId.Null;

            double MyScale = Atend.Base.Design.DProductProperties.AccessSelectBySoftwareCode((int)Atend.Control.Enum.ProductType.Rod).Scale;
            double MyCommentScale = Atend.Base.Design.DProductProperties.AccessSelectBySoftwareCode((int)Atend.Control.Enum.ProductType.Rod).CommentScale;

            PromptEntityOptions peo = new PromptEntityOptions("\nSelect Container :");
            PromptEntityResult per = ed.GetEntity(peo);
            if (per.Status == PromptStatus.OK)
            {
                DrawRodJig drawRod;
                Atend.Base.Acad.AT_INFO at_info = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(per.ObjectId);
                if (at_info.ParentCode != "NONE" && (at_info.NodeType == (int)Atend.Control.Enum.ProductType.Pole || at_info.NodeType == (int)Atend.Control.Enum.ProductType.PoleTip))
                {
                    drawRod = new DrawRodJig(Atend.Global.Acad.UAcad.GetEntityByObjectID(per.ObjectId), MyScale);
                    while (conti)
                    {

                        PromptResult pr = ed.Drag(drawRod);

                        if (pr.Status == PromptStatus.OK && drawRod.PartOneIsActive)
                        {

                            drawRod.PartOneIsActive = false;
                            pr = ed.Drag(drawRod);

                            if (pr.Status == PromptStatus.OK && !drawRod.PartOneIsActive)
                            {

                                conti = false;
                                //ed.WriteMessage("1 \n");
                                #region Save Data Here

                                List<Entity> Entities = drawRod.GetEntities();
                                //ed.WriteMessage("2 \n");
                                if (SaveRodData(at_info.NodeCode))
                                {
                                    // ed.WriteMessage("3 \n");

                                    //foreach (Entity ent in Entities)
                                    //{
                                    //    NewDrawnCollection.Add(DrawEntityOnScreen(ent, Atend.Control.Enum.AutoCadLayerName.MID_AIR.ToString()));
                                    //}

                                    //---------------------
                                    ObjectId ConsolElseOI = ObjectId.Null;
                                    foreach (Entity ent in Entities)
                                    {

                                        ObjectId newDrawnoi = Atend.Global.Acad.UAcad.DrawEntityOnScreen(ent, Atend.Control.Enum.AutoCadLayerName.MED_AIR.ToString());
                                        Atend.Global.Acad.AcadJigs.MyPolyLine mPoly = ent as Atend.Global.Acad.AcadJigs.MyPolyLine;
                                        if (mPoly != null)
                                        {
                                            //ed.WriteMessage("POLY LINE FOUND\n");
                                            if (mPoly.AdditionalDictionary.ContainsKey("ProductType"))
                                            {
                                                object ProductType = null;
                                                mPoly.AdditionalDictionary.TryGetValue("ProductType", out ProductType);
                                                if (ProductType != null)
                                                {
                                                    if (Convert.ToInt32(ProductType) == (int)Atend.Control.Enum.ProductType.ConsolElse)
                                                    {
                                                        ConsolElseOI = newDrawnoi;
                                                        Atend.Base.Acad.AT_INFO ConsolElseInfo = new Atend.Base.Acad.AT_INFO(newDrawnoi);
                                                        ConsolElseInfo.ParentCode = at_info.NodeCode;
                                                        ConsolElseInfo.NodeCode = RodPack.Code.ToString();
                                                        ConsolElseInfo.NodeType = Convert.ToInt32(ProductType);
                                                        ConsolElseInfo.ProductCode = 0;
                                                        ConsolElseInfo.Insert();
                                                    }
                                                }
                                            }
                                        }
                                        NewDrawnCollection.Add(newDrawnoi);

                                    }

                                    //NewDrawnCollection.Add(Atend.Global.Acad.UAcad.DrawEntityOnScreen(
                                    //Atend.Global.Acad.Global.WriteNoteMText(
                                    //    Atend.Base.Equipment.ERod.SelectByCode(Atend.Base.Acad.AcadGlobal.dPackageForRod.ProductCode).Comment,
                                    //    new Point3d(drawRod.CenterPoint02.X + 10, drawRod.CenterPoint02.Y, 0)),
                                    //    Atend.Control.Enum.AutoCadLayerName.MID_AIR.ToString()));


                                    if (ConsolElseOI != ObjectId.Null)
                                    {

                                        Atend.Base.Acad.AT_SUB ConsolElseSub = new Atend.Base.Acad.AT_SUB(ConsolElseOI);
                                        foreach (ObjectId oi in NewDrawnCollection)
                                        {
                                            if (oi != ConsolElseOI)
                                            {
                                                //ed.WriteMessage("ConsolElseSubOI:{0}\n", oi);
                                                ConsolElseSub.SubIdCollection.Add(oi);
                                            }
                                        }
                                        ConsolElseSub.SubIdCollection.Add(per.ObjectId);
                                        ConsolElseSub.Insert();
                                    }
                                    //---------------------




                                    foreach (ObjectId oi in NewDrawnCollection)
                                    {
                                        if (oi != ConsolElseOI)
                                        {
                                            Atend.Base.Acad.AT_INFO a = new Atend.Base.Acad.AT_INFO(oi);
                                            a.ParentCode = at_info.NodeCode;
                                            a.NodeCode = RodPack.Code.ToString();
                                            a.NodeType = (int)Atend.Control.Enum.ProductType.Rod;
                                            a.ProductCode = RodPack.ProductCode; ;
                                            a.Insert();
                                        }
                                    }

                                    //for (int i = 1; i <= 4; i++)
                                    //{

                                    //    ed.WriteMessage("4 \n");
                                    //    switch (i)
                                    //    {

                                    //        case 1:
                                    //            ed.WriteMessage("5 \n");
                                    //            #region Draw & Save ConsolElse Here
                                    //            ConsolElseOI = DrawAndSaveConsolElse(Entities);
                                    //            #endregion
                                    //            break;
                                    //        case 2:
                                    //            ed.WriteMessage("6 \n");
                                    //            #region Draw & Save Connection Here
                                    //            ConnectionOI = DrawAndSaveConnection(Entities);
                                    //            #endregion
                                    //            break;
                                    //        case 3:
                                    //            ed.WriteMessage("7 \n");
                                    //            #region Draw & Save Rod Here
                                    //            RodOI = DrawAndSaveRod(Entities);
                                    //            #endregion
                                    //            break;
                                    //        case 4:
                                    //            ed.WriteMessage("8 \n");
                                    //            #region Draw & Save RodTriangle Here
                                    //            RodTriangeOI = DrawAndSaveRodTriangle(Entities);
                                    //            #endregion
                                    //            break;
                                    //    }

                                    //}

                                    //save additional data here

                                    //ed.WriteMessage("9 \n");
                                    //Atend.Base.Acad.AT_INFO ConsolElseInfo = new Atend.Base.Acad.AT_INFO(ConsolElseOI);
                                    //ConsolElseInfo.ParentCode = at_info.NodeCode;
                                    //ConsolElseInfo.NodeCode = Atend.Base.Acad.AcadGlobal.dPackageForRod.Code.ToString();
                                    //ConsolElseInfo.NodeType = (int)Atend.Control.Enum.ProductType.Rod;
                                    //ConsolElseInfo.ProductCode = Atend.Base.Acad.AcadGlobal.dPackageForRod.ProductCode;
                                    //ConsolElseInfo.Insert();


                                    //ed.WriteMessage("10 \n");
                                    //Atend.Base.Acad.AT_SUB ConsolElseSub = new Atend.Base.Acad.AT_SUB(ConsolElseOI);
                                    //ConsolElseSub.SubIdCollection.Add(ConnectionOI);
                                    //ConsolElseSub.Insert();


                                    //``````````````````````````
                                    //ed.WriteMessage("11 \n");
                                    //ConsolElseInfo = new Atend.Base.Acad.AT_INFO(ConnectionOI);
                                    //ConsolElseInfo.ParentCode = at_info.NodeCode;
                                    //ConsolElseInfo.NodeCode = Atend.Base.Acad.AcadGlobal.dPackageForRod.Code.ToString();
                                    //ConsolElseInfo.NodeType = (int)Atend.Control.Enum.ProductType.Rod;
                                    //ConsolElseInfo.ProductCode = Atend.Base.Acad.AcadGlobal.dPackageForRod.ProductCode;
                                    //ConsolElseInfo.Insert();


                                    //ed.WriteMessage("12 \n");
                                    //ConsolElseSub = new Atend.Base.Acad.AT_SUB(ConnectionOI);
                                    //ConsolElseSub.SubIdCollection.Add(ConsolElseOI);
                                    //ConsolElseSub.SubIdCollection.Add(RodOI);
                                    //ConsolElseSub.Insert();

                                    //```````````````````````````````````````````

                                    //ed.WriteMessage("13 \n");
                                    //ConsolElseInfo = new Atend.Base.Acad.AT_INFO(RodOI);
                                    //ConsolElseInfo.ParentCode = at_info.NodeCode;
                                    //ConsolElseInfo.NodeCode = Atend.Base.Acad.AcadGlobal.dPackageForRod.Code.ToString();
                                    //ConsolElseInfo.NodeType = (int)Atend.Control.Enum.ProductType.Rod;
                                    //ConsolElseInfo.ProductCode = Atend.Base.Acad.AcadGlobal.dPackageForRod.ProductCode;
                                    //ConsolElseInfo.Insert();


                                    //ed.WriteMessage("14 \n");
                                    //ConsolElseSub = new Atend.Base.Acad.AT_SUB(RodOI);
                                    //ConsolElseSub.SubIdCollection.Add(ConnectionOI);
                                    //ConsolElseSub.SubIdCollection.Add(RodTriangeOI);
                                    //ConsolElseSub.Insert();

                                    //```````````````````````````````````````````
                                    //ed.WriteMessage("15 \n");
                                    //ConsolElseInfo = new Atend.Base.Acad.AT_INFO(RodTriangeOI);
                                    //ConsolElseInfo.ParentCode = at_info.NodeCode;
                                    //ConsolElseInfo.NodeCode = Atend.Base.Acad.AcadGlobal.dPackageForRod.Code.ToString();
                                    //ConsolElseInfo.NodeType = (int)Atend.Control.Enum.ProductType.Rod;
                                    //ConsolElseInfo.ProductCode = Atend.Base.Acad.AcadGlobal.dPackageForRod.ProductCode;
                                    //ConsolElseInfo.Insert();

                                    //--------------------------
                                    //ed.WriteMessage("Number of Entity : {0} \n", NewDrawnCollection.Count);

                                    ObjectId NewCreatedGroup =
                                    Atend.Global.Acad.Global.MakeGroup(RodPack.Code.ToString(), NewDrawnCollection);


                                    //Atend.Base.Equipment.ERod ERodForComment = Atend.Base.Equipment.ERod.SelectByCode(RodPack.ProductCode);

                                    //ed.WriteMessage("scale:{0} comment:{1}\n",MyCommentScale,ERod.Comment);
                                    ObjectId txtOI = Atend.Global.Acad.UAcad.DrawEntityOnScreen(Atend.Global.Acad.UAcad.WriteNote(ERod.Comment, new Point3d(drawRod.CenterPoint02.X, drawRod.CenterPoint02.Y, 0), MyCommentScale), Atend.Control.Enum.AutoCadLayerName.GENERAL.ToString());
                                    //ed.WriteMessage("text was writen\n");

                                    Atend.Base.Acad.AT_INFO GroupInfo1 = new Atend.Base.Acad.AT_INFO(txtOI);
                                    GroupInfo1.ParentCode = RodPack.Code.ToString();
                                    GroupInfo1.NodeCode = "";
                                    GroupInfo1.NodeType = (int)Atend.Control.Enum.ProductType.Comment;
                                    GroupInfo1.ProductCode = 0;
                                    GroupInfo1.Insert();


                                    //Atend.Base.Acad.AT_INFO GroupInfo = new Atend.Base.Acad.AT_INFO(NewCreatedGroup);
                                    //GroupInfo.ParentCode = at_info.NodeCode;
                                    //GroupInfo.NodeCode = Atend.Base.Acad.AcadGlobal.dPackageForRod.Code.ToString();
                                    //GroupInfo.NodeType = (int)Atend.Control.Enum.ProductType.BankKhazan;
                                    //GroupInfo.ProductCode = Atend.Base.Acad.AcadGlobal.dPackageForKhazanTip.ProductCode;
                                    //GroupInfo.Insert();





                                    //    Atend.Base.Equipment.EKhazanTip khazanTip = Atend.Base.Equipment.EKhazanTip.SelectByCode(
                                    //        Atend.Base.Acad.AcadGlobal.dPackageForKhazanTip.ProductCode);


                                    //    ObjectId txtOI = DrawEntityOnScreen(
                                    //    Atend.Global.Acad.UAcad.WriteNote(khazanTip.Description, drawKhazan.GetCommentPoSition()),
                                    //    Atend.Control.Enum.AutoCadLayerName.GENERAL.ToString());


                                    Atend.Base.Acad.AT_INFO GroupInfo = new Atend.Base.Acad.AT_INFO(NewCreatedGroup);
                                    GroupInfo.ParentCode = at_info.NodeCode;
                                    GroupInfo.NodeCode = RodPack.Code.ToString();
                                    GroupInfo.NodeType = (int)Atend.Control.Enum.ProductType.Rod;
                                    GroupInfo.ProductCode = RodPack.ProductCode;
                                    GroupInfo.Insert();


                                    Atend.Base.Acad.AT_SUB.AddToAT_SUB(NewCreatedGroup, per.ObjectId);
                                    //ed.WriteMessage("PoleOI for KHazan:{0}", per.ObjectId);
                                    Atend.Base.Acad.AT_SUB GroupSub = new Atend.Base.Acad.AT_SUB(NewCreatedGroup);
                                    GroupSub.SubIdCollection.Add(per.ObjectId);
                                    //ed.WriteMessage("TXTOI for KHazan:{0}", txtOI);
                                    GroupSub.SubIdCollection.Add(txtOI);
                                    GroupSub.Insert();


                                    //    Atend.Base.Acad.AT_SUB.AddToAT_SUB(NewCreatedGroup, per.ObjectId);

                                    //    Atend.Base.Acad.AT_SUB GroupSub = new Atend.Base.Acad.AT_SUB(NewCreatedGroup);
                                    //    GroupSub.SubIdCollection.Add(per.ObjectId);
                                    //    GroupSub.SubIdCollection.Add(txtOI);
                                    //    GroupSub.Insert();



                                }


                                #endregion

                            }
                            else
                            {
                                conti = false;
                            }

                        }
                        else
                        {
                            conti = false;
                        }

                    }

                }


            }




        }

        public static void RotateRod(double LastAngleDegree, double NewAngleDegree, ObjectId PoleOI, ObjectId RodOI, Point3d CenterPoint)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            using (DocumentLock docLock = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.LockDocument())
            {
                Database db = Application.DocumentManager.MdiActiveDocument.Database;
                using (Transaction tr = db.TransactionManager.StartTransaction())
                {
                    ObjectIdCollection RodSub = Atend.Global.Acad.UAcad.GetGroupSubEntities(RodOI);

                    foreach (ObjectId oi in RodSub)
                    {
                        //Atend.Base.Acad.AT_INFO KhazanSubInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oi);
                        //if (KhazanSubInfo.ParentCode != "NONE" && KhazanSubInfo.NodeType == (int)Atend.Control.Enum.ProductType.ConsolElse)
                        //{
                        Entity ent = tr.GetObject(oi, OpenMode.ForWrite) as Entity;
                        if (ent != null)
                        {

                            ////Polyline LineEntity = ent as Polyline;
                            ////if (LineEntity != null)
                            ////{
                            //ed.WriteMessage("khazan entity found \n");
                            //KhazanOI = oi;
                            //Atend.Acad.AcadMove.LastCenterPoint = Atend.Global.Acad.UAcad.CenterOfEntity(Atend.Global.Acad.UAcad.GetEntityByObjectID(oi));
                            Matrix3d trans = Matrix3d.Rotation(((LastAngleDegree * Math.PI) / 180) * -1, ed.CurrentUserCoordinateSystem.CoordinateSystem3d.Zaxis, CenterPoint);
                            ent.TransformBy(trans);

                            trans = Matrix3d.Rotation(((NewAngleDegree * Math.PI) / 180), ed.CurrentUserCoordinateSystem.CoordinateSystem3d.Zaxis, CenterPoint);
                            ent.TransformBy(trans);

                            //Matrix3d m = new Matrix3d();
                            //Atend.Acad.AcadMove.ConsolOI = ConsolOI;
                            //Atend.Acad.AcadMove.isMoveConsolOnly = true;

                            ////}
                        }
                        //}
                    }
                    tr.Commit();
                    //Atend.Acad.AcadMove.BankKhazanOI = KhazanOI;
                    //Atend.Acad.AcadMove.AllowToMove = true;
                    //Atend.Acad.AcadMove.MoveKhazan(KhazanOI);
                }
            }
        }

        //private bool SaveRodData(string ParentCode)
        //{


        //    Guid Code = new Guid(ParentCode);
        //    Atend.Base.Design.DPackage Package = Atend.Base.Design.DPackage.AccessSelectByNodeCode(Code);

        //    Atend.Base.Acad.AcadGlobal.RodData.dPackageForRod.ParentCode = Package.Code;

        //    if (Atend.Base.Acad.AcadGlobal.RodData.dPackageForRod.AccessInsert())
        //    {
        //        System.Data.DataTable OperationTable = Atend.Base.Equipment.EOperation.SelectByProductCodeType(Atend.Base.Acad.AcadGlobal.RodData.dPackageForRod.ProductCode, Atend.Base.Acad.AcadGlobal.RodData.dPackageForRod.Type);

        //        if (OperationTable.Rows.Count > 0)
        //            foreach (DataRow OperationRow in OperationTable.Rows)
        //            {
        //                //ed.WriteMessage("\n115\n");
        //                Atend.Base.Design.DPackage P = new Atend.Base.Design.DPackage();
        //                P.ParentCode = Atend.Base.Acad.AcadGlobal.RodData.dPackageForRod.Code;
        //                P.Type = (int)Atend.Control.Enum.ProductType.Operation;
        //                P.ProductCode = Convert.ToInt32(OperationRow["ProductId"].ToString());
        //                P.Count = 1;

        //                if (P.AccessInsert())
        //                { //ed.WriteMessage("\n116\n");
        //                }
        //                else
        //                    throw new System.Exception("Operation Transaction");
        //            }
        //        else
        //        {
        //            //ed.WriteMessage("\n1167\n");
        //        }
        //        return true;
        //    }
        //    else
        //        return false;
        //}
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        private bool SaveRodData(string NodeCode)
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
                    //Atend.Base.Equipment.ERod ro = Atend.Base.Equipment.ERod.AccessSelectByXCode(ERod.XCode);
                    if (!UseAccess)
                    {
                        //if (ro.Code == -1)
                        {
                            if (!ERod.AccessInsert(aTransaction, aConnection, true , true))
                            {
                                throw new System.Exception("eRod.AccessInsert failed");
                            }
                        }

                    }


                    Atend.Base.Design.DPackage dPack = Atend.Base.Design.DPackage.AccessSelectByNodeCode(new Guid(NodeCode));
                    RodPack.Count = 1;
                    RodPack.IsExistance = Existance;
                    RodPack.ProductCode = ERod.Code;
                    //if (ro.Code == -1)
                    //    RodPack.ProductCode = ERod.Code;
                    //else
                    //    RodPack.ProductCode = ro.Code;
                    RodPack.Type = (int)Atend.Control.Enum.ProductType.Rod;
                    RodPack.ParentCode = dPack.Code;
                    RodPack.LoadCode = 0;
                    RodPack.Number = "";
                    RodPack.ProjectCode = ProjectCode;
                    if (!RodPack.AccessInsert(aTransaction, aConnection))
                    {
                        throw new System.Exception("RodPack.AccessInsert failed");
                    }


                }
                catch (System.Exception ex1)
                {
                    ed.WriteMessage("ERROR SaveRodData 02 :{0} \n", ex1.Message);
                    aTransaction.Rollback();
                    aConnection.Close();
                    return false;

                }

            }
            catch (System.Exception ex)
            {
                ed.WriteMessage("ERROR SaveRodData 01 :{0} \n", ex.Message);
                aConnection.Close();
                return false;
            }

            aTransaction.Commit();
            aConnection.Close();

            #region Not to Get Green

            Atend.Base.Acad.AcadGlobal.RodData.UseAccess = true;
            //UseAccess = true;

            #endregion


            return true;

        }

        public bool UpdateRodData(Guid EXCode)
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
                    RodPack = Atend.Base.Design.DPackage.AccessSelectByCode(EXCode);
                    //ed.WriteMessage("**RodPAck.Code={0}\n",RodPack.Code);
                    if (!UseAccess)
                    {
                        if (!ERod.AccessInsert(aTransaction, aConnection, true , true))
                        {
                            throw new System.Exception("eRod.AccessInsert failed");
                        }
                    }
                    RodPack.IsExistance = Existance;
                    RodPack.ProductCode = ERod.Code;
                    RodPack.ProjectCode = ProjectCode;
                    RodPack.Number = "";
                    if (RodPack.AccessUpdate(aTransaction, aConnection))
                    {
                        //ed.WriteMessage("ERod.Code={0},SelectedRod.ObjId={1}\n",ERod.Code,selectedObjectId);
                        Atend.Base.Acad.AT_INFO atinfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(selectedObjectId);// id);

                        atinfo.ProductCode = ERod.Code;
                        atinfo.Insert();
                        ChangeComment(selectedObjectId, ERod.Comment);
                    }
                    else
                    {
                        throw new System.Exception("RodPack.AccessInsert2 failed");
                    }

                }
                catch (System.Exception ex1)
                {
                    ed.WriteMessage("ERROR Updaterod 01(transaction) : {0} \n", ex1.Message);
                    aTransaction.Rollback();
                    aConnection.Close();
                    return false;

                }

            }
            catch (System.Exception ex)
            {
                ed.WriteMessage("ERROR UpdateRod 01 : {0} \n", ex.Message);
                aConnection.Close();
                return false;
            }

            aTransaction.Commit();
            aConnection.Close();
            return true;
        }

        public static bool DeleteRodData(ObjectId RodOI)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            try
            {
                Atend.Base.Acad.AT_INFO Rodinfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(RodOI);
                if (!Atend.Base.Design.DPackage.AccessDelete(new Guid(Rodinfo.NodeCode.ToString())))
                {
                    throw new System.Exception("Error In Delete DPackage\n");
                }

            }
            catch (System.Exception ex)
            {
                ed.WriteMessage("GRA ERROR Rod Data : {0} \n", ex.Message);
                return false;
            }
            return true;
        }

        public static bool DeleteRodData(ObjectId RodOI, OleDbTransaction _Transaction, OleDbConnection _Connection)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            try
            {
                Atend.Base.Acad.AT_INFO Rodinfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(RodOI);
                if (!Atend.Base.Design.DPackage.AccessDelete(new Guid(Rodinfo.NodeCode.ToString()), _Transaction, _Connection))
                {
                    throw new System.Exception("Error In Delete DPackage\n");
                }
            }
            catch (System.Exception ex)
            {
                ed.WriteMessage("Data ERROR Transaction.Rod: {0} \n", ex.Message);
                _Transaction.Rollback();
                return false;
            }
            return true;
        }

        public static bool DeleteRod(ObjectId RodOI)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            try
            {
                ObjectId id = Atend.Global.Acad.UAcad.GetEntityGroup(RodOI);
                ObjectIdCollection CollectionRod = Atend.Global.Acad.UAcad.GetGroupSubEntities(id);
                foreach (ObjectId collect in CollectionRod)
                {
                    Atend.Base.Acad.AT_INFO at_info = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(collect);
                    if (at_info.ParentCode != "NONE" && at_info.NodeType == (int)Atend.Control.Enum.ProductType.ConsolElse)
                    {
                        Atend.Base.Acad.AT_SUB EntitySb = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(RodOI);
                        foreach (ObjectId oi in EntitySb.SubIdCollection)
                        {
                            Atend.Base.Acad.AT_INFO poleInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oi);
                            if (poleInfo.ParentCode != "NONE" && (poleInfo.NodeType == (int)Atend.Control.Enum.ProductType.Pole || poleInfo.NodeType == (int)Atend.Control.Enum.ProductType.PoleTip))
                            {
                                //Delete comment
                                Atend.Base.Acad.AT_SUB EntitySb2 = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(oi);
                                foreach (ObjectId oisub in EntitySb2.SubIdCollection)
                                {
                                    Atend.Base.Acad.AT_INFO poleInfosub = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oisub);
                                    if (poleInfosub.ParentCode != "NONE" && poleInfosub.NodeType == (int)Atend.Control.Enum.ProductType.Rod)
                                    {
                                        Atend.Base.Acad.AT_SUB RodCollection = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(oisub);
                                        foreach (ObjectId obj in RodCollection.SubIdCollection)
                                        {
                                            Atend.Base.Acad.AT_INFO at_infosub = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(obj);
                                            if (at_infosub.ParentCode != "NONE" && at_infosub.NodeType == (int)Atend.Control.Enum.ProductType.Comment)
                                            {
                                                if (!Atend.Global.Acad.AcadRemove.DeleteEntityByObjectId(obj))
                                                {
                                                    throw new System.Exception("Error In Delete Comment\n");
                                                }
                                            }
                                        }

                                    }
                                }

                                //Delete from AT_SUB
                                Atend.Base.Acad.AT_SUB.RemoveFromAT_SUB(id, oi);
                            }
                        }


                    }

                }
                foreach (ObjectId collect in CollectionRod)
                {
                    Atend.Base.Acad.AT_INFO at_info = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(collect);
                    if (at_info.ParentCode != "NONE" && at_info.NodeType == (int)Atend.Control.Enum.ProductType.Rod)
                    {
                        if (!Atend.Global.Acad.AcadRemove.DeleteEntityByObjectId(collect))
                        {
                            throw new System.Exception("Error In Delete Comment\n");
                        }
                    }
                }

                //+++
                if (!Atend.Global.Acad.AcadRemove.DeleteEntityByObjectId(RodOI))
                {
                    throw new System.Exception("Error In Delete Rod\n");
                }

            }
            catch (System.Exception ex)
            {
                ed.WriteMessage("GRA ERROR Rod : {0} \n", ex.Message);
                return false;
            }
            return true;
        }

        public void ChangeComment(ObjectId SelectedLineObjectID, string Text)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            using (DocumentLock dl = Application.DocumentManager.MdiActiveDocument.LockDocument())
            {
                Atend.Base.Acad.AT_SUB at_sub =
                Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(SelectedLineObjectID);

                Atend.Base.Acad.AT_INFO at_info;
                if (at_sub.SubIdCollection.Count > 0)
                {
                    foreach (ObjectId oi in at_sub.SubIdCollection)
                    {
                        at_info = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oi);
                        if (at_info.ParentCode.Equals("NONE"))
                        {
                        }
                        else if (at_info.NodeType == (int)Atend.Control.Enum.ProductType.Comment)
                        {
                            ChangeEntityText(oi, Text);
                        }
                    }
                }
                else
                {
                }
            }
        }

        public void ChangeEntityText(ObjectId SelectedTextObjectID, string Text)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            Database db = Application.DocumentManager.MdiActiveDocument.Database;
            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                DBText dbtext = (DBText)tr.GetObject(SelectedTextObjectID, OpenMode.ForWrite);
                if (dbtext != null)
                {
                    dbtext.TextString = Text;
                }
                tr.Commit();
            }
        }


    }
}
