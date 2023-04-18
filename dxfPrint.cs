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
            DocumentCollection documents = Application.DocumentManager;

            foreach (Document item in documents)
            {
                //Database acCurDb = acDoc.Database;
                string dwgName = item.Name;
                using (var db = new Database(false, true))
                {
                    db.ReadDwgFile(dwgName, FileOpenMode.OpenForReadAndAllShare, true, null);
                    string dxfName = System.IO.Path.ChangeExtension(dwgName, "dxf");
                    db.DxfOut(dxfName, 16, true);
                } 
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
