using Microsoft.AspNetCore.Mvc;
using TrackingService.Services;

namespace TrackingService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnalyticsController : ControllerBase
    {
        [HttpGet("realtime")]
        public IActionResult GetRealtimeStats()
        {
            var snapshot = LiveAggregator.GetSnapshot();
            return Ok(snapshot);
        }

        // (Opzionale) potresti aggiungere Endpoint su DW 
        //  GET /api/analytics/funnel-report?days=7 ...
    }
}
