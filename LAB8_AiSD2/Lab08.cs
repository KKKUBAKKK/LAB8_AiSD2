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
            DiGraph<int> g = new DiGraph<int>(l * h + 2);
            int start = l * h;
            int end = l * h + 1;
            for (int j = 0; j < l; j++)
            {
                g.AddEdge(j, end, Int32.MaxValue);
            }

            // int[,] currentPleasure = new int[h, l];
            // for (int i = 0; i < l; i++)
            //     currentPleasure[0, i] = pleasure[0, i];
            // int columns = l;
            // int rows = h;
            for (int i = 1; i < l; i++)
            {
                for (int j = 0; j < h - 1; j++)
                {
                    if (pleasure[i, j] - 1 > 0)
                    {
                        int ind = i * h + j;
                        g.AddEdge(start, ind, pleasure[i, j]);

                        g.AddEdge(ind, ind - h, pleasure[i, j] - 1);
                        g.AddEdge(ind, ind - h + 1, pleasure[i, j] - 1);
                    }
                }
            }

            (int flowValue, var f) = Flows.FordFulkerson(g, start, end);
            
            return flowValue > 0;
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
