using System;

[Serializable]
public struct Range
{
	public float from;
	public float to;
	
	public Range(float fromValue, float toValue)
	{
		from = fromValue;
		to = toValue;
	}
}