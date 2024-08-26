using AxiansSample.Web.Controllers;
using AxiansSample.Web.Models;
using AxiansSample.Web.Models.ViewModels;
using Microsoft.AspNetCore.SignalR;

namespace AxiansSample.Web.Services
{
    public class IncidentHub : Hub
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AxiansSampleDbConext _context;

        public IncidentHub(ILogger<HomeController> logger, AxiansSampleDbConext context)
        {
            _logger = logger;
            _context = context;
        }
        public async Task generateSomeIncidents()
        {
            // Generated some new incidents
            _context.GenerateIncidents();
            // Cast to list of IncidentViewModel
            var incidents = _context.Incidents
                .Select(i =>
                    {
                        return new IncidentViewModel
                        {
                            Id = i.Id,
                            CIID = i.Id,
                            ErrorCode = i.ErrorCode,
                            Severity = i.Severity,
                            Description = i.Description,
                        };
                    })
                .ToList();
            // Get the ID of the calling client (browser)
            var client = Clients.Client(this.Context.ConnectionId);
            // Call updatedIncidents of the client
            await client.SendAsync("updatedIncidents", incidents);
        }
    }
}
