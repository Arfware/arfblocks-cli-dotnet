namespace TodoApp.Application.RequestHandlers.Departments.Queries.All;
public class Mapper
{
	public List<ResponseModel> Map(List<Department> departments)
	{
		var mappedDepartments = new List<ResponseModel>();

		departments?.ForEach((department) =>
		{
			mappedDepartments.Add(new ResponseModel()
			{
				Id = department.Id,
				Name = department.Name,
				DepartmentFactor = department.DepartmentFactor,
				DepartmentType = DepartmentTypes.Marketing,
				EmployeeCount = department.EmployeeCount,
				Aliases = new List<string>() { "Sales", "Marketting", "SEO" }
			});
		});

		return mappedDepartments;
	}
}