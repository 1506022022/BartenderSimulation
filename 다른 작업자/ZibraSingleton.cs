using com.zibra.liquid.Solver;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZibraSingleton : MonoBehaviour
{
    private static ZibraSingleton instance;
    public static ZibraSingleton Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<ZibraSingleton>();
            return instance;
        }
    }

    [SerializeField] private ZibraLiquid zibraLiquid;
    public ZibraLiquid _ZibraLiquid
    {
        get
        {
            return zibraLiquid;
        }
    }
}