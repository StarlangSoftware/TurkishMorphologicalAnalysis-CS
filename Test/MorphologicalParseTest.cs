using MorphologicalAnalysis;
using NUnit.Framework;

namespace Test
{
    public class MorphologicalParseTest
    {
        MorphologicalParse parse1, parse2, parse3, parse4, parse5, parse6, parse7, parse8, parse9;

        [SetUp]
        public void Setup()
        {
            parse1 = new MorphologicalParse("bayan+NOUN+A3SG+PNON+NOM");
            parse2 = new MorphologicalParse("yaşa+VERB+POS^DB+ADJ+PRESPART");
            parse3 = new MorphologicalParse("serbest+ADJ");
            parse4 = new MorphologicalParse("et+VERB^DB+VERB+PASS^DB+VERB+ABLE+NEG+AOR+A3SG");
            parse5 = new MorphologicalParse("sür+VERB^DB+VERB+CAUS^DB+VERB+PASS+POS^DB+NOUN+INF2+A3SG+P3SG+NOM");
            parse6 = new MorphologicalParse("değiş+VERB^DB+VERB+CAUS^DB+VERB+PASS+POS^DB+VERB+ABLE+AOR^DB+ADJ+ZERO");
            parse7 = new MorphologicalParse(
                "iyi+ADJ^DB+VERB+BECOME^DB+VERB+CAUS^DB+VERB+PASS+POS^DB+VERB+ABLE^DB+NOUN+INF2+A3PL+P3PL+ABL");
            parse8 = new MorphologicalParse("değil+ADJ^DB+VERB+ZERO+PAST+A3SG");
            parse9 = new MorphologicalParse("hazır+ADJ^DB+VERB+ZERO+PAST+A3SG");
        }

        [Test]
        public void TestGetTransitionList()
        {
            Assert.AreEqual("NOUN+A3SG+PNON+NOM", parse1.GetTransitionList());
            Assert.AreEqual("VERB+POS+ADJ+PRESPART", parse2.GetTransitionList());
            Assert.AreEqual("ADJ", parse3.GetTransitionList());
            Assert.AreEqual("VERB+VERB+PASS+VERB+ABLE+NEG+AOR+A3SG", parse4.GetTransitionList());
            Assert.AreEqual("VERB+VERB+CAUS+VERB+PASS+POS+NOUN+INF2+A3SG+P3SG+NOM", parse5.GetTransitionList());
            Assert.AreEqual("VERB+VERB+CAUS+VERB+PASS+POS+VERB+ABLE+AOR+ADJ+ZERO", parse6.GetTransitionList());
            Assert.AreEqual("ADJ+VERB+BECOME+VERB+CAUS+VERB+PASS+POS+VERB+ABLE+NOUN+INF2+A3PL+P3PL+ABL",
                parse7.GetTransitionList());
            Assert.AreEqual("ADJ+VERB+ZERO+PAST+A3SG", parse8.GetTransitionList());
        }

        [Test]
        public void TestGetTag()
        {
            Assert.AreEqual("A3SG", parse1.GetTag(2));
            Assert.AreEqual("PRESPART", parse2.GetTag(4));
            Assert.AreEqual("serbest", parse3.GetTag(0));
            Assert.AreEqual("AOR", parse4.GetTag(7));
            Assert.AreEqual("P3SG", parse5.GetTag(10));
            Assert.AreEqual("ABLE", parse6.GetTag(8));
            Assert.AreEqual("ABL", parse7.GetTag(15));
        }

        [Test]
        public void TestGetTagSize()
        {
            Assert.AreEqual(5, parse1.TagSize());
            Assert.AreEqual(5, parse2.TagSize());
            Assert.AreEqual(2, parse3.TagSize());
            Assert.AreEqual(9, parse4.TagSize());
            Assert.AreEqual(12, parse5.TagSize());
            Assert.AreEqual(12, parse6.TagSize());
            Assert.AreEqual(16, parse7.TagSize());
            Assert.AreEqual(6, parse8.TagSize());
        }

        [Test]
        public void TestSize()
        {
            Assert.AreEqual(1, parse1.Size());
            Assert.AreEqual(2, parse2.Size());
            Assert.AreEqual(1, parse3.Size());
            Assert.AreEqual(3, parse4.Size());
            Assert.AreEqual(4, parse5.Size());
            Assert.AreEqual(5, parse6.Size());
            Assert.AreEqual(6, parse7.Size());
            Assert.AreEqual(2, parse8.Size());
        }

        [Test]
        public void TestGetRootPos()
        {
            Assert.AreEqual("NOUN", parse1.GetRootPos());
            Assert.AreEqual("VERB", parse2.GetRootPos());
            Assert.AreEqual("ADJ", parse3.GetRootPos());
            Assert.AreEqual("VERB", parse4.GetRootPos());
            Assert.AreEqual("VERB", parse5.GetRootPos());
            Assert.AreEqual("VERB", parse6.GetRootPos());
            Assert.AreEqual("ADJ", parse7.GetRootPos());
            Assert.AreEqual("ADJ", parse8.GetRootPos());
        }

        [Test]
        public void TestGetPos()
        {
            Assert.AreEqual("NOUN", parse1.GetPos());
            Assert.AreEqual("ADJ", parse2.GetPos());
            Assert.AreEqual("ADJ", parse3.GetPos());
            Assert.AreEqual("VERB", parse4.GetPos());
            Assert.AreEqual("NOUN", parse5.GetPos());
            Assert.AreEqual("ADJ", parse6.GetPos());
            Assert.AreEqual("NOUN", parse7.GetPos());
            Assert.AreEqual("VERB", parse8.GetPos());
        }

        [Test]
        public void TestGetWordWithPos()
        {
            Assert.AreEqual("bayan+NOUN", parse1.GetWordWithPos().GetName());
            Assert.AreEqual("yaşa+VERB", parse2.GetWordWithPos().GetName());
            Assert.AreEqual("serbest+ADJ", parse3.GetWordWithPos().GetName());
            Assert.AreEqual("et+VERB", parse4.GetWordWithPos().GetName());
            Assert.AreEqual("sür+VERB", parse5.GetWordWithPos().GetName());
            Assert.AreEqual("değiş+VERB", parse6.GetWordWithPos().GetName());
            Assert.AreEqual("iyi+ADJ", parse7.GetWordWithPos().GetName());
            Assert.AreEqual("değil+ADJ", parse8.GetWordWithPos().GetName());
        }

        [Test]
        public void TestLastIGContainsCase()
        {
            Assert.AreEqual("NOM", parse1.LastIGContainsCase());
            Assert.AreEqual("NULL", parse2.LastIGContainsCase());
            Assert.AreEqual("NULL", parse3.LastIGContainsCase());
            Assert.AreEqual("NULL", parse4.LastIGContainsCase());
            Assert.AreEqual("NOM", parse5.LastIGContainsCase());
            Assert.AreEqual("NULL", parse6.LastIGContainsCase());
            Assert.AreEqual("ABL", parse7.LastIGContainsCase());
        }

        [Test]
        public void TestLastIGContainsPossessive()
        {
            Assert.False(parse1.LastIGContainsPossessive());
            Assert.False(parse2.LastIGContainsPossessive());
            Assert.False(parse3.LastIGContainsPossessive());
            Assert.False(parse4.LastIGContainsPossessive());
            Assert.True(parse5.LastIGContainsPossessive());
            Assert.False(parse6.LastIGContainsPossessive());
            Assert.True(parse7.LastIGContainsPossessive());
        }

        [Test]
        public void TestIsPlural()
        {
            Assert.False(parse1.IsPlural());
            Assert.False(parse2.IsPlural());
            Assert.False(parse3.IsPlural());
            Assert.False(parse4.IsPlural());
            Assert.False(parse5.IsPlural());
            Assert.False(parse6.IsPlural());
            Assert.True(parse7.IsPlural());
        }

        [Test]
        public void TestIsAuxiliary()
        {
            Assert.False(parse1.IsAuxiliary());
            Assert.False(parse2.IsAuxiliary());
            Assert.False(parse3.IsAuxiliary());
            Assert.True(parse4.IsAuxiliary());
            Assert.False(parse5.IsAuxiliary());
            Assert.False(parse6.IsAuxiliary());
            Assert.False(parse7.IsAuxiliary());
        }

        [Test]
        public void TestIsNoun()
        {
            Assert.True(parse1.IsNoun());
            Assert.True(parse5.IsNoun());
            Assert.True(parse7.IsNoun());
        }

        [Test]
        public void TestIsAdjective()
        {
            Assert.True(parse2.IsAdjective());
            Assert.True(parse3.IsAdjective());
            Assert.True(parse6.IsAdjective());
        }

        [Test]
        public void TestIsVerb()
        {
            Assert.True(parse4.IsVerb());
            Assert.True(parse8.IsVerb());
        }

        [Test]
        public void TestIsRootVerb()
        {
            Assert.True(parse2.IsRootVerb());
            Assert.True(parse4.IsRootVerb());
            Assert.True(parse5.IsRootVerb());
            Assert.True(parse6.IsRootVerb());
        }

        [Test]
        public void TestGetTreePos()
        {
            Assert.AreEqual("NP", parse1.GetTreePos());
            Assert.AreEqual("ADJP", parse2.GetTreePos());
            Assert.AreEqual("ADJP", parse3.GetTreePos());
            Assert.AreEqual("VP", parse4.GetTreePos());
            Assert.AreEqual("NP", parse5.GetTreePos());
            Assert.AreEqual("ADJP", parse6.GetTreePos());
            Assert.AreEqual("NP", parse7.GetTreePos());
            Assert.AreEqual("NEG", parse8.GetTreePos());
            Assert.AreEqual("NOMP", parse9.GetTreePos());
        }
    }
}