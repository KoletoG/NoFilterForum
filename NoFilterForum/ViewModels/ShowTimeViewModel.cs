namespace Web.ViewModels
{
    public class ShowTimeViewModel
    {
        public string? DisplayTimeAgo { get; private set; }

        public ShowTimeViewModel(DateTime timeOfObject)
        {
            TimeSpan timeDifference = DateTime.UtcNow - timeOfObject;
            
            CalculateDisplayTimeAgo(timeDifference);
        }
        private void CalculateDisplayTimeAgo(TimeSpan timediff)
        {
            double totalSeconds = timediff.TotalSeconds;
            double totalMinutes = timediff.TotalMinutes;
            double totalHours = timediff.TotalHours;
            double totalDays = timediff.TotalDays;

            if (totalSeconds < 60) DisplayTimeAgo = $"{totalSeconds:0} seconds ago";
            else if (totalMinutes < 60) DisplayTimeAgo = $"{totalMinutes:0} minutes ago";
            else if (totalHours < 24) DisplayTimeAgo = $"{totalHours:0} hours ago";
            else if (totalDays < 32) DisplayTimeAgo = $"{totalDays:0} days ago";
            else if (totalDays < 365) DisplayTimeAgo = $"{(totalDays / 30):0} months ago";
            else DisplayTimeAgo = $"{(totalDays / 365):0} years ago";
        }
    }
}
