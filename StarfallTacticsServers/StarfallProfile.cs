using StarfallTactics.StarfallTacticsServers.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace StarfallTactics.StarfallTacticsServers
{
    public partial class StarfallProfile
    {
        [JsonPropertyName("nickname")]
        public string Nickname { get; set; } = "NewPlayer";

        [JsonPropertyName("temporarypass")]
        public string TemporaryPass { get; set; } = "a0b1c2d3e4f5g6h7i8j9k10l";

        [JsonPropertyName("character_mode_profile")]
        public CharacterModeProfile CharacterModeProfile { get; set; } = new CharacterModeProfile();

        [JsonPropertyName("ranked_mode_profile")]
        public RankedModeProfile RankedModeProfile { get; set; } = new RankedModeProfile();

        [JsonIgnore]
        public Character CurrentCharacter { get; set; } = null;

        [JsonIgnore]
        public StarfallDatabase Database { get; set; } = null;

        [JsonIgnore]
        public int MatchmakerId { get; set; } = -1;

        [JsonIgnore]
        public string MatchmakerAuth { get; set; } = string.Empty;

        [JsonIgnore]
        public int IndexSpace => MatchmakerId < 0 ? 0 : MatchmakerId * 5000;


        public event EventHandler<EventArgs> Edited;

        [JsonIgnore]
        protected object Locker { get; } = new object();

        public void SelectCharacter(Character character)
        {
            List<Character> chars = CharacterModeProfile?.Chars;

            if (chars?.Contains(character) == true)
                CurrentCharacter = character;
        }

        public void Use(Action<UsageHandler> action)
        {
            lock (Locker)
            {
                UsageHandler handler = new UsageHandler(this);
                action.Invoke(handler);

                if (handler.Edited == true)
                    OnEdited(EventArgs.Empty);
            }
        }

        protected virtual void OnEdited(EventArgs args)
        {
            Edited?.Invoke(this, args);
        }

        public class UsageHandler
        {
            public StarfallProfile Profile { get; }

            public bool Edited
            {
                get => edited;
                set
                {
                    if (value == true && value != edited)
                        edited = value;
                }
            }

            private bool edited = false;

            public UsageHandler(StarfallProfile profile)
            {
                Profile = profile;
            }
        }
    }
}
