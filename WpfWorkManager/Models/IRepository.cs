using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfWorkManager.Models
{
    public interface IRepository
    {
        IEnumerable<Employee> Employees { get; }
        IEnumerable<LoginModel> LoginModels { get; }
        IEnumerable<Issue> Issues { get; }
        IEnumerable<IssueCondition> IssueConditions { get; }
        IEnumerable<Command> Commands { get; }
        IEnumerable<Sprint> Sprints { get; }
    }

    public class Repository : IRepository
    {
        TaskContext taskContext;

        public Repository(TaskContext taskContext) => this.taskContext = taskContext;

        public IEnumerable<Employee> Employees => taskContext.Employees;
        public IEnumerable<LoginModel> LoginModels => taskContext.LoginModels;
        public IEnumerable<Issue> Issues => taskContext.Issues;
        public IEnumerable<IssueCondition> IssueConditions => taskContext.IssueConditions;
        public IEnumerable<Command> Commands => taskContext.Commands;
        public IEnumerable<Sprint> Sprints => taskContext.Sprints;
    }
}
