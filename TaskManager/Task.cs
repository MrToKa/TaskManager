using System.Collections.Generic;

namespace TaskManager
{
    public class Task
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Assignee { get; set; }

        public int Priority { get; set; }

        public HashSet<string> Dependencies { get; set; }

        public override string ToString()
        {
            return this.Title;
        }
    }
}