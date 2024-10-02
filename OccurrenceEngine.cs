using System;
using System.IO;
using System.Linq;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace DestinyTrailDotNet
{
    public class OccurrenceEngine
    {
        private readonly Occurrence[] _occurrences;
        private readonly string[] _randomNames;
        private readonly string[] _statuses;

        public OccurrenceEngine(string yamlFilePath,  string[] randomNames, string[] statuses)
        {
            _occurrences = LoadOccurrences(yamlFilePath);
            _randomNames = randomNames;
            _statuses = statuses;

        }

        private Occurrence[] LoadOccurrences(string yamlFilePath)
        {
            var yaml = File.ReadAllText(yamlFilePath);
            var deserializer = new DeserializerBuilder()
                //.WithNamingConvention(CamelCaseNamingConvention.Instance) // Ensure naming convention matches
                .Build();

            // Deserialize the YAML into OccurrenceData
            var occurrenceData = deserializer.Deserialize<OccurrenceData>(yaml);
            return occurrenceData.Occurrences ?? []; // Ensure this property is being accessed correctly
        }


        public Occurrence PickRandomOccurrence()
        {
            double totalProbability = _occurrences.Sum(o => o.Probability);
            Random random = new Random();
            double randomValue = random.NextDouble() * totalProbability;

            foreach (var occurrence in _occurrences)
            {
                if (randomValue < occurrence.Probability)
                {
                    return occurrence;
                }
                randomValue -= occurrence.Probability;
            }

            return _occurrences.Last();
        }

        public Occurrence ProcessOccurrence(Occurrence occurrence)
        {
            var person = GetRandomPerson();
            // Replace the {name} token in the display text with the person's name
            occurrence.DisplayText = occurrence.DisplayText.Replace("{name}", person.Name);
            return occurrence;
        }

        private Person GetRandomPerson()
        {
            Random random = new Random();
            string id = Guid.NewGuid().ToString(); // Generate a unique ID
            string name = _randomNames[random.Next(_randomNames.Length)]; // Pick a random name from the array
            
            // Pick a random status from the array and create a new Status object
            var randomStatus = _statuses[random.Next(_statuses.Length)];

            return new Person
            {
                ID = id,
                Name = name,
                Status = new Status { Name = randomStatus } // Assigning a strongly typed Status
            };
        }

    }
}
