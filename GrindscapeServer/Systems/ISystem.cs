namespace GrindscapeServer.Systems
{
    // Interface to be used for all Systems used by the Server
    public interface ISystem
    {
        string SystemName { get; }
        ESystemType SystemType { get; }
        ESystemStatus SystemStatus { get; }
        event EventHandler<SystemStatusChangedEventArgs> SystemStatusChanged;

        ManualResetEvent SystemShutdownEvent { get; }

        // Called when the application Starts
        void InitializeSystem();

        // Called when the Server Starts
        void StartSystem();

        // Called when the Server Stops
        void StopSystem();

        // Called when the application Stops
        void ShutdownSystem();

        void UpdateSystemState();
    }

    public enum ESystemStatus
    {
        Uninitialized,
        Initialized,
        Starting,
        Running,
        Paused,
        Stopping,
        Stopped,
        Terminating,
        Terminated
    }

    public enum ESystemType
    {
        Thread,
        Singleton,
        Static
    }

    // Define an event argument class
    public class SystemStatusChangedEventArgs(ESystemStatus newStatus) : EventArgs
    {
        public ESystemStatus NewStatus { get; set; } = newStatus;
    }

    // Add this to the top of a class that implements ISystem
    //#region SystemInterface
    //public string SystemName => "ExampleSystem";

    //public ESystemType SystemType => ESystemType.Thread;

    //private ESystemStatus _systemStatus = ESystemStatus.Uninitialized;
    //public ESystemStatus SystemStatus
    //{
    //    get => _systemStatus;
    //    set
    //    {
    //        if (_systemStatus != value)
    //        {
    //            _systemStatus = value;
    //            OnSystemStatusChanged(new SystemStatusChangedEventArgs(value));
    //        }
    //    }
    //}

    //public event EventHandler<SystemStatusChangedEventArgs>? SystemStatusChanged;

    //protected virtual void OnSystemStatusChanged(SystemStatusChangedEventArgs e)
    //{
    //    SystemStatusChanged?.Invoke(this, e);
    //}

    //public ManualResetEvent SystemShutdownEvent { get; set; } = new ManualResetEvent(false);

    //public void InitializeSystem()
    //{

    //}

    //public void StartSystem()
    //{

    //}

    //public void StopSystem()
    //{
    //    SystemShutdownEvent.Reset();
    //}

    //private void UpdateSystemState()
    //{

    //}
    //#endregion
}
