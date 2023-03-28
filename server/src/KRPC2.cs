using SpaceWarp;
using SpaceWarp.API.Mods;
using BepInEx;
using KSP.Messages;
using KRPC;
using KRPC.Server;
using KRPC.Service;
using KRPC2.Utils;

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
            Game.Messages.Subscribe(typeof(GameStateChangedMessage), OnGameStateChanged, false, true);
            Game.Messages.Subscribe(typeof(PauseStateChangedMessage), OnPauseStateChanged, false, true);

            if (core == null)
            {
                core = Core.Instance;
                config = ConfigurationFile.Instance;
                foreach (var server in config.Configuration.Servers)
                    core.Add(server.Create());

                core.OnClientRequestingConnection += (s, e) => e.Request.Allow();
            }

            CallContext.GameScene = GameScene.None;
            CallContext.IsPaused = () => IsPaused;
            CallContext.Pause = () =>
            {
                KRPC.Utils.Logger.WriteLine("Pause game");
                Game.ViewController.SetPause(true);
            };
            CallContext.Unpause = () =>
            {
                KRPC.Utils.Logger.WriteLine("Unpause game");
                Game.ViewController.SetPause(false);
            };
        }

        private void OnGameStateChanged(MessageCenterMessage obj)
        {
            var gameStateMessage = obj as GameStateChangedMessage;
            if (gameStateMessage == null)
                return;
            var gameState = gameStateMessage.CurrentState;
            KRPC.Utils.Logger.WriteLine("Game state changed to " + gameState);
            // FIXME: why can't use use ToGameScene as an extension method?
            var gameScene = GameScenesExtensions.ToGameScene(gameState);
            KRPC.Utils.Logger.WriteLine("Game scene switched to " + gameScene);
            CallContext.GameScene = gameScene;

            // Stop the server when we are not in a game scene (e.g. in the main menu)
            if (gameScene == GameScene.None)
            {
                core.StopAll();
                return;
            }

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

        private void OnPauseStateChanged(MessageCenterMessage obj)
        {
            var pauseStateMessage = obj as PauseStateChangedMessage;
            if (pauseStateMessage == null)
                return;
            var pauseState = pauseStateMessage.Paused;
            KRPC.Utils.Logger.WriteLine("Game pause state changed to " + pauseState);
            IsPaused = pauseState;
        }

        /// <summary>
        /// Whether the game is paused
        /// </summary>
        public bool IsPaused { get; private set; }

        /// <summary>
        /// Update the servers, called on each tick.
        /// </summary>
        public void FixedUpdate()
        {
            if (core != null && core.AnyRunning)
                core.Update();
        }

        /// <summary>
        /// Update the servers, when the game is paused.
        /// </summary>
        public void Update()
        {
            if (IsPaused && !config.Configuration.PauseServerWithGame)
                FixedUpdate();
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
