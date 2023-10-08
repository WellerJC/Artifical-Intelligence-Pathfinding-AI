using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathfinder
{
    struct Node
    {
        public Coord2 GridPosition;
        public int VertexIndex;
        public int parent;
        public double H;
        public double G;
        public double F; 
        public int level;
        public enum NodeState { Untested, Open, Closed };

    }
}
