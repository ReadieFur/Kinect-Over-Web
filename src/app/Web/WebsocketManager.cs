using System;
using System.Linq;
using System.Collections.Generic;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace KinectOverWeb.Web
{
    public class WebsocketManager
    {
        public bool enabled { private set; get; } = false;

        public readonly Protocol protocol;
        public readonly System.Net.IPAddress ip;
        public readonly int port;

        private WebSocketServer server;

        public WebsocketManager(/*Protocol _protocol,*/ System.Net.IPAddress _ip, int _port)
        {
            protocol = Protocol.ws;
            ip = _ip;
            port = _port;
            server = new WebSocketServer($"{protocol}://{ip}:{port}");
        }

        public void StartServer()
        {
            server.Start();
            enabled = true;
        }

        public void StopServer()
        {
            server.Stop();
            enabled = false;
        }

        public WebSocketServiceHost AddEndpoint(string _path, Action<WebSocketBehavior> _initializer = null)
        {
            if (server.WebSocketServices.Hosts.FirstOrDefault(_host => _host.Path == _path) != null) { return null; }
            
            server.WebSocketServices.AddService<WebsocketService>(_path, _initializer);

            /*WebSocket websocket = new WebSocket($"{protocol}://{ip}:{port}{_path}");
            while (!websocket.IsAlive) { websocket.Connect(); }
            websocket.Close();*/

            return server.WebSocketServices.Hosts.FirstOrDefault(_host => _host.Path == _path);
        }

        public bool RemoveEndpoint(string _path)
        {
            if (server.WebSocketServices.Hosts.FirstOrDefault(_host => _host.Path == _path) == null) { return false; }
            server.WebSocketServices.RemoveService(_path);
            return true;
        }

        public WebSocketServiceHost GetEndpoint(string _path)
        {
            return server.WebSocketServices.Hosts.FirstOrDefault(_host => _host.Path == _path);
        }

        public enum Protocol
        {
            ws = 0,
            wss = 1
        }
    }
}
