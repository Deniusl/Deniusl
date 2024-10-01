namespace PPValues
{
	public class PlayerPrefsBool : PlayerPrefsValue<bool>
	{
		public PlayerPrefsBool(string key, bool defaultValue)
			: base(key, defaultValue)
		{
		}
		

		protected override bool GetValue(string key, bool defaultValue)
			=> PlayerPrefsExtensions.GetBool(key, defaultValue);

		protected override void SetValue(string key, bool value)
			=> PlayerPrefsExtensions.SetBool(key, value);
	}
}