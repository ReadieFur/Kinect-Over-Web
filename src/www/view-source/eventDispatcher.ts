export class EventDispatcher
{
    private events: Events = {};

    public AddEventListener(event: string, callback: (data?: any) => any): boolean
    {
        if (this.events[event] !== undefined) { return false; }
        this.events[event] = { listeners: [] };
        this.events[event].listeners.push(callback);
        return true;
    }

    public RemoveEventListener(event: string, callback: (data?: any) => any): boolean
    {
        if (this.events[event] === undefined) { return false; }
        for (let i = 0; i < this.events[event].listeners.length; i++)
        { if (this.events[event].listeners[i] === callback) { delete this.events[event].listeners[i]; } }
        return true;
    }

    public DispatchEvent(event: string, data?: any): boolean
    {
        if (this.events[event] === undefined) { return false; }
        this.events[event].listeners.forEach((listener: any) => { listener(data); });
        return true;
    }
}

type Events =
{
    [eventName: string]:
    {
        listeners: { (data?: any): any; } []
    }
}