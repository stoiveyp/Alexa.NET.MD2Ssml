using System;
using System.Collections.Generic;
using System.Text;
using Alexa.NET.Response.Ssml;
using CommonMark.Syntax;

namespace Alexa.NET.MD2Ssml
{
    internal class InlineInterpreter
    {
        public static void Interpret(ConverterContext context)
        {
            switch (context.Enumerable.Current.Inline.Tag)
            {
                case InlineTag.Strong:
                    HandleStrong(context);
                    break;
                case InlineTag.Emphasis:
                    HandleEmphasis(context);
                    break;
                case InlineTag.String:case InlineTag.RawHtml:
                    HandleText(context);
                    break;
                case InlineTag.LineBreak:case InlineTag.SoftBreak:
                    HandleBreak(context);
                    break;
                default:
                    throw new InvalidOperationException($"Unable to handle inline tag {context.Enumerable.Current.Inline.Tag}");
            }
        }

        private static void HandleBreak(ConverterContext context)
        {
            context.PopUntil<Paragraph>();
            var sentence = new Sentence();

            context.Add(sentence);
            context.AddStack.Push(sentence);
        }

        private static void HandleText(ConverterContext context)
        {
            context.Add(new PlainText(context.Enumerable.Current.Inline.LiteralContent));
        }

        private static void HandleStrong(ConverterContext context)
        {
            if (!context.Enumerable.Current.IsOpening)
            {
                throw new InvalidOperationException("Closing strong without opening");
            }

            var text = TextUntil(context, InlineTag.Strong);
            context.Add(new Emphasis(text) { Level = EmphasisLevel.Strong });
        }

        public static void HandleEmphasis(ConverterContext context)
        {
            if (!context.Enumerable.Current.IsOpening)
            {
                throw new InvalidOperationException("Closing emphasis without opening");
            }

            var text = TextUntil(context, InlineTag.Emphasis);
            context.Add(new Prosody
            {
                Elements = new List<ISsml>{new PlainText(text)},
                Pitch = ProsodyPitch.ExtraHigh
            });
        }

        private static string TextUntil(ConverterContext context, InlineTag tag)
        {
            var osb = new StringBuilder();
            while (context.Enumerable.MoveNext())
            {
                var element = context.Enumerable.Current;
                if (element.Inline == null || element.Inline.Tag == tag)
                {
                    break;
                }

                osb.Append(element.Inline.LiteralContent);
            }

            return osb.ToString();
        }
    }
}