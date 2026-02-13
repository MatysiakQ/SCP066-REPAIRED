using Exiled.API.Interfaces;
using System.ComponentModel;

namespace Scp066
{
    public class Config : IConfig
    {
        [Description("Whether or not the plugin is enabled.")]
        public bool IsEnabled { get; set; } = true;

        [Description("Whether or not debug messages should be shown in the console.")]
        public bool Debug { get; set; } = false;

        [Description("The name of the schematic to be spawned (MapEditorReborn).")]
        public string SchematicName { get; set; } = "Scp066";

        [Description("Chance (1-100) for a regular SCP to be replaced by SCP-066 at the start of the round.")]
        public int SpawnChance { get; set; } = 10;

        [Description("Volume of SCP-066 sounds (0.0 to 1.0).")]
        public float Volume { get; set; } = 0.8f;

        [Description("Duration of the Beethoven symphony attack in seconds.")]
        public int AttackDuration { get; set; } = 24;

        [Description("Damage dealt per second during the symphony.")]
        public float Damage { get; set; } = 5f;

        [Description("Radius of the attack and audio audibility.")]
        public float AttackRadius { get; set; } = 8f;

        [Description("Cooldowns for abilities in seconds.")]
        public float CooldownEric { get; set; } = 10f;
        public float CooldownMusic { get; set; } = 15f;
        public float CooldownAttack { get; set; } = 45f;
    }
}