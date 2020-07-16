using Nodux.Core;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

namespace Nodux.Tests.Core
{
    public class TypeUtilTest
    {
        [Test]
        public void ListUpUnityObjectsTest()
        {
            Assert.AreEqual(2, TypeUtil.ListUpUnityObjects(typeof(Sample), new Sample()).Length,  "Type of Sample should be 2");
        }

        #pragma warning disable CS0414
        private class Sample
        {
            private int Value = 0;
            public Button Button = null;
            [SerializeField] private Text Text = null;

            private void Dummy()
            {
                // NOTE: suppress warnings of unused
                this.Value = 2;
                this.Button = null;
                this.Text = null;
            }
        }
        #pragma warning restore CS0414
    }
}