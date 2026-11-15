using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logger
{

    public static void Log(string msg)
        => Debug.Log($"{DateTime.Now} {msg}");

    public static void LogWarning(string msg)
        => Debug.LogWarning($"{DateTime.Now} {msg}");

    public static void LogError(string msg)
        => Debug.LogError($"{DateTime.Now} {msg}");
}
