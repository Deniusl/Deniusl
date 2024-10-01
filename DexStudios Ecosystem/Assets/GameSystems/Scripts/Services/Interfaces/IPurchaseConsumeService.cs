#nullable enable
using Cysharp.Threading.Tasks;
using UnityEngine.Purchasing.Security;

namespace GameSystems.Scripts.Services.Interfaces
{
    public interface IPurchaseConsumeService
    {
        UniTask ConsumePurchase(BuyFlow.Receipt receipt, GooglePurchaseState? googlePurchaseState, ServerRequest serverRequest,
            string url);
    }
}