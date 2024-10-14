using Microsoft.VisualStudio.TestTools.UnitTesting;
using LmsWrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LmsWrapper.Tests
{
    [TestClass()]
    public class LoadUnloadLmsTests
    {
        [TestMethod()]
        public void LoadLmsAsyncTest()
        {
            // Arrange
            LoadUnloadLms loadUnloadLms = new LoadUnloadLms();

            // Act
            bool result = loadUnloadLms.LoadLms("lmstudio-community/Phi-3.5-mini-instruct-GGUF");

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod()]
        public void UnloadLmsAsyncTest()
        {
            // Arrange
            LoadUnloadLms loadUnloadLms = new LoadUnloadLms();

            // Act
            var modelName = "lmstudio-community/Phi-3.5-mini-instruct-GGUF";
            loadUnloadLms.LoadLms(modelName);
            bool result = loadUnloadLms.UnloadLms();

            // Assert
            Assert.IsTrue(result);
        }
    }
}