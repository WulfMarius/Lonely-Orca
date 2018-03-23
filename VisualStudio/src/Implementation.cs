using UnityEngine;

namespace LonelyOrca
{
    public class Bootstrap
    {
        public static void OnLoad()
        {
            Debug.Log("[Lonely-Orca]: Version " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version);

            uConsole.RegisterCommand("orca-trigger", new uConsole.DebugCommand(OrcaTrigger));
        }

        private static void OrcaTrigger()
        {
            OrcaTravel orcaTravel = GameObject.FindObjectOfType<OrcaTravel>();
            if (orcaTravel == null)
            {
                Debug.Log("  No orca found.");
                return;
            }

            orcaTravel.SkipDelay();
        }
    }
}
