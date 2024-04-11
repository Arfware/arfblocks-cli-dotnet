namespace TodoApp.Application.RequestHandlers.Departments.Queries.All;
public class ResponseModel : IResponseModel<Array>
{
	public Guid Id { get; set; }
	public string Name { get; set; }
	public int EmployeeCount { get; set; }
	public double DepartmentFactor { get; set; }
	public DepartmentTypes DepartmentType { get; set; }
	public TodoTaskStatus TaskStatus { get; set; }

	public List<string> Aliases { get; set; }
}

public class RequestModel : IRequestModel
{
	public bool ListAll { get; set; }
	public bool IsDeleted { get; set; }
}