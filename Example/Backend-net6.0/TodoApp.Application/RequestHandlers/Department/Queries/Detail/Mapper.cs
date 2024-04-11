namespace TodoApp.Application.RequestHandlers.Departments.Queries.Detail;

public class Mapper
{
	public ResponseModel MapToResponse(Department department)
	{
		return new ResponseModel()
		{
			Id = department.Id,
			Name = department.Name,
			DepartmentFactor = department.DepartmentFactor,
			DepartmentType = DepartmentTypes.Marketing,
			EmployeeCount = department.EmployeeCount,
			Aliases = new List<string>() { "Sales", "Marketting", "SEO" }
		};
	}
}