using System;
using System.Collections.Generic;
using System.Linq;
using Alexa.NET.Response.Ssml;
using CommonMark.Syntax;

namespace Alexa.NET.MD2Ssml
{
    public class ConverterContext
    {
        public IEnumerator<EnumeratorEntry> Enumerable { get; }
        public Speech Speech { get; }
        public Stack<ISsml> AddStack { get; }

        public ConverterContext(IEnumerator<EnumeratorEntry> enumerable)
        {
            Speech = new Speech();
            AddStack = new Stack<ISsml>();
            Enumerable = enumerable;
        }

        public void Add(ISsml element)
        {
            var invalid = new InvalidOperationException($"Unable to add {element.GetType()}");
            switch (AddStack.Peek())
            {
                case Paragraph paragraph:
                    if (!(element is IParagraphSsml))
                    {
                        throw invalid;
                    }
                    paragraph.Elements.Add((IParagraphSsml)element);
                    break;
                case Sentence sentence:
                    if (!(element is ISentenceSsml))
                    {
                        throw invalid;
                    }
                    sentence.Elements.Add((ISentenceSsml)element);
                    break;
                default:
                    throw invalid;
            }
        }

        public void PopUntil<T>()
        {
            while (AddStack.Count > 0 && AddStack.Peek().GetType() != typeof(T))
            {
                AddStack.Pop();
            }
        }
    }
}