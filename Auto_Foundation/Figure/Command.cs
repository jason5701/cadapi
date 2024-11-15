using Auto_Foundation.Figure.Model;
using Auto_Foundation.Figure.View;
using Auto_Foundation.Figure.ViewModel;
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
    public void SetLayersOff()
    {
        sm.SetLayersOff();
    }
    [CommandMethod("GTLF", CommandFlags.UsePickSet | CommandFlags.Redraw)]
    public void SetLayerFrozen()
    {
        sm.SetLayerFrozen();
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
    [CommandMethod("GTFAV")]
    public void PropertiesCopyToObject()
    {
        ChangeLayersViewModel viewModel = new ChangeLayersViewModel();
        ChangeLayers window = new ChangeLayers(viewModel);
        window.ShowDialog();
    }
    [CommandMethod("GTEWI")]
    public void CreateWipeout()
    {
        ClCAD.SelectObjectWipeout();
    }

    [CommandMethod("GTEXTR")]
    public void ObjectXclip()
    {
        XclipViewModel viewModel = new XclipViewModel();
        XClipWindow window = new XClipWindow(viewModel);
        window.ShowDialog();
    }
    [CommandMethod("GTDEX")]
    public void MakeSameNodes()
    {
        ClCAD.ModifyDimensionStartPoint();
    }
    [CommandMethod("GTATBL")]
    public void SetAlignedTableText()
    {
        SetAlignedTableTextViewModel viewModel = new SetAlignedTableTextViewModel();
        SetAlignedTableText window = new SetAlignedTableText(viewModel);
        window.ShowDialog();
    }

    [CommandMethod("GTMPL")]
    public void MultiPlotTest()
    {
        MultiPlotViewModel viewModel = new MultiPlotViewModel();
        MultiPlot window = new MultiPlot(viewModel);
        window.ShowDialog();
    }
}
