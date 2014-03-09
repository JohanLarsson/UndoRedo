namespace UndoRedo
{
    using System.Windows;
    using System.Windows.Data;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new Vm();
        }

        private void Binding_OnSourceUpdated(object sender, DataTransferEventArgs e)
        {
            
        }

        private void Binding_OnTargetUpdated(object sender, DataTransferEventArgs e)
        {

        }
    }
}
