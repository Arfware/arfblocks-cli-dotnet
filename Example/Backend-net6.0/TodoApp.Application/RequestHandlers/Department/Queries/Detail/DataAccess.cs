namespace TodoApp.Application.RequestHandlers.Departments.Queries.Detail;

public class DataAccess : IDataAccess
{
	public DataAccess(ArfBlocksDependencyProvider depencyProvider)
	{ }

	public async Task<Department> GetAllDepartments()
	{
		await Task.CompletedTask;

		return new Department()
		{
			Id = Guid.Parse("85fa1b1b-4092-419c-b602-4e69b96ce243"),
			Name = "Sales",
			// DepartmentType = DepartmentTypes.Sales,
			DepartmentFactor = 3.2,
			EmployeeCount = 18,
			CreatedAt = DateTime.Now,
		};
	}
}
