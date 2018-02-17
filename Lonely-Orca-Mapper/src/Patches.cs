using Harmony;

using UnityEngine;

namespace LonelyOrca
{
    [HarmonyPatch(typeof(PlayAkSound), "OnEnable")]
    internal class PlayAkSoundPatch
    {
        private static void Postfix(PlayAkSound __instance)
        {
            GameAudioManager.Play3DSound(__instance.soundName, __instance.gameObject);
        }
    }

    [HarmonyPatch(typeof(GameManager), "Start")]
    internal class GameManager_Start
    {
        private static void Postfix(GameManager __instance)
        {
            if (GameManager.m_ActiveScene == "WhalingStationRegion")
            {
                GameObject orcaRoot = Resources.Load("orca_root") as GameObject;

                if (orcaRoot)
                {
                    GameObject orca = GameObject.Instantiate(orcaRoot);
                    orca.AddComponent<OrcaTravel>();
                }
            }
        }
    }
}