using Auto_Foundation.Figure.Model;
using Auto_Foundation.Figure.View;
using Autodesk.AutoCAD.Geometry;
using System.Windows.Input;

namespace Auto_Foundation.Figure.ViewModel
{
    public class CombineViewModel : BaseViewModel
    {
        private SettingViewModel _SettingViewModel;
        public SettingViewModel SettingViewModel
        {
            get { return _SettingViewModel; }
            set { _SettingViewModel = value; OnPropertyChanged(); }
        }

        private LineViewModel _LineViewModel;
        public LineViewModel LineViewModel
        {
            get { return _LineViewModel; }
            set { _LineViewModel = value; OnPropertyChanged(); }
        }

        private OctagonModel _OctagonModel;
        public OctagonModel OctagonModel
        {
            get { return _OctagonModel; }
            set { _OctagonModel = value; OnPropertyChanged(); }
        }

        private Point3d _CenterPoint;
        public Point3d CenterPoint
        {
            get { return _CenterPoint; }
            set { _CenterPoint = value; OnPropertyChanged();}
        }

        #region ICommand
        public ICommand DrawBtnCommand { get; set; }
        public ICommand CancelBtnCommand { get; set; }
        #endregion
        public CombineViewModel()
        {
            OctagonModel = new OctagonModel();
            SettingViewModel = new SettingViewModel(OctagonModel);
            LineViewModel = new LineViewModel();
            DrawBtnCommand = new RelayCommand<CombineWindow>((p) => { return true; }, (p) =>
            {
                p.Hide();
                CenterPoint = ClCAD.GetPointsFromUser("pick center");
                DrawAutoCAD();
            });
            CancelBtnCommand = new RelayCommand<CombineWindow>((p) => { return true; }, (p) =>
            {
                p.Close();
            });
        }

        private void DrawAutoCAD()
        {
            OctagonModel.SettingModel.DrawFondation(CenterPoint);
        }
    }
}
