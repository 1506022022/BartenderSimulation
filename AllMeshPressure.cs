using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using Unity.VisualScripting;
using UnityEngine;

public class AllMeshPressure : MonoBehaviour
{
    Vector3 beforeRot, beforePos;

    Mesh _mesh;
    Vector3[] _vertices;
    Dictionary<float, List<Vector3>> _layerVertices;
    Mesh _prefabMesh;
    public float ThresholdY = 5.0f;
    public Vector2 RangeY = new Vector2();
    [SerializeField] GameObject _vertexPrefab;
    [SerializeField] Mesh _originMesh;
    private void Awake()
    {
        _mesh = GetComponent<MeshFilter>()?.mesh;
        _vertices = _mesh.vertices;

        Debug.Assert(_mesh != null, "Mesh를 추가해주세요.");
        PrefabMeshCreate();
        LoweringPrecision();
        Layering();
    }
    void Update()
    {
        PrefabMeshCreate();
        if (beforePos != transform.position || beforeRot != transform.eulerAngles)
        {
            beforePos = transform.position;
            beforeRot = transform.eulerAngles;
            for (int i = 0; i < _prefabMesh.vertices.Length; i++)
            {
                _prefabMesh.vertices[i] = transform.rotation * _prefabMesh.vertices[i] + transform.position;
                _prefabMesh.vertices[i].y = Mathf.Clamp(Floor(_prefabMesh.vertices[i].y, 1), _prefabMesh.vertices.Min(x => x.y), _prefabMesh.vertices.Max(x => x.y));
            }
            Layering();
        }
        ClampY();
        Pressure();
    }

    private void PrefabMeshCreate()
    {
        _prefabMesh = Instantiate(_originMesh);
    }


    private void ClampY()
    {
        if (RangeY == Vector2.zero)
            ThresholdY = Mathf.Clamp(ThresholdY, _layerVertices.Min(x => x.Key), _layerVertices.Max(x => x.Key));
        else
            ThresholdY = Mathf.Clamp(ThresholdY, RangeY.x, RangeY.y);
    }

    float Round(float originalNumber, float numberOfDigits)
    {
        float roundedNumber = Mathf.Round(originalNumber * Mathf.Pow(10, numberOfDigits)) / Mathf.Pow(10, numberOfDigits);
        return roundedNumber;
    }
    float Floor(float originalNumber, float numberOfDigits)
    {
        float flooredNumber = Mathf.Floor(originalNumber * Mathf.Pow(10, numberOfDigits)) / Mathf.Pow(10, numberOfDigits);
        return flooredNumber;
    }
    void Pressure()
    {
        Vector3[] copyVertices = _prefabMesh.vertices.ToArray();
        bool isFlat = _prefabMesh.vertices.Where(x => x.y <= ThresholdY).Count() == 0;

        if (_mesh == null) return;
        if (isFlat) return;

        float upSideLayer = _prefabMesh.vertices.Max(x => x.y);
        if (_vertices.Where(x => x.y > ThresholdY).Count() > 0)
            upSideLayer = _prefabMesh.vertices.Where(x => x.y > ThresholdY).Min(x => x.y);
        var targetLayer = _layerVertices.Where(x => x.Key <= ThresholdY).Max(x => x.Key);
        var targetLayerVertices = _layerVertices[targetLayer];

        for (int i = 0; i < copyVertices.Length; i++)
        {
            if (copyVertices[i].y > ThresholdY)
            {
                var vertex = copyVertices[i];

                var upLayerVertices = _prefabMesh.vertices.Where(x => x.y == upSideLayer);
                var upLayerMinDistance = upLayerVertices.Min(x => Vector3.Distance(vertex, x));
                var upLayerMinDistanceVector = upLayerVertices.Where(x => Vector3.Distance(vertex, x) == upLayerMinDistance).First();
                vertex = upLayerMinDistanceVector;

                var minDistance = targetLayerVertices.Min(x => Vector3.Distance(vertex, x));
                var minDistanceVector = targetLayerVertices.Where(x => Vector3.Distance(vertex, x) == minDistance).First();
                float lerpT = Mathf.Abs(vertex.y - ThresholdY) / Mathf.Abs(vertex.y - minDistanceVector.y);
                var lerpVector = Vector3.Lerp(vertex, minDistanceVector, lerpT);

                copyVertices[i].y = ThresholdY;
                copyVertices[i].x = lerpVector.x;
                copyVertices[i].z = lerpVector.z;
            }
        }

        _mesh.vertices = copyVertices;
        _mesh.RecalculateBounds();
    }
    [ContextMenu("d")]
    void CreateShowVerticesModel()
    {
        List<Vector3> unique = new List<Vector3>();
        Mesh newMesh = new Mesh();
        newMesh.vertices = _vertices;
        newMesh.triangles = _mesh.triangles;
        GameObject obj = new GameObject("버텍스 모델");

        Debug.Assert(_vertices != null && _vertices.Length > 0, "vertices를 먼저 할당해주세요.");

        obj.AddComponent<MeshRenderer>();
        obj.AddComponent<MeshFilter>().mesh = newMesh;
        int i = 0;
        foreach (var item in _layerVertices)
        {
            foreach (var vertex in item.Value)
            {
                if (!unique.Any(x => x.Equals(vertex)))
                {
                    unique.Add(vertex);
                    var abc = Instantiate(_vertexPrefab, obj.transform);
                    abc.transform.SetLocalPositionAndRotation(vertex, Quaternion.identity);
                    abc.GetComponent<MeshRenderer>().material.color = i % 2 == 0 ? Color.black : Color.red;
                }
            }
            i++;
        }
    }
    /// <summary>
    /// 정밀도 저하
    /// </summary>
    void LoweringPrecision()
    {
        Debug.Assert(_originMesh.vertices != null && _originMesh.vertices.Length > 0, "vertices를 먼저 할당해주세요.");

        for (int i = 0; i < _originMesh.vertices.Length; i++)
        {
            _originMesh.vertices[i].y = Round(_originMesh.vertices[i].y, 2);
            _originMesh.vertices[i].y = _vertices[i].y > 1.5f ? 1.56f : _originMesh.vertices[i].y;
            _originMesh.vertices[i].x = Round(_originMesh.vertices[i].x, 2);
            _originMesh.vertices[i].z = Round(_originMesh.vertices[i].z, 2);
        }
    }
    /// <summary>
    /// 계층화
    /// </summary>
    void Layering()
    {
        _layerVertices = new Dictionary<float, List<Vector3>>();
        var list = _prefabMesh.vertices?.OrderBy(x => x.y).ToList();
        var unique = new List<Vector3>();

        Debug.Assert(_prefabMesh.vertices != null && _prefabMesh.vertices.Length > 0, "vertices를 먼저 할당해주세요.");

        foreach (var item in list)
        {
            if (unique.Any(x => x.Equals(item))) continue;

            unique.Add(item);
            if (_layerVertices.TryGetValue(item.y, out List<Vector3> e))
                e.Add(item);
            else
                _layerVertices.Add(item.y, new List<Vector3>() { item });
        }
    }
}
