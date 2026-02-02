namespace TodoApp.Application.RequestHandlers.Departments.Queries.All;

public class DataAccess : IDataAccess
{
	public DataAccess(ArfBlocksDependencyProvider depencyProvider)
	{
	}

	public async Task<List<Department>> GetAllDepartments()
	{
		await Task.CompletedTask;

		return new List<Department>(){
				new Department(){
					Id=Guid.Parse("85fa1b1b-4092-419c-b602-4e69b96ce243"),
					Name="Sales",
					// DepartmentType = DepartmentTypes.Sales,
					DepartmentFactor = 3.2,
					EmployeeCount = 18,
					CreatedAt = DateTime.Now,
				},
				new Department()
				{
					Id=Guid.Parse("5f62fd07-d4cd-4761-a4be-bc4fa2104123"),
					Name="Marketing",
					// DepartmentType = DepartmentTypes.Marketing,
					DepartmentFactor = 2.7,
					EmployeeCount = 17,
					CreatedAt = DateTime.Now,
				}
			};
	}
}
