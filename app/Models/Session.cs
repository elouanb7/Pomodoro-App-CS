namespace app.Models
{
    public class Session
    {
        public string Name { get; set; }
        public int TotalWorkTime { get; set; } = 0;
        public int TotalBreakTime { get; set; } = 0;
        public int RealWorkTime { get; set; } = 0;
        public int RealBreakTime { get; set; } = 0;
        public DateTime DateTime { get; set; }

        public Session()
        {
        }
    }
}