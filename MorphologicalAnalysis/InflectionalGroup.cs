using System.Collections.Generic;

namespace MorphologicalAnalysis
{
    public class InflectionalGroup
    {
        private readonly List<MorphologicalTag> _ig;

        private static readonly string[] Tags =
        {
            "NOUN", "ADV", "ADJ", "VERB", "A1SG",
            "A2SG", "A3SG", "A1PL", "A2PL", "A3PL",
            "P1SG", "P2SG", "P3SG", "P1PL", "P2PL",
            "P3PL", "PROP", "PNON", "NOM", "WITH",
            "WITHOUT", "ACC", "DAT", "GEN", "ABL",
            "ZERO", "ABLE", "NEG", "PAST",
            "CONJ", "DET", "DUP", "INTERJ", "NUM",
            "POSTP", "PUNC", "QUES", "AGT", "BYDOINGSO",
            "CARD", "CAUS", "DEMONSP", "DISTRIB", "FITFOR",
            "FUTPART", "INF", "NESS", "ORD", "PASS",
            "PASTPART", "PRESPART", "QUESP", "QUANTP", "RANGE",
            "RATIO", "REAL", "RECIP", "REFLEX", "REFLEXP",
            "TIME", "WHEN", "WHILE", "WITHOUTHAVINGDONESO", "PCABL",
            "PCACC", "PCDAT", "PCGEN", "PCINS", "PCNOM",
            "ACQUIRE", "ACTOF", "AFTERDOINGSO", "ALMOST", "AS",
            "ASIF", "BECOME", "EVERSINCE", "FEELLIKE", "HASTILY",
            "INBETWEEN", "JUSTLIKE", "LY", "NOTABLESTATE", "RELATED",
            "REPEAT", "SINCE", "SINCEDOINGSO", "START", "STAY",
            "EQU", "INS", "AOR", "DESR", "FUT",
            "IMP", "NARR", "NECES", "OPT", "PAST",
            "PRES", "PROG1", "PROG2", "COND", "COP",
            "POS", "PRON", "LOC", "REL", "DEMONS",
            "INF2", "INF3", "BSTAG", "ESTAG", "BTTAG",
            "ETTAG", "BDTAG", "EDTAG", "INF1", "ASLONGAS",
            "DIST", "ADAMANTLY", "PERCENT", "WITHOUTBEINGABLETOHAVEDONESO", "DIM",
            "PERS", "FRACTION", "HASHTAG", "EMAIL", "DATE", "NONE", "CODE", "METRIC"
        };

        public static readonly MorphologicalTag[] MorphoTags =
        {
            MorphologicalTag.NOUN, MorphologicalTag.ADVERB, MorphologicalTag.ADJECTIVE,
            MorphologicalTag.VERB, MorphologicalTag.A1SG, MorphologicalTag.A2SG, MorphologicalTag.A3SG, MorphologicalTag
                .A1PL,
            MorphologicalTag.A2PL, MorphologicalTag.A3PL, MorphologicalTag.P1SG, MorphologicalTag.P2SG, MorphologicalTag
                .P3SG,
            MorphologicalTag.P1PL,
            MorphologicalTag.P2PL, MorphologicalTag.P3PL, MorphologicalTag.PROPERNOUN, MorphologicalTag
                .PNON,
            MorphologicalTag.NOMINATIVE,
            MorphologicalTag.WITH, MorphologicalTag.WITHOUT, MorphologicalTag.ACCUSATIVE, MorphologicalTag
                .DATIVE,
            MorphologicalTag.GENITIVE,
            MorphologicalTag.ABLATIVE, MorphologicalTag.ZERO, MorphologicalTag.ABLE, MorphologicalTag
                .NEGATIVE,
            MorphologicalTag
                .PASTTENSE,
            MorphologicalTag.CONJUNCTION, MorphologicalTag.DETERMINER, MorphologicalTag.DUPLICATION, MorphologicalTag
                .INTERJECTION,
            MorphologicalTag.NUMBER,
            MorphologicalTag.POSTPOSITION, MorphologicalTag.PUNCTUATION, MorphologicalTag.QUESTION, MorphologicalTag
                .AGENT,
            MorphologicalTag.BYDOINGSO,
            MorphologicalTag.CARDINAL, MorphologicalTag.CAUSATIVE, MorphologicalTag
                .DEMONSTRATIVEPRONOUN,
            MorphologicalTag.DISTRIBUTIVE, MorphologicalTag.FITFOR,
            MorphologicalTag.FUTUREPARTICIPLE, MorphologicalTag.INFINITIVE, MorphologicalTag.NESS, MorphologicalTag
                .ORDINAL,
            MorphologicalTag.PASSIVE,
            MorphologicalTag.PASTPARTICIPLE, MorphologicalTag.PRESENTPARTICIPLE, MorphologicalTag
                .QUESTIONPRONOUN,
            MorphologicalTag.QUANTITATIVEPRONOUN, MorphologicalTag.RANGE,
            MorphologicalTag.RATIO, MorphologicalTag.REAL, MorphologicalTag.RECIPROCAL, MorphologicalTag
                .REFLEXIVE,
            MorphologicalTag.REFLEXIVEPRONOUN,
            MorphologicalTag.TIME, MorphologicalTag.WHEN, MorphologicalTag.WHILE, MorphologicalTag
                .WITHOUTHAVINGDONESO,
            MorphologicalTag.PCABLATIVE,
            MorphologicalTag.PCACCUSATIVE, MorphologicalTag.PCDATIVE, MorphologicalTag.PCGENITIVE, MorphologicalTag
                .PCINSTRUMENTAL,
            MorphologicalTag.PCNOMINATIVE,
            MorphologicalTag.ACQUIRE, MorphologicalTag.ACTOF, MorphologicalTag.AFTERDOINGSO, MorphologicalTag
                .ALMOST,
            MorphologicalTag.AS,
            MorphologicalTag.ASIF, MorphologicalTag.BECOME, MorphologicalTag.EVERSINCE, MorphologicalTag
                .FEELLIKE,
            MorphologicalTag.HASTILY,
            MorphologicalTag.INBETWEEN, MorphologicalTag.JUSTLIKE, MorphologicalTag.LY, MorphologicalTag
                .NOTABLESTATE,
            MorphologicalTag.RELATED,
            MorphologicalTag.REPEAT, MorphologicalTag.SINCE, MorphologicalTag.SINCEDOINGSO, MorphologicalTag
                .START,
            MorphologicalTag.STAY,
            MorphologicalTag.EQUATIVE, MorphologicalTag.INSTRUMENTAL, MorphologicalTag.AORIST, MorphologicalTag
                .DESIRE,
            MorphologicalTag.FUTURE,
            MorphologicalTag.IMPERATIVE, MorphologicalTag.NARRATIVE, MorphologicalTag.NECESSITY, MorphologicalTag
                .OPTATIVE,
            MorphologicalTag.PAST,
            MorphologicalTag.PRESENT, MorphologicalTag.PROGRESSIVE1, MorphologicalTag.PROGRESSIVE2, MorphologicalTag
                .CONDITIONAL,
            MorphologicalTag.COPULA,
            MorphologicalTag.POSITIVE, MorphologicalTag.PRONOUN, MorphologicalTag.LOCATIVE, MorphologicalTag
                .RELATIVE,
            MorphologicalTag.DEMONSTRATIVE,
            MorphologicalTag.INFINITIVE2, MorphologicalTag.INFINITIVE3, MorphologicalTag
                .BEGINNINGOFSENTENCE,
            MorphologicalTag.ENDOFSENTENCE, MorphologicalTag.BEGINNINGOFTITLE,
            MorphologicalTag.ENDOFTITLE, MorphologicalTag.BEGINNINGOFDOCUMENT, MorphologicalTag
                .ENDOFDOCUMENT,
            MorphologicalTag.INFINITIVE, MorphologicalTag.ASLONGAS,
            MorphologicalTag.DISTRIBUTIVE, MorphologicalTag.ADAMANTLY, MorphologicalTag.PERCENT, MorphologicalTag
                .WITHOUTBEINGABLETOHAVEDONESO,
            MorphologicalTag.DIMENSION,
            MorphologicalTag.PERSONALPRONOUN, MorphologicalTag.FRACTION, MorphologicalTag.HASHTAG, MorphologicalTag
                .EMAIL,
            MorphologicalTag.DATE, MorphologicalTag.CODE, MorphologicalTag.METRIC, MorphologicalTag.NONE
        };

        /**
         * <summary>The getMorphologicalTag method takes a String tag as an input and if the input matches with one of the elements of
         * tags array, it then gets the morphoTags of this tag and returns it.</summary>
         *
         * <param name="tag">String to get morphoTags from.</param>
         * <returns>morphoTags if found, null otherwise.</returns>
         */
        public static MorphologicalTag GetMorphologicalTag(string tag)
        {
            for (var j = 0; j < Tags.Length; j++)
            {
                if (tag.ToUpper() == Tags[j])
                {
                    return MorphoTags[j];
                }
            }

            return MorphologicalTag.NONE;
        }

        /**
         * <summary>The getTag method takes a MorphologicalTag type tag as an input and returns its corresponding tag from tags array.</summary>
         *
         * <param name="tag">MorphologicalTag type input to find tag from.</param>
         * <returns>tag if found, null otherwise.</returns>
         */
        public static string GetTag(MorphologicalTag tag)
        {
            for (var j = 0; j < MorphoTags.Length; j++)
            {
                if (tag == MorphoTags[j])
                {
                    return Tags[j];
                }
            }

            return null;
        }

        /**
         * <summary>A constructor of {@link InflectionalGroup} class which initializes the IG {@link ArrayList} by parsing given input
         * String IG by + and calling the getMorphologicalTag method with these substrings. If getMorphologicalTag method returns
         * a tag, it adds this tag to the IG {@link ArrayList}.</summary>
         *
         * <param name="ig">String input.</param>
         */
        public InflectionalGroup(string ig)
        {
            MorphologicalTag tag;

            string morphologicalTag;
            this._ig = new List<MorphologicalTag>();

            var st = ig;
            while (st.Contains("+"))
            {
                morphologicalTag = st.Substring(0, st.IndexOf("+"));
                tag = GetMorphologicalTag(morphologicalTag);
                if (tag != MorphologicalTag.NONE)
                {
                    this._ig.Add(tag);
                }

                st = st.Substring(st.IndexOf("+") + 1);
            }

            morphologicalTag = st;
            tag = GetMorphologicalTag(morphologicalTag);
            if (tag != MorphologicalTag.NONE)
            {
                this._ig.Add(tag);
            }
        }

        /**
         * <summary>Another getTag method which takes index as an input and returns the corresponding tag from IG {@link ArrayList}.</summary>
         *
         * <param name="index">to get tag.</param>
         * <returns>tag at input index.</returns>
         */
        public MorphologicalTag GetTag(int index)
        {
            return _ig[index];
        }

        /**
         * <summary>The size method returns the size of the IG {@link ArrayList}.</summary>
         *
         * <returns>the size of the IG {@link ArrayList}.</returns>
         */
        public int Size()
        {
            return _ig.Count;
        }

        /**
         * <summary>Overridden toString method to return resulting tags in IG {@link ArrayList}.</summary>
         *
         * <returns>String result.</returns>
         */
        public override string ToString()
        {
            var result = GetTag(_ig[0]);
            for (var i = 1; i < _ig.Count; i++)
            {
                result = result + "+" + GetTag(_ig[i]);
            }

            return result;
        }

        /**
         * <summary>The containsCase method loops through the tags in IG {@link ArrayList} and finds out the tags of the NOMINATIVE,
         * ACCUSATIVE, DATIVE, LOCATIVE or ABLATIVE cases.</summary>
         *
         * <returns>tag which holds the condition.</returns>
         */
        public MorphologicalTag ContainsCase()
        {
            foreach (var tag in _ig)
            {
                if (tag == MorphologicalTag.NOMINATIVE || tag == MorphologicalTag.ACCUSATIVE ||
                    tag == MorphologicalTag.DATIVE || tag == MorphologicalTag.LOCATIVE ||
                    tag == MorphologicalTag.ABLATIVE)
                {
                    return tag;
                }
            }

            return MorphologicalTag.NONE;
        }

        /**
         * <summary>The containsPlural method loops through the tags in IG {@link ArrayList} and checks whether the tags are from
         * the agreement plural or possessive plural, i.e A1PL, A2PL, A3PL, P1PL, P2PL and P3PL.</summary>
         *
         * <returns>true if the tag is plural, false otherwise.</returns>
         */
        public bool ContainsPlural()
        {
            foreach (var tag in _ig)
            {
                if (tag == MorphologicalTag.A1PL || tag == MorphologicalTag.A2PL ||
                    tag == MorphologicalTag.A3PL ||
                    tag == MorphologicalTag.P1PL || tag == MorphologicalTag.P2PL ||
                    tag == MorphologicalTag.P3PL)
                {
                    return true;
                }
            }

            return false;
        }

        /**
         * <summary>The containsTag method takes a MorphologicalTag type tag as an input and loops through the tags in
         * IG {@link ArrayList} and returns true if the input matches with on of the tags in the IG.</summary>
         *
         * <param name="tag">MorphologicalTag type input to search for.</param>
         * <returns>true if tag matches with the tag in IG, false otherwise.</returns>
         */
        public bool ContainsTag(MorphologicalTag tag)
        {
            foreach (var currentTag in _ig)
            {
                if (currentTag == tag)
                {
                    return true;
                }
            }

            return false;
        }

        /**
         * <summary>The containsPossessive method loops through the tags in IG {@link ArrayList} and returns true if the tag in IG is
         * one of the possessives: P1PL, P1SG, P2PL, P2SG, P3PL AND P3SG.</summary>
         *
         * <returns>true if it contains possessive tag, false otherwise.</returns>
         */
        public bool ContainsPossessive()
        {
            foreach (var tag in _ig)
            {
                if (tag == MorphologicalTag.P1PL || tag == MorphologicalTag.P1SG ||
                    tag == MorphologicalTag.P2PL ||
                    tag == MorphologicalTag.P2SG || tag == MorphologicalTag.P3PL ||
                    tag == MorphologicalTag.P3SG)
                {
                    return true;
                }
            }

            return false;
        }
    }
}