using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathfinder
{
    class Graph
    {
        private double[,] graph;
        Level gamelevel;
        string[] s;
        int size;
        double diagonalWeight = 1.414;

        public Graph(Level level)
        {
            gamelevel = level;
            size = gamelevel.tiles.GetLength(0) * gamelevel.tiles.GetLength(1);
            s = new string[size];
            graph = new double[size, size];
            ;
        }

        public double[,] GenerateGraph()
        {
            int row = 0, col = 0;
            Coord2 pos;
            int v_adj = -1;

            // Store the Graph in Adj Matrix
            // Initialize Adj Matrix with zeros
            for (int i = 0; i < gamelevel.tiles.GetLength(0); i++)
                for (int j = 0; j < gamelevel.tiles.GetLength(1); j++)
                    graph[i, j] = 0;

            // Convert the grid into Graph datastructure
            for (int v = 0; v < gamelevel.tiles.GetLength(0) * gamelevel.tiles.GetLength(1); v++)
            {
                // Is the current position a obstacle? If so we don't have to calculate the movements
                pos.X = col;
                pos.Y = row;
                if (gamelevel.ValidPosition(pos))
                {
                    // Eight Movements: Up, Left, Down, Right, Four Diagonals
                    //1. Left     
                    pos.X = col - 1;
                    pos.Y = row;
                    v_adj = v - 1;
                    if (gamelevel.ValidPosition(pos))
                    {
                        graph[v, v_adj] = 1;
                        if (s[v] != null)
                            s[v] = s[v] + "," + v_adj.ToString();
                        else
                            s[v] = v_adj.ToString();
                    }

                    //2. Right
                    pos.X = col + 1;
                    pos.Y = row;
                    v_adj = v + 1;
                    if (gamelevel.ValidPosition(pos))
                    {
                        graph[v, v_adj] = 1;
                        if (s[v] != null)
                            s[v] = s[v] + "," + v_adj.ToString();
                        else
                            s[v] = v_adj.ToString();
                    }
                    //3. Down
                    pos.X = col;
                    pos.Y = row + 1;
                    v_adj = v + gamelevel.tiles.GetLength(0);
                    if (gamelevel.ValidPosition(pos))
                    {
                        graph[v, v_adj] = 1;
                        if (s[v] != null)
                            s[v] = s[v] + "," + v_adj.ToString();
                        else
                            s[v] = v_adj.ToString();
                    }
                    //4. Up
                    pos.X = col;
                    pos.Y = row - 1;
                    v_adj = v - gamelevel.tiles.GetLength(0);
                    if (gamelevel.ValidPosition(pos))
                    {
                        graph[v, v_adj] = 1;
                        if (s[v] != null)
                            s[v] = s[v] + "," + v_adj.ToString();
                        else
                            s[v] = v_adj.ToString();
                    }

                    //5. Up Left Diagonal
                    pos.X = col - 1;
                    pos.Y = row - 1;
                    v_adj = v - gamelevel.tiles.GetLength(0) - 1;
                    if (gamelevel.ValidPosition(pos))
                    {
                        graph[v, v_adj] = diagonalWeight;
                        if (s[v] != null)
                            s[v] = s[v] + "," + v_adj.ToString();
                        else
                            s[v] = v_adj.ToString();
                    }

                    //6. Up Right Diagonal
                    pos.X = col + 1;
                    pos.Y = row - 1;
                    v_adj = v - gamelevel.tiles.GetLength(0) + 1;
                    if (gamelevel.ValidPosition(pos))
                    {
                        graph[v, v_adj] = diagonalWeight;
                        if (s[v] != null)
                            s[v] = s[v] + "," + v_adj.ToString();
                        else
                            s[v] = v_adj.ToString();
                    }
                    //7. Down Left Diagonal
                    pos.X = col - 1;
                    pos.Y = row + 1;
                    v_adj = v + gamelevel.tiles.GetLength(0) - 1;
                    if (gamelevel.ValidPosition(pos))
                    {
                        graph[v, v_adj] = diagonalWeight;
                        if (s[v] != null)
                            s[v] = s[v] + "," + v_adj.ToString();
                        else
                            s[v] = v_adj.ToString();
                    }
                    //8. Down Right Diagonal
                    pos.X = col + 1;
                    pos.Y = row + 1;
                    v_adj = v + gamelevel.tiles.GetLength(0) + 1;
                    if (gamelevel.ValidPosition(pos))
                    {
                        graph[v, v_adj] = diagonalWeight;
                        if (s[v] != null)
                            s[v] = s[v] + "," + v_adj.ToString();
                        else
                            s[v] = v_adj.ToString();
                    }
                }
                col++;

                if (col >= gamelevel.tiles.GetLength(0))
                {
                    col = 0;
                    row++;
                }

            }
            return graph;
        }
        public string displayGraph(double[,] g)
        {
            string s = "";
            for (int i = 0; i < g.GetLength(0); i++)
            {
                s += "\n";
                for (int j = 0; j < g.GetLength(1); j++)
                    s += graph[i, j].ToString() + " ";
            }
            return s;
        }
    }
}
