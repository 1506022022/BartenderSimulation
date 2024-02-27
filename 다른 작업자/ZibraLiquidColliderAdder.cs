using com.zibra.liquid.Manipulators;
using com.zibra.liquid.Solver;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZibraLiquidColliderAdder : MonoBehaviour
{
    [SerializeField] private ZibraLiquidCollider zibraLiquidCollider;

    private void Awake()
    {
        //ZibraSingleton.Instance._ZibraLiquid.AddCollider(zibraLiquidCollider);
    }
}
