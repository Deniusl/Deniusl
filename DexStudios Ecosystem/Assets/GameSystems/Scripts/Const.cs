using NearClientUnity;
using System;
using static System.Net.WebRequestMethods;

namespace GameSystems.Scripts
{
	public static class Const
	{
		public static class Health
		{
			public static double oneHpInWei => HP.oneHealthInWei;
		}
		
		public static class NearHealth
		{
			public static readonly double nearHpDiff = Math.Pow(10, 6);
		}
		
		public static class ConcordiumHealth
		{
			public static readonly double concordiumHpDiff = Math.Pow(10, 10);
		}
		
		public static class Wallet
		{
			public static readonly double VARAToHuman = Math.Pow(10, 12);
			public static readonly double BNBToHuman = Math.Pow(10, 18);
			public static readonly double NEARToHuman = Math.Pow(10, 24);
			public static readonly double CCDToHuman = Math.Pow(10, 6);
			public static readonly double AptosToHuman = Math.Pow(10, 8);
		}

		public static class Account
		{
			public const string Zero = "0x0000000000000000000000000000000000000000";
			public const string Empty = "";
        }
        public static class URL
        {
			public static readonly string MOTODEX_SHARE_URL = "https://motodex.go.link";
			public static readonly string MOTODEX_APP_URL = "https://app.motodex.dexstudios.games";
        }
	}
}