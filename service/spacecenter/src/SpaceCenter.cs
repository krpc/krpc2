using SpaceWarp;
using SpaceWarp.API.Mods;
using BepInEx;
using KRPC.Service.Attributes;
using KSP.Game;
using KSP.Sim.impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRPC2.SpaceCenter
{
    // Plugin class seems to be required for the DLL to get loaded into the game
    [BepInPlugin("krpc2spacecenter", "krpc2spacecenter", "0.1.0")]
    [BepInDependency(SpaceWarpPlugin.ModGuid, SpaceWarpPlugin.ModVer)]
    public class SpaceCenterPlugin : BaseSpaceWarpPlugin
    {
    }

    // Note: named "SpaceCenter2" for now to not collide with the KSP1 service of the same name.
    // We can change this once clients have proper namespacing for KSP1/2 services.
    [KRPCService(Name = "SpaceCenter2", Id = 99)]
    public static class SpaceCenter
    {
        private static VesselComponent ActiveVessel => GameManager.Instance.Game.ViewController.GetActiveSimVessel(true);

        [KRPCProperty]
        public static double HorizontalSurfaceSpeed => ActiveVessel.HorizontalSrfSpeed;

        [KRPCProperty]
        public static double VerticalSurfaceSpeed => ActiveVessel.VerticalSrfSpeed;

        [KRPCProperty]
        public static double TerrainAltitude => ActiveVessel.AltitudeFromTerrain;

        [KRPCProperty]
        public static double SealevelAltitude => ActiveVessel.AltitudeFromSeaLevel;
    }
}
