using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FlowFields
{
    public class GridDirection
    {
        public readonly Vector3Int Vector;

        private GridDirection(int x, int y, int z) 
        {
            Vector = new Vector3Int(x, y, z);
        }

        public static implicit operator Vector3Int(GridDirection direction)
        {
            return direction.Vector;
        }

        // No Direction
        public static readonly GridDirection None           = new GridDirection( 0, 0, 0);
        
        //
        // Straigt
        //
        public static readonly GridDirection North          = new GridDirection( 0, 0, 1);
        public static readonly GridDirection South          = new GridDirection( 0, 0,-1);
        public static readonly GridDirection East           = new GridDirection( 1, 0, 0);
        public static readonly GridDirection West           = new GridDirection(-1, 0, 0);
        
        // Corners
        public static readonly GridDirection NorthEast      = new GridDirection( 1, 0, 1);
        public static readonly GridDirection NorthWest      = new GridDirection(-1, 0, 1);
        public static readonly GridDirection SouthEast      = new GridDirection( 1, 0,-1);
        public static readonly GridDirection SouthWest      = new GridDirection(-1, 0, 1);
        
        //
        // Up
        //
        public static readonly GridDirection Up             = new GridDirection( 0, 1, 0);

        // Up corners
        public static readonly GridDirection UpNorthEast    = new GridDirection( 1, 1, 1);
        public static readonly GridDirection UpNorthWest    = new GridDirection(-1, 1, 1);
        public static readonly GridDirection UpSouthEast    = new GridDirection( 1, 1,-1);
        public static readonly GridDirection UpSouthWest    = new GridDirection(-1, 1, 1);
        
        // Up Straigt
        public static readonly GridDirection UpNorth        = new GridDirection( 0, 1, 1);
        public static readonly GridDirection UpSouth        = new GridDirection( 0, 1,-1);
        public static readonly GridDirection UpEast         = new GridDirection( 1, 1, 0);
        public static readonly GridDirection UpWest         = new GridDirection(-1, 1, 0);

        //
        // Down
        //
        public static readonly GridDirection Down           = new GridDirection( 0,-1, 0);

        // Down corners
        public static readonly GridDirection DownNorthEast  = new GridDirection( 1,-1, 1);
        public static readonly GridDirection DownNorthWest  = new GridDirection(-1,-1, 1);
        public static readonly GridDirection DownSouthEast  = new GridDirection( 1,-1,-1);
        public static readonly GridDirection DownSouthWest  = new GridDirection(-1,-1, 1);
        
        // Down Straigt
        public static readonly GridDirection DownNorth      = new GridDirection( 0,-1, 1);
        public static readonly GridDirection DownSouth      = new GridDirection( 0,-1,-1);
        public static readonly GridDirection DownEast       = new GridDirection( 1,-1, 0);
        public static readonly GridDirection DownWest       = new GridDirection(-1,-1, 0);
        //

        public static readonly List<GridDirection> CardinalDirections = new List<GridDirection>()
        {
            Up,
            Down,

            North,
            South,
            East,
            West
        };

        public static readonly List<GridDirection> CardinalDirectionsAndNone = new List<GridDirection>()
        {
            None,

            Up,
            Down,

            North,
            South,
            East,
            West
        };
        public static readonly List<GridDirection> CardinalAndIntercardinalDirections = new List<GridDirection>()
        {
            Up,
            Down,

            North,
            South,
            East,
            West,

            NorthEast,
            NorthWest,
            SouthEast,
            SouthWest,

            UpNorthEast,
            UpNorthWest,
            UpSouthEast,
            UpSouthWest,

            DownNorthEast,
            DownNorthWest,
            DownSouthEast,
            DownSouthWest,

            UpNorth,
            UpSouth,
            UpEast,
            UpWest,

            DownNorth,
            DownSouth,
            DownEast,
            DownWest,
        };

        public static readonly List<GridDirection> AllDirections = new List<GridDirection>()
        {
            None,

            Up,
            Down,

            North,
            South,
            East,
            West,

            NorthEast,
            NorthWest,
            SouthEast,
            SouthWest,

            UpNorthEast,
            UpNorthWest,
            UpSouthEast,
            UpSouthWest,

            DownNorthEast,
            DownNorthWest,
            DownSouthEast,
            DownSouthWest,

            UpNorth,
            UpSouth,
            UpEast,
            UpWest,

            DownNorth,
            DownSouth,
            DownEast,
            DownWest,
        };

        internal static GridDirection GetDirectionFromV3I(Vector3Int vector3Int)
        {
            return CardinalAndIntercardinalDirections.DefaultIfEmpty(None).FirstOrDefault(direction => direction == vector3Int);
        }
    }
}
