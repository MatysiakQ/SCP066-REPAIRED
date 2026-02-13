using System;
using CommandSystem;
using Exiled.API.Features;

namespace Scp066
{
    // Rejestrujemy komendę pod "ClientCommandHandler" - czyli zwykła konsola gracza (~)
    [CommandHandler(typeof(ClientCommandHandler))]
    public class Scp066AbilityCommand : ICommand
    {
        // To wpisuje gracz w bindzie: .scp066
        public string Command => "scp066";
        public string[] Aliases => new[] { "66", "066" };
        public string Description => "Używanie umiejętności SCP-066 (Bindy).";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            // 1. Sprawdzamy, czy gracz to gracz
            Player player = Player.Get(sender);
            if (player == null)
            {
                response = "Komenda tylko dla graczy.";
                return false;
            }

            // 2. Sprawdzamy, czy gracz ma komponent SCP-066
            if (!player.GameObject.TryGetComponent(out Scp066AbilityComponent ability))
            {
                response = "Nie jesteś SCP-066!";
                return false;
            }

            // 3. Sprawdzamy argumenty (co chce zrobić?)
            if (arguments.Count < 1)
            {
                response = "Użycie: .scp066 [eric / music / boom]";
                return false;
            }

            string action = arguments.At(0).ToLower();

            switch (action)
            {
                case "eric":
                case "e":
                    ability.PlayEric();
                    response = "Odtwarzam Erica.";
                    return true;

                case "music":
                case "m":
                    ability.PlayMusic();
                    response = "Odtwarzam muzykę.";
                    return true;

                case "boom":
                case "beethoven":
                case "b":
                    // Tu możemy dodać sprawdzenie, czy już nie trwa atak
                    ability.TriggerBeethoven();
                    response = "Beethoven aktywowany.";
                    return true;

                default:
                    response = "Nieznana umiejętność. Dostępne: eric, music, boom";
                    return false;
            }
        }
    }
}