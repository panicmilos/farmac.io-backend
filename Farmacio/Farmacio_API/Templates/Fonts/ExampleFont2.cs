using EmailService.Constracts;
using EmailService.Enums;
using EmailService.Models;

namespace Farmacio_API.Templates.Fonts
{
    public class ExampleFont2 : ITemplate<TextOptions>
    {
        public string Name { get; } = "Font2";

        public TextOptions GetTemplate()
        {
            return new TextOptions
            {
                ShouldBeBold = false,
                ShouldBeItalic = true,
                ShouldBeUnderline = true,
                Size = HtmlHSize.H1,
                Color = "red"
            };
        }
    }
}