﻿using System;
using System.Linq;
using Alexa.NET.Response.Ssml;
using Xunit;
using Xunit.Abstractions;

namespace Alexa.NET.MD2Ssml.Tests
{
    public class ConverterTests
    {
        private ITestOutputHelper Output { get; }

        public ConverterTests(ITestOutputHelper output)
        {
            Output = output;
        }

        [Fact]
        public void CanHandleStrong()
        {
            var speech = MarkdownConverter.Convert("**hello!**");
            Assert.Single(speech.Elements);
            var first = NonStructureElement(speech) as Emphasis;
            Assert.NotNull(first);
            Assert.Equal(EmphasisLevel.Strong, first.Level);
            Assert.Equal("hello!", first.Text);
            Output.WriteLine(speech.ToXml());
        }

        [Fact]
        public void CanHandleEmphasis()
        {
            var speech = MarkdownConverter.Convert("*hello*");
            Assert.Single(speech.Elements);

            var first = NonStructureElement(speech) as Prosody;
            Assert.NotNull(first);
            Assert.Equal(ProsodyPitch.ExtraHigh, first.Pitch);

            Output.WriteLine(speech.ToXml());
        }

        [Fact]
        public void CanHandlePlainText()
        {
            var speech = MarkdownConverter.Convert("hello");
            Assert.Single(speech.Elements);

            var first = NonStructureElement(speech) as PlainText;
            Assert.NotNull(first);
            Assert.Equal("hello", first.Text);

            Output.WriteLine(speech.ToXml());
        }

        [Fact]
        public void CanHandleMulipleInlineInSingleSentence()
        {
            var speech = MarkdownConverter.Convert("**hello** world");
            Assert.Single(speech.Elements);

            var first = NonStructureElement(speech) as Emphasis;
            var second = NonStructureElement(speech, 1) as PlainText;

            Assert.NotNull(first);
            Assert.NotNull(second);
            Assert.Equal("hello", first.Text);
            Assert.Equal(" world", second.Text);

            Output.WriteLine(speech.ToXml());
        }

        [Fact]
        public void TreatLineBreakAsNewSentence()
        {
            var speech = MarkdownConverter.Convert($"hello{Environment.NewLine}world.");
            Assert.Single(speech.Elements);

            var paragraph = speech.Elements.First() as Paragraph;
            Assert.NotNull(paragraph);
            Assert.Equal(2, paragraph.Elements.Count);
            Assert.True(paragraph.Elements.All(e => e is Sentence));

            Output.WriteLine(speech.ToXml());
        }

        [Fact]
        public void CorrectlyHandlesNewParagraph()
        {
            var speech = MarkdownConverter.Convert($"hello{Environment.NewLine}world.{Environment.NewLine}{Environment.NewLine}New Paragraph");

            Assert.Equal(2,speech.Elements.Count);
            Assert.True(speech.Elements.All(e => e is Paragraph));

            Output.WriteLine(speech.ToXml());
        }

        private ISsml NonStructureElement(Speech speech, int skip = 0)
        {
            var paragraph = speech.Elements.First() as Paragraph;
            Assert.NotNull(paragraph);

            var sentence = paragraph.Elements.First() as Sentence;
            Assert.NotNull(sentence);

            return sentence.Elements.Skip(skip).First();
        }
    }
}
