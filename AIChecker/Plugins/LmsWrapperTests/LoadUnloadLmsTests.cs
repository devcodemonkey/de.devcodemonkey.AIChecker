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
            bool result = loadUnloadLms.Load("lmstudio-community/Phi-3.5-mini-instruct-GGUF");

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
            loadUnloadLms.Load(modelName);
            bool result = loadUnloadLms.Unload();

            // Assert
            Assert.IsTrue(result);
        }
    }
}