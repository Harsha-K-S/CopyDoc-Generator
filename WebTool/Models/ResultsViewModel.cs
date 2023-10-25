namespace WebTool
{
    public class ResultsViewModel
    {
        public HtmlContainerCollection Containers { get; set; }
        public Request Request { get; set; }

        public ResultsViewModel() { }

        public ResultsViewModel(HtmlContainerCollection containers, Request request)
        {
            Containers = containers;
            Request = request;
        }
    }
}