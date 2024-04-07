using Auto_Foundation.Figure.View;
using Auto_Foundation.Figure.ViewModel;
using Autodesk.AutoCAD.Runtime;

public class Command
{
    [CommandMethod("GTF")]
    public void DrawFDN()
    {
        CombineViewModel viewModel = new CombineViewModel();
        CombineWindow window = new CombineWindow(viewModel);
        window.ShowDialog();
    }
}
