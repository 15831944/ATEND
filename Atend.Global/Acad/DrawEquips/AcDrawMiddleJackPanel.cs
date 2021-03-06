﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
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

    public class AcDrawMiddleJackPanel
    {

        //~~~~~~~~~~~~~~~~~~~ Properties ~~~~~~~~~~~~~~~~~~~~~~~~//

        Guid _nodeCode = Guid.Empty;
        public Guid NodeCode
        {
            get { return _nodeCode; }
            set { _nodeCode = value; }
        }

        Guid _parentCode;
        public Guid ParentCode
        {
            get { return _parentCode; }
            set { _parentCode = value; }
        }

        int _productCode;
        public int ProductCode
        {
            get { return _productCode; }
            set { _productCode = value; }
        }

        Dictionary<Guid, Atend.Base.Equipment.EJackPanelCell> _jackpanelCells;
        public Dictionary<Guid, Atend.Base.Equipment.EJackPanelCell> JackpanelCells
        {
            get { return _jackpanelCells; }
            set { _jackpanelCells = value; }
        }

        //~~~~~~~~~~~~~~~~~~~~~~~ CLASS ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//


        public class DrawMiddleJackPanelJig : DrawJig
        {

            //~~~~~~~~~~~~~~~~~~~~ properties~~~~~~~~~~~~~~~~~~~~~~~//

            int _middleJackPanelProductCode = 0;
            public int MiddleJackPanelProductCode
            {
                get { return _middleJackPanelProductCode; }
                set { _middleJackPanelProductCode = value; }
            }


            Guid _middleJaclPanelParentCode = Guid.Empty;
            public Guid MiddleJaclPanelParentCode
            {
                get { return _middleJaclPanelParentCode; }
                set { _middleJaclPanelParentCode = value; }
            }

            Dictionary<Guid, Atend.Base.Equipment.EJackPanelCell> _cells;
            public Dictionary<Guid, Atend.Base.Equipment.EJackPanelCell> Cells
            {
                get { return _cells; }
                set { _cells = value; }
            }


            //~~~~~~~~~~~~~~~~~~~~ Local Variable ~~~~~~~~~~~~~~~~~~~~~~~//
            Point3d CenterPoint = Point3d.Origin;
            List<Entity> Entities = new List<Entity>();
            private Autodesk.AutoCAD.GraphicsInterface.TextStyle _style;
            Entity ContainerEntity = null;
            double MyScale = 1;


            public DrawMiddleJackPanelJig(Entity Container, double Scale)
            {
                MyScale = Scale;
                Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
                ContainerEntity = Container;
                _style = new Autodesk.AutoCAD.GraphicsInterface.TextStyle();
                _style.Font = new Autodesk.AutoCAD.GraphicsInterface.FontDescriptor("Calibri", false, true, 0, 0);
                _style.TextSize = 10;

            }

            protected override SamplerStatus Sampler(JigPrompts prompts)
            {

                JigPromptPointOptions ppo = new JigPromptPointOptions("\nSelect MiddlejackPanel position");
                PromptPointResult ppr = prompts.AcquirePoint(ppo);

                if (ppr.Status == PromptStatus.OK)
                {
                    if (ppr.Value == CenterPoint)
                    {
                        return SamplerStatus.NoChange;
                    }

                    else
                    {
                        if (Atend.Global.Acad.UAcad.IsInsideCurve((Curve)ContainerEntity, ppr.Value) == true)
                        {

                            CenterPoint = ppr.Value;
                            return SamplerStatus.OK;
                        }
                        else
                        {
                            return SamplerStatus.NoChange;
                        }
                    }
                }
                else
                {
                    return SamplerStatus.Cancel;
                }



            }

            protected override bool WorldDraw(Autodesk.AutoCAD.GraphicsInterface.WorldDraw draw)
            {
                Entities.Clear();
                Autodesk.AutoCAD.GraphicsInterface.WorldGeometry wg2 = draw.Geometry as Autodesk.AutoCAD.GraphicsInterface.WorldGeometry;
                string STR = "**";
                Editor ed = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor;
                AcDrawSecsionerCell.DrawSecionerCellJig02 Secsioner;
                AcDrawDezhangtorCell.DrawDezhangtorCellJig02 Dezhangtor;
                AcDrawBusCouplerSecsionerCell.DrawBusCouplerSecsionerCellJig02 BusSecsioner;
                AcDrawBusCouplerDezhangtorCell.DrawBusCouplerDezhangtorCellJig02 BusDezhangtor;
                AcDrawMeasureCell.DrawMeasureCellJig02 Measure;
                AcDrawReleCell.DrawReleCellJig02 Rele;
                AcDrawFuziblCell.DrawFuziblCellCellJig02 Fuzibl;
                //List<Guid> Guids = new List<Guid>();

                int CellCounter = 1;
                if (MiddleJaclPanelParentCode != Guid.Empty)
                {
                    //foreach (Atend.Base.Equipment.EJackPanelCell jc in Cells)
                    foreach (Guid Key in Cells.Keys)
                    {
                        Atend.Base.Equipment.EJackPanelCell CellTemp = Cells[Key];
                        if (CellTemp != null)
                        {
                            switch (CellTemp.ProductType)
                            {
                                case 1:
                                    //اندازه گیری
                                    STR = STR + "001";
                                    if (Key != null)
                                    {
                                        Measure = new AcDrawMeasureCell.DrawMeasureCellJig02(CellTemp.ProductCode, Key, MyScale);
                                        List<Entity> TempEnt1 = Measure.GetDemo(new Point3d(CenterPoint.X + (CellCounter - 1) * 40, CenterPoint.Y, CenterPoint.Z));
                                        foreach (Entity ent in TempEnt1)
                                        {
                                            Entities.Add(ent);
                                        }
                                    }
                                    break;
                                case 2:
                                    //رله
                                    STR = STR + "002";
                                    if (Key != null)
                                    {
                                        Rele = new AcDrawReleCell.DrawReleCellJig02(CellTemp.ProductCode, Key, MyScale);
                                        List<Entity> TempEnt1 = Rele.GetDemo(new Point3d(CenterPoint.X + (CellCounter - 1) * 40, CenterPoint.Y, CenterPoint.Z));
                                        foreach (Entity ent in TempEnt1)
                                        {
                                            Entities.Add(ent);
                                        }
                                    }

                                    break;
                                case 3:
                                    //کلید سکسیونر
                                    STR = STR + "003";
                                    if (Key != null)
                                    {
                                        Secsioner = new AcDrawSecsionerCell.DrawSecionerCellJig02(CellTemp.ProductCode, Key, MyScale);
                                        List<Entity> TempEnt1 = Secsioner.GetDemo(new Point3d(CenterPoint.X + (CellCounter - 1) * 40, CenterPoint.Y, CenterPoint.Z));
                                        foreach (Entity ent in TempEnt1)
                                        {
                                            Entities.Add(ent);
                                        }
                                    }
                                    break;
                                case 4:
                                    //کلید دژنکتور
                                    STR = STR + "004";
                                    if (Key != null)
                                    {
                                        //STR = STR + "004";
                                        Dezhangtor = new AcDrawDezhangtorCell.DrawDezhangtorCellJig02(CellTemp.ProductCode, Key, MyScale);
                                        List<Entity> TempEnt2 = Dezhangtor.GetDemo(new Point3d(CenterPoint.X + (CellCounter - 1) * 40, CenterPoint.Y, CenterPoint.Z));
                                        foreach (Entity ent in TempEnt2)
                                        {
                                            Entities.Add(ent);
                                        }
                                    }
                                    break;
                                case 5:
                                    //BusCoupler سکسیونر
                                    STR = STR + "005";
                                    if (Key != null)
                                    {
                                        //STR = STR + "005";
                                        BusSecsioner = new AcDrawBusCouplerSecsionerCell.DrawBusCouplerSecsionerCellJig02(CellTemp.ProductCode, Key, MyScale);
                                        List<Entity> TempEnt3 = BusSecsioner.GetDemo(new Point3d(CenterPoint.X + (CellCounter - 1) * 40, CenterPoint.Y, CenterPoint.Z));

                                        foreach (Entity ent in TempEnt3)
                                        {
                                            Entities.Add(ent);
                                        }
                                    }
                                    break;
                                case 6:
                                    //BusCoupler دژنکتور
                                    STR = STR + "006";
                                    if (Key != null)
                                    {
                                        //STR = STR + "006";
                                        BusDezhangtor = new AcDrawBusCouplerDezhangtorCell.DrawBusCouplerDezhangtorCellJig02(CellTemp.ProductCode, Key, MyScale);
                                        List<Entity> TempEnt4 = BusDezhangtor.GetDemo(new Point3d(CenterPoint.X + (CellCounter - 1) * 40, CenterPoint.Y, CenterPoint.Z));

                                        foreach (Entity ent in TempEnt4)
                                        {
                                            Entities.Add(ent);
                                        }
                                    }
                                    break;
                                case 7:
                                    //فوزیبل
                                    STR = STR + "007";
                                    if (Key != null)
                                    {
                                        //STR = STR + "007";
                                        Fuzibl = new AcDrawFuziblCell.DrawFuziblCellCellJig02(CellTemp.ProductCode, Key, MyScale);
                                        List<Entity> TempEnt4 = Fuzibl.GetDemo(new Point3d(CenterPoint.X + (CellCounter - 1) * 40, CenterPoint.Y, CenterPoint.Z));
                                        foreach (Entity ent in TempEnt4)
                                        {
                                            Entities.Add(ent);
                                        }
                                    }
                                    break;

                            }
                            CellCounter++;
                        }

                    }
                }// parent !=null

                Matrix3d trans1 = Matrix3d.Scaling(MyScale, new Point3d(CenterPoint.X, CenterPoint.Y, 0));
                foreach (Entity en in Entities)
                {
                    en.TransformBy(trans1);
                }


                foreach (Entity ent in Entities)
                {
                    draw.Geometry.Draw(ent);
                }

                // SHOW POSITION VALUE

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

                        "Cell:" + STR, // Text

                        true,                  // Rawness

                        _style                // TextStyle

                            );


                    // Remember to pop our transforms off the stack

                    wg2.PopModelTransform();

                    wg2.PopModelTransform();

                }

                // END OF SHOW POSITION VALUE


                return true;

            }

            public List<Entity> GetEntities()
            {
                return Entities;
            }

        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        private void __DrawMiddleJackPanel(Entity PostContainerEntity)
        {

            Editor ed = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor;
            bool conti = true;
            Dictionary<Guid, List<Entity>> MyCells = new Dictionary<Guid, List<Entity>>();
            double MyScale = Atend.Base.Design.DProductProperties.AccessSelectBySoftwareCode((int)Atend.Control.Enum.ProductType.MiddleJackPanel).Scale;

            DrawMiddleJackPanelJig MidJ = new DrawMiddleJackPanelJig(PostContainerEntity, MyScale);
            MidJ.MiddleJackPanelProductCode = ProductCode;
            MidJ.MiddleJaclPanelParentCode = ParentCode;
            MidJ.Cells = JackpanelCells;
            PromptResult pr;
            //ed.WriteMessage("------ START MJ {0} -------\n",NodeCode);
            while (conti)
            {
                pr = ed.Drag(MidJ);
                if (pr.Status == PromptStatus.OK)
                {
                    conti = false;
                    #region save data here
                    ObjectIdCollection OIC = new ObjectIdCollection();
                    List<Guid> CellsGuid = new List<Guid>();

                    List<Entity> Entities = new List<Entity>();
                    Entities = MidJ.GetEntities();
                    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

                    #region Seprate different cells from each other
                    //ed.WriteMessage("Seprate different cells from each other\n");
                    foreach (Entity ent in Entities)
                    {

                        Atend.Global.Acad.AcadJigs.MyPolyLine poly = ent as Atend.Global.Acad.AcadJigs.MyPolyLine;
                        object CellCode = null;
                        if (poly != null)
                        {
                            //ed.WriteMessage("~~~Poly~~~:{0}\n", poly.AdditionalDictionary.ContainsKey("ProductCode"));
                            if (poly.AdditionalDictionary.ContainsKey("Code"))
                            {
                                poly.AdditionalDictionary.TryGetValue("Code", out CellCode);

                            }
                        }
                        else
                        {
                            Atend.Global.Acad.AcadJigs.MyLine lin = ent as Atend.Global.Acad.AcadJigs.MyLine;
                            if (lin != null)
                            {
                                if (lin.AdditionalDictionary.ContainsKey("Code"))
                                {
                                    lin.AdditionalDictionary.TryGetValue("Code", out CellCode);
                                }
                            }
                            else
                            {
                                Atend.Global.Acad.AcadJigs.MyCircle cir = ent as Atend.Global.Acad.AcadJigs.MyCircle;
                                if (cir != null)
                                {
                                    if (cir.AdditionalDictionary.ContainsKey("Code"))
                                    {
                                        cir.AdditionalDictionary.TryGetValue("Code", out CellCode);
                                    }
                                }
                            }
                        }
                        if (CellCode != null)
                        {
                            //ed.WriteMessage("Cellcode:{0}\n", CellCode);
                            if (!MyCells.ContainsKey(new Guid(CellCode.ToString())))
                            {
                                //ed.WriteMessage("\nCell Cuid Code:{0}\n", CellCode.ToString());
                                MyCells.Add(new Guid(CellCode.ToString()), new List<Entity>());
                                CellsGuid.Add(new Guid(CellCode.ToString()));

                                List<Entity> Entities1;
                                MyCells.TryGetValue(new Guid(CellCode.ToString()), out Entities1);
                                Entities1.Add(ent);

                            }
                            else
                            {
                                List<Entity> Entities1;
                                MyCells.TryGetValue(new Guid(CellCode.ToString()), out Entities1);
                                Entities1.Add(ent);
                            }

                        }

                    }
                    #endregion

                    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
                    foreach (Guid _ExistGuid in MyCells.Keys)
                    {

                        try
                        {
                            #region Draw different cells
                            //if (MyCells.ContainsKey(_ExistGuid))
                            //{
                            //ed.WriteMessage("Cell count : {0} \n", MyCells.Count);
                            List<Entity> MyList = null;
                            MyCells.TryGetValue(_ExistGuid, out MyList);
                            if (MyList != null)
                            {
                                //ed.WriteMessage("~~~~~ Entities found ~~~~~\n");
                                ObjectIdCollection CurrentCellEntities = new ObjectIdCollection();
                                ObjectId CurrentCellObjectId = ObjectId.Null;
                                foreach (Entity ent in MyList)
                                {

                                    //-----------------------


                                    object MyProductType = null;
                                    object MYProductCode = null;
                                    object myCodeGuid = null;
                                    Atend.Global.Acad.AcadJigs.MyPolyLine poly = ent as Atend.Global.Acad.AcadJigs.MyPolyLine;
                                    if (poly != null)
                                    {
                                        //ed.WriteMessage("~~~POLY~~~{0}\n", poly.AdditionalDictionary.ContainsKey("ProductCode"));
                                        if (poly.AdditionalDictionary.ContainsKey("Code"))
                                        {
                                            poly.AdditionalDictionary.TryGetValue("Code", out myCodeGuid);
                                        }
                                        if (poly.AdditionalDictionary.ContainsKey("ProductType"))
                                        {
                                            poly.AdditionalDictionary.TryGetValue("ProductType", out MyProductType);
                                        }
                                        if (poly.AdditionalDictionary.ContainsKey("ProductCode"))
                                        {
                                            poly.AdditionalDictionary.TryGetValue("ProductCode", out MYProductCode);
                                        }


                                    }
                                    else
                                    {
                                        Atend.Global.Acad.AcadJigs.MyLine lin = ent as Atend.Global.Acad.AcadJigs.MyLine;
                                        if (lin != null)
                                        {
                                            if (lin.AdditionalDictionary.ContainsKey("Code"))
                                            {
                                                lin.AdditionalDictionary.TryGetValue("Code", out myCodeGuid);
                                            }
                                            if (lin.AdditionalDictionary.ContainsKey("ProductType"))
                                            {
                                                lin.AdditionalDictionary.TryGetValue("ProductType", out MyProductType);
                                            }
                                            if (lin.AdditionalDictionary.ContainsKey("ProductCode"))
                                            {
                                                lin.AdditionalDictionary.TryGetValue("ProductCode", out MYProductCode);
                                            }

                                        }
                                        else
                                        {
                                            Atend.Global.Acad.AcadJigs.MyCircle cir = ent as Atend.Global.Acad.AcadJigs.MyCircle;
                                            if (cir != null)
                                            {
                                                if (cir.AdditionalDictionary.ContainsKey("Code"))
                                                {
                                                    cir.AdditionalDictionary.TryGetValue("Code", out myCodeGuid);
                                                }
                                                if (cir.AdditionalDictionary.ContainsKey("ProductType"))
                                                {
                                                    cir.AdditionalDictionary.TryGetValue("ProductType", out MyProductType);
                                                }
                                                if (cir.AdditionalDictionary.ContainsKey("ProductCode"))
                                                {
                                                    cir.AdditionalDictionary.TryGetValue("ProductCode", out MYProductCode);
                                                }

                                            }
                                        }
                                    }

                                    //------------------------
                                    //ed.WriteMessage("Entity Type:{0}\n", ProductType);
                                    ObjectId NewCellEntities = Atend.Global.Acad.UAcad.DrawEntityOnScreen(ent, Atend.Control.Enum.AutoCadLayerName.MED_GROUND.ToString());
                                    OIC.Add(NewCellEntities);
                                    //                                    CurrentCellEntities.Add(NewCellEntities);
                                    if (MyProductType != null)
                                    {
                                        if (Convert.ToInt32(MyProductType) == (int)Atend.Control.Enum.ProductType.Cell)
                                        {
                                            CurrentCellObjectId = NewCellEntities;
                                            //ed.WriteMessage("CurrentCellObjectId was found\n");
                                        }
                                        else
                                        {
                                            CurrentCellEntities.Add(NewCellEntities);
                                        }
                                    }
                                    //ed.WriteMessage("NodeCode : {0}\n", NodeCode);

                                    Atend.Base.Acad.AT_INFO CellSubInfo = new Atend.Base.Acad.AT_INFO(NewCellEntities);
                                    if (myCodeGuid != null)
                                    {
                                        //ed.WriteMessage("cell entity was found\n");
                                        CellSubInfo.ParentCode = NodeCode.ToString(); //CurrentMiddleJackPanelCodeGuid.ToString();
                                        CellSubInfo.NodeCode = myCodeGuid.ToString();
                                    }
                                    else
                                    {
                                        CellSubInfo.ParentCode = myCodeGuid.ToString();
                                        CellSubInfo.NodeCode = "";
                                    }
                                    //ed.WriteMessage("2\n");

                                    if (MYProductCode != null)
                                    {
                                        CellSubInfo.ProductCode = Convert.ToInt32(MYProductCode);
                                    }
                                    else
                                    {
                                        CellSubInfo.ProductCode = 0;
                                    }
                                    //ed.WriteMessage("3\n");

                                    if (MyProductType != null)
                                    {
                                        CellSubInfo.NodeType = Convert.ToInt32(MyProductType);
                                    }
                                    else
                                    {
                                        CellSubInfo.NodeType = 0;
                                    }
                                    //ed.WriteMessage("4\n");

                                    CellSubInfo.Insert();


                                }

                                //insert cell sub
                                //ed.WriteMessage("%%% CurrentCellObjectId %%% {0} : {1} \n", CurrentCellObjectId, CurrentCellEntities.Count);
                                if (CurrentCellObjectId != null && CurrentCellEntities.Count != 0)
                                {
                                    //ed.WriteMessage("5:1\n");
                                    //Atend.Base.Acad.AT_SUB cellsub = new Atend.Base.Acad.AT_SUB(CurrentCellObjectId);
                                    foreach (ObjectId oi in CurrentCellEntities)
                                    {
                                        //cellsub.SubIdCollection.Add(oi);
                                        Atend.Base.Acad.AT_SUB.AddToAT_SUB(oi, CurrentCellObjectId);
                                    }
                                    //cellsub.Insert();
                                    //ed.WriteMessage("5:2\n");
                                }
                            }
                            //}
                            // ed.WriteMessage("One Pack Was drawn \n");
                            //}
                            #endregion
                        }
                        catch (System.Exception ex)
                        {
                            ed.WriteMessage("Error while drawing: {0} \n", ex.Message);
                            return;
                        }
                    }

                    #region Group all cells of Middlejack panel

                    if (NodeCode != null)
                    {
                        //ed.WriteMessage("Group all cells of Middlejack panel\n");
                        ObjectId GroupOI = Atend.Global.Acad.Global.MakeGroup(NodeCode.ToString() + "-MJP", OIC);

                        Atend.Base.Acad.AT_INFO MiddleGroupInfo = new Atend.Base.Acad.AT_INFO(GroupOI);
                        MiddleGroupInfo.ParentCode = ParentCode.ToString();
                        MiddleGroupInfo.NodeCode = NodeCode.ToString();
                        MiddleGroupInfo.NodeType = (int)Atend.Control.Enum.ProductType.MiddleJackPanel;
                        MiddleGroupInfo.ProductCode = ProductCode;
                        MiddleGroupInfo.Insert();


                        Atend.Base.Acad.AT_SUB.AddToAT_SUB(GroupOI, PostContainerEntity.ObjectId);
                        Atend.Base.Acad.AT_SUB.AddToAT_SUB(PostContainerEntity.ObjectId, GroupOI);

                    }
                    #endregion

                    #endregion
                }
                else
                {
                    conti = false;
                }


            }

        }

        public void DrawMiddleJackPanel02(Entity PostContainerEntity)
        {

            Editor ed = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor;
            bool conti = true;
            Dictionary<Guid, List<Entity>> MyCells = new Dictionary<Guid, List<Entity>>();
            double MyScale = Atend.Base.Design.DProductProperties.AccessSelectBySoftwareCode((int)Atend.Control.Enum.ProductType.MiddleJackPanel).Scale;

            DrawMiddleJackPanelJig MidJ = new DrawMiddleJackPanelJig(PostContainerEntity, MyScale);
            MidJ.MiddleJackPanelProductCode = ProductCode;
            MidJ.MiddleJaclPanelParentCode = ParentCode;
            MidJ.Cells = JackpanelCells;
            PromptResult pr;
            while (conti)
            {
                pr = ed.Drag(MidJ);
                if (pr.Status == PromptStatus.OK)
                {
                    conti = false;
                    #region save data here
                    ObjectIdCollection OIC = new ObjectIdCollection();
                    List<Guid> CellsGuid = new List<Guid>();

                    List<Entity> Entities = new List<Entity>();
                    Entities = MidJ.GetEntities();
                    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

                    #region Seprate different cells from each other
                    foreach (Entity ent in Entities)
                    {
                        Atend.Global.Acad.AcadJigs.MyPolyLine poly = ent as Atend.Global.Acad.AcadJigs.MyPolyLine;
                        object CellCode = null;
                        if (poly != null)
                        {
                            if (poly.AdditionalDictionary.ContainsKey("Code"))
                            {
                                poly.AdditionalDictionary.TryGetValue("Code", out CellCode);
                            }
                        }
                        else
                        {
                            Atend.Global.Acad.AcadJigs.MyLine lin = ent as Atend.Global.Acad.AcadJigs.MyLine;
                            if (lin != null)
                            {
                                if (lin.AdditionalDictionary.ContainsKey("Code"))
                                {
                                    lin.AdditionalDictionary.TryGetValue("Code", out CellCode);
                                }
                            }
                            else
                            {
                                Atend.Global.Acad.AcadJigs.MyCircle cir = ent as Atend.Global.Acad.AcadJigs.MyCircle;
                                if (cir != null)
                                {
                                    if (cir.AdditionalDictionary.ContainsKey("Code"))
                                    {
                                        cir.AdditionalDictionary.TryGetValue("Code", out CellCode);
                                    }
                                }
                            }
                        }
                        if (CellCode != null)
                        {
                            if (!MyCells.ContainsKey(new Guid(CellCode.ToString())))
                            {
                                MyCells.Add(new Guid(CellCode.ToString()), new List<Entity>());
                                CellsGuid.Add(new Guid(CellCode.ToString()));
                                List<Entity> Entities1;
                                MyCells.TryGetValue(new Guid(CellCode.ToString()), out Entities1);
                                Entities1.Add(ent);

                            }
                            else
                            {
                                List<Entity> Entities1;
                                MyCells.TryGetValue(new Guid(CellCode.ToString()), out Entities1);
                                Entities1.Add(ent);
                            }

                        }

                    }
                    #endregion

                    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
                    ObjectIdCollection BusOIs = new ObjectIdCollection();
                    foreach (Guid _ExistGuid in MyCells.Keys)
                    {

                        try
                        {
                            #region Draw different cells
                            List<Entity> MyList = null;
                            MyCells.TryGetValue(_ExistGuid, out MyList);
                            if (MyList != null)
                            {
                                ObjectIdCollection CurrentCellEntities = new ObjectIdCollection();
                                ObjectId CurrentCellObjectId = ObjectId.Null;
                                foreach (Entity ent in MyList)
                                {
                                    object MyProductType = null;
                                    object MYProductCode = null;
                                    object myCodeGuid = null;
                                    Atend.Global.Acad.AcadJigs.MyPolyLine poly = ent as Atend.Global.Acad.AcadJigs.MyPolyLine;
                                    if (poly != null)
                                    {
                                        if (poly.AdditionalDictionary.ContainsKey("Code"))
                                        {
                                            poly.AdditionalDictionary.TryGetValue("Code", out myCodeGuid);
                                        }
                                        if (poly.AdditionalDictionary.ContainsKey("ProductType"))
                                        {
                                            poly.AdditionalDictionary.TryGetValue("ProductType", out MyProductType);
                                        }
                                        if (poly.AdditionalDictionary.ContainsKey("ProductCode"))
                                        {
                                            poly.AdditionalDictionary.TryGetValue("ProductCode", out MYProductCode);
                                        }


                                    }
                                    else
                                    {
                                        Atend.Global.Acad.AcadJigs.MyLine lin = ent as Atend.Global.Acad.AcadJigs.MyLine;
                                        if (lin != null)
                                        {
                                            if (lin.AdditionalDictionary.ContainsKey("Code"))
                                            {
                                                lin.AdditionalDictionary.TryGetValue("Code", out myCodeGuid);
                                            }
                                            if (lin.AdditionalDictionary.ContainsKey("ProductType"))
                                            {
                                                lin.AdditionalDictionary.TryGetValue("ProductType", out MyProductType);
                                            }
                                            if (lin.AdditionalDictionary.ContainsKey("ProductCode"))
                                            {
                                                lin.AdditionalDictionary.TryGetValue("ProductCode", out MYProductCode);
                                            }

                                        }
                                        else
                                        {
                                            Atend.Global.Acad.AcadJigs.MyCircle cir = ent as Atend.Global.Acad.AcadJigs.MyCircle;
                                            if (cir != null)
                                            {
                                                if (cir.AdditionalDictionary.ContainsKey("Code"))
                                                {
                                                    cir.AdditionalDictionary.TryGetValue("Code", out myCodeGuid);
                                                }
                                                if (cir.AdditionalDictionary.ContainsKey("ProductType"))
                                                {
                                                    cir.AdditionalDictionary.TryGetValue("ProductType", out MyProductType);
                                                }
                                                if (cir.AdditionalDictionary.ContainsKey("ProductCode"))
                                                {
                                                    cir.AdditionalDictionary.TryGetValue("ProductCode", out MYProductCode);
                                                }

                                            }
                                        }
                                    }

                                    //------------------------
                                    ObjectId NewCellEntities = Atend.Global.Acad.UAcad.DrawEntityOnScreen(ent, Atend.Control.Enum.AutoCadLayerName.MED_GROUND.ToString());
                                    OIC.Add(NewCellEntities);
                                    if (MyProductType != null)
                                    {
                                        if (Convert.ToInt32(MyProductType) == (int)Atend.Control.Enum.ProductType.Cell)
                                        {
                                            CurrentCellObjectId = NewCellEntities;
                                        }
                                        else
                                        {
                                            CurrentCellEntities.Add(NewCellEntities);
                                        }
                                    }
                                    Atend.Base.Acad.AT_INFO CellSubInfo = new Atend.Base.Acad.AT_INFO(NewCellEntities);
                                    if (myCodeGuid != null)
                                    {
                                        CellSubInfo.ParentCode = NodeCode.ToString();
                                        CellSubInfo.NodeCode = myCodeGuid.ToString();
                                    }
                                    else
                                    {
                                        CellSubInfo.ParentCode = myCodeGuid.ToString();
                                        CellSubInfo.NodeCode = "";
                                    }

                                    if (MYProductCode != null)
                                    {
                                        CellSubInfo.ProductCode = Convert.ToInt32(MYProductCode);
                                    }
                                    else
                                    {
                                        CellSubInfo.ProductCode = 0;
                                    }

                                    if (MyProductType != null)
                                    {
                                        CellSubInfo.NodeType = Convert.ToInt32(MyProductType);
                                        if (Convert.ToInt32(MyProductType) == (int)Atend.Control.Enum.ProductType.Bus)
                                        {
                                            CellSubInfo.ProductCode = Atend.Base.Equipment.EJAckPanel.AccessSelectByCode(ProductCode).MasterProductCode;
                                            BusOIs.Add(NewCellEntities);
                                            //ed.WriteMessage("Bus OI : {0}\n", NewCellEntities);
                                        }
                                    }
                                    else
                                    {
                                        CellSubInfo.NodeType = 0;
                                    }
                                    CellSubInfo.Insert();


                                }

                                if (CurrentCellObjectId != null && CurrentCellEntities.Count != 0)
                                {
                                    foreach (ObjectId oi in CurrentCellEntities)
                                    {
                                        Atend.Base.Acad.AT_SUB.AddToAT_SUB(oi, CurrentCellObjectId);
                                    }
                                }
                            }
                            #endregion
                        }
                        catch (System.Exception ex)
                        {
                            ed.WriteMessage("Error while drawing: {0} \n", ex.Message);
                            return;
                        }
                    }
                    //ed.WriteMessage("Bus Count : {0} \n", BusOIs.Count);

                    #region Join Buses to each other
                    foreach (Guid _ExistGuid in MyCells.Keys)
                    {
                        try
                        {

                        }
                        catch (System.Exception ex)
                        {
                            ed.WriteMessage("Error while Connect bus to each other: {0} \n", ex.Message);
                            return;
                        }
                    }
                    #endregion

                    //if (BusOIs.Count > 0)
                    //{
                    //    for (int BC = 0; BC < BusOIs.Count - 1; BC++)
                    //    {

                    //        Atend.Global.Acad.DrawEquips.AcDrawTerminal _AcDrawTerminal = new AcDrawTerminal();
                    //        _AcDrawTerminal.DrawTerminal(Atend.Global.Acad.UAcad.CenterOfEntity(Atend.Global.Acad.UAcad.GetEntityByObjectID(BusOIs[BC])), Atend.Global.Acad.UAcad.CenterOfEntity(Atend.Global.Acad.UAcad.GetEntityByObjectID(BusOIs[BC])));


                    //        //Atend.Base.Acad.AT_INFO BusInfo=new Atend.Base.Acad.AT_INFO(



                    //    }
                    //}
                    #region Group all cells of Middlejack panel

                    if (NodeCode != Guid.Empty)
                    {
                        //ed.WriteMessage("Group all cells of Middlejack panel\n");
                        ObjectId GroupOI = Atend.Global.Acad.Global.MakeGroup(NodeCode.ToString() + "-MJP", OIC);

                        Atend.Base.Acad.AT_INFO MiddleGroupInfo = new Atend.Base.Acad.AT_INFO(GroupOI);
                        MiddleGroupInfo.ParentCode = ParentCode.ToString();
                        MiddleGroupInfo.NodeCode = NodeCode.ToString();
                        MiddleGroupInfo.NodeType = (int)Atend.Control.Enum.ProductType.MiddleJackPanel;
                        MiddleGroupInfo.ProductCode = ProductCode;
                        MiddleGroupInfo.Insert();

                        Atend.Base.Acad.AT_SUB.AddToAT_SUB(GroupOI, PostContainerEntity.ObjectId);
                        Atend.Base.Acad.AT_SUB.AddToAT_SUB(PostContainerEntity.ObjectId, GroupOI);

                    }
                    #endregion

                    #endregion
                }
                else
                {
                    conti = false;
                }


            }

        }


        //public void DrawMiddleJackPanel02(Entity PostContainerEntity)
        //{

        //    Editor ed = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor;
        //    bool conti = true;
        //    Dictionary<Guid, List<Entity>> MyCells = new Dictionary<Guid, List<Entity>>();
        //    double MyScale = Atend.Base.Design.DProductProperties.AccessSelectBySoftwareCode((int)Atend.Control.Enum.ProductType.MiddleJackPanel).Scale;

        //    DrawMiddleJackPanelJig MidJ = new DrawMiddleJackPanelJig(PostContainerEntity, MyScale);
        //    MidJ.MiddleJackPanelProductCode = ProductCode;
        //    MidJ.MiddleJaclPanelParentCode = ParentCode;
        //    MidJ.Cells = JackpanelCells;
        //    PromptResult pr;
        //    //ed.WriteMessage("------ START MJ {0} -------\n",NodeCode);
        //    while (conti)
        //    {
        //        pr = ed.Drag(MidJ);
        //        if (pr.Status == PromptStatus.OK)
        //        {
        //            conti = false;
        //            #region save data here
        //            ObjectIdCollection OIC = new ObjectIdCollection();
        //            List<Guid> CellsGuid = new List<Guid>();

        //            List<Entity> Entities = new List<Entity>();
        //            Entities = MidJ.GetEntities();
        //            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        //            #region Seprate different cells from each other
        //            //ed.WriteMessage("Seprate different cells from each other\n");
        //            foreach (Entity ent in Entities)
        //            {

        //                Atend.Global.Acad.AcadJigs.MyPolyLine poly = ent as Atend.Global.Acad.AcadJigs.MyPolyLine;
        //                object CellCode = null;
        //                if (poly != null)
        //                {
        //                    //ed.WriteMessage("~~~Poly~~~:{0}\n", poly.AdditionalDictionary.ContainsKey("ProductCode"));
        //                    if (poly.AdditionalDictionary.ContainsKey("Code"))
        //                    {
        //                        poly.AdditionalDictionary.TryGetValue("Code", out CellCode);

        //                    }
        //                }
        //                else
        //                {
        //                    Atend.Global.Acad.AcadJigs.MyLine lin = ent as Atend.Global.Acad.AcadJigs.MyLine;
        //                    if (lin != null)
        //                    {
        //                        if (lin.AdditionalDictionary.ContainsKey("Code"))
        //                        {
        //                            lin.AdditionalDictionary.TryGetValue("Code", out CellCode);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        Atend.Global.Acad.AcadJigs.MyCircle cir = ent as Atend.Global.Acad.AcadJigs.MyCircle;
        //                        if (cir != null)
        //                        {
        //                            if (cir.AdditionalDictionary.ContainsKey("Code"))
        //                            {
        //                                cir.AdditionalDictionary.TryGetValue("Code", out CellCode);
        //                            }
        //                        }
        //                    }
        //                }
        //                if (CellCode != null)
        //                {
        //                    //ed.WriteMessage("Cellcode:{0}\n", CellCode);
        //                    if (!MyCells.ContainsKey(new Guid(CellCode.ToString())))
        //                    {
        //                        //ed.WriteMessage("\nCell Cuid Code:{0}\n", CellCode.ToString());
        //                        MyCells.Add(new Guid(CellCode.ToString()), new List<Entity>());
        //                        CellsGuid.Add(new Guid(CellCode.ToString()));

        //                        List<Entity> Entities1;
        //                        MyCells.TryGetValue(new Guid(CellCode.ToString()), out Entities1);
        //                        Entities1.Add(ent);

        //                    }
        //                    else
        //                    {
        //                        List<Entity> Entities1;
        //                        MyCells.TryGetValue(new Guid(CellCode.ToString()), out Entities1);
        //                        Entities1.Add(ent);
        //                    }

        //                }

        //            }
        //            #endregion

        //            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //            foreach (Guid _ExistGuid in MyCells.Keys)
        //            {

        //                try
        //                {
        //                    #region Draw different cells
        //                    //if (MyCells.ContainsKey(_ExistGuid))
        //                    //{
        //                    //ed.WriteMessage("Cell count : {0} \n", MyCells.Count);
        //                    List<Entity> MyList = null;
        //                    MyCells.TryGetValue(_ExistGuid, out MyList);
        //                    if (MyList != null)
        //                    {
        //                        //ed.WriteMessage("~~~~~ Entities found ~~~~~\n");
        //                        ObjectIdCollection CurrentCellEntities = new ObjectIdCollection();
        //                        ObjectId CurrentCellObjectId = ObjectId.Null;
        //                        foreach (Entity ent in MyList)
        //                        {

        //                            //-----------------------


        //                            object MyProductType = null;
        //                            object MYProductCode = null;
        //                            object myCodeGuid = null;
        //                            Atend.Global.Acad.AcadJigs.MyPolyLine poly = ent as Atend.Global.Acad.AcadJigs.MyPolyLine;
        //                            if (poly != null)
        //                            {
        //                                //ed.WriteMessage("~~~POLY~~~{0}\n", poly.AdditionalDictionary.ContainsKey("ProductCode"));
        //                                if (poly.AdditionalDictionary.ContainsKey("Code"))
        //                                {
        //                                    poly.AdditionalDictionary.TryGetValue("Code", out myCodeGuid);
        //                                }
        //                                if (poly.AdditionalDictionary.ContainsKey("ProductType"))
        //                                {
        //                                    poly.AdditionalDictionary.TryGetValue("ProductType", out MyProductType);
        //                                }
        //                                if (poly.AdditionalDictionary.ContainsKey("ProductCode"))
        //                                {
        //                                    poly.AdditionalDictionary.TryGetValue("ProductCode", out MYProductCode);
        //                                }


        //                            }
        //                            else
        //                            {
        //                                Atend.Global.Acad.AcadJigs.MyLine lin = ent as Atend.Global.Acad.AcadJigs.MyLine;
        //                                if (lin != null)
        //                                {
        //                                    if (lin.AdditionalDictionary.ContainsKey("Code"))
        //                                    {
        //                                        lin.AdditionalDictionary.TryGetValue("Code", out myCodeGuid);
        //                                    }
        //                                    if (lin.AdditionalDictionary.ContainsKey("ProductType"))
        //                                    {
        //                                        lin.AdditionalDictionary.TryGetValue("ProductType", out MyProductType);
        //                                    }
        //                                    if (lin.AdditionalDictionary.ContainsKey("ProductCode"))
        //                                    {
        //                                        lin.AdditionalDictionary.TryGetValue("ProductCode", out MYProductCode);
        //                                    }

        //                                }
        //                                else
        //                                {
        //                                    Atend.Global.Acad.AcadJigs.MyCircle cir = ent as Atend.Global.Acad.AcadJigs.MyCircle;
        //                                    if (cir != null)
        //                                    {
        //                                        if (cir.AdditionalDictionary.ContainsKey("Code"))
        //                                        {
        //                                            cir.AdditionalDictionary.TryGetValue("Code", out myCodeGuid);
        //                                        }
        //                                        if (cir.AdditionalDictionary.ContainsKey("ProductType"))
        //                                        {
        //                                            cir.AdditionalDictionary.TryGetValue("ProductType", out MyProductType);
        //                                        }
        //                                        if (cir.AdditionalDictionary.ContainsKey("ProductCode"))
        //                                        {
        //                                            cir.AdditionalDictionary.TryGetValue("ProductCode", out MYProductCode);
        //                                        }

        //                                    }
        //                                }
        //                            }

        //                            //------------------------
        //                            //ed.WriteMessage("Entity Type:{0}\n", ProductType);
        //                            ObjectId NewCellEntities = Atend.Global.Acad.UAcad.DrawEntityOnScreen(ent, Atend.Control.Enum.AutoCadLayerName.MED_GROUND.ToString());
        //                            OIC.Add(NewCellEntities);
        //                            //                                    CurrentCellEntities.Add(NewCellEntities);
        //                            if (MyProductType != null)
        //                            {
        //                                if (Convert.ToInt32(MyProductType) == (int)Atend.Control.Enum.ProductType.Cell)
        //                                {
        //                                    CurrentCellObjectId = NewCellEntities;
        //                                    //ed.WriteMessage("CurrentCellObjectId was found\n");
        //                                }
        //                                else
        //                                {
        //                                    CurrentCellEntities.Add(NewCellEntities);
        //                                }
        //                            }
        //                            //ed.WriteMessage("NodeCode : {0}\n", NodeCode);

        //                            Atend.Base.Acad.AT_INFO CellSubInfo = new Atend.Base.Acad.AT_INFO(NewCellEntities);
        //                            if (myCodeGuid != null)
        //                            {
        //                                //ed.WriteMessage("cell entity was found\n");
        //                                CellSubInfo.ParentCode = NodeCode.ToString(); //CurrentMiddleJackPanelCodeGuid.ToString();
        //                                CellSubInfo.NodeCode = myCodeGuid.ToString();
        //                            }
        //                            else
        //                            {
        //                                CellSubInfo.ParentCode = myCodeGuid.ToString();
        //                                CellSubInfo.NodeCode = "";
        //                            }
        //                            //ed.WriteMessage("2\n");

        //                            if (MYProductCode != null)
        //                            {
        //                                CellSubInfo.ProductCode = Convert.ToInt32(MYProductCode);
        //                            }
        //                            else
        //                            {
        //                                CellSubInfo.ProductCode = 0;
        //                            }
        //                            //ed.WriteMessage("3\n");

        //                            if (MyProductType != null)
        //                            {
        //                                CellSubInfo.NodeType = Convert.ToInt32(MyProductType);
        //                            }
        //                            else
        //                            {
        //                                CellSubInfo.NodeType = 0;
        //                            }
        //                            //ed.WriteMessage("4\n");

        //                            CellSubInfo.Insert();


        //                        }

        //                        //insert cell sub
        //                        //ed.WriteMessage("%%% CurrentCellObjectId %%% {0} : {1} \n", CurrentCellObjectId, CurrentCellEntities.Count);
        //                        if (CurrentCellObjectId != null && CurrentCellEntities.Count != 0)
        //                        {
        //                            //ed.WriteMessage("5:1\n");
        //                            //Atend.Base.Acad.AT_SUB cellsub = new Atend.Base.Acad.AT_SUB(CurrentCellObjectId);
        //                            foreach (ObjectId oi in CurrentCellEntities)
        //                            {
        //                                //cellsub.SubIdCollection.Add(oi);
        //                                Atend.Base.Acad.AT_SUB.AddToAT_SUB(oi, CurrentCellObjectId);
        //                            }
        //                            //cellsub.Insert();
        //                            //ed.WriteMessage("5:2\n");
        //                        }
        //                    }
        //                    //}
        //                    // ed.WriteMessage("One Pack Was drawn \n");
        //                    //}
        //                    #endregion
        //                }
        //                catch (System.Exception ex)
        //                {
        //                    ed.WriteMessage("Error while drawing: {0} \n", ex.Message);
        //                    return;
        //                }
        //            }

        //            #region Group all cells of Middlejack panel

        //            if (NodeCode != null)
        //            {
        //                //ed.WriteMessage("Group all cells of Middlejack panel\n");
        //                ObjectId GroupOI = Atend.Global.Acad.Global.MakeGroup(NodeCode.ToString() + "-MJP", OIC);

        //                Atend.Base.Acad.AT_INFO MiddleGroupInfo = new Atend.Base.Acad.AT_INFO(GroupOI);
        //                MiddleGroupInfo.ParentCode = ParentCode.ToString();
        //                MiddleGroupInfo.NodeCode = NodeCode.ToString();
        //                MiddleGroupInfo.NodeType = (int)Atend.Control.Enum.ProductType.MiddleJackPanel;
        //                MiddleGroupInfo.ProductCode = ProductCode;
        //                MiddleGroupInfo.Insert();


        //                Atend.Base.Acad.AT_SUB.AddToAT_SUB(GroupOI, PostContainerEntity.ObjectId);

        //            }
        //            #endregion

        //            #endregion
        //        }
        //        else
        //        {
        //            conti = false;
        //        }


        //    }

        //}



    }
}
