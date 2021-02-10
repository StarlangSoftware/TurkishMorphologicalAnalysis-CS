using MorphologicalAnalysis;
using NUnit.Framework;

namespace Test
{
    public class InflectionalGroupTest
    {
        
        [Test]
        public void TestGetMorphologicalTag() {
            Assert.AreEqual(InflectionalGroup.GetMorphologicalTag("noun"), MorphologicalTag.NOUN);
            Assert.AreEqual(InflectionalGroup.GetMorphologicalTag("without"), MorphologicalTag.WITHOUT);
            Assert.AreEqual(InflectionalGroup.GetMorphologicalTag("interj"), MorphologicalTag.INTERJECTION);
            Assert.AreEqual(InflectionalGroup.GetMorphologicalTag("inf2"), MorphologicalTag.INFINITIVE2);
        }

        [Test]
        public void Size()
        {
            var inflectionalGroup1 = new InflectionalGroup("ADJ");
            Assert.AreEqual(1, inflectionalGroup1.Size());
            var inflectionalGroup2 = new InflectionalGroup("ADJ+JUSTLIKE");
            Assert.AreEqual(2, inflectionalGroup2.Size());
            var inflectionalGroup3 = new InflectionalGroup("ADJ+FUTPART+P1PL");
            Assert.AreEqual(3, inflectionalGroup3.Size());
            var inflectionalGroup4 = new InflectionalGroup("NOUN+A3PL+P1PL+ABL");
            Assert.AreEqual(4, inflectionalGroup4.Size());
            var inflectionalGroup5 = new InflectionalGroup("ADJ+WITH+A3SG+P3SG+ABL");
            Assert.AreEqual(5, inflectionalGroup5.Size());
            var inflectionalGroup6 = new InflectionalGroup("VERB+ABLE+NEG+FUT+A3PL+COP");
            Assert.AreEqual(6, inflectionalGroup6.Size());
            var inflectionalGroup7 = new InflectionalGroup("VERB+ABLE+NEG+AOR+A3SG+COND+A1SG");
            Assert.AreEqual(7, inflectionalGroup7.Size());
        }

        [Test]
        public void ContainsCase()
        {
            var inflectionalGroup1 = new InflectionalGroup("NOUN+ACTOF+A3PL+P1PL+NOM");
            Assert.NotNull(inflectionalGroup1.ContainsCase());
            var inflectionalGroup2 = new InflectionalGroup("NOUN+A3PL+P1PL+ACC");
            Assert.NotNull(inflectionalGroup2.ContainsCase());
            var inflectionalGroup3 = new InflectionalGroup("NOUN+ZERO+A3SG+P3PL+DAT");
            Assert.NotNull(inflectionalGroup3.ContainsCase());
            var inflectionalGroup4 = new InflectionalGroup("PRON+QUANTP+A1PL+P1PL+LOC");
            Assert.NotNull(inflectionalGroup4.ContainsCase());
            var inflectionalGroup5 = new InflectionalGroup("NOUN+AGT+A3SG+P2SG+ABL");
            Assert.NotNull(inflectionalGroup5.ContainsCase());
        }

        [Test]
        public void ContainsPlural()
        {
            var inflectionalGroup1 = new InflectionalGroup("VERB+NEG+NECES+A1PL");
            Assert.True(inflectionalGroup1.ContainsPlural());
            var inflectionalGroup2 = new InflectionalGroup("PRON+PERS+A2PL+PNON+NOM");
            Assert.True(inflectionalGroup2.ContainsPlural());
            var inflectionalGroup3 = new InflectionalGroup("NOUN+DIM+A3PL+P2SG+GEN");
            Assert.True(inflectionalGroup3.ContainsPlural());
            var inflectionalGroup4 = new InflectionalGroup("NOUN+A3PL+P1PL+GEN");
            Assert.True(inflectionalGroup4.ContainsPlural());
            var inflectionalGroup5 = new InflectionalGroup("NOUN+ZERO+A3SG+P2PL+INS");
            Assert.True(inflectionalGroup5.ContainsPlural());
            var inflectionalGroup6 = new InflectionalGroup("PRON+QUANTP+A3PL+P3PL+LOC");
            Assert.True(inflectionalGroup6.ContainsPlural());
        }

        [Test]
        public void ContainsTag()
        {
            var inflectionalGroup1 = new InflectionalGroup("NOUN+ZERO+A3SG+P1SG+NOM");
            Assert.True(inflectionalGroup1.ContainsTag(MorphologicalTag.NOUN));
            var inflectionalGroup2 = new InflectionalGroup("NOUN+AGT+A3PL+P2SG+ABL");
            Assert.True(inflectionalGroup2.ContainsTag(MorphologicalTag.AGENT));
            var inflectionalGroup3 = new InflectionalGroup("NOUN+INF2+A3PL+P3SG+NOM");
            Assert.True(inflectionalGroup3.ContainsTag(MorphologicalTag.NOMINATIVE));
            var inflectionalGroup4 = new InflectionalGroup("NOUN+ZERO+A3SG+P1PL+ACC");
            Assert.True(inflectionalGroup4.ContainsTag(MorphologicalTag.ZERO));
            var inflectionalGroup5 = new InflectionalGroup("NOUN+ZERO+A3SG+P2PL+INS");
            Assert.True(inflectionalGroup5.ContainsTag(MorphologicalTag.P2PL));
            var inflectionalGroup6 = new InflectionalGroup("PRON+QUANTP+A3PL+P3PL+LOC");
            Assert.True(inflectionalGroup6.ContainsTag(MorphologicalTag.QUANTITATIVEPRONOUN));
        }

        [Test]
        public void ContainsPossessive()
        {
            var inflectionalGroup1 = new InflectionalGroup("NOUN+ZERO+A3SG+P1SG+NOM");
            Assert.True(inflectionalGroup1.ContainsPossessive());
            var inflectionalGroup2 = new InflectionalGroup("NOUN+AGT+A3PL+P2SG+ABL");
            Assert.True(inflectionalGroup2.ContainsPossessive());
            var inflectionalGroup3 = new InflectionalGroup("NOUN+INF2+A3PL+P3SG+NOM");
            Assert.True(inflectionalGroup3.ContainsPossessive());
            var inflectionalGroup4 = new InflectionalGroup("NOUN+ZERO+A3SG+P1PL+ACC");
            Assert.True(inflectionalGroup4.ContainsPossessive());
            var inflectionalGroup5 = new InflectionalGroup("NOUN+ZERO+A3SG+P2PL+INS");
            Assert.True(inflectionalGroup5.ContainsPossessive());
            var inflectionalGroup6 = new InflectionalGroup("PRON+QUANTP+A3PL+P3PL+LOC");
            Assert.True(inflectionalGroup6.ContainsPossessive());
        }
    }
}