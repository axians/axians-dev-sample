namespace AxiansSample.Web.Models.ViewModels
{
    public class IncidentViewModel
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
    }
}
