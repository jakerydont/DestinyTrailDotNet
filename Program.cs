using System;
using System.IO;
using System.Threading.Tasks;

using System.Collections.Generic;
using System.IO.Pipes;
using System.Security;

namespace DestinyTrailDotNet
{
    class Program
    {

 

        static async Task Main(string[] args)
        {
              
            int milesTraveled = 0;

            string occurrencesFilePath = "Occurrences.yaml"; // Update this path as needed
            string statusesFilePath = "Statuses.yaml"; // Path to the new statuses file
            string pacesFilePath = "Paces.yaml"; // Path to the new paces file
            string randomNamesPath = "RandomNames.yaml";

            string[] statuses = Utility.LoadYaml<StatusData>(statusesFilePath).Statuses.ToArray();
            string[] randomNames = Utility.LoadYaml<RandomNamesData>(randomNamesPath).RandomNames.ToArray();

            var party = new WagonParty(randomNames);
            Console.WriteLine(party.GetNames());

            OccurrenceEngine occurrenceEngine = new OccurrenceEngine(occurrencesFilePath, party, statuses);

            // Load paces from the YAML file
            PaceData paceData = Utility.LoadYaml<PaceData>(pacesFilePath);
            var pace = paceData.Paces.First(pace => pace.Name == "grueling");


            // Start with an initial date
            DateTime currentDate = new DateTime(1860, 10, 1); // Start from October 1, 1850

            // Loop to pick a new random occurrence every 5 seconds
            while (true)
            {


                // Pick a random occurrence based on probability
                Occurrence randomOccurrence = occurrenceEngine.PickRandomOccurrence();
                var occurrence = occurrenceEngine.ProcessOccurrence(randomOccurrence);


                // Output the date and display text of the occurrence along with the person's name
                Console.WriteLine($"\n{currentDate:MMMM d, yyyy}\n------\n{occurrence.DisplayText}");


                milesTraveled += pace.Factor;
                // Output the date and display text of the occurrence along with the person's name
                Console.WriteLine($"Distance traveled: {milesTraveled} miles ({milesTraveled} km)");
            

                // Increment the date by one day
                currentDate = currentDate.AddDays(1);

                // Wait for 5 seconds before picking a new occurrence
                await Task.Delay(5000); // 5000 milliseconds = 5 seconds
            }
        }



    }
}
