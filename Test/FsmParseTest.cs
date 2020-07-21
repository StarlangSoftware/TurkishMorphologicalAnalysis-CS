using MorphologicalAnalysis;
using NUnit.Framework;

namespace Test
{
    public class FsmParseTest
    {
        FsmParse parse1, parse2, parse3, parse4, parse5, parse6, parse7, parse8, parse9;

        [SetUp]
        public void Setup()
        {
            var fsm = new FsmMorphologicalAnalyzer();
            parse1 = fsm.MorphologicalAnalysis("açılır").GetFsmParse(0);
            parse2 = fsm.MorphologicalAnalysis("koparılarak").GetFsmParse(0);
            parse3 = fsm.MorphologicalAnalysis("toplama").GetFsmParse(0);
            parse4 = fsm.MorphologicalAnalysis("değerlendirmede").GetFsmParse(0);
            parse5 = fsm.MorphologicalAnalysis("soruşturmasının").GetFsmParse(0);
            parse6 = fsm.MorphologicalAnalysis("karşılaştırmalı").GetFsmParse(0);
            parse7 = fsm.MorphologicalAnalysis("esaslarını").GetFsmParse(0);
            parse8 = fsm.MorphologicalAnalysis("güçleriyle").GetFsmParse(0);
            parse9 = fsm.MorphologicalAnalysis("bulmayacakları").GetFsmParse(0);
        }

        [Test]
        public void TestGetLastLemmaWithTag()
        {
            Assert.AreEqual("açıl", parse1.GetLastLemmaWithTag("VERB"));
            Assert.AreEqual("koparıl", parse2.GetLastLemmaWithTag("VERB"));
            Assert.AreEqual("değerlendir", parse4.GetLastLemmaWithTag("VERB"));
            Assert.AreEqual("soruştur", parse5.GetLastLemmaWithTag("VERB"));
            Assert.AreEqual("karşılaştırmalı", parse6.GetLastLemmaWithTag("ADJ"));
        }

        [Test]
        public void TestGetLastLemma()
        {
            Assert.AreEqual("açılır", parse1.GetLastLemma());
            Assert.AreEqual("koparılarak", parse2.GetLastLemma());
            Assert.AreEqual("değerlendirme", parse4.GetLastLemma());
            Assert.AreEqual("soruşturma", parse5.GetLastLemma());
            Assert.AreEqual("karşılaştırmalı", parse6.GetLastLemma());
        }

        [Test]
        public void TestGetTransitionList()
        {
            Assert.AreEqual("aç+VERB^DB+VERB+PASS+POS+AOR^DB+ADJ+ZERO", parse1.ToString());
            Assert.AreEqual("kop+VERB^DB+VERB+CAUS^DB+VERB+PASS+POS^DB+ADV+BYDOINGSO", parse2.ToString());
            Assert.AreEqual("topla+NOUN+A3SG+P1SG+DAT", parse3.ToString());
            Assert.AreEqual("değer+NOUN+A3SG+PNON+NOM^DB+VERB+ACQUIRE^DB+VERB+CAUS+POS^DB+NOUN+INF2+A3SG+PNON+LOC",
                parse4.ToString());
            Assert.AreEqual("sor+VERB+RECIP^DB+VERB+CAUS+POS^DB+NOUN+INF2+A3SG+P3SG+GEN", parse5.ToString());
            Assert.AreEqual("karşı+ADJ^DB+VERB+BECOME^DB+VERB+CAUS+POS^DB+NOUN+INF2+A3SG+PNON+NOM^DB+ADJ+WITH", parse6.ToString());
            Assert.AreEqual("esas+ADJ^DB+NOUN+ZERO+A3PL+P2SG+ACC", parse7.ToString());
            Assert.AreEqual("güç+ADJ^DB+NOUN+ZERO+A3PL+P3PL+INS", parse8.ToString());
            Assert.AreEqual("bul+VERB+NEG^DB+ADJ+FUTPART+P3PL", parse9.ToString());
        }

        [Test]
        public void TestWithList()
        {
            Assert.AreEqual("aç+Hl+Hr", parse1.WithList());
            Assert.AreEqual("kop+Ar+Hl+yArAk", parse2.WithList());
            Assert.AreEqual("topla+Hm+yA", parse3.WithList());
            Assert.AreEqual("değer+lAn+DHr+mA+DA", parse4.WithList());
            Assert.AreEqual("sor+Hs+DHr+mA+sH+nHn", parse5.WithList());
            Assert.AreEqual("karşı+lAs+DHr+mA+lH", parse6.WithList());
            Assert.AreEqual("esas+lAr+Hn+yH", parse7.WithList());
            Assert.AreEqual("güç+lArH+ylA", parse8.WithList());
            Assert.AreEqual("bul+mA+yAcAk+lArH", parse9.WithList());
        }

        [Test]
        public void TestSuffixList()
        {
            Assert.AreEqual("VerbalRoot(F5PR)(aç)+PassiveHl(açıl)+AdjectiveRoot(VERB)(açılır)", parse1.SuffixList());
            Assert.AreEqual("VerbalRoot(F1P1)(kop)+CausativeAr(kopar)+PassiveHl(koparıl)+Adverb1(koparılarak)",
                parse2.SuffixList());
            Assert.AreEqual("NominalRoot(topla)+Possessive(toplam)+Case1(toplama)", parse3.SuffixList());
            Assert.AreEqual(
                "NominalRoot(değer)+VerbalRoot(F5PR)(değerlen)+CausativeDHr(değerlendir)+NominalRoot(değerlendirme)+Case1(değerlendirmede)",
                parse4.SuffixList());
            Assert.AreEqual(
                "VerbalRoot(F5PR)(sor)+Reciprocal(soruş)+CausativeDHr(soruştur)+NominalRoot(soruşturma)+Possessive3(soruşturması)+Case1(soruşturmasının)",
                parse5.SuffixList());
            Assert.AreEqual(
                "AdjectiveRoot(karşı)+VerbalRoot(F5PR)(karşılaş)+CausativeDHr(karşılaştır)+NominalRoot(karşılaştırma)+AdjectiveRoot(NOUN)(karşılaştırmalı)",
                parse6.SuffixList());
            Assert.AreEqual("AdjectiveRoot(esas)+Plural(esaslar)+Possessive(esasların)+AccusativeNoun(esaslarını)",
                parse7.SuffixList());
            Assert.AreEqual("AdjectiveRoot(güç)+Possesive3(güçleri)+Case1(güçleriyle)", parse8.SuffixList());
            Assert.AreEqual(
                "VerbalRoot(F5PW)(bul)+Negativema(bulma)+AdjectiveParticiple(bulmayacak)+Adjective(bulmayacakları)",
                parse9.SuffixList());
        }
    }
}