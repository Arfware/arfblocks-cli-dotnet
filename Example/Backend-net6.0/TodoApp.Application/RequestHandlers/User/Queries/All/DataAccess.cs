namespace TodoApp.Application.RequestHandlers.Users.Queries.All;

public class DataAccess : IDataAccess
{

	public DataAccess(ArfBlocksDependencyProvider depencyProvider)
	{
	}

	public async Task<List<User>> GetAllUsers()
	{
		await Task.CompletedTask;

		return new List<User>()
			{
				new User()
				{
					Id= Guid.Parse("cf4892e8-f0bf-4a0d-b860-e30fb8ae4509"),
					Email="john@company.com",
					FirstName="John",
					LastName="Doe",
					Department = new Department()
					{
						Id=Guid.Parse("85fa1b1b-4092-419c-b602-4e69b96ce243"),
						Name="Sales",
					}
				}
			};
	}
}