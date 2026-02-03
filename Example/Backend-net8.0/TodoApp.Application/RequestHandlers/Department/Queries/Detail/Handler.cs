namespace TodoApp.Application.RequestHandlers.Departments.Queries.Detail;

public class Handler : IRequestHandler
{
	private readonly DataAccess _dataAccessLayer;
	public Handler(ArfBlocksDependencyProvider dependencyProvider, object dataAccess)
	{
		_dataAccessLayer = (DataAccess)dataAccess;
	}

	public async Task<ArfBlocksRequestResult> Handle(IRequestModel payload, EndpointContext context, CancellationToken cancellationToken)
	{
		// Get All Departments from DB
		var allDepartments = await _dataAccessLayer.GetAllDepartments();

		// Build and Return Response
		var response = new Mapper().MapToResponse(allDepartments);
		return ArfBlocksResults.Success(response);
	}
}
