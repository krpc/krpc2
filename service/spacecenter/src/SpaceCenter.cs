using SpaceWarp;
using SpaceWarp.API.Mods;
using BepInEx;
using System;
using KSP.Game;
using KSP.Sim.impl;
using KRPC.Service;
using KRPC.Service.Attributes;

namespace KRPC2.SpaceCenter
{
    /// <summary>
    /// kRPC2 SpaceCenter service.
    /// Plugin class seems to be required for the DLL to be loaded
    /// </summary>
    [BepInPlugin("krpc2spacecenter", "krpc2spacecenter", "0.1.0")]
    [BepInDependency(SpaceWarpPlugin.ModGuid, SpaceWarpPlugin.ModVer)]
    public class SpaceCenterPlugin : BaseSpaceWarpPlugin
    {
    }

    /// <summary>
    /// Space center service.
    /// </summary>
    // Note: named "SpaceCenter2" for now to not collide with the KSP1 service of the same name.
    // We can change this once clients have proper namespacing for KSP1/2 services.
    [KRPCService(Name = "SpaceCenter2", Id = 99)]
    public static class SpaceCenter
    {
        private static VesselComponent InternalActiveVessel => GameManager.Instance.Game.ViewController.GetActiveSimVessel(true);

        /// <summary>
        /// The currently active vessel.
        /// </summary>
        [KRPCProperty(GameScene = GameScene.Flight)]
        public static Vessel ActiveVessel
        {
            get { return new Vessel(InternalActiveVessel); }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
