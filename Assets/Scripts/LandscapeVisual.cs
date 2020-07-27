using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LandscapeCreator;

public class LandscapeVisual : MonoBehaviour
{
    [SerializeField] private int _seed;
    [SerializeField] private Vector2Int _size;
    private int _dispersion;
    [HideInInspector] public List<GameObject> CellPrefab;
    [SerializeField] private float _scale;
    [SerializeField] private Transform _prefabsParent;

    public void Build()
    {
        _dispersion = CellPrefab.Count - 1;
        var children = _prefabsParent.GetComponentsInChildren<Transform>();
        foreach (var child in children)
        {
            if (child != _prefabsParent) { DestroyImmediate(child.gameObject); }
        }
        var creator = new LandscapeGenerator(_size, _dispersion, _scale, _seed);
        creator.Build();

        for (int x = 0; x < _size.x; x++)
        {
            for (int y = 0; y < _size.y; y++)
            {
                Debug.Log(creator.HeightMap[x, y]);
                var position = new Vector3(x, creator.HeightMap[x, y], y);
                if (CellPrefab[creator.HeightMap[x, y]] != null)
                {
                    var cell = Instantiate(CellPrefab[creator.HeightMap[x, y]], position, Quaternion.identity);
                    cell.transform.parent = _prefabsParent;
                }

            }
        }
    }
}
