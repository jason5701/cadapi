using Auto_Foundation.Figure.Model;
using Auto_Foundation.Figure.View;
using System.Windows;
using System.Windows.Input;

namespace Auto_Foundation.Figure.ViewModel
{
    public class XclipViewModel : BaseViewModel
    {
        private XclipModel _XclipModel;
        public XclipModel XclipModel
        {
            get { return _XclipModel; }
            set { _XclipModel = value; }
        }

        public ICommand TrimCheckCommand { get; set; }
        public ICommand XClipcheckCommand { get; set; }
        public ICommand XClipCommand { get; set; }
        public ICommand XClipCancelCommand { get; set; }
        
        public XclipViewModel()
        {
            XclipModel = new XclipModel();
            TrimCheckCommand = new RelayCommand<XClipWindow>((p) => { return true; }, (p) =>
            {
                XclipModel.WayToCut = true;
            });
            XClipcheckCommand = new RelayCommand<XClipWindow>((p) => { return true; }, (p) =>
            {
                XclipModel.WayToCut = false;
            });
            XClipCommand = new RelayCommand<XClipWindow>((p) => { return true; }, (p) =>
            {
                p.Hide();
                ClCAD.ClipObjectCommand(XclipModel.Scale);
                if (XclipModel.ShowAgain)
                {
                    p.Show();
                }
            });
            XClipCancelCommand = new RelayCommand<XClipWindow>((p) => { return true; }, (p) =>
            {
                p.Hide();
                XclipModel.WayToCut = true;
                XclipModel.Scale = 1;
            });
        }
    }
}
