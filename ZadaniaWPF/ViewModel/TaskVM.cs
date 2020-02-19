using System;
using System.ComponentModel;
using System.Windows.Input;

using ZadaniaWPF.Model;

namespace ZadaniaWPF.ViewModel
{
    class TaskVM : INotifyPropertyChanged
    {
        #region props_var
        private Task model;

        public int Id
        {
            get => model.Id;
        }
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
            get => !DoRealize && (DateTime.Now >= MaxTermin);
        }


        private ICommand realizeTask;

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
                            OnPropertyChanged("DoRealize", "NotRealizeAfterMaxDate");
                            EditInDB(model);
                        },
                        predicate =>
                        {
                            return !model.DoRealize;
                        });
                return realizeTask;
            }
        }

        private void EditInDB(Task model)
        {
            using (Database.TasksWpfEntities1 db = new Database.TasksWpfEntities1())
            {
                var task = db.Tasks.Find(model.Id);
                task.tsk_isRealized = model.DoRealize;

                db.TasksHistories.Add(
                    new Database.TasksHistory()
                    {
                        tsh_EventDate = DateTime.Now,
                        tsh_TaskId = task.tsk_id,
                        tsh_EventType = db.DictTaskEventTypes
                            .Find(task.tsk_isRealized ? 2 : 1)
                            .tev_id
                    });

                db.SaveChanges();
            }
        }
        private void SaveInDb(TaskVM task)
        {
            using (Database.TasksWpfEntities1 db = new Database.TasksWpfEntities1())
            {
                db.Tasks.Add(new Database.Task()
                {
                    tsk_name = task.Desc,
                    tsk_priority = (int)task.Priority,
                    tsk_maxDate = task.MaxTermin,
                    tsk_isRealized = task.DoRealize
                });
                db.SaveChanges();
            }
        }

        #endregion

        #region constructors
        public TaskVM(Task model)
        {
            this.model = model;
        }

        public TaskVM(int id, string desc, DateTime createDate, DateTime maxDate, Priority priority, bool doRealize)
        {
            model = new Task(id, desc, createDate, maxDate, priority, doRealize);
            
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
