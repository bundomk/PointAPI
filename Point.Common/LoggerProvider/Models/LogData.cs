using Newtonsoft.Json;
using System;

namespace Point.Common.LoggerProvider
{
    public class LogData
    {
        public LogData()
        { }

        public LogData(Guid referenceKey, string tags, string attributes, object message = null)
        {
            ReferenceKey = referenceKey;
            Tags = tags;
            Attributes = attributes;
            Message = message;
        }

        public Guid ReferenceKey { get; set; }
        public string Tags { get; set; }
        public string Attributes { get; set; }
        public object Message { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
