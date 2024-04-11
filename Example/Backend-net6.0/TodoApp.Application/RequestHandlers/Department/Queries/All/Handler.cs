namespace TodoApp.Application.RequestHandlers.Departments.Queries.All;

public class Handler : IRequestHandler
{
	private readonly DataAccess _dataAccessLayer;
	public Handler(ArfBlocksDependencyProvider dependencyProvider, object dataAccess)
	{
		_dataAccessLayer = (DataAccess)dataAccess;
	}

	public async Task<ArfBlocksRequestResult> Handle(IRequestModel payload, CancellationToken cancellationToken)
	{
		// Get All Departments from DB
		var allDepartments = await _dataAccessLayer.GetAllDepartments();

		// Build and Return Response
		var mappedTasks = new Mapper().Map(allDepartments);
		return ArfBlocksResults.Success(mappedTasks);
	}
}
