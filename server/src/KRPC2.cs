using SpaceWarp;
using SpaceWarp.API.Mods;
using BepInEx;
using KRPC;
using KRPC.Server;
using KRPC.Server.TCP;
using KRPC.Service;
using System;
using System.Net;

namespace KRPC2
{
    /// <summary>
    /// kRPC2 mod
    /// </summary>
    [BepInPlugin("krpc2", "krpc2", "0.1.0")]
    [BepInDependency(SpaceWarpPlugin.ModGuid, SpaceWarpPlugin.ModVer)]
    public class KRPC2 : BaseSpaceWarpPlugin
    {
        private Core core;

        /// <summary>
        /// Initialize the mod
        /// </summary>
        public override void OnInitialized()
        {
            Logger.LogInfo("Initializing core...");
            core = Core.Instance;
            CallContext.GameScene = GameScene.SpaceCenter;
            // FIXME: need to add handlers for game pausing and unpausing
            core.OnClientRequestingConnection += (s, e) => e.Request.Allow();
            Logger.LogInfo("Core initialized");

            Logger.LogInfo("Starting server (rpc port 50000, stream port 50001");
            var rpcTcpServer = new TCPServer(IPAddress.Any, 50000);
            var streamTcpServer = new TCPServer(IPAddress.Any, 50001);
            var rpcServer = new KRPC.Server.ProtocolBuffers.RPCServer(rpcTcpServer);
            var streamServer = new KRPC.Server.ProtocolBuffers.StreamServer(streamTcpServer);
            var server = new Server(Guid.NewGuid(), Protocol.ProtocolBuffersOverTCP, "KRPC2 Server", rpcServer, streamServer);
            core.Add(server);
            core.StartAll();
            Logger.LogInfo("Server started");
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
