using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace WarMod.Modules;

[AttributeUsage(AttributeTargets.Class)]
public class RegisterModuleAttribute : Attribute
{
	public int Priority = -1;

	public static List<Type> FindModules()
	{
		return Assembly.GetExecutingAssembly()
			.GetTypes()
			.Where(t => t.IsSubclassOf(typeof(Module)) && t.GetCustomAttribute<RegisterModuleAttribute>() != null)
			.OrderBy(t => t.GetCustomAttribute<RegisterModuleAttribute>().Priority)
			.ToList();
	}
}
