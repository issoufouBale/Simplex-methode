using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Simplex
{
    public class Simplex1
    {
        //source - симплекс таблица без базисных переменных
        double[,] table; //симплекс таблица

        int m, n;

        List<int> basis; //список базисных переменных

        public Simplex1(double[,] source)
        {
            m = source.GetLength(0);
            n = source.GetLength(1);
            table = new double[m, n + m - 1];
            basis = new List<int>();

            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < table.GetLength(1); j++)
                {
                    if (j < n)
                        table[i, j] = source[i, j];
                    else
                        table[i, j] = 0;
                }
                //выставляем коэффициент 1 перед базисной переменной в строке
                if ((n + i) < table.GetLength(1))
                {
                    table[i, n + i] = 1;
                    basis.Add(n + i);
                }
            }

            n = table.GetLength(1);
        }

        //result - в этот массив будут записаны полученные значения X
        public double[,] Calculate(double[] result)
        {
            int mainCol, mainRow; //ведущие столбец и строка

            while (!IsItEnd())
            {
                mainCol = findMainCol();
                mainRow = findMainRow(mainCol);
                basis[mainRow] = mainCol;

                double[,] new_table = new double[m, n];

                for (int j = 0; j < n; j++)
                    new_table[mainRow, j] = table[mainRow, j] / table[mainRow, mainCol];

                for (int i = 0; i < m; i++)
                {
                    if (i == mainRow)
                        continue;

                    for (int j = 0; j < n; j++)
                        new_table[i, j] = table[i, j] - table[i, mainCol] * new_table[mainRow, j];
                }
                table = new_table;
            }

            //заносим в result найденные значения X
            for (int i = 0; i < result.Length; i++)
            {
                int k = basis.IndexOf(i + 1);
                if (k != -1)
                    result[i] = table[k, 0];
                else
                    result[i] = 0;
            }

            return table;
        }

        private bool IsItEnd()
        {
            bool flag = true;

            for (int j = 1; j < n; j++)
            {
                if (table[m - 1, j] < 0)
                {
                    flag = false;
                    break;
                }
            }

            return flag;
        }

        private int findMainCol()
        {
            int mainCol = 1;

            for (int j = 2; j < n; j++)
                if (table[m - 1, j] < table[m - 1, mainCol])
                    mainCol = j;

            return mainCol;
        }

        private int findMainRow(int mainCol)
        {
            int mainRow = 0;

            for (int i = 0; i < m - 1; i++)
                if (table[i, mainCol] > 0)
                {
                    mainRow = i;
                    break;
                }

            for (int i = mainRow + 1; i < m - 1; i++)
                if ((table[i, mainCol] > 0) && ((table[i, 0] / table[i, mainCol]) < (table[mainRow, 0] / table[mainRow, mainCol])))
                    mainRow = i;

            return mainRow;
        }


    }
}


namespace testSimplex
{
    class Program
    {
        static void Main(string[] args)
        {
            double[,] table = { {5,3, 2,4},
                                {7, 9,12,8},
                                {5,4, 2,1},
                                { 10,8,11,3}
                               
            };

            double[] result = new double[7];
            double[,] table_result;
            // Simplex1.Simplex S = new Simplex1.Simplex(table);
            Simplex.Simplex1 S = new Simplex.Simplex1(table);
            table_result = S.Calculate(result);

            Console.WriteLine("Result of complex-table:");
            for (int i = 0; i < table_result.GetLength(0); i++)
            {
                for (int j = 0; j < table_result.GetLength(1); j++)
                    Console.Write(table_result[i, j] + " ");
                Console.WriteLine();
            }

            Console.WriteLine();
            Console.WriteLine("Result:");
            Console.WriteLine("X[1] = " + result[0]);
            Console.WriteLine("X[2] = " + result[1]);
            Console.WriteLine("X[3] = " + result[2]);
            Console.WriteLine("X[4] = " + result[3]);
            Console.WriteLine("X[5] = " + result[4]);
            Console.WriteLine("X[6] = " + result[5]);
            Console.WriteLine("X[7] = " + result[6]);

            Console.ReadLine();
        }
    }
}