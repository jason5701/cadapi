using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using Application = Autodesk.AutoCAD.ApplicationServices.Application;

namespace Auto_Foundation.Figure.Model
{
    public class SettingModel : BaseViewModel
    {
        private double _BottomFDNLength;
        public double BottomFDNLength
        {
            get { return _BottomFDNLength; }
            set { _BottomFDNLength = value; OnPropertyChanged(); }
        }
        private double _BottomFDNCenter;
        public double BottomFDNCenter
        {
            get { return _BottomFDNCenter; }
            set { _BottomFDNCenter = value; OnPropertyChanged(); }
        }
        private double _BottomFDNHeight;
        public double BottomFDNHeight
        {
            get { return _BottomFDNHeight; }
            set { _BottomFDNHeight = value; OnPropertyChanged(); }
        }
        private double _TopFDNLength;
        public double TopFDNLength
        {
            get { return _TopFDNLength; }
            set { _TopFDNLength = value; OnPropertyChanged(); }
        }
        private double _TopFDNCenter;
        public double TopFDNCenter
        {
            get { return _TopFDNCenter; }
            set { _TopFDNCenter = value; OnPropertyChanged(); }
        }
        private double _TopFDNHeight;
        public double TopFDNHeight
        {
            get { return _TopFDNHeight; }
            set { _TopFDNHeight = value; OnPropertyChanged(); }
        }
        private double _Depth;
        public double Depth
        {
            get { return _Depth; }
            set { _Depth = value; OnPropertyChanged(); }
        }
        private double _X;
        public double X
        {
            get { return _X; }
            set { _X = value; OnPropertyChanged(); }
        }
        private short _ColorIndex;
        public short ColorIndex
        {
            get { return _ColorIndex; }
            set { _ColorIndex = value; OnPropertyChanged(); }
        }
        private string _ColorValue;
        public string ColorValue
        {
            get { return _ColorValue; }
            set { _ColorValue = value; OnPropertyChanged(); }
        }
        private List<string> _Linetypes;

        public List<string> Linetypes
        {
            get { return _Linetypes; }
            set { _Linetypes = value; OnPropertyChanged(); }
        }
        private double _BCDCircle;
        public double BCDCircle
        {
            get { return _BCDCircle; }
            set { _BCDCircle = value; OnPropertyChanged(); }
        }
        private string _SelectedLinetype;

        public string SelectedLinetype
        {
            get { return _SelectedLinetype; }
            set { _SelectedLinetype = value; OnPropertyChanged(); }
        }
        public static List<string> getAllLinetypes()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;

            string path = HostApplicationServices.Current.FindFile("acad.lin", db, FindFileHint.Default);

            using (StreamReader sr = new StreamReader(path))
            {
                List<string> linetypes = new List<string>()
                {
                    "Continuous"
                };
                string line;
                Char[] c = new char[] { ',' };
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.StartsWith("*"))
                    {
                        string[] info = line.Split(c);
                        linetypes.Add(info[0].Substring(1));
                    }
                }
                return linetypes;
            }
        }
        public SettingModel()
        {
            BottomFDNLength = 2500;
            BottomFDNCenter = Math.Ceiling(BottomFDNLength * (Math.Sqrt(2) - 1));
            BottomFDNHeight = 500;
            TopFDNLength = 2200;
            TopFDNCenter = Math.Ceiling(TopFDNLength * (Math.Sqrt(2) - 1)); ;
            TopFDNHeight = 800;
            Depth = 300;
            X = 100;
            ColorIndex = 0;
            ColorValue = "white";
            Linetypes = getAllLinetypes();
            SelectedLinetype = "Continuous";
            BCDCircle = 1600;
        }
        #region setting layer
        public static void CreateLayer()
        {
            ClCAD.CreateLayer("BOTTOMFDN", 0, "Continuous", LineWeight.LineWeight018, false);
            ClCAD.CreateLayer("TOPFDN", 0, "Continuous", LineWeight.LineWeight018, false);
            ClCAD.CreateLayer("SYMBOL", 1, "ZIGZAG", LineWeight.LineWeight018, false);
            ClCAD.CreateLayer("CENTER", 2, "CENTER", LineWeight.LineWeight013, true);
            ClCAD.CreateLayer("DIMS", 1, "Continuous", LineWeight.LineWeight030, true);
        }
        public static void CreateTextStyle()
        {
            ClCAD.CreateTextStyle("TestTextStyle1", "Arial", 0, 1, false, false);
            ClCAD.CreateTextStyle("TestTextStyle2", "Arial", 0, 1, false, false);
            ClCAD.CreateTextStyle("TestTextStyle3", "Arial", 0, 1, false, true);
        }
        public static void CreateDimStyle()
        {
            ClCAD.DimStyleSettings TL100 = new ClCAD.DimStyleSettings()
            {
                Name = "TL1-100",
                ScaleFactor = 1
            };
            ClCAD.CreateDimStyles(TL100);

            ClCAD.DimStyleSettings TL200 = new ClCAD.DimStyleSettings()
            {
                Name = "TL1-200",
                Fit = 2,
            };
            ClCAD.CreateDimStyles(TL200);

            ClCAD.DimStyleSettings TL150 = new ClCAD.DimStyleSettings()
            {
                Name = "TL1-150",
                Fit = 1.5,
            };
            ClCAD.CreateDimStyles(TL150);

            ClCAD.DimStyleSettings TL25 = new ClCAD.DimStyleSettings()
            {
                Name = "TL1-25",
                Fit = 0.25,
                Arrow_Size = 300,
            };
            ClCAD.CreateDimStyles(TL25);
            ClCAD.DimStyleSettings TL20 = new ClCAD.DimStyleSettings()
            {
                Name = "TL1-20",
                Fit = 0.2,
            };
            ClCAD.CreateDimStyles(TL20);

            ClCAD.DimStyleSettings TL10 = new ClCAD.DimStyleSettings()
            {
                Name = "TL1-10",
                Fit = 0.1,
            };
            ClCAD.CreateDimStyles(TL10);

            ClCAD.DimStyleSettings TL75 = new ClCAD.DimStyleSettings()
            {
                Name = "TL1-75",
                Fit = 0.75,
            };
            ClCAD.CreateDimStyles(TL75);

            ClCAD.DimStyleSettings TL125 = new ClCAD.DimStyleSettings()
            {
                Name = "TL1-125",
                Fit = 1.25,
            };
            ClCAD.CreateDimStyles(TL125);

        }

        #endregion
        #region method
        public void DrawFondation(Point3d? value)
        {
            #region Section
            Point3d p0 = (Point3d)value;
            Point3d pStart = new Point3d(p0.X - TopFDNLength /2, p0.Y + BottomFDNHeight / 2, 0);
            Point3d pNext = new Point3d(p0.X - BottomFDNLength / 2, pStart.Y, 0);
            Point3d p1 = new Point3d(p0.X - BottomFDNLength / 2, p0.Y - BottomFDNHeight / 2, 0);
            Point3d p2 = new Point3d(p1.X + BottomFDNLength, p1.Y, 0);
            Point3d p3 = new Point3d(p2.X, p2.Y + BottomFDNHeight, 0);
            Point3d p4 = new Point3d(p0.X + TopFDNLength / 2, p3.Y, 0);
            //Point3d p4 = new Point3d(p1.X, p3.Y, 0);
            ClCAD.SetLayerCurrent("BOTTOMFDN");
            ClCAD.CreatePolylineFromListPoints(new List<Point3d> { pStart, pNext, p1, p2, p3, p4 }, false, SelectedLinetype, ColorIndex);
            Point3d p5 = new Point3d(p0.X - TopFDNLength / 2, p0.Y + BottomFDNHeight / 2, 0);
            Point3d p6 = new Point3d(p5.X + TopFDNLength, p5.Y, 0);
            Point3d p7 = new Point3d(p6.X, p6.Y + TopFDNHeight, 0);
            Point3d p8 = new Point3d(p5.X, p7.Y, 0);
            ClCAD.SetLayerCurrent("TOPFDN");
            ClCAD.CreatePolylineFromListPoints(new List<Point3d> { p6, p7, p8, p5 }, false, SelectedLinetype, ColorIndex);
            Point3d py1 = new Point3d(p0.X, p1.Y - 70, 0);
            Point3d py2 = new Point3d(p0.X, p7.Y + 70, 0);
            ClCAD.SetLayerCurrent("CENTER");
            ClCAD.CreateLine(py1, py2, 25);
            #region SymBol
            Point3d g1 = new Point3d(pStart.X, p7.Y - Depth, 0);
            Point3d g2 = new Point3d(g1.X - X, g1.Y , 0);
            Point3d g3 = new Point3d(g2.X, g1.Y - X, 0);
            Point3d g4 = new Point3d(pStart.X, g3.Y, 0);
            ClCAD.SetLayerCurrent("SYMBOL");
            ClCAD.CreateLine(p5, p6, 20);
            ClCAD.CreateBlock(g4, "SYMBOL6");
            ClCAD.CreatePolylineFromListPoints(new List<Point3d> { g1, g2, g3, g4 }, false, "Continuous", 1);
            #endregion
            #endregion
            #region Section DIM
            ClCAD.SetLayerCurrent("DIMS");
            ClCAD.SetDimStyleCurrent("TL1-25");
            List<Point3d> dsX = new List<Point3d>()
            {
                new Point3d(p1.X, p1.Y - 100, 0),
                new Point3d(p2.X, p1.Y - 100, 0),
            };
            ClCAD.CreateDimension_X(dsX, 25, 1);
            List<Point3d> dsY = new List<Point3d>()
            {
                new Point3d(p1.X - 50, p1.Y, 0),
                new Point3d(p1.X - 50, p5.Y, 0),
                new Point3d(p1.X - 50, g1.Y, 0),
                new Point3d(p1.X - 50, p7.Y, 0),
            };
            ClCAD.CreateDimension_Y(dsY, 25, 1);
            #endregion
            #region Plan
            Point3d pCenter = new Point3d(p0.X, p0.Y + Math.Max(BottomFDNLength, BottomFDNHeight) * 2, 0);
            Point3d pp1 = new Point3d(pCenter.X - BottomFDNCenter / 2, pCenter.Y + BottomFDNLength / 2, 0);
            Point3d pp2 = new Point3d(pp1.X + BottomFDNCenter, pp1.Y, 0);
            Point3d pp3 = new Point3d(pCenter.X + BottomFDNLength / 2, pCenter.Y + BottomFDNCenter / 2, 0);
            Point3d pp4 = new Point3d(pp3.X, pCenter.Y - BottomFDNCenter / 2, 0);
            Point3d pp5 = new Point3d(pp2.X, pp2.Y - BottomFDNLength, 0);
            Point3d pp6 = new Point3d(pp1.X, pp5.Y, 0);
            Point3d pp7 = new Point3d(pCenter.X - BottomFDNLength / 2, pp4.Y, 0);
            Point3d pp8 = new Point3d(pp7.X, pp3.Y, 0);
            ClCAD.SetLayerCurrent("BOTTOMFDN");
            ClCAD.CreatePolylineFromListPoints
            (
                new List<Point3d> { pp1, pp2, pp3, pp4, pp5, pp6, pp7, pp8 }, 
                true, SelectedLinetype, ColorIndex
            );
            Point3d pp9 = new Point3d(pCenter.X - TopFDNCenter / 2, pCenter.Y + TopFDNLength / 2, 0);
            Point3d pp10 = new Point3d(pp9.X + TopFDNCenter, pp9.Y, 0);
            Point3d pp11 = new Point3d(pCenter.X + TopFDNLength / 2, pCenter.Y + TopFDNCenter / 2, 0);
            Point3d pp12 = new Point3d(pp11.X, pCenter.Y - TopFDNCenter / 2, 0);
            Point3d pp13 = new Point3d(pp10.X, pp10.Y - TopFDNLength, 0);
            Point3d pp14 = new Point3d(pp9.X, pp13.Y, 0);
            Point3d pp15 = new Point3d(pCenter.X - TopFDNLength / 2, pp12.Y, 0);
            Point3d pp16 = new Point3d(pp15.X, pp11.Y, 0);
            ClCAD.CreatePolylineFromListPoints
            (
                new List<Point3d> { pp9, pp10, pp11, pp12, pp13, pp14, pp15, pp16 },
                true, SelectedLinetype, ColorIndex
            );
            Point3d px1 = new Point3d(p1.X - 70, pCenter.Y, 0);
            Point3d px2 = new Point3d(p2.X + 70, pCenter.Y, 0);
            py1 = new Point3d(pCenter.X, pp1.Y + 70, 0);
            py2 = new Point3d(pCenter.X, pp6.Y - 70, 0);
            ClCAD.SetLayerCurrent("CENTER");
            ClCAD.CreateLine(px1, px2);
            ClCAD.CreateLine(py1, py2);
            //#region Symbol
            //ClCAD.SetLayerCurrent("SYMBOL");
            //ClCAD.CreateCircle(pCenter, BCDCircle/2, "Continuous", 1);
            //#endregion
            #endregion
            #region PlanDIM
            ClCAD.SetLayerCurrent("DIMS");
            ClCAD.SetDimStyleCurrent("TL1-25");
            dsX = new List<Point3d>()
            {
                new Point3d(pp7.X, pp5.Y - 100, 0),
                new Point3d(pp3.X, pp5.Y - 100, 0),
            };
            ClCAD.CreateDimension_X(dsX, 25, 2);
            dsX = new List<Point3d>() {
                new Point3d(pp7.X, pp5.Y - 100, 0),
                new Point3d(pp1.X, pp5.Y - 100, 0),
                new Point3d(pCenter.X, pp5.Y - 100, 0),
                new Point3d(pp2.X, pp5.Y - 100, 0),
                new Point3d(pp3.X, pp5.Y - 100, 0),
            };
            ClCAD.CreateDimension_X(dsX, 25, 1);
            dsY = new List<Point3d>()
            {
                new Point3d(pp7.X - 100, pp9.Y, 0),
                new Point3d(pp7.X - 100, pp13.Y, 0),
            };
            ClCAD.CreateDimension_Y(dsY, 25, 2);
            dsY = new List<Point3d>()
            {
                new Point3d(pp7.X - 100, pp9.Y, 0),
                new Point3d(pp7.X - 100, pp11.Y, 0),
                new Point3d(pp7.X - 100, pCenter.Y, 0),
                new Point3d(pp7.X - 100, pp12.Y, 0),
                new Point3d(pp7.X - 100, pp13.Y, 0),
            };
            ClCAD.CreateDimension_Y(dsY, 25, 1);
            #endregion

            ClCAD.ZoomAll();
        }
        #endregion
    }
}
