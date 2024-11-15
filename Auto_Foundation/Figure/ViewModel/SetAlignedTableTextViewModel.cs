using Auto_Foundation.Figure.Model;
using Auto_Foundation.Figure.View;
using System.Windows;
using System.Windows.Input;

namespace Auto_Foundation.Figure.ViewModel
{
    public class SetAlignedTableTextViewModel : BaseViewModel
    {
        private SetAlignedTableTextModel _SetAlignedTableTextModel;
        public SetAlignedTableTextModel SetAlignedTableTextModel
        {
            get { return _SetAlignedTableTextModel; }
            set { _SetAlignedTableTextModel = value; }
        }
        public ICommand SetAlignedTableTextCancelCommand { get; set; }
        public ICommand SetAlignedTableTextConfirmCommand { get; set; }
        public ICommand LeftAlignedCheckCommand { get; set; }
        public ICommand CenterAlignedCheckCommand { get; set; }
        public ICommand RightAlignedCheckCommand { get; set; }
        
        public SetAlignedTableTextViewModel()
        {
            SetAlignedTableTextModel = new SetAlignedTableTextModel();
            LeftAlignedCheckCommand = new RelayCommand<SetAlignedTableText>((p) => { return true; }, (p) =>
            {
                SetAlignedTableTextModel.LeftAligned = true;
                SetAlignedTableTextModel.CenterAligned = false;
                SetAlignedTableTextModel.RightAligned = false;
                SetAlignedTableTextModel.Space = 0;
            });
            CenterAlignedCheckCommand = new RelayCommand<SetAlignedTableText>((p) => { return true; }, (p) =>
            {
                SetAlignedTableTextModel.LeftAligned = false;
                SetAlignedTableTextModel.CenterAligned = true;
                SetAlignedTableTextModel.RightAligned = false;
                SetAlignedTableTextModel.Space = 0;
            });
            RightAlignedCheckCommand = new RelayCommand<SetAlignedTableText>((p) => { return true; }, (p) =>
            {
                SetAlignedTableTextModel.LeftAligned = false;
                SetAlignedTableTextModel.CenterAligned = false;
                SetAlignedTableTextModel.RightAligned = true;
                SetAlignedTableTextModel.Space = 0;
            });
            SetAlignedTableTextCancelCommand = new RelayCommand<SetAlignedTableText>((p) => { return true; }, (p) =>
            {
                p.Hide();
                SetAlignedTableTextModel.LeftAligned = true;
                SetAlignedTableTextModel.CenterAligned = false;
                SetAlignedTableTextModel.RightAligned = false;
                SetAlignedTableTextModel.Space = 0;
            });
            SetAlignedTableTextConfirmCommand = new RelayCommand<SetAlignedTableText>((p) => { return true; }, (p) =>
            {
                p.Hide();
                var direction = string.Empty;
                int space = SetAlignedTableTextModel.Space;
                if (SetAlignedTableTextModel.LeftAligned)
                {
                    direction = "left";
                }else if (SetAlignedTableTextModel.CenterAligned)
                {
                    direction = "center";
                }else if(SetAlignedTableTextModel.RightAligned)
                {
                    direction = "right";
                }
                ClCAD.AlignText(direction, space);
            });
        }
    }
}
