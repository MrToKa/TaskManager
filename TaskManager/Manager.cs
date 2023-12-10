using System;
using System.Collections.Generic;

namespace TaskManager
{
    public class Manager : IManager
    {
        public Dictionary<string, Task> tasks = new Dictionary<string, Task>();
        public Dictionary<string, List<string>> dependencies = new Dictionary<string, List<string>>();
        
        public void AddDependency(string taskId, string dependentTaskId)
        {
            if (tasks.ContainsKey(taskId) && tasks.ContainsKey(dependentTaskId))
            {
                if (dependencies.ContainsKey(taskId))
                {
                    dependencies[taskId].Add(dependentTaskId);
                }
                else
                {
                    dependencies.Add(taskId, new List<string>() { dependentTaskId });
                }
            }
            else
            {
                throw new ArgumentException();
            }
        }

        public void AddTask(Task task)
        {
            if (tasks.ContainsKey(task.Id))
            {
                throw new ArgumentException();
            }
            tasks.Add(task.Id, task);
        }

        public bool Contains(string taskId)
        {
            return tasks.ContainsKey(taskId);
        }

        public Task Get(string taskId)
        {
            if (tasks.ContainsKey(taskId))
            {
                return tasks[taskId];
            }
            throw new ArgumentException();
        }

        public IEnumerable<Task> GetDependencies(string taskId)
        {
            var result = new List<Task>();
            var stack = new Stack<string>();
            stack.Push(taskId);

            while (stack.Count > 0)
            {
                var currentTaskId = stack.Pop();
                if (dependencies.TryGetValue(currentTaskId, out var dependentTaskIds))
                {
                    foreach (var dependentTaskId in dependentTaskIds)
                    {
                        if (tasks.TryGetValue(dependentTaskId, out var task))
                        {
                            result.Add(task);
                            stack.Push(dependentTaskId);
                        }
                    }
                }
            }

            return result;
        }

        private List<Task> GetDependencies(string taskId, List<Task> tasks)
        {
            if (dependencies.ContainsKey(taskId))
            {
                foreach (var item in dependencies[taskId])
                {
                    if (this.tasks.ContainsKey(item))
                    {
                        tasks.Add(this.tasks[item]);
                        GetDependencies(item, tasks);
                    }
                }
            }
            return tasks;
        }

        public IEnumerable<Task> GetDependents(string taskId)
        {
            var result = new List<Task>();

            GetDependents(taskId, result);

            return result;
        }

        private List<Task> GetDependents(string taskId, List<Task> result)
        {
            foreach (var item in dependencies)
            {
                if (item.Value.Contains(taskId))
                {
                    result.Add(tasks[item.Key]);
                    GetDependents(item.Key, result);
                }
            }
            return result;
        }

        public void RemoveDependency(string taskId, string dependentTaskId)
        {
            if (tasks.ContainsKey(taskId) && tasks.ContainsKey(dependentTaskId))
            {
                if (dependencies.ContainsKey(taskId))
                {
                    dependencies[taskId].Remove(dependentTaskId);
                }
                else
                {
                    throw new ArgumentException();
                }
            }
            else
            {
                throw new ArgumentException();
            }
        }

        public void RemoveTask(string taskId)
        {
            if (!tasks.ContainsKey(taskId))
            {
                throw new ArgumentException();
            }
            tasks.Remove(taskId);
            dependencies.Remove(taskId);

            foreach (var item in dependencies)
            {
                if (item.Value.Contains(taskId))
                {
                    item.Value.Remove(taskId);
                }
            }
        }

        public int Size()
        {
            return tasks.Count;
        }
    }
}