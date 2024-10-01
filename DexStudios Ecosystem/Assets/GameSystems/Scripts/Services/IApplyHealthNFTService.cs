using Cysharp.Threading.Tasks;

public interface IApplyHealthNFTService
{
    string TokenId { get; set; }
    public int Type { get; set; }
    public TxStatus TxStatus { get; set; }
    public IAddHealthService AddHealthService { get; set; }
    UniTask<bool> ApplyHealth();
}