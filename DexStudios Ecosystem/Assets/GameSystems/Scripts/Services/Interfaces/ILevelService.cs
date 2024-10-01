namespace GameSystems.Scripts.Services.Interfaces
{
    public interface ILevelService
    {
        LevelPreset LevelPreset { get; set; }
        string LevelType { get; set; }
        string LevelTokenId { get; set; }
        bool IsTutorial { get; set; }

        void ResetValues();
    }
}