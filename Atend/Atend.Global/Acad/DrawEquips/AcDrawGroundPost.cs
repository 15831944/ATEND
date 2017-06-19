﻿using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;

using Autodesk.AutoCAD.EditorInput;
//using Autodesk.AutoCAD.GraphicsInterface;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.Geometry;
//using Autodesk.AutoCAD.Interop;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.Windows;
using Autodesk.AutoCAD.Colors;


namespace Atend.Global.Acad.DrawEquips
{




    public class AcDrawGroundPost
    {
        //~~~~~~~~~~~~~~~~~~~~~~ properties ~~~~~~~~~~~~~~~~~~//

        bool _UseAccess;
        public bool UseAccess
        {
            get { return _UseAccess; }
            set { _UseAccess = value; }
        }

        Atend.Base.Equipment.EGroundPost _eGroundPost;
        public Atend.Base.Equipment.EGroundPost eGroundPost
        {
            get { return _eGroundPost; }
            set { _eGroundPost = value; }
        }

        List<Atend.Base.Equipment.EJAckPanel> _eJackPanelMiddles;
        public List<Atend.Base.Equipment.EJAckPanel> eJackPanelMiddles
        {
            get { return _eJackPanelMiddles; }
            set { _eJackPanelMiddles = value; }
        }


        List<Atend.Base.Equipment.EJackPanelWeek> _eJackPanelWeeks;
        public List<Atend.Base.Equipment.EJackPanelWeek> eJackPanelWeeks
        {
            get { return _eJackPanelWeeks; }
            set { _eJackPanelWeeks = value; }
        }

        List<Atend.Base.Equipment.ETransformer> _eTransformers;
        public List<Atend.Base.Equipment.ETransformer> eTransformers
        {
            get { return _eTransformers; }
            set { _eTransformers = value; }
        }

        int _Existance;
        public int Existance
        {
            get { return _Existance; }
            set { _Existance = value; }
        }

        int _ProjectcODE;

        ArrayList _eJackPanelMiddleExistance;
        public ArrayList eJackPanelMiddleExistance
        {
            get { return _eJackPanelMiddleExistance; }
            set { _eJackPanelMiddleExistance = value; }
        }

        ArrayList _eJackPanelMiddleProjectCode;
        public ArrayList eJackPanelMiddleProjectCode
        {
            get { return _eJackPanelMiddleProjectCode; }
            set { _eJackPanelMiddleProjectCode = value; }
        }

        ArrayList _eJackPanelWeekExistance;
        public ArrayList eJackPanelWeekExistance
        {
            get { return _eJackPanelWeekExistance; }
            set { _eJackPanelWeekExistance = value; }
        }

        ArrayList _eJackPanelWeekProjectCode;
        public ArrayList eJackPanelWeekProjectCode
        {
            get { return _eJackPanelWeekProjectCode; }
            set { _eJackPanelWeekProjectCode = value; }
        }

        ArrayList _eTransformerExistance;
        public ArrayList eTransformerExistance
        {
            get { return _eTransformerExistance; }
            set { _eTransformerExistance = value; }
        }

        ArrayList _eTransformerProjectCode;
        public ArrayList eTransformerProjectCode
        {
            get { return _eTransformerProjectCode; }
            set { _eTransformerProjectCode = value; }
        }

        List<PostEquips> PostEquipInserted;
        public class PostEquips
        {
            public Guid ParentCode;
            public int ProductCode = 0;
            public int ProductType = 0;
            public Guid CodeGuid;
        }


        int projectCode;
        public int ProjectCode
        {
            get { return projectCode; }
            set { projectCode = value; }
        }

        Atend.Base.Design.DPackage GroundPostPack = new Atend.Base.Design.DPackage();

        private ObjectId selectedObjectId;
        public ObjectId SelectedObjectId
        {
            get { return selectedObjectId; }
            set { selectedObjectId = value; }
        }

        //~~~~~~~~~~~~~~~~~~~~~~ class ~~~~~~~~~~~~~~~~~~//

        public class DrawGroundPostJig : DrawJig
        {
            double GroundPostWidth = 0;
            double GroundPostHeight = 0;
            double scale = 1;
            Point3d BasePoint;
            double NewAngle, BaseAngle = 0;
            Atend.Global.Acad.AcadJigs.MyPolyLine GroundPostEntiry;
            public bool GetAngle = false;
            private Autodesk.AutoCAD.GraphicsInterface.TextStyle _style;

            public DrawGroundPostJig(double Width, double Height, double Scale)
            {
                scale = Scale;
                // SET TEXT STYLE

                _style = new Autodesk.AutoCAD.GraphicsInterface.TextStyle();

                _style.Font = new Autodesk.AutoCAD.GraphicsInterface.FontDescriptor("Calibri", false, true, 0, 0);

                _style.TextSize = 10;

                // END OF SET TEXT STYLE


                GroundPostHeight = Height * 100;
                GroundPostWidth = Width * 100;
                scale = Scale;
                BasePoint = Point3d.Origin;
                GroundPostEntiry = CreatePostEntity(BasePoint, GroundPostWidth, GroundPostHeight);

            }

            private Atend.Global.Acad.AcadJigs.MyPolyLine CreatePostEntity(Point3d CenterPoint, double Width, double Height)
            {

                double BaseX = CenterPoint.X;
                double BaseY = CenterPoint.Y;

                Atend.Global.Acad.AcadJigs.MyPolyLine pLine = new Atend.Global.Acad.AcadJigs.MyPolyLine();
                //pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX - (Width / 2), BaseY - (Height / 2)), 0, 0, 0);
                //pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX + (Width / 2), BaseY - (Height / 2)), 0, 0, 0);
                //pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX + (Width / 2), BaseY + (Height / 2)), 0, 0, 0);
                //pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX - (Width / 2), BaseY + (Height / 2)), 0, 0, 0);
                //pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX - (Width / 2), BaseY - (Height / 2)), 0, 0, 0);


                pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX - (Width / 2), BaseY + (Height / 2)), 0, 0, 0);
                pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX + (Width / 2), BaseY + (Height / 2)), 0, 0, 0);
                pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX + (Width / 2), BaseY - (Height / 2)), 0, 0, 0);
                pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX - (Width / 2), BaseY - (Height / 2)), 0, 0, 0);
                pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX - (Width / 2), BaseY + (Height / 2)), 0, 0, 0);




                pLine.Closed = true;




                //ed.WriteMessage("B1 \n");
                //SaveExtensionData(pLine, (int)Atend.Control.Enum.ProductType.Pole);


                return pLine;

            }

            protected override SamplerStatus Sampler(JigPrompts prompts)
            {
                //throw new System.Exception("The method or operation is not implemented.");
                if (!GetAngle)
                {

                    JigPromptPointOptions jppo = new JigPromptPointOptions("\nEnter Ground Post Position :");
                    PromptPointResult ppr = prompts.AcquirePoint(jppo);
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
                        //Error occured
                        return SamplerStatus.Cancel;
                    }
                }
                else
                {
                    //time to get angle

                    JigPromptAngleOptions pao = new JigPromptAngleOptions("\nEnter Angle Of Ground post : ");
                    pao.UseBasePoint = true;
                    pao.BasePoint = BasePoint;

                    PromptDoubleResult pdr = prompts.AcquireAngle(pao);

                    if (pdr.Status == PromptStatus.OK)
                    {
                        if (pdr.Value == NewAngle)
                        {
                            return SamplerStatus.NoChange;

                        }
                        else
                        {
                            NewAngle = pdr.Value - NewAngle;
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

                Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;

                // SHOW POSITION VALUE

                Autodesk.AutoCAD.GraphicsInterface.WorldGeometry wg2 = draw.Geometry as Autodesk.AutoCAD.GraphicsInterface.WorldGeometry;

                if (wg2 != null)
                {

                    // Push our transforms onto the stack

                    wg2.PushOrientationTransform(Autodesk.AutoCAD.GraphicsInterface.OrientationBehavior.Screen);

                    wg2.PushPositionTransform(Autodesk.AutoCAD.GraphicsInterface.PositionBehavior.Screen, new Point2d(30, 30));

                    // Draw our screen-fixed text

                    wg2.Text(

                        new Point3d(0, 0, 0),  // Position

                        new Vector3d(0, 0, 1), // Normal

                        new Vector3d(1, 0, 0), // Direction

                        BasePoint.ToString(), // Text

                        true,                  // Rawness

                        _style                // TextStyle

                            );


                    // Remember to pop our transforms off the stack

                    wg2.PopModelTransform();

                    wg2.PopModelTransform();

                }

                // END OF SHOW POSITION VALUE



                if (!GetAngle)
                {
                    GroundPostEntiry = CreatePostEntity(BasePoint, GroundPostWidth, GroundPostHeight);

                    //~~~~~~~~ SCALE ~~~~~~~~~~

                    Matrix3d trans1 = Matrix3d.Scaling(scale, new Point3d(BasePoint.X, BasePoint.Y, 0));
                    GroundPostEntiry.TransformBy(trans1);

                    //~~~~~~~~~~~~~~~~~~~~~~~~~

                }
                else
                {

                    Matrix3d trans = Matrix3d.Rotation(NewAngle - BaseAngle, ed.CurrentUserCoordinateSystem.CoordinateSystem3d.Zaxis,

                                                       new Point3d(BasePoint.X, BasePoint.Y, 0));

                    GroundPostEntiry.TransformBy(trans);

                    BaseAngle = NewAngle;
                    NewAngle = 0;

                }

                draw.Geometry.Draw(GroundPostEntiry);

                return true;
            }

            public Entity GetEntity()
            {
                return GroundPostEntiry;
            }

        }

        //~~~~~~~~~~~~~~~~~~~~~~ methods ~~~~~~~~~~~~~~~~~~//

        public AcDrawGroundPost()
        {
            _eJackPanelMiddles = new List<Atend.Base.Equipment.EJAckPanel>();
            _eJackPanelWeeks = new List<Atend.Base.Equipment.EJackPanelWeek>();
            _eTransformers = new List<Atend.Base.Equipment.ETransformer>();
            _eJackPanelMiddleExistance = new ArrayList();
            _eJackPanelWeekExistance = new ArrayList();
            _eTransformerExistance = new ArrayList();
            PostEquipInserted = new List<PostEquips>();
        }

        public void DrawGroundPost()
        {

            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            double w, h;
            Entity PostEntity = null;
            Dictionary<Guid, Atend.Base.Equipment.EJackPanelCell> MJCells = new Dictionary<Guid, Atend.Base.Equipment.EJackPanelCell>();

            double MyScale = Atend.Base.Design.DProductProperties.AccessSelectBySoftwareCode((int)Atend.Control.Enum.ProductType.GroundPost).Scale;
            double MyCommentScale = Atend.Base.Design.DProductProperties.AccessSelectBySoftwareCode((int)Atend.Control.Enum.ProductType.GroundPost).CommentScale;

            Atend.Global.Design.frmPostSize FPS = new Atend.Global.Design.frmPostSize();
            if (Application.ShowModalDialog(FPS) == System.Windows.Forms.DialogResult.OK)
            {
                h = FPS.Tol;
                w = FPS.Arz;
                //________________________________________________
                // All data was getting well
                DrawGroundPostJig drawGroundPostJig = new DrawGroundPostJig(w, h, MyScale);
                bool Conti = true;

                while (Conti)
                {
                    PromptResult pr = ed.Drag(drawGroundPostJig);
                    if (pr.Status == PromptStatus.OK)
                    {
                        drawGroundPostJig.GetAngle = true;
                        pr = ed.Drag(drawGroundPostJig);
                        if (pr.Status == PromptStatus.OK)
                        {
                            //ed.WriteMessage("go for save\n");
                            Conti = false;
                            #region SaveDataHere

                            if (SaveGroundPostData02())
                            {
                                //Draw Post
                                ObjectId oi = Atend.Global.Acad.UAcad.DrawEntityOnScreen(drawGroundPostJig.GetEntity(), Atend.Control.Enum.AutoCadLayerName.POST.ToString());
                                PostEntity = Atend.Global.Acad.UAcad.GetEntityByObjectID(oi);

                                Guid CurrentPostCodinDPost = Guid.Empty;
                                int ProductCode = 0;
                                PostEquips ForDelete = null;

                                foreach (PostEquips p in PostEquipInserted)
                                {
                                    if (p.ProductType == (int)Atend.Control.Enum.ProductType.GroundPost)
                                    {
                                        CurrentPostCodinDPost = p.CodeGuid;
                                        ProductCode = p.ProductCode;
                                        ForDelete = p;
                                    }
                                }


                                if (ForDelete != null)
                                {
                                    //ed.WriteMessage("GROUNDPOST found \n");
                                    Atend.Base.Acad.AT_INFO postInfo = new Atend.Base.Acad.AT_INFO(oi);
                                    postInfo.ParentCode = "";
                                    postInfo.NodeCode = CurrentPostCodinDPost.ToString();
                                    postInfo.NodeType = (int)Atend.Control.Enum.ProductType.GroundPost;
                                    postInfo.ProductCode = ProductCode;

                                    postInfo.Insert();

                                    string comment = string.Format("Ground Post {0} KVA", eGroundPost.Capacity);
                                    ObjectId DrawnText = Atend.Global.Acad.UAcad.DrawEntityOnScreen(Atend.Global.Acad.UAcad.WriteNote(comment, Atend.Global.Acad.UAcad.CenterOfEntity(drawGroundPostJig.GetEntity()), MyCommentScale), Atend.Control.Enum.AutoCadLayerName.GENERAL.ToString());

                                    Atend.Base.Acad.AT_INFO textInfo = new Atend.Base.Acad.AT_INFO(DrawnText);
                                    textInfo.ParentCode = CurrentPostCodinDPost.ToString();
                                    textInfo.NodeCode = "";
                                    textInfo.NodeType = (int)Atend.Control.Enum.ProductType.Comment;
                                    textInfo.ProductCode = 0;
                                    textInfo.Insert();

                                    Atend.Base.Acad.AT_SUB PostSub = new Atend.Base.Acad.AT_SUB(oi);
                                    PostSub.SubIdCollection.Add(DrawnText);
                                    PostSub.Insert();

                                    PostEquipInserted.Remove(ForDelete);

                                    //Draw Week Jack

                                    PostEquips ForDelete1 = null;
                                    foreach (Atend.Base.Equipment.EJackPanelWeek jw in eJackPanelWeeks)
                                    {
                                        //ed.WriteMessage("JW code :{0} type :{1} \n", jw.Code, (int)Atend.Control.Enum.ProductType.WeekJackPanel);
                                        foreach (PostEquips pes in PostEquipInserted)
                                        {
                                            //ed.WriteMessage("Type:{0} Code:{1} \n",pes.ProductType , pes.ProductCode);
                                            if (pes.ProductType == (int)Atend.Control.Enum.ProductType.WeekJackPanel && pes.ProductCode == jw.Code)
                                            {
                                                ForDelete1 = pes;
                                                // ed.WriteMessage("***\n");
                                            }
                                        }

                                        if (ForDelete1 != null)
                                        {
                                            //ed.WriteMessage("Week found \n");
                                            AcDrawWeekJackPanel ADWJ = new AcDrawWeekJackPanel();
                                            ADWJ.NodeCode = ForDelete1.CodeGuid;
                                            ADWJ.ParentCode = ForDelete1.ParentCode;
                                            ADWJ.ProductCode = ForDelete1.ProductCode;
                                            ADWJ.DrawWeekJackPanel(PostEntity, jw.FeederCount);
                                            PostEquipInserted.Remove(ForDelete1);
                                        }
                                        ForDelete1 = null;
                                    }

                                    //Draw Transformer
                                    foreach (Atend.Base.Equipment.ETransformer tf in eTransformers)
                                    {
                                        foreach (PostEquips pes in PostEquipInserted)
                                        {
                                            if (pes.ProductType == (int)Atend.Control.Enum.ProductType.Transformer && pes.ProductCode == tf.Code)
                                            {
                                                ForDelete1 = pes;
                                            }
                                        }

                                        if (ForDelete1 != null)
                                        {
                                            AcDrawTransformer ADT = new AcDrawTransformer();
                                            ADT.NodeCode = ForDelete1.CodeGuid;
                                            ADT.ParentCode = ForDelete1.ParentCode;
                                            ADT.ProductCode = ForDelete1.ProductCode;

                                            ADT.DrawTransformer(PostEntity);
                                            PostEquipInserted.Remove(ForDelete1);
                                        }
                                        ForDelete1 = null;
                                    }


                                    //draw middle jackpanel
                                    foreach (Atend.Base.Equipment.EJAckPanel jm in eJackPanelMiddles)
                                    {
                                        MJCells = new Dictionary<Guid, Atend.Base.Equipment.EJackPanelCell>();
                                        //ed.WriteMessage("Current jm code : {0} \n", jm.Code);
                                        foreach (PostEquips pes in PostEquipInserted)
                                        {
                                            if (pes.ProductType == (int)Atend.Control.Enum.ProductType.MiddleJackPanel && pes.ProductCode == jm.Code)
                                            {
                                                ForDelete1 = pes;
                                            }
                                        }

                                        if (ForDelete1 != null)
                                        {
                                            //PostEquips ForDelete2 = null;
                                            foreach (PostEquips pes in PostEquipInserted)
                                            {
                                                if (pes.ProductType == (int)Atend.Control.Enum.ProductType.Cell && pes.ParentCode == ForDelete1.CodeGuid)
                                                {
                                                    //ForDelete2 = pes;
                                                    MJCells.Add(pes.CodeGuid, Atend.Base.Equipment.EJackPanelCell.AccessSelectByCode(pes.ProductCode));

                                                }
                                            }


                                            //List<Atend.Base.Equipment.EJackPanelCell> cells = new List<Atend.Base.Equipment.EJackPanelCell>();
                                            ////System.Data.DataTable CellsList = Atend.Base.Equipment.EJackPanelCell.AccessSelectByJackPanelCode(jm.Code);
                                            ////foreach (DataRow cell1 in CellsList.Rows)
                                            ////{
                                            ////    Atend.Base.Equipment.EJackPanelCell SelectedJackPanelmiddleCell = new Atend.Base.Equipment.EJackPanelCell();
                                            ////    SelectedJackPanelmiddleCell.XCode = new Guid(cell1["XCode"].ToString());
                                            ////    SelectedJackPanelmiddleCell.JackPanelCode = Convert.ToByte(cell1["JackPanelCode"].ToString());
                                            ////    SelectedJackPanelmiddleCell.ProductCode = Convert.ToByte(cell1["ProductCode"].ToString());
                                            ////    SelectedJackPanelmiddleCell.ProductType = Convert.ToByte(cell1["ProductType"].ToString());
                                            ////    SelectedJackPanelmiddleCell.Num = Convert.ToByte(cell1["CellNum"].ToString());
                                            ////    //cells.Add(SelectedJackPanelmiddleCell);

                                            ////}//


                                            if (MJCells.Count != 0)
                                            {
                                                //ed.WriteMessage("Middle jack panel productcode:{0}\n", Convert.ToInt32(groundPost.arMiddleJAckPAnel[i]));
                                                AcDrawMiddleJackPanel MyDrawMiddleJackPanel = new AcDrawMiddleJackPanel();
                                                //ed.WriteMessage("ForDelete1.CodeGuid : {0} \n", ForDelete1.CodeGuid);
                                                MyDrawMiddleJackPanel.NodeCode = ForDelete1.CodeGuid;
                                                MyDrawMiddleJackPanel.ParentCode = ForDelete1.ParentCode;
                                                MyDrawMiddleJackPanel.ProductCode = ForDelete1.ProductCode;
                                                MyDrawMiddleJackPanel.JackpanelCells = MJCells;
                                                MyDrawMiddleJackPanel.DrawMiddleJackPanel02(PostEntity);
                                            }
                                            PostEquipInserted.Remove(ForDelete1);
                                        }
                                        ForDelete1 = null;
                                    }

                                }//if (ForDelete != null)

                            }
                            #endregion

                        }
                        else
                        {
                            Conti = false;
                        }
                    }
                    else
                    {
                        Conti = false;
                    }

                }
                //_________________________________________
            }


        }

        public void DrawGroundPostUpdate()
        {

            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            double w, h;
            Entity PostEntity = null;
            Dictionary<Guid, Atend.Base.Equipment.EJackPanelCell> MJCells = new Dictionary<Guid, Atend.Base.Equipment.EJackPanelCell>();

            double MyScale = Atend.Base.Design.DProductProperties.AccessSelectBySoftwareCode((int)Atend.Control.Enum.ProductType.GroundPost).Scale;
            double MyCommentScale = Atend.Base.Design.DProductProperties.AccessSelectBySoftwareCode((int)Atend.Control.Enum.ProductType.GroundPost).CommentScale;

            Atend.Global.Design.frmPostSize FPS = new Atend.Global.Design.frmPostSize();
            if (Application.ShowModalDialog(FPS) == System.Windows.Forms.DialogResult.OK)
            {
                h = FPS.Tol;
                w = FPS.Arz;
                //________________________________________________
                // All data was getting well
                DrawGroundPostJig drawGroundPostJig = new DrawGroundPostJig(w, h, MyScale);
                bool Conti = true;

                while (Conti)
                {
                    PromptResult pr = ed.Drag(drawGroundPostJig);
                    if (pr.Status == PromptStatus.OK)
                    {
                        drawGroundPostJig.GetAngle = true;
                        pr = ed.Drag(drawGroundPostJig);
                        if (pr.Status == PromptStatus.OK)
                        {
                            Conti = false;
                            #region SaveDataHere

                            //if (SaveGroundPostData())
                            //{
                            //Draw Post
                            ObjectId oi = Atend.Global.Acad.UAcad.DrawEntityOnScreen(drawGroundPostJig.GetEntity(), Atend.Control.Enum.AutoCadLayerName.POST.ToString());
                            PostEntity = Atend.Global.Acad.UAcad.GetEntityByObjectID(oi);

                            Guid CurrentPostCodinDPost = Guid.Empty;
                            int ProductCode = 0;
                            PostEquips ForDelete = null;

                            foreach (PostEquips p in PostEquipInserted)
                            {
                                if (p.ProductType == (int)Atend.Control.Enum.ProductType.GroundPost)
                                {
                                    CurrentPostCodinDPost = p.CodeGuid;
                                    ProductCode = p.ProductCode;
                                    ForDelete = p;
                                }
                            }


                            if (ForDelete != null)
                            {
                                //ed.WriteMessage("GROUNDPOST found \n");
                                Atend.Base.Acad.AT_INFO postInfo = new Atend.Base.Acad.AT_INFO(oi);
                                postInfo.ParentCode = "";
                                postInfo.NodeCode = CurrentPostCodinDPost.ToString();
                                postInfo.NodeType = (int)Atend.Control.Enum.ProductType.GroundPost;
                                postInfo.ProductCode = ProductCode;
                                postInfo.Insert();

                                string comment = string.Format("Ground Post {0} KVA", eGroundPost.Capacity);
                                ObjectId DrawnText = Atend.Global.Acad.UAcad.DrawEntityOnScreen(Atend.Global.Acad.UAcad.WriteNote(comment, Atend.Global.Acad.UAcad.CenterOfEntity(drawGroundPostJig.GetEntity()), MyCommentScale), Atend.Control.Enum.AutoCadLayerName.GENERAL.ToString());

                                Atend.Base.Acad.AT_INFO textInfo = new Atend.Base.Acad.AT_INFO(DrawnText);
                                textInfo.ParentCode = CurrentPostCodinDPost.ToString();
                                textInfo.NodeCode = "";
                                textInfo.NodeType = (int)Atend.Control.Enum.ProductType.Comment;
                                textInfo.ProductCode = 0;
                                textInfo.Insert();

                                Atend.Base.Acad.AT_SUB PostSub = new Atend.Base.Acad.AT_SUB(oi);
                                PostSub.SubIdCollection.Add(DrawnText);
                                PostSub.Insert();

                                PostEquipInserted.Remove(ForDelete);

                                //Draw Week Jack

                                PostEquips ForDelete1 = null;
                                foreach (Atend.Base.Equipment.EJackPanelWeek jw in eJackPanelWeeks)
                                {
                                    //ed.WriteMessage("JW code :{0} type :{1} \n", jw.Code, (int)Atend.Control.Enum.ProductType.WeekJackPanel);
                                    foreach (PostEquips pes in PostEquipInserted)
                                    {
                                        //ed.WriteMessage("Type:{0} Code:{1} \n",pes.ProductType , pes.ProductCode);
                                        if (pes.ProductType == (int)Atend.Control.Enum.ProductType.WeekJackPanel && pes.ProductCode == jw.Code)
                                        {
                                            ForDelete1 = pes;
                                            // ed.WriteMessage("***\n");
                                        }
                                    }

                                    if (ForDelete1 != null)
                                    {
                                        //ed.WriteMessage("Week found \n");
                                        AcDrawWeekJackPanel ADWJ = new AcDrawWeekJackPanel();
                                        ADWJ.NodeCode = ForDelete1.CodeGuid;
                                        ADWJ.ParentCode = ForDelete1.ParentCode;
                                        ADWJ.ProductCode = ForDelete1.ProductCode;
                                        ADWJ.DrawWeekJackPanel(PostEntity, jw.FeederCount);
                                        PostEquipInserted.Remove(ForDelete1);
                                    }
                                    ForDelete1 = null;
                                }

                                //Draw Transformer
                                foreach (Atend.Base.Equipment.ETransformer tf in eTransformers)
                                {
                                    foreach (PostEquips pes in PostEquipInserted)
                                    {
                                        if (pes.ProductType == (int)Atend.Control.Enum.ProductType.Transformer && pes.ProductCode == tf.Code)
                                        {
                                            ForDelete1 = pes;
                                        }
                                    }

                                    if (ForDelete1 != null)
                                    {
                                        AcDrawTransformer ADT = new AcDrawTransformer();
                                        ADT.NodeCode = ForDelete1.CodeGuid;
                                        ADT.ParentCode = ForDelete1.ParentCode;
                                        ADT.ProductCode = ForDelete1.ProductCode;

                                        ADT.DrawTransformer(PostEntity);
                                        PostEquipInserted.Remove(ForDelete1);
                                    }
                                    ForDelete1 = null;
                                }


                                //draw middle jackpanel
                                foreach (Atend.Base.Equipment.EJAckPanel jm in eJackPanelMiddles)
                                {
                                    foreach (PostEquips pes in PostEquipInserted)
                                    {
                                        if (pes.ProductType == (int)Atend.Control.Enum.ProductType.MiddleJackPanel && pes.ProductCode == jm.Code)
                                        {
                                            ForDelete1 = pes;
                                        }
                                    }

                                    if (ForDelete1 != null)
                                    {
                                        //PostEquips ForDelete2 = null;
                                        foreach (PostEquips pes in PostEquipInserted)
                                        {
                                            if (pes.ProductType == (int)Atend.Control.Enum.ProductType.Cell && pes.ParentCode == ForDelete1.CodeGuid)
                                            {
                                                //ForDelete2 = pes;

                                                MJCells.Add(pes.CodeGuid, Atend.Base.Equipment.EJackPanelCell.AccessSelectByCode(pes.ProductCode));

                                            }
                                        }


                                        //List<Atend.Base.Equipment.EJackPanelCell> cells = new List<Atend.Base.Equipment.EJackPanelCell>();
                                        ////System.Data.DataTable CellsList = Atend.Base.Equipment.EJackPanelCell.AccessSelectByJackPanelCode(jm.Code);
                                        ////foreach (DataRow cell1 in CellsList.Rows)
                                        ////{
                                        ////    Atend.Base.Equipment.EJackPanelCell SelectedJackPanelmiddleCell = new Atend.Base.Equipment.EJackPanelCell();
                                        ////    SelectedJackPanelmiddleCell.XCode = new Guid(cell1["XCode"].ToString());
                                        ////    SelectedJackPanelmiddleCell.JackPanelCode = Convert.ToByte(cell1["JackPanelCode"].ToString());
                                        ////    SelectedJackPanelmiddleCell.ProductCode = Convert.ToByte(cell1["ProductCode"].ToString());
                                        ////    SelectedJackPanelmiddleCell.ProductType = Convert.ToByte(cell1["ProductType"].ToString());
                                        ////    SelectedJackPanelmiddleCell.Num = Convert.ToByte(cell1["CellNum"].ToString());
                                        ////    //cells.Add(SelectedJackPanelmiddleCell);

                                        ////}//


                                        if (MJCells.Count != 0)
                                        {
                                            //ed.WriteMessage("Middle jack panel productcode:{0}\n", Convert.ToInt32(groundPost.arMiddleJAckPAnel[i]));
                                            AcDrawMiddleJackPanel MyDrawMiddleJackPanel = new AcDrawMiddleJackPanel();
                                            MyDrawMiddleJackPanel.NodeCode = ForDelete1.CodeGuid;
                                            MyDrawMiddleJackPanel.ParentCode = ForDelete1.ParentCode;
                                            MyDrawMiddleJackPanel.ProductCode = ForDelete1.ProductCode;
                                            MyDrawMiddleJackPanel.JackpanelCells = MJCells;
                                            MyDrawMiddleJackPanel.DrawMiddleJackPanel02(PostEntity);
                                        }
                                        PostEquipInserted.Remove(ForDelete1);
                                    }
                                    ForDelete1 = null;
                                }

                            }//if (ForDelete != null)

                            //}
                            #endregion

                        }
                        else
                        {
                            Conti = false;
                        }
                    }
                    else
                    {
                        Conti = false;
                    }

                }
                //_________________________________________
            }


        }

        private bool __SaveGroundPostData()
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;


            //ed.WriteMessage("\n~~~~~~~~~~~~~ Start Save GroundPost Data ~~~~~~~~~~~~~~~~~~\n");
            OleDbConnection aConnection = new OleDbConnection(Atend.Control.ConnectionString.AccessCnString);
            OleDbTransaction aTransaction;
            //Atend.Base.Equipment.EGroundPost gp = Atend.Base.Equipment.EGroundPost.SelectByXCode(GroundPostXCode);
            try
            {
                aConnection.Open();
                aTransaction = aConnection.BeginTransaction();
                try
                {

                    if (!UseAccess)
                    {

                        if (!eGroundPost.AccessInsert(aTransaction, aConnection, true, true))
                        {
                            throw new System.Exception("eGroundPost.AccessInsert failed");
                        }

                        //////////////foreach (Atend.Base.Equipment.EJAckPanel selectedMiddleJack in eJackPanelMiddles)
                        //////////////{

                        //////////////    //ed.WriteMessage("1 \n");
                        //////////////    Atend.Base.Equipment.EBus CurrentBus = Atend.Base.Equipment.EBus.SelectByXCode(selectedMiddleJack.MasterProductXCode);
                        //////////////    if (CurrentBus.Code != -1)
                        //////////////    {
                        //////////////        if (!CurrentBus.AccessInsert(aTransaction, aConnection, true, true))
                        //////////////        {
                        //////////////            throw new System.Exception("CurrentBus.AccessInsert failed");
                        //////////////        }

                        //////////////    }
                        //////////////    else
                        //////////////    {
                        //////////////        throw new System.Exception("Current bus was not found");
                        //////////////    }



                        //////////////    selectedMiddleJack.MasterProductCode = CurrentBus.Code;

                        //////////////    if (!selectedMiddleJack.AccessInsert(aTransaction, aConnection, true, true))
                        //////////////    {
                        //////////////        throw new System.Exception("selectedMiddleJack.AccessInsert afiled");

                        //////////////    }
                        //////////////    else
                        //////////////    {
                        //////////////        //insert cell by xcode
                        //////////////        System.Data.DataTable CellsList = Atend.Base.Equipment.EJackPanelCell.SelectByJackPanelXCode(selectedMiddleJack.XCode);
                        //////////////        foreach (DataRow cell1 in CellsList.Rows)
                        //////////////        {


                        //////////////            Atend.Base.Equipment.ECell eCell = Atend.Base.Equipment.ECell.SelectByXCodeForDesign(new Guid(cell1["ProductXCode"].ToString()));
                        //////////////            if (!eCell.AccessInsert(aTransaction, aConnection, true, true))
                        //////////////            {
                        //////////////                throw new System.Exception("eCell.AccessInsert failed");
                        //////////////            }

                        //////////////            Atend.Base.Equipment.EJackPanelCell SelectedJackPanelmiddleCell = new Atend.Base.Equipment.EJackPanelCell();
                        //////////////            SelectedJackPanelmiddleCell.XCode = new Guid(cell1["XCode"].ToString());
                        //////////////            SelectedJackPanelmiddleCell.JackPanelCode = selectedMiddleJack.Code;

                        //////////////            SelectedJackPanelmiddleCell.ProductCode = eCell.Code;
                        //////////////            SelectedJackPanelmiddleCell.ProductType = Convert.ToByte(cell1["ProductType"].ToString());
                        //////////////            SelectedJackPanelmiddleCell.Num = Convert.ToByte(cell1["CellNum"].ToString());

                        //////////////            if (!SelectedJackPanelmiddleCell.AccessInsert(aTransaction, aConnection, true, true))
                        //////////////            {
                        //////////////                throw new System.Exception("SelectedJackPanelmiddleCell.AccessInsert \n");
                        //////////////            }
                        //////////////        }//
                        //////////////    }
                        //////////////}
                        //ed.WriteMessage("WJ inserted \n");


                        //////////foreach (Atend.Base.Equipment.EJackPanelWeek selecteweekjack in eJackPanelWeeks)
                        //////////{

                        //////////    Atend.Base.Equipment.EAutoKey_3p Key = Atend.Base.Equipment.EAutoKey_3p.SelectByXCode(selecteweekjack.AutoKey3pXCode);
                        //////////    if (Key.Code != -1)
                        //////////    {
                        //////////        if (!Key.AccessInsert(aTransaction, aConnection, true, true))
                        //////////        {
                        //////////            throw new System.Exception("Key.AccessInsert");
                        //////////        }
                        //////////    }
                        //////////    else
                        //////////    {
                        //////////        throw new System.Exception("Key was not found");
                        //////////    }


                        //////////    selecteweekjack.AutoKey3pCode = Key.Code;
                        //////////    if (!selecteweekjack.AccessInsert(aTransaction, aConnection, true, true))
                        //////////    {
                        //////////        throw new System.Exception("selectesweekjack.accessinsert failed");
                        //////////    }
                        //////////    else
                        //////////    {
                        //////////        //insert cell by xcode
                        //////////        System.Data.DataTable CellsList = Atend.Base.Equipment.EJackPanelWeekCell.SelectByJackPanelWeekXCode(selecteweekjack.XCode);

                        //////////        foreach (DataRow dr in CellsList.Rows)
                        //////////        {

                        //////////            Atend.Base.Equipment.EJackPanelWeekCell selectedjackpanelweekcell = new Atend.Base.Equipment.EJackPanelWeekCell();
                        //////////            selectedjackpanelweekcell.IsNightLight = Convert.ToBoolean(dr["IsNightLight"]);
                        //////////            selectedjackpanelweekcell.XCode = new Guid(dr["XCode"].ToString());
                        //////////            selectedjackpanelweekcell.JackPanelWeekCode = selecteweekjack.Code;
                        //////////            selectedjackpanelweekcell.Num = Convert.ToInt32(dr["Num"]);

                        //////////            if (!selectedjackpanelweekcell.AccessInsert(aTransaction, aConnection, true, true))
                        //////////            {
                        //////////                throw new System.Exception("selectedjackpanelweekcell.accessinsert \n");
                        //////////            }
                        //////////        }

                        //////////    }
                        //////////}

                        //ed.WriteMessage("{0} : {1}", _eGroundPost.Code, (int)Atend.Control.Enum.ProductType.GroundPost);
                        Atend.Base.Equipment.EContainerPackage CurrentPostContainerPack = Atend.Base.Equipment.EContainerPackage.AccessSelectByContainerCodeAndType(_eGroundPost.Code, (int)Atend.Control.Enum.ProductType.GroundPost, aTransaction, aConnection);
                        if (CurrentPostContainerPack.Code != -1)
                        {
                            //ed.WriteMessage("post was found \n");
                            eJackPanelMiddles.Clear();
                            eJackPanelWeeks.Clear();
                            eTransformers.Clear();
                            System.Data.DataTable PostProductPack = Atend.Base.Equipment.EProductPackage.AccessSelectByContainerPackageCode(CurrentPostContainerPack.Code, aTransaction, aConnection);
                            foreach (DataRow dr in PostProductPack.Rows)
                            {
                                if (Convert.ToInt32(dr["TableType"]) == (int)Atend.Control.Enum.ProductType.Transformer)
                                {
                                    //ed.WriteMessage("tRansformer was found \n");
                                    for (int i1 = 1; i1 <= Convert.ToInt32(dr["Count"]); i1++)
                                    {
                                        //ed.WriteMessage("i:{0} : {1} \n", i1, dr["ProductCode"]);
                                        Atend.Base.Equipment.ETransformer ET = Atend.Base.Equipment.ETransformer.AccessSelectByCode(Convert.ToInt32(dr["ProductCode"]), aTransaction, aConnection);
                                        if (ET.Code != -1)
                                        {
                                            eTransformers.Add(ET);
                                        }
                                    }
                                }


                                if (Convert.ToInt32(dr["TableType"]) == (int)Atend.Control.Enum.ProductType.WeekJackPanel)
                                {
                                    for (int i1 = 1; i1 <= Convert.ToInt32(dr["Count"]); i1++)
                                    {
                                        Atend.Base.Equipment.EJackPanelWeek EW = Atend.Base.Equipment.EJackPanelWeek.AccessSelectByCode(Convert.ToInt32(dr["ProductCode"]), aTransaction, aConnection);
                                        if (EW.Code != -1)
                                        {
                                            eJackPanelWeeks.Add(EW);
                                            //ed.WriteMessage("eJackPanelWeeks.code:{0}", EW.Code);
                                        }
                                    }
                                }


                                if (Convert.ToInt32(dr["TableType"]) == (int)Atend.Control.Enum.ProductType.MiddleJackPanel)
                                {
                                    for (int i1 = 1; i1 <= Convert.ToInt32(dr["Count"]); i1++)
                                    {
                                        Atend.Base.Equipment.EJAckPanel EM = Atend.Base.Equipment.EJAckPanel.AccessSelectByCode(Convert.ToInt32(dr["ProductCode"]), aTransaction, aConnection);
                                        if (EM.Code != -1)
                                        {
                                            eJackPanelMiddles.Add(EM);
                                        }
                                    }
                                }

                            }


                            //foreach (Atend.Base.Equipment.ETransformer SelectedTransformer in eTransformers)
                            //{
                            //    if (!SelectedTransformer.AccessInsert(aTransaction, aConnection, true, true))
                            //    {
                            //        throw new System.Exception("SelectedTransformer.AccessInsert failed");
                            //    }
                            //}
                        }
                    }// they were not use access
                    //ed.WriteMessage("T counter : {0} \n", eTransformers.Count);
                    //ed.WriteMessage("w counter : {0} \n", eJackPanelWeeks.Count);
                    //ed.WriteMessage("m counter : {0} \n", eJackPanelMiddles.Count);



                    //DPost
                    Atend.Base.Design.DPost dPost = new Atend.Base.Design.DPost();
                    dPost.Number = "N001";
                    dPost.ProductCode = eGroundPost.Code;
                    if (!dPost.AccessInsert(aTransaction, aConnection))
                    {
                        throw new System.Exception("dPost.AccessInsert failed");
                    }
                    //ed.WriteMessage("DPost inserted \n");
                    //DPackage
                    Atend.Base.Design.DPackage dPackPost = new Atend.Base.Design.DPackage();
                    dPackPost.NodeCode = dPost.Code;
                    dPackPost.Count = 1;
                    dPackPost.Type = (int)Atend.Control.Enum.ProductType.GroundPost; //gp.Type;//?? eGroundPost.Type;
                    dPackPost.ProductCode = eGroundPost.Code;
                    dPackPost.IsExistance = Existance;
                    dPackPost.ProjectCode = ProjectCode;
                    dPackPost.Number = "";
                    if (!dPackPost.AccessInsert(aTransaction, aConnection))
                    {
                        throw new System.Exception("dPackPost.AccessInsert failed");
                    }
                    else
                    {
                        // add to inserted
                        PostEquips pe = new PostEquips();
                        pe.CodeGuid = dPackPost.Code;
                        pe.ProductCode = dPackPost.ProductCode;
                        pe.ProductType = (int)Atend.Control.Enum.ProductType.GroundPost;
                        PostEquipInserted.Add(pe);
                    }
                    //insert middle jackpanel
                    int i = 0;
                    foreach (Atend.Base.Equipment.EJAckPanel EJP in eJackPanelMiddles)
                    {
                        Atend.Base.Design.DPackage dPack = new Atend.Base.Design.DPackage();
                        dPack.Count = 1;
                        dPack.Type = (int)Atend.Control.Enum.ProductType.MiddleJackPanel;
                        dPack.ProductCode = EJP.Code;
                        //ed.WriteMessage("dPack.ProductCode : {0} \n", dPack.ProductCode);
                        dPack.IsExistance = Convert.ToInt32(eJackPanelMiddleExistance[i]);
                        dPack.ProjectCode = Convert.ToInt32(eJackPanelMiddleProjectCode[i]);
                        dPack.ParentCode = dPackPost.Code;
                        dPack.Number = "";
                        if (!dPack.AccessInsert(aTransaction, aConnection))
                        {
                            throw new System.Exception("dPack.AccessInsert failed");
                        }
                        else
                        {

                            // add to inserted
                            PostEquips pe = new PostEquips();
                            pe.CodeGuid = dPack.Code;
                            pe.ProductCode = dPack.ProductCode;
                            pe.ProductType = (int)Atend.Control.Enum.ProductType.MiddleJackPanel;
                            pe.ParentCode = dPackPost.Code;
                            PostEquipInserted.Add(pe);

                            //insert cell by xcode

                            System.Data.DataTable CellsList = Atend.Base.Equipment.EJackPanelCell.AccessSelectByJackPanelCode(EJP.Code, aTransaction, aConnection);
                            foreach (DataRow dr in CellsList.Rows)
                            {

                                Atend.Base.Design.DPackage dPack1 = new Atend.Base.Design.DPackage();
                                dPack1.Count = 1;
                                dPack1.Type = (int)Atend.Control.Enum.ProductType.Cell;
                                dPack1.ProductCode = Convert.ToInt32(dr["ProductCode"]);
                                dPack1.IsExistance = Convert.ToInt32(eJackPanelMiddleExistance[i]);
                                dPack1.ProjectCode = Convert.ToInt32(eJackPanelMiddleProjectCode[i]);
                                dPack1.ParentCode = dPack.Code;
                                dPack1.Number = "";
                                if (!dPack1.AccessInsert(aTransaction, aConnection))
                                {
                                    throw new System.Exception("dPack1.AccessInsert failed");
                                }
                                else
                                {
                                    Atend.Base.Design.DCellStatus sCellStatus = new Atend.Base.Design.DCellStatus();
                                    sCellStatus.IsClosed = false;
                                    sCellStatus.JackPanelCode = dPack.Code;
                                    sCellStatus.CellCode = dPack1.Code;
                                    if (!sCellStatus.AccessInsert(aTransaction, aConnection))
                                    {
                                        throw new System.Exception("sCellStatus.AccessInsert failed");
                                    }
                                    // add to inserted
                                    pe = new PostEquips();
                                    pe.CodeGuid = dPack1.Code;
                                    pe.ProductCode = dPack1.ProductCode;
                                    pe.ProductType = (int)Atend.Control.Enum.ProductType.Cell;
                                    pe.ParentCode = dPack.Code;
                                    PostEquipInserted.Add(pe);
                                }
                            }
                        }
                        i++;
                    }
                    //insert transformer
                    int j = 0;
                    foreach (Atend.Base.Equipment.ETransformer transformer in eTransformers)
                    {
                        Atend.Base.Design.DPackage transformerPackage = new Atend.Base.Design.DPackage();
                        transformerPackage.Count = 1;
                        transformerPackage.IsExistance = Convert.ToInt32(eTransformerExistance[j]);
                        transformerPackage.ProjectCode = Convert.ToInt32(eTransformerProjectCode[j]);
                        transformerPackage.ParentCode = dPackPost.Code;
                        transformerPackage.ProductCode = transformer.Code;
                        transformerPackage.Type = (int)Atend.Control.Enum.ProductType.Transformer;
                        transformerPackage.Number = "";
                        if (!transformerPackage.AccessInsert(aTransaction, aConnection))
                        {
                            throw new System.Exception("transformerPackage.AccessInsert failed");
                        }
                        else
                        {
                            // add to inserted
                            PostEquips pe = new PostEquips();
                            pe.CodeGuid = transformerPackage.Code;
                            pe.ProductCode = transformerPackage.ProductCode;
                            pe.ProductType = (int)Atend.Control.Enum.ProductType.Transformer;
                            pe.ParentCode = dPackPost.Code;
                            PostEquipInserted.Add(pe);
                        }
                        j++;
                    }
                    //insert weekJackpanel
                    j = 0;
                    foreach (Atend.Base.Equipment.EJackPanelWeek jackpanelweek in eJackPanelWeeks)
                    {
                        Atend.Base.Design.DPackage jackpanelweekPackage = new Atend.Base.Design.DPackage();
                        jackpanelweekPackage.Count = 1;
                        jackpanelweekPackage.IsExistance = Convert.ToInt32(eJackPanelWeekExistance[j]);
                        jackpanelweekPackage.ProjectCode = Convert.ToInt32(eJackPanelWeekProjectCode[j]);
                        jackpanelweekPackage.ParentCode = dPackPost.Code;
                        jackpanelweekPackage.ProductCode = jackpanelweek.Code;
                        jackpanelweekPackage.Type = (int)Atend.Control.Enum.ProductType.WeekJackPanel;
                        jackpanelweekPackage.Number = "";
                        if (!jackpanelweekPackage.AccessInsert(aTransaction, aConnection))
                        {
                            throw new System.Exception("jackpanelweekPackage.AccessInsert failed");
                        }
                        else
                        {
                            // add to inserted
                            PostEquips pe = new PostEquips();
                            pe.CodeGuid = jackpanelweekPackage.Code;
                            pe.ProductCode = jackpanelweekPackage.ProductCode;
                            pe.ProductType = (int)Atend.Control.Enum.ProductType.WeekJackPanel;
                            pe.ParentCode = dPackPost.Code;
                            PostEquipInserted.Add(pe);
                        }
                        j++;
                    }
                    ////////foreach (PostEquips p in PostEquipInserted)
                    ////////{
                    ////////    ed.WriteMessage("~~~~~~~~~~~~~~~~~~~~~~~~~~\n");
                    ////////    ed.WriteMessage("CodeGuid:{0}\nParentCode:{1}\nProductCode:{2}\nProductType:{3}\n", p.CodeGuid, p.ParentCode, p.ProductCode, p.ProductType);
                    ////////    ed.WriteMessage("~~~~~~~~~~~~~~~~~~~~~~~~~~\n");
                    ////////}

                }
                catch (System.Exception ex1)
                {
                    ed.WriteMessage("ERROR SaveGroundPostData: 02 {0} \n", ex1.Message);
                    aTransaction.Rollback();
                    aConnection.Close();
                    return false;
                }
            }
            catch (System.Exception ex)
            {
                ed.WriteMessage("ERROR SaveGroundPostData: 01 {0} \n", ex.Message);
                aConnection.Close();
                return false;
            }

            aTransaction.Commit();
            aConnection.Close();


            #region Not to Get Green

            Atend.Base.Acad.AcadGlobal.GroundPostData.UseAccess = true;
            UseAccess = true;

            #endregion

            //ed.WriteMessage("~~~~~~~~~~~~~ End Save GroundPost Data ~~~~~~~~~~~~~~~~~~\n");
            return true;


        }

        private bool SaveGroundPostData02()
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;


            //ed.WriteMessage("\n~~~~~~~~~~~~~ Start Save GroundPost Data ~~~~~~~~~~~~~~~~~~\n");
            OleDbConnection aConnection = new OleDbConnection(Atend.Control.ConnectionString.AccessCnString);
            OleDbTransaction aTransaction;
            //Atend.Base.Equipment.EGroundPost gp = Atend.Base.Equipment.EGroundPost.SelectByXCode(GroundPostXCode);
            try
            {
                aConnection.Open();
                aTransaction = aConnection.BeginTransaction();
                try
                {

                    if (!UseAccess)
                    {
                        if (!eGroundPost.AccessInsert(aTransaction, aConnection, true, true))
                        {
                            throw new System.Exception("eGroundPost.AccessInsert failed");
                        }

                        //ed.WriteMessage("{0} : {1}", _eGroundPost.Code, (int)Atend.Control.Enum.ProductType.GroundPost);
                        Atend.Base.Equipment.EContainerPackage CurrentPostContainerPack = Atend.Base.Equipment.EContainerPackage.AccessSelectByContainerCodeAndType(_eGroundPost.Code, (int)Atend.Control.Enum.ProductType.GroundPost, aTransaction, aConnection);
                        if (CurrentPostContainerPack.Code != -1)
                        {
                            //ed.WriteMessage("post was found \n");
                            eJackPanelMiddles.Clear();
                            eJackPanelWeeks.Clear();
                            eTransformers.Clear();
                            System.Data.DataTable PostProductPack = Atend.Base.Equipment.EProductPackage.AccessSelectByContainerPackageCode(CurrentPostContainerPack.Code, aTransaction, aConnection);
                            foreach (DataRow dr in PostProductPack.Rows)
                            {
                                if (Convert.ToInt32(dr["TableType"]) == (int)Atend.Control.Enum.ProductType.Transformer)
                                {
                                    //ed.WriteMessage("tRansformer was found \n");
                                    for (int i1 = 1; i1 <= Convert.ToInt32(dr["Count"]); i1++)
                                    {
                                        //ed.WriteMessage("i:{0} : {1} \n", i1, dr["ProductCode"]);
                                        Atend.Base.Equipment.ETransformer ET = Atend.Base.Equipment.ETransformer.AccessSelectByCode(Convert.ToInt32(dr["ProductCode"]), aTransaction, aConnection);
                                        if (ET.Code != -1)
                                        {
                                            eTransformers.Add(ET);
                                        }
                                    }
                                }


                                if (Convert.ToInt32(dr["TableType"]) == (int)Atend.Control.Enum.ProductType.WeekJackPanel)
                                {
                                    for (int i1 = 1; i1 <= Convert.ToInt32(dr["Count"]); i1++)
                                    {
                                        Atend.Base.Equipment.EJackPanelWeek EW = Atend.Base.Equipment.EJackPanelWeek.AccessSelectByCode(Convert.ToInt32(dr["ProductCode"]), aTransaction, aConnection);
                                        if (EW.Code != -1)
                                        {
                                            eJackPanelWeeks.Add(EW);
                                            //ed.WriteMessage("eJackPanelWeeks.code:{0}", EW.Code);
                                        }
                                    }
                                }


                                if (Convert.ToInt32(dr["TableType"]) == (int)Atend.Control.Enum.ProductType.MiddleJackPanel)
                                {
                                    for (int i1 = 1; i1 <= Convert.ToInt32(dr["Count"]); i1++)
                                    {
                                        Atend.Base.Equipment.EJAckPanel EM = Atend.Base.Equipment.EJAckPanel.AccessSelectByCode(Convert.ToInt32(dr["ProductCode"]), aTransaction, aConnection);
                                        if (EM.Code != -1)
                                        {
                                            eJackPanelMiddles.Add(EM);
                                        }
                                    }
                                }

                            }

                        }
                    }// they were not use access

                    //DPost
                    Atend.Base.Design.DPost dPost = new Atend.Base.Design.DPost();
                    dPost.Number = "N001";
                    dPost.ProductCode = eGroundPost.Code;
                    if (!dPost.AccessInsert(aTransaction, aConnection))
                    {
                        throw new System.Exception("dPost.AccessInsert failed");
                    }
                    //ed.WriteMessage("DPost inserted \n");
                    //DPackage
                    Atend.Base.Design.DPackage dPackPost = new Atend.Base.Design.DPackage();
                    dPackPost.NodeCode = dPost.Code;
                    dPackPost.Count = 1;
                    dPackPost.Type = (int)Atend.Control.Enum.ProductType.GroundPost; //gp.Type;//?? eGroundPost.Type;
                    dPackPost.ProductCode = eGroundPost.Code;
                    dPackPost.IsExistance = Existance;
                    dPackPost.ProjectCode = ProjectCode;
                    dPackPost.Number = "";
                    if (!dPackPost.AccessInsert(aTransaction, aConnection))
                    {
                        throw new System.Exception("dPackPost.AccessInsert failed");
                    }
                    else
                    {
                        // add to inserted
                        PostEquips pe = new PostEquips();
                        pe.CodeGuid = dPackPost.Code;
                        pe.ProductCode = dPackPost.ProductCode;
                        pe.ProductType = (int)Atend.Control.Enum.ProductType.GroundPost;
                        PostEquipInserted.Add(pe);
                    }
                    //insert middle jackpanel
                    int i = 0;
                    foreach (Atend.Base.Equipment.EJAckPanel EJP in eJackPanelMiddles)
                    {
                        Atend.Base.Design.DPackage dPack = new Atend.Base.Design.DPackage();
                        dPack.Count = 1;
                        dPack.Type = (int)Atend.Control.Enum.ProductType.MiddleJackPanel;
                        dPack.ProductCode = EJP.Code;
                        //ed.WriteMessage("dPack.ProductCode : {0} \n", dPack.ProductCode);
                        dPack.IsExistance = Convert.ToInt32(eJackPanelMiddleExistance[i]);
                        dPack.ProjectCode = Convert.ToInt32(eJackPanelMiddleProjectCode[i]);
                        dPack.ParentCode = dPackPost.Code;
                        dPack.Number = "";
                        if (!dPack.AccessInsert(aTransaction, aConnection))
                        {
                            throw new System.Exception("dPack.AccessInsert failed");
                        }
                        else
                        {

                            // add to inserted
                            PostEquips pe = new PostEquips();
                            pe.CodeGuid = dPack.Code;
                            pe.ProductCode = dPack.ProductCode;
                            pe.ProductType = (int)Atend.Control.Enum.ProductType.MiddleJackPanel;
                            pe.ParentCode = dPackPost.Code;
                            PostEquipInserted.Add(pe);

                            //insert cell by xcode

                            System.Data.DataTable CellsList = Atend.Base.Equipment.EJackPanelCell.AccessSelectByJackPanelCode(EJP.Code, aTransaction, aConnection);
                            foreach (DataRow dr in CellsList.Rows)
                            {

                                Atend.Base.Design.DPackage dPack1 = new Atend.Base.Design.DPackage();
                                dPack1.Count = 1;
                                dPack1.Type = (int)Atend.Control.Enum.ProductType.Cell;
                                dPack1.ProductCode = Convert.ToInt32(dr["ProductCode"]);
                                dPack1.IsExistance = Convert.ToInt32(eJackPanelMiddleExistance[i]);
                                dPack1.ProjectCode = Convert.ToInt32(eJackPanelMiddleProjectCode[i]);
                                dPack1.ParentCode = dPack.Code;
                                dPack1.Number = "";
                                if (!dPack1.AccessInsert(aTransaction, aConnection))
                                {
                                    throw new System.Exception("dPack1.AccessInsert failed");
                                }
                                else
                                {
                                    Atend.Base.Design.DCellStatus sCellStatus = new Atend.Base.Design.DCellStatus();
                                    sCellStatus.IsClosed = false;
                                    sCellStatus.JackPanelCode = dPack.Code;
                                    sCellStatus.CellCode = dPack1.Code;
                                    if (!sCellStatus.AccessInsert(aTransaction, aConnection))
                                    {
                                        throw new System.Exception("sCellStatus.AccessInsert failed");
                                    }
                                    // add to inserted
                                    pe = new PostEquips();
                                    pe.CodeGuid = dPack1.Code;
                                    pe.ProductCode = dPack1.ProductCode;
                                    pe.ProductType = (int)Atend.Control.Enum.ProductType.Cell;
                                    pe.ParentCode = dPack.Code;
                                    PostEquipInserted.Add(pe);
                                }
                            }
                        }
                        i++;
                    }
                    //insert transformer
                    int j = 0;
                    foreach (Atend.Base.Equipment.ETransformer transformer in eTransformers)
                    {
                        Atend.Base.Design.DPackage transformerPackage = new Atend.Base.Design.DPackage();
                        transformerPackage.Count = 1;
                        transformerPackage.IsExistance = Convert.ToInt32(eTransformerExistance[j]);
                        transformerPackage.ProjectCode = Convert.ToInt32(eTransformerProjectCode[j]);
                        transformerPackage.ParentCode = dPackPost.Code;
                        transformerPackage.ProductCode = transformer.Code;
                        transformerPackage.Type = (int)Atend.Control.Enum.ProductType.Transformer;
                        transformerPackage.Number = "";
                        if (!transformerPackage.AccessInsert(aTransaction, aConnection))
                        {
                            throw new System.Exception("transformerPackage.AccessInsert failed");
                        }
                        else
                        {
                            // add to inserted
                            PostEquips pe = new PostEquips();
                            pe.CodeGuid = transformerPackage.Code;
                            pe.ProductCode = transformerPackage.ProductCode;
                            pe.ProductType = (int)Atend.Control.Enum.ProductType.Transformer;
                            pe.ParentCode = dPackPost.Code;
                            PostEquipInserted.Add(pe);
                        }
                        j++;
                    }
                    //insert weekJackpanel
                    j = 0;
                    foreach (Atend.Base.Equipment.EJackPanelWeek jackpanelweek in eJackPanelWeeks)
                    {
                        Atend.Base.Design.DPackage jackpanelweekPackage = new Atend.Base.Design.DPackage();
                        jackpanelweekPackage.Count = 1;
                        jackpanelweekPackage.IsExistance = Convert.ToInt32(eJackPanelWeekExistance[j]);
                        jackpanelweekPackage.ProjectCode = Convert.ToInt32(eJackPanelWeekProjectCode[j]);
                        jackpanelweekPackage.ParentCode = dPackPost.Code;
                        jackpanelweekPackage.ProductCode = jackpanelweek.Code;
                        jackpanelweekPackage.Type = (int)Atend.Control.Enum.ProductType.WeekJackPanel;
                        jackpanelweekPackage.Number = "";
                        if (!jackpanelweekPackage.AccessInsert(aTransaction, aConnection))
                        {
                            throw new System.Exception("jackpanelweekPackage.AccessInsert failed");
                        }
                        else
                        {
                            // add to inserted
                            PostEquips pe = new PostEquips();
                            pe.CodeGuid = jackpanelweekPackage.Code;
                            pe.ProductCode = jackpanelweekPackage.ProductCode;
                            pe.ProductType = (int)Atend.Control.Enum.ProductType.WeekJackPanel;
                            pe.ParentCode = dPackPost.Code;
                            PostEquipInserted.Add(pe);
                        }
                        j++;
                    }
                    ////////foreach (PostEquips p in PostEquipInserted)
                    ////////{
                    ////////    ed.WriteMessage("~~~~~~~~~~~~~~~~~~~~~~~~~~\n");
                    ////////    ed.WriteMessage("CodeGuid:{0}\nParentCode:{1}\nProductCode:{2}\nProductType:{3}\n", p.CodeGuid, p.ParentCode, p.ProductCode, p.ProductType);
                    ////////    ed.WriteMessage("~~~~~~~~~~~~~~~~~~~~~~~~~~\n");
                    ////////}

                }
                catch (System.Exception ex1)
                {
                    ed.WriteMessage("ERROR SaveGroundPostData: 02 {0} \n", ex1.Message);
                    aTransaction.Rollback();
                    aConnection.Close();
                    return false;
                }
            }
            catch (System.Exception ex)
            {
                ed.WriteMessage("ERROR SaveGroundPostData: 01 {0} \n", ex.Message);
                aConnection.Close();
                return false;
            }

            aTransaction.Commit();
            aConnection.Close();


            #region Not to Get Green

            Atend.Base.Acad.AcadGlobal.GroundPostData.UseAccess = true;
            UseAccess = true;

            #endregion

            //ed.WriteMessage("~~~~~~~~~~~~~ End Save GroundPost Data ~~~~~~~~~~~~~~~~~~\n");
            return true;


        }

        //private static void ShieldPicture(Point2dCollection Points)
        //{
        //    LineSegment3d ls1 = new LineSegment3d(new Point3d(Points[0].X, Points[0].Y, 0), new Point3d(Points[2].X, Points[2].Y, 0));
        //    Point3d CenterPoint = ls1.MidPoint;
        //    ls1 = new LineSegment3d(new Point3d(Points[0].X, Points[0].Y, 0), new Point3d(Points[1].X, Points[1].Y, 0));
        //    Circle MyCir = new Circle();
        //    MyCir.Radius = (ls1.Length / 3) / 2;
        //    MyCir.Center = CenterPoint;
        //    MyCir.Normal = Vector3d.ZAxis;
        //    Atend.Global.Acad.UAcad.DrawEntityOnScreen(MyCir, Atend.Control.Enum.AutoCadLayerName.SHELL.ToString());
        //}


        //private static ObjectId CreateWhiteBack(ObjectId oi , Transaction _transaction)
        //{
        //    Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
        //    Document doc = Application.DocumentManager.MdiActiveDocument;
        //    Database db = doc.Database;
        //    //Transaction tr = db.TransactionManager.StartTransaction();
        //    Transaction tr = _transaction;
        //    ObjectId hatId = ObjectId.Null;
        //    try
        //    {
        //        BlockTable bt = (BlockTable)tr.GetObject(doc.Database.BlockTableId, OpenMode.ForRead);
        //        ed.WriteMessage("1\n");
        //        BlockTableRecord btr = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);
        //        ed.WriteMessage("2\n");
        //        Hatch hat = new Hatch();
        //        ed.WriteMessage("3\n");
        //        hat.SetDatabaseDefaults();
        //        ed.WriteMessage("4\n");
        //        hat.HatchObjectType = HatchObjectType.GradientObject;
        //        ed.WriteMessage("5\n");
        //        hat.SetGradient(GradientPatternType.PreDefinedGradient, "LINEAR");
        //        ed.WriteMessage("6\n");
        //        hat.GradientOneColorMode = false;
        //        ed.WriteMessage("7\n");
        //        GradientColor[] gcs = new GradientColor[2];
        //        ed.WriteMessage("8\n");
        //        gcs[0] = new GradientColor(Color.FromRgb(255, 255, 255), 0);
        //        ed.WriteMessage("9\n");
        //        gcs[1] = new GradientColor(Color.FromRgb(255, 255, 255), 1);
        //        ed.WriteMessage("10\n");
        //        hat.SetGradientColors(gcs);
        //        ed.WriteMessage("11\n");
        //        hatId = btr.AppendEntity(hat);
        //        ed.WriteMessage("12\n");
        //        tr.AddNewlyCreatedDBObject(hat, true);
        //        ed.WriteMessage("13\n");
        //        ObjectIdCollection ids = new ObjectIdCollection();
        //        ed.WriteMessage("14\n");
        //        ids.Add(oi);
        //        ed.WriteMessage("15\n");
        //        hat.Associative = true;
        //        ed.WriteMessage("16\n");
        //        hat.AppendLoop(HatchLoopTypes.Default, ids);
        //        ed.WriteMessage("17\n");
        //        hat.EvaluateHatch(true);
        //        ed.WriteMessage("18\n");
        //        //tr.Commit();

        //    }
        //    catch (System.Exception ex)
        //    {
        //        ed.WriteMessage("ERROR Wipeout : {0} \n", ex.Message);
        //    }
        //    return hatId;
        //}

        public static void DrawShield()
        {
            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            Dictionary<string, Point2dCollection> MyDic = new Dictionary<string, Point2dCollection>();
            Dictionary<string, ObjectId> MyDic1 = new Dictionary<string, ObjectId>();
            try
            {
                //using (DocumentLock dlock = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.LockDocument())
                //{
                TypedValue[] tvs = new TypedValue[] { new TypedValue((int)DxfCode.LayerName, "POST") };
                SelectionFilter sf = new SelectionFilter(tvs);
                PromptSelectionResult psr = ed.SelectAll(sf);

                if (psr.Value != null)
                {

                    ObjectId[] ids = psr.Value.GetObjectIds();
                    //ObjectIdCollection OIC = new ObjectIdCollection();
                    foreach (ObjectId oi in ids)
                    {
                        Atend.Base.Acad.AT_INFO PostInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oi);
                        if (PostInfo.ParentCode != "NONE" && PostInfo.NodeType == (int)Atend.Control.Enum.ProductType.GroundPost)
                        {
                            //ed.WriteMessage("Ground post found on screen \n");
                            Entity ent = Atend.Global.Acad.UAcad.GetEntityByObjectID(oi);
                            Polyline pl = ent as Polyline;
                            if (pl != null)
                            {
                                Point2dCollection pts = new Point2dCollection(5); //p2;
                                for (int i = 0; i < pl.NumberOfVertices; i++)
                                {
                                    Point2d p = pl.GetPoint2dAt(i);
                                    double a = p.X * 1;
                                    double b = p.Y * 1;
                                    //ed.WriteMessage("x:{0},{1} \n", a, b);
                                    pts.Add(new Point2d(a, b));
                                }
                                MyDic.Add(PostInfo.NodeCode, pts);
                                MyDic1.Add(PostInfo.NodeCode, oi);
                            }
                        }
                        //ed.WriteMessage("--------------------------------------- \n");
                    }
                    ids = null;


                    //int counter = 1;
                    foreach (string a in MyDic.Keys)
                    {
                        //ed.WriteMessage("   ....   >>>>   counter:{0} \n", counter);
                        Point2dCollection p = new Point2dCollection();
                        MyDic.TryGetValue(a, out p);
                        Atend.Global.Acad.Global.CreateWhiteBack(p);
                        //counter++;
                    }

                    foreach (string NodeCode in MyDic1.Keys)
                    {
                        ObjectId Objectid = ObjectId.Null;
                        MyDic1.TryGetValue(NodeCode, out Objectid);
                        if (Objectid != ObjectId.Null)
                        {
                            Atend.Base.Design.DPackage PostPack = Atend.Base.Design.DPackage.AccessSelectByCode(new Guid(NodeCode));
                            if (PostPack.Code != Guid.Empty)
                            {
                                //ed.WriteMessage("Ground post found in dpacklage \n");
                                Atend.Base.Base.BEquipStatus ES = Atend.Base.Base.BEquipStatus.SelectByACode(PostPack.IsExistance);
                                Atend.Base.Equipment.EGroundPost EG = Atend.Base.Equipment.EGroundPost.AccessSelectByCode(PostPack.ProductCode);
                                if (EG.Code != -1)
                                {
                                    //ed.WriteMessage("Ground post found in e ground post \n");
                                    /////////////////////////////////Atend.Global.Acad.Global.CreateWhiteBack(oi);
                                    //ed.WriteMessage("1\n");
                                    if (EG.GroundType == 0)
                                    {
                                        //روزمینی
                                        //ed.WriteMessage("2\n");
                                        if (EG.AdvanceType == 2)
                                        {
                                            //کیوسک
                                            if (ES.Name.IndexOf("موجود") != -1)
                                            {
                                                //ed.WriteMessage("MOJOOD \n");
                                                ShieldForGroundType3(true, Objectid);
                                            }
                                            else
                                            {
                                                //ed.WriteMessage("PISHNAHADI \n");
                                                ShieldForGroundType3(false, Objectid);
                                            }

                                        }
                                        else
                                        {
                                            //ed.WriteMessage("3\n");
                                            if (ES.Name.IndexOf("موجود") != -1)
                                            {
                                                //ed.WriteMessage("MOJOOD \n");
                                                ShieldForGroundType1(true, Objectid);
                                            }
                                            else
                                            {
                                                //ed.WriteMessage("PISHANAHADI \n");
                                                //ed.WriteMessage("4\n");
                                                ShieldForGroundType1(false, Objectid);
                                            }
                                        }
                                    }
                                    else if (EG.GroundType == 1)
                                    {
                                        //زیرزمینی
                                        if (EG.AdvanceType == 2)
                                        {
                                            //کیوسک
                                            if (ES.Name.IndexOf("موجود") != -1)
                                            {
                                                //ed.WriteMessage("MOJOOD \n");
                                                ShieldForGroundType3(true, Objectid);
                                            }
                                            else
                                            {
                                                //ed.WriteMessage("PISHNAHADI \n");
                                                ShieldForGroundType3(false, Objectid);
                                            }
                                        }
                                        else
                                        {
                                            if (ES.Name.IndexOf("موجود") != -1)
                                            {
                                                //ed.WriteMessage("MOJOOD \n");
                                                ShieldForGroundType2(true, Objectid);
                                            }
                                            else
                                            {
                                                //ed.WriteMessage("PISHANAHADI \n");
                                                ShieldForGroundType2(false, Objectid);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    Atend.Global.Acad.Notification notification = new Atend.Global.Acad.Notification();
                                    notification.Title = "بروز خطا";
                                    notification.Msg = "مشخصات فنی یکی از پست های زمینی یافت نشد";
                                    notification.infoCenterBalloon();
                                    throw new System.Exception("Post was not in EGround Post");

                                }
                            }
                            else
                            {
                                Atend.Global.Acad.Notification notification = new Atend.Global.Acad.Notification();
                                notification.Title = "بروز خطا";
                                notification.Msg = "اطلاعات یکی از پست های زمینی در پایگاه داده یافت نشد";
                                notification.infoCenterBalloon();
                                throw new System.Exception("Post was not in DPackage Post");

                            }
                        }
                    }
                    ed.WriteMessage("PostShield finished \n");
                }
            }
            catch (System.Exception ex)
            {
                ed.WriteMessage("ERROR :{0} \n", ex.Message);
            }

        }

        /// <summary>
        /// روزمینی
        /// </summary>
        /// <param name="IsExist"></param>
        private static void ShieldForGroundType1(bool IsExist, ObjectId oi)
        {

            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            //Document doc = Application.DocumentManager.MdiActiveDocument;
            //Database db = doc.Database;
            //Transaction tr = db.TransactionManager.StartTransaction();
            ObjectId hatId = ObjectId.Null;
            ObjectIdCollection OIC = new ObjectIdCollection();
            Polyline pl = Atend.Global.Acad.UAcad.GetEntityByObjectID(oi) as Polyline;
            try
            {
                if (pl != null)
                {
                    //ed.WriteMessage("5\n");
                    LineSegment3d ls = new LineSegment3d(pl.GetPoint3dAt(0), pl.GetPoint3dAt(1));
                    Point3d CenterPoint = Atend.Global.Acad.UAcad.CenterOfEntity(Atend.Global.Acad.UAcad.GetEntityByObjectID(oi));
                    Circle c = new Circle(CenterPoint, new Vector3d(0, 0, 1), ls.Length / 3);
                    OIC.Add(Atend.Global.Acad.UAcad.DrawEntityOnScreen(c, Atend.Control.Enum.AutoCadLayerName.SHELL.ToString()));
                    //ed.WriteMessage("ro zamini finished \n");
                    if (IsExist)
                    {
                        try
                        {
                            Transaction tr = doc.TransactionManager.StartTransaction();
                            using (tr)
                            {
                                // Check the entity is a closed curve
                                //DBObject obj = tr.GetObject(CurrentPoleOI, OpenMode.ForRead);
                                //Curve cur = obj as Curve;
                                //if (cur != null && cur.Closed == false)
                                //{
                                //    //ed.WriteMessage("\nLoop must be a closed curve.");
                                //}
                                //else
                                //{
                                // We'll add the hatch to the model space
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
                                btr.AppendEntity(hat);
                                tr.AddNewlyCreatedDBObject(hat, true);
                                // Add the hatch loop and complete the hatch
                                ObjectIdCollection ids = new ObjectIdCollection();
                                ids.Add(c.ObjectId);
                                hat.Associative = true;
                                hat.AppendLoop(HatchLoopTypes.Default, ids);
                                hat.EvaluateHatch(true);
                                tr.Commit();
                                //}
                            }
                        }
                        catch (System.Exception ex)
                        {
                            ed.WriteMessage("ERROR Wipeout : {0} \n", ex.Message);
                        }
                        //OIC.Add(hatId);
                        //ed.WriteMessage("ro zamini finished  with exist\n");
                    }//if (IsExist)
                }
                //ed.WriteMessage("6\n");
            }
            catch (System.Exception ex)
            {
                ed.WriteMessage("ERROR AD : {0}", ex.Message);
            }
        }

        /// <summary>
        /// زیرزمینی
        /// </summary>
        /// <param name="IsExist"></param>
        private static void ShieldForGroundType2(bool IsExist, ObjectId oi)
        {
            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            //Document doc = Application.DocumentManager.MdiActiveDocument;
            //Database db = doc.Database;
            //Transaction tr = db.TransactionManager.StartTransaction();
            ObjectId hatId = ObjectId.Null;
            ObjectIdCollection OIC = new ObjectIdCollection();
            Polyline pl = Atend.Global.Acad.UAcad.GetEntityByObjectID(oi) as Polyline;
            if (pl != null)
            {
                LineSegment3d ls = new LineSegment3d(pl.GetPoint3dAt(0), pl.GetPoint3dAt(1));
                Point3d CenterPoint = Atend.Global.Acad.UAcad.CenterOfEntity(Atend.Global.Acad.UAcad.GetEntityByObjectID(oi));
                Circle c = new Circle(CenterPoint, new Vector3d(0, 0, 1), ls.Length / 3);
                OIC.Add(Atend.Global.Acad.UAcad.DrawEntityOnScreen(c, Atend.Control.Enum.AutoCadLayerName.SHELL.ToString()));
                Line l = new Line(pl.GetPoint3dAt(0), CenterPoint);
                Line l2 = new Line(pl.GetPoint3dAt(1), CenterPoint);
                OIC.Add(Atend.Global.Acad.UAcad.DrawEntityOnScreen(l, Atend.Control.Enum.AutoCadLayerName.SHELL.ToString()));
                OIC.Add(Atend.Global.Acad.UAcad.DrawEntityOnScreen(l2, Atend.Control.Enum.AutoCadLayerName.SHELL.ToString()));
                //ed.WriteMessage("zir zamini finished \n");
                if (IsExist)
                {
                    try
                    {
                        Transaction tr = doc.TransactionManager.StartTransaction();
                        using (tr)
                        {
                            // Check the entity is a closed curve
                            //DBObject obj = tr.GetObject(CurrentPoleOI, OpenMode.ForRead);
                            //Curve cur = obj as Curve;
                            //if (cur != null && cur.Closed == false)
                            //{
                            //    //ed.WriteMessage("\nLoop must be a closed curve.");
                            //}
                            //else
                            //{
                            // We'll add the hatch to the model space
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
                            btr.AppendEntity(hat);
                            tr.AddNewlyCreatedDBObject(hat, true);
                            // Add the hatch loop and complete the hatch
                            ObjectIdCollection ids = new ObjectIdCollection();
                            ids.Add(c.ObjectId);
                            hat.Associative = true;
                            hat.AppendLoop(HatchLoopTypes.Default, ids);
                            hat.EvaluateHatch(true);
                            tr.Commit();
                            //}
                        }
                    }
                    catch (System.Exception ex)
                    {
                        ed.WriteMessage("ERROR Wipeout : {0} \n", ex.Message);
                    }
                }
            }
        }

        /// <summary>
        /// کیوسک
        /// </summary>
        /// <param name="IsExist"></param>
        private static void ShieldForGroundType3(bool IsExist, ObjectId oi)
        {
            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            //Document doc = Application.DocumentManager.MdiActiveDocument;
            //Database db = doc.Database;
            //Transaction tr = db.TransactionManager.StartTransaction();
            ObjectId hatId = ObjectId.Null;
            ObjectIdCollection OIC = new ObjectIdCollection();
            Polyline pl = Atend.Global.Acad.UAcad.GetEntityByObjectID(oi) as Polyline;
            if (pl != null)
            {
                LineSegment3d ls = new LineSegment3d(pl.GetPoint3dAt(0), pl.GetPoint3dAt(1));
                LineSegment3d ls1 = new LineSegment3d(pl.GetPoint3dAt(1), pl.GetPoint3dAt(2));
                Circle c;
                if (ls.Length < ls1.Length)
                {
                    Point3d CenterPoint = Atend.Global.Acad.UAcad.CenterOfEntity(Atend.Global.Acad.UAcad.GetEntityByObjectID(oi));
                    c = new Circle(CenterPoint, new Vector3d(0, 0, 1), ls.Length / 3);

                    Vector3d Vect1 = ls.EndPoint - ls.StartPoint, norm1 = Vect1.GetNormal();
                    double Length = Vect1.Length / 2;
                    Point3d anotherPoint = ls.StartPoint + (norm1 * Length);
                    Ray r = new Ray();
                    r.BasePoint = CenterPoint;
                    r.SecondPoint = anotherPoint;

                    Atend.Global.Acad.UAcad.DrawEntityOnScreen(c, Atend.Control.Enum.AutoCadLayerName.SHELL.ToString());

                    LineSegment3d temp = new LineSegment3d(CenterPoint, anotherPoint);
                    Point3d p3 = r.GetPointAtDist(temp.Length * 2);
                    //ed.WriteMessage("point:{0} \n",p3);
                    //Atend.Global.Acad.UAcad.DrawEntityOnScreen(r, Atend.Control.Enum.AutoCadLayerName.SHELL.ToString());

                    Line l1 = new Line(ls.StartPoint, p3);
                    Atend.Global.Acad.UAcad.DrawEntityOnScreen(l1, Atend.Control.Enum.AutoCadLayerName.SHELL.ToString());

                    Line l2 = new Line(ls.EndPoint, p3);
                    Atend.Global.Acad.UAcad.DrawEntityOnScreen(l2, Atend.Control.Enum.AutoCadLayerName.SHELL.ToString());

                }
                else
                {
                    Point3d CenterPoint = Atend.Global.Acad.UAcad.CenterOfEntity(Atend.Global.Acad.UAcad.GetEntityByObjectID(oi));
                    c = new Circle(CenterPoint, new Vector3d(0, 0, 1), ls1.Length / 3);

                    Vector3d Vect1 = ls1.EndPoint - ls1.StartPoint, norm1 = Vect1.GetNormal();
                    double Length = Vect1.Length / 2;
                    Point3d anotherPoint = ls1.StartPoint + (norm1 * Length);
                    Ray r = new Ray();
                    r.BasePoint = CenterPoint;
                    r.SecondPoint = anotherPoint;

                    Atend.Global.Acad.UAcad.DrawEntityOnScreen(c, Atend.Control.Enum.AutoCadLayerName.SHELL.ToString());


                    LineSegment3d temp = new LineSegment3d(CenterPoint, anotherPoint);
                    Point3d p3 = r.GetPointAtDist(temp.Length * 2);
                    //ed.WriteMessage("point:{0} \n", p3);
                    //Atend.Global.Acad.UAcad.DrawEntityOnScreen(r, Atend.Control.Enum.AutoCadLayerName.SHELL.ToString());

                    Line l1 = new Line(ls1.StartPoint, p3);
                    Atend.Global.Acad.UAcad.DrawEntityOnScreen(l1, Atend.Control.Enum.AutoCadLayerName.SHELL.ToString());

                    Line l2 = new Line(ls1.EndPoint, p3);
                    Atend.Global.Acad.UAcad.DrawEntityOnScreen(l2, Atend.Control.Enum.AutoCadLayerName.SHELL.ToString());


                }


                if (IsExist)
                {
                    try
                    {
                        Transaction tr = doc.TransactionManager.StartTransaction();
                        using (tr)
                        {
                            // Check the entity is a closed curve
                            //DBObject obj = tr.GetObject(CurrentPoleOI, OpenMode.ForRead);
                            //Curve cur = obj as Curve;
                            //if (cur != null && cur.Closed == false)
                            //{
                            //    //ed.WriteMessage("\nLoop must be a closed curve.");
                            //}
                            //else
                            //{
                            // We'll add the hatch to the model space
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
                            btr.AppendEntity(hat);
                            tr.AddNewlyCreatedDBObject(hat, true);
                            // Add the hatch loop and complete the hatch
                            ObjectIdCollection ids = new ObjectIdCollection();
                            ids.Add(c.ObjectId);
                            hat.Associative = true;
                            hat.AppendLoop(HatchLoopTypes.Default, ids);
                            hat.EvaluateHatch(true);
                            tr.Commit();
                            //}
                        }
                    }
                    catch (System.Exception ex)
                    {
                        ed.WriteMessage("ERROR Wipeout : {0} \n", ex.Message);
                    }
                }//
            }
        }

        public bool UpdateGroundPostData(Guid EXCode)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            try
            {
                if (DeleteGroundPostData(selectedObjectId))
                {
                    if (!DeleteGroundPost(selectedObjectId))
                    {
                        throw new System.Exception("Error in Delete Graphic");
                    }
                    if (!Atend.Global.Acad.AcadRemove.DeleteEntityByObjectId(selectedObjectId))
                    {
                        throw new System.Exception("can not remove GroundPost");
                    }
                }
                else
                {
                    throw new System.Exception("Error in Delete Data");
                }
                //DrawGroundPost();
                return true;
            }
            catch (System.Exception ex)
            {
                ed.WriteMessage("ERROR UpdateGroundPost 01 : {0} \n", ex.Message);
                return false;
            }


            //OOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO
            //    Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            //    OleDbTransaction aTransaction;
            //    OleDbConnection aConnection = new OleDbConnection(Atend.Control.ConnectionString.AccessCnString);
            //    try
            //    {
            //        aConnection.Open();
            //        aTransaction = aConnection.BeginTransaction();
            //        try
            //        {
            //            GroundPostPack = Atend.Base.Design.DPackage.AccessSelectByCode(EXCode);
            //            if (!UseAccess)
            //            {
            //                if (!eGroundPost.AccessInsert(aTransaction, aConnection, true))
            //                {
            //                    throw new System.Exception("eGroundPost.AccessInsert failed");
            //                }
            //                //else
            //                //{
            //                //    foreach (Atend.Base.Equipment.EStreetBoxPhuse sbPhuses in eStreetBoxPhuse)
            //                //    {
            //                //        if (!sbPhuses.AccessInsert(aTransaction, aConnection, true))
            //                //        {
            //                //            throw new System.Exception("eStreetBoxPhuse.AccessInsert failed");
            //                //        }
            //                //    }
            //                //}
            //            }

            //            //DPost
            //            Atend.Base.Design.DPost dPost = new Atend.Base.Design.DPost();
            //            dPost.Number = "N001";
            //            dPost.ProductCode = eGroundPost.Code;
            //            if (!dPost.AccessInsert(aTransaction, aConnection))
            //            {
            //                throw new System.Exception("dPost.AccessInsert failed");
            //            }
            //            ed.WriteMessage("******* 1\n");
            //            //DPackage
            //            Atend.Base.Design.DPackage dPackPost = new Atend.Base.Design.DPackage();
            //            dPackPost.NodeCode = dPost.Code;
            //            dPackPost.Count = 1;
            //            dPackPost.Type = (int)Atend.Control.Enum.ProductType.GroundPost;
            //            dPackPost.ProductCode = eGroundPost.Code;
            //            dPackPost.IsExistance = Existance;
            //            dPackPost.ProjectCode = ProjectCode;
            //            dPackPost.Number = "";
            //            if (!dPackPost.AccessInsert(aTransaction, aConnection))
            //            {
            //                throw new System.Exception("dPackPost.AccessInsert failed");
            //            }
            //            //Delete previous post
            //            //if (!Atend.Base.Design.DPackage.AccessDelete(EXCode))
            //            //{
            //            //    throw new System.Exception("dPackPost.AccessDelete failed");
            //            //}
            //            ed.WriteMessage("******* 2 before insert jackpanelmid\n");
            //            int i = 0;
            //            foreach (Atend.Base.Equipment.EJAckPanel EJP in eJackPanelMiddles)
            //            {
            //                Atend.Base.Design.DPackage dPack = new Atend.Base.Design.DPackage();
            //                dPack.Count = 1;
            //                dPack.Type = (int)Atend.Control.Enum.ProductType.MiddleJackPanel;
            //                dPack.ProductCode = EJP.Code;
            //                dPack.IsExistance = Convert.ToByte(eJackPanelMiddleExistance[i]);
            //                dPack.ProjectCode = Convert.ToInt32(eJackPanelMiddleProjectCode[i]);
            //                dPack.ParentCode = dPackPost.Code;
            //                dPack.Number = "";
            //                if (!dPack.AccessInsert(aTransaction, aConnection))
            //                {
            //                    throw new System.Exception("dPack.AccessInsert failed");
            //                }
            //                else
            //                {
            //                    // add to inserted
            //                    PostEquips pe = new PostEquips();
            //                    pe.CodeGuid = dPack.Code;
            //                    pe.ProductCode = dPack.ProductCode;
            //                    pe.ProductType = (int)Atend.Control.Enum.ProductType.MiddleJackPanel;
            //                    pe.ParentCode = dPackPost.Code;
            //                    PostEquipInserted.Add(pe);

            //                    //insert cell by xcode

            //                    System.Data.DataTable CellsList = Atend.Base.Equipment.EJackPanelCell.AccessSelectByJackPanelCode(EJP.Code, aTransaction, aConnection);
            //                    foreach (DataRow dr in CellsList.Rows)
            //                    {

            //                        Atend.Base.Design.DPackage dPack1 = new Atend.Base.Design.DPackage();
            //                        dPack1.Count = 1;
            //                        dPack1.Type = (int)Atend.Control.Enum.ProductType.Cell;
            //                        dPack1.ProductCode = Convert.ToInt32(dr["ProductCode"]);
            //                        dPack1.IsExistance = Convert.ToByte(eJackPanelMiddleExistance[i]);
            //                        dPack1.ProjectCode = Convert.ToInt32(eJackPanelMiddleProjectCode[i]);
            //                        dPack1.ParentCode = dPack.Code;
            //                        dPack1.Number = "";
            //                        if (!dPack1.AccessInsert(aTransaction, aConnection))
            //                        {
            //                            throw new System.Exception("dPack1.AccessInsert failed");
            //                        }
            //                        else
            //                        {
            //                            Atend.Base.Design.DCellStatus sCellStatus = new Atend.Base.Design.DCellStatus();
            //                            sCellStatus.IsClosed = false;
            //                            sCellStatus.JackPanelCode = dPack.Code;
            //                            sCellStatus.CellCode = dPack1.Code;
            //                            if (!sCellStatus.AccessInsert(aTransaction, aConnection))
            //                            {
            //                                throw new System.Exception("sCellStatus.AccessInsert failed");
            //                            }


            //                            // add to inserted
            //                            pe = new PostEquips();
            //                            pe.CodeGuid = dPack1.Code;
            //                            pe.ProductCode = dPack1.ProductCode;
            //                            pe.ProductType = (int)Atend.Control.Enum.ProductType.Cell;
            //                            pe.ParentCode = dPack.Code;
            //                            PostEquipInserted.Add(pe);
            //                        }
            //                    }


            //                }
            //                i++;
            //            }
            //            //ed.WriteMessage("MJ FINSH\n");

            //            ed.WriteMessage("****** 3 before insert trans\n");
            //            //insert transformer
            //            int j = 0;
            //            foreach (Atend.Base.Equipment.ETransformer transformer in eTransformers)
            //            {
            //                Atend.Base.Design.DPackage transformerPackage = new Atend.Base.Design.DPackage();
            //                transformerPackage.Count = 1;
            //                transformerPackage.IsExistance = Convert.ToByte(eTransformerExistance[j]);
            //                transformerPackage.ProjectCode = Convert.ToInt32(eTransformerExistance[j]);
            //                transformerPackage.ParentCode = dPackPost.Code;
            //                transformerPackage.ProductCode = transformer.Code;
            //                transformerPackage.Type = (int)Atend.Control.Enum.ProductType.Transformer;
            //                transformerPackage.Number = "";
            //                if (!transformerPackage.AccessInsert(aTransaction, aConnection))
            //                {
            //                    throw new System.Exception("transformerPackage.AccessInsert failed");
            //                }
            //                else
            //                {
            //                    // add to inserted
            //                    PostEquips pe = new PostEquips();
            //                    pe.CodeGuid = transformerPackage.Code;
            //                    pe.ProductCode = transformerPackage.ProductCode;
            //                    pe.ProductType = (int)Atend.Control.Enum.ProductType.Transformer;
            //                    pe.ParentCode = dPackPost.Code;
            //                    PostEquipInserted.Add(pe);
            //                }
            //                j++;
            //            }
            //            //ed.WriteMessage("TRAN FINSH\n");

            //            //insert weekJackpanel
            //            ed.WriteMessage("******* 4 before insert jackpanelweek\n");
            //            j = 0;
            //            foreach (Atend.Base.Equipment.EJackPanelWeek jackpanelweek in eJackPanelWeeks)
            //            {
            //                Atend.Base.Design.DPackage jackpanelweekPackage = new Atend.Base.Design.DPackage();
            //                jackpanelweekPackage.Count = 1;
            //                jackpanelweekPackage.IsExistance = Convert.ToByte(eJackPanelWeekExistance[j]);
            //                jackpanelweekPackage.ProjectCode = Convert.ToInt32(eJackPanelWeekProjectCode[j]);
            //                jackpanelweekPackage.ParentCode = dPackPost.Code;
            //                jackpanelweekPackage.ProductCode = jackpanelweek.Code;
            //                jackpanelweekPackage.Type = (int)Atend.Control.Enum.ProductType.WeekJackPanel;
            //                jackpanelweekPackage.Number = "";
            //                if (!jackpanelweekPackage.AccessInsert(aTransaction, aConnection))
            //                {
            //                    throw new System.Exception("jackpanelweekPackage.AccessInsert failed");
            //                }
            //                else
            //                {
            //                    // add to inserted
            //                    PostEquips pe = new PostEquips();
            //                    pe.CodeGuid = jackpanelweekPackage.Code;
            //                    pe.ProductCode = jackpanelweekPackage.ProductCode;
            //                    pe.ProductType = (int)Atend.Control.Enum.ProductType.WeekJackPanel;
            //                    pe.ParentCode = dPackPost.Code;
            //                    PostEquipInserted.Add(pe);
            //                }
            //                j++;
            //            }
            //            //GroundPostPack.IsExistance = Existance;
            //            //GroundPostPack.ProductCode = eGroundPost.Code;
            //            //GroundPostPack.ProjectCode = ProjectCode;
            //            //GroundPostPack.Number = "";
            //            //if (GroundPostPack.AccessUpdate(aTransaction, aConnection))
            //            //{
            //            //    Atend.Base.Acad.AT_INFO atinfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(selectedObjectId);
            //            //    atinfo.ProductCode = eGroundPost.Code;
            //            //    atinfo.Insert();
            //            //}
            //            //else
            //            //{
            //            //    throw new System.Exception("GroundPostPack.AccessInsert(tr) failed");
            //            //}
            //            ed.WriteMessage("******* 5\n");
            //            foreach (Atend.Base.Equipment.EJAckPanel selectedMiddleJack in eJackPanelMiddles)
            //            {
            //                Atend.Base.Equipment.EBus CurrentBus = Atend.Base.Equipment.EBus.AccessSelectByXCode(selectedMiddleJack.MasterProductXCode);
            //                if (CurrentBus.Code != -1)
            //                {

            //                    if (!CurrentBus.AccessInsert(aTransaction, aConnection, true))
            //                    {
            //                        throw new System.Exception("CurrentBus.AccessInsert failed");
            //                    }
            //                }
            //                else
            //                {
            //                    throw new System.Exception("Current bus was not found");
            //                }

            //                selectedMiddleJack.MasterProductCode = CurrentBus.Code;
            //                if (!selectedMiddleJack.AccessInsert(aTransaction, aConnection, true))
            //                {
            //                    throw new System.Exception("selectedMiddleJack.AccessInsert afiled");
            //                }
            //                else
            //                {
            //                    //insert cell by xcode
            //                    System.Data.DataTable CellsList = Atend.Base.Equipment.EJackPanelCell.SelectByJackPanelXCode(selectedMiddleJack.XCode);
            //                    foreach (DataRow cell1 in CellsList.Rows)
            //                    {
            //                        Atend.Base.Equipment.ECell eCell = Atend.Base.Equipment.ECell.SelectByXCodeForDesign(new Guid(cell1["ProductXCode"].ToString()));
            //                        if (!eCell.AccessInsert(aTransaction, aConnection, true))
            //                        {
            //                            throw new System.Exception("eCell.AccessInsert failed");
            //                        }

            //                        Atend.Base.Equipment.EJackPanelCell SelectedJackPanelmiddleCell = new Atend.Base.Equipment.EJackPanelCell();
            //                        SelectedJackPanelmiddleCell.XCode = new Guid(cell1["XCode"].ToString());
            //                        SelectedJackPanelmiddleCell.JackPanelCode = selectedMiddleJack.Code;

            //                        SelectedJackPanelmiddleCell.ProductCode = eCell.Code;
            //                        SelectedJackPanelmiddleCell.ProductType = Convert.ToByte(cell1["ProductType"].ToString());
            //                        SelectedJackPanelmiddleCell.Num = Convert.ToByte(cell1["CellNum"].ToString());

            //                        if (!SelectedJackPanelmiddleCell.AccessInsert(aTransaction, aConnection))
            //                        {
            //                            throw new System.Exception("SelectedJackPanelmiddleCell.AccessInsert \n");
            //                        }

            //                        if (!Atend.Base.Equipment.EContainerPackage.SentFromLocalToAccess(eCell.XCode, (int)Atend.Control.Enum.ProductType.Cell, eCell.Code, aTransaction, aConnection))
            //                        {
            //                            throw new System.Exception("SentFromLocalToAccess failed");
            //                        }

            //                    }
            //                }
            //            }
            //            ////////////////////////////////////////////
            //            //    Atend.Base.Design.DPackage jackpanelweekPackage = new Atend.Base.Design.DPackage();
            //            //    jackpanelweekPackage.Count = 1;
            //            //    jackpanelweekPackage.IsExistance = Convert.ToByte(eJackPanelWeekExistance[j]);
            //            //    jackpanelweekPackage.ProjectCode = Convert.ToInt32(eJackPanelWeekProjectCode[j]);
            //            //    jackpanelweekPackage.ParentCode = dPackPost.Code;
            //            //    jackpanelweekPackage.ProductCode = jackpanelweek.Code;
            //            //    jackpanelweekPackage.Type = (int)Atend.Control.Enum.ProductType.WeekJackPanel;
            //            //    jackpanelweekPackage.Number = "";
            //            //    if (!jackpanelweekPackage.AccessInsert(aTransaction, aConnection))
            //            //    {
            //            //        throw new System.Exception("jackpanelweekPackage.AccessInsert failed");
            //            //    }
            //            //    else
            //            //    {
            //            //        // add to inserted
            //            //        PostEquips pe = new PostEquips();
            //            //        pe.CodeGuid = jackpanelweekPackage.Code;
            //            //        pe.ProductCode = jackpanelweekPackage.ProductCode;
            //            //        pe.ProductType = (int)Atend.Control.Enum.ProductType.WeekJackPanel;
            //            //        pe.ParentCode = dPackPost.Code;
            //            //        PostEquipInserted.Add(pe);
            //            //    }
            //            //    j++;
            //            //}
            //            ////////////////////////////////////////////

            //            ed.WriteMessage("******* 6\n");
            //            foreach (Atend.Base.Equipment.EJackPanelWeek selecteweekjack in eJackPanelWeeks)
            //            {
            //                Atend.Base.Equipment.EAutoKey_3p Key = Atend.Base.Equipment.EAutoKey_3p.AccessSelectByXCode(selecteweekjack.AutoKey3pXCode);
            //                if (Key.Code != -1)
            //                {
            //                    if (!Key.AccessInsert(aTransaction, aConnection, true))
            //                    {
            //                        throw new System.Exception("Key.AccessInsert");
            //                    }
            //                }
            //                else
            //                {
            //                    throw new System.Exception("Key was not found");
            //                }

            //                selecteweekjack.AutoKey3pCode = Key.Code;
            //                if (!selecteweekjack.AccessInsert(aTransaction, aConnection, true))
            //                {
            //                    throw new System.Exception("selectesweekjack.accessinsert failed");
            //                }
            //                else
            //                {
            //                    //insert cell by xcode
            //                    System.Data.DataTable CellsList = Atend.Base.Equipment.EJackPanelWeekCell.SelectByJackPanelWeekXCode(selecteweekjack.XCode);
            //                    foreach (DataRow dr in CellsList.Rows)
            //                    {
            //                        Atend.Base.Equipment.EJackPanelWeekCell selectedjackpanelweekcell = new Atend.Base.Equipment.EJackPanelWeekCell();
            //                        selectedjackpanelweekcell.IsNightLight = Convert.ToBoolean(dr["IsNightLight"]);
            //                        selectedjackpanelweekcell.XCode = new Guid(dr["XCode"].ToString());
            //                        selectedjackpanelweekcell.JackPanelWeekCode = selecteweekjack.Code;
            //                        selectedjackpanelweekcell.Num = Convert.ToInt32(dr["Num"]);
            //                        if (!selectedjackpanelweekcell.AccessInsert(aTransaction, aConnection))
            //                        {
            //                            throw new System.Exception("selectedjackpanelweekcell.accessinsert \n");
            //                        }

            //                        if (!Atend.Base.Equipment.EContainerPackage.SentFromLocalToAccess(selectedjackpanelweekcell.XCode, (int)Atend.Control.Enum.ProductType.Cell, selectedjackpanelweekcell.Code, aTransaction, aConnection))
            //                        {
            //                            throw new System.Exception("SentFromLocalToAccess failed");
            //                        }
            //                    }
            //                }
            //            }

            //            ed.WriteMessage("******* 7\n");
            //            foreach (Atend.Base.Equipment.ETransformer SelectedTransformer in eTransformers)
            //            {
            //                if (!SelectedTransformer.AccessInsert(aTransaction, aConnection, true))
            //                {
            //                    throw new System.Exception("SelectedTransformer.AccessInsert failed");
            //                }
            //            }

            //        }
            //        catch (System.Exception ex1)
            //        {
            //            ed.WriteMessage("ERROR UpdateGroundPost 01(transaction) : {0} \n", ex1.Message);
            //            aTransaction.Rollback();
            //            aConnection.Close();
            //            return false;
            //        }
            //        aTransaction.Commit();

            //        if (DeleteGroundPostData(selectedObjectId))
            //        {
            //            if (!DeleteGroundPost(selectedObjectId))
            //            {
            //                throw new System.Exception("Error in Delete Graphic");
            //            }
            //            if (!Atend.Global.Acad.AcadRemove.DeleteEntityByObjectId(selectedObjectId))
            //            {
            //                throw new System.Exception("can not remove GroundPost");
            //            }
            //        }
            //        else
            //        {
            //            throw new System.Exception("Error in Delete Data");
            //        }
            //        //DrawGroundPostUpdate();
            //        DrawGroundPost();
            //        aConnection.Close();
            //        return true;
            //    }
            //    catch (System.Exception ex)
            //    {
            //        ed.WriteMessage("ERROR UpdateGroundPost 01 : {0} \n", ex.Message);
            //        aConnection.Close();
            //        return false;
            //    }

        }

        public bool UpdateGroundPostWithoutDraw(Guid EXCode, System.Data.DataTable dtSuEquip)
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
                    Atend.Base.Design.DPackage dPackPost = new Atend.Base.Design.DPackage();
                    dPackPost.Code = EXCode;
                    dPackPost.IsExistance = Existance;
                    dPackPost.ProjectCode = ProjectCode;
                    if (!dPackPost.AccessUpdateProjectCodeAndIsExistance(aTransaction, aConnection))
                    {
                        throw new System.Exception("DPackage.Update failed");
                    }

                    foreach (DataRow dr in dtSuEquip.Rows)
                    {
                        dPackPost = Atend.Base.Design.DPackage.AccessSelectByProductCodeType(Convert.ToInt32(dr["Code"].ToString()), Convert.ToInt32(dr["Type"].ToString()));
                        if (dPackPost.ProductCode != -1)
                        {
                            dPackPost.IsExistance = Convert.ToInt32(dr["IsExistance"].ToString());
                            dPackPost.ProjectCode = Convert.ToInt32(dr["ProjectCode"].ToString());
                            if (!dPackPost.AccessUpdate(aTransaction, aConnection))
                            {
                                throw new System.Exception("DPackage.Update_SUB failed");
                            }

                            //For Update Sub MiddleJackPanel
                            if (Convert.ToInt32(dr["Type"].ToString()) == Convert.ToInt32(Atend.Control.Enum.ProductType.MiddleJackPanel))
                            {
                                if (!dPackPost.AccessUpdateSubMiddleJackPanel(aTransaction, aConnection))
                                {
                                    throw new System.Exception("DPackage.Update_SUBMiddleJackPanel failed");
                                }
                            }
                        }
                        else
                        {
                            throw new System.Exception("DPackage.Update_SUB2 failed");
                        }

                    }
                }
                catch (System.Exception ex1)
                {
                    ed.WriteMessage("ERROR UpdateAirPostWithoutDraw 01(transaction) : {0} \n", ex1.Message);
                    aTransaction.Rollback();
                    aConnection.Close();
                    return false;
                }

                aTransaction.Commit();
                aConnection.Close();
                return true;
            }
            catch (System.Exception ex)
            {
                ed.WriteMessage("ERROR UpdateAirPostWithoutDraw 01 : {0} \n", ex.Message);
                aConnection.Close();
                return false;
            }

        }

        public static bool DeleteGroundPostData(ObjectId GroundPostOI)
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
                    Atend.Base.Acad.AT_SUB SubGP = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(GroundPostOI);
                    foreach (ObjectId oi in SubGP.SubIdCollection)
                    {
                        Atend.Base.Acad.AT_INFO atinfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oi);
                        ObjectIdCollection Collection = Atend.Global.Acad.UAcad.GetGroupSubEntities(atinfo.SelectedObjectId);

                        foreach (ObjectId collect in Collection)
                        {
                            Atend.Base.Acad.AT_INFO atinfo2 = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(collect);
                            if (atinfo2.ParentCode != "NONE" && atinfo2.NodeType == (int)Atend.Control.Enum.ProductType.HeaderCabel)
                            {
                                Atend.Base.Acad.AT_SUB subheadercabel = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(collect);
                                foreach (ObjectId sb in subheadercabel.SubIdCollection)
                                {
                                    Atend.Base.Acad.AT_INFO atinfo3 = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(sb);
                                    if (atinfo3.ParentCode != "NONE" && atinfo3.NodeType == (int)Atend.Control.Enum.ProductType.GroundCabel)
                                    {
                                        if (!Atend.Base.Design.DBranch.AccessDelete(new Guid(atinfo3.NodeCode.ToString()), _Transaction, _Connection))
                                        {
                                            throw new System.Exception("Error In Delete dbranch\n");
                                        }
                                    }
                                }
                            }
                        }
                    }

                    Atend.Base.Acad.AT_INFO conductorinfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(GroundPostOI);
                    Atend.Base.Design.DPackage package = Atend.Base.Design.DPackage.AccessSelectByCode(new Guid(conductorinfo.NodeCode.ToString()));
                    System.Data.DataTable dtMiddleJackPanel = Atend.Base.Design.DPackage.AccessSelectByParentCodeAndType(package.Code, Convert.ToInt32(Atend.Control.Enum.ProductType.MiddleJackPanel));

                    //Delete From Post
                    if (!Atend.Base.Design.DPost.AccessDelete(package.NodeCode, _Transaction, _Connection))
                    {
                        throw new System.Exception("Error In Delete DPost\n");
                    }

                    //Delete MiddleJackPanel From CellStatus
                    foreach (DataRow dr in dtMiddleJackPanel.Rows)
                    {
                        if (!Atend.Base.Design.DCellStatus.AccessDeleteByJackPanelCode(new Guid(dr["Code"].ToString()), _Transaction, _Connection))
                        {
                            throw new System.Exception("Error In Delete dCellStatus\n");
                        }
                    }

                    //delete GroundPost
                    if (!Atend.Base.Design.DPackage.AccessDelete(package.Code, _Transaction, _Connection))
                    {
                        throw new System.Exception("Error In Delete GroundPost in DPackage\n");
                    }
                    foreach (DataRow dr in dtMiddleJackPanel.Rows)
                    {
                        System.Data.DataTable dtCell = Atend.Base.Design.DPackage.AccessSelectByParentCodeAndType(new Guid(dr["Code"].ToString()), Convert.ToInt32(Atend.Control.Enum.ProductType.Cell));
                        foreach (DataRow drcell in dtCell.Rows)
                        {
                            if (!Atend.Base.Design.DPackage.AccessDelete(new Guid(drcell["Code"].ToString()), _Transaction, _Connection))
                            {
                                throw new System.Exception("Error In Delete Cell in DPackage\n");
                            }
                        }
                        if (!Atend.Base.Design.DPackage.AccessDelete(new Guid(dr["Code"].ToString()), _Transaction, _Connection))
                        {
                            throw new System.Exception("Error In Delete MiddleJackPanel in DPackage\n");
                        }
                    }
                    System.Data.DataTable dtTransformer = Atend.Base.Design.DPackage.AccessSelectByParentCodeAndType(package.Code, Convert.ToInt32(Atend.Control.Enum.ProductType.Transformer));
                    foreach (DataRow dr in dtTransformer.Rows)
                    {
                        if (!Atend.Base.Design.DPackage.AccessDelete(new Guid(dr["Code"].ToString()), _Transaction, _Connection))
                        {
                            throw new System.Exception("Error In Delete Transformer in DPackage\n");
                        }
                    }
                    System.Data.DataTable dtWeekJackPanel = Atend.Base.Design.DPackage.AccessSelectByParentCodeAndType(package.Code, Convert.ToInt32(Atend.Control.Enum.ProductType.WeekJackPanel));
                    foreach (DataRow dr in dtWeekJackPanel.Rows)
                    {
                        if (!Atend.Base.Design.DPackage.AccessDelete(new Guid(dr["Code"].ToString()), _Transaction, _Connection))
                        {
                            throw new System.Exception("Error In Delete WeekJackPanel in DPackage\n");
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    ed.WriteMessage("GRA ERROR GroundPost(Transaction) : {0} \n", ex.Message);
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

        public static bool DeleteGroundPost(ObjectId GroundPostOI)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            try
            {
                Atend.Base.Acad.AT_SUB streetSub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(GroundPostOI);
                foreach (ObjectId oi in streetSub.SubIdCollection)
                {
                    ObjectIdCollection Collection = Atend.Global.Acad.UAcad.GetGroupSubEntities(oi);
                    bool ismove = false;
                    ArrayList arrIsMoveCabel = new ArrayList();
                    ObjectIdCollection headerobji = new ObjectIdCollection();
                    foreach (ObjectId collect in Collection)
                    {
                        //////////////////////////////
                        Atend.Base.Acad.AT_INFO atinfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(collect);
                        if (atinfo.ParentCode != "NONE" && atinfo.NodeType == (int)Atend.Control.Enum.ProductType.Transformer)
                        {
                            ObjectId id = Atend.Global.Acad.UAcad.GetEntityGroup(collect);
                            Atend.Base.Acad.AT_SUB sub2 = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(id);
                            foreach (ObjectId h in sub2.SubIdCollection)
                            {
                                Atend.Base.Acad.AT_INFO at_info20 = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(h);
                                if (at_info20.ParentCode != "NONE" && at_info20.NodeType == (int)Atend.Control.Enum.ProductType.Comment)
                                {
                                    if (ismove == false)
                                    {
                                        if (!Atend.Global.Acad.AcadRemove.DeleteEntityByObjectId(h))
                                        {
                                            throw new System.Exception("can not remove entity");
                                        }
                                    }
                                    ismove = true;
                                }
                            }
                        }

                        //for out of area headercabel
                        Atend.Base.Acad.AT_SUB sub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(collect);
                        foreach (ObjectId objsub in sub.SubIdCollection)
                        {
                            bool sw = false;
                            Atend.Base.Acad.AT_INFO at_info1 = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(objsub);
                            if (at_info1.ParentCode != "NONE" && at_info1.NodeType == (int)Atend.Control.Enum.ProductType.GroundCabel)
                            {
                                for (int i = 0; i < arrIsMoveCabel.Count; i++)
                                {
                                    if (arrIsMoveCabel[i].ToString() == objsub.ToString())
                                        sw = true;
                                }
                                if (!sw)
                                {
                                    Atend.Base.Acad.AT_SUB subcable = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(at_info1.SelectedObjectId);
                                    foreach (ObjectId headeroi in subcable.SubIdCollection)
                                    {
                                        Atend.Base.Acad.AT_INFO at_infocom = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(headeroi);
                                        if (at_infocom.ParentCode != "NONE" && at_infocom.NodeType == (int)Atend.Control.Enum.ProductType.Comment)
                                        {
                                            if (!Atend.Global.Acad.AcadRemove.DeleteEntityByObjectId(headeroi))
                                            {
                                                throw new System.Exception("can not remove entity");
                                            }
                                        }
                                        if (at_infocom.ParentCode != "NONE" && at_infocom.NodeType == (int)Atend.Control.Enum.ProductType.HeaderCabel)
                                        {
                                            Polyline p1 = Atend.Global.Acad.UAcad.GetEntityByObjectID(GroundPostOI) as Polyline;
                                            if (Atend.Global.Acad.UAcad.IsInsideCurve((Curve)p1, Atend.Global.Acad.UAcad.CenterOfEntity(Atend.Global.Acad.UAcad.GetEntityByObjectID(headeroi))) == false)
                                            {
                                                Atend.Base.Acad.AT_SUB.RemoveFromAT_SUB(objsub, headeroi);
                                            }
                                        }
                                    }
                                    arrIsMoveCabel.Add(objsub);
                                    if (!Atend.Global.Acad.AcadRemove.DeleteEntityByObjectId(objsub))
                                    {
                                        throw new System.Exception("can not remove entity");
                                    }
                                }
                            }
                        }
                        if (!Atend.Global.Acad.AcadRemove.DeleteEntityByObjectId(collect))
                        {
                            throw new System.Exception("can not remove entity");
                        }
                        //////////////////////////////
                        //+++++++++++++++++++++++++++++++++++++++++
                        //Atend.Base.Acad.AT_SUB GroupSub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(collect);
                        //foreach (ObjectId GroupSubOi in GroupSub.SubIdCollection)
                        //{
                        //    if (!Atend.Global.Acad.AcadRemove.DeleteEntityByObjectId(GroupSubOi))
                        //    {
                        //        throw new System.Exception("can not remove sub entity");
                        //    }
                        //}

                        //if (!Atend.Global.Acad.AcadRemove.DeleteEntityByObjectId(collect))
                        //{
                        //    throw new System.Exception("can not remove entity");
                        //}
                        //ObjectIdCollection OIC = Atend.Global.Acad.UAcad.GetGroupSubEntities(collect);
                        //foreach (ObjectId oi1 in OIC)
                        //{
                        //    if (!Atend.Global.Acad.AcadRemove.DeleteEntityByObjectId(oi1))
                        //    {
                        //        throw new System.Exception("can not remove entity");
                        //    }
                        //}
                        //+++++++++++++++++++++++++++++++++++++++
                    }
                    if (!Atend.Global.Acad.AcadRemove.DeleteEntityByObjectId(oi))
                    {
                        throw new System.Exception("can not remove entity");
                    }
                }

                //+++
                if (!Atend.Global.Acad.AcadRemove.DeleteEntityByObjectId(GroundPostOI))
                {
                    throw new System.Exception("can not remove GroundPost");
                }


            }
            catch (System.Exception ex)
            {
                ed.WriteMessage("Error DeleteGroundPost : {0} \n", ex.Message);
                return false;
            }
            //ed.WriteMessage("DELETE GroundPost\n");
            return true;
        }

        public static void ShowDescription(ObjectId oi, OleDbConnection _Conection)
        {

            int ProductCode = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oi).ProductCode;
            Entity CurrentEntity = Atend.Global.Acad.UAcad.GetEntityByObjectID(oi);
            double CommentScale = Atend.Base.Design.DProductProperties.AccessSelectBySoftwareCode((int)Atend.Control.Enum.ProductType.AirPost).Scale;
            Atend.Base.Equipment.EGroundPost _EGroundPost = Atend.Base.Equipment.EGroundPost.AccessSelectByCode(ProductCode, _Conection);
            if (_EGroundPost.Code != -1)
            {

                Point3d EntityCenterPoint = Atend.Global.Acad.UAcad.CenterOfEntity(CurrentEntity);
                Entity TextEntity = Atend.Global.Acad.UAcad.WriteNote(_EGroundPost.Comment, EntityCenterPoint, CommentScale);
                Atend.Global.Acad.UAcad.DrawEntityOnScreen(TextEntity, Atend.Control.Enum.AutoCadLayerName.DESCRIPTION.ToString());


            }
        }


    }
}