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
                LayoutManager acLayoutMgr = LayoutManager.Current;
                // Step through and list each named layout and Model
                foreach (DBDictionaryEntry item in lays)
                {

                    Layout acLayout = acadTrans.GetObject(acLayoutMgr.GetLayoutId(item.Key),
                                                    OpenMode.ForRead) as Layout;
                    doc.Editor.WriteMessage("\n  " + acLayout.LayoutName);
                    using (PlotInfo plotInfo = new PlotInfo())
                    {
                        plotInfo.Layout = acLayout.ObjectId;
                        // Get a copy of the PlotSettings from the layout
                        using (PlotSettings plotSettings = new PlotSettings(acLayout.ModelType))
                        {
                            plotSettings.CopyFrom(acLayout);
                            // Update the PlotSettings object
                            PlotSettingsValidator plotSettingsVdr = PlotSettingsValidator.Current;

                            // Set the plot type
                            plotSettingsVdr.SetPlotType(plotSettings, Autodesk.AutoCAD.DatabaseServices.PlotType.Extents);

                            // Set the plot scale
                            plotSettingsVdr.SetUseStandardScale(plotSettings, true);
                            plotSettingsVdr.SetStdScaleType(plotSettings, StdScaleType.ScaleToFit);

                            // Center the plot
                            plotSettingsVdr.SetPlotCentered(plotSettings, true);

                            // Set the plot device to use
                            plotSettingsVdr.SetPlotConfigurationName(plotSettings, "DWF6 ePlot.pc3", "ANSI_A_(8.50_x_11.00_Inches)");

                            // Set the plot info as an override since it will
                            // not be saved back to the layout
                            plotInfo.OverrideSettings = plotSettings;

                            // Validate the plot info
                            using (PlotInfoValidator plotInfoVdr = new PlotInfoValidator())
                            {
                                plotInfoVdr.MediaMatchingPolicy = MatchingPolicy.MatchEnabled;
                                //plotInfoVdr.Validate(plotInfo);

                                // Check to see if a plot is already in progress
                                if (PlotFactory.ProcessPlotState == ProcessPlotState.NotPlotting)
                                {
                                    using (PlotEngine acPlEng = PlotFactory.CreatePublishEngine())
                                    {
                                        // Track the plot progress with a Progress dialog
                                        using (PlotProgressDialog acPlProgDlg = new PlotProgressDialog(false, 1, true))
                                        {
                                            using ((acPlProgDlg))
                                            {
                                                // Define the status messages to display 
                                                // when plotting starts
                                                acPlProgDlg.set_PlotMsgString(PlotMessageIndex.DialogTitle, "Plot Progress");
                                                acPlProgDlg.set_PlotMsgString(PlotMessageIndex.CancelJobButtonMessage, "Cancel Job");
                                                acPlProgDlg.set_PlotMsgString(PlotMessageIndex.CancelSheetButtonMessage, "Cancel Sheet");
                                                acPlProgDlg.set_PlotMsgString(PlotMessageIndex.SheetSetProgressCaption, "Sheet Set Progress");
                                                acPlProgDlg.set_PlotMsgString(PlotMessageIndex.SheetProgressCaption, "Sheet Progress");

                                                // Set the plot progress range
                                                acPlProgDlg.LowerPlotProgressRange = 0;
                                                acPlProgDlg.UpperPlotProgressRange = 100;
                                                acPlProgDlg.PlotProgressPos = 0;

                                                // Display the Progress dialog
                                                acPlProgDlg.OnBeginPlot();
                                                acPlProgDlg.IsVisible = true;

                                                // Start to plot the layout
                                                acPlEng.BeginPlot(acPlProgDlg, null);

                                                // Define the plot output
                                                acPlEng.BeginDocument(plotInfo, acLayout.LayoutName, null, 1, true, "c:\\myplot");

                                                // Display information about the current plot
                                                acPlProgDlg.set_PlotMsgString(PlotMessageIndex.Status, "Plotting: " + acLayout.LayoutName + " - " + acLayout.LayoutName);

                                                // Set the sheet progress range
                                                acPlProgDlg.OnBeginSheet();
                                                acPlProgDlg.LowerSheetProgressRange = 0;
                                                acPlProgDlg.UpperSheetProgressRange = 100;
                                                acPlProgDlg.SheetProgressPos = 0;

                                                // Plot the first sheet/layout
                                                using (PlotPageInfo acPlPageInfo = new PlotPageInfo())
                                                {
                                                    acPlEng.BeginPage(acPlPageInfo, plotInfo, true, null);
                                                }

                                                acPlEng.BeginGenerateGraphics(null);
                                                acPlEng.EndGenerateGraphics(null);

                                                // Finish plotting the sheet/layout
                                                acPlEng.EndPage(null);
                                                acPlProgDlg.SheetProgressPos = 100;
                                                acPlProgDlg.OnEndSheet();

                                                // Finish plotting the document
                                                acPlEng.EndDocument(null);

                                                // Finish the plot
                                                acPlProgDlg.PlotProgressPos = 100;
                                                acPlProgDlg.OnEndPlot();
                                                acPlEng.EndPlot(null);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                
                }

                // Abort the changes to the database
                acadTrans.Abort();



                //print the required layouts


                // Reference the Layout Manager
                

                // Get the current layout and output its name in the Command Line window
                
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
