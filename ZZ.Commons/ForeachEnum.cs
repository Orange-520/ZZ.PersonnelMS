namespace ZZ.Commons
{
	public static class ForeachEnum
	{
		/// <summary>
		/// 判断int值在枚举类型中是否存在
		/// </summary>
		/// <typeparam name="TEnum">枚举类型</typeparam>
		/// <param name="value"></param>
		/// <returns>存在返回枚举类型，不存在返回枚举类型中的 None</returns>
		/// <exception cref="ArgumentException"></exception>
		public static TEnum GetEnumFromInt<TEnum>(int value) where TEnum : struct, Enum
		{
			if (!typeof(TEnum).IsEnum)
			{
				throw new ArgumentException("TEnum must be an enumerated type");
			}

			foreach (TEnum enumValue in Enum.GetValues(typeof(TEnum)))
			{
				if (Convert.ToInt32(enumValue) == value)
				{
					return enumValue;
				}
			}

			return (TEnum)Enum.Parse(typeof(TEnum), "None");
		}
	}
}
