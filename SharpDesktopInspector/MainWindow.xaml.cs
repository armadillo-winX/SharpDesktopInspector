using System.Windows;

namespace SharpDesktopInspector
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int Target { get; set; }

        public string? TargetType { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void OKButtonClick(object sender, RoutedEventArgs e)
        {
            this.Target = TargetComboBox.SelectedIndex;
            this.TargetType = TargetTypeBox.Text;
            this.DialogResult = true;
        }

        private void TargetComboBoxSelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            TargetTypeBox.IsEnabled = TargetComboBox.SelectedIndex == 1;
        }

        private void MainWindowLoaded(object sender, RoutedEventArgs e)
        {
            TargetComboBox.SelectedIndex = this.Target;
            TargetTypeBox.Text = this.TargetType != null ? this.TargetType : string.Empty;
        }
    }
}
