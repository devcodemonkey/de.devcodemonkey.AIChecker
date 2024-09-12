﻿using de.devcodemonkey.AIChecker.UseCases.PluginInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace de.devcodemonkey.AIChecker.Importer.JsonDeserializer
{
    public class Deserializer<T> : IDeserializer<T>
    {
        public List<T> DeserializedClass { get; private set; }

        public async Task<List<T>> DeserialzeFileAsync(string filePath)
        {
            // Open the file as a stream for async reading
            using FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

            DeserializedClass = await JsonSerializer.DeserializeAsync<List<T>>(fileStream);

            // Deserialize the JSON string to a list of QuizItem objects
            return DeserializedClass!;
        }
    }
}
