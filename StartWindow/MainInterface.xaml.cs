using ATMLogic;
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
    /// Logika interakcji dla klasy MainInterface.xaml
    /// </summary>
    public partial class MainInterface : UserControl
    {

        public delegate void CardInsertionEventHandler(object source, DragEventArgs e);
        public delegate void CardLessClickedEventHandler(object source, EventArgs e);
        public delegate void CreateAccountClickedEventHandler(object source, EventArgs e);

        public event CardInsertionEventHandler? CardInserted;
        public event CardLessClickedEventHandler? CardLessClicked;
        public event CreateAccountClickedEventHandler? CreateAccountClicked;
        public MainInterface()
        {
            InitializeComponent();
        }

        protected virtual void btCardLess_Click(object sender, RoutedEventArgs e)
        {
            if(CardLessClicked != null)
                CardLessClicked(sender, e);
        }

        protected virtual void btCreateAccount_Click(object sender, RoutedEventArgs e)
        {
            if (CreateAccountClicked != null)
                CreateAccountClicked(sender, e);
        }


        protected virtual void imgCardInsertion_Drop(object sender, DragEventArgs e)
        {
            if(CardInserted != null)
                CardInserted(sender, e);
        }
    }
}
