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

        [CommandMethod("dxfprint")]
        public void printDXF()
        {

            Database db = HostApplicationServices.WorkingDatabase;
            Transaction trans = db.TransactionManager.StartTransaction();
            BlockTable blkTbl = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
            BlockTableRecord msBlkRec = trans.GetObject(blkTbl[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as
            BlockTableRecord;
            Point3d pnt1 = new Point3d(0, 0, 0);
            Point3d pnt2 = new Point3d(10, 10, 0);
            Line lineObj = new Line(pnt1, pnt2);
            msBlkRec.AppendEntity(lineObj);
            trans.AddNewlyCreatedDBObject(lineObj, true);
            trans.Commit();
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            //ed.WriteMessage("\nLine Is Created Successfully.");

            PromptPointOptions prPtOpt = new PromptPointOptions("\nSpecify start point: ");
            prPtOpt.AllowArbitraryInput = false;
            prPtOpt.AllowNone = true;

            PromptPointResult prPtRes1 = ed.GetPoint(prPtOpt);
            if (prPtRes1.Status != PromptStatus.OK) return;
            pnt1 = prPtRes1.Value;

            prPtOpt.BasePoint = pnt1;
            prPtOpt.UseBasePoint = true;
            prPtOpt.UseDashedLine = true;
            prPtOpt.Message = "\nSpecify end point:";
            PromptPointResult prPtRes2 = ed.GetPoint(prPtOpt);
            if (prPtRes2.Status != PromptStatus.OK) return;
            pnt2 = prPtRes2.Value;

            AcDbLine line = new AcDbLine();
        }

        #endregion
        public void Initialize()
        {
            throw new NotImplementedException();
        }

        public void Terminate()
        {
            throw new NotImplementedException();
        }
    }
}
