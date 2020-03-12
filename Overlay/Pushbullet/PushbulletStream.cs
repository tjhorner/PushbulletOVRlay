using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Websocket.Client;

namespace PushbulletOVRlay.Pushbullet
{
    class PushbulletStream
    {
        public event EventHandler<Push> MessageReceived;
        private string apiKey;
        private WebsocketClient ws;

        public PushbulletStream(string apiKey)
        {
            this.apiKey = apiKey;
        }

        public void Start()
        {
            ws = new WebsocketClient(new Uri($"wss://stream.pushbullet.com/websocket/{apiKey}"));
            ws.MessageReceived.Subscribe(OnWebSocketMessage);
            ws.Start();
        }

        private void OnWebSocketMessage(ResponseMessage resp)
        {
            var msg = JsonConvert.DeserializeObject<PushbulletMessage>(resp.Text);
            if(msg.MessageType == "push" && msg.Push.PushType == "mirror")
            {
                MessageReceived.Invoke(this, msg.Push);
            }
        }

        public void Stop()
        {
            if(ws == null) return;
            //ws.Stop(System.Net.WebSockets.WebSocketCloseStatus.)
            ws = null;
        }
    }
}
