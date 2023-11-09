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
    /// Logika interakcji dla klasy AccountCreation.xaml
    /// </summary>
    public partial class AccountCreation : UserControl
    {

        public delegate void ClearClickedEventHandler(object sender, EventArgs e);
        public delegate void CreateClickedEventHandler(object sender, EventArgs e);
        public delegate void CancelClickedEventHandler(object sender, EventArgs e);

        public event ClearClickedEventHandler? ClearClicked;
        public event CreateClickedEventHandler? CreateClicked;
        public event CancelClickedEventHandler? CancelClicked;
        public AccountCreation()
        {
            InitializeComponent();
        }

        protected virtual void btClear_Click(object sender, RoutedEventArgs e)
        {
            if ( ClearClicked != null )
                ClearClicked(sender, e);
            
        }

        protected virtual void btCreate_Click(object sender, RoutedEventArgs e)
        {
            if ( CreateClicked != null )
                CreateClicked(sender, e);
        }

        protected virtual void btCancel_Click(object sender, RoutedEventArgs e)
        {
            if ( CancelClicked != null ) 
                CancelClicked(sender, e);
        }
    }
}
