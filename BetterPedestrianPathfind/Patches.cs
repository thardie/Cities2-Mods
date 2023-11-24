using Game.Prefabs;
using HarmonyLib;
using Unity.Entities;

namespace BetterPedestrianPathfind;

[HarmonyPatch]
class Patches
{
    [HarmonyPatch(typeof(PedestrianPathfind), "Initialize")]
    [HarmonyPrefix]
    static bool Prefix(ref EntityManager entityManager, ref Entity entity, PedestrianPathfind __instance) {
        PathfindCostInfo m_WalkingCost = new PathfindCostInfo(0f, 0f, 0f, 0.01f);
        PathfindCostInfo m_CrosswalkCost = new PathfindCostInfo(0f, 0f, 0f, 5f);
        PathfindCostInfo m_UnsafeCrosswalkCost = new PathfindCostInfo(0f, 1000f, 0f, 5f);
        PathfindCostInfo m_SpawnCost = new PathfindCostInfo(5f, 0f, 0f, 0f);

        var myLogSource = BepInEx.Logging.Logger.CreateLogSource("BetterPedestrianPathfind");
        myLogSource.LogInfo("Override PedestrianPathfind called");
        BepInEx.Logging.Logger.Sources.Remove(myLogSource);

        PathfindPedestrianData componentData = default(PathfindPedestrianData);
        componentData.m_WalkingCost = m_WalkingCost.ToPathfindCosts();
        componentData.m_CrosswalkCost = m_CrosswalkCost.ToPathfindCosts();
        componentData.m_UnsafeCrosswalkCost = m_UnsafeCrosswalkCost.ToPathfindCosts();
        componentData.m_SpawnCost = m_SpawnCost.ToPathfindCosts();
        entityManager.SetComponentData(entity, componentData);
        return false;
    }
}