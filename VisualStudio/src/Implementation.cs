using UnityEngine;

namespace LonelyOrca
{
    public class Implementation
    {
        private const string NAME = "Lonely-Orca";

        public static void OnLoad()
        {
            Log("Version " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version);

            uConsole.RegisterCommand("orca-trigger", new uConsole.DebugCommand(OrcaTrigger));
        }

        internal static void Log(string message)
        {
            Debug.LogFormat("[" + NAME + "] {0}", message);
        }

        internal static void Log(string message, params object[] parameters)
        {
            string preformattedMessage = string.Format("[" + NAME + "] {0}", message);
            Debug.LogFormat(preformattedMessage, parameters);
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