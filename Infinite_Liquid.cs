using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitySimpleLiquid;

[RequireComponent(typeof(LiquidContainer))]
public class Infinite_Liquid : MonoBehaviour
{
    private LiquidContainer container;
    private void Awake()
    {
        container = GetComponent<LiquidContainer>();
    }

    private void Update()
    {
        InfiniteLiquid();
    }

    private void InfiniteLiquid()
    {
        container.FillAmountPercent = 1f;
    }
}
