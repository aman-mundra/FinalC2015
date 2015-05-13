using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows;

namespace CheckBook
{
    public class CheckBookVM : BaseVM
    {
        public CheckBookVM()
        {
        }

        CbDb _Db = new CbDb();

        private int _RowsPerPage = 5;
        private int _CurrentPage = 1;
        public int CurrentPage
        {
            get { return _CurrentPage; }
            set { _CurrentPage = value; OnPropertyChanged(); OnPropertyChanged("CurrentTransactions"); }
        }

        private Transaction _newTransaction = new Transaction { Date = DateTime.Now };
        public Transaction newTransaction
        {
            get { return _newTransaction; }
            set { _newTransaction = value; OnPropertyChanged(); }

        }

        private ObservableCollection<Transaction> _Transactions;
        public ObservableCollection<Transaction> Transactions
        {
            get { return _Transactions; }
            set { _Transactions = value; OnPropertyChanged(); OnPropertyChanged("Accounts"); }
        }

        public IEnumerable<Account> Accounts
        {
            get { return _Db.Accounts.Local; }
        }

        public IEnumerable<Transaction> CurrentTransactions
        {
            get { return Transactions.Skip((_CurrentPage - 1) * _RowsPerPage).Take(_RowsPerPage); }
        }

        private DelegateCommand _SaveTransaction;

        public ICommand Save
        {
            get
            {
                if (_SaveTransaction == null)
                {
 
                    _SaveTransaction = new DelegateCommand
                    {
                        ExecuteFunction = x  =>
                            {
                                _Db.Transactions.Add(_newTransaction);
                                var updateac = (from A in Accounts where A == _newTransaction.Account select A).Single();
                                updateac.Balance = updateac.Balance + _newTransaction.Amount; 
                                _Db.SaveChanges();
                                _newTransaction = new Transaction {Date = DateTime.Now};
                            },
                            CanExecuteFunction = _ => _newTransaction.Account != null && _newTransaction.Amount != 0
                };
                _newTransaction.PropertyChanged += (s, e) => _SaveTransaction.OnCanExecuteChanged();
            }
                return _SaveTransaction;
            }
        }





                            
                    

               

        public DelegateCommand NewTransaction
        {
            get
            {
                return new DelegateCommand
                {
                    ExecuteFunction = _ =>
                    {
                        Transactions.Add(new Transaction { });
                        CurrentPage = Transactions.Count / _RowsPerPage + 1;
                    }
                };
            }
        }

        private string _Name;

        public string Name
        {
            get { return _Name; }
            set { _Name = value; OnPropertyChanged(); }
        }
        private string _Email;

        public string Email
        {
            get { return _Email; }
            set { _Email = value; OnPropertyChanged(); }
        }
        private string _Picture;

        public string Picture
        {
            get { return _Picture; }
            set { _Picture = value; OnPropertyChanged(); }
        }

        public void Fill()
        {
            Transactions = _Db.Transactions.Local;
            _Db.Accounts.ToList();
            _Db.Transactions.ToList();

        }
    }
}
