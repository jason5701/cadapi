using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auto_Foundation.Figure.Model
{
    public class OctagonModel:BaseViewModel
    {
        private SettingModel _SettingModel;
        public SettingModel SettingModel
        {
            get { return _SettingModel; }
            set { _SettingModel = value; OnPropertyChanged(); }
        }

        public OctagonModel()
        {
            SettingModel = new SettingModel();
            SettingModel.CreateLayer();
            SettingModel.CreateTextStyle();
            SettingModel.CreateDimStyle();
        }
    }
}
