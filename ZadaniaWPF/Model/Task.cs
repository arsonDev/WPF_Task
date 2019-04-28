using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZadaniaWPF.Model
{
    class Task
    {
        public string Desc { get; private set; }
        public DateTime CreateDate { get; private set; }
        public DateTime MaxDate { get; private set; }
        public Priority Priority { get; private set; }
        public bool DoRealize { get; set; }

        public Task(string desc, DateTime createDate, DateTime maxDate, Priority priority, bool doRealize)
        {
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
    }
}
