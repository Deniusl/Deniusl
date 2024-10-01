using System;
using BuyFlow;
using Cysharp.Threading.Tasks;
using GameSystems.Scripts.Services.Interfaces;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Purchasing.Security;

namespace GameSystems.Scripts.Services
{
	public class ConsumePurchaseService : IPurchaseConsumeService
	{
		public async UniTask ConsumePurchase(
			Receipt receipt, 
			GooglePurchaseState? googlePurchaseState, 
			ServerRequest serverRequest, 
			string url)
		{
			var consumePurchaseData = new ConsumePurchaseData
			{
				idfa = receipt.idfa,
				productIds = receipt.productIds,
				orderId = receipt.orderId,
				receiptData = new ConsumeReceiptData
				{
					orderId = receipt.orderId,
					packageName = "app.motodex.android",
					productId = receipt.productIds,
					purchaseTime = ConvertToUnixTimestampMilliseconds(receipt.receiptData.purchaseTime),
					purchaseState = googlePurchaseState ?? default,
					purchaseToken = receipt.receiptData.purchaseToken,
					acknowledged = false
				}
			};

			Debug.Log($"Consume - sendData: {JsonConvert.SerializeObject(consumePurchaseData)}");

			await serverRequest.Post(url, consumePurchaseData, attempts: 5);
		}

		private static long ConvertToUnixTimestampMilliseconds(DateTime dateTime)
		{
			return new DateTimeOffset(dateTime).ToUnixTimeMilliseconds();
		}
	}
}