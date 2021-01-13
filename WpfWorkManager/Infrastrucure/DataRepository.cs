using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfWorkManager.Models;
using WpfWorkManager.Models.DataViewModels;

namespace WpfWorkManager.Infrastrucure
{
    public class DataRepository
    {
        IRepository repository;

        public DataRepository(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task<List<SimpleUser>> GetSimpleUserDatasListAsync(int id)
        {
            using (var context = new TaskContext())
            {
                if (context.Issues.Count() > 0)
                {
                    return await Task.Run(() =>
                    {
                        return (from sprint in context.Sprints
                                join issue in context.Issues on sprint.Id equals issue.SprintId
                                join issueCond in context.IssueConditions on issue.Id equals issueCond.IssueId into issueCondTemp
                                from IssueCond in issueCondTemp.DefaultIfEmpty()

                                where issue.EmployeeId == id && issue.IsDeleted == false

                                select new SimpleUser()
                                {
                                    UserId = issue.EmployeeId,
                                    UserIssue = issue.TaskName,
                                    ChangeDate = IssueCond.IssueReportChangeDate,
                                    EndDate = IssueCond.IssueEndDate,
                                    IssueChange = IssueCond.IssueReportChange,
                                    IssueComent = IssueCond.IssueComment,
                                    IssueReport = IssueCond.IssueReport,
                                    StartDate = IssueCond.IssueStartDate,
                                    IssueState = IssueCond.IssueState,
                                    Comment = issue.Comment,
                                    Description = issue.Description
                                }).ToList();

                    });
                }
            }
            return null;
        }

        public async Task<List<AllEmployee>> GetAllEmployeiesAsync()
        {
            var result = await Task.Run(() =>
            {
                return (from emp in repository.Employees.Where(x => x.IsRetired == false) 
                        select new AllEmployee()
                        {
                            Name = emp.Name,
                            Role = emp.Role == 0 ? "TeamLeader" : (emp.Role == 1 ? "Руководитель" : "Сотрудник"), 
                            IntRole = emp.Role, 
                            IsRetired = emp.IsRetired,
                            LeadeID = emp.LeaderId,
                            Password = emp.Password,
                            Id = emp.Id
                        }).ToList();
            });

            List<AllEmployee> list = new List<AllEmployee>();
            var temp = result.OrderBy(x => x.LeadeID);
            foreach (var t in temp)
            {
                if (t.LeadeID == 1 || t.LeadeID == null)
                    list.Add(t);

                foreach (var r in temp.Where(x => x.LeadeID == t.Id && x.LeadeID != 1))
                    list.Add(r);
            }

            return list;
        }

        public async Task<List<Leaders>> GetLeadersAsync()
        {
            var result = await Task.Run(() =>
            {
                return (from emp in repository.Employees.Where(x => x.IsRetired == false)
                        where emp.Role == 1
                        select new Leaders()
                        {
                            Id = emp.Id,
                            Name = emp.Name
                        }).ToList();
            });
            return result;
        }
    }
}
