using com.zibra.liquid.Manipulators;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Craft_Beer_Handle : MonoBehaviour
{
    [SerializeField] private Animator controller;
    [SerializeField] private ZibraLiquidEmitter liquidEmitter;
    [SerializeField] private float liquidCreateCount;

    private bool isCreated = false;
    private bool isMoving = false;
    public void Handle()
    {
        if (isMoving) return;

        controller.SetTrigger("Handling");
    }

    public void IsCreated()
    {
        isCreated = true;
        liquidEmitter.VolumePerSimTime = liquidCreateCount;
    }

    public void IsNotCreated()
    {
        isCreated = false;
        liquidEmitter.VolumePerSimTime = 0f;
    }

    public void IsMoving()
    {
        isMoving = true;
    }
    
    public void IsNotMoving()
    {
        isMoving = false;
    }
}
