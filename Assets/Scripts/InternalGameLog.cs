using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InternalGameLog
{
    public static void LogMessage(string message)
    {
        #if UNITY_EDITOR
        Debug.Log(message);
        #endif
    }

    public static void LogError(string message)
    {
        #if UNITY_EDITOR
        Debug.LogWarning(message);
        #endif
    }
}
