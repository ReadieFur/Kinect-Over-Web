import { EventDispatcher } from './eventDispatcher.js';

export class WebsocketClient
{
    public readonly protocol: "wss" | "ws";
    public readonly ip: string;
    public readonly port: number;

    private endpoints: {[key: string]: WebsocketEndpoint};

    constructor(_port: number, _ip?: string | null | undefined)
    {
        this.port = _port;

        if (_ip === undefined || _ip === null)
        {
            this.ip = "127.0.0.1";
        }
        else if (RegExp(/\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}/).test(_ip))
        {
            this.ip = _ip;
        }
        else
        {
            throw new SyntaxError("Invalid IP");
        }

        this.protocol = "ws"; //For now the protocol will be forced to 'WS'.
        this.endpoints = {};
    }

    public AddEndpoint(endpoint: string): WebsocketEndpoint
    {
        return this.endpoints[endpoint] = new WebsocketEndpoint(this.protocol, this.ip, this.port, endpoint);
    }

    public RemoveEndpoint(endpoint: string): void
    {
        delete this.endpoints[endpoint];
    }

    public GetEndpoint(endpoint: string): WebsocketEndpoint
    {
        return this.endpoints[endpoint];
    }
}

class WebsocketEndpoint
{
    private readonly protocol: string;
    private readonly ip: string;
    private readonly port: number;
    private readonly path: string;
    private eventDispatcher: EventDispatcher;
    private websocket: WebSocket | null;

    constructor(_protocol: string, _ip: string, _port: number, _path: string)
    {
        this.protocol = _protocol;
        this.ip = _ip;
        this.port = _port;
        this.path = _path;
        this.eventDispatcher = new EventDispatcher();
        this.websocket = null;
    }

    public Connect(): void
    {
        this.websocket = new WebSocket(`${this.protocol}://${this.ip}:${this.port}${this.path}`);
        this.websocket.onopen = (ev: Event) => { this.OnOpen(ev); };
        this.websocket.onclose = (ev: Event) => { this.OnClose(ev); };
        this.websocket.onerror = (ev: Event) => { this.OnError(ev); };
        this.websocket.onmessage = (ev: MessageEvent<any>) => { this.OnMessage(ev); };
    }

    public AddEventListener(event: "open" | "close" | "error" | "message", callback: (data?: any) => any): void
    {
        this.eventDispatcher.AddEventListener(event, callback);
    }

    public RemoveEventListener(event: "open" | "close" | "error" | "message", callback: (data?: any) => any): void
    {
        this.eventDispatcher.RemoveEventListener(event, callback);
    }

    private OnOpen(ev: Event): void
    {
        this.eventDispatcher.DispatchEvent("open", ev);
    }

    private OnClose(ev: Event): void
    {
        this.eventDispatcher.DispatchEvent("close", ev);
        setTimeout(() => { this.Connect(); }, 5000);
    }

    private OnError(ev: Event): void
    {
        this.eventDispatcher.DispatchEvent("error", ev);
        //setTimeout(() => { this.Connect(); }, 5000); //This is causing the websocket to fill up memory.
    }

    private OnMessage(ev: MessageEvent<any>): void
    {
        var jsonData: object = JSON.parse(ev.data);
        this.eventDispatcher.DispatchEvent("message", jsonData);
    }
}

export interface IWebsocketEndpoint
{
    Connect(): void,
    AddEventListener(event: "open" | "close" | "error" | "message", callback: (data?: any) => any): void,
    RemoveEventListener(event: "open" | "close" | "error" | "message", callback: (data?: any) => any): void
}

//#region Interfaces
//I didnt bother with creating them all, so quite a few of the interfaces have missing properties.
export interface Joints
{
    SpineBase: Joint;
    SpineMid: Joint;
    Neck: Joint;
    Head: Joint;
    ShoulderLeft: Joint;
    ElbowLeft: Joint;
    WristLeft: Joint;
    HandLeft: Joint;
    ShoulderRight: Joint;
    ElbowRight: Joint;
    WristRight: Joint;
    HandRight: Joint;
    HipLeft: Joint;
    KneeLeft: Joint;
    AnkleLeft: Joint;
    FootLeft: Joint;
    HipRight: Joint;
    KneeRight: Joint;
    AnkleRight: Joint;
    FootRight: Joint;
    SpineShoulder: Joint;
    HandTipLeft: Joint;
    ThumbLeft: Joint;
    HandTipRight: Joint;
    ThumbRight: Joint;
}
  
export interface Joint
{
    Position: Position;
    TrackingState: number;
}
  
export interface Position
{
    X: number | string;
    Y: number | string;
    Z: number | string;
}

export enum TrackingState
{
    NotTracked = 0,
    Inferred = 1,
    Tracked = 2
}
//#endregion