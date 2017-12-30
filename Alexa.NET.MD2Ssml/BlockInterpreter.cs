using System;
using Alexa.NET.Response.Ssml;
using CommonMark.Syntax;

namespace Alexa.NET.MD2Ssml
{
    public static class BlockInterpreter
    {
        public static void Interpret(ConverterContext context)
        {
            switch (context.Enumerable.Current.Block.Tag)
            {
                case BlockTag.Document:
                    break;
                case BlockTag.Paragraph:
                    HandleParagraph(context);
                    break;
                default:
                    throw new InvalidOperationException($"Unable to handle block tag {context.Enumerable.Current.Block.Tag}");
            }
        }

        private static void HandleParagraph(ConverterContext context)
        {
            context.AddStack.Clear();

            if (context.Enumerable.Current.IsOpening)
            {
                var sentence = new Sentence();
                var paragraph = new Paragraph();
                paragraph.Elements.Add(sentence);
                context.Speech.Elements.Add(paragraph);
                
                context.AddStack.Push(paragraph);
                context.AddStack.Push(sentence);
            }
        }
    }
}
