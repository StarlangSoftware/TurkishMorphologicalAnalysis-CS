using System;
using System.Collections.Generic;
using Dictionary.Dictionary;

namespace MorphologicalAnalysis
{
    public class MorphologicalParse
    {
        protected readonly List<InflectionalGroup> InflectionalGroups;
        protected readonly Word Root;

        /**
         * <summary>An empty constructor of {@link MorphologicalParse} class.</summary>
         */
        public MorphologicalParse()
        {
        }

        /**
         * <summary>The no-arg getWord method returns root {@link Word}.</summary>
         *
         * <returns>root {@link Word}.</returns>
         */
        public Word GetWord()
        {
            return Root;
        }

        /**
         * <summary>Another constructor of {@link MorphologicalParse} class which takes a {@link String} parse as an input. First it creates
         * an {@link ArrayList} as iGs for inflectional groups, and while given String contains derivational boundary (^DB+), it
         * adds the substring to the iGs {@link ArrayList} and continue to use given String from 4th index. If it does not contain ^DB+,
         * it directly adds the given String to the iGs {@link ArrayList}. Then, it creates a new {@link ArrayList} as
         * inflectionalGroups and checks for some cases.
         * <p/>
         * If the first item of iGs {@link ArrayList} is ++Punc, it creates a new root as +, and by calling
         * {@link InflectionalGroup} method with Punc it initializes the IG {@link ArrayList} by parsing given input
         * String IG by + and calling the getMorphologicalTag method with these substrings. If getMorphologicalTag method returns
         * a tag, it adds this tag to the IG {@link ArrayList} and also to the inflectionalGroups {@link ArrayList}.
         * <p/>
         * If the first item of iGs {@link ArrayList} has +, it creates a new word of first item's substring from index 0 to +,
         * and assigns it to root. Then, by calling {@link InflectionalGroup} method with substring from index 0 to +,
         * it initializes the IG {@link ArrayList} by parsing given input String IG by + and calling the getMorphologicalTag
         * method with these substrings. If getMorphologicalTag method returns a tag, it adds this tag to the IG {@link ArrayList}
         * and also to the inflectionalGroups {@link ArrayList}.
         * <p/>
         * If the first item of iGs {@link ArrayList} does not contain +, it creates a new word with first item and assigns it as root.
         * <p/>
         * At the end, it loops through the items of iGs and by calling {@link InflectionalGroup} method with these items
         * it initializes the IG {@link ArrayList} by parsing given input String IG by + and calling the getMorphologicalTag
         * method with these substrings. If getMorphologicalTag method returns a tag, it adds this tag to the IG {@link ArrayList}
         * and also to the inflectionalGroups {@link ArrayList}.</summary>
         *
         * <param name="parse">String input.</param>
         */
        public MorphologicalParse(string parse)
        {
            var iGs = new List<string>();
            var st = parse;
            while (st.Contains("^DB+"))
            {
                iGs.Add(st.Substring(0, st.IndexOf("^DB+")));
                st = st.Substring(st.IndexOf("^DB+") + 4);
            }

            iGs.Add(st);
            InflectionalGroups = new List<InflectionalGroup>();
            if (iGs[0] == "++Punc")
            {
                Root = new Word("+");
                InflectionalGroups.Add(new InflectionalGroup("Punc"));
            }
            else
            {
                if (iGs[0].IndexOf('+') != -1)
                {
                    Root = new Word(iGs[0].Substring(0, iGs[0].IndexOf('+')));
                    InflectionalGroups.Add(new InflectionalGroup(iGs[0].Substring(iGs[0].IndexOf('+') + 1)));
                }
                else
                {
                    Root = new Word(iGs[0]);
                }

                int i;
                for (i = 1; i < iGs.Count; i++)
                {
                    InflectionalGroups.Add(new InflectionalGroup(iGs[i]));
                }
            }
        }

        /**
         * <summary>Another constructor of {@link MorphologicalParse} class which takes an {@link ArrayList} inflectionalGroups as an input.
         * First, it initializes inflectionalGroups {@link ArrayList} and if the first item of the {@link ArrayList} has +, it gets
         * the substring from index 0 to + and assigns it as root, and by calling {@link InflectionalGroup} method with substring from index 0 to +,
         * it initializes the IG {@link ArrayList} by parsing given input String IG by + and calling the getMorphologicalTag
         * method with these substrings. If getMorphologicalTag method returns a tag, it adds this tag to the IG {@link ArrayList}
         * and also to the inflectionalGroups {@link ArrayList}. However, if the first item does not contain +, it directly prints out
         * indicating that there is no root for that item of this Inflectional Group.
         * <p/>
         * At the end, it loops through the items of inflectionalGroups and by calling {@link InflectionalGroup} method with these items
         * it initializes the IG {@link ArrayList} by parsing given input String IG by + and calling the getMorphologicalTag
         * method with these substrings. If getMorphologicalTag method returns a tag, it adds this tag to the IG {@link ArrayList}
         * and also to the inflectionalGroups {@link ArrayList}.</summary>
         *
         * <param name="inflectionalGroups">{@link ArrayList} input.</param>
         */
        public MorphologicalParse(List<string> inflectionalGroups)
        {
            int i;
            this.InflectionalGroups = new List<InflectionalGroup>();
            if (inflectionalGroups[0].IndexOf('+') != -1)
            {
                Root = new Word(inflectionalGroups[0].Substring(0, inflectionalGroups[0].IndexOf('+')));
                this.InflectionalGroups.Add(new InflectionalGroup(inflectionalGroups[0]
                    .Substring(inflectionalGroups[0].IndexOf('+') + 1)));
            }

            for (i = 1; i < inflectionalGroups.Count; i++)
            {
                this.InflectionalGroups.Add(new InflectionalGroup(inflectionalGroups[i]));
            }
        }

        /**
         * <summary>The getTransitionList method gets the first item of inflectionalGroups {@link ArrayList} as a {@link String}, then loops
         * through the items of inflectionalGroups and concatenates them by using +.</summary>
         *
         * <returns>String that contains transition list.</returns>
         */
        public string GetTransitionList()
        {
            var result = InflectionalGroups[0].ToString();
            for (var i = 1; i < InflectionalGroups.Count; i++)
            {
                result = result + "+" + InflectionalGroups[i];
            }

            return result;
        }

        /**
         * <summary>The getInflectionalGroupString method takes an {@link Integer} index as an input and if index is 0, it directly returns the
         * root and the first item of inflectionalGroups {@link ArrayList}. If the index is not 0, it then returns the corresponding
         * item of inflectionalGroups {@link ArrayList} as a {@link String}.</summary>
         *
         * <param name="index">Integer input.</param>
         * <returns>corresponding item of inflectionalGroups at given index as a {@link String}.</returns>
         */
        public string GetInflectionalGroupString(int index)
        {
            if (index == 0)
            {
                return Root.GetName() + "+" + InflectionalGroups[0];
            }

            return InflectionalGroups[index].ToString();
        }

        /**
         * <summary>The getInflectionalGroup method takes an {@link Integer} index as an input and it directly returns the {@link InflectionalGroup}
         * at given index.</summary>
         *
         * <param name="index">Integer input.</param>
         * <returns>InflectionalGroup at given index.</returns>
         */
        public InflectionalGroup GetInflectionalGroup(int index)
        {
            return InflectionalGroups[index];
        }

        /**
         * <summary>The getLastInflectionalGroup method directly returns the last {@link InflectionalGroup} of inflectionalGroups {@link ArrayList}.</summary>
         *
         * <returns>the last {@link InflectionalGroup} of inflectionalGroups {@link ArrayList}.</returns>
         */
        public InflectionalGroup GetLastInflectionalGroup()
        {
            return GetInflectionalGroup(InflectionalGroups.Count - 1);
        }

        /**
         * <summary>The getTag method takes an {@link Integer} index as an input and and if the given index is 0, it directly return the root.
         * then, it loops through the inflectionalGroups {@link ArrayList} it returns the MorphologicalTag of the corresponding inflectional group.</summary>
         *
         * <param name="index">Integer input.</param>
         * <returns>the MorphologicalTag of the corresponding inflectional group, or null of invalid index inputs.</returns>
         */
        public string GetTag(int index)
        {
            var size = 1;
            if (index == 0)
            {
                return Root.GetName();
            }

            foreach (var group in InflectionalGroups)
            {
                if (index < size + group.Size())
                {
                    return InflectionalGroup.GetTag(group.GetTag(index - size));
                }

                size += group.Size();
            }

            return null;
        }

        /**
         * <summary>The tagSize method loops through the inflectionalGroups {@link ArrayList} and accumulates the sizes of each inflectional group
         * in the inflectionalGroups.</summary>
         *
         * <returns>total size of the inflectionalGroups {@link ArrayList}.</returns>
         */
        public int TagSize()
        {
            var size = 1;
            foreach (var group in InflectionalGroups)
            {
                size += group.Size();
            }

            return size;
        }

        /**
         * <summary>The size method returns the size of the inflectionalGroups {@link ArrayList}.</summary>
         *
         * <returns>the size of the inflectionalGroups {@link ArrayList}.</returns>
         */
        public int Size()
        {
            return InflectionalGroups.Count;
        }

        /**
         * <summary>The firstInflectionalGroup method returns the first inflectional group of inflectionalGroups {@link ArrayList}.</summary>
         *
         * <returns>the first inflectional group of inflectionalGroups {@link ArrayList}.</returns>
         */
        public InflectionalGroup FirstInflectionalGroup()
        {
            return InflectionalGroups[0];
        }

        /**
         * <summary>The lastInflectionalGroup method returns the last inflectional group of inflectionalGroups {@link ArrayList}.</summary>
         *
         * <returns>the last inflectional group of inflectionalGroups {@link ArrayList}.</returns>
         */
        public InflectionalGroup LastInflectionalGroup()
        {
            return InflectionalGroups[InflectionalGroups.Count - 1];
        }

        /**
         * <summary>The getWordWithPos method returns root with the MorphologicalTag of the first inflectional as a new word.</summary>
         *
         * <returns>root with the MorphologicalTag of the first inflectional as a new word.</returns>
         */
        public Word GetWordWithPos()
        {
            return new Word(Root.GetName() + "+" + InflectionalGroup.GetTag(FirstInflectionalGroup().GetTag(0)));
        }

        /**
         * <summary>The getPos method returns the MorphologicalTag of the last inflectional group.</summary>
         *
         * <returns>the MorphologicalTag of the last inflectional group.</returns>
         */
        public string GetPos()
        {
            return InflectionalGroup.GetTag(LastInflectionalGroup().GetTag(0));
        }

        /**
         * <summary>The getRootPos method returns the MorphologicalTag of the first inflectional group.</summary>
         *
         * <returns>the MorphologicalTag of the first inflectional group.</returns>
         */
        public string GetRootPos()
        {
            return InflectionalGroup.GetTag(FirstInflectionalGroup().GetTag(0));
        }

        /**
         * <summary>The lastIGContainsCase method returns the MorphologicalTag of last inflectional group if it is one of the NOMINATIVE,
         * ACCUSATIVE, DATIVE, LOCATIVE or ABLATIVE cases, null otherwise.</summary>
         *
         * <returns>the MorphologicalTag of last inflectional group if it is one of the NOMINATIVE,
         * ACCUSATIVE, DATIVE, LOCATIVE or ABLATIVE cases, null otherwise.</returns>
         */
        public string LastIGContainsCase()
        {
            MorphologicalTag caseTag = LastInflectionalGroup().ContainsCase();
            if (caseTag != MorphologicalTag.NONE)
            {
                return InflectionalGroup.GetTag(caseTag);
            }

            return "NULL";
        }

        /**
         * <summary>The lastIGContainsTag method takes a MorphologicalTag as an input and returns true if the last inflectional group's
         * MorphologicalTag matches with one of the tags in the IG {@link ArrayList}, false otherwise.</summary>
         *
         * <param name="tag">{@link MorphologicalTag} type input.</param>
         * <returns>true if the last inflectional group's MorphologicalTag matches with one of the tags in the IG {@link ArrayList}, false otherwise.</returns>
         */
        public bool LastIGContainsTag(MorphologicalTag tag)
        {
            return LastInflectionalGroup().ContainsTag(tag);
        }

        /**
         * <summary>lastIGContainsPossessive method returns true if the last inflectional group contains one of the
         * possessives: P1PL, P1SG, P2PL, P2SG, P3PL AND P3SG, false otherwise.</summary>
         *
         * <returns>true if the last inflectional group contains one of the possessives: P1PL, P1SG, P2PL, P2SG, P3PL AND P3SG, false otherwise.</returns>
         */
        public bool LastIGContainsPossessive()
        {
            return LastInflectionalGroup().ContainsPossessive();
        }

        /**
         * <summary>The isCapitalWord method returns true if the character at first index o f root is an uppercase letter, false otherwise.</summary>
         *
         * <returns>true if the character at first index o f root is an uppercase letter, false otherwise.</returns>
         */
        public bool IsCapitalWord()
        {
            return char.IsUpper(Root.GetName()[0]);
        }

        /**
         * <summary>The isNoun method returns true if the past of speech is NOUN, false otherwise.</summary>
         *
         * <returns>true if the past of speech is NOUN, false otherwise.</returns>
         */
        public bool IsNoun()
        {
            return GetPos() == "NOUN";
        }

        /**
         * <summary>The isVerb method returns true if the past of speech is VERB, false otherwise.</summary>
         *
         * <returns>true if the past of speech is VERB, false otherwise.</returns>
         */
        public bool IsVerb()
        {
            return GetPos() == "VERB";
        }

        /**
         * <summary>The isRootVerb method returns true if the past of speech of root is VERB, false otherwise.</summary>
         *
         * <returns>true if the past of speech of root is VERB, false otherwise.</returns>
         */
        public bool IsRootVerb()
        {
            return GetRootPos() == "VERB";
        }

        /**
         * <summary>The isAdjective method returns true if the past of speech is ADJ, false otherwise.</summary>
         *
         * <returns>true if the past of speech is ADJ, false otherwise.</returns>
         */
        public bool IsAdjective()
        {
            return GetPos() == "ADJ";
        }

        /**
         * <summary>The isProperNoun method returns true if the first inflectional group's MorphologicalTag is a PROPERNOUN, false otherwise.</summary>
         *
         * <returns>true if the first inflectional group's MorphologicalTag is a PROPERNOUN, false otherwise.</returns>
         */
        public bool IsProperNoun()
        {
            return GetInflectionalGroup(0).ContainsTag(MorphologicalTag.PROPERNOUN);
        }

        /**
         * <summary>The isPunctuation method returns true if the first inflectional group's MorphologicalTag is a PUNCTUATION, false otherwise.</summary>
         *
         * <returns>true if the first inflectional group's MorphologicalTag is a PUNCTUATION, false otherwise.</returns>
         */
        public bool IsPunctuation()
        {
            return GetInflectionalGroup(0).ContainsTag(MorphologicalTag.PUNCTUATION);
        }

        /**
         * <summary>The isCardinal method returns true if the first inflectional group's MorphologicalTag is a CARDINAL, false otherwise.</summary>
         *
         * <returns>true if the first inflectional group's MorphologicalTag is a CARDINAL, false otherwise.</returns>
         */
        public bool IsCardinal()
        {
            return GetInflectionalGroup(0).ContainsTag(MorphologicalTag.CARDINAL);
        }

        /**
         * <summary>The isOrdinal method returns true if the first inflectional group's MorphologicalTag is a ORDINAL, false otherwise.</summary>
         *
         * <returns>true if the first inflectional group's MorphologicalTag is a ORDINAL, false otherwise.</returns>
         */
        public bool IsOrdinal()
        {
            return GetInflectionalGroup(0).ContainsTag(MorphologicalTag.ORDINAL);
        }

        /**
         * <summary>The isReal method returns true if the first inflectional group's MorphologicalTag is a REAL, false otherwise.</summary>
         *
         * <returns>true if the first inflectional group's MorphologicalTag is a REAL, false otherwise.</returns>
         */
        public bool IsReal()
        {
            return GetInflectionalGroup(0).ContainsTag(MorphologicalTag.REAL);
        }

        /**
         * <summary>The isNumber method returns true if the first inflectional group's MorphologicalTag is REAL or CARDINAL, false otherwise.</summary>
         *
         * <returns>true if the first inflectional group's MorphologicalTag is a REAL or CARDINAL, false otherwise.</returns>
         */
        public bool IsNumber()
        {
            return IsReal() || IsCardinal();
        }

        /**
         * <summary>The isTime method returns true if the first inflectional group's MorphologicalTag is a TIME, false otherwise.</summary>
         *
         * <returns>true if the first inflectional group's MorphologicalTag is a TIME, false otherwise.</returns>
         */
        public bool IsTime()
        {
            return GetInflectionalGroup(0).ContainsTag(MorphologicalTag.TIME);
        }

        /**
         * <summary>The isDate method returns true if the first inflectional group's MorphologicalTag is a DATE, false otherwise.</summary>
         *
         * <returns>true if the first inflectional group's MorphologicalTag is a DATE, false otherwise.</returns>
         */
        public bool IsDate()
        {
            return GetInflectionalGroup(0).ContainsTag(MorphologicalTag.DATE);
        }

        /**
         * <summary>The isHashTag method returns true if the first inflectional group's MorphologicalTag is a HASHTAG, false otherwise.</summary>
         *
         * <returns>true if the first inflectional group's MorphologicalTag is a HASHTAG, false otherwise.</returns>
         */
        public bool IsHashTag()
        {
            return GetInflectionalGroup(0).ContainsTag(MorphologicalTag.HASHTAG);
        }

        /**
         * <summary>The isEmail method returns true if the first inflectional group's MorphologicalTag is a EMAIL, false otherwise.</summary>
         *
         * <returns>true if the first inflectional group's MorphologicalTag is a EMAIL, false otherwise.</returns>
         */
        public bool IsEmail()
        {
            return GetInflectionalGroup(0).ContainsTag(MorphologicalTag.EMAIL);
        }

        /**
         * <summary>The isPercent method returns true if the first inflectional group's MorphologicalTag is a PERCENT, false otherwise.</summary>
         *
         * <returns>true if the first inflectional group's MorphologicalTag is a PERCENT, false otherwise.</returns>
         */
        public bool IsPercent()
        {
            return GetInflectionalGroup(0).ContainsTag(MorphologicalTag.PERCENT);
        }

        /**
         * <summary>The isFraction method returns true if the first inflectional group's MorphologicalTag is a FRACTION, false otherwise.</summary>
         *
         * <returns>true if the first inflectional group's MorphologicalTag is a FRACTION, false otherwise.</returns>
         */
        public bool IsFraction()
        {
            return GetInflectionalGroup(0).ContainsTag(MorphologicalTag.FRACTION);
        }

        /**
         * <summary>The isRange method returns true if the first inflectional group's MorphologicalTag is a RANGE, false otherwise.</summary>
         *
         * <returns>true if the first inflectional group's MorphologicalTag is a RANGE, false otherwise.</returns>
         */
        public bool IsRange()
        {
            return GetInflectionalGroup(0).ContainsTag(MorphologicalTag.RANGE);
        }

        /**
         * <summary>The isPlural method returns true if {@link InflectionalGroup}'s MorphologicalTags are from the agreement plural
         * or possessive plural, i.e A1PL, A2PL, A3PL, P1PL, P2PL or P3PL, and false otherwise.</summary>
         *
         * <returns>true if {@link InflectionalGroup}'s MorphologicalTags are from the agreement plural or possessive plural.</returns>
         */
        public bool IsPlural()
        {
            foreach (var inflectionalGroup in InflectionalGroups)
            {
                if (inflectionalGroup.ContainsPlural())
                {
                    return true;
                }
            }

            return false;
        }

        /**
         * <summary>The isAuxiliary method returns true if the root equals to the et, ol, or yap, and false otherwise.</summary>
         *
         * <returns>true if the root equals to the et, ol, or yap, and false otherwise.</returns>
         */
        public bool IsAuxiliary()
        {
            return Root.GetName() == "et" || Root.GetName() == "ol" || Root.GetName() == "yap";
        }

        /**
         * <summary>The containsTag method takes a MorphologicalTag as an input and loops through the inflectionalGroups {@link ArrayList},
         * returns true if the input matches with on of the tags in the IG, false otherwise.</summary>
         *
         * <param name="tag">checked tag</param>
         * <returns>true if the input matches with on of the tags in the IG, false otherwise.</returns>
         */
        public bool ContainsTag(MorphologicalTag tag)
        {
            foreach (var inflectionalGroup in InflectionalGroups) {
                if (inflectionalGroup.ContainsTag(tag))
                {
                    return true;
                }
            }
            return false;
        }

        /**
         * <summary>The getTreePos method returns the tree pos tag of a morphological analysis.</summary>
         *
         * <returns>Tree pos tag of the morphological analysis in string form.</returns>
         */
        public string GetTreePos()
        {
            if (IsProperNoun())
            {
                return "NP";
            }

            if (IsVerb())
            {
                return "VP";
            }

            if (IsAdjective())
            {
                return "ADJP";
            }

            if (IsNoun())
            {
                return "NP";
            }

            if (ContainsTag(MorphologicalTag.ADVERB))
            {
                return "ADVP";
            }

            if (IsCardinal())
            {
                return "QP";
            }

            if (ContainsTag(MorphologicalTag.POSTPOSITION))
            {
                return "PP";
            }

            if (ContainsTag(MorphologicalTag.CONJUNCTION))
            {
                return "CONJP";
            }

            if (ContainsTag(MorphologicalTag.DETERMINER))
            {
                return "DP";
            }

            if (ContainsTag(MorphologicalTag.INTERJECTION))
            {
                return "INTJ";
            }

            if (ContainsTag(MorphologicalTag.QUESTIONPRONOUN))
            {
                return "WP";
            }

            if (ContainsTag(MorphologicalTag.PRONOUN))
            {
                return "NP";
            }

            return "-XXX-";
        }

        /**
         * <summary>The overridden toString method gets the root and the first inflectional group as a result {@link String} then concatenates
         * with ^DB+ and the following inflectional groups.</summary>
         *
         * <returns>result {@link String}.</returns>
         */
        public override string ToString()
        {
            var result = Root.GetName() + "+" + InflectionalGroups[0];
            for (var i = 1; i < InflectionalGroups.Count; i++)
                result = result + "^DB+" + InflectionalGroups[i];
            return result;
        }
    }
}