using System;
using Newtonsoft.Json;

namespace Pomodoro
{
    public class NotesItem
    {
        public string Id { get; set; }

        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }

        [JsonProperty(PropertyName = "delete")]
        public bool Delete { get; set; }
    }
}

