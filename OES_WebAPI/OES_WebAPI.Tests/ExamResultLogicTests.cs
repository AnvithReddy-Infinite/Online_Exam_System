using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace OES_WebAPI.Tests
{
    [TestClass]
    public class ExamResultLogicTests
    {
        // Simple helper method for result calculation
        private bool IsPassed(int score, int passMarks)
        {
            return score >= passMarks;
        }

        [TestMethod]
        public void Exam_Result_Should_Be_Pass_Or_Fail_Based_On_Score()
        {
            // Arrange
            int passMarks = 5;

            int passingScore = 7;
            int failingScore = 3;

            // Act
            bool passResult = IsPassed(passingScore, passMarks);
            bool failResult = IsPassed(failingScore, passMarks);

            // Assert
            Assert.IsTrue(passResult, "Score above pass marks should pass");
            Assert.IsFalse(failResult, "Score below pass marks should fail");
        }
    }
}
