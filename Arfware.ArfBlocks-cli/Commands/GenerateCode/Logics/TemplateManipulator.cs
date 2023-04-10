using Arfware.ArfBlocksCli.Commands.GenerateDocs.Models;

namespace Arfware.ArfBlocksCli.Commands.GenerateCode.Logics;

internal class TemplateManipulator
{
	public static string FillNamespace(string template, string pluralObjectName, string actionName, string pluralEndpointType)
	{
		template = TemplateManipulator.ReplaceTemplateObject(template, "PluralObjectName", pluralObjectName);
		template = TemplateManipulator.ReplaceTemplateObject(template, "PluralEndpointType", pluralEndpointType);
		template = TemplateManipulator.ReplaceTemplateObject(template, "ActionName", actionName);

		return template;
	}

	public static string ReplaceObjectNames(string template, string pureObjectName, string pluralObjectName, string variableObjectName)
	{
		template = ReplaceTemplateObject(template, "PureObjectName", pureObjectName);
		template = ReplaceTemplateObject(template, "PluralObjectName", pluralObjectName);
		template = ReplaceTemplateObject(template, "VariableObjectName", variableObjectName);

		return template;
	}

	public static string ReplaceTemplateObject(string template, string keyword, string value)
	{
		var templateKeyword = "{{" + keyword + "}}";
		return template.Replace(templateKeyword, value);
	}

	// ================================================================================ \\
	// ============================= Inner Template Utils ============================= \\
	// ================================================================================ \\

	public static string FillRepeatableTemplate(string template, string repeatableTemplateName, List<PropertyDefinitionModel> properties)
	{
		var (innerTemplate, innerTemplateStartIndex, innerTemplateEndIndex) = GetRepeatableInnerTemplate(template, repeatableTemplateName);
		var replacedInnerTemplate = "";
		foreach (var property in properties)
		{
			var tempTemplate = ReplaceTemplateObject(innerTemplate, "PropertyType", property.PropertyTypeAsString);
			tempTemplate = ReplaceTemplateObject(tempTemplate, "PropertyName", property.Name);
			replacedInnerTemplate += tempTemplate;
		}
		template = ReplaceRepeatableTemplate(template, replacedInnerTemplate, repeatableTemplateName);

		return template;
	}

	private static string ReplaceRepeatableTemplate(string template, string replacedInnerTemplate, string repeatableTemplateName)
	{
		(var innerTemplateStartExpression, var innerTemplateEndExpression) = GetRepeatableStatements(StatementTypes.Loop, repeatableTemplateName);

		var startIndex = template.IndexOf(innerTemplateStartExpression);
		var endIndex = template.IndexOf(innerTemplateEndExpression) + innerTemplateEndExpression.Length;

		var beforeInnerTemplate = template.Substring(0, startIndex);
		var afterInnerTemplate = template.Substring(endIndex, template.Length - endIndex);
		return beforeInnerTemplate + replacedInnerTemplate + afterInnerTemplate;
	}

	private static (string InnerTemplate, int startIndex, int endIndex) GetRepeatableInnerTemplate(string template, string repeatableTemplateName)
	{
		(var innerTemplateStartExpression, var innerTemplateEndExpression) = GetRepeatableStatements(StatementTypes.Loop, repeatableTemplateName);

		var startIndex = template.IndexOf(innerTemplateStartExpression) + innerTemplateStartExpression.Length;
		var endIndex = template.IndexOf(innerTemplateEndExpression);

		var innerTemplate = template.Substring(startIndex, endIndex - startIndex);
		return (innerTemplate, startIndex, endIndex);
	}

	// ================================================================================ \\
	// ============================= Template Utils =================================== \\
	// ================================================================================ \\

	public static (string StartStatement, string EndStatement) GetRepeatableStatements(StatementTypes statementType, string name)
	{
		var innerTemplateStartExpression = "";
		var innerTemplateEndExpression = "";

		switch (statementType)
		{
			case StatementTypes.Loop:
				innerTemplateStartExpression = "#repeatable-start-" + name + "#";
				innerTemplateEndExpression = "#repeatable-end-" + name + "#";
				break;

			default:
				throw new Exception("Statement Type Not Handled");
		}

		return (innerTemplateStartExpression, innerTemplateEndExpression);
	}

	public static string GetPluralName(string name)
	{
		if (name.EndsWith("ry"))
			return name.Substring(0, name.Length - 3) + "ies";

		return name + "s";
	}

	public static string GetVariableName(string name)
	{
		return name.ToLowerInvariant();
	}

}