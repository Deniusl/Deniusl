using Cysharp.Threading.Tasks;
using GameSystems.Scripts;
using UI;
using UnityEngine;
using Network =  GameSystems.Scripts.Network;

public class ApplyHealthNFTWebService : IApplyHealthNFTService
{
    public string TokenId { get; set; }
    public int Type { get; set; }
    public TxStatus TxStatus { get; set; }
    public IAddHealthService AddHealthService { get; set; }

    public async UniTask<bool> ApplyHealth()
    {
        TxStatus.gameObject.SetActive(true);
        string response = string.Empty;
        
#if UNITY_WEBGL && !UNITY_EDITOR
        response = await WebGLSendContractHealthApply.HealthApply(Contracts.MotoDEXnft, AddHealthService.HealthApplyMotoTokenId(), TokenId); 
#elif UNITY_EDITOR
       response = "success";
#endif
        bool status = await TxStatus.SetStatus(Chain.InUse, Network.InUse, response, TxStatusType.ADD_HEALTH); 
        Debug.Log("OnApplyHealthClick: " + response);

        return status;
    }
}