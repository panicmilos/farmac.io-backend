using EmailService.Constracts;
using EmailService.Models;

namespace Farmacio_API.Templates.Fonts
{
    public class ExampleFont1 : ITemplate<TextOptions>
    {
        public string Name { get; } = "Font1";

        public TextOptions GetTemplate(params object[] templateParams)
        {
            return new TextOptions
            {
                ShouldBeBold = true,
                ShouldBeItalic = false,
                ShouldBeUnderline = false,
                Color = "green"
            };
        }
    }
}