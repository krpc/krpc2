using KRPC.Service;
using KSP.Game;

namespace KRPC2.Utils
{
    static class GameScenesExtensions
    {
        internal static GameScene ToGameScene(GameState state)
        {
            switch (state)
            {
                case GameState.KerbalSpaceCenter:
                    return GameScene.SpaceCenter;
                case GameState.FlightView:
                    return GameScene.Flight;
                case GameState.Map3DView:
                    return GameScene.TrackingStation;
                case GameState.VehicleAssemblyBuilder:
                    return GameScene.Editor | GameScene.EditorVAB | GameScene.EditorSPH;
                default:
                    return GameScene.None;
            }
        }
    }
}
