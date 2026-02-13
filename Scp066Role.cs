using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.CustomRoles.API.Features;
using MEC;
using Mirror;
using PlayerRoles;
using ProjectMER.Features;
using ProjectMER.Features.Objects;
using System.Collections.Generic;
using UnityEngine;

namespace Scp066
{
    public class Scp066Role : CustomRole
    {
        public override uint Id { get; set; } = 660;
        public override string Name { get; set; } = "SCP-066";
        public override RoleTypeId Role { get; set; } = RoleTypeId.Scp0492;
        public override int MaxHealth { get; set; } = 3000;
        public override float SpawnChance { get; set; } = 0f;
        public override string CustomInfo { get; set; } = "SCP-066";
        public override string Description { get; set; } = "A mobile mass of braided yarn and eyes.";

        private readonly Dictionary<Player, SchematicObject> _models = new Dictionary<Player, SchematicObject>();

        protected override void RoleAdded(Player player)
        {
            if (player == null) return;

            player.MaxHealth = MaxHealth;
            player.Health = MaxHealth;
            player.EnableEffect(EffectType.Invisible);

            if (!player.GameObject.TryGetComponent(out Scp066AbilityComponent comp))
                player.GameObject.AddComponent<Scp066AbilityComponent>();

            Timing.CallDelayed(0.5f, () =>
            {
                if (player == null || !Check(player)) return;

                try
                {
                    // FIXED: SchematicName now exists in Config
                    var schematic = ObjectSpawner.SpawnSchematic(Plugin.Instance.Config.SchematicName, player.Position);
                    if (schematic != null)
                    {
                        _models[player] = schematic;
                        schematic.transform.parent = player.GameObject.transform;
                        schematic.transform.localPosition = new Vector3(0, -0.5f, 0);

                        NetworkIdentity id = schematic.gameObject.GetComponent<NetworkIdentity>();
                        if (id != null && !player.IsNPC)
                        {
                            player.Connection.Send(new ObjectDestroyMessage { netId = id.netId });
                        }
                    }
                }
                catch (System.Exception e) { Log.Error($"[SCP-066] Schematic Spawn Error: {e}"); }

                Timing.RunCoroutine(HideBodyLoop(player));
            });

            base.RoleAdded(player);
        }

        private IEnumerator<float> HideBodyLoop(Player player)
        {
            for (int i = 0; i < 5; i++)
            {
                if (player == null) yield break;
                foreach (var renderer in player.GameObject.GetComponentsInChildren<SkinnedMeshRenderer>())
                {
                    renderer.enabled = false;
                }
                yield return Timing.WaitForSeconds(0.5f);
            }
        }

        protected override void RoleRemoved(Player player)
        {
            player.DisableEffect(EffectType.Invisible);
            if (_models.TryGetValue(player, out SchematicObject s))
            {
                s?.Destroy();
                _models.Remove(player);
            }
            base.RoleRemoved(player);
        }
    }
}