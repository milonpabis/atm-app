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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    // Main window ma byc pusty i zrobic kontrolke startowa, mainwindow bedzie tylko odbieral eventy i je obslugiwal
    public partial class MainWindow : Window
    {
        ATM atm = new ATM();
        User? LoggedUser = null;
        MainInterface mainInterface;
        LoggedPanel loggedPanel;
        AccountCreation accountCreation;
        CardLessPanel cardLessPanel;
        public MainWindow()
        {
            InitializeComponent();
            mainInterface = new MainInterface();
            loggedPanel = new LoggedPanel();
            accountCreation = new AccountCreation();
            cardLessPanel = new CardLessPanel();

            mainInterface.CreateAccountClicked += MainInterface_CreateAccountClicked;
            mainInterface.CardLessClicked += MainInterface_CardLessClicked;
            mainInterface.CardInserted += MainInterface_CardInserted;

            loggedPanel.CancelClicked += LoggedPanel_CancelClicked;
            loggedPanel.DepositClicked += LoggedPanel_DepositClicked;
            loggedPanel.WithdrawClicked += LoggedPanel_WithdrawClicked;

            accountCreation.ClearClicked += AccountCreation_ClearClicked;
            accountCreation.CancelClicked += AccountCreation_CancelClicked;
            accountCreation.CreateClicked += AccountCreation_CreateClicked;

            cardLessPanel.ClearClicked += CardLessPanel_ClearClicked;
            cardLessPanel.CancelClicked += CardLessPanel_CancelClicked;
            cardLessPanel.ContinueClicked += CardLessPanel_ContinueClicked;

            this.MainContent.Content = mainInterface;

        }

        private void CardLessPanel_ContinueClicked(object sender, EventArgs e)
        {
            MessageBox.Show("continue: to be implemented");
        }

        private void CardLessPanel_CancelClicked(object sender, EventArgs e)
        {
            MessageBox.Show("cancel: to be implemented");
        }

        private void CardLessPanel_ClearClicked(object sender, EventArgs e)
        {
            MessageBox.Show("clear: to be implemented");
        }

        public void AccountCreation_CreateClicked(object sender, EventArgs e)
        {
            MessageBox.Show("create: to be implemented");
        }

        public void AccountCreation_CancelClicked(object sender, EventArgs e)
        {
            accountCreation.tbName.Text = String.Empty;
            accountCreation.tbPIN.Text = String.Empty;
            this.MainContent.Content = mainInterface;
        }

        public void AccountCreation_ClearClicked(object sender, EventArgs e)
        {
            accountCreation.tbName.Text = String.Empty;
            accountCreation.tbPIN.Text = String.Empty;
        }

        public void LoggedPanel_WithdrawClicked(object sender, EventArgs e)
        {
            MessageBox.Show("withdraw: to be implemented");
        }

        public void LoggedPanel_DepositClicked(object sender, EventArgs e)
        {
            MessageBox.Show("deposit: to be implemented");
        }

        public void LoggedPanel_CancelClicked(object sender, EventArgs e)
        {
            LoggedUser = null;
            this.MainContent.Content = mainInterface;
        }

        public void MainInterface_CardInserted(object source, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            string filePath = files[0];

            var user = CardService.RetrieveUser(atm, filePath);
            if (user != null)
            {
                LoggedUser = user;
                HandleLoginSite();
            }
            else
                MessageBox.Show("Incorrect input");
        }

        public void MainInterface_CardLessClicked(object source, EventArgs e)
        {
            this.MainContent.Content = cardLessPanel;
        }

        public void MainInterface_CreateAccountClicked(object source, EventArgs e)
        {
            this.MainContent.Content = accountCreation;
        }








        private void HandleLoginSite()
        {
            if( LoggedUser != null )
            {
                this.MainContent.Content = loggedPanel;
                loggedPanel.lbWelcome.Content = string.Format("Hello " + LoggedUser.Name);
            }
        }
    }
}
