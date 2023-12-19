using UnityEngine;

namespace FlowFields
{
    [System.Serializable]
    public struct FlowFieldData
    {
        public Vector3Int GridSize;
        public Vector3[] _bestDirectionVector;

        public Vector3 GetDirection(int x, int y,int z)
        {

            return _bestDirectionVector[GetGridIndex(x,y,z,GridSize)];
        }

        public FlowFieldData(FlowField field)
        {
            GridSize = field.GridSize;
            //_bestDirectionFloat = new int[field.GridSize.x, field.GridSize.y, field.GridSize.z, 3];
            _bestDirectionVector = new Vector3[GetGridIndex(field.GridSize.x, field.GridSize.y, field.GridSize.z, field.GridSize)];
            for (int y = 0; y < field.GridSize.y; y++)
                for (int z = 0; z < field.GridSize.z; z++)
                    for (int x = 0; x < field.GridSize.x; x++)
                    {
                        _bestDirectionVector[GetGridIndex(x, y, z, field.GridSize)] = (Vector3Int)field.Grid[x, y, z].BestDirection;
                    }
        }

        private static int GetGridIndex(int x, int y, int z, Vector3Int GridSize)
        {
            return (z * GridSize.x * GridSize.y) + (y * GridSize.x) + x;
        }
    }
}
