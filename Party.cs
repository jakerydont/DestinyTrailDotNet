namespace DestinyTrailDotNet
{
    public class Party
    {
        private int memberCounter = 0;
        public List<Person> Members {get;set;}

        public Party(string[] randomNames, int size = 5)
        {
            Members = new List<Person>();

            for (var i = 0; i < size; i++) {
                var member = GenerateRandomPerson(randomNames);
                Members.Add(member);
            }

        }

        public Person GetRandomMember() {
            Random random = new Random();
            var person = Members[random.Next(Members.Count)];
            return person;
        }

        
        public Person GenerateRandomPerson(string[] randomNames)
        {
            Random random = new Random();
            var id = memberCounter;
            memberCounter++;
            var name = randomNames[random.Next(randomNames.Length)];

            return new Person
            {
                ID = id,
                Name = name,
                Status = new Status { Name = "Good" }
            };
        }

    }
}
