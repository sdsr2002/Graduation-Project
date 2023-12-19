using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace FlowFields
{
    [CreateAssetMenu(fileName = "new GridData", menuName = "FlowField/GridData")]
    public class FlowFieldGridData : ScriptableObject
    {
        [HideInInspector]
        public FlowFieldData[] Grid;

        [HideInInspector]
        public Vector3 Offset;

        [HideInInspector]
        public Vector3Int GridSize;
        [HideInInspector]
        public float cellDiameter;
        [HideInInspector]
        public float cellRadius;
        [HideInInspector]
        public bool _generatedDataInside;

        [Header("Generating Grid")]
        [HideInInspector]
        public bool DataLocked;
        [HideInInspector]
        public static GridController GridController;
        [Range(1,1000)]
        public int waitSpeed = 10;

        public Vector3 GetCellDirectionFromWorldPosition(Vector3 worldPos, Vector3 targetPos)
        {
            // Gets Destination cell Index
            float percentX = (targetPos.x - Offset.x) / (GridSize.x * cellDiameter);
            float percentY = (targetPos.y - Offset.y) / (GridSize.y * cellDiameter);
            float percentZ = (targetPos.z - Offset.z) / (GridSize.z * cellDiameter);

            percentX = Mathf.Clamp01(percentX);
            percentY = Mathf.Clamp01(percentY);
            percentZ = Mathf.Clamp01(percentZ);

            int targetX = Mathf.Clamp(Mathf.FloorToInt((GridSize.x - 1) * percentX), 0, GridSize.x - 1);
            int targetY = Mathf.Clamp(Mathf.FloorToInt((GridSize.y - 1) * percentY), 0, GridSize.y - 1);
            int targetZ = Mathf.Clamp(Mathf.FloorToInt((GridSize.z - 1) * percentZ), 0, GridSize.z - 1);

            // Gets Destination cell Index
            percentX = (worldPos.x - Offset.x) / (GridSize.x * cellDiameter);
            percentY = (worldPos.y - Offset.y) / (GridSize.y * cellDiameter);
            percentZ = (worldPos.z - Offset.z) / (GridSize.z * cellDiameter);

            percentX = Mathf.Clamp01(percentX);
            percentY = Mathf.Clamp01(percentY);
            percentZ = Mathf.Clamp01(percentZ);

            int x = Mathf.Clamp(Mathf.FloorToInt((GridSize.x - 1) * percentX), 0, GridSize.x - 1);
            int y = Mathf.Clamp(Mathf.FloorToInt((GridSize.y - 1) * percentY), 0, GridSize.y - 1);
            int z = Mathf.Clamp(Mathf.FloorToInt((GridSize.z - 1) * percentZ), 0, GridSize.z - 1);
            //Debug.Log(Grid[GetGridIndex(targetX, targetY, targetZ)]);
            // Get targets "Flowfield" then your "Cell" from that "FlowField"
            return Grid[GetGridIndex(targetX, targetY, targetZ)].GetDirection(x,y,z).normalized;
            //return Grid[GetIndex(targetX, targetY, targetZ)].Grid[GetIndex(x, y, z)];
        }

        private int GetGridIndex(int x, int y, int z)
        {
            return (z * GridSize.x * GridSize.y) + (y * GridSize.x) + x;
        }

    }
}

