using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using UnityLeaf.PluginEditor;

namespace UnityLeaf.Tests.Util
{
    public class TypeUtilTest
    {
        [Test]
        public void ListUpUnityObjectsTest()
        {
            Assert.AreEqual(2, TypeUtil.ListUpUnityObjects(typeof(Sample), new Sample()).Length,  "Type of Sample should be 2");
        }

        private class Sample
        {
            private int Value;
            public Button Button;
            [SerializeField] private Text Text;
        }
    }
}