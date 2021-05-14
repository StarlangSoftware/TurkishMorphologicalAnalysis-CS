using MorphologicalAnalysis;
using NUnit.Framework;

namespace Test
{
    public class DisambiguationCorpusTest
    {
        [Test]
        public void TestCorpus()
        {
            var corpus = new DisambiguationCorpus("../../../penntreebank.txt");
            Assert.AreEqual(19109, corpus.SentenceCount());
            Assert.AreEqual(170211, corpus.NumberOfWords());
        }
    }
}