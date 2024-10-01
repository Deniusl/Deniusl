using System;
using UnityEngine;

namespace PPValues
{
	public class PlayerPrefsInt : PlayerPrefsValue<int>
	{
		public PlayerPrefsInt(string key, int defaultValue, params Func<int, int>[] valuePreprocessors)
			: base(key, defaultValue, valuePreprocessors)
		{
		}


		protected override int GetValue(string key, int defaultValue)
			=> PlayerPrefs.GetInt(key, defaultValue);

		protected override void SetValue(string key, int value)
			=> PlayerPrefs.SetInt(key, value);
	}
}