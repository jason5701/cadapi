using Application = Autodesk.AutoCAD.ApplicationServices.Core.Application;
using Autodesk.AutoCAD.Colors;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.PlottingServices;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

public class ClCAD
{
    public static void CreateBlock(Point3d pStart, string blockName)
    {
        var doc = Application.DocumentManager.MdiActiveDocument;
        Database db = doc.Database;

        using (doc.LockDocument())
        using (Transaction tr = db.TransactionManager.StartTransaction())
        {
            GetBlock(blockName);

            BlockTable bt = (BlockTable)tr.GetObject(db.BlockTableId,OpenMode.ForRead);
            BlockTableRecord btr = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);
            BlockReference br = new BlockReference(pStart, bt[blockName]);

            btr.AppendEntity(br);
            tr.AddNewlyCreatedDBObject(br, true);
            tr.Commit();
        }
    }
    public static void GetBlock(string blockName)
    {
        var doc = Application.DocumentManager.MdiActiveDocument;
        using (Database openDb = new Database(false, true))
        {
            string path = "D:/test/repos/training/Auto_Foundation/referenceBlock/blocks.dwg";
            openDb.ReadDwgFile(path, System.IO.FileShare.ReadWrite, true, "");

            ObjectIdCollection ids = new ObjectIdCollection();
            using (Transaction tr = openDb.TransactionManager.StartTransaction())
            {
                BlockTable bt = (BlockTable)tr.GetObject(openDb.BlockTableId, OpenMode.ForRead);
                if (bt.Has(blockName))
                {
                    ids.Add(bt[blockName]);
                }
                tr.Commit();
                if (ids.Count != 0)
                {
                    Database db = doc.Database;
                    IdMapping iMap = new IdMapping();
                    db.WblockCloneObjects(ids, db.BlockTableId, iMap, DuplicateRecordCloning.Ignore, false);
                }
            }
        }
    }
    public static void CreateDimStyles(DimStyleSettings ds)
    {
        var doc = Application.DocumentManager.MdiActiveDocument;
        Database db = doc.Database;

        using (doc.LockDocument())
        using (Transaction tr = db.TransactionManager.StartTransaction())
        {
            DimStyleTable dimStyleTable = (DimStyleTable)tr.GetObject(db.DimStyleTableId, OpenMode.ForWrite);
            TextStyleTable tl = (TextStyleTable)tr.GetObject(db.TextStyleTableId, OpenMode.ForRead);
            if (!dimStyleTable.Has(ds.Name))
            {
                DimStyleTableRecord dtr = new DimStyleTableRecord();
                dtr.Name = ds.Name;

                // Line settings
                dtr.Dimclrd = Color.FromColorIndex(ColorMethod.ByAci, ds.ColorDimLine);
                dtr.Dimclre = Color.FromColorIndex(ColorMethod.ByAci, ds.ColorExtendLine);
                dtr.Dimclrt = Color.FromColorIndex(ColorMethod.ByAci, ds.ColorText);
                dtr.Dimexo = ds.ExtendBeyondTicks;
                dtr.Dimexe = ds.ExtendBeyondDimLine;
                dtr.Dimdli = ds.OffsetFromOrigin;

                // Symbols
                dtr.Dimasz = ds.Arrow_Size;

                // Text settings
                if (tl.Has(ds.NameTextStyle))
                {
                    dtr.Dimtxsty = tl[ds.NameTextStyle];
                }
                dtr.Dimtxt = ds.TextHeight;
                dtr.Dimdle = ds.OffsetFromDimLine;

                // Fit
                dtr.Dimscale = ds.Fit;

                // Primary Units
                dtr.Dimtad = 1; // above text with horizontal dimension
                dtr.Dimtih = false; // above text with vertical dimension
                dtr.Dimlfac = ds.ScaleFactor;
                dtr.Dimatfit = 0;
                dtr.Dimdec = ds.DecimalPoint;
                db.SetDimstyleData(dtr);
                dimStyleTable.Add(dtr);
                tr.AddNewlyCreatedDBObject(dtr, true);
            }

            tr.Commit();
        }
    }
    public static Polyline PlineNetBreak(Point3d p1, Point3d p6, double angle) // angle = 200
    {
        Polyline pl = new Polyline();
        var doc = Application.DocumentManager.MdiActiveDocument;
        Database db = doc.Database;

        using (doc.LockDocument())
        using (Transaction tr = db.TransactionManager.StartTransaction())
        {
            BlockTable bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead);
            BlockTableRecord btr = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);
            pl = new Polyline();
            Vector3d vt = p6 -p1;
            Vector3d vtdv = vt.GetNormal();
            double width = vt.Length;
            Vector3d vtdv_angle = vt.CrossProduct(Vector3d.ZAxis).GetNormal();
            Point3d p2 = p1 + vtdv.MultiplyBy(width / 2) - vtdv.MultiplyBy(angle / 2);
            Point3d p3 = p2 + vtdv_angle.MultiplyBy(angle);
            Point3d p4 = p6 + vtdv.MultiplyBy(width / 2) - vtdv.MultiplyBy(angle / 2);
            Point3d p5 = p4 + vtdv_angle.MultiplyBy(angle);
            pl.AddVertexAt(0, new Point2d(p1.X, p1.Y), 0, 0, 0);
            pl.AddVertexAt(1, new Point2d(p2.X, p2.Y), 0, 0, 0);
            pl.AddVertexAt(2, new Point2d(p3.X, p3.Y), 0, 0, 0);
            pl.AddVertexAt(3, new Point2d(p4.X, p4.Y), 0, 0, 0);
            pl.AddVertexAt(4, new Point2d(p5.X, p5.Y), 0, 0, 0);
            pl.AddVertexAt(5, new Point2d(p6.X, p6.Y), 0, 0, 0);
            pl.SetDatabaseDefaults();
            btr.AppendEntity(pl);
            tr.AddNewlyCreatedDBObject(pl, true);
            tr.Commit();
        }
        return pl;
    }
    public static void CreatehatchFromListPointP(List<Point3d> listPoint, string nameHatch, double patternScale)
    {
        var doc = Application.DocumentManager.MdiActiveDocument;
        Database db = doc.Database;

        using (doc.LockDocument())
        using (Transaction tr = db.TransactionManager.StartTransaction())
        {
            BlockTable bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead);
            BlockTableRecord btr = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);
            Polyline pl = new Polyline();
            pl.SetDatabaseDefaults();
            for(int i = 0; i < listPoint.Count; i++)
            {
                Point3d pt = listPoint[i];
                pl.AddVertexAt(i, new Point2d(pt.X, pt.Y), 0, 0, 0);
            }
            pl.Closed= true;
            btr.AppendEntity(pl);
            tr.AddNewlyCreatedDBObject(pl, true);
            ObjectIdCollection objId = new ObjectIdCollection { pl.ObjectId };
            Hatch hc = new Hatch();

            hc.SetDatabaseDefaults();
            hc.PatternScale = patternScale;
            hc.SetHatchPattern(HatchPatternType.CustomDefined, nameHatch);
            btr.AppendEntity(hc);
            tr.AddNewlyCreatedDBObject(hc, true);

            hc.AppendLoop(HatchLoopTypes.Outermost, objId);
            hc.EvaluateHatch(true);

            hc.Associative = true;

            tr.Commit();
        }
    }
    public static void CreatePolylineFromListPoints(List<Point3d> dsP, bool close, string linetypeName, short colorIndex)
    {
        var doc = Application.DocumentManager.MdiActiveDocument;
        Database db = doc.Database;

        using (doc.LockDocument())
        using (Transaction tr = db.TransactionManager.StartTransaction())
        {
            LinetypeTable lt = (LinetypeTable)tr.GetObject(db.LinetypeTableId, OpenMode.ForRead);
            BlockTable bt = tr.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
            BlockTableRecord btr = tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
            Polyline pl = new Polyline();
            
            for (int i = 0; i < dsP.Count; i++)
            {
                pl.AddVertexAt(i, new Point2d(dsP[i].X, dsP[i].Y), 0, 0, 0);
            }
            if (close && dsP.Count > 1)
            {
                pl.Closed = true;
            }
            if (!lt.Has(linetypeName))
            {
                db.LoadLineTypeFile(linetypeName, "acadiso.lin");
            }
            pl.LinetypeId = lt[linetypeName];
            pl.ColorIndex = colorIndex;
            btr.AppendEntity(pl);
            tr.AddNewlyCreatedDBObject(pl, true);
            tr.Commit();
        }
    }
    public static void CreatePolylineFromListPoints(List<Point3d> dsP, bool close)
    {
        var doc = Application.DocumentManager.MdiActiveDocument;
        Database db = doc.Database;

        using (doc.LockDocument())
        using (Transaction tr = db.TransactionManager.StartTransaction())
        {
            BlockTable bt = tr.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
            BlockTableRecord btr = tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
            Polyline pl = new Polyline();
            for(int i = 0; i < dsP.Count; i++)
            {
                pl.AddVertexAt(i, new Point2d(dsP[i].X, dsP[i].Y),0,0,0);
            }
            if (close && dsP.Count>1)
            {
                pl.Closed = true;
            }

            btr.AppendEntity(pl);
            tr.AddNewlyCreatedDBObject(pl, true);
            tr.Commit();
        }
    }
    public static void CreateLine(Point3d P1, Point3d P2, double scale)
    {
        var doc = Application.DocumentManager.MdiActiveDocument;
        Database db = doc.Database;

        using (doc.LockDocument())
        using (Transaction tr = db.TransactionManager.StartTransaction())
        {
            BlockTable bt = tr.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
            BlockTableRecord btr = tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
            if(scale > 0)
            {
                db.Ltscale = scale;
            }
            Line l = new Line(P1, P2);

            btr.AppendEntity(l);
            tr.AddNewlyCreatedDBObject(l, true);
            tr.Commit();
        }
    }
    public static void CreateLine(Point3d P1, Point3d P2)
    {
        var doc = Application.DocumentManager.MdiActiveDocument;
        Database db = doc.Database;

        using (doc.LockDocument())
        using (Transaction tr = db.TransactionManager.StartTransaction())
        {
            BlockTable bt = tr.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
            BlockTableRecord btr = tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

            Line l = new Line(P1,P2);

            btr.AppendEntity(l);
            tr.AddNewlyCreatedDBObject(l, true);
            tr.Commit();
        }
    }
    public static void CreateTextStyle(string nameTextStyle, string nameFont, double textSize, double xScale, bool isSHX, bool isBold)
    {
        var doc = Application.DocumentManager.MdiActiveDocument;
        Database db = doc.Database;

        using (doc.LockDocument())
        using (Transaction tr = db.TransactionManager.StartTransaction())
        {
            TextStyleTable tt = (TextStyleTable)tr.GetObject(db.TextStyleTableId, OpenMode.ForWrite);
            if(!tt.Has(nameTextStyle))
            {
                TextStyleTableRecord ttr = new TextStyleTableRecord()
                {
                    Name = nameTextStyle,
                    ObliquingAngle = 0,
                    XScale = xScale,
                    IsVertical = false,
                    IsShapeFile = false,
                };
                if (isSHX)
                {
                    ttr.FileName = nameFont;
                    ttr.BigFontFileName = default;
                    ttr.Font = default;
                }
                else
                {
                    ttr.Font = new Autodesk.AutoCAD.GraphicsInterface.FontDescriptor(nameFont, isBold, false, default, default);
                }
                tt.UpgradeOpen();
                tt.Add(ttr);
                tr.AddNewlyCreatedDBObject(ttr, true);
            }
            else
            {
                TextStyleTableRecord ttr = (TextStyleTableRecord)tr.GetObject(tt[nameTextStyle], OpenMode.ForWrite);
                ttr.Name = nameTextStyle;
                ttr.FileName = nameFont;
                if (isSHX)
                {
                    ttr.FileName = nameFont;
                    ttr.BigFontFileName = default;
                    ttr.Font = default;
                }
                else
                {
                    ttr.Font = new Autodesk.AutoCAD.GraphicsInterface.FontDescriptor(nameFont, isBold, false, default, default);
                }
                ttr.ObliquingAngle = 0;
                ttr.XScale = xScale;
                ttr.TextSize = textSize;
                ttr.IsVertical = false;
                ttr.IsShapeFile  = false;
            }
            tr.Commit();
        } 
    }
    public static void CreateDimension_X(List<Point3d> dsX, double dimScale, int step)
    {
        for(int i=0; i < dsX.Count - 1; i++)
        {
            DimX(dsX[i], dsX[i + 1], -step * 700 * dimScale * 0.01);
        }
    }
    public static void CreateDimension_Y(List<Point3d> dsY, double dimScale, int step)
    {
        for (int i = 0; i < dsY.Count - 1; i++)
        {
            DimY(dsY[i], dsY[i + 1], -step * 700 * dimScale * 0.01);
        }
    }
    public static void SetDimStyleCurrent(string styleName)
    {
        var doc = Application.DocumentManager.MdiActiveDocument;
        Database db = doc.Database;

        using (doc.LockDocument())
        using (Transaction tr = db.TransactionManager.StartTransaction())
        {
            DimStyleTable dt = (DimStyleTable)tr.GetObject(db.DimStyleTableId, OpenMode.ForRead);
            if (dt.Has(styleName))
            {
                DimStyleTableRecord dtr = (DimStyleTableRecord)tr.GetObject(dt[styleName], OpenMode.ForWrite);
                dtr.Dispose();
                db.Dimstyle = dt[styleName];
            }
            tr.Commit();
        }
    }
    public static void SetLayerCurrent(string layerName)
    {
        var doc = Application.DocumentManager.MdiActiveDocument;
        Database db = doc.Database;
        using (doc.LockDocument())
        using (Transaction tr = db.TransactionManager.StartTransaction())
        {
            LayerTable lt = (LayerTable)tr.GetObject(db.LayerTableId,OpenMode.ForRead);
            if(lt.Has(layerName))
            {
                db.Clayer = lt[layerName];
            }
            else
            {
                CreateLayer(layerName, 0, "Continuonus", LineWeight.LineWeight005, false);
            }
            tr.Commit();
                
        }
    }
    public static void CreateCircle(Point3d pCenter, double radius, string linetypeName, short colorIndex)
    {
        var doc = Application.DocumentManager.MdiActiveDocument;
        Database db = doc.Database;
        using (doc.LockDocument())
        using (Transaction tr = db.TransactionManager.StartTransaction())
        {
            LinetypeTable lt = (LinetypeTable)tr.GetObject(db.LinetypeTableId,OpenMode.ForRead);
            BlockTableRecord btr = (BlockTableRecord)tr.GetObject(db.CurrentSpaceId, OpenMode.ForWrite);
            Circle c = new Circle();
            c.SetDatabaseDefaults();
            c.Center = pCenter;
            c.Radius = radius;
            if (!lt.Has(linetypeName))
            {
                db.LoadLineTypeFile(linetypeName, "acad.lin");
            }
            c.LinetypeId = lt[linetypeName];
            c.ColorIndex = colorIndex;
            btr.AppendEntity(c);
            tr.AddNewlyCreatedDBObject(c, true);
            tr.Commit();
        }
    }
    public static void CreateCircle(Point3d pCenter, double radius)
    {
        var doc = Application.DocumentManager.MdiActiveDocument;
        Database db = doc.Database;
        using (doc.LockDocument())
        using (Transaction tr = db.TransactionManager.StartTransaction())
        {
            BlockTableRecord btr = (BlockTableRecord)tr.GetObject(db.CurrentSpaceId, OpenMode.ForWrite);
            Circle c = new Circle();
            c.SetDatabaseDefaults();
            c.Center = pCenter;
            c.Radius = radius;
            btr.AppendEntity(c);
            tr.AddNewlyCreatedDBObject(c, true);
            tr.Commit();
        }
    }
    public static Point3d IntersectPoint(Entity l1, Entity l2)
    {
        Point3dCollection pts = new Point3dCollection();
        l1.IntersectWith(l2, Intersect.ExtendArgument, pts, IntPtr.Zero, IntPtr.Zero);
        foreach (Point3d point1 in pts) return point1;
        return new Point3d(-10001, 22122, 25333);
    }
    public static Circle CreateCircleReturnCircle(Point3d pCenter, double radius)
    {
        Circle c =new Circle();
        c.SetDatabaseDefaults();
        c.Center = pCenter;
        c.Radius = radius;
        return c;
    }
    public static Point3d MiddlePoint(Point3d p1, Point3d p2)
    {
        return new Point3d(p1.X / 2 + p2.X / 2, p1.Y / 2 + p2.Y / 2, 0);
    }
    public static Point3d? GetPointsFromUser(string param)
    {
        PromptPointResult ppr = Application.DocumentManager.MdiActiveDocument.
            Editor.GetPoint(new PromptPointOptions("\n" + param));
        if (ppr.Status != PromptStatus.OK) return null;// new Point3d(-111, -123, -147);
        else return ppr.Value;
    }
    public static SelectionSet GetSelctionFromUser()
    {
        var doc = Application.DocumentManager.MdiActiveDocument;

        PromptSelectionResult psr = doc.Editor.SelectImplied();
        if (psr.Status != PromptStatus.OK) 
        {
            psr = doc.Editor.GetSelection();
            return psr.Value; 
        }
        return psr.Value;
    }
    public static void CreateLayer(string layerName, short color, string linetypeName, LineWeight lineWeight, bool canPrint)
    {
        var doc = Application.DocumentManager.MdiActiveDocument;
        Database db = doc.Database;
        using (doc.LockDocument())
        using (Transaction tr = db.TransactionManager.StartTransaction())
        {
            LayerTable layertable = (LayerTable)tr.GetObject(db.LayerTableId, OpenMode.ForRead);
            LinetypeTable linetable = (LinetypeTable)tr.GetObject(db.LinetypeTableId, OpenMode.ForRead);
            LayerTableRecord ltr = new LayerTableRecord();
            try
            {
                if (!linetable.Has(linetypeName)) { 
                    db.LoadLineTypeFile(linetypeName, "acad.lin");
                    ltr.LinetypeObjectId = linetable[linetypeName];
                }
                if (!linetable.Has(linetypeName)) linetypeName = "Continuous";
            }
            catch
            {

            }

            if (!layertable.Has(layerName))
            {
                ltr.Name = layerName;
                ltr.Color = Color.FromColorIndex(ColorMethod.ByAci, color);
                ltr.LineWeight = lineWeight;
                layertable.UpgradeOpen();
                layertable.Add(ltr);
                tr.AddNewlyCreatedDBObject(ltr, true);
            }
            tr.Commit();
        }
    }
    public static void DimX(Point3d P1, Point3d P2, double Y1)
    {
        if (P1.X == P2.X) return;
        var doc = Application.DocumentManager.MdiActiveDocument;
        Database db = doc.Database;
        using (doc.LockDocument())
        using (Transaction tr = db.TransactionManager.StartTransaction())
        {
            BlockTable bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead);
            BlockTableRecord btr = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);
            RotatedDimension rd = new RotatedDimension();
            rd.SetDatabaseDefaults();
            rd.XLine1Point = P1;
            rd.XLine2Point = P2;
            rd.Rotation = 0;
            double Y;
            if (Y1 > 0) Y = Math.Max(P1.Y, P2.Y);
            else Y = Math.Min(P1.Y, P2.Y);
            rd.DimLinePoint = new Point3d((P1.X + P2.X) / 2, Y + Y1, 0);
            rd.DimensionStyle = db.Dimstyle;
            btr.AppendEntity(rd);
            tr.AddNewlyCreatedDBObject(rd, true);
            tr.Commit();
        }
    }
    public static void DimX(Point3d P1, Point3d P2, double Y1, string TextValue)
    {
        if (P1.X == P2.X) return;
        var doc = Application.DocumentManager.MdiActiveDocument;
        Database db = doc.Database;
        using (doc.LockDocument())
        using (Transaction tr = db.TransactionManager.StartTransaction())
        {
            BlockTable bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead);
            BlockTableRecord btr = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);
            RotatedDimension rd = new RotatedDimension();
            rd.SetDatabaseDefaults();
            rd.XLine1Point = P1;
            rd.XLine2Point = P2;
            rd.Rotation = 0;
            double Y;
            if (Y1 > 0) Y = Math.Max(P1.Y, P2.Y);
            else Y = Math.Min(P1.Y, P2.Y);
            rd.DimLinePoint = new Point3d((P1.X + P2.X) / 2, Y + Y1, 0);
            rd.DimensionStyle = db.Dimstyle;
            rd.DimensionText = TextValue;
            btr.AppendEntity(rd);
            tr.AddNewlyCreatedDBObject(rd, true);
            tr.Commit();
        }
    }
    public static void DimX(double X1, double X2, double Y, double Y1)
    {
        if (X1 == X2) return;
        var doc = Application.DocumentManager.MdiActiveDocument;
        Database db = doc.Database;
        using (doc.LockDocument())
        using (Transaction tr = db.TransactionManager.StartTransaction())
        {
            BlockTable bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead);
            BlockTableRecord btr = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);
            RotatedDimension rd = new RotatedDimension();
            rd.SetDatabaseDefaults();
            rd.XLine1Point = new Point3d(X1, Y, 0);
            rd.XLine2Point = new Point3d(X2, Y, 0);
            rd.Rotation = 0;
            rd.DimLinePoint = new Point3d((X1 + X2) / 2, Y + Y1, 0);
            rd.DimensionStyle = db.Dimstyle;
            btr.AppendEntity(rd);
            tr.AddNewlyCreatedDBObject(rd, true);
            tr.Commit();
        }
    }
    public static void DimX(double X1,double X2, double Y,double Y1,string TextValue)
    {
        if (X1 == X2) return;
        var doc = Application.DocumentManager.MdiActiveDocument;
        Database db = doc.Database;
        using (doc.LockDocument())
        using (Transaction tr = db.TransactionManager.StartTransaction())
        {
            BlockTable bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead);
            BlockTableRecord btr = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);
            RotatedDimension rd = new RotatedDimension();
            rd.SetDatabaseDefaults();
            rd.XLine1Point = new Point3d(X1, Y, 0);
            rd.XLine2Point = new Point3d(X2, Y, 0);
            rd.Rotation = 0;
            rd.DimLinePoint = new Point3d((X1 + X2) / 2, Y + Y1, 0);
            rd.DimensionStyle = db.Dimstyle;
            rd.DimensionText = TextValue;
            btr.AppendEntity(rd);
            tr.AddNewlyCreatedDBObject(rd, true);
            tr.Commit();
        }
    }
    public static void DimY(Point3d P1, Point3d P2, double X1)
    {
        if (P1.Y == P2.Y) return;
        var doc = Application.DocumentManager.MdiActiveDocument;
        Database db = doc.Database;
        using (doc.LockDocument())
        using (Transaction tr = db.TransactionManager.StartTransaction())
        {
            BlockTable bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead);
            BlockTableRecord btr = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);
            RotatedDimension rd = new RotatedDimension();
            rd.SetDatabaseDefaults();
            rd.XLine1Point = P1;
            rd.XLine2Point = P2;
            rd.Rotation = Math.PI / 2;
            double X;
            if (X1 > 0) X = Math.Max(P1.X, P2.X);
            else X = Math.Min(P1.X, P2.X);
            rd.DimLinePoint = new Point3d(X + X1, (P1.Y + P2.Y) / 2, 0);
            rd.DimensionStyle = db.Dimstyle;
            btr.AppendEntity(rd);
            tr.AddNewlyCreatedDBObject(rd, true);
            tr.Commit();
        }
    }
    public static void DimY(Point3d P1, Point3d P2, double X1, string TextValue)
    {
        if (P1.Y == P2.Y) return;
        var doc = Application.DocumentManager.MdiActiveDocument;
        Database db = doc.Database;
        using (doc.LockDocument())
        using (Transaction tr = db.TransactionManager.StartTransaction())
        {
            BlockTable bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead);
            BlockTableRecord btr = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);
            RotatedDimension rd = new RotatedDimension();
            rd.SetDatabaseDefaults();
            rd.XLine1Point = P1;
            rd.XLine2Point = P2;
            rd.Rotation = Math.PI / 2;
            double X;
            if (X1 > 0) X = Math.Max(P1.X, P2.X);
            else X = Math.Min(P1.X, P2.X);
            rd.DimLinePoint = new Point3d(X + X1, (P1.Y + P2.Y) / 2, 0);
            rd.DimensionStyle = db.Dimstyle;
            rd.DimensionText = TextValue;
            btr.AppendEntity(rd);
            tr.AddNewlyCreatedDBObject(rd, true);
            tr.Commit();
        }
    }
    public static void DimY(double Y1, double Y2, double X, double X1)
    {
        if (Y1 == Y2) return;
        var doc = Application.DocumentManager.MdiActiveDocument;
        Database db = doc.Database;
        using (doc.LockDocument())
        using (Transaction tr = db.TransactionManager.StartTransaction())
        {
            BlockTable bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead);
            BlockTableRecord btr = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);
            RotatedDimension rd = new RotatedDimension();
            rd.SetDatabaseDefaults();
            rd.XLine1Point = new Point3d(X, Y1, 0);
            rd.XLine2Point = new Point3d(X, Y2, 0);
            rd.Rotation = Math.PI / 2;
            rd.DimLinePoint = new Point3d(X + X1, (Y1 + Y2) / 2, 0);
            rd.DimensionStyle = db.Dimstyle;
            btr.AppendEntity(rd);
            tr.AddNewlyCreatedDBObject(rd, true);
            tr.Commit();
        }
    }
    public static void DimY(double Y1, double Y2, double X, double X1, string TextValue)
    {
        if (Y1 == Y2) return;
        var doc = Application.DocumentManager.MdiActiveDocument;
        Database db = doc.Database;
        using (doc.LockDocument())
        using (Transaction tr = db.TransactionManager.StartTransaction())
        {
            BlockTable bt = (BlockTable)tr.GetObject(db.BlockTableId,OpenMode.ForRead);
            BlockTableRecord btr = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.ModelSpace],OpenMode.ForWrite);
            RotatedDimension rd = new RotatedDimension();
            rd.SetDatabaseDefaults();
            rd.XLine1Point = new Point3d(X, Y1, 0);
            rd.XLine2Point = new Point3d(X, Y2, 0);
            rd.Rotation = Math.PI / 2;
            rd.DimLinePoint = new Point3d(X+X1,(Y1+Y2)/2,0);
            rd.DimensionStyle = db.Dimstyle;
            rd.DimensionText = TextValue;
            btr.AppendEntity(rd);
            tr.AddNewlyCreatedDBObject(rd, true);
            tr.Commit();
        }
    }
    //public static void ZoomAll()
    //{
    //    var app = AcadApplication;
    //    app.GetType().InvokeMember("ZoomExtents", BindingFlags.InvokeMethod, null, app, null);
    //}
    public class DimStyleSettings
    {
        public string Name { get; set; } = "DTL";
        //Line
        public double ExtendBeyondTicks { get; set; } = 100;
        public double ExtendBeyondDimLine { get; set; } = 150;
        public double OffsetFromOrigin { get; set; } = 150;
        public short ColorDimLine { get; set; } = 2;
        public short ColorExtendLine { get; set; } = 2;

        //Symbols
        public double Arrow_Size { get; set; } = 62.5;
        //Text
        public string NameTextStyle { get; set; } = "TestTextStyle3";
        public double TextHeight { get; set; } = 200;
        public short ColorText { get; set; } = 2;
        public double OffsetFromDimLine { get; set; } = 120;
        //Fit
        public double Fit { get; set; } = 1;
        //Primary Units
        public double ScaleFactor { get; set; } = 1;
        public int DecimalPoint { get; set; } = 0;
    }
    public static void HideLayers(SelectionSet ss)
    {
        var doc = Application.DocumentManager.MdiActiveDocument;
        Database db = doc.Database;

        using(doc.LockDocument())
        using(Transaction tr = db.TransactionManager.StartTransaction())
        {
            HashSet<ObjectId> layers = new HashSet<ObjectId>();

            foreach (SelectedObject o in ss)
            {
                Entity ent = (Entity)tr.GetObject(o.ObjectId, OpenMode.ForRead);
                if(ent != null)
                {
                    LayerTableRecord ltr = (LayerTableRecord)tr.GetObject(ent.LayerId, OpenMode.ForWrite);
                    if(ltr != null && !ltr.IsFrozen && !ltr.IsOff) // !layers.Contains(ltr.ObjectId))
                    {
                        ltr.IsOff = true;
                        doc.Editor.WriteMessage($"\n'{ltr.Name}' is hidden.");
                        layers.Add(ltr.ObjectId);
                    }
                }
            }

            tr.Commit();
        }
    }
    public static void ShowAllLayers()
    {
        var doc = Application.DocumentManager.MdiActiveDocument;
        Database db = doc.Database;

        using(doc.LockDocument())
        using(Transaction tr = db.TransactionManager.StartTransaction())
        {
            LayerTable lt = (LayerTable)tr.GetObject(db.LayerTableId, OpenMode.ForRead);
            db.Clayer = lt["0"];

            foreach(ObjectId o in lt)
            {
                LayerTableRecord ltr = (LayerTableRecord)tr.GetObject(o, OpenMode.ForRead);
                if (ltr.Name != "0")
                {
                    ltr.UpgradeOpen();

                    ltr.IsOff = false;
                }
            }
            tr.Commit();
        }
    }
    public static List<string> GetXrefLayers(string xrefPath)
    {
        var doc = Application.DocumentManager.MdiActiveDocument;
        Database db = doc.Database;
        List<string> layers = new List<string>();

        using (doc.LockDocument())
        using(Transaction tr = db.TransactionManager.StartTransaction())
        {
            Database xrefDb = new Database(false, true);
            xrefDb.ReadDwgFile(xrefPath, FileOpenMode.OpenForReadAndAllShare, true, "");
            using(Transaction xrefTr = xrefDb.TransactionManager.StartTransaction())
            {
                LayerTable lt = (LayerTable)xrefTr.GetObject(xrefDb.LayerTableId, OpenMode.ForRead);
                foreach(ObjectId o in lt)
                {
                    if (o.ObjectClass.IsDerivedFrom(RXClass.GetClass(typeof(LayerTableRecord))))
                    {
                        LayerTableRecord ltr = (LayerTableRecord)xrefTr.GetObject(o, OpenMode.ForRead);
                        layers.Add(ltr.Name);
                    }

                }
                xrefTr.Commit();
            }
            tr.Commit();
        }
        return layers;
    }
    public static void GetXrefFilePath() // string fileName
    {
        var doc = Application.DocumentManager.MdiActiveDocument;
        Database db = doc.Database;
        List<string> layers = new List<string>();

        using (doc.LockDocument())
        using(Transaction tr = db.TransactionManager.StartTransaction())
        {
            BlockTable bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead);
            foreach(ObjectId id in bt)
            {
                BlockTableRecord btr = (BlockTableRecord)tr.GetObject(id,OpenMode.ForRead);
                if (btr.IsFromExternalReference)
                {
                    layers = GetXrefLayers(btr.PathName); 
                }
            }
            if (layers.Count > 0)
            {
                PromptKeywordOptions pko = new PromptKeywordOptions("\nHide or Change Color(Press h or c)");
                pko.AllowNone = true;
                pko.Keywords.Add("h");
                pko.Keywords.Add("c");

                PromptResult pr = doc.Editor.GetKeywords(pko);
                LayerTable lt = (LayerTable)tr.GetObject(db.LayerTableId, OpenMode.ForRead);
                if(pr.Status == PromptStatus.OK)
                {
                    if(pr.StringResult.Equals("h", StringComparison.OrdinalIgnoreCase))
                    {
                        foreach (ObjectId o in lt)
                        {
                            LayerTableRecord ltr = (LayerTableRecord)tr.GetObject(o, OpenMode.ForWrite);

                            foreach (string layer in layers)
                            {
                                if (ltr.Name.Contains(layer))
                                {
                                    if (ltr != null && !ltr.IsFrozen && !ltr.IsOff)
                                    {
                                        ltr.IsOff = true;
                                        doc.Editor.WriteMessage($"\n{ltr.Name} is hidden");
                                    }
                                }
                            }
                        }
                        doc.Editor.WriteMessage($"\nhidden count: ${layers.Count}");
                    }
                    else if(pr.StringResult.Equals("c", StringComparison.OrdinalIgnoreCase))
                    {
                        doc.Editor.WriteMessage("\nTodo change color");
                    }
                }
                
                tr.Commit();
            }

        }
    }



    #region Test Method
    public static string GetCurrentDocName()
    {
        var doc = Application.DocumentManager.MdiActiveDocument;

        return doc.Name;
    }
    public static void ProcessDwg(string path, Action<Database> action)
    {
        using (var db = new Database(false, true))
        {
            db.ReadDwgFile(path, FileOpenMode.OpenForReadAndAllShare, false, null);
            using (new WorkingDatabase(db))
            {
                action(db);
            }
        }
    }
    public static List<string> SearchObjectByBlock(
        DrawClass obj,
        string blockName, 
        string device, 
        string paper, 
        string styleName,
        bool isCombineFile
        )
    {
        List<string> pdfPath = new List<string>();
        var ed = Application.DocumentManager.MdiActiveDocument.Editor;
        var bgPlot = Application.GetSystemVariable("Backgroundplot");
        Application.SetSystemVariable("Backgroundplot", 0);
        ProcessDwg(obj.FilePath, db =>
        {
            HostApplicationServices.WorkingDatabase = db;
            using (var tr = db.TransactionManager.StartTransaction())
            {
                try
                {
                    List<PointClass> points = new List<PointClass>();
                    var l = new Layout();
                    var p1 = new Point3d();
                    var p2 = new Point3d();

                    var bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead);
                    var ms = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);
                        foreach (ObjectId msId in ms)
                        {
                            if (msId.ObjectClass.DxfName == "INSERT")
                            {
                                var br = (BlockReference)tr.GetObject(msId, OpenMode.ForRead);
                                var btr = (BlockTableRecord)tr.GetObject(br.BlockTableRecord, OpenMode.ForRead);
                                if (btr.Name.Equals(blockName, StringComparison.OrdinalIgnoreCase))
                                {
                                    if(btr.IsFromExternalReference) db.ResolveXrefs(true, false);
                                    foreach (ObjectId id in btr)
                                    {
                                        var ent = (Entity)tr.GetObject(id, OpenMode.ForRead);
                                        if(ent != null&& ent is Polyline pl && pl.NumberOfVertices == 4)
                                        {
                                            var mat = br.BlockTransform;

                                            p1 = pl.GetPoint3dAt(3).TransformBy(mat);
                                            p2 = pl.GetPoint3dAt(1).TransformBy(mat);

                                            using (ViewTableRecord vt = ed.GetCurrentView())
                                            {
                                                var co = Matrix3d.WorldToPlane(vt.ViewDirection) *
                                                    Matrix3d.Displacement(vt.Target.GetAsVector().Negate()) *
                                                    Matrix3d.Rotation(vt.ViewTwist, vt.ViewDirection, vt.Target);

                                                var x = p1.TransformBy(co);
                                                var y = p2.TransformBy(co);
                                                var lm = LayoutManager.Current;
                                                l = (Layout)tr.GetObject(lm.GetLayoutId(lm.CurrentLayout), OpenMode.ForRead);
                                                points.Add(new PointClass(x, y));
                                            }
                                        }
                                    }
                                }
                            }
                        
                        }
                    if (points.Count > 0)
                    {
                        pdfPath = ProcessPlotPdf(
                            l, 
                            points, 
                            obj.FilePath, 
                            device, 
                            paper, 
                            styleName, 
                            isCombineFile
                            );
                    }
                    else ed.WriteMessage("\nThere is no same blocks: " +
                        System.IO.Path.GetFileNameWithoutExtension(obj.FilePath)+"\n");
                    tr.Commit();
                }
                catch (Autodesk.AutoCAD.Runtime.Exception ex)
                {
                    ed.WriteMessage("\nerror: " + ex.Message);
                }
            }
    });
        Application.SetSystemVariable("Backgroundplot", bgPlot);
        return pdfPath;
    }
    public static List<string> ProcessPlotPdf(
        Layout lo,
        List<PointClass> points,
        string path,
        string device,
        string paper,
        string styleName,
        bool isCombine
        )
    {
        int count = 1;
        int numSheet = 1;
        var dir = "D:\\sample\\1_150\\PDF\\";
        var result = new List<string>();
        //var dir = Path.GetDirectoryName(path)+"\\PDF\\";
        //FolderBrowserDialog dialog = new FolderBrowserDialog();
        //dialog.Description = "저장 폴더 선택";
        //var result = dialog.ShowDialog();
        //if (result != DialogResult.OK) return;

        //string folderPath = dialog.SelectedPath;
        var piv = new PlotInfoValidator { MediaMatchingPolicy = MatchingPolicy.MatchEnabled };

        PlotInfo pi = new PlotInfo();
        using (PlotEngine pe = PlotFactory.CreatePublishEngine())
        {
            using (PlotProgressDialog ppd = new PlotProgressDialog(false, (isCombine ? 1 : points.Count), true))
            {
                foreach(PointClass p in points)
                {
                    var pdfPath = $"{dir}{Path.GetFileNameWithoutExtension(path)}{(isCombine ? "" : ("_" + count))}.pdf";
                    var ps = new PlotSettings(lo.ModelType);
                    ps.CopyFrom(lo);

                    var psv = PlotSettingsValidator.Current;

                    var pwe = new Extents2d(p.P1.X, p.P2.Y, p.P2.X, p.P1.Y);

                    ps.ScaleLineweights = true;
                    psv.SetPlotWindowArea(ps, pwe);
                    psv.SetPlotType(
                        ps,
                        Autodesk.AutoCAD.DatabaseServices.PlotType.Window
                    );

                    psv.SetUseStandardScale(ps, true);
                    psv.SetStdScaleType(ps, StdScaleType.ScaleToFit);
                    psv.SetPlotRotation(ps, PlotRotation.Degrees180);
                    psv.SetPlotCentered(ps, true);
                    psv.SetPlotConfigurationName(ps, device, paper);
                    psv.SetCurrentStyleSheet(ps, styleName);

                    pi = new PlotInfo() { Layout = lo.ObjectId };
                    pi.OverrideSettings = ps;
                    piv.Validate(pi);

                    if (numSheet == 1)
                    {
                        ppd.set_PlotMsgString(PlotMessageIndex.DialogTitle, "Custom Plot Progress");
                        ppd.set_PlotMsgString(PlotMessageIndex.CancelJobButtonMessage, "Cancel Job");
                        ppd.set_PlotMsgString(PlotMessageIndex.CancelSheetButtonMessage, "Cancel Sheet");
                        ppd.set_PlotMsgString(PlotMessageIndex.SheetSetProgressCaption, "Sheet Set Progress");
                        ppd.set_PlotMsgString(PlotMessageIndex.SheetProgressCaption, "Sheet Progress");
                        ppd.LowerPlotProgressRange = 0;
                        ppd.UpperPlotProgressRange = 100;
                        ppd.PlotProgressPos = 0;

                        ppd.OnBeginPlot();
                        ppd.IsVisible = true;
                        pe.BeginPlot(ppd, null);

                        pe.BeginDocument(
                            pi,
                            Path.GetFileNameWithoutExtension(path),
                            null,
                            1,
                            true,
                            pdfPath
                        );
                    }
                    ppd.OnBeginSheet();

                    ppd.LowerSheetProgressRange = 0;
                    ppd.UpperSheetProgressRange = 100;
                    ppd.SheetProgressPos = 0;

                    PlotPageInfo ppi = new PlotPageInfo();
                    pe.BeginPage(ppi, pi, (isCombine ? numSheet == points.Count : true), null);

                    pe.BeginGenerateGraphics(null);
                    ppd.SheetProgressPos = 50;
                    pe.EndGenerateGraphics(null);

                    pe.EndPage(null);
                    ppd.SheetProgressPos = 100;
                    ppd.OnEndSheet();
                    if(isCombine) 
                    { 
                       if(numSheet == 1)
                       {
                           result.Add(pdfPath);
                       }
                        numSheet++; 
                    }
                    else
                    {
                        count++;
                        pe.EndDocument(null);

                        ppd.PlotProgressPos = 100;
                        ppd.OnEndPlot();
                        pe.EndPlot(null);
                        result.Add(pdfPath);
                    }
                }
                if(isCombine)
                {
                    pe.EndDocument(null);

                    ppd.PlotProgressPos = 100;
                    ppd.OnEndPlot();
                    pe.EndPlot(null);
                }
            }
        }
        return result;
    }
    public static ObservableCollection<BlockClass> SetChooseBlocks()
    {
        var doc = Application.DocumentManager.MdiActiveDocument;
        Database db = doc.Database;
        ObservableCollection<BlockClass> blocks = new ObservableCollection<BlockClass>();
        bool isCheck = false;

        PromptSelectionOptions pso = new PromptSelectionOptions();
        pso.MessageForAdding = "\nSelect Blocks: ";
        var filters = new TypedValue[] { new TypedValue((int)DxfCode.Start, "INSERT") };
        SelectionFilter filter = new SelectionFilter(filters);
        PromptSelectionResult psr = doc.Editor.GetSelection(pso, filter);
        if (psr.Status != PromptStatus.OK) { return null; }

        using (doc.LockDocument())
        using(Transaction tr = db.TransactionManager.StartTransaction())
        {
            foreach (SelectedObject s in psr.Value)
            {
                var br = (BlockReference)tr.GetObject(s.ObjectId, OpenMode.ForRead);
                var btr = (BlockTableRecord)tr.GetObject(br.BlockTableRecord, OpenMode.ForRead);
                var blockName = string.Empty;
                var fileName = string.Empty;

                //if (btr.IsFromExternalReference)
                //{
                //    db.ResolveXrefs(true, false);
                //    foreach (var i in btr)
                //    {
                //        var ent = (Entity)tr.GetObject(i, OpenMode.ForRead);
                //        if (ent != null && ent is BlockReference inBr)
                //        {
                //            var targetBtr = (BlockTableRecord)tr.GetObject(inBr.BlockTableRecord, OpenMode.ForRead);

                //            foreach (var id in targetBtr)
                //            {
                //                var targetEnt = (Entity)tr.GetObject(id, OpenMode.ForRead);
                //                if(targetEnt!=null && targetEnt is Polyline pl && pl.NumberOfVertices ==4)
                //                {
                //                    string[] splitName = inBr.Name.Split('|');
                //                    blockName = splitName[1];
                //                    fileName = splitName[0];
                //                    break;
                //                }
                //            }
                //        }
                //    }
                //}else
                //{
                    foreach(var i in btr)
                    {
                        var ent = (Entity)tr.GetObject(i, OpenMode.ForRead);
                        if(ent!=null&&ent is Polyline pl && pl.NumberOfVertices == 4)
                        {
                            blockName = btr.Name;
                            fileName = System.IO.Path.GetFileName(doc.Name);
                            break;
                        }
                    }
                //}

                for (int i = 0; i < blocks.Count; i++)
                {
                    if (blocks[i].BlockName.Equals(blockName, StringComparison.OrdinalIgnoreCase))
                    {
                        isCheck = true;
                    }
                }
                if (!isCheck)
                    blocks.Add(new BlockClass(blockName, fileName));
            }
        }
        return blocks;
    }
    public static List<string> GetPrinterDeviceNames()
    {
        PlotConfigManager.RefreshList(RefreshCode.All);
        PlotSettingsValidator psv = PlotSettingsValidator.Current;

        var printList = psv.GetPlotDeviceList().Cast<string>().ToList();

        return printList;
    }
    public static List<string> GetPrinterPaperSizes(string deviceName)
    {
        PlotConfig pc = PlotConfigManager.SetCurrentConfig(deviceName);
        List<string> l = new List<string>();

        if (deviceName == "없음" || deviceName == "None") return l;
        foreach(var p in pc.CanonicalMediaNames)
        {
            l.Add(p);
        }


        return l;
    }
    public static List<string> GetPrinterStyleNames()
    {
        PlotConfigManager.RefreshList(RefreshCode.All);
        PlotSettingsValidator psv = PlotSettingsValidator.Current;

        var ctbList = psv.GetPlotStyleSheetList().Cast<string>().ToList();

        return ctbList;
    }
    //public static void ListPrinters()
    //{
    //    Document doc = Application.DocumentManager.MdiActiveDocument;
    //    Database db = doc.Database;

    //    PlotConfigManager.RefreshList(RefreshCode.All);
    //    PlotConfig pc = PlotConfigManager.SetCurrentConfig("DWG To PDF.pc3");
    //    pc.RefreshMediaNameList();
    //    PlotSettingsValidator psv = PlotSettingsValidator.Current;
    //    //PlotSettings ps = new PlotSettings(true);

    //    var printList = psv.GetPlotDeviceList().Cast<string>().ToList();
    //    var ctbList = psv.GetPlotStyleSheetList().Cast<string>().ToList();
    //    printList.ForEach(p => doc.Editor.WriteMessage("\n\t print: {0}", p));
    //    ctbList.ForEach(p => doc.Editor.WriteMessage("\n\t ctb: {0}", p));
    //    foreach(var p in pc.CanonicalMediaNames)
    //    {
    //        doc.Editor.WriteMessage("\n\t size: {0}", p);
    //    }
    //    //var paperList = psv.GetCanonicalMediaNameList(ps).Cast<string>().ToList();
    //    //paperList.ForEach(p => doc.Editor.WriteMessage("\n\t size: {0}", p));
    //}
    #endregion
}