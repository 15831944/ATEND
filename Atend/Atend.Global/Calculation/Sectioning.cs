﻿using System;
using System.Collections.Generic;
using System.Text;

using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using System.Data.Sql;
using System.Data;
using System.Data.OleDb;
using System.Collections;

namespace Atend.Global.Calculation
{

    public class Sectioning
    {
        Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
        Boolean isConsol = true;
        Boolean isConductor = true;
        string currentBranch;
        System.Data.DataColumn col1 = new System.Data.DataColumn("ConsolGuid");
        System.Data.DataColumn col2 = new System.Data.DataColumn("PoleGuid");
        System.Data.DataColumn col3 = new System.Data.DataColumn("BranchGuid");
        System.Data.DataColumn col4 = new System.Data.DataColumn("ConsolObjectId");
        System.Data.DataColumn col5 = new System.Data.DataColumn("SectionNo");

        public System.Data.DataTable dt = new System.Data.DataTable();
        public System.Data.DataTable dtGlobal;//= new System.Data.DataTable();
        public System.Data.DataTable dtConsol;
        public System.Data.DataTable dtPoleConductor = new System.Data.DataTable();

        ObjectId objConsol = ObjectId.Null;
        int counterSection = 1;
        int currentSectionCount = 0;
        ArrayList sectionCollection = new ArrayList();

        #region Section Part1
        public void DetermineSection()
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            ed.WriteMessage("i am in determine\n");
            dt = new System.Data.DataTable();
            dt.Columns.Add(col1);
            dt.Columns.Add(col2);
            dt.Columns.Add(col3);
            dt.Columns.Add(col4);
            dt.Columns.Add(col5);
            ed.WriteMessage("~~~~~~~~~1\n");

            dtPoleConductor = new System.Data.DataTable();
            dtPoleConductor.Columns.Add("PoleGuid");
            dtPoleConductor.Columns.Add("ConsolGuid");
            dtPoleConductor.Columns.Add("BranchGuid");
            dtPoleConductor.Columns.Add("Type");
            dtPoleConductor.Columns.Add("Angle");
            ed.WriteMessage("~~~~~~~~~2\n");
            System.Data.DataTable dtPoles = Atend.Global.Calculation.General.General.GetDesignPoles();
            dtConsol = Atend.Base.Design.DConsol.AccessSelectByType();//.بدست اوردن کلیه کنسول های انتهایی و کششی
            ed.WriteMessage("dtConsol.Count= " + dtConsol.Rows.Count.ToString() + "\n");
            //PromptIntegerResult ir = ed.GetInteger("");

            foreach (DataRow dr in dtConsol.Rows)//پیمایش کلیه مقادیر کنسول ها
            {
                Atend.Base.Design.DConsol dconsol = Atend.Base.Design.DConsol.AccessSelectByCode(new Guid(dr["Code"].ToString()));//پیدا کردن شماره پایه کنسول جاری
                //ed.WriteMessage("dconsolParentCode=" + dconsol.ParentCode + "\n");

                //ObjectId objIdPole = Atend.Global.Acad.UAcad.GetPoleByGuid(dconsol.ParentCode);//بدست اورد objIDپایه جاری
                DataRow[] drPole = dtPoles.Select(" PoleGuid Like '" + dconsol.ParentCode.ToString() + "'");
                //ed.WriteMessage("OBJIDPole= " + objIdPole.ToString() + "\n");
                //Befor
                //if (!objIdPole.IsNull)
                //{

                //    objConsol = Atend.Global.Acad.UAcad.GetConsolObjectId(objIdPole, dconsol.Code);//بدست اوردنOBJID کنسول جاری
                //    //ed.WriteMessage("OBJCONSOL= " + objConsol.ToString() + "\n");

                //    dtGlobal = Atend.Global.Acad.UAcad.GetConsolInfoByObjectId(objConsol);
                //    //ed.WriteMessage("dtGlobal.Count =" + dtGlobal.Rows.Count.ToString() + "\n");

                //    if (IsNotExitConsol())
                //    {
                //        RecSection();
                //        ed.WriteMessage("Information Save Ok\n");
                //    }
                //    else
                //    {
                //        ed.WriteMessage("information Donot Save Ok\n");
                //    }
                //}
                //@@@@@@@
                if (!(drPole.Length == 0))
                {

                    ObjectId curObj = new ObjectId(new IntPtr(Convert.ToInt32(drPole[0]["PoleOI"].ToString())));
                    objConsol = Atend.Global.Acad.UAcad.GetConsolObjectId(curObj, dconsol.Code);//بدست اوردنOBJID کنسول جاری
                    ed.WriteMessage("OBJCONSOL= " + objConsol.ToString() + "\n");

                    dtGlobal = Atend.Global.Acad.UAcad.GetConsolInfoByObjectId(objConsol);
                    //ed.WriteMessage("dtGlobal.Count =" + dtGlobal.Rows.Count.ToString() + "\n");

                    if (IsNotExitConsol())
                    {
                        RecSection();
                        ed.WriteMessage("Information Save Ok\n");
                    }
                    else
                    {
                        ed.WriteMessage("information Donot Save Ok\n");
                    }
                }


            }


            //foreach (DataRow drF in dt.Rows)
            //{
            //    ed.WriteMessage("ConsolGUId= " + drF["ConsolGuid"].ToString() + "   PoleGuid =" + drF["PoleGuid"].ToString() + "   SectionNo =" + drF["SectionNo"].ToString() + "\n");

            //}
        }


        public bool RecSection()
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            ed.WriteMessage("i am In RecSection\n");


            Insert();
            //PromptIntegerResult ir = ed.GetInteger("");
            //ed.WriteMessage("objConsol= " + objConsol.ToString() + "\n");
            //ed.WriteMessage("CurrentBarench =" + currentBranch.ToString() + "\n");


            DataRow[] drpolecond = dtPoleConductor.Select(" PoleGuid Like '" + dtGlobal.Rows[0]["PoleGuid"].ToString() + "'");
            if (drpolecond.Length == 0)
            {
                System.Data.DataTable dtTempPoleCond = Atend.Global.Acad.UAcad.GetPoleConductors(new Guid(dtGlobal.Rows[0]["PoleGuid"].ToString()));
                foreach (DataRow dr1 in dtTempPoleCond.Rows)
                {
                    DataRow drAddCond = dtPoleConductor.NewRow();
                    drAddCond["PoleGuid"] = dr1["Poleguid"].ToString();
                    drAddCond["ConsolGuid"] = dr1["ConsolGuid"].ToString();
                    drAddCond["BranchGuid"] = dr1["BranchGuid"].ToString();
                    drAddCond["Type"] = dr1["Type"].ToString();
                    drAddCond["Angle"] = dr1["Angle"].ToString();
                    dtPoleConductor.Rows.Add(drAddCond);
                }
            }



            objConsol = Atend.Global.Acad.UAcad.GetNextConsol(objConsol, new Guid(currentBranch));


            dtGlobal = Atend.Global.Acad.UAcad.GetConsolInfoByObjectId(objConsol);
            //ed.WriteMessage("dtGlobal1= " + dtGlobal.Rows.Count.ToString() + "\n");
            //ed.WriteMessage("dtGlobal.Rows.Count= " + dtGlobal.Rows.Count.ToString() + "\n");








            Atend.Base.Design.DConsol dconsol = Atend.Base.Design.DConsol.AccessSelectByCode(new Guid(dtGlobal.Rows[0]["ConsolGuid"].ToString()));
            Atend.Base.Equipment.EConsol consol = Atend.Base.Equipment.EConsol.AccessSelectByCode(dconsol.ProductCode);
            //ed.WriteMessage("ConsolType= " + consol.ConsolType.ToString() + "\n");
            if (consol.ConsolType == 0 || consol.ConsolType == 1)
            {
                //ed.WriteMessage("I Am In the IF  ConsolType\n");
                DataRow drNew = dt.NewRow();
                drNew["ConsolGuid"] = dtGlobal.Rows[0]["ConsolGuid"].ToString();
                drNew["PoleGuid"] = dtGlobal.Rows[0]["PoleGuid"].ToString();
                drNew["BranchGuid"] = "";
                drNew["ConsolObjectId"] = dtGlobal.Rows[0]["ConsolObjectId"].ToString();
                drNew["SectionNo"] = counterSection.ToString();
                dt.Rows.Add(drNew);
                //currentBranch = dtGlobal.Rows[0]["BranchGuid"].ToString();

                counterSection++;
                //PromptIntegerResult re = ed.GetInteger("");
                //ed.WriteMessage("CounterSection= " + counterSection.ToString() + "\n");


                DataRow[] drpolecond1 = dtPoleConductor.Select(" PoleGuid Like '" + dtGlobal.Rows[0]["PoleGuid"].ToString() + "'");
                if (drpolecond1.Length == 0)
                {
                    System.Data.DataTable dtTempPoleCond1 = Atend.Global.Acad.UAcad.GetPoleConductors(new Guid(dtGlobal.Rows[0]["PoleGuid"].ToString()));
                    foreach (DataRow dr11 in dtTempPoleCond1.Rows)
                    {
                        DataRow drAddCond1 = dtPoleConductor.NewRow();
                        drAddCond1["PoleGuid"] = dr11["Poleguid"].ToString();
                        drAddCond1["ConsolGuid"] = dr11["ConsolGuid"].ToString();
                        drAddCond1["BranchGuid"] = dr11["BranchGuid"].ToString();
                        drAddCond1["Type"] = dr11["Type"].ToString();
                        drAddCond1["Angle"] = dr11["Angle"].ToString();
                        dtPoleConductor.Rows.Add(drAddCond1);
                    }
                }


                return true;
            }
            else
            {
                //ed.WriteMessage("Call Rec\n");
                RecSection();

            }
            return false;

        }

        public bool Insert()
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            isConductor = true;
            if (dt.Rows.Count > 0)
            {
                //ed.WriteMessage("i am In First IF\n");
                foreach (DataRow dr1 in dt.Rows)
                {

                    if (dr1["BranchGuid"].ToString() == dtGlobal.Rows[0]["BranchGuid"].ToString())
                    {
                        isConductor = false;
                    }

                }

                //if (dtGlobal.Rows.Count > 1)
                //{


                //ed.WriteMessage("Current Branch guid");
                if (isConductor)
                {
                    //ed.WriteMessage("i am In If Conductor\n");
                    DataRow drNew = dt.NewRow();
                    drNew["ConsolGuid"] = dtGlobal.Rows[0]["ConsolGuid"].ToString();
                    drNew["PoleGuid"] = dtGlobal.Rows[0]["PoleGuid"].ToString();
                    drNew["BranchGuid"] = dtGlobal.Rows[0]["BranchGuid"].ToString(); ;
                    drNew["ConsolObjectId"] = dtGlobal.Rows[0]["ConsolObjectId"].ToString();
                    drNew["SectionNo"] = counterSection.ToString();
                    dt.Rows.Add(drNew);
                    currentBranch = dtGlobal.Rows[0]["BranchGuid"].ToString();
                }
                else
                {
                    //ed.WriteMessage("i am In IsConductor Else\n");
                    DataRow drNew = dt.NewRow();
                    drNew["ConsolGuid"] = dtGlobal.Rows[0]["ConsolGuid"].ToString();
                    drNew["PoleGuid"] = dtGlobal.Rows[0]["PoleGuid"].ToString();
                    drNew["BranchGuid"] = dtGlobal.Rows[1]["BranchGuid"].ToString();
                    drNew["ConsolObjectId"] = dtGlobal.Rows[0]["ConsolObjectId"].ToString();
                    drNew["SectionNo"] = counterSection.ToString();

                    dt.Rows.Add(drNew);
                    currentBranch = dtGlobal.Rows[1]["BranchGuid"].ToString();
                }
                //}
                //else
                //{
                //    ed.WriteMessage("i am In  Else\n");
                //    DataRow drNew = dt.NewRow();
                //    drNew["ConsolGuid"] = dtGlobal.Rows[0]["ConsolGuid"].ToString();
                //    drNew["PoleGuid"] = dtGlobal.Rows[0]["PoleGuid"].ToString();
                //    drNew["BranchGuid"] = dtGlobal.Rows[0]["BranchGuid"].ToString();
                //    drNew["ConsolObjectId"] = dtGlobal.Rows[0]["ConsolObjectId"].ToString();
                //    drNew["SectionNo"] = counterSection.ToString();

                //    dt.Rows.Add(drNew);
                //    currentBranch = dtGlobal.Rows[0]["BranchGuid"].ToString();
                //}


            }
            else
            {
                //ed.WriteMessage("i am In Second Else\n");
                DataRow drNew = dt.NewRow();
                drNew["ConsolGuid"] = dtGlobal.Rows[0]["ConsolGuid"].ToString();
                drNew["PoleGuid"] = dtGlobal.Rows[0]["PoleGuid"].ToString();
                drNew["BranchGuid"] = dtGlobal.Rows[0]["BranchGuid"].ToString();
                drNew["ConsolObjectId"] = dtGlobal.Rows[0]["ConsolObjectId"].ToString();
                drNew["SectionNo"] = counterSection.ToString();
                dt.Rows.Add(drNew);
                currentBranch = dtGlobal.Rows[0]["BranchGuid"].ToString();
            }

            return true;
        }
        public bool IsNotExitConsol()
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            //ed.WriteMessage("I Am In ISExistance1\n");
            DataRow[] dr = dt.Select(" ConsolGuid LIKE '" + dtGlobal.Rows[0]["ConsolGuid"].ToString() + "'");
            //ed.WriteMessage("I Am In IsExistConsol\n");
            if (dr.Length == 0)
            {
                //ed.WriteMessage("True\n");
                return true;
            }
            else
            {
                //ed.WriteMessage("FAlse\n");
                return false;
            }
        }
        #endregion

        #region SectionPart 2
        public void FinalSection()
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            bool Check = true;

            int MaxSec = 0;
            int MaxNo = 0;
            DataRow[] dr1 = null;
            //ed.WriteMessage("CounterSection= "+counterSection.ToString()+"\n");
            for (int i = 1; i < counterSection; i++)
            {
                sectionCollection.Clear(); MaxSec = 0; MaxNo = 0;
                string strSelect = "PoleGuid in (";
                System.Data.DataRow[] dr = dt.Select(" SectionNo=" + i.ToString());
                currentSectionCount = dr.Length;

                foreach (DataRow ar in dr)
                {
                    strSelect += "'" + ar["PoleGuid"].ToString() + "'" + ",";
                }
                strSelect = strSelect.Substring(0, strSelect.Length - 1);
                strSelect += ")";
                //ed.WriteMessage("STRSELECT= " + strSelect + "\n");
                if (dr.Length > 0)
                {
                    for (int j = 1; j < counterSection; j++)
                    {
                        dr1 = dt.Select(" SectionNo=" + j.ToString() + " AND " + strSelect);
                        if (dr.Length == dr1.Length)
                            sectionCollection.Add(j);

                    }

                }

                //ed.WriteMessage("CountOfRow= "+dr1.Length.ToString()+"\n");
                //ed.WriteMessage("SectionCollectionCount= "+sectionCollection.Count.ToString()+"\n");


                for (int k = 0; k <= sectionCollection.Count - 1; k++)
                {
                    DataRow[] drs = dt.Select(" SectionNo=" + sectionCollection[k].ToString());
                    //ed.WriteMessage("DRS= "+drs.Length.ToString()+"\n");
                    if (MaxSec < drs.Length)
                    {
                        MaxSec = drs.Length;
                        MaxNo = Convert.ToInt32(drs[0]["SectionNo"].ToString());

                    }

                }
                //ed.WriteMessage("MaxSec="+MaxSec.ToString()+"MaxNo= "+MaxNo.ToString()+"\n");
                for (int k = 0; k <= sectionCollection.Count - 1; k++)
                {
                    DataRow[] drFinal = dt.Select(" SectionNo=" + sectionCollection[k].ToString());
                    foreach (DataRow drd in drFinal)
                    {
                        drd["SectionNO"] = MaxNo.ToString();

                        //ed.WriteMessage("AcceptChange  "+drd["SectionNo"].ToString()+"\n");
                    }
                }




            }
        }
        #endregion

        #region Section Part3

        public void TransferSectionToDataBase()
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            //DataRow[] dr = dt.Select(" Distinct PoleGuid");
            //foreach (DataRow dr1 in dr)
            //{
            //    ed.WriteMessage("PoleGuid= {0}  SectionNo= {1}",dr1["PoleGuid"].ToString(),dr1["SectionNo"].ToString()+"\n");
            //}
            ArrayList ProductList = new ArrayList();
            ArrayList PoleSectionList = new ArrayList();

            sectionCollection.Clear();


            bool check = true;
            //###########Extra
            //Atend.Base.Design.DPoleSection.AccessDelete(Atend.Control.Common.SelectedDesignCode);
            //Atend.Base.Design.DSection.AccessDelete(Atend.Control.Common.SelectedDesignCode);
            //######

            for (int i = 0; i < counterSection; i++)
            {
                DataRow[] d = dt.Select(" SectionNo=" + i.ToString());
                if (d.Length > 0)
                {
                    sectionCollection.Add(i.ToString());
                }
            }
            //ed.WriteMessage("CountOfSection= " + sectionCollection.Count.ToString() + "\n");

            for (int i = 0; i <= sectionCollection.Count - 1; i++)
            {
                Atend.Base.Design.DSection section = new Atend.Base.Design.DSection();
                DataRow[] SectionRow = dt.Select("SectionNo=" + sectionCollection[i].ToString());
                //ed.WriteMessage("SectionRow= " + SectionRow.Length.ToString() + "\n");
                ProductList.Clear();
                PoleSectionList.Clear();
                //ed.WriteMessage("Iterate = "+i.ToString()+" \n");
                foreach (DataRow dr in SectionRow)
                {
                    check = true;
                    for (int j = 0; j < ProductList.Count; j++)
                    {
                        if (dr["PoleGuid"].ToString() == ProductList[j].ToString())
                        {
                            check = false;
                        }
                    }
                    //ed.WriteMessage("Befor Check\n");
                    if (check)
                    {
                        Atend.Base.Design.DPoleSection PoleSection = new Atend.Base.Design.DPoleSection();
                        //ed.WriteMessage("i am in the check\n\n");
                        ProductList.Add(dr["PoleGuid"].ToString());
                        //PoleSection.DesignCode = Atend.Control.Common.SelectedDesignCode;
                        PoleSection.ProductCode = new Guid(dr["PoleGuid"].ToString());
                        PoleSection.ProductType = Convert.ToInt32(Atend.Control.Enum.ProductType.Pole);

                        PoleSectionList.Add(PoleSection);
                    }
                    //ed.WriteMessage("Befor Checkaaa\n");

                    if (dr["BranchGuid"].ToString() != "")
                    {
                        Atend.Base.Design.DPoleSection PoleSection = new Atend.Base.Design.DPoleSection();
                        //ed.WriteMessage("BranchGuid= " + dr["BranchGuid"].ToString() + "\n");
                        //PoleSection.DesignCode = Atend.Control.Common.SelectedDesignCode;
                        PoleSection.ProductCode = new Guid(dr["BranchGuid"].ToString());
                        PoleSection.ProductType = Convert.ToInt32(Atend.Control.Enum.ProductType.Conductor);
                        PoleSectionList.Add(PoleSection);
                    }
                }
                section.PoleSection = PoleSectionList;
                section.Number = FindMaxNumber() + 1;
                //ed.WriteMessage("Number= " + section.Number.ToString());
                //section.DesignCode = Atend.Control.Common.SelectedDesignCode;
                //System.Data.DataTable dtSectionNumber = Atend.Base.Design.DSection.selectByDesignCode(Atend.Control.Common.SelectedDesignCode);
                //ed.WriteMessage("Befor Insert\n");
                if (section.AccessInsert())
                {
                    ed.WriteMessage("Inser Success\n");
                }
                else
                {
                    ed.WriteMessage("Error In Insert\n");
                }

            }

            Atend.Global.Acad.UAcad.PoleNumbering();

        }

        public int FindMaxNumber()
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            System.Data.DataTable dtMax = Atend.Base.Design.DSection.AccessSelectAll();
            //ed.WriteMessage("dtMaxCount= "+dtMax.Rows.Count.ToString()+"\n");
            int Max = 0;
            //ed.WriteMessage("findMaxNumber\n");
            foreach (DataRow dr1 in dtMax.Rows)
            {
                //ed.WriteMessage("DBNumber= "+dr1["Number"].ToString()+"\n");
                if (Max < Convert.ToInt32(dr1["Number"].ToString()))
                {
                    Max = Convert.ToInt32(dr1["Number"].ToString());
                }
            }
            //ed.WriteMessage("MaxNumber= " + Max.ToString() + "\n");
            return Max;
        }
        #endregion

        #region Sectionning
        public void Createsection()
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            ed.WriteMessage("I Am In Create Section\n");
            DetermineSection();
            ed.WriteMessage("I Pass PArt1\n");
            FinalSection();
            ed.WriteMessage("I PAss Part2\n");
            TransferSectionToDataBase();
            ed.WriteMessage("I PAss PArt3\n");
        }
        #endregion

    }
    //New Section

    public class Section
    {
        Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
        System.Data.DataTable dtPoleSubList = new System.Data.DataTable();
        public System.Data.DataTable dtBranchList = new System.Data.DataTable();
        public System.Data.DataTable dtGlobal = new System.Data.DataTable();
        public System.Data.DataTable dtStartEnd = new System.Data.DataTable();
        public System.Data.DataTable dtTemp1 = new System.Data.DataTable();


        

        System.Data.DataTable dPackPoleCalamp;
        System.Data.DataTable dPackPoleConsol;
        System.Data.DataTable dSub;
        

        
        int sectionNO;
      
        public Section()
        {
            //dtPoleSubList.Columns.Add("PoleOI");
            //dtPoleSubList.Columns.Add("SubOI");
            //dtPoleSubList.Columns.Add("SubGuid");
            //dtPoleSubList.Columns.Add("Type");
            //dtPoleSubList.Columns.Add("PoleGuid");

            //dtBranchList.Columns.Add("Node1Guid");
            //dtBranchList.Columns.Add("Node2Guid");
            //dtBranchList.Columns.Add("BranchGuid");
            //dtBranchList.Columns.Add("Type");

            dtGlobal.Columns.Add("PoleGuid");
            dtGlobal.Columns.Add("NodeGuid");
            dtGlobal.Columns.Add("BranchGuid");
            dtGlobal.Columns.Add("BranchType");
            dtGlobal.Columns.Add("NodeType");
            dtGlobal.Columns.Add("SectionNo");
            dtGlobal.Columns.Add("PoleType");


            dtTemp1.Columns.Add("PoleGuid");
            dtTemp1.Columns.Add("NodeGuid");
            dtTemp1.Columns.Add("BranchGuid");
            dtTemp1.Columns.Add("BranchType");
            dtTemp1.Columns.Add("NodeType");
            dtTemp1.Columns.Add("SectionNo");
            dtTemp1.Columns.Add("PoleType");

            dtStartEnd.Columns.Add("SectionNo");
            dtStartEnd.Columns.Add("Start");
            dtStartEnd.Columns.Add("End");
            
           

        }
        public void FindListOfPole()
        {
            dPackPoleCalamp = Atend.Base.Design.DPackage.AccessSelectCalamp(Convert.ToInt32(Atend.Control.Enum.ProductType.Kalamp));
            dPackPoleConsol = Atend.Base.Design.DConsol.AccessSelectByType();
            //ed.WriteMessage("dPackForCalamp={0}\n", dPackPoleCalamp.Rows.Count);
            //ed.WriteMessage("dPackConsol={0}\n", dPackPoleConsol.Rows.Count);
        }
        public void DetermineSection()
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            //ed.WriteMessage("DetermineSection\n");
            dtPoleSubList = Atend.Global.Acad.UAcad.FillPoleSubList();
            //foreach (DataRow dr in dtPoleSubList.Rows)
            //{
            //    ed.WriteMessage("PoleGuid={0},SubGuid={1}\n", dr["PoleGuid"].ToString(), dr["SubGuid"].ToString());
            //}
            dtBranchList = Atend.Global.Acad.UAcad.FillBranchList();

            //ed.WriteMessage("DtBranch First={0}\n", dtBranchList.Rows.Count);
            FindListOfPole();
            sectionNO = 1;
            int Type = Convert.ToInt32(Atend.Control.Enum.ProductType.Kalamp);
            foreach (DataRow dr in dPackPoleCalamp.Rows)
            {
                Atend.Base.Design.DPackage dParent = Atend.Base.Design.DPackage.AccessSelectByCode(new Guid(dr["ParentCode"].ToString()));
                //ed.WriteMessage("ParentCode={0}\n", dParent.NodeCode.ToString());
                DataRow[] drs = dtPoleSubList.Select(" PoleGuid Like '" + dParent.NodeCode + "'  AND Type='" + Type + "'");
                if (drs.Length != 0)
                {
                    //ed.WriteMessage("drs.Lenght={0}\n", drs.Length);
                    foreach (DataRow drSub in drs)
                    {
                        //ed.WriteMessage("drSub={0}\n", drSub["SubGuid"].ToString());
                        DataRow[] drGlobal = dtGlobal.Select(" PoleGuid Like '" + drSub["PoleGuid"].ToString() + "' " + " And " + " NodeGuid Like '" + drSub["SubGuid"].ToString() + "'");
                        if (drGlobal.Length == 0)
                        {
                            LineNevigation(drSub, sectionNO, Convert.ToInt32(Atend.Control.Enum.ProductType.SelfKeeper));
                            sectionNO++;
                        }
                    }
                }

            }
            //ed.WriteMessage("*****Go To DPackPoleConsol\n");
            int i = 0;
            foreach (DataRow dr in dPackPoleConsol.Rows)
            {
                //ed.WriteMessage("##ParentCode={0}\n", dr["ParentCode"].ToString());
                DataRow[] drs = dtPoleSubList.Select(" PoleGuid Like '" + dr["ParentCode"].ToString() + "'");
                if (drs.Length != 0)
                {
                    foreach (DataRow drSub in drs)
                    {
                        DataRow[] drGlobal = dtGlobal.Select(" PoleGuid='" + drSub["PoleGuid"].ToString() + "' " + "  And  " + "  NodeGuid='" + drSub["SubGuid"].ToString() + "'");
                        if (drGlobal.Length == 0)
                        {
                            //ed.WriteMessage("i Am in The IF\n");
                            LineNevigation(drSub, sectionNO, Convert.ToInt32(Atend.Control.Enum.ProductType.Conductor));
                            //ed.WriteMessage("i Come Back From LineNavigation\n");
                            sectionNO++;
                        }
                    }
                }

            }
            //ed.WriteMessage(" Finish\n");
            TransferToDataBase();

        }
        public void LineNevigation(DataRow CurrentRow, int SectionNo, int Type)
        {
            Guid SelectedBranch;//= new Guid(CurrentRow["BranchGuid"].ToString());
            Guid guidSub;
            System.Data.DataTable dtTable2;
            //ed.WriteMessage("LineNevigation : CurrentRow[SubGuid]={0}\n", CurrentRow["SubGuid"].ToString());
            //ed.WriteMessage("Type={0} dtBranchList={1}\n", Type, dtBranchList.Rows.Count);
            System.Data.DataTable dtTable1 = Atend.Global.Acad.UAcad.GetNodeBranches(new Guid(CurrentRow["SubGuid"].ToString()), Type, dtBranchList);
            //ed.WriteMessage("dtTable1.rows.count={0}\n", dtTable1.Rows.Count);
            DataRow drStartEnd = dtStartEnd.NewRow();
            if (dtTable1.Rows.Count == 1)
            {
                //ed.WriteMessage("1 SubGuid={0}\n", CurrentRow["SubGuid"].ToString());
                SelectedBranch = new Guid(dtTable1.Rows[0]["BranchGuid"].ToString());
                DataRow dr = dtGlobal.NewRow();
                dr["PoleGuid"] = CurrentRow["PoleGuid"].ToString();
                dr["NodeGuid"] = CurrentRow["SubGuid"].ToString();
                dr["BranchGuid"] = SelectedBranch.ToString();
                dr["BranchType"] = Type.ToString();
                dr["NodeType"] = CurrentRow["Type"].ToString();
                dr["SectionNo"] = SectionNo.ToString();
                dr["PoleType"] = CurrentRow["PoleType"].ToString();
                dtGlobal.Rows.Add(dr);
                //ed.WriteMessage("Start={0}\n",CurrentRow["PoleGuid"].ToString());
                drStartEnd["Start"] = CurrentRow["PoleGuid"].ToString();
                drStartEnd["SectionNo"] = sectionNO.ToString();
                //ed.WriteMessage("Get GuidSub\n");
                guidSub = Atend.Global.Acad.UAcad.GetNextNode(new Guid(CurrentRow["SubGuid"].ToString()), SelectedBranch, dtBranchList);
                while (SelectedBranch != Guid.Empty)
                {
                    //ed.WriteMessage("guidSub={0}\n", guidSub.ToString());
                    DataRow[] drSub = dtPoleSubList.Select(" SubGuid Like '" + guidSub.ToString() + "'");
                    //ed.WriteMessage(" Get NodeBranch\n");
                    dtTable2 = Atend.Global.Acad.UAcad.GetNodeBranches(new Guid(drSub[0]["SubGuid"].ToString()), Type, dtBranchList);
                    //ed.WriteMessage("dtTable2.rows.count={0}\n", dtTable2.Rows.Count);


                    if (dtTable2.Rows.Count == 2)
                    {
                        //ed.WriteMessage("3\n");
                        if (new Guid(dtTable2.Rows[0]["BranchGuid"].ToString()) != SelectedBranch)
                        {
                            //ed.WriteMessage("assign Row 0 SubGuid={0}\n ", drSub[0]["SubGuid"].ToString());
                            SelectedBranch = new Guid(dtTable2.Rows[0]["BranchGuid"].ToString());
                            DataRow dr1 = dtGlobal.NewRow();
                            dr1["PoleGuid"] = drSub[0]["PoleGuid"].ToString();
                            dr1["NodeGuid"] = drSub[0]["SubGuid"].ToString();
                            dr1["BranchGuid"] = SelectedBranch.ToString();
                            dr1["BranchType"] = Type;
                            dr1["NodeType"] = drSub[0]["Type"].ToString();
                            dr1["SectionNo"] = SectionNo.ToString();
                            dr1["PoleType"] = CurrentRow["PoleType"].ToString();

                            dtGlobal.Rows.Add(dr1);

                        }
                        else
                        {
                            //ed.WriteMessage("assign Row 2 SubGuid={0}\n", drSub[0]["SubGuid"].ToString());
                            SelectedBranch = new Guid(dtTable2.Rows[1]["BranchGuid"].ToString());
                            DataRow dr1 = dtGlobal.NewRow();
                            dr1["PoleGuid"] = drSub[0]["PoleGuid"].ToString();
                            dr1["NodeGuid"] = drSub[0]["SubGuid"].ToString();
                            dr1["BranchGuid"] = SelectedBranch.ToString();
                            dr1["BranchType"] = Type;
                            dr1["NodeType"] = drSub[0]["Type"].ToString();
                            dr1["SectionNo"] = SectionNo.ToString();
                            dr1["PoleType"] = CurrentRow["PoleType"].ToString();

                            dtGlobal.Rows.Add(dr1);
                        }
                    }
                    else
                    {
                        //ed.WriteMessage("Assign End Pole SubGUID={0}\n", drSub[0]["SubGuid"].ToString());
                        SelectedBranch = Guid.Empty;
                        DataRow dr1 = dtGlobal.NewRow();
                        dr1["PoleGuid"] = drSub[0]["PoleGuid"].ToString();
                        dr1["NodeGuid"] = drSub[0]["SubGuid"].ToString();
                        dr1["BranchGuid"] = "_";
                        dr1["BranchType"] = Type;
                        dr1["NodeType"] = drSub[0]["Type"].ToString();
                        dr1["SectionNo"] = SectionNo.ToString();
                        dr1["PoleType"] = CurrentRow["PoleType"].ToString();

                        dtGlobal.Rows.Add(dr1);
                        //ed.WriteMessage("END={0}\n", drSub[0]["PoleGuid"].ToString());
                        drStartEnd["End"] = drSub[0]["PoleGuid"].ToString();
                        dtStartEnd.Rows.Add(drStartEnd);
                    }
                    guidSub = Atend.Global.Acad.UAcad.GetNextNode(guidSub, SelectedBranch, dtBranchList);
                    //ed.WriteMessage("SelectedBranch={0}\n", SelectedBranch.ToString());
                }


            }
        }

        private System.Data.DataTable MySelect(System.Data.DataTable dt, int i)
        {
            dtTemp1.Rows.Clear();
            string s = "SectionNo=" + i.ToString();
            //ed.WriteMessage("FILTER = {0} \n", dtGlobal.Select(s).Length);
            //Answer = dt.Select(s);

            int RowCounter = 0;
            foreach (DataRow dr in dt.Rows)
            {
                if (Convert.ToInt32(dr["SectionNo"]) == i)
                {
                    DataRow drTemp = dtTemp1.NewRow();
                    drTemp["PoleGuid"] = dr["PoleGuid"].ToString();
                    drTemp["NodeGuid"] = dr["NodeGuid"].ToString();
                    drTemp["BranchGuid"] = dr["BranchGuid"].ToString();
                    drTemp["BranchType"] = dr["BranchType"].ToString();
                    drTemp["NodeType"] = dr["NodeType"].ToString();
                    drTemp["SectionNo"] = dr["SectionNo"].ToString();
                    drTemp["PoleType"] = dr["PoleType"].ToString();
                    dtTemp1.Rows.Add(drTemp);
                    //Answer[RowCounter] = dr;
                    RowCounter++;
                }
            }

            return dtTemp1;
        }

        private System.Data.DataTable MySelectStartEnd(int i)
        {
            System.Data.DataTable dtStartEndTemp = new System.Data.DataTable();
            dtStartEndTemp.Columns.Add("SectionNo");
            dtStartEndTemp.Columns.Add("Start");
            dtStartEndTemp.Columns.Add("End");
            int RowCounter = 0;
            foreach (DataRow dr in dtStartEnd.Rows)
            {
                if (Convert.ToInt32(dr["SectionNo"]) == i)
                {
                    DataRow drTemp = dtStartEndTemp.NewRow();
                    drTemp["SectionNo"] = dr["SectionNo"].ToString();
                    drTemp["Start"] = dr["Start"].ToString();
                    drTemp["End"] = dr["End"].ToString();
                    //ed.WriteMessage("****Start={0},End={1}\n",dr["Start"].ToString(),dr["End"].ToString());
                    dtStartEndTemp.Rows.Add(drTemp);
                    //Answer[RowCounter] = dr;
                    RowCounter++;
                }
            }
            return dtStartEndTemp;
        }

        public void TransferToDataBase()
        {
            Atend.Base.Design.DSection.AccessDelete();
            Atend.Base.Design.DPoleSection.AccessDelete();
            Atend.Base.Calculating.CStartEnd.AccessDelete();
            Atend.Base.Design.DGlobal.AccessDelete();
            //ed.WriteMessage("dtGlobal.Rows.Count={0},SectionNo={1}\n", dtGlobal.Rows.Count, sectionNO);
            //foreach (DataRow dr in dtGlobal.Rows)
            //{
            //    ed.WriteMessage("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\n");
            //    ed.WriteMessage("PoleGuid={0},SectionNo={1}\n", dr["PoleGuid"].ToString(), dr["SectionNo"].ToString());
            //    ed.WriteMessage("NodeGuid={0} ,NodeType={1}\n", dr["NodeGuid"].ToString(), dr["NodeType"].ToString());
            //    ed.WriteMessage("BranchGuid={0} ,BranchType={1}\n", dr["Branchguid"].ToString(), dr["BranchType"].ToString());
            //}


            //foreach (DataRow dr in dtStartEnd.Rows)
            //{
            //    ed.WriteMessage("Satrt={0},End={1},sectionNo={2}\n",dr["Start"].ToString(),dr["End"].ToString(),dr["SectionNo"].ToString());
            //}

            ArrayList ArrSection = new ArrayList();
            ArrayList ArrGlobal = new ArrayList();
            if (dtGlobal.Rows.Count != 0)
            {
                for (int i = 1; i < sectionNO; i++)
                {
                    ArrSection.Clear();
                    ArrGlobal.Clear();
                    //ed.WriteMessage("\n~~~ 743 ~~~ SNO : {0} : {1} : {2}\n", i, dtGlobal.Rows.Count, sectionNO);
                    string s = "SectionNo=" + i.ToString();
                    //ed.WriteMessage("FILTER = {0} \n", dtGlobal.Select(s).Length);
                    //DataRow[] dtTemp = dtGlobal.Select(s);
                    System.Data.DataTable dtTemp = MySelect(dtGlobal, i);
                    if (dtTemp.Rows.Count > 0)
                    {
                        Atend.Base.Design.DSection section = new Atend.Base.Design.DSection();
                        section.Number = i;
                        //foreach (DataRow dr in dtTemp)
                        //{
                        //    ed.WriteMessage("$$$PoleGuid={0},SectionNo={1}\n", dr["PoleGuid"].ToString(), dr["SectionNo"].ToString());
                        //    ed.WriteMessage("$$$NodeGuid={0} ,NodeType={1}\n", dr["NodeGuid"].ToString(), dr["NodeType"].ToString());
                        //    ed.WriteMessage("$$$BranchGuid={0} ,BranchType={1}\n", dr["Branchguid"].ToString(), dr["BranchType"].ToString());
                        //}
                        //ed.WriteMessage("dtTemp.rows.count={0}\n", dtTemp.Length);
                        foreach (DataRow dr in dtTemp.Rows)
                        {
                            //ed.WriteMessage("I Am In The For \n");
                            Atend.Base.Design.DGlobal global = new Atend.Base.Design.DGlobal();
                            if (dr["BranchGuid"].ToString() != "_")
                            {
                                global.BranchGuid = new Guid(dr["BranchGuid"].ToString());
                            }
                            else
                            {
                                global.BranchGuid = Guid.Empty;

                            }
                            global.BranchType = Convert.ToInt32(dr["BranchType"].ToString());
                            global.NodeGuid = new Guid(dr["NodeGuid"].ToString());
                            global.NodeType = Convert.ToInt32(dr["NodeType"].ToString());
                            global.PoleGuid = new Guid(dr["PoleGuid"].ToString());
                            global.PoleType = Convert.ToInt32(dr["PoleType"].ToString());
                            global.SectionNo = Convert.ToInt32(dr["SectionNo"].ToString());

                            ArrGlobal.Add(global);

                            Atend.Base.Design.DPoleSection PoleSEction = new Atend.Base.Design.DPoleSection();
                            PoleSEction.ProductCode = new Guid(dr["PoleGuid"].ToString());
                            PoleSEction.ProductType = Convert.ToInt32(dr["PoleType"].ToString());
                            ArrSection.Add(PoleSEction);
                            //ed.WriteMessage("Add Pole\n");
                            if (dr["BranchGuid"].ToString() != "_")
                            {
                                Atend.Base.Design.DPoleSection PoleSEctionBranch = new Atend.Base.Design.DPoleSection();

                                PoleSEctionBranch.ProductCode = new Guid(dr["BranchGuid"].ToString());
                                PoleSEctionBranch.ProductType = Convert.ToInt32(dr["BranchType"].ToString());
                                ArrSection.Add(PoleSEctionBranch);
                                //ed.WriteMessage("Add Branch\n");
                            }
                        }
                        //ed.WriteMessage("ArrSection.Count={0}\n", ArrSection.Count);
                        //for (int j = 0; j < ArrSection.Count; j++)
                        //{
                        //    Atend.Base.Design.DPoleSection dSec = (Atend.Base.Design.DPoleSection)ArrSection[j];
                        //    //ed.WriteMessage("***ProductCode={0}\n", dSec.ProductCode.ToString());
                        //    //ed.WriteMessage("***Producttype={0}\n", dSec.ProductType.ToString());
                        //    //ed.WriteMessage("***SecCode={0}\n", dSec.SectionCode);

                        //}
                        section.PoleSection = ArrSection;
                        section.GlobalSection = ArrGlobal;
                        section.AccessInsert();
                        //DataRow[] drStartEnd = dtStartEnd.Select(" SectionNo=" + i.ToString());
                        System.Data.DataTable drStartEnd = MySelectStartEnd(i);
                        Atend.Base.Calculating.CStartEnd startEnd = new Atend.Base.Calculating.CStartEnd();
                        //ed.WriteMessage("StartEnd S={0} E={1},SectionCode={2}\n", drStartEnd[0]["Start"].ToString(), drStartEnd[0]["End"].ToString(), section.Code);
                        if (drStartEnd.Rows.Count > 0)
                        {
                            startEnd.SectionCode = section.Code;
                            startEnd.EndPole = new Guid(drStartEnd.Rows[0]["End"].ToString());
                            startEnd.StartPole = new Guid(drStartEnd.Rows[0]["Start"].ToString());
                            startEnd.AccessInsert();
                        }
                    }
                    else
                    {
                        ed.WriteMessage("سکشنی با این شماره موجود نیست" + "\n");
                    }
                }
            }
            else
            {
                ed.WriteMessage("سکشن یافت نشد\n");
            }
            //ed.WriteMessage("****StartEnd={0}\n",dtStartEnd.Rows.Count);
            //foreach (DataRow dr in dtStartEnd.Rows)
            //{
            //    Atend.Base.Calculating.CStartEnd startEnd = new Atend.Base.Calculating.CStartEnd();
            //    ed.WriteMessage("StartEnd S={0} E={1}\n",dr["Start"].ToString(),dr["End"].ToString());
            //    startEnd.EndPole = new Guid(dr["End"].ToString());
            //    startEnd.StartPole = new Guid(dr["Start"].ToString());
            //    startEnd.AccessInsert();
            //}
            ed.WriteMessage("FINISH\n");
        }




        //////public void FindListOfPole02()
        //////{
        //////    dPackPoleCalamp = Atend.Base.Design.DPackage.AccessSelectCalamp(Convert.ToInt32(Atend.Control.Enum.ProductType.Kalamp),aConnection);
        //////    dPackPoleConsol = Atend.Base.Design.DConsol.AccessSelectByType(aConnection);
        //////    //ed.WriteMessage("dPackForCalamp={0}\n", dPackPoleCalamp.Rows.Count);
        //////    //ed.WriteMessage("dPackConsol={0}\n", dPackPoleConsol.Rows.Count);
        //////}
        //////public void DetermineSection02()
        //////{
        //////    Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
        //////    //ed.WriteMessage("DetermineSection\n");
        //////    dtPoleSubList = Atend.Global.Acad.UAcad.FillPoleSubList();
        //////    //foreach (DataRow dr in dtPoleSubList.Rows)
        //////    //{
        //////    //    ed.WriteMessage("PoleGuid={0},SubGuid={1}\n", dr["PoleGuid"].ToString(), dr["SubGuid"].ToString());
        //////    //}
        //////    dtBranchList = Atend.Global.Acad.UAcad.FillBranchList();

        //////    //ed.WriteMessage("DtBranch First={0}\n", dtBranchList.Rows.Count);
        //////    FindListOfPole02();
        //////    sectionNO = 1;
        //////    int Type = Convert.ToInt32(Atend.Control.Enum.ProductType.Kalamp);
        //////    foreach (DataRow dr in dPackPoleCalamp.Rows)
        //////    {
        //////        Atend.Base.Design.DPackage dParent = Atend.Base.Design.DPackage.AccessSelectByCode(new Guid(dr["ParentCode"].ToString()),aConnection);
        //////        //ed.WriteMessage("ParentCode={0}\n", dParent.NodeCode.ToString());
        //////        DataRow[] drs = dtPoleSubList.Select(" PoleGuid Like '" + dParent.NodeCode + "'  AND Type='" + Type + "'");
        //////        if (drs.Length != 0)
        //////        {
        //////            //ed.WriteMessage("drs.Lenght={0}\n", drs.Length);
        //////            foreach (DataRow drSub in drs)
        //////            {
        //////                //ed.WriteMessage("drSub={0}\n", drSub["SubGuid"].ToString());
        //////                DataRow[] drGlobal = dtGlobal.Select(" PoleGuid Like '" + drSub["PoleGuid"].ToString() + "' " + " And " + " NodeGuid Like '" + drSub["SubGuid"].ToString() + "'");
        //////                if (drGlobal.Length == 0)
        //////                {
        //////                    LineNevigation02(drSub, sectionNO, Convert.ToInt32(Atend.Control.Enum.ProductType.SelfKeeper));
        //////                    sectionNO++;
        //////                }
        //////            }
        //////        }

        //////    }
        //////    //ed.WriteMessage("*****Go To DPackPoleConsol\n");
        //////    int i = 0;
        //////    foreach (DataRow dr in dPackPoleConsol.Rows)
        //////    {
        //////        //ed.WriteMessage("##ParentCode={0}\n", dr["ParentCode"].ToString());
        //////        DataRow[] drs = dtPoleSubList.Select(" PoleGuid Like '" + dr["ParentCode"].ToString() + "'");
        //////        if (drs.Length != 0)
        //////        {
        //////            foreach (DataRow drSub in drs)
        //////            {
        //////                DataRow[] drGlobal = dtGlobal.Select(" PoleGuid='" + drSub["PoleGuid"].ToString() + "' " + "  And  " + "  NodeGuid='" + drSub["SubGuid"].ToString() + "'");
        //////                if (drGlobal.Length == 0)
        //////                {
        //////                    //ed.WriteMessage("i Am in The IF\n");
        //////                    LineNevigation02(drSub, sectionNO, Convert.ToInt32(Atend.Control.Enum.ProductType.Conductor));
        //////                    //ed.WriteMessage("i Come Back From LineNavigation\n");
        //////                    sectionNO++;
        //////                }
        //////            }
        //////        }

        //////    }
        //////    //ed.WriteMessage(" Finish\n");
        //////    TransferToDataBase02();

        //////}
        //////public void LineNevigation02(DataRow CurrentRow, int SectionNo, int Type)
        //////{
        //////    Guid SelectedBranch;//= new Guid(CurrentRow["BranchGuid"].ToString());
        //////    Guid guidSub;
        //////    System.Data.DataTable dtTable2;
        //////    //ed.WriteMessage("LineNevigation : CurrentRow[SubGuid]={0}\n", CurrentRow["SubGuid"].ToString());
        //////    //ed.WriteMessage("Type={0} dtBranchList={1}\n", Type, dtBranchList.Rows.Count);
        //////    System.Data.DataTable dtTable1 = Atend.Global.Acad.UAcad.GetNodeBranches(new Guid(CurrentRow["SubGuid"].ToString()), Type, dtBranchList);
        //////    //ed.WriteMessage("dtTable1.rows.count={0}\n", dtTable1.Rows.Count);
        //////    DataRow drStartEnd = dtStartEnd.NewRow();
        //////    if (dtTable1.Rows.Count == 1)
        //////    {
        //////        //ed.WriteMessage("1 SubGuid={0}\n", CurrentRow["SubGuid"].ToString());
        //////        SelectedBranch = new Guid(dtTable1.Rows[0]["BranchGuid"].ToString());
        //////        DataRow dr = dtGlobal.NewRow();
        //////        dr["PoleGuid"] = CurrentRow["PoleGuid"].ToString();
        //////        dr["NodeGuid"] = CurrentRow["SubGuid"].ToString();
        //////        dr["BranchGuid"] = SelectedBranch.ToString();
        //////        dr["BranchType"] = Type.ToString();
        //////        dr["NodeType"] = CurrentRow["Type"].ToString();
        //////        dr["SectionNo"] = SectionNo.ToString();
        //////        dr["PoleType"] = CurrentRow["PoleType"].ToString();
        //////        dtGlobal.Rows.Add(dr);
        //////        //ed.WriteMessage("Start={0}\n",CurrentRow["PoleGuid"].ToString());
        //////        drStartEnd["Start"] = CurrentRow["PoleGuid"].ToString();
        //////        drStartEnd["SectionNo"] = sectionNO.ToString();
        //////        //ed.WriteMessage("Get GuidSub\n");
        //////        guidSub = Atend.Global.Acad.UAcad.GetNextNode(new Guid(CurrentRow["SubGuid"].ToString()), SelectedBranch, dtBranchList);
        //////        while (SelectedBranch != Guid.Empty)
        //////        {
        //////            //ed.WriteMessage("guidSub={0}\n", guidSub.ToString());
        //////            DataRow[] drSub = dtPoleSubList.Select(" SubGuid Like '" + guidSub.ToString() + "'");
        //////            //ed.WriteMessage(" Get NodeBranch\n");
        //////            dtTable2 = Atend.Global.Acad.UAcad.GetNodeBranches(new Guid(drSub[0]["SubGuid"].ToString()), Type, dtBranchList);
        //////            //ed.WriteMessage("dtTable2.rows.count={0}\n", dtTable2.Rows.Count);


        //////            if (dtTable2.Rows.Count == 2)
        //////            {
        //////                //ed.WriteMessage("3\n");
        //////                if (new Guid(dtTable2.Rows[0]["BranchGuid"].ToString()) != SelectedBranch)
        //////                {
        //////                    //ed.WriteMessage("assign Row 0 SubGuid={0}\n ", drSub[0]["SubGuid"].ToString());
        //////                    SelectedBranch = new Guid(dtTable2.Rows[0]["BranchGuid"].ToString());
        //////                    DataRow dr1 = dtGlobal.NewRow();
        //////                    dr1["PoleGuid"] = drSub[0]["PoleGuid"].ToString();
        //////                    dr1["NodeGuid"] = drSub[0]["SubGuid"].ToString();
        //////                    dr1["BranchGuid"] = SelectedBranch.ToString();
        //////                    dr1["BranchType"] = Type;
        //////                    dr1["NodeType"] = drSub[0]["Type"].ToString();
        //////                    dr1["SectionNo"] = SectionNo.ToString();
        //////                    dr1["PoleType"] = CurrentRow["PoleType"].ToString();

        //////                    dtGlobal.Rows.Add(dr1);

        //////                }
        //////                else
        //////                {
        //////                    //ed.WriteMessage("assign Row 2 SubGuid={0}\n", drSub[0]["SubGuid"].ToString());
        //////                    SelectedBranch = new Guid(dtTable2.Rows[1]["BranchGuid"].ToString());
        //////                    DataRow dr1 = dtGlobal.NewRow();
        //////                    dr1["PoleGuid"] = drSub[0]["PoleGuid"].ToString();
        //////                    dr1["NodeGuid"] = drSub[0]["SubGuid"].ToString();
        //////                    dr1["BranchGuid"] = SelectedBranch.ToString();
        //////                    dr1["BranchType"] = Type;
        //////                    dr1["NodeType"] = drSub[0]["Type"].ToString();
        //////                    dr1["SectionNo"] = SectionNo.ToString();
        //////                    dr1["PoleType"] = CurrentRow["PoleType"].ToString();

        //////                    dtGlobal.Rows.Add(dr1);
        //////                }
        //////            }
        //////            else
        //////            {
        //////                //ed.WriteMessage("Assign End Pole SubGUID={0}\n", drSub[0]["SubGuid"].ToString());
        //////                SelectedBranch = Guid.Empty;
        //////                DataRow dr1 = dtGlobal.NewRow();
        //////                dr1["PoleGuid"] = drSub[0]["PoleGuid"].ToString();
        //////                dr1["NodeGuid"] = drSub[0]["SubGuid"].ToString();
        //////                dr1["BranchGuid"] = "_";
        //////                dr1["BranchType"] = Type;
        //////                dr1["NodeType"] = drSub[0]["Type"].ToString();
        //////                dr1["SectionNo"] = SectionNo.ToString();
        //////                dr1["PoleType"] = CurrentRow["PoleType"].ToString();

        //////                dtGlobal.Rows.Add(dr1);
        //////                //ed.WriteMessage("END={0}\n", drSub[0]["PoleGuid"].ToString());
        //////                drStartEnd["End"] = drSub[0]["PoleGuid"].ToString();
        //////                dtStartEnd.Rows.Add(drStartEnd);
        //////            }
        //////            guidSub = Atend.Global.Acad.UAcad.GetNextNode(guidSub, SelectedBranch, dtBranchList);
        //////            //ed.WriteMessage("SelectedBranch={0}\n", SelectedBranch.ToString());
        //////        }


        //////    }
        //////}
        //////private System.Data.DataTable MySelect02(System.Data.DataTable dt, int i)
        //////{
        //////    dtTemp1.Rows.Clear();
        //////    string s = "SectionNo=" + i.ToString();
        //////    //ed.WriteMessage("FILTER = {0} \n", dtGlobal.Select(s).Length);
        //////    //Answer = dt.Select(s);

        //////    int RowCounter = 0;
        //////    foreach (DataRow dr in dt.Rows)
        //////    {
        //////        if (Convert.ToInt32(dr["SectionNo"]) == i)
        //////        {
        //////            DataRow drTemp = dtTemp1.NewRow();
        //////            drTemp["PoleGuid"] = dr["PoleGuid"].ToString();
        //////            drTemp["NodeGuid"] = dr["NodeGuid"].ToString();
        //////            drTemp["BranchGuid"] = dr["BranchGuid"].ToString();
        //////            drTemp["BranchType"] = dr["BranchType"].ToString();
        //////            drTemp["NodeType"] = dr["NodeType"].ToString();
        //////            drTemp["SectionNo"] = dr["SectionNo"].ToString();
        //////            drTemp["PoleType"] = dr["PoleType"].ToString();
        //////            dtTemp1.Rows.Add(drTemp);
        //////            //Answer[RowCounter] = dr;
        //////            RowCounter++;
        //////        }
        //////    }

        //////    return dtTemp1;
        //////}
        //////private System.Data.DataTable MySelectStartEnd02(int i)
        //////{
        //////    System.Data.DataTable dtStartEndTemp = new System.Data.DataTable();
        //////    dtStartEndTemp.Columns.Add("SectionNo");
        //////    dtStartEndTemp.Columns.Add("Start");
        //////    dtStartEndTemp.Columns.Add("End");
        //////    int RowCounter = 0;
        //////    foreach (DataRow dr in dtStartEnd.Rows)
        //////    {
        //////        if (Convert.ToInt32(dr["SectionNo"]) == i)
        //////        {
        //////            DataRow drTemp = dtStartEndTemp.NewRow();
        //////            drTemp["SectionNo"] = dr["SectionNo"].ToString();
        //////            drTemp["Start"] = dr["Start"].ToString();
        //////            drTemp["End"] = dr["End"].ToString();
        //////            //ed.WriteMessage("****Start={0},End={1}\n",dr["Start"].ToString(),dr["End"].ToString());
        //////            dtStartEndTemp.Rows.Add(drTemp);
        //////            //Answer[RowCounter] = dr;
        //////            RowCounter++;
        //////        }
        //////    }
        //////    return dtStartEndTemp;
        //////}
        //////public void TransferToDataBase02()
        //////{
        //////    Atend.Base.Design.DSection.AccessDelete(aConnection);
        //////    Atend.Base.Design.DPoleSection.AccessDelete(aConnection);
        //////    Atend.Base.Calculating.CStartEnd.AccessDelete(aConnection);
        //////    Atend.Base.Design.DGlobal.AccessDelete(aConnection);
        //////    //ed.WriteMessage("dtGlobal.Rows.Count={0},SectionNo={1}\n", dtGlobal.Rows.Count, sectionNO);
        //////    //foreach (DataRow dr in dtGlobal.Rows)
        //////    //{
        //////    //    ed.WriteMessage("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\n");
        //////    //    ed.WriteMessage("PoleGuid={0},SectionNo={1}\n", dr["PoleGuid"].ToString(), dr["SectionNo"].ToString());
        //////    //    ed.WriteMessage("NodeGuid={0} ,NodeType={1}\n", dr["NodeGuid"].ToString(), dr["NodeType"].ToString());
        //////    //    ed.WriteMessage("BranchGuid={0} ,BranchType={1}\n", dr["Branchguid"].ToString(), dr["BranchType"].ToString());
        //////    //}


        //////    //foreach (DataRow dr in dtStartEnd.Rows)
        //////    //{
        //////    //    ed.WriteMessage("Satrt={0},End={1},sectionNo={2}\n",dr["Start"].ToString(),dr["End"].ToString(),dr["SectionNo"].ToString());
        //////    //}

        //////    ArrayList ArrSection = new ArrayList();
        //////    ArrayList ArrGlobal = new ArrayList();
        //////    if (dtGlobal.Rows.Count != 0)
        //////    {
        //////        for (int i = 1; i < sectionNO; i++)
        //////        {
        //////            ArrSection.Clear();
        //////            ArrGlobal.Clear();
        //////            //ed.WriteMessage("\n~~~ 743 ~~~ SNO : {0} : {1} : {2}\n", i, dtGlobal.Rows.Count, sectionNO);
        //////            string s = "SectionNo=" + i.ToString();
        //////            //ed.WriteMessage("FILTER = {0} \n", dtGlobal.Select(s).Length);
        //////            //DataRow[] dtTemp = dtGlobal.Select(s);
        //////            System.Data.DataTable dtTemp = MySelect02(dtGlobal, i);
        //////            if (dtTemp.Rows.Count > 0)
        //////            {
        //////                Atend.Base.Design.DSection section = new Atend.Base.Design.DSection();
        //////                section.Number = i;
        //////                //foreach (DataRow dr in dtTemp)
        //////                //{
        //////                //    ed.WriteMessage("$$$PoleGuid={0},SectionNo={1}\n", dr["PoleGuid"].ToString(), dr["SectionNo"].ToString());
        //////                //    ed.WriteMessage("$$$NodeGuid={0} ,NodeType={1}\n", dr["NodeGuid"].ToString(), dr["NodeType"].ToString());
        //////                //    ed.WriteMessage("$$$BranchGuid={0} ,BranchType={1}\n", dr["Branchguid"].ToString(), dr["BranchType"].ToString());
        //////                //}
        //////                //ed.WriteMessage("dtTemp.rows.count={0}\n", dtTemp.Length);
        //////                foreach (DataRow dr in dtTemp.Rows)
        //////                {
        //////                    //ed.WriteMessage("I Am In The For \n");
        //////                    Atend.Base.Design.DGlobal global = new Atend.Base.Design.DGlobal();
        //////                    if (dr["BranchGuid"].ToString() != "_")
        //////                    {
        //////                        global.BranchGuid = new Guid(dr["BranchGuid"].ToString());
        //////                    }
        //////                    else
        //////                    {
        //////                        global.BranchGuid = Guid.Empty;

        //////                    }
        //////                    global.BranchType = Convert.ToInt32(dr["BranchType"].ToString());
        //////                    global.NodeGuid = new Guid(dr["NodeGuid"].ToString());
        //////                    global.NodeType = Convert.ToInt32(dr["NodeType"].ToString());
        //////                    global.PoleGuid = new Guid(dr["PoleGuid"].ToString());
        //////                    global.PoleType = Convert.ToInt32(dr["PoleType"].ToString());
        //////                    global.SectionNo = Convert.ToInt32(dr["SectionNo"].ToString());

        //////                    ArrGlobal.Add(global);

        //////                    Atend.Base.Design.DPoleSection PoleSEction = new Atend.Base.Design.DPoleSection();
        //////                    PoleSEction.ProductCode = new Guid(dr["PoleGuid"].ToString());
        //////                    PoleSEction.ProductType = Convert.ToInt32(dr["PoleType"].ToString());
        //////                    ArrSection.Add(PoleSEction);
        //////                    //ed.WriteMessage("Add Pole\n");
        //////                    if (dr["BranchGuid"].ToString() != "_")
        //////                    {
        //////                        Atend.Base.Design.DPoleSection PoleSEctionBranch = new Atend.Base.Design.DPoleSection();

        //////                        PoleSEctionBranch.ProductCode = new Guid(dr["BranchGuid"].ToString());
        //////                        PoleSEctionBranch.ProductType = Convert.ToInt32(dr["BranchType"].ToString());
        //////                        ArrSection.Add(PoleSEctionBranch);
        //////                        //ed.WriteMessage("Add Branch\n");
        //////                    }
        //////                }
        //////                //ed.WriteMessage("ArrSection.Count={0}\n", ArrSection.Count);
        //////                //for (int j = 0; j < ArrSection.Count; j++)
        //////                //{
        //////                //    Atend.Base.Design.DPoleSection dSec = (Atend.Base.Design.DPoleSection)ArrSection[j];
        //////                //    //ed.WriteMessage("***ProductCode={0}\n", dSec.ProductCode.ToString());
        //////                //    //ed.WriteMessage("***Producttype={0}\n", dSec.ProductType.ToString());
        //////                //    //ed.WriteMessage("***SecCode={0}\n", dSec.SectionCode);

        //////                //}
        //////                section.PoleSection = ArrSection;
        //////                section.GlobalSection = ArrGlobal;
        //////                section.AccessInsert();
        //////                //DataRow[] drStartEnd = dtStartEnd.Select(" SectionNo=" + i.ToString());
        //////                System.Data.DataTable drStartEnd = MySelectStartEnd(i);
        //////                Atend.Base.Calculating.CStartEnd startEnd = new Atend.Base.Calculating.CStartEnd();
        //////                //ed.WriteMessage("StartEnd S={0} E={1},SectionCode={2}\n", drStartEnd[0]["Start"].ToString(), drStartEnd[0]["End"].ToString(), section.Code);
        //////                if (drStartEnd.Rows.Count>0 )
        //////                {
        //////                    startEnd.SectionCode = section.Code;
        //////                    startEnd.EndPole = new Guid(drStartEnd.Rows[0]["End"].ToString());
        //////                    startEnd.StartPole = new Guid(drStartEnd.Rows[0]["Start"].ToString());
        //////                    startEnd.AccessInsert();
        //////                }
        //////            }
        //////            else
        //////            {
        //////                ed.WriteMessage("سکشنی با این شماره موجود نیست" + "\n");
        //////            }
        //////        }
        //////    }
        //////    else
        //////    {
        //////        ed.WriteMessage("سکشن یافت نشد\n");
        //////    }
        //////    //ed.WriteMessage("****StartEnd={0}\n",dtStartEnd.Rows.Count);
        //////    //foreach (DataRow dr in dtStartEnd.Rows)
        //////    //{
        //////    //    Atend.Base.Calculating.CStartEnd startEnd = new Atend.Base.Calculating.CStartEnd();
        //////    //    ed.WriteMessage("StartEnd S={0} E={1}\n",dr["Start"].ToString(),dr["End"].ToString());
        //////    //    startEnd.EndPole = new Guid(dr["End"].ToString());
        //////    //    startEnd.StartPole = new Guid(dr["Start"].ToString());
        //////    //    startEnd.AccessInsert();
        //////    //}
        //////    ed.WriteMessage("FINISH\n");
        //////}
    }

}