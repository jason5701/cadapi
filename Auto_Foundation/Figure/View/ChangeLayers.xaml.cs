using Auto_Foundation.Figure.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Auto_Foundation.Figure.View
{
    /// <summary>
    /// Interaction logic for ChangeLayers.xaml
    /// </summary>
    public partial class ChangeLayers : Window
    {
        private ChangeLayersViewModel _viewModel;
        public ChangeLayers(ChangeLayersViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = viewModel;
        }
        private void DataGridColorCell_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var cell = sender as DataGridCell;
            if (cell != null)
            {
                var dataContext = cell.DataContext as LayerClass;

                if (dataContext != null)
                {
                    _viewModel.SelectedColorCommand(dataContext);
                }
            }
        }
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cb = sender as ComboBox;
            if (cb.IsDropDownOpen)
            {
                DataGridCell cell = FindVisualParent<DataGridCell>(cb);
                if (cell != null && cell.IsEditing)
                {
                    DataGrid dataGrid = FindVisualParent<DataGrid>(cb);
                    if (dataGrid != null)
                    {
                        _viewModel.SelectedCommand(cb.SelectedItem);
                        dataGrid.CommitEdit(DataGridEditingUnit.Cell, true);
                    }
                }
            }
        }
        private T FindVisualParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);

            if (parentObject == null) return null;

            T parent = parentObject as T;
            if (parent != null)
                return parent;
            else
                return FindVisualParent<T>(parentObject);
        }

    }
}
