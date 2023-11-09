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
using ATMLogic;

namespace StartWindow
{
    /// <summary>
    /// Logika interakcji dla klasy LoggedPanel.xaml
    /// </summary>
    public partial class LoggedPanel : UserControl
    {

        public delegate void CancelClickedEventHandler(object sender, EventArgs e);
        public delegate void DepositClickedEventHandler(object sender, EventArgs e);
        public delegate void WithdrawClickedEventHandler(object sender, EventArgs e);

        public event CancelClickedEventHandler? CancelClicked;
        public event DepositClickedEventHandler? DepositClicked;
        public event WithdrawClickedEventHandler? WithdrawClicked;

        public LoggedPanel()
        {
            InitializeComponent();
        }

        protected virtual void btCancel_Click(object sender, RoutedEventArgs e)
        {
            if( CancelClicked != null )
            {
                CancelClicked(sender, e);
            }
        }

        protected virtual void btDeposit_Click(object sender, RoutedEventArgs e)
        {
            if( DepositClicked != null )
            {
                DepositClicked(sender, e);
            }
        }

        protected virtual void btWithdraw_Click(object sender, RoutedEventArgs e)
        {
            if ( WithdrawClicked != null )
            {
                WithdrawClicked(sender, e);
            }
        }
    }
}
