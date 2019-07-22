using System;
using System.Collections.Generic;
using System.Text;

namespace RestFuncApp
{
    public class Task
    {
        public string Id { get; set; } = Guid.NewGuid().ToString("n");
        public DateTime CreatedTime { get; set; } = DateTime.UtcNow;
        public string TaskDescription { get; set; }
        public bool IsCompleted { get; set; }
    }


    public class TaskCreateModel
    {
        public string TaskDescription { get; set; }
    }



    public class TaskUpdateModel
    {
        public string TaskDescription { get; set; }
        public bool IsCompleted { get; set; }
    }
}
