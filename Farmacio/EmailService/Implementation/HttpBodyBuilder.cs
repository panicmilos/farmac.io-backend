using EmailService.Constracts;
using EmailService.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmailService.Implementation
{
    public class HttpBodyBuilder : IBodyBuilder
    {
        private readonly StringBuilder _bodyBuilder;

        public HttpBodyBuilder()
        {
            _bodyBuilder = new StringBuilder();
        }

        public IBodyBuilder AddText(string text, TextOptions options)
        {
            if (options == null)
            {
                _bodyBuilder.Append(text);
            }
            else
            {
                var htmlText = text;

                if (options.ShouldBeBold)
                    htmlText = "<b>" + htmlText + "</b>";

                if (options.ShouldBeItalic)
                    htmlText = "<i>" + htmlText + "</i>";

                if (options.ShouldBeUnderline)
                    htmlText = "<u>" + htmlText + "</u>";

                if (options.Size != null)
                    htmlText = $"<h{(int)options.Size}>" + htmlText + $"</h{(int)options.Size}>";

                if (!string.IsNullOrEmpty(options.Color))
                    htmlText = $"<font color=\"{options.Color}\">" + htmlText + "</font>";

                _bodyBuilder.Append(htmlText);
            }

            return this;
        }

        public IBodyBuilder AddText(string text, Action<ITextOptionsBuilder> optionsBuilderActions)
        {
            var optionsBuilder = new TextOptionsBuilder();
            optionsBuilderActions(optionsBuilder);
            var options = optionsBuilder.Build();

            AddText(text, options);

            return this;
        }

        public IBodyBuilder AddUnorderedList(IList<string> items)
        {
            _bodyBuilder.Append("<ul>");
            foreach (var item in items)
            {
                _bodyBuilder.Append($"<li>{item}</li>");
            }
            _bodyBuilder.Append("</ul>");

            return this;
        }

        public IBodyBuilder AddOrderedList(IList<string> items)
        {
            _bodyBuilder.Append("<ol>");
            foreach (var item in items)
            {
                _bodyBuilder.Append($"<li>{item}</li>");
            }
            _bodyBuilder.Append("</ol>");

            return this;
        }

        public IBodyBuilder AddNewLine()
        {
            _bodyBuilder.AppendLine();

            return this;
        }

        public IBodyBuilder AddImage(string imageUrl)
        {
            _bodyBuilder.AppendLine();
            _bodyBuilder.Append($"<img src={imageUrl}></img>");
            _bodyBuilder.AppendLine();

            return this;
        }

        public IBodyBuilder AddImage(EmailImage image)
        {
            return AddImage(image.Source);
        }

        public string Build()
        {
            return _bodyBuilder.ToString();
        }
    }
}