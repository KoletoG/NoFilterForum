namespace Web.ViewModels
{
    public class PagingViewModel
    {
        public int Page {  get; private init; }
        public int TotalPages { get; private init; }
        public string ControllerName { get; private init; }
        public string ActionName { get; private init; }
        public PagingViewModel(int page, int totalPages, string controllerName, string actionName)
        {
            Page = page;
            TotalPages = totalPages;
            ControllerName = controllerName;
            ActionName = actionName;
        }
    }
}
