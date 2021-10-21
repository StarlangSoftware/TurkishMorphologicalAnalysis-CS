using Dictionary.Dictionary;
using MorphologicalAnalysis;
using NUnit.Framework;

namespace Test
{
    public class FsmParseListTest
    {
        FsmParseList parse1, parse2, parse3, parse4, parse5, parse6, parse7, parse8, parse9, parse10, parse11, parse12, 
            parse13, parse14;

        [SetUp]
        public void Setup()
        {
            var fsm = new FsmMorphologicalAnalyzer();
            parse1 = fsm.MorphologicalAnalysis("açılır");
            parse2 = fsm.MorphologicalAnalysis("koparılarak");
            parse3 = fsm.MorphologicalAnalysis("toplama");
            parse4 = fsm.MorphologicalAnalysis("değerlendirmede");
            parse5 = fsm.MorphologicalAnalysis("soruşturmasının");
            parse6 = fsm.MorphologicalAnalysis("karşılaştırmalı");
            parse7 = fsm.MorphologicalAnalysis("esaslarını");
            parse8 = fsm.MorphologicalAnalysis("güçleriyle");
            parse9 = fsm.MorphologicalAnalysis("bulmayacakları");
            parse10 = fsm.MorphologicalAnalysis("kitabı");
            parse11 = fsm.MorphologicalAnalysis("kitapları");
            parse12 = fsm.MorphologicalAnalysis("o");
            parse13 = fsm.MorphologicalAnalysis("arabası");
            parse14 = fsm.MorphologicalAnalysis("sana");
        }

        [Test]
        public void TestSize()
        {
            Assert.AreEqual(2, parse1.Size());
            Assert.AreEqual(2, parse2.Size());
            Assert.AreEqual(6, parse3.Size());
            Assert.AreEqual(4, parse4.Size());
            Assert.AreEqual(5, parse5.Size());
            Assert.AreEqual(12, parse6.Size());
            Assert.AreEqual(8, parse7.Size());
            Assert.AreEqual(6, parse8.Size());
            Assert.AreEqual(5, parse9.Size());
            Assert.AreEqual(4, parse14.Size());
        }

        [Test]
        public void TestRootWords()
        {
            Assert.AreEqual("aç", parse1.RootWords());
            Assert.AreEqual("kop$kopar", parse2.RootWords());
            Assert.AreEqual("topla$toplam$toplama", parse3.RootWords());
            Assert.AreEqual("değer$değerlen$değerlendir$değerlendirme", parse4.RootWords());
            Assert.AreEqual("sor$soru$soruş$soruştur$soruşturma", parse5.RootWords());
            Assert.AreEqual("karşı$karşıla$karşılaş$karşılaştır$karşılaştırma$karşılaştırmalı", parse6.RootWords());
            Assert.AreEqual("esas", parse7.RootWords());
            Assert.AreEqual("güç", parse8.RootWords());
            Assert.AreEqual("bul", parse9.RootWords());
        }

        [Test]
        public void TestGetParseWithLongestRootWord()
        {
            Assert.AreEqual(new Word("kopar"), parse2.GetParseWithLongestRootWord().GetWord());
            Assert.AreEqual(new Word("toplama"), parse3.GetParseWithLongestRootWord().GetWord());
            Assert.AreEqual(new Word("değerlendirme"), parse4.GetParseWithLongestRootWord().GetWord());
            Assert.AreEqual(new Word("soruşturma"), parse5.GetParseWithLongestRootWord().GetWord());
            Assert.AreEqual(new Word("karşılaştırmalı"), parse6.GetParseWithLongestRootWord().GetWord());
        }

        [Test]
        public void TestReduceToParsesWithSameRootAndPos()
        {
            parse2.ReduceToParsesWithSameRootAndPos(new Word("kop+VERB"));
            Assert.AreEqual(1, parse2.Size());
            parse3.ReduceToParsesWithSameRootAndPos(new Word("topla+VERB"));
            Assert.AreEqual(2, parse3.Size());
            parse6.ReduceToParsesWithSameRootAndPos(new Word("karşıla+VERB"));
            Assert.AreEqual(2, parse6.Size());
        }

        [Test]
        public void TestReduceToParsesWithSameRoot()
        {
            parse2.ReduceToParsesWithSameRoot("kop");
            Assert.AreEqual(1, parse2.Size());
            parse3.ReduceToParsesWithSameRoot("topla");
            Assert.AreEqual(3, parse3.Size());
            parse6.ReduceToParsesWithSameRoot("karşı");
            Assert.AreEqual(4, parse6.Size());
            parse7.ReduceToParsesWithSameRoot("esas");
            Assert.AreEqual(8, parse7.Size());
            parse8.ReduceToParsesWithSameRoot("güç");
            Assert.AreEqual(6, parse8.Size());
        }

        [Test]
        public void TestConstructParseListForDifferentRootWithPos()
        {
            Assert.AreEqual(1, parse1.ConstructParseListForDifferentRootWithPos().Count);
            Assert.AreEqual(2, parse2.ConstructParseListForDifferentRootWithPos().Count);
            Assert.AreEqual(5, parse3.ConstructParseListForDifferentRootWithPos().Count);
            Assert.AreEqual(4, parse4.ConstructParseListForDifferentRootWithPos().Count);
            Assert.AreEqual(5, parse5.ConstructParseListForDifferentRootWithPos().Count);
            Assert.AreEqual(7, parse6.ConstructParseListForDifferentRootWithPos().Count);
            Assert.AreEqual(2, parse7.ConstructParseListForDifferentRootWithPos().Count);
            Assert.AreEqual(2, parse8.ConstructParseListForDifferentRootWithPos().Count);
            Assert.AreEqual(1, parse9.ConstructParseListForDifferentRootWithPos().Count);
        }

        [Test]
        public void TestParsesWithoutPrefixAndSuffix()
        {
            Assert.AreEqual("P3SG+NOM$PNON+ACC", parse10.ParsesWithoutPrefixAndSuffix());
            Assert.AreEqual("A3PL+P3PL+NOM$A3PL+P3SG+NOM$A3PL+PNON+ACC$A3SG+P3PL+NOM",
                parse11.ParsesWithoutPrefixAndSuffix());
            Assert.AreEqual("DET$PRON+DEMONSP+A3SG+PNON+NOM$PRON+PERS+A3SG+PNON+NOM",
                parse12.ParsesWithoutPrefixAndSuffix());
            Assert.AreEqual("NOUN^DB+ADJ+ALMOST$NOUN+A3SG+P3SG+NOM",
                parse13.ParsesWithoutPrefixAndSuffix());
        }
    }
}