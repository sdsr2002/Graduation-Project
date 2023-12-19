using System;
using System.Collections.Generic;
using UnityEngine;

namespace FlowFields
{
    [System.Serializable]
    public class FlowField
    {
        public Cell[,,] Grid {  get; private set; }
        public Vector3Int GridSize {  get; private set; }
        public float CellRadius {  get; private set; }

        public Cell DestinationCell;

        public Vector3 Offset { get; private set; }

        private float cellDiameter;

        public FlowField(Vector3Int gridSize, float cellRadius, Vector3 offset)
        {
            GridSize = gridSize;
            CellRadius = cellRadius;
            cellDiameter = cellRadius * 2f;
            Offset = offset;
        }

        public void CreateGrid()
        {
            Grid = new Cell[GridSize.x, GridSize.y, GridSize.z];

            for (int y = 0; y < GridSize.y; y++)
            {

                for (int z = 0; z < GridSize.z; z++)
                {

                    for (int x = 0; x < GridSize.x; x++)
                    {
                        Vector3 worldPos = new Vector3(
                            cellDiameter * x + CellRadius + Offset.x,
                            cellDiameter * y + CellRadius + Offset.y,
                            cellDiameter * z + CellRadius + Offset.z
                            );

                        Grid[x, y, z] = new Cell(worldPos, new Vector3Int(x, y, z));
                    }
                }
            }

        }

        public void CreateCostField(LayerMask terrainMask)
        {
            Vector3 cellHalfExtents = Vector3.one * CellRadius;
            int b = 0;
            Cell curCell;
            for (int y = 0; y < GridSize.y; y++)
                for (int z = 0; z < GridSize.z; z++)
                    for (int x = 0; x < GridSize.x; x++)
                    {
                        curCell = Grid[x, y, z];

                        if (y != 0)
                        {
                            b = 0;
                            for(int i = y; i >= 0; i--)
                            {
                                if (b > 3)
                                {
                                    curCell.IncreaseCost(Mathf.FloorToInt(short.MaxValue * 0.75f));
                                    break;
                                }

                                if (Grid[x, i, z].IsGround || i == 0)
                                {
                                    curCell.IncreaseCost((int)Mathf.Pow(5,b));
                                    break;
                                }
                                b++;
                            }
                        }
                        else
                        {
                            curCell.IsGround = true;
                        }

                        Collider[] obstacles = Physics.OverlapSphere(curCell.WorldPos, CellRadius, terrainMask);
                        //bool hasIncreasedCost = false;
                        foreach(Collider col in obstacles)
                        {
                            if (col.gameObject.layer == 0)
                            {
                                curCell.IncreaseCost(short.MaxValue);
                                curCell.IsGround = true;
                                continue;
                            }
                            //else if (!hasIncreasedCost && col.gameObject.layer == 0)
                            //{
                            //    curCell.IncreaseCost(3);
                            //    hasIncreasedCost = true;
                            //}
                        }
                    }
        }

        public void CreateIntegrationField(Cell _destinationCell)
        {
            DestinationCell = _destinationCell;

            DestinationCell.Cost = 0;
            DestinationCell.BestCost = 0;

            Queue<Cell> cellsToCheck = new Queue<Cell>();

            cellsToCheck.Enqueue(DestinationCell);

            while(cellsToCheck.Count > 0)
            {
                Cell curCell = cellsToCheck.Dequeue();

                List<Cell> curNeighbors = GetNeighborCells(curCell.GridIndex, GridDirection.CardinalDirections);
                foreach(Cell curNeighbor in curNeighbors)
                {
                    if (curNeighbor.Cost == short.MaxValue)
                    {
                        continue;
                    }
                    if (curNeighbor.Cost + curCell.BestCost < curNeighbor.BestCost)
                    {
                        curNeighbor.BestCost = (ushort)(curNeighbor.Cost + curCell.BestCost);
                        cellsToCheck.Enqueue(curNeighbor);
                    }

                }
            }

        }

        public void CreateFlowField()
        {
            foreach(Cell curCell in Grid)
            {
                List<Cell> curNeighbors = GetNeighborCells(curCell.GridIndex, GridDirection.AllDirections);
                int bestCost = curCell.BestCost;
                foreach(Cell curNeighborCell in curNeighbors)
                {
                    if (curNeighborCell.BestCost < bestCost)
                    {
                        bestCost = curNeighborCell.BestCost;
                        curCell.BestDirection = GridDirection.GetDirectionFromV3I(curNeighborCell.GridIndex - curCell.GridIndex);
                    }
                }
            }
        }

        #region Helper Methods
        private List<Cell> GetNeighborCells(Vector3Int nodeIndex, List<GridDirection> cardinalDirections)
        {
            List<Cell> neighborCells = new List<Cell>();

            foreach(Vector3Int curDirection in cardinalDirections)
            {
                Cell newNeighbor = GetCellAtRelativePos(nodeIndex, curDirection);
                if (newNeighbor != null)
                {
                    neighborCells.Add(newNeighbor);
                }
            }
            return neighborCells;
        }

        private Cell GetCellAtRelativePos(Vector3Int nodeIndex, Vector3Int curDirection)
        {
            Vector3Int finalPos = nodeIndex + curDirection;

            if (finalPos.x < 0 || finalPos.x >= GridSize.x || finalPos.y < 0 || finalPos.y >= GridSize.y || finalPos.z < 0 || finalPos.z >= GridSize.z)
            {
                return null;
            }
            else
            {
                return Grid[finalPos.x, finalPos.y, finalPos.z];
            }
        }

        public Cell GetCellFromWorldPosition(Vector3 worldPos)
        {
            float percentX = (worldPos.x - Offset.x) / (GridSize.x * cellDiameter);
            float percentY = (worldPos.y - Offset.y) / (GridSize.y * cellDiameter);
            float percentZ = (worldPos.z - Offset.z) / (GridSize.z * cellDiameter);

            percentX = Mathf.Clamp01(percentX);
            percentY = Mathf.Clamp01(percentY);
            percentZ = Mathf.Clamp01(percentZ);

            int x = Mathf.Clamp(Mathf.FloorToInt((GridSize.x) * percentX), 0, GridSize.x - 1);
            int y = Mathf.Clamp(Mathf.FloorToInt((GridSize.y) * percentY), 0, GridSize.y - 1);
            int z = Mathf.Clamp(Mathf.FloorToInt((GridSize.z) * percentZ), 0, GridSize.z - 1);
            return Grid[x, y, z];
        }

        internal void OverideGrid(Cell[,,] grid)
        {
            Grid = grid;
        }

        //internal Cell GetCellFromWorldPosition(Vector3 mouseWorldPos)
        //{

        //    Cell returnCell = null;
        //    float shortestDistance = float.MaxValue;

        //    foreach (Cell curCell in Grid)
        //    {
        //        float newDistance = Vector3.Distance(curCell.WorldPos, mouseWorldPos);
        //        if (newDistance < shortestDistance)
        //        {
        //            returnCell = curCell;
        //            shortestDistance = newDistance;
        //        }
        //    }

        //    return returnCell;
        //}
        #endregion

    }

}

