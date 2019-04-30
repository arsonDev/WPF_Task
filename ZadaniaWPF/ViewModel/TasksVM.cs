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
        private const string xmlPath = "zadania.xml";

        private AllTasks allTasks;
        public ObservableCollection<TaskVM> TasksList { get; } = new ObservableCollection<TaskVM>();

        public ICommand save;

        public ICommand SaveOnClose
        {
            get
            {
                if (save == null)
                    save = new RelayCommand(
                        action =>
                        {
                            Model.xmlFile.Save(xmlPath, allTasks);
                        },
                        predicate =>
                        { return true; });
                return save;
            }
        }


        public ICommand delete;

        public ICommand DeleteTask
        {
            get
            {
                if (delete != null)
                {
                    delete = new RelayCommand(
                        action =>
                        {
                            int index = (int)action;
                            TaskVM task = TasksList[index];
                            if (!task.DoRealize)
                            {
                                MessageBoxResult mbr = MessageBox.Show("Czy chcesz usunąć niezrealizowane zadanie?", "ZadaniaWPF",
                                    MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
                                if (mbr == MessageBoxResult.No) return;
                            }
                            TasksList.Remove(task);
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

        public ICommand add;

        public ICommand C_AddTask
        {
            get
            {
                if (add != null)
                {
                    add = new RelayCommand(action =>
                    {
                        TaskVM task = action as TaskVM;
                        if (task != null)
                            TasksList.Add(task);
                    },
                    pred =>
                    {
                        return (pred as TaskVM) != null;
                    });
                }
                return add;
            }
        }

        #endregion

        #region construct
        public TasksVM()
        {
            if (System.IO.File.Exists(xmlPath))
                allTasks = xmlFile.Load(xmlPath);
            else
                allTasks = new AllTasks();

            // for tests
            allTasks.AddTask(new Model.Task("test1", DateTime.Now.AddDays(-1), DateTime.Now.AddDays(-1), Priority.MniejWażne, false));
            allTasks.AddTask(new Model.Task("test2", DateTime.Now, DateTime.Now.AddDays(9), Priority.Krytyczne, false));
            allTasks.AddTask(new Model.Task("test3", DateTime.Now, DateTime.Now.AddDays(2), Priority.Ważne, true));
            allTasks.AddTask(new Model.Task("test4", DateTime.Now, DateTime.Now.AddDays(3), Priority.MniejWażne, false));
            allTasks.AddTask(new Model.Task("test5", DateTime.Now, DateTime.Now.AddDays(4), Priority.Krytyczne, true));
            allTasks.AddTask(new Model.Task("test6", DateTime.Now, DateTime.Now.AddDays(5), Priority.MniejWażne, false));

            CopyTasks();
        }
        #endregion

        #region methods
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



        #endregion
    }
}
