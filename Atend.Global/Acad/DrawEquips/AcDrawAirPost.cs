﻿using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;

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

    public class AcDrawAirPost
    {

        //~~~~~~~~~~~~~~~~~~~~~~ properties ~~~~~~~~~~~~~~~~~~//

        bool _UseAccess;
        public bool UseAccess
        {
            get { return _UseAccess; }
            set { _UseAccess = value; }
        }

        Atend.Base.Equipment.EAirPost _eAirPost;
        public Atend.Base.Equipment.EAirPost eAirPost
        {
            get { return _eAirPost; }
            set { _eAirPost = value; }
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

        int _projectCode;
        public int ProjectCode
        {
            get { return _projectCode; }
            set { _projectCode = value; }
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

        Atend.Base.Design.DPackage AirPostPack = new Atend.Base.Design.DPackage();

        private ObjectId selectedObjectId;
        public ObjectId SelectedObjectId
        {
            get { return selectedObjectId; }
            set { selectedObjectId = value; }
        }

        //~~~~~~~~~~~~~~~~~~~~~~ class ~~~~~~~~~~~~~~~~~~//
        /*
        public class DrawAirPostJig : DrawJig
        {

            double GroundPostWidth = 0;
            double GroundPostHeight = 0;
            double scale = 1;
            Point3d BasePoint;
            double NewAngle, BaseAngle = 0;
            Atend.Global.Acad.AcadJigs.MyPolyLine GroundPostEntiry;
            public bool GetAngle = false;
            private TextStyle _style;


            public DrawAirPostJig(double Width, double Height, double Scale)
            {

                // SET TEXT STYLE

                _style = new TextStyle();

                _style.Font = new FontDescriptor("Calibri", false, true, 0, 0);

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
                pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX - (Width / 2), BaseY - (Height / 2)), 0, 0, 0);
                pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX + (Width / 2), BaseY - (Height / 2)), 0, 0, 0);
                pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX + (Width / 2), BaseY + (Height / 2)), 0, 0, 0);
                pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX - (Width / 2), BaseY + (Height / 2)), 0, 0, 0);
                pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX - (Width / 2), BaseY - (Height / 2)), 0, 0, 0);
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

                WorldGeometry2 wg2 = draw.Geometry as WorldGeometry2;

                if (wg2 != null)
                {

                    // Push our transforms onto the stack

                    wg2.PushOrientationTransform(OrientationBehavior.Screen);

                    wg2.PushPositionTransform(PositionBehavior.Screen, new Point2d(30, 30));

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
        */


        public class DrawAirPostJig : DrawJig
        {

            double GroundPostWidth = 0;
            double GroundPostHeight = 0;
            double scale = 1;
            Point3d BasePoint;
            double NewAngle, BaseAngle = 0;
            Atend.Global.Acad.AcadJigs.MyPolyLine GroundPostEntiry;
            public bool GetAngle = false;
            private Autodesk.AutoCAD.GraphicsInterface.TextStyle _style;



            public DrawAirPostJig(double Width, double Height, double Scale)
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
                pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX - (Width / 2), BaseY - (Height / 2)), 0, 0, 0);
                pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX + (Width / 2), BaseY - (Height / 2)), 0, 0, 0);
                pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX + (Width / 2), BaseY + (Height / 2)), 0, 0, 0);
                pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX - (Width / 2), BaseY + (Height / 2)), 0, 0, 0);
                pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX - (Width / 2), BaseY - (Height / 2)), 0, 0, 0);
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

                    JigPromptPointOptions jppo = new JigPromptPointOptions("\nEnter Air Post Position :");
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

                    JigPromptAngleOptions pao = new JigPromptAngleOptions("\nEnter Angle Of Air post : ");
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

            protected override bool WorldDraw( Autodesk.AutoCAD.GraphicsInterface.WorldDraw draw)
            {

                Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;

                // SHOW POSITION VALUE

                Autodesk.AutoCAD.GraphicsInterface.WorldGeometry  wg2 = draw.Geometry as Autodesk.AutoCAD.GraphicsInterface.WorldGeometry;

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


        //~~~~~~~~~~~~~~~~~~~~~~~~~ methods ~~~~~~~~~~~~~~~~~~//

        public AcDrawAirPost()
        {
            //_eJackPanelMiddles = new List<Atend.Base.Equipment.EJAckPanel>();
            _eJackPanelWeeks = new List<Atend.Base.Equipment.EJackPanelWeek>();
            _eTransformers = new List<Atend.Base.Equipment.ETransformer>();
            //_eJackPanelMiddleExistance = new ArrayList();
            _eJackPanelWeekExistance = new ArrayList();
            _eTransformerExistance = new ArrayList();
            PostEquipInserted = new List<PostEquips>();
        }

        public void DrawAirPost()
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            double w, h;
            Entity PostEntity = null;
            Dictionary<Guid, Atend.Base.Equipment.EJackPanelCell> MJCells = new Dictionary<Guid, Atend.Base.Equipment.EJackPanelCell>();

            double MyScale = Atend.Base.Design.DProductProperties.AccessSelectBySoftwareCode((int)Atend.Control.Enum.ProductType.AirPost).Scale;
            double MyCommentScale = Atend.Base.Design.DProductProperties.AccessSelectBySoftwareCode((int)Atend.Control.Enum.ProductType.AirPost).CommentScale;

            Atend.Global.Design.frmPostSize FPS = new Atend.Global.Design.frmPostSize();
            if (Application.ShowModalDialog(FPS) == System.Windows.Forms.DialogResult.OK)
            {
                h = FPS.Tol;
                w = FPS.Arz;
                //________________________________________________
                // All data was getting well
                DrawAirPostJig drawGroundPostJig = new DrawAirPostJig(w, h, MyScale);
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

                            if (SaveAirPostData())
                            {

                                //Draw Post
                                ObjectId oi = Atend.Global.Acad.UAcad.DrawEntityOnScreen(drawGroundPostJig.GetEntity(), Atend.Control.Enum.AutoCadLayerName.POST.ToString());
                                PostEntity = Atend.Global.Acad.UAcad.GetEntityByObjectID(oi);

                                Guid CurrentPostCodinDPost = Guid.Empty;
                                int ProductCode = 0;
                                PostEquips ForDelete = null;

                                foreach (PostEquips p in PostEquipInserted)
                                {
                                    if (p.ProductType == (int)Atend.Control.Enum.ProductType.AirPost)
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
                                    postInfo.NodeType = (int)Atend.Control.Enum.ProductType.AirPost;
                                    postInfo.ProductCode = ProductCode;
                                    postInfo.Insert();

                                    string comment = string.Format("Air Post {0} KVA", eAirPost.Capacity);
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

        public bool SaveAirPostData()
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;


            //ed.WriteMessage("\n~~~~~~~~~~~~~ Start Save AirPost Data ~~~~~~~~~~~~~~~~~~\n");
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
                        //ed.WriteMessage("go for not useaccess \n");
                        if (!eAirPost.AccessInsert(aTransaction, aConnection, true, true))
                        {
                            throw new System.Exception("eAirPost.AccessInsert failed");
                        }


                        Atend.Base.Equipment.EContainerPackage CurrentPostContainerPack = Atend.Base.Equipment.EContainerPackage.AccessSelectByContainerCodeAndType(eAirPost.Code, (int)Atend.Control.Enum.ProductType.AirPost, aTransaction, aConnection);
                        if (CurrentPostContainerPack.Code != -1)
                        {
                            //ed.WriteMessage("post was found \n");
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


                                ////////if (Convert.ToInt32(dr["TableType"]) == (int)Atend.Control.Enum.ProductType.MiddleJackPanel)
                                ////////{
                                ////////    for (int i1 = 1; i1 <= Convert.ToInt32(dr["Count"]); i1++)
                                ////////    {
                                ////////        Atend.Base.Equipment.EJAckPanel EM = Atend.Base.Equipment.EJAckPanel.AccessSelectByCode(Convert.ToInt32(dr["ProductCode"]), aTransaction, aConnection);
                                ////////        if (EM.Code != -1)
                                ////////        {
                                ////////            eJackPanelMiddles.Add(EM);
                                ////////        }
                                ////////    }
                                ////////}

                            }
                        }



                        //ed.WriteMessage("go for jw \n");
                        ////////////foreach (Atend.Base.Equipment.EJackPanelWeek selecteweekjack in eJackPanelWeeks)
                        ////////////{

                        ////////////    Atend.Base.Equipment.EAutoKey_3p Key = Atend.Base.Equipment.EAutoKey_3p.SelectByXCode(selecteweekjack.AutoKey3pXCode);
                        ////////////    if (Key.Code != -1)
                        ////////////    {
                        ////////////        if (!Key.AccessInsert(aTransaction, aConnection, true, true))
                        ////////////        {
                        ////////////            throw new System.Exception("Key.AccessInsert");
                        ////////////        }
                        ////////////    }
                        ////////////    else
                        ////////////    {
                        ////////////        throw new System.Exception("Key was not found");
                        ////////////    }


                        ////////////    selecteweekjack.AutoKey3pCode = Key.Code;
                        ////////////    if (!selecteweekjack.AccessInsert(aTransaction, aConnection, true, true))
                        ////////////    {
                        ////////////        throw new System.Exception("selectesweekjack.accessinsert failed");
                        ////////////    }
                        ////////////    else
                        ////////////    {
                        ////////////        //insert cell by xcode
                        ////////////        System.Data.DataTable CellsList = Atend.Base.Equipment.EJackPanelWeekCell.SelectByJackPanelWeekXCode(selecteweekjack.XCode);
                        ////////////        ed.WriteMessage("dtDellSList.Count={0}\n",CellsList.Rows.Count);
                        ////////////        foreach (DataRow dr in CellsList.Rows)
                        ////////////        {

                        ////////////            Atend.Base.Equipment.EJackPanelWeekCell selectedjackpanelweekcell = new Atend.Base.Equipment.EJackPanelWeekCell();
                        ////////////            selectedjackpanelweekcell.IsNightLight = Convert.ToBoolean(dr["IsNightLight"]);
                        ////////////            selectedjackpanelweekcell.XCode = new Guid(dr["XCode"].ToString());
                        ////////////            selectedjackpanelweekcell.JackPanelWeekCode = selecteweekjack.Code;
                        ////////////            selectedjackpanelweekcell.Num = Convert.ToInt32(dr["Num"]);

                        ////////////            if (!selectedjackpanelweekcell.AccessInsert(aTransaction, aConnection, true,true))
                        ////////////            {
                        ////////////                throw new System.Exception("selectedjackpanelweekcell.accessinsert \n");
                        ////////////            }

                        ////////////            //WENT TO
                        ////////////            //if (!Atend.Base.Equipment.EContainerPackage.SentFromLocalToAccess(selectedjackpanelweekcell.XCode, (int)Atend.Control.Enum.ProductType.Cell, selectedjackpanelweekcell.Code, aTransaction, aConnection))
                        ////////////            //{
                        ////////////            //    throw new System.Exception("SentFromLocalToAccess failed");
                        ////////////            //}


                        ////////////        }

                        ////////////    }
                        ////////////}

                        //ed.WriteMessage("go for tf \n");
                        ////////foreach (Atend.Base.Equipment.ETransformer SelectedTransformer in eTransformers)
                        ////////{
                        ////////    if (!SelectedTransformer.AccessInsert(aTransaction, aConnection, true, true))
                        ////////    {
                        ////////        throw new System.Exception("SelectedTransformer.AccessInsert failed");
                        ////////    }
                        ////////}


                    }//they are not in access




                    //DPost
                    //ed.WriteMessage("go for dpost \n");
                    Atend.Base.Design.DPost dPost = new Atend.Base.Design.DPost();
                    dPost.Number = "N001";
                    dPost.ProductCode = eAirPost.Code;
                    if (!dPost.AccessInsert(aTransaction, aConnection))
                    {
                        throw new System.Exception("dPost.AccessInsert failed");
                    }

                    //DPackage
                    //ed.WriteMessage("go for dpackage \n");
                    Atend.Base.Design.DPackage dPackPost = new Atend.Base.Design.DPackage();
                    dPackPost.NodeCode = dPost.Code;
                    dPackPost.Count = 1;
                    dPackPost.Type = (int)Atend.Control.Enum.ProductType.AirPost; //gp.Type;//?? eGroundPost.Type;
                    dPackPost.ProductCode = eAirPost.Code;
                    dPackPost.Number = "POST000";
                    dPackPost.IsExistance = Existance;
                    dPackPost.ProjectCode = ProjectCode;
                    dPackPost.LoadCode = 0;
                    if (!dPackPost.AccessInsert(aTransaction, aConnection))
                    {
                        throw new System.Exception("dPackPost.AccessInsert failed");
                    }
                    else
                    {
                        // add to inserted
                        PostEquips pe = new PostEquips();
                        pe.CodeGuid = dPackPost.Code;
                        pe.ProductCode = eAirPost.Code;
                        pe.ProductType = (int)Atend.Control.Enum.ProductType.AirPost;
                        PostEquipInserted.Add(pe);
                    }


                    //insert transformer
                    //ed.WriteMessage("go for transformer : existance count : {0} projectcount:{1} : {2} \n",
                    //eTransformerExistance.Count, eTransformerProjectCode.Count, eTransformers.Count);
                    int j = 0;
                    foreach (Atend.Base.Equipment.ETransformer transformer in eTransformers)
                    {
                        Atend.Base.Design.DPackage transformerPackage = new Atend.Base.Design.DPackage();
                        transformerPackage.Count = 1;
                        transformerPackage.IsExistance = Convert.ToInt32(eTransformerExistance[j]);
                        transformerPackage.ProjectCode = Convert.ToInt32(eTransformerProjectCode[j]);
                        transformerPackage.ParentCode = dPackPost.Code;
                        transformerPackage.ProductCode = transformer.Code;
                        transformerPackage.Number = "TRANS000";
                        transformerPackage.LoadCode = 0;
                        transformerPackage.Type = (int)Atend.Control.Enum.ProductType.Transformer;
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
                    //ed.WriteMessage("TRAN FINSH\n");

                    //insert weekJackpanel
                    j = 0;
                    foreach (Atend.Base.Equipment.EJackPanelWeek jackpanelweek in eJackPanelWeeks)
                    {
                        Atend.Base.Design.DPackage jackpanelweekPackage = new Atend.Base.Design.DPackage();
                        jackpanelweekPackage.Count = 1;
                        jackpanelweekPackage.IsExistance = Convert.ToInt32(eJackPanelWeekExistance[j]);
                        jackpanelweekPackage.ParentCode = dPackPost.Code;
                        jackpanelweekPackage.ProductCode = jackpanelweek.Code;
                        jackpanelweekPackage.Type = (int)Atend.Control.Enum.ProductType.WeekJackPanel;
                        jackpanelweekPackage.ProjectCode = Convert.ToInt32(eJackPanelWeekProjectCode[j]);
                        jackpanelweekPackage.Number = "WEEK000";
                        jackpanelweekPackage.LoadCode = 0;

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

                    //ed.WriteMessage("WJ FINSH\n");

                    //if (!UseAccess)
                    //{
                    //WENT TO
                    //if (!Atend.Base.Equipment.EContainerPackage.SentFromLocalToAccess(eAirPost.XCode, (int)Atend.Control.Enum.ProductType.AirPost, eAirPost.Code, aTransaction, aConnection))
                    //{
                    //    throw new System.Exception("SentFromLocalToAccess failed");
                    //}
                    //}


                    //////foreach (PostEquips p in PostEquipInserted)
                    //////{
                    //////    //ed.WriteMessage("~~~~~~~~~~~~~~~~~~~~~~~~~~\n");
                    //////    //ed.WriteMessage("CodeGuid:{0}\nParentCode:{1}\nProductCode:{2}\nProductType:{3}\n", p.CodeGuid, p.ParentCode, p.ProductCode, p.ProductType);
                    //////    //ed.WriteMessage("~~~~~~~~~~~~~~~~~~~~~~~~~~\n");
                    //////}

                }
                catch (System.Exception ex1)
                {
                    ed.WriteMessage("ERROR SaveAirPostData: 02 {0} \n", ex1.Message);
                    aTransaction.Rollback();
                    aConnection.Close();
                    return false;
                }
            }
            catch (System.Exception ex)
            {
                ed.WriteMessage("ERROR SaveAirPostData: 01 {0} \n", ex.Message);
                aConnection.Close();
                return false;
            }
            aTransaction.Commit();
            aConnection.Close();

            #region Not to Get Green

            Atend.Base.Acad.AcadGlobal.AirPostData.UseAccess = true;
            //UseAccess = true;

            #endregion


            //ed.WriteMessage("~~~~~~~~~~~~~ End Save AirPost Data ~~~~~~~~~~~~~~~~~~\n");
            return true;


        }

        /*
        public Entity DrawAirPost(int AirPostProductCode)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            double w, h, s = 0.9;
            Entity PostEntity = null;

            ////////////////////PromptDoubleOptions pdo = new PromptDoubleOptions("Enter Ground Post Width : ");
            ////////////////////PromptDoubleResult pdr = ed.GetDouble(pdo);

            ////////////////////if (pdr.Status == PromptStatus.OK)
            ////////////////////{
            ////////////////////    w = pdr.Value;
            ////////////////////    pdo.Message = "Enter Ground Post Heigth : ";
            ////////////////////    pdr = ed.GetDouble(pdo);

            ////////////////////    if (pdr.Status == PromptStatus.OK)
            ////////////////////    {

            ////////////////////h = pdr.Value;


            //////////////////Atend.Design.frmPostSize FPS = new Atend.Design.frmPostSize();
            //////////////////if (FPS.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //////////////////{
            //////////////////    h = FPS.Tol;
            //////////////////    w = FPS.Arz;
            //////////////////    //________________________________________________
            //////////////////    // All data was getting well
            //////////////////    Atend.Global.Acad.AcadJigs.DrawAirPostJig drawGroundPostJig = new Atend.Global.Acad.AcadJigs.DrawAirPostJig(w, h, s);
            //////////////////    bool Conti = true;

            //////////////////    while (Conti)
            //////////////////    {
            //////////////////        PromptResult pr =
            //////////////////        ed.Drag(drawGroundPostJig);

            //////////////////        if (pr.Status == PromptStatus.OK)
            //////////////////        {
            //////////////////            //Conti = false;
            //////////////////            drawGroundPostJig.GetAngle = true;
            //////////////////            pr = ed.Drag(drawGroundPostJig);

            //////////////////            if (pr.Status == PromptStatus.OK)
            //////////////////            {
            //////////////////                Conti = false;
            //////////////////                //ed.WriteMessage("Time to save data \n ");

            //////////////////                #region SaveDataHere

            //////////////////                if (SaveAirPostData(AirPostProductCode))
            //////////////////                {

            //////////////////                    ObjectId oi = Atend.Global.Acad.UAcad.DrawEntityOnScreen(drawGroundPostJig.GetEntity(), Atend.Control.Enum.AutoCadLayerName.POST.ToString());
            //////////////////                    PostEntity = Atend.Global.Acad.UAcad.GetEntityByObjectID(oi);

            //////////////////                    Guid CurrentPostCodinDPost = new Guid();
            //////////////////                    //EXTRA
            //////////////////                    //int ProductCode = 0;
            //////////////////                    Guid ProductCode = Guid.Empty;
            //////////////////                    foreach (Atend.Base.Acad.AcadGlobal.AirPostData.PostEquips p in Atend.Base.Acad.AcadGlobal.AirPostData.PostEquipInserted)
            //////////////////                    {
            //////////////////                        if (p.ProductType == (int)Atend.Control.Enum.ProductType.AirPost)
            //////////////////                        {
            //////////////////                            CurrentPostCodinDPost = p.CodeGuid;
            //////////////////                            ProductCode = p.ProductCode;
            //////////////////                        }
            //////////////////                    }

            //////////////////                    Atend.Base.Acad.AT_INFO postInfo = new Atend.Base.Acad.AT_INFO(oi);
            //////////////////                    postInfo.ParentCode = "";
            //////////////////                    postInfo.NodeCode = CurrentPostCodinDPost.ToString();
            //////////////////                    postInfo.NodeType = (int)Atend.Control.Enum.ProductType.AirPost;
            //////////////////                    postInfo.ProductCode = ProductCode;
            //////////////////                    postInfo.Insert();

            //////////////////                    ObjectId DrawnText = Atend.Global.Acad.UAcad.DrawEntityOnScreen(Atend.Global.Acad.Global.WriteNoteMText("PostInformation \r\n\rSecond Information",
            //////////////////                        Atend.Global.Acad.UAcad.CenterOfEntity(drawGroundPostJig.GetEntity())), Atend.Control.Enum.AutoCadLayerName.GENERAL.ToString());
            //////////////////                    //ed.WriteMessage("text drawn \n");
            //////////////////                    Atend.Base.Acad.AT_INFO textInfo = new Atend.Base.Acad.AT_INFO(DrawnText);
            //////////////////                    textInfo.ParentCode = CurrentPostCodinDPost.ToString();
            //////////////////                    textInfo.NodeCode = "";
            //////////////////                    textInfo.NodeType = (int)Atend.Control.Enum.ProductType.Comment;
            //////////////////                    textInfo.ProductCode = 0;
            //////////////////                    textInfo.Insert();

            //////////////////                    //ed.WriteMessage("information saved for text \n");
            //////////////////                    Atend.Base.Acad.AT_SUB PostSub = new Atend.Base.Acad.AT_SUB(oi);
            //////////////////                    PostSub.SubIdCollection.Add(DrawnText);
            //////////////////                    PostSub.Insert();
            //////////////////                    //ed.WriteMessage("information saved for post \n");
            //////////////////                }
            //////////////////                #endregion
            //////////////////                //ed.WriteMessage("Entity Drawn on the screen . \n");

            //////////////////            }
            //////////////////            //else
            //////////////////            //{
            //////////////////            //    Conti = false;
            //////////////////            //    ed.WriteMessage("Drawing failed \n");
            //////////////////            //}
            //////////////////        }

            //////////////////    }
            //////////////////    //_________________________________________
            //////////////////}
            ////////////////////    }
            ////////////////////    else
            ////////////////////    {
            ////////////////////        ed.WriteMessage("\nError while getting data from user.\n");
            ////////////////////    }
            ////////////////////}
            ////////////////////else
            ////////////////////{
            ////////////////////    ed.WriteMessage("\nError while getting data from user.\n");
            ////////////////////}
            return PostEntity;
        }

        private bool SaveAirPostData(int AirPostProductCode)
        {
            //Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;


            //ed.WriteMessage("~~~~~~~~~~~~~ Start Save AirPost Data ~~~~~~~~~~~~~~~~~~\n");
            //OleDbConnection aConnection = new OleDbConnection(Atend.Control.ConnectionString.AccessCnString);
            //OleDbTransaction aTransaction;
            //try
            //{
            //    aConnection.Open();
            //    aTransaction = aConnection.BeginTransaction();
            //    try
            //    {
            //        if (_UseAccess == false)
            //        {
            //            //ePost
            //            if (!eAirPost.AccessInsert(aTransaction, aConnection))
            //            {
            //                throw new System.Exception("EAirPost.AccessInsert failed");
            //            }
            //            int i = 0;
            //            foreach (Atend.Base.Equipment.EJackPanelWeek SelectedJAckPanelWeek in eJackPanelWeeks)
            //            {
            //                SelectedJAckPanelWeek.XCode = AirPostXCode;
            //                if (Convert.ToBoolean(eJackPanelWeeksUseAccess[i]) == false)
            //                {
            //                    if (!SelectedJAckPanelWeek.AccessInsert(aTransaction, aConnection))
            //                    {
            //                        throw new System.Exception("SelectedJAckPanelWeek.AccessInsert failed");
            //                    }
            //                }
            //                i++;
            //            }
            //            //eWeekJackPanelCell
            //            //foreach (Atend.Base.Equipment.EJackPanelWeekCell SelectedJAckPanelWeekCell in eJackPanelWeekCells)
            //            //{
            //            //    SelectedJAckPanelWeekCell.XCode = AirPostXCode;
            //            //    if (Convert.ToBoolean(eJackPanelWeekCellsUseAccess[i]) == false)
            //            //    {
            //            //        if (!SelectedJAckPanelWeekCell.AccessInsert(aTransaction, aConnection))
            //            //        {
            //            //            throw new System.Exception("SelectedJAckPanelWeekCell.AccessInsert failed");
            //            //        }
            //            //    }
            //            //}
            //            //eTransformer
            //            i = 0;
            //            foreach (Atend.Base.Equipment.ETransformer SelectedTransformer in eTransformers)
            //            {
            //                SelectedTransformer.XCode = AirPostXCode;
            //                if (Convert.ToBoolean(eTransformersUseAccess[i]) == false)
            //                {
            //                    if (!SelectedTransformer.AccessInsert(aTransaction, aConnection))
            //                    {
            //                        throw new System.Exception("SelectedTransformer.AccessInsert failed");
            //                    }
            //                }
            //                i++;
            //            }

            //        }//endif (_UseAccess == false)
            //        //DPackage
            //        //AirPostInfo = new Atend.Base.Design.DPackage();
            //        ////AirPostInfo.NodeCode = AirPostXCode;
            //        //AirPostInfo.Count = 1;
            //        //AirPostInfo.ParentCode = gp.XCode;//OR?? eAirPost.XCode;
            //        //AirPostInfo.Type = gp.Type;//?? eAirPost.Type;
            //        //AirPostInfo.ProductCode = gp.ProductCode;//?? eAirPost.ProductCode;
            //        //AirPostInfo.IsExistance = Existance;
            //        if (!AirPostInfo.AccessInsert(aTransaction, aConnection))
            //        {
            //            throw new System.Exception("DPackage.AccessInsert failed");
            //        }
            //        //DPost
            //        //DPost = new Atend.Base.Design.DPost();
            //        //DPost.Number = "N001";
            //        //DPost.ProductCode = eAirPost.Code;
            //        if (!DPost.AccessInsert(aTransaction, aConnection))
            //        {
            //            throw new System.Exception("DPost.AccessInsert failed");
            //        }
            //        //DCellStatus
            //        DCellStatus = new Atend.Base.Design.DCellStatus();
            //        ///// ??
            //        if (!DCellStatus.AccessInsert(aTransaction, aConnection))
            //        {
            //            throw new System.Exception("DCellStatus.AccessInsert failed");
            //        }


            //        int j = 0;
            //        foreach (Atend.Base.Equipment.ETransformer transformer in eTransformers)
            //        {
            //            Atend.Base.Design.DPackage transformerPackage = new Atend.Base.Design.DPackage();
            //            transformerPackage.Count = Convert.ToInt32(eTransformerCount[j]);
            //            transformerPackage.IsExistance = Convert.ToByte(eTransformerExistance[j]);
            //            transformerPackage.ParentCode = AirPostXCode;
            //            //transformerPackage.ProductCode = ; ??
            //            //transformerPackage.NodeCode=; ??
            //            transformerPackage.Type = (int)Atend.Control.Enum.ProductType.Transformer;
            //            if (!transformerPackage.AccessInsert(aTransaction, aConnection))
            //            {
            //                throw new System.Exception("transformerPackage.AccessInsert failed");
            //            }
            //            else
            //            {
            //                dPackages.Add(transformerPackage);
            //            }
            //            j++;
            //        }

            //        j = 0;
            //        foreach (Atend.Base.Equipment.EJAckPanel jackpanelmiddle in eJackPanelMiddles)
            //        {
            //            Atend.Base.Design.DPackage jackpanelmiddlePackage = new Atend.Base.Design.DPackage();
            //            jackpanelmiddlePackage.Count = Convert.ToInt32(eJackPanelMiddleCount[j]);
            //            jackpanelmiddlePackage.IsExistance = Convert.ToByte(eJackPanelMiddleExistance[j]);
            //            jackpanelmiddlePackage.ParentCode = AirPostXCode;
            //            //jackpanelmiddlePackage.ProductCode = ; ??
            //            //jackpanelmiddlePackage.NodeCode=; ??
            //            jackpanelmiddlePackage.Type = (int)Atend.Control.Enum.ProductType.MiddleJackPanel;
            //            if (!jackpanelmiddlePackage.AccessInsert(aTransaction, aConnection))
            //            {
            //                throw new System.Exception("jackpanelmiddlePackage.AccessInsert failed");
            //            }
            //            else
            //            {
            //                dPackages.Add(jackpanelmiddlePackage);
            //            }
            //            j++;
            //        }

            //        j = 0;
            //        foreach (Atend.Base.Equipment.EJackPanelWeek jackpanelweek in eJackPanelWeeks)
            //        {
            //            Atend.Base.Design.DPackage jackpanelweekPackage = new Atend.Base.Design.DPackage();
            //            jackpanelweekPackage.Count = Convert.ToInt32(eJackPanelWeekCount[j]);
            //            jackpanelweekPackage.IsExistance = Convert.ToByte(eJackPanelWeekExistance[j]);
            //            jackpanelweekPackage.ParentCode = AirPostXCode;
            //            //jackpanelweekPackage.ProductCode = ; ??
            //            //jackpanelweekPackage.NodeCode=; ??
            //            jackpanelweekPackage.Type = (int)Atend.Control.Enum.ProductType.WeekJackPanel;
            //            if (!jackpanelweekPackage.AccessInsert(aTransaction, aConnection))
            //            {
            //                throw new System.Exception("jackpanelweekPackage.AccessInsert failed");
            //            }
            //            else
            //            {
            //                dPackages.Add(jackpanelweekPackage);
            //            }
            //            j++;
            //        }

            //    }
            //    catch (System.Exception ex1)
            //    {
            //        ed.WriteMessage("ERROR SaveAirPostData: 02 {0} \n", ex1.Message);
            //        aTransaction.Rollback();
            //        aConnection.Close();
            //        return false;
            //    }
            //}
            //catch (System.Exception ex)
            //{
            //    ed.WriteMessage("ERROR SaveAirPostData: 01 {0} \n", ex.Message);
            //    aConnection.Close();
            //    return false;
            //}
            //aTransaction.Commit();
            //aConnection.Close();
            //ed.WriteMessage("~~~~~~~~~~~~~ End Save AirPost Data ~~~~~~~~~~~~~~~~~~\n");
            //return true;



        ////////////////    //if (Atend.Base.Design.NodeTransaction.InsertGroundPost(GroundPostProductCode))
        ////////////////    //{

        ////////////////    Atend.Base.Design.NodeTransaction.InsertAirPost(AirPostProductCode);
        ////////////////    return true;
        ////////////////    //}
        ////////////////    //else
        ////////////////    //{
        ////////////////    //    return false;
        ////////////////    //}

        ////////////////}

        ////////////////public static bool InsertAirPost(int PostCode)
        ////////////////{
        ////////////////    Editor edl = Application.DocumentManager.MdiActiveDocument.Editor;

        ////////////////    Atend.Base.Equipment.EAirPost APost = Atend.Base.Equipment.EAirPost.SelectByCode(PostCode);
        ////////////////    Atend.Base.Design.DPost DPost = new Atend.Base.Design.DPost();

        ////////////////    //DPost.DesignCode = Atend.Control.Common.SelectedDesignCode;
        ////////////////    DPost.Number = "N";
        ////////////////    DPost.ProductCode = APost.XCode;

        ////////////////    edl.WriteMessage("\n111\n");


        ////////////////    SqlConnection Connection = new SqlConnection(Atend.Control.ConnectionString.cnString);
        ////////////////    SqlTransaction Transaction;


        ////////////////    try
        ////////////////    {
        ////////////////        Connection.Open();
        ////////////////        Transaction = Connection.BeginTransaction();


        ////////////////        try
        ////////////////        {
        ////////////////            if (DPost.AccessInsert(Transaction, Connection))
        ////////////////            {
        ////////////////                edl.WriteMessage("DPost.Code= " + DPost.Code + "\n");
        ////////////////                Atend.Base.Design.DPackage PostPack = new Atend.Base.Design.DPackage();
        ////////////////                PostPack.NodeCode = DPost.Code;
        ////////////////                PostPack.Count = 1;
        ////////////////                PostPack.Type = Convert.ToInt32(Atend.Control.Enum.ProductType.AirPost);
        ////////////////                PostPack.ProductCode = DPost.ProductCode;


        ////////////////                edl.WriteMessage("\n112\n");
        ////////////////                Atend.Base.Acad.AcadGlobal.AirPostData.PostEquips Row = new Atend.Base.Acad.AcadGlobal.AirPostData.PostEquips();

        ////////////////                //DPackage PostPack = DPackage.SelectByNodeCode(DPost.Code);

        ////////////////                if (PostPack.Insert(Transaction, Connection))
        ////////////////                {
        ////////////////                    edl.WriteMessage("\n113\n");
        ////////////////                    Row.CodeGuid = PostPack.Code;
        ////////////////                    Row.ProductType = (int)Atend.Control.Enum.ProductType.AirPost;
        ////////////////                    Row.ProductCode = PostPack.ProductCode;

        ////////////////                    Atend.Base.Acad.AcadGlobal.AirPostData.PostEquipInserted.Add(Row);
        ////////////////                }
        ////////////////                else
        ////////////////                {
        ////////////////                    edl.WriteMessage("Error NodeTransaction.InsertAirPost : while saving DPackage For DPost \n");
        ////////////////                    throw new exception("while saving DPost in DPackage ");
        ////////////////                }

        ////////////////                Atend.Base.Equipment.EContainerPackage CPackage = Atend.Base.Equipment.EContainerPackage.selectByContainerCodeAndType(APost.Code, (int)Atend.Control.Enum.ProductType.AirPost);
        ////////////////                System.Data.DataTable ProductTbl = Atend.Base.Equipment.EProductPackage.SelectByContainerPackageCode(CPackage.Code);

        ////////////////                edl.WriteMessage("\n114\n");
        ////////////////                foreach (DataRow ProductRow in ProductTbl.Rows)
        ////////////////                {
        ////////////////                    edl.WriteMessage("\n115\n");
        ////////////////                    switch ((Atend.Control.Enum.ProductType)Convert.ToInt16(ProductRow["TableType"].ToString()))
        ////////////////                    {

        ////////////////                        case Atend.Control.Enum.ProductType.Transformer:

        ////////////////                            //edl.WriteMessage("\n116\n");
        ////////////////                            Atend.Base.Equipment.ETransformer Trans = Atend.Base.Equipment.ETransformer.SelectByCode(Convert.ToInt32(ProductRow["ProductCode"].ToString()));
        ////////////////                            for (int i = 0; i < Convert.ToInt32(ProductRow["Count"].ToString()); i++)
        ////////////////                            {
        ////////////////                                Atend.Base.Design.DPackage Pack = new Atend.Base.Design.DPackage();
        ////////////////                                Pack.ParentCode = PostPack.Code;
        ////////////////                                Pack.Count = 1;// Convert.ToInt32(ProductRow["Count"].ToString());
        ////////////////                                Pack.ProductCode = Trans.XCode;
        ////////////////                                Pack.Type = (int)Atend.Control.Enum.ProductType.Transformer;

        ////////////////                                if (Pack.AccessInsert(Transaction, Connection))
        ////////////////                                {
        ////////////////                                    //edl.WriteMessage("\n117\n");
        ////////////////                                    Atend.Base.Acad.AcadGlobal.AirPostData.PostEquips RTrans = new Atend.Base.Acad.AcadGlobal.AirPostData.PostEquips();
        ////////////////                                    RTrans.CodeGuid = Pack.Code;
        ////////////////                                    RTrans.ParentCode = Pack.ParentCode;
        ////////////////                                    RTrans.ProductCode = Pack.ProductCode;
        ////////////////                                    RTrans.ProductType = (int)Atend.Control.Enum.ProductType.Transformer;

        ////////////////                                    Atend.Base.Acad.AcadGlobal.AirPostData.PostEquipInserted.Add(RTrans);

        ////////////////                                    //Atend.Base.Equipment.EContainerPackage ContP = Atend.Base.Equipment.EContainerPackage.selectByContainerCodeAndType(Trans.Code, (int)Atend.Control.Enum.ProductType.Transformer);

        ////////////////                                    //if (!SubProduct(ContP.Code, (int)Atend.Control.Enum.ProductType.Transformer, Pack.ParentCode, Transaction, Connection))
        ////////////////                                    //{
        ////////////////                                    //    edl.WriteMessage("Error NodeTransaction.InsertGroundPost : while saving Sub Products \n");
        ////////////////                                    //    throw new Exception("while saving Sub Products in DPackage ");
        ////////////////                                    //}

        ////////////////                                    if (!SubProduct(Convert.ToInt32(ProductRow["ContainerPackageCode"].ToString()), Convert.ToInt32(ProductRow["TableType"].ToString()), Pack.ParentCode, Transaction, Connection))
        ////////////////                                    {
        ////////////////                                        edl.WriteMessage("Error NodeTransaction.InsertAirPost : while saving Sub Products \n");
        ////////////////                                        throw new exception("while saving Sub Products in DPackage ");
        ////////////////                                    }
        ////////////////                                }
        ////////////////                                else
        ////////////////                                {
        ////////////////                                    edl.WriteMessage("Error NodeTransaction.InsertAirPost : while saving DPackage For Transformer \n");
        ////////////////                                    throw new exception("while saving Transformer in DPackage ");
        ////////////////                                }

        ////////////////                            }
        ////////////////                            break;

        ////////////////                        case Atend.Control.Enum.ProductType.MiddleJackPanel:

        ////////////////                            //edl.WriteMessage("\n118\n");
        ////////////////                            Atend.Base.Equipment.EJAckPanel JackPanel = Atend.Base.Equipment.EJAckPanel.SelectByCode(Convert.ToInt32(ProductRow["ProductCode"].ToString()));

        ////////////////                            for (int i = 0; i < Convert.ToInt32(ProductRow["Count"].ToString()); i++)
        ////////////////                            {
        ////////////////                                Atend.Base.Design.DPackage JackPack = new DPackage();
        ////////////////                                JackPack.ParentCode = PostPack.Code;
        ////////////////                                JackPack.Count = 1;//Convert.ToInt32(ProductRow["Count"].ToString());
        ////////////////                                JackPack.Type = (int)Atend.Control.Enum.ProductType.MiddleJackPanel;
        ////////////////                                JackPack.ProductCode = JackPanel.XCode;

        ////////////////                                if (JackPack.Insert(Transaction, Connection))
        ////////////////                                {
        ////////////////                                    //edl.WriteMessage("\n119\n");
        ////////////////                                    Atend.Base.Acad.AcadGlobal.PostEquips RJack = new Atend.Base.Acad.AcadGlobal.PostEquips();
        ////////////////                                    RJack.CodeGuid = JackPack.Code;
        ////////////////                                    RJack.ParentCode = JackPack.ParentCode;
        ////////////////                                    RJack.ProductCode = JackPack.ProductCode;
        ////////////////                                    RJack.ProductType = (int)Atend.Control.Enum.ProductType.MiddleJackPanel;

        ////////////////                                    Atend.Base.Acad.AcadGlobal.PostEquipInserted.Add(RJack);
        ////////////////                                }
        ////////////////                                else
        ////////////////                                {
        ////////////////                                    edl.WriteMessage("Error NodeTransaction.InsertAirPost : while saving DPackage For MiddleJackPanel \n");
        ////////////////                                    throw new Exception("while saving MiddleJackPanel in DPackage ");
        ////////////////                                }

        ////////////////                                DataTable JackTbl = Atend.Base.Equipment.EJackPanelCell.SelectByJackPanelCode(JackPanel.Code);

        ////////////////                                //edl.WriteMessage("\n120\n");
        ////////////////                                foreach (DataRow JackRow in JackTbl.Rows)
        ////////////////                                {

        ////////////////                                    DPackage P = new DPackage();
        ////////////////                                    P.Count = 1;// Convert.ToInt32(Dr["Count"].ToString());
        ////////////////                                    P.ParentCode = JackPack.Code;
        ////////////////                                    P.Type = (int)Atend.Control.Enum.ProductType.Cell;//Convert.ToInt32(Dr["Type"].ToString());
        ////////////////                                    //edl.WriteMessage("~~~~Cell productCode:{0}~~~~\n", JackRow["ProductCode"].ToString());
        ////////////////                                    P.ProductCode = new Guid(JackRow["ProductCode"].ToString());

        ////////////////                                    if (P.Insert(Transaction, Connection))
        ////////////////                                    {
        ////////////////                                        //edl.WriteMessage("\n125\n");
        ////////////////                                        Atend.Base.Acad.AcadGlobal.PostEquips RCell = new Atend.Base.Acad.AcadGlobal.PostEquips();
        ////////////////                                        RCell.CodeGuid = P.Code;
        ////////////////                                        RCell.ParentCode = P.ParentCode;
        ////////////////                                        RCell.ProductCode = P.ProductCode;
        ////////////////                                        RCell.ProductType = (int)Atend.Control.Enum.ProductType.Cell;

        ////////////////                                        Atend.Base.Acad.AcadGlobal.PostEquipInserted.Add(RCell);
        ////////////////                                    }
        ////////////////                                    else
        ////////////////                                    {
        ////////////////                                        edl.WriteMessage("Error NodeTransaction.InsertAirPost : while saving DPackage For Cell \n");
        ////////////////                                        throw new Exception("while saving Cell in DPackage");
        ////////////////                                    }

        ////////////////                                    //edl.WriteMessage("\n121\n");
        ////////////////                                    Atend.Base.Equipment.EContainerPackage CP = Atend.Base.Equipment.EContainerPackage.selectByContainerCodeAndType(Convert.ToInt32(JackRow["ProductCode"].ToString()), (int)Atend.Control.Enum.ProductType.Cell);
        ////////////////                                    DataTable Tbl = Atend.Base.Equipment.EProductPackage.SelectByContainerPackageCode(CP.Code);

        ////////////////                                    edl.WriteMessage("\n122\n");
        ////////////////                                    foreach (DataRow Dr in Tbl.Rows)
        ////////////////                                    {
        ////////////////                                        //edl.WriteMessage("\n123\n");
        ////////////////                                        DPackage P1 = new DPackage();
        ////////////////                                        P1.Count = Convert.ToInt32(Dr["Count"].ToString());
        ////////////////                                        P1.ParentCode = P.Code; //JackPack.Code;
        ////////////////                                        P1.Type = Convert.ToInt32(Dr["TableType"].ToString());
        ////////////////                                        P1.ProductCode = new Guid(Dr["ProductCode"].ToString());


        ////////////////                                        //edl.WriteMessage("\n124\n");
        ////////////////                                        if (P1.Insert(Transaction, Connection))
        ////////////////                                        {
        ////////////////                                            //    //edl.WriteMessage("\n125\n");
        ////////////////                                            //    Atend.Base.Acad.AcadGlobal.PostEquips RCell = new Atend.Base.Acad.AcadGlobal.PostEquips();
        ////////////////                                            //    RCell.CodeGuid = P.Code;
        ////////////////                                            //    RCell.ParentCode = P.ParentCode;
        ////////////////                                            //    RCell.ProductCode = P.ProductCode;
        ////////////////                                            //    RCell.ProductType = (int)Atend.Control.Enum.ProductType.Cell;

        ////////////////                                            //Atend.Base.Acad.AcadGlobal.PostEquipInserted.Add(RCell);

        ////////////////                                            if (!SubProduct(Convert.ToInt32(Dr["ProductCode"].ToString()), Convert.ToInt32(Dr["TableType"].ToString()), P1.Code, Transaction, Connection))
        ////////////////                                            {
        ////////////////                                                edl.WriteMessage("Error NodeTransaction.InsertAirPost : while saving Sub Products \n");
        ////////////////                                                throw new Exception("while saving Sub Products in DPackage ");
        ////////////////                                            }

        ////////////////                                        }
        ////////////////                                        else
        ////////////////                                        {
        ////////////////                                            edl.WriteMessage("Error NodeTransaction.InsertAirPost : while saving DPackage For Cell \n");
        ////////////////                                            throw new Exception("while saving Cell in DPackage");
        ////////////////                                        }

        ////////////////                                        //edl.WriteMessage("\n126\n");
        ////////////////                                        DCellStatus CellSt = new DCellStatus();
        ////////////////                                        CellSt.CellCode = P.Code;
        ////////////////                                        CellSt.JackPanelCode = JackPack.Code;
        ////////////////                                        CellSt.IsClosed = false;
        ////////////////                                        if (CellSt.Insert(Transaction, Connection))
        ////////////////                                        { }
        ////////////////                                        else
        ////////////////                                        {
        ////////////////                                            edl.WriteMessage("Error NodeTransaction.InsertGroundPost : while saving Cell Status For \n");
        ////////////////                                            throw new Exception("while saving Cell Status");
        ////////////////                                        }
        ////////////////                                    }
        ////////////////                                }
        ////////////////                            }
        ////////////////                            break;

        ////////////////                        case Atend.Control.Enum.ProductType.WeekJackPanel:

        ////////////////                            //edl.WriteMessage("\n127\n");
        ////////////////                            Atend.Base.Equipment.EJackPanelWeek JackPanelW = Atend.Base.Equipment.EJackPanelWeek.SelectByCode(Convert.ToInt32(ProductRow["ProductCode"].ToString()));

        ////////////////                            for (int i = 0; i < Convert.ToInt32(ProductRow["Count"].ToString()); i++)
        ////////////////                            {
        ////////////////                                Atend.Base.Design.DPackage JackPackW = new DPackage();
        ////////////////                                JackPackW.ParentCode = PostPack.Code;
        ////////////////                                JackPackW.Count = 1;//Convert.ToInt32(ProductRow["Count"].ToString());
        ////////////////                                JackPackW.Type = (int)Atend.Control.Enum.ProductType.WeekJackPanel;
        ////////////////                                JackPackW.ProductCode = JackPanelW.XCode;

        ////////////////                                //edl.WriteMessage("\n128\n");
        ////////////////                                if (JackPackW.Insert(Transaction, Connection))
        ////////////////                                {
        ////////////////                                    //edl.WriteMessage("\n129\n");
        ////////////////                                    Atend.Base.Acad.AcadGlobal.PostEquips RJPW = new Atend.Base.Acad.AcadGlobal.PostEquips();
        ////////////////                                    RJPW.CodeGuid = JackPackW.Code;
        ////////////////                                    RJPW.ParentCode = JackPackW.ParentCode;
        ////////////////                                    RJPW.ProductCode = JackPackW.ProductCode;
        ////////////////                                    RJPW.ProductType = (int)Atend.Control.Enum.ProductType.WeekJackPanel;

        ////////////////                                    Atend.Base.Acad.AcadGlobal.PostEquipInserted.Add(RJPW);
        ////////////////                                }
        ////////////////                                else
        ////////////////                                {
        ////////////////                                    edl.WriteMessage("Error NodeTransaction.InsertAirPost : while saving DPackage For WeekJackPanel \n");
        ////////////////                                    throw new Exception("while saving WeekJack Panel in DPackage ");
        ////////////////                                }


        ////////////////                                DataTable JackTblW = Atend.Base.Equipment.EJackPanelWeekCell.SelectByJackPanelWeekCode(JackPanelW.Code);
        ////////////////                                //edl.WriteMessage("\n130\n");

        ////////////////                                foreach (DataRow JackRow in JackTblW.Rows)
        ////////////////                                {

        ////////////////                                    DPackage P = new DPackage();
        ////////////////                                    P.Count = 1;// Convert.ToInt32(Dr["Count"].ToString());
        ////////////////                                    P.ParentCode = JackPackW.Code;
        ////////////////                                    P.Type = (int)Atend.Control.Enum.ProductType.Cell;
        ////////////////                                    P.ProductCode = new Guid(JackRow["Code"].ToString());

        ////////////////                                    //edl.WriteMessage("\n134\n");

        ////////////////                                    if (P.Insert(Transaction, Connection))
        ////////////////                                    { }
        ////////////////                                    else
        ////////////////                                    {
        ////////////////                                        edl.WriteMessage("Error NodeTransaction.InsertAirPost : while saving DPackage For Cell \n");
        ////////////////                                        throw new Exception("while saving Cell in DPackage ");
        ////////////////                                    }
        ////////////////                                    // edl.WriteMessage("\n131\n");

        ////////////////                                    Atend.Base.Equipment.EContainerPackage CP = Atend.Base.Equipment.EContainerPackage.selectByContainerCodeAndType(Convert.ToInt32(JackRow["Code"].ToString()), (int)Atend.Control.Enum.ProductType.Cell);
        ////////////////                                    DataTable Tbl = Atend.Base.Equipment.EProductPackage.SelectByContainerPackageCode(CP.Code);

        ////////////////                                    //edl.WriteMessage("\n132\n");

        ////////////////                                    foreach (DataRow Dr in Tbl.Rows)
        ////////////////                                    {
        ////////////////                                        //edl.WriteMessage("\n133\n");

        ////////////////                                        DPackage P1 = new DPackage();
        ////////////////                                        P1.Count = Convert.ToInt32(Dr["Count"].ToString());
        ////////////////                                        P1.ParentCode = P.Code;
        ////////////////                                        P1.Type = Convert.ToInt32(Dr["TableType"].ToString());
        ////////////////                                        P1.ProductCode = new Guid(Dr["ProductCode"].ToString());

        ////////////////                                        ////edl.WriteMessage("\n134\n");

        ////////////////                                        if (P1.Insert(Transaction, Connection))
        ////////////////                                        {
        ////////////////                                            //edl.WriteMessage("\n135\n");
        ////////////////                                            //Atend.Base.Acad.AcadGlobal.PostEquips RC = new Atend.Base.Acad.AcadGlobal.PostEquips();
        ////////////////                                            //RC.CodeGuid = P.Code;
        ////////////////                                            //RC.ParentCode = P.ParentCode;
        ////////////////                                            //RC.ProductCode = P.ProductCode;
        ////////////////                                            //RC.ProductType = (int)Atend.Control.Enum.ProductType.Cell;

        ////////////////                                            //Atend.Base.Acad.AcadGlobal.PostEquipInserted.Add(RC);
        ////////////////                                            if (!SubProduct(Convert.ToInt32(Dr["ProductCode"].ToString()), Convert.ToInt32(Dr["TableType"].ToString()), P1.Code, Transaction, Connection))
        ////////////////                                            {
        ////////////////                                                edl.WriteMessage("Error NodeTransaction.InsertAirPost : while saving Sub Products \n");
        ////////////////                                                throw new Exception("while saving Sub Products in DPackage ");
        ////////////////                                            }
        ////////////////                                        }
        ////////////////                                        else
        ////////////////                                        {
        ////////////////                                            edl.WriteMessage("Error NodeTransaction.InsertAirPost : while saving DPackage For Cell \n");
        ////////////////                                            throw new Exception("while saving Cell in DPackage ");
        ////////////////                                        }

        ////////////////                                    }
        ////////////////                                }
        ////////////////                            }
        ////////////////                            break;

        ////////////////                        default:

        ////////////////                            //edl.WriteMessage("\n136\n");

        ////////////////                            Atend.Base.Design.DPackage Package = new DPackage();
        ////////////////                            Package.ParentCode = PostPack.Code;
        ////////////////                            Package.Count = Convert.ToInt32(ProductRow["Count"].ToString());
        ////////////////                            Package.ProductCode = new Guid(ProductRow["ProductCode"].ToString());
        ////////////////                            Package.Type = Convert.ToInt32(ProductRow["TableType"].ToString());

        ////////////////                            //edl.WriteMessage("\n137\n");

        ////////////////                            if (Package.Insert(Transaction, Connection))
        ////////////////                            {
        ////////////////                                //edl.WriteMessage("\n138\n");

        ////////////////                                //Atend.Base.Acad.AcadGlobal.PostEquips RD = new Atend.Base.Acad.AcadGlobal.PostEquips();
        ////////////////                                //RD.CodeGuid = Package.Code;
        ////////////////                                //RD.ParentCode = Package.ParentCode;
        ////////////////                                //RD.ProductCode = Package.ProductCode;
        ////////////////                                //RD.ProductType = (int)Atend.Control.Enum.ProductType.Cell;

        ////////////////                                //Atend.Base.Acad.AcadGlobal.PostEquipInserted.Add(RD);

        ////////////////                                if (!SubProduct(Convert.ToInt32(ProductRow["ProductCode"].ToString()), Convert.ToInt32(ProductRow["TableType"].ToString()), Package.Code, Transaction, Connection))
        ////////////////                                {
        ////////////////                                    edl.WriteMessage("Error NodeTransaction.InsertAirPost : while saving Sub Products \n");
        ////////////////                                    throw new Exception("while saving Sub Products in DPackage ");
        ////////////////                                }
        ////////////////                            }
        ////////////////                            else
        ////////////////                            {
        ////////////////                                edl.WriteMessage("Error NodeTransaction.InsertAirPost : while saving DPackage For Default \n");
        ////////////////                                throw new Exception("while saving Default in DPackage ");
        ////////////////                            }
        ////////////////                            break;


        ////////////////                        //case Atend.Control.Enum.ProductType.Transformer:

        ////////////////                        //    edl.WriteMessage("\n116\n");
        ////////////////                        //    Atend.Base.Equipment.ETransformer Trans = Atend.Base.Equipment.ETransformer.SelectByCode(Convert.ToInt32(ProductRow["ProductCode"].ToString()));

        ////////////////                        //    for (int i = 0; i < Convert.ToInt32(ProductRow["Count"].ToString()); i++)
        ////////////////                        //    {
        ////////////////                        //        Atend.Base.Design.DPackage Pack = new DPackage();
        ////////////////                        //        Pack.ParentCode = PostPack.Code;
        ////////////////                        //        Pack.Count = 1;//Convert.ToInt32(ProductRow["Count"].ToString());
        ////////////////                        //        Pack.ProductCode = Trans.Code;
        ////////////////                        //        Pack.Type = (int)Atend.Control.Enum.ProductType.Transformer;

        ////////////////                        //        if (Pack.Insert(Transaction, Connection))
        ////////////////                        //        {
        ////////////////                        //            edl.WriteMessage("\n117\n");
        ////////////////                        //            Atend.Base.Acad.AcadGlobal.PostEquips RTrans = new Atend.Base.Acad.AcadGlobal.PostEquips();
        ////////////////                        //            RTrans.CodeGuid = Pack.Code;
        ////////////////                        //            RTrans.ParentCode = Pack.ParentCode;
        ////////////////                        //            RTrans.ProductCode = Pack.ProductCode;
        ////////////////                        //            RTrans.ProductType = (int)Atend.Control.Enum.ProductType.Transformer;

        ////////////////                        //            Atend.Base.Acad.AcadGlobal.PostEquipInserted.Add(RTrans);

        ////////////////                        //            if (!SubProduct(Convert.ToInt32(ProductRow["ContainerPackageCode"].ToString()), Convert.ToInt32(ProductRow["TableType"].ToString()), Pack.ParentCode, Transaction, Connection))
        ////////////////                        //            {
        ////////////////                        //                edl.WriteMessage("Error NodeTransaction.InsertGroundPost : while saving Sub Products \n");
        ////////////////                        //                throw new Exception("while saving Sub Products in DPackage ");
        ////////////////                        //            }
        ////////////////                        //        }
        ////////////////                        //        else
        ////////////////                        //        {
        ////////////////                        //            edl.WriteMessage("Error NodeTransaction.InsertAirPost : while saving DPackage For Transformer \n");
        ////////////////                        //            throw new Exception("while saving Transformer in DPackage ");
        ////////////////                        //        }
        ////////////////                        //    }

        ////////////////                        //    break;

        ////////////////                        //case Atend.Control.Enum.ProductType.MiddleJackPanel:

        ////////////////                        //    edl.WriteMessage("\n118\n");
        ////////////////                        //    Atend.Base.Equipment.EJAckPanel JackPanel = Atend.Base.Equipment.EJAckPanel.SelectByCode(Convert.ToInt32(ProductRow["ProductCode"].ToString()));

        ////////////////                        //    for (int i = 0; i < Convert.ToInt32(ProductRow["Count"].ToString()); i++)
        ////////////////                        //    {
        ////////////////                        //        Atend.Base.Design.DPackage JackPack = new DPackage();
        ////////////////                        //        JackPack.ParentCode = PostPack.Code;
        ////////////////                        //        JackPack.Count = 1;//Convert.ToInt32(ProductRow["Count"].ToString());
        ////////////////                        //        JackPack.Type = (int)Atend.Control.Enum.ProductType.MiddleJackPanel;
        ////////////////                        //        JackPack.ProductCode = JackPanel.Code;

        ////////////////                        //        if (JackPack.Insert(Transaction, Connection))
        ////////////////                        //        {
        ////////////////                        //            edl.WriteMessage("\n119\n");
        ////////////////                        //            Atend.Base.Acad.AcadGlobal.PostEquips RJack = new Atend.Base.Acad.AcadGlobal.PostEquips();
        ////////////////                        //            RJack.CodeGuid = JackPack.Code;
        ////////////////                        //            RJack.ParentCode = JackPack.ParentCode;
        ////////////////                        //            RJack.ProductCode = JackPack.ProductCode;
        ////////////////                        //            RJack.ProductType = (int)Atend.Control.Enum.ProductType.MiddleJackPanel;

        ////////////////                        //            Atend.Base.Acad.AcadGlobal.PostEquipInserted.Add(RJack);
        ////////////////                        //        }
        ////////////////                        //        else
        ////////////////                        //        {
        ////////////////                        //            edl.WriteMessage("Error NodeTransaction.InsertAirPost : while saving DPackage For MiddleJackPanel \n");
        ////////////////                        //            throw new Exception("while saving MiddleJackPanel in DPackage ");
        ////////////////                        //        }


        ////////////////                        //        DataTable JackTbl = Atend.Base.Equipment.EJackPanelCell.SelectByJackPanelCode(JackPanel.Code);

        ////////////////                        //        edl.WriteMessage("\n120\n");
        ////////////////                        //        foreach (DataRow JackRow in JackTbl.Rows)
        ////////////////                        //        {
        ////////////////                        //            DPackage P = new DPackage();
        ////////////////                        //            P.Count = 1;// Convert.ToInt32(Dr["Count"].ToString());
        ////////////////                        //            P.ParentCode = JackPack.Code;
        ////////////////                        //            P.Type = (int)Atend.Control.Enum.ProductType.Cell;//Convert.ToInt32(Dr["Type"].ToString());
        ////////////////                        //            P.ProductCode = Convert.ToInt32(JackRow["ProductCode"].ToString());

        ////////////////                        //            if (P.Insert(Transaction, Connection))
        ////////////////                        //            {
        ////////////////                        //                //edl.WriteMessage("\n125\n");
        ////////////////                        //                Atend.Base.Acad.AcadGlobal.PostEquips RCell = new Atend.Base.Acad.AcadGlobal.PostEquips();
        ////////////////                        //                RCell.CodeGuid = P.Code;
        ////////////////                        //                RCell.ParentCode = P.ParentCode;
        ////////////////                        //                RCell.ProductCode = P.ProductCode;
        ////////////////                        //                RCell.ProductType = (int)Atend.Control.Enum.ProductType.Cell;

        ////////////////                        //                Atend.Base.Acad.AcadGlobal.PostEquipInserted.Add(RCell);
        ////////////////                        //            }
        ////////////////                        //            else
        ////////////////                        //            {
        ////////////////                        //                edl.WriteMessage("Error NodeTransaction.InsertGroundPost : while saving DPackage For Cell \n");
        ////////////////                        //                throw new Exception("while saving Cell in DPackage");
        ////////////////                        //            }

        ////////////////                        //            edl.WriteMessage("\n121\n");
        ////////////////                        //            Atend.Base.Equipment.EContainerPackage CP = Atend.Base.Equipment.EContainerPackage.selectByContainerCodeAndType(Convert.ToInt32(JackRow["ProductCode"].ToString()), (int)Atend.Control.Enum.ProductType.Cell);
        ////////////////                        //            DataTable Tbl = Atend.Base.Equipment.EProductPackage.SelectByContainerPackageCode(CP.Code);

        ////////////////                        //            edl.WriteMessage("\n122\n");
        ////////////////                        //            foreach (DataRow Dr in Tbl.Rows)
        ////////////////                        //            {
        ////////////////                        //                edl.WriteMessage("\n123\n");
        ////////////////                        //                //DPackage P = new DPackage();
        ////////////////                        //                //P.Count = 1;// Convert.ToInt32(Dr["Count"].ToString());
        ////////////////                        //                //P.ParentCode = JackPack.Code;
        ////////////////                        //                //P.Type = (int)Atend.Control.Enum.ProductType.Cell;
        ////////////////                        //                //P.ProductCode = Convert.ToInt32(Dr["ProductCode"].ToString());

        ////////////////                        //                //edl.WriteMessage("\n124\n");
        ////////////////                        //                //if (P.Insert(Transaction, Connection))
        ////////////////                        //                //{
        ////////////////                        //                //    edl.WriteMessage("\n125\n");
        ////////////////                        //                //    Atend.Base.Acad.AcadGlobal.PostEquips RCell = new Atend.Base.Acad.AcadGlobal.PostEquips();
        ////////////////                        //                //    RCell.CodeGuid = P.Code;
        ////////////////                        //                //    RCell.ParentCode = P.ParentCode;
        ////////////////                        //                //    RCell.ProductCode = P.ProductCode;
        ////////////////                        //                //    RCell.ProductType = (int)Atend.Control.Enum.ProductType.Cell;

        ////////////////                        //                //    Atend.Base.Acad.AcadGlobal.PostEquipInserted.Add(RCell);

        ////////////////                        //                if (!SubProduct(Convert.ToInt32(Dr["ContainerPackageCode"].ToString()), (int)Atend.Control.Enum.ProductType.Cell, P.ParentCode, Transaction, Connection))
        ////////////////                        //                {
        ////////////////                        //                    edl.WriteMessage("Error NodeTransaction.InsertGroundPost : while saving Sub Products \n");
        ////////////////                        //                    throw new Exception("while saving Sub Products in DPackage ");
        ////////////////                        //                }

        ////////////////                        //                //}
        ////////////////                        //                //else
        ////////////////                        //                //{
        ////////////////                        //                //    edl.WriteMessage("Error NodeTransaction.InsertAirPost : while saving DPackage For Cell \n");
        ////////////////                        //                //    throw new Exception("while saving Cell in DPackage");
        ////////////////                        //                //}

        ////////////////                        //                edl.WriteMessage("\n126\n");
        ////////////////                        //                DCellStatus CellSt = new DCellStatus();
        ////////////////                        //                CellSt.CellCode = P.Code;
        ////////////////                        //                CellSt.JackPanelCode = JackPack.Code;
        ////////////////                        //                CellSt.IsClosed = false;
        ////////////////                        //                if (CellSt.Insert(Transaction, Connection))
        ////////////////                        //                { }
        ////////////////                        //                else
        ////////////////                        //                {
        ////////////////                        //                    edl.WriteMessage("Error NodeTransaction.InsertAirPost : while saving Cell Status For \n");
        ////////////////                        //                    throw new Exception("while saving Cell Status");
        ////////////////                        //                }
        ////////////////                        //            }
        ////////////////                        //        }
        ////////////////                        //    }
        ////////////////                        //    break;

        ////////////////                        //case Atend.Control.Enum.ProductType.WeekJackPanel:

        ////////////////                        //    edl.WriteMessage("\n127\n");
        ////////////////                        //    Atend.Base.Equipment.EJackPanelWeek JackPanelW = Atend.Base.Equipment.EJackPanelWeek.SelectByCode(Convert.ToInt32(ProductRow["ProductCode"].ToString()));

        ////////////////                        //    for (int i = 0; i < Convert.ToInt32(ProductRow["Count"].ToString()); i++)
        ////////////////                        //    {
        ////////////////                        //        Atend.Base.Design.DPackage JackPackW = new DPackage();
        ////////////////                        //        JackPackW.ParentCode = PostPack.Code;
        ////////////////                        //        JackPackW.Count = 1;//Convert.ToInt32(ProductRow["Count"].ToString());
        ////////////////                        //        JackPackW.Type = (int)Atend.Control.Enum.ProductType.WeekJackPanel;
        ////////////////                        //        JackPackW.ProductCode = JackPanelW.Code;

        ////////////////                        //        edl.WriteMessage("\n128\n");
        ////////////////                        //        if (JackPackW.Insert(Transaction, Connection))
        ////////////////                        //        {
        ////////////////                        //            edl.WriteMessage("\n129\n");
        ////////////////                        //            Atend.Base.Acad.AcadGlobal.PostEquips RJPW = new Atend.Base.Acad.AcadGlobal.PostEquips();
        ////////////////                        //            RJPW.CodeGuid = JackPackW.Code;
        ////////////////                        //            RJPW.ParentCode = JackPackW.ParentCode;
        ////////////////                        //            RJPW.ProductCode = JackPackW.ProductCode;
        ////////////////                        //            RJPW.ProductType = (int)Atend.Control.Enum.ProductType.WeekJackPanel;

        ////////////////                        //            Atend.Base.Acad.AcadGlobal.PostEquipInserted.Add(RJPW);
        ////////////////                        //        }
        ////////////////                        //        else
        ////////////////                        //        {
        ////////////////                        //            edl.WriteMessage("Error NodeTransaction.InsertAirPost : while saving DPackage For WeekJackPanel \n");
        ////////////////                        //            throw new Exception("while saving WeekJack Panel in DPackage ");
        ////////////////                        //        }

        ////////////////                        //        DataTable JackTblW = Atend.Base.Equipment.EJackPanelWeekCell.SelectByJackPanelWeekCode(JackPanelW.Code);
        ////////////////                        //        edl.WriteMessage("\n130\n");

        ////////////////                        //        foreach (DataRow JackRow in JackTblW.Rows)
        ////////////////                        //        {
        ////////////////                        //            DPackage P = new DPackage();
        ////////////////                        //            P.Count = 1;// Convert.ToInt32(Dr["Count"].ToString());
        ////////////////                        //            P.ParentCode = JackPackW.Code;
        ////////////////                        //            P.Type = (int)Atend.Control.Enum.ProductType.Cell;
        ////////////////                        //            P.ProductCode = Convert.ToInt32(JackRow["Code"].ToString());

        ////////////////                        //            //edl.WriteMessage("\n134\n");

        ////////////////                        //            if (P.Insert(Transaction, Connection))
        ////////////////                        //            { }
        ////////////////                        //            else
        ////////////////                        //            {
        ////////////////                        //                edl.WriteMessage("Error NodeTransaction.InsertGroundPost : while saving DPackage For Cell \n");
        ////////////////                        //                throw new Exception("while saving Cell in DPackage ");
        ////////////////                        //            }

        ////////////////                        //            edl.WriteMessage("\n131\n");

        ////////////////                        //            Atend.Base.Equipment.EContainerPackage CP = Atend.Base.Equipment.EContainerPackage.selectByContainerCodeAndType(Convert.ToInt32(JackRow["Code"].ToString()), (int)Atend.Control.Enum.ProductType.Cell);
        ////////////////                        //            DataTable Tbl = Atend.Base.Equipment.EProductPackage.SelectByContainerPackageCode(CP.Code);

        ////////////////                        //            edl.WriteMessage("\n132\n");

        ////////////////                        //            foreach (DataRow Dr in Tbl.Rows)
        ////////////////                        //            {
        ////////////////                        //                edl.WriteMessage("\n133\n");

        ////////////////                        //                //DPackage P = new DPackage();
        ////////////////                        //                //P.Count = 1;// Convert.ToInt32(Dr["Count"].ToString());
        ////////////////                        //                //P.ParentCode = JackPackW.Code;
        ////////////////                        //                //P.Type = (int)Atend.Control.Enum.ProductType.Cell;
        ////////////////                        //                //P.ProductCode = Convert.ToInt32(Dr["ProductCode"].ToString());

        ////////////////                        //                //edl.WriteMessage("\n134\n");

        ////////////////                        //                //if (P.Insert(Transaction, Connection))
        ////////////////                        //                //{
        ////////////////                        //                //    edl.WriteMessage("\n135\n");

        ////////////////                        //                //Atend.Base.Acad.AcadGlobal.PostEquips RC = new Atend.Base.Acad.AcadGlobal.PostEquips();
        ////////////////                        //                //RC.CodeGuid = P.Code;
        ////////////////                        //                //RC.ParentCode = P.ParentCode;
        ////////////////                        //                //RC.ProductCode = P.ProductCode;
        ////////////////                        //                //RC.ProductType = (int)Atend.Control.Enum.ProductType.Cell;

        ////////////////                        //                //Atend.Base.Acad.AcadGlobal.PostEquipInserted.Add(RC);
        ////////////////                        //                if (!SubProduct(Convert.ToInt32(Dr["ContainerPackageCode"].ToString()), (int)Atend.Control.Enum.ProductType.Cell, P.ParentCode, Transaction, Connection))
        ////////////////                        //                {
        ////////////////                        //                    edl.WriteMessage("Error NodeTransaction.InsertGroundPost : while saving Sub Products \n");
        ////////////////                        //                    throw new Exception("while saving Sub Products in DPackage ");
        ////////////////                        //                }

        ////////////////                        //                //}
        ////////////////                        //                //else
        ////////////////                        //                //{
        ////////////////                        //                //    edl.WriteMessage("Error NodeTransaction.InsertAirPost : while saving DPackage For Cell \n");
        ////////////////                        //                //    throw new Exception("while saving Cell in DPackage ");
        ////////////////                        //                //}

        ////////////////                        //            }
        ////////////////                        //        }
        ////////////////                        //    }
        ////////////////                        //    break;

        ////////////////                        //default:

        ////////////////                        //    edl.WriteMessage("\n136\n");

        ////////////////                        //    Atend.Base.Design.DPackage Package = new DPackage();
        ////////////////                        //    Package.ParentCode = PostPack.Code;
        ////////////////                        //    Package.Count = Convert.ToInt32(ProductRow["Count"].ToString());
        ////////////////                        //    Package.ProductCode = Convert.ToInt32(ProductRow["ProductCode"].ToString());
        ////////////////                        //    Package.Type = Convert.ToInt32(ProductRow["Type"].ToString());

        ////////////////                        //    edl.WriteMessage("\n137\n");

        ////////////////                        //    if (Package.Insert(Transaction, Connection))
        ////////////////                        //    {
        ////////////////                        //        edl.WriteMessage("\n138\n");

        ////////////////                        //        Atend.Base.Acad.AcadGlobal.PostEquips RD = new Atend.Base.Acad.AcadGlobal.PostEquips();
        ////////////////                        //        RD.CodeGuid = Package.Code;
        ////////////////                        //        RD.ParentCode = Package.ParentCode;
        ////////////////                        //        RD.ProductCode = Package.ProductCode;
        ////////////////                        //        RD.ProductType = (int)Atend.Control.Enum.ProductType.Cell;

        ////////////////                        //        Atend.Base.Acad.AcadGlobal.PostEquipInserted.Add(RD);

        ////////////////                        //        if (!SubProduct(Convert.ToInt32(ProductRow["ContainerPackageCode"].ToString()), Convert.ToInt32(ProductRow["TableType"].ToString()), Package.ParentCode, Transaction, Connection))
        ////////////////                        //        {
        ////////////////                        //            edl.WriteMessage("Error NodeTransaction.InsertGroundPost : while saving Sub Products \n");
        ////////////////                        //            throw new Exception("while saving Sub Products in DPackage ");
        ////////////////                        //        }
        ////////////////                        //    }
        ////////////////                        //    else
        ////////////////                        //    {
        ////////////////                        //        edl.WriteMessage("Error NodeTransaction.InsertAirPost : while saving DPackage For Default \n");
        ////////////////                        //        throw new Exception("while saving Default in DPackage ");
        ////////////////                        //    }
        ////////////////                        //    break;


        ////////////////                    }
        ////////////////                }

        ////////////////                //if (PostPack.Type == (int)Atend.Control.Enum.ProductType.AirPost)
        ////////////////                //{
        ////////////////                //    //Atend.Base.Design.DPackage Pack = Atend.Base.Design.DPackage.SelectByCode(PR.CodeGuid);
        ////////////////                //    DataTable OprTable = Atend.Base.Equipment.EOperation.SelectByProductCodeType(PostPack.ProductCode, (int)Atend.Control.Enum.ProductType.GroundPost);

        ////////////////                //    foreach (DataRow OprRow in OprTable.Rows)
        ////////////////                //    {
        ////////////////                //        Atend.Base.Design.DPackage Package = new DPackage();
        ////////////////                //        Package.ParentCode = PostPack.Code;
        ////////////////                //        Package.Count = 1;
        ////////////////                //        Package.Type = (int)Atend.Control.Enum.ProductType.Operation;
        ////////////////                //        Package.ProductCode = Convert.ToInt32(OprRow["ProductID"].ToString());

        ////////////////                //        if (!Package.Insert(Transaction, Connection))
        ////////////////                //        {
        ////////////////                //            edl.WriteMessage("Error NodeTransaction.InsertAirPost : while saving DPackage For Operation \n");
        ////////////////                //            throw new Exception("while saving Operation in DPackage ");
        ////////////////                //        }
        ////////////////                //    }

        ////////////////                //}
        ////////////////            }
        ////////////////            else
        ////////////////            {
        ////////////////                edl.WriteMessage("Error NodeTransaction.InsertAirPost : while saving DPost  \n");
        ////////////////                throw new Exception("while saving DPost");
        ////////////////            }

        ////////////////        }
        ////////////////        catch (System.Exception ex2)
        ////////////////        {
        ////////////////            edl.WriteMessage(string.Format("Error NodeTransaction.InsertAirPost  : {0} \n", ex2.Message));
        ////////////////            Transaction.Rollback();
        ////////////////            return false;
        ////////////////        }

        ////////////////    }
        ////////////////    catch (System.Exception ex1)
        ////////////////    {
        ////////////////        edl.WriteMessage(string.Format("Error NodeTransaction.InsertAirPost  : {0} \n", ex1.Message));
        ////////////////        Connection.Close();
        ////////////////        return false;
        ////////////////    }

        ////////////////    foreach (Atend.Base.Acad.AcadGlobal.PostEquips PR in Atend.Base.Acad.AcadGlobal.PostEquipInserted)
        ////////////////    {
        ////////////////        edl.WriteMessage("\nPackageCode = " + PR.CodeGuid.ToString() + "   ProductCode = " + PR.ProductCode.ToString() + "   Type = " + PR.ProductType.ToString() + "\n");

        ////////////////    }


        ////////////////    foreach (Atend.Base.Acad.AcadGlobal.PostEquips p in Atend.Base.Acad.AcadGlobal.PostEquipInserted)
        ////////////////    {
        ////////////////        edl.WriteMessage("PC:{0},\nCG:{1},\nPT:{2},\nPC:{3}\n", p.ParentCode, p.CodeGuid, p.ProductType, p.ProductCode);
        ////////////////        edl.WriteMessage("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\n");
        ////////////////    }


        ////////////////    Transaction.Commit();
        ////////////////    Connection.Close();

            return true;
        }
        */

        public static bool DeleteAirPostData(ObjectId AirPostOI)
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
                    Atend.Base.Acad.AT_SUB SubGP = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(AirPostOI);
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

                    Atend.Base.Acad.AT_INFO conductorinfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(AirPostOI);
                    Atend.Base.Design.DPackage package = Atend.Base.Design.DPackage.AccessSelectByCode(new Guid(conductorinfo.NodeCode.ToString()));

                    //Delete From Post
                    if (!Atend.Base.Design.DPost.AccessDelete(package.NodeCode, _Transaction, _Connection))
                    {
                        throw new System.Exception("Error In Delete DPost\n");
                    }

                    //delete AirPost
                    if (!Atend.Base.Design.DPackage.AccessDelete(package.Code, _Transaction, _Connection))
                    {
                        throw new System.Exception("Error In Delete GroundPost in DPackage\n");
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
                    ed.WriteMessage("GRA ERROR AirPost(Transaction) : {0} \n", ex.Message);
                    _Transaction.Rollback();
                    return false;
                }
            }
            catch (System.Exception ex1)
            {
                ed.WriteMessage(string.Format("Error in AirPost Transaction : {0} \n", ex1.Message));
                _Connection.Close();
                return false;
            }

            _Transaction.Commit();
            _Connection.Close();
            return true;
        }

        public static bool DeleteAirPost(ObjectId AirPostOI)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            try
            {
                Atend.Base.Acad.AT_SUB airSub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(AirPostOI);
                foreach (ObjectId oi in airSub.SubIdCollection)
                {
                    ObjectIdCollection Collection = Atend.Global.Acad.UAcad.GetGroupSubEntities(oi);
                    bool ismove = false;
                    ArrayList arrIsMoveCabel = new ArrayList();
                    ObjectIdCollection headerobji = new ObjectIdCollection();
                    foreach (ObjectId collect in Collection)
                    {
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
                                            Polyline p1 = Atend.Global.Acad.UAcad.GetEntityByObjectID(AirPostOI) as Polyline;
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
                    }

                    if (!Atend.Global.Acad.AcadRemove.DeleteEntityByObjectId(oi))
                    {
                        throw new System.Exception("can not remove entity");
                    }
                }

                //+++
                if (!Atend.Global.Acad.AcadRemove.DeleteEntityByObjectId(AirPostOI))
                {
                    throw new System.Exception("can not remove GroundPost");
                }


            }
            catch (System.Exception ex)
            {
                ed.WriteMessage("Error DeleteAirPost : {0} \n", ex.Message);
                return false;
            }
            //ed.WriteMessage("DELETE AirPost\n");
            return true;
        }

        public bool UpdateAirPostData(Guid EXCode)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            try
            {
                if (DeleteAirPostData(selectedObjectId))
                {
                    if (!DeleteAirPost(selectedObjectId))
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
                //Atend.Design.frmEditDrawAirPost02.AirPost.DrawAirPost();
                //DrawAirPost();
                return true;

            }
            catch (System.Exception ex)
            {
                ed.WriteMessage("ERROR UpdateAirPost 01 : {0} \n", ex.Message);
                return false;
            }

            //OOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO
            //    Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            //    OleDbTransaction aTransaction;
            //    OleDbConnection aConnection = new OleDbConnection(Atend.Control.ConnectionString.AccessCnString);
            //    try
            //    {
            //        aConnection.Open();
            //        aTransaction = aConnection.BeginTransaction();
            //        try
            //        {
            //            AirPostPack = Atend.Base.Design.DPackage.AccessSelectByCode(EXCode);
            //            if (!UseAccess)
            //            {
            //                if (!eAirPost.AccessInsert(aTransaction, aConnection, true))
            //                {
            //                    throw new System.Exception("eAirPost.AccessInsert failed");
            //                }
            //            }

            //            //DPost
            //            Atend.Base.Design.DPost dPost = new Atend.Base.Design.DPost();
            //            dPost.Number = "N001";
            //            dPost.ProductCode = eAirPost.Code;
            //            if (!dPost.AccessInsert(aTransaction, aConnection))
            //            {
            //                throw new System.Exception("dPost.AccessInsert failed");
            //            }

            //            //DPackage
            //            Atend.Base.Design.DPackage dPackPost = new Atend.Base.Design.DPackage();
            //            dPackPost.NodeCode = dPost.Code;
            //            dPackPost.Count = 1;
            //            dPackPost.Type = (int)Atend.Control.Enum.ProductType.AirPost;
            //            dPackPost.ProductCode = eAirPost.Code;
            //            dPackPost.IsExistance = Existance;
            //            dPackPost.ProjectCode = ProjectCode;
            //            dPackPost.Number = "";
            //            if (!dPackPost.AccessInsert(aTransaction, aConnection))
            //            {
            //                throw new System.Exception("dPackPost.AccessInsert failed");
            //            }

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

            //            //insert weekJackpanel
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
            //            ed.WriteMessage("ERROR UpdateAirPost 01(transaction) : {0} \n", ex1.Message);
            //            aTransaction.Rollback();
            //            aConnection.Close();
            //            return false;
            //        }
            //        aTransaction.Commit();

            //        if (DeleteAirPostData(selectedObjectId))
            //        {
            //            if (!DeleteAirPost(selectedObjectId))
            //            {
            //                throw new System.Exception("Error in Delete Graphic");
            //            }
            //            if (!Atend.Global.Acad.AcadRemove.DeleteEntityByObjectId(selectedObjectId))
            //            {
            //                throw new System.Exception("can not remove AirPost");
            //            }
            //        }
            //        else
            //        {
            //            throw new System.Exception("Error in Delete Data");
            //        }
            //        DrawAirPost();
            //        aConnection.Close();
            //        return true;
            //    }
            //    catch (System.Exception ex)
            //    {
            //        ed.WriteMessage("ERROR UpdateAirPost 01 : {0} \n", ex.Message);
            //        aConnection.Close();
            //        return false;
            //    }

        }

        public bool UpdateAirPostWithoutDraw(Guid EXCode, System.Data.DataTable dtSuEquip)
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
                        }
                        else
                        {
                            throw new System.Exception("DPackage.Update_SUB2 failed");
                        }
                        //if (Convert.ToInt32(dr["Type"].ToString()) == Convert.ToInt32(Atend.Control.Enum.ProductType.Transformer))
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

        public static void ShowDescription(ObjectId oi, OleDbConnection _Conection)
        {

            int ProductCode = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oi).ProductCode;
            Entity CurrentEntity = Atend.Global.Acad.UAcad.GetEntityByObjectID(oi);
            double CommentScale = Atend.Base.Design.DProductProperties.AccessSelectBySoftwareCode((int)Atend.Control.Enum.ProductType.AirPost).Scale;
            Atend.Base.Equipment.EAirPost _EAirPost = Atend.Base.Equipment.EAirPost.AccessSelectByCode(ProductCode, _Conection);
            if (_EAirPost.Code != -1)
            {

                Point3d EntityCenterPoint = Atend.Global.Acad.UAcad.CenterOfEntity(CurrentEntity);
                Entity TextEntity = Atend.Global.Acad.UAcad.WriteNote(_EAirPost.Comment, EntityCenterPoint, CommentScale);
                Atend.Global.Acad.UAcad.DrawEntityOnScreen(TextEntity, Atend.Control.Enum.AutoCadLayerName.DESCRIPTION.ToString());


                Atend.Base.Acad.AT_INFO DescriptionInfo = new Atend.Base.Acad.AT_INFO(TextEntity.ObjectId);
                DescriptionInfo.ParentCode = "";
                DescriptionInfo.ProductCode = 0;
                DescriptionInfo.NodeCode = "";
                DescriptionInfo.NodeType = (int)Atend.Control.Enum.ProductType.Description;
                DescriptionInfo.Insert();

            }
        }

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
                        if (PostInfo.ParentCode != "NONE" && PostInfo.NodeType == (int)Atend.Control.Enum.ProductType.AirPost)
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
                                if (ES.Name.IndexOf("موجود") != -1)
                                {
                                    //ed.WriteMessage("MOJOOD \n");
                                    //ShieldForGroundType3(true, Objectid);
                                    ShieldForAirPost(true, Objectid);
                                }
                                else
                                {
                                    //ed.WriteMessage("PISHNAHADI \n");
                                    //ShieldForGroundType3(false, Objectid);
                                    ShieldForAirPost(false, Objectid);
                                }
                            }
                            else
                            {
                                Atend.Global.Acad.Notification notification = new Atend.Global.Acad.Notification();
                                notification.Title = "بروز خطا";
                                notification.Msg = "اطلاعات یکی از پست های هوایی در پایگاه داده یافت نشد";
                                notification.infoCenterBalloon();
                                throw new System.Exception("Post was not in DPackage Post");
                            }
                        }
                    }
                    ed.WriteMessage("PostAir Shield finished \n");
                }
            }
            catch (System.Exception ex)
            {
                ed.WriteMessage("ERROR :{0} \n", ex.Message);
            }

        }

        private static void ShieldForAirPost(bool IsExist, ObjectId oi)
        {
            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            Polyline pl = Atend.Global.Acad.UAcad.GetEntityByObjectID(oi) as Polyline;
            try
            {
                if (pl != null)
                {
                    LineSegment3d ls1 = new LineSegment3d(pl.GetPoint3dAt(0), pl.GetPoint3dAt(1));
                    LineSegment3d ls2 = new LineSegment3d(pl.GetPoint3dAt(1), pl.GetPoint3dAt(2));

                    double Radius = 0;
                    Point3d CenterPoint1 = Point3d.Origin;
                    Point3d CenterPoint2 = Point3d.Origin;

                    if (ls1.Length < ls2.Length)
                    {
                        Radius = ls1.Length / 3;

                        LineSegment3d ls3 = new LineSegment3d(pl.GetPoint3dAt(2), pl.GetPoint3dAt(3));
                        LineSegment3d ls4 = new LineSegment3d(pl.GetPoint3dAt(3), pl.GetPoint3dAt(4));


                        Vector3d Vect1 = ls1.EndPoint - ls1.StartPoint, norm1 = Vect1.GetNormal();
                        Vector3d Vect2 = ls2.EndPoint - ls2.StartPoint, norm2 = Vect2.GetNormal();
                        Vector3d Vect3 = ls3.EndPoint - ls3.StartPoint, norm3 = Vect3.GetNormal();
                        Vector3d Vect4 = ls4.EndPoint - ls4.StartPoint, norm4 = Vect4.GetNormal();

                        double Length1 = Vect1.Length / 2;
                        double Length2 = Vect2.Length / 3;
                        double Length3 = Vect3.Length / 2;
                        double Length4 = Vect4.Length / 3;

                        Point3d CenterPoint3 = ls1.StartPoint + (norm1 * Length1);

                        CenterPoint1 = ls2.StartPoint + (norm2 * Length2);
                        CenterPoint2 = ls2.StartPoint + (norm2 * Length2 * 2);

                        Point3d CenterPoint4 = ls3.StartPoint + (norm3 * Length3);

                        Point3d CenterPoint5 = ls4.StartPoint + (norm4 * Length4);
                        Point3d CenterPoint6 = ls4.StartPoint + (norm4 * Length4 * 2);



                        Ray r1 = new Ray();
                        r1.BasePoint = CenterPoint3;
                        r1.SecondPoint = CenterPoint4;

                        Ray r2 = new Ray();
                        r2.BasePoint = CenterPoint1;
                        r2.SecondPoint = CenterPoint6;


                        Ray r3 = new Ray();
                        r3.BasePoint = CenterPoint2;
                        r3.SecondPoint = CenterPoint5;

                        //Atend.Global.Acad.UAcad.DrawEntityOnScreen(r1, Atend.Control.Enum.AutoCadLayerName.SHELL.ToString());
                        //Atend.Global.Acad.UAcad.DrawEntityOnScreen(r2, Atend.Control.Enum.AutoCadLayerName.SHELL.ToString());
                        //Atend.Global.Acad.UAcad.DrawEntityOnScreen(r3, Atend.Control.Enum.AutoCadLayerName.SHELL.ToString());


                        Point3dCollection p3c1 = new Point3dCollection();
                        r1.IntersectWith(r2, Intersect.OnBothOperands, p3c1, 0, 0);


                        Point3dCollection p3c2 = new Point3dCollection();
                        r1.IntersectWith(r3, Intersect.OnBothOperands, p3c2, 0, 0);

                        if (p3c1.Count > 0 && p3c2.Count > 0)
                        {
                            CenterPoint1 = p3c1[0];
                            CenterPoint2 = p3c2[0];
                        }

                    }
                    else
                    {
                        Radius = ls2.Length / 3;


                        LineSegment3d ls3 = new LineSegment3d(pl.GetPoint3dAt(2), pl.GetPoint3dAt(3));
                        LineSegment3d ls4 = new LineSegment3d(pl.GetPoint3dAt(3), pl.GetPoint3dAt(4));


                        Vector3d Vect1 = ls1.EndPoint - ls1.StartPoint, norm1 = Vect1.GetNormal();
                        Vector3d Vect2 = ls2.EndPoint - ls2.StartPoint, norm2 = Vect2.GetNormal();
                        Vector3d Vect3 = ls3.EndPoint - ls3.StartPoint, norm3 = Vect3.GetNormal();
                        Vector3d Vect4 = ls4.EndPoint - ls4.StartPoint, norm4 = Vect4.GetNormal();

                        double Length1 = Vect1.Length / 3;
                        double Length2 = Vect2.Length / 2;
                        double Length3 = Vect3.Length / 3;
                        double Length4 = Vect4.Length / 2;

                        CenterPoint1 = ls1.StartPoint + (norm1 * Length1);
                        CenterPoint2 = ls1.StartPoint + (norm1 * Length1 * 2);

                        Point3d CenterPoint3 = ls2.StartPoint + (norm2 * Length2);

                        Point3d CenterPoint4 = ls3.StartPoint + (norm3 * Length3);
                        Point3d CenterPoint5 = ls3.StartPoint + (norm3 * Length3 * 2);

                        Point3d CenterPoint6 = ls4.StartPoint + (norm4 * Length4);



                        Ray r1 = new Ray();
                        r1.BasePoint = CenterPoint3;
                        r1.SecondPoint = CenterPoint6;

                        Ray r2 = new Ray();
                        r2.BasePoint = CenterPoint1;
                        r2.SecondPoint = CenterPoint5;


                        Ray r3 = new Ray();
                        r3.BasePoint = CenterPoint2;
                        r3.SecondPoint = CenterPoint4;

                        //Atend.Global.Acad.UAcad.DrawEntityOnScreen(r1, Atend.Control.Enum.AutoCadLayerName.SHELL.ToString());
                        //Atend.Global.Acad.UAcad.DrawEntityOnScreen(r2, Atend.Control.Enum.AutoCadLayerName.SHELL.ToString());
                        //Atend.Global.Acad.UAcad.DrawEntityOnScreen(r3, Atend.Control.Enum.AutoCadLayerName.SHELL.ToString());


                        Point3dCollection p3c1 = new Point3dCollection();
                        r1.IntersectWith(r2, Intersect.OnBothOperands, p3c1, 0, 0);


                        Point3dCollection p3c2 = new Point3dCollection();
                        r1.IntersectWith(r3, Intersect.OnBothOperands, p3c2, 0, 0);

                        if (p3c1.Count > 0 && p3c2.Count > 0)
                        {
                            CenterPoint1 = p3c1[0];
                            CenterPoint2 = p3c2[0];
                        }


                    }

                    if (CenterPoint1 != Point3d.Origin && CenterPoint2 != Point3d.Origin)
                    {
                        if (IsExist)
                        {
                            Circle c1 = new Circle(CenterPoint1, new Vector3d(0, 0, 1), Radius);
                            Atend.Global.Acad.UAcad.DrawEntityOnScreen(c1, Atend.Control.Enum.AutoCadLayerName.SHELL.ToString());
                            Circle c2 = new Circle(CenterPoint2, new Vector3d(0, 0, 1), Radius);
                            Atend.Global.Acad.UAcad.DrawEntityOnScreen(c2, Atend.Control.Enum.AutoCadLayerName.SHELL.ToString());


                            //try
                            //{
                            //    Transaction tr = doc.TransactionManager.StartTransaction();
                            //    using (tr)
                            //    {
                            //        BlockTable bt = (BlockTable)tr.GetObject(doc.Database.BlockTableId, OpenMode.ForRead);
                            //        BlockTableRecord btr = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);
                            //        Hatch hat = new Hatch();
                            //        hat.SetDatabaseDefaults();
                            //        // Firstly make it clear we want a gradient fill
                            //        hat.HatchObjectType = HatchObjectType.GradientObject;
                            //        hat.LayerId = Atend.Global.Acad.UAcad.GetLayerById(Atend.Control.Enum.AutoCadLayerName.SHELL.ToString());
                            //        //Let's use the pre-defined spherical gradient
                            //        //LINEAR, CYLINDER, INVCYLINDER, SPHERICAL, INVSPHERICAL, HEMISPHERICAL, INVHEMISPHERICAL, CURVED, and INVCURVED. 
                            //        hat.SetGradient(GradientPatternType.PreDefinedGradient, "LINEAR");
                            //        // We're defining two colours
                            //        hat.GradientOneColorMode = false;
                            //        GradientColor[] gcs = new GradientColor[2];
                            //        // First colour must have value of 0
                            //        gcs[0] = new GradientColor(Color.FromRgb(0, 0, 0), 0);
                            //        // Second colour must have value of 1
                            //        gcs[1] = new GradientColor(Color.FromRgb(0, 0, 0), 1);
                            //        hat.SetGradientColors(gcs);
                            //        // Add the hatch to the model space
                            //        // and the transaction
                            //        btr.AppendEntity(hat);
                            //        tr.AddNewlyCreatedDBObject(hat, true);
                            //        // Add the hatch loop and complete the hatch
                            //        ObjectIdCollection ids = new ObjectIdCollection();
                            //        ids.Add(c1.ObjectId);
                            //        ids.Add(c2.ObjectId);


                            //        hat.Associative = true;
                            //        hat.AppendLoop(HatchLoopTypes.Default, ids);
                            //        hat.EvaluateHatch(true);
                            //        tr.Commit();
                            //        //}
                            //    }
                            //}
                            //catch (System.Exception ex)
                            //{
                            //    ed.WriteMessage("ERROR Wipeout : {0} \n", ex.Message);
                            //}
                        }//if (IsExist)
                        else
                        {
                            Circle c1 = new Circle(CenterPoint1, new Vector3d(0, 0, 1), Radius);
                            Atend.Global.Acad.UAcad.DrawEntityOnScreen(c1, Atend.Control.Enum.AutoCadLayerName.SHELL.ToString());
                            Circle c2 = new Circle(CenterPoint2, new Vector3d(0, 0, 1), Radius);
                            Atend.Global.Acad.UAcad.DrawEntityOnScreen(c2, Atend.Control.Enum.AutoCadLayerName.SHELL.ToString());

                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                ed.WriteMessage("ERROR Air post shield : {0} \n", ex.Message);
            }
        }

    }
}
