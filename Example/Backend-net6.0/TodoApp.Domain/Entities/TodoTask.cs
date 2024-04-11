using System;
using TodoApp.Domain.Entities.Base;

namespace TodoApp.Domain.Entities
{
	public class TodoTask : BaseEntity
	{
		public string Title { get; set; }
		public string Description { get; set; }
		public TodoTaskStatus Status { get; set; }
		public DateTime StatusChangedAt { get; set; }

		// Department Relation
		public Guid AssignedDepartmentId { get; set; }
		public Department AssignedDepartment { get; set; }

		// Creator User Relation
		public Guid CreatedById { get; set; }
		public User CreatedBy { get; set; }
	}

	public enum TodoTaskStatus
	{
		Pending,
		Completed,
		Rejected,
	}
}
