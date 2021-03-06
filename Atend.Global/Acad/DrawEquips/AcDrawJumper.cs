using System;
using System.Collections.Generic;
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


namespace Atend.Global.Acad.DrawEquips
{

    public class AcDrawJumper
    {


        Atend.Base.Design.DBranch JumperBtranch = new Atend.Base.Design.DBranch();

        public class DrawJumperJig : DrawJig
        {

            Point3d StartPoint, EndPoint;

            public DrawJumperJig()
            {
            }

            private Entity CreateArcEntity(Point3d _CenterPoint, Point3d StartPoint, Point3d EndPoint)
            {

                double Radius = _CenterPoint.DistanceTo(StartPoint);
                Vector2d x1 = new Point2d(_CenterPoint.X, _CenterPoint.Y).GetVectorTo(new Point2d(StartPoint.X, StartPoint.Y));
                double StartAngle = x1.Angle;
                Vector2d x2 = new Point2d(_CenterPoint.X, _CenterPoint.Y).GetVectorTo(new Point2d(EndPoint.X, EndPoint.Y));
                double EndAngle = x2.Angle;

                Arc a = new Arc(_CenterPoint, Radius, StartAngle, EndAngle);
                return a;

            }

            protected override SamplerStatus Sampler(JigPrompts prompts)
            {
                //throw new System.Exception("The method or operation is not implemented.");

                return SamplerStatus.OK;

            }

            protected override bool WorldDraw(Autodesk.AutoCAD.GraphicsInterface.WorldDraw draw)
            {
                //throw new System.Exception("The method or operation is not implemented.");

                return true;
            }

        }

        public AcDrawJumper()
        {
            JumperBtranch = new Atend.Base.Design.DBranch();
        }


        //update from tehran 7/15
        public void DrawJumper()
        {

            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            PromptEntityOptions peo = new PromptEntityOptions("");
            PromptEntityResult perOne, perTwo;
            ObjectId SharedPole = ObjectId.Null;
            Atend.Base.Acad.AT_INFO conductorInfo;
            Atend.Base.Acad.AT_INFO conductorInfo1;

            #region Conductor One


            peo.Message = "\nSelect First Conductor : ";
            perOne = ed.GetEntity(peo);
            if (perOne.Status == PromptStatus.OK)
            {
                conductorInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(perOne.ObjectId);
                if (conductorInfo.ParentCode != "NONE" && (conductorInfo.NodeType == (int)Atend.Control.Enum.ProductType.Conductor || conductorInfo.NodeType == (int)Atend.Control.Enum.ProductType.SelfKeeper || conductorInfo.NodeType == (int)Atend.Control.Enum.ProductType.GroundCabel))
                {
                    JumperBtranch.LeftNodeCode = new Guid(conductorInfo.NodeCode);
                    #region Conductor Two
                    peo.Message = "\nSelect Second Conductor : ";
                    perTwo = ed.GetEntity(peo);
                    if (perTwo.Status == PromptStatus.OK)
                    {
                        conductorInfo1 = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(perTwo.ObjectId);
                        if (conductorInfo1.ParentCode != "NONE" && (conductorInfo1.NodeType == (int)Atend.Control.Enum.ProductType.Conductor || conductorInfo1.NodeType == (int)Atend.Control.Enum.ProductType.SelfKeeper || conductorInfo1.NodeType == (int)Atend.Control.Enum.ProductType.GroundCabel))
                        {
                            JumperBtranch.RightNodeCode = new Guid(conductorInfo1.NodeCode);
                            JumperBtranch.Number = "Jumper";
                            ObjectIdCollection FirstConductorPole = new ObjectIdCollection();
                            Atend.Base.Acad.AT_SUB firstConductorSub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(perOne.ObjectId);
                            ObjectIdCollection SecondConductorPole = new ObjectIdCollection();
                            Atend.Base.Acad.AT_SUB SecondConductorSub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(perTwo.ObjectId);
                            foreach (ObjectId oi in firstConductorSub.SubIdCollection)
                            {
                                //Atend.Base.Acad.AT_INFO at_info = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oi);
                                //if (at_info.ParentCode != "NONE" && at_info.NodeType == (int)Atend.Control.Enum.ProductType.Consol)
                                //{
                                Atend.Base.Acad.AT_SUB at_sub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(oi);
                                foreach (ObjectId oii in at_sub.SubIdCollection)
                                {
                                    Atend.Base.Acad.AT_INFO at_poleinfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oii);

                                    if (at_poleinfo.ParentCode != "NONE" && at_poleinfo.NodeType == (int)Atend.Control.Enum.ProductType.Pole)
                                    {
                                        FirstConductorPole.Add(oii);
                                    }

                                }

                                // }

                            }
                            foreach (ObjectId oi in SecondConductorSub.SubIdCollection)
                            {
                                //Atend.Base.Acad.AT_INFO at_info = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oi);
                                //if (at_info.ParentCode != "NONE" && at_info.NodeType == (int)Atend.Control.Enum.ProductType.Consol)
                                //{
                                Atend.Base.Acad.AT_SUB at_sub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(oi);
                                foreach (ObjectId oii in at_sub.SubIdCollection)
                                {
                                    Atend.Base.Acad.AT_INFO at_poleinfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oii);
                                    if (at_poleinfo.ParentCode != "NONE" && at_poleinfo.NodeType == (int)Atend.Control.Enum.ProductType.Pole)
                                    {
                                        SecondConductorPole.Add(oii);
                                    }

                                }

                                //}

                            }

                            foreach (ObjectId oi in FirstConductorPole)
                            {
                                foreach (ObjectId oii in SecondConductorPole)
                                {
                                    if (oi == oii)
                                    {
                                        SharedPole = oi;
                                    }

                                }

                            }



                            // draw arc
                            if (SharedPole != ObjectId.Null)
                            {
                                //Entity ContainerEntity = Atend.Global.Acad.UAcad.GetEntityByObjectID(SharedPole);
                                //Polyline p = ContainerEntity as Polyline;
                                //Point3d CenterPoint = Atend.Global.Acad.UAcad.CenterOfEntity(p);
                                Point3d StartPoint = perOne.PickedPoint; ;
                                Point3d EndPoint = perTwo.PickedPoint;

                                Line firstLine = (Line)Atend.Global.Acad.UAcad.GetEntityByObjectID(perOne.ObjectId);
                                Line secondLine = (Line)Atend.Global.Acad.UAcad.GetEntityByObjectID(perTwo.ObjectId);
                                if (firstLine != null && secondLine != null)
                                {
                                    //ed.WriteMessage("\nGET CLOSEST POINT \n");
                                    LineSegment3d ls1 = new LineSegment3d(firstLine.StartPoint, firstLine.EndPoint);
                                    LineSegment3d ls2 = new LineSegment3d(secondLine.StartPoint, secondLine.EndPoint);

                                    StartPoint = ls1.GetClosestPointTo(StartPoint).Point;
                                    EndPoint = ls2.GetClosestPointTo(EndPoint).Point;

                                    PointOnCurve3d pp = ls1.GetClosestPointTo(StartPoint);
                                    if (pp.IsOn(StartPoint))
                                    {
                                    }

                                    pp = ls2.GetClosestPointTo(EndPoint);
                                    if (pp.IsOn(EndPoint))
                                    {
                                    }
                                }

                                Entity a = CreateArcEntity(StartPoint, EndPoint);
                                a.LayerId = firstLine.LayerId;
                                JumperBtranch.ProductCode = conductorInfo.ProductCode;
                                if (SaveJumperData())
                                {
                                    

                                    ObjectId NewArc = Atend.Global.Acad.UAcad.DrawEntityOnScreen(a, Atend.Control.Enum.AutoCadLayerName.GENERAL.ToString());

                                    Atend.Base.Acad.AT_INFO jumperinfo = new Atend.Base.Acad.AT_INFO(NewArc);
                                    jumperinfo.NodeCode = JumperBtranch.Code.ToString();
                                    jumperinfo.ParentCode = "";
                                    ed.WriteMessage("   *** JUMPER PCODE : {0} \n",conductorInfo.ProductCode);
                                    jumperinfo.ProductCode = conductorInfo.ProductCode;
                                    jumperinfo.NodeType = (int)Atend.Control.Enum.ProductType.Jumper;
                                    jumperinfo.Angle = 0;
                                    jumperinfo.Insert();


                                    Atend.Base.Acad.AT_SUB jumperSub = new Atend.Base.Acad.AT_SUB(NewArc);
                                    //ed.WriteMessage("JUMPER : {0}\n",perOne.ObjectId);
                                    jumperSub.SubIdCollection.Add(perOne.ObjectId);
                                    //ed.WriteMessage("JUMPER : {0}\n", perTwo.ObjectId);
                                    jumperSub.SubIdCollection.Add(perTwo.ObjectId);
                                    jumperSub.Insert();

                                    Atend.Base.Acad.AT_SUB.AddToAT_SUB(NewArc, perOne.ObjectId);
                                    Atend.Base.Acad.AT_SUB.AddToAT_SUB(NewArc, perTwo.ObjectId);

                                }

                            }
                        }
                        else
                        {
                            return;
                        }

                    }//if (perTwo.Status == PromptStatus.OK)
                    else
                    {
                        return;
                    }

                    #endregion

                }
                else
                {
                    return;
                }

            }
            else
            {
                return;
            }

            #endregion
        }

        private Entity CreateArcEntity(Point3d StartPoint, Point3d EndPoint)
        {

            //double Radius;
            //double StartAngle;
            //double EndAngle;

            //ed.WriteMessage("Start POint {0}, \nEnd Point : {1} \n", StartPoint, EndPoint);
            //Radius = _CenterPoint.DistanceTo(StartPoint);
            //Line l1 = new Line();
            //l1.StartPoint = _CenterPoint;
            //l1.EndPoint = StartPoint;
            ////DrawEntityOnScreen(l1);

            //Line l2 = new Line(_CenterPoint, EndPoint);
            //l2.StartPoint = _CenterPoint;
            //l2.EndPoint = EndPoint;


            //StartAngle = l1.Angle;
            //EndAngle = l2.Angle;

            //ed.WriteMessage("Start Angle : {0}\n", StartAngle);
            //ed.WriteMessage("End Angle : {0}\n", EndAngle);

            //Arc a = new Arc(_CenterPoint, Radius, StartAngle, EndAngle);
            //Line a = new Line(StartPoint, EndPoint);
            Polyline a = new Polyline();
            a.AddVertexAt(a.NumberOfVertices, new Point2d(StartPoint.X, StartPoint.Y), -0.5, 0, 0);
            a.AddVertexAt(a.NumberOfVertices, new Point2d(EndPoint.X, EndPoint.Y), 0, 0, 0);
            a.ColorIndex = 12;
            return a;

        }

        private bool SaveJumperData()
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            OleDbConnection aconnection = new OleDbConnection(Atend.Control.ConnectionString.AccessCnString);
            OleDbTransaction atransaction;

            try
            {
                aconnection.Open();
                atransaction = aconnection.BeginTransaction();

                try
                {

                    JumperBtranch.IsExist = 0;
                    JumperBtranch.IsWeek = true;
                    //JumperBtranch.LeftNodeCode = LeftNode;
                    //JumperBtranch.RightNodeCode = RigthNode;
                    JumperBtranch.Number = "JUMP 001";
                    JumperBtranch.Lenght = 0;
                    JumperBtranch.ProductType = (int)Atend.Control.Enum.ProductType.Jumper;
                    JumperBtranch.Sag = 0;
                    if (!JumperBtranch.AccessInsert(atransaction, aconnection))
                    {
                        throw new System.Exception("JumperBtranch.AccessInsert failed");
                    }


                }
                catch (System.Exception ex1)
                {
                    ed.WriteMessage("ERROR SaveJumperData 02 :{0} \n", ex1.Message);
                    atransaction.Rollback();
                    aconnection.Close();
                    return false;
                }

            }
            catch (System.Exception ex)
            {
                ed.WriteMessage("ERROR SaveJumperData 01 :{0} \n", ex.Message);
                aconnection.Close();
                return false;
            }

            atransaction.Commit();
            aconnection.Close();
            return true;

        }

        public static bool DeleteJumperData(ObjectId jumper)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            try
            {
                Atend.Base.Acad.AT_INFO _info = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(jumper);
                if (!Atend.Base.Design.DBranch.AccessDelete(new Guid(_info.NodeCode.ToString())))
                {
                    throw new System.Exception("Error In Delete Jumper\n");
                }

            }
            catch (System.Exception ex)
            {
                ed.WriteMessage(" ERROR Jumper : {0} \n", ex.Message);
                return false;
            }
            return true;
        }

        public static bool DeleteJumper(ObjectId jumper)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            try
            {

                Atend.Base.Acad.AT_SUB jumperSub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(jumper);
                foreach (ObjectId jumperSubOI in jumperSub.SubIdCollection)
                {
                    Atend.Base.Acad.AT_INFO SubInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(jumperSubOI);
                    if (SubInfo.ParentCode != "NONE" && SubInfo.NodeType == (int)Atend.Control.Enum.ProductType.Conductor)
                    {
                        Atend.Base.Acad.AT_SUB conductorSub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(jumperSubOI);
                        foreach (ObjectId oi in conductorSub.SubIdCollection)
                        {
                            Atend.Base.Acad.AT_INFO at_info = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oi);
                            if (at_info.ParentCode != "NONE" && at_info.NodeType == (int)Atend.Control.Enum.ProductType.Jumper && at_info.SelectedObjectId == jumper)
                            {
                                Atend.Base.Acad.AT_SUB.RemoveFromAT_SUB(oi, jumperSubOI);
                            }
                        }
                    }
                }
                //+++
                if (!Atend.Global.Acad.AcadRemove.DeleteEntityByObjectId(jumper))
                {
                    throw new System.Exception("GRA while delete conductor \n");
                }
            }
            catch (System.Exception ex)
            {
                ed.WriteMessage("GRA ERROR JUMPER : {0} \n", ex.Message);
                return false;
            }
            return true;
        }

    }
}
