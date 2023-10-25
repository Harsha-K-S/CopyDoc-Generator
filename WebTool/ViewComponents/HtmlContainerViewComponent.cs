using Microsoft.AspNetCore.Mvc;

namespace WebTool
{
    public class HtmlContainerViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(HtmlContainerViewComponentModel model)
        {
            return View(model);
        }
    }
}
