using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Threading.Tasks;

namespace FlowFields
{
    [CustomEditor(typeof(FlowFieldGridData))]
    public class FlowFieldGridDataEditor : Editor
    {
        FlowFieldGridData flowFieldGridData { get { return target as FlowFieldGridData; } }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.BeginHorizontal();
            FlowFieldGridData.GridController = EditorGUILayout.ObjectField(FlowFieldGridData.GridController, typeof(GridController), true) as GridController;
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Generate"))
            {
                if (FlowFieldGridData.GridController != null)
                {
                    GenerateFlowFields(FlowFieldGridData.GridController);
                    EditorUtility.SetDirty(FlowFieldGridData.GridController);
                }
                // Debug.Log(AssetDatabase.GetAssetPath(flowFieldGridData));

            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Delete Data"))
            {
                flowFieldGridData.Grid = null;
                flowFieldGridData.DataLocked = false;
                flowFieldGridData._generatedDataInside = false;
                EditorUtility.SetDirty(flowFieldGridData);
            }
            EditorGUILayout.EndHorizontal();

            base.OnInspectorGUI();
        }

        public async void GenerateFlowFields(GridController gridController)
        {
            if (flowFieldGridData.DataLocked)
            {
                Debug.Log("Can't generate data. Data is Locked");
                return;
            }
            flowFieldGridData.DataLocked = true;
            flowFieldGridData._generatedDataInside = false;
            // Get Diameters from "GridController"
            flowFieldGridData.Offset = new Vector3(gridController.transform.position.x, gridController.transform.position.y, gridController.transform.position.z);
            flowFieldGridData.GridSize = new Vector3Int(gridController.GridSize.x, gridController.GridSize.y, gridController.GridSize.z);
            flowFieldGridData.cellDiameter = gridController.CellRadius * 2f;
            flowFieldGridData.cellRadius = gridController.CellRadius;
            // Generate FlowField Grid
            flowFieldGridData.Grid = new FlowFieldData[gridController.GridSize.x * gridController.GridSize.y * gridController.GridSize.z];
            //for (int y = 0; y < gridController.GridSize.y; y++)
            //{
            //    for (int z = 0; z < gridController.GridSize.z; z++)
            //    {
            //        for (int x = 0; x < gridController.GridSize.x; x++)
            //        {
            //            flowFieldGridData.Grid[GetGridIndex(x,y,z)] = new FlowFieldData();
            //        }
            //    }
            //}

            // Generate All the FlowFields
            for (int y = 0; y < gridController.GridSize.y; y++)
            {
                await Task.Delay(flowFieldGridData.waitSpeed);
                if (Application.isPlaying) return;
                Debug.Log($"Started on Layer:{y + 1}");
                for (int z = 0; z < gridController.GridSize.z; z++)
                {
                    for (int x = 0; x < gridController.GridSize.x; x++)
                    {
                        await Task.Delay(flowFieldGridData.waitSpeed);
                        // Create new Grid
                        FlowField field = new FlowField(flowFieldGridData.GridSize, flowFieldGridData.cellRadius, flowFieldGridData.Offset);
                        field.CreateGrid();

                        // Create CostField
                        field.CreateCostField(gridController.TerrainMask);

                        // Target Cell
                        Cell destinationCell = field.Grid[x, y, z];

                        // Generate Integrations from Target Cell
                        field.CreateIntegrationField(destinationCell);

                        // Create FlowField Directions
                        field.CreateFlowField();

                        flowFieldGridData.Grid[GetGridIndex(x, y, z)] = new FlowFieldData(field);
                        //Debug.Log($"Completed Field {x},{y},{z}");
                    }
                }
            }
            flowFieldGridData._generatedDataInside = true;
            //AssetDatabase.CreateAsset(flowFieldGridData ,AssetDatabase.GetAssetPath(flowFieldGridData));
            EditorUtility.SetDirty(flowFieldGridData);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("Done");
            return;
        }

        private int GetGridIndex(int x, int y, int z)
        {
            return (z * flowFieldGridData.GridSize.x * flowFieldGridData.GridSize.y) + (y * flowFieldGridData.GridSize.x) + x;
        }
    }
}
