using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using ProjectMER;
using MEC;
using Mirror;
using PlayerRoles;
using ProjectMER.Features;
using ProjectMER.Features.Objects;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Scp066
{
    public class Scp066AbilityComponent : MonoBehaviour
    {
        private Player _player;
        private SchematicObject _schematic;
        private AudioPlayer _audioPlayer;
        private Speaker _speaker;

        private bool _isAttackActive;
        private float _cdEric, _cdMusic, _cdAttack;

        // Zegar do hintów, żeby nie zawiesić serwera w EXILED 9
        private float _hintTimer = 0f;

        private string _audioPath = Path.Combine(Paths.Configs, "Plugins", "scp066", "Audio");

        private void Start()
        {
            _player = Player.Get(gameObject);
            if (_player == null || Plugin.Instance == null)
            {
                Destroy(this);
                return;
            }

            try
            {
                _schematic = ObjectSpawner.SpawnSchematic(Plugin.Instance.Config.SchematicName, _player.Position);
                if (_schematic != null)
                {
                    _schematic.transform.parent = null;
                    _schematic.transform.localScale = Vector3.one;

                    Timing.CallDelayed(1.5f, () => {
                        if (_player != null && _player.Connection != null && _schematic != null)
                        {
                            NetworkIdentity ni = _schematic.gameObject.GetComponent<NetworkIdentity>();
                            if (ni != null) _player.Connection.Send(new ObjectDestroyMessage { netId = ni.netId });
                        }
                    });
                }
            }
            catch (Exception e) { Log.Error($"[MER] Błąd schematu: {e}"); }

            Timing.CallDelayed(1.2f, () =>
            {
                try
                {
                    if (!Directory.Exists(_audioPath))
                    {
                        Log.Warn($"[SCP-066] Brak folderu audio! Utwórz go: {_audioPath}");
                        // NIE PRZERYWAMY KODU, pozwalamy na atak bez dźwięku!
                    }
                    else
                    {
                        LoadClipIfExists("Beethoven");
                        LoadClipIfExists("Eric1"); LoadClipIfExists("Eric2"); LoadClipIfExists("Eric3");
                        LoadClipIfExists("Notes1"); LoadClipIfExists("Notes2"); LoadClipIfExists("Notes3");
                        LoadClipIfExists("Notes4"); LoadClipIfExists("Notes5"); LoadClipIfExists("Notes6");
                    }

                    string botId = $"SCP066-{_player.Id}-{UnityEngine.Random.Range(1, 9999)}";

                    _audioPlayer = AudioPlayer.CreateOrGet(botId, onIntialCreation: (p) =>
                    {
                        float radius = Plugin.Instance.Config.AttackRadius;
                        _speaker = p.AddSpeaker("Main", _player.Position, radius, false, radius);
                        if (_speaker != null) _speaker.transform.parent = null;
                    });
                }
                catch (Exception e)
                {
                    Log.Error($"[SCP-066] Błąd Inicjalizacji Audio: {e}");
                }
            });

            Exiled.Events.Handlers.Player.Hurting += OnHurting;
        }

        private void LoadClipIfExists(string alias)
        {
            string path = Path.Combine(_audioPath, alias + ".ogg");
            if (File.Exists(path)) AudioClipStorage.LoadClip(path, alias);
        }

        private void OnHurting(HurtingEventArgs ev)
        {
            // BLOKUJEMY ZWYKŁY ATAK (z łapy). Obrażenia z 'Beethoven' mają DamageType.Custom
            if (ev.Attacker == _player && ev.DamageHandler.Type != DamageType.Custom)
            {
                ev.IsAllowed = false;
                ev.Amount = 0;
            }
        }

        private void Update()
        {
            if (_player == null || !_player.IsAlive) return;

            if (_schematic != null)
            {
                _schematic.transform.position = _player.Position + new Vector3(0, -0.55f, 0);
                _schematic.transform.rotation = _player.Rotation;
            }

            if (_speaker != null)
                _speaker.transform.position = _player.Position;

            if (_cdEric > 0) _cdEric -= Time.deltaTime;
            if (_cdMusic > 0) _cdMusic -= Time.deltaTime;
            if (_cdAttack > 0) _cdAttack -= Time.deltaTime;

            // Timer do Hintów - odświeżamy tylko raz na sekundę
            _hintTimer += Time.deltaTime;
            if (_hintTimer >= 1f)
            {
                ShowHints();
                _hintTimer = 0f; // Zaczynamy liczyć od nowa
            }
        }

        private bool TryPlayAudio(string alias)
        {
            if (_audioPlayer == null)
            {
                // Jeśli serwer nie wykrył folderu z audio, powiadamia gracza, ale nie przerywa skilla
                _player.ShowHint("<color=red>Brak plików Audio na serwerze!</color>", 2f);
                return false;
            }
            _audioPlayer.AddClip(alias, Plugin.Instance.Config.Volume);
            return true;
        }

        public void TriggerBeethoven()
        {
            if (_isAttackActive)
            {
                _player.ShowHint("<color=red>Beethoven już trwa!</color>", 2f);
                return;
            }

            if (_cdAttack > 0)
            {
                _player.ShowHint($"<color=red>Cooldown Beethoven: {(int)_cdAttack}s</color>", 2f);
                return;
            }

            // Próbujemy odpalić muzykę. Niezależnie czy się uda, atak idzie dalej.
            TryPlayAudio("Beethoven");

            _cdAttack = Plugin.Instance.Config.CooldownAttack;
            Timing.RunCoroutine(AttackProcess());
        }

        public void PlayEric()
        {
            if (_cdEric > 0)
            {
                _player.ShowHint($"<color=red>Cooldown Eric: {(int)_cdEric}s</color>", 2f);
                return;
            }

            if (TryPlayAudio($"Eric{UnityEngine.Random.Range(1, 4)}"))
            {
                _cdEric = Plugin.Instance.Config.CooldownEric;
            }
        }

        public void PlayMusic()
        {
            if (_cdMusic > 0)
            {
                _player.ShowHint($"<color=red>Cooldown Music: {(int)_cdMusic}s</color>", 2f);
                return;
            }

            if (TryPlayAudio($"Notes{UnityEngine.Random.Range(1, 7)}"))
            {
                _cdMusic = Plugin.Instance.Config.CooldownMusic;
            }
        }

        private IEnumerator<float> AttackProcess()
        {
            _isAttackActive = true;

            int duration = Plugin.Instance.Config.AttackDuration;
            float damage = Plugin.Instance.Config.Damage;
            float radius = Plugin.Instance.Config.AttackRadius;

            for (int i = 0; i < duration; i++)
            {
                if (_player == null || !_player.IsAlive) break;

                foreach (Player target in Player.List)
                {
                    if (target == null || target == _player) continue;
                    if (target.Role.Team == Team.SCPs) continue;
                    if (!target.IsAlive) continue;

                    if (Vector3.Distance(_player.Position, target.Position) > radius) continue;

                    try
                    {
                        target.Hurt(damage, "SCP-066 (Symphony)");
                        target.EnableEffect(EffectType.Concussed, 2f);
                    }
                    catch (Exception e)
                    {
                        Log.Warn($"[SCP-066] Błąd przy zadawaniu obrażeń: {e.Message}");
                    }
                }

                yield return Timing.WaitForSeconds(1f);
            }

            _isAttackActive = false;
        }

        private void ShowHints()
        {
            string att = _isAttackActive
                ? "<color=red>GRA</color>"
                : (_cdAttack > 0 ? $"<color=orange>{(int)_cdAttack}s</color>" : "<color=green>GOTOWY</color>");

            string eric = _cdEric > 0 ? $"<color=orange>{(int)_cdEric}s</color>" : "<color=green>OK</color>";
            string music = _cdMusic > 0 ? $"<color=orange>{(int)_cdMusic}s</color>" : "<color=green>OK</color>";

            _player.ShowHint(
                $"<align=right><b>SCP-066</b>\nBeethoven: {att}\nEric: {eric}\nNotes: {music}</align>",
                1.5f
            );
        }

        private void OnDestroy()
        {
            Exiled.Events.Handlers.Player.Hurting -= OnHurting;
            if (_schematic != null) _schematic.Destroy();
            if (_audioPlayer != null) _audioPlayer.Destroy();
        }
    }
}