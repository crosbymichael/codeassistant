using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CodeAssistant.Controls
{
    /// <summary>
    /// Interaction logic for ArgumentsControl.xaml
    /// </summary>
    public partial class ArgumentsControl : UserControl
    {
        public ArgumentsControl()
        {
            InitializeComponent();
        }

        private void txtArguments_GotFocus(object sender, RoutedEventArgs e)
        {
            if (this.txtArguments.Text == "Arguments...")
            {
                this.txtArguments.Text = string.Empty;
            }
        }
    }
}
