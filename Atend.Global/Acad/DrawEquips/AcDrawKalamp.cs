using System;
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

//get from tehran 7/15
namespace Atend.Global.Acad.DrawEquips
{
    public class AcDrawKalamp
    {
        //~~~~~~~~~~~~~~~~~~~~~~~~~~ Properties ~~~~~~~~~~~~~~~~~~~~~~~~~//

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

        Atend.Base.Equipment.EClamp _eClamp;
        public Atend.Base.Equipment.EClamp eClamp
        {
            get { return _eClamp; }
            set { _eClamp = value; }
        }

        Atend.Base.Design.DPackage ClampPack;

        private ObjectId selectedObjectId;
        public ObjectId SelectedObjectId
        {
            get { return selectedObjectId; }
            set { selectedObjectId = value; }
        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~ CLASS ~~~~~~~~~~~~~~~~~~~~~~~~~//

        class DrawKalampJig : DrawJig
        {

            private List<Entity> Entities = new List<Entity>();
            public Point2d BasePoint = Point2d.Origin;
            private double NewAngle, BaseAngle = 0;
            public bool GetPoint = true;
            public bool GetAngle = false;
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            private Autodesk.AutoCAD.GraphicsInterface.TextStyle _style;
            double MyScale = 1;

            public DrawKalampJig(double Scale)
            {
                MyScale = Scale;

                // SET TEXT STYLE

                _style = new Autodesk.AutoCAD.GraphicsInterface.TextStyle();

                _style.Font = new Autodesk.AutoCAD.GraphicsInterface.FontDescriptor("Calibri", false, true, 0, 0);

                _style.TextSize = 10;

                // END OF SET TEXT STYLE

                //AddRegAppTableRecord(RegAppName);


                //Add Consol



            }

            private Entity CreateKalampEntity(Point2d BasePoint)
            {

                double BaseX = BasePoint.X;
                double BaseY = BasePoint.Y;

                Atend.Global.Acad.AcadJigs.MyPolyLine pLine = new Atend.Global.Acad.AcadJigs.MyPolyLine();

                pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX, BaseY + 5), 0, 0, 0);
                pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX, BaseY - 5), 0, 0, 0);
                pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX + 5, BaseY), 0, 0, 0);
                pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX, BaseY + 5), 0, 0, 0);
                pLine.Closed = true;

                Atend.Global.Acad.AcadJigs.SaveExtensionData(pLine, (int)Atend.Control.Enum.ProductType.Kalamp);

                return pLine;

            }

            protected override SamplerStatus Sampler(JigPrompts prompts)
            {
                //throw new System.Exception("The method or operation is not implemented.");


                if (GetPoint)
                {
                    JigPromptPointOptions jppo = new JigPromptPointOptions("Select kalamp Position : \n");

                    jppo.UserInputControls = (UserInputControls.Accept3dCoordinates |
                                             UserInputControls.NullResponseAccepted |
                                             UserInputControls.NoNegativeResponseAccepted);

                    PromptPointResult ppr = prompts.AcquirePoint(jppo);

                    if (ppr.Status == PromptStatus.OK)
                    {
                        Point2d NewPoint = new Point2d(ppr.Value.X, ppr.Value.Y);

                        if (NewPoint.IsEqualTo(BasePoint))
                        {

                            //v2d = new Vector2d(0, 0);

                            return SamplerStatus.NoChange;
                        }
                        else
                        {
                            //v2d = BasePoint - NewPoint;

                            BasePoint = new Point2d(ppr.Value.X, ppr.Value.Y);

                            //ed.WriteMessage("POINT OK. \n");

                            return SamplerStatus.OK;
                        }
                    }
                    else
                    {
                        //v2d = new Vector2d(0, 0);

                        return SamplerStatus.Cancel;
                    }

                }
                else if (GetAngle)
                {
                    JigPromptAngleOptions jpao = new JigPromptAngleOptions("Select kalamp Angle : \n");

                    jpao.UseBasePoint = true;

                    jpao.BasePoint = new Point3d(BasePoint.X, BasePoint.Y, 0);

                    PromptDoubleResult pdr = prompts.AcquireAngle(jpao);

                    if (pdr.Status == PromptStatus.OK)
                    {
                        if (pdr.Value == NewAngle)
                        {

                            return SamplerStatus.NoChange;
                        }
                        else
                        {

                            NewAngle = pdr.Value;// -NewAngle;

                            return SamplerStatus.OK;

                        }

                    }
                    else
                    {

                        return SamplerStatus.Cancel;

                    }

                }

                return SamplerStatus.NoChange;

            }

            protected override bool WorldDraw(Autodesk.AutoCAD.GraphicsInterface.WorldDraw draw)
            {

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

                        BasePoint.ToString() + ":" + NewAngle.ToString() + ":" + BaseAngle.ToString(), // Text

                        true,                  // Rawness

                        _style                // TextStyle

                            );


                    // Remember to pop our transforms off the stack

                    wg2.PopModelTransform();

                    wg2.PopModelTransform();

                }

                // END OF SHOW POSITION VALUE

                if (GetPoint)
                {
                    Entities.Clear();
                    Entities.Add(CreateKalampEntity(BasePoint));


                    //~~~~~~~~ SCALE ~~~~~~~~~~

                    Matrix3d trans1 = Matrix3d.Scaling(MyScale, new Point3d(BasePoint.X, BasePoint.Y, 0));
                    foreach (Entity en in Entities)
                    {
                        en.TransformBy(trans1);
                    }

                    //~~~~~~~~~~~~~~~~~~~~~~~~~
                }
                else if (GetAngle)
                {
                    Matrix3d trans = Matrix3d.Rotation(NewAngle - BaseAngle, ed.CurrentUserCoordinateSystem.CoordinateSystem3d.Zaxis,

                                                       new Point3d(BasePoint.X, BasePoint.Y, 0));


                    foreach (Entity en in Entities)
                    {

                        en.TransformBy(trans);

                    }

                    BaseAngle = NewAngle;
                    //NewAngle = 0;

                }

                //~~~~~~~~ SCALE ~~~~~~~~~~

                //////if (Atend.Control.Common.SelectedDesignScale != 0)
                //////{
                //////    double ScaleValue = 1 / Atend.Control.Common.SelectedDesignScale;
                //////    Matrix3d trans1 = Matrix3d.Scaling(1.50,

                //////                       new Point3d(BasePoint.X, BasePoint.Y, 0));

                //////    foreach (Entity en in Entities)
                //////    {

                //////        en.TransformBy(trans1);

                //////    }
                //////}

                //~~~~~~~~~~~~~~~~~~~~~~~~~


                foreach (Entity en in Entities)
                {

                    draw.Geometry.Draw(en);

                }


                return true;
            }

            public List<Entity> GetEntities()
            {
                Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;

                foreach (Entity ent in Entities)
                {

                    Atend.Global.Acad.AcadJigs.MyPolyLine n = ent as Atend.Global.Acad.AcadJigs.MyPolyLine;

                    //ed.WriteMessage("additiona count : {0}  \n", n.AdditionalDictionary.Count);

                }

                return Entities;
            }


        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~ METHOD ~~~~~~~~~~~~~~~~~~~~~~~~~//

        public AcDrawKalamp()
        {
            ClampPack = new Atend.Base.Design.DPackage();
        }

        //MOUSAVI->AutoPoleInstallation
        public ObjectId DrawKalamp(Point3d CenterPoint, ObjectId ParentOI)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            ObjectId HeaderOI = ObjectId.Null;

            double MyScale = Atend.Base.Design.DProductProperties.AccessSelectBySoftwareCode((int)Atend.Control.Enum.ProductType.Kalamp).Scale;
            double MyCommentScale = Atend.Base.Design.DProductProperties.AccessSelectBySoftwareCode((int)Atend.Control.Enum.ProductType.Kalamp).CommentScale;


            double BaseX = CenterPoint.X - 2.5;
            double BaseY = CenterPoint.Y;

            Atend.Global.Acad.AcadJigs.MyPolyLine pLine = new Atend.Global.Acad.AcadJigs.MyPolyLine();
            pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX, BaseY + 5), 0, 0, 0);
            pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX, BaseY - 5), 0, 0, 0);
            pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX + 5, BaseY), 0, 0, 0);
            pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX, BaseY + 5), 0, 0, 0);
            pLine.Closed = true;


            if (ParentOI != ObjectId.Null)
            {
                Atend.Base.Acad.AT_INFO ParentInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(ParentOI);
                ClampPack.ParentCode = Atend.Base.Design.DPackage.AccessSelectByNodeCode(new Guid(ParentInfo.NodeCode)).Code;
                //ed.WriteMessage("parent code :{0}", ParentInfo.NodeCode);

            }
            if (SaveKalampData())
            {

                string LayerName = "";
                if (eClamp.VoltageLevel == 400)
                {
                    LayerName = Atend.Control.Enum.AutoCadLayerName.LOW_AIR.ToString();
                }
                else
                {
                    LayerName = Atend.Control.Enum.AutoCadLayerName.MED_AIR.ToString();
                }

                Matrix3d trans1 = Matrix3d.Scaling(MyScale, CenterPoint);
                pLine.TransformBy(trans1);


                HeaderOI = Atend.Global.Acad.UAcad.DrawEntityOnScreen(pLine, LayerName);

                Atend.Base.Acad.AT_INFO at_info = new Atend.Base.Acad.AT_INFO(HeaderOI);
                //ed.WriteMessage("ParentCode For connection Point: {0}\n", ParentInfo.NodeCode);
                if (ParentOI != ObjectId.Null)
                {
                    Atend.Base.Acad.AT_SUB.AddToAT_SUB(HeaderOI, ParentOI);

                    Atend.Base.Acad.AT_INFO ParentInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(ParentOI);
                    at_info.ParentCode = ParentInfo.NodeCode;


                }
                else
                {
                    at_info.ParentCode = "";
                }
                at_info.NodeCode = ClampPack.Code.ToString();
                at_info.NodeType = (int)Atend.Control.Enum.ProductType.Kalamp;
                at_info.ProductCode = eClamp.Code;
                at_info.Insert();

                Atend.Base.Acad.AT_SUB ConnectionPSub = new Atend.Base.Acad.AT_SUB(HeaderOI);
                ConnectionPSub.SubIdCollection.Add(ParentOI);
                ConnectionPSub.Insert();


                ObjectId TextOi = Atend.Global.Acad.UAcad.DrawEntityOnScreen(Atend.Global.Acad.UAcad.WriteNote(ClampPack.Number, Atend.Global.Acad.UAcad.CenterOfEntity(Atend.Global.Acad.UAcad.GetEntityByObjectID(HeaderOI)), MyCommentScale), Atend.Control.Enum.AutoCadLayerName.GENERAL.ToString());

                Atend.Base.Acad.AT_INFO textInfo = new Atend.Base.Acad.AT_INFO(TextOi);
                textInfo.ParentCode = ClampPack.Code.ToString();
                textInfo.NodeType = (int)Atend.Control.Enum.ProductType.Comment;
                textInfo.NodeCode = "";
                textInfo.ProductCode = 0;
                textInfo.Insert();

                Atend.Base.Acad.AT_SUB.AddToAT_SUB(TextOi, HeaderOI);


            }
            return HeaderOI;


        }

        public void DrawKalamp()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            double MyScale = Atend.Base.Design.DProductProperties.AccessSelectBySoftwareCode((int)Atend.Control.Enum.ProductType.Kalamp).Scale;
            double MyCommentScale = Atend.Base.Design.DProductProperties.AccessSelectBySoftwareCode((int)Atend.Control.Enum.ProductType.Kalamp).CommentScale;

            using (DocumentLock docLock = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.LockDocument())
            {
                bool conti = true;
                int i = 0;
                DrawKalampJig _drawKalamp = new DrawKalampJig(MyScale);
                ObjectId ParentOI = ObjectId.Null;
                string ParentCode = string.Empty;

                while (conti)
                {
                    PromptResult pr;
                    pr = ed.Drag(_drawKalamp);
                    if (pr.Status == PromptStatus.OK && !_drawKalamp.GetAngle)
                    {

                        System.Data.DataTable PointContainerList = Atend.Global.Acad.Global.PointInsideWhichEntity(new Point3d(_drawKalamp.BasePoint.X, _drawKalamp.BasePoint.Y, 0));
                        System.Data.DataTable Parents = Atend.Global.Acad.UAcad.DetermineParent((int)Atend.Control.Enum.ProductType.Kalamp);
                        foreach (System.Data.DataRow dr in PointContainerList.Rows)
                        {
                            DataRow[] drs = Parents.Select(string.Format("SoftwareCode={0}", Convert.ToInt32(dr["Type"])));
                            if (drs.Length != 0)
                            {
                                ParentOI = new ObjectId(new IntPtr(Convert.ToInt32(dr["ObjectId"])));
                                ParentCode = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(ParentOI).NodeCode;
                                //ed.WriteMessage("Value assigned $$$$$$$$$$$$$$$$$\n");
                            }
                        }



                        _drawKalamp.GetPoint = false;
                        _drawKalamp.GetAngle = true;

                    }
                    else if (pr.Status == PromptStatus.OK && _drawKalamp.GetAngle)
                    {

                        conti = false;
                        List<Entity> entities = _drawKalamp.GetEntities();


                        if (SaveKalampData())
                        {

                            foreach (Entity ent in entities)
                            {

                                string LayerName = "";
                                if (eClamp.VoltageLevel == 400)
                                {
                                    LayerName = Atend.Control.Enum.AutoCadLayerName.LOW_AIR.ToString();
                                }
                                else
                                {
                                    LayerName = Atend.Control.Enum.AutoCadLayerName.MED_AIR.ToString();
                                }
                                ObjectId KOI = Atend.Global.Acad.UAcad.DrawEntityOnScreen(ent, LayerName);

                                Atend.Base.Acad.AT_INFO KalampInfo = new Atend.Base.Acad.AT_INFO(KOI);
                                KalampInfo.NodeCode = ClampPack.Code.ToString();
                                KalampInfo.NodeType = ClampPack.Type;
                                KalampInfo.ProductCode = eClamp.Code;
                                if (ParentOI != ObjectId.Null)
                                {
                                    KalampInfo.ParentCode = ParentCode;
                                }
                                else
                                {
                                    KalampInfo.ParentCode = "";
                                }
                                KalampInfo.Insert();


                                ObjectId TextOi = Atend.Global.Acad.UAcad.DrawEntityOnScreen(Atend.Global.Acad.UAcad.WriteNote(ClampPack.Number, Atend.Global.Acad.UAcad.CenterOfEntity(Atend.Global.Acad.UAcad.GetEntityByObjectID(KOI)), MyCommentScale), Atend.Control.Enum.AutoCadLayerName.GENERAL.ToString());


                                Atend.Base.Acad.AT_INFO textInfo = new Atend.Base.Acad.AT_INFO(TextOi);
                                textInfo.ParentCode = ClampPack.Code.ToString();
                                textInfo.NodeType = (int)Atend.Control.Enum.ProductType.Comment;
                                textInfo.NodeCode = "";
                                textInfo.ProductCode = 0;
                                textInfo.Insert();

                                Atend.Base.Acad.AT_SUB.AddToAT_SUB(TextOi, KOI);


                                if (ParentOI != ObjectId.Null)
                                {
                                    Atend.Base.Acad.AT_SUB.AddToAT_SUB(ParentOI, KOI);
                                    Atend.Base.Acad.AT_SUB.AddToAT_SUB(KOI, ParentOI);
                                }

                            }

                        }
                    }
                    else
                    {
                        conti = false;
                    }
                }


            }

        }

        public static void RotateKalamp(double LastAngleDegree, double NewAngleDegree, ObjectId PoleOI, ObjectId KalampOI, Point3d CenterPoint)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            using (DocumentLock docLock = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.LockDocument())
            {
                Database db = Application.DocumentManager.MdiActiveDocument.Database;
                Atend.Global.Acad.AcadMove.LastCenterPoint = Atend.Global.Acad.UAcad.CenterOfEntity(Atend.Global.Acad.UAcad.GetEntityByObjectID(KalampOI));
                using (Transaction tr = db.TransactionManager.StartTransaction())
                {
                    Entity ent = (Entity)tr.GetObject(KalampOI, OpenMode.ForWrite);
                    Polyline LineEntity = ent as Polyline;
                    if (LineEntity != null)
                    {
                        //ed.WriteMessage("LineEntity entity found \n");
                        Matrix3d trans = Matrix3d.Rotation(((LastAngleDegree * Math.PI) / 180) * -1, ed.CurrentUserCoordinateSystem.CoordinateSystem3d.Zaxis, CenterPoint);
                        LineEntity.TransformBy(trans);

                        trans = Matrix3d.Rotation(((NewAngleDegree * Math.PI) / 180), ed.CurrentUserCoordinateSystem.CoordinateSystem3d.Zaxis, CenterPoint);
                        LineEntity.TransformBy(trans);

                        //Matrix3d m = new Matrix3d();
                        //Atend.Acad.AcadMove.ConsolOI = ConsolOI;
                        //Atend.Acad.AcadMove.isMoveConsolOnly = true;
                        Atend.Global.Acad.AcadMove.AllowToMove = true;
                        Atend.Global.Acad.AcadMove.MoveKalamp(KalampOI);

                    }
                    tr.Commit();
                }
            }
        }

        private bool SaveKalampData()
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
                    //Atend.Base.Equipment.EClamp cl = Atend.Base.Equipment.EClamp.AccessSelectByXCode(eClamp.XCode);
                    if (!UseAccess)
                    {
                        //if (cl.Code == -1)
                        {
                            if (!eClamp.AccessInsert(aTransaction, aConnection, true, true))
                            {
                                throw new System.Exception("eClamp.AccessInsert failed");
                            }
                        }
                    }

                    //ClampPack  = new Atend.Base.Design.DPackage();
                    ClampPack.Count = 1;
                    ClampPack.IsExistance = Existance;
                    ClampPack.ProjectCode = ProjectCode;
                    ed.WriteMessage("calamp.Isexistance={0}\n", ClampPack.IsExistance);

                    ClampPack.ProductCode = eClamp.Code;
                    //if (cl.Code == -1)
                    //    ClampPack.ProductCode = eClamp.Code;
                    //else
                    //    ClampPack.ProductCode = cl.Code;
                    ClampPack.Type = (int)Atend.Control.Enum.ProductType.Kalamp;
                    Atend.Control.Common.Counters.ClampCounter++;
                    ClampPack.Number = string.Format("Clamp{0}", Atend.Control.Common.Counters.ClampCounter);
                    if (!ClampPack.AccessInsert(aTransaction, aConnection))
                    {
                        throw new System.Exception("ClampPack.AccessInsert failed");
                    }


                }
                catch (System.Exception ex1)
                {
                    ed.WriteMessage("ERROR SaveKalampData 01 : {0} \n", ex1.Message);
                    aTransaction.Rollback();
                    aConnection.Close();
                    return false;

                }

            }
            catch (System.Exception ex)
            {
                ed.WriteMessage("ERROR SaveKalampData 01 : {0} \n", ex.Message);
                aConnection.Close();
                return false;
            }

            aTransaction.Commit();
            aConnection.Close();

            #region Not to Get Green

            Atend.Base.Acad.AcadGlobal.ClampData.UseAccess = true;
            UseAccess = true;

            #endregion

            return true;

        }

        public bool SaveKalampDataForExternalCall(Atend.Base.Acad.AT_INFO NodeInformation)
        {
            if (!SaveKalampData())
            {
                return false;
            }
            else
            {
                NodeInformation.ProductCode = ClampPack.ProductCode;
                NodeInformation.NodeCode = ClampPack.Code.ToString();
                NodeInformation.Insert();
            }
            return true;
        }

        public bool UpdateKalampData(Guid NodeCode)
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
                    ClampPack = Atend.Base.Design.DPackage.AccessSelectByCode(NodeCode);
                    if (!UseAccess)
                    {
                        if (!eClamp.AccessInsert(aTransaction, aConnection, true, true))//aTransaction, aConnection))??
                        {

                            throw new System.Exception("eClamp.AccessInsert failed");
                        }

                    }
                    ClampPack.IsExistance = Existance;
                    ClampPack.ProductCode = eClamp.Code;
                    ClampPack.ProjectCode = ProjectCode;
                    if (ClampPack.AccessUpdate())
                    {
                        Atend.Base.Acad.AT_INFO atinfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(selectedObjectId);
                        atinfo.ProductCode = eClamp.Code;
                        atinfo.Insert();
                    }
                    else
                    {
                        throw new System.Exception("ClampPack.AccessInsert2 failed");
                    }

                }
                catch (System.Exception ex1)
                {
                    ed.WriteMessage("ERROR UpdateKalampData 01 : {0} \n", ex1.Message);
                    aTransaction.Rollback();
                    aConnection.Close();
                    return false;

                }

            }
            catch (System.Exception ex)
            {
                ed.WriteMessage("ERROR UpdateKalampData 01 : {0} \n", ex.Message);
                aConnection.Close();
                return false;
            }
            aTransaction.Commit();
            aConnection.Close();
            return true;
        }

        public static bool DeleteKalampData(ObjectId KalampOI)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            try
            {
                Atend.Base.Acad.AT_SUB SubGP = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(KalampOI);
                foreach (ObjectId oi in SubGP.SubIdCollection)
                {
                    Atend.Base.Acad.AT_INFO atinfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oi);
                    if (atinfo.ParentCode != "NONE" && atinfo.NodeType == (int)Atend.Control.Enum.ProductType.SelfKeeper)
                    {
                        if (!Atend.Base.Design.DBranch.AccessDelete(new Guid(atinfo.NodeCode.ToString())))
                        {
                            throw new System.Exception("Error In Delete dbranch\n");
                        }
                    }
                    if (atinfo.ParentCode != "NONE" && (atinfo.NodeType == (int)Atend.Control.Enum.ProductType.Breaker || atinfo.NodeType == (int)Atend.Control.Enum.ProductType.Disconnector || atinfo.NodeType == (int)Atend.Control.Enum.ProductType.CatOut))
                    {
                        if (!Atend.Base.Design.DPackage.AccessDelete(new Guid(atinfo.NodeCode.ToString())))
                        {
                            throw new System.Exception("Error In Delete DPackage\n");
                        }
                    }
                    if (atinfo.ParentCode != "NONE" && atinfo.NodeType == (int)Atend.Control.Enum.ProductType.Terminal)
                    {
                        if (!Atend.Base.Design.DBranch.AccessDelete(new Guid(atinfo.NodeCode.ToString())))
                        {
                            throw new System.Exception("Error In Delete dbranch\n");
                        }
                    }
                }

                Atend.Base.Acad.AT_INFO Kalampinfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(KalampOI);
                if (!Atend.Base.Design.DPackage.AccessDelete(new Guid(Kalampinfo.NodeCode.ToString())))
                {
                    throw new System.Exception("Error In Delete DPackage\n");
                }

            }
            catch (System.Exception ex)
            {
                ed.WriteMessage("GRA ERROR Kalamp : {0} \n", ex.Message);
                return false;
            }
            return true;
        }

        public static bool DeleteKalampData(ObjectId KalampOI, OleDbTransaction _Transaction, OleDbConnection _Connection)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            try
            {
                Atend.Base.Acad.AT_SUB SubGP = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(KalampOI);
                foreach (ObjectId oi in SubGP.SubIdCollection)
                {
                    Atend.Base.Acad.AT_INFO atinfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oi);
                    if (atinfo.ParentCode != "NONE" && atinfo.NodeType == (int)Atend.Control.Enum.ProductType.SelfKeeper)
                    {
                        if (!Atend.Base.Design.DBranch.AccessDelete(new Guid(atinfo.NodeCode.ToString()), _Transaction, _Connection))
                        {
                            throw new System.Exception("Error In Delete dbranch\n");
                        }
                    }
                    if (atinfo.ParentCode != "NONE" && (atinfo.NodeType == (int)Atend.Control.Enum.ProductType.Breaker || atinfo.NodeType == (int)Atend.Control.Enum.ProductType.Disconnector || atinfo.NodeType == (int)Atend.Control.Enum.ProductType.CatOut))
                    {
                        if (!Atend.Base.Design.DPackage.AccessDelete(new Guid(atinfo.NodeCode.ToString()), _Transaction, _Connection))
                        {
                            throw new System.Exception("Error In Delete DPackage\n");
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

                Atend.Base.Acad.AT_INFO Kalampinfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(KalampOI);
                if (!Atend.Base.Design.DPackage.AccessDelete(new Guid(Kalampinfo.NodeCode.ToString()), _Transaction, _Connection))
                {
                    throw new System.Exception("Error In Delete DPackage\n");
                }

            }
            catch (System.Exception ex)
            {
                ed.WriteMessage("GRA ERROR Kalamp : {0} \n", ex.Message);
                _Transaction.Rollback();
                return false;
            }
            return true;
        }

        public static bool DeleteKalamp(ObjectId KalampOI)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            try
            {
                Atend.Base.Acad.AT_SUB sub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(KalampOI);
                foreach (ObjectId objsub in sub.SubIdCollection)
                {
                    Atend.Base.Acad.AT_INFO at_info1 = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(objsub);
                    if (at_info1.ParentCode != "NONE" && at_info1.NodeType == (int)Atend.Control.Enum.ProductType.SelfKeeper)
                    {
                        //Move Comment 
                        Atend.Base.Acad.AT_SUB subBranch = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(objsub);
                        foreach (ObjectId collect in subBranch.SubIdCollection)
                        {
                            Atend.Base.Acad.AT_INFO atinfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(collect);
                            if (atinfo.ParentCode != "NONE" && atinfo.NodeType == (int)Atend.Control.Enum.ProductType.Comment)
                            {
                                if (!Atend.Global.Acad.AcadRemove.DeleteEntityByObjectId(collect))
                                {
                                    throw new System.Exception("Error In Delete Cabel Comment\n");
                                }
                            }
                            //Delete From AT_SUB other kalamp
                            if (atinfo.ParentCode != "NONE" && atinfo.NodeType == (int)Atend.Control.Enum.ProductType.Kalamp && collect != KalampOI)
                            {
                                Atend.Base.Acad.AT_SUB.RemoveFromAT_SUB(objsub, collect);
                            }
                        }

                        if (!Atend.Global.Acad.AcadRemove.DeleteEntityByObjectId(objsub))
                        {
                            throw new System.Exception("Error In Delete Cabel\n");
                        }
                    }

                    if (at_info1.ParentCode != "NONE" && at_info1.NodeType == (int)Atend.Control.Enum.ProductType.Terminal)
                    {
                        Atend.Base.Acad.AT_SUB subBranch = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(objsub);
                        foreach (ObjectId collect in subBranch.SubIdCollection)
                        {
                            Atend.Base.Acad.AT_INFO atinfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(collect);

                            //Delete From AT_SUB other kalamp
                            if (atinfo.ParentCode != "NONE" && atinfo.NodeType == (int)Atend.Control.Enum.ProductType.Kalamp && collect != KalampOI)
                            {
                                Atend.Base.Acad.AT_SUB.RemoveFromAT_SUB(objsub, collect);
                            }
                        }

                        if (!Atend.Global.Acad.AcadRemove.DeleteEntityByObjectId(objsub))
                        {
                            throw new System.Exception("Error In Delete Terminal\n");
                        }
                    }

                    if (at_info1.ParentCode != "NONE" && at_info1.NodeType == (int)Atend.Control.Enum.ProductType.Comment)
                    {
                        if (!Atend.Global.Acad.AcadRemove.DeleteEntityByObjectId(objsub))
                        {
                            throw new System.Exception("Error In Delete Comment\n");
                        }
                    }
                    if (at_info1.ParentCode != "NONE" && at_info1.NodeType == (int)Atend.Control.Enum.ProductType.Breaker)
                    {
                        ObjectIdCollection CollectionBreaker = Atend.Global.Acad.UAcad.GetGroupSubEntities(objsub);
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
                    if (at_info1.ParentCode != "NONE" && at_info1.NodeType == (int)Atend.Control.Enum.ProductType.Disconnector)
                    {
                        ObjectIdCollection CollectionDisconnector = Atend.Global.Acad.UAcad.GetGroupSubEntities(objsub);
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
                    if (at_info1.ParentCode != "NONE" && at_info1.NodeType == (int)Atend.Control.Enum.ProductType.CatOut)
                    {
                        ObjectIdCollection CollectionDisconnector = Atend.Global.Acad.UAcad.GetGroupSubEntities(objsub);
                        foreach (ObjectId collect in CollectionDisconnector)
                        {
                            Atend.Base.Acad.AT_INFO at_infoo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(collect);
                            if (at_infoo.ParentCode != "NONE" && at_infoo.NodeType == (int)Atend.Control.Enum.ProductType.Terminal)
                            {
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
                    if (at_info1.ParentCode != "NONE" && (at_info1.NodeType == (int)Atend.Control.Enum.ProductType.Pole || at_info1.NodeType == (int)Atend.Control.Enum.ProductType.PoleTip))
                    {
                        Atend.Base.Acad.AT_SUB.RemoveFromAT_SUB(KalampOI, objsub);
                    }
                }

                //+++
                if (!Atend.Global.Acad.AcadRemove.DeleteEntityByObjectId(KalampOI))
                {
                    throw new System.Exception("GRA while delete Kalamp \n");
                }

            }
            catch (System.Exception ex)
            {
                ed.WriteMessage("GRA ERROR Kalamp : {0} \n", ex.Message);
                return false;
            }
            return true;
        }



    }
}
