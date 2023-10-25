using System.ComponentModel.DataAnnotations;

namespace WebTool
{
    public class HtmlElementDeleteModel
    {
        [Required]
        public string ElementId { get; set; }

        [Required]
        [Url]
        public string Url { get; set; }
    }
}
