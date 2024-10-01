using GameSystems.Scripts.Services.Interfaces;

namespace GameSystems.Scripts.Services
{
    public class LevelService: ILevelService
    {
        public LevelPreset LevelPreset { get; set; }
        public string LevelType { get; set; }
        public string LevelTokenId { get; set; }
        public bool IsTutorial { get; set; }

        public void ResetValues()
        {
            LevelPreset = default;
            LevelType = default;
            LevelTokenId = default;
            IsTutorial = default;
        }
    }
}