using System;
using System.Collections.Generic;
using TodoApp.Domain.Entities.Base;

namespace TodoApp.Domain.Entities
{
	public class Department : BaseEntity
	{
		public string Name { get; set; }
		public int EmployeeCount { get; set; }
		public double DepartmentFactor { get; set; }
		public DepartmentTypes DepartmentType { get; set; }
	}

	public enum DepartmentTypes
	{
		Sales,
		Marketing,
		Software,
		HumanResources,
	}


}
