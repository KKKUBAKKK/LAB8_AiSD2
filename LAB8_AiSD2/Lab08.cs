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
            int[,] values = new int[X, Y];

            for (int y = Y - 1; y >= 0; y--)
            {
                for (int x = X - 1 - y; x >= 0; x--)
                {
                    int ind = y * X + x;

                    if (pleasure[x, y] > 0)
                    {
                        g.AddEdge(start, ind, pleasure[x, y]);
                        values[x, y] = pleasure[x, y];

                        if (pleasure[x, y] > 1 && y > 0)
                        {
                            g.AddEdge(ind, ind - X, pleasure[x, y] - 1);
                            g.AddEdge(ind, ind - X + 1, pleasure[x, y] - 1);
                        }
                    }

                    if (y + 1 < Y)
                    {
                        int parent = ind + X;
                        if (values[x, y + 1] > 1)
                        {
                            g.AddEdge(parent, ind, values[x, y + 1] - 1);
                            values[x, y] += values[x, y + 1] - 1;
                        }

                        parent -= 1;
                        if (x - 1 >= 0 && values[x - 1, y + 1] > 1)
                        {
                            g.AddEdge(parent, ind, values[x - 1, y + 1] - 1);
                            values[x, y] += values[x - 1, y + 1] - 1;
                        }
                    }
                    
                }
            }

            for (int i = 0; i < Y; i++)
            {
                if (values[i, 0] > 1)
                    g.AddEdge(i, end, values[i, 0] - 1);
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
