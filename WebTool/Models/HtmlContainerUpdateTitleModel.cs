using System.ComponentModel.DataAnnotations;

namespace WebTool
{
    public class HtmlContainerUpdateTitleModel
    {
        [Required]
        public string ContainerId { get; set; }

        [Required]
        public string ContainerTitle { get; set; }

        [Required]
        [Url]
        public string Url { get; set; }
    }
}
