using System;

namespace AxiansSample.Web.Models
{
    /// <summary>
    /// This class is supposed to mimic an Entity Framewwork DbContext
    /// </summary>
    public class AxiansSampleDbConext
    {
        public AxiansSampleDbConext()
        {
            GenerateIncidents();
        }
        public List<Incident> Incidents = new List<Incident>();
        public void GenerateIncidents() 
        {
            var incidents = new List<Incident>();
            var random = new Random();
            var numberOfIncidents = random.Next(1, 10);

            for (var i = 0; i < numberOfIncidents; i++)
            {
                var error = random.Next(1, 4) switch
                {
                    1 => new Incident
                    {
                        Id = $"INC{random.Next(1, 9999).ToString().PadLeft(4, '0')}",
                        CIID = random.Next(1, 9999).ToString(),
                        ErrorCode = "100",
                        Description = "Redis cache is running with high CPU"
                    },
                    2 => new Incident
                    {
                        Id = $"INC{random.Next(1, 9999).ToString().PadLeft(4, '0')}",
                        CIID = random.Next(1, 9999).ToString(),
                        ErrorCode = "101",
                        Description = "Redis cache has restarted"
                    },
                    3 => new Incident
                    {
                        Id = $"INC{random.Next(1, 9999).ToString().PadLeft(4, '0')}",
                        CIID = random.Next(1, 9999).ToString(),
                        ErrorCode = "102",
                        Description = "ServiceBus Queue depth is high"
                    },
                    4 => new Incident
                    {
                        Id = $"INC{random.Next(1, 9999).ToString().PadLeft(4, '0')}",
                        CIID = random.Next(1, 9999).ToString(),
                        ErrorCode = "103",
                        Description = "Container application has crashed"
                    }
                };
                incidents.Add(error);
            }
            this.Incidents.AddRange(incidents);
        }
    }

    public class Incident
    {
        /// <summary>
        /// Identifier of the incident
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Configuration Item Id is the identifier of the resource related to the incident
        /// </summary>
        public string CIID { get; set; }
        public string ErrorCode { get; set; }
        public string Severity { get; set; }
        public string Description { get; set; }
        public string Status { get; set; } = "Open";
    }
}
