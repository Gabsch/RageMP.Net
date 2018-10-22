using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using RageMP.Net.Data;
using RageMP.Net.Enums;
using RageMP.Net.Interfaces;
using RageMP.Net.Scripting;

namespace RageMP.Net.Example
{
    public class ExampleResource : IResource
    {
        public async Task OnStartAsync()
        {
            MP.Logger.Info($"{nameof(ExampleResource)}: {nameof(OnStartAsync)}");

            MP.Events.PlayerChat += OnPlayerChat;
            MP.Events.PlayerReady += OnPlayerReady;

            MP.Events.PlayerDeath += OnPlayerDeath;
            MP.Events.PlayerCommand += OnPlayerCommand;

            var vehicle = await MP.Vehicles.NewAsync(VehicleHash.T20, Vector3.One);

            vehicle.SetColorRgb(new ColorRgba(255, 0, 255), new ColorRgba(0, 125, 25));
        }

        private Task OnPlayerReady(IPlayer player)
        {
            player.Dimension = MP.GlobalDimension;

            return Task.CompletedTask;
        }

        public Task OnStopAsync()
        {
            MP.Logger.Info($"{nameof(ExampleResource)}: {nameof(OnStopAsync)}");

            return Task.CompletedTask;
        }

        private async Task OnPlayerCommand(IPlayer player, string text)
        {
            if (text == "v")
            {
                var vehicle = await MP.Vehicles.NewAsync(VehicleHash.T20, player.Position, 0, "", 255, false, true, player.Dimension);

                player.PutIntoVehicle(vehicle, -1);

                return;
            }

            if (text == "e")
            {
                throw new Exception("TEST");

                return;
            }

            if (text == "g")
            {
                var vehicle = player.Vehicle;
                if (vehicle == null)
                {
                    player.OutputChatBoxAsync("Vehicle not found");

                    return;
                }

                var occupant = vehicle.GetOccupant(-1);
                if (occupant == null)
                {
                    player.OutputChatBoxAsync("Occupant not found");

                    return;
                }

                player.OutputChatBoxAsync($"Occupant {occupant.SocialClubName} found");

                return;
            }

            if (text == "o")
            {
                var vehicle = player.Vehicle;
                if (vehicle == null)
                {
                    player.OutputChatBoxAsync("Vehicle not found");

                    return;
                }

                foreach (var occupant in vehicle.Occupants)
                {
                    MP.Logger.Info($"Occupant: {occupant.SocialClubName}");
                }

                return;
            }

            if (text == "d")
            {
                MP.Logger.Debug($"HasData: {player.HasSharedData("TEST")}");

                player.SetSharedData("TEST", 12345);

                player.SetSharedData(new Dictionary<string, object>
                {
                    { "KEY1", 123.45 },
                    { "KEY2", player },
                    { "KEY3", player.Position },
                    { "KEY4", "LEL" },
                });

                return;
            }

            if (text == "r")
            {
                player.ResetSharedData("TEST");

                return;
            }

            if (text == "w")
            {
                foreach (var weapon in player.Weapons)
                {
                    MP.Logger.Debug($"Weapon: {weapon.Key} -> {weapon.Value}");
                }

                player.GiveWeapons(new Dictionary<uint, uint>
                {
                    { 0xD8DF3C3C, 20 },
                    { 0x5EF9FEC4, 123 },
                    { 0xE284C527, 500 }
                });

                return;
            }

            if (text == "c")
            {
                player.SetCustomization(false, new HeadBlendData(10, 34, 0, 42, 23, 0, 0.8f, 0.7f, 0.1f), 2, 3, 4, new float[0], new Dictionary<int, HeadOverlayData>(), new Dictionary<uint, uint>());

                return;
            }

            if (text == "m")
            {
                if (player.Vehicle == null)
                {
                    return;
                }

                player.Vehicle.SetColorRgb(new ColorRgba(255, 0, 0), new ColorRgba(0, 52, 0));

                return;
            }
        }

        private Task OnPlayerDeath(IPlayer player, uint reason, IPlayer killerplayer)
        {
            player.Spawn(player.Position, player.Heading);

            return Task.CompletedTask;
        }

        private Task OnPlayerChat(IPlayer player, string text)
        {
            MP.Markers.NewAsync(0, player.Position, Vector3.Zero, Vector3.UnitZ, 1, new ColorRgba(255, 255, 0, 255), true);

            MP.Objects.NewAsync(MP.Joaat("prop_coffin_01"), player.Position, Vector3.One);

            MP.TextLabels.NewAsync(player.Position, "HÖHÖ, höhö", 0, new ColorRgba(255, 255, 0, 255), 20, true);

            return Task.CompletedTask;
        }
    }
}
