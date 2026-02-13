using Exiled.API.Features;
using Exiled.CustomRoles.API;
using Exiled.CustomRoles.API.Features;
using Exiled.Events.EventArgs.Player;
using PlayerRoles;
using MEC;
using System;
using System.IO;

namespace Scp066
{
    public class Plugin : Plugin<Config>
    {
        public override string Name => "Scp066";
        public override string Author => "Matysiak / Reforged";
        public override Version Version => new Version(10, 3, 0);

        public static Plugin Instance;
        private Scp066Role _scp066Role;

        public override void OnEnabled()
        {
            Instance = this;
            _scp066Role = new Scp066Role();
            _scp066Role.Register();

            Exiled.Events.Handlers.Player.ChangingRole += OnChangingRole;
            LoadAudioFiles();
            base.OnEnabled();
        }

        private void OnChangingRole(ChangingRoleEventArgs ev)
        {
            if (ev.Player != null && ev.NewRole.GetTeam() == Team.SCPs && ev.NewRole != RoleTypeId.Scp0492 && ev.Reason == Exiled.API.Enums.SpawnReason.RoundStart)
            {
                if (UnityEngine.Random.Range(1, 101) <= Config.SpawnChance)
                {
                    Log.Info($"[SCP-066] Roll success! Replacing {ev.Player.Nickname} with SCP-066.");
                    Timing.CallDelayed(0.5f, () => _scp066Role.AddRole(ev.Player));
                }
            }
        }

        private void LoadAudioFiles()
        {
            string audioPath = Path.Combine(Paths.Configs, "Plugins", "scp066", "Audio");
            if (!Directory.Exists(audioPath)) { Directory.CreateDirectory(audioPath); return; }

            void Load(string fileName, string alias)
            {
                string path = Path.Combine(audioPath, fileName);
                if (File.Exists(path))
                {
                    AudioClipStorage.LoadClip(path, alias);
                    Log.Info($"[SCP-066] Audio loaded: {alias}");
                }
            }

            try
            {
                for (int i = 1; i <= 3; i++) Load($"Eric{i}.ogg", $"Eric{i}");
                for (int i = 1; i <= 6; i++) Load($"Notes{i}.ogg", $"Notes{i}");
                Load("Beethoven.ogg", "Beethoven");
            }
            catch (Exception e) { Log.Error($"[SCP-066] Audio Loading Error: {e}"); }
        }
    }
}