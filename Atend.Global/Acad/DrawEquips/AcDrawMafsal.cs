﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

using Autodesk.AutoCAD.EditorInput;
//using Autodesk.AutoCAD.GraphicsInterface;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.Geometry;
//using Autodesk.AutoCAD.Interop;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.Windows;
using Autodesk.AutoCAD.Colors;
using Autodesk.AutoCAD.DatabaseServices;


namespace Atend.Global.Acad.DrawEquips
{
    public class AcDrawMafsal
    {

        private Atend.Base.Design.DPackage mafsalInfo;


        public Atend.Base.Design.DPackage MafsalInfo
        {
            get { return mafsalInfo; }
            set { mafsalInfo = value; }
        }


        private class DrawMafsalJig : DrawJig
        {


            Point3d CenterPoint = Point3d.Origin;

            Autodesk.AutoCAD.DatabaseServices.Entity ConnectionPointEntity;
            double MyScale = 1;


            public DrawMafsalJig(double Scale)
            {
                MyScale = Scale;
                Circle c = new Circle(CenterPoint, new Vector3d(0, 0, 0), 10);

                c.ColorIndex = 3;

                ConnectionPointEntity = c;
            }

            protected override SamplerStatus Sampler(JigPrompts prompts)
            {
                //throw new System.Exception("The method or operation is not implemented.");

                JigPromptPointOptions ppo = new JigPromptPointOptions("\nSelect Mafsal Position :");

                PromptPointResult ppr = prompts.AcquirePoint(ppo);

                if (ppr.Status == PromptStatus.OK)
                {

                    if (ppr.Value == CenterPoint)
                    {
                        return SamplerStatus.NoChange;
                    }
                    else
                    {

                        CenterPoint = ppr.Value;

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

                Circle c = new Circle(CenterPoint, new Vector3d(0, 0, 1), 10);
                //c.Annotative = AnnotativeStates.True;
                //c.Annotative 
                c.ColorIndex = 151;

                ConnectionPointEntity = c;

                //~~~~~~~~ SCALE ~~~~~~~~~~

                Matrix3d trans1 = Matrix3d.Scaling(MyScale, new Point3d(CenterPoint.X, CenterPoint.Y, 0));
                //foreach (Entity en in Entities)
                //{
                ConnectionPointEntity.TransformBy(trans1);
                //}

                //~~~~~~~~~~~~~~~~~~~~~~~~~

                draw.Geometry.Draw(ConnectionPointEntity);

                return true;
            }

            public Entity GetEntity()
            {
                return ConnectionPointEntity;
            }

            public Entity GetDemo(Point3d MyCenterPoint)
            {
                Circle c = new Circle(MyCenterPoint, new Vector3d(0, 0, 1), 10);

                c.ColorIndex = 151;

                ConnectionPointEntity = c;

                return c;
            }

        }



        public void DrawMafsal()
        {

            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            //ed.WriteMessage("aaaaa\n");
            PromptKeywordOptions PSO = new PromptKeywordOptions("\nترسیم مفصل");
            PSO.Keywords.Add("New", "New", "New");
            PSO.Keywords.Add("Break", "Break", "Break Cabel");
            PSO.Keywords.Default = "New";
            PromptResult psr = ed.GetKeywords(PSO);

            double MyScale = Atend.Base.Design.DProductProperties.AccessSelectBySoftwareCode((int)Atend.Control.Enum.ProductType.Mafsal).Scale;


            DrawMafsalJig DCP = new DrawMafsalJig(MyScale);
            if (psr.Status == PromptStatus.OK)
            {
                switch (psr.StringResult)
                {
                    case "New":

                        bool Conti = true;
                        PromptResult pr;
                        while (Conti)
                        {

                            pr = ed.Drag(DCP);

                            if (pr.Status == PromptStatus.OK)
                            {
                                Conti = false;
                                Entity entity = DCP.GetEntity();
                                if (SaveMafsalData())
                                {
                                    Atend.Global.Acad.UAcad.DrawEntityOnScreen(entity, Atend.Control.Enum.AutoCadLayerName.GENERAL.ToString());
                                    Atend.Base.Acad.AT_INFO at_info = new Atend.Base.Acad.AT_INFO(entity.ObjectId);
                                    at_info.ParentCode = "";
                                    at_info.NodeCode = MafsalInfo.Code.ToString();
                                    at_info.NodeType = (int)Atend.Control.Enum.ProductType.Mafsal;
                                    at_info.ProductCode = Convert.ToInt32(MafsalInfo.ProductCode);
                                    at_info.Insert();
                                }

                            }
                            else
                            {
                                Conti = false;
                            }

                        }
                        break;
                    case "Break":

                        PromptEntityOptions peo = new PromptEntityOptions("\nمحل برش کابل را انتخاب نمایید");
                        PromptEntityResult per = ed.GetEntity(peo);
                        if (per.Status == PromptStatus.OK)
                        {

                            Polyline SelectedCable = Atend.Global.Acad.UAcad.GetEntityByObjectID(per.ObjectId) as Polyline;
                            if (SelectedCable != null)
                            {
                                //////SelectedCable.UpgradeOpen();
                                //////SelectedCable.SetField("Name",new Field("Parisa", true));
                                int NearestSegmentIndex = 0;
                                double NearestSegmentDistance = 100000;
                                Point3d NearestPoint = Point3d.Origin;
                                for (int counter = 0; counter < SelectedCable.NumberOfVertices - 1; counter++)
                                {
                                    LineSegment3d ls1 = new LineSegment3d(SelectedCable.GetPoint3dAt(counter), SelectedCable.GetPoint3dAt(counter + 1));
                                    Point3d SelectedPoint = ls1.GetClosestPointTo(per.PickedPoint).Point;
                                    //ed.WriteMessage("SP:{0}\n", SelectedPoint);

                                    if (SelectedPoint.DistanceTo(per.PickedPoint) < NearestSegmentDistance)
                                    {
                                        NearestSegmentDistance = SelectedPoint.DistanceTo(per.PickedPoint);
                                        NearestSegmentIndex = counter;
                                        NearestPoint = SelectedPoint;
                                    }

                                }
                                //ed.WriteMessage("SINDEX:{0}\n", NearestSegmentIndex);
                                //seprate Cable

                                Polyline Part1 = new Polyline();
                                Polyline Part2 = new Polyline();
                                for (int counter = 0; counter <= NearestSegmentIndex; counter++)
                                {
                                    Part1.AddVertexAt(Part1.NumberOfVertices, SelectedCable.GetPoint2dAt(counter), 0, 0, 0);
                                    //ed.WriteMessage("POINT:{0}\n", SelectedCable.GetPoint2dAt(counter));
                                }
                                Part1.AddVertexAt(Part1.NumberOfVertices, new Point2d(NearestPoint.X, NearestPoint.Y), 0, 0, 0);

                                Part2.AddVertexAt(Part2.NumberOfVertices, new Point2d(NearestPoint.X, NearestPoint.Y), 0, 0, 0);
                                for (int counter = NearestSegmentIndex + 1; counter <= SelectedCable.NumberOfVertices - 1; counter++)
                                {
                                    Part2.AddVertexAt(Part2.NumberOfVertices, SelectedCable.GetPoint2dAt(counter), 0, 0, 0);
                                }
                                if (SaveMafsalData())
                                {
                                    try
                                    {
                                        Atend.Base.Acad.AT_INFO LastCableInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(per.ObjectId);
                                        Atend.Base.Acad.AT_SUB LastCableSub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(per.ObjectId);
                                        //ed.WriteMessage("-3\n");
                                        //determine header belong to part1 or part2
                                        ObjectIdCollection Blong1 = new ObjectIdCollection();
                                        ObjectIdCollection Blong2 = new ObjectIdCollection();
                                        foreach (ObjectId oi in LastCableSub.SubIdCollection)
                                        {
                                            Atend.Base.Acad.AT_INFO oiInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oi);
                                            Point3d FirstPoint = SelectedCable.GetPoint3dAt(0);
                                            //ed.WriteMessage("-3.1\n");
                                            if (oiInfo.ParentCode != "NONE" && FirstPoint != null)
                                            {
                                                //ed.WriteMessage("-3.2\n");
                                                switch ((Atend.Control.Enum.ProductType)oiInfo.NodeType)
                                                {
                                                    case Atend.Control.Enum.ProductType.Mafsal:
                                                        //ed.WriteMessage("-3.3\n");
                                                        if (Atend.Global.Acad.UAcad.CenterOfEntity(Atend.Global.Acad.UAcad.GetEntityByObjectID(oi)) == FirstPoint)
                                                        {
                                                            //ed.WriteMessage("-3.4\n");
                                                            Blong1.Add(oi);
                                                        }
                                                        else
                                                        {
                                                            //ed.WriteMessage("-3.5\n");
                                                            Blong2.Add(oi);
                                                        }
                                                        break;
                                                    case Atend.Control.Enum.ProductType.HeaderCabel:
                                                        //ed.WriteMessage("-3.6\n");
                                                        //ed.WriteMessage("color:{0}\n",curv.ColorIndex);
                                                        //ed.WriteMessage("-3.6.1\n");
                                                        if (Atend.Global.Acad.UAcad.CenterOfEntity(Atend.Global.Acad.UAcad.GetEntityByObjectID(oi)) == FirstPoint)
                                                        {
                                                            //ed.WriteMessage("-3.7\n");
                                                            Blong1.Add(oi);
                                                        }
                                                        else
                                                        {
                                                            //ed.WriteMessage("-3.8\n");
                                                            Blong2.Add(oi);
                                                        }
                                                        break;
                                                    case Atend.Control.Enum.ProductType.ConnectionPoint:
                                                        if (Atend.Global.Acad.UAcad.CenterOfEntity(Atend.Global.Acad.UAcad.GetEntityByObjectID(oi)) == FirstPoint)
                                                        {
                                                            Blong1.Add(oi);
                                                        }
                                                        else
                                                        {
                                                            Blong2.Add(oi);
                                                        }

                                                        break;
                                                }
                                            }
                                        }

                                        Entity MafsalEntiyt = DCP.GetDemo(NearestPoint);
                                        ObjectId NewDrawnMafsalOI = Atend.Global.Acad.UAcad.DrawEntityOnScreen(MafsalEntiyt, Atend.Control.Enum.AutoCadLayerName.GENERAL.ToString());

                                        Atend.Base.Acad.AT_INFO newinfo1 = new Atend.Base.Acad.AT_INFO(NewDrawnMafsalOI);
                                        newinfo1.ParentCode = "";
                                        newinfo1.NodeCode = MafsalInfo.Code.ToString();
                                        newinfo1.NodeType = (int)Atend.Control.Enum.ProductType.Mafsal;
                                        newinfo1.ProductCode = MafsalInfo.ProductCode;
                                        newinfo1.Insert();



                                        //ed.WriteMessage("-4\n");
                                        Atend.Global.Acad.AcadRemove.DeleteEntityByObjectId(SelectedCable.ObjectId);
                                        //ed.WriteMessage("1\n");
                                        Atend.Global.Acad.UAcad.DrawEntityOnScreen(Part1, Atend.Control.Enum.AutoCadLayerName.MED_GROUND.ToString());
                                        //ed.WriteMessage("2\n");
                                        Atend.Global.Acad.UAcad.DrawEntityOnScreen(Part2, Atend.Control.Enum.AutoCadLayerName.MED_GROUND.ToString());
                                        //ed.WriteMessage("3\n");

                                        Atend.Base.Acad.AT_SUB mafsalsub = new Atend.Base.Acad.AT_SUB(NewDrawnMafsalOI);
                                        mafsalsub.SubIdCollection.Add(Part1.ObjectId);
                                        mafsalsub.SubIdCollection.Add(Part2.ObjectId);
                                        mafsalsub.Insert();
                                        //ed.WriteMessage("4\n");

                                        LastCableInfo.SelectedObjectId = Part1.ObjectId;
                                        LastCableInfo.Insert();
                                        ///ed.WriteMessage("5\n");

                                        LastCableInfo.SelectedObjectId = Part2.ObjectId;
                                        LastCableInfo.Insert();
                                        //ed.WriteMessage("6\n");

                                        Atend.Base.Acad.AT_SUB.AddToAT_SUB(NewDrawnMafsalOI, Part1.ObjectId);

                                        foreach (ObjectId oi in Blong1)
                                        {
                                            Atend.Base.Acad.AT_SUB.AddToAT_SUB(oi, Part1.ObjectId);
                                        }

                                        Atend.Base.Acad.AT_SUB.AddToAT_SUB(NewDrawnMafsalOI, Part2.ObjectId);

                                        foreach (ObjectId oi in Blong2)
                                        {
                                            Atend.Base.Acad.AT_SUB.AddToAT_SUB(oi, Part2.ObjectId);
                                        }

                                    }
                                    catch (System.Exception ex)
                                    {
                                        ed.WriteMessage("~~~{0}~~~~~~\n", ex.Message);
                                    }
                                }//catch ended here
                            }

                        }


                        break;
                }
            }

        }

        private bool SaveMafsalData()
        {

            if (MafsalInfo.AccessInsert())
            {
                return true;
            }
            else
            {
                return false;
            }

        }


    }
}