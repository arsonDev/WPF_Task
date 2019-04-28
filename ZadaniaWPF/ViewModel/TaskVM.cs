using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using ZadaniaWPF.Model;

namespace ZadaniaWPF.ViewModel
{
    class TaskVM : INotifyPropertyChanged
    {
        #region props_var
        private Task model;
        public string Desc
        {
            get => model.Desc;
        }
        public Priority Priority
        {
            get => model.Priority;
        }
        public DateTime CreateDate
        {
            get => model.CreateDate;
        }
        public DateTime MaxTermin
        {
            get => model.MaxDate;
        }
        public bool DoRealize
        {
            get => model.DoRealize;
        }
        public bool NotRealizeAfterMaxDate
        {
            get => DoRealize && (DateTime.Now > MaxTermin);
        }

        ICommand realizeTask;

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand RealizeTask
        {
            get
            {
                if (realizeTask == null)
                    realizeTask = new RelayCommand(
                        action =>
                        {
                            model.DoRealize = !model.DoRealize;
                            OnPropertyChanged("realize", "NotRealizeAfterMaxDate");
                        },
                        predicate =>
                        {
                            return !model.DoRealize;
                        });
                return realizeTask;
            }
        }

        #endregion

        #region constructors
        public TaskVM(Task model)
        {
            this.model = model;
        }
        public TaskVM(string desc, DateTime createDate, DateTime maxDate, Priority priority, bool doRealize)
        {
            model = new Task(desc, createDate, maxDate, priority, doRealize);
        }


        #endregion

        #region methods
        public Task GetModel() => model;

        public void OnPropertyChanged(params string[] propNames)
        {
            if (PropertyChanged != null)
            {
                foreach (string prop in propNames)
                    PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }

        #endregion

    }
}
