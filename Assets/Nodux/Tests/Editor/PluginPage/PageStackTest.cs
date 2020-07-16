using Nodux.PluginPage;
using NUnit.Framework;

namespace Nodux.Tests.PluginPage
{
    public class PageStackTest
    {
        [Test]
        public void PushPopTest()
        {
            var stack = new PageStack();
            Assert.That(stack.Count, Is.EqualTo(0));

            stack.Push(new Page {Name = "Page1", Data = null});
            Assert.That(stack.Count, Is.EqualTo(1));
            Assert.That(stack.Peek().Name, Is.EqualTo("Page1"));

            stack.Push(new Page {Name = "Page2", Data = null});
            Assert.That(stack.Count, Is.EqualTo(2));
            Assert.That(stack.Peek().Name, Is.EqualTo("Page2"));

            var page2 = stack.Pop();
            Assert.That(page2.Name, Is.EqualTo("Page2"));
            Assert.That(stack.Count, Is.EqualTo(1));
            Assert.That(stack.Peek().Name, Is.EqualTo("Page1"));

            var page1 = stack.Pop();
            Assert.That(page1.Name, Is.EqualTo("Page1"));
            Assert.That(stack.Count, Is.EqualTo(0));
        }
    }
}