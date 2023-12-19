using UnityEngine;
using System;

namespace FlowFields
{
    [System.Serializable]
    public struct FlowFieldData
    {
        public Vector3Int GridSize;
        public byte[] _bestDirectionVector;

        public Vector3 GetDirection(int x, int y,int z)
        {
            Vector3 direction = new Vector3(
                Convert.ToInt32(_bestDirectionVector[GetGridIndex(x, y, z, 0, GridSize)] - 1),
                Convert.ToInt32(_bestDirectionVector[GetGridIndex(x, y, z, 1, GridSize)] - 1),
                Convert.ToInt32(_bestDirectionVector[GetGridIndex(x, y, z, 2, GridSize)] - 1)
                );
            return direction;
        }

        public FlowFieldData(FlowField field)
        {
            GridSize = field.GridSize;
            //_bestDirectionFloat = new int[field.GridSize.x, field.GridSize.y, field.GridSize.z, 3];
            _bestDirectionVector = new byte[GetGridIndex(field.GridSize.x, field.GridSize.y, field.GridSize.z, 3, field.GridSize)];
            for (int y = 0; y < field.GridSize.y; y++)
                for (int z = 0; z < field.GridSize.z; z++)
                    for (int x = 0; x < field.GridSize.x; x++)
                    {
                        Vector3 vec = (Vector3Int)field.Grid[x, y, z].BestDirection;
                        _bestDirectionVector[GetGridIndex(x, y, z,0,GridSize)] = Convert.ToByte(vec.x + 1);
                        _bestDirectionVector[GetGridIndex(x, y, z,1,GridSize)] = Convert.ToByte(vec.y + 1);
                        _bestDirectionVector[GetGridIndex(x, y, z,2,GridSize)] = Convert.ToByte(vec.z + 1);
                    }
        }

        private static int GetGridIndex(int x, int y, int z, int i, Vector3Int GridSize)
        {
            return  (i * GridSize.z * GridSize.x * GridSize.y) + (z * GridSize.x * GridSize.y) + (y * GridSize.x) + x;
        }
    }
}
