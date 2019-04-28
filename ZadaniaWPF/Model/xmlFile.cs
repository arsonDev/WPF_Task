using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ZadaniaWPF.Model
{
    static class xmlFile
    {
        public static void Save(string path, AllTasks tasks)
        {
            try
            {
                XDocument xml = new XDocument(
                    new XDeclaration("1.0", "utf-8", "yes"),
                    new XComment($"Saving Date: {DateTime.Now}"),
                    new XElement("Tasks",
                        from Task task in tasks
                        select new XElement("Task",
                            new XElement("Dest", task.Desc),
                            new XElement("CreateDate", task.CreateDate),
                            new XElement("Termin", task.MaxDate),
                            new XElement("Priority", (byte)task.Priority),
                            new XElement("Realize", task.DoRealize))
                    ));
                xml.Save(path);
            }
            catch(Exception e)
            {
                throw new Exception("Problem z zapisem do pliku",e);
            }
        }

        public static AllTasks Load(string path)
        {
            try
            {
                XDocument xml = XDocument.Load(path);
                IEnumerable<Task> data =
                    from task in xml.Root.Descendants("Task")
                    select new Task(
                        task.Element("Desc").Value,
                        DateTime.Parse(task.Element("CreateDate").Value),
                        DateTime.Parse(task.Element("Termin").Value),
                        (Priority)byte.Parse(task.Element("Priority").Value),
                        bool.Parse(task.Element("Realize").Value)
                        );
                AllTasks tasks = new AllTasks();
                foreach (Task task in data) tasks.AddTask(task);
                return tasks;
            }catch(Exception e)
            {
                throw new Exception("Problem z odczytem z pliku", e);
            }
        }
    }
}
