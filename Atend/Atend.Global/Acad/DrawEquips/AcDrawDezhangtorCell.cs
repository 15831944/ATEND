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


namespace Atend.Global.Acad.DrawEquips
{


    public class AcDrawDezhangtorCell
    {

        //    private class DrawDezhangtorCellJig : DrawJig
        //    {
        //        List<Entity> Entities = new List<Entity>();
        //        Point3d CenterPoint = Point3d.Origin;
        //        Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
        //        int ProductCode = 0;
        //        string CodeGuid;
        //        double MyScale = 1;

        //        public DrawDezhangtorCellJig(int CurrentProductCode, Guid Code, double Scale)
        //        {
        //            MyScale = Scale;

        //            CodeGuid = Code.ToString();
        //            ProductCode = CurrentProductCode;
        //            //cell
        //            Entities.Add(CreateCellEntity(CenterPoint, 40, 70));

        //            //bus
        //            Entities.Add(CreateLine(new Point3d(CenterPoint.X - 20, CenterPoint.Y + 30, CenterPoint.Z),
        //                new Point3d(CenterPoint.X + 20, CenterPoint.Y + 30, CenterPoint.Z),
        //                (int)Atend.Control.Enum.ProductType.Bus,
        //                190,
        //                30));



        //            //additional line

        //            Entities.Add(CreateLine(new Point3d(CenterPoint.X, CenterPoint.Y + 30, CenterPoint.Z),
        //                new Point3d(CenterPoint.X, CenterPoint.Y + 25, CenterPoint.Z),
        //               0,
        //                190,
        //                30));
        //            Entities.Add(CreateLine(new Point3d(CenterPoint.X, CenterPoint.Y + 20, CenterPoint.Z),
        //                new Point3d(CenterPoint.X, CenterPoint.Y + 10, CenterPoint.Z),
        //               0,
        //                190,
        //                30));
        //            Entities.Add(CreateLine(new Point3d(CenterPoint.X, CenterPoint.Y + 5, CenterPoint.Z),
        //                new Point3d(CenterPoint.X, CenterPoint.Y - 25, CenterPoint.Z),
        //               0,
        //                190,
        //                30));



        //            Entities.Add(CreateLine(new Point3d(CenterPoint.X - 10, CenterPoint.Y + 15, CenterPoint.Z),
        //          new Point3d(CenterPoint.X + 10, CenterPoint.Y + 15, CenterPoint.Z),
        //         0,
        //          190,
        //          30));
        //            Entities.Add(CreateLine(new Point3d(CenterPoint.X + 10, CenterPoint.Y + 15, CenterPoint.Z),
        //                new Point3d(CenterPoint.X + 10, CenterPoint.Y, CenterPoint.Z),
        //               0,
        //                190,
        //                30));
        //            Entities.Add(CreateLine(new Point3d(CenterPoint.X + 10, CenterPoint.Y, CenterPoint.Z),
        //                new Point3d(CenterPoint.X - 10, CenterPoint.Y, CenterPoint.Z),
        //               0,
        //                190,
        //                30));
        //            Entities.Add(CreateLine(new Point3d(CenterPoint.X - 10, CenterPoint.Y, CenterPoint.Z),
        //                new Point3d(CenterPoint.X - 10, CenterPoint.Y + 15, CenterPoint.Z),
        //               0,
        //                190,
        //                30));


        //            //ground
        //            Entities.Add(CreateLine(new Point3d(CenterPoint.X, CenterPoint.Y - 10, CenterPoint.Z),
        //                new Point3d(CenterPoint.X - 5, CenterPoint.Y - 10, CenterPoint.Z),
        //               0,
        //                190,
        //                30));
        //            Entities.Add(CreateLine(new Point3d(CenterPoint.X - 10, CenterPoint.Y - 10, CenterPoint.Z),
        //                new Point3d(CenterPoint.X - 15, CenterPoint.Y - 10, CenterPoint.Z),
        //               0,
        //                190,
        //                30));
        //            Entities.Add(CreateLine(new Point3d(CenterPoint.X - 15, CenterPoint.Y - 10, CenterPoint.Z),
        //                new Point3d(CenterPoint.X - 15, CenterPoint.Y - 15, CenterPoint.Z),
        //               0,
        //                190,
        //                30));
        //            Entities.Add(CreateLine(new Point3d(CenterPoint.X - 18, CenterPoint.Y - 15, CenterPoint.Z),
        //                new Point3d(CenterPoint.X - 12, CenterPoint.Y - 15, CenterPoint.Z),
        //               0,
        //                190,
        //                30));
        //            Entities.Add(CreateLine(new Point3d(CenterPoint.X - 17, CenterPoint.Y - 16, CenterPoint.Z),
        //                new Point3d(CenterPoint.X - 13, CenterPoint.Y - 16, CenterPoint.Z),
        //               0,
        //                190,
        //                30));
        //            Entities.Add(CreateLine(new Point3d(CenterPoint.X - 16, CenterPoint.Y - 17, CenterPoint.Z),
        //                new Point3d(CenterPoint.X - 14, CenterPoint.Y - 17, CenterPoint.Z),
        //               0,
        //                190,
        //                30));


        //            //keys
        //            Entities.Add(CreateLine(new Point3d(CenterPoint.X - 5, CenterPoint.Y + 25, CenterPoint.Z),
        //                new Point3d(CenterPoint.X, CenterPoint.Y + 20, CenterPoint.Z),
        //               0,
        //                190,
        //                30));
        //            Entities.Add(CreateLine(new Point3d(CenterPoint.X - 5, CenterPoint.Y + 10, CenterPoint.Z),
        //                new Point3d(CenterPoint.X, CenterPoint.Y + 5, CenterPoint.Z),
        //               0,
        //                190,
        //                30));
        //            Entities.Add(CreateLine(new Point3d(CenterPoint.X - 5, CenterPoint.Y - 10, CenterPoint.Z),
        //                new Point3d(CenterPoint.X - 10, CenterPoint.Y - 5, CenterPoint.Z),
        //               0,
        //                190,
        //                30));


        //            //header cable
        //            Point3dCollection Points = new Point3dCollection();
        //            Points.Add(new Point3d(CenterPoint.X, CenterPoint.Y - 30, CenterPoint.Z));
        //            Points.Add(new Point3d(CenterPoint.X - 5, CenterPoint.Y - 25, CenterPoint.Z));
        //            Points.Add(new Point3d(CenterPoint.X + 5, CenterPoint.Y - 25, CenterPoint.Z));
        //            Points.Add(new Point3d(CenterPoint.X, CenterPoint.Y - 30, CenterPoint.Z));
        //            Entities.Add(CreateHeaderCable(Points));
        //        }


        //        private Entity CreateLine(Point3d StartPoint, Point3d EndPoint, int ProductType, int ColorIndex, double Thickness)
        //        {
        //            Atend.Global.Acad.AcadJigs.MyLine mLine = new Atend.Global.Acad.AcadJigs.MyLine();
        //            mLine.StartPoint = StartPoint;
        //            mLine.EndPoint = EndPoint;

        //            if (Thickness != 0)
        //            {
        //                mLine.Thickness = Thickness;
        //            }


        //            //if (ProductType != 0)
        //            //{
        //            Atend.Global.Acad.AcadJigs.SaveExtensionData(mLine, ProductType);
        //            Atend.Global.Acad.AcadJigs.SaveExtensionData(mLine, CodeGuid);
        //            //}
        //            //if (ColorIndex != 0)
        //            //{
        //            mLine.ColorIndex = ColorIndex;
        //            //}

        //            return mLine;
        //        }

        //        private Entity CreateConnectionPoint(Point3d CenterPoint, double Radius)
        //        {
        //            Atend.Global.Acad.AcadJigs.MyCircle c = new Atend.Global.Acad.AcadJigs.MyCircle();

        //            c.Center = CenterPoint;

        //            c.Normal = new Vector3d(0, 0, 1);

        //            c.Radius = Radius;

        //            c.ColorIndex = 3;

        //            Atend.Global.Acad.AcadJigs.SaveExtensionData(c, (int)Atend.Control.Enum.ProductType.ConnectionPoint);
        //            Atend.Global.Acad.AcadJigs.SaveExtensionData(c, CodeGuid);
        //            return c;
        //        }

        //        private Entity CreateCellEntity(Point3d CenterPoint, double Width, double Height)
        //        {

        //            double BaseX = CenterPoint.X;
        //            double BaseY = CenterPoint.Y;

        //            Atend.Global.Acad.AcadJigs.MyPolyLine pLine = new Atend.Global.Acad.AcadJigs.MyPolyLine();
        //            pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX - (Width / 2), BaseY - (Height / 2)), 0, 0, 0);
        //            pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX + (Width / 2), BaseY - (Height / 2)), 0, 0, 0);
        //            pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX + (Width / 2), BaseY + (Height / 2)), 0, 0, 0);
        //            pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX - (Width / 2), BaseY + (Height / 2)), 0, 0, 0);
        //            pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(BaseX - (Width / 2), BaseY - (Height / 2)), 0, 0, 0);
        //            pLine.Closed = true;


        //            Atend.Global.Acad.AcadJigs.SaveExtensionData(pLine, (int)Atend.Control.Enum.ProductType.Cell);
        //            Atend.Global.Acad.AcadJigs.SaveExtensionData(pLine, (long)ProductCode);
        //            Atend.Global.Acad.AcadJigs.SaveExtensionData(pLine, CodeGuid);
        //            return pLine;

        //        }

        //        private Entity CreateHeaderCable(Point3dCollection P3C)
        //        {

        //            Atend.Global.Acad.AcadJigs.MyPolyLine pLine = new Atend.Global.Acad.AcadJigs.MyPolyLine();
        //            pLine.ColorIndex = 40;
        //            pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(P3C[0].X, P3C[0].Y), 0, 0, 0);
        //            pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(P3C[1].X, P3C[1].Y), 0, 0, 0);
        //            pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(P3C[2].X, P3C[2].Y), 0, 0, 0);
        //            pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(P3C[0].X, P3C[0].Y), 0, 0, 0);

        //            Atend.Global.Acad.AcadJigs.SaveExtensionData(pLine, (int)Atend.Control.Enum.ProductType.HeaderCabel);
        //            Atend.Global.Acad.AcadJigs.SaveExtensionData(pLine, CodeGuid);
        //            pLine.Closed = true;

        //            return pLine;

        //        }

        //        protected override SamplerStatus Sampler(JigPrompts prompts)
        //        {
        //            //throw new System.Exception("The method or operation is not implemented.");

        //            JigPromptPointOptions ppo = new JigPromptPointOptions("\nCell Position:");

        //            PromptPointResult ppr = prompts.AcquirePoint(ppo);

        //            if (ppr.Status == PromptStatus.OK)
        //            {

        //                if (ppr.Value == CenterPoint)
        //                {
        //                    return SamplerStatus.NoChange;
        //                }
        //                else
        //                {
        //                    CenterPoint = ppr.Value;
        //                    return SamplerStatus.OK;
        //                }
        //            }
        //            else
        //            {
        //                return SamplerStatus.Cancel;
        //            }



        //        }

        //        protected override bool WorldDraw(Autodesk.AutoCAD.GraphicsInterface.WorldDraw draw)
        //        {
        //            //throw new System.Exception("The method or operation is not implemented.");
        //            Entities.Clear();

        //            //cell
        //            Entities.Add(CreateCellEntity(CenterPoint, 40, 70));

        //            //bus
        //            Entities.Add(CreateLine(new Point3d(CenterPoint.X - 20, CenterPoint.Y + 30, CenterPoint.Z),
        //                new Point3d(CenterPoint.X + 20, CenterPoint.Y + 30, CenterPoint.Z),
        //                (int)Atend.Control.Enum.ProductType.Bus,
        //                190,
        //                30));



        //            //additional line

        //            Entities.Add(CreateLine(new Point3d(CenterPoint.X, CenterPoint.Y + 30, CenterPoint.Z),
        //                new Point3d(CenterPoint.X, CenterPoint.Y + 25, CenterPoint.Z),
        //               0,
        //                0,
        //                30));
        //            Entities.Add(CreateLine(new Point3d(CenterPoint.X, CenterPoint.Y + 20, CenterPoint.Z),
        //                new Point3d(CenterPoint.X, CenterPoint.Y + 10, CenterPoint.Z),
        //               0,
        //                0,
        //                30));
        //            Entities.Add(CreateLine(new Point3d(CenterPoint.X, CenterPoint.Y + 5, CenterPoint.Z),
        //                new Point3d(CenterPoint.X, CenterPoint.Y - 25, CenterPoint.Z),
        //               0,
        //                0,
        //                30));



        //            Entities.Add(CreateLine(new Point3d(CenterPoint.X - 10, CenterPoint.Y + 15, CenterPoint.Z),
        //          new Point3d(CenterPoint.X + 10, CenterPoint.Y + 15, CenterPoint.Z),
        //         0,
        //          0,
        //          30));
        //            Entities.Add(CreateLine(new Point3d(CenterPoint.X + 10, CenterPoint.Y + 15, CenterPoint.Z),
        //                new Point3d(CenterPoint.X + 10, CenterPoint.Y, CenterPoint.Z),
        //               0,
        //                0,
        //                30));
        //            Entities.Add(CreateLine(new Point3d(CenterPoint.X + 10, CenterPoint.Y, CenterPoint.Z),
        //                new Point3d(CenterPoint.X - 10, CenterPoint.Y, CenterPoint.Z),
        //               0,
        //                0,
        //                30));
        //            Entities.Add(CreateLine(new Point3d(CenterPoint.X - 10, CenterPoint.Y, CenterPoint.Z),
        //                new Point3d(CenterPoint.X - 10, CenterPoint.Y + 15, CenterPoint.Z),
        //               0,
        //                0,
        //                30));


        //            //ground
        //            Entities.Add(CreateLine(new Point3d(CenterPoint.X, CenterPoint.Y - 10, CenterPoint.Z),
        //                new Point3d(CenterPoint.X - 5, CenterPoint.Y - 10, CenterPoint.Z),
        //               0,
        //                0,
        //                30));
        //            Entities.Add(CreateLine(new Point3d(CenterPoint.X - 10, CenterPoint.Y - 10, CenterPoint.Z),
        //                new Point3d(CenterPoint.X - 15, CenterPoint.Y - 10, CenterPoint.Z),
        //               0,
        //                0,
        //                30));
        //            Entities.Add(CreateLine(new Point3d(CenterPoint.X - 15, CenterPoint.Y - 10, CenterPoint.Z),
        //                new Point3d(CenterPoint.X - 15, CenterPoint.Y - 15, CenterPoint.Z),
        //               0,
        //                0,
        //                30));
        //            Entities.Add(CreateLine(new Point3d(CenterPoint.X - 18, CenterPoint.Y - 15, CenterPoint.Z),
        //                new Point3d(CenterPoint.X - 12, CenterPoint.Y - 15, CenterPoint.Z),
        //               0,
        //                0,
        //                30));
        //            Entities.Add(CreateLine(new Point3d(CenterPoint.X - 17, CenterPoint.Y - 16, CenterPoint.Z),
        //                new Point3d(CenterPoint.X - 13, CenterPoint.Y - 16, CenterPoint.Z),
        //               0,
        //                0,
        //                30));
        //            Entities.Add(CreateLine(new Point3d(CenterPoint.X - 16, CenterPoint.Y - 17, CenterPoint.Z),
        //                new Point3d(CenterPoint.X - 14, CenterPoint.Y - 17, CenterPoint.Z),
        //               0,
        //                0,
        //                30));


        //            //keys
        //            //////Entities.Add(CreateLine(new Point3d(CenterPoint.X - 5, CenterPoint.Y + 25, CenterPoint.Z),
        //            //////    new Point3d(CenterPoint.X, CenterPoint.Y + 20, CenterPoint.Z),
        //            //////   0,
        //            //////    150,
        //            //////    30));
        //            //////Entities.Add(CreateLine(new Point3d(CenterPoint.X - 5, CenterPoint.Y + 10, CenterPoint.Z),
        //            //////    new Point3d(CenterPoint.X, CenterPoint.Y + 5, CenterPoint.Z),
        //            //////   0,
        //            //////    150,
        //            //////    30));
        //            //////Entities.Add(CreateLine(new Point3d(CenterPoint.X - 5, CenterPoint.Y - 10, CenterPoint.Z),
        //            //////    new Point3d(CenterPoint.X - 10, CenterPoint.Y - 5, CenterPoint.Z),
        //            //////   0,
        //            //////    150,
        //            //////    30));


        //            Entities.Add(CreateLine(new Point3d(CenterPoint.X, CenterPoint.Y + 20, CenterPoint.Z),
        //new Point3d(CenterPoint.X - 5, CenterPoint.Y + 25, CenterPoint.Z),
        //0,
        //150,
        //30));
        //            Entities.Add(CreateLine(new Point3d(CenterPoint.X, CenterPoint.Y + 5, CenterPoint.Z),
        //                new Point3d(CenterPoint.X - 5, CenterPoint.Y + 10, CenterPoint.Z),
        //               0,
        //                150,
        //                30));
        //            Entities.Add(CreateLine(new Point3d(CenterPoint.X - 5, CenterPoint.Y - 10, CenterPoint.Z),
        //                new Point3d(CenterPoint.X - 10, CenterPoint.Y - 5, CenterPoint.Z),
        //               0,
        //                150,
        //                30));



        //            //header cable
        //            Point3dCollection Points = new Point3dCollection();
        //            Points.Add(new Point3d(CenterPoint.X, CenterPoint.Y - 30, CenterPoint.Z));
        //            Points.Add(new Point3d(CenterPoint.X - 5, CenterPoint.Y - 25, CenterPoint.Z));
        //            Points.Add(new Point3d(CenterPoint.X + 5, CenterPoint.Y - 25, CenterPoint.Z));
        //            Points.Add(new Point3d(CenterPoint.X, CenterPoint.Y - 30, CenterPoint.Z));
        //            Entities.Add(CreateHeaderCable(Points));


        //            foreach (Entity ent in Entities)
        //            {
        //                draw.Geometry.Draw(ent);
        //            }

        //            return true;

        //        }

        //        public List<Entity> GetEntities()
        //        {
        //            return Entities;
        //        }

        //        public List<Entity> __GetDemo(Point3d _CenterPoint)
        //        {
        //            Entities.Clear();
        //            //cell
        //            Entities.Add(CreateCellEntity(_CenterPoint, 40, 70));

        //            //bus
        //            Entities.Add(CreateLine(new Point3d(_CenterPoint.X - 20, _CenterPoint.Y + 30, _CenterPoint.Z),
        //                new Point3d(_CenterPoint.X + 20, _CenterPoint.Y + 30, _CenterPoint.Z),
        //                (int)Atend.Control.Enum.ProductType.Bus,
        //                190,
        //                30));



        //            //additional line

        //            Entities.Add(CreateLine(new Point3d(_CenterPoint.X, _CenterPoint.Y + 30, _CenterPoint.Z),
        //                new Point3d(_CenterPoint.X, _CenterPoint.Y + 25, _CenterPoint.Z),
        //               0,
        //                0,
        //                30));
        //            Entities.Add(CreateLine(new Point3d(_CenterPoint.X, _CenterPoint.Y + 20, _CenterPoint.Z),
        //                new Point3d(_CenterPoint.X, _CenterPoint.Y + 10, _CenterPoint.Z),
        //               0,
        //                0,
        //                30));
        //            Entities.Add(CreateLine(new Point3d(_CenterPoint.X, _CenterPoint.Y + 5, _CenterPoint.Z),
        //                new Point3d(_CenterPoint.X, _CenterPoint.Y - 25, _CenterPoint.Z),
        //               0,
        //                0,
        //                30));



        //            Entities.Add(CreateLine(new Point3d(_CenterPoint.X - 10, _CenterPoint.Y + 15, _CenterPoint.Z),
        //          new Point3d(_CenterPoint.X + 10, _CenterPoint.Y + 15, _CenterPoint.Z),
        //         0,
        //          0,
        //          30));
        //            Entities.Add(CreateLine(new Point3d(_CenterPoint.X + 10, _CenterPoint.Y + 15, _CenterPoint.Z),
        //                new Point3d(_CenterPoint.X + 10, _CenterPoint.Y, _CenterPoint.Z),
        //               0,
        //                0,
        //                30));
        //            Entities.Add(CreateLine(new Point3d(_CenterPoint.X + 10, _CenterPoint.Y, _CenterPoint.Z),
        //                new Point3d(_CenterPoint.X - 10, _CenterPoint.Y, _CenterPoint.Z),
        //               0,
        //                0,
        //                30));
        //            Entities.Add(CreateLine(new Point3d(_CenterPoint.X - 10, _CenterPoint.Y, _CenterPoint.Z),
        //                new Point3d(_CenterPoint.X - 10, _CenterPoint.Y + 15, _CenterPoint.Z),
        //               0,
        //                0,
        //                30));


        //            //ground
        //            Entities.Add(CreateLine(new Point3d(_CenterPoint.X, _CenterPoint.Y - 10, _CenterPoint.Z),
        //                new Point3d(_CenterPoint.X - 5, _CenterPoint.Y - 10, _CenterPoint.Z),
        //               0,
        //                0,
        //                30));
        //            Entities.Add(CreateLine(new Point3d(_CenterPoint.X - 10, _CenterPoint.Y - 10, _CenterPoint.Z),
        //                new Point3d(_CenterPoint.X - 15, _CenterPoint.Y - 10, _CenterPoint.Z),
        //               0,
        //                0,
        //                30));
        //            Entities.Add(CreateLine(new Point3d(_CenterPoint.X - 15, _CenterPoint.Y - 10, _CenterPoint.Z),
        //                new Point3d(_CenterPoint.X - 15, _CenterPoint.Y - 15, _CenterPoint.Z),
        //               0,
        //                0,
        //                30));
        //            Entities.Add(CreateLine(new Point3d(_CenterPoint.X - 18, _CenterPoint.Y - 15, _CenterPoint.Z),
        //                new Point3d(_CenterPoint.X - 12, _CenterPoint.Y - 15, _CenterPoint.Z),
        //               0,
        //                0,
        //                30));
        //            Entities.Add(CreateLine(new Point3d(_CenterPoint.X - 17, _CenterPoint.Y - 16, _CenterPoint.Z),
        //                new Point3d(_CenterPoint.X - 13, _CenterPoint.Y - 16, _CenterPoint.Z),
        //               0,
        //                0,
        //                30));
        //            Entities.Add(CreateLine(new Point3d(_CenterPoint.X - 16, _CenterPoint.Y - 17, _CenterPoint.Z),
        //                new Point3d(_CenterPoint.X - 14, _CenterPoint.Y - 17, _CenterPoint.Z),
        //               0,
        //                0,
        //                30));


        //            //keys
        //            //////Entities.Add(CreateLine(new Point3d(_CenterPoint.X - 5, _CenterPoint.Y + 25, _CenterPoint.Z),
        //            //////    new Point3d(_CenterPoint.X, _CenterPoint.Y + 20, _CenterPoint.Z),
        //            //////   0,
        //            //////    150,
        //            //////    30));
        //            //////Entities.Add(CreateLine(new Point3d(_CenterPoint.X - 5, _CenterPoint.Y + 10, _CenterPoint.Z),
        //            //////    new Point3d(_CenterPoint.X, _CenterPoint.Y + 5, _CenterPoint.Z),
        //            //////   0,
        //            //////    150,
        //            //////    30));
        //            //////Entities.Add(CreateLine(new Point3d(_CenterPoint.X - 5, _CenterPoint.Y - 10, _CenterPoint.Z),
        //            //////    new Point3d(_CenterPoint.X - 10, _CenterPoint.Y - 5, _CenterPoint.Z),
        //            //////   0,
        //            //////    150,
        //            //////    30));
        //            Entities.Add(CreateLine(new Point3d(_CenterPoint.X, _CenterPoint.Y + 20, _CenterPoint.Z),
        //new Point3d(_CenterPoint.X - 5, _CenterPoint.Y + 25, _CenterPoint.Z),
        //(int)Atend.Control.Enum.ProductType.KeyElse,
        //150,
        //30));
        //            Entities.Add(CreateLine(new Point3d(_CenterPoint.X, _CenterPoint.Y + 5, _CenterPoint.Z),
        //                new Point3d(_CenterPoint.X - 5, _CenterPoint.Y + 10, _CenterPoint.Z),
        //               (int)Atend.Control.Enum.ProductType.Key,
        //                150,
        //                30));
        //            Entities.Add(CreateLine(new Point3d(_CenterPoint.X - 5, _CenterPoint.Y - 10, _CenterPoint.Z),
        //                new Point3d(_CenterPoint.X - 10, _CenterPoint.Y - 5, _CenterPoint.Z),
        //               (int)Atend.Control.Enum.ProductType.KeyElse,
        //                150,
        //                30));

        //            //header cable
        //            Point3dCollection Points = new Point3dCollection();
        //            Points.Add(new Point3d(_CenterPoint.X, _CenterPoint.Y - 30, _CenterPoint.Z));
        //            Points.Add(new Point3d(_CenterPoint.X - 5, _CenterPoint.Y - 25, _CenterPoint.Z));
        //            Points.Add(new Point3d(_CenterPoint.X + 5, _CenterPoint.Y - 25, _CenterPoint.Z));
        //            Points.Add(new Point3d(_CenterPoint.X, _CenterPoint.Y - 30, _CenterPoint.Z));
        //            Entities.Add(CreateHeaderCable(Points));

        //            //Matrix3d trans1 = Matrix3d.Scaling(MyScale, new Point3d(CenterPoint.X, CenterPoint.Y, 0));
        //            //foreach (Entity en in Entities)
        //            //{
        //            //    en.TransformBy(trans1);
        //            //}


        //            return Entities;
        //        }

        //    }


        public class DrawDezhangtorCellJig02 : DrawJig
        {
            List<Entity> Entities = new List<Entity>();
            Point3d CenterPoint = Point3d.Origin;
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            int ProductCode = 0;
            string CodeGuid;
            double MyScale = 1;

            public DrawDezhangtorCellJig02(int CurrentProductCode, Guid Code, double Scale)
            {
                MyScale = Scale;

                CodeGuid = Code.ToString();
                ProductCode = CurrentProductCode;
                ////////   //cell
                ////////   Entities.Add(CreateCellEntity(CenterPoint, 40, 70));

                ////////   //bus
                ////////   Entities.Add(CreateLine(new Point3d(CenterPoint.X - 20, CenterPoint.Y + 30, CenterPoint.Z),
                ////////       new Point3d(CenterPoint.X + 20, CenterPoint.Y + 30, CenterPoint.Z),
                ////////       (int)Atend.Control.Enum.ProductType.Bus,
                ////////       190,
                ////////       30));



                ////////   //additional line

                ////////   Entities.Add(CreateLine(new Point3d(CenterPoint.X, CenterPoint.Y + 30, CenterPoint.Z),
                ////////       new Point3d(CenterPoint.X, CenterPoint.Y + 25, CenterPoint.Z),
                ////////      0,
                ////////       190,
                ////////       30));
                ////////   Entities.Add(CreateLine(new Point3d(CenterPoint.X, CenterPoint.Y + 20, CenterPoint.Z),
                ////////       new Point3d(CenterPoint.X, CenterPoint.Y + 10, CenterPoint.Z),
                ////////      0,
                ////////       190,
                ////////       30));
                ////////   Entities.Add(CreateLine(new Point3d(CenterPoint.X, CenterPoint.Y + 5, CenterPoint.Z),
                ////////       new Point3d(CenterPoint.X, CenterPoint.Y - 25, CenterPoint.Z),
                ////////      0,
                ////////       190,
                ////////       30));



                ////////   Entities.Add(CreateLine(new Point3d(CenterPoint.X - 10, CenterPoint.Y + 15, CenterPoint.Z),
                //////// new Point3d(CenterPoint.X + 10, CenterPoint.Y + 15, CenterPoint.Z),
                ////////0,
                //////// 190,
                //////// 30));
                ////////   Entities.Add(CreateLine(new Point3d(CenterPoint.X + 10, CenterPoint.Y + 15, CenterPoint.Z),
                ////////       new Point3d(CenterPoint.X + 10, CenterPoint.Y, CenterPoint.Z),
                ////////      0,
                ////////       190,
                ////////       30));
                ////////   Entities.Add(CreateLine(new Point3d(CenterPoint.X + 10, CenterPoint.Y, CenterPoint.Z),
                ////////       new Point3d(CenterPoint.X - 10, CenterPoint.Y, CenterPoint.Z),
                ////////      0,
                ////////       190,
                ////////       30));
                ////////   Entities.Add(CreateLine(new Point3d(CenterPoint.X - 10, CenterPoint.Y, CenterPoint.Z),
                ////////       new Point3d(CenterPoint.X - 10, CenterPoint.Y + 15, CenterPoint.Z),
                ////////      0,
                ////////       190,
                ////////       30));


                ////////   //ground
                ////////   Entities.Add(CreateLine(new Point3d(CenterPoint.X, CenterPoint.Y - 10, CenterPoint.Z),
                ////////       new Point3d(CenterPoint.X - 5, CenterPoint.Y - 10, CenterPoint.Z),
                ////////      0,
                ////////       190,
                ////////       30));
                ////////   Entities.Add(CreateLine(new Point3d(CenterPoint.X - 10, CenterPoint.Y - 10, CenterPoint.Z),
                ////////       new Point3d(CenterPoint.X - 15, CenterPoint.Y - 10, CenterPoint.Z),
                ////////      0,
                ////////       190,
                ////////       30));
                ////////   Entities.Add(CreateLine(new Point3d(CenterPoint.X - 15, CenterPoint.Y - 10, CenterPoint.Z),
                ////////       new Point3d(CenterPoint.X - 15, CenterPoint.Y - 15, CenterPoint.Z),
                ////////      0,
                ////////       190,
                ////////       30));
                ////////   Entities.Add(CreateLine(new Point3d(CenterPoint.X - 18, CenterPoint.Y - 15, CenterPoint.Z),
                ////////       new Point3d(CenterPoint.X - 12, CenterPoint.Y - 15, CenterPoint.Z),
                ////////      0,
                ////////       190,
                ////////       30));
                ////////   Entities.Add(CreateLine(new Point3d(CenterPoint.X - 17, CenterPoint.Y - 16, CenterPoint.Z),
                ////////       new Point3d(CenterPoint.X - 13, CenterPoint.Y - 16, CenterPoint.Z),
                ////////      0,
                ////////       190,
                ////////       30));
                ////////   Entities.Add(CreateLine(new Point3d(CenterPoint.X - 16, CenterPoint.Y - 17, CenterPoint.Z),
                ////////       new Point3d(CenterPoint.X - 14, CenterPoint.Y - 17, CenterPoint.Z),
                ////////      0,
                ////////       190,
                ////////       30));


                ////////   //keys
                ////////   Entities.Add(CreateLine(new Point3d(CenterPoint.X - 5, CenterPoint.Y + 25, CenterPoint.Z),
                ////////       new Point3d(CenterPoint.X, CenterPoint.Y + 20, CenterPoint.Z),
                ////////      0,
                ////////       190,
                ////////       30));
                ////////   Entities.Add(CreateLine(new Point3d(CenterPoint.X - 5, CenterPoint.Y + 10, CenterPoint.Z),
                ////////       new Point3d(CenterPoint.X, CenterPoint.Y + 5, CenterPoint.Z),
                ////////      0,
                ////////       190,
                ////////       30));
                ////////   Entities.Add(CreateLine(new Point3d(CenterPoint.X - 5, CenterPoint.Y - 10, CenterPoint.Z),
                ////////       new Point3d(CenterPoint.X - 10, CenterPoint.Y - 5, CenterPoint.Z),
                ////////      0,
                ////////       190,
                ////////       30));


                ////////   //header cable
                ////////   Point3dCollection Points = new Point3dCollection();
                ////////   Points.Add(new Point3d(CenterPoint.X, CenterPoint.Y - 30, CenterPoint.Z));
                ////////   Points.Add(new Point3d(CenterPoint.X - 5, CenterPoint.Y - 25, CenterPoint.Z));
                ////////   Points.Add(new Point3d(CenterPoint.X + 5, CenterPoint.Y - 25, CenterPoint.Z));
                ////////   Points.Add(new Point3d(CenterPoint.X, CenterPoint.Y - 30, CenterPoint.Z));
                ////////   Entities.Add(CreateHeaderCable(Points));
            }

            private Entity CreateLine(Point3d StartPoint, Point3d EndPoint, int ProductType, int ColorIndex, double Thickness)
            {
                Atend.Global.Acad.AcadJigs.MyLine mLine = new Atend.Global.Acad.AcadJigs.MyLine();
                mLine.StartPoint = StartPoint;
                mLine.EndPoint = EndPoint;

                if (Thickness != 0)
                {
                    mLine.Thickness = Thickness;
                }


                //if (ProductType != 0)
                //{
                Atend.Global.Acad.AcadJigs.SaveExtensionData(mLine, ProductType);
                Atend.Global.Acad.AcadJigs.SaveExtensionData(mLine, CodeGuid);
                //}
                //if (ColorIndex != 0)
                //{
                mLine.ColorIndex = ColorIndex;
                //}

                return mLine;
            }

            //private Entity CreateConnectionPoint(Point3d CenterPoint, double Radius)
            //{
            //    Atend.Global.Acad.AcadJigs.MyCircle c = new Atend.Global.Acad.AcadJigs.MyCircle();

            //    c.Center = CenterPoint;

            //    c.Normal = new Vector3d(0, 0, 1);

            //    c.Radius = Radius;

            //    c.ColorIndex = 3;

            //    Atend.Global.Acad.AcadJigs.SaveExtensionData(c, (int)Atend.Control.Enum.ProductType.ConnectionPoint);
            //    Atend.Global.Acad.AcadJigs.SaveExtensionData(c, CodeGuid);
            //    return c;
            //}

            private Entity CreateCellEntity(Point3d CenterPoint, double Width, double Height)
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


                Atend.Global.Acad.AcadJigs.SaveExtensionData(pLine, (int)Atend.Control.Enum.ProductType.Cell);
                Atend.Global.Acad.AcadJigs.SaveExtensionData(pLine, (long)ProductCode);
                Atend.Global.Acad.AcadJigs.SaveExtensionData(pLine, CodeGuid);
                return pLine;

            }

            private Entity CreateHeaderCable(Point3dCollection P3C)
            {

                Atend.Global.Acad.AcadJigs.MyPolyLine pLine = new Atend.Global.Acad.AcadJigs.MyPolyLine();
                pLine.ColorIndex = 40;
                pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(P3C[0].X, P3C[0].Y), 0, 0, 0);
                pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(P3C[1].X, P3C[1].Y), 0, 0, 0);
                pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(P3C[2].X, P3C[2].Y), 0, 0, 0);
                pLine.AddVertexAt(pLine.NumberOfVertices, new Point2d(P3C[0].X, P3C[0].Y), 0, 0, 0);

                Atend.Global.Acad.AcadJigs.SaveExtensionData(pLine, (int)Atend.Control.Enum.ProductType.HeaderCabel);
                Atend.Global.Acad.AcadJigs.SaveExtensionData(pLine, CodeGuid);
                pLine.Closed = true;

                return pLine;

            }

            private Entity CreateBus(Point3d BasePoint, int ColorIndex)
            {
                Atend.Global.Acad.AcadJigs.MyPolyLine BusEntity = new AcadJigs.MyPolyLine();
                BusEntity.AddVertexAt(BusEntity.NumberOfVertices, new Point2d(BasePoint.X - 17.5, BasePoint.Y + 32.5), 0, 0, 0);
                BusEntity.AddVertexAt(BusEntity.NumberOfVertices, new Point2d(BasePoint.X + 17.5, BasePoint.Y + 32.5), 0, 0, 0);
                BusEntity.AddVertexAt(BusEntity.NumberOfVertices, new Point2d(BasePoint.X + 17.5, BasePoint.Y + 27.5), 0, 0, 0);
                BusEntity.AddVertexAt(BusEntity.NumberOfVertices, new Point2d(BasePoint.X - 17.5, BasePoint.Y + 27.5), 0, 0, 0);
                BusEntity.AddVertexAt(BusEntity.NumberOfVertices, new Point2d(BasePoint.X - 17.5, BasePoint.Y + 32.5), 0, 0, 0);
                BusEntity.ColorIndex = ColorIndex;

                Atend.Global.Acad.AcadJigs.SaveExtensionData(BusEntity, (int)Atend.Control.Enum.ProductType.Bus);
                Atend.Global.Acad.AcadJigs.SaveExtensionData(BusEntity, (long)ProductCode);
                Atend.Global.Acad.AcadJigs.SaveExtensionData(BusEntity, CodeGuid);
                return BusEntity;
            }

            protected override SamplerStatus Sampler(JigPrompts prompts)
            {
                //throw new System.Exception("The method or operation is not implemented.");

                JigPromptPointOptions ppo = new JigPromptPointOptions("\nCell Position:");

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
                //throw new System.Exception("The method or operation is not implemented.");
                Entities.Clear();
                return true;

            }

            public List<Entity> GetEntities()
            {
                return Entities;
            }

            public List<Entity> GetDemo(Point3d _CenterPoint)
            {
                Entities.Clear();
                //cell
                Entities.Add(CreateCellEntity(_CenterPoint, 40, 70));

                //bus
                Entities.Add(CreateBus(_CenterPoint, 190));

                //additional line
                Entities.Add(CreateLine(new Point3d(_CenterPoint.X, _CenterPoint.Y + 30, _CenterPoint.Z), new Point3d(_CenterPoint.X, _CenterPoint.Y + 25, _CenterPoint.Z), 0, 0, 30));
                Entities.Add(CreateLine(new Point3d(_CenterPoint.X, _CenterPoint.Y + 20, _CenterPoint.Z), new Point3d(_CenterPoint.X, _CenterPoint.Y + 10, _CenterPoint.Z), 0, 0, 30));
                Entities.Add(CreateLine(new Point3d(_CenterPoint.X, _CenterPoint.Y + 5, _CenterPoint.Z), new Point3d(_CenterPoint.X, _CenterPoint.Y - 25, _CenterPoint.Z), 0, 0, 30));

                Entities.Add(CreateLine(new Point3d(_CenterPoint.X - 10, _CenterPoint.Y + 15, _CenterPoint.Z), new Point3d(_CenterPoint.X + 10, _CenterPoint.Y + 15, _CenterPoint.Z), 0, 0, 30));
                Entities.Add(CreateLine(new Point3d(_CenterPoint.X + 10, _CenterPoint.Y + 15, _CenterPoint.Z), new Point3d(_CenterPoint.X + 10, _CenterPoint.Y, _CenterPoint.Z), 0, 0, 30));
                Entities.Add(CreateLine(new Point3d(_CenterPoint.X + 10, _CenterPoint.Y, _CenterPoint.Z), new Point3d(_CenterPoint.X - 10, _CenterPoint.Y, _CenterPoint.Z), 0, 0, 30));
                Entities.Add(CreateLine(new Point3d(_CenterPoint.X - 10, _CenterPoint.Y, _CenterPoint.Z), new Point3d(_CenterPoint.X - 10, _CenterPoint.Y + 15, _CenterPoint.Z), 0, 0, 30));

                //ground
                Entities.Add(CreateLine(new Point3d(_CenterPoint.X, _CenterPoint.Y - 10, _CenterPoint.Z), new Point3d(_CenterPoint.X - 5, _CenterPoint.Y - 10, _CenterPoint.Z), 0, 0, 30));
                Entities.Add(CreateLine(new Point3d(_CenterPoint.X - 10, _CenterPoint.Y - 10, _CenterPoint.Z), new Point3d(_CenterPoint.X - 15, _CenterPoint.Y - 10, _CenterPoint.Z), 0, 0, 30));
                Entities.Add(CreateLine(new Point3d(_CenterPoint.X - 15, _CenterPoint.Y - 10, _CenterPoint.Z), new Point3d(_CenterPoint.X - 15, _CenterPoint.Y - 15, _CenterPoint.Z), 0, 0, 30));
                Entities.Add(CreateLine(new Point3d(_CenterPoint.X - 18, _CenterPoint.Y - 15, _CenterPoint.Z), new Point3d(_CenterPoint.X - 12, _CenterPoint.Y - 15, _CenterPoint.Z), 0, 0, 30));
                Entities.Add(CreateLine(new Point3d(_CenterPoint.X - 17, _CenterPoint.Y - 16, _CenterPoint.Z), new Point3d(_CenterPoint.X - 13, _CenterPoint.Y - 16, _CenterPoint.Z), 0, 0, 30));
                Entities.Add(CreateLine(new Point3d(_CenterPoint.X - 16, _CenterPoint.Y - 17, _CenterPoint.Z), new Point3d(_CenterPoint.X - 14, _CenterPoint.Y - 17, _CenterPoint.Z), 0, 0, 30));

                //keys
                Entities.Add(CreateLine(new Point3d(_CenterPoint.X, _CenterPoint.Y + 20, _CenterPoint.Z), new Point3d(_CenterPoint.X - 5, _CenterPoint.Y + 25, _CenterPoint.Z), (int)Atend.Control.Enum.ProductType.KeyElse, 150, 30));
                Entities.Add(CreateLine(new Point3d(_CenterPoint.X, _CenterPoint.Y + 5, _CenterPoint.Z), new Point3d(_CenterPoint.X - 5, _CenterPoint.Y + 10, _CenterPoint.Z), (int)Atend.Control.Enum.ProductType.Key, 150, 30));
                Entities.Add(CreateLine(new Point3d(_CenterPoint.X - 5, _CenterPoint.Y - 10, _CenterPoint.Z), new Point3d(_CenterPoint.X - 10, _CenterPoint.Y - 5, _CenterPoint.Z), (int)Atend.Control.Enum.ProductType.KeyElse, 150, 30));

                //header cable
                Point3dCollection Points = new Point3dCollection();
                Points.Add(new Point3d(_CenterPoint.X, _CenterPoint.Y - 30, _CenterPoint.Z));
                Points.Add(new Point3d(_CenterPoint.X - 5, _CenterPoint.Y - 25, _CenterPoint.Z));
                Points.Add(new Point3d(_CenterPoint.X + 5, _CenterPoint.Y - 25, _CenterPoint.Z));
                Points.Add(new Point3d(_CenterPoint.X, _CenterPoint.Y - 30, _CenterPoint.Z));
                Entities.Add(CreateHeaderCable(Points));


                return Entities;
            }

        }



        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        public void DrawDezhangtorCell(int ProductCode, Guid Code)
        {
            //////////Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            //////////bool conti = true;
            //////////Atend.Global.Acad.AcadJigs.DrawDezhangtorCellJig DrawDezhangtor = new Atend.Global.Acad.AcadJigs.DrawDezhangtorCellJig(ProductCode, Code);
            //////////PromptResult pr;

            //////////PromptEntityOptions peo = new PromptEntityOptions("\nSelect Post Entity :");
            //////////PromptEntityResult per = ed.GetEntity(peo);
            //////////if (per.Status == PromptStatus.OK)
            //////////{

            //////////    Atend.Base.Acad.AT_INFO PostInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(per.ObjectId);
            //////////    if (PostInfo.ParentCode != "NONE" && PostInfo.NodeType == (int)Atend.Control.Enum.ProductType.Post)
            //////////    {
            //////////        while (conti)
            //////////        {

            //////////            pr = ed.Drag(DrawDezhangtor);
            //////////            if (pr.Status == PromptStatus.OK)
            //////////            {
            //////////                conti = false;

            //////////                #region Save Data Here
            //////////                object ProductType = null;
            //////////                ObjectIdCollection OIC = new ObjectIdCollection();
            //////////                ObjectId CellOi = ObjectId.Null;
            //////////                ObjectIdCollection CellSubOI = new ObjectIdCollection();

            //////////                List<Entity> Entities = DrawDezhangtor.GetEntities();
            //////////                foreach (Entity ent in Entities)
            //////////                {

            //////////                    ProductType = null;
            //////////                    Atend.Global.Acad.AcadJigs.MyCircle mc = ent as Atend.Global.Acad.AcadJigs.MyCircle;
            //////////                    if (mc != null)
            //////////                    {
            //////////                        #region Save circle shapes here

            //////////                        if (mc.AdditionalDictionary.ContainsKey("ProductType"))
            //////////                        {
            //////////                            mc.AdditionalDictionary.TryGetValue("ProductType", out ProductType);
            //////////                            if (ProductType != null)
            //////////                            {
            //////////                                ObjectId oi = Atend.Global.Acad.UAcad.DrawEntityOnScreen(mc, Atend.Control.Enum.AutoCadLayerName.MID_GROUND.ToString());
            //////////                                OIC.Add(oi);
            //////////                                Atend.Base.Acad.AT_INFO DrawnEntityInfo = new Atend.Base.Acad.AT_INFO(oi);
            //////////                                DrawnEntityInfo.ParentCode = "";
            //////////                                DrawnEntityInfo.NodeType = Convert.ToInt32(ProductType);
            //////////                                DrawnEntityInfo.NodeCode = "";
            //////////                                DrawnEntityInfo.ProductCode = Atend.Base.Acad.AcadGlobal.eCell.Code;
            //////////                                DrawnEntityInfo.Insert();

            //////////                                switch ((Atend.Control.Enum.ProductType)ProductType)
            //////////                                {
            //////////                                    case Atend.Control.Enum.ProductType.Cell:
            //////////                                        CellOi = oi;
            //////////                                        break;
            //////////                                    case Atend.Control.Enum.ProductType.ConnectionPoint:
            //////////                                        CellSubOI.Add(oi);
            //////////                                        break;
            //////////                                    case Atend.Control.Enum.ProductType.HeaderCabel:
            //////////                                        CellSubOI.Add(oi);
            //////////                                        break;
            //////////                                    case Atend.Control.Enum.ProductType.Bus:
            //////////                                        CellSubOI.Add(oi);
            //////////                                        break;
            //////////                                    case Atend.Control.Enum.ProductType.Key:
            //////////                                        CellSubOI.Add(oi);
            //////////                                        break;
            //////////                                }


            //////////                            }
            //////////                        }
            //////////                        else
            //////////                        {
            //////////                            return;
            //////////                        }


            //////////                        #endregion

            //////////                    }
            //////////                    else
            //////////                    {
            //////////                        Atend.Global.Acad.AcadJigs.MyPolyLine mp = ent as Atend.Global.Acad.AcadJigs.MyPolyLine;
            //////////                        if (mp != null)
            //////////                        {
            //////////                            #region Save Polyline shapes here

            //////////                            if (mp.AdditionalDictionary.ContainsKey("ProductType"))
            //////////                            {
            //////////                                mp.AdditionalDictionary.TryGetValue("ProductType", out ProductType);
            //////////                                if (ProductType != null)
            //////////                                {
            //////////                                    ObjectId oi = Atend.Global.Acad.UAcad.DrawEntityOnScreen(mp, Atend.Control.Enum.AutoCadLayerName.MID_GROUND.ToString());
            //////////                                    OIC.Add(oi);
            //////////                                    Atend.Base.Acad.AT_INFO DrawnEntityInfo = new Atend.Base.Acad.AT_INFO(oi);
            //////////                                    DrawnEntityInfo.ParentCode = "";
            //////////                                    DrawnEntityInfo.NodeType = Convert.ToInt32(ProductType);
            //////////                                    DrawnEntityInfo.NodeCode = "";
            //////////                                    DrawnEntityInfo.ProductCode = Atend.Base.Acad.AcadGlobal.eCell.Code;
            //////////                                    DrawnEntityInfo.Insert();

            //////////                                    switch ((Atend.Control.Enum.ProductType)ProductType)
            //////////                                    {
            //////////                                        case Atend.Control.Enum.ProductType.Cell:
            //////////                                            CellOi = oi;
            //////////                                            break;
            //////////                                        case Atend.Control.Enum.ProductType.ConnectionPoint:
            //////////                                            CellSubOI.Add(oi);
            //////////                                            break;
            //////////                                        case Atend.Control.Enum.ProductType.HeaderCabel:
            //////////                                            CellSubOI.Add(oi);
            //////////                                            break;
            //////////                                        case Atend.Control.Enum.ProductType.Bus:
            //////////                                            CellSubOI.Add(oi);
            //////////                                            break;
            //////////                                        case Atend.Control.Enum.ProductType.Key:
            //////////                                            CellSubOI.Add(oi);
            //////////                                            break;
            //////////                                    }

            //////////                                }
            //////////                            }
            //////////                            else
            //////////                            {
            //////////                                return;
            //////////                            }




            //////////                            #endregion

            //////////                        }
            //////////                        else
            //////////                        {
            //////////                            Atend.Global.Acad.AcadJigs.MyLine ml = ent as Atend.Global.Acad.AcadJigs.MyLine;
            //////////                            if (ml != null)
            //////////                            {
            //////////                                #region Save line shapes here

            //////////                                if (ml.AdditionalDictionary.ContainsKey("ProductType"))
            //////////                                {
            //////////                                    ml.AdditionalDictionary.TryGetValue("ProductType", out ProductType);
            //////////                                    if (ProductType != null)
            //////////                                    {
            //////////                                        ObjectId oi = Atend.Global.Acad.UAcad.DrawEntityOnScreen(ml, Atend.Control.Enum.AutoCadLayerName.MID_GROUND.ToString());
            //////////                                        OIC.Add(oi);
            //////////                                        Atend.Base.Acad.AT_INFO DrawnEntityInfo = new Atend.Base.Acad.AT_INFO(oi);
            //////////                                        DrawnEntityInfo.ParentCode = "";
            //////////                                        DrawnEntityInfo.NodeType = Convert.ToInt32(ProductType);
            //////////                                        DrawnEntityInfo.NodeCode = "";
            //////////                                        DrawnEntityInfo.ProductCode = Atend.Base.Acad.AcadGlobal.eCell.Code;
            //////////                                        DrawnEntityInfo.Insert();


            //////////                                        switch ((Atend.Control.Enum.ProductType)ProductType)
            //////////                                        {
            //////////                                            case Atend.Control.Enum.ProductType.Cell:
            //////////                                                CellOi = oi;
            //////////                                                break;
            //////////                                            case Atend.Control.Enum.ProductType.ConnectionPoint:
            //////////                                                CellSubOI.Add(oi);
            //////////                                                break;
            //////////                                            case Atend.Control.Enum.ProductType.HeaderCabel:
            //////////                                                CellSubOI.Add(oi);
            //////////                                                break;
            //////////                                            case Atend.Control.Enum.ProductType.Bus:
            //////////                                                CellSubOI.Add(oi);
            //////////                                                break;
            //////////                                            case Atend.Control.Enum.ProductType.Key:
            //////////                                                CellSubOI.Add(oi);
            //////////                                                break;
            //////////                                        }

            //////////                                    }
            //////////                                }
            //////////                                else
            //////////                                {
            //////////                                    return;
            //////////                                }



            //////////                                #endregion

            //////////                            }
            //////////                            else
            //////////                            {
            //////////                                // it was nothing
            //////////                            }
            //////////                        }
            //////////                    }

            //////////                }


            //////////                Atend.Base.Acad.AT_INFO CellInfo = new Atend.Base.Acad.AT_INFO(CellOi);
            //////////                CellInfo.ParentCode = PostInfo.NodeCode;
            //////////                CellInfo.NodeCode = "";
            //////////                CellInfo.NodeType = (int)Atend.Control.Enum.ProductType.Cell;
            //////////                CellInfo.ProductCode = 0;
            //////////                CellInfo.Insert();

            //////////                Atend.Base.Acad.AT_SUB CellSub = new Atend.Base.Acad.AT_SUB(CellOi);
            //////////                CellSub.SubIdCollection = CellSubOI;
            //////////                CellSub.Insert();


            //////////                //ed.WriteMessage("all thing was drawn \n");

            //////////                ObjectId GOI = Atend.Global.Acad.Global.MakeGroup(Guid.NewGuid().ToString(), OIC);
            //////////                Atend.Base.Acad.AT_INFO DrawnGroupInfo = new Atend.Base.Acad.AT_INFO(GOI);
            //////////                DrawnGroupInfo.ParentCode = "";
            //////////                DrawnGroupInfo.NodeType = (int)Atend.Control.Enum.ProductType.Cell;
            //////////                DrawnGroupInfo.NodeCode = "";
            //////////                DrawnGroupInfo.ProductCode = Atend.Base.Acad.AcadGlobal.eCell.Code;
            //////////                DrawnGroupInfo.Insert();
            //////////                OIC.Add(GOI);

            //////////                //ed.WriteMessage("group made \n");


            //////////                //ed.WriteMessage(" post oi :{0} \n", per.ObjectId);
            //////////                //foreach (ObjectId oi in OIC)
            //////////                //{
            //////////                //ed.WriteMessage(" Sub oi :{0} \n", GOI);
            //////////                Atend.Base.Acad.AT_SUB.AddToAT_SUB(GOI, per.ObjectId);
            //////////                //ed.WriteMessage("Sub inserted \n");
            //////////                //}
            //////////                #endregion
            //////////            }
            //////////            else
            //////////            {
            //////////                conti = false;
            //////////            }

            //////////        }
            //////////    }

            //////////}
        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~



    }
}
