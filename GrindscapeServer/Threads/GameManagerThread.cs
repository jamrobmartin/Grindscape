using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrindscapeServer.Threads
{
    public class GameManagerThread
    {
        private Thread thread;
        private bool isRunning;
        private object lockObject = new object();

        public GameManagerThread()
        {
            thread = new Thread(GameManagementLoop);
            isRunning = false;
        }

        public void Start()
        {
            lock (lockObject)
            {
                if (!isRunning)
                {
                    isRunning = true;
                    thread = new Thread(GameManagementLoop);
                    thread.Start();
                    Debug.WriteLine($"GameManagementLoop Started!");
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
                    Debug.WriteLine($"GameManagementLoop Stopped!");
                }
            }
        }

        private void GameManagementLoop()
        {
            while (isRunning)
            {
                // Client management logic goes here
                // This loop continues until isRunning is set to false
                Thread.Sleep(1000);
            }
        }
    }

}
