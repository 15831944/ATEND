using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.OleDb;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.GraphicsInterface;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.Geometry;
//using Autodesk.AutoCAD.Interop;
using Autodesk.Windows;
using Autodesk.AutoCAD.Colors;

//get from tehran 7/15
namespace Atend.Global.Acad.DrawEquips
{
    public class AcDrawGround02
    {

        //~~~~~~~~~~~~~~~~~~~ properties ~~~~~~~~~~~~~~~~~~~~~~~~~//
        bool useAccess;
        public bool UseAccess
        {
            get { return useAccess; }
            set { useAccess = value; }
        }

        int projectCode;
        public int ProjectCode
        {
            get { return projectCode; }
            set { projectCode = value; }
        }

        int existance;
        public int Existance
        {
            get { return existance; }
            set { existance = value; }
        }

        Atend.Base.Equipment.EGround _eGround;
        public Atend.Base.Equipment.EGround eGround
        {
            get { return _eGround; }
            set { _eGround = value; }
        }

        Atend.Base.Design.DPackage GroundPack = new Atend.Base.Design.DPackage();

        private ObjectId selectedObjectId;
        public ObjectId SelectedObjectId
        {
            get { return selectedObjectId; }
            set { selectedObjectId = value; }
        }



        //~~~~~~~~~~~~~~~~~~~ class ~~~~~~~~~~~~~~~~~~~~~~~~~//
        //class DrawGroundJig : DrawJig
        //{

        //    List<Entity> Entities = new List<Entity>();
        //    public Point3d StartPoint = Point3d.Origin;
        //    Point3d EndPoint = Point3d.Origin;
        //    public bool GetStartPoint = false, GetEndPoint = true;
        //    double MyScale = 1;

        //    public DrawGroundJig(double Scale)
        //    {
        //        Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
        //        MyScale = Scale;
        //    }

        //    protected override SamplerStatus Sampler(JigPrompts prompts)
        //    {

        //        JigPromptPointOptions PPO = new JigPromptPointOptions();
        //        PromptPointResult PPr;
        //        //if (GetStartPoint)
        //        //{
        //        //    PPO.Message = "First Point";
        //        //    PPr = prompts.AcquirePoint(PPO);
        //        //    if (PPr.Status == PromptStatus.OK)
        //        //    {
        //        //        if (StartPoint != PPr.Value)
        //        //        {
        //        //            StartPoint = PPr.Value;
        //        //            EndPoint = new Point3d(StartPoint.X + 40, StartPoint.Y, StartPoint.Z);
        //        //            return SamplerStatus.OK;
        //        //        }
        //        //        else
        //        //        {
        //        //            return SamplerStatus.NoChange;
        //        //        }
        //        //    }
        //        //    else
        //        //    {
        //        //        return SamplerStatus.Cancel;
        //        //    }
        //        //}
        //        //else 
        //        if (GetEndPoint)
        //        {
        //            PPO.Message = "Second Point";
        //            PPr = prompts.AcquirePoint(PPO);
        //            if (PPr.Status == PromptStatus.OK)
        //            {
        //                if (PPr.Value == EndPoint)
        //                {
        //                    return SamplerStatus.NoChange;
        //                }
        //                else
        //                {
        //                    EndPoint = PPr.Value;
        //                    return SamplerStatus.OK;
        //                }
        //            }
        //            else
        //            {
        //                return SamplerStatus.Cancel;
        //            }
        //        }

        //        return SamplerStatus.OK;
        //    }

        //    protected override bool WorldDraw(Autodesk.AutoCAD.GraphicsInterface.WorldDraw draw)
        //    {
        //        Entities.Clear();
        //        Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
        //        //if (GetStartPoint)
        //        //{

        //        Entities.Add(CreateLine(StartPoint, EndPoint, (int)Atend.Control.Enum.ProductType.Ground, 0, 0));
        //        Matrix3d trans1 = Matrix3d.Scaling(MyScale, EndPoint);

        //        ////////Entity ent = CreateBoxEntity(EndPoint, 7, 7);
        //        ////////ent.TransformBy(trans1);
        //        ////////Entities.Add(ent);


        //        Entity ent = CreateLine(new Point3d(EndPoint.X, EndPoint.Y - 2.5, EndPoint.Z), new Point3d(EndPoint.X, EndPoint.Y + 2.5, EndPoint.Z), (int)Atend.Control.Enum.ProductType.Ground, 0, 0);
        //        ent.TransformBy(trans1);
        //        Entities.Add(ent);

        //        ent = CreateLine(new Point3d(EndPoint.X + 0.5, EndPoint.Y - 2, EndPoint.Z), new Point3d(EndPoint.X + 0.5, EndPoint.Y + 2, EndPoint.Z), (int)Atend.Control.Enum.ProductType.Ground, 0, 0);
        //        ent.TransformBy(trans1);
        //        Entities.Add(ent);

        //        ent = CreateLine(new Point3d(EndPoint.X + 1, EndPoint.Y - 1.5, EndPoint.Z), new Point3d(EndPoint.X + 1, EndPoint.Y + 1.5, EndPoint.Z), (int)Atend.Control.Enum.ProductType.Ground, 0, 0);
        //        ent.TransformBy(trans1);
        //        Entities.Add(ent);

        //        //}
        //        //else if (GetEndPoint)
        //        //{
        //        //}



        //        foreach (Entity ent1 in Entities)
        //        {
        //            draw.Geometry.Draw(ent1);
        //        }

        //        return true;
        //    }

        //    private Entity CreateLine(Point3d StartPoint, Point3d EndPoint, int ProductType, int ColorIndex, double Thickness)
        //    {
        //        Atend.Global.Acad.AcadJigs.MyLine mLine = new Atend.Global.Acad.AcadJigs.MyLine();
        //        mLine.StartPoint = StartPoint;
        //        mLine.EndPoint = EndPoint;

        //        //if (Thickness != 0)
        //        //{
        //        //    mLine.Thickness = Thickness;
        //        //}


        //        //Atend.Global.Acad.AcadJigs.SaveExtensionData(mLine, ProductType);
        //        //Atend.Global.Acad.AcadJigs.SaveExtensionData(mLine, CodeGuid);
        //        mLine.ColorIndex = ColorIndex;

        //        return mLine;
        //    }

        //    private Entity CreateBoxEntity(Point3d CenterPoint, double Width, double Height)
        //    {

        //        double BaseX = CenterPoint.X;
        //        double BaseY = CenterPoint.Y;

        //        Atend.Global.Acad.AcadJigs.MyPolyLine pLine = new Atend.Global.Acad.AcadJigs.MyPolyLine();
        //        pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX - (Width / 2), BaseY - (Height / 2)), 0, 0, 0);
        //        pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX + (Width / 2), BaseY - (Height / 2)), 0, 0, 0);
        //        pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX + (Width / 2), BaseY + (Height / 2)), 0, 0, 0);
        //        pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX - (Width / 2), BaseY + (Height / 2)), 0, 0, 0);
        //        pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX - (Width / 2), BaseY - (Height / 2)), 0, 0, 0);
        //        pLine.Closed = true;


        //        //Atend.Global.Acad.AcadJigs.SaveExtensionData(pLine, (int)Atend.Control.Enum.ProductType.);
        //        //Atend.Global.Acad.AcadJigs.SaveExtensionData(pLine, (long)ProductCode);
        //        //Atend.Global.Acad.AcadJigs.SaveExtensionData(pLine, CodeGuid);
        //        return pLine;

        //    }

        //    public List<Entity> GetEntities()
        //    {
        //        return Entities;
        //    }


        //}

        class DrawGroundJig02 : DrawJig
        {

            List<Entity> Entities = new List<Entity>();

            public Point3d StartPoint = Point3d.Origin;
            Point3d EndPoint = Point3d.Origin;
            public bool GetStartPoint = false, GetEndPoint = false;

            double MyScale = 1;

            public DrawGroundJig02(double Scale)
            {
                Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
                MyScale = Scale;
            }

            protected override SamplerStatus Sampler(JigPrompts prompts)
            {
                JigPromptPointOptions PPO = new JigPromptPointOptions();
                PromptPointResult PPr;
                if (GetStartPoint)
                {
                    PPO.Message = "First Point";
                    PPr = prompts.AcquirePoint(PPO);
                    if (PPr.Status == PromptStatus.OK)
                    {
                        if (StartPoint != PPr.Value)
                        {
                            StartPoint = PPr.Value;
                            EndPoint = new Point3d(StartPoint.X, StartPoint.Y, StartPoint.Z);
                            return SamplerStatus.OK;
                        }
                        else
                        {
                            return SamplerStatus.NoChange;
                        }
                    }
                    else
                    {
                        return SamplerStatus.Cancel;
                    }
                }
                else if (GetEndPoint)
                {
                    PPO.Message = "Second Point";
                    PPr = prompts.AcquirePoint(PPO);
                    if (PPr.Status == PromptStatus.OK)
                    {
                        if (PPr.Value == EndPoint)
                        {
                            return SamplerStatus.NoChange;
                        }
                        else
                        {
                            EndPoint = PPr.Value;
                            return SamplerStatus.OK;
                        }
                    }
                    else
                    {
                        return SamplerStatus.Cancel;
                    }
                }

                return SamplerStatus.OK;
            }

            protected override bool WorldDraw(Autodesk.AutoCAD.GraphicsInterface.WorldDraw draw)
            {
                Entities.Clear();
                Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;

                Entities.Add(CreateLine(StartPoint, EndPoint, (int)Atend.Control.Enum.ProductType.Ground, 0, 0));
                Matrix3d trans1 = Matrix3d.Scaling(MyScale, EndPoint);
                Matrix3d StartTrans = Matrix3d.Scaling(MyScale, StartPoint);


                Line l = new Line(StartPoint, EndPoint);
                double newAngle = l.Angle;
                Matrix3d trans3 = Matrix3d.Rotation(newAngle, ed.CurrentUserCoordinateSystem.CoordinateSystem3d.Zaxis, EndPoint);
                
                Entity ent = CreateLine(new Point3d(EndPoint.X, EndPoint.Y - 2.5, EndPoint.Z), new Point3d(EndPoint.X, EndPoint.Y + 2.5, EndPoint.Z), (int)Atend.Control.Enum.ProductType.Ground, 0, 0);
                ent.TransformBy(trans1);
                ent.TransformBy(trans3);
                Entities.Add(ent);

                ent = CreateLine(new Point3d(EndPoint.X + 0.5, EndPoint.Y - 2, EndPoint.Z), new Point3d(EndPoint.X + 0.5, EndPoint.Y + 2, EndPoint.Z), (int)Atend.Control.Enum.ProductType.Ground, 0, 0);
                ent.TransformBy(trans1);
                ent.TransformBy(trans3);
                Entities.Add(ent);

                ent = CreateLine(new Point3d(EndPoint.X + 1, EndPoint.Y - 1.5, EndPoint.Z), new Point3d(EndPoint.X + 1, EndPoint.Y + 1.5, EndPoint.Z), (int)Atend.Control.Enum.ProductType.Ground, 0, 0);
                ent.TransformBy(trans1);
                ent.TransformBy(trans3);
                Entities.Add(ent);

                ent = CreateConsol(StartPoint, 5, 5);
                ent.TransformBy(StartTrans);
                Entities.Add(ent);

                foreach (Entity ent1 in Entities)
                {
                    draw.Geometry.Draw(ent1);
                }

                return true;
            }

            private Entity CreateLine(Point3d StartPoint, Point3d EndPoint, int ProductType, int ColorIndex, double Thickness)
            {
                Atend.Global.Acad.AcadJigs.MyLine mLine = new Atend.Global.Acad.AcadJigs.MyLine();
                mLine.StartPoint = StartPoint;
                mLine.EndPoint = EndPoint;

                //if (Thickness != 0)
                //{
                //    mLine.Thickness = Thickness;
                //}


                //Atend.Global.Acad.AcadJigs.SaveExtensionData(mLine, ProductType);
                //Atend.Global.Acad.AcadJigs.SaveExtensionData(mLine, CodeGuid);
                mLine.ColorIndex = ColorIndex;

                return mLine;
            }

            private Entity CreateBoxEntity(Point3d CenterPoint, double Width, double Height)
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


                //Atend.Global.Acad.AcadJigs.SaveExtensionData(pLine, (int)Atend.Control.Enum.ProductType.);
                //Atend.Global.Acad.AcadJigs.SaveExtensionData(pLine, (long)ProductCode);
                //Atend.Global.Acad.AcadJigs.SaveExtensionData(pLine, CodeGuid);
                return pLine;

            }

            private Entity CreateConsol(Point3d BasePoint, double Width, double Height)
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

            public List<Entity> GetEntities()
            {
                return Entities;
            }

        }

        //~~~~~~~~~~~~~~~~~~~ method ~~~~~~~~~~~~~~~~~~~~~~~~~//

        //public void DrawGround()
        //{
        //    Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
        //    bool conti = true;
        //    //ed.WriteMessage("1\n");            
        //    List<Entity> Entities;
        //    //ed.WriteMessage("2\n");
        //    double MyScale = Atend.Base.Design.DProductProperties.AccessSelectBySoftwareCode((int)Atend.Control.Enum.ProductType.Ground).Scale;
        //    //ed.WriteMessage("3\n");
        //    //double MyCommentScale = Atend.Base.Design.DProductProperties.AccessSelectBySoftwareCode((int)Atend.Control.Enum.ProductType.Ground).CommentScale;
        //    DrawGroundJig _DrawGroundJig = new DrawGroundJig(MyScale);

        //    while (conti)
        //    {
        //        PromptEntityOptions PEO = new PromptEntityOptions("Select Parent:");
        //        PromptEntityResult PER = ed.GetEntity(PEO);
        //        if (PER.Status == PromptStatus.OK)
        //        {
        //            Atend.Base.Acad.AT_INFO SelectedEntityInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(PER.ObjectId);
        //            if (SelectedEntityInfo.ParentCode != "NONE")
        //            {

        //                System.Data.DataTable Parents = Atend.Global.Acad.UAcad.DetermineParent((int)Atend.Control.Enum.ProductType.Ground);
        //                DataRow[] drs = Parents.Select(string.Format("SoftwareCode={0}", SelectedEntityInfo.NodeType));
        //                if (drs.Length > 0)
        //                {
        //                    while (conti)
        //                    {
        //                        _DrawGroundJig.GetStartPoint = false;
        //                        _DrawGroundJig.StartPoint = PER.PickedPoint;
        //                        PromptResult pr = ed.Drag(_DrawGroundJig);
        //                        //pr = ed.Drag(_DrawGroundJig);
        //                        if (pr.Status == PromptStatus.OK)
        //                        {
        //                            conti = false;
        //                            _DrawGroundJig.GetEndPoint = false;
        //                            Entities = _DrawGroundJig.GetEntities();

        //                            #region SaveData
        //                            ObjectIdCollection OIC = new ObjectIdCollection();

        //                            if (SaveGroundData(SelectedEntityInfo.NodeCode))
        //                            {
        //                                foreach (Entity ent in Entities)
        //                                {

        //                                    OIC.Add(Atend.Global.Acad.UAcad.DrawEntityOnScreen(ent, Atend.Control.Enum.AutoCadLayerName.GENERAL.ToString()));

        //                                    Atend.Base.Acad.AT_INFO GroundInfo = new Atend.Base.Acad.AT_INFO(OIC[OIC.Count - 1]);
        //                                    GroundInfo.ParentCode = SelectedEntityInfo.NodeCode;
        //                                    GroundInfo.NodeCode = GroundPack.Code.ToString();
        //                                    GroundInfo.NodeType = (int)Atend.Control.Enum.ProductType.Ground;
        //                                    GroundInfo.ProductCode = eGround.Code;
        //                                    GroundInfo.Insert();

        //                                }

        //                                ObjectId GOI = Atend.Global.Acad.Global.MakeGroup(GroundPack.Code.ToString(), OIC);
        //                                Atend.Base.Acad.AT_INFO GroundGroupInfo = new Atend.Base.Acad.AT_INFO(GOI);
        //                                GroundGroupInfo.ParentCode = SelectedEntityInfo.NodeCode;
        //                                GroundGroupInfo.NodeCode = GroundPack.Code.ToString();
        //                                GroundGroupInfo.NodeType = (int)Atend.Control.Enum.ProductType.Ground;
        //                                GroundGroupInfo.ProductCode = eGround.Code;
        //                                GroundGroupInfo.Insert();


        //                                Atend.Base.Acad.AT_SUB.AddToAT_SUB(GOI, PER.ObjectId);


        //                                Atend.Base.Acad.AT_SUB GroundSub = new Atend.Base.Acad.AT_SUB(GOI);
        //                                GroundSub.SubIdCollection.Add(PER.ObjectId);
        //                                GroundSub.Insert();

        //                            }

        //                            #endregion
        //                        }
        //                        else
        //                        {
        //                            conti = false;
        //                        }
        //                    }// end while
        //                }
        //                else
        //                {

        //                    string s = "";
        //                    foreach (DataRow dr in Parents.Rows)
        //                    {
        //                        s = s + Atend.Base.Design.DProductProperties.AccessSelectByCodeDrawable(Convert.ToInt32(dr["ContainerCode"])).ProductName + "-";
        //                    }
        //                    Atend.Global.Acad.Notification notification = new Atend.Global.Acad.Notification();
        //                    notification.Title = "������� ����";
        //                    notification.Msg = s;
        //                    notification.infoCenterBalloon();

        //                }
        //            }
        //        }
        //        else
        //        {
        //            conti = false;
        //        }
        //    }
        //}

        private bool IsValidParent(Point3d MyPoint, out ObjectId ParentObjectId)
        {

            bool Answer = false;
            ParentObjectId = ObjectId.Null;

            System.Data.DataTable RealParents = Atend.Global.Acad.UAcad.DetermineParent((int)Atend.Control.Enum.ProductType.Ground);
            System.Data.DataTable VirtualParents = Atend.Global.Acad.Global.PointInsideWhichEntity(MyPoint);

            foreach (DataRow dr in RealParents.Rows)
            {
                DataRow[] drs = VirtualParents.Select("type=" + dr["SoftwareCode"]);
                if (drs.Length > 0)
                {
                    Answer = true;
                    ParentObjectId = new ObjectId(new IntPtr(Convert.ToInt32(drs[0]["ObjectId"].ToString())));
                }
            }


            return Answer;

        }

        public void DrawGround02()
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            bool conti = true;
            //ed.WriteMessage("1\n");            
            List<Entity> Entities;
            //ed.WriteMessage("2\n");
            double MyScale = Atend.Base.Design.DProductProperties.AccessSelectBySoftwareCode((int)Atend.Control.Enum.ProductType.Ground).Scale;
            //ed.WriteMessage("3\n");
            //double MyCommentScale = Atend.Base.Design.DProductProperties.AccessSelectBySoftwareCode((int)Atend.Control.Enum.ProductType.Ground).CommentScale;
            DrawGroundJig02 _DrawGroundJig = new DrawGroundJig02(MyScale);

            while (conti)
            {
                //PromptEntityOptions PEO = new PromptEntityOptions("Select Parent:");
                //PromptEntityResult PER = ed.GetEntity(PEO);
                //if (PER.Status == PromptStatus.OK)
                //{
                //Atend.Base.Acad.AT_INFO SelectedEntityInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(PER.ObjectId);
                //if (SelectedEntityInfo.ParentCode != "NONE")
                //{

                //System.Data.DataTable Parents = Atend.Global.Acad.UAcad.DetermineParent((int)Atend.Control.Enum.ProductType.Ground);
                //DataRow[] drs = Parents.Select(string.Format("SoftwareCode={0}", SelectedEntityInfo.NodeType));
                //if (drs.Length > 0)
                //{
                ObjectId ParentOI = ObjectId.Null;
                while (conti)
                {
                    _DrawGroundJig.GetStartPoint = true;
                    _DrawGroundJig.GetEndPoint = false;
                    PromptResult pr = ed.Drag(_DrawGroundJig);
                    //pr = ed.Drag(_DrawGroundJig);
                    if (pr.Status == PromptStatus.OK )
                    {
                        if (IsValidParent(_DrawGroundJig.StartPoint, out ParentOI))
                        {
                            _DrawGroundJig.GetStartPoint = false;
                            _DrawGroundJig.GetEndPoint = true;
                            pr = ed.Drag(_DrawGroundJig);
                            //pr = ed.Drag(_DrawGroundJig);
                            if (pr.Status == PromptStatus.OK)
                            {

                                _DrawGroundJig.GetStartPoint = false;
                                _DrawGroundJig.GetEndPoint = false;
                                conti = false;

                                Entities = _DrawGroundJig.GetEntities();
                                //foreach (Entity en in Entities)
                                //{
                                //    Atend.Global.Acad.UAcad.DrawEntityOnScreen(en, Atend.Control.Enum.AutoCadLayerName.GENERAL.ToString());
                                //}

                                #region SaveData
                                ObjectIdCollection OIC = new ObjectIdCollection();
                                Atend.Base.Acad.AT_INFO SelectedEntityInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(ParentOI);
                                if (SaveGroundData(SelectedEntityInfo.NodeCode))
                                {
                                    foreach (Entity ent in Entities)
                                    {

                                        OIC.Add(Atend.Global.Acad.UAcad.DrawEntityOnScreen(ent, Atend.Control.Enum.AutoCadLayerName.GENERAL.ToString()));

                                        Atend.Base.Acad.AT_INFO GroundInfo = new Atend.Base.Acad.AT_INFO(OIC[OIC.Count - 1]);
                                        GroundInfo.ParentCode = SelectedEntityInfo.NodeCode;
                                        GroundInfo.NodeCode = GroundPack.Code.ToString();
                                        GroundInfo.NodeType = (int)Atend.Control.Enum.ProductType.Ground;
                                        GroundInfo.ProductCode = eGround.Code;
                                        GroundInfo.Insert();

                                    }

                                    ObjectId GOI = Atend.Global.Acad.Global.MakeGroup(GroundPack.Code.ToString(), OIC);
                                    Atend.Base.Acad.AT_INFO GroundGroupInfo = new Atend.Base.Acad.AT_INFO(GOI);
                                    GroundGroupInfo.ParentCode = SelectedEntityInfo.NodeCode;
                                    GroundGroupInfo.NodeCode = GroundPack.Code.ToString();
                                    GroundGroupInfo.NodeType = (int)Atend.Control.Enum.ProductType.Ground;
                                    GroundGroupInfo.ProductCode = eGround.Code;
                                    GroundGroupInfo.Insert();


                                    Atend.Base.Acad.AT_SUB.AddToAT_SUB(GOI, ParentOI);


                                    Atend.Base.Acad.AT_SUB GroundSub = new Atend.Base.Acad.AT_SUB(GOI);
                                    GroundSub.SubIdCollection.Add(ParentOI);
                                    GroundSub.Insert();

                                }

                                #endregion

                            }

                            else
                            {
                                conti = false;
                            }


                        
                            conti = false;
                        }
                    }
                    else 
                    {
                        ed.WriteMessage("parent was not valid\n");
                        conti = false;
                    }
                }// end while
                //}
                //else
                //{

                //    string s = "";
                //    foreach (DataRow dr in Parents.Rows)
                //    {
                //        s = s + Atend.Base.Design.DProductProperties.AccessSelectByCodeDrawable(Convert.ToInt32(dr["ContainerCode"])).ProductName + "-";
                //    }
                //    Atend.Global.Acad.Notification notification = new Atend.Global.Acad.Notification();
                //    notification.Title = "������� ����";
                //    notification.Msg = s;
                //    notification.infoCenterBalloon();

                //}
                //}
                //}
                //else
                //{
                //    conti = false;
                //}
            }
        }

        private bool SaveGroundData(string NodeCode)
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
                        if (!eGround.AccessInsert(aTransaction, aConnection, true, true))
                        {
                            throw new System.Exception("eGround.AccessInsert failed");
                        }
                    }
                    //ed.WriteMessage("--->>>>----1\n");
                    GroundPack.Count = 1;
                    GroundPack.IsExistance = Existance;
                    GroundPack.LoadCode = 0;
                    GroundPack.NodeCode = new Guid();
                    GroundPack.Number = "GROUND";
                    ed.WriteMessage("NodeCode:{0}\n", NodeCode);
                    Atend.Base.Design.DPackage TempPackage = Atend.Base.Design.DPackage.AccessSelectByNodeCode(new Guid(NodeCode), aTransaction, aConnection);
                    if (TempPackage.Code == Guid.Empty)
                    {
                        TempPackage = Atend.Base.Design.DPackage.AccessSelectByCode(new Guid(NodeCode), aTransaction, aConnection);
                        if (TempPackage.Code == Guid.Empty)
                        {
                            throw new System.Exception("GroundPack.AccessSelectByNodeCode failed");
                        }
                    }
                    GroundPack.ParentCode = TempPackage.Code;
                    GroundPack.ProductCode = eGround.Code;
                    GroundPack.ProjectCode = projectCode;
                    GroundPack.Type = (int)Atend.Control.Enum.ProductType.Ground;
                    if (!GroundPack.AccessInsert(aTransaction, aConnection))
                    {
                        throw new System.Exception("GroundPack.AccessInsert failed");
                    }
                    //ed.WriteMessage("--->>>>----2\n");
                }
                catch (System.Exception ex1)
                {
                    ed.WriteMessage("ERROR SaveGroundData 02 : {0}\n", ex1.Message);
                    aTransaction.Rollback();
                    aConnection.Close();
                    return false;
                }

            }
            catch (System.Exception ex)
            {
                ed.WriteMessage("ERROR SaveGroundData 01 : {0}\n", ex.Message);
                aConnection.Close();
                return false;
            }

            aTransaction.Commit();
            aConnection.Close();

            #region Not to Get Green

            Atend.Base.Acad.AcadGlobal.GroundData.UseAccess = true;
            //UseAccess = true;

            #endregion

            return true;

        }

        public bool UpdateGroundData(Guid EXCode)
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
                    GroundPack = Atend.Base.Design.DPackage.AccessSelectByCode(EXCode);
                    if (!UseAccess)
                    {
                        if (!eGround.AccessInsert(aTransaction, aConnection, true, true))
                        {
                            throw new System.Exception("eGround.AccessInsert failed");
                        }
                    }
                    GroundPack.IsExistance = Existance;
                    GroundPack.ProjectCode = ProjectCode;
                    GroundPack.ProductCode = eGround.Code;
                    GroundPack.Number = "";
                    if (GroundPack.AccessUpdate(aTransaction, aConnection))
                    {
                        Atend.Base.Acad.AT_INFO atinfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(selectedObjectId);
                        atinfo.ProductCode = eGround.Code;
                        atinfo.Insert();
                    }
                    else
                    {
                        throw new System.Exception("GroundPack.AccessInsert2 failed");
                    }

                }
                catch (System.Exception ex1)
                {
                    ed.WriteMessage("ERROR UpdateGround 01(transaction) : {0} \n", ex1.Message);
                    aTransaction.Rollback();
                    aConnection.Close();
                    return false;

                }

            }
            catch (System.Exception ex)
            {
                ed.WriteMessage("ERROR UpdateGround 01 : {0} \n", ex.Message);
                aConnection.Close();
                return false;
            }

            aTransaction.Commit();
            aConnection.Close();
            return true;
        }

        public static bool DeleteGroundData(ObjectId GroundOI)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            try
            {
                //delete Ground
                Atend.Base.Acad.AT_INFO Groundinfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(GroundOI);
                if (!Atend.Base.Design.DPackage.AccessDelete(new Guid(Groundinfo.NodeCode.ToString())))
                {
                    throw new System.Exception("Error In Delete dpackage\n");
                }
            }
            catch (System.Exception ex)
            {
                ed.WriteMessage("Data ERROR Ground : {0} \n", ex.Message);
                return false;
            }
            return true;
        }

        public static bool DeleteGroundData(ObjectId GroundOI, OleDbTransaction _Transaction, OleDbConnection _Connection)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            try
            {
                //delete Ground
                Atend.Base.Acad.AT_INFO Groundinfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(GroundOI);
                if (!Atend.Base.Design.DPackage.AccessDelete(new Guid(Groundinfo.NodeCode.ToString()), _Transaction, _Connection))
                {
                    throw new System.Exception("Error In Delete dpackage\n");
                }
            }
            catch (System.Exception ex)
            {
                ed.WriteMessage("Data ERROR Transaction.Ground : {0} \n", ex.Message);
                _Transaction.Rollback();
                return false;
            }
            return true;
        }

        public static bool DeleteGround(ObjectId GroundOI)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            try
            {
                ObjectId id = Atend.Global.Acad.UAcad.GetEntityGroup(GroundOI);
                //Find Parent
                Atend.Base.Acad.AT_SUB EntitySb = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(id);
                foreach (ObjectId oi in EntitySb.SubIdCollection)
                {
                    Atend.Base.Acad.AT_INFO pInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oi);
                    if (pInfo.ParentCode != "NONE" && (pInfo.NodeType == (int)Atend.Control.Enum.ProductType.Pole || pInfo.NodeType == (int)Atend.Control.Enum.ProductType.StreetBox ||
                                                        pInfo.NodeType == (int)Atend.Control.Enum.ProductType.DB || pInfo.NodeType == (int)Atend.Control.Enum.ProductType.GroundPost ||
                                                        pInfo.NodeType == (int)Atend.Control.Enum.ProductType.AirPost || pInfo.NodeType == (int)Atend.Control.Enum.ProductType.PoleTip))
                    {
                        Atend.Base.Acad.AT_SUB pSub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(oi);
                        foreach (ObjectId soi in pSub.SubIdCollection)
                        {
                            Atend.Base.Acad.AT_INFO Info1 = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(soi);
                            if (Info1.ParentCode != "NONE" && Info1.NodeType == (int)Atend.Control.Enum.ProductType.Ground && id == soi)
                            {
                                Atend.Base.Acad.AT_SUB.RemoveFromAT_SUB(id, oi);
                            }
                        }
                    }
                }

                //Delete Group
                ObjectIdCollection Collection = Atend.Global.Acad.UAcad.GetGroupSubEntities(id);
                foreach (ObjectId collect in Collection)
                {
                    if (!Atend.Global.Acad.AcadRemove.DeleteEntityByObjectId(collect))
                    {
                        throw new System.Exception("Error In Delete Group\n");
                    }
                }

                //+++
                if (!Atend.Global.Acad.AcadRemove.DeleteEntityByObjectId(GroundOI))
                {
                    throw new System.Exception("GRA while delete StreetBox\n");
                }

            }
            catch (System.Exception ex)
            {
                ed.WriteMessage("GRA ERROR Ground: {0} \n", ex.Message);
                return false;
            }
            return true;
        }

        public static void RotateGround(double LastAngleDegree, double NewAngleDegree, ObjectId PoleOI, ObjectId GroundOI, Point3d CenterPoint)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            using (DocumentLock docLock = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.LockDocument())
            {
                Database db = Application.DocumentManager.MdiActiveDocument.Database;
                using (Transaction tr = db.TransactionManager.StartTransaction())
                {
                    ObjectIdCollection GroundSub = Atend.Global.Acad.UAcad.GetGroupSubEntities(GroundOI);

                    foreach (ObjectId oi in GroundSub)
                    {
                        //Atend.Base.Acad.AT_INFO KhazanSubInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oi);
                        //if (KhazanSubInfo.ParentCode != "NONE" && KhazanSubInfo.NodeType == (int)Atend.Control.Enum.PGrounductType.ConsolElse)
                        //{
                        Entity ent = tr.GetObject(oi, OpenMode.ForWrite) as Entity;
                        if (ent != null)
                        {

                            Matrix3d trans = Matrix3d.Rotation(((LastAngleDegree * Math.PI) / 180) * -1, ed.CurrentUserCoordinateSystem.CoordinateSystem3d.Zaxis, CenterPoint);
                            ent.TransformBy(trans);

                            trans = Matrix3d.Rotation(((NewAngleDegree * Math.PI) / 180), ed.CurrentUserCoordinateSystem.CoordinateSystem3d.Zaxis, CenterPoint);
                            ent.TransformBy(trans);


                            ////}
                        }
                        //}
                    }
                    tr.Commit();
                }
            }
        }



    }
}
