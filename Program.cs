﻿using System;
using System.IO;
using System.Threading.Tasks;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using System.Collections.Generic;
using System.IO.Pipes;

namespace DestinyTrailDotNet
{
    class Program
    {
        private static string[] RandomNames = 
        {
            "Alice", "Bob", "Charlie", "Diana", "Ethan", 
            "Fiona", "George", "Hannah", "Isaac", "Julia", 
            "Kevin", "Laura", "Michael", "Nina", "Oliver",
            "Paula", "Quentin", "Rachel", "Sam", "Tina",
            "Ulysses", "Victor", "Wendy", "Xander", "Yvonne", "Zeke"
        };

 

        static async Task Main(string[] args)
        {
              
            int milesTraveled = 0;

            string occurrencesFilePath = "Occurrences.yaml"; // Update this path as needed
            string statusesFilePath = "Statuses.yaml"; // Path to the new statuses file
            string pacesFilePath = "Paces.yaml"; // Path to the new paces file


            string[] statuses = LoadYaml<StatusData>(statusesFilePath).Statuses.ToArray();


            OccurrenceEngine occurrenceEngine = new OccurrenceEngine(occurrencesFilePath,  RandomNames, statuses);

            // Load paces from the YAML file
            PaceData paceData = LoadYaml<PaceData>(pacesFilePath);
            var pace = paceData.Paces.First(pace => pace.Name == "grueling");


            // Start with an initial date
            DateTime currentDate = new DateTime(1850, 10, 1); // Start from October 1, 1850

            // Loop to pick a new random occurrence every 5 seconds
            while (true)
            {


                // Pick a random occurrence based on probability
                Occurrence randomOccurrence = occurrenceEngine.PickRandomOccurrence();
                var occurrence = occurrenceEngine.ProcessOccurrence(randomOccurrence);


                // Output the date and display text of the occurrence along with the person's name
                Console.WriteLine($"{currentDate:MMMM d, yyyy}: {occurrence.DisplayText}");


                milesTraveled += pace.Factor;
                // Output the date and display text of the occurrence along with the person's name
                Console.WriteLine($"Distance traveled: {milesTraveled} miles ({milesTraveled} km)");
            

                // Increment the date by one day
                currentDate = currentDate.AddDays(1);

                // Wait for 5 seconds before picking a new occurrence
                await Task.Delay(5000); // 5000 milliseconds = 5 seconds
            }
        }

        // Generic method to load YAML into specified type T
        private static T LoadYaml<T>(string yamlFilePath)
        {
            var yaml = File.ReadAllText(yamlFilePath);
            var deserializer = new DeserializerBuilder()
                .Build(); // Removed the naming convention

            return deserializer.Deserialize<T>(yaml); // Deserialize and return the result
        }

    }
}
