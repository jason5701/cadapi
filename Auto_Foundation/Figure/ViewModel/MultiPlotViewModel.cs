using Auto_Foundation.Figure.Model;
using Auto_Foundation.Figure.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;

namespace Auto_Foundation.Figure.ViewModel
{
    public class MultiPlotViewModel : BaseViewModel
    {
        private MultiPlotSettingModel _MultiPlotSettingModel;
        public MultiPlotSettingModel MultiPlotSettingModel
        {
            get { return _MultiPlotSettingModel; }
            set { _MultiPlotSettingModel = value; }
        }
        #region ICommand
        //public ICommand LoadSettingViewCommand { get; set; }
        public ICommand PlotBtnCommand { get; set; }
        public ICommand CancelBtnCommand { get; set; }
        public ICommand DeviceName_SelectionChanged_Command { get; set; }
        public ICommand BlockSetBtnCommand { get; set; }
        public ICommand Drawing_Add_Command { get; set; }
        public ICommand DrawingFolder_Add_Command { get; set; }
        public ICommand CurrentDrawingCheckCommand { get; set; }
        public ICommand PDFCombineCheckCommand { get; set; }
        public ICommand CombinePDFEachFileCommand { get; set; }
        public ICommand DirectorySetBtnCommand { get; set; }
        public ICommand Directory_Changed_Command { get; set; }
        
        # endregion
        public MultiPlotViewModel()
        {
            MultiPlotSettingModel = new MultiPlotSettingModel();
            //LoadSettingViewCommand = new RelayCommand<MultiPlot>((p) => { return true; }, (p) =>
            //{
            //    string curDocPath = ClCAD.GetCurrentDocName();
            //    string curDocName = Path.GetFileNameWithoutExtension(curDocPath);
            //    MultiPlotSettingModel.DrawingList.Add(
            //        new DrawClass(curDocName, MultiPlotSettingModel.StyleName, curDocPath)
            //        );
            //});
            DrawingFolder_Add_Command = new RelayCommand<MultiPlot>((p) => { return true; }, (p) =>
            {
                FolderBrowserDialog dialog = new FolderBrowserDialog();
                dialog.Description = "폴더 선택";
                var result = dialog.ShowDialog();
                if (result != DialogResult.OK) return;
                
                string folderPath = dialog.SelectedPath;
                string[] dwgPaths = Directory.GetFiles(folderPath,"*.dwg",SearchOption.TopDirectoryOnly);

                foreach(string path in dwgPaths)
                {
                    bool isCheck = false;
                    string fileName = System.IO.Path.GetFileNameWithoutExtension(path);
                    if (MultiPlotSettingModel.DrawingList.Count > 0)
                    {
                        for (int i = 0; i < MultiPlotSettingModel.DrawingList.Count; i++)
                        {
                            if (MultiPlotSettingModel.DrawingList[i].FilePath.Equals(path, StringComparison.OrdinalIgnoreCase))
                            {
                                isCheck = true;
                            }
                        }
                        if (!isCheck)
                            MultiPlotSettingModel.DrawingList.Add(new DrawClass(fileName, MultiPlotSettingModel.StyleName, path));
                    }
                    else
                    {
                        MultiPlotSettingModel.DrawingList.Add(new DrawClass(fileName, MultiPlotSettingModel.StyleName, path));
                    }
                }
            });
            Drawing_Add_Command = new RelayCommand<MultiPlot>((p) => { return true; }, (p) =>
            {
                var curDWGPath = ClCAD.GetCurrentDocName();

                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "AutoCAD Drawing (*.dwg)|*.dwg";
                dialog.Multiselect = true;

                var result = dialog.ShowDialog();

                if (result != DialogResult.OK) return;
                string[] selectedFilePaths = dialog.FileNames;
                foreach(string path in selectedFilePaths)
                {
                    bool isCheck = false;
                    string fileName = System.IO.Path.GetFileNameWithoutExtension(path);
                    if (MultiPlotSettingModel.DrawingList.Count > 0)
                    {
                        for (int i = 0; i < MultiPlotSettingModel.DrawingList.Count; i++)
                        {
                            if (MultiPlotSettingModel.DrawingList[i].FilePath.Equals(path, StringComparison.OrdinalIgnoreCase))
                            {
                                isCheck = true;
                            }else if (curDWGPath.Equals(path, StringComparison.OrdinalIgnoreCase))
                            {
                                isCheck = true;
                            }
                        }
                        if (!isCheck)
                        MultiPlotSettingModel.DrawingList.Add(new DrawClass(fileName, MultiPlotSettingModel.StyleName, path));
                    }
                    else
                    {
                        if (curDWGPath.Equals(path, StringComparison.OrdinalIgnoreCase))
                        {
                            isCheck = true;
                        }
                        if (!isCheck)
                            MultiPlotSettingModel.DrawingList.Add(new DrawClass(fileName, MultiPlotSettingModel.StyleName, path));
                    }
                }

            });
            BlockSetBtnCommand = new RelayCommand<MultiPlot>((p) => { return true; }, (p) =>
            {
                p.Hide();
                ObservableCollection<BlockClass> blocks = ClCAD.SetChooseBlocks();
                if(blocks != null)
                {
                    MultiPlotSettingModel.BlockList = blocks;
                }
                p.ShowDialog();
            });
            DeviceName_SelectionChanged_Command = new RelayCommand<MultiPlot>((p) => { return true; }, (p) =>
            {
                if(MultiPlotSettingModel.DeviceName == "None")
                {
                    MultiPlotSettingModel.PaperSizeName = "None";
                    MultiPlotSettingModel.PaperSizeNames = null;
                }else if(MultiPlotSettingModel.DeviceName == "없음")
                {
                    MultiPlotSettingModel.PaperSizeName = "없음";
                    MultiPlotSettingModel.PaperSizeNames = null;
                }
                else
                {
                    var list = ClCAD.GetPrinterPaperSizes(MultiPlotSettingModel.DeviceName);
                    MultiPlotSettingModel.PaperSizeName = list[0];
                    MultiPlotSettingModel.PaperSizeNames = list;
                }
            });
            PlotBtnCommand = new RelayCommand<MultiPlot>((p) => { return true; }, (p) =>
            {
                List<string> pdfPaths = new List<string>();
                var device = MultiPlotSettingModel.DeviceName;
                var paper = MultiPlotSettingModel.PaperSizeName;
                var style = MultiPlotSettingModel.StyleName;

                var list = MultiPlotSettingModel.DrawingList;
                var blocks = MultiPlotSettingModel.BlockList;
                var currentPath = ClCAD.GetCurrentDocName();
                var currentDWGName = Path.GetFileNameWithoutExtension(currentPath);
                list.Add(new DrawClass(currentDWGName, style, currentPath));

                if(device == "None" || device == "없음")
                {
                    MessageBox.Show("프린터를 선택하십시요.");
                }
                else
                {
                    if (paper == null || paper == "")
                    {
                        MessageBox.Show("용지를 선택하십시요.");
                    }
                    else
                    {
                        p.Hide();
                        foreach (var b in blocks)
                        {
                            foreach (var o in list)
                            {
                                List<string> pdfPath = 
                                    ClCAD.SearchObjectByBlock(
                                        o, 
                                        b.BlockName, 
                                        device, 
                                        paper, 
                                        style
                                        );
                                if (pdfPath.Count > 0)
                                {
                                    foreach(var path in pdfPath)
                                    {
                                        pdfPaths.Add(path);
                                    }
                                }
                            }
                        }
                    }
                }
                if (pdfPaths.Count > 1 && MultiPlotSettingModel.IsCombinePDF)
                {
                    var dir = Path.GetDirectoryName(pdfPaths[0]);
                    var pdfName = MultiPlotSettingModel.MergePDFName != string.Empty ? MultiPlotSettingModel.MergePDFName : "merged";
                    var mergeName = dir + "\\" + pdfName + ".pdf";
                    Assistant.MergePDF(pdfPaths, mergeName);
                }


            });
            CancelBtnCommand = new RelayCommand<MultiPlot>((p) => { return true; }, (p) =>
            {
                p.Close();
            });
            PDFCombineCheckCommand = new RelayCommand<MultiPlot>((p) => { return true; }, (p) =>
            {
               MultiPlotSettingModel.IsCombinePDF = !MultiPlotSettingModel.IsCombinePDF;
            });
            //CurrentDrawingCheckCommand = new RelayCommand<MultiPlot>((p) => { return true; }, (p) =>
            //{
            //    var style = MultiPlotSettingModel.StyleName;
            //    var list = MultiPlotSettingModel.DrawingList;
            //    var currentPath = ClCAD.GetCurrentDocName();
            //    var currentDWGName = Path.GetFileNameWithoutExtension(currentPath);
            //    if (MultiPlotSettingModel.IsCurrentDrawing) list.Add(new DrawClass(currentDWGName, style, currentPath));
            //    else list.Remove(list.Where(i => i.DwgName == currentDWGName).Single());
            //    MultiPlotSettingModel.IsCurrentDrawing = !MultiPlotSettingModel.IsCurrentDrawing;
            //});
            CombinePDFEachFileCommand = new RelayCommand<MultiPlot>((p) => { return true; }, (p) =>
            {
                MultiPlotSettingModel.IsCombineEachFile = !MultiPlotSettingModel.IsCombineEachFile;
            });
            DirectorySetBtnCommand = new RelayCommand<MultiPlot>((p) => { return true; }, (p) =>
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                fbd.Description = "경로를 선택하세요.";
                fbd.ShowNewFolderButton = true;

                DialogResult result = fbd.ShowDialog();

                if (result != DialogResult.OK && string.IsNullOrWhiteSpace(fbd.SelectedPath)) return;

                // Get the selected folder path
                string selectedFolderPath = fbd.SelectedPath;
                MultiPlotSettingModel.SelectedDirectory = selectedFolderPath;
            });
            Directory_Changed_Command = new RelayCommand<MultiPlot>((p) => { return true; }, (p) =>
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                fbd.Description = "경로를 선택하세요.";
                fbd.ShowNewFolderButton = true;

                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    MessageBox.Show("here");
                    // Get the selected folder path
                    string selectedFolderPath = fbd.SelectedPath;
                    MultiPlotSettingModel.SelectedDirectory = selectedFolderPath;
                }
            });
        }
    }
}