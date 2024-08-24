using System;
using System.Collections.Generic;
using Dictionary.Dictionary;

namespace MorphologicalAnalysis
{
    public class MorphologicalParse
    {
        protected List<InflectionalGroup> inflectionalGroups;
        protected Word root;

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
            return root;
        }

        /**
         * <summary>Another constructor of {@link MorphologicalParse} class which takes a {@link String} parse as an input. First it creates
         * an {@link ArrayList} as iGs for inflectional groups, and while given String Contains derivational boundary (^DB+), it
         * adds the substring to the iGs {@link ArrayList} and continue to use given String from 4th index. If it does not contain ^DB+,
         * it directly adds the given String to the iGs {@link ArrayList}. Then, it creates a new {@link ArrayList} as
         * inflectionalGroups and checks for some cases.
         * <p>
         * If the first item of iGs {@link ArrayList} is ++Punc, it creates a new root as +, and by calling
         * {@link InflectionalGroup} method with Punc it initializes the IG {@link ArrayList} by parsing given input
         * String IG by + and calling the getMorphologicalTag method with these substrings. If getMorphologicalTag method returns
         * a tag, it adds this tag to the IG {@link ArrayList} and also to the inflectionalGroups {@link ArrayList}.
         * </p><p>
         * If the first item of iGs {@link ArrayList} has +, it creates a new word of first item's substring from index 0 to +,
         * and assigns it to root. Then, by calling {@link InflectionalGroup} method with substring from index 0 to +,
         * it initializes the IG {@link ArrayList} by parsing given input String IG by + and calling the getMorphologicalTag
         * method with these substrings. If getMorphologicalTag method returns a tag, it adds this tag to the IG {@link ArrayList}
         * and also to the inflectionalGroups {@link ArrayList}.
         * </p><p>
         * If the first item of iGs {@link ArrayList} does not contain +, it creates a new word with first item and assigns it as root.
         * </p>
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
            inflectionalGroups = new List<InflectionalGroup>();
            if (iGs[0] == "++Punc")
            {
                root = new Word("+");
                inflectionalGroups.Add(new InflectionalGroup("Punc"));
            }
            else
            {
                if (iGs[0].IndexOf('+') != -1)
                {
                    root = new Word(iGs[0].Substring(0, iGs[0].IndexOf('+')));
                    inflectionalGroups.Add(new InflectionalGroup(iGs[0].Substring(iGs[0].IndexOf('+') + 1)));
                }
                else
                {
                    root = new Word(iGs[0]);
                }

                int i;
                for (i = 1; i < iGs.Count; i++)
                {
                    inflectionalGroups.Add(new InflectionalGroup(iGs[i]));
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
         * <p>
         * At the end, it loops through the items of inflectionalGroups and by calling {@link InflectionalGroup} method with these items
         * it initializes the IG {@link ArrayList} by parsing given input String IG by + and calling the getMorphologicalTag
         * method with these substrings. If getMorphologicalTag method returns a tag, it adds this tag to the IG {@link ArrayList}
         * and also to the inflectionalGroups {@link ArrayList}.</p></summary>
         *
         * <param name="inflectionalGroups">{@link ArrayList} input.</param>
         */
        public MorphologicalParse(List<string> inflectionalGroups)
        {
            int i;
            this.inflectionalGroups = new List<InflectionalGroup>();
            if (inflectionalGroups[0].IndexOf('+') != -1)
            {
                root = new Word(inflectionalGroups[0].Substring(0, inflectionalGroups[0].IndexOf('+')));
                this.inflectionalGroups.Add(new InflectionalGroup(inflectionalGroups[0]
                    .Substring(inflectionalGroups[0].IndexOf('+') + 1)));
            }

            for (i = 1; i < inflectionalGroups.Count; i++)
            {
                this.inflectionalGroups.Add(new InflectionalGroup(inflectionalGroups[i]));
            }
        }

        /**
         * <summary>The getTransitionList method gets the first item of inflectionalGroups {@link ArrayList} as a {@link String}, then loops
         * through the items of inflectionalGroups and concatenates them by using +.</summary>
         *
         * <returns>String that Contains transition list.</returns>
         */
        public string GetTransitionList()
        {
            var result = inflectionalGroups[0].ToString();
            for (var i = 1; i < inflectionalGroups.Count; i++)
            {
                result = result + "+" + inflectionalGroups[i];
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
                return root.GetName() + "+" + inflectionalGroups[0];
            }

            return inflectionalGroups[index].ToString();
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
            return inflectionalGroups[index];
        }

        /**
         * <summary>The getLastInflectionalGroup method directly returns the last {@link InflectionalGroup} of inflectionalGroups {@link ArrayList}.</summary>
         *
         * <returns>the last {@link InflectionalGroup} of inflectionalGroups {@link ArrayList}.</returns>
         */
        public InflectionalGroup GetLastInflectionalGroup()
        {
            return GetInflectionalGroup(inflectionalGroups.Count - 1);
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
                return root.GetName();
            }

            foreach (var group in inflectionalGroups)
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
            foreach (var group in inflectionalGroups)
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
            return inflectionalGroups.Count;
        }

        /**
         * <summary>The firstInflectionalGroup method returns the first inflectional group of inflectionalGroups {@link ArrayList}.</summary>
         *
         * <returns>the first inflectional group of inflectionalGroups {@link ArrayList}.</returns>
         */
        public InflectionalGroup FirstInflectionalGroup()
        {
            return inflectionalGroups[0];
        }

        /**
         * <summary>The lastInflectionalGroup method returns the last inflectional group of inflectionalGroups {@link ArrayList}.</summary>
         *
         * <returns>the last inflectional group of inflectionalGroups {@link ArrayList}.</returns>
         */
        public InflectionalGroup LastInflectionalGroup()
        {
            return inflectionalGroups[inflectionalGroups.Count - 1];
        }

        /**
         * <summary>The getWordWithPos method returns root with the MorphologicalTag of the first inflectional as a new word.</summary>
         *
         * <returns>root with the MorphologicalTag of the first inflectional as a new word.</returns>
         */
        public Word GetWordWithPos()
        {
            return new Word(root.GetName() + "+" + InflectionalGroup.GetTag(FirstInflectionalGroup().GetTag(0)));
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
         * <summary>lastIGContainsPossessive method returns true if the last inflectional group Contains one of the
         * possessives: P1PL, P1SG, P2PL, P2SG, P3PL AND P3SG, false otherwise.</summary>
         *
         * <returns>true if the last inflectional group Contains one of the possessives: P1PL, P1SG, P2PL, P2SG, P3PL AND P3SG, false otherwise.</returns>
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
            return char.IsUpper(root.GetName()[0]);
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
            foreach (var inflectionalGroup in inflectionalGroups)
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
            return root.GetName() == "et" || root.GetName() == "ol" || root.GetName() == "yap";
        }

        /**
         * <summary>The ContainsTag method takes a MorphologicalTag as an input and loops through the inflectionalGroups {@link ArrayList},
         * returns true if the input matches with on of the tags in the IG, false otherwise.</summary>
         *
         * <param name="tag">checked tag</param>
         * <returns>true if the input matches with on of the tags in the IG, false otherwise.</returns>
         */
        public bool ContainsTag(MorphologicalTag tag)
        {
            foreach (var inflectionalGroup in inflectionalGroups)
            {
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

            if (root.GetName().Equals("değil"))
            {
                return "NEG";
            }

            if (IsVerb())
            {
                if (LastIGContainsTag(MorphologicalTag.ZERO))
                {
                    return "NOMP";
                }

                return "VP";
            }

            if (IsAdjective())
            {
                return "ADJP";
            }

            if (IsNoun() || IsPercent())
            {
                return "NP";
            }

            if (ContainsTag(MorphologicalTag.ADVERB))
            {
                return "ADVP";
            }

            if (IsNumber() || IsFraction())
            {
                return "NUM";
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

            if (IsPunctuation())
            {
                switch (root.GetName())
                {
                    case "!":
                    case "?":
                        return ".";
                    case ";":
                    case "-":
                    case "--":
                        return ":";
                    case "(":
                    case "-LRB-":
                    case "-lrb-":
                        return "-LRB-";
                    case ")":
                    case "-RRB-":
                    case "-rrb-":
                        return "-RRB-";
                    default:
                        return root.GetName();
                }
            }

            return "-XXX-";
        }

        /// <summary>
        /// Returns the pronoun type of the parse for universal dependency feature ProType.
        /// </summary>
        /// <returns>"Art" if the pronoun is also a determiner; "Prs" if the pronoun is personal pronoun; "Rcp" if the
        /// pronoun is 'birbiri'; "Ind" if the pronoun is an indeterminate pronoun; "Neg" if the pronoun is 'hiçbiri';
        /// "Int" if the pronoun is a question pronoun; "Dem" if the pronoun is a demonstrative pronoun.</returns>
        private string GetPronType()
        {
            var lemma = root.GetName();
            if (ContainsTag(MorphologicalTag.DETERMINER))
            {
                return "Art";
            }

            if (lemma.Equals("kendi") || ContainsTag(MorphologicalTag.PERSONALPRONOUN))
            {
                return "Prs";
            }

            if (lemma.Equals("birbiri") || lemma.Equals("birbirleri"))
            {
                return "Rcp";
            }

            if (lemma.Equals("birçoğu") || lemma.Equals("hep") || lemma.Equals("kimse")
                || lemma.Equals("bazı") || lemma.Equals("biri") || lemma.Equals("çoğu")
                || lemma.Equals("hepsi") || lemma.Equals("diğeri") || lemma.Equals("tümü")
                || lemma.Equals("herkes") || lemma.Equals("kimi") || lemma.Equals("öbür")
                || lemma.Equals("öteki") || lemma.Equals("birkaçı") || lemma.Equals("topu")
                || lemma.Equals("başkası"))
            {
                return "Ind";
            }

            if (lemma.Equals("hiçbiri"))
            {
                return "Neg";
            }

            if (lemma.Equals("kim") || lemma.Equals("nere") || lemma.Equals("ne")
                || lemma.Equals("hangi") || lemma.Equals("nasıl") || lemma.Equals("kaç")
                || lemma.Equals("mi") || lemma.Equals("mı") || lemma.Equals("mu") || lemma.Equals("mü"))
            {
                return "Int";
            }

            if (ContainsTag(MorphologicalTag.DEMONSTRATIVEPRONOUN))
            {
                return "Dem";
            }

            return null;
        }

        /// <summary>
        /// Returns the numeral type of the parse for universal dependency feature NumType.
        /// </summary>
        /// <returns>"Ord" if the parse is Time, Ordinal or the word is '%' or 'kaçıncı'; "Dist" if the word is a
        /// distributive number such as 'beşinci'; "Card" if the number is cardinal or any number or the word is 'kaç'.</returns>
        private string GetNumType()
        {
            var lemma = root.GetName();
            if (lemma.Equals("%") || ContainsTag(MorphologicalTag.TIME))
            {
                return "Ord";
            }

            if (ContainsTag(MorphologicalTag.ORDINAL) || lemma.Equals("kaçıncı"))
            {
                return "Ord";
            }

            if (ContainsTag(MorphologicalTag.CARDINAL) || ContainsTag(MorphologicalTag.NUMBER) || lemma.Equals("kaç"))
            {
                return "Card";
            }

            if (ContainsTag(MorphologicalTag.DISTRIBUTIVE))
            {
                return "Dist";
            }

            return null;
        }

        /// <summary>
        /// Returns the value for the dependency feature Reflex.
        /// </summary>
        /// <returns>"Yes" if the root word is 'kendi', null otherwise.</returns>
        private string GetReflex()
        {
            var lemma = root.GetName();
            if (lemma.Equals("kendi"))
            {
                return "Yes";
            }

            return null;
        }

        /// <summary>
        /// Returns the agreement of the parse for the universal dependency feature Number.
        /// </summary>
        /// <returns>"Sing" if the agreement of the parse is singular (Contains A1SG, A2SG, A3SG); "Plur" if the agreement
        /// of the parse is plural (Contains A1PL, A2PL, A3PL).</returns>
        private string GetNumber()
        {
            if (ContainsTag(MorphologicalTag.A1SG) || ContainsTag(MorphologicalTag.A2SG) ||
                ContainsTag(MorphologicalTag.A3SG)
                || ContainsTag(MorphologicalTag.P1SG) || ContainsTag(MorphologicalTag.P2SG) ||
                ContainsTag(MorphologicalTag.P3SG))
            {
                return "Sing";
            }

            if (ContainsTag(MorphologicalTag.A1PL) || ContainsTag(MorphologicalTag.A2PL) ||
                ContainsTag(MorphologicalTag.A3PL)
                || ContainsTag(MorphologicalTag.P1PL) || ContainsTag(MorphologicalTag.P2PL) ||
                ContainsTag(MorphologicalTag.P3PL))
            {
                return "Plur";
            }

            return null;
        }

        /// <summary>
        /// Returns the possessive agreement of the parse for the universal dependency feature [Pos].
        /// </summary>
        /// <returns>"Sing" if the possessive agreement of the parse is singular (Contains P1SG, P2SG, P3SG); "Plur" if the
        /// possessive agreement of the parse is plural (Contains P1PL, P2PL, P3PL).</returns>
        private string GetPossessiveNumber()
        {
            if (ContainsTag(MorphologicalTag.P1SG) || ContainsTag(MorphologicalTag.P2SG) ||
                ContainsTag(MorphologicalTag.P3SG))
            {
                return "Sing";
            }

            if (ContainsTag(MorphologicalTag.P1PL) || ContainsTag(MorphologicalTag.P2PL) ||
                ContainsTag(MorphologicalTag.P3PL))
            {
                return "Plur";
            }

            return null;
        }

        /// <summary>
        /// Returns the case marking of the parse for the universal dependency feature case.
        /// </summary>
        /// <returns>"Acc" for accusative marker; "Dat" for dative marker; "Gen" for genitive marker; "Loc" for locative
        /// marker; "Ins" for instrumentative marker; "Abl" for ablative marker; "Nom" for nominative marker.</returns>
        private string GetCase()
        {
            if (ContainsTag(MorphologicalTag.ACCUSATIVE) || ContainsTag(MorphologicalTag.PCACCUSATIVE))
            {
                return "Acc";
            }

            if (ContainsTag(MorphologicalTag.DATIVE) || ContainsTag(MorphologicalTag.PCDATIVE))
            {
                return "Dat";
            }

            if (ContainsTag(MorphologicalTag.GENITIVE) || ContainsTag(MorphologicalTag.PCGENITIVE))
            {
                return "Gen";
            }

            if (ContainsTag(MorphologicalTag.LOCATIVE))
            {
                return "Loc";
            }

            if (ContainsTag(MorphologicalTag.INSTRUMENTAL) || ContainsTag(MorphologicalTag.PCINSTRUMENTAL))
            {
                return "Ins";
            }

            if (ContainsTag(MorphologicalTag.ABLATIVE) || ContainsTag(MorphologicalTag.PCABLATIVE))
            {
                return "Abl";
            }

            if (ContainsTag(MorphologicalTag.EQUATIVE))
            {
                return "Equ";
            }

            if (ContainsTag(MorphologicalTag.NOMINATIVE) || ContainsTag(MorphologicalTag.PCNOMINATIVE))
            {
                return "Nom";
            }

            return null;
        }

        /// <summary>
        /// Returns the definiteness of the parse for the universal dependency feature definite. It applies only for
        /// determiners in Turkish.
        /// </summary>
        /// <returns>"Ind" for 'bir', 'bazı', or 'birkaç'. "Def" for 'her', 'bu', 'şu', 'o', 'bütün'.</returns>
        private string GetDefinite()
        {
            var lemma = root.GetName();
            if (ContainsTag(MorphologicalTag.DETERMINER))
            {
                if (lemma.Equals("bir") || lemma.Equals("bazı") || lemma.Equals("birkaç") || lemma.Equals("birçok") ||
                    lemma.Equals("kimi"))
                {
                    return "Ind";
                }

                if (lemma.Equals("her") || lemma.Equals("bu") || lemma.Equals("şu") || lemma.Equals("o") ||
                    lemma.Equals("bütün"))
                {
                    return "Def";
                }
            }

            return null;
        }

        /// <summary>
        /// Returns the degree of the parse for the universal dependency feature degree.
        /// </summary>
        /// <returns>"Cmp" for comparative adverb 'daha'; "Sup" for superlative adjective or adverb 'en'.</returns>
        private string GetDegree()
        {
            var lemma = root.GetName();
            if (lemma.Equals("daha"))
            {
                return "Cmp";
            }

            if (lemma.Equals("en") && !IsNoun())
            {
                return "Sup";
            }

            return null;
        }

        /// <summary>
        /// Returns the polarity of the verb for the universal dependency feature polarity.
        /// </summary>
        /// <returns>"Pos" for positive polarity containing tag POS; "Neg" for negative polarity containing tag NEG.</returns>
        private string GetPolarity()
        {
            if (root.GetName().Equals("değil"))
            {
                return "Neg";
            }

            if (ContainsTag(MorphologicalTag.POSITIVE))
            {
                return "Pos";
            }

            if (ContainsTag(MorphologicalTag.NEGATIVE))
            {
                return "Neg";
            }

            return null;
        }

        /// <summary>
        /// Returns the person of the agreement of the parse for the universal dependency feature person.
        /// </summary>
        /// <returns>"1" for first person; "2" for second person; "3" for third person.</returns>
        private string GetPerson()
        {
            if (ContainsTag(MorphologicalTag.A1SG) || ContainsTag(MorphologicalTag.A1PL)
                                                   || ContainsTag(MorphologicalTag.P1SG) ||
                                                   ContainsTag(MorphologicalTag.P1PL))
            {
                return "1";
            }

            if (ContainsTag(MorphologicalTag.A2SG) || ContainsTag(MorphologicalTag.A2PL)
                                                   || ContainsTag(MorphologicalTag.P2SG) ||
                                                   ContainsTag(MorphologicalTag.P2PL))
            {
                return "2";
            }

            if (ContainsTag(MorphologicalTag.A3SG) || ContainsTag(MorphologicalTag.A3PL)
                                                   || ContainsTag(MorphologicalTag.P3SG) ||
                                                   ContainsTag(MorphologicalTag.P3PL))
            {
                return "3";
            }

            return null;
        }

        /// <summary>
        /// Returns the person of the possessive agreement of the parse for the universal dependency feature [pos].
        /// </summary>
        /// <returns>"1" for first person; "2" for second person; "3" for third person.</returns>
        private string GetPossessivePerson()
        {
            if (ContainsTag(MorphologicalTag.P1SG) || ContainsTag(MorphologicalTag.P1PL))
            {
                return "1";
            }

            if (ContainsTag(MorphologicalTag.P2SG) || ContainsTag(MorphologicalTag.P2PL))
            {
                return "2";
            }

            if (ContainsTag(MorphologicalTag.P3SG) || ContainsTag(MorphologicalTag.P3PL))
            {
                return "3";
            }

            return null;
        }

        /// <summary>
        /// Returns the voice of the verb parse for the universal dependency feature voice.
        /// </summary>
        /// <returns>"CauPass" if the verb parse is both causative and passive; "Pass" if the verb parse is only passive;
        /// "Rcp" if the verb parse is reciprocal; "Cau" if the verb parse is only causative; "Rfl" if the verb parse is
        /// reflexive.</returns>
        private string GetVoice()
        {
            if (ContainsTag(MorphologicalTag.CAUSATIVE) && ContainsTag(MorphologicalTag.PASSIVE))
            {
                return "CauPass";
            }

            if (ContainsTag(MorphologicalTag.PASSIVE))
            {
                return "Pass";
            }

            if (ContainsTag(MorphologicalTag.RECIPROCAL))
            {
                return "Rcp";
            }

            if (ContainsTag(MorphologicalTag.CAUSATIVE))
            {
                return "Cau";
            }

            if (ContainsTag(MorphologicalTag.REFLEXIVE))
            {
                return "Rfl";
            }

            return null;
        }

        /// <summary>
        /// Returns the aspect of the verb parse for the universal dependency feature aspect.
        /// </summary>
        /// <returns>"Perf" for past, narrative and future tenses; "Prog" for progressive tenses; "Hab" for Aorist; "Rapid"
        /// for parses containing HASTILY tag; "Dur" for parses containing START, STAY or REPEAT tags.</returns>
        private string GetAspect()
        {
            if (ContainsTag(MorphologicalTag.PASTTENSE) || ContainsTag(MorphologicalTag.NARRATIVE) ||
                ContainsTag(MorphologicalTag.FUTURE))
            {
                return "Perf";
            }

            if (ContainsTag(MorphologicalTag.PROGRESSIVE1) || ContainsTag(MorphologicalTag.PROGRESSIVE2))
            {
                return "Prog";
            }

            if (ContainsTag(MorphologicalTag.AORIST))
            {
                return "Hab";
            }

            if (ContainsTag(MorphologicalTag.HASTILY))
            {
                return "Rapid";
            }

            if (ContainsTag(MorphologicalTag.START) || ContainsTag(MorphologicalTag.STAY) ||
                ContainsTag(MorphologicalTag.REPEAT))
            {
                return "Dur";
            }

            return null;
        }

        /// <summary>
        /// Returns the tense of the verb parse for universal dependency feature tense.
        /// </summary>
        /// <returns>"Past" for simple past tense; "Fut" for future tense; "Pqp" for narrative past tense; "Pres" for other
        /// past tenses.</returns>
        private string GetTense()
        {
            if (ContainsTag(MorphologicalTag.NARRATIVE) && ContainsTag(MorphologicalTag.PASTTENSE))
            {
                return "Pqp";
            }

            if (ContainsTag(MorphologicalTag.NARRATIVE) || ContainsTag(MorphologicalTag.PASTTENSE))
            {
                return "Past";
            }

            if (ContainsTag(MorphologicalTag.FUTURE))
            {
                return "Fut";
            }

            if (!ContainsTag(MorphologicalTag.PASTTENSE) && !ContainsTag(MorphologicalTag.FUTURE))
            {
                return "Pres";
            }

            return null;
        }

        /// <summary>
        /// Returns the modality of the verb parse for the universal dependency feature mood.
        /// </summary>
        /// <returns>"GenNecPot" if both necessitative and potential is combined with a suffix of general modality;
        /// "CndGenPot" if both conditional and potential is combined with a suffix of general modality;
        /// "GenNec" if necessitative is combined with a suffix of general modality;
        /// "GenPot" if potential is combined with a suffix of general modality;
        /// "NecPot" if necessitative is combined with potential;
        /// "DesPot" if desiderative is combined with potential;
        /// "CndPot" if conditional is combined with potential;
        /// "CndGen" if conditional is combined with a suffix of general modality;
        /// "Imp" for imperative; "Cnd" for simple conditional; "Des" for simple desiderative; "Opt" for optative; "Nec" for
        /// simple necessitative; "Pot" for simple potential; "Gen" for simple suffix of a general modality.</returns>
        private string GetMood()
        {
            if ((ContainsTag(MorphologicalTag.COPULA) || ContainsTag(MorphologicalTag.AORIST)) &&
                ContainsTag(MorphologicalTag.NECESSITY) && ContainsTag(MorphologicalTag.ABLE))
            {
                return "GenNecPot";
            }

            if ((ContainsTag(MorphologicalTag.COPULA) || ContainsTag(MorphologicalTag.AORIST)) &&
                ContainsTag(MorphologicalTag.CONDITIONAL) && ContainsTag(MorphologicalTag.ABLE))
            {
                return "CndGenPot";
            }

            if ((ContainsTag(MorphologicalTag.COPULA) || ContainsTag(MorphologicalTag.AORIST)) &&
                ContainsTag(MorphologicalTag.NECESSITY))
            {
                return "GenNec";
            }

            if ((ContainsTag(MorphologicalTag.COPULA) || ContainsTag(MorphologicalTag.AORIST)) &&
                ContainsTag(MorphologicalTag.ABLE))
            {
                return "GenPot";
            }

            if (ContainsTag(MorphologicalTag.NECESSITY) && ContainsTag(MorphologicalTag.ABLE))
            {
                return "NecPot";
            }

            if (ContainsTag(MorphologicalTag.DESIRE) && ContainsTag(MorphologicalTag.ABLE))
            {
                return "DesPot";
            }

            if (ContainsTag(MorphologicalTag.CONDITIONAL) && ContainsTag(MorphologicalTag.ABLE))
            {
                return "CndPot";
            }

            if (ContainsTag(MorphologicalTag.CONDITIONAL) &&
                (ContainsTag(MorphologicalTag.COPULA) || ContainsTag(MorphologicalTag.AORIST)))
            {
                return "CndGen";
            }

            if (ContainsTag(MorphologicalTag.IMPERATIVE))
            {
                return "Imp";
            }

            if (ContainsTag(MorphologicalTag.CONDITIONAL))
            {
                return "Cnd";
            }

            if (ContainsTag(MorphologicalTag.DESIRE))
            {
                return "Des";
            }

            if (ContainsTag(MorphologicalTag.OPTATIVE))
            {
                return "Opt";
            }

            if (ContainsTag(MorphologicalTag.NECESSITY))
            {
                return "Nec";
            }

            if (ContainsTag(MorphologicalTag.ABLE))
            {
                return "Pot";
            }

            if (ContainsTag(MorphologicalTag.PASTTENSE) || ContainsTag(MorphologicalTag.PROGRESSIVE1) ||
                ContainsTag(MorphologicalTag.FUTURE))
            {
                return "Ind";
            }

            if ((ContainsTag(MorphologicalTag.COPULA) || ContainsTag(MorphologicalTag.AORIST)))
            {
                return "Gen";
            }

            if (ContainsTag(MorphologicalTag.ZERO) && !ContainsTag(MorphologicalTag.A3PL))
            {
                return "Gen";
            }

            return null;
        }

        /// <summary>
        /// Returns the form of the verb parse for the universal dependency feature verbForm.
        /// </summary>
        /// <returns>"Part" for participles; "Vnoun" for infinitives; "Conv" for parses contaning tags SINCEDOINGSO,
        /// WITHOUTHAVINGDONESO, WITHOUTBEINGABLETOHAVEDONESO, BYDOINGSO, AFTERDOINGSO, INFINITIVE3; "Fin" for others.</returns>
        private string GetVerbForm()
        {
            if (ContainsTag(MorphologicalTag.PASTPARTICIPLE) || ContainsTag(MorphologicalTag.FUTUREPARTICIPLE) ||
                ContainsTag(MorphologicalTag.PRESENTPARTICIPLE))
            {
                return "Part";
            }

            if (ContainsTag(MorphologicalTag.INFINITIVE) || ContainsTag(MorphologicalTag.INFINITIVE2))
            {
                return "Vnoun";
            }

            if (ContainsTag(MorphologicalTag.SINCEDOINGSO) || ContainsTag(MorphologicalTag.WITHOUTHAVINGDONESO) ||
                ContainsTag(MorphologicalTag.WITHOUTBEINGABLETOHAVEDONESO) || ContainsTag(MorphologicalTag.BYDOINGSO) ||
                ContainsTag(MorphologicalTag.AFTERDOINGSO) || ContainsTag(MorphologicalTag.INFINITIVE3))
            {
                return "Conv";
            }

            if (ContainsTag(MorphologicalTag.COPULA) || ContainsTag(MorphologicalTag.ABLE) ||
                ContainsTag(MorphologicalTag.AORIST) || ContainsTag(MorphologicalTag.PROGRESSIVE2)
                || ContainsTag(MorphologicalTag.DESIRE) || ContainsTag(MorphologicalTag.NECESSITY) ||
                ContainsTag(MorphologicalTag.CONDITIONAL) || ContainsTag(MorphologicalTag.IMPERATIVE) ||
                ContainsTag(MorphologicalTag.OPTATIVE)
                || ContainsTag(MorphologicalTag.PASTTENSE) || ContainsTag(MorphologicalTag.NARRATIVE) ||
                ContainsTag(MorphologicalTag.PROGRESSIVE1) || ContainsTag(MorphologicalTag.FUTURE)
                || (ContainsTag(MorphologicalTag.ZERO) && !ContainsTag(MorphologicalTag.A3PL)))
            {
                return "Fin";
            }

            return null;
        }

        private string GetEvident()
        {
            if (ContainsTag(MorphologicalTag.NARRATIVE))
            {
                return "Nfh";
            }
            else
            {
                if (ContainsTag(MorphologicalTag.COPULA) || ContainsTag(MorphologicalTag.ABLE) ||
                    ContainsTag(MorphologicalTag.AORIST) || ContainsTag(MorphologicalTag.PROGRESSIVE2)
                    || ContainsTag(MorphologicalTag.DESIRE) || ContainsTag(MorphologicalTag.NECESSITY) ||
                    ContainsTag(MorphologicalTag.CONDITIONAL) || ContainsTag(MorphologicalTag.IMPERATIVE) ||
                    ContainsTag(MorphologicalTag.OPTATIVE)
                    || ContainsTag(MorphologicalTag.PASTTENSE) || ContainsTag(MorphologicalTag.NARRATIVE) ||
                    ContainsTag(MorphologicalTag.PROGRESSIVE1) || ContainsTag(MorphologicalTag.FUTURE))
                {
                    return "Fh";
                }
            }

            return null;
        }

        /// <summary>
        /// Construct the universal dependency features as an array of strings. Each element represents a single feature.
        /// Every feature is given as featureType = featureValue.
        /// </summary>
        /// <param name="uPos">Universal dependency part of speech tag for the parse.</param>
        /// <returns>An array of universal dependency features for this parse.</returns>
        public List<string> GetUniversalDependencyFeatures(string uPos)
        {
            var featureList = new List<string>();
            var pronType = GetPronType();
            if (pronType != null && uPos != "NOUN" && uPos != "ADJ" && uPos != "VERB" && uPos != "CCONJ" &&
                uPos != "PROPN")
            {
                featureList.Add("PronType=" + pronType);
            }

            var numType = GetNumType();
            if (numType != null && uPos != "VERB" && uPos != "NOUN" && uPos != "ADV")
            {
                featureList.Add("NumType=" + numType);
            }

            var reflex = GetReflex();
            if (reflex != null && uPos != "ADJ" && uPos != "VERB")
            {
                featureList.Add("Reflex=" + reflex);
            }

            var degree = GetDegree();
            if (degree != null && uPos != "ADJ")
            {
                featureList.Add("Degree=" + degree);
            }

            if (IsNoun() || IsVerb() || root.GetName().Equals("mi") || (pronType != null && !pronType.Equals("Art")))
            {
                var number = GetNumber();
                if (number != null)
                {
                    featureList.Add("Number=" + number);
                }

                var possessiveNumber = GetPossessiveNumber();
                if (possessiveNumber != null)
                {
                    featureList.Add("Number[psor]=" + possessiveNumber);
                }

                var person = GetPerson();
                if (person != null && uPos != "PROPN")
                {
                    featureList.Add("Person=" + person);
                }

                var possessivePerson = GetPossessivePerson();
                if (possessivePerson != null && uPos != "PROPN")
                {
                    featureList.Add("Person[psor]=" + possessivePerson);
                }
            }

            if (IsNoun() || (pronType != null && !pronType.Equals("Art")))
            {
                var case_ = GetCase();
                if (case_ != null)
                {
                    featureList.Add("Case=" + case_);
                }
            }

            if (ContainsTag(MorphologicalTag.DETERMINER))
            {
                var definite = GetDefinite();
                if (definite != null)
                {
                    featureList.Add("Definite=" + definite);
                }
            }

            if (IsVerb() || root.GetName().Equals("mi"))
            {
                var polarity = GetPolarity();
                if (polarity != null)
                {
                    featureList.Add("Polarity=" + polarity);
                }

                var voice = GetVoice();
                if (voice != null)
                {
                    featureList.Add("Voice=" + voice);
                }

                var aspect = GetAspect();
                if (aspect != null && uPos != "PROPN" && !root.GetName().Equals("mi"))
                {
                    featureList.Add("Aspect=" + aspect);
                }

                var tense = GetTense();
                if (tense != null && uPos != "PROPN")
                {
                    featureList.Add("Tense=" + tense);
                }

                var mood = GetMood();
                if (mood != null && uPos != "PROPN" && !root.GetName().Equals("mi"))
                {
                    featureList.Add("Mood=" + mood);
                }

                var verbForm = GetVerbForm();
                if (verbForm != null && uPos != "PROPN")
                {
                    featureList.Add("VerbForm=" + verbForm);
                }

                var evident = GetEvident();
                if (mood != null && !root.GetName().Equals("mi"))
                {
                    featureList.Add("Evident=" + evident);
                }
            }

            featureList.Sort(StringComparer.OrdinalIgnoreCase);
            return featureList;
        }

        /// <summary>
        /// Returns the universal dependency part of speech for this parse.
        /// </summary>
        /// <returns>"AUX" for word 'değil; "PROPN" for proper nouns; "NOUN for nouns; "ADJ" for adjectives; "ADV" for
        /// adverbs; "INTJ" for interjections; "VERB" for verbs; "PUNCT" for punctuation symbols; "DET" for determiners;
        /// "NUM" for numerals; "PRON" for pronouns; "ADP" for post participles; "SCONJ" or "CCONJ" for conjunctions.</returns>
        public string GetUniversalDependencyPos()
        {
            var lemma = root.GetName();
            if (lemma.Equals("değil"))
            {
                return "AUX";
            }

            if (IsProperNoun())
            {
                return "PROPN";
            }

            if (IsNoun())
            {
                return "NOUN";
            }

            if (IsAdjective())
            {
                return "ADJ";
            }

            if (GetPos().Equals("ADV"))
            {
                return "ADV";
            }

            if (ContainsTag(MorphologicalTag.INTERJECTION))
            {
                return "INTJ";
            }

            if (IsVerb())
            {
                return "VERB";
            }

            if (IsPunctuation() || IsHashTag())
            {
                return "PUNCT";
            }

            if (ContainsTag(MorphologicalTag.DETERMINER))
            {
                return "DET";
            }

            if (IsNumber() || IsDate() || IsTime() || IsOrdinal() || IsFraction() || lemma.Equals("%"))
            {
                return "NUM";
            }

            if (GetPos().Equals("PRON"))
            {
                return "PRON";
            }

            if (GetPos().Equals("POSTP"))
            {
                return "ADP";
            }

            if (GetPos().Equals("QUES"))
            {
                return "AUX";
            }

            if (GetPos().Equals("CONJ"))
            {
                if (lemma.Equals("ki") || lemma.Equals("eğer") || lemma.Equals("diye"))
                {
                    return "SCONJ";
                }
                else
                {
                    return "CCONJ";
                }
            }

            return "X";
        }

        /**
         * <summary>The overridden toString method gets the root and the first inflectional group as a result {@link String} then concatenates
         * with ^DB+ and the following inflectional groups.</summary>
         *
         * <returns>result {@link String}.</returns>
         */
        public override string ToString()
        {
            var result = root.GetName() + "+" + inflectionalGroups[0];
            for (var i = 1; i < inflectionalGroups.Count; i++)
                result = result + "^DB+" + inflectionalGroups[i];
            return result;
        }
    }
}