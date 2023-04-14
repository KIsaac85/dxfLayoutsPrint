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

namespace dxfLayoutsPrint
{
    public class dxfPrint : IExtensionApplication
    {
        #region Commands

        [CommandMethod("dxfp")]
        public void printDXF()
        {
            //Get the current document and database
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database database = doc.Database;
            
            // Get the layout dictionary of the current database
            using (Transaction acadTrans = database.TransactionManager.StartTransaction())
            {
                //Get the name of layouts
                DBDictionary lays =
                    acadTrans.GetObject(database.LayoutDictionaryId,
                        OpenMode.ForRead) as DBDictionary;

                doc.Editor.WriteMessage("\nLayouts:");

                // Step through and list each named layout and Model
                foreach (DBDictionaryEntry item in lays)
                {
                    doc.Editor.WriteMessage("\n  " + item.m_value);
                }

                // Abort the changes to the database
                acadTrans.Abort();



                //print the required layouts
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
