using Game.Prefabs;
using HarmonyLib;
using Unity.Entities;

namespace BetterPedestrianPathfind;

[HarmonyPatch]
class Patches
{
    private static PathfindCostInfo my_WalkingCost = new PathfindCostInfo(0f, 0f, 0f, 0.01f);
    private static PathfindCostInfo my_CrosswalkCost = new PathfindCostInfo(0f, 0f, 0f, 5f);
    private static PathfindCostInfo my_UnsafeCrosswalkCost = new PathfindCostInfo(0f, 1000f, 0f, 5f);
    private static PathfindCostInfo my_SpawnCost = new PathfindCostInfo(5f, 0f, 0f, 0f);

    [HarmonyPatch(typeof(PedestrianPathfind), "Initialize")]
    [HarmonyPrefix]
    static bool Prefix(ref EntityManager entityManager, ref Entity entity, PedestrianPathfind __instance) {
        var myLogSource = BepInEx.Logging.Logger.CreateLogSource("BetterPedestrianPathfind");
        myLogSource.LogInfo("Override PedestrianPathfind called");
        BepInEx.Logging.Logger.Sources.Remove(myLogSource);

        PathfindPedestrianData componentData = default(PathfindPedestrianData);
        componentData.m_WalkingCost = my_WalkingCost.ToPathfindCosts();
        componentData.m_CrosswalkCost = my_CrosswalkCost.ToPathfindCosts();
        componentData.m_UnsafeCrosswalkCost = my_UnsafeCrosswalkCost.ToPathfindCosts();
        componentData.m_SpawnCost = my_SpawnCost.ToPathfindCosts();
        entityManager.SetComponentData(entity, componentData);
        return false;
    }
}