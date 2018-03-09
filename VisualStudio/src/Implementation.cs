using UnityEngine;
using UnityEngine.SceneManagement;

namespace LonelyOrca
{
    public class Bootstrap
    {
        public static void OnLoad()
        {
            Debug.Log("[Lonely-Orca]: Version " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version);
        }
    }
}
