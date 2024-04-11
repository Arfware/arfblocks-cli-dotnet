using System;
using Arfware.ArfBlocks.Core;
using Arfware.ArfBlocks.Core.Abstractions;
using Microsoft.AspNetCore.Http;
using TodoApp.Infrastructure.Services;

namespace TodoApp.Application.Configuration
{

	public class ProjectNameConfigurations : IConfigurationClass
	{
		public EnvironmentConfiguration EnvironmentConfiguration { get; set; }
	}
}
