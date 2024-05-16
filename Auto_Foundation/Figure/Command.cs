using Auto_Foundation.Figure.Model;
using Auto_Foundation.Figure.View;
using Auto_Foundation.Figure.ViewModel;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;

public class Command
{
    SetLayerModel sm = new SetLayerModel();

    [CommandMethod("GTF")]
    public void DrawFDN()
    {
        CombineViewModel viewModel = new CombineViewModel();
        CombineWindow window = new CombineWindow(viewModel);
        window.ShowDialog();
    }
    [CommandMethod("GTL", CommandFlags.UsePickSet|CommandFlags.Redraw)]
    public void SetLayersOnOff()
    {
        sm.SetLayersOnOff();
    }
    [CommandMethod("GTS")]
    public void ShowAllLayers()
    {
        sm.ShowAllLayers();
    }
    [CommandMethod("GTX")]
    public void GetXrefLayers()
    {
        sm.GetXrefLayer();
    }
    [CommandMethod("TestGT")]
    //public void TestMethod()
    //{
    //    sm.TestMethod();
    //}


    [CommandMethod("GTMPL")]
    public void MultiPlotTest()
    {
        MultiPlotViewModel viewModel = new MultiPlotViewModel();
        MultiPlot window = new MultiPlot(viewModel);
        window.ShowDialog();
    }
}
