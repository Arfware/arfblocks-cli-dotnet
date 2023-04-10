using System.Reflection;
using Arfware.ArfBlocks.Core.Models;
using Arfware.ArfBlocksCli.Commands.GenerateDocs.Models;

namespace Arfware.ArfBlocksCli.Commands.GenerateDocs;

internal class TypeExtractor
{
	//************************************************************************************\\
	// Type Extracting
	//************************************************************************************\\

	// public static Dictionary<Type, string> classTypeList = new Dictionary<Type, string>();
	public static Dictionary<Type, string> enumTypeList = new Dictionary<Type, string>();

	public static (string classAsTypescript, Dictionary<Type, string> types) CreateTypescriptClass(string objectName, string actionName, Type type, EndpointModelType modelType)
	{
		System.Console.WriteLine($"{objectName}-{actionName}");

		var tabAsString = GetTabsAsString(3);
		var modelTypeAsString = modelType == EndpointModelType.RequestModel ? "RequestModel" : "ResponseModel";
		var name = $"I{modelTypeAsString}";
		var responseString = $"{tabAsString}export interface {name}" + " {";

		var _classTypeList = new Dictionary<Type, string>();

		var properties = type.GetProperties().ToList();
		properties.ForEach((property) =>
		{
			var propertyName = char.ToLowerInvariant(property.Name[0]) + property.Name.Substring(1);
			var propertyType = property.PropertyType;

			if (Nullable.GetUnderlyingType(property.PropertyType) != null)
			{
				propertyName += "?";
				propertyType = Nullable.GetUnderlyingType(property.PropertyType);
			}

			var propertyTypeAsString = GetTypescriptPropertyType(propertyType, _classTypeList);
			responseString += $"\n{tabAsString}\t{propertyName}: {propertyTypeAsString};";
		});

		responseString += $"\n{tabAsString}" + "}";

		// Don't Add ResponseModel to Child List
		// _classTypeList.Add(type, responseString);

		return (responseString, _classTypeList);
		// return name;
	}

	private static string CreateInnerTypescriptClass(Type type, Dictionary<Type, string> _classTypeList)
	{
		var tabAsString = GetTabsAsString(3);
		var name = $"I{type.Name}";
		var responseString = $"{tabAsString}export interface {name}" + " {";

		var properties = type.GetProperties().ToList();
		properties.ForEach((property) =>
		{
			var propertyName = char.ToLowerInvariant(property.Name[0]) + property.Name.Substring(1);
			var propertyType = property.PropertyType;

			if (Nullable.GetUnderlyingType(property.PropertyType) != null)
			{
				propertyName += "?";
				propertyType = Nullable.GetUnderlyingType(property.PropertyType);
			}

			var propertyTypeAsString = GetTypescriptPropertyType(propertyType, _classTypeList);
			responseString += $"\n{tabAsString}\t{propertyName}: {propertyTypeAsString};";
		});

		responseString += $"\n{tabAsString}" + "}";


		_classTypeList[type] = responseString;

		return name;
	}

	private static string CreateEnumAsString(Type PropertyType)
	{
		var name = PropertyType.Name;
		var tabAsString = GetTabsAsString(2);
		var str = $"{tabAsString}export enum {name}" + " {";
		var enumNames = PropertyType.GetEnumNames().ToList();
		var enumUnderlyingType = PropertyType.GetEnumUnderlyingType();

		for (int i = 0; i < enumNames.Count; i++)
		{
			str += $"\n{tabAsString}\t{enumNames[i]}";

			if (enumUnderlyingType == typeof(System.Int32))
				str += $" = {i}";

			str += ",";
		}
		str += $"\n{tabAsString}" + "}";

		enumTypeList[PropertyType] = str;
		return name;
	}

	private static string GetTypescriptPropertyType(Type PropertyType, Dictionary<Type, string> _classTypeList)
	{
		var tabAsString = GetTabsAsString(0);

		if (PropertyType == typeof(string))
		{
			return "string";
		}
		else if (PropertyType == typeof(int))
		{
			return "number";
		}
		else if (PropertyType == typeof(float) || PropertyType == typeof(double))
		{
			return "Double";
		}
		else if (PropertyType == typeof(Guid))
		{
			return "Guid";
		}
		else if (PropertyType == typeof(DateTime))
		{
			return "Date";
		}
		else if (PropertyType == typeof(bool))
		{
			return "boolean";
		}
		else if (PropertyType.IsEnum)
		{
			CreateEnumAsString(PropertyType);

			return $"Enums.{PropertyType.Name}";
		}
		else if (PropertyType.IsGenericType && (PropertyType.GetGenericTypeDefinition() == typeof(List<>)))
		{
			var listType = PropertyType.GetGenericArguments().SingleOrDefault();
			if (listType == null)
			{
				System.Console.WriteLine(PropertyType.FullName);
				System.Console.WriteLine("ERROR: Property Could Not Handled");
				return "__ERROR_GENERIC_LIST_TYPE_NOT_HANDLED__";
			}

			return GetTypescriptPropertyType(listType, _classTypeList) + "[]";
		}
		else if (PropertyType.IsClass)
		{
			return CreateInnerTypescriptClass(PropertyType, _classTypeList);
		}
		else
		{
			return "__ERROR_TYPE_NOT_HANDLED__";
		}
	}

	private static string GetTabsAsString(int tabCount)
	{
		var tabsAsString = "";
		for (var i = 0; i < tabCount; i++)
			tabsAsString += "\t";

		return tabsAsString;
	}
}
