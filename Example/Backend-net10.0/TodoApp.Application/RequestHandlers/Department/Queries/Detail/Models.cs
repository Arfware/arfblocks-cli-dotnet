namespace TodoApp.Application.RequestHandlers.Departments.Queries.Detail;

public class ResponseModel : IResponseModel
{
	public Guid Id { get; set; }
	public string Name { get; set; }
	public int EmployeeCount { get; set; }
	public double DepartmentFactor { get; set; }
	public DepartmentTypes DepartmentType { get; set; }
	public TodoTaskStatus TaskStatus { get; set; }

	public Dictionary<Guid, string> Values { get; set; }

	[SkipDocumentation]
	public List<byte> Bytes { get; set; }

	public List<string> Aliases { get; set; }
}

public class RequestModel : IRequestModel
{
	public Guid Id { get; set; }
}
