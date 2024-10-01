namespace GameSystems.Scripts.Services.Interfaces
{
    public interface IPlayerService
    {
        NftItemAction GetNftItemAction();

        void SetNftItemAction(NftItemAction nftItemAction);
    }
}