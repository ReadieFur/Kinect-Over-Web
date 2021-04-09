namespace KinectOverWeb.Kinect
{
    public class FrameSources
    {
        //I could use this but I wont for now as I want to use the function.
        //public bool None { get; private set; } = true;
        //public bool Colour_Camera { get; private set; } = false;
        //public bool Infrared_Camera { get; private set; } = false;
        //public bool Long_Exposure_Infrared_Camera { get; private set; } = false;
        //public bool Depth { get; private set; } = false;
        //public bool BodyIndex { get; private set; } = false;
        //public bool Body { get; private set; } = false;
        //public bool Audio { get; private set; } = false;

        //private bool None = true;
        //private bool Colour_Camera = false;
        //private bool Infrared_Camera = false;
        //private bool Long_Exposure_Infrared_Camera = false;
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
                /*case SourceTypes.Color:
                    return Colour_Camera;*/
                /*case SourceTypes.Infrared_Camera:
                    return Infrared_Camera;*/
                /*case SourceTypes.Long_Exposure_Infrared_Camera:
                    return Long_Exposure_Infrared_Camera;
                /*case SourceTypes.Depth:
                    return Depth;*/
                /*case SourceTypes.BodyIndex:
                    return BodyIndex;*/
                /*case SourceTypes.Body:
                    return Body;*/
                case SourceTypes.Body_Points_Mapped_To_Colour:
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
                /*case SourceTypes.Color:
                    Colour_Camera = _enabled;
                    break;*/
                /*case SourceTypes.Infrared_Camera:
                    Infrared_Camera = _enabled;
                    break;*/
                /*case SourceTypes.Long_Exposure_Infrared_Camera:
                    Long_Exposure_Infrared_Camera = _enabled;
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
                case SourceTypes.Body_Points_Mapped_To_Colour:
                    BodyColour = _enabled;
                    break;
                /*case SourceTypes.Audio:
                    Audio = _enabled;
                    break;*/
            }
        }

        public enum SourceTypes
        {
            None = 0,
            //Colour_Camera = 1,
            //Infrared_Camera = 2,
            //Long_Exposure_Infrared_Camera = 3,
            //Depth_Camera = 4,
            //BodyIndex = 5,
            //Body_Points = 6,
            Body_Points_Mapped_To_Colour = 7,
            //Audio = 8
        }
    }
}
