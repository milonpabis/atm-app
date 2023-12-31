﻿using System;
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
using System.Windows.Threading;
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
        SummaryPanel summaryPanel;

        
        public MainWindow()
        {
            InitializeComponent();
            mainInterface = new MainInterface();
            loggedPanel = new LoggedPanel();
            accountCreation = new AccountCreation();
            cardLessPanel = new CardLessPanel();
            transactionPanel = new TransactionPanel();
            summaryPanel = new SummaryPanel();

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

            summaryPanel.OnTick += SummaryPanel_OnTick;

            this.MainContent.Content = mainInterface;

        }

        private void SummaryPanel_OnTick(object sender, int count, EventArgs e)
        {
            if (count >= 0)
                summaryPanel.lbTimer.Content = $"Exiting in {count}s...";
            else
            {
                this.MainContent.Content = mainInterface;
                summaryPanel.lbTimer.Content = "Exiting in 3s...";
            }

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
                                ErrorTransaction("Insufficient funds!");
                            }

                            // when user exceeded limit ( cardless )
                            if (result.GetType().Equals(typeof(LimitExceededError)))
                            {
                                ErrorTransaction("Limit exceeded!");
                            }
                        }

                        // when success
                        else
                        {
                            LoggedUser = null;
                            ClearTransaction();
                            this.MainContent.Content = summaryPanel;

                        }
                        
                    }

                    // when input is wrong
                    else
                    {
                        ErrorTransaction("Incorrect input!");
                    }
                    
                }
                
                // when PIN is wrong
                else
                {
                    ErrorTransaction("Wrong PIN!");
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
            else
            {
                ErrorCardLess("Wrong input!");
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
            User user = new User(accountCreation.tbPIN.Text.Trim(), accountCreation.tbName.Text.Trim());
            try
            {
                var result = atm.AddUser(user);
                if(result != null)
                {
                    if(result.GetType() == typeof(TypedDataError))
                        ErrorAccountCreation("Wrong input!");
                    else
                        ErrorAccountCreation("System error! Please try again.");
                    return;
                }
                
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
                return;
            }

            // happens after successful creation

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
            this.transactionPanel.lbTitle.Content = "WITHDRAW";
            this.MainContent.Content = transactionPanel;
        }

        public void LoggedPanel_DepositClicked(object sender, EventArgs e)
        {
            atm.ToDeposit = true;
            this.transactionPanel.lbTitle.Content = "DEPOSIT";
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
            // successful login with card
            if (user != null)
            {
                LedControlMainInterface(Colors.Green, true);
                LoggedUser = user;
                atm.Limit = 20000;
            }
            // unsuccessful login with card
            else
                LedControlMainInterface(Colors.Red, false);
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

        private void LedControlMainInterface(Color color, bool success)
        {
            this.mainInterface.ledControl.Fill = new SolidColorBrush(color);
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Tick += (sender, e) =>
            {
                this.mainInterface.ledControl.Fill = null;
                timer.Stop();
                if (success)
                    HandleLoginSite();
            };
            timer.Start();
        }

        private void ErrorAccountCreation(string message)
        {
            this.accountCreation.lbError.Content = message;
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(2);
            timer.Tick += (sender, e) =>
            {
                accountCreation.lbError.Content = String.Empty;
                timer.Stop();
            };
            timer.Start();
        }

        private void ErrorCardLess(string message)
        {
            this.cardLessPanel.lbError.Content = message;
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(2);
            timer.Tick += (sender, e) =>
            {
                cardLessPanel.lbError.Content = String.Empty;
                timer.Stop();
            };
            timer.Start();
        }

        private void ErrorTransaction(string message)
        {
            this.transactionPanel.lbError.Content = message;
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(2);
            timer.Tick += (sender, e) =>
            {
                transactionPanel.lbError.Content = String.Empty;
                timer.Stop();
            };
            timer.Start();

        }
    }
}
