using EmailService.Contracts;
using EmailService.Enums;
using EmailService.Models;

namespace EmailService.Constracts
{
    public interface ITextOptionsBuilder : IBuilder<TextOptions>
    {
        ITextOptionsBuilder SetBold();

        ITextOptionsBuilder SetItalic();

        ITextOptionsBuilder SetUnderline();

        ITextOptionsBuilder SetSize(HtmlHSize size);

        ITextOptionsBuilder SetColor(string color);
    }
}