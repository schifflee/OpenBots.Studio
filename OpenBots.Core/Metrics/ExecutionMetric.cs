using System;
using System.Collections.Generic;
using OpenBots.Core.Model.EngineModel;

namespace OpenBots.Core.Metrics
{
    public class ExecutionMetric
    {
        public string FileName { get; set; }
        public TimeSpan AverageExecutionTime { get; set; }
        public List<ScriptFinishedEventArgs> ExecutionData { get; set; }
    }
}
