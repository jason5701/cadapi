using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.GraphicsInterface;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using Visibility = System.Windows.Visibility;

namespace Auto_Foundation.Figure.Model
{
    public class MultiPlotSettingModel:BaseViewModel
    {
        private string _DeviceName;

        public string DeviceName
        {
            get { return _DeviceName; }
            set { _DeviceName = value; OnPropertyChanged(); }
        }
        private string _PaperSizeName;

        public string PaperSizeName
        {
            get { return _PaperSizeName; }
            set { _PaperSizeName = value; OnPropertyChanged(); }
        }
        private string _StyleName;
        public string StyleName
        {
            get { return _StyleName; }
            set { _StyleName = value; OnPropertyChanged(); }
        }
        private List<string> _DeviceNames;

        public List<string> DeviceNames
        {
            get { return _DeviceNames; }
            set { _DeviceNames = value; OnPropertyChanged(); }
        }
        private List<string> _PaperSizeNames;

        public List<string> PaperSizeNames
        {
            get { return _PaperSizeNames; }
            set { _PaperSizeNames = value; OnPropertyChanged(); }
        }
        private List<string> _StyleNames;

        public List<string> StyleNames
        {
            get { return _StyleNames; }
            set { _StyleNames = value; OnPropertyChanged(); }
        }

        private ObservableCollection<BlockClass> _BlockList;
        public ObservableCollection<BlockClass> BlockList
        {
            get { return _BlockList; }
            set { _BlockList = value; OnPropertyChanged(); }
        }

        private ObservableCollection<DrawClass> _DrawingList;
        public ObservableCollection<DrawClass> DrawingList
        {
            get { return _DrawingList; }
            set { _DrawingList = value; OnPropertyChanged(); }
        }

        //private bool _IsCurrentDrawing;
        //public bool IsCurrentDrawing
        //{
        //    get { return _IsCurrentDrawing; }
        //    set { _IsCurrentDrawing = value; OnPropertyChanged(); }
        //}
        private bool _IsCombinePDF;
        public bool IsCombinePDF
        {
            get { return _IsCombinePDF; }
            set { _IsCombinePDF = value; OnPropertyChanged(); }
        }
        private bool _IsCombineEachFile;
        public bool IsCombineEachFile
        {
            get { return _IsCombineEachFile; }
            set { _IsCombineEachFile = value; OnPropertyChanged(); }
        }
        private string _MergePDFName;
        public string MergePDFName
        {
            get { return _MergePDFName; }
            set { _MergePDFName = value; OnPropertyChanged(); }
        }
        private string _SelectedDirectory;
        public string SelectedDirectory
        {
            get { return _SelectedDirectory; }
            set { _SelectedDirectory = value; OnPropertyChanged(); }
        }
        public MultiPlotSettingModel()
        {
            var isCheck = Regex.IsMatch(ClCAD.GetPrinterDeviceNames()[0], @"^[a-zA-Z]+$");
            DeviceName = isCheck ? "None" : "없음";
            PaperSizeName = isCheck ? "None" : "없음";
            StyleName = "monochrome.ctb";
            DeviceNames = ClCAD.GetPrinterDeviceNames();
            PaperSizeNames = ClCAD.GetPrinterPaperSizes(DeviceName);
            StyleNames = ClCAD.GetPrinterStyleNames();
            BlockList = new ObservableCollection<BlockClass>();
            DrawingList = new ObservableCollection<DrawClass>();
            //IsCurrentDrawing = true;
            IsCombineEachFile = false;
            IsCombinePDF = false;
            MergePDFName = string.Empty;
            SelectedDirectory = "C:\\";
        }
    }
}
