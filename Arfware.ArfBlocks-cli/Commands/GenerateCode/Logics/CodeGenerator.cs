using Arfware.ArfBlocksCli.Commands.GenerateDocs.Models;

namespace Arfware.ArfBlocksCli.Commands.GenerateCode.Logics;

internal class CodeGenerator
{
	private readonly EndpointDefinitionModel _endpointDefinition;
	private readonly string _outputPath;

	private readonly string pureObjectName;
	private readonly string pluralObjectName;
	private readonly string variableObjectName;

	public CodeGenerator(EndpointDefinitionModel endpointDefinition, string outputPath)
	{
		_endpointDefinition = endpointDefinition;
		_outputPath = outputPath;

		pureObjectName = _endpointDefinition.ObjectName;
		pluralObjectName = TemplateManipulator.GetPluralName(endpointDefinition.ObjectName);
		variableObjectName = TemplateManipulator.GetVariableName(endpointDefinition.ObjectName);
	}

	public async Task<bool> Generate(char[] actions)
	{
		var result = false;
		foreach (var action in actions)
		{
			switch (action)
			{
				case 'c':
					var actionName = "Create";
					var outputDirectory = this.CreateOutputDirectory(this.pureObjectName, actionName, EndpointTypes.Command);
					result = await this.GenerateTemplates(EndpointTypes.Command, actionName, outputDirectory, new List<TemplateTypes>()
					{
						TemplateTypes.DataAccess,
						TemplateTypes.Handler,
						TemplateTypes.Mapper,
						TemplateTypes.Models,
						TemplateTypes.PostHandler,
						TemplateTypes.Validator,
						TemplateTypes.Verificator,
						TemplateTypes.Test,
					});
					break;

				default:
					throw new Exception($"Action Not Found with '{action}' Key");
			}
		}

		return result;
	}

	private async Task<bool> GenerateTemplates(EndpointTypes endpointType, string actionName, string outputDirectory, List<TemplateTypes> templateTypes)
	{
		var pluralEndpointType = this.GetEndpointTypeAsString(endpointType, true);
		var result = false;
		foreach (var templateType in templateTypes)
		{
			var rawTemplate = await this.GetRawTemplate(actionName, templateType);
			var template = "";

			switch (templateType)
			{
				case TemplateTypes.Test:
					template = this.Generate_Test(rawTemplate, actionName, pluralEndpointType);
					break;

				case TemplateTypes.DataAccess:
					template = this.Generate_DataAccess(rawTemplate, actionName, pluralEndpointType);
					break;
				case TemplateTypes.Handler:
					template = this.Generate_Handler(rawTemplate, actionName, pluralEndpointType);
					break;

				case TemplateTypes.Mapper:
					template = this.Generate_Mapper(rawTemplate, actionName, pluralEndpointType);
					break;

				case TemplateTypes.Models:
					template = this.Generate_Models(rawTemplate, actionName, pluralEndpointType);
					break;

				case TemplateTypes.PostHandler:
					template = this.Generate_PostHandler(rawTemplate, actionName, pluralEndpointType);
					break;

				case TemplateTypes.PreHandler:
					throw new Exception($"Template Type Not Handled: {templateType}");

				case TemplateTypes.Validator:
					template = this.Generate_Validator(rawTemplate, actionName, pluralEndpointType);
					break;

				case TemplateTypes.Verificator:
					template = this.Generate_Verificator(rawTemplate, actionName, pluralEndpointType);
					break;

				default:
					throw new Exception($"Template Type Not Handled: {templateType}");
			}

			result = await this.CreateFile(outputDirectory, templateType, endpointType, template);
			if (!result)
				break;
		}

		return result;
	}

	private async Task<string> GetRawTemplate(string actionName, TemplateTypes templateType)
	{
		var fileName = this.GetTemplateTypeAsString(templateType);

		if (templateType == TemplateTypes.Test)
			fileName = "Tests/" + fileName;

		return await FileOperator.Read($"Commands/GenerateCode/Templates/Handlers/{actionName}/{fileName}.xtemplate");
	}

	private string CreateOutputDirectory(string objectName, string actionName, EndpointTypes endpointType)
	{
		var pluralObjectName = TemplateManipulator.GetPluralName(objectName);
		var endpointTypeAsString = this.GetEndpointTypeAsString(endpointType, true);

		var outputDirectory = Path.Combine(_outputPath, pluralObjectName, endpointTypeAsString, actionName);
		var testOutputDirectory = Path.Combine(outputDirectory, "Tests");

		if (!Directory.Exists(outputDirectory))
			Directory.CreateDirectory(outputDirectory);
		else
			throw new Exception($"Directory Already Exist: {outputDirectory}");

		Directory.CreateDirectory(testOutputDirectory);

		return outputDirectory;
	}

	private async Task<bool> CreateFile(string outputDirectory, TemplateTypes templateType, EndpointTypes endpointType, string content)
	{
		var templateTypeAsString = this.GetTemplateTypeAsString(templateType);
		outputDirectory = templateType == TemplateTypes.Test ? Path.Combine(outputDirectory, "Tests") : outputDirectory;

		var path = Path.Combine(outputDirectory, $"{templateTypeAsString}.cs");

		return await FileOperator.Write(path, content);
	}

	private string GetEndpointTypeAsString(EndpointTypes endpointType, bool isPlural)
	{
		var typeAsString = "";

		switch (endpointType)
		{
			case EndpointTypes.Command:
				typeAsString = "Command";
				break;

			case EndpointTypes.Query:
				typeAsString = "Query";
				break;

			default:
				throw new Exception($"Endpoint Type Not Handled: {endpointType}");
		}

		return isPlural ? TemplateManipulator.GetPluralName(typeAsString) : typeAsString;
	}


	private string GetTemplateTypeAsString(TemplateTypes templateType)
	{
		switch (templateType)
		{
			case TemplateTypes.DataAccess:
				return "DataAccess";
			case TemplateTypes.Handler:
				return "Handler";
			case TemplateTypes.Mapper:
				return "Mapper";
			case TemplateTypes.Models:
				return "Models";
			case TemplateTypes.PostHandler:
				return "PostHandler";
			case TemplateTypes.PreHandler:
				return "PreHandler";
			case TemplateTypes.Validator:
				return "Validator";
			case TemplateTypes.Verificator:
				return "Verificator";
			case TemplateTypes.Test:
				return "Success";

			default:
				throw new Exception($"Template Type Not Handled: {templateType}");
		}
	}

	private string Generate_Models(string template, string actionName, string pluralEndpointType)
	{
		var requestModelProperties = new List<PropertyDefinitionModel>();
		var responseModelProperties = new List<PropertyDefinitionModel>();

		switch (actionName)
		{
			case "Create":
				requestModelProperties = _endpointDefinition.EntityProperties.Where(p => !p.IsKey && p.IsPrimitive).ToList();
				responseModelProperties = _endpointDefinition.EntityProperties.Where(p => p.IsKey).ToList();
				break;

			default:
				break;
		}

		template = TemplateManipulator.FillNamespace(template, pluralObjectName, actionName, pluralEndpointType);
		template = TemplateManipulator.ReplaceObjectNames(template, pureObjectName, pluralObjectName, variableObjectName);

		template = TemplateManipulator.FillRepeatableTemplate(template, "requestModelProperties", requestModelProperties);
		template = TemplateManipulator.FillRepeatableTemplate(template, "responseModelProperties", responseModelProperties);

		return template;
	}

	private string Generate_Mapper(string template, string actionName, string pluralEndpointType)
	{
		var requestModelProperties = new List<PropertyDefinitionModel>();
		var responseModelProperties = new List<PropertyDefinitionModel>();

		switch (actionName)
		{
			case "Create":
				requestModelProperties = _endpointDefinition.EntityProperties.Where(p => !p.IsKey && p.IsPrimitive).ToList();
				responseModelProperties = _endpointDefinition.EntityProperties.Where(p => p.IsKey).ToList();
				break;

			default:
				break;
		}

		template = TemplateManipulator.FillNamespace(template, pluralObjectName, actionName, pluralEndpointType);
		template = TemplateManipulator.ReplaceObjectNames(template, pureObjectName, pluralObjectName, variableObjectName);

		template = TemplateManipulator.FillRepeatableTemplate(template, "requestModelProperties", requestModelProperties);
		template = TemplateManipulator.FillRepeatableTemplate(template, "responseModelProperties", responseModelProperties);

		return template;
	}

	private string Generate_DataAccess(string template, string actionName, string pluralEndpointType)
	{
		template = TemplateManipulator.FillNamespace(template, pluralObjectName, actionName, pluralEndpointType);

		template = TemplateManipulator.ReplaceObjectNames(template, pureObjectName, pluralObjectName, variableObjectName);

		return template;
	}

	private string Generate_Handler(string template, string actionName, string pluralEndpointType)
	{
		template = TemplateManipulator.FillNamespace(template, pluralObjectName, actionName, pluralEndpointType);

		template = TemplateManipulator.ReplaceObjectNames(template, pureObjectName, pluralObjectName, variableObjectName);

		return template;
	}

	private string Generate_Validator(string template, string actionName, string pluralEndpointType)
	{
		template = TemplateManipulator.FillNamespace(template, pluralObjectName, actionName, pluralEndpointType);

		template = TemplateManipulator.ReplaceObjectNames(template, pureObjectName, pluralObjectName, variableObjectName);

		// Build Request Model Validator
		var requestModelProperties = _endpointDefinition.EntityProperties.Where(p => p.IsPrimitive && (p.PropertyTypeAsString == "string")).ToList();
		template = TemplateManipulator.FillRepeatableTemplate(template, "requestModelProperties", requestModelProperties);

		return template;
	}

	private string Generate_Verificator(string template, string actionName, string pluralEndpointType)
	{
		template = TemplateManipulator.FillNamespace(template, pluralObjectName, actionName, pluralEndpointType);

		template = TemplateManipulator.ReplaceObjectNames(template, pureObjectName, pluralObjectName, variableObjectName);

		return template;
	}

	private string Generate_PostHandler(string template, string actionName, string pluralEndpointType)
	{
		template = TemplateManipulator.FillNamespace(template, pluralObjectName, actionName, pluralEndpointType);

		template = TemplateManipulator.ReplaceObjectNames(template, pureObjectName, pluralObjectName, variableObjectName);

		return template;
	}

	private string Generate_Test(string template, string actionName, string pluralEndpointType)
	{
		template = TemplateManipulator.FillNamespace(template, pluralObjectName, actionName, pluralEndpointType);
		template = TemplateManipulator.ReplaceObjectNames(template, pureObjectName, pluralObjectName, variableObjectName);

		// Build Inner Templates
		var requestModelProperties = _endpointDefinition.EntityProperties.Where(p => !p.IsKey && p.IsPrimitive && (p.PropertyTypeAsString == "string")).ToList();
		template = TemplateManipulator.FillRepeatableTemplate(template, "requestModelProperties", requestModelProperties);
		template = TemplateManipulator.FillRepeatableTemplate(template, "requestModelProperties", requestModelProperties);

		return template;
	}
}
