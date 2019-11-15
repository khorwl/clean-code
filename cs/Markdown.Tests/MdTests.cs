﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Markdown.Parser;
using Markdown.Tree;
using NUnit.Framework;

namespace Markdown.Tests
{
    public class MdTests
    {
        private Md md;

        [SetUp]
        public void SetUp()
        {
            md = new Md();
        }

        private static IEnumerable<TestCaseData> GetMarkdownAndHtmlResult()
        {
            yield return new TestCaseData("this is plain text", "this is plain text")
                .SetName("shouldn't wrap plain text");

            yield return new TestCaseData("_this is plain text_", "<em>this is plain text</em>")
                .SetName("should wrap italic text right");

            yield return new TestCaseData("__this is plain text__", "<strong>this is plain text</strong>")
                .SetName("should wrap bold text right");

            yield return new TestCaseData("__this is _plain_ text__", "<strong>this is <em>plain</em> text</strong>")
                .SetName("italic are wrapped inside bold");

            yield return new TestCaseData("_this is __plain__ text_", "<em>this is plain text</em>")
                .SetName("bold are ignored at italic");

            yield return new TestCaseData("__1__ _56_", "__1__ _56_")
                .SetName("text with digits aren't wrapped in tags");

            yield return new TestCaseData("_this is \n text_", "<em>this is \n text</em>")
                .SetName("newline must be included at text");

            yield return new TestCaseData("_this is text__", "_this is text__")
                .SetName("with italic start and bold end shouldn't wrap in tags");

            yield return new TestCaseData("__this is text_", "__this is text_")
                .SetName("with bold start and italic end shouldn't wrap in tags");
        }

        [TestCaseSource(nameof(GetMarkdownAndHtmlResult))]
        public void Render_SimpleCases_ShouldReturnRightHtml(string markdown, string html)
        {
            var actual = md.Render(markdown);

            actual.Should().BeEquivalentTo(html);
        }

    }
}