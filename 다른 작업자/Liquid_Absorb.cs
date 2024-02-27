using com.zibra.liquid.Manipulators;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnitySimpleLiquid;

public class Liquid_Absorb : MonoBehaviour
{
    [SerializeField] private Water_Create liquidSystem;
    private Vector3 _originPos;

    [SerializeField] private MeshPressure meshPressure;

    [SerializeField] private ZibraLiquidVoid liquidVoid;

    [SerializeField] private MeshRenderer zibraLiquidMeshRenderer;

    [SerializeField] private MeshRenderer liquidMeshRenderer;

    [SerializeField] private long deleteLiquidCount = 0;

    [SerializeField] private float zibraLiquidVolume;

    [SerializeField] private UnityEvent onFullLiquid;
    private void Awake()
    {
        _originPos = transform.parent.position;
    }
    private void Update()
    {
        LiquidAbsorb();
        //PosChange();
    }

    private void PosChange()
    {
        var pos = _originPos;
        pos.y += meshPressure.ThresholdWorldY * transform.lossyScale.y;
        transform.position = pos;
    }

    private void LiquidCallback(ForceInteractionData data)
    {
        var color = data.LiquidColor;
        meshPressure._liquidMat.color = color;
    }

    private void LiquidAbsorb()
    {
        if (!liquidSystem) return;
        if (liquidSystem.LiquidAmountPercent >= 1f)
        {
            liquidVoid.enabled = false;
            onFullLiquid?.Invoke();
            return;
        }
        else
            liquidVoid.enabled = true;

        if (liquidVoid.DeletedParticleCountPerFrame <= 0) return;

        deleteLiquidCount += liquidVoid.DeletedParticleCountPerFrame;
        liquidSystem.LiquidAmountPercent += liquidVoid.DeletedParticleCountPerFrame * zibraLiquidVolume;

        liquidMeshRenderer.materials = zibraLiquidMeshRenderer.materials;

        //var mat = Instantiate(zibraLiquidMeshRenderer.materials[0]);

        //mat.SetFloat("_Smoothness", 0.2f);
        //mat.SetFloat("_NormalStrenght", 0f);

        //liquidMeshRenderer.material = mat;

        //var liquidColor = meshPressure._liquidMat.GetColor("_DeepColor");

        //var combineColor = new Color();
        //ColorLogic();

        //meshPressure._liquidMat.SetColor("_DeepColor", combineColor);
        //meshPressure._liquidMat.SetColor("_ShallowColor", combineColor * 0.9f);

        //void ColorLogic()
        //{
        //    combineColor = Color.white;
        //}
    }
}
