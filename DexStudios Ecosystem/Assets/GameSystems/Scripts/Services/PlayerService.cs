using GameSystems.Scripts.Providers;
using GameSystems.Scripts.Services.Interfaces;

namespace GameSystems.Scripts.Services
{
    public class PlayerService: IPlayerService
    {
        private NftItemAction _nftItemAction;
        
        public NftItemAction GetNftItemAction()
        {
            return _nftItemAction;
        }

        public void SetNftItemAction(NftItemAction nftItemAction)
        {
            _nftItemAction = nftItemAction;
            PlayerServiceProvider.NftItemAction = _nftItemAction;
        }
    }
}