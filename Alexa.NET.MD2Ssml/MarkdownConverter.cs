using System;
using System.Collections.Generic;
using System.Text;
using Alexa.NET.Response.Ssml;
using CommonMark;
using CommonMark.Syntax;

namespace Alexa.NET.MD2Ssml
{
    public static class MarkdownConverter
    {
        public static Func<EnumeratorEntry,ConverterContext,bool> ConversionHandled { get; set; }

        public static Speech Convert(string markdownText)
        {
            return Convert(CommonMarkConverter.Parse(markdownText));
        }

        private static Speech Convert(Block block)
        {
            var context = new ConverterContext(block.AsEnumerable().GetEnumerator());

            while (context.Enumerable.MoveNext())
            {
                var element = context.Enumerable.Current;
                if (ConversionHandled?.Invoke(element, context) ?? false)
                {
                    continue;
                }

                if (element.Block != null)
                {
                    BlockInterpreter.Interpret(context);
                }

                if (element.Inline != null)
                {
                    InlineInterpreter.Interpret(context);
                }
            }

            return context.Speech;
        }
    }
}
