using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;


namespace ComputeMatrixTest
{
    public class Program
    {
        //static List<decimal> values;
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                // Parse args to get Filename, c and N values when the EXE is run with the inline Command Params from Debug directory.
                Log("This is not supported yet, will be enhanced in the future version. Stay tuned!");
            }
            else
            {
                // all the known errors have been handled with the error messages, but adding try/catch block for unknown errors at runtime
                try
                {
                    Log("Please enter a filename:");
                    string fileName = Console.ReadLine();

                    // can force the user to enter positive value for 'c' and 'N' using a do-while loop, if need be;
                    Log("Please enter value for c:");
                    int c;
                    if (!int.TryParse(Console.ReadLine(), out c))
                    {
                        Log("Error - value must be positive Integer for c");
                        //return;
                    }

                    Console.WriteLine("Please enter value for N:");
                    int N;
                    if (!int.TryParse(Console.ReadLine(), out N))
                    {
                        Log("Error - value must be positive Integer for N");
                        //return;
                    }

                    if (IsValidInput(fileName, c, N))
                    {
                        // The float data type can store only 7 significant digits, but the sample input has more significant digits, so use decimal.
                        decimal[,] matrix = new decimal[c + 1, c + 1];
                        ComputeOutputMatrix(matrix, fileName, c, N);
                        DisplayMatrixToConsole(matrix);
                    }
                }
                catch (InvalidCastException io)
                {
                    LogError(io.ToString());
                }
                catch (NullReferenceException ex)
                {
                    LogError(ex.ToString());
                }
                catch (Exception ex)
                {
                    LogError(ex.ToString());
                }
            }
            Log(Environment.NewLine + "Press any key to exit...");
            Console.ReadKey();
        }

        public static bool IsValidInput(string fileName, int c, int N)
        {
            try
            {
                bool validInput = true;
                if (!IsFileExists(ref fileName))
                {
                    Log("File not found, cannot proceed further");
                    validInput = false;
                }
                else
                {
                    if (c < 1 || N < 2 || c >= N)
                    {
                        Log("Invalid c or N (value of c must be greater than 0 & N must be greater than 1; Also c must be less than N)");
                        validInput = false;
                    }
                    else
                    {
                        if (N > (GetFileContent(fileName)).Count)
                        {
                            Log("Value of N cannot be more than the number of entries in the input file");
                            validInput = false;
                        }
                    }

                }
                return validInput;
            }
            catch (NullReferenceException ex)
            {
                LogError(ex.ToString());
                return false;
            }
            catch (Exception ex)
            {
                LogError(ex.ToString());
                return false;
            }
            
        }

        public static void ComputeOutputMatrix(decimal[,] matrix, string fileName, int c, int N)
        {
            /* 
             * Can call IsValidInput function before looping for extra safety, 
               but it is not needed in this stand-alone application.
            */
            try
            {
                List<decimal> values = GetFileContent(fileName);
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
            catch (ArrayTypeMismatchException ex)
            {
                LogError(ex.ToString());
            }
            catch (IndexOutOfRangeException ex)
            {
                LogError(ex.ToString());
            }
            catch (Exception ex)
            {
                LogError(ex.ToString());
            } 
        }
        public static List<decimal> GetFileContent(string fileName)
        {
            List<decimal> values = new List<decimal>();
            if (IsFileExists(ref fileName))
            {
                StreamReader sr = null;
                try
                {
                    sr = new StreamReader(fileName);
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
                            Log("Invalid entry at line " + loopCounter.ToString() + ", so it will be ignored in the computation.");
                        }
                    }
                }
                catch (IOException ex)
                {
                    LogError(ex.ToString());
                }
                catch (OutOfMemoryException ex)
                {
                    LogError(ex.ToString());
                }
                catch(Exception ex)
                {
                    LogError(ex.ToString());
                }
                finally
                {
                    if (sr != null)
                        sr.Dispose();
                }
            }

            return values;
        }

        public static bool IsFileExists(ref string fileName)
        {
            try
            {
                // check with full file path
                if (!File.Exists(fileName))
                {
                    //check in the project directory - 2 steps above debug
                    fileName = Path.Combine("../../", fileName);
                    if (!File.Exists(fileName))
                    {
                        // this is not necessary, just adding to demostrate the use of try/catch blocks
                        throw new FileNotFoundException();
                    }
                }
            }
            catch (NullReferenceException ex)
            {
                LogError(ex.ToString());
                return false;
            }
            catch (FileNotFoundException ex)
            {
                LogError(ex.ToString());
                return false;
            }
            catch (Exception ex)
            {
                LogError(ex.ToString());
                return false;
            }

            return true;
        }
        static void DisplayMatrixToConsole(decimal[,] array)
        {
            try
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
            catch (IndexOutOfRangeException ex)
            {
                LogError(ex.ToString());
            }
            catch (Exception ex)
            {
                LogError(ex.ToString());
            }

        }
        public static void LogError(string message)
        {
            // Display an apologies message to the end user
            Log("An unexpected error occurred, apologies for the inconvience this may have caused. Please try again later!");
            //Log(message);
            // Do something with the exception error message.
            // Like email support team with full error message, log error to a text file etc.
            
        }

        public static void Log(string msg)
        {
            Console.WriteLine(msg);
        }
    }

}
