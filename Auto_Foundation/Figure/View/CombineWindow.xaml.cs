using Auto_Foundation.Figure.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Auto_Foundation.Figure.View
{
    /// <summary>
    /// Interaction logic for CombineWindow.xaml
    /// </summary>
    public partial class CombineWindow : Window
    {
        private CombineViewModel _viewModel;
        public CombineWindow(CombineViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = viewModel;
        }
    }
}
