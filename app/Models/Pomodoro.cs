using System;
using System.Timers;

namespace app.Models
{
    public class Pomodoro
    {
        public int WorkDuration { get; private set; } = 15; // Default work duration in minutes
        public int BreakDuration { get; private set; } = 5; // Default break duration in minutes
        public TimeSpan TimeRemaining { get; private set; }
        public bool IsLongProgram { get; private set; }
        public Programs Program { get; private set; } = Programs.Short;
        public Steps Step { get; private set; } = Steps.Work;
        public bool TimerEnabled => _timer?.Enabled ?? false;

        public bool TimerNull => _timer == null ? true : false;


        private System.Timers.Timer? _timer;

        public event Action? OnTimerTick;
        public event Action<string>? OnNotify;

        public enum Programs
        {
            Short,
            Long
        }

        public enum Steps
        {
            Work,
            Break
        }

        public void StartTimer()
        {
            StopTimer();
            Step = Steps.Work;
            TimeRemaining = TimeSpan.FromMinutes(WorkDuration);
            _timer = new System.Timers.Timer(1000);
            _timer.Elapsed += TimerTick;
            _timer.Start();
        }

        public void StartBreak()
        {
            StopTimer();
            Step = Steps.Break;
            TimeRemaining = TimeSpan.FromMinutes(BreakDuration);
            _timer = new System.Timers.Timer(1000);
            _timer.Elapsed += TimerTick;
            _timer.Start();
        }

        public void Pause()
        {
            _timer?.Stop();
        }

        public void Unpause()
        {
            _timer?.Start();
        }

        public void StopTimer()
        {
            if (_timer == null) return;
            _timer.Stop();
            _timer.Dispose();
            _timer = null;
        }

        public void ChangeProgram()
        {
            IsLongProgram = !IsLongProgram;
            if (IsLongProgram)
            {
                Program = Programs.Long;
                WorkDuration = 45;
                BreakDuration = 15;
            }
            else
            {
                Program = Programs.Short;
                WorkDuration = 15;
                BreakDuration = 5;
            }
        }

        private void TimerTick(object sender, ElapsedEventArgs e)
        {
            if (TimeRemaining.TotalSeconds <= 0)
            {
                // Timer finished
                StopTimer();
                if (Step == Steps.Work)
                {
                    Notify("Work time elapsed! Break time starting.");
                    StartBreak();
                }
                else
                {
                    Notify("Break time elapsed! Please start work session manually.");
                    Step = Steps.Work;
                }
            }
            else
            {
                TimeRemaining = TimeRemaining.Subtract(TimeSpan.FromSeconds(1));
                OnTimerTick?.Invoke();
            }
        }

        private void Notify(string message)
        {
            OnNotify?.Invoke(message);
        }
    }
}