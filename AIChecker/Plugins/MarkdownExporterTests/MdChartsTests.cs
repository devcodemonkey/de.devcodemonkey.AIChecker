using Microsoft.VisualStudio.TestTools.UnitTesting;
using de.devcodemonkey.AIChecker.MarkdownExporter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;

namespace de.devcodemonkey.AIChecker.MarkdownExporter.Tests
{
    [TestClass()]
    public class MdChartsTests
    {
        private MdCharts _chartService;

        [TestInitialize]
        public void SetUp()
        {
            _chartService = new MdCharts();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Values are required.")]
        public void CreateBarChart_ValuesNull_ThrowsArgumentException()
        {
            // Arrange
            double[] values = null;
            string[] descriptions = { "A", "B", "C" };

            // Act
            _chartService.CreateBarChart(values, descriptions);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Descriptions are required.")]
        public void CreateBarChart_DescriptionsNull_ThrowsArgumentException()
        {
            // Arrange
            double[] values = { 1, 2, 3 };
            string[] descriptions = null;

            // Act
            _chartService.CreateBarChart(values, descriptions);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Values and descriptions must have the same length.")]
        public void CreateBarChart_ValuesAndDescriptionsDifferentLength_ThrowsArgumentException()
        {
            // Arrange
            double[] values = { 1, 2, 3 };
            string[] descriptions = { "A", "B" }; // different length

            // Act
            _chartService.CreateBarChart(values, descriptions);
        }

        [TestMethod]
        public void CreateBarChart_ValidInput_ReturnsFilePath()
        {
            // Arrange
            double[] values = { 1, 2, 3 };
            string[] descriptions = { "A", "B", "C" };            

            // Act
            string fileName = _chartService.CreateBarChart(values, descriptions);

            // Assert
            Assert.IsFalse(string.IsNullOrEmpty(fileName), "File name should not be null or empty.");
            Assert.IsTrue(fileName.EndsWith(".png"), "File name should end with .png");
        }
    }
}