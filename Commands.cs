using System;
using CommandSystem;
using Exiled.API.Features;
using Exiled.CustomRoles.API.Features;
using Exiled.Permissions.Extensions;

namespace Scp066
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class Scp066Command : ICommand
    {
        public string Command => "scp066";
        public string[] Aliases => new[] { "066" };
        public string Description => "Zarzadzanie SCP-066";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission("scp066.admin")) { response = "Brak uprawnien."; return false; }
            if (arguments.Count < 2) { response = "Uzycie: scp066 give/remove <nick/id>"; return false; }

            Player target = Player.Get(arguments.At(1));
            if (target == null) { response = "Nie znaleziono gracza."; return false; }

            if (!CustomRole.TryGet(660, out CustomRole role)) { response = "Blad roli."; return false; }

            if (arguments.At(0).ToLower() == "give") { role.AddRole(target); response = "Nadano."; return true; }
            role.RemoveRole(target); response = "Zabrano."; return true;
        }
    }
}