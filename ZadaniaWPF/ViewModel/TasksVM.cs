using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Input;

using ZadaniaWPF.Model;

namespace ZadaniaWPF.ViewModel
{
    class TasksVM
    {
        #region props_&_var

        private AllTasks allTasks;
        private ICommand delete;
        private ICommand add;

        public ObservableCollection<TaskVM> TasksList { get; } = new ObservableCollection<TaskVM>();

        public ICommand DeleteTask
        {
            get
            {
                if (delete == null)
                {
                    delete = new RelayCommand(
                        action =>
                        {
                            TaskVM task = TasksList[(int)action];
                            if (!task.DoRealize)
                            {
                                MessageBoxResult mbr = MessageBox.Show("Czy chcesz usunąć niezrealizowane zadanie?", "ZadaniaWPF",
                                    MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
                                if (mbr == MessageBoxResult.No) return;
                            }

                            DeleteTaskFromDb(task);

                        },
                        pred =>
                        {
                            if (pred == null) return false;
                            int index = (int)pred;
                            return index >= 0;
                        });

                }
                return delete;
            }
        }

        public ICommand AddTask
        {
            get
            {
                if (add == null)
                {
                    add = new RelayCommand(
                        action =>
                        {
                            TaskVM task = action as TaskVM;
                            if (task != null)
                            {
                                SaveInDb(task);
                            }
                        },
                        pred =>
                        {
                            return true;
                        });
                }
                return add;
            }
        }

        #endregion

        #region construct
        public TasksVM()
        {
            
        }
        #endregion

        #region methods
        private void LoadTasks()
        {
            using (Database.TasksWpfEntities1 db = new Database.TasksWpfEntities1())
            {
                allTasks = new AllTasks();
                foreach (var dbTask in db.Tasks)
                {
                    allTasks.AddTask(
                        new Task(
                            dbTask.tsk_id,
                            dbTask.tsk_name,
                            dbTask.tsk_CreateDate ?? DateTime.Now,
                            dbTask.tsk_maxDate,
                            (Priority)dbTask.tsk_priority,
                            dbTask.tsk_isRealized));
                }

            }

            CopyTasks();
        }

        private void CopyTasks()
        {
            TasksList.CollectionChanged -= ModelSync;
            TasksList.Clear();
            foreach (Task task in allTasks)
                TasksList.Add(new TaskVM(task));
            TasksList.CollectionChanged += ModelSync;
        }

        private void ModelSync(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    {
                        TaskVM task = (TaskVM)e.NewItems[0];
                        if (task != null)
                            allTasks.AddTask(task.GetModel());
                        break;
                    }
                case NotifyCollectionChangedAction.Remove:
                    {
                        TaskVM task = (TaskVM)e.OldItems[0];
                        if (task != null)
                            allTasks.RemoveTask(task.GetModel());
                        break;
                    }
            }
        }

        private void DeleteTaskFromDb(TaskVM task)
        {
            using (Database.TasksWpfEntities1 db = new Database.TasksWpfEntities1())
            {
                db.TaskDelete(task.Id);
            }

            TasksList.Remove(task);
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
                    tsk_isRealized = task.DoRealize,
                    tsk_CreateDate = DateTime.Now
                });
                db.SaveChanges();
            }
        }

        #endregion
    }
}
