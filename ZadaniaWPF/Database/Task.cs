//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ZadaniaWPF.Database
{
    using System;
    using System.Collections.Generic;
    
    public partial class Task
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Task()
        {
            this.TasksHistories = new HashSet<TasksHistory>();
        }
    
        public int tsk_id { get; set; }
        public string tsk_name { get; set; }
        public int tsk_priority { get; set; }
        public System.DateTime tsk_maxDate { get; set; }
        public Nullable<System.DateTime> tsk_CreateDate { get; set; }
        public bool tsk_isRealized { get; set; }
    
        public virtual TaskPriority TaskPriority { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TasksHistory> TasksHistories { get; set; }
    }
}
