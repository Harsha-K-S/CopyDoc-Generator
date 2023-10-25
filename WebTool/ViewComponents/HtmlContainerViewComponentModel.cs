namespace WebTool
{
    public class HtmlContainerViewComponentModel
    {
        public HtmlContainer Container { get; set; }
        public HtmlContainer Parent { get; set; }

        public HtmlContainerViewComponentModel(HtmlContainer container, HtmlContainer parent)
        {
            Container = container;
            Parent = parent;
        }
    }
}
