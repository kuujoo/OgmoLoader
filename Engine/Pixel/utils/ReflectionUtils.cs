using System;
using System.Reflection;

namespace kuujoo.Pixel
{
    public static class ReflectionUtils
	{
		public static FieldInfo GetFieldInfo(Type type, string fieldName)
		{
			FieldInfo fieldInfo = null;
			do
			{
				fieldInfo = type.GetField(fieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				type = type.BaseType;
			} while (fieldInfo == null && type != null);
			return fieldInfo;
		}
	}
}