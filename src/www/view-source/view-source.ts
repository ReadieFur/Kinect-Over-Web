import { Main } from "../assets/js/main.js";
import { HeaderSlide } from "../assets/js/headerSlide.js";
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

class ViewSource
{
    private messageElement!: HTMLHeadingElement;
    private canvas!: HTMLCanvasElement;
    private canvas2D!: CanvasRenderingContext2D;
    /*private canvasWebGL!: WebGLRenderingContext;
    private uColour!: WebGLUniformLocation;
    private aVertexPosition!: number;
    private aspectRatio!: number;
    private widthMultiplier!: number;
    private heightMultiplier!: number;*/
    private source!: string;
    private websocketClient!: WebsocketClient;
    private endpoint!: IWebsocketEndpoint;

    //For some of these redirects I should alert the user instead of just redirecting.
    public async Init()
    {
        new Main();
        new HeaderSlide();

        this.messageElement = Main.ThrowIfNullOrUndefined(document.querySelector("#messageElement"));

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
        var _canvas2D: CanvasRenderingContext2D | null = this.canvas.getContext("2d");
        if (_canvas2D === null) { return; }
        this.canvas2D = _canvas2D;
        /*var _canvasWebGL: WebGLRenderingContext | null = this.canvas.getContext("webgl");
        if (_canvasWebGL === null) { Main.NavigateHome(true); }
        this.canvasWebGL = _canvasWebGL!;
        await this.InitCanvasWebGL();*/

        setInterval(() => { console.clear(); }, 600000); //Try to clear some memory every 10 mins (mainly for clearing disconnected client errors)

        this.websocketClient = new WebsocketClient(1311, Main.urlParams.get("ip"));
        this.endpoint = this.websocketClient.AddEndpoint(`/kinect-over-web/${this.source}`);
        this.endpoint.AddEventListener("open", (ev) => { this.OnOpen(ev); })
        this.endpoint.AddEventListener("message", (ev) => { this.OnMessage(ev); });
        this.endpoint.AddEventListener("close", (ev) => { this.OnClose(ev); })
        this.messageElement.innerText = "Connecting...";
        this.endpoint.Connect();

        //var message = JSON.parse(`[{"SpineBase":{"Position":{"X":1709.6333,"Y":1041.46326,"Z":0.643749058},"TrackingState":2},"SpineMid":{"Position":{"X":1703.0863,"Y":628.5443,"Z":0.69394964},"TrackingState":2},"Neck":{"Position":{"X":1699.73755,"Y":296.897339,"Z":0.7272453},"TrackingState":2},"Head":{"Position":{"X":1662.62671,"Y":90.42227,"Z":0.743000746},"TrackingState":2},"ShoulderLeft":{"Position":{"X":1815.16968,"Y":235.420654,"Z":0.6820441},"TrackingState":1},"ElbowLeft":{"Position":{"X":1520.90112,"Y":450.972168,"Z":0.725537539},"TrackingState":1},"WristLeft":{"Position":{"X":1770.03528,"Y":672.4723,"Z":0.5716434},"TrackingState":2},"HandLeft":{"Position":{"X":1745.5033,"Y":717.9655,"Z":0.549292564},"TrackingState":2},"ShoulderRight":{"Position":{"X":1750.49292,"Y":486.0276,"Z":0.6698719},"TrackingState":1},"ElbowRight":{"Position":{"X":1727.0907,"Y":761.706238,"Z":0.550148},"TrackingState":2},"WristRight":{"Position":{"X":1717.19458,"Y":425.2526,"Z":0.7015094},"TrackingState":2},"HandRight":{"Position":{"X":1722.93542,"Y":352.3387,"Z":0.698337734},"TrackingState":1},"HipLeft":{"Position":{"X":1712.91675,"Y":974.9962,"Z":0.6245679},"TrackingState":2},"KneeLeft":{"Position":{"X":1489.948,"Y":747.3233,"Z":0.8766317},"TrackingState":1},"AnkleLeft":{"Position":{"X":1305.44824,"Y":366.765839,"Z":0.821504354},"TrackingState":1},"FootLeft":{"Position":{"X":1470.524,"Y":430.574951,"Z":0.693843067},"TrackingState":1},"HipRight":{"Position":{"X":1710.08923,"Y":1111.07471,"Z":0.6100365},"TrackingState":2},"KneeRight":{"Position":{"X":"-Infinity","Y":"-Infinity","Z":0.549630165},"TrackingState":1},"AnkleRight":{"Position":{"X":"-Infinity","Y":"-Infinity","Z":0.515213},"TrackingState":1},"FootRight":{"Position":{"X":"-Infinity","Y":"-Infinity","Z":0.5413592},"TrackingState":1},"SpineShoulder":{"Position":{"X":1700.26587,"Y":373.6996,"Z":0.721021235},"TrackingState":2},"HandTipLeft":{"Position":{"X":1773.75793,"Y":871.6687,"Z":0.5137372},"TrackingState":2},"ThumbLeft":{"Position":{"X":1795.93616,"Y":718.137939,"Z":0.516750038},"TrackingState":2},"HandTipRight":{"Position":{"X":1700.73035,"Y":341.144348,"Z":0.7350642},"TrackingState":2},"ThumbRight":{"Position":{"X":1754.71375,"Y":323.023224,"Z":0.6552121},"TrackingState":2}}]`);
        //this.OnMessage(message);
    }

    private OnOpen(_ev: any)
    {
        this.messageElement.style.display = "none";
    }

    //#region Canvas2D
    private OnClose(_ev: any)
    {
        this.canvas2D.clearRect(0, 0, this.canvas.width, this.canvas.height);
        this.messageElement.style.display = "block";
        this.messageElement.innerText = "Client not connected";
    }

    private OnMessage(_ev: object): void
    {
        if ((<IFrameSourceTypeDescriptor>(<KeyValuePair>FrameSourceTypes)[this.source]).type == "Point")
        {
            //Add user customisation for point data. E.g. change the colours of all people or individual people e.t.c.
            var bodies: Joints[] = _ev as Joints[];
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
            (_joint.TrackingState == TrackingState.Inferred ? 10 : 20) * (1 / (_joint.Position.Z < 0 ? 0 : _joint.Position.Z)),
            0,
            2 * Math.PI,
            false
        );
        this.canvas2D.fillStyle = _joint.TrackingState == TrackingState.Inferred ? "#FFFF00" : "#00FF00";
        this.canvas2D.fill();
        this.canvas2D.stroke();
        this.canvas2D.closePath();
    }
    //#endregion

    //#region CanvasWebGL
    //This was my 'first' time using WebGL, so there will be lots of comments.
    //I am attempting WebGL here in hopes that it prforms better when using the GPU to render the image.
    //I had tested most of this in another folder which I made these comments in,
    //but I don't want to upload that as its own GitHub repo so for my first public WebGL set of code I will be leaving my comments in here (some comments have been stripped).
    //The tutorial I followed for this basic WebGL setup can be found here: https://www.creativebloq.com/javascript/get-started-webgl-draw-square-7112981

    //UPDATE/CONCLUSION: So I got this working, there were a few visual bugs that I didnt like, I will probably come back to that.
    //But anyway, which runs better, the WebGL context or 2D context? From my testing and with my code, 2D, the WebGL one RUNS LIKE ASS in comparison.
    //I wont be using WebGL for now until I optimise my code.
    //It's a little annoying that it didn't work as well as I had hoped considering the amount of time I spend looking into WebGL,
    //but at least I learnt some things about it and it could be handy in the future.
    //I may come back to this WebGL stuff and try and make it better but for now I will be using the 2D context.
    //I will be leaving this WebGL stuff in here but commented out, this project is very experimental anyway. The C# stuff is also quite messy, the NDI stuff I made also isn't used.

    /*private async InitCanvasWebGL()
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
        this.uColour = Main.ThrowIfNullOrUndefined(this.canvasWebGL.getUniformLocation(program, "uColor"));

        //Get the 'aVertexPosition' variable from the shader.
        this.aVertexPosition = this.canvasWebGL.getAttribLocation(program, "aVertexPosition");
        //Enable the static variable. (Didn't quite understand this step).
        this.canvasWebGL.enableVertexAttribArray(this.aVertexPosition);

        //Coordinate -1,-1 is the top left of the canvas.
        //Coordinate 0,0 is the center of the canvas.
        //Coordinate 1,1 is the bottom right of the canvas.
        //In WebGL there are three main drawing types: points, lines and triangles.

        this.aspectRatio = this.canvas.width / this.canvas.height;
        this.widthMultiplier = this.aspectRatio > 1 ? this.aspectRatio : 1;
        this.heightMultiplier = this.aspectRatio < 1 ? this.aspectRatio : 1;
    }

    //I could probably cut down on some of these calculations and save a bit of peformance if I did some of the calculations in the calling function so they are not repeated.
    //WebGL circle using triangles: https://github.com/davidwparker/programmingtil-webgl/tree/master/0027-drawing-a-circle
    private MarkJointWebGL(_joint: Joint): void
    {
        if
        (
            _joint.TrackingState == TrackingState.NotTracked ||
            typeof(_joint.Position.X) !== "number" ||
            typeof(_joint.Position.Y) !== "number" ||
            typeof(_joint.Position.Z) !== "number" ||
            isNaN(_joint.Position.X) ||
            isNaN(_joint.Position.Y) ||
            isNaN(_joint.Position.Z)
        )
        { return; }

        var scale = (_joint.TrackingState == TrackingState.Inferred ? 0.015 : 0.03) *
            (1 / (_joint.Position.Z < 0 ? 0 : _joint.Position.Z)); /
            ((this.aspectRatio >= 1 ? this.canvas.width : this.canvas.height) / 1); //With the default values when scaled the circle is way too small to be seen, I will come back to this device scaling.

        var vertices: number[] = [];
        //2 because we are drawing a 2D shape (2 points).
        //Is this 2 sets of two per shape?
        var vertexCount: number = 2;

        var x: number = ((_joint.Position.X * (1 - -1)) / this.canvas.width) + -1;
        var y: number = -(((_joint.Position.Y * (1 - -1)) / this.canvas.height) + -1);

        for (let i = 0; i <= 360; i++)
        {
            var j = i * Math.PI / 180;

            var outerCoordinate: [number, number] =
            [
                ((Math.sin(j) / this.widthMultiplier) * scale) + x, //X
                ((Math.cos(j) * this.heightMultiplier) * scale) + y //Y
            ];
            var innerCoordinate: [number, number] =
            [
                x,
                y
            ];

            vertices = vertices.concat(outerCoordinate);
            vertices = vertices.concat(innerCoordinate);
        }

        //Create a buffer for the data to be stored in.
        var buffer: WebGLBuffer = Main.ThrowIfNullOrUndefined(this.canvasWebGL.createBuffer());
        this.canvasWebGL.bindBuffer(this.canvasWebGL.ARRAY_BUFFER, buffer);
        this.canvasWebGL.bufferData(this.canvasWebGL.ARRAY_BUFFER, new Float32Array(vertices), this.canvasWebGL.STATIC_DRAW);

        //Write the 'vbuffer' to the 'aVertexPosition' variable.
        this.canvasWebGL.vertexAttribPointer(this.aVertexPosition, vertexCount, this.canvasWebGL.FLOAT, false, 0, 0);

        //Set the colour.
        //Set the 'uColor' variable value with 'uniform4fv' as the variable type is 'vec4'.
        this.canvasWebGL.uniform4fv(this.uColour, _joint.TrackingState === TrackingState.Inferred ? [1.0, 1.0, 0.0, 1.0] : [0.0, 1.0, 0.0, 1.0]);

        //I'm assuming that the way this works is the renderer completes the triangle from where the previous strip was placed.
        //The last parameter is the number of triangles in the array.
        this.canvasWebGL.drawArrays(this.canvasWebGL.TRIANGLE_STRIP, 0, vertices.length / vertexCount);
    }

    private JoinJointsWebGL(_joint1: Joint, _joint2: Joint): void
    {
        if
        (
            _joint1.TrackingState == TrackingState.NotTracked ||
            _joint2.TrackingState == TrackingState.NotTracked ||
            typeof(_joint1.Position.X) !== "number" ||
            typeof(_joint1.Position.Y) !== "number" ||
            typeof(_joint1.Position.Z) !== "number" ||
            typeof(_joint2.Position.X) !== "number" ||
            typeof(_joint2.Position.Y) !== "number" ||
            typeof(_joint2.Position.Z) !== "number" ||
            isNaN(_joint1.Position.X) ||
            isNaN(_joint1.Position.Y) ||
            isNaN(_joint1.Position.Z) ||
            isNaN(_joint2.Position.X) ||
            isNaN(_joint2.Position.Y) ||
            isNaN(_joint2.Position.Z)
        )
        { return; }

        var width = (_joint1.TrackingState == TrackingState.Inferred || _joint2.TrackingState == TrackingState.Inferred ? 0.004 : 0.008) *
        (((1 / (_joint1.Position.Z < 0 ? 0 : _joint1.Position.Z)) + (1 / (_joint2.Position.Z < 0 ? 0 : _joint2.Position.Z))) / 2); /
            ((this.aspectRatio >= 1 ? this.canvas.width : this.canvas.height) / 1);

        var x1: number = ((_joint1.Position.X * (1 - -1)) / this.canvas.width) + -1;
        var y1: number = -(((_joint1.Position.Y * (1 - -1)) / this.canvas.height) + -1);
        var x2: number = ((_joint2.Position.X * (1 - -1)) / this.canvas.width) + -1;
        var y2: number = -(((_joint2.Position.Y * (1 - -1)) / this.canvas.height) + -1);

        var vertexCount: number = 2;
        var rectangle:
        [
            //Triangle 1:
            number, number,
            number, number,
            number, number,
            //Triangle 2:
            number, number,
            number, number,
            number, number
        ] =
        [
            //Triangle 1:
            //Bottom
            x1 + width, y1 + width,
            //Middle
            x2 + width, y2 + width,
            //Top
            x2 - width, y2 - width,
            //Triangle 2:
            //Bottom
            x1 + width, y1 + width,
            //Middle
            x1 - width, y1 - width,
            //Top
            x2 - width, y2 - width
        ];

        var buffer: WebGLBuffer = Main.ThrowIfNullOrUndefined(this.canvasWebGL.createBuffer());
        this.canvasWebGL.bindBuffer(this.canvasWebGL.ARRAY_BUFFER, buffer);
        this.canvasWebGL.bufferData(this.canvasWebGL.ARRAY_BUFFER, new Float32Array(rectangle), this.canvasWebGL.STATIC_DRAW);
        this.canvasWebGL.vertexAttribPointer(this.aVertexPosition, vertexCount, this.canvasWebGL.FLOAT, false, 0, 0);

        this.canvasWebGL.uniform4fv(this.uColour, _joint1.TrackingState === TrackingState.Inferred || _joint2.TrackingState === TrackingState.Inferred ? [1.0, 1.0, 0.0, 1.0] : [0.0, 1.0, 0.0, 1.0]);

        this.canvasWebGL.drawArrays(this.canvasWebGL.TRIANGLES, 0, rectangle.length / vertexCount);
    }

    private OnMessage(_ev: object): void
    {
        if ((<IFrameSourceTypeDescriptor>(<KeyValuePair>FrameSourceTypes)[this.source]).type == "Point")
        {
            var bodies: Joints[] = _ev as Joints[];
            //Set the canvas colour buffer, RGBA(0-1).
            this.canvasWebGL.clearColor(0.0, 0.0, 0.0, 0.0);
            //Set the canvas background to the buffered colour.
            this.canvasWebGL.clear(this.canvasWebGL.COLOR_BUFFER_BIT);
            bodies.forEach(body =>
            {
                this.JoinJointsWebGL(body.Head, body.Neck);
                this.JoinJointsWebGL(body.Neck, body.SpineShoulder);
                this.JoinJointsWebGL(body.SpineShoulder, body.ShoulderLeft);
                this.JoinJointsWebGL(body.SpineShoulder, body.ShoulderRight);
                this.JoinJointsWebGL(body.SpineShoulder, body.SpineMid);
                this.JoinJointsWebGL(body.ShoulderLeft, body.ElbowLeft);
                this.JoinJointsWebGL(body.ShoulderRight, body.ElbowRight);
                this.JoinJointsWebGL(body.ElbowLeft, body.WristLeft);
                this.JoinJointsWebGL(body.ElbowRight, body.WristRight);
                this.JoinJointsWebGL(body.WristLeft, body.HandLeft);
                this.JoinJointsWebGL(body.WristRight, body.HandRight);
                this.JoinJointsWebGL(body.HandLeft, body.HandTipLeft);
                this.JoinJointsWebGL(body.HandRight, body.HandTipRight);
                this.JoinJointsWebGL(body.WristLeft, body.ThumbLeft);
                this.JoinJointsWebGL(body.WristRight, body.ThumbRight);
                this.JoinJointsWebGL(body.SpineMid, body.SpineBase);
                this.JoinJointsWebGL(body.SpineBase, body.HipLeft);
                this.JoinJointsWebGL(body.SpineBase, body.HipRight);
                this.JoinJointsWebGL(body.HipLeft, body.KneeLeft);
                this.JoinJointsWebGL(body.HipRight, body.KneeRight);
                this.JoinJointsWebGL(body.KneeLeft, body.AnkleLeft);
                this.JoinJointsWebGL(body.KneeRight, body.AnkleRight);
                this.JoinJointsWebGL(body.AnkleLeft, body.FootLeft);
                this.JoinJointsWebGL(body.AnkleRight, body.FootRight);

                Object.keys(body).forEach(jointKey => { this.MarkJointWebGL((<any>body)[jointKey] as Joint); });
            });
        }
    }*/
    //#endregion
}
new ViewSource().Init();

type KeyValuePair = {[key: string]: any}