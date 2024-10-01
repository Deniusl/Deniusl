using UnityEngine;

namespace PPValues
{
	public static class PlayerPrefsExtensions
	{
		public static bool GetBool(string key, bool defaultValue)
		{
			return ToBool(PlayerPrefs.GetInt(key, ToInt(defaultValue)));
		}
		
		public static void SetBool(string key, bool value)
		{
			PlayerPrefs.SetInt(key, ToInt(value));
		}
		
		private static int ToInt(bool value) => value ? 1 : 0;
		private static bool ToBool(int value) => value == 1;
	}
}