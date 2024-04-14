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
            int[,] visited = new int[X, Y];

            for (int y = Y - 1; y > 0; y--)
            {
                for (int x = X - 1 - y; x >= 0; x--)
                {
                    // Saving current index
                    int ind = y * X + x;

                    // If pleasure is > 1 adding edge from start to ind with weight = pleasure - 1
                    if (pleasure[x, y] > 1)
                    {
                        g.AddEdge(start, ind, pleasure[x, y] - 1);
                    }
                    
                    // Adding pleasure to visited (without subtracting 1 for now)
                    visited[x, y] += pleasure[x, y];

                    // If visited is > 1 adding edges to children
                    if (visited[x, y] > 1)
                    {
                        g.AddEdge(ind, ind - X, visited[x, y] - 1);
                        visited[x, y - 1] += visited[x, y] - 1; 
                        g.AddEdge(ind, ind - X + 1, visited[x, y] - 1);
                        visited[x + 1, y - 1] += visited[x, y] - 1; 
                    }
                }
            }

            // If values in the 0 row are positive add edges from them to the end
            for (int x = X - 1; x >= 0; x--)
            {
                if (visited[x, 0] > 0)
                    g.AddEdge(x, end, visited[x, 0]);
            }

            (int maxFlow, DiGraph<int> graphFlow) = Flows.FordFulkerson(g, start, end);

            return maxFlow > 0;
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
            blockOrder = null;
            return null;
        }
    }
}
