using Microsoft.Identity.Client;
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
    /// Logika interakcji dla klasy CardLessPanel.xaml
    /// </summary>
    public partial class CardLessPanel : UserControl
    {

        public delegate void ClearClickedEventHandler(object sender, EventArgs e);
        public delegate void ContinueClickedEventHandler(object sender, EventArgs e);
        public delegate void CancelClickedEventHandler(object sender, EventArgs e);

        public event ClearClickedEventHandler? ClearClicked;
        public event ContinueClickedEventHandler? ContinueClicked;
        public event CancelClickedEventHandler? CancelClicked;
        public CardLessPanel()
        {
            InitializeComponent();
        }

        private void btClear_Click(object sender, RoutedEventArgs e)
        {
            if( ClearClicked != null)
            {
                ClearClicked(sender, e);
            }
        }

        private void btContinue_Click(object sender, RoutedEventArgs e)
        {
            if(  ContinueClicked != null)
            {
                ContinueClicked(sender, e);
            }
        }

        private void btCancel_Click(object sender, RoutedEventArgs e)
        {
            if(  CancelClicked != null)
            {
                CancelClicked(sender, e);
            }
        }

        private void NumericPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if(!int.TryParse(e.Text, out _))
                e.Handled = true;
        }
    }
}
