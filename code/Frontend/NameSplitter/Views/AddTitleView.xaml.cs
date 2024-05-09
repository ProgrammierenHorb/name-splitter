using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace NameSplitter.Views
{
    /// <summary>
    /// Interaction logic for AddTitleView.xaml
    /// </summary>
    public partial class AddTitleView: Window
    {
        public AddTitleView()
        {
            InitializeComponent();
        }

        private void NumberValidationTextBox( object sender, TextCompositionEventArgs e )
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}