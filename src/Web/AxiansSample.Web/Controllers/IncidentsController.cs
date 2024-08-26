using AxiansSample.Web.Models;
using AxiansSample.Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;

namespace AxiansSample.Web.Controllers
{
    public class IncidentsController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AxiansSampleDbConext _context;

        public IncidentsController(ILogger<HomeController> logger, AxiansSampleDbConext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<JsonResult> getIncidents(string seachText)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(seachText))
                {
                    var model = _context.Incidents
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
                    return Json(model);
                }
                else
                {
                    var model = _context.Incidents
                        .Where(i => i.Id.Contains(seachText) || i.CIID.Contains(seachText) || i.Description.Contains(seachText))
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
                    return Json(model);
                }
            }
            catch (Exception ex)
            {
                return Json(new { error = "Something went wrong"});
            }
        }
    }
}
