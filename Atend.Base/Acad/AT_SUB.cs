using System;
using System.Collections.Generic;
using System.Text;

using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;

namespace Atend.Base.Acad
{
    public class AT_SUB
    {


        Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;

        ObjectIdCollection subIdCollection;

        public ObjectIdCollection SubIdCollection
        {
            get { return subIdCollection; }
            set { subIdCollection = value; }
        }

        private ObjectId selectedObjectId;

        public ObjectId SelectedObjectId
        {
            get { return selectedObjectId; }
            set { selectedObjectId = value; }
        }

        public AT_SUB(ObjectId CurrentObjectId)
        {
            SubIdCollection = new ObjectIdCollection();
            SelectedObjectId = CurrentObjectId;

        }

        public AT_SUB()
        {
            SubIdCollection = new ObjectIdCollection();

        }

        public void Insert()
        {

            //ed.writeMessage("Insert Sub \n");
            Database db = Application.DocumentManager.MdiActiveDocument.Database;
            using (DocumentLock docLock = Application.DocumentManager.MdiActiveDocument.LockDocument())
            {

                using (Transaction tr = db.TransactionManager.StartTransaction())
                {

                    DBObject dbObject = tr.GetObject(SelectedObjectId, OpenMode.ForRead);

                    if (dbObject.ExtensionDictionary == ObjectId.Null)
                    {

                        dbObject.UpgradeOpen();

                        dbObject.CreateExtensionDictionary();

                    }

                    Xrecord xrec = new Xrecord();

                    ResultBuffer rb = new ResultBuffer();

                    int i = 0;
                    foreach (ObjectId id in SubIdCollection)
                    {

                        //rb.Add(new TypedValue((int)DxfCode.SoftPointerId + i, id));
                        rb.Add(new TypedValue((int)DxfCode.HardPointerId + i, id));
                        i++;
                    }

                    xrec.Data = rb;

                    DBDictionary extDict = (DBDictionary)tr.GetObject(dbObject.ExtensionDictionary, OpenMode.ForWrite);

                    extDict.SetAt("AT_SUB", xrec);

                    tr.AddNewlyCreatedDBObject(xrec, true);

                    tr.Commit();

                }
            }
            //ed.writeMessage("Insert Sub finished \n");
        }

        private static void Delete(ObjectId SelectedOI , string DictionaryName)
        {
            //ed.writeMessage("Insert Sub \n");
            Database db = Application.DocumentManager.MdiActiveDocument.Database;
            using (DocumentLock docLock = Application.DocumentManager.MdiActiveDocument.LockDocument())
            {

                using (Transaction tr = db.TransactionManager.StartTransaction())
                {

                    DBObject dbObject = tr.GetObject(SelectedOI, OpenMode.ForWrite);

                    if (dbObject.ExtensionDictionary != ObjectId.Null)
                    {

                        dbObject.UpgradeOpen();

                        DBDictionary extDict = (DBDictionary)tr.GetObject(dbObject.ExtensionDictionary, OpenMode.ForWrite);

                        extDict.Remove(DictionaryName);

                    }

                    //Xrecord xrec = new Xrecord();

                    //ResultBuffer rb = new ResultBuffer();

                    //int i = 0;
                    //foreach (ObjectId id in SubIdCollection)
                    //{

                    //    //rb.Add(new TypedValue((int)DxfCode.SoftPointerId + i, id));
                    //    rb.Add(new TypedValue((int)DxfCode.HardPointerId + i, id));
                    //    i++;
                    //}

                    //xrec.Data = rb;

                    //DBDictionary extDict = (DBDictionary)tr.GetObject(dbObject.ExtensionDictionary, OpenMode.ForWrite);

                    //extDict.SetAt("AT_SUB", xrec);

                    //tr.AddNewlyCreatedDBObject(xrec, true);

                    tr.Commit();

                }
            }
            //ed.writeMessage("Insert Sub finished \n");
        }

        public static AT_SUB SelectBySelectedObjectId(ObjectId SelectedObjectId)
        {

            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;

            AT_SUB at_SUB = new AT_SUB();

            Database db = Application.DocumentManager.MdiActiveDocument.Database;
            using (DocumentLock docLock = Application.DocumentManager.MdiActiveDocument.LockDocument())
            {

                using (Transaction tr = db.TransactionManager.StartTransaction())
                {

                    DBObject ent = tr.GetObject(SelectedObjectId, OpenMode.ForRead);

                    if (ent.ExtensionDictionary != ObjectId.Null)
                    {

                        DBDictionary ExtDict = (DBDictionary)tr.GetObject(ent.ExtensionDictionary, OpenMode.ForRead);

                        if (ExtDict.Contains("AT_SUB"))
                        {
                            at_SUB.SelectedObjectId = SelectedObjectId;
                            Xrecord xrec = (Xrecord)tr.GetObject(ExtDict.GetAt("AT_SUB"), OpenMode.ForRead);
                            foreach (TypedValue tv in xrec.Data)
                            {


                                string Temp = tv.Value.ToString().Substring(1, tv.Value.ToString().Length - 2);
                                //ed.WriteMessage("{0} \n", Temp);
                                at_SUB.SubIdCollection.Add(new ObjectId(new IntPtr(int.Parse(Temp))));

                            }

                        }
                        else
                        {
                            //ed.WriteMessage("AT_SUB : NOT EXIST \n");
                        }

                    }

                }
            }
            return at_SUB;

        }

        public static void AddToAT_SUB(ObjectId NewSub, ObjectId Container)
        {
            bool NewSubExisted = false;
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;

            try
            {
                AT_SUB at_sub = AT_SUB.SelectBySelectedObjectId(Container);
                foreach (ObjectId oi in at_sub.SubIdCollection)
                {

                    if (NewSub == oi)
                    {
                        NewSubExisted = true;
                    }
                }
                if (NewSubExisted == false)
                {
                    at_sub.SubIdCollection.Add(NewSub);
                    at_sub.Insert();
                }
            }
            catch
            {
                AT_SUB at_sub = new AT_SUB(Container);
                at_sub.SubIdCollection.Add(NewSub);
                at_sub.Insert();

            }
            //ed.writeMessage("SubInserted Finished\n");

        }

        public static void RemoveFromAT_SUB(ObjectId OldSub, ObjectId Container)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            bool OldSubExisted = false;
            try
            {
                AT_SUB at_sub = AT_SUB.SelectBySelectedObjectId(Container);
                foreach (ObjectId oi in at_sub.SubIdCollection)
                {

                    if (OldSub == oi)
                    {
                        at_sub.SubIdCollection.Remove(oi);
                        OldSubExisted = true;
                    }

                }


                //if (OldSubExisted == true)
                //{

                if (at_sub.SubIdCollection.Count > 0)
                {
                    at_sub.Insert();
                }
                else
                {
                    //int a = 0;
                    Delete(Container, "AT_SUB");
                }

                //}
            }
            catch (System.Exception ex)
            {
                //ed.WriteMessage("Error RemoveFromAT_SUB: " + ex.Message + "\n");
            }

        }


    }
}
