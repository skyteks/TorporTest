using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SOSingleton<T> : ScriptableObject where T : ScriptableObject
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = CreateInstance<T>();
            }

            return _instance;
        }
    }
}
