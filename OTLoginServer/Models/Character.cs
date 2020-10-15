using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace OTLoginServer.Models
{
    public class Character
    {
        public enum VocationEnum
        {
            None,
            Sorcerer,
            Druid,
            Paladin,
            Knight,
            MasterSorcerer,
            ElderDruid,
            RoyalPaladin,
            EliteKnight,
        }

        public int WorldId { get; set; } = 0;
        public string Name { get; set; }
        public int Level { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public VocationEnum Vocation { get; set; }

        public bool IsMale { get; set; }
        public bool IsHidden { get; set; } = false;
        public bool IsMainCharacter { get; set; } = false;
        public bool Tutorial { get; set; }
        public int OutfitId { get; set; }
        public int HeadColor { get; set; }
        public int TorsoColor { get; set; }
        public int LegsColor { get; set; }
        public int DetailColor { get; set; }
        public int AddonsFlags { get; set; }
        public bool IsTournamentParticipant { get; set; } = false;
    }
}