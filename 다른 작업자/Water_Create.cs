using com.zibra.liquid.DataStructures;
using com.zibra.liquid.Manipulators;
using com.zibra.liquid.Solver;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitySimpleLiquid;

public class Water_Create : MonoBehaviour
{
    [SerializeField] private bool canLiquidCreate = true;

    [SerializeField] private bool disableWaterInit = true;
    [Range(0f, 1f)]
    [SerializeField] private float liquidAmountPercent;
    public float LiquidAmountPercent
    {
        get
        {
            return liquidAmountPercent;
        }
        set
        {
            if (value < 0f) liquidAmountPercent = 0f;
            else if(value > 1f) liquidAmountPercent = 1f;
            else liquidAmountPercent = value;

            LiquidMeshChange();
        }
    }
    [SerializeField] private ZibraLiquidEmitter zibraLiquidEmitter;

    [Tooltip("액체 파티클 최소 소환 갯수")]
    [SerializeField] private float minLiquidCreateCount;

    [Tooltip("액체 파티클 최대 소환 갯수")]
    [SerializeField] private float maxLiquidCreateCount;

    [Tooltip("각도가 어느 정도 기울였을 때 액체가 흐르게 할 것인가 (-1 : 바닥방향, 1 : 하늘방향) ")]
    [Range(-1f, 1f)]
    [SerializeField] private float liquidCreateAngle;

    [Tooltip("액체가 나오는 속력의 최소 값")]
    [SerializeField] private float liquidMinPower;
    [Tooltip("액체가 나오는 속력의 최대 값")]
    [SerializeField] private float liquidMaxPower;

    [SerializeField] private Color liquidColor;

    [SerializeField] private MeshPressure liquidMesh;

    [Tooltip("Zibra Liquid 한 방울의 크기 ( 1 : 가득 찬 상태 )")]
    [SerializeField] private float zibraLiquidVolume;

    [SerializeField] private MeshRenderer zibraLiquidMeshRenderer;
    [SerializeField] private MeshRenderer liquidMeshRenderer;

    private void LiquidState(bool _is)
    {
        if (_is)
            AbleCreateLiquid();
        else
            UnableCreateLiquid();



        void UnableCreateLiquid()
        {
            zibraLiquidEmitter.VolumePerSimTime = 0f;
        }
        void AbleCreateLiquid()
        {
            var power = PowerNormalizing(transform.up.y);
            if (RemainLiquid())
            {
                zibraLiquidEmitter.VolumePerSimTime = Mathf.Lerp(minLiquidCreateCount, maxLiquidCreateCount, power);
                ColorChange(liquidColor);
            }
            else
                zibraLiquidEmitter.VolumePerSimTime = 0f;
        }
    }

    private void OnDisable()
    {
        if (disableWaterInit)
            LiquidAmountPercent = 1f;
        else
            LiquidAmountPercent = 0f;
    }
    private void LiquidMeshChange()
    {
        if (liquidMesh)
            liquidMesh.Amount = LiquidAmountPercent;
    }

    bool RemainLiquid()
    {
        return liquidAmountPercent > 0f;
    }

    private void Awake()
    {
        LiquidMeshChange();
    }
    private void Start()
    {
        //ColorChange(liquidColor);
    }
    private void Update()
    {
        if (canLiquidCreate)
        {
            LiquidState(IsWaterDrop());
            LiquidVelocitySetting();

            // 액체 파티클을 소환한 만큼 액체 양을 줄임
            LiquidDrop();
        }
        
        

        // 기울기에 따른 액체의 속도
        void LiquidVelocitySetting()
        {
            var upMaxVector = Vector3.zero;
            upMaxVector.y = liquidMaxPower;

            var upMinVector = Vector3.zero;
            upMinVector.y = liquidMinPower;

            var liquidPowerNormal = PowerNormalizing(transform.up.y);

            zibraLiquidEmitter.InitialVelocity = Vector3.Lerp(upMinVector, upMaxVector, liquidPowerNormal);
        }

        // 액체가 흐르는 각도인가 체크
        bool IsWaterDrop()
        {
            var upVector = transform.up;

            return upVector.y <= liquidCreateAngle;
        }
    }

    private void LiquidDrop()
    {
        if (RemainLiquid())
        {
            LiquidAmountPercent -= zibraLiquidEmitter.CreatedParticlesPerFrame * zibraLiquidVolume;
        }
    }

    // 색 변환 함수
    private void ColorChange(Color color)
    {
        zibraLiquidMeshRenderer.materials = liquidMeshRenderer.materials;
    }

    // Y벡터만을 가지고 liquidCreateAngle의 범위에 맞춰 0과1 사이의 값으로 변환하는 노말라이즈 함수
    // 입력받는 변수 값 = 바라보고 있는 벡터의 Y값
    private float PowerNormalizing(float upVector_y)
    {
        float angleNormalize = upVector_y;
        float min = liquidCreateAngle;
        float max = -1f;

        angleNormalize -= liquidCreateAngle;
        min -= liquidCreateAngle;
        max -= liquidCreateAngle;

        angleNormalize /= max;
        
        return angleNormalize;
    }
}
