using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrindscapeServer.Threads
{
    public class ClientManagerThread
    {
        private Thread thread;
        private bool isRunning;
        private object lockObject = new object();

        public ClientManagerThread()
        {
            thread = new Thread(ClientManagementLoop);
            isRunning = false;
        }

        public void Start()
        {
            lock (lockObject)
            {
                if (!isRunning)
                {
                    isRunning = true;
                    thread = new Thread(ClientManagementLoop);
                    thread.Start();
                    Debug.WriteLine($"ClientManagementLoop Started!");
                }
            }
        }

        public void Stop()
        {
            lock (lockObject)
            {
                if (isRunning)
                {
                    isRunning = false;
                    // Implement logic to gracefully stop the client manager thread
                    thread.Join(); // Wait for the thread to finish
                    Debug.WriteLine($"ClientManagementLoop Stopped!");
                }
            }
        }

        private void ClientManagementLoop()
        {
            while (isRunning)
            {
                // Client management logic goes here
                // This loop continues until isRunning is set to false
            }
        }
    }

}
