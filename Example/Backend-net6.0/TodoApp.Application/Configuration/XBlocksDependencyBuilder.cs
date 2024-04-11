using Microsoft.AspNetCore.Http;

namespace TodoApp.Application.Configuration
{
	public class ApplicationDependencyProvider : ArfBlocksDependencyProvider
	{
		public ApplicationDependencyProvider(IHttpContextAccessor httpContextAccessor, ProjectNameConfigurations projectConfigurations)
		{
			// Instances
			base.Add<EnvironmentConfiguration>(projectConfigurations.EnvironmentConfiguration);
			base.Add<IHttpContextAccessor>(httpContextAccessor);

			// Types
			base.Add<EnvironmentService, EnvironmentService>();
		}
	}
}
