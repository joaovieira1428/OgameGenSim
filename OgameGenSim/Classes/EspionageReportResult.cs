using System.Net;

namespace OgameGenSim.Classes;

public class EspionageReportResult
{
    public HttpStatusCode StatusCode { get; set; }
    public required string Message { get; set; }
    public CombatInformation CombatInformation { get; set; }
}
