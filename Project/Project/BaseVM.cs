using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
//using System.Windows.Media;

namespace Model
{
    public class BaseVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /* private System.Windows.Media.Brush _Colour = System.Windows.Media.Brushes.Red;

        public System.Windows.Media.Brush Colour
        {
            get { return _Colour; }
            set
            {
                Colour = value;
                OnPropertyChanged("Colour");
            }
        } 
         */
    }
    public class DelegateCommand : ICommand
    {
        public Predicate<object> CanExecuteFunction { get; set; }
        public Action<object> ExecuteFunction { get; set; }
        public bool CanExecute(object parameter)
        {
            if (CanExecuteFunction != null)
                return CanExecuteFunction(parameter);
            else
                return true;
        }
        event EventHandler _CanExecuteChanged = (s, e) => { };
        public event EventHandler CanExecuteChanged
        {
            add { _CanExecuteChanged += value; }
            remove { _CanExecuteChanged -= value; }
        }
        public void OnCanExecuteChanged()
        {
            _CanExecuteChanged(this, new EventArgs());
        }
        public void Execute(object parameter)
        {
            if (ExecuteFunction != null) ExecuteFunction(parameter);
        }
    }
    public class DeadCommand : ICommand
    {
        public bool CanExecute(object parameter)
        {
            return false;
        }
        public event EventHandler CanExecuteChanged;
        public void Execute(object parameter)
        {
            throw new NotImplementedException();
        }
    }
}
