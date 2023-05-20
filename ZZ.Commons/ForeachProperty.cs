using System.Reflection;

namespace ZZ.Commons
{
	public static class ForeachProperty
	{
		public static TOut TransReflection<TIn, TOut>(TIn tIn)
		{
			// 获取对象实例
			TOut tOut = Activator.CreateInstance<TOut>();
			Type tInType = tIn.GetType();
			// 获取实例的属性集合
			foreach (PropertyInfo itemOut in tOut.GetType().GetProperties())
			{
				PropertyInfo? itemIn = tInType.GetProperty(itemOut.Name);
				if (itemIn != null)
				{
					itemOut.SetValue(tOut, itemIn.GetValue(tIn));
				}
			}
			return tOut;
		}
	}
}
