using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class Rimmer : MonoBehaviour
{
    [SerializeField] List<GameObject> _layers;
    Interactable interactable;
    int _currentLayer;
    bool _isActionDelay;
    public float _OpenAngel = 180;
    private void Awake()
    {
        if (_layers == null || _layers.Count == 0)
        {
            AddItemsToLayers();
        }
        foreach (var layer in _layers)
        {
            layer.GetComponents<Collider>().Where(x => x.isTrigger).ForEach(x => x.enabled = false);
        }
        interactable = GetComponent<Interactable>();
    }

    private void OnTriggerStay(Collider other)
    {
        var manager = other.GetComponent<GameObjectManager>();
        if (manager == null || ID.GetGroup(manager.GetID) != ID.ItemGroup.Glass) return;
        var materials = other.GetComponent<MeshRenderer>().materials;
        foreach (var material in materials)
        {
            if (material.shader.name == "Shader Graphs/SaltEffect")
            {
                AddEffect(material);
            }
        }
    }

    void AddEffect(Material material)
    {
        float effect = material.GetFloat("_Effect");
        effect = Mathf.Clamp(effect + 0.01f, 0, 1);
        material.SetFloat("_Effect", effect);
    }
    private void Update()
    {

        var hand = interactable.attachedToHand;
        bool isInput = false;
        if (hand != null)
        {
            isInput = hand.grabGripAction.GetState(hand.handType);

        }
        if (isInput && !_isActionDelay)
        {
            NextLayer();
            StartCoroutine(ActionDelay());
        }
        if (_isActionDelay)
        {
            _isActionDelay = false;
            for (int i = 0; i < _layers.Count; i++)
            {
                float closeAngle = (i * 90) % 360;
                float OpenAngleOrClose = _OpenAngel * (i == _currentLayer ? 1 : 0);
                float currentZRot = _layers[i].transform.localEulerAngles.z;
                float TargetZRot = (closeAngle + OpenAngleOrClose) % 360;

                if (Mathf.Abs(currentZRot - TargetZRot) > 0.1)
                {
                    _isActionDelay = true;
                    if (TargetZRot < currentZRot) TargetZRot += 360;
                    Rotate(i, TargetZRot, currentZRot);
                }
            }
        }

        void Rotate(int i, float RotZ, float currentZRot)
        {
            if (Mathf.Abs(RotZ - currentZRot) < 7)
            {

                _layers[i].transform.localEulerAngles = new Vector3(0, 0, RotZ);
            }
            else
            {
                _layers[i].transform.Rotate(0, 0, 6, Space.Self);
            }
        }
    }

    void NextLayer()
    {
        _layers[_currentLayer].GetComponents<Collider>().Where(x => x.isTrigger).ForEach(x => x.enabled = false);
        _currentLayer++;
        if (_currentLayer >= _layers.Count) _currentLayer = 0;
        _layers[_currentLayer].GetComponents<Collider>().Where(x => x.isTrigger).ForEach(x => x.enabled = true);
    }
    IEnumerator ActionDelay()
    {
        _isActionDelay = true;
        while (_isActionDelay) yield return null;
    }
    private void AddItemsToLayers()
    {
        _layers = new List<GameObject>();
        for (int i = 0; i < transform.childCount; i++)
        {
            _layers.Add(transform.GetChild(i).gameObject);
        }
    }
}
