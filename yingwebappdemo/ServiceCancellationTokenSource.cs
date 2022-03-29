namespace yingwebappdemo
{
    using System.Threading;
    using System;
    public class ServiceCancellationTokenSource : CancellationTokenSource
    {
        private int executionCount;
        private bool cancelTask;
        private int remainingWaitTime;      // time in seconds
        private readonly int cancelTime;              // time in seconds
        private readonly int sleepTime;      // time in seconds

        public ServiceCancellationTokenSource(int waitTime, int cancelTime, int sleepTime)
        {
            this.executionCount = 0;
            this.remainingWaitTime = waitTime;
            this.cancelTime = cancelTime;
            this.sleepTime = sleepTime;
        }

        public void IncrementExecutionCount()
        {
            Interlocked.Increment(ref executionCount);
        }
        public void DecrementExecutionCount()
        {
            Interlocked.Decrement(ref executionCount);
        }

        public int ExecutionCount => executionCount;

        public bool CancleTask()
        {
            return cancelTask;
        }

        public void WaitAndExit(int minWaitTimeInSeconds = 1)
        {
            Console.WriteLine($"The number of pending task: {executionCount}");

            // Sleep miniWaitTimeInSeconds secs before checking the execution count.
            Thread.Sleep(minWaitTimeInSeconds * 1000);

            // wait to allow some on going task to finish
            // set canceltask flag when there it below the threshold
            while (remainingWaitTime > 0 && executionCount > 0)
            {
                Console.WriteLine($"WaitAndExit - remainingWaitTime: {remainingWaitTime}, cancelTime: {cancelTime}, sleepTime: {sleepTime}, executionCount: {executionCount}");

                remainingWaitTime -= sleepTime;
                if (remainingWaitTime < cancelTime)
                {
                    cancelTask = true;
                }
                Thread.Sleep(sleepTime * 1000);
            }
        }
    }
}
