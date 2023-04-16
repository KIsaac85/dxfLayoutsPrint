using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.PlottingServices;

namespace dxfLayoutsPrint
{
    public class dxfPrint : IExtensionApplication
    {
        #region Commands

        [CommandMethod("dxfp")]
        public void printDXF()
        {
            // Get the current document and database, and start a transaction
            Document acDoc = Application.DocumentManager.MdiActiveDocument;
            Database acCurDb = acDoc.Database;

            using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
            {
                // Reference the Layout Manager
                LayoutManager acLayoutMgr = LayoutManager.Current;



                DBDictionary lays =
             acTrans.GetObject(acCurDb.LayoutDictionaryId,
                 OpenMode.ForRead) as DBDictionary;

                acDoc.Editor.WriteMessage("\nLayouts:");

                // Step through and list each named layout and Model
                foreach (DBDictionaryEntry item in lays)
                {
                   // acDoc.Editor.WriteMessage("\n  " + item.Key);
                    
                    // Get the current layout and output its name in the Command Line window
                    Layout acLayout = acTrans.GetObject(item.m_value,OpenMode.ForRead) as Layout;
                    
                    if (acLayout.LayoutName!="Model")
                    {
                        acDoc.Editor.WriteMessage("\n  " + acLayout.LayoutName);
                        
                    }
                }

                // Abort the changes to the database
                acTrans.Abort();
            }
        }

        #endregion
        public void Initialize()
        {
           
        }

        public void Terminate()
        {
            
        }
    }
}
