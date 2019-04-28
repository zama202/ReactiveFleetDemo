using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Model.EventModel
{
    public class StreamEvent
    {
        public DateTime ed { get; set; }
        public string pf { get; set; }
        public string pt { get; set; }
        public string sid { get; set; }
        public string code { get; set; }
        public Pos pos { get; set; }
        public object data { get; set; }
        public string pk { get; set; }
        public DateTime EventProcessedUtcTime { get; set; }
        public int PartitionId { get; set; }
        public DateTime EventEnqueuedUtcTime { get; set; }
        public IoTHub IoTHub { get; set; }
    }


    public class Pos
    {
        public string type { get; set; }
        public List<double> coordinates { get; set; }
    }

    public class IoTHub
    {
        public object MessageId { get; set; }
        public object CorrelationId { get; set; }
        public string ConnectionDeviceId { get; set; }
        public string ConnectionDeviceGenerationId { get; set; }
        public DateTime EnqueuedTime { get; set; }
        public object StreamId { get; set; }
    }
}
