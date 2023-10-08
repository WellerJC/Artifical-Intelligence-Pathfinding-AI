using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathfinder
{
    class AIBotLRTA : AiBotBase
    {
        double[,] graph;
        int gridsize;

        Coord2 source;
        Coord2 target;
        public IDictionary<int, NodeLRTA> nodeList;
        int currentVertex, targetVertex;

        public AIBotLRTA(double[,] graph, Coord2 source, Coord2 target, int gridsize) : base(source.X, source.Y)
        {
            this.graph = graph;
            this.gridsize = gridsize;
            this.source = source;
            this.target = target;

            nodeList = new Dictionary<int, NodeLRTA>();

            initialise();
        }
        private void initialise()
        {
            NodeLRTA newNode;
            for (int i = 0; i < graph.GetLength(0); i++)
            {
                newNode = new NodeLRTA();
                newNode.stateCost = 0;
                newNode.VertexIndex = i;
                newNode.gridPosition = VertexToGridPosition(i);

                nodeList.Add(i, newNode);
            }
            currentVertex = GridPostionToVertex(source);
            targetVertex = GridPostionToVertex(target);
        }

        protected override void ChooseNextGridLocation(Level level, Player plr)
        {
            if (currentVertex != targetVertex)
            {
                int nextVertex = Lookahead(currentVertex);
                Coord2 nextMove = VertexToGridPosition(nextVertex);

                SetNextGridPosition(nextMove, level);
                currentVertex = nextVertex;
            }
        }

        private int Lookahead(int vertexIndex)
        {
            double min = int.MaxValue;
            ArrayList minVertex = new ArrayList();

            for (int i = 0; i < graph.GetLength(0); i++)
            {
                if (graph[vertexIndex, i] >= 1)
                {
                    if (min >= graph[vertexIndex, i])
                    {
                        if (min > graph[vertexIndex, i])
                        {
                            minVertex.Clear();
                            minVertex.Add(i);
                        }
                        else
                        {
                            minVertex.Add(i);
                        }

                        min = graph[vertexIndex, i];
                    }
                }
            }
            int nVertex;
            if (minVertex.Count > 1)
            {
                Random rnd = new Random();
                int r = rnd.Next(minVertex.Count);
                nVertex = (int)minVertex[r];
            }
            else
            {
                nVertex = (int)minVertex[0];
            }
            LRTA_Cost(vertexIndex, nVertex);
            return nVertex;
        }

        private void LRTA_Cost(int cVertex, int nVertex)
        {
            NodeLRTA tmp;
            nodeList.TryGetValue(cVertex, out tmp);
            tmp.stateCost += graph[cVertex, nVertex];

            nodeList[cVertex] = tmp;
        }

        private Coord2 VertexToGridPosition(int vertexIndex)
        {
            Coord2 c;
            c.X = (vertexIndex % gridsize);
            c.Y = (vertexIndex / gridsize);
            return c;
        }

        public int GridPostionToVertex(Coord2 c)
        {
            int v = c.Y * gridsize + c.X;
            return v;
        }
    }
}
