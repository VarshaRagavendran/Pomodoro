using System;
using Newtonsoft.Json;

namespace Pomodoro
{
    public class TaskItem
    {
        public string Id { get; set; }

        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }

        [JsonProperty(PropertyName = "complete")]
        public bool Complete { get; set; }
    }
}

