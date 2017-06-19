﻿using System;
using System.Collections;
using System.Collections.Generic;
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

//get from tehran 7/15
namespace Atend.Global.Acad.DrawEquips
{
    public class AcDrawPole
    {
        //~~~~~~~~~~~~~~~ Properties ~~~~~~~~~~~~~~~~~~~~~~~~~~~//

        Atend.Base.Design.DNode dNode = new Atend.Base.Design.DNode();
        List<Atend.Base.Design.DPackage> dPackages = new List<Atend.Base.Design.DPackage>();
        Atend.Base.Design.DPackage PolePackage = new Atend.Base.Design.DPackage();
        Atend.Base.Design.DPackage HalterPackage = new Atend.Base.Design.DPackage();


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

        double _Height;
        public double Height
        {
            get { return _Height; }
            set { _Height = value; }
        }

        public Guid EXCode;
        public Guid NodeCode;

        int projectCode;
        public int ProjectCode
        {
            get { return projectCode; }
            set { projectCode = value; }
        }

        int halterprojectCode;
        public int HalterProjectCode
        {
            get { return halterprojectCode; }
            set { halterprojectCode = value; }
        }

        int halterexist;
        public int HalterExist
        {
            get { return halterexist; }
            set { halterexist = value; }
        }

        Atend.Base.Equipment.EHalter _eHalter;
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

        List<Atend.Base.Design.DPackage> beforDPackage = new List<Atend.Base.Design.DPackage>();
        public List<Atend.Base.Design.DPackage> BeforDPackage
        {
            get { return beforDPackage; }
            set { beforDPackage = value; }
        }

        class DrawPoleJig : DrawJig
        {

            private List<Autodesk.AutoCAD.DatabaseServices.Entity> Entities = new List<Autodesk.AutoCAD.DatabaseServices.Entity>();
            Point2d BasePoint = Point2d.Origin;
            public Point2d PoleCenterPoint
            {
                get { return BasePoint; }
                set { BasePoint = value; }
            }

            private double NewAngle, BaseAngle = 0;
            int _InsulatorCount;
            public bool GetPoint = true;
            public bool GetAngle = false;
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            private Autodesk.AutoCAD.GraphicsInterface.TextStyle _style;
            double MyScale = 1;

            public DrawPoleJig(int ConsolCount, double Scale)
            {

                MyScale = Scale;
                // SET TEXT STYLE

                _style = new Autodesk.AutoCAD.GraphicsInterface.TextStyle();

                _style.Font = new Autodesk.AutoCAD.GraphicsInterface.FontDescriptor("Calibri", false, true, 0, 0);

                _style.TextSize = 10;

                // END OF SET TEXT STYLE

                //AddRegAppTableRecord(RegAppName);

                _InsulatorCount = ConsolCount;

                //Add Pole

                Autodesk.AutoCAD.DatabaseServices.Entity poleEntity = CreatePoleEntity(BasePoint);

                Entities.Add(poleEntity);

                //Add Consol

                Autodesk.AutoCAD.DatabaseServices.Entity MyConsolEntity = null;
                switch (_InsulatorCount)
                {
                    case 1:

                        MyConsolEntity = CreateInsulatorEntity(new Point2d(BasePoint.X, BasePoint.Y));
                        Entities.Add(MyConsolEntity);

                        break;
                    case 2:

                        MyConsolEntity = CreateInsulatorEntity(new Point2d(BasePoint.X + 30, BasePoint.Y));
                        Entities.Add(MyConsolEntity);
                        MyConsolEntity = CreateInsulatorEntity(new Point2d(BasePoint.X - 30, BasePoint.Y));
                        Entities.Add(MyConsolEntity);


                        break;
                    case 3:
                        MyConsolEntity = CreateInsulatorEntity(new Point2d(BasePoint.X + 30, BasePoint.Y));
                        Entities.Add(MyConsolEntity);
                        MyConsolEntity = CreateInsulatorEntity(new Point2d(BasePoint.X, BasePoint.Y));
                        Entities.Add(MyConsolEntity);
                        MyConsolEntity = CreateInsulatorEntity(new Point2d(BasePoint.X - 30, BasePoint.Y));
                        Entities.Add(MyConsolEntity);

                        break;
                    case 4:
                        MyConsolEntity = CreateInsulatorEntity(new Point2d(BasePoint.X + 30, BasePoint.Y));
                        Entities.Add(MyConsolEntity);
                        MyConsolEntity = CreateInsulatorEntity(new Point2d(BasePoint.X, BasePoint.Y - 12));
                        Entities.Add(MyConsolEntity);
                        MyConsolEntity = CreateInsulatorEntity(new Point2d(BasePoint.X, BasePoint.Y + 12));
                        Entities.Add(MyConsolEntity);
                        MyConsolEntity = CreateInsulatorEntity(new Point2d(BasePoint.X - 30, BasePoint.Y));
                        Entities.Add(MyConsolEntity);

                        break;
                }


                ////////////int InsulatorCountInList = (int)Math.Ceiling(Convert.ToDecimal(_InsulatorCount / 2));

                //////////////int ToleranceX = (int)Math.Ceiling(Convert.ToDecimal(100 / InsulatorCountInList));

                ////////////int ToleranceX = 30;


                ////////////Double StartX = BasePoint.X - 50;

                ////////////for (int i = 0; i < InsulatorCountInList; i++)
                ////////////{

                ////////////    Entity ConsolEntity = CreateInsulatorEntity(new Point2d(StartX + ((i + 1) * ToleranceX), 12));

                ////////////    //Atend.Global.Acad.AcadJigs.SaveExtensionData(ConsolEntity, (int)Atend.Control.Enum.ProductType.Consol);

                ////////////    Entities.Add(ConsolEntity);

                ////////////}

                ////////////InsulatorCountInList = _InsulatorCount - InsulatorCountInList;

                ////////////for (int i = 0; i < InsulatorCountInList; i++)
                ////////////{

                ////////////    Entity ConsolEntity = CreateInsulatorEntity(new Point2d(StartX + ((i + 1) * ToleranceX), -12));

                ////////////    //SaveExtensionData(ConsolEntity, (int)Atend.Control.Enum.ProductType.Consol);

                ////////////    Entities.Add(ConsolEntity);
                ////////////}

            }

            private Autodesk.AutoCAD.DatabaseServices.Entity CreatePoleEntity(Point2d BasePoint)
            {

                double BaseX = BasePoint.X;
                double BaseY = BasePoint.Y;

                Atend.Global.Acad.AcadJigs.MyPolyLine pLine = new Atend.Global.Acad.AcadJigs.MyPolyLine();
                pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX - 50, BaseY - 25), 0, 0, 0);
                pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX + 50, BaseY - 25), 0, 0, 0);
                pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX + 50, BaseY + 25), 0, 0, 0);
                pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX - 50, BaseY + 25), 0, 0, 0);
                pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX - 50, BaseY - 25), 0, 0, 0);
                pLine.Closed = true;

                //ed.WriteMessage("B1 \n");
                Atend.Global.Acad.AcadJigs.SaveExtensionData(pLine, (int)Atend.Control.Enum.ProductType.Pole);

                return pLine;

            }

            private Autodesk.AutoCAD.DatabaseServices.Entity CreateInsulatorEntity(Point2d BasePoint)
            {

                double BaseX = BasePoint.X;
                double BaseY = BasePoint.Y;

                Atend.Global.Acad.AcadJigs.MyPolyLine pLine = new Atend.Global.Acad.AcadJigs.MyPolyLine();
                pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX - 5, BaseY - 5), 0, 0, 0);
                pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX + 5, BaseY - 5), 0, 0, 0);
                pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX + 5, BaseY + 5), 0, 0, 0);
                pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX - 5, BaseY + 5), 0, 0, 0);
                pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX - 5, BaseY - 5), 0, 0, 0);
                pLine.Closed = true;

                Atend.Global.Acad.AcadJigs.SaveExtensionData(pLine, (int)Atend.Control.Enum.ProductType.Consol);

                return pLine;

            }

            protected override SamplerStatus Sampler(JigPrompts prompts)
            {
                //throw new System.Exception("The method or operation is not implemented.");


                if (GetPoint)
                {
                    JigPromptPointOptions jppo = new JigPromptPointOptions("Select Pole Position : \n");

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
                    JigPromptAngleOptions jpao = new JigPromptAngleOptions("Select Pole Angle : \n");

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

                            NewAngle = pdr.Value - NewAngle;

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

                        BasePoint.ToString(), // Text

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

                    Entities.Add(CreatePoleEntity(BasePoint));


                    Autodesk.AutoCAD.DatabaseServices.Entity MyConsolEntity = null;
                    switch (_InsulatorCount)
                    {
                        case 1:

                            MyConsolEntity = CreateInsulatorEntity(new Point2d(BasePoint.X, BasePoint.Y));
                            Entities.Add(MyConsolEntity);

                            break;
                        case 2:

                            MyConsolEntity = CreateInsulatorEntity(new Point2d(BasePoint.X + 30, BasePoint.Y));
                            Entities.Add(MyConsolEntity);
                            MyConsolEntity = CreateInsulatorEntity(new Point2d(BasePoint.X - 30, BasePoint.Y));
                            Entities.Add(MyConsolEntity);


                            break;
                        case 3:
                            MyConsolEntity = CreateInsulatorEntity(new Point2d(BasePoint.X + 30, BasePoint.Y));
                            Entities.Add(MyConsolEntity);
                            MyConsolEntity = CreateInsulatorEntity(new Point2d(BasePoint.X, BasePoint.Y));
                            Entities.Add(MyConsolEntity);
                            MyConsolEntity = CreateInsulatorEntity(new Point2d(BasePoint.X - 30, BasePoint.Y));
                            Entities.Add(MyConsolEntity);

                            break;
                        case 4:
                            MyConsolEntity = CreateInsulatorEntity(new Point2d(BasePoint.X + 30, BasePoint.Y));
                            Entities.Add(MyConsolEntity);
                            MyConsolEntity = CreateInsulatorEntity(new Point2d(BasePoint.X, BasePoint.Y - 12));
                            Entities.Add(MyConsolEntity);
                            MyConsolEntity = CreateInsulatorEntity(new Point2d(BasePoint.X, BasePoint.Y + 12));
                            Entities.Add(MyConsolEntity);
                            MyConsolEntity = CreateInsulatorEntity(new Point2d(BasePoint.X - 30, BasePoint.Y));
                            Entities.Add(MyConsolEntity);

                            break;
                    }

                    //~~~~~~~~ SCALE ~~~~~~~~~~

                    //////if (Atend.Control.Common.SelectedDesignScale != 0)
                    //////{
                    //////    double ScaleValue = 1 / Atend.Control.Common.SelectedDesignScale;
                    Matrix3d trans1 = Matrix3d.Scaling(MyScale, new Point3d(BasePoint.X, BasePoint.Y, 0));
                    foreach (Entity en in Entities)
                    {
                        en.TransformBy(trans1);
                    }
                    //////}

                    //~~~~~~~~~~~~~~~~~~~~~~~~~



                }
                else if (GetAngle)
                {
                    Matrix3d trans = Matrix3d.Rotation(NewAngle - BaseAngle, ed.CurrentUserCoordinateSystem.CoordinateSystem3d.Zaxis,

                                                       new Point3d(BasePoint.X, BasePoint.Y, 0));

                    foreach (Autodesk.AutoCAD.DatabaseServices.Entity en in Entities)
                    {

                        en.TransformBy(trans);

                    }

                    BaseAngle = NewAngle;
                    NewAngle = 0;

                }


                foreach (Autodesk.AutoCAD.DatabaseServices.Entity en in Entities)
                {

                    draw.Geometry.Draw(en);

                }


                return true;
            }

            public List<Autodesk.AutoCAD.DatabaseServices.Entity> GetEntities()
            {
                Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
                //ed.WriteMessage("~~~angle = {0} \n", NewAngle - BaseAngle);

                foreach (Autodesk.AutoCAD.DatabaseServices.Entity ent in Entities)
                {

                    Atend.Global.Acad.AcadJigs.MyPolyLine n = ent as Atend.Global.Acad.AcadJigs.MyPolyLine;

                    //ed.WriteMessage("additiona count : {0}  \n", n.AdditionalDictionary.Count);

                }

                return Entities;
            }

            public List<Autodesk.AutoCAD.DatabaseServices.Entity> GetDemo(Point3d CenterPoint, double RadianAngle)
            {
                Entities.Clear();
                Entities.Add(CreatePoleEntity(new Point2d(CenterPoint.X, CenterPoint.Y)));


                Autodesk.AutoCAD.DatabaseServices.Entity MyConsolEntity = null;
                switch (_InsulatorCount)
                {
                    case 1:

                        MyConsolEntity = CreateInsulatorEntity(new Point2d(CenterPoint.X, CenterPoint.Y));
                        Entities.Add(MyConsolEntity);

                        break;
                    case 2:

                        MyConsolEntity = CreateInsulatorEntity(new Point2d(CenterPoint.X + 30, CenterPoint.Y));
                        Entities.Add(MyConsolEntity);
                        MyConsolEntity = CreateInsulatorEntity(new Point2d(CenterPoint.X - 30, CenterPoint.Y));
                        Entities.Add(MyConsolEntity);


                        break;
                    case 3:
                        MyConsolEntity = CreateInsulatorEntity(new Point2d(CenterPoint.X + 30, CenterPoint.Y));
                        Entities.Add(MyConsolEntity);
                        MyConsolEntity = CreateInsulatorEntity(new Point2d(CenterPoint.X, CenterPoint.Y));
                        Entities.Add(MyConsolEntity);
                        MyConsolEntity = CreateInsulatorEntity(new Point2d(CenterPoint.X - 30, CenterPoint.Y));
                        Entities.Add(MyConsolEntity);

                        break;
                    case 4:
                        MyConsolEntity = CreateInsulatorEntity(new Point2d(CenterPoint.X + 30, CenterPoint.Y));
                        Entities.Add(MyConsolEntity);
                        MyConsolEntity = CreateInsulatorEntity(new Point2d(CenterPoint.X, CenterPoint.Y - 12));
                        Entities.Add(MyConsolEntity);
                        MyConsolEntity = CreateInsulatorEntity(new Point2d(CenterPoint.X, CenterPoint.Y + 12));
                        Entities.Add(MyConsolEntity);
                        MyConsolEntity = CreateInsulatorEntity(new Point2d(CenterPoint.X - 30, CenterPoint.Y));
                        Entities.Add(MyConsolEntity);

                        break;
                }


                Matrix3d trans1 = Matrix3d.Scaling(MyScale, new Point3d(CenterPoint.X, CenterPoint.Y, 0));
                foreach (Autodesk.AutoCAD.DatabaseServices.Entity en in Entities)
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



        class DrawPoleJig02 : DrawJig
        {

            private List<Autodesk.AutoCAD.DatabaseServices.Entity> Entities = new List<Autodesk.AutoCAD.DatabaseServices.Entity>();
            public Point2d BasePoint = Point2d.Origin;
            private double NewAngle, BaseAngle = 0;
            int _InsulatorCount;
            public bool GetPoint = true;
            public bool GetAngle = false;
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            private Autodesk.AutoCAD.GraphicsInterface.TextStyle _style;
            double MyScale = 1;

            public DrawPoleJig02(int ConsolCount, double Scale)
            {

                MyScale = Scale;
                // SET TEXT STYLE

                _style = new Autodesk.AutoCAD.GraphicsInterface.TextStyle();

                _style.Font = new Autodesk.AutoCAD.GraphicsInterface.FontDescriptor("Calibri", false, true, 0, 0);

                _style.TextSize = 10;

                // END OF SET TEXT STYLE

                //AddRegAppTableRecord(RegAppName);

                _InsulatorCount = ConsolCount;

                //Add Pole

                Autodesk.AutoCAD.DatabaseServices.Entity poleEntity = CreatePoleEntity(BasePoint);

                Entities.Add(poleEntity);

                //Add Consol

                Autodesk.AutoCAD.DatabaseServices.Entity MyConsolEntity = null;
                switch (_InsulatorCount)
                {
                    case 1:

                        MyConsolEntity = CreateInsulatorEntity(new Point2d(BasePoint.X, BasePoint.Y));
                        Entities.Add(MyConsolEntity);

                        break;
                    case 2:

                        MyConsolEntity = CreateInsulatorEntity(new Point2d(BasePoint.X + 30, BasePoint.Y));
                        Entities.Add(MyConsolEntity);
                        MyConsolEntity = CreateInsulatorEntity(new Point2d(BasePoint.X - 30, BasePoint.Y));
                        Entities.Add(MyConsolEntity);


                        break;
                    case 3:
                        MyConsolEntity = CreateInsulatorEntity(new Point2d(BasePoint.X + 30, BasePoint.Y));
                        Entities.Add(MyConsolEntity);
                        MyConsolEntity = CreateInsulatorEntity(new Point2d(BasePoint.X, BasePoint.Y));
                        Entities.Add(MyConsolEntity);
                        MyConsolEntity = CreateInsulatorEntity(new Point2d(BasePoint.X - 30, BasePoint.Y));
                        Entities.Add(MyConsolEntity);

                        break;
                    case 4:
                        MyConsolEntity = CreateInsulatorEntity(new Point2d(BasePoint.X + 30, BasePoint.Y));
                        Entities.Add(MyConsolEntity);
                        MyConsolEntity = CreateInsulatorEntity(new Point2d(BasePoint.X, BasePoint.Y - 12));
                        Entities.Add(MyConsolEntity);
                        MyConsolEntity = CreateInsulatorEntity(new Point2d(BasePoint.X, BasePoint.Y + 12));
                        Entities.Add(MyConsolEntity);
                        MyConsolEntity = CreateInsulatorEntity(new Point2d(BasePoint.X - 30, BasePoint.Y));
                        Entities.Add(MyConsolEntity);

                        break;
                }


                ////////////int InsulatorCountInList = (int)Math.Ceiling(Convert.ToDecimal(_InsulatorCount / 2));

                //////////////int ToleranceX = (int)Math.Ceiling(Convert.ToDecimal(100 / InsulatorCountInList));

                ////////////int ToleranceX = 30;


                ////////////Double StartX = BasePoint.X - 50;

                ////////////for (int i = 0; i < InsulatorCountInList; i++)
                ////////////{

                ////////////    Entity ConsolEntity = CreateInsulatorEntity(new Point2d(StartX + ((i + 1) * ToleranceX), 12));

                ////////////    //Atend.Global.Acad.AcadJigs.SaveExtensionData(ConsolEntity, (int)Atend.Control.Enum.ProductType.Consol);

                ////////////    Entities.Add(ConsolEntity);

                ////////////}

                ////////////InsulatorCountInList = _InsulatorCount - InsulatorCountInList;

                ////////////for (int i = 0; i < InsulatorCountInList; i++)
                ////////////{

                ////////////    Entity ConsolEntity = CreateInsulatorEntity(new Point2d(StartX + ((i + 1) * ToleranceX), -12));

                ////////////    //SaveExtensionData(ConsolEntity, (int)Atend.Control.Enum.ProductType.Consol);

                ////////////    Entities.Add(ConsolEntity);
                ////////////}

            }

            private Autodesk.AutoCAD.DatabaseServices.Entity CreatePoleEntity(Point2d BasePoint)
            {

                double BaseX = BasePoint.X;
                double BaseY = BasePoint.Y;

                Atend.Global.Acad.AcadJigs.MyPolyLine pLine = new Atend.Global.Acad.AcadJigs.MyPolyLine();
                pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX - 50, BaseY - 25), 0, 0, 0);
                pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX + 50, BaseY - 25), 0, 0, 0);
                pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX + 50, BaseY + 25), 0, 0, 0);
                pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX - 50, BaseY + 25), 0, 0, 0);
                pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX - 50, BaseY - 25), 0, 0, 0);
                pLine.Closed = true;

                //ed.WriteMessage("B1 \n");
                Atend.Global.Acad.AcadJigs.SaveExtensionData(pLine, (int)Atend.Control.Enum.ProductType.Pole);

                return pLine;

            }

            private Autodesk.AutoCAD.DatabaseServices.Entity CreateInsulatorEntity(Point2d BasePoint)
            {

                double BaseX = BasePoint.X;
                double BaseY = BasePoint.Y;

                Atend.Global.Acad.AcadJigs.MyPolyLine pLine = new Atend.Global.Acad.AcadJigs.MyPolyLine();
                pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX - 5, BaseY - 5), 0, 0, 0);
                pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX + 5, BaseY - 5), 0, 0, 0);
                pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX + 5, BaseY + 5), 0, 0, 0);
                pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX - 5, BaseY + 5), 0, 0, 0);
                pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX - 5, BaseY - 5), 0, 0, 0);
                pLine.Closed = true;

                Atend.Global.Acad.AcadJigs.SaveExtensionData(pLine, (int)Atend.Control.Enum.ProductType.Consol);

                return pLine;

            }

            protected override SamplerStatus Sampler(JigPrompts prompts)
            {
                if (GetPoint)
                {
                    JigPromptPointOptions jppo = new JigPromptPointOptions("Select Pole Position : \n");

                    jppo.UserInputControls = (UserInputControls.Accept3dCoordinates |
                                             UserInputControls.NullResponseAccepted |
                                             UserInputControls.NoNegativeResponseAccepted);

                    PromptPointResult ppr = prompts.AcquirePoint(jppo);

                    if (ppr.Status == PromptStatus.OK)
                    {
                        if (!Atend.Global.Acad.DrawEquips.AcDrawForbidenArea.PointWasInForbidenArea(ppr.Value))
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
                    }
                    else
                    {
                        //v2d = new Vector2d(0, 0);

                        return SamplerStatus.Cancel;
                    }

                }
                else if (GetAngle)
                {
                    JigPromptAngleOptions jpao = new JigPromptAngleOptions("Select Pole Angle : \n");

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

                            NewAngle = pdr.Value - NewAngle;

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

                        BasePoint.ToString(), // Text

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

                    Entities.Add(CreatePoleEntity(BasePoint));


                    Autodesk.AutoCAD.DatabaseServices.Entity MyConsolEntity = null;
                    switch (_InsulatorCount)
                    {
                        case 1:

                            MyConsolEntity = CreateInsulatorEntity(new Point2d(BasePoint.X, BasePoint.Y));
                            Entities.Add(MyConsolEntity);

                            break;
                        case 2:

                            MyConsolEntity = CreateInsulatorEntity(new Point2d(BasePoint.X + 30, BasePoint.Y));
                            Entities.Add(MyConsolEntity);
                            MyConsolEntity = CreateInsulatorEntity(new Point2d(BasePoint.X - 30, BasePoint.Y));
                            Entities.Add(MyConsolEntity);


                            break;
                        case 3:
                            MyConsolEntity = CreateInsulatorEntity(new Point2d(BasePoint.X + 30, BasePoint.Y));
                            Entities.Add(MyConsolEntity);
                            MyConsolEntity = CreateInsulatorEntity(new Point2d(BasePoint.X, BasePoint.Y));
                            Entities.Add(MyConsolEntity);
                            MyConsolEntity = CreateInsulatorEntity(new Point2d(BasePoint.X - 30, BasePoint.Y));
                            Entities.Add(MyConsolEntity);

                            break;
                        case 4:
                            MyConsolEntity = CreateInsulatorEntity(new Point2d(BasePoint.X + 30, BasePoint.Y));
                            Entities.Add(MyConsolEntity);
                            MyConsolEntity = CreateInsulatorEntity(new Point2d(BasePoint.X, BasePoint.Y - 12));
                            Entities.Add(MyConsolEntity);
                            MyConsolEntity = CreateInsulatorEntity(new Point2d(BasePoint.X, BasePoint.Y + 12));
                            Entities.Add(MyConsolEntity);
                            MyConsolEntity = CreateInsulatorEntity(new Point2d(BasePoint.X - 30, BasePoint.Y));
                            Entities.Add(MyConsolEntity);

                            break;
                    }

                    //~~~~~~~~ SCALE ~~~~~~~~~~

                    //////if (Atend.Control.Common.SelectedDesignScale != 0)
                    //////{
                    //////    double ScaleValue = 1 / Atend.Control.Common.SelectedDesignScale;
                    Matrix3d trans1 = Matrix3d.Scaling(MyScale, new Point3d(BasePoint.X, BasePoint.Y, 0));
                    foreach (Autodesk.AutoCAD.DatabaseServices.Entity en in Entities)
                    {
                        en.TransformBy(trans1);
                    }
                    //////}

                    //~~~~~~~~~~~~~~~~~~~~~~~~~



                }
                else if (GetAngle)
                {
                    //Matrix3d trans = Matrix3d.Rotation(NewAngle - BaseAngle, ed.CurrentUserCoordinateSystem.CoordinateSystem3d.Zaxis,

                    //                                   new Point3d(BasePoint.X, BasePoint.Y, 0));

                    //foreach (Entity en in Entities)
                    //{

                    //    en.TransformBy(trans);

                    //}

                    //BaseAngle = NewAngle;
                    //NewAngle = 0;

                    //%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
                    Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
                    //if (GetAngle)
                    //{
                    Matrix3d trans = Matrix3d.Rotation(NewAngle, ed.CurrentUserCoordinateSystem.CoordinateSystem3d.Zaxis, new Point3d(BasePoint.X, BasePoint.Y, 0));
                    foreach (Autodesk.AutoCAD.DatabaseServices.Entity en in Entities)
                    {
                        en.TransformBy(trans);
                    }
                    //}
                }


                foreach (Autodesk.AutoCAD.DatabaseServices.Entity en in Entities)
                {

                    draw.Geometry.Draw(en);

                }


                return true;
            }

            public List<Autodesk.AutoCAD.DatabaseServices.Entity> GetEntities()
            {
                Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
                //ed.WriteMessage("~~~angle = {0} \n", NewAngle - BaseAngle);

                foreach (Autodesk.AutoCAD.DatabaseServices.Entity ent in Entities)
                {

                    Atend.Global.Acad.AcadJigs.MyPolyLine n = ent as Atend.Global.Acad.AcadJigs.MyPolyLine;

                    //ed.WriteMessage("additiona count : {0}  \n", n.AdditionalDictionary.Count);

                }

                return Entities;
            }

            public List<Autodesk.AutoCAD.DatabaseServices.Entity> GetDemo(Point3d CenterPoint, double RadianAngle)
            {
                Entities.Clear();
                Entities.Add(CreatePoleEntity(new Point2d(CenterPoint.X, CenterPoint.Y)));


                Autodesk.AutoCAD.DatabaseServices.Entity MyConsolEntity = null;
                switch (_InsulatorCount)
                {
                    case 1:

                        MyConsolEntity = CreateInsulatorEntity(new Point2d(CenterPoint.X, CenterPoint.Y));
                        Entities.Add(MyConsolEntity);

                        break;
                    case 2:

                        MyConsolEntity = CreateInsulatorEntity(new Point2d(CenterPoint.X + 30, CenterPoint.Y));
                        Entities.Add(MyConsolEntity);
                        MyConsolEntity = CreateInsulatorEntity(new Point2d(CenterPoint.X - 30, CenterPoint.Y));
                        Entities.Add(MyConsolEntity);


                        break;
                    case 3:
                        MyConsolEntity = CreateInsulatorEntity(new Point2d(CenterPoint.X + 30, CenterPoint.Y));
                        Entities.Add(MyConsolEntity);
                        MyConsolEntity = CreateInsulatorEntity(new Point2d(CenterPoint.X, CenterPoint.Y));
                        Entities.Add(MyConsolEntity);
                        MyConsolEntity = CreateInsulatorEntity(new Point2d(CenterPoint.X - 30, CenterPoint.Y));
                        Entities.Add(MyConsolEntity);

                        break;
                    case 4:
                        MyConsolEntity = CreateInsulatorEntity(new Point2d(CenterPoint.X + 30, CenterPoint.Y));
                        Entities.Add(MyConsolEntity);
                        MyConsolEntity = CreateInsulatorEntity(new Point2d(CenterPoint.X, CenterPoint.Y - 12));
                        Entities.Add(MyConsolEntity);
                        MyConsolEntity = CreateInsulatorEntity(new Point2d(CenterPoint.X, CenterPoint.Y + 12));
                        Entities.Add(MyConsolEntity);
                        MyConsolEntity = CreateInsulatorEntity(new Point2d(CenterPoint.X - 30, CenterPoint.Y));
                        Entities.Add(MyConsolEntity);

                        break;
                }


                Matrix3d trans1 = Matrix3d.Scaling(MyScale, new Point3d(CenterPoint.X, CenterPoint.Y, 0));
                foreach (Autodesk.AutoCAD.DatabaseServices.Entity en in Entities)
                {
                    en.TransformBy(trans1);
                }

                Matrix3d trans = Matrix3d.Rotation(RadianAngle, ed.CurrentUserCoordinateSystem.CoordinateSystem3d.Zaxis, new Point3d(CenterPoint.X, CenterPoint.Y, 0));
                foreach (Autodesk.AutoCAD.DatabaseServices.Entity en in Entities)
                {
                    en.TransformBy(trans);
                }

                return Entities;
            }

        }



        public AcDrawPole()
        {
            eConsolUseAccess = new ArrayList();
            eConsolExistance = new ArrayList();
            eConsolCount = new ArrayList();
            eConsolProjectCode = new ArrayList();
            eConsols = new List<Atend.Base.Equipment.EConsol>();
        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//

        private void ResetClass()
        {
            dNode = new Atend.Base.Design.DNode();
            dPackages = new List<Atend.Base.Design.DPackage>();
            PolePackage = new Atend.Base.Design.DPackage();
            HalterPackage = new Atend.Base.Design.DPackage();

        }

        //public bool DrawPole(Point3d CenterPoint, double Angle)
        //{
        //    Document doc = Application.DocumentManager.MdiActiveDocument;
        //    Editor ed = doc.Editor;
        //    ResetClass();
        //    //ed.WriteMessage("Start DrawPole : {0} \n", Angle);
        //    ObjectId NewPoleObjectId = ObjectId.Null;
        //    ObjectIdCollection NewConsolObjectIds = new ObjectIdCollection();

        //    double MyScale = Atend.Base.Design.DProductProperties.AccessSelectBySoftwareCode((int)Atend.Control.Enum.ProductType.Pole).Scale;
        //    double MyCommentScale = Atend.Base.Design.DProductProperties.AccessSelectBySoftwareCode((int)Atend.Control.Enum.ProductType.Pole).CommentScale;
        //    double MyCommentScaleConsol = Atend.Base.Design.DProductProperties.AccessSelectBySoftwareCode((int)Atend.Control.Enum.ProductType.Consol).CommentScale;

        //    using (DocumentLock docLock = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.LockDocument())
        //    {
        //        int i = 0;
        //        DrawPoleJig drawPoleJig = new DrawPoleJig(eConsols.Count, MyScale);
        //        List<Entity> entities = drawPoleJig.GetDemo(CenterPoint, (Math.PI * Angle) / 180);

        //        #region Save data here

        //        if (SavePoleData())
        //        {
        //            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //            foreach (Entity ent in entities)
        //            {
        //                object productType = null;
        //                Entity newEntity = ent;
        //                Atend.Global.Acad.AcadJigs.MyPolyLine myPoly = ent as Atend.Global.Acad.AcadJigs.MyPolyLine;
        //                if (myPoly.AdditionalDictionary.ContainsKey("ProductType"))
        //                {
        //                    myPoly.AdditionalDictionary.TryGetValue("ProductType", out productType);
        //                }
        //                else
        //                {
        //                    return false;
        //                }
        //                //ed.WriteMessage("ProductType= " + productType.ToString() + "\n");

        //                if (Convert.ToInt32(productType) == (int)Atend.Control.Enum.ProductType.Pole)
        //                {
        //                    NewPoleObjectId = Atend.Global.Acad.UAcad.DrawEntityOnScreen(ent, Atend.Control.Enum.AutoCadLayerName.GENERAL.ToString());

        //                    Atend.Base.Acad.AT_INFO at_info = new Atend.Base.Acad.AT_INFO();
        //                    at_info.ParentCode = "";
        //                    at_info.NodeCode = dNode.Code.ToString();
        //                    at_info.NodeType = Convert.ToInt32(productType);
        //                    at_info.ProductCode = dNode.ProductCode;
        //                    at_info.SelectedObjectId = ent.ObjectId;
        //                    // calculate pole angle according to ground

        //                    Polyline AngleLine = ent as Polyline;
        //                    if (AngleLine != null)
        //                    {
        //                        Line myLine = new Line(AngleLine.GetPoint3dAt(0), AngleLine.GetPoint3dAt(1));
        //                        //ed.WriteMessage("~~~ angle :{0}\n", (180 * myLine.Angle) / Math.PI);
        //                        at_info.Angle = (180 * myLine.Angle) / Math.PI;
        //                    }
        //                    else
        //                    {
        //                        at_info.Angle = 0;
        //                    }

        //                    // end of calculate pole angle according to ground
        //                    at_info.Insert();



        //                    ObjectId TextOi = Atend.Global.Acad.UAcad.DrawEntityOnScreen(Atend.Global.Acad.UAcad.WriteNote(PolePackage.Number, Atend.Global.Acad.UAcad.CenterOfEntity(Atend.Global.Acad.UAcad.GetEntityByObjectID(ent.ObjectId)), MyCommentScale), Atend.Control.Enum.AutoCadLayerName.GENERAL.ToString());

        //                    Atend.Base.Acad.AT_INFO textInfo = new Atend.Base.Acad.AT_INFO(TextOi);
        //                    textInfo.ParentCode = at_info.NodeCode;
        //                    textInfo.NodeType = (int)Atend.Control.Enum.ProductType.Comment;
        //                    textInfo.NodeCode = "";
        //                    textInfo.ProductCode = 0;
        //                    textInfo.Insert();

        //                    Atend.Base.Acad.AT_SUB textSub = new Atend.Base.Acad.AT_SUB(TextOi);
        //                    textSub.SubIdCollection.Add(NewPoleObjectId);
        //                    textSub.Insert();

        //                    Atend.Base.Acad.AT_SUB polesub = new Atend.Base.Acad.AT_SUB(NewPoleObjectId);
        //                    polesub.SubIdCollection.Add(TextOi);
        //                    polesub.Insert();
        //                }
        //                else if (Convert.ToInt32(productType) == (int)Atend.Control.Enum.ProductType.Consol)
        //                {
        //                    // add extention data
        //                    //ed.WriteMessage("The Entity Is Consol\n");
        //                    //Atend.Base.Design.DPackage package = dPackages[i];
        //                    bool IsWeek = false;
        //                    Atend.Base.Equipment.EConsol eConsol = eConsols[i];  //Atend.Base.Equipment.EConsol.SelectByCodeForDesign(package.ProductCode);


        //                    switch (eConsol.VoltageLevel)
        //                    {
        //                        case 20000:
        //                            IsWeek = false;
        //                            break;
        //                        case 11000:
        //                            IsWeek = false;
        //                            break;
        //                        case 33000:
        //                            IsWeek = false;
        //                            break;
        //                        case 400:
        //                            IsWeek = true;
        //                            break;
        //                    }
        //                    string LayerName;
        //                    if (IsWeek)
        //                    {
        //                        LayerName = Atend.Control.Enum.AutoCadLayerName.LOW_AIR.ToString();
        //                    }
        //                    else
        //                    {
        //                        LayerName = Atend.Control.Enum.AutoCadLayerName.MED_AIR.ToString();
        //                    }


        //                    Atend.Base.Design.DConsol consol = new Atend.Base.Design.DConsol();
        //                    //ed.WriteMessage("ConsolCount= " + dConsolCode.Count.ToString() + "\n");
        //                    consol.Code = dPackages[i].Code;
        //                    consol.LoadCode = 0;
        //                    consol.ProductCode = eConsol.Code;
        //                    consol.ParentCode = dNode.Code;

        //                    if (consol.AccessInsert())
        //                    {
        //                        ObjectId NewConsolObjectID = Atend.Global.Acad.UAcad.DrawEntityOnScreen(ent, LayerName);
        //                        NewConsolObjectIds.Add(NewConsolObjectID);

        //                        Atend.Base.Acad.AT_INFO at_info = new Atend.Base.Acad.AT_INFO();
        //                        at_info.ParentCode = dNode.Code.ToString();
        //                        at_info.NodeCode = dPackages[i].Code.ToString();
        //                        at_info.NodeType = Convert.ToInt32(productType);
        //                        at_info.ProductCode = dPackages[i].ProductCode;
        //                        at_info.SelectedObjectId = ent.ObjectId;
        //                        at_info.Insert();


        //                        ObjectId TextOi = Atend.Global.Acad.UAcad.DrawEntityOnScreen(Atend.Global.Acad.UAcad.WriteNote(dPackages[i].Number, ((Polyline)Atend.Global.Acad.UAcad.GetEntityByObjectID(ent.ObjectId)).GetPoint3dAt(0), MyCommentScaleConsol), Atend.Control.Enum.AutoCadLayerName.GENERAL.ToString());

        //                        Atend.Base.Acad.AT_INFO textInfo = new Atend.Base.Acad.AT_INFO(TextOi);
        //                        textInfo.ParentCode = at_info.NodeCode;
        //                        textInfo.NodeType = (int)Atend.Control.Enum.ProductType.Comment;
        //                        textInfo.NodeCode = "";
        //                        textInfo.ProductCode = 0;
        //                        textInfo.Insert();


        //                        Atend.Base.Acad.AT_SUB at_sub = new Atend.Base.Acad.AT_SUB();
        //                        at_sub.SelectedObjectId = NewConsolObjectID;
        //                        at_sub.SubIdCollection.Add(NewPoleObjectId);
        //                        at_sub.SubIdCollection.Add(TextOi);
        //                        at_sub.Insert();


        //                        Atend.Base.Acad.AT_SUB textSub = new Atend.Base.Acad.AT_SUB(TextOi);
        //                        textSub.SubIdCollection.Add(NewConsolObjectID);
        //                        textSub.Insert();

        //                    }

        //                    i++;
        //                    // ed.WriteMessage("Extension was done \n");

        //                }//End of DRaw consol

        //            }// Draw Finished

        //            //insert consols as a sub for pole
        //            foreach (ObjectId obji in NewConsolObjectIds)
        //            {
        //                Atend.Base.Acad.AT_SUB.AddToAT_SUB(obji, NewPoleObjectId);
        //            }

        //            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //        #endregion

        //    }
        //    return true;
        //}

        public bool DrawPole(Point3d CenterPoint, double Angle, out Autodesk.AutoCAD.DatabaseServices.ObjectIdCollection NewDrawnNodes)
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            NewDrawnNodes = new Autodesk.AutoCAD.DatabaseServices.ObjectIdCollection();
            ResetClass();
            //ed.WriteMessage("Start DrawPole : {0} \n", Angle);
            Autodesk.AutoCAD.DatabaseServices.ObjectId NewPoleObjectId = Autodesk.AutoCAD.DatabaseServices.ObjectId.Null;
            Autodesk.AutoCAD.DatabaseServices.ObjectIdCollection NewConsolObjectIds = new Autodesk.AutoCAD.DatabaseServices.ObjectIdCollection();

            double MyScale = Atend.Base.Design.DProductProperties.AccessSelectBySoftwareCode((int)Atend.Control.Enum.ProductType.Pole).Scale;
            double MyCommentScale = Atend.Base.Design.DProductProperties.AccessSelectBySoftwareCode((int)Atend.Control.Enum.ProductType.Pole).CommentScale;
            double MyCommentScaleConsol = Atend.Base.Design.DProductProperties.AccessSelectBySoftwareCode((int)Atend.Control.Enum.ProductType.Consol).CommentScale;

            using (DocumentLock docLock = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.LockDocument())
            {
                int i = 0;
                DrawPoleJig drawPoleJig = new DrawPoleJig(eConsols.Count, MyScale);
                List<Autodesk.AutoCAD.DatabaseServices.Entity> entities = drawPoleJig.GetDemo(CenterPoint, (Math.PI * Angle) / 180);

                #region Save data here

                if (SavePoleData())
                {
                    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
                    foreach (Autodesk.AutoCAD.DatabaseServices.Entity ent in entities)
                    {
                        object productType = null;
                        Autodesk.AutoCAD.DatabaseServices.Entity newEntity = ent;
                        Atend.Global.Acad.AcadJigs.MyPolyLine myPoly = ent as Atend.Global.Acad.AcadJigs.MyPolyLine;
                        if (myPoly.AdditionalDictionary.ContainsKey("ProductType"))
                        {
                            myPoly.AdditionalDictionary.TryGetValue("ProductType", out productType);
                        }
                        else
                        {
                            return false;
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
                            // calculate pole angle according to ground

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

                            // end of calculate pole angle according to ground
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

                                Atend.Base.Acad.AT_SUB textSub = new Atend.Base.Acad.AT_SUB(TextOi);
                                textSub.SubIdCollection.Add(NewPoleObjectId);
                                textSub.Insert();

                                Atend.Base.Acad.AT_SUB polesub = new Atend.Base.Acad.AT_SUB(NewPoleObjectId);
                                polesub.SubIdCollection.Add(TextOi);
                                polesub.Insert();
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
                    foreach (Autodesk.AutoCAD.DatabaseServices.ObjectId obji in NewConsolObjectIds)
                    {
                        Atend.Base.Acad.AT_SUB.AddToAT_SUB(obji, NewPoleObjectId);
                    }

                    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
                    if (eConsols.Count != 0)
                    {
                        NewDrawnNodes = NewConsolObjectIds;
                    }
                    else
                    {
                        NewDrawnNodes.Add(NewPoleObjectId);
                    }
                }
                else
                {
                    return false;
                }
                #endregion

            }
            return true;
        }

        public void DrawPole()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            ResetClass();
            //Database db = HostApplicationServices.WorkingDatabase;
            Database db = Application.DocumentManager.MdiActiveDocument.Database;
            //Point3d TablePosition;
            ObjectId NewPoleObjectId = ObjectId.Null;
            ObjectIdCollection NewConsolObjectIds = new ObjectIdCollection();

            double MyScale = Atend.Base.Design.DProductProperties.AccessSelectBySoftwareCode((int)Atend.Control.Enum.ProductType.Pole).Scale;
            double MyCommentScale = Atend.Base.Design.DProductProperties.AccessSelectBySoftwareCode((int)Atend.Control.Enum.ProductType.Pole).CommentScale;
            double MyCommentScaleConsol = Atend.Base.Design.DProductProperties.AccessSelectBySoftwareCode((int)Atend.Control.Enum.ProductType.Consol).CommentScale;

            using (DocumentLock docLock = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.LockDocument())
            {
                bool conti = true;
                int i = 0;
                DrawPoleJig drawPoleJig = new DrawPoleJig(eConsols.Count, MyScale);
                while (conti)
                {
                    PromptResult pr;
                    pr = ed.Drag(drawPoleJig);
                    if (pr.Status == PromptStatus.OK && !drawPoleJig.GetAngle)
                    {
                        //ed.WriteMessage("1\n");
                        if (!Atend.Global.Acad.DrawEquips.AcDrawForbidenArea.PointWasInForbidenArea(new Point3d(drawPoleJig.PoleCenterPoint.X, drawPoleJig.PoleCenterPoint.Y, 0)))
                        {
                            //ed.WriteMessage("3\n");
                            drawPoleJig.GetPoint = false;
                            drawPoleJig.GetAngle = true;
                        }
                        //ed.WriteMessage("2\n");
                    }
                    else if (pr.Status == PromptStatus.OK && drawPoleJig.GetAngle)
                    {
                        conti = false;
                        List<Autodesk.AutoCAD.DatabaseServices.Entity> entities = drawPoleJig.GetEntities();
                        #region Save data here
                        if (SavePoleData())
                        {
                            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
                            foreach (Autodesk.AutoCAD.DatabaseServices.Entity ent in entities)
                            {
                                object productType = null;
                                Autodesk.AutoCAD.DatabaseServices.Entity newEntity = ent;
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
                                    // calculate pole angle according to ground

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

                                    // end of calculate pole angle according to ground
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

                                        Atend.Base.Acad.AT_SUB textSub = new Atend.Base.Acad.AT_SUB(TextOi);
                                        textSub.SubIdCollection.Add(NewPoleObjectId);
                                        textSub.Insert();

                                        Atend.Base.Acad.AT_SUB polesub = new Atend.Base.Acad.AT_SUB(NewPoleObjectId);
                                        polesub.SubIdCollection.Add(TextOi);
                                        polesub.Insert();
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
                            foreach (Autodesk.AutoCAD.DatabaseServices.ObjectId obji in NewConsolObjectIds)
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
        }///current

        public void DrawPole02()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            ResetClass();
            Database db = Application.DocumentManager.MdiActiveDocument.Database;
            ObjectId NewPoleObjectId = ObjectId.Null;
            ObjectIdCollection NewConsolObjectIds = new ObjectIdCollection();

            double MyScale = Atend.Base.Design.DProductProperties.AccessSelectBySoftwareCode((int)Atend.Control.Enum.ProductType.Pole).Scale;
            double MyCommentScale = Atend.Base.Design.DProductProperties.AccessSelectBySoftwareCode((int)Atend.Control.Enum.ProductType.Pole).CommentScale;
            double MyCommentScaleConsol = Atend.Base.Design.DProductProperties.AccessSelectBySoftwareCode((int)Atend.Control.Enum.ProductType.Consol).CommentScale;

            using (DocumentLock docLock = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.LockDocument())
            {
                bool conti = true;
                int i = 0;
                DrawPoleJig02 drawPoleJig = new DrawPoleJig02(eConsols.Count, MyScale);
                while (conti)
                {
                    PromptResult pr;
                    pr = ed.Drag(drawPoleJig);
                    if (pr.Status == PromptStatus.OK && !drawPoleJig.GetAngle)
                    {
                        drawPoleJig.GetPoint = false;
                        drawPoleJig.GetAngle = true;
                    }
                    else if (pr.Status == PromptStatus.OK && drawPoleJig.GetAngle)
                    {
                        conti = false;
                        List<Autodesk.AutoCAD.DatabaseServices.Entity> entities = drawPoleJig.GetEntities();
                        #region Save data here
                        if (SavePoleData())
                        {
                            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
                            foreach (Autodesk.AutoCAD.DatabaseServices.Entity ent in entities)
                            {
                                object productType = null;
                                Autodesk.AutoCAD.DatabaseServices.Entity newEntity = ent;
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
                                    // calculate pole angle according to ground

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

                                    // end of calculate pole angle according to ground
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

                                        Atend.Base.Acad.AT_SUB textSub = new Atend.Base.Acad.AT_SUB(TextOi);
                                        textSub.SubIdCollection.Add(NewPoleObjectId);
                                        textSub.Insert();

                                        Atend.Base.Acad.AT_SUB polesub = new Atend.Base.Acad.AT_SUB(NewPoleObjectId);
                                        polesub.SubIdCollection.Add(TextOi);
                                        polesub.Insert();
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
                            foreach (Autodesk.AutoCAD.DatabaseServices.ObjectId obji in NewConsolObjectIds)
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

        public static void RotatePole(double NewAngleDegree, Guid PoleCode)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            //ed.WriteMessage("Angle:{0} , PoleCode:{1} \n", NewAngleDegree, PoleCode);
            ObjectId PoleOI = Atend.Global.Acad.UAcad.GetPoleByGuid(PoleCode);
            if (PoleOI != ObjectId.Null)
            {
                //ed.WriteMessage("pole oi found \n");
                Autodesk.AutoCAD.DatabaseServices.Entity PoleEntity = Atend.Global.Acad.UAcad.GetEntityByObjectID(PoleOI);
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
                            Autodesk.AutoCAD.DatabaseServices.Entity ent = (Autodesk.AutoCAD.DatabaseServices.Entity)tr.GetObject(PoleOI, OpenMode.ForWrite);
                            Polyline LineEntity = ent as Polyline;
                            if (LineEntity != null)
                            {
                                //ed.WriteMessage("LineEntity entity found TETA:{0} \n", PoleInfo.Angle);
                                Matrix3d trans = Matrix3d.Rotation(((PoleInfo.Angle * Math.PI) / 180) * -1, ed.CurrentUserCoordinateSystem.CoordinateSystem3d.Zaxis, CenterPoint);
                                LineEntity.TransformBy(trans);

                                trans = Matrix3d.Rotation(((NewAngleDegree * Math.PI) / 180), ed.CurrentUserCoordinateSystem.CoordinateSystem3d.Zaxis, CenterPoint);
                                LineEntity.TransformBy(trans);

                            }
                            tr.Commit();
                            PoleInfo.Angle = NewAngleDegree;
                            PoleInfo.Insert();

                            Atend.Base.Acad.AT_SUB PoleSubs = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(PoleOI);
                            foreach (Autodesk.AutoCAD.DatabaseServices.ObjectId oi in PoleSubs.SubIdCollection)
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
                                    case Atend.Control.Enum.ProductType.KablSho:
                                        AcDrawKablsho.RotateKablsho(LastAngleDegree, NewAngleDegree, PoleOI, oi, CenterPoint);
                                        break;
                                    //case Atend.Control.Enum.ProductType.Comment:
                                    //    UAcad.RotateComment(LastAngleDegree, NewAngleDegree, PoleOI, oi, CenterPoint);
                                    //    break;
                                    case Atend.Control.Enum.ProductType.HeaderCabel:
                                        AcDrawHeaderCabel.RotateHeaderCable(LastAngleDegree, NewAngleDegree, PoleOI, oi, CenterPoint);
                                        break;
                                    case Atend.Control.Enum.ProductType.BankKhazan:
                                        AcDrawKhazan.RotateKhazan(LastAngleDegree, NewAngleDegree, PoleOI, oi, CenterPoint);
                                        break;
                                    case Atend.Control.Enum.ProductType.Rod:
                                        AcDrawRod.RotateRod(LastAngleDegree, NewAngleDegree, PoleOI, oi, CenterPoint);
                                        break;
                                    case Atend.Control.Enum.ProductType.Light:
                                        AcDrawLight.RotateLight(LastAngleDegree, NewAngleDegree, PoleOI, oi, CenterPoint);
                                        break;
                                    case Atend.Control.Enum.ProductType.Ground:
                                        AcDrawGround.RotateGround(LastAngleDegree, NewAngleDegree, PoleOI, oi, CenterPoint);
                                        break;
                                    
                                    
                                }
                            }

                        }//transaction
                    }

                }

                //Point3d Lastcp = Atend.Global.Acad.UAcad.CenterOfEntity(Atend.Global.Acad.UAcad.GetEntityByObjectID(PoleOI));
                //Point3d newcp = new Point3d(1, 0, 0);
                //Matrix3d mat = Matrix3d.Displacement(newcp - Lastcp);
                              
                //Atend.Global.Acad.AcadMove.EntityInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(PoleOI);
                //Atend.Global.Acad.AcadMove.EntitySub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(PoleOI);

                //Atend.Global.Acad.AcadMove.PoleOI = PoleOI;
                //Atend.Global.Acad.AcadMove.CurrentEntity = Atend.Global.Acad.UAcad.GetEntityByObjectID(PoleOI);
                //Atend.Global.Acad.AcadMove.LastCenterPoint =
                //    Atend.Global.Acad.UAcad.CenterOfEntity(Atend.Global.Acad.UAcad.GetEntityByObjectID(PoleOI));
                //Atend.Global.Acad.AcadMove.swBreaker = true;
                //Atend.Global.Acad.AcadMove.swDisconnector = true;
                //Atend.Global.Acad.AcadMove.swCatOut = true;

                //Atend.Global.Acad.AcadMove.MovePole(PoleOI, mat);

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
                    int i = 0;
                    ArrayList l1 = new ArrayList();
                    foreach (Atend.Base.Equipment.EConsol SelectedConsol in eConsols)
                    {
                        if (!Convert.ToBoolean(eConsolUseAccess[i]))
                        {
                            if (!SelectedConsol.AccessInsert(aTransaction, aConnection, true, true))
                            {
                                throw new System.Exception("SelectedConsol.AccessInsert failed");
                            }

                        }
                        i++;
                    }

                    if (eHalterCount != 0)
                    {
                        if (!eHalter.AccessInsert(aTransaction, aConnection, true, true))
                        {
                            throw new System.Exception("Halter.AccessInsert failed");
                        }
                    }

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
                    //ed.WriteMessage("d pole info finished\n");

                    PolePackage = new Atend.Base.Design.DPackage();
                    PolePackage.Count = 1;
                    PolePackage.IsExistance = Existance;
                    PolePackage.NodeCode = dNode.Code;
                    PolePackage.ProjectCode = ProjectCode;
                    PolePackage.ProductCode = ePole.Code;
                    PolePackage.Type = (int)Atend.Control.Enum.ProductType.Pole;
                    Atend.Control.Common.Counters.PoleCounter++;
                    PolePackage.Number = string.Format("P{2}-{0:00}{1:00}", ePole.Height, Math.Round(ePole.Power / 100, 0), Atend.Control.Common.Counters.PoleCounter);
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
                    //ed.WriteMessage(" d pole package finished\n");


                    if (eHalterCount != 0)
                    {
                        HalterPackage = new Atend.Base.Design.DPackage();
                        HalterPackage.Count = eHalterCount;
                        HalterPackage.IsExistance = HalterExist;
                        HalterPackage.NodeCode = Guid.Empty;
                        HalterPackage.ProjectCode = HalterProjectCode;
                        HalterPackage.ProductCode = eHalter.Code;
                        HalterPackage.ParentCode = PolePackage.Code;
                        HalterPackage.Type = (int)Atend.Control.Enum.ProductType.Halter;
                        HalterPackage.Number = "HALTER";
                        if (!HalterPackage.AccessInsert(aTransaction, aConnection))
                        {
                            throw new System.Exception("HalterPackage.AccessInsert failed");
                        }
                    }

                    i = 0;
                    //ed.WriteMessage("econsols:{0} , eConsolCount:{1} , eConsolExistance:{2} \n", eConsols.Count, eConsolCount.Count, eConsolExistance.Count);
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
                    //ed.WriteMessage("consold finished\n");

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

        public bool UpdatePoleData(Autodesk.AutoCAD.DatabaseServices.ObjectId PoleObjectId, ArrayList ConsolCodeForDel, Guid PoleCode)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            OleDbTransaction aTransaction;
            OleDbConnection aConnection = new OleDbConnection(Atend.Control.ConnectionString.AccessCnString);
            ObjectId PoleobjNew;
            ArrayList AddConsol = new ArrayList();
            try
            {
                aConnection.Open();
                aTransaction = aConnection.BeginTransaction();
                try
                {
                    PolePackage = Atend.Base.Design.DPackage.AccessSelectByCode(EXCode);
                    if (!UseAccess)
                    {

                        if (!ePole.AccessInsert(aTransaction, aConnection, true, true))
                            throw new System.Exception("ePole.AccessInsert failed");
                        //ed.WriteMessage("Inserted EPole\n");
                    }

                    PolePackage.IsExistance = Existance;
                    PolePackage.ProductCode = ePole.Code;
                    PolePackage.ProjectCode = ProjectCode;
                    Atend.Control.Common.Counters.PoleCounter++;
                    PolePackage.Number = string.Format("P{2}:{0:00}{1:00}", ePole.Height, Math.Round(ePole.Power / 100, 0), Atend.Control.Common.Counters.PoleCounter);
                    if (!PolePackage.AccessUpdate(aTransaction, aConnection))
                    {
                        throw new System.Exception("PolePackage.AccessInsert(tr) failed");
                    }

                    foreach (Atend.Base.Design.DPackage pack in beforDPackage)
                    {
                        //ed.WriteMessage("Update Pack\n");
                        if (!pack.AccessUpdate(aTransaction, aConnection))
                        {
                            throw new System.Exception("Update dPackage Failed");
                        }
                    }
                    //ed.WriteMessage("Update last last consol data\n");


                    foreach (string gu in ConsolCodeForDel)
                    {
                        //ed.WriteMessage("DeleteConsol\n");
                        if (!Atend.Base.Design.DPackage.AccessDelete(new Guid(gu), aTransaction, aConnection))
                        {
                            throw new System.Exception("ConsolPackage.AccessDelete failed");

                        }

                        if (!Atend.Base.Design.DConsol.AccessDelete(new Guid(gu), aTransaction, aConnection))
                        {
                            throw new System.Exception("DConsol.AccessDelete failed");

                        }
                    }
                    //ed.WriteMessage("After Delete Pack\n");
                    //if (Atend.Base.Equipment.EHalter.AccessSelectByXCode(eHalter.XCode).Code == -1)
                    //{ 
                    if (eHalterCount != 0)
                    {
                        if (!eHalter.AccessInsert(aTransaction, aConnection, true, true))
                            throw new System.Exception("Halter.AccessInsert failed");


                        //ed.WriteMessage("ehalter AccessInsert\n");

                        HalterPackage = new Atend.Base.Design.DPackage();
                        HalterPackage.Count = eHalterCount;
                        System.Data.DataTable DpackageSubs = Atend.Base.Design.DPackage.AccessSelectByParentCodeAndType(PolePackage.Code, (int)Atend.Control.Enum.ProductType.Halter);
                        if (DpackageSubs.Rows.Count > 0)
                        {
                            //halter was exist
                            HalterPackage.Code = new Guid(DpackageSubs.Rows[0]["Code"].ToString());
                            HalterPackage.IsExistance = HalterExist;
                            HalterPackage.NodeCode = Guid.Empty;
                            HalterPackage.ProjectCode = HalterProjectCode;
                            HalterPackage.ProductCode = eHalter.Code;
                            HalterPackage.ParentCode = PolePackage.Code;
                            HalterPackage.Type = (int)Atend.Control.Enum.ProductType.Halter;
                            HalterPackage.Number = "Halter"; //string.Format("P{2}-{0:00}{1:00}", Math.Round(ePole.Power / 100, 0), Math.Round(ePole.Height, 0), Atend.Control.Common.Counters.PoleCounter);
                            if (!HalterPackage.AccessUpdate(aTransaction, aConnection))
                            {
                                throw new System.Exception("HalterPackage.AccessUpdate failed");
                            }
                            //ed.WriteMessage("halter dpackage updated\n");
                        }
                        else
                        {
                            //Halter was not exist
                            HalterPackage = new Atend.Base.Design.DPackage();
                            HalterPackage.Count = eHalterCount;
                            HalterPackage.IsExistance = HalterExist;
                            HalterPackage.NodeCode = Guid.Empty;
                            HalterPackage.ProjectCode = HalterProjectCode;
                            HalterPackage.ProductCode = eHalter.Code;
                            HalterPackage.ParentCode = PolePackage.Code;
                            HalterPackage.Type = (int)Atend.Control.Enum.ProductType.Halter;
                            HalterPackage.Number = "HALTER";
                            if (!HalterPackage.AccessInsert(aTransaction, aConnection))
                            {
                                throw new System.Exception("HalterPackage.AccessInsert failed");
                            }
                        }
                    }
                    else if (eHalterCount == 0)
                    {
                        //ed.WriteMessage("halter is 0 \n");
                        System.Data.DataTable halters = Atend.Base.Design.DPackage.AccessSelectByParentCodeAndType(PolePackage.Code, (int)Atend.Control.Enum.ProductType.Halter);
                        if (halters.Rows.Count > 0)
                        {
                            //ed.WriteMessage("halter code - {0}\n", halters.Rows.Count);
                            if (!Atend.Base.Design.DPackage.AccessDelete(new Guid(halters.Rows[0]["Code"].ToString()), aTransaction, aConnection))
                            {
                                throw new System.Exception("Error In Delete dpackage.halter\n");
                            }
                        }
                        //ed.WriteMessage("halter deleted \n");
                    }
                    //ed.WriteMessage("halter finished \n");

                    int i = 0;
                    foreach (Atend.Base.Equipment.EConsol Consols in eConsols)
                    {
                        if (!Convert.ToBoolean(eConsolUseAccess[i]))
                        {
                            if (!Consols.AccessInsert(aTransaction, aConnection, true, true))
                            {
                                throw new System.Exception("eConsols.AccessInsert failed");
                            }
                        }
                        //ed.WriteMessage("Consol.Code={0}\n", Consols.Code);
                        Atend.Base.Acad.AT_INFO poleInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(PoleObjectId);
                        Atend.Base.Design.DPackage dp = Atend.Base.Design.DPackage.AccessSelectByNodeCode(new Guid(poleInfo.NodeCode));
                        ed.WriteMessage("PoleINfo.NodeCode={0}\n", poleInfo.NodeCode);

                        Atend.Base.Design.DPackage ConsolPackage = new Atend.Base.Design.DPackage();
                        ConsolPackage.Count = Convert.ToInt32(eConsolCount[i]);
                        ConsolPackage.IsExistance = Convert.ToInt32(eConsolExistance[i]);
                        ConsolPackage.ParentCode = dp.Code;
                        ConsolPackage.ProductCode = Consols.Code;
                        Atend.Control.Common.Counters.ConsolCounter++;
                        ConsolPackage.Number = string.Format("Ins{0:0000}", Atend.Control.Common.Counters.ConsolCounter);
                        ConsolPackage.Type = (int)Atend.Control.Enum.ProductType.Consol;
                        ConsolPackage.ProjectCode = Convert.ToInt32(eConsolProjectCode[i]);
                        if (!ConsolPackage.AccessInsert(aTransaction, aConnection))
                        {
                            throw new System.Exception("ConsolPackage.AccessInsert failed");
                        }

                        Atend.Base.Design.DConsol dConsol = new Atend.Base.Design.DConsol();
                        dConsol.LoadCode = 0;
                        dConsol.ParentCode = dp.Code;
                        dConsol.ProductCode = Consols.Code;
                        dConsol.Code = ConsolPackage.Code;
                        if (!dConsol.AccessInsert())
                        {
                            throw new System.Exception("dConsol.AccessInsert failed");
                        }
                        //ed.WriteMessage("ConsolPackage.Code={0}\n",ConsolPackage.Code);
                        AddConsol.Add(ConsolPackage.Code);

                        i++;
                    }
                    //ed.WriteMessage("Consol finished \n");

                    Atend.Base.Acad.AT_INFO poleInfo1 = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(PoleObjectId);
                    Atend.Base.Design.DNode node = Atend.Base.Design.DNode.AccessSelectByCode(new Guid(poleInfo1.NodeCode));

                    dPoleInfo.HalterType = eHalter.Code;
                    dPoleInfo.HalterCount = eHalterCount;
                    if (!dPoleInfo.AccessUpdate(aTransaction, aConnection))
                    {
                        throw new System.Exception("PoleInfo.AccessInsert failed");
                    }


                    node.Height = Height;
                    node.ProductCode = ePole.Code;
                    if (!node.AccessUpdate(aTransaction, aConnection))
                    {
                        throw new System.Exception("node.AccessUpdate Failed");
                    }

                    if (PoleObjectId != ObjectId.Null)
                    {
                        Atend.Base.Acad.AT_INFO atinfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(PoleObjectId);
                        atinfo.ProductCode = ePole.Code;
                        atinfo.Insert();
                    }

                    PoleobjNew = ChangePoleShape(PoleObjectId, ePole.Code);
                    //ed.WriteMessage("ChangePoleShape finished \n");
                    if (PoleobjNew == ObjectId.Null)
                    {
                        throw new System.Exception("ChangeShape Has Error\n");
                    }

                }
                catch (System.Exception ex1)
                {
                    ed.WriteMessage("ERROR UpdatePoleData(Transaction) 02 : {0} \n", ex1.Message);
                    aTransaction.Rollback();
                    aConnection.Close();
                    return false;

                }

            }
            catch (System.Exception ex)
            {
                ed.WriteMessage("ERROR UpdatePoleData 01 : {0} \n", ex.Message);
                aConnection.Close();
                return false;
            }

            aTransaction.Commit();
            foreach (string gu in ConsolCodeForDel)
            {
                if (!Atend.Global.Acad.DrawEquips.AcDrawConsol.DeleteConsol(PoleobjNew, gu))
                    throw new System.Exception("ACDrawConsol.DeleteConsol Failed");
            }
            ed.WriteMessage("aaa\n");
            Atend.Global.Acad.DrawEquips.AcDrawConsol.insertConsol(PoleobjNew, AddConsol);
            aConnection.Close();
            return true;
        }

        public static bool DeletePoleData(Autodesk.AutoCAD.DatabaseServices.ObjectId poleOI)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            OleDbConnection _Connection = new OleDbConnection(Atend.Control.ConnectionString.AccessCnString);
            OleDbTransaction _Transaction;

            try
            {
                _Connection.Open();
                _Transaction = _Connection.BeginTransaction();
                try
                {
                    Atend.Base.Acad.AT_INFO atinfopole = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(poleOI);
                    Atend.Base.Design.DPackage pack = Atend.Base.Design.DPackage.AccessSelectByNodeCode(new Guid(atinfopole.NodeCode.ToString()), _Transaction, _Connection);
                    if (!Atend.Base.Design.DPackage.AccessDelete(pack.Code, _Transaction, _Connection))
                    {
                        throw new System.Exception("Error In Delete dpackage.pole\n");
                    }

                    //DELETE HALTER FROM DPACKAGE 
                    System.Data.DataTable dt = Atend.Base.Design.DPackage.AccessSelectByParentCodeAndType(pack.Code, Convert.ToInt32(Atend.Control.Enum.ProductType.Halter), _Transaction, _Connection);
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (!Atend.Base.Design.DPackage.AccessDelete(new Guid(dr["Code"].ToString()), _Transaction, _Connection))
                        {
                            throw new System.Exception("Error In Delete dpackage.Halter\n");
                        }
                    }

                    Atend.Base.Acad.AT_SUB SubGP = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(poleOI);
                    foreach (Autodesk.AutoCAD.DatabaseServices.ObjectId oi in SubGP.SubIdCollection)
                    {
                        Atend.Base.Acad.AT_INFO poleSubInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oi);
                        if (poleSubInfo.ParentCode != "NONE" && poleSubInfo.NodeType == (int)Atend.Control.Enum.ProductType.Consol)
                        {
                            if (!Atend.Global.Acad.DrawEquips.AcDrawConsol.DeleteConsolData(oi, _Transaction, _Connection))
                            {
                                throw new System.Exception("Error In Delete dpackage.consol\n");
                            }
                        }
                        if (poleSubInfo.ParentCode != "NONE" && poleSubInfo.NodeType == (int)Atend.Control.Enum.ProductType.BankKhazan)
                        {
                            ObjectIdCollection CollectionKhazan = Atend.Global.Acad.UAcad.GetGroupSubEntities(oi);
                            foreach (Autodesk.AutoCAD.DatabaseServices.ObjectId collect in CollectionKhazan)
                            {
                                Atend.Base.Acad.AT_INFO at_info = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(collect);
                                if (at_info.ParentCode != "NONE" && at_info.NodeType == (int)Atend.Control.Enum.ProductType.ConsolElse)
                                {
                                    if (!Atend.Global.Acad.DrawEquips.AcDrawKhazan.DeleteKhazanData(collect, _Transaction, _Connection))
                                    {
                                        throw new System.Exception("Error In Delete Khazan\n");
                                    }
                                }
                            }
                        }
                        if (poleSubInfo.ParentCode != "NONE" && poleSubInfo.NodeType == (int)Atend.Control.Enum.ProductType.Rod)
                        {
                            ObjectIdCollection CollectionRod = Atend.Global.Acad.UAcad.GetGroupSubEntities(oi);
                            foreach (Autodesk.AutoCAD.DatabaseServices.ObjectId collect in CollectionRod)
                            {
                                Atend.Base.Acad.AT_INFO at_info = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(collect);
                                if (at_info.ParentCode != "NONE" && at_info.NodeType == (int)Atend.Control.Enum.ProductType.ConsolElse)
                                {
                                    if (!Atend.Global.Acad.DrawEquips.AcDrawRod.DeleteRodData(collect, _Transaction, _Connection))
                                    {
                                        throw new System.Exception("Error In Delete Rod\n");
                                    }
                                }
                            }
                        }
                        if (poleSubInfo.ParentCode != "NONE" && poleSubInfo.NodeType == (int)Atend.Control.Enum.ProductType.Kalamp)
                        {
                            if (!Atend.Global.Acad.DrawEquips.AcDrawKalamp.DeleteKalampData(oi, _Transaction, _Connection))
                            {
                                throw new System.Exception("Error In Delete Kalamp\n");
                            }
                        }
                        if (poleSubInfo.ParentCode != "NONE" && poleSubInfo.NodeType == (int)Atend.Control.Enum.ProductType.HeaderCabel)
                        {
                            if (!Atend.Global.Acad.DrawEquips.AcDrawHeaderCabel.DeleteHeaderCabelData(oi, _Transaction, _Connection))
                            {
                                throw new System.Exception("Error In Delete HeaderCabel\n");
                            }
                        }
                        if (poleSubInfo.ParentCode != "NONE" && poleSubInfo.NodeType == (int)Atend.Control.Enum.ProductType.KablSho)
                        {
                            if (!Atend.Global.Acad.DrawEquips.AcDrawKablsho.DeleteKablshoData(oi, _Transaction, _Connection))
                            {
                                throw new System.Exception("Error In Delete Kablsho\n");
                            }
                        }
                        if (poleSubInfo.ParentCode != "NONE" && poleSubInfo.NodeType == (int)Atend.Control.Enum.ProductType.Ground)
                        {
                            ObjectIdCollection Collection = Atend.Global.Acad.UAcad.GetGroupSubEntities(oi);
                            if (!Atend.Global.Acad.DrawEquips.AcDrawGround.DeleteGroundData(Collection[0], _Transaction, _Connection))//poleSubOI))
                            {
                                throw new System.Exception("Error In Delete Ground\n");
                            }
                        }
                        if (poleSubInfo.ParentCode != "NONE" && poleSubInfo.NodeType == (int)Atend.Control.Enum.ProductType.Light)
                        {
                            ObjectIdCollection Collection = Atend.Global.Acad.UAcad.GetGroupSubEntities(oi);
                            if (!Atend.Global.Acad.DrawEquips.AcDrawLight.DeleteLightData(Collection[0], _Transaction, _Connection))
                            {
                                throw new System.Exception("Error In Delete Light\n");
                            }
                        }
                        if (poleSubInfo.ParentCode != "NONE" && poleSubInfo.NodeType == (int)Atend.Control.Enum.ProductType.Halter)
                        {
                            //ed.WriteMessage("__________ halter.data\n");
                            //HALTER DELETE IN FIRST FUNCTION BY DT(//DELETE HALTER FROM DPACKAGE)
                            //NO SHAPE (this time) D_POLEINFO&D_PACKAGE
                        }
                    }
                }
                catch (System.Exception ex1)
                {
                    ed.WriteMessage("ERROR DeletePole(trans) 02 : {0}\n", ex1.Message);
                    _Transaction.Rollback();
                    _Connection.Close();
                    return false;
                }
            }
            catch (System.Exception ex)
            {
                ed.WriteMessage("ERROR DeletePole(trans) 01 : {0}\n", ex.Message);
                _Connection.Close();
                return false;
            }
            _Transaction.Commit();
            _Connection.Close();
            return true;
        }

        public static bool DeletePole(Autodesk.AutoCAD.DatabaseServices.ObjectId PoleOI)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            try
            {
                Atend.Base.Acad.AT_SUB poleSub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(PoleOI);
                foreach (Autodesk.AutoCAD.DatabaseServices.ObjectId poleSubOI in poleSub.SubIdCollection)
                {
                    Atend.Base.Acad.AT_INFO poleSubInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(poleSubOI);
                    if (poleSubInfo.ParentCode != "NONE" && poleSubInfo.NodeType == (int)Atend.Control.Enum.ProductType.Comment)
                    {
                        if (!Atend.Global.Acad.AcadRemove.DeleteEntityByObjectId(poleSubOI))
                        {
                            throw new System.Exception("Error In Delete Comment\n");
                        }
                    }
                    if (poleSubInfo.ParentCode != "NONE" && poleSubInfo.NodeType == (int)Atend.Control.Enum.ProductType.Consol)
                    {
                        if (!Atend.Global.Acad.DrawEquips.AcDrawConsol.DeleteConsol(poleSubOI))
                        {
                            throw new System.Exception("Error In Delete Consol\n");
                        }
                        if (!Atend.Global.Acad.AcadRemove.DeleteEntityByObjectId(poleSubOI))
                        {
                            throw new System.Exception("Error In Delete Consol.2\n");
                        }
                    }
                    if (poleSubInfo.ParentCode != "NONE" && poleSubInfo.NodeType == (int)Atend.Control.Enum.ProductType.BankKhazan)
                    {
                        ObjectIdCollection CollectionKhazan = Atend.Global.Acad.UAcad.GetGroupSubEntities(poleSubOI);
                        foreach (Autodesk.AutoCAD.DatabaseServices.ObjectId collect in CollectionKhazan)
                        {
                            Atend.Base.Acad.AT_INFO at_info = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(collect);
                            if (at_info.ParentCode != "NONE" && at_info.NodeType == (int)Atend.Control.Enum.ProductType.ConsolElse)
                            {
                                if (!Atend.Global.Acad.DrawEquips.AcDrawKhazan.DeleteKhazan(collect))
                                {
                                    throw new System.Exception("Error In Delete Khazan\n");
                                }
                                if (!Atend.Global.Acad.AcadRemove.DeleteEntityByObjectId(collect))
                                {
                                    throw new System.Exception("Error In Delete Khazan.2\n");
                                }
                            }
                        }
                    }
                    if (poleSubInfo.ParentCode != "NONE" && poleSubInfo.NodeType == (int)Atend.Control.Enum.ProductType.Rod)
                    {
                        ObjectIdCollection CollectionRod = Atend.Global.Acad.UAcad.GetGroupSubEntities(poleSubOI);
                        foreach (Autodesk.AutoCAD.DatabaseServices.ObjectId collect in CollectionRod)
                        {
                            Atend.Base.Acad.AT_INFO at_info = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(collect);
                            if (at_info.ParentCode != "NONE" && at_info.NodeType == (int)Atend.Control.Enum.ProductType.ConsolElse)
                            {
                                if (!Atend.Global.Acad.DrawEquips.AcDrawRod.DeleteRod(collect))
                                {
                                    throw new System.Exception("Error In Delete Rod\n");
                                }
                                if (!Atend.Global.Acad.AcadRemove.DeleteEntityByObjectId(collect))
                                {
                                    throw new System.Exception("Error In Delete Rod.2\n");
                                }
                            }
                        }
                    }
                    if (poleSubInfo.ParentCode != "NONE" && poleSubInfo.NodeType == (int)Atend.Control.Enum.ProductType.Kalamp)
                    {
                        if (!Atend.Global.Acad.DrawEquips.AcDrawKalamp.DeleteKalamp(poleSubOI))
                        {
                            throw new System.Exception("Error In Delete Kalamp\n");
                        }
                        if (!Atend.Global.Acad.AcadRemove.DeleteEntityByObjectId(poleSubOI))
                        {
                            throw new System.Exception("Error In Delete KA\n");
                        }
                    }
                    if (poleSubInfo.ParentCode != "NONE" && poleSubInfo.NodeType == (int)Atend.Control.Enum.ProductType.HeaderCabel)
                    {
                        if (!Atend.Global.Acad.DrawEquips.AcDrawHeaderCabel.DeleteHeaderCabel(poleSubOI))
                        {
                            throw new System.Exception("Error In Delete HeaderCabel\n");
                        }
                        if (!Atend.Global.Acad.AcadRemove.DeleteEntityByObjectId(poleSubOI))
                        {
                            throw new System.Exception("Error In Delete HC\n");
                        }
                    }
                    if (poleSubInfo.ParentCode != "NONE" && poleSubInfo.NodeType == (int)Atend.Control.Enum.ProductType.KablSho)
                    {
                        if (!Atend.Global.Acad.DrawEquips.AcDrawKablsho.DeleteKablsho(poleSubOI))
                        {
                            throw new System.Exception("Error In Delete Kablsho\n");
                        }
                        if (!Atend.Global.Acad.AcadRemove.DeleteEntityByObjectId(poleSubOI))
                        {
                            throw new System.Exception("Error In Delete ksh\n");
                        }
                    }
                    if (poleSubInfo.ParentCode != "NONE" && poleSubInfo.NodeType == (int)Atend.Control.Enum.ProductType.Ground)
                    {
                        ObjectIdCollection Collection = Atend.Global.Acad.UAcad.GetGroupSubEntities(poleSubOI);
                        if (!Atend.Global.Acad.DrawEquips.AcDrawGround.DeleteGround(Collection[0]))//poleSubOI))
                        {
                            throw new System.Exception("Error In Delete Ground\n");
                        }
                    }
                    if (poleSubInfo.ParentCode != "NONE" && poleSubInfo.NodeType == (int)Atend.Control.Enum.ProductType.Light)
                    {
                        ObjectIdCollection Collection = Atend.Global.Acad.UAcad.GetGroupSubEntities(poleSubOI);
                        if (!Atend.Global.Acad.DrawEquips.AcDrawLight.DeleteLight(Collection[0]))
                        {
                            throw new System.Exception("Error In Delete Light\n");
                        }
                    }
                    if (poleSubInfo.ParentCode != "NONE" && poleSubInfo.NodeType == (int)Atend.Control.Enum.ProductType.Halter)
                    {
                        //ed.WriteMessage("__________ halter.Graphic\n");
                        //NO SHAPE (this time)
                    }
                }
                //+++
                if (!Atend.Global.Acad.AcadRemove.DeleteEntityByObjectId(PoleOI))
                {
                    throw new System.Exception("GRA while delete pole \n");
                }

            }
            catch (System.Exception ex)
            {
                ed.WriteMessage("GRA ERROR POLE : {0} \n", ex.Message);
                return false;
            }
            return true;
        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//

        public void DrawPoleTip(Point3d Centerpoint, double Angle)
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;

            ObjectId NewPoleObjectId = ObjectId.Null;
            ObjectIdCollection NewConsolObjectIds = new ObjectIdCollection();

            double MyScale = Atend.Base.Design.DProductProperties.AccessSelectBySoftwareCode((int)Atend.Control.Enum.ProductType.Pole).Scale;
            double MyCommentScale = Atend.Base.Design.DProductProperties.AccessSelectBySoftwareCode((int)Atend.Control.Enum.ProductType.Pole).CommentScale;
            double MyCommentScaleConsol = Atend.Base.Design.DProductProperties.AccessSelectBySoftwareCode((int)Atend.Control.Enum.ProductType.Consol).CommentScale;

            using (DocumentLock docLock = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.LockDocument())
            {
                int i = 0;
                DrawPoleJig drawPoleJig = new DrawPoleJig(eConsols.Count, MyScale);
                List<Autodesk.AutoCAD.DatabaseServices.Entity> entities = drawPoleJig.GetDemo(Centerpoint, Angle);
                #region Save data here
                if (SavePoleTipData())
                {
                    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
                    foreach (Autodesk.AutoCAD.DatabaseServices.Entity ent in entities)
                    {
                        object productType = null;
                        Autodesk.AutoCAD.DatabaseServices.Entity newEntity = ent;
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
                            consol.Code = dPackages[i].Code;
                            consol.LoadCode = 0;
                            consol.ProductCode = eConsol.Code;
                            consol.ParentCode = dNode.Code;

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

                                Atend.Base.Acad.AT_SUB TextSub = new Atend.Base.Acad.AT_SUB(TextOi);
                                TextSub.SubIdCollection.Add(NewConsolObjectID);
                                TextSub.Insert();

                            }

                            i++;

                        }//End of DRaw consol

                    }// Draw Finished

                    //insert consols as a sub for pole
                    foreach (Autodesk.AutoCAD.DatabaseServices.ObjectId obji in NewConsolObjectIds)
                    {
                        Atend.Base.Acad.AT_SUB.AddToAT_SUB(obji, NewPoleObjectId);
                    }

                }
                #endregion

            }
        }

        public void DrawPolTip()
        {

            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            //Database db = HostApplicationServices.WorkingDatabase;
            Database db = Application.DocumentManager.MdiActiveDocument.Database;
            //Point3d TablePosition;
            ObjectId NewPoleObjectId = ObjectId.Null;
            ObjectIdCollection NewConsolObjectIds = new ObjectIdCollection();
            //ed.WriteMessage("~~~Design scale :{0}~~~ \n", Atend.Control.Common.SelectedDesignScale);

            //ed.WriteMessage("Pole:{0}", Atend.Control.Common.Counters.PoleCounter);
            //ed.WriteMessage("Consol:{0}", Atend.Control.Common.Counters.ConsolCounter);
            //ed.WriteMessage("Clamp:{0}", Atend.Control.Common.Counters.ClampCounter);
            //ed.WriteMessage("HeaderCabel:{0}\n", Atend.Control.Common.Counters.HeadercableCounter);
            double MyScale = Atend.Base.Design.DProductProperties.AccessSelectBySoftwareCode((int)Atend.Control.Enum.ProductType.Pole).Scale;
            double MyCommentScale = Atend.Base.Design.DProductProperties.AccessSelectBySoftwareCode((int)Atend.Control.Enum.ProductType.Pole).CommentScale;
            double MyCommentScaleConsol = Atend.Base.Design.DProductProperties.AccessSelectBySoftwareCode((int)Atend.Control.Enum.ProductType.Consol).CommentScale;

            using (DocumentLock docLock = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.LockDocument())
            {
                bool conti = true;
                int i = 0;
                DrawPoleJig drawPoleJig = new DrawPoleJig(eConsols.Count, MyScale);

                //ed.WriteMessage("ConsolCount was sent {0} \n", dConsolCount);

                while (conti)
                {

                    PromptResult pr;

                    pr = ed.Drag(drawPoleJig);

                    if (pr.Status == PromptStatus.OK && !drawPoleJig.GetAngle)
                    {

                        drawPoleJig.GetPoint = false;
                        drawPoleJig.GetAngle = true;

                    }
                    else if (pr.Status == PromptStatus.OK && drawPoleJig.GetAngle)
                    {

                        conti = false;
                        List<Autodesk.AutoCAD.DatabaseServices.Entity> entities = drawPoleJig.GetEntities();

                        #region Save data here
                        if (SavePoleTipData())
                        {
                            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
                            foreach (Autodesk.AutoCAD.DatabaseServices.Entity ent in entities)
                            {
                                object productType = null;
                                Autodesk.AutoCAD.DatabaseServices.Entity newEntity = ent;
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
                                    consol.Code = dPackages[i].Code;
                                    consol.LoadCode = 0;
                                    consol.ProductCode = eConsol.Code;
                                    consol.ParentCode = dNode.Code;

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

                                        Atend.Base.Acad.AT_SUB TextSub = new Atend.Base.Acad.AT_SUB(TextOi);
                                        TextSub.SubIdCollection.Add(NewConsolObjectID);
                                        TextSub.Insert();

                                    }

                                    i++;
                                    // ed.WriteMessage("Extension was done \n");

                                }//End of DRaw consol

                            }// Draw Finished

                            //insert consols as a sub for pole
                            foreach (Autodesk.AutoCAD.DatabaseServices.ObjectId obji in NewConsolObjectIds)
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

        private bool SavePoleTipData()
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
                        HalterPackage.IsExistance = HalterExist;
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

            Atend.Base.Acad.AcadGlobal.PoleData.UseAccess = true;
            UseAccess = true;
            for (int i = 0; i < Atend.Base.Acad.AcadGlobal.PoleData.eConsolUseAccess.Count; i++)
            {
                Atend.Base.Acad.AcadGlobal.PoleData.eConsolUseAccess[i] = true;
            }

            #endregion


            //ed.WriteMessage("~~~~~~~~~~~~~ End Save Pole Data ~~~~~~~~~~~~~~~~~~\n");

            return true;
        }

        public bool UpdatePoleDataTip(Autodesk.AutoCAD.DatabaseServices.ObjectId PoleObjectId, ArrayList ConsolCodeForDel, Guid PoleCode)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            //ed.WriteMessage("#### :{0}\n", ConsolCodeForDel.Count);
            OleDbTransaction aTransaction;
            ObjectId PoleobjNew;
            OleDbConnection aConnection = new OleDbConnection(Atend.Control.ConnectionString.AccessCnString);
            ArrayList AddConsol = new ArrayList();
            try
            {
                aConnection.Open();
                aTransaction = aConnection.BeginTransaction();
                try
                {
                    //ed.WriteMessage("+1\n");
                    foreach (string gu in ConsolCodeForDel)
                    {
                        if (!Atend.Base.Design.DPackage.AccessDelete(new Guid(gu), aTransaction, aConnection))
                        {
                            throw new System.Exception("ConsolPackage.AccessDelete failed");

                        }

                        if (!Atend.Base.Design.DConsol.AccessDelete(new Guid(gu), aTransaction, aConnection))
                        {
                            throw new System.Exception("DConsol.AccessDelete failed");

                        }
                    }
                    
                    //ed.WriteMessage("+2\n");
                    PolePackage = Atend.Base.Design.DPackage.AccessSelectByNodeCode(PoleCode);
                    //ed.WriteMessage("+3\n");

                    //WENT TO
                    //if (!Atend.Base.Equipment.EContainerPackage.SentFromLocalToAccess(eHalter.XCode, (int)Atend.Control.Enum.ProductType.Halter, eHalter.Code, aTransaction, aConnection))
                    //{
                    //    throw new System.Exception("SentFromLocalToAccess failed");
                    //}

                    if (!UseAccess)
                    {
                        //ed.WriteMessage("+4\n");
                        //ed.WriteMessage("Insert EPole\n");
                        if (!ePole.AccessInsert(aTransaction, aConnection, true, true))
                            throw new System.Exception("ePole.AccessInsert failed");
                        //ed.WriteMessage("+5\n");
                        //WENT TO
                        //if (!Atend.Base.Equipment.EContainerPackage.SentFromLocalToAccess(ePole.XCode, (int)Atend.Control.Enum.ProductType.Pole, ePole.Code, aTransaction, aConnection))
                        //{
                        //    throw new System.Exception("SentFromLocalToAccess failed");
                        //}

                        ePoleTip.PoleCode = ePole.Code;
                        ePoleTip.HalterID = eHalter.Code;
                        if (!ePoleTip.AccessInsert(aTransaction, aConnection, true, true))
                            throw new System.Exception("ePoleTip.AccessInsert failed");
                        //ed.WriteMessage("+6\n");
                    }
                    //ed.WriteMessage("PoleTip.Code={0}\n", ePoleTip.Code);
                    PolePackage.IsExistance = Existance;
                    PolePackage.ProductCode = ePoleTip.Code;
                    //ed.WriteMessage("**ProductCode={0}\n", PolePackage.ProductCode);
                    PolePackage.ProjectCode = ProjectCode;
                    PolePackage.Number = string.Format("P{2}-{0:00}{1:00}", ePole.Height, Math.Round(ePole.Power / 100, 0), Atend.Control.Common.Counters.PoleCounter);
                    //PolePackage.Number = "";
                    
                    if (!PolePackage.AccessUpdate(aTransaction, aConnection))
                    {
                        throw new System.Exception("PolePackage.AccessInsert(tr) failed");
                    }
                    
                    //ed.WriteMessage("+7\n");
                    //foreach (string gu in ConsolCodeForDel)
                    //{
                    //    //ed.WriteMessage("DeleteConsol\n");
                    //    if (!Atend.Base.Design.DPackage.AccessDelete(new Guid(gu), aTransaction, aConnection))
                    //    {
                    //        throw new System.Exception("ConsolPackage.AccessDelete failed");

                    //    }

                    //    if (!Atend.Base.Design.DConsol.AccessDelete(new Guid(gu), aTransaction, aConnection))
                    //    {
                    //        throw new System.Exception("DConsol.AccessDelete failed");

                    //    }
                    //}

                    //ed.WriteMessage("Halter\n");
                    
                    if (eHalterCount != 0)
                    {
                        if (!eHalter.AccessInsert(aTransaction, aConnection, true, true))
                            throw new System.Exception("Halter.AccessInsert failed");
                        //ed.WriteMessage("+9\n");
                        HalterPackage = new Atend.Base.Design.DPackage();
                        HalterPackage.Count = eHalterCount;
                        //ed.WriteMessage("+10\n");
                        //ed.WriteMessage("********** :{0},{1}\n", PolePackage.Code, (int)Atend.Control.Enum.ProductType.Halter);
                        System.Data.DataTable DpackageSubs = Atend.Base.Design.DPackage.AccessSelectByParentCodeAndType(PolePackage.Code, (int)Atend.Control.Enum.ProductType.Halter);
                        //ed.WriteMessage("^^^^^^\n");
                        if (DpackageSubs.Rows.Count > 0)
                        {
                            //halter was exist
                            HalterPackage.Code = new Guid(DpackageSubs.Rows[0]["Code"].ToString());
                            HalterPackage.IsExistance = HalterExist;
                            HalterPackage.NodeCode = Guid.Empty;
                            //ed.WriteMessage("POLE****{0}\n", ProjectCode);
                            HalterPackage.ProjectCode = HalterProjectCode;
                            HalterPackage.ProductCode = eHalter.Code;
                            HalterPackage.ParentCode = PolePackage.Code;
                            //if (po.Code == -1)
                            //    PolePackage.ProductCode = ePole.Code;
                            //else
                            //    PolePackage.ProductCode = po.Code;
                            HalterPackage.Type = (int)Atend.Control.Enum.ProductType.Halter;
                            //Atend.Control.Common.Counters.PoleCounter++;
                            //ed.WriteMessage("power:{0} heigth:{1}\n", ePole.Power, ePole.Height);
                            HalterPackage.Number = "HALTER"; //string.Format("P{2}-{0:00}{1:00}", Math.Round(ePole.Power / 100, 0), Math.Round(ePole.Height, 0), Atend.Control.Common.Counters.PoleCounter);

                            //ed.WriteMessage("\nHalter Exist = {0} \n Halter ProjectCode = {1}\n", HalterPackage.IsExistance.ToString(), HalterPackage.ProjectCode.ToString());

                            //if (po.Code == -1)
                            //    PolePackage.Number = string.Format("P{0:00}{1:00}:{2}", ePole.Power, ePole.Height, Atend.Control.Common.Counters.PoleCounter);
                            //else
                            //    PolePackage.Number = string.Format("P{0:00}{1:00}:{2}", po.Power, po.Height, Atend.Control.Common.Counters.PoleCounter);
                            if (!HalterPackage.AccessUpdate(aTransaction, aConnection))
                            {
                                throw new System.Exception("HalterPackage.AccessUpdate failed");
                            }
                        }
                        else
                        {
                            //Halter was not exist
                            HalterPackage = new Atend.Base.Design.DPackage();
                            HalterPackage.Count = eHalterCount;
                            HalterPackage.IsExistance = HalterExist;
                            HalterPackage.NodeCode = Guid.Empty;
                            HalterPackage.ProjectCode = HalterProjectCode;
                            HalterPackage.ProductCode = eHalter.Code;
                            HalterPackage.ParentCode = PolePackage.Code;
                            HalterPackage.Type = (int)Atend.Control.Enum.ProductType.Halter;
                            HalterPackage.Number = "HALTER";
                            if (!HalterPackage.AccessInsert(aTransaction, aConnection))
                            {
                                throw new System.Exception("HalterPackage.AccessInsert failed");
                            }
                        }
                    }
                    else if (eHalterCount == 0)
                    {
                        System.Data.DataTable tt1 = Atend.Base.Design.DPackage.AccessSelectByParentCodeAndType(PolePackage.Code, (int)Atend.Control.Enum.ProductType.Halter);
                        if (tt1.Rows.Count != 0)
                        {
                            if (!Atend.Base.Design.DPackage.AccessDelete(new Guid(tt1.Rows[0]["Code"].ToString()), aTransaction, aConnection))
                            {
                                throw new System.Exception("Error In Delete dpackage.halter\n");
                            }
                        }
                    }
                    
                    int i = 0;
                    foreach (Atend.Base.Equipment.EConsol Consols in eConsols)
                    {
                        //ed.WriteMessage("UseAcces={0}\n", eConsolUseAccess[i].ToString());
                        if (!Convert.ToBoolean(eConsolUseAccess[i]))
                        {
                            //ed.WriteMessage("Consol.AccessInsert\n");
                            if (!Consols.AccessInsert(aTransaction, aConnection, true, true))
                            {
                                throw new System.Exception("eConsols.AccessInsert failed");
                            }
                        }
                        //ed.WriteMessage("Consol.Code={0}\n", Consols.Code);
                        Atend.Base.Acad.AT_INFO poleInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(PoleObjectId);
                        Atend.Base.Design.DPackage dp = Atend.Base.Design.DPackage.AccessSelectByNodeCode(new Guid(poleInfo.NodeCode));

                        Atend.Base.Design.DPackage ConsolPackage = new Atend.Base.Design.DPackage();
                        ConsolPackage.Count = Convert.ToInt32(eConsolCount[i]);
                        ConsolPackage.IsExistance = Convert.ToInt32(eConsolExistance[i]);
                        ConsolPackage.ParentCode = dp.Code;
                        ConsolPackage.ProductCode = Consols.Code;
                        Atend.Control.Common.Counters.ConsolCounter++;
                        ConsolPackage.Number = string.Format("Ins{0:0000}", Atend.Control.Common.Counters.ConsolCounter);
                        ConsolPackage.Type = (int)Atend.Control.Enum.ProductType.Consol;
                        ConsolPackage.ProjectCode = Convert.ToInt32(eConsolProjectCode[i]);
                        if (!ConsolPackage.AccessInsert(aTransaction, aConnection))
                        {
                            throw new System.Exception("ConsolPackage.AccessInsert failed");
                        }


                        Atend.Base.Design.DConsol dConsol = new Atend.Base.Design.DConsol();
                        dConsol.LoadCode = 0;
                        dConsol.ParentCode = PolePackage.Code;
                        dConsol.ProductCode = Consols.Code;
                        dConsol.Code = ConsolPackage.Code;
                        if (!dConsol.AccessInsert())
                        {
                            throw new System.Exception("dConsol.AccessInsert failed");
                        }

                        //ed.WriteMessage("ConsolPackage.Code={0}\n", ConsolPackage.Code);
                        AddConsol.Add(ConsolPackage.Code);

                        i++;
                    }

                    //ed.WriteMessage("PoleInfo\n");

                    Atend.Base.Acad.AT_INFO poleInfo1 = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(PoleObjectId);
                    //ed.WriteMessage("PoleINFO1={0}\n", poleInfo1.NodeCode);
                    Atend.Base.Design.DNode node = Atend.Base.Design.DNode.AccessSelectByCode(new Guid(poleInfo1.NodeCode));
                    node.Height = Height;
                    node.ProductCode = ePoleTip.Code;


                    Atend.Base.Design.DPoleInfo dpoleInfo = Atend.Base.Design.DPoleInfo.AccessSelectByNodeCode(new Guid(poleInfo1.NodeCode));
                    dpoleInfo.HalterType = eHalter.Code;
                   
                    if (!dpoleInfo.AccessUpdate(aTransaction, aConnection))
                    {
                        throw new System.Exception("dPoleInfo.AcccessUpdate Failed");
                    }
                    
                    if (!node.AccessUpdate(aTransaction, aConnection))
                    {
                        throw new System.Exception("Node.AccessUpdate Failed");
                    }
                    
                    if (PoleObjectId != ObjectId.Null)
                    {
                        Atend.Base.Acad.AT_INFO atinfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(PoleObjectId);
                        atinfo.ProductCode = ePoleTip.Code;
                        atinfo.Insert();
                    }

                    
                    PoleobjNew = ChangePoleShape(PoleObjectId, ePole.Code);
                    if (PoleobjNew == ObjectId.Null)
                    {
                        throw new System.Exception("ChangeShape Has Error\n");
                    }
                }
                catch (System.Exception ex1)
                {
                    ed.WriteMessage("ERROR UpdatePoleData(Transaction) 01 : {0} \n", ex1.Message);
                    aTransaction.Rollback();
                    aConnection.Close();
                    return false;

                }
            }
            catch (System.Exception ex)
            {
                ed.WriteMessage("ERROR UpdatePoleData 01 : {0} \n", ex.Message);
                aConnection.Close();
                return false;
            }

            aTransaction.Commit();
            foreach (string gu in ConsolCodeForDel)
            {
                if (!Atend.Global.Acad.DrawEquips.AcDrawConsol.DeleteConsol(PoleobjNew, gu))
                    throw new System.Exception("ACDrawConsol.DeleteConsol Failed");
            }
            //ed.WriteMessage("objPole={0},AddConsol={1}\n", PoleObjectId, AddConsol.Count);
            Atend.Global.Acad.DrawEquips.AcDrawConsol.insertConsol(PoleobjNew, AddConsol);
            aConnection.Close();
            return true;
        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//

        public Autodesk.AutoCAD.DatabaseServices.ObjectId ChangePoleShape(Autodesk.AutoCAD.DatabaseServices.ObjectId PoleObjectId, int NewProductCode)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;

            Atend.Base.Acad.AT_INFO LastPoleInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(PoleObjectId);
            LastPoleInfo.ProductCode = NewProductCode;
            Atend.Base.Acad.AT_SUB LastPoleSub = Atend.Base.Acad.AT_SUB.SelectBySelectedObjectId(PoleObjectId);
            Point3d LastPoleCenterPoint = Point3d.Origin;

            double MyScale = Atend.Base.Design.DProductProperties.AccessSelectBySoftwareCode((int)Atend.Control.Enum.ProductType.Pole).Scale;
            double MyCommentScale = Atend.Base.Design.DProductProperties.AccessSelectBySoftwareCode((int)Atend.Control.Enum.ProductType.Pole).CommentScale;
            double MyCommentScaleConsol = Atend.Base.Design.DProductProperties.AccessSelectBySoftwareCode((int)Atend.Control.Enum.ProductType.Consol).CommentScale;


            LastPoleCenterPoint = UAcad.CenterOfEntity(UAcad.GetEntityByObjectID(PoleObjectId));
            ObjectId NewDrawPole = ObjectId.Null;
            //ed.WriteMessage("shape:{0} :::: {1}", ePole.Shape, ePole.Type);
            switch (ePole.Shape)
            {
                case 0:
                    //ed.WriteMessage("---->>>.---- TYPE=0\n");
                    NewDrawPole = Atend.Global.Acad.UAcad.DrawEntityOnScreen(DrawCirclePole(LastPoleCenterPoint, MyScale), Atend.Control.Enum.AutoCadLayerName.GENERAL.ToString());
                    //Atend.Global.Acad.DrawEquips.AcDrawCirclePole.RotatePoleCircle(LastPoleInfo.Angle, new Guid(LastPoleInfo.NodeCode));
                    //RotatePole();
                    break;
                case 1:
                    if (ePole.Type == 2)//pertic
                    {
                        //ed.WriteMessage("---->>>.---- TYPE=1.2\n");
                        //doc.SendStringToExecute("_PolePolygon ", true, false, false);
                        NewDrawPole = Atend.Global.Acad.UAcad.DrawEntityOnScreen(DrawPolePolygon(LastPoleCenterPoint, MyScale), Atend.Control.Enum.AutoCadLayerName.GENERAL.ToString());
                        //Atend.Global.Acad.DrawEquips.AcDrawPolygonPole.RotatePolePolygon(LastPoleInfo.Angle, new Guid(LastPoleInfo.NodeCode));
                        //RotatePole();
                    }
                    else
                    {
                        //ed.WriteMessage("---->>>.---- TYPE=1.else\n");
                        //doc.SendStringToExecute("_POLE ", true, false, false);
                        NewDrawPole = Atend.Global.Acad.UAcad.DrawEntityOnScreen(DrawPoleEntity(LastPoleCenterPoint, MyScale), Atend.Control.Enum.AutoCadLayerName.GENERAL.ToString());
                        //RotatePole(LastPoleInfo.Angle, new Guid(LastPoleInfo.NodeCode));
                    }
                    break;
                default:
                    //ed.WriteMessage("---->>>.---- TYPE=defaulkt\n");
                    NewDrawPole = Atend.Global.Acad.UAcad.DrawEntityOnScreen(DrawPoleEntity(LastPoleCenterPoint, MyScale), Atend.Control.Enum.AutoCadLayerName.GENERAL.ToString());
                    //RotatePole(LastPoleInfo.Angle, new Guid(LastPoleInfo.NodeCode));
                    //RotatePole(LastPoleInfo.Angle, new Guid(LastPoleInfo.NodeCode));
                    break;

            }


            if (NewDrawPole != null)
            {
                LastPoleInfo.SelectedObjectId = NewDrawPole;
                LastPoleInfo.ProductCode = ePole.Code;
                LastPoleInfo.Insert();
                //ed.WriteMessage("shape:{0} :::: {1}\n", ePole.Shape, ePole.Type);
                switch (ePole.Shape)
                {
                    case 0:
                        //ed.WriteMessage("---->>>.---- TYPE=0\n");
                        Atend.Global.Acad.DrawEquips.AcDrawCirclePole.RotatePoleCircle(LastPoleInfo.Angle, new Guid(LastPoleInfo.NodeCode));
                        break;
                    case 1:
                        if (ePole.Type == 2)//pertic
                        {
                            //ed.WriteMessage("---->>>.---- TYPE=1.2\n");
                            Atend.Global.Acad.DrawEquips.AcDrawPolygonPole.RotatePolePolygon(LastPoleInfo.Angle, new Guid(LastPoleInfo.NodeCode));
                        }
                        else
                        {
                            //ed.WriteMessage("---->>>.---- TYPE=1.else\n");
                            Atend.Global.Acad.DrawEquips.AcDrawPole.RotatePole(LastPoleInfo.Angle, new Guid(LastPoleInfo.NodeCode));
                        }
                        break;
                    default:
                        //ed.WriteMessage("---->>>.---- TYPE=defaulkt\n");
                        Atend.Global.Acad.DrawEquips.AcDrawPole.RotatePole(LastPoleInfo.Angle, new Guid(LastPoleInfo.NodeCode));
                        break;

                }

                foreach (Autodesk.AutoCAD.DatabaseServices.ObjectId oi in LastPoleSub.SubIdCollection)
                {
                    Atend.Base.Acad.AT_INFO consolInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oi);
                    if (consolInfo.ParentCode != "NONE" && (consolInfo.NodeType == (int)Atend.Control.Enum.ProductType.Consol || consolInfo.NodeType == (int)Atend.Control.Enum.ProductType.Kalamp || consolInfo.NodeType == (int)Atend.Control.Enum.ProductType.HeaderCabel || consolInfo.NodeType == (int)Atend.Control.Enum.ProductType.KablSho || consolInfo.NodeType == (int)Atend.Control.Enum.ProductType.Comment))
                    {
                        //ed.writeMessage("NewPole= " + NewDrawPole.ToString() + "lastPole= " + PoleObjectId.ToString() + "\n");

                        Atend.Base.Acad.AT_SUB.AddToAT_SUB(NewDrawPole, oi);
                        //ed.writeMessage("Consol:{0}", oi);
                        Atend.Base.Acad.AT_SUB.RemoveFromAT_SUB(PoleObjectId, oi);

                        if (consolInfo.NodeType == (int)Atend.Control.Enum.ProductType.Comment)
                        {
                            ChangeEntityText(oi, PolePackage.Number);

                        }

                    }
                }

                LastPoleSub.SelectedObjectId = NewDrawPole;
                LastPoleSub.Insert();


                //ed.WriteMessage("go for delete entity \n");
                AcadRemove.DeleteEntityByObjectId(PoleObjectId);
            }
            //ed.WriteMessage("finish Change Shape {0} : {1}\n", LastPoleInfo.Angle, LastPoleInfo.NodeCode);
            //RotatePole(LastPoleInfo.Angle, new Guid(LastPoleInfo.NodeCode));

            return NewDrawPole;
        }

        /// <summary>
        /// rectangle pole entity
        /// </summary>
        /// <param name="BasePoint"></param>
        /// <returns></returns>
        private Autodesk.AutoCAD.DatabaseServices.Entity DrawPoleEntity(Point3d BasePoint, double Scale)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            double BaseX = BasePoint.X;
            double BaseY = BasePoint.Y;

            Polyline pLine = new Polyline();
            pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX - 50, BaseY - 25), 0, 0, 0);
            pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX + 50, BaseY - 25), 0, 0, 0);
            pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX + 50, BaseY + 25), 0, 0, 0);
            pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX - 50, BaseY + 25), 0, 0, 0);
            pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX - 50, BaseY - 25), 0, 0, 0);
            pLine.Closed = true;

            Matrix3d trans1 = Matrix3d.Scaling(Scale, new Point3d(BasePoint.X, BasePoint.Y, 0));
            pLine.TransformBy(trans1);

            return pLine;

        }

        /// <summary>
        /// circle pole Autodesk.AutoCAD.DatabaseServices.Entity
        /// </summary>
        /// <param name="BasePoint"></param>
        /// <returns></returns>
        private Autodesk.AutoCAD.DatabaseServices.Entity DrawCirclePole(Point3d BasePoint, double Scale)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            Circle MyCir = new Circle();
            MyCir.Radius = 50;
            MyCir.Center = BasePoint;
            MyCir.Normal = Vector3d.ZAxis;

            Matrix3d trans1 = Matrix3d.Scaling(Scale, new Point3d(BasePoint.X, BasePoint.Y, 0));
            MyCir.TransformBy(trans1);

            return MyCir;
        }

        /// <summary>
        /// polygon pole entity
        /// </summary>
        /// <param name="BasePoint"></param>
        /// <returns></returns>
        private Autodesk.AutoCAD.DatabaseServices.Entity DrawPolePolygon(Point3d BasePoint, double Scale)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            double BaseX = BasePoint.X;
            double BaseY = BasePoint.Y;

            Polyline pLine = new Polyline();
            pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX - 70, BaseY), 0, 0, 0);
            pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX - 35, BaseY + 35), 0, 0, 0);
            pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX + 35, BaseY + 35), 0, 0, 0);
            pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX + 70, BaseY), 0, 0, 0);
            pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX + 35, BaseY - 35), 0, 0, 0);
            pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX - 35, BaseY - 35), 0, 0, 0);
            pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX - 70, BaseY), 0, 0, 0);

            pLine.Closed = true;
            Matrix3d trans1 = Matrix3d.Scaling(Scale, new Point3d(BasePoint.X, BasePoint.Y, 0));
            pLine.TransformBy(trans1);

            return pLine;


        }

        public void ChangeEntityText(Autodesk.AutoCAD.DatabaseServices.ObjectId SelectedTextObjectID, string Text)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;

            //Database db = HostApplicationServices.WorkingDatabase;
            Database db = Application.DocumentManager.MdiActiveDocument.Database;
            using (DocumentLock dl = Application.DocumentManager.MdiActiveDocument.LockDocument())
            {
                using (Transaction tr = db.TransactionManager.StartTransaction())
                {
                    DBText dbtext = (DBText)tr.GetObject(SelectedTextObjectID, OpenMode.ForWrite);

                    if (dbtext != null)
                    {
                        //ed.WriteMessage("*********THING FOUND.\n");

                        dbtext.TextString = Text;
                    }

                    tr.Commit();

                    //ed.Regen();
                }
            }
        }

        public static List<Autodesk.AutoCAD.DatabaseServices.Entity> LegendEntity(Point3d CenterPoint, string Text)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            double bassX = CenterPoint.X;
            double bassY = CenterPoint.Y;
            List<Autodesk.AutoCAD.DatabaseServices.Entity> Entities = new List<Autodesk.AutoCAD.DatabaseServices.Entity>();

            Polyline p = new Polyline();
            p.AddVertexAt(p.NumberOfVertices, new Point2d(bassX - 3, bassY + 6), 0, 0, 0);
            p.AddVertexAt(p.NumberOfVertices, new Point2d(bassX + 3, bassY + 6), 0, 0, 0);
            p.AddVertexAt(p.NumberOfVertices, new Point2d(bassX + 3, bassY - 6), 0, 0, 0);
            p.AddVertexAt(p.NumberOfVertices, new Point2d(bassX - 3, bassY - 6), 0, 0, 0);
            p.AddVertexAt(p.NumberOfVertices, new Point2d(bassX - 3, bassY + 6), 0, 0, 0);
            Entities.Add(p);


            MText mtext = new MText();
            mtext.Location = new Point3d(bassX - 8, bassY, 0);
            mtext.Contents = Text;
            Entities.Add(mtext);


            //ed.WriteMessage("entied add \n");
            return Entities;
        }

        public static void ShowDescription(Autodesk.AutoCAD.DatabaseServices.ObjectId oi, OleDbConnection _Conection)
        {

            int ProductCode = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oi).ProductCode;
            Autodesk.AutoCAD.DatabaseServices.Entity CurrentEntity = Atend.Global.Acad.UAcad.GetEntityByObjectID(oi);
            double CommentScale = Atend.Base.Design.DProductProperties.AccessSelectBySoftwareCode((int)Atend.Control.Enum.ProductType.AirPost).Scale;
            Atend.Base.Equipment.EPole _EPole = Atend.Base.Equipment.EPole.AccessSelectByCode(ProductCode, _Conection);
            if (_EPole.Code != -1)
            {

                Point3d EntityCenterPoint = Atend.Global.Acad.UAcad.CenterOfEntity(CurrentEntity);
                Entity TextEntity = Atend.Global.Acad.UAcad.WriteNote(_EPole.Comment, EntityCenterPoint, CommentScale);
                Atend.Global.Acad.UAcad.DrawEntityOnScreen(TextEntity, Atend.Control.Enum.AutoCadLayerName.DESCRIPTION.ToString());


            }
        }

        public static void DrawShield()
        {
            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            Dictionary<string, Point2dCollection> MyDic = new Dictionary<string, Point2dCollection>();
            Dictionary<string, ObjectId> MyDic1 = new Dictionary<string, ObjectId>();
            Dictionary<string, ObjectId> MyDicCircle = new Dictionary<string, ObjectId>();
            try
            {
                TypedValue[] tvs = new TypedValue[] { new TypedValue((int)DxfCode.LayerName, Atend.Control.Enum.AutoCadLayerName.GENERAL.ToString()) };
                SelectionFilter sf = new SelectionFilter(tvs);
                PromptSelectionResult psr = ed.SelectAll(sf);
                if (psr.Value != null)
                {
                    ObjectId[] ids = psr.Value.GetObjectIds();
                    //ObjectIdCollection OIC = new ObjectIdCollection();

                    foreach (Autodesk.AutoCAD.DatabaseServices.ObjectId oi in ids)
                    {
                        Atend.Base.Acad.AT_INFO PostInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oi);
                        if (PostInfo.ParentCode != "NONE" && PostInfo.NodeType == (int)Atend.Control.Enum.ProductType.Pole)
                        {
                            Autodesk.AutoCAD.DatabaseServices.Entity ent = Atend.Global.Acad.UAcad.GetEntityByObjectID(oi);
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
                                else if (pl.NumberOfVertices == 7)
                                {
                                    pts = new Point2dCollection(7);
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
                            else
                            {
                                //circle pole
                                Circle plc = ent as Circle;
                                if (plc != null)
                                {
                                    MyDicCircle.Add(PostInfo.NodeCode, oi);
                                }
                            }
                        }
                        //ed.WriteMessage("--------------------------------------- \n");
                    }
                    ids = null;
                    foreach (string a in MyDic.Keys)
                    {
                        Point2dCollection p = new Point2dCollection();
                        MyDic.TryGetValue(a, out p);
                        Atend.Global.Acad.Global.CreateWhiteBack(p);
                    }

                    foreach (string NodeCode in MyDic1.Keys)
                    {
                        ObjectId PoleOI = ObjectId.Null;

                        if (MyDic1.TryGetValue(NodeCode, out PoleOI))
                        {
                            //ed.WriteMessage("NodeCode:{0}\n", NodeCode);
                            Atend.Base.Design.DPackage PolePack = Atend.Base.Design.DPackage.AccessSelectByNodeCode(new Guid(NodeCode));
                            if (PolePack.Code != Guid.Empty)
                            {
                                Atend.Base.Base.BEquipStatus ES = Atend.Base.Base.BEquipStatus.SelectByACode(PolePack.IsExistance);
                                if (ES.Name.IndexOf("موجود") != -1)
                                {
                                    ed.WriteMessage("mojood : {0} \n", PoleOI);
                                    ShieldForPole(PoleOI);
                                }

                            }
                            else
                            {
                                Atend.Global.Acad.Notification notification = new Atend.Global.Acad.Notification();
                                notification.Title = "بروز خطا";
                                notification.Msg = "اطلاعات یکی از پایه ها در پایگاه داده یافت نشد";
                                notification.infoCenterBalloon();
                                throw new System.Exception("pole was not in DPackage Post");

                            }
                        }
                    }


                    foreach (string NodeCode in MyDicCircle.Keys)
                    {
                        ObjectId PoleOI = ObjectId.Null;

                        if (MyDicCircle.TryGetValue(NodeCode, out PoleOI))
                        {
                            //ed.WriteMessage("NodeCode:{0}\n", NodeCode);
                            Atend.Base.Design.DPackage PolePack = Atend.Base.Design.DPackage.AccessSelectByNodeCode(new Guid(NodeCode));
                            if (PolePack.Code != Guid.Empty)
                            {
                                Atend.Base.Base.BEquipStatus ES = Atend.Base.Base.BEquipStatus.SelectByACode(PolePack.IsExistance);
                                if (ES.Name.IndexOf("موجود") != -1)
                                {
                                    ed.WriteMessage("mojood : {0} \n", PoleOI);
                                    ShieldForCirclePole(PoleOI, true);
                                }
                                else
                                {
                                    ShieldForCirclePole(PoleOI, false);
                                }

                            }
                            else
                            {
                                Atend.Global.Acad.Notification notification = new Atend.Global.Acad.Notification();
                                notification.Title = "بروز خطا";
                                notification.Msg = "اطلاعات یکی از پایه ها در پایگاه داده یافت نشد";
                                notification.infoCenterBalloon();
                                throw new System.Exception("pole was not in DPackage Post");

                            }
                        }
                    }


                    ed.WriteMessage("pole finished \n");
                }
            }
            catch (System.Exception ex)
            {
                ed.WriteMessage("ERROR IN POLE :{0} \n", ex.Message);
            }
        }

        public static void DrawShieldTip()
        {
            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            Dictionary<string, Point2dCollection> MyDic = new Dictionary<string, Point2dCollection>();
            Dictionary<string, ObjectId> MyDic1 = new Dictionary<string, ObjectId>();
            Dictionary<string, ObjectId> MyDicCircle = new Dictionary<string, ObjectId>();
            try
            {
                TypedValue[] tvs = new TypedValue[] { new TypedValue((int)DxfCode.LayerName, Atend.Control.Enum.AutoCadLayerName.GENERAL.ToString()) };
                SelectionFilter sf = new SelectionFilter(tvs);
                PromptSelectionResult psr = ed.SelectAll(sf);
                if (psr.Value != null)
                {
                    ObjectId[] ids = psr.Value.GetObjectIds();
                    //ObjectIdCollection OIC = new ObjectIdCollection();

                    foreach (Autodesk.AutoCAD.DatabaseServices.ObjectId oi in ids)
                    {
                        Atend.Base.Acad.AT_INFO PostInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(oi);
                        if (PostInfo.ParentCode != "NONE" && PostInfo.NodeType == (int)Atend.Control.Enum.ProductType.PoleTip)
                        {
                            Autodesk.AutoCAD.DatabaseServices.Entity ent = Atend.Global.Acad.UAcad.GetEntityByObjectID(oi);
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
                                else if (pl.NumberOfVertices == 7)
                                {
                                    pts = new Point2dCollection(7);
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
                            else
                            {
                                //circle pole
                                Circle plc = ent as Circle;
                                if (plc != null)
                                {
                                    MyDicCircle.Add(PostInfo.NodeCode, oi);
                                }
                            }
                        }
                        //ed.WriteMessage("--------------------------------------- \n");
                    }
                    ids = null;
                    foreach (string a in MyDic.Keys)
                    {
                        Point2dCollection p = new Point2dCollection();
                        MyDic.TryGetValue(a, out p);
                        Atend.Global.Acad.Global.CreateWhiteBack(p);
                    }

                    foreach (string NodeCode in MyDic1.Keys)
                    {
                        ObjectId PoleOI = ObjectId.Null;

                        if (MyDic1.TryGetValue(NodeCode, out PoleOI))
                        {
                            //ed.WriteMessage("NodeCode:{0}\n", NodeCode);
                            Atend.Base.Design.DPackage PolePack = Atend.Base.Design.DPackage.AccessSelectByNodeCode(new Guid(NodeCode));
                            if (PolePack.Code != Guid.Empty)
                            {
                                Atend.Base.Base.BEquipStatus ES = Atend.Base.Base.BEquipStatus.SelectByACode(PolePack.IsExistance);
                                if (ES.Name.IndexOf("موجود") != -1)
                                {
                                    ed.WriteMessage("mojood : {0} \n", PoleOI);
                                    ShieldForPole(PoleOI);
                                }

                            }
                            else
                            {
                                Atend.Global.Acad.Notification notification = new Atend.Global.Acad.Notification();
                                notification.Title = "بروز خطا";
                                notification.Msg = "اطلاعات یکی از پایه ها در پایگاه داده یافت نشد";
                                notification.infoCenterBalloon();
                                throw new System.Exception("pole was not in DPackage Post");

                            }
                        }
                    }


                    foreach (string NodeCode in MyDicCircle.Keys)
                    {
                        ObjectId PoleOI = ObjectId.Null;

                        if (MyDicCircle.TryGetValue(NodeCode, out PoleOI))
                        {
                            //ed.WriteMessage("NodeCode:{0}\n", NodeCode);
                            Atend.Base.Design.DPackage PolePack = Atend.Base.Design.DPackage.AccessSelectByNodeCode(new Guid(NodeCode));
                            if (PolePack.Code != Guid.Empty)
                            {
                                Atend.Base.Base.BEquipStatus ES = Atend.Base.Base.BEquipStatus.SelectByACode(PolePack.IsExistance);
                                if (ES.Name.IndexOf("موجود") != -1)
                                {
                                    ed.WriteMessage("mojood : {0} \n", PoleOI);
                                    ShieldForCirclePole(PoleOI, true);
                                }
                                else
                                {
                                    ShieldForCirclePole(PoleOI, false);
                                }

                            }
                            else
                            {
                                Atend.Global.Acad.Notification notification = new Atend.Global.Acad.Notification();
                                notification.Title = "بروز خطا";
                                notification.Msg = "اطلاعات یکی از پایه ها در پایگاه داده یافت نشد";
                                notification.infoCenterBalloon();
                                throw new System.Exception("pole was not in DPackage Post");

                            }
                        }
                    }


                    ed.WriteMessage("pole finished \n");
                }
            }
            catch (System.Exception ex)
            {
                ed.WriteMessage("ERROR IN POLE :{0} \n", ex.Message);
            }
        }

        private static void ShieldForPole(Autodesk.AutoCAD.DatabaseServices.ObjectId CurrentPoleOI)
        {
            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            //ed.WriteMessage("SHIELD \n");
            try
            {

                Transaction tr = doc.TransactionManager.StartTransaction();
                using (tr)
                {
                    // Check the entity is a closed curve
                    DBObject obj = tr.GetObject(CurrentPoleOI, OpenMode.ForRead);
                    Curve cur = obj as Curve;
                    if (cur != null && cur.Closed == false)
                    {
                        //ed.WriteMessage("\nLoop must be a closed curve.");
                    }
                    else
                    {
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
                        ObjectId hatId = btr.AppendEntity(hat);
                        tr.AddNewlyCreatedDBObject(hat, true);
                        // Add the hatch loop and complete the hatch
                        ObjectIdCollection ids = new ObjectIdCollection();
                        ids.Add(obj.ObjectId);
                        hat.Associative = true;
                        hat.AppendLoop(HatchLoopTypes.Default, ids);
                        hat.EvaluateHatch(true);
                        tr.Commit();
                    }
                }

            }
            catch (System.Exception ex)
            {
                ed.WriteMessage("ERROR IN POLE shield:{0} \n", ex.Message);
            }
        }

        private static void ShieldForCirclePole(Autodesk.AutoCAD.DatabaseServices.ObjectId CurrentPoleOI, bool IsExist)
        {
            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            //ed.WriteMessage("SHIELD \n");
            try
            {

                Transaction tr = doc.TransactionManager.StartTransaction();
                using (tr)
                {
                    // Check the entity is a closed curve
                    DBObject obj = tr.GetObject(CurrentPoleOI, OpenMode.ForRead);
                    Curve cur = obj as Curve;
                    if (cur != null && cur.Closed == false)
                    {
                        //ed.WriteMessage("\nLoop must be a closed curve.");
                    }
                    else
                    {
                        if (IsExist)
                        {
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
                            ObjectId hatId = btr.AppendEntity(hat);
                            tr.AddNewlyCreatedDBObject(hat, true);
                            // Add the hatch loop and complete the hatch
                            ObjectIdCollection ids = new ObjectIdCollection();
                            ids.Add(obj.ObjectId);
                            hat.Associative = true;
                            hat.AppendLoop(HatchLoopTypes.Default, ids);
                            hat.EvaluateHatch(true);
                            tr.Commit();
                        }
                        else
                        {
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
                            gcs[0] = new GradientColor(Color.FromRgb(255, 255, 255), 0);
                            // Second colour must have value of 1
                            gcs[1] = new GradientColor(Color.FromRgb(255, 255, 255), 1);
                            hat.SetGradientColors(gcs);
                            // Add the hatch to the model space
                            // and the transaction
                            ObjectId hatId = btr.AppendEntity(hat);
                            tr.AddNewlyCreatedDBObject(hat, true);
                            // Add the hatch loop and complete the hatch
                            ObjectIdCollection ids = new ObjectIdCollection();
                            ids.Add(obj.ObjectId);
                            hat.Associative = true;
                            hat.AppendLoop(HatchLoopTypes.Default, ids);
                            hat.EvaluateHatch(true);
                            tr.Commit();

                        }
                    }
                }

            }
            catch (System.Exception ex)
            {
                ed.WriteMessage("ERROR IN POLE shield:{0} \n", ex.Message);
            }
        }


    }
}
