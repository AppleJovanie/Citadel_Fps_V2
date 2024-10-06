using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalRereferences : MonoBehaviour
{
   public static GlobalRereferences Instance { get;  set; }
   public GameObject bulletImpactEffectPrefab;
   public GameObject bloodSprayEffect;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
}
