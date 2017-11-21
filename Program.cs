using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace PrintMatrix
{
    class Program
    {
        static void Main(string[] args)
        {
            
            if(args.Contains("/h") || args.Length == 0)
            {
                log("Please enter filename:");
                string _filename = Console.ReadLine();

                log("Please enter value for c:");
                int _c;
                if(!int.TryParse(Console.ReadLine().ToString(), out _c))
                {
                    log("Please Input positive Integer for c");
                }


                Console.WriteLine("Please enter value for N:");
                int _n;
                if (!int.TryParse(Console.ReadLine().ToString(), out _n))
                {
                    log("Please Input positive Integer for N");
                }

                //computeOutputMatrix("../../TEST.PRN", _c, _n);
                computeOutputMatrix("../../TEST.PRN", _c, _n);
                Console.ReadKey();
            }
            else
            {

            }
        }


        static void DisplayOuputToConsole(decimal[,] array)
        {
            // Loop over array and display it.
            for (int i = 0; i <= array.GetUpperBound(0); i++)
            {
                for (int x = 0; x <= array.GetUpperBound(1); x++)
                {
                    Console.Write(array[i, x]);
                    Console.Write(" ");
                }
                Console.WriteLine();
            }
        }

        static void computeOutputMatrix(string fileName, int c, int N)
        {

            List<decimal> values = getFileContent(fileName);
            if(values.Count ==0)
            {
                log("No values exist in the file");
            }
            else if (N > values.Count)
            {
                log("Invalid N");
            }
            else if (c > N)
            {
                log("Invalid c");
            }
            else
            {
                // The float data type can store only 7 significant digits, but the sample input has more significant digits, so use decimal.
                decimal[,] outputArray = new decimal[c + 1, c + 1];
                for (int k = 0; k <= c; k++)
                {
                    for (int j = k; j <= c; j++)
                    {
                        decimal sum = 0;
                        for (int i = c; i < N; i++)
                        {
                            sum += values[i - k] * values[i - j];
                        }
                        outputArray[k, j] = Math.Round(sum, 6, MidpointRounding.AwayFromZero);
                        if (k != j)
                        {
                            outputArray[j, k] = outputArray[k, j];
                        }
                    }
                }
                DisplayOuputToConsole(outputArray);
            }
        }

        static List<decimal> getFileContent(string fileName)
        {
            List<decimal> values = new List<decimal>();
            if (File.Exists(fileName))
            {
                StreamReader sr = new StreamReader(fileName); //"../../TEST.PRN"
                string line = string.Empty;

                while ((line = sr.ReadLine()) != null)
                {
                    decimal value;
                    if (decimal.TryParse(line, NumberStyles.Any, CultureInfo.InvariantCulture, out value))
                    {
                        values.Add(value);
                    }
                    else
                    {
                        log("Invalid value");
                    }
                }
            }
            else
            {
                log("FileNotFound");
            }

            return values;
        }
        static void log(string msg)
        {
            Console.WriteLine(msg);
        }
    }
}
