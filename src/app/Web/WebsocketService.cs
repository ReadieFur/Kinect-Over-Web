using System;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace KinectOverWeb.Web
{
    public class WebsocketService : WebSocketBehavior
    {
        public event Action<WebsocketService> onOpen;
        public event Action<WebsocketService, MessageEventArgs> onMessage;
        public event Action<WebsocketService, CloseEventArgs> onClose;
        public event Action<WebsocketService, ErrorEventArgs> onError;

        protected override void OnOpen()
        {
            if (onOpen != null) { onOpen(this); }
        }

        protected override void OnMessage(MessageEventArgs _message)
        {
            if (onMessage != null) { onMessage(this, _message); }
        }

        protected override void OnClose(CloseEventArgs _close)
        {
            if (onClose != null) { onClose(this, _close); }
        }

        protected override void OnError(ErrorEventArgs _error)
        {
            if (onError != null) { onError(this, _error); }
        }
    }
}
