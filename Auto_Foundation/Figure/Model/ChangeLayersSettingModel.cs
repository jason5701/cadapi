using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Auto_Foundation.Figure.Model
{
    public class ChangeLayersSettingModel:BaseViewModel
    {
        private ObservableCollection<LayerClass> _LayerList;
        public ObservableCollection<LayerClass> LayerList
        {
            get { return _LayerList; }
            set { _LayerList = value; OnPropertyChanged(); }
        }
        private LayerClass _SelectedLayer;
        public LayerClass SelectedLayer
        {
            get { return _SelectedLayer; }
            set { _SelectedLayer = value; OnPropertyChanged(); }
        }
        private string _ColorValue;
        public string ColorValue
        {
            get { return _ColorValue; }
            set { _ColorValue = value; OnPropertyChanged(); }
        }

        public ChangeLayersSettingModel() 
        { 
            LayerList = new ObservableCollection<LayerClass>();
            SelectedLayer = null;
            ColorValue = "White";
        }
    }
}
