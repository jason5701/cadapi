using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Colors;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Controls;

public class ClCAD
{
    public static void CreateBlock(Point3d pStart, string blockName)
    {
        Document doc = Application.DocumentManager.MdiActiveDocument;
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
        Document doc = Application.DocumentManager.MdiActiveDocument;
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
    //public static void GetBlockFromExternalFile(string blockName, Point3d p)
    //{
    //    Document doc = Application.DocumentManager.MdiActiveDocument;
    //    Database db = doc.Database;

    //    using (doc.LockDocument())
    //    using (Transaction tr = db.TransactionManager.StartTransaction())
    //    {
    //        using (Database externalDb = new Database(false, true))
    //        {
    //                try
    //                {
    //                    string path = "D:\\test\\repos\\training\\Auto_Foundation\\referenceBlock\\blocks.dwg";
    //                    externalDb.ReadDwgFile(path, FileOpenMode.OpenForReadAndAllShare, true, "");
    //                }
    //                catch
    //                {
    //                    doc.Editor.WriteMessage($"\nError: Failed to open the external DWG file.");
    //                    return;
    //                }
    //            using(Transaction exTr = externalDb.TransactionManager.StartTransaction())
    //            {
    //                BlockTable externalBlockTable = (BlockTable)tr.GetObject(externalDb.BlockTableId, OpenMode.ForRead);

    //                if (!externalBlockTable.Has(blockName))
    //                {
    //                    doc.Editor.WriteMessage($"\nError: Block '{blockName}' not found in the external DWG file.");
    //                    return;
    //                }
    //                ObjectId blockId = externalBlockTable[blockName];
    //                BlockTableRecord currentSpace = (BlockTableRecord)tr.GetObject(db.CurrentSpaceId, OpenMode.ForWrite);
    //                BlockReference blockRef = new BlockReference(p, blockId);
    //                currentSpace.AppendEntity(blockRef);
    //                tr.AddNewlyCreatedDBObject(blockRef, true);

    //                doc.Editor.WriteMessage($"\nBlock '{blockName}' inserted from the external DWG file.");
    //                exTr.Commit();
    //            }
    //            tr.Commit();
    //                doc.Editor.Command("_-INSERT", "", "", "0,0", "1", "0");
    //        }
    //    }
    //}
    //public static void GetBlock(Point3d p, string blockName)
    //{
    //    Document doc = Application.DocumentManager.MdiActiveDocument;
    //    Database db = doc.Database;

    //    using (doc.LockDocument())
    //    using (Transaction tr = db.TransactionManager.StartTransaction())
    //    {
    //        string path = "D:/test/repos/training/Auto_Foundation/referenceBlock/blocks.dwg";
    //        //BlockTable currentBt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead);
    //        using (Database eDb = new Database(false, true))
    //        {
    //            eDb.ReadDwgFile(path, FileOpenMode.OpenTryForReadShare, true, "");
    //            //using (Transaction newTr = eDb.TransactionManager.StartTransaction())
    //            //{
    //                //BlockTable oBt = (BlockTable)newTr.GetObject(eDb.BlockTableId, OpenMode.ForRead);
    //                //if (oBt.Has(blockName))
    //                //{
    //                    try
    //                    {

    //                        //ObjectId id = oBt[blockName];

    //                        //BlockTableRecord btr = (BlockTableRecord)newTr.GetObject(id, OpenMode.ForRead);
    //                        //BlockTableRecord currentBtr = (BlockTableRecord)tr.GetObject(db.CurrentSpaceId, OpenMode.ForWrite);
    //                        db.Insert(Path.GetFileNameWithoutExtension(path), eDb, true);
    //                        //BlockReference br = new BlockReference(p, id);
    //                        //currentBtr.AppendEntity(br);
    //                        //tr.AddNewlyCreatedDBObject(br, true);

    //                        doc.Editor.Command("_-INSERT", blockName, "", "0,0", "1", "0");
    //                    }
    //                    catch
    //                    {
    //                        doc.Editor.WriteMessage($"\nBlock '{blockName}' not found");
    //                    //}
    //                }
    //            //    newTr.Commit();
    //            //}
    //        }
    //        tr.Commit();
    //    }
    //}
    public static void CreateDimStyles(DimStyleSettings ds)
    {
        Document doc = Application.DocumentManager.MdiActiveDocument;
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
        Document doc = Application.DocumentManager.MdiActiveDocument;
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
        Document doc = Application.DocumentManager.MdiActiveDocument;
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
        Document doc = Application.DocumentManager.MdiActiveDocument;
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
        Document doc = Application.DocumentManager.MdiActiveDocument;
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
        Document doc = Application.DocumentManager.MdiActiveDocument;
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
        Document doc = Application.DocumentManager.MdiActiveDocument;
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
        Document doc = Application.DocumentManager.MdiActiveDocument;
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
        Document doc = Application.DocumentManager.MdiActiveDocument;
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
        Document doc = Application.DocumentManager.MdiActiveDocument;
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
        Document doc = Application.DocumentManager.MdiActiveDocument;
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
        Document doc = Application.DocumentManager.MdiActiveDocument;
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
    public static Point3d GetPointsFromUser(string param)
    {
        PromptPointResult ppr = Application.DocumentManager.MdiActiveDocument.
            Editor.GetPoint(new PromptPointOptions("\n" + param));
        if (ppr.Status != PromptStatus.OK) return new Point3d(-111, -123, -147);
        return ppr.Value;
    }
    public static void CreateLayer(string layerName, short color, string linetypeName, LineWeight lineWeight, bool canPrint)
    {
        Document doc = Application.DocumentManager.MdiActiveDocument;
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
        Document doc = Application.DocumentManager.MdiActiveDocument;
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
        Document doc = Application.DocumentManager.MdiActiveDocument;
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
        Document doc = Application.DocumentManager.MdiActiveDocument;
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
        Document doc = Application.DocumentManager.MdiActiveDocument;
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
        Document doc = Application.DocumentManager.MdiActiveDocument;
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
        Document doc = Application.DocumentManager.MdiActiveDocument;
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
        Document doc = Application.DocumentManager.MdiActiveDocument;
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
        Document doc = Application.DocumentManager.MdiActiveDocument;
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
}