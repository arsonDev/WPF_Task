using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using ZadaniaWPF.Model;

namespace ZadaniaWPF.ViewModel
{
    class TasksVM
    {
        #region props_&_var
        private const string xmlPath = "zadania.xml";

        private AllTasks allTasks;
        public ObservableCollection<TaskVM> TasksList { get; } = new ObservableCollection<TaskVM>();

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
