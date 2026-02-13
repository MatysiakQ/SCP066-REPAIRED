using System;
using Exiled.API.Features;
using UnityEngine;
using MEC;
using System.Collections.Generic;
using PlayerRoles;
using Exiled.API.Enums;
using Exiled.Events.EventArgs.Player;

namespace Scp066
{
    public class Scp066AbilityComponent : MonoBehaviour
    {
        private Player _player;
        private bool _isAttackActive;
        private float _cdEric, _cdMusic, _cdAttack;
        private AudioPlayer _audioPlayer;

        private void Start()
        {
            _player = Player.Get(gameObject);
            _player.Scale = new Vector3(0.35f, 0.35f, 0.35f);

            Exiled.Events.Handlers.Player.Hurting += OnHurting;

            _player.Broadcast(10, "<size=25><b>You are SCP-066</b></size>\nUse binds to play sounds! (.scp066 music/eric/boom)");

            Timing.CallDelayed(0.7f, () =>
            {
                try
                {
                    string playerId = $"SCP066-{_player.Id}-{UnityEngine.Random.Range(1, 9999)}";
                    _audioPlayer = AudioPlayer.CreateOrGet(playerId, onIntialCreation: (p) =>
                    {
                        Speaker speaker = p.AddSpeaker("Main", isSpatial: false, minDistance: 0f, maxDistance: 40f);
                        speaker.transform.parent = _player.GameObject.transform;
                        speaker.transform.localPosition = Vector3.zero;
                    });
                }
                catch (Exception e) { Log.Error($"[SCP-066] Audio Init Error: {e}"); }
            });
        }

        private void OnHurting(HurtingEventArgs ev)
        {
            if (ev.Attacker == _player && ev.DamageType == DamageType.Scp0492)
            {
                ev.Amount = 0;
            }
        }

        public void TriggerBeethoven()
        {
            if (_audioPlayer == null || _isAttackActive || _cdAttack > 0) return;
            _audioPlayer.AddClip("Beethoven", volume: Plugin.Instance.Config.Volume);
            _cdAttack = Plugin.Instance.Config.CooldownAttack;
            Timing.RunCoroutine(AttackProcess());
        }

        public void PlayEric()
        {
            if (_cdEric > 0 || _audioPlayer == null) return;
            _audioPlayer.AddClip($"Eric{UnityEngine.Random.Range(1, 4)}", volume: Plugin.Instance.Config.Volume);
            _cdEric = Plugin.Instance.Config.CooldownEric;
        }

        public void PlayMusic()
        {
            if (_cdMusic > 0 || _audioPlayer == null) return;
            _audioPlayer.AddClip($"Notes{UnityEngine.Random.Range(1, 7)}", volume: Plugin.Instance.Config.Volume);
            _cdMusic = Plugin.Instance.Config.CooldownMusic;
        }

        private void Update()
        {
            if (_player == null || !_player.IsAlive) return;
            if (_cdEric > 0) _cdEric -= Time.deltaTime;
            if (_cdMusic > 0) _cdMusic -= Time.deltaTime;
            if (_cdAttack > 0) _cdAttack -= Time.deltaTime;
            ShowHints();
        }

        private IEnumerator<float> AttackProcess()
        {
            _isAttackActive = true;
            for (int i = 0; i < (int)Plugin.Instance.Config.AttackDuration; i++)
            {
                if (_player == null || !_player.IsAlive) break;
                foreach (Player target in Player.List)
                {
                    if (target != _player && target.Role.Team != Team.SCPs && target.IsAlive && Vector3.Distance(_player.Position, target.Position) <= Plugin.Instance.Config.AttackRadius)
                    {
                        target.Hurt(Plugin.Instance.Config.Damage, "SCP-066 (Symphony)");
                        target.EnableEffect(EffectType.Concussed, 2f);
                    }
                }
                yield return Timing.WaitForSeconds(1f);
            }
            _isAttackActive = false;
        }

        private void ShowHints()
        {
            string att = _isAttackActive ? "<color=red>PLAYING</color>" : (_cdAttack > 0 ? $"{(int)_cdAttack}s" : "<color=green>READY</color>");
            _player.ShowHint($"<align=right><size=20>SCP-066</size>\nBoom: {att}\nEric: {(_cdEric > 0 ? (int)_cdEric + "s" : "OK")}\nMusic: {(_cdMusic > 0 ? (int)_cdMusic + "s" : "OK")}</align>", 1f);
        }

        private void OnDestroy()
        {
            Exiled.Events.Handlers.Player.Hurting -= OnHurting;
            if (_audioPlayer != null) { _audioPlayer.Destroy(); _audioPlayer = null; }
        }
    }
}