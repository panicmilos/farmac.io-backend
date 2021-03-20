using EmailService.Constracts;
using EmailService.Enums;
using EmailService.Models;

namespace EmailService.Implementation
{
    public class TextOptionsBuilder : ITextOptionsBuilder
    {
        private bool _shouldBeBold;
        private bool _shouldBeItalic;
        private bool _shouldBeUnderline;
        private HtmlHSize? _size;
        private string _color;

        public TextOptionsBuilder()
        {
        }

        public ITextOptionsBuilder SetBold()
        {
            _shouldBeBold = true;

            return this;
        }

        public ITextOptionsBuilder SetItalic()
        {
            _shouldBeItalic = true;

            return this;
        }

        public ITextOptionsBuilder SetUnderline()
        {
            _shouldBeUnderline = true;

            return this;
        }

        public ITextOptionsBuilder SetSize(HtmlHSize size)
        {
            _size = size;

            return this;
        }

        public ITextOptionsBuilder SetColor(string color)
        {
            _color = color;

            return this;
        }

        public TextOptions Build()
        {
            return new TextOptions
            {
                ShouldBeBold = _shouldBeBold,
                ShouldBeItalic = _shouldBeItalic,
                ShouldBeUnderline = _shouldBeUnderline,
                Size = _size,
                Color = _color
            };
        }
    }
}