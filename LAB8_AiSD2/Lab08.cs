using System;
using System.Collections.Generic;
using ASD.Graphs;

namespace ASD
{
    public class Lab08 : MarshalByRefObject
    {
        /// <summary>Etap I: prace przedprojektowe</summary>
        /// <param name="l">Długość działki, którą dysponuje Kameleon Kazik.</param>
        /// <param name="h">Maksymalna wysokość budowli.</param>
        /// <param name="pleasure">Tablica rozmiaru [l,h] zawierająca wartości zadowolenia p(x,y) dla każdych x i y.</param>
        /// <returns>Odpowiedź na pytanie, czy istnieje budowla zadowalająca Kazika.</returns>
        public bool Stage1ExistsBuilding(int l, int h, int[,] pleasure)
        {
            int X = l;
            int Y = h;
            DiGraph<int> g = new DiGraph<int>(X * Y + 2);
            int start = X * Y;
            int end = X * Y + 1;
            int allFlows = 0;

            for (int y = Y - 1; y > 0; y--)
            {
                for (int x = X - 1 - y; x >= 0; x--)
                {
                    int ind = y * X + x;
                    if (pleasure[x, y] > 0)
                    {
                        g.AddEdge(start, ind, pleasure[x, y]);
                        allFlows += pleasure[x, y];
                    }
                    g.AddEdge(ind, end, 1);
                    g.AddEdge(ind, ind - X, Int32.MaxValue);
                    g.AddEdge(ind, ind - X + 1, Int32.MaxValue);
                }
            }

            for (int x = 0; x < X; x++)
            {
                if (pleasure[x, 0] > 0)
                {
                    g.AddEdge(start, x, pleasure[x, 0]);
                    allFlows += pleasure[x, 0];
                }
                g.AddEdge(x, end, 1);
            }

            (int maxFlow, DiGraph<int> graphFlow) = Flows.FordFulkerson(g, start, end);

            return allFlows > maxFlow;
        }

        /// <summary>Etap II: kompletny projekt</summary>
        /// <param name="l">Długość działki, którą dysponuje Kameleon Kazik.</param>
        /// <param name="h">Maksymalna wysokość budowli.</param>
        /// <param name="pleasure">Tablica rozmiaru [l,h] zawierająca wartości zadowolenia p(x,y) dla każdych x i y.</param>
        /// <param name="blockOrder">Argument wyjściowy, w którym należy zwrócić poprawną kolejność ustawienia bloków w znalezionym rozwiązaniu;
        ///     kolejność jest poprawna, gdy przed blokiem (x,y) w tablicy znajdują się bloki (x,y-1) i (x+1,y-1) lub gdy y=0. 
        ///     Ustawiane bloki powinny mieć współrzędne niewychodzące poza granice obszaru budowy (0<=x<l, 0<=y<h).
        ///     W przypadku braku rozwiązania należy zwrócić null.</param>
        /// <returns>Maksymalna wartość zadowolenia z budowli; jeśli nie istnieje budowla zadowalająca Kazika, zależy zwrócić null.</returns>
        public int? Stage2GetOptimalBuilding(int l, int h, int[,] pleasure, out (int x, int y)[] blockOrder)
        {
            // USING STAGE 1
            int X = l;
            int Y = h;
            DiGraph<int> g = new DiGraph<int>(X * Y + 2);
            int start = X * Y;
            int end = X * Y + 1;
            int allFlows = 0;

            for (int y = Y - 1; y > 0; y--)
            {
                for (int x = X - 1 - y; x >= 0; x--)
                {
                    int ind = y * X + x;
                    if (pleasure[x, y] > 0)
                    {
                        g.AddEdge(start, ind, pleasure[x, y]);
                        allFlows += pleasure[x, y];
                    }
                    g.AddEdge(ind, end, 1);
                    g.AddEdge(ind, ind - X, Int32.MaxValue);
                    g.AddEdge(ind, ind - X + 1, Int32.MaxValue);
                }
            }

            for (int x = 0; x < X; x++)
            {
                if (pleasure[x, 0] > 0)
                {
                    g.AddEdge(start, x, pleasure[x, 0]);
                    allFlows += pleasure[x, 0];
                }
                g.AddEdge(x, end, 1);
            }

            (int maxFlow, DiGraph<int> graphFlow) = Flows.FordFulkerson(g, start, end);
            // END OF STAGE 1

            // If all pleasure used is lower than o equal to maxFlow, then task is impossible
            if (allFlows <= maxFlow)
            {
                blockOrder = new (int x, int y)[0];
                return null;
            }

            // Creating residual network
            DiGraph<int> resNet = new DiGraph<int>(g.VertexCount, graphFlow.Representation);
            foreach (var edge in g.DFS().SearchAll())
            {
                if (!graphFlow.HasEdge(edge.From, edge.To))
                {
                    resNet.AddEdge(edge.From, edge.To, edge.Weight);
                    continue;
                }

                int flow = graphFlow.GetEdgeWeight(edge.From, edge.To);
                if (edge.Weight - flow > 0)
                    resNet.AddEdge(edge.From, edge.To, edge.Weight - flow);
                resNet.AddEdge(edge.To, edge.From, flow);
            }
            
            // Finding the minimal cut
            bool[,] built = new bool[X, Y];
            foreach (var edge in resNet.BFS().SearchFrom(start))
            {
                if (edge.To < X * Y)
                    built[edge.To % X, edge.To / X] = true;
            }
            
            // Creating a list of used blocks
            List<(int x, int y)> builtBlocks = new List<(int x, int y)>();
            for (int y = 0; y < Y; y++)
                for (int x = 0; x < X - y; x++)
                    if (built[x, y])
                        builtBlocks.Add((x, y));
            
            // Returning found values
            blockOrder = builtBlocks.ToArray();
            return allFlows - maxFlow;
        }
    }
}
