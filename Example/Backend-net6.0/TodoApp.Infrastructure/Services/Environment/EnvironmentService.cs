using System;

namespace TodoApp.Infrastructure.Services
{
	public class EnvironmentService
	{
		public string EnvironmentName { get; }
		public CustomEnvironments Environment { get; }
		public string ApiUrl { get; }
		public string UiUrl { get; }

		public EnvironmentService(EnvironmentConfiguration configuration)
		{
			switch (configuration.EnvironmentName)
			{
				case "Production":
					this.Environment = CustomEnvironments.Production;
					this.ApiUrl = "https://api.TodoApp.com";
					this.UiUrl = "https://TodoApp.com";
					break;
				case "Staging":
					this.Environment = CustomEnvironments.Staging;
					this.ApiUrl = "https://api-test.TodoApp.com";
					this.UiUrl = "https://test.TodoApp.com";
					break;
				case "Development":
					this.Environment = CustomEnvironments.Development;
					this.ApiUrl = "http://localhost:5000";
					this.UiUrl = "http://localhost:3000";
					break;
			}

			this.EnvironmentName = configuration.EnvironmentName;
		}
	}
}
