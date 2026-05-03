using Exiled.API.Features;
using Exiled.CustomRoles.API;
using Exiled.CustomRoles.API.Features;
using PlayerRoles;
using MEC;
using System.Linq;
using UnityEngine;
using UserSettings.ServerSpecific;

namespace Scp066
{
    public class Plugin : Plugin<Config>
    {
        public static Plugin Instance;
        private Scp066Role _scp066Role;

        private SSKeybindSetting _beethovenKeybind;
        private SSKeybindSetting _ericKeybind;
        private SSKeybindSetting _musicKeybind;

        public override void OnEnabled()
        {
            Instance = this;
            _scp066Role = new Scp066Role();
            _scp066Role.Register();

            // Ustawiamy twarde ID (6601, 6602, 6603) żeby klient gry się nie dławił
            _beethovenKeybind = new SSKeybindSetting(6601, "Atak: Beethoven", KeyCode.None, hint: "Atak obszarowy");
            _ericKeybind = new SSKeybindSetting(6602, "Dźwięk: Eric", KeyCode.None, hint: "Odtwarza 'Eric'");
            _musicKeybind = new SSKeybindSetting(6603, "Dźwięk: Notes", KeyCode.None, hint: "Losowe nuty");

            var settings = new ServerSpecificSettingBase[]
            {
                new SSGroupHeader("SCP-066"),
                _beethovenKeybind,
                _ericKeybind,
                _musicKeybind
            };

            if (ServerSpecificSettingsSync.DefinedSettings == null)
                ServerSpecificSettingsSync.DefinedSettings = settings;
            else
                ServerSpecificSettingsSync.DefinedSettings = ServerSpecificSettingsSync.DefinedSettings.Concat(settings).ToArray();

            ServerSpecificSettingsSync.SendToAll();
            ServerSpecificSettingsSync.ServerOnSettingValueReceived += ProcessUserInput;

            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStarted;
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStarted;
            ServerSpecificSettingsSync.ServerOnSettingValueReceived -= ProcessUserInput;

            CustomRole.UnregisterRoles();
            Instance = null;
            base.OnDisabled();
        }

        private void ProcessUserInput(ReferenceHub sender, ServerSpecificSettingBase setting)
        {
            if (sender == null || setting == null) return;

            if (setting is SSKeybindSetting kb && kb.SyncIsPressed)
            {
                if (sender.gameObject.TryGetComponent<Scp066AbilityComponent>(out var scp066))
                {
                    if (setting.SettingId == _beethovenKeybind.SettingId) scp066.TriggerBeethoven();
                    else if (setting.SettingId == _ericKeybind.SettingId) scp066.PlayEric();
                    else if (setting.SettingId == _musicKeybind.SettingId) scp066.PlayMusic();
                }
            }
        }

        private void OnRoundStarted()
        {
            Timing.CallDelayed(3.0f, () =>
            {
                if (UnityEngine.Random.Range(1, 101) > Config.SpawnChance) return;
                if (!CustomRole.TryGet(660, out CustomRole scp066Role)) return;

                var scpPool = Player.List.Where(p => p.Role.Team == Team.SCPs
                                                  && p.Role.Type != RoleTypeId.Scp079
                                                  && !CustomRole.Registered.Any(r => r.Check(p))).ToList();

                if (scpPool.Count == 0) return;

                Player selected = scpPool[Random.Range(0, scpPool.Count)];
                scp066Role.AddRole(selected);

                Timing.CallDelayed(0.8f, () =>
                {
                    if (selected == null || !selected.IsAlive) return;

                    var room = Room.List.FirstOrDefault(r => r.Type == Exiled.API.Enums.RoomType.HczArmory);
                    if (room == null)
                        room = Room.List.Where(r => r.Zone == Exiled.API.Enums.ZoneType.HeavyContainment).OrderBy(_ => UnityEngine.Random.value).FirstOrDefault();

                    if (room != null) selected.Teleport(room.Position + Vector3.up * 2.5f);
                });
            });
        }
    }
}