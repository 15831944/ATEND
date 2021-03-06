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


    public class AcDrawConnectionPoint
    {

        public class DrawConnectionPointJig : DrawJig
        {


            Point3d CenterPoint = Point3d.Origin;

            Entity ConnectionPointEntity;


            public DrawConnectionPointJig()
            {
                Circle c = new Circle(CenterPoint, new Vector3d(0, 0, 0), 10);

                c.ColorIndex = 3;

                ConnectionPointEntity = c;
            }

            protected override SamplerStatus Sampler(JigPrompts prompts)
            {
                //throw new System.Exception("The method or operation is not implemented.");

                JigPromptPointOptions ppo = new JigPromptPointOptions("\nSelect ConnectionPoint Position :");

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

                c.ColorIndex = 3;

                ConnectionPointEntity = c;

                draw.Geometry.Draw(ConnectionPointEntity);

                return true;
            }

            public Entity GetEntity()
            {
                return ConnectionPointEntity;
            }

            public Entity GetDemo(Point3d MyCenterPoint)
            {
                Circle c = new Circle(MyCenterPoint, new Vector3d(0, 0, 1), 5);

                c.ColorIndex = 3;

                ConnectionPointEntity = c;

                return c;
            }

        }



        /// <summary>
        /// this method had command before for drwing
        /// </summary>
        public void DrawConnectionPoint()
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            DrawConnectionPointJig DCP = new DrawConnectionPointJig();

            bool Conti = true;

            PromptResult pr;

            while (Conti)
            {

                pr = ed.Drag(DCP);

                if (pr.Status == PromptStatus.OK)
                {
                    Conti = false;

                    // time to draw

                    Entity entity = DCP.GetEntity();

                    Atend.Global.Acad.UAcad.DrawEntityOnScreen(entity , Atend.Control.Enum.AutoCadLayerName.GENERAL.ToString());

                    Atend.Base.Acad.AT_INFO at_info = new Atend.Base.Acad.AT_INFO();

                    at_info.ParentCode = "";
                    at_info.NodeCode = "";
                    at_info.NodeType = (int)Atend.Control.Enum.ProductType.ConnectionPoint;
                    at_info.ProductCode = 0;
                    at_info.SelectedObjectId = entity.ObjectId;
                    at_info.Insert();
                }

            }



        }

        public static ObjectId DrawConnectionPoint(Point3d CenterPoint, ObjectId ParentOI)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            DrawConnectionPointJig DCP = new DrawConnectionPointJig();
            Entity ent = DCP.GetDemo(CenterPoint);
            ObjectId ConnectionOI = ObjectId.Null;
            if (ent != null)
            {
                ConnectionOI = Atend.Global.Acad.UAcad.DrawEntityOnScreen(ent, Atend.Control.Enum.AutoCadLayerName.GENERAL.ToString());


                Atend.Base.Acad.AT_INFO ParentInfo = Atend.Base.Acad.AT_INFO.SelectBySelectedObjectId(ParentOI);

                Atend.Base.Acad.AT_INFO at_info = new Atend.Base.Acad.AT_INFO(ConnectionOI);
                //ed.WriteMessage("ParentCode For connection Point: {0}\n", ParentInfo.NodeCode);
                at_info.ParentCode = ParentInfo.NodeCode;
                at_info.NodeCode = "";
                at_info.NodeType = (int)Atend.Control.Enum.ProductType.ConnectionPoint;
                at_info.ProductCode = 0;
                at_info.Insert();

                Atend.Base.Acad.AT_SUB ConnectionPSub = new Atend.Base.Acad.AT_SUB(ConnectionOI);
                ConnectionPSub.SubIdCollection.Add(ParentOI);
                ConnectionPSub.Insert();

                Atend.Base.Acad.AT_SUB.AddToAT_SUB(ConnectionOI, ParentOI);
            }
            return ConnectionOI;
        }



    }
}
