using System;
using System.Collections.Generic;
using UnityEngine;

namespace PathFinding
{
    [System.Serializable]
    public class PathNode
    {
        public bool IsSolid = false;
        public bool IsGround = false;

        public bool IsClosed = false;

        public int howFarFromGround = 0;

        public float PathCost => howFarFromGround * 100f;

        [NonSerialized]
        public List<PathNode> Neighbors = new List<PathNode>();

        public Vector3 Position = new Vector3(0,0,0);

        public float DistanceToTarget = 0;
        public float DistanceToStart = 0;
        public float CombinedValue => DistanceToTarget + DistanceToStart;

        public PathNode PrevNode;

        public List<PathNode> GetPath(List<PathNode> nodes, PathNode Start)
        {
            if (nodes.Contains(this))
            {
                Debug.LogWarning("Pathfinding looped back, creating Stack overflow");
                return nodes;
            }
            nodes.Add(this);

            //Debug.Log(CombinedValue);
            //Debug.Log(Position);
            //Debug.Log(IsSolid); 
            if (Start == this)
                return nodes;

            //Neighbors.Sort((x, y) => x.CombinedValue.CompareTo(y.CombinedValue));

            //return Neighbors[0].GetPath(nodes, Start);

            return PrevNode.GetPath(nodes, Start);
        }
    }
}
