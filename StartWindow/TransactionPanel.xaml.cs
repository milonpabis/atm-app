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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StartWindow
{
    /// <summary>
    /// Logika interakcji dla klasy TransactionPanel.xaml
    /// </summary>
    public partial class TransactionPanel : UserControl
    {
        public delegate void ClearClickedEventHandler(object sender, EventArgs e);
        public delegate void ProceedClickedEventHandler(object sender, EventArgs e);
        public delegate void CancelClickedEventHandler(object sender, EventArgs e);

        public event ClearClickedEventHandler? ClearClicked;
        public event ProceedClickedEventHandler? ProceedClicked;
        public event CancelClickedEventHandler? CancelClicked;
        public TransactionPanel()
        {
            InitializeComponent();
        }

        private void btClear_Click(object sender, RoutedEventArgs e)
        {
            if( ClearClicked != null )
            {
                ClearClicked(sender, e);
            }
        }

        private void btProceed_Click(object sender, RoutedEventArgs e)
        {
            if( ProceedClicked != null )
            {
                ProceedClicked(sender, e);
            }
        }

        private void btCancel_Click(object sender, RoutedEventArgs e)
        {
            if( CancelClicked != null )
            {
                CancelClicked(sender, e);
            }
        }

        private void NumericPreviewInput(object sender, TextCompositionEventArgs e)
        {
            if (!int.TryParse(e.Text, out _))
                e.Handled = true;
        }
    }
}
