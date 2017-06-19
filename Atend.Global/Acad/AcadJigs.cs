﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;


using Autodesk.AutoCAD.EditorInput;
//

using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.Geometry;
//using Autodesk.AutoCAD.Interop;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.Windows;
using Autodesk.AutoCAD.Colors;


namespace Atend.Global.Acad
{
    public class AcadJigs
    {

   
        public class MyPolyLine : Polyline
        {
            private Dictionary<string, object> additionalDictionary = new Dictionary<string, object>();

            public Dictionary<string, object> AdditionalDictionary
            {
                get { return additionalDictionary; }
                set { additionalDictionary = value; }
            }

        }

        public class MyCircle : Circle
        {
            private Dictionary<string, object> additionalDictionary = new Dictionary<string, object>();

            public Dictionary<string, object> AdditionalDictionary
            {
                get { return additionalDictionary; }
                set { additionalDictionary = value; }
            }

        }

        public class MyLine : Line
        {



            private Dictionary<string, object> additionalDictionary = new Dictionary<string, object>();

            public Dictionary<string, object> AdditionalDictionary
            {
                get { return additionalDictionary; }
                set { additionalDictionary = value; }
            }


        }

        public static void SaveExtensionData(Entity CurrentEntity, int CurrentProductType)
        {

            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;

            Atend.Global.Acad.AcadJigs.MyPolyLine myPolyLine = CurrentEntity as Atend.Global.Acad.AcadJigs.MyPolyLine;

            if (myPolyLine != null)
            {

                myPolyLine.AdditionalDictionary.Add("ProductType", CurrentProductType);
            }
            else
            {

                Atend.Global.Acad.AcadJigs.MyCircle _MyCircle = CurrentEntity as Atend.Global.Acad.AcadJigs.MyCircle;

                if (_MyCircle != null)
                {
                    _MyCircle.AdditionalDictionary.Add("ProductType", CurrentProductType);
                }
                else
                {

                    MyLine _Line = CurrentEntity as MyLine;

                    if (_Line != null)
                    {

                        _Line.AdditionalDictionary.Add("ProductType", CurrentProductType);

                    }
                }

            }


        }

        public static void SaveExtensionData(Entity CurrentEntity, long CurrentProductCode)
        {

            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;

            Atend.Global.Acad.AcadJigs.MyPolyLine myPolyLine = CurrentEntity as Atend.Global.Acad.AcadJigs.MyPolyLine;

            if (myPolyLine != null)
            {

                myPolyLine.AdditionalDictionary.Add("ProductCode", CurrentProductCode);
            }
            else
            {

                Atend.Global.Acad.AcadJigs.MyCircle _MyCircle = CurrentEntity as Atend.Global.Acad.AcadJigs.MyCircle;

                if (_MyCircle != null)
                {
                    _MyCircle.AdditionalDictionary.Add("ProductCode", CurrentProductCode);
                }
                else
                {

                    MyLine _Line = CurrentEntity as MyLine;

                    if (_Line != null)
                    {

                        _Line.AdditionalDictionary.Add("ProductCode", CurrentProductCode);

                    }
                }

            }


        }

        public static void SaveExtensionData(Entity CurrentEntity, String CodeGuid)
        {

            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;

            Atend.Global.Acad.AcadJigs.MyPolyLine myPolyLine = CurrentEntity as Atend.Global.Acad.AcadJigs.MyPolyLine;

            if (myPolyLine != null)
            {

                myPolyLine.AdditionalDictionary.Add("Code", CodeGuid);
            }
            else
            {

                Atend.Global.Acad.AcadJigs.MyCircle _MyCircle = CurrentEntity as Atend.Global.Acad.AcadJigs.MyCircle;

                if (_MyCircle != null)
                {
                    _MyCircle.AdditionalDictionary.Add("Code", CodeGuid);
                }
                else
                {

                    MyLine _Line = CurrentEntity as MyLine;

                    if (_Line != null)
                    {

                        _Line.AdditionalDictionary.Add("Code", CodeGuid);

                    }
                }

            }


        }

   


        #region Cable-Air Jig

        //public class DrawCableAirJig : DrawJig
        //{


        //    Point3d CenterPoint = Point3d.Origin, LastPoint;
        //    //Point3dCollection p3c;
        //    Entity ContainerEntity = null;
        //    List<Entity> Entities = new List<Entity>();


        //    public DrawCableAirJig(Entity Container)
        //    {

        //        //p3c = ConvertEntityToPoint3dCollection(Container);
        //        ContainerEntity = Container;
        //        Entities.Add(DrawConnectionPoint(CenterPoint));

        //    }

        //    private Entity DrawConnectionPoint(Point3d CenterPoint)
        //    {
        //        MyCircle c = new MyCircle();
        //        c.Normal = Vector3d.ZAxis;
        //        c.ColorIndex = 3;
        //        c.Center = CenterPoint;
        //        c.Radius = 5;

        //        SaveExtensionData(c, (int)Atend.Control.Enum.ProductType.ConnectionPoint);
        //        return c;

        //    }

        //    protected override SamplerStatus Sampler(JigPrompts prompts)
        //    {
        //        //throw new System.Exception("The method or operation is not implemented.");

        //        JigPromptPointOptions ppo = new JigPromptPointOptions("\nSelect Cable Position:");
        //        PromptPointResult ppr = prompts.AcquirePoint(ppo);

        //        if (ppr.Status == PromptStatus.OK)
        //        {

        //            if (Atend.Global.Acad.UAcad.IsInsideCurve((Curve)ContainerEntity, ppr.Value) == true)
        //            {


        //                if (CenterPoint == ppr.Value)
        //                {
        //                    CenterPoint = ppr.Value;
        //                    return SamplerStatus.OK;
        //                }

        //                else
        //                {
        //                    return SamplerStatus.NoChange;
        //                }

        //            }
        //            else
        //            {
        //                return SamplerStatus.NoChange;
        //            }

        //        }
        //        else
        //        {
        //            return SamplerStatus.Cancel;
        //        }


        //    }

        //    protected override bool WorldDraw(Autodesk.AutoCAD.GraphicsInterface.WorldDraw draw)
        //    {
        //        //throw new System.Exception("The method or operation is not implemented.");

        //        Entities.Clear();
        //        Entities.Add(DrawConnectionPoint(CenterPoint));


        //        foreach (Entity ent in Entities)
        //        {
        //            draw.Geometry.Draw(ent);
        //        }

        //        return true;

        //    }


        //}

        #endregion


        #region Conductor Jig
        //~~~~~~~~~ Start Conductor Jig Part ~~~~~~~~~~~~~~~~~~~~
        //public class DrawConductorJig : EntityJig
        //{

        //    public Point3dCollection P3DC = new Point3dCollection();
        //    public Point3d NewPoint;
        //    bool contiLine = false;

        //    public DrawConductorJig()
        //        : base(new Line())
        //    {

        //        P3DC.Clear();

        //    }



        //    protected override SamplerStatus Sampler(JigPrompts prompts)
        //    {


        //        JigPromptPointOptions jigOpts = new JigPromptPointOptions();
        //        jigOpts.UserInputControls = (UserInputControls.Accept3dCoordinates |
        //                                     UserInputControls.NullResponseAccepted |
        //                                     UserInputControls.NoNegativeResponseAccepted);

        //        if (P3DC.Count == 0)
        //        {
        //            // For the first vertex, just ask for the point
        //            jigOpts.Message = "\nStart Point Of Conductor: ";
        //        }
        //        else if (P3DC.Count == 1)
        //        {
        //            // For subsequent vertices, use a base point
        //            jigOpts.BasePoint = P3DC[P3DC.Count - 1];
        //            jigOpts.UseBasePoint = true;
        //            jigOpts.Message = "\nEnd Point Of Conductor: ";
        //        }

        //        PromptPointResult res = prompts.AcquirePoint(jigOpts);
        //        if (NewPoint == res.Value)
        //        {
        //            return SamplerStatus.NoChange;
        //        }
        //        else if (res.Status == PromptStatus.OK)
        //        {
        //            NewPoint = res.Value;
        //            P3DC.Add(NewPoint);
        //            return SamplerStatus.OK;
        //        }
        //        return SamplerStatus.Cancel;


        //    }

        //    protected override bool Update()
        //    {

        //        //Line l = Entity as Line;

        //        //if (P3DC.Count == 1)
        //        //{
        //        //    l.StartPoint = P3DC[0];
        //        //}
        //        //else if (P3DC.Count > 1)
        //        //{
        //        //    l.EndPoint = P3DC[1];
        //        //}

        //        if (contiLine)
        //        {
        //            Line l = Entity as Line;

        //            l.EndPoint = NewPoint;
        //        }

        //        if (P3DC.Count >= 1)
        //        {
        //            P3DC.Clear();
        //        }

        //        return true;
        //    }

        //    public Entity GetEntity()
        //    {
        //        return Entity;
        //    }

        //    public void SetStartPoint(Point3d StartPoint)
        //    {

        //        Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;

        //        //ed.WriteMessage("SSP1 \n");
        //        Line line = Entity as Line;

        //        //ed.WriteMessage("SSP2 \n");
        //        P3DC.Add(StartPoint);

        //        //ed.WriteMessage("SSP3 \n");
        //        line.StartPoint = StartPoint;

        //        //ed.WriteMessage("SSP4 \n");
        //        Update();

        //        //ed.WriteMessage("SSP5 \n");
        //        contiLine = true;

        //        //ed.WriteMessage("start point finished \n");
        //    }

        //    public void SetEndPoint(Point3d EndPoint)
        //    {

        //        Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;

        //        Line line = Entity as Line;

        //        //ed.WriteMessage("1 \n");
        //        //P3DC[1] = EndPoint;

        //        //ed.WriteMessage("2 \n");
        //        line.EndPoint = EndPoint;


        //        //ed.WriteMessage("3 \n");
        //        contiLine = false;

        //        //ed.WriteMessage("4 \n");
        //        //ed.WriteMessage("end point finished \n");
        //    }


        //}
        //~~~~~~~~~ End Conductor Jig Part ~~~~~~~~~~~~~~~~~~~~
        #endregion



        #region Consol Jig

        //public class DrawConsol : DrawJig
        //{


        //    Point3d CenterPoint = Point3d.Origin;
        //    Entity ConsolEntity;
        //    //Point3dCollection ContainerCollection;
        //    Entity ContainerEntity = null;
        //    private TextStyle _style;



        //    public DrawConsol(Entity Container)
        //    {
        //        // SET TEXT STYLE

        //        ContainerEntity = Container;

        //        _style = new TextStyle();

        //        _style.Font = new FontDescriptor("Calibri", false, true, 0, 0);

        //        _style.TextSize = 10;

        //        // END OF SET TEXT STYLE

        //        //ContainerCollection = AcadJigs.ConvertEntityToPoint3dCollection(Container);

        //        ConsolEntity = CreateConsolEntity(CenterPoint, 50, 50);

        //    }

        //    public class MyPolyLine : Polyline
        //    {

        //        Dictionary<string, object> additionalDictionary = new Dictionary<string, object>();

        //        public Dictionary<string, object> AdditionalDictionary
        //        {
        //            get { return additionalDictionary; }
        //            set { additionalDictionary = value; }
        //        }


        //    }


        //    private MyPolyLine CreateConsolEntity(Point3d CenterPoint, double Width, double Height)
        //    {

        //        double BaseX = CenterPoint.X;
        //        double BaseY = CenterPoint.Y;

        //        MyPolyLine pLine = new MyPolyLine();
        //        pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX - (Width / 2), BaseY - (Height / 2)), 0, 0, 0);
        //        pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX + (Width / 2), BaseY - (Height / 2)), 0, 0, 0);
        //        pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX + (Width / 2), BaseY + (Height / 2)), 0, 0, 0);
        //        pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX - (Width / 2), BaseY + (Height / 2)), 0, 0, 0);
        //        pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX - (Width / 2), BaseY - (Height / 2)), 0, 0, 0);
        //        pLine.Closed = true;

        //        //ed.WriteMessage("B1 \n");
        //        //SaveExtensionData(pLine, (int)Atend.Control.Enum.ProductType.Pole);


        //        return pLine;

        //    }

        //    protected override SamplerStatus Sampler(JigPrompts prompts)
        //    {
        //        //throw new System.Exception("The method or operation is not implemented.");

        //        JigPromptPointOptions ppo = new JigPromptPointOptions("Enter Consol Position : ");
        //        PromptPointResult ppr = prompts.AcquirePoint(ppo);

        //        if (ppr.Status == PromptStatus.OK)
        //        {
        //            if (ppr.Value == CenterPoint)
        //            {
        //                return SamplerStatus.NoChange;
        //            }
        //            else
        //            {

        //                //if(AcadJigs.InsidePolygon(


        //                if (Atend.Global.Acad.UAcad.IsInsideCurve((Curve)ContainerEntity, ppr.Value) == true)
        //                {
        //                    CenterPoint = ppr.Value;
        //                    return SamplerStatus.OK;

        //                }
        //                else
        //                {
        //                    return SamplerStatus.Cancel;
        //                }

        //            }

        //        }
        //        else
        //        {
        //            return SamplerStatus.Cancel;
        //        }



        //        return SamplerStatus.OK;
        //    }

        //    protected override bool WorldDraw(Autodesk.AutoCAD.GraphicsInterface.WorldDraw draw)
        //    {
        //        Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;

        //        // SHOW POSITION VALUE

        //        WorldGeometry2 wg2 = draw.Geometry as WorldGeometry2;

        //        if (wg2 != null)
        //        {

        //            // Push our transforms onto the stack

        //            wg2.PushOrientationTransform(OrientationBehavior.Screen);

        //            wg2.PushPositionTransform(PositionBehavior.Screen, new Point2d(30, 30));

        //            // Draw our screen-fixed text

        //            wg2.Text(

        //                new Point3d(0, 0, 0),  // Position

        //                new Vector3d(0, 0, 1), // Normal

        //                new Vector3d(1, 0, 0), // Direction

        //                CenterPoint.ToString(), // Text

        //                true,                  // Rawness

        //                _style                // TextStyle

        //                    );


        //            // Remember to pop our transforms off the stack

        //            wg2.PopModelTransform();

        //            wg2.PopModelTransform();

        //        }

        //        // END OF SHOW POSITION VALUE



        //        ConsolEntity = CreateConsolEntity(CenterPoint, 50, 50);
        //        draw.Geometry.Draw(ConsolEntity);
        //        return true;
        //    }
        //}

        #endregion


        #region OneDirectionConsol

        //public class DrawOneDirectionConsol : DrawJig
        //{

        //    Point3d CenterPoint = Point3d.Origin, SecondPoint;
        //    //Point3dCollection ContainerCollection;
        //    Entity ContainerEntity = null;
        //    List<Entity> Entities = new List<Entity>();
        //    public bool PartTwo = false;
        //    private TextStyle _style;
        //    Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;


        //    public DrawOneDirectionConsol(Entity Container)
        //    {

        //        ContainerEntity = Container;
        //        // SET TEXT STYLE

        //        _style = new TextStyle();

        //        _style.Font = new FontDescriptor("Calibri", false, true, 0, 0);

        //        _style.TextSize = 10;

        //        // END OF SET TEXT STYLE



        //        //ContainerCollection = AcadJigs.ConvertEntityToPoint3dCollection(Container);
        //        //ed.WriteMessage("after convert entity. \n");
        //        SecondPoint = new Point3d(CenterPoint.X + 10, CenterPoint.Y + 40, CenterPoint.Z);
        //        //ed.WriteMessage("after define point 2th.\n");


        //        Entities.Add(CreatePartOneEntity(CenterPoint, 10, 10));
        //        //ed.WriteMessage("One.\n");
        //        Entities.Add(CreatePartTwoEntity(CenterPoint, SecondPoint));
        //        //ed.WriteMessage("Two.\n");
        //        Entities.Add(CreatePartThreeEntity(CenterPoint, SecondPoint, 10));
        //        //ed.WriteMessage("Three.\n");


        //    }


        //    public class MyPolyLine : Polyline
        //    {

        //        Dictionary<string, object> additionalDictionary = new Dictionary<string, object>();

        //        public Dictionary<string, object> AdditionalDictionary
        //        {
        //            get { return additionalDictionary; }
        //            set { additionalDictionary = value; }
        //        }


        //    }


        //    private Entity CreatePartOneEntity(Point3d CenterPoint, double Width, double Height)
        //    {

        //        double BaseX = CenterPoint.X;
        //        double BaseY = CenterPoint.Y;

        //        MyPolyLine pLine = new MyPolyLine();
        //        pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX - (Width / 2), BaseY - (Height / 2)), 0, 0, 0);
        //        pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX + (Width / 2), BaseY - (Height / 2)), 0, 0, 0);
        //        pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX + (Width / 2), BaseY + (Height / 2)), 0, 0, 0);
        //        pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX - (Width / 2), BaseY + (Height / 2)), 0, 0, 0);
        //        pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX - (Width / 2), BaseY - (Height / 2)), 0, 0, 0);
        //        pLine.Closed = true;

        //        //ed.WriteMessage("B1 \n");
        //        //SaveExtensionData(pLine, (int)Atend.Control.Enum.ProductType.Pole);

        //        return pLine;




        //    }


        //    private Entity CreatePartTwoEntity(Point3d StartPoint, Point3d EndPoint)
        //    {

        //        Line l = new Line();

        //        l.StartPoint = StartPoint;
        //        l.EndPoint = EndPoint;

        //        return l;

        //    }


        //    private Entity CreatePartThreeEntity(Point3d _CenterPoint, Point3d PointOnArc, double _Angle)
        //    {


        //        double Radius = CenterPoint.DistanceTo(PointOnArc);

        //        //Line l = new Line(_CenterPoint, PointOnArc);
        //        //l.StartPoint = _CenterPoint;
        //        //l.EndPoint = PointOnArc;

        //        Vector2d x = new Point2d(CenterPoint.X, CenterPoint.Y).GetVectorTo(new Point2d(PointOnArc.X, PointOnArc.Y));



        //        double Angle = (180 * x.Angle) / Math.PI;
        //        double Start, End;
        //        //NewAngle = Angle;
        //        //ed.WriteMessage("Angle is : {0} \n", Angle);
        //        Start = Angle - _Angle;
        //        Start = (Start * Math.PI) / 180;
        //        End = Angle + _Angle;
        //        End = (End * Math.PI) / 180;
        //        //double Angle = l.Angle;


        //        Arc a = new Arc(CenterPoint, Radius, Start, End);

        //        return a;

        //    }


        //    protected override SamplerStatus Sampler(JigPrompts prompts)
        //    {
        //        //throw new System.Exception("The method or operation is not implemented.");


        //        if (!PartTwo)
        //        {

        //            JigPromptPointOptions ppo = new JigPromptPointOptions("\nEnter Tensional Consol Position: ");
        //            PromptPointResult ppr = prompts.AcquirePoint(ppo);
        //            if (ppr.Status == PromptStatus.OK)
        //            {

        //                if (ppr.Value == CenterPoint)
        //                {
        //                    return SamplerStatus.NoChange;
        //                }
        //                else
        //                {

        //                    if (Atend.Global.Acad.UAcad.IsInsideCurve((Curve)ContainerEntity, ppr.Value) == true)
        //                    {

        //                        CenterPoint = ppr.Value;

        //                        return SamplerStatus.OK;
        //                    }
        //                    else
        //                    {
        //                        return SamplerStatus.NoChange;
        //                    }

        //                }

        //            }

        //            else
        //            {
        //                return SamplerStatus.Cancel;
        //            }

        //        }
        //        else
        //        {

        //            //PartTwo=True;

        //            JigPromptPointOptions ppo = new JigPromptPointOptions("\nNext Point:");
        //            PromptPointResult ppr = prompts.AcquirePoint(ppo);

        //            if (ppr.Status == PromptStatus.OK)
        //            {

        //                if (ppr.Value == SecondPoint)
        //                {
        //                    return SamplerStatus.NoChange;
        //                }
        //                else
        //                {

        //                    //Distance = ppr.Value.DistanceTo(CenterPoint);

        //                    //Line l = new Line(ppr.Value, CenterPoint);

        //                    //NewAngle = l.Angle;

        //                    SecondPoint = ppr.Value;

        //                    return SamplerStatus.OK;
        //                }

        //            }
        //            else
        //            {
        //                return SamplerStatus.Cancel;
        //            }


        //        }
        //    }


        //    protected override bool WorldDraw(Autodesk.AutoCAD.GraphicsInterface.WorldDraw draw)
        //    {
        //        //throw new System.Exception("The method or operation is not implemented.");

        //        Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;

        //        double angle = 0;





        //        if (!PartTwo)
        //        {
        //            Entities.Clear();

        //            SecondPoint = new Point3d(CenterPoint.X + 10, CenterPoint.Y + 40, CenterPoint.Z);
        //            Entities.Add(CreatePartOneEntity(CenterPoint, 10, 10));
        //            Entities.Add(CreatePartTwoEntity(CenterPoint, SecondPoint));


        //            //Line l = new Line(CenterPoint, SecondPoint);

        //            //angle = l.Angle;

        //            //angle = (180 * angle) / Math.PI;



        //            Entities.Add(CreatePartThreeEntity(SecondPoint, SecondPoint, 10));



        //        }
        //        else
        //        {

        //            Entities.Clear();

        //            // PartTwo=True
        //            //SecondPoint = CenterPoint.Add(new Vector3d(10, 40, 0));
        //            Entities.Add(CreatePartOneEntity(CenterPoint, 10, 10));
        //            Entities.Add(CreatePartTwoEntity(CenterPoint, SecondPoint));


        //            //Line l = new Line(CenterPoint, SecondPoint);

        //            //angle = l.Angle;

        //            //angle = (180 * angle) / Math.PI;

        //            Entities.Add(CreatePartThreeEntity(CenterPoint, SecondPoint, 30));


        //        }

        //        // SHOW POSITION VALUE

        //        WorldGeometry2 wg2 = draw.Geometry as WorldGeometry2;

        //        if (wg2 != null)
        //        {

        //            // Push our transforms onto the stack

        //            wg2.PushOrientationTransform(OrientationBehavior.Screen);

        //            wg2.PushPositionTransform(PositionBehavior.Screen, new Point2d(30, 30));

        //            // Draw our screen-fixed text

        //            wg2.Text(

        //                new Point3d(0, 0, 0),  // Position

        //                new Vector3d(0, 0, 1), // Normal

        //                new Vector3d(1, 0, 0), // Direction

        //                CenterPoint.ToString(), // Text

        //                true,                  // Rawness

        //                _style                // TextStyle

        //                    );


        //            // Remember to pop our transforms off the stack

        //            wg2.PopModelTransform();

        //            wg2.PopModelTransform();

        //        }

        //        // END OF SHOW POSITION VALUE


        //        foreach (Entity e in Entities)
        //        {
        //            draw.Geometry.Draw(e);
        //        }


        //        return true;

        //    }

        //    public List<Entity> GetEntity()
        //    {
        //        //ed.WriteMessage("GOTO GEt Entity\n");
        //        return Entities;
        //    }

        //}

        #endregion


        #region TwoDirectionConsol

        //public class DrawTwoDirectionConsol : DrawJig
        //{

        //    List<Entity> Entities = new List<Entity>();
        //    public byte ActivePart = 1;
        //    Point3d CenterPoint = Point3d.Origin, PointOne, PointTwo;
        //    //Point3dCollection ContainerCollection;
        //    Entity ContainerEntity = null;


        //    public class MyPolyLine : Polyline
        //    {

        //        Dictionary<string, object> additionalDictionary = new Dictionary<string, object>();

        //        public Dictionary<string, object> AdditionalDictionary
        //        {
        //            get { return additionalDictionary; }
        //            set { additionalDictionary = value; }
        //        }


        //    }


        //    private Entity CreatePartOneEntity(Point3d CenterPoint, double Width, double Height)
        //    {

        //        double BaseX = CenterPoint.X;
        //        double BaseY = CenterPoint.Y;

        //        MyPolyLine pLine = new MyPolyLine();
        //        pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX - (Width / 2), BaseY - (Height / 2)), 0, 0, 0);
        //        pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX + (Width / 2), BaseY - (Height / 2)), 0, 0, 0);
        //        pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX + (Width / 2), BaseY + (Height / 2)), 0, 0, 0);
        //        pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX - (Width / 2), BaseY + (Height / 2)), 0, 0, 0);
        //        pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX - (Width / 2), BaseY - (Height / 2)), 0, 0, 0);
        //        pLine.Closed = true;

        //        //ed.WriteMessage("B1 \n");
        //        //SaveExtensionData(pLine, (int)Atend.Control.Enum.ProductType.Pole);

        //        return pLine;


        //    }


        //    private Entity CreatePartTwoEntity(Point3d StartPoint, Point3d EndPoint)
        //    {

        //        Line l = new Line();

        //        l.StartPoint = StartPoint;
        //        l.EndPoint = EndPoint;

        //        return l;

        //    }


        //    private Entity CreatePartThreeEntity(Point3d _CenterPoint, Point3d PointOnArc, double _Angle)
        //    {


        //        double Radius = CenterPoint.DistanceTo(PointOnArc);

        //        //Line l = new Line(_CenterPoint, PointOnArc);
        //        //l.StartPoint = _CenterPoint;
        //        //l.EndPoint = PointOnArc;

        //        Vector2d x = new Point2d(CenterPoint.X, CenterPoint.Y).GetVectorTo(new Point2d(PointOnArc.X, PointOnArc.Y));



        //        double Angle = (180 * x.Angle) / Math.PI;
        //        double Start, End;
        //        //NewAngle = Angle;
        //        //ed.WriteMessage("Angle is : {0} \n", Angle);
        //        Start = Angle - _Angle;
        //        Start = (Start * Math.PI) / 180;
        //        End = Angle + _Angle;
        //        End = (End * Math.PI) / 180;
        //        //double Angle = l.Angle;


        //        Arc a = new Arc(CenterPoint, Radius, Start, End);

        //        return a;

        //    }


        //    public DrawTwoDirectionConsol(Entity Container)
        //    {

        //        ContainerEntity = Container;

        //        PointOne = new Point3d(CenterPoint.X + 10, CenterPoint.Y + 40, CenterPoint.Z);
        //        PointTwo = new Point3d(CenterPoint.X - 10, CenterPoint.Y - 40, CenterPoint.Z);

        //        Entities.Add(CreatePartOneEntity(CenterPoint, 10, 10));
        //        Entities.Add(CreatePartTwoEntity(CenterPoint, PointOne));
        //        Entities.Add(CreatePartThreeEntity(CenterPoint, PointOne, 20));
        //        Entities.Add(CreatePartTwoEntity(CenterPoint, PointTwo));
        //        Entities.Add(CreatePartThreeEntity(CenterPoint, PointTwo, 20));

        //        //ContainerCollection = ConvertEntityToPoint3dCollection(Container);

        //    }


        //    protected override SamplerStatus Sampler(JigPrompts prompts)
        //    {
        //        JigPromptPointOptions ppo = new JigPromptPointOptions();
        //        PromptPointResult ppr;

        //        if (ActivePart == 1)
        //        {

        //            ppo.Message = "Select Consol Position : \n";
        //            ppr = prompts.AcquirePoint(ppo);

        //            if (ppr.Status == PromptStatus.OK)
        //            {


        //                if (ppr.Value == CenterPoint)
        //                {
        //                    return SamplerStatus.NoChange;
        //                }
        //                else
        //                {

        //                    if (Atend.Global.Acad.UAcad.IsInsideCurve((Curve)ContainerEntity, ppr.Value) == true)
        //                    {
        //                        CenterPoint = ppr.Value;
        //                        return SamplerStatus.OK;
        //                    }
        //                    else
        //                    {
        //                        return SamplerStatus.NoChange;
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                return SamplerStatus.Cancel;
        //            }



        //        }
        //        else if (ActivePart == 2)
        //        {
        //            ppo.Message = "Select ARC one poisition : \n";
        //            ppr = prompts.AcquirePoint(ppo);

        //            if (ppr.Status == PromptStatus.OK)
        //            {


        //                if (ppr.Value == PointOne)
        //                {
        //                    return SamplerStatus.NoChange;
        //                }
        //                else
        //                {

        //                    PointOne = ppr.Value;
        //                    return SamplerStatus.OK;
        //                }
        //            }
        //            else
        //            {
        //                return SamplerStatus.Cancel;
        //            }


        //        }
        //        else if (ActivePart == 3)
        //        {
        //            ppo.Message = "Select ARC two position : \n"; ;
        //            ppr = prompts.AcquirePoint(ppo);

        //            if (ppr.Status == PromptStatus.OK)
        //            {


        //                if (ppr.Value == PointTwo)
        //                {
        //                    return SamplerStatus.NoChange;
        //                }
        //                else
        //                {

        //                    PointTwo = ppr.Value;
        //                    return SamplerStatus.OK;
        //                }
        //            }
        //            else
        //            {
        //                return SamplerStatus.Cancel;
        //            }



        //        }

        //        return SamplerStatus.Cancel;
        //    }


        //    protected override bool WorldDraw(Autodesk.AutoCAD.GraphicsInterface.WorldDraw draw)
        //    {
        //        //throw new System.Exception("The method or operation is not implemented.");

        //        Entities.Clear();


        //        if (ActivePart == 1)
        //        {

        //            PointOne = new Point3d(CenterPoint.X + 10, CenterPoint.Y + 40, CenterPoint.Z);
        //            PointTwo = new Point3d(CenterPoint.X - 10, CenterPoint.Y - 40, CenterPoint.Z);

        //            Entities.Add(CreatePartOneEntity(CenterPoint, 10, 10));
        //            Entities.Add(CreatePartTwoEntity(CenterPoint, PointOne));
        //            Entities.Add(CreatePartThreeEntity(CenterPoint, PointOne, 20));
        //            Entities.Add(CreatePartTwoEntity(CenterPoint, PointTwo));
        //            Entities.Add(CreatePartThreeEntity(CenterPoint, PointTwo, 20));
        //        }
        //        else if (ActivePart == 2)
        //        {

        //            PointTwo = new Point3d(CenterPoint.X - 10, CenterPoint.Y - 40, CenterPoint.Z);

        //            Entities.Add(CreatePartOneEntity(CenterPoint, 10, 10));
        //            Entities.Add(CreatePartTwoEntity(CenterPoint, PointOne));
        //            Entities.Add(CreatePartThreeEntity(CenterPoint, PointOne, 20));
        //            Entities.Add(CreatePartTwoEntity(CenterPoint, PointTwo));
        //            Entities.Add(CreatePartThreeEntity(CenterPoint, PointTwo, 20));


        //        }
        //        else if (ActivePart == 3)
        //        {

        //            Entities.Add(CreatePartOneEntity(CenterPoint, 10, 10));
        //            Entities.Add(CreatePartTwoEntity(CenterPoint, PointOne));
        //            Entities.Add(CreatePartThreeEntity(CenterPoint, PointOne, 20));
        //            Entities.Add(CreatePartTwoEntity(CenterPoint, PointTwo));
        //            Entities.Add(CreatePartThreeEntity(CenterPoint, PointTwo, 20));

        //        }

        //        foreach (Entity ent in Entities)
        //        {
        //            draw.Geometry.Draw(ent);
        //        }

        //        return true;
        //    }

        //    public List<Entity> GetEntity()
        //    {
        //        return Entities;
        //    }


        //}


        #endregion


        #region PostCell Jig

        //public class DrwaPostCell : DrawJig
        //{

        //    //Point3dCollection ContainetCellection;
        //    Entity ContainerEntity = null;
        //    Point3d CenterPoint = Point3d.Origin;


        //    public DrwaPostCell(Entity Contaier)
        //    {

        //        ContainerEntity = Contaier;
        //        //ContainetCellection = ConvertEntityToPoint3dCollection(Contaier);
        //        CreatePartOneEntity(CenterPoint, 10, 30);
        //        CreatePartTwoEntity(CenterPoint, 10, 2);

        //    }

        //    public class MyPolyLine : Polyline
        //    {

        //        Dictionary<string, object> additionalDictionary = new Dictionary<string, object>();

        //        public Dictionary<string, object> AdditionalDictionary
        //        {
        //            get { return additionalDictionary; }
        //            set { additionalDictionary = value; }
        //        }


        //    }

        //    private Entity CreatePartOneEntity(Point3d CenterPoint, double Width, double Height)
        //    {

        //        double BaseX = CenterPoint.X;
        //        double BaseY = CenterPoint.Y;

        //        MyPolyLine pLine = new MyPolyLine();
        //        pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX - (Width / 2), BaseY - (Height / 2)), 0, 0, 0);
        //        pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX + (Width / 2), BaseY - (Height / 2)), 0, 0, 0);
        //        pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX + (Width / 2), BaseY + (Height / 2)), 0, 0, 0);
        //        pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX - (Width / 2), BaseY + (Height / 2)), 0, 0, 0);
        //        pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX - (Width / 2), BaseY - (Height / 2)), 0, 0, 0);
        //        pLine.Closed = true;

        //        //ed.WriteMessage("B1 \n");
        //        //SaveExtensionData(pLine, (int)Atend.Control.Enum.ProductType.Pole);

        //        return pLine;

        //    }


        //    private Entity CreatePartTwoEntity(Point3d CenterPoint, double Width, double Height)
        //    {

        //        // it sould get filled

        //        double BaseX = CenterPoint.X;
        //        double BaseY = CenterPoint.Y;

        //        MyPolyLine pLine = new MyPolyLine();
        //        pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX - (Width / 2), BaseY - (Height / 2)), 0, 0, 0);
        //        pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX + (Width / 2), BaseY - (Height / 2)), 0, 0, 0);
        //        pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX + (Width / 2), BaseY + (Height / 2)), 0, 0, 0);
        //        pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX - (Width / 2), BaseY + (Height / 2)), 0, 0, 0);
        //        pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX - (Width / 2), BaseY - (Height / 2)), 0, 0, 0);
        //        pLine.Closed = true;

        //        //ed.WriteMessage("B1 \n");
        //        //SaveExtensionData(pLine, (int)Atend.Control.Enum.ProductType.Pole);

        //        return pLine;



        //    }


        //    protected override SamplerStatus Sampler(JigPrompts prompts)
        //    {
        //        //throw new System.Exception("The method or operation is not implemented.");
        //        return SamplerStatus.OK;
        //    }


        //    protected override bool WorldDraw(Autodesk.AutoCAD.GraphicsInterface.WorldDraw draw)
        //    {
        //        //throw new System.Exception("The method or operation is not implemented.");
        //        return true;
        //    }


        //}

        #endregion

  


        #region Terminal Jig

        //public class DrawTerminalJig : EntityJig
        //{
        //    public Point3d tempPoint = Point3d.Origin;
        //    Point3dCollection p3c = new Point3dCollection();

        //    public DrawTerminalJig()
        //        : base(new Polyline())
        //    {

        //        Polyline pl = Entity as Polyline;
        //        if (pl != null)
        //        {
        //            pl.AddVertexAt(pl.NumberOfVertices, new Point2d(tempPoint.X, tempPoint.Y), 0, 0, 0);
        //        }

        //    }

        //    protected override SamplerStatus Sampler(JigPrompts prompts)
        //    {

        //        JigPromptPointOptions ppo = new JigPromptPointOptions();
        //        ppo.UserInputControls = (UserInputControls.Accept3dCoordinates |
        //                                     UserInputControls.NullResponseAccepted |
        //                                     UserInputControls.NoNegativeResponseAccepted);
        //        ppo.Message = "\nSelect Point:";


        //        if (p3c.Count > 0)
        //        {
        //            ppo.UseBasePoint = true;
        //            ppo.BasePoint = new Point3d(100, 100, 0); //p3c[p3c.Count - 1];
        //            //ppo.Keywords.Add(p3c.Count.ToString());
        //            //ppo.Keywords.Add(p3c[p3c.Count - 1].ToString());

        //        }

        //        PromptPointResult pr = prompts.AcquirePoint(ppo);
        //        if (pr.Status == PromptStatus.OK)
        //        {

        //            if (tempPoint == pr.Value)
        //            {
        //                return SamplerStatus.NoChange;
        //            }
        //            else
        //            {
        //                tempPoint = pr.Value;
        //                return SamplerStatus.OK;
        //            }
        //        }
        //        else
        //        {
        //            return SamplerStatus.Cancel;
        //        }
        //    }

        //    protected override bool Update()
        //    {
        //        Polyline pl = Entity as Polyline;
        //        if (pl != null)
        //        {
        //            pl.SetPointAt(pl.NumberOfVertices - 1, new Point2d(tempPoint.X, tempPoint.Y));
        //        }
        //        return false;
        //    }

        //    public void AddVertex()
        //    {
        //        Polyline pl = Entity as Polyline;
        //        if (pl != null)
        //        {
        //            pl.AddVertexAt(pl.NumberOfVertices, new Point2d(p3c[p3c.Count - 1].X, p3c[p3c.Count - 1].Y), 0, 0, 0);
        //        }
        //    }

        //    public void AddPoint()
        //    {
        //        p3c.Add(tempPoint);
        //    }

        //    public void SetLastPoint(Point3d MyPoint)
        //    {
        //        Polyline pl = Entity as Polyline;
        //        pl.SetPointAt(pl.NumberOfVertices - 1, new Point2d(MyPoint.X, MyPoint.Y));
        //    }

        //    public void SetStartPoint(Point3d MyPoint)
        //    {
        //        Polyline pl = Entity as Polyline;
        //        pl.SetPointAt(0, new Point2d(MyPoint.X, MyPoint.Y));
        //    }

        //    public void AddPoint(Point3d MyPoint)
        //    {
        //        p3c.Add(MyPoint);
        //    }

        //    public Entity GetEntity()
        //    {
        //        return Entity;
        //    }

        //    public void RemoveLastVertex()
        //    {
        //        Polyline pl = Entity as Polyline;
        //        pl.RemoveVertexAt(pl.NumberOfVertices - 1);
        //    }

        //}

        #endregion

    


        #region ContinousLine Jig

        //public class DrawContinousLineJig : EntityJig
        //{

        //    Entity ent = null;
        //    public Point3d CentrePoint = Point3d.Origin;

        //    public DrawContinousLineJig()
        //        : base(new Polyline())
        //    {
        //    }


        //    protected override SamplerStatus Sampler(JigPrompts prompts)
        //    {

        //        JigPromptPointOptions ppo = new JigPromptPointOptions("\nSelect Point:");
        //        PromptPointResult ppr = prompts.AcquirePoint(ppo);
        //        if (ppr.Status == PromptStatus.OK)
        //        {
        //            if (ppr.Value == CentrePoint)
        //            {
        //                return SamplerStatus.NoChange;
        //            }
        //            else
        //            {
        //                CentrePoint = ppr.Value;
        //                return SamplerStatus.OK;
        //            }
        //        }
        //        else
        //        {
        //            return SamplerStatus.Cancel;
        //        }

        //    }


        //    protected override bool Update()
        //    {

        //        Polyline _pl = Entity as Polyline;
        //        if (_pl != null)
        //        {
        //            if (_pl.NumberOfVertices == 2)
        //            {
        //                _pl.SetPointAt(_pl.NumberOfVertices - 1, new Point2d(CentrePoint.X, CentrePoint.Y));
        //            }
        //        }

        //        return true;
        //    }


        //    public void AddVertex()
        //    {
        //        Polyline _pl = Entity as Polyline;
        //        if (_pl != null)
        //        {
        //            //_pl.SetPointAt(1, new Point2d(EndPoint.X, EndPoint.Y));
        //            _pl.AddVertexAt(_pl.NumberOfVertices, new Point2d(CentrePoint.X, CentrePoint.Y), 0, 0, 0);
        //        }

        //    }

        //    public void SetEndPoint(Point3d EndPoint)
        //    {
        //        Polyline _pl = Entity as Polyline;
        //        if (_pl != null)
        //        {
        //            //_pl.SetPointAt(1, new Point2d(EndPoint.X, EndPoint.Y));
        //            _pl.SetPointAt(_pl.NumberOfVertices - 1, new Point2d(EndPoint.X, EndPoint.Y));
        //        }
        //    }

        //    public void SetStartPoint(Point3d StartPoint)
        //    {
        //        Polyline _pl = Entity as Polyline;
        //        if (_pl != null)
        //        {
        //            _pl.AddVertexAt(_pl.NumberOfVertices, new Point2d(StartPoint.X, StartPoint.Y), 0, 0, 0);
        //        }
        //    }


        //    public Entity GetEntity()
        //    {
        //        return Entity;
        //    }

        //}

        #endregion

      

        #region CatOut Jig
        //public class DrawCatoutJig : DrawJig
        //{

        //    Point3d StartPoint = Point3d.Origin, EndPoint = Point3d.Origin;
        //    List<Entity> Entities = new List<Entity>();




        //    public DrawCatoutJig()
        //    {



        //    }

        //    private Entity DrawCatOut(Point3d BasePoint, double Width, double Height)
        //    {

        //        double BaseX = BasePoint.X;
        //        double BaseY = BasePoint.Y;

        //        MyPolyLine pLine = new MyPolyLine();
        //        pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX - (Width / 2), BaseY - (Height / 2)), 0, 0, 0);
        //        pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX + (Width / 2), BaseY - (Height / 2)), 0, 0, 0);
        //        pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX + (Width / 2), BaseY + (Height / 2)), 0, 0, 0);
        //        pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX - (Width / 2), BaseY + (Height / 2)), 0, 0, 0);
        //        pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX - (Width / 2), BaseY - (Height / 2)), 0, 0, 0);
        //        pLine.Closed = true;

        //        //SaveExtensionData(pLine, (int)Atend.Control.Enum.ProductType.Consol);
        //        return pLine;

        //    }

        //    private Entity CreateCatout(Line CatoutLine)
        //    {
        //        double LineTul = CatoutLine.Length;
        //        double Tul = 0, Arz = 0;
        //        Tul = LineTul / 3;
        //        Arz = LineTul / 9;

        //        Point3d LineMidPoint = new LineSegment3d(CatoutLine.StartPoint, CatoutLine.EndPoint).MidPoint;
        //        Entity CatoutEntity = DrawCatOut(LineMidPoint, Arz, Tul);

        //        double newAngle = new Point2d(CatoutLine.StartPoint.X, CatoutLine.StartPoint.Y).GetVectorTo(new Point2d(CatoutLine.EndPoint.X, CatoutLine.EndPoint.Y)).Angle;// ((m_deltaAngle - m_baseAngle) * Math.PI) / 180;

        //        //Matrix3d trans = Matrix3d.Rotation(newAngle,
        //        //                                   m_ucs.CoordinateSystem3d.Zaxis,
        //        //                                   LineMidPoint);

        //        //CatoutEntity.TransformBy(trans);

        //        return CatoutEntity;
        //    }

        //    private Entity CreateLine(Point3d StartPoint, Point3d EndPoint, int ProductType, int ColorIndex, double Thickness)
        //    {
        //        MyLine mLine = new MyLine();
        //        mLine.StartPoint = StartPoint;
        //        mLine.EndPoint = EndPoint;

        //        if (Thickness != 0)
        //        {
        //            mLine.Thickness = Thickness;
        //        }


        //        //if (ProductType != 0)
        //        //{
        //        SaveExtensionData(mLine, ProductType);

        //        //}
        //        //if (ColorIndex != 0)
        //        //{
        //        mLine.ColorIndex = ColorIndex;
        //        //}

        //        return mLine;
        //    }

        //    protected override SamplerStatus Sampler(JigPrompts prompts)
        //    {

        //        JigPromptPointOptions ppo = new JigPromptPointOptions();
        //        PromptResult pr;

        //        if (StartPoint != Point3d.Origin)
        //        {
        //            ppo.Message = "\nStartPoint:";



        //        }
        //        else if (EndPoint != Point3d.Origin)
        //        {
        //            ppo.Message = "\nEndPoint:";
        //        }
        //        else
        //        {
        //            return SamplerStatus.Cancel;
        //        }
        //        return SamplerStatus.OK;

        //    }

        //    protected override bool WorldDraw(Autodesk.AutoCAD.GraphicsInterface.WorldDraw draw)
        //    {
        //        return true;
        //    }

        //}
        #endregion

    
        #region Addition Helper Methods

        //---------------- START INNER POINT CODE PART -----------
        //const int INSIDE = 0;
        //const int OUTSIDE = 1;

        //private static double MIN(double x, double y)
        //{
        //    if (x < y)
        //        return x;
        //    else
        //        return y;
        //}

        //private static double MAX(double x, double y)
        //{

        //    if (x > y)
        //        return x;
        //    else
        //        return y;

        //}

        //private static int InsidePolygon(Point3dCollection P3C, Point3d point)
        //{
        //    //ed.WriteMessage("I Am In InsidePolygon\n");
        //    int counter = 0;
        //    int N;
        //    double xinters;
        //    Point3d p1, p2;

        //    p1 = P3C[0];
        //    N = P3C.Count - 1;
        //    //ed.WriteMessage("N= "+N.ToString()+"\n");

        //    for (int i = 1; i <= N; i++)
        //    {
        //        //ed.WriteMessage("i= "+i.ToString()+"\n");
        //        p2 = P3C[i - ((int)(i / N) * i)];
        //        //ed.WriteMessage("{0} REMAIL", (N - ((int)(i / N) * i)));
        //        //ed.WriteMessage("p2= "+p2+"\n");
        //        if (point.Y > MIN(p1.Y, p2.Y))
        //        {
        //            //ed.WriteMessage("I AM IN The First IF\n");
        //            //ed.WriteMessage("Point.y= "+point.Y.ToString()+"\n");
        //            //ed.WriteMessage("p1.Y= "+p1.Y.ToString()+"\n");
        //            //ed.WriteMessage("p2.Y= "+p2.Y.ToString()+"\n");
        //            if (point.Y <= MAX(p1.Y, p2.Y))
        //            {
        //                //ed.WriteMessage("I AM IN The Second IF\n");
        //                //ed.WriteMessage("point.X= "+point.X.ToString()+"\n");
        //                //ed.WriteMessage("p1.X= "+p1.X.ToString()+"\n");
        //                //ed.WriteMessage("p2.X= "+p2.X.ToString()+"\n");
        //                if (point.X <= MAX(p1.X, p2.X))
        //                {
        //                    //ed.WriteMessage("I Am In The Third If\n");
        //                    //ed.WriteMessage("p1.Y= " + p1.Y.ToString() + "\n");
        //                    //ed.WriteMessage("p2.Y= "+p2.Y.ToString()+"\n");

        //                    if (p1.Y != p2.Y)
        //                    {
        //                        //ed.WriteMessage("I Am In The Forth IF\n");

        //                        xinters = (point.Y - p1.Y) * (p2.X - p1.X) / (p2.Y - p1.Y) + p1.X;
        //                        //ed.WriteMessage("Xinters= "+xinters.ToString()+"\n");
        //                        if (p1.X == p2.X || point.X <= xinters)
        //                        {
        //                            //ed.WriteMessage("I Am In The Fifth If\n");
        //                            counter++;
        //                            //ed.WriteMessage("Counter= "+counter.ToString()+"\n");
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        p1 = p2;
        //    }
        //    if (counter - (counter * (int)(counter / 2)) == 0)
        //        return OUTSIDE;
        //    else
        //        return INSIDE;

        //}

        //private static Point3dCollection ConvertEntityToPoint3dCollection(Entity polyLIneEntity)
        //{

        //    Polyline p = polyLIneEntity as Polyline;
        //    Point3dCollection pc = new Point3dCollection();

        //    if (p != null)
        //    {



        //        for (int i = 0; i < p.NumberOfVertices; i++)
        //        {
        //            pc.Add(p.GetPointAtParameter(i));
        //            //ed.WriteMessage("{0} \n", p.GetPointAtParameter(i));
        //        }
        //        //ed.WriteMessage("{0}.:0O0:. \n", InsidePolygon(pc, ppr.Value));

        //    }

        //    return pc;
        //}
        //---------------- END INNER POINT CODE PART -----------

        #endregion


    }
}