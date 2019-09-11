using System;

namespace Bedrock.Shared.Utility
{
    public class Timer
    {
        #region Constructors
        private Timer() { }
        #endregion

        #region Properties
        public DateTime StartTime { get; set; }

        public DateTime StopTime { get; set; }

        public TimeSpan ElapsedTime
        {
            get { return StopTime - StartTime; }
        }

        public int ElapsedTimeMilliseconds
        {
            get { return ElapsedTime.Milliseconds; }
        }

        public double ElapsedTimeTotalMilliseconds
        {
            get { return ElapsedTime.TotalMilliseconds; }
        }

        public int ElapsedTimeSeconds
        {
            get { return ElapsedTime.Seconds; }
        }

        public double ElapsedTimeTotalSeconds
        {
            get { return ElapsedTime.TotalSeconds; }
        }

        public int ElapsedTimeMinutes
        {
            get { return ElapsedTime.Minutes; }
        }

        public double ElapsedTimeTotalMinutes
        {
            get { return ElapsedTime.TotalMinutes; }
        }
        #endregion

        #region Public Methods
        public static Timer Create(bool isStart = false)
        {
            var returnValue = new Timer();

            if (isStart)
                returnValue.Start();

            return returnValue;
        }

        public void Start()
        {
            StartTime = DateTime.Now;
        }

        public void Stop()
        {
            StopTime = DateTime.Now;
        }

        public void Reset()
        {
            StartTime = DateTime.MinValue;
            StopTime = DateTime.MinValue;
        }
        #endregion
    }
}
