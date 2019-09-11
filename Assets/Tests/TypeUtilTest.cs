using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using UnityLeaf.PluginEditor;

namespace UnityLeaf.Tests.Util
{
    public class TypeUtilTest
    {
        [Test]
        public void CountUnityObjectsTest()
        {
            Assert.AreEqual(2, TypeUtil.CountUnityObjects(typeof(Sample)),  "Type of Sample should be 2");
        }

        private class Sample
        {
            public Button Button;
            [SerializeField] private Text Text;
        }
    }
}