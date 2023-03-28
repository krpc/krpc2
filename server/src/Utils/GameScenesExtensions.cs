using KRPC.Service;
using KSP.Game;

namespace KRPC2.Utils
{
    static class GameScenesExtensions
    {
        internal static GameScene ToGameScene(GameState state)
        {
            // FIXME: we are ignoring a lot of game states. We should probably extend krpc-core to support more scenes.
            switch (state)
            {
                case GameState.KerbalSpaceCenter:
                    return GameScene.SpaceCenter;
                case GameState.FlightView:
                case GameState.Launchpad:
                case GameState.Map3DView:
                case GameState.Runway:
                    return GameScene.Flight;
                case GameState.TrackingStation:
                    return GameScene.TrackingStation;
                case GameState.VehicleAssemblyBuilder:
                    return GameScene.EditorVAB;
                default:
                    return GameScene.None;
            }
        }
    }
}
