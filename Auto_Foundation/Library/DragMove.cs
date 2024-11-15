using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.GraphicsInterface;
using iTextSharp.text.pdf;
using static iTextSharp.text.pdf.events.IndexEvents;

public class DragMove
{
    private Document _dwg;
    private SelectionSet _ss;

    private Point3d _basePoint;
    private Line _rubberLine = null;
    private double _scale = 1;

    public DragMove(Document dwg, SelectionSet ss, Point3d basePoint, double scale)
    {
        _basePoint = basePoint;
        _dwg = dwg;
        _ss = ss;
        _scale = scale;
    }

    #region public methods
    public void DoDrag()
    {
        if (_ss.Count == 0) return;
        _rubberLine = null;

        using (var tr = _dwg.TransactionManager.StartTransaction())
        {
            SetHighlight(true, tr, _scale);

            PromptPointResult ppr = _dwg.Editor.Drag(
                _ss,
                "\nmove to: ",
                delegate(Point3d pt, ref Matrix3d mat)
                {
                    if (pt == _basePoint) 
                    {
                        return SamplerStatus.NoChange;
                    }
                    else
                    {
                        if (_rubberLine == null)
                        {
                            _rubberLine = new Line(_basePoint, pt);
                            _rubberLine.SetDatabaseDefaults(_dwg.Database);

                            IntegerCollection intCol = new IntegerCollection();
                            TransientManager.CurrentTransientManager.AddTransient(_rubberLine, TransientDrawingMode.DirectShortTerm, 128, intCol);
                        }
                        else
                        {
                            _rubberLine.EndPoint = pt;

                            IntegerCollection intCol = new IntegerCollection();
                            TransientManager.CurrentTransientManager.UpdateTransient(_rubberLine, intCol);
                        }

                        mat = Matrix3d.Displacement(_basePoint.GetVectorTo(pt));
                        return SamplerStatus.OK;
                    }
                }
            );

            if (_rubberLine != null)
            {
                IntegerCollection intCol = new IntegerCollection();
                TransientManager.CurrentTransientManager.EraseTransient(_rubberLine, intCol);
                _rubberLine.Dispose();
                _rubberLine= null;
            }

            if (ppr.Status == PromptStatus.Cancel)
            {
                foreach (ObjectId id in _ss.GetObjectIds())
                {
                    var ent = (Entity)tr.GetObject(id, OpenMode.ForWrite);
                    ent.Erase();
                }
            }
            else if (ppr.Status == PromptStatus.OK) 
            {
                MoveObjects(ppr.Value, tr);
            }

            SetHighlight(false, tr, _scale);
            tr.Commit();
        }
    }
    #endregion
    #region private methods
    private void MoveObjects(Point3d pt, Transaction tr)
    {
        Matrix3d mat = Matrix3d.Displacement(_basePoint.GetVectorTo(pt));
        foreach(ObjectId id in _ss.GetObjectIds())
        {
            var ent = (Entity)tr.GetObject(id,OpenMode.ForWrite);
            ent.TransformBy(mat);
        }
    }
    private void SetHighlight(bool highlight, Transaction tr, double scale)
    {
        foreach (ObjectId id in _ss.GetObjectIds())
        { 
            Entity ent = (Entity)tr.GetObject(id, OpenMode.ForWrite);
            if (highlight)
            {
                Matrix3d scaleMatrix = Matrix3d.Scaling(scale, _basePoint);
                ent.TransformBy(scaleMatrix);
                ent.Highlight();
            }
            else ent.Unhighlight();
        }
    }
    #endregion
}