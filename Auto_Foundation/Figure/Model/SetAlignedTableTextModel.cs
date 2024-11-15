using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Auto_Foundation.Figure.Model
{
    public class SetAlignedTableTextModel : BaseViewModel
    {
        private bool _LeftAligned;
        public bool LeftAligned
        {
            get { return _LeftAligned; }
            set { _LeftAligned = value; OnPropertyChanged(); }
        }
        private bool _CenterAligned;
        public bool CenterAligned
        {
            get { return _CenterAligned; }
            set { _CenterAligned = value; OnPropertyChanged(); }
        }
        private bool _RightAligned;
        public bool RightAligned
        {
            get { return _RightAligned; }
            set { _RightAligned = value; OnPropertyChanged(); }
        }
        private int _Space;
        public int Space
        {
            get { return _Space; }
            set { _Space = value; OnPropertyChanged(); }
        }
        public SetAlignedTableTextModel()
        {
            LeftAligned = true;
            CenterAligned = false;
            RightAligned = false;
            Space = 0;
        }
    }
}
