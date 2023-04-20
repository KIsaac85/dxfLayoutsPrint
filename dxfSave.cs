using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;


namespace dxfFileOut
{
    public class dxfSave : IExtensionApplication
    {
        #region Commands
        public DocumentCollection documents { get; set; }
        public string dwgName { get; set; }
        public string dxfName { get; set; }



        [CommandMethod("dxfs")]
        public void printDXF()
        {


            // Get the current document and database, and start a transaction
            documents = Application.DocumentManager;

            foreach (Document item in documents)
            {
                //Database acCurDb = acDoc.Database;
                dwgName = item.Name;
                using (Database db = new Database(false, true))
                {
                    db.ReadDwgFile(dwgName, FileOpenMode.OpenForReadAndAllShare, true, null);
                    dxfName = System.IO.Path.ChangeExtension(dwgName, "dxf");
                    db.DxfOut(dxfName, 16, DwgVersion.AC1027);
                    
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
