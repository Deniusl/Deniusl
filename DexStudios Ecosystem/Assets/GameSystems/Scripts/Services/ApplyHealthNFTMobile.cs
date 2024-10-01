using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GameSystems.Scripts.Services
{
    public class ApplyHealthNFTMobile: IApplyHealthNFTService
    {
        public string TokenId { get; set; }
        
        public int Type { get; set; }
        
        public TxStatus TxStatus { get; set; }
        
        public IAddHealthService AddHealthService { get; set; }

        public async UniTask<bool> ApplyHealth()
        {
            Debug.Log($"#debug #motoTokenId - {AddHealthService.HealthApplyMotoTokenId()} #pillId - {TokenId}");
            
            var response = await RequestSystem.Instance.GetApplyHealth(AddHealthService.HealthApplyMotoTokenId(), TokenId);
            
            Debug.Log($"#debug #response - {response}");

            response = "success";
            
            bool status = await TxStatus.SetStatus(Chain.InUse, Network.InUse, response, TxStatusType.ADD_HEALTH); 

            return true;
        }
    }
}