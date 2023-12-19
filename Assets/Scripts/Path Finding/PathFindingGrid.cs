using System.Collections.Generic;
using UnityEngine;

namespace PathFinding
{
    public class PathFindingGrid : MonoBehaviour
    {
        private void Awake()
        {
            CreateGrid();
#if UNITY_EDITOR
            DebugGetPath();
#endif
        }

        #region Creating Grid
        [SerializeField]
        private Vector3Int _gridSize = new Vector3Int(3,3,3);

        private List<PathNode> _allNodes = new List<PathNode>();

        private void CreateGrid()
        {
            _allNodes = new List<PathNode>();

            for (int i = 0; i < _gridSize.x * _gridSize.y * _gridSize.z; i++)
            {
                _allNodes.Add(new PathNode());
            }

            for (int y = 0; y < _gridSize.y; y++)
            {
                for (int z = 0; z < _gridSize.z; z++)
                {
                    for (int x = 0; x < _gridSize.x; x++)
                    {
                        PathNode node = GetNode(x, y, z);
                        if (Physics.CheckBox(transform.position + new Vector3(x,y,z),Vector3.one * 0.4f))
                        {
                            node.IsSolid = true;
                            node.IsGround = true;
                            node.Position = new Vector3Int(x, y, z) + transform.position;
                            continue;
                        }
                        else if (y == 0)
                        {
                            node.IsGround = true;
                        }
                        else
                        {
                            for(int gy = 1; gy < y + 1; gy++)
                            {
                                if(GetNode(x, y - gy, z).IsGround)
                                {
                                    node.howFarFromGround = gy;
                                    break;
                                }
                            }
                        }
                        node.Position = new Vector3Int(x, y, z) + transform.position;
                        AssignNeighbor(x, y, z);
                    }
                }
            }
        }

        private void AssignNeighbor(int x, int y, int z)
        {
            PathNode thatNode = GetNode(x,y,z);
            List<PathNode> newNeighbor = new List<PathNode>();

            for (int ax = -1; ax < 2; ax++)
            {
                for (int ay = -1; ay < 2; ay++)
                {
                    for (int az = -1; az < 2; az++)
                    {
                        if (x + ax >= _gridSize.x || x + ax < 0) { continue; }
                        if (y + ay >= _gridSize.y || y + ay < 0) { continue; }
                        if (z + az >= _gridSize.z || z + az < 0) { continue; }
                        
                        //if (ax ==  1 && az ==  1) continue;
                        //if (ax == -1 && az ==  1) continue;
                        //if (ax ==  1 && az == -1) continue;
                        //if (ax == -1 && az == -1) continue;

                        if (ax == 1 && ay == 1) continue;
                        if (ax == -1 && ay == 1) continue;
                        if (ax == 1 && ay == -1) continue;
                        if (ax == -1 && ay == -1) continue;

                        if (ay == 1 && az == 1) continue;
                        if (ay == -1 && az == 1) continue;
                        if (ay == 1 && az == -1) continue;
                        if (ay == -1 && az == -1) continue;

                        PathNode node = GetNode(x + ax, y + ay, z + az);
                        if (!node.IsSolid)
                            newNeighbor.Add(node);

                    }
                }
            }
            thatNode.Neighbors = newNeighbor;
        }
        #endregion

        public List<PathNode> FindPath(Vector3Int originNodePosition, Vector3Int destinationNodePosition)
        {
            List<PathNode> allNodes = new List<PathNode>(_allNodes);
            PathNode DestinationNode = GetNode(destinationNodePosition, ref allNodes);

            List<PathNode> openNodes = new List<PathNode>() { GetNode(originNodePosition, ref allNodes) };

            for (int i = 0; i < allNodes.Count; i++)
            {
                allNodes[i].IsClosed = false;
            }

            while (openNodes.Count > 0)
            {
                // TODO: Optimize Sort
                //openNodes.Sort((x, y) => x.CombinedValue.CompareTo(y.CombinedValue));

                int index = 0;
                for(int i = 0; i < openNodes.Count; i++)
                {
                    if (openNodes[i].CombinedValue < openNodes[index].CombinedValue)
                        index = i;
                }

                //

                PathNode currentNode = openNodes[index];
                openNodes.RemoveAt(index);
                currentNode.IsClosed = true;

                if (currentNode == DestinationNode)
                {
                    //Debug.Log("Done");
                    return currentNode.GetPath(new List<PathNode>(), GetNode(originNodePosition));
                }

                //Debug.Log(currentNode.Neighbors.Count);
                foreach (PathNode neighbor in currentNode.Neighbors)
                {
                    if (neighbor.IsClosed)
                        continue;


                    float costToNeighbor = currentNode.CombinedValue + neighbor.PathCost;
                    if (costToNeighbor < neighbor.DistanceToStart || !openNodes.Contains(neighbor))
                    {
                        neighbor.DistanceToStart = costToNeighbor;
                        neighbor.DistanceToTarget = Vector3.Distance(DestinationNode.Position, neighbor.Position);
                        neighbor.PrevNode = currentNode;

                        if (!openNodes.Contains(neighbor))
                            openNodes.Add(neighbor);
                    }
                }
            }

            Debug.LogWarning("Could not find Path");
            return new List<PathNode>();
        }

        #region GetNode
        private PathNode GetNode(int x, int y, int z)
        {
            return GetNode(x,y,z, ref _allNodes);
        }

        private PathNode GetNode(Vector3Int position)
        {
            return GetNode(position.x, position.y, position.z, ref _allNodes);
        }

        private PathNode GetNode(Vector3Int position, ref List<PathNode> allNodes)
        {
            return GetNode(position.x, position.y, position.z, ref allNodes);
        }

        private PathNode GetNode(int x, int y, int z,ref List<PathNode> allNodes)
        {
            return allNodes[GetNodeIndex(x, y, z)];
        }

        private int GetNodeIndex(int x, int y, int z)
        {
            return (z * _gridSize.x * _gridSize.y) + (y * _gridSize.x) + x;
            //0
            //+ x * 0
            //+ x * z * 0

            //return ((x > 0 ? x - 1 : 0)
            //    + (_gridSize.x) * (z > 0 ? z - 1 : 0)
            //    + (_gridSize.x) * (_gridSize.z) * (y > 0 ?  y - 1 : 0));
        }

        #endregion

        #region Debuging
#if UNITY_EDITOR

        private void OnValidate()
        {
            DebugGetPath();
        }

        [Space]
        [Header("Debugging")]
        [SerializeField]
        private Vector3Int nodeToSearch = new Vector3Int(49, 0, 0);
        [SerializeField]
        private Vector3Int node2ToSearch = new Vector3Int(0, 0, 0);

        [Space]
        [SerializeField]
        private bool _showGround;
        [SerializeField]
        private bool _showPath;

        private List<PathNode> _debugNodes = new List<PathNode>();

        [ContextMenu("TestPath")]
        private void DebugGetPath()
        {
            if (!Application.isPlaying) return;

            _debugNodes = FindPath(nodeToSearch, node2ToSearch);
        }

        private void OnDrawGizmos()
        {
            if (_showPath)
            {
                if (_debugNodes != null)
                foreach(PathNode node in _debugNodes)
                {
                    Gizmos.DrawCube(node.Position, Vector3.one);
                }
            }

            if (_showGround)
            {
                if (_allNodes != null)
                for (int i = 0; i < _allNodes.Count; i++)
                {
                    Gizmos.color = Color.green;
                    if (_allNodes[i].IsGround)
                    {
                        Gizmos.DrawWireCube(_allNodes[i].Position, Vector2.one);
                    }
                }
            }
            if (_allNodes.Count > 0)
            {
                Gizmos.color = Color.yellow;

                // Bottom
                PathNode DrawNode = GetNode(0, 0, 0);
                Gizmos.DrawCube(DrawNode.Position, Vector3.one);

                DrawNode = GetNode(_gridSize.x - 1, 0, 0);
                Gizmos.DrawCube(DrawNode.Position, Vector3.one);

                DrawNode = GetNode(_gridSize.x - 1, 0, _gridSize.z - 1);
                Gizmos.DrawCube(DrawNode.Position, Vector3.one);

                DrawNode = GetNode(0, 0, _gridSize.z - 1);
                Gizmos.DrawCube(DrawNode.Position, Vector3.one);

                // Top
                DrawNode = GetNode(0, _gridSize.y - 1, 0);
                Gizmos.DrawCube(DrawNode.Position, Vector3.one);
            }

        }
#endif
        #endregion
    }
}