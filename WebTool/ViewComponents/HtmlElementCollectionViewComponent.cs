using Microsoft.AspNetCore.Mvc;

namespace WebTool
{
    public class HtmlElementCollectionViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(HtmlElementCollection elements, string containerId, HtmlContainer parent)
        {
            ViewBag.ContainerId = containerId;
            ViewBag.Parent = parent;

            return View(elements);
        }
    }
}
