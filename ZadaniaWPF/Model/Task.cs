using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZadaniaWPF.Model
{
    class Task
    {
        public int Id { get; private set; }
        public string Desc { get; private set; }
        public DateTime CreateDate { get; private set; }
        public DateTime MaxDate { get; private set; }
        public Priority Priority { get; private set; }
        public bool DoRealize { get; set; }

        public Task(int id, string desc, DateTime createDate, DateTime maxDate, Priority priority, bool doRealize)
        {
            Id = id;
            Desc = desc;
            CreateDate = createDate;
            MaxDate = maxDate;
            Priority = priority;
            DoRealize = doRealize;
        }

        public override string ToString()
        {
            return $"{Desc} , " +
                $"priorytet: {Priority}, " +
                $"Data Utworzenia: {CreateDate}, " +
                $"Termin wykonania: {MaxDate}, " +
                $"Status: {(DoRealize ? "Nie wykonane" : "Wykonane")}";
        }

        public static string PriorityDescription(Priority priorytet)
        {
            switch (priorytet)
            {
                case Priority.MniejWażne:
                    return "mniej ważne";
                case Priority.Ważne:
                    return "ważne";
                case Priority.Krytyczne:
                    return "krytyczne";
                default:
                    throw new Exception("Nierozpoznany priorytet zadania");
            }
        }

        public static Priority DescPriorityParser(string descPriority)
        {
            switch (descPriority)
            {
                case "mniej ważne":
                    return Priority.MniejWażne;
                case "ważne":
                    return Priority.Ważne;
                case "krytyczne":
                    return Priority.Krytyczne;
                default:
                    throw new Exception("Nierozpoznany opis priorytetu zadania");
            }
        }
    }
}
