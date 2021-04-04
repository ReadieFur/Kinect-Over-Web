using Microsoft.Kinect;

namespace KinectOverNDI.Kinect
{
    public class FrameSources
    {
        //I could use this but I wont for now as I want to use the function.
        //public bool None { get; private set; } = true;
        //public bool Colour { get; private set; } = false;
        //public bool Infrared { get; private set; } = false;
        //public bool LongExposureInfrared { get; private set; } = false;
        //public bool Depth { get; private set; } = false;
        //public bool BodyIndex { get; private set; } = false;
        //public bool Body { get; private set; } = false;
        //public bool Audio { get; private set; } = false;

        //private bool None = true;
        private bool Colour = false;
        //private bool Infrared = false;
        //private bool LongExposureInfrared = false;
        //private bool Depth = false;
        //private bool BodyIndex = false;
        public bool Body { get; private set; } = false;
        //public bool Audio { get; private set; } = false;

        public void AddSource(FrameSourceTypes _source)
        {
            ChangeSource(_source, true);
        }

        public void RemoveSource(FrameSourceTypes _source)
        {
            ChangeSource(_source, false);
        }

        public bool IsSourceEnabled(FrameSourceTypes _source)
        {
            switch (_source)
            {
                /*case FrameSourceTypes.None:
                    return None;*/
                case FrameSourceTypes.Color:
                    return Colour;
                /*case FrameSourceTypes.Infrared:
                    return Infrared;*/
                /*case FrameSourceTypes.LongExposureInfrared:
                    return LongExposureInfrared;
                /*case FrameSourceTypes.Depth:
                    return Depth;*/
                /*case FrameSourceTypes.BodyIndex:
                    return BodyIndex;*/
                case FrameSourceTypes.Body:
                    return Body;
                /*case FrameSourceTypes.Audio:
                    return Audio;*/
                default:
                    return false;
            }
        }

        private void ChangeSource(FrameSourceTypes _source, bool _enabled)
        {
            switch (_source)
            {
                /*case FrameSourceTypes.None:
                    None = _enabled;
                    break;*/
                case FrameSourceTypes.Color:
                    Colour = _enabled;
                    break;
                /*case FrameSourceTypes.Infrared:
                    Infrared = _enabled;
                    break;*/
                /*case FrameSourceTypes.LongExposureInfrared:
                    LongExposureInfrared = _enabled;
                    break;*/
                /*case FrameSourceTypes.Depth:
                    Depth = _enabled;
                    break;*/
                /*case FrameSourceTypes.BodyIndex:
                    BodyIndex = _enabled;
                    break;*/
                case FrameSourceTypes.Body:
                    Body = _enabled;
                    break;
                /*case FrameSourceTypes.Audio:
                    Audio = _enabled;
                    break;*/
            }
        }
    }
}
