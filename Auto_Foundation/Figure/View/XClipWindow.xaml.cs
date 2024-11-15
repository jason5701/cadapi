using Auto_Foundation.Figure.ViewModel;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Auto_Foundation.Figure.View
{
    /// <summary>
    /// Interaction logic for XClipWindow.xaml
    /// </summary>
    public partial class XClipWindow : Window
    {
        private XclipViewModel _viewModel;
        public XClipWindow(XclipViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = viewModel;
        }
        public sealed class InvertBooleanConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                if (value is bool boolValue)
                {
                    return !boolValue;
                }
                return false;
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                if (value is bool boolValue)
                {
                    return !boolValue;
                }
                return false;
            }
        }
    }
}
