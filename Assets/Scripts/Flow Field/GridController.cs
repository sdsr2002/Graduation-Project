using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace FlowFields
{
    public class GridController : MonoBehaviour
    {
#if UNITY_EDITOR
        [SerializeField]
        private Vector3Int _gridSize = new Vector3Int(10, 10, 10);
        [SerializeField]
        private float _cellRadius = 0.5f;
        [SerializeField]
        private FlowField curFlowField;

        public Vector3Int GridSize { get { return _gridSize; } private set { _gridSize = value; } }
        public float CellRadius { get { return _cellRadius; } private set { _cellRadius = value; } }
        public LayerMask TerrainMask { get { return _terrainMask; } private set { _terrainMask = value; } }

        public FlowField FlowField { get { return curFlowField; } }

        [SerializeField]
        private LayerMask _terrainMask = 0 << 8;
        private void InitializeFlowField()
        {
            curFlowField = new FlowField(GridSize, CellRadius, transform.position);
            curFlowField.CreateGrid();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                InitializeFlowField();
                curFlowField.CreateCostField(_terrainMask);

                Vector3 mousePos = Input.mousePosition;

                Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 50f));

                Cell destinationCell = curFlowField.GetCellFromWorldPosition(mouseWorldPos);

                curFlowField.CreateIntegrationField(destinationCell);

                curFlowField.CreateFlowField();
            }
        }

        public void GenerateFlowField(Vector3Int TargetCellIndex)
        {
            // Create new Grid
            InitializeFlowField();
            curFlowField.CreateCostField(TerrainMask);

            // Target Cell
            Cell destinationCell = curFlowField.Grid[TargetCellIndex.x, TargetCellIndex.y, TargetCellIndex.z];

            // Generate Integrations from Target Cell
            curFlowField.CreateIntegrationField(destinationCell);

            // Create FlowField Directions
            curFlowField.CreateFlowField();
        }

        [SerializeField, Header("Debug")]
        private DebugMode _debugMode;

        private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(transform.position, _cellRadius);
            for (int i = 2; i <= (GridSize.x * 2); i+=2)
            {
                Gizmos.DrawSphere(transform.position + Vector3.right * i * _cellRadius, _cellRadius);
            }

            for (int i = 2; i <= (GridSize.z * 2); i += 2)
            {
                Gizmos.DrawSphere(transform.position + Vector3.forward * i * _cellRadius, _cellRadius);
            }

            for (int i = 2; i <= (GridSize.y * 2); i += 2)
            {
                Gizmos.DrawSphere(transform.position + Vector3.up * i * _cellRadius, _cellRadius);
            }

            Gizmos.DrawLine(transform.position, transform.position + new Vector3(0, 0, _gridSize.z * _cellRadius * 2));
            Gizmos.DrawLine(transform.position , transform.position + new Vector3(_gridSize.x * _cellRadius * 2, 0, 0));
            Gizmos.DrawLine(transform.position , transform.position + new Vector3(0, _gridSize.y * _cellRadius * 2, 0));

            if (!Application.isPlaying || _debugMode == DebugMode.None) return;
            if (curFlowField != null)
                for (int y = 0; y < GridSize.y; y++)
                    for (int z = 0; z < GridSize.z; z++)
                        for (int x = 0; x < GridSize.x; x++)
                        {
                            //Handles.Label(curFlowField.Grid[x,y,z].WorldPos, "Best Cost:" + curFlowField.Grid[x, y, z].BestCost.ToString());
                            //Handles.Label(curFlowField.Grid[x,y,z].WorldPos + new Vector3(0,0.5f,0), "Cost:" + curFlowField.Grid[x, y, z].Cost.ToString());
                            switch (_debugMode)
                            {
                                case DebugMode.BestCost:
                                    if (curFlowField.Grid[x, y, z].Cost == short.MaxValue) Gizmos.color = Color.black;
                                    else if (curFlowField.Grid[x, y, z].BestCost > 1000f) Gizmos.color = Color.red;
                                    else if (curFlowField.Grid[x, y, z].BestCost > 250f) Gizmos.color = Color.yellow;
                                    else if (curFlowField.Grid[x, y, z].BestCost > 20f) Gizmos.color = Color.green;
                                    else Gizmos.color = Color.white;
                                    break;
                                case DebugMode.Cost:
                                    if (curFlowField.Grid[x, y, z].Cost == short.MaxValue) Gizmos.color = Color.black;
                                    else if (curFlowField.Grid[x, y, z].Cost > 1000f) Gizmos.color = Color.red;
                                    else if (curFlowField.Grid[x, y, z].Cost > 100f) Gizmos.color = Color.yellow;
                                    else Gizmos.color = Color.white;
                                    break;
                                case DebugMode.Numbers:
                                    Handles.Label(curFlowField.Grid[x, y, z].WorldPos, "Best Cost:" + curFlowField.Grid[x, y, z].BestCost.ToString());
                                    Handles.Label(curFlowField.Grid[x, y, z].WorldPos + new Vector3(0, 0.5f, 0), "Cost:" + curFlowField.Grid[x, y, z].Cost.ToString());
                                    break;
                            }
                            if (_debugMode != DebugMode.Numbers)
                            {
                                if (curFlowField.Grid[x, y, z].BestCost != 0f)
                                    Gizmos.DrawLine(curFlowField.Grid[x, y, z].WorldPos, curFlowField.Grid[x, y, z].WorldPos + ((Vector3)(Vector3Int)curFlowField.Grid[x, y, z].BestDirection * CellRadius) * 2f);
                                else
                                    Gizmos.DrawSphere(curFlowField.Grid[x, y, z].WorldPos, CellRadius);
                            }

                        }
        }

        private enum DebugMode
        {
            None,
            Cost,
            BestCost,
            Numbers
        }
#endif
    }
}
