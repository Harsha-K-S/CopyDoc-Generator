using System.ComponentModel.DataAnnotations;

namespace WebTool
{
    public class HtmlElementUpdateModel
    {
        [Required]
        public HtmlElement Element { get; set; }

        [Required]
        [Url]
        public string Url { get; set; }
    }
}
