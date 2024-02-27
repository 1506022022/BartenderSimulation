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

    [Tooltip("��ü ��ƼŬ �ּ� ��ȯ ����")]
    [SerializeField] private float minLiquidCreateCount;

    [Tooltip("��ü ��ƼŬ �ִ� ��ȯ ����")]
    [SerializeField] private float maxLiquidCreateCount;

    [Tooltip("������ ��� ���� ��￴�� �� ��ü�� �帣�� �� ���ΰ� (-1 : �ٴڹ���, 1 : �ϴù���) ")]
    [Range(-1f, 1f)]
    [SerializeField] private float liquidCreateAngle;

    [Tooltip("��ü�� ������ �ӷ��� �ּ� ��")]
    [SerializeField] private float liquidMinPower;
    [Tooltip("��ü�� ������ �ӷ��� �ִ� ��")]
    [SerializeField] private float liquidMaxPower;

    [SerializeField] private Color liquidColor;

    [SerializeField] private MeshPressure liquidMesh;

    [Tooltip("Zibra Liquid �� ����� ũ�� ( 1 : ���� �� ���� )")]
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

            // ��ü ��ƼŬ�� ��ȯ�� ��ŭ ��ü ���� ����
            LiquidDrop();
        }
        
        

        // ���⿡ ���� ��ü�� �ӵ�
        void LiquidVelocitySetting()
        {
            var upMaxVector = Vector3.zero;
            upMaxVector.y = liquidMaxPower;

            var upMinVector = Vector3.zero;
            upMinVector.y = liquidMinPower;

            var liquidPowerNormal = PowerNormalizing(transform.up.y);

            zibraLiquidEmitter.InitialVelocity = Vector3.Lerp(upMinVector, upMaxVector, liquidPowerNormal);
        }

        // ��ü�� �帣�� �����ΰ� üũ
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

    // �� ��ȯ �Լ�
    private void ColorChange(Color color)
    {
        zibraLiquidMeshRenderer.materials = liquidMeshRenderer.materials;
    }

    // Y���͸��� ������ liquidCreateAngle�� ������ ���� 0��1 ������ ������ ��ȯ�ϴ� �븻������ �Լ�
    // �Է¹޴� ���� �� = �ٶ󺸰� �ִ� ������ Y��
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
