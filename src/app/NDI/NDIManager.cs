using System.Collections.Generic;

namespace KinectOverWeb.NDI
{
    public class NDIManager
    {
        private Dictionary<string, NDIStream> streams;

        public NDIManager()
        {
            streams = new Dictionary<string, NDIStream>();
        }

        public NDIStream CreateStream(string _name, int _width, int _height, int _framerate)
        {
            if (streams.ContainsKey(_name)) { return null; }
            NDIStream stream = new NDIStream(_name, _width, _height, _framerate);
            streams.Add(_name, stream);
            return stream;
        }

        public bool RemoveStream(string _name)
        {
            streams.TryGetValue(_name, out NDIStream stream);
            if (stream == null) { return false; }
            stream.Dispose();
            streams.Remove(_name);
            return true;
        }

        public NDIStream GetStream(string _name)
        {
            streams.TryGetValue(_name, out NDIStream stream);
            return stream;
        }
    }
}
