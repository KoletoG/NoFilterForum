namespace Web.ViewModels.VIP
{
    public class VIPIndexViewModel
    {
        public required bool isVIP {  get; set; }
        public IEnumerable<string> Traits { get; set; }
        public VIPIndexViewModel() 
        {
            Traits = new List<string>();
        }
    }
}
