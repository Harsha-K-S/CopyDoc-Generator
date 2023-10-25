using System.ComponentModel.DataAnnotations;

namespace WebTool
{
    public class HtmlElementCloneModel
    {
        [Required]
        public string ContainerId { get; set; }

        [Required]
        [Url]
        public string Url { get; set; }
    }
}
