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
using System.Xml.XPath;
using ATMLogic;

namespace StartWindow
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    // TODO
    // - create input limitations for TextBoxes
    // - implement functionalities:
    //   a) create user
    //   b) cardless login
    //   c) withdraw/deposit in logged
    //   d) proceed transaction
    // - add final UserControl
    public partial class MainWindow : Window
    {
        ATM atm = new ATM();
        User? LoggedUser = null;
        MainInterface mainInterface;
        LoggedPanel loggedPanel;
        AccountCreation accountCreation;
        CardLessPanel cardLessPanel;
        TransactionPanel transactionPanel;
        public MainWindow()
        {
            InitializeComponent();
            mainInterface = new MainInterface();
            loggedPanel = new LoggedPanel();
            accountCreation = new AccountCreation();
            cardLessPanel = new CardLessPanel();
            transactionPanel = new TransactionPanel();

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

            transactionPanel.ClearClicked += TransactionPanel_ClearClicked;
            transactionPanel.CancelClicked += TransactionPanel_CancelClicked;
            transactionPanel.ProceedClicked += TransactionPanel_ProceedClicked;

            this.MainContent.Content = mainInterface;

        }

        private void TransactionPanel_ProceedClicked(object sender, EventArgs e)
        {
            //MessageBox.Show("proceed: to be implemented");
            int amount;
            if(LoggedUser != null)
            {
                if (LoggedUser.PIN.Equals(transactionPanel.tbPIN.Text))
                {
                    if(int.TryParse(transactionPanel.tbAmount.Text.Trim(), out amount))
                    {
                        var result = atm.ChangeAmount(LoggedUser, amount);
                        if( result != null )
                        {

                            // when user has no money for transaction
                            if (result.GetType().Equals(typeof(UnsufficientAmountError)))
                            {
                                MessageBox.Show("NO MONEY!!!");
                            }

                            // when user exceeded limit ( cardless )
                            if (result.GetType().Equals(typeof(LimitExceededError)))
                            {
                                MessageBox.Show("Limit Exceeded!");
                            }
                        }
                        
                    }

                    // when input is wrong
                    else
                    {
                        MessageBox.Show("INCORRECT INPUT!!!");
                    }
                    
                }
                
                // when PIN is wrong
                else
                {

                }
            }
            
        }

        private void TransactionPanel_CancelClicked(object sender, EventArgs e)
        {
            ClearTransaction();
            this.MainContent.Content = loggedPanel;
        }

        private void TransactionPanel_ClearClicked(object sender, EventArgs e)
        {
            ClearTransaction();
        }

        private void CardLessPanel_ContinueClicked(object sender, EventArgs e)
        {
            //MessageBox.Show("continue: to be implemented");
            string inputCard = cardLessPanel.tbCardNumber.Text.Trim();
            string inputCVV = cardLessPanel.tbCVV.Text.Trim();
            if( atm.CanLogIn(inputCard, inputCVV))
            {
                User user = atm.RetrieveUser(inputCard, inputCVV);
                LoggedUser = user;
                atm.Limit = 500;
                ClearCardLess();
                HandleLoginSite();
            }
        }

        private void CardLessPanel_CancelClicked(object sender, EventArgs e)
        {
            ClearCardLess();
            LoggedUser = null;
            this.MainContent.Content = mainInterface;
        }

        private void CardLessPanel_ClearClicked(object sender, EventArgs e)
        {
            ClearCardLess();
        }

        public void AccountCreation_CreateClicked(object sender, EventArgs e)
        {
            //MessageBox.Show("create: to be implemented");
            User user = new User(accountCreation.tbPIN.Text.Trim(), accountCreation.tbName.Text.Trim());
            try
            {
                var result = atm.AddUser(user);
                if(result != null)
                {
                    if(result.GetType() == typeof(TypedDataError))
                    {
                        MessageBox.Show("Incorrect Data Input");
                    }

                    else
                    {
                        MessageBox.Show("UNLUCKY!!!");
                    }

                    // happens after error while creating
                    ClearAccountCreation();

                    return;
                }
                
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
                return;
            }

            // happens after creating an account

            ClearAccountCreation();
            MessageBox.Show(string.Format($"Your card is stored in cards folder\nYour PIN: {user.PIN}\nYour card number: {user.CardNumber}\nYour CVV: {user.CVV}"));
            this.MainContent.Content = mainInterface;
            
                

        }

        public void AccountCreation_CancelClicked(object sender, EventArgs e)
        {
            ClearAccountCreation();
            this.MainContent.Content = mainInterface;
        }

        public void AccountCreation_ClearClicked(object sender, EventArgs e)
        {
            ClearAccountCreation();
        }

        public void LoggedPanel_WithdrawClicked(object sender, EventArgs e)
        {
            atm.ToDeposit = false;
            this.MainContent.Content = transactionPanel;
        }

        public void LoggedPanel_DepositClicked(object sender, EventArgs e)
        {
            atm.ToDeposit = true;
            this.MainContent.Content = transactionPanel;
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
                atm.Limit = 99999;
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
                loggedPanel.lbAmount.Content = string.Format($"Your status: {LoggedUser.Amount}$");
            }
        }

        private void ClearCardLess()
        {
            cardLessPanel.tbCardNumber.Text = String.Empty;
            cardLessPanel.tbCVV.Text = String.Empty;
        }

        private void ClearAccountCreation()
        {
            accountCreation.tbName.Text = String.Empty;
            accountCreation.tbPIN.Text = String.Empty;
        }

        private void ClearTransaction()
        {
            transactionPanel.tbAmount.Text = String.Empty;
            transactionPanel.tbPIN.Text = String.Empty;
        }
    }
}
