using System;
using UnityEngine;


namespace PPValues
{
	public class PlayerPrefsString : PlayerPrefsValue<string>
	{
		public PlayerPrefsString(string key, string defaultValue, params Func<string, string>[] valuePreprocessors)
			: base(key, defaultValue, valuePreprocessors)
		{
		}


		protected override string GetValue(string key, string defaultValue)
			=> PlayerPrefs.GetString(key, defaultValue);

		protected override void SetValue(string key, string value)
			=> PlayerPrefs.SetString(key, value);
	}
}