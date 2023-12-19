using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FlowFields
{
    [System.Serializable]
    public class Cell
    {
        [SerializeField]
        private Vector3 _worldPos;
        public Vector3 WorldPos { get { return _worldPos; } private set { _worldPos = value; } }

        [SerializeField]
        private Vector3Int _gridIndex;
        public Vector3Int GridIndex { get { return _gridIndex; } private set { _gridIndex = value; } }

        public short Cost;
        public ushort BestCost;
        public bool IsGround;

        public GridDirection BestDirection = GridDirection.None;

        public Cell(Vector3 worldPos, Vector3Int gridIndex)
        {
            WorldPos = worldPos;
            GridIndex = gridIndex;
            Cost = 1;
            BestCost = ushort.MaxValue;
        }

        public void IncreaseCost(int costIncrease)
        {
            if (Cost == short.MaxValue)
            {
                return; // Return if value already max
            }
            if (Cost + costIncrease >= short.MaxValue)
            {
                Cost = short.MaxValue;
            }
            else
            {
                Cost += (short)costIncrease;
            }
        }
    }
}
