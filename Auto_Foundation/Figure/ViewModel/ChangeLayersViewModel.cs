using Auto_Foundation.Figure.Model;
using Auto_Foundation.Figure.View;
using Autodesk.AutoCAD.Colors;
using Autodesk.AutoCAD.DatabaseServices;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;
using ColorDialog = Autodesk.AutoCAD.Windows.ColorDialog;

namespace Auto_Foundation.Figure.ViewModel
{
    public class ChangeLayersViewModel:BaseViewModel
    {
        private ChangeLayersSettingModel _ChangeLayersSettingModel;
        public ChangeLayersSettingModel ChangeLayersSettingModel
        {
            get { return _ChangeLayersSettingModel; }
            set { _ChangeLayersSettingModel = value; }
        }
        public ICommand GetObjectCommand { get; set; }
        public ICommand CreateNewLayerCommand { get; set; }
        public ICommand ClearCommand { get; set; }
        public ICommand RemoveObjectCommand { get; set; }
        public ICommand ApplyLayerCommand { get; set; }
        public ICommand SelectObjectCommand { get; set; }
        
        public ChangeLayersViewModel()
        {
            ChangeLayersSettingModel = new ChangeLayersSettingModel();
            GetObjectCommand = new RelayCommand<ChangeLayers>((p) => { return true; }, (p) =>
            {
                p.Hide();
                var layers = new ObservableCollection<LayerClass>();
                var ss = ClCAD.GetSelctionFromUser();
                if (ss != null)
                {
                    layers = ClCAD.Getlayer(ss);
                    if (ChangeLayersSettingModel.LayerList.Count == 0)
                    {
                        ChangeLayersSettingModel.LayerList = layers;
                    }
                    else
                    {
                        foreach (var layer in layers)
                        {
                            if (!ChangeLayersSettingModel.LayerList.Any(l => l.LayerName == layer.LayerName))
                            {
                                ChangeLayersSettingModel.LayerList.Add(layer);
                            }
                        }
                    }
                    p.ShowDialog();
                }
                else
                {
                    p.ShowDialog();
                   return;
                }
            });
            CreateNewLayerCommand = new RelayCommand<ChangeLayers>((p) => { return true; }, (p) =>
            {
                var l = "new Layer";
                ClCAD.CreateLayer(l, 0, "Continuous", LineWeight.ByLineWeightDefault, false);
                var findLayer = ClCAD.Getlayer(l);
                ChangeLayersSettingModel.LayerList.Add(findLayer);

            });
            RemoveObjectCommand = new RelayCommand<ChangeLayers>((p) => { return true; }, (p) =>
            {
                if (ChangeLayersSettingModel.SelectedLayer == null) return;
                else
                {
                    ChangeLayersSettingModel.LayerList.Remove(ChangeLayersSettingModel.SelectedLayer);
                }
            });
            ClearCommand = new RelayCommand<ChangeLayers>((p) => { return true; }, (p) =>
            {
                ChangeLayersSettingModel.LayerList.Clear();
            });
            ApplyLayerCommand = new RelayCommand<ChangeLayers>((p) => { return true; }, (p) =>
            {
                MessageBox.Show("ApplyLayerCommand clicked");
            });
            SelectObjectCommand = new RelayCommand<ChangeLayers>((p) => { return true; }, (p) =>
            {
                p.Hide();
                var ss = ClCAD.GetSelctionFromUser();
                if(ss!= null)
                {
                    short color = ChangeLayersSettingModel.SelectedLayer.ColorIndex;
                    LineWeight weight = ChangeLayersSettingModel.SelectedLayer.Weight;
                    string type = ChangeLayersSettingModel.SelectedLayer.Type;
                    ClCAD.SetObjectColor(ss, color);
                    ClCAD.SetObjectLineWeight(ss, weight);
                    ClCAD.SetObjectLinetype(ss, type);
                    p.ShowDialog();
                }
                else
                {
                    p.ShowDialog();
                    return;
                }
            });
        }

        public void SelectedCommand(object selectedItem)
        {
            if (selectedItem != null)
            {
                var l = ChangeLayersSettingModel.LayerList.FirstOrDefault(layer => layer.LayerName == ChangeLayersSettingModel.SelectedLayer.LayerName);

                if (selectedItem is string str)
                {
                    l.Type = str;
                }else if(selectedItem is LineWeight lw)
                {
                    l.Weight = lw;
                }else if(selectedItem is bool b)
                {
                    l.Plot = b;
                }
            }
        }
        public void SelectedColorCommand(LayerClass lc)
        {
            ColorDialog cd = new ColorDialog();
            var selectedLayer = ChangeLayersSettingModel.SelectedLayer;
            if (selectedLayer != null)
            {
                cd.Color = Color.FromColorIndex(ColorMethod.ByAci, selectedLayer.ColorIndex);
            }
            DialogResult result = cd.ShowDialog();
            if (result == DialogResult.OK)
            {
                var l = ChangeLayersSettingModel.LayerList.FirstOrDefault(layer => layer.LayerName == selectedLayer.LayerName);
                if (l != null) 
                {
                    l.Color = cd.Color;
                    l.ColorIndex = cd.Color.ColorIndex;
                }
            }
        }
    }
}
