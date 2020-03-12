using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace PushbulletOVRlay.Pushbullet
{
    class Push
    {
        [JsonProperty("type")]
        public string PushType;

        [JsonProperty("icon")]
        public string Icon;

        [JsonProperty("title")]
        public string Title;

        [JsonProperty("body")]
        public string Body;

        [JsonProperty("source_user_iden")]
        public string SourceUserId;

        [JsonProperty("source_device_iden")]
        public string SourceDeviceId;

        [JsonProperty("application_name")]
        public string ApplicationName;

        [JsonProperty("dismissable")]
        public bool IsDismissable;

        [JsonProperty("package_name")]
        public string PackageName;

        [JsonProperty("notification_id")]
        public string NotificationId;

        [JsonProperty("notification_tag")]
        public string NotificationTag;

        [JsonProperty("has_root")]
        public bool IsRooted;

        [JsonProperty("client_version")]
        public int ClientVersion;

        public Bitmap IconAsBitmap()
        {
            var iconBytes = Convert.FromBase64String(Icon);
            var memStream = new MemoryStream(iconBytes);
            return new Bitmap(memStream);
        }
    }
}
