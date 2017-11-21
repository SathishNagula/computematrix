using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;


namespace ComputeMatrixTest
{
    class Program
    {
        static List<decimal> values;
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                // Parse args to get Filename, c and N values when the EXE is run with the inline Command Params from Debug directory.
                log("This is not supported yet, will be enhanced in the future version. Stay tuned!");
            }
            else
            {
                log("Please enter a filename:");
                string fileName = Console.ReadLine();

                // can force the user to enter positive value for 'c' and 'N' using a do-while loop, if need be;
                log("Please enter value for c:");
                int c;
                if (!int.TryParse(Console.ReadLine().ToString(), out c))
                {
                    log("Error - value must be positive Integer for c");
                    //return;
                }

                Console.WriteLine("Please enter value for N:");
                int N;
                if (!int.TryParse(Console.ReadLine().ToString(), out N))
                {
                    log("Error - value must be positive Integer for N");
                    //return;
                }

                if (IsValidInput(fileName, c, N))
                {
                    // The float data type can store only 7 significant digits, but the sample input has more significant digits, so use decimal.
                    decimal[,] matrix = new decimal[c + 1, c + 1];
                    computeOutputMatrix(matrix, fileName, c, N);
                    DisplayMatrixToConsole(matrix);
                }
            }
            log(Environment.NewLine + "Press any key to exit...");
            Console.ReadKey();
        }

        public static bool IsValidInput(string fileName, int c, int N)
        {
            bool validInput = true;
            if (!IsFileExists(ref fileName))
            {
                log("File not found, cannot proceed further");
                validInput = false;
            }
            else
            {
                if (c < 1 || N < 2 || c >= N)
                {
                    log("Invalid c or N (value of c must be greater than 0 & N must be greater than 1; Also c must be less than N)");
                    validInput = false;
                }
                else
                {
                    List<decimal> values = getFileContent(fileName);
                    if (N > values.Count)
                    {
                        log("Value of N cannot be more than the number of entries in the input file");
                        validInput = false;
                    }
                }

            }
            return validInput;
        }

        public static void computeOutputMatrix(decimal[,] matrix, string fileName, int c, int N)
        {
            /* 
             * Can call IsValidInput function before looping for extra safety, 
               but it is not needed in this stand-alone application.
            */
            for (int k = 0; k <= c; k++)
            {
                for (int j = k; j <= c; j++)
                {
                    decimal sum = 0;
                    for (int i = c; i < N; i++)
                    {
                        sum += values[i - k] * values[i - j];
                    }
                    matrix[k, j] = Math.Round(sum, 6, MidpointRounding.AwayFromZero);
                    if (k != j)
                    {
                        matrix[j, k] = matrix[k, j];
                    }
                }
            }
            
        }
        public static List<decimal> getFileContent(string fileName)
        {
            values = new List<decimal>();
            if (IsFileExists(ref fileName))
            {
                using (StreamReader sr = new StreamReader(fileName))
                {
                string line = string.Empty;
                int loopCounter = 0;

                while ((line = sr.ReadLine()) != null)
                {
                    loopCounter += 1;
                    decimal value;
                    if (decimal.TryParse(line, NumberStyles.Any, CultureInfo.InvariantCulture, out value))
                    {
                        values.Add(value);
                    }
                    else
                    {
                        log("Invalid entry at line " + loopCounter.ToString() + ", so it will be ignored in the computation.");
                    }
                }
                }
            }
            else
            {
                log("File Not Found");
            }

            return values;
        }

        public static bool IsFileExists(ref string fileName)
        {
            // check with full file path
            if (!File.Exists(fileName))
            {
                //check in the project directory - 2 steps above debug
                fileName = Path.Combine("../../", fileName);
                if (!File.Exists(fileName))
                {
                    return false;
                }
            }
            return true;
        }
        static void DisplayMatrixToConsole(decimal[,] array)
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

        public static void log(string msg)
        {
            Console.WriteLine(msg);
        }
    }
}
