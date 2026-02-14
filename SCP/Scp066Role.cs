using Exiled.API.Features;
using Exiled.CustomRoles.API.Features;
using CustomPlayerEffects;
using MEC;
using PlayerRoles;
using UnityEngine;

namespace Scp066
{
    public class Scp066Role : CustomRole
    {
        public override uint Id { get; set; } = 660;
        public override string Name { get; set; } = "SCP-066";
        public override RoleTypeId Role { get; set; } = RoleTypeId.Scp0492;
        public override int MaxHealth { get; set; } = 2000;
        public override float SpawnChance { get; set; } = 0f;
        public override string CustomInfo { get; set; } = "SCP-066";
        public override string Description { get; set; } = "Eric's Toy";

        protected override void RoleAdded(Player player)
        {
            if (player == null || !Check(player)) return;

            Timing.CallDelayed(0.5f, () =>
            {
                if (player == null || !player.IsAlive) return;

                player.MaxHealth = 2000f;
                player.Health = 2000f;
                player.HumeShield = 500f;
                player.MaxHumeShield = 500f;


                player.Scale = new Vector3(0.3f, 0.3f, 0.3f);

                player.EnableEffect<Slowness>(30, 9999f);

                player.ClearInventory();
            });

            if (!player.GameObject.TryGetComponent(out Scp066AbilityComponent _))
                player.GameObject.AddComponent<Scp066AbilityComponent>();

            base.RoleAdded(player);
        }

        protected override void RoleRemoved(Player player)
        {
            player.Scale = Vector3.one;
            player.DisableEffect<Slowness>();

            if (player.GameObject.TryGetComponent(out Scp066AbilityComponent comp))
                Object.Destroy(comp);

            base.RoleRemoved(player);
        }
    }
}
