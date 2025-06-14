namespace Web.ViewModels
{
    public class ShowTimeViewModel
    {
        public TimeSpan TimeDifference { get; }
        public ShowTimeViewModel(DateTime timeOfObject)
        {
            TimeDifference = DateTime.UtcNow - timeOfObject;
        }
    }
}
