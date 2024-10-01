using System;
using Cysharp.Threading.Tasks;
using Services;
using UnityEngine;

namespace GameSystems.Scripts.Services
{
    public class UDIDService: IGetUniqueIDService
    {
        private const string UDID_KEY = "UDID";
        
        public string UniqueID => _cashedUDID.Value;
        
        private LocalCashValue<string> _cashedUDID;
        
	    //private string _localDeviceIDFA = "5F919114-D24C-444A-BBDD-D1AF09261A51";
        //private string _localDeviceIDFA = "449D50EB-95A3-43C4-AFB4-D092ECFDEFB7";
        private string _localDeviceIDFA = "37d5832832df5450c3b64da28af9ea2e";

        public UDIDService()
        {
            _cashedUDID = new LocalCashValue<string>(UDID_KEY);
        }
        
        public async UniTask<string> GetUniqueID()
        {
            await UniTask.Delay(1000);

#if UNITY_EDITOR
            _cashedUDID.SetValue(_localDeviceIDFA);
#elif UNITY_ANDROID
            var deviceID = SystemInfo.deviceUniqueIdentifier;
            Debug.Log($"ID - {deviceID}");
            _cashedUDID.SetValue(deviceID);
#endif
            return _cashedUDID.Value;
        }
    }
}