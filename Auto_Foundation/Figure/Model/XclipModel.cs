namespace Auto_Foundation.Figure.Model
{
    public class XclipModel : BaseViewModel
    {
        private int _Scale;
        public int Scale
        {
            get { return _Scale; }
            set { _Scale = value; OnPropertyChanged(); }
        }
        private bool _WayToCut;
        public bool WayToCut
        {
            get { return _WayToCut; }
            set { _WayToCut = value; OnPropertyChanged(); }
        }
        private bool _ShowAgain;
        public bool ShowAgain
        {
            get { return _ShowAgain; }
            set { _ShowAgain = value; OnPropertyChanged(); }
        }
        public XclipModel()
        {
            Scale = 1;
            WayToCut = true;
            ShowAgain = true;
        }
    }
}
