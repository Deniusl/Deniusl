namespace GameSystems.Scripts
{
    public enum Types
    {
        // Moto:
        RED_BULLER = 0,
        ZEBRA_GRRR = 1,
        ROBO_HORSE = 2,
        METAL_EYES = 3,
        BROWN_KILLER = 4,
        CRAZY_LINE = 5,
        MAGIC_BOX = 6,
        
        // Pills:
        HEALTH_PILL_15 = 7,
        HEALTH_PILL_35 = 8,
        HEALTH_PILL_75 = 9,
        HEALTH_PILL_125 = 10,

        // Tracks:
        TRACK_LONDON = 100,
        TRACK_DUBAI = 101,
        TRACK_ABU_DHABI = 102,
        TRACK_BEIJIN = 103,
        TRACK_TOKYO = 104,
        TRACK_MELBURN = 105,
        TRACK_NYC = 106,
        TRACK_TAIPEI = 107,
        TRACK_PISA = 108,
        TRACK_ISLAMABAD = 109,
        
        ADD_BALANCE = 1000,
    }
    
    public static class TypesExtensions
    {
        public static bool IsMoto(this Types type) => type is >= Types.RED_BULLER and <= Types.CRAZY_LINE;
        
        public static bool IsPill(this Types type) => type is >= Types.HEALTH_PILL_15 and <= Types.HEALTH_PILL_125;
        
        public static bool IsTrack(this Types type) => type is >= Types.TRACK_LONDON and <= Types.TRACK_ISLAMABAD;
    }

}