using EmailService.Contracts;
using EmailService.Models;
using System;
using System.Collections.Generic;

namespace EmailService.Constracts
{
    public interface IBodyBuilder : IBuilder<string>
    {
        IBodyBuilder AddText(string text, TextOptions options = null);

        IBodyBuilder AddText(string text, Action<ITextOptionsBuilder> optionsBuilderActions);

        IBodyBuilder AddOrderedList(IList<string> items);

        IBodyBuilder AddUnorderedList(IList<string> items);

        IBodyBuilder AddImageFromUrl(string imageUrl);

        IBodyBuilder AddNewLine();
    }
}