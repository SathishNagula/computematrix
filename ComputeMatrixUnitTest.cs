using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace ComputeMatrix.Test
{
    [TestClass]
    public class ComputeMatrixUnitTest
    {
        [TestMethod]
        public void TestOutputMatrix()
        {
            //Arrange
            decimal[,] expected = new decimal[,]
            {
                {0.279525M, 0.276682M, 0.268098M, 0.254212M, 0.235722M},
                {0.276682M, 0.280218M, 0.277855M, 0.269717M, 0.256231M},
                {0.268098M, 0.277855M, 0.281864M, 0.279912M, 0.272113M},
                {0.254212M, 0.269717M, 0.279912M, 0.284270M, 0.282571M},
                {0.235722M, 0.256231M, 0.272113M, 0.282571M, 0.287076M}
            };
            
            // Act
           decimal [,] actual = new decimal [5,5];
           ComputeMatrixTest.Program.ComputeOutputMatrix(actual, GetTestDataFile(), 4, 300);

           //Assert
           CollectionAssert.AreEqual(expected, actual);
        }
        public static string GetTestDataFile()
        {
            string fileName = "TEST.PRN";
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            var pathItems = baseDir.Split(Path.DirectorySeparatorChar);
            var pos = pathItems.Reverse().ToList().FindIndex(x => string.Equals("bin", x));
            string projectPath = String.Join(Path.DirectorySeparatorChar.ToString(), pathItems.Take(pathItems.Length - pos - 1));
            return Path.Combine(projectPath, "Test_Data", fileName);
        }
    }
}
