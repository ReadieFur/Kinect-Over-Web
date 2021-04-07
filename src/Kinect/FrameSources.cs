namespace KinectOverWeb.Kinect
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
        //private bool Body = false;
        private bool BodyColour = false;
        //private bool Audio = false;

        public void AddSource(SourceTypes _source)
        {
            ChangeSource(_source, true);
        }

        public void RemoveSource(SourceTypes _source)
        {
            ChangeSource(_source, false);
        }

        public bool IsSourceEnabled(SourceTypes _source)
        {
            switch (_source)
            {
                /*case SourceTypes.None:
                    return None;*/
                case SourceTypes.Color:
                    return Colour;
                /*case SourceTypes.Infrared:
                    return Infrared;*/
                /*case SourceTypes.LongExposureInfrared:
                    return LongExposureInfrared;
                /*case SourceTypes.Depth:
                    return Depth;*/
                /*case SourceTypes.BodyIndex:
                    return BodyIndex;*/
                /*case SourceTypes.Body:
                    return Body;*/
                case SourceTypes.BodyColour:
                    return BodyColour;
                /*case SourceTypes.Audio:
                    return Audio;*/
                default:
                    return false;
            }
        }

        private void ChangeSource(SourceTypes _source, bool _enabled)
        {
            switch (_source)
            {
                /*case FrameSources.None:
                    None = _enabled;
                    break;*/
                case SourceTypes.Color:
                    Colour = _enabled;
                    break;
                /*case SourceTypes.Infrared:
                    Infrared = _enabled;
                    break;*/
                /*case SourceTypes.LongExposureInfrared:
                    LongExposureInfrared = _enabled;
                    break;*/
                /*case SourceTypes.Depth:
                    Depth = _enabled;
                    break;*/
                /*case SourceTypes.BodyIndex:
                    BodyIndex = _enabled;
                    break;*/
                /*case SourceTypes.Body:
                    Body = _enabled;
                    break;*/
                case SourceTypes.BodyColour:
                    BodyColour = _enabled;
                    break;
                    /*case SourceTypes.Audio:
                        Audio = _enabled;
                        break;*/
            }
        }

        public enum SourceTypes
        {
            //None = 0,
            Color = 1,
            //Infrared = 2,
            //LongExposureInfrared = 3,
            //Depth = 4,
            //BodyIndex = 5,
            //Body = 6,
            BodyColour = 7,
            //Audio = 8
        }
    }
}
