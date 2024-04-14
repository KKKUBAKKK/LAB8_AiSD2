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
            int rows = l;
            int columns = h;
            DiGraph<int> g = new DiGraph<int>(rows * columns + 2);
            int start = rows * columns;
            int end = rows * columns + 1;
            int[,] values = new int[rows, columns];

            for (int r = rows - 1; r >= 0; r--)
            {
                for (int c = columns - 1 - r; c >= 0; c--)
                {
                    int ind = r * columns + c;

                    if (pleasure[r, c] > 0)
                    {
                        g.AddEdge(start, ind, pleasure[r, c]);
                        values[r, c] = pleasure[r, c];

                        if (pleasure[r, c] > 1 && r > 0)
                        {
                            g.AddEdge(ind, ind - columns, pleasure[r, c] - 1);
                            g.AddEdge(ind, ind - columns + 1, pleasure[r, c] - 1);
                        }
                    }

                    if (r + 1 < rows)
                    {
                        int parent = ind + columns;
                        if (values[r + 1, c] > 1)
                        {
                            g.AddEdge(parent, ind, values[r + 1, c] - 1);
                            values[r, c] += values[r + 1, c] - 1;
                        }

                        parent -= 1;
                        if (c - 1 >= 0 && values[r + 1, c - 1] > 1)
                        {
                            g.AddEdge(parent, ind, values[r + 1, c - 1] - 1);
                            values[r, c] += values[r + 1, c - 1] - 1;
                        }
                    }
                    
                }
            }

            for (int i = 0; i < columns; i++)
            {
                if (values[0, i] > 1)
                    g.AddEdge(i, end, values[0, i] - 1);
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
