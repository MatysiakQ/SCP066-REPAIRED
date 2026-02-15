using Exiled.API.Features;
using Exiled.CustomRoles.API;
using Exiled.CustomRoles.API.Features;
using PlayerRoles;
using MEC;
using System.Linq;
using UnityEngine;

namespace Scp066
{
    public class Plugin : Plugin<Config>
    {
        public static Plugin Instance;
        private Scp066Role _scp066Role;

        public override void OnEnabled()
        {
            Instance = this;
            _scp066Role = new Scp066Role();
            _scp066Role.Register();

            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStarted;
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStarted;
            CustomRole.UnregisterRoles();
            Instance = null;
            base.OnDisabled();
        }

        private void OnRoundStarted()
        {
            // Czekamy 3s aż gra rozda role, ale przed SCP-153 (który czeka 5s)
            Timing.CallDelayed(3.0f, () =>
            {
                // Jedna kość na rundę - 10% szansy że w ogóle się pojawi
                if (UnityEngine.Random.Range(1, 101) > Config.SpawnChance) return;

                if (!CustomRole.TryGet(660, out CustomRole scp066Role)) return;

                // Pula: SCPy, bez 079, bez graczy którzy już mają CustomRole
                var scpPool = Player.List.Where(p => p.Role.Team == Team.SCPs
                                                  && p.Role.Type != RoleTypeId.Scp079
                                                  && !CustomRole.Registered.Any(r => r.Check(p))).ToList();

                if (scpPool.Count == 0)
                {
                    Log.Warn("[SCP-066] Brak dostepnych SCP do przypisania roli.");
                    return;
                }

                Player selected = scpPool[Random.Range(0, scpPool.Count)];
                scp066Role.AddRole(selected);

                // Teleport do HCZ
                Timing.CallDelayed(0.8f, () =>
                {
                    var room = Room.List.FirstOrDefault(r => r.Type == Exiled.API.Enums.RoomType.Lcz173);
                    if (room != null) selected.Teleport(room.Position + Vector3.up * 2.5f);
                });

                Log.Info($"[SCP-066] Wylosowano gracza {selected.Nickname} z puli SCP.");
            });
        }
    }
}














































