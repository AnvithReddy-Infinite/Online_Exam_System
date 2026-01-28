using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.SqlClient;
using System.Configuration;

namespace OES_WebAPI.Tests
{
    [TestClass]
    public class DatabaseConnectionTests
    {
        [TestMethod]
        public void Database_Connection_Should_Open_Successfully()
        {
            // Arrange
            string connectionString = ConfigurationManager
                .ConnectionStrings["DefaultConnection"]
                .ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Act
                connection.Open();

                // Assert
                Assert.AreEqual(
                    System.Data.ConnectionState.Open,
                    connection.State
                );
            }
        }
    }
}

