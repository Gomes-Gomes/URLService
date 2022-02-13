using System;
using System.Collections.Generic;

namespace URLService.Services.HealthCheck
{
    public class HealthCheckResponse
    {
        public string Status { get; set; }

        public IEnumerable<HealthCheck> checks { get; set; }

        public TimeSpan Duration { get; set; }
    }
}