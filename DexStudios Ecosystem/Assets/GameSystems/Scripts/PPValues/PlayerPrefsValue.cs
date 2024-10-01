using System.Collections.Generic;
using PPValues;


namespace PPValues
{
	public abstract class PlayerPrefsValue<T>
	{
		private static readonly Dictionary<string, PlayerPrefsValue<T>> _cache = new ();
		
		private readonly string _key;
		private readonly T _defaultValue;
		private readonly System.Func<T, T>[] _valuePreprocessors;

		private readonly PlayerPrefsValue<T> _cachedInstance;

		private bool _valueIsCached;
		private T _value;

		public T Value
		{
			get
			{
				if (_cachedInstance != null)
					return _cachedInstance.Value;
					
				if (!_valueIsCached)
				{
					_value = GetValue(_key, _defaultValue);
					_valueIsCached = true;
				}
				_value = GetValue(_key, _defaultValue);
				_valueIsCached = true;

				return _value;
			}
			set
			{
				if (_cachedInstance != null)
				{
					_cachedInstance.Value = value;
					return;
				}
				
				_value = GetProcessedValue(value);
				SetValue(_key, _value);
				_valueIsCached = true;
			}
		}

		
		protected PlayerPrefsValue(string key, T defaultValue = default, params System.Func<T, T>[] valuePreprocessors)
		{
			_key = key;
			_defaultValue = defaultValue;
			_valuePreprocessors = valuePreprocessors;

			if (_cache.ContainsKey(key) && _cache[key] != null)
				_cachedInstance = _cache[key];
			else
				_cache[key] = this;
		}

		protected abstract T GetValue(string key, T defaultValue);

		protected abstract void SetValue(string key, T value);
		
		public static implicit operator T(PlayerPrefsValue<T> playerPrefsValue)
			=> playerPrefsValue.Value;

		private T GetProcessedValue(T value)
		{
			if (_valuePreprocessors != null)
			{
				foreach (var valuePreprocessor in _valuePreprocessors)
					value = valuePreprocessor(value);
			}

			return value;
		}
	}
}