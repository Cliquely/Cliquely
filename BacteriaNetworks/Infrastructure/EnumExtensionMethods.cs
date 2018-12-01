using System;
using System.ComponentModel;
using System.Linq;

namespace BacteriaNetworks.Infrastructure
{
	public static class EnumExtensionMethods
	{
		public static string GetDescription<T>(this T e) where T : IConvertible
		{
			if (!(e is Enum)) return string.Empty;

			var type = e.GetType();
			var val = Enum.GetValues(type).Cast<T>().First(x => x.Equals(e));
			var memInfo = type.GetMember(type.GetEnumName(val));

			if (memInfo[0]
				.GetCustomAttributes(typeof(DescriptionAttribute), false)
				.FirstOrDefault() is DescriptionAttribute descriptionAttribute)
			{
				return descriptionAttribute.Description;
			}

			return string.Empty;
		}
	}
}
