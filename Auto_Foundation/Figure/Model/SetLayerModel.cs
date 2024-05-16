using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.EditorInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auto_Foundation.Figure.Model
{
    public class SetLayerModel
    {
        private SelectionSet _ss;
        public SelectionSet ss
        {
            get { return _ss; }
            set { _ss = value; }
        }
        public void SetLayersOnOff()
        {

            ss = ClCAD.GetSelctionFromUser();
            if (ss != null)
            {
                ClCAD.HideLayers(ss);
            }
            else
            {
                Application.DocumentManager.MdiActiveDocument.Editor.WriteMessage("\nthere is no selection");
                return;
            }
        }
        public void ShowAllLayers()
        {
            ClCAD.ShowAllLayers();
        }
        public void GetXrefLayer()
        {
            ClCAD.GetXrefFilePath();
        }
        //public void TestMethod()
        //{
        //    ClCAD.ListPrinters();
        //}
    }
}
