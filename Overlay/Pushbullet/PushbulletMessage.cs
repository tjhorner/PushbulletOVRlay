using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PushbulletOVRlay.Pushbullet
{
    class PushbulletMessage
    {
        [JsonProperty("type")]
        public string MessageType;

        [JsonProperty("push")]
        public Push Push;
    }
}
