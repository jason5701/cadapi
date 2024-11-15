using Auto_Foundation.Figure.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Interaction logic for SetAlignedTableText.xaml
    /// </summary>
    public partial class SetAlignedTableText : Window
    {
        private SetAlignedTableTextViewModel _viewModel;

        public SetAlignedTableText(SetAlignedTableTextViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = viewModel;
        }
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !int.TryParse(e.Text, out _);
        }
        public sealed class BooleanToVisibilityConverter : IValueConverter
        {
            public bool IsReversed { get; set; }
            public bool UseHidden { get; set; }
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                var val = System.Convert.ToBoolean(value, CultureInfo.InvariantCulture);
                if (this.IsReversed)
                {
                    val = !val;
                }
                if (val)
                {
                    return Visibility.Visible;
                }
                return this.UseHidden ? Visibility.Collapsed : Visibility.Hidden;
            }
            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }
    }
}
