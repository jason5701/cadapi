using Auto_Foundation.Figure.Model;
using Auto_Foundation.Figure.View;
using System;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using ColorDialog = Autodesk.AutoCAD.Windows.ColorDialog;
using MessageBox = System.Windows.MessageBox;

namespace Auto_Foundation.Figure.ViewModel
{
    public class SettingViewModel:BaseViewModel
    {
        private OctagonModel _OctagonModel;
        public OctagonModel OctagonModel
        {
            get { return _OctagonModel; }
            set { _OctagonModel = value; }
        }

        #region Command
        public ICommand LoadSettingViewCommand {  get; set; }
        public ICommand ColorBtnCommand { get; set; }
        public ICommand BottomFDNLength_TextChanged_Command { get; set; }
        public ICommand BottomFDNHeight_TextChanged_Command { get; set; }
        public ICommand TopFDNLength_TextChanged_Command { get; set; }
        public ICommand TopFDNHeight_TextChanged_Command { get; set; }
        public ICommand BCDCircle_TextChanged_Command { get; set; }
        
        # endregion
        public SettingViewModel(OctagonModel octagonModel)
        {
            OctagonModel = octagonModel;
            LoadSettingViewCommand = new RelayCommand<CombineWindow>((p) => { return true; }, (p) =>
            {
                SettingView uc = FindChildClass.FindChild<SettingView>(p, "SettingUC");
                DrawSettingImage(uc);
            });
            ColorBtnCommand = new RelayCommand<CombineWindow>((p) => { return true; }, (p) =>
            {
                ColorDialog cd = new ColorDialog();
                DialogResult result = cd.ShowDialog();
                if (result == DialogResult.OK)
                {
                    OctagonModel.SettingModel.ColorIndex = cd.Color.ColorIndex;
                    SolidColorBrush sb = new SolidColorBrush(Color.FromRgb(cd.Color.ColorValue.R, cd.Color.ColorValue.G, cd.Color.ColorValue.B));
                    OctagonModel.SettingModel.ColorValue = sb.ToString();
                }
            });
            BottomFDNLength_TextChanged_Command = new RelayCommand<CombineWindow>((p) => { return true; }, (p) =>
            {
                if(OctagonModel.SettingModel.BottomFDNLength > OctagonModel.SettingModel.TopFDNLength)
                {
                    SettingView uc = FindChildClass.FindChild<SettingView>(p, "SettingUC");
                    uc.Section.Children.Clear();
                    uc.Plan.Children.Clear();
                    OctagonModel.SettingModel.BottomFDNCenter =
                            Math.Ceiling(OctagonModel.SettingModel.BottomFDNLength * (Math.Sqrt(2) - 1));
                    DrawSettingImage(uc);
                }
                else
                {
                    MessageBox.Show("Top Foundation Length is {0}", OctagonModel.SettingModel.TopFDNLength.ToString());
                    OctagonModel.SettingModel.BottomFDNLength = OctagonModel.SettingModel.TopFDNLength + 100;
                }
                
            });
            BottomFDNHeight_TextChanged_Command = new RelayCommand<CombineWindow>((p) => { return true; }, (p) =>
            {
                SettingView uc = FindChildClass.FindChild<SettingView>(p, "SettingUC");
                uc.Section.Children.Clear();
                uc.Plan.Children.Clear();
                DrawSettingImage(uc);
            });
            TopFDNLength_TextChanged_Command = new RelayCommand<CombineWindow>((p) => { return true; }, (p) =>
            {
                if(OctagonModel.SettingModel.TopFDNLength > OctagonModel.SettingModel.BottomFDNLength)
                {
                    MessageBox.Show("Bottom Foundation Length is {0}", OctagonModel.SettingModel.TopFDNLength.ToString());
                    OctagonModel.SettingModel.TopFDNLength = OctagonModel.SettingModel.BottomFDNLength - 100;
                }
                else
                {
                    SettingView uc = FindChildClass.FindChild<SettingView>(p, "SettingUC");
                    uc.Section.Children.Clear();
                    uc.Plan.Children.Clear();
                    OctagonModel.SettingModel.TopFDNCenter =
                            Math.Ceiling(OctagonModel.SettingModel.TopFDNLength * (Math.Sqrt(2) - 1));
                    DrawSettingImage(uc);
                }
            });
            TopFDNHeight_TextChanged_Command = new RelayCommand<CombineWindow>((p) => { return true; }, (p) =>
            {
                SettingView uc = FindChildClass.FindChild<SettingView>(p, "SettingUC");
                uc.Section.Children.Clear();
                uc.Plan.Children.Clear();
                DrawSettingImage(uc);
            });
            BCDCircle_TextChanged_Command = new RelayCommand<CombineWindow>((p) => { return true; }, (p) =>
            {
                SettingView uc = FindChildClass.FindChild<SettingView>(p, "SettingUC");
                uc.Section.Children.Clear();
                uc.Plan.Children.Clear();
                DrawSettingImage(uc);
            });
        }

        private void DrawSettingImage(SettingView uc)
        {
            DrawCanvas.DrawSection(uc.Section, OctagonModel.SettingModel.BottomFDNLength, OctagonModel.SettingModel.TopFDNLength,
                OctagonModel.SettingModel.BottomFDNHeight, OctagonModel.SettingModel.TopFDNHeight, OctagonModel.SettingModel.Depth);
            DrawCanvas.DrawPlan(uc.Plan, OctagonModel.SettingModel.BottomFDNLength, OctagonModel.SettingModel.TopFDNLength,
                OctagonModel.SettingModel.BottomFDNCenter, OctagonModel.SettingModel.TopFDNCenter, OctagonModel.SettingModel.BCDCircle);
        }
    }
}
