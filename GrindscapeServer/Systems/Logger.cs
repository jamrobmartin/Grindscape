﻿using System.Collections.Concurrent;
using System.Diagnostics;


namespace GrindscapeServer.Systems
{
    public class Logger : ISystem
    {
        #region SystemInterface
        public string SystemName => "Logger";

        public ESystemType SystemType => ESystemType.Singleton;

        private ESystemStatus _systemStatus = ESystemStatus.Uninitialized;
        public ESystemStatus SystemStatus
        {
            get => _systemStatus;
            set
            {
                if (_systemStatus != value)
                {
                    _systemStatus = value;
                    OnSystemStatusChanged(new SystemStatusChangedEventArgs(value));
                }
            }
        }

        public event EventHandler<SystemStatusChangedEventArgs>? SystemStatusChanged;

        protected virtual void OnSystemStatusChanged(SystemStatusChangedEventArgs e)
        {
            SystemStatusChanged?.Invoke(this, e);
        }

        public ManualResetEvent SystemShutdownEvent { get; set; } = new ManualResetEvent(false);

        public void InitializeSystem()
        {
            Initialize();
        }

        public void StartSystem()
        {
            // This System is always on, so nothing to do on Start/Stop
        }

        public void StopSystem()
        {
            // This System is always on, so nothing to do on Start/Stop
        }

        public void ShutdownSystem()
        {
            SystemShutdownEvent.Reset();
            _ = ShutdownAsync();
        }

        public void UpdateSystemState()
        {

            SystemStatus = Initialized ? ESystemStatus.Initialized : ESystemStatus.Uninitialized;

            if (SystemStatus == ESystemStatus.Uninitialized)
            {
                SystemShutdownEvent.Set();
            }
        }
        #endregion

        #region Logger Message
        public enum LogLevel
        {
            Debug,
            Info,
            Warning,
            Error
        }

        public class LoggerMessage
        {
            public LogLevel LogLevel;
            public string System;
            public string Message;

            public LoggerMessage()
            {
                LogLevel = LogLevel.Debug;
                System = "Default";
                Message = "Default";
            }

            public LoggerMessage(LogLevel logLevel, string system, string message)
            {
                LogLevel = logLevel;
                System = system;
                Message = message;
            }
        }

        public class LoggerMessageEventArgs(LoggerMessage message) : EventArgs
        {
            public LoggerMessage Message { get; private set; } = message;
        }
        #endregion

        #region Event
        public event EventHandler<LoggerMessageEventArgs> MessageLogged = delegate { };

        public void RegisterMessageLoggedEventHandler(EventHandler<LoggerMessageEventArgs> handler)
        {
            MessageLogged += handler;
        }

        public void UnRegisterMessageLoggedEventHandler(EventHandler<LoggerMessageEventArgs> handler)
        {
            MessageLogged -= handler;
        }
        #endregion

        #region Singleton
        private static readonly Lazy<Logger> instance = new(() => new Logger(), LazyThreadSafetyMode.ExecutionAndPublication);
        public static Logger Instance { get; private set; } = instance.Value;
        #endregion

        #region Fields and Properties
        private readonly ConcurrentQueue<LoggerMessage> logQueue = new();
        private string LogFilePath = "log_default.txt";
        private CancellationTokenSource? cancellationTokenSource;
        private Task? LoggerLoopTask;

        public bool Initialized { get; private set; } = false;

        #endregion

        #region Constructor

        // Private constructor to prevent external instantiation
        private Logger()
        {

        }

        #endregion

        #region Public Methods
        public bool Initialize()
        {
            if (!Initialized)
            {
                // In the case that the Logs directory doesnt exist yet, create it
                Directory.CreateDirectory("Logs");

                // Set the log file path with a timestamp prefix
                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                LogFilePath = $"Logs/log_{timestamp}.txt";
                File.Create(LogFilePath).Close();

                if (File.Exists(LogFilePath))
                {
                    Initialized = true;
                }

                cancellationTokenSource = new();
                // Start a background task to handle logging
                LoggerLoopTask = Task.Factory.StartNew(LogMessageLoop, cancellationTokenSource.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);

                // Figure out how to turn this off in releases.
                // Debugging 
                if (true)
                {
                    // First we send 4 test messages that should get displayed on the System Logger

                    Logger.LoggerMessage message1 = new()
                    {
                        Message = "Test!",
                        System = SystemName,
                        LogLevel = Logger.LogLevel.Debug
                    };
                    Logger.Instance.LogMessage(message1);

                    Logger.LoggerMessage message2 = new()
                    {
                        Message = "Test!",
                        System = SystemName,
                        LogLevel = Logger.LogLevel.Info
                    };
                    Logger.Instance.LogMessage(message2);

                    Logger.LoggerMessage message3 = new()
                    {
                        Message = "Test!",
                        System = SystemName,
                        LogLevel = Logger.LogLevel.Warning
                    };
                    Logger.Instance.LogMessage(message3);

                    Logger.LoggerMessage message4 = new()
                    {
                        Message = "Test!",
                        System = SystemName,
                        LogLevel = Logger.LogLevel.Error
                    };
                    Logger.Instance.LogMessage(message4);

                    // Then we send 4 test messages that should get displayed on the Client Logger

                    Logger.LoggerMessage message5 = new()
                    {
                        Message = "Test!",
                        System = "ClientConsole",
                        LogLevel = Logger.LogLevel.Debug
                    };
                    Logger.Instance.LogMessage(message5);

                    Logger.LoggerMessage message6 = new()
                    {
                        Message = "Test!",
                        System = "ClientConsole",
                        LogLevel = Logger.LogLevel.Info
                    };
                    Logger.Instance.LogMessage(message6);

                    Logger.LoggerMessage message7 = new()
                    {
                        Message = "Test!",
                        System = "ClientConsole",
                        LogLevel = Logger.LogLevel.Warning
                    };
                    Logger.Instance.LogMessage(message7);

                    Logger.LoggerMessage message8 = new()
                    {
                        Message = "Test!",
                        System = "ClientConsole",
                        LogLevel = Logger.LogLevel.Error
                    };
                    Logger.Instance.LogMessage(message8);
                }
            }

            UpdateSystemState();

            return Initialized;
        }

        public void LogMessage(LoggerMessage message)
        {
            // Enqueue the log message
            logQueue.Enqueue(message);
        }

        public async Task ShutdownAsync()
        {
            if (Initialized)
            {
                Initialized = false;

                // Signal the background task to stop and wait for it to complete
                cancellationTokenSource?.Cancel();

                if (LoggerLoopTask != null)
                    await LoggerLoopTask;

                // Flush any remaining messages
                while (logQueue.TryDequeue(out var message))
                {
                    // Log the message to the log file
                    await LogMessageToFileAsync(message);

                    // Notify any MessageLogged Event Handlers
                    LoggerMessageEventArgs e = new(message);
                    await NotifyMessageLoggedEventHandlers(e);
                }
            }

            UpdateSystemState();
        }

        #endregion

        #region Private Methods
        private async void LogMessageLoop()
        {
            if (cancellationTokenSource == null)
                return;

            while (!cancellationTokenSource.Token.IsCancellationRequested)
            {
                // The while statement will start by checking if a cancellation has been requested.
                // If it hasnt, it will enter the loop.
                // Inside the loop, we will continually dequeue messages and log them until none remain
                // After all messages have been logged, wait for a second, then loop.

                // Try to pull a message from the Queue. If there isnt one, wait a second.
                while (logQueue.TryDequeue(out var message))
                {
                    // Log the message to the log file
                    await LogMessageToFileAsync(message);

                    // Notify any MessageLogged Event Handlers
                    LoggerMessageEventArgs e = new(message);
                    await NotifyMessageLoggedEventHandlers(e);

                }

                // Sleep for a short time or use other mechanisms for controlling the loop frequency
                await Task.Delay(TimeSpan.FromSeconds(1));

            }
        }

        private async Task LogMessageToFileAsync(LoggerMessage message)
        {
            try
            {
                using StreamWriter writer = File.AppendText(LogFilePath);
                // Format the log entry
                string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{message.LogLevel}] [{message.System}] {message.Message}";
                await writer.WriteLineAsync(logEntry);

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                // Figure out a better way to log exceptions from within the logger.
            }
        }

        private async Task NotifyMessageLoggedEventHandlers(LoggerMessageEventArgs e)
        {
            try
            {
                EventHandler<LoggerMessageEventArgs> handlers = MessageLogged;

                if (handlers != null)
                {
                    foreach (EventHandler<LoggerMessageEventArgs> handler in handlers.GetInvocationList().Cast<EventHandler<LoggerMessageEventArgs>>())
                    {
                        await Task.Run(() => handler.Invoke(null, e));
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                // Figure out a better way to log exceptions from within the logger.
            }
        }

        #endregion
    }
}
