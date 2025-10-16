using System;

public interface IBindableEvent<T>
{
    Action<T> OnEvent { get; set; }
    Action OnEventNoPass { get; set; }
}

public class BindableEvent<T> : IBindableEvent<T> where T : IEvent
{
    public Action<T> onEvent { get; set; } = P => { };
    public Action onEventNoPass { get; set; } = () => { };

    Action<T> IBindableEvent<T>.OnEvent
    {
        get => this.onEvent;
        set => this.onEvent = value;
    }

    Action IBindableEvent<T>.OnEventNoPass
    {
        get => this.onEventNoPass;
        set => this.onEventNoPass = value;
    }
    
    public BindableEvent(Action<T> onEvent) { this.onEvent = onEvent; }
    public BindableEvent(Action onEvent) { this.onEventNoPass = onEvent; }
    
    public void HookAction(Action<T> onEvent) { this.onEvent += onEvent; }
    public void UnhookAction(Action<T> onEvent) { this.onEvent -= onEvent; }
}
