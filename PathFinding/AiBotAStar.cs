using System;
using System.Collections.Generic;
using System.Linq;

namespace Pathfinder
{
    class AiBotAStar : AiBotBase
    {       
        double[,] graph;
        int gridsize;

        Coord2 Start;
        Coord2 Target;

        public List<Node> openList;
        public List<Node> closedList;
        public List<Node> Path;

        int[,] tiles;

        bool TargetFound = false;
        bool ok = false;

        Level level;

        public AiBotAStar(int x, int y) : base(x, y)
        {

            this.Start.X = x;
            this.Start.Y = y;


            openList = new List<Node>();
            closedList = new List<Node>();
            Path = new List<Node>();
        }

        public Coord2 Search(Node currentVertex)
        {

            Graph game = new Graph(level);
            graph = game.GenerateGraph();


            while ((openList.Count > 0) & (TargetFound == false))
            {

                for (int i = 0; i < graph.GetLength(1); i++)
                {

                    if (graph[currentVertex.VertexIndex, i] >= 1 & tiles[VertextToGridPosition(i).X, VertextToGridPosition(i).Y] == 0)
                    {

                        Node option = new Node();

                        option.parent = currentVertex.VertexIndex;
                        option.G = currentVertex.G + graph[currentVertex.VertexIndex, i];
                        option.level = currentVertex.level + 1;
                        option.VertexIndex = i;
                        option.GridPosition = VertextToGridPosition(i);
                        option.H = CalculateH(option.GridPosition, Target);
                        option.F = option.G + option.H;


                        if (openList.Exists(node => node.VertexIndex == i))
                        {
                            if (openList[openList.FindIndex(node => node.VertexIndex == i)].G > option.G)
                            {
                                openList.RemoveAt(openList.FindIndex(node => node.VertexIndex == i));
                                openList.Add(option);

                                continue;
                            }
                            else
                                continue;
                        }

                        else if (closedList.Exists(node => node.VertexIndex == i))
                        {
                            continue;
                        }

                        else
                        {
                            openList.Add(option);
                        }

                        if (option.GridPosition == Target)
                        {
                            break;
                        }
                    }
                }

                openList.Remove(currentVertex);
                closedList.Add(currentVertex);


                openList.OrderBy(node => node.F);
                currentVertex = openList.First();


                if (currentVertex.GridPosition == Target)
                {
                    TargetFound = true;

                    Path.Add(currentVertex);

                    for (int x = currentVertex.level; x > 0; x--)
                    {
                        foreach (Node node in closedList)
                        {
                            if (node.VertexIndex == currentVertex.parent)
                            {

                                Path.Add(node);
                                currentVertex = node;
                                break;
                            }
                        }
                    }
                }
            }

            return Path[(Path.Count() - 2)].GridPosition;
        }

        protected override void ChooseNextGridLocation(Level level, Player plr)
        {

            tiles = level.tiles;

            this.level = level;
            this.gridsize = level.tiles.GetLength(0);
            this.Target = plr.GridPosition;


            Node First = new Node
            {
                parent = -1,
                G = 0,
                level = 0
            };

            First.GridPosition.X = GridPosition.X;
            First.GridPosition.Y = GridPosition.Y;

            First.VertexIndex = GridPositionToVertex(First.GridPosition);
            First.F = First.G + First.H;

            openList.Add(First);

            if (Path.Count() > 0)
            {
                foreach (Node node in Path)
                {
                    if (node.parent == GridPositionToVertex(GridPosition))
                    {
                        ok = SetNextGridPosition(node.GridPosition, level);
                    }
                }
            }

            else
            {
                ok = SetNextGridPosition(Search(First), level);
            }
        }


        private double CalculateH(Coord2 Start, Coord2 Target)
        {
            double H = Math.Sqrt(Convert.ToDouble(((Target.X - Start.X) * (Target.X - Start.X)) + ((Target.Y - Start.Y) * (Target.Y - Start.Y))));

            return H;
        }


        private int GridPositionToVertex(Coord2 c)
        {
            int v = c.Y * gridsize + c.X;

            return v;
        }


        private Coord2 VertextToGridPosition(int vertexIndex)
        {
            Coord2 c;

            c.X = (vertexIndex % gridsize);
            c.Y = (vertexIndex / gridsize);

            return c;
        }
    }
}
