using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LandscapeCreator
{
    public class LandscapeGenerator
    {
        private int _seed;
        private Vector2Int _size;
        private int[,] _heightMap;
        private int _dispersion;
        float _scale;

        public int[,] HeightMap { get => _heightMap; private set => _heightMap = value; }

        public LandscapeGenerator(Vector2Int size, int dispersion, float scale, int seed)
        {
            _seed = seed;
            _size = new Vector2Int(size.x, size.y);
            _dispersion = dispersion;
            _heightMap = new int[_size.x, _size.y];
            _scale = scale;
        }

        public void Build()
        {
            for (int x = 0; x < _size.x; x++)
            {
                for (int y = 0; y < _size.y; y++)
                {
                    _heightMap[x, y] = Mathf.RoundToInt(Mathf.PerlinNoise((x + _seed) * _scale, (y + _seed) * _scale) * _dispersion);
                }
            }
        }

    }

}

