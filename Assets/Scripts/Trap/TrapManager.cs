using System.Collections.Generic;
using UnityEngine;

//TP2 - Delpiano y Claro

public class TrapManager : MonoBehaviour
{
    public static TrapManager Instance { get; private set; }

    private Dictionary<object, object> activeTraps;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            activeTraps = new Dictionary<object, object>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void TriggerTrap<T>(T trap, object target)
    {
        if (!activeTraps.ContainsKey(trap))
        {
            ITrap<T> trapInterface = trap as ITrap<T>;
            if (trapInterface != null)
            {
                trapInterface.ActivateTrap((T)target);
                activeTraps.Add(trap, target);
            }
        }
    }

    public void ReleaseTrap<T>(T trap, object target)
    {
        if (activeTraps.ContainsKey(trap) && activeTraps[trap] == target)
        {
            ITrap<T> trapInterface = trap as ITrap<T>;
            if (trapInterface != null)
            {
                trapInterface.DeactivateTrap((T)target);
                activeTraps.Remove(trap);
            }
        }
    }
}

