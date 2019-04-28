using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZadaniaWPF.Model
{
    class AllTasks : IEnumerable<Task>
    {
        private List<Task> listTasks = new List<Task>();
        public void AddTask(Task task) => listTasks.Add(task);

        public void RemoveTask(Task task) => listTasks.Remove(task);

        public IEnumerator<Task> GetEnumerator() => listTasks.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => (IEnumerator)this.GetEnumerator();

        public int TaskCount
        {
            get => listTasks.Count;
        }

        public Task this[int index]
        {
            get => listTasks[index];
        }

        

    }
}
