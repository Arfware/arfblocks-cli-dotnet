namespace TodoApp.Application.RequestHandlers.Users.Queries.All;

// [InternalHandler]
// [AuthorizedHandler]
[AllowAnonymousHandler]
public class Handler : IRequestHandler
{
	private readonly DataAccess _dataAccessLayer;
	public Handler(ArfBlocksDependencyProvider dependencyProvider, object dataAccess)
	{
		_dataAccessLayer = (DataAccess)dataAccess;
	}

	public async Task<ArfBlocksRequestResult> Handle(IRequestModel payload, CancellationToken cancellationToken)
	{
		// Get All Users from DB
		var allUsers = await _dataAccessLayer.GetAllUsers();

		// Build and Return Response
		var mappedTasks = new Mapper().Map(allUsers);
		return ArfBlocksResults.Success(mappedTasks);
	}
}