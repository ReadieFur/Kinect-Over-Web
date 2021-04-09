import { Main } from "../assets/js/main";
import { HeaderSlide } from "../assets/js/headerSlide";
import { WebsocketClient, IWebsocketEndpoint, Joints, Joint, Position, TrackingState } from './websocketClient.js';

class FrameSourceTypes
{
    //None = 0,
    //Colour_Camera = 1,
    //Infrared_Camera = 2,
    //Long_Exposure_Infrared_Camera = 3,
    //Depth_Camera = 4,
    //BodyIndex = 5,
    //Body_Points = 6,
    public static Body_Points_Mapped_To_Colour: IFrameSourceTypeDescriptor =
    {
        type: "Point",
        width: 1920,
        height: 1080
    }
    //Audio = 8
}

interface IFrameSourceTypeDescriptor
{
    type: "Image" | "Point",
    width: number,
    height: number
}

type KeyValuePair = {[key: string]: any}

class ViewSource
{
    private canvas!: HTMLCanvasElement;
    private canvas2D!: CanvasRenderingContext2D;
    private canvasWebGL!: WebGLRenderingContext;
    private source!: string;
    private websocketClient!: WebsocketClient;
    private endpoint!: IWebsocketEndpoint;

    //For some of these redirects I should alert the user instead of just redirecting.
    public async Init()
    {
        new Main();
        new HeaderSlide();

        var path: string[] = window.location.pathname.split('/');
        if (path.length < 3) { Main.NavigateHome(true); }
        var frameSourceKeys: string[] = Object.keys(FrameSourceTypes);
        for (let i = 0; i < frameSourceKeys.length; i++)
        {
            if (path[path.length - 2] == frameSourceKeys[i])
            {
                this.source = path[path.length - 2];
                break;
            }
        }
        if (this.source !== path[path.length - 2]) { Main.NavigateHome(true); }

        this.canvas = Main.ThrowIfNullOrUndefined(document.querySelector("#canvas"));
        var aspectRatio: number = (<IFrameSourceTypeDescriptor>(<KeyValuePair>FrameSourceTypes)[this.source]).width /
            (<IFrameSourceTypeDescriptor>(<KeyValuePair>FrameSourceTypes)[this.source]).height;
        var width: number = document.body.clientWidth;
        var height: number = document.body.clientWidth / aspectRatio;
        if (height > document.body.clientHeight)
        {
            width = document.body.clientHeight * aspectRatio;
            height = document.body.clientHeight;
        }
        this.canvas.style.width = `${width}px`;
        this.canvas.style.height = `${height}px`;
        this.canvas.width = (<IFrameSourceTypeDescriptor>(<KeyValuePair>FrameSourceTypes)[this.source]).width;
        this.canvas.height = (<IFrameSourceTypeDescriptor>(<KeyValuePair>FrameSourceTypes)[this.source]).height;
        //I could test for multiple contexts.
        /*var _canvas2D: CanvasRenderingContext2D | null = this.canvas.getContext("2d");
        if (_canvas2D === null) { return; }
        this.canvas2D = _canvas2D;*/
        var _canvasWebGL: WebGLRenderingContext | null = this.canvas.getContext("webgl");
        if (_canvasWebGL === null) { Main.NavigateHome(true); }
        this.canvasWebGL = _canvasWebGL!;
        await this.InitCanvasWebGL();

        setInterval(() => { console.clear(); }, 600000); //Try to clear some memory every 10 mins (mainly for clearing disconnected client errors)

        this.websocketClient = new WebsocketClient(1311, Main.urlParams.get("ip"));
        this.endpoint = this.websocketClient.AddEndpoint(`/kinect-over-web/${this.source}`);
        this.endpoint.AddEventListener("message", (ev) => { this.OnMessage(ev); });
        this.endpoint.AddEventListener("close", (ev) => { this.OnClose(ev); })
        this.endpoint.Connect();
    }

    private OnClose(ev: Event)
    {
        console.log(ev);
    }

    //#region Canvas2D
    /*private OnMessage(ev: object): void
    {
        if ((<IFrameSourceTypeDescriptor>(<KeyValuePair>FrameSourceTypes)[this.source]).type == "Point")
        {
            var bodies: Joints[] = ev as Joints[];
            this.canvas2D.clearRect(0, 0, this.canvas.width, this.canvas.height);
            bodies.forEach(body =>
            {
                this.JoinJoints2D(body.Head, body.Neck);
                this.JoinJoints2D(body.Neck, body.SpineShoulder);
                this.JoinJoints2D(body.SpineShoulder, body.ShoulderLeft);
                this.JoinJoints2D(body.SpineShoulder, body.ShoulderRight);
                this.JoinJoints2D(body.SpineShoulder, body.SpineMid);
                this.JoinJoints2D(body.ShoulderLeft, body.ElbowLeft);
                this.JoinJoints2D(body.ShoulderRight, body.ElbowRight);
                this.JoinJoints2D(body.ElbowLeft, body.WristLeft);
                this.JoinJoints2D(body.ElbowRight, body.WristRight);
                this.JoinJoints2D(body.WristLeft, body.HandLeft);
                this.JoinJoints2D(body.WristRight, body.HandRight);
                this.JoinJoints2D(body.HandLeft, body.HandTipLeft);
                this.JoinJoints2D(body.HandRight, body.HandTipRight);
                this.JoinJoints2D(body.WristLeft, body.ThumbLeft);
                this.JoinJoints2D(body.WristRight, body.ThumbRight);
                this.JoinJoints2D(body.SpineMid, body.SpineBase);
                this.JoinJoints2D(body.SpineBase, body.HipLeft);
                this.JoinJoints2D(body.SpineBase, body.HipRight);
                this.JoinJoints2D(body.HipLeft, body.KneeLeft);
                this.JoinJoints2D(body.HipRight, body.KneeRight);
                this.JoinJoints2D(body.KneeLeft, body.AnkleLeft);
                this.JoinJoints2D(body.KneeRight, body.AnkleRight);
                this.JoinJoints2D(body.AnkleLeft, body.FootLeft);
                this.JoinJoints2D(body.AnkleRight, body.FootRight);

                Object.keys(body).forEach(jointKey => { this.MarkJoint2D((<any>body)[jointKey] as Joint); });
            });
        }
    }

    private JoinJoints2D(_joint1: Joint, _joint2: Joint): void
    {
        if (
            _joint1.TrackingState === TrackingState.NotTracked ||
            _joint2.TrackingState === TrackingState.NotTracked ||
            typeof(_joint1.Position.X) === "string" ||
            typeof(_joint1.Position.Y) === "string" ||
            typeof(_joint1.Position.Z) === "string" ||
            typeof(_joint2.Position.X) === "string" ||
            typeof(_joint2.Position.Y) === "string" ||
            typeof(_joint2.Position.Z) === "string"
        )
        { return; }


        this.canvas2D.beginPath();
        this.canvas2D.strokeStyle = _joint1.TrackingState == TrackingState.Inferred || _joint2.TrackingState == TrackingState.Inferred ? "#FFFF00" : "#00FF00";
        this.canvas2D.lineWidth = (_joint1.TrackingState == TrackingState.Inferred || _joint2.TrackingState == TrackingState.Inferred ? 4 : 8) *
            ((1 / (_joint1.Position.Z < 0 ? 0 : _joint1.Position.Z)) + (1 / (_joint2.Position.Z < 0 ? 0 : _joint2.Position.Z)) / 2);
        this.canvas2D.moveTo(_joint1.Position.X, _joint1.Position.Y);
        this.canvas2D.lineTo(_joint2.Position.X, _joint2.Position.Y);
        this.canvas2D.stroke();
        this.canvas2D.closePath();
    }

    private MarkJoint2D(_joint: Joint)
    {
        if (
            _joint.TrackingState == TrackingState.NotTracked ||
            typeof(_joint.Position.X) === "string" ||
            typeof(_joint.Position.Y) === "string" ||
            typeof(_joint.Position.Z) === "string"
        )
        { return; }

        this.canvas2D.beginPath();
        //I quite like it with the hollow circles but I will keep them filled for now.
        this.canvas2D.strokeStyle = "transparent";
        this.canvas2D.arc(
            _joint.Position.X,
            _joint.Position.Y,
            (_joint.TrackingState == TrackingState.Inferred ? 15 : 30) * (1 / (_joint.Position.Z < 0 ? 0 : _joint.Position.Z)),
            0,
            2 * Math.PI,
            false
        );
        this.canvas2D.fillStyle = _joint.TrackingState == TrackingState.Inferred ? "#FFFF00" : "#00FF00";
        this.canvas2D.fill();
        this.canvas2D.stroke();
        this.canvas2D.closePath();
    }*/
    //#endregion

    //#region CanvasWebGL
    //This was me 'first' time using WebGL, so there will be lots of comments.
    //I had tested most of this in another folder which I made these comments in,
    //but I don't want to upload that as its own GitHub repo so for my first public WebGL set of code I will be leaving my comments in here (some comments have been stripped).
    //The tutorial I followed for this basic WebGL setup can be found here: https://www.creativebloq.com/javascript/get-started-webgl-draw-square-7112981
    private async InitCanvasWebGL()
    {
        var vertexShaderSource: string = await jQuery.ajax(
        {
            type: "GET",
            url: `${Main.WEB_ROOT}/view-source/vertex.glsl`,
            dataType: "text",
            error: Main.ThrowAJAXJsonError,
        });

        var fragmentShaderSource: string = await jQuery.ajax(
        {
            type: "GET",
            url: `${Main.WEB_ROOT}/view-source/fragment.glsl`,
            dataType: "text",
            error: Main.ThrowAJAXJsonError,
        });

        //Set the viewport size.
        this.canvasWebGL.viewport(0, 0, this.canvas.width, this.canvas.height);
        //Set the canvas colour buffer, RGBA(0-1).
        this.canvasWebGL.clearColor(0, 0, 0, 0);
        //Set the canvas background to the buffered colour.
        this.canvasWebGL.clear(this.canvasWebGL.COLOR_BUFFER_BIT);

        //Create the vertex shader variable.
        var vertexShader: WebGLShader = Main.ThrowIfNullOrUndefined(this.canvasWebGL.createShader(this.canvasWebGL.VERTEX_SHADER));
        //Set the vertex shader source code.
        this.canvasWebGL.shaderSource(vertexShader, vertexShaderSource);
        //Compile the vertex shader.
        this.canvasWebGL.compileShader(vertexShader);
        //Check if the compilier failed to build the shader.
        if (!this.canvasWebGL.getShaderParameter(vertexShader, this.canvasWebGL.COMPILE_STATUS)) { console.log(this.canvasWebGL.getShaderInfoLog(vertexShader)); }

        var fragmentShader = Main.ThrowIfNullOrUndefined(this.canvasWebGL.createShader(this.canvasWebGL.FRAGMENT_SHADER));
        this.canvasWebGL.shaderSource(fragmentShader, fragmentShaderSource);
        this.canvasWebGL.compileShader(fragmentShader);
        if (!this.canvasWebGL.getShaderParameter(fragmentShader, this.canvasWebGL.COMPILE_STATUS)) { console.log(this.canvasWebGL.getShaderInfoLog(fragmentShader)); }

        //Create the program variable.
        //This is the program that will be used to render the image in WebGL.
        var program: WebGLProgram = Main.ThrowIfNullOrUndefined(this.canvasWebGL.createProgram());
        //Add the vertex shader to the program.
        this.canvasWebGL.attachShader(program, vertexShader);
        //Add the fragment shader to the program.
        this.canvasWebGL.attachShader(program, fragmentShader);
        //Attach the program to WebGL.
        this.canvasWebGL.linkProgram(program);
        //Check if the program failed to attach to WebGL.
        if (!this.canvasWebGL.getProgramParameter(program, this.canvasWebGL.LINK_STATUS)) { console.log(this.canvasWebGL.getProgramInfoLog(program)); }

        //Tell WebGL to use the program we created earlier.
        this.canvasWebGL.useProgram(program);

        //Get the 'uColor' variable location from the shader.
        var uColor: WebGLUniformLocation = Main.ThrowIfNullOrUndefined(this.canvasWebGL.getUniformLocation(program, "uColor"));
        //Set the 'uColor' variable value with 'uniform4fv' as the variable type is 'vec4'.
        this.canvasWebGL.uniform4fv(uColor, [0.5, 0.0, 0.0, 1.0]);

        //Get the 'aVertexPosition' variable from the shader.
        var aVertexPosition: number = this.canvasWebGL.getAttribLocation(program, "aVertexPosition");
        //Enable the static variable. (Didn't quite understand this step).
        this.canvasWebGL.enableVertexAttribArray(aVertexPosition);

        //Coordinate -1,-1 is the top left of the canvas.
        //Coordinate 0,0 is the center of the canvas.
        //Coordinate 1,1 is the bottom right of the canvas.
        //In WebGL there are three main drawing types: points, lines and triangles.
    }

    private OnMessage(ev: object): void
    {

    }
    //#endregion
}
new ViewSource().Init();