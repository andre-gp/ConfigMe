using UnityEngine;

namespace ConfigMe
{
    public class ConfigMeLogger
    {
        const string INFO_PREFIX = "<b><color=#b2e9f7>[ConfigMe]</color></b> ";
        const string ERROR_PREFIX = "<b><color=#fa5757>[ConfigMe]</color></b> ";


        public static void LogInfo(string message)
        {
            Debug.Log(INFO_PREFIX + message);
        }

        public static void LogInfo(string message, Object context)
        {
            Debug.Log(INFO_PREFIX + message, context);
        }

        public static void LogError(string error)
        {
            Debug.LogError(ERROR_PREFIX + error);
        }

        public static void LogError(string error, Object context)
        {
            Debug.LogError(ERROR_PREFIX + error, context);
        }

    }
}
