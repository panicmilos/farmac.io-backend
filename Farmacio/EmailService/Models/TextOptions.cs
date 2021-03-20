using EmailService.Enums;

namespace EmailService.Models
{
    public class TextOptions
    {
        public bool ShouldBeBold { get; set; }
        public bool ShouldBeItalic { get; set; }
        public bool ShouldBeUnderline { get; set; }
        public HtmlHSize? Size { get; set; }
        public string Color { get; set; }
    }
}