using SpaceWarp;
using SpaceWarp.API.Mods;
using BepInEx;
using KRPC;
using KRPC.Server;
using KRPC.Service;

namespace KRPC2
{
    /// <summary>
    /// kRPC2 mod
    /// </summary>
    [BepInPlugin("krpc2", "krpc2", "0.1.0")]
    [BepInDependency(SpaceWarpPlugin.ModGuid, SpaceWarpPlugin.ModVer)]
    public class KRPC2 : BaseSpaceWarpPlugin
    {
        internal static ConfigurationFile config;
        private Core core;

        /// <summary>
        /// Initialize the mod
        /// </summary>
        public override void OnInitialized()
        {
            if (core != null)
                return;

            core = Core.Instance;
            config = ConfigurationFile.Instance;
            foreach (var server in config.Configuration.Servers)
                core.Add(server.Create());

            // FIXME: set game scene correctly
            CallContext.GameScene = GameScene.All;
            // FIXME: need to add handlers for game pausing and unpausing
            core.OnClientRequestingConnection += (s, e) => e.Request.Allow();

            if (config.Configuration.AutoStartServers)
            {
                KRPC.Utils.Logger.WriteLine("Auto-starting server");
                try
                {
                    core.StartAll();
                }
                catch (ServerException e)
                {
                    KRPC.Utils.Logger.WriteLine(
                        "Failed to auto-start servers:" + e,
                        KRPC.Utils.Logger.Severity.Error
                    );
                }
            }
        }

        /// <summary>
        /// Update the servers, called on each tick
        /// </summary>
        public void Update()
        {
            if (core != null)
                core.Update();
        }

        /// <summary>
        /// Stop the servers when the game exits
        /// </summary>
        public void OnApplicationQuit()
        {
            core.StopAll();
        }
    }
}
