using Dictionary.Dictionary;
using Dictionary.Language;

namespace MorphologicalAnalysis
{
    public class MorphotacticEngine
    {
        public static string ResolveD(TxtWord root, string formation, string formationToCheck)
        {
            if (root.IsAbbreviation())
            {
                return formation + 'd';
            }

            if (Word.LastPhoneme(formationToCheck) >= '0' && Word.LastPhoneme(formationToCheck) <= '9')
            {
                switch (Word.LastPhoneme(formationToCheck))
                {
                    case '3':
                    case '4':
                    case '5':
                        //3->3'tü, 5->5'ti, 4->4'tü
                        return formation + 't';
                    case '0':
                        if (root.GetName().EndsWith("40") || root.GetName().EndsWith("60") ||
                            root.GetName().EndsWith("70"))
                            //40->40'tı, 60->60'tı, 70->70'ti
                            return formation + 't';
                        else
                            //30->30'du, 50->50'ydi, 80->80'di
                            return formation + 'd';
                    default:
                        return formation + 'd';
                }
            }

            if (TurkishLanguage.IsSertSessiz(Word.LastPhoneme(formationToCheck)))
            {
                //yap+DH->yaptı
                return formation + 't';
            }

            //sar+DH->sardı
            return formation + 'd';
        }

        public static string ResolveA(TxtWord root, string formation, bool rootWord, string formationToCheck)
        {
            if (root.IsAbbreviation())
            {
                return formation + 'e';
            }

            if (Word.LastVowel(formationToCheck) >= '0' && Word.LastVowel(formationToCheck) <= '9')
            {
                switch (Word.LastVowel(formationToCheck))
                {
                    case '6':
                    case '9':
                        //6'ya, 9'a
                        return formation + 'a';
                    case '0':
                        if (root.GetName().EndsWith("10") || root.GetName().EndsWith("30") ||
                            root.GetName().EndsWith("40") || root.GetName().EndsWith("60") ||
                            root.GetName().EndsWith("90"))
                            //10'a, 30'a, 40'a, 60'a, 90'a
                            return formation + 'a';
                        else
                            //20'ye, 50'ye, 80'e, 70'e
                            return formation + 'e';
                    default:
                        //3'e, 8'e, 4'e, 2'ye
                        return formation + 'e';
                }
            }

            if (TurkishLanguage.IsBackVowel(Word.LastVowel(formationToCheck)))
            {
                if (root.NotObeysVowelHarmonyDuringAgglutination() && rootWord)
                {
                    //alkole, anormale, ampule, tümamirali, spirali, sosyali
                    return formation + 'e';
                }

                //sakala, kabala, eve, kediye
                return formation + 'a';
            }

            if (TurkishLanguage.IsFrontVowel(Word.LastVowel(formationToCheck)))
            {
                if (root.NotObeysVowelHarmonyDuringAgglutination() && rootWord)
                {
                    //sakala, kabala, eve, kediye
                    return formation + 'a';
                }

                //alkole, anormale, ampule, tümamirali, spirali, sosyali
                return formation + 'e';
            }

            if (root.IsNumeral() || root.IsFraction() || root.IsReal())
            {
                if (root.GetName().EndsWith("6") || root.GetName().EndsWith("9") || root.GetName().EndsWith("10") ||
                    root.GetName().EndsWith("30") || root.GetName().EndsWith("40") || root.GetName().EndsWith("60") ||
                    root.GetName().EndsWith("90"))
                {
                    return formation + 'a';
                }

                return formation + 'e';
            }

            return formation;
        }
        
        public static string ResolveH(TxtWord root, string formation, bool beginningOfSuffix,
            bool specialCaseTenseSuffix, bool rootWord, string formationToCheck)
        {
            if (root.IsAbbreviation())
                return formation + 'i';
            if (beginningOfSuffix && TurkishLanguage.IsVowel(Word.LastPhoneme(formationToCheck)) && !specialCaseTenseSuffix)
            {
                return formation;
            }

            if (specialCaseTenseSuffix)
            {
                //eğer ek Hyor eki ise,
                if (rootWord)
                {
                    if (root.VowelAChangesToIDuringYSuffixation())
                    {
                        if (TurkishLanguage.IsFrontRoundedVowel(Word.BeforeLastVowel(formationToCheck)))
                        {
                            //büyülüyor, bölümlüyor, çözümlüyor, döşüyor
                            return formation.Substring(0, formation.Length - 1) + 'ü';
                        }

                        if (TurkishLanguage.IsFrontUnroundedVowel(Word.BeforeLastVowel(formationToCheck)))
                        {
                            //adresliyor, alevliyor, ateşliyor, bekliyor
                            return formation.Substring(0, formation.Length - 1) + 'i';
                        }

                        if (TurkishLanguage.IsBackRoundedVowel(Word.BeforeLastVowel(formationToCheck)))
                        {
                            //buğuluyor, bulguluyor, çamurluyor, aforozluyor
                            return formation.Substring(0, formation.Length - 1) + 'u';
                        }

                        if (TurkishLanguage.IsBackUnroundedVowel(Word.BeforeLastVowel(formationToCheck)))
                        {
                            //açıklıyor, çalkalıyor, gazlıyor, gıcırdıyor
                            return formation.Substring(0, formation.Length - 1) + 'ı';
                        }
                    }
                }

                if (TurkishLanguage.IsVowel(Word.LastPhoneme(formationToCheck)))
                {
                    if (TurkishLanguage.IsFrontRoundedVowel(Word.BeforeLastVowel(formationToCheck)))
                    {
                        return formation.Substring(0, formation.Length - 1) + 'ü';
                    }

                    if (TurkishLanguage.IsFrontUnroundedVowel(Word.BeforeLastVowel(formationToCheck)))
                    {
                        return formation.Substring(0, formation.Length - 1) + 'i';
                    }

                    if (TurkishLanguage.IsBackRoundedVowel(Word.BeforeLastVowel(formationToCheck)))
                    {
                        return formation.Substring(0, formation.Length - 1) + 'u';
                    }

                    if (TurkishLanguage.IsBackUnroundedVowel(Word.BeforeLastVowel(formationToCheck)))
                    {
                        return formation.Substring(0, formation.Length - 1) + 'ı';
                    }
                }
            }

            if (TurkishLanguage.IsFrontRoundedVowel(Word.LastVowel(formationToCheck)) ||
                (TurkishLanguage.IsBackRoundedVowel(Word.LastVowel(formationToCheck)) &&
                 root.NotObeysVowelHarmonyDuringAgglutination()))
            {
                return formation + 'ü';
            }

            if ((TurkishLanguage.IsFrontUnroundedVowel(Word.LastVowel(formationToCheck)) && !root.NotObeysVowelHarmonyDuringAgglutination()) ||
                ((Word.LastVowel(formationToCheck) == 'a' || Word.LastVowel(formationToCheck) == 'â') && root.NotObeysVowelHarmonyDuringAgglutination()))
            {
                return formation + 'i';
            }

            if (TurkishLanguage.IsBackRoundedVowel(Word.LastVowel(formationToCheck)))
            {
                return formation + 'u';
            }

            if (TurkishLanguage.IsBackUnroundedVowel(Word.LastVowel(formationToCheck)) || (TurkishLanguage.IsFrontUnroundedVowel(Word.LastVowel(formationToCheck)) && root.NotObeysVowelHarmonyDuringAgglutination()))
            {
                return formation + 'ı';
            }

            if (root.IsNumeral() || root.IsFraction() || root.IsReal())
            {
                if (root.GetName().EndsWith("6") || root.GetName().EndsWith("40") || root.GetName().EndsWith("60") ||
                    root.GetName().EndsWith("90"))
                {
                    //6'yı, 40'ı, 60'ı
                    return formation + 'ı';
                }

                if (root.GetName().EndsWith("3") || root.GetName().EndsWith("4") || root.GetName().EndsWith("00"))
                {
                    //3'ü, 4'ü, 100'ü
                    return formation + 'ü';
                }

                if (root.GetName().EndsWith("9") || root.GetName().EndsWith("10") ||
                    root.GetName().EndsWith("30"))
                {
                    //9'u, 10'u, 30'u
                    return formation + 'u';
                }

                //2'yi, 5'i, 8'i
                return formation + 'i';
            }

            return formation;
        }

        /**
         * <summary>The resolveC method takes a {@link string} formation as an input. If the last phoneme is on of the "çfhkpsşt", it
         * concatenates given formation with 'ç', if not it concatenates given formation with 'c'.</summary>
         *
         * <param name="formation">{@link string} input.</param>
         * <returns>resolved string.</returns>
         */
        public static string ResolveC(string formation, string formationToCheck)
        {
            if (TurkishLanguage.IsSertSessiz(Word.LastPhoneme(formationToCheck)))
            {
                return formation + 'ç';
            }

            return formation + 'c';
        }

        /**
         * <summary>The resolveS method takes a {@link string} formation as an input. It then concatenates given formation with 's'.</summary>
         *
         * <param name="formation">{@link string} input.</param>
         * <returns>resolved string.</returns>
         */
        public static string ResolveS(string formation)
        {
            return formation + 's';
        }

        /**
         * <summary>The resolveSh method takes a {@link string} formation as an input. If the last character is a vowel, it concatenates
         * given formation with ş, if the last character is not a vowel, and not 't' it directly returns given formation, but if it
         * is equal to 't', it transforms it to 'd'.</summary>
         *
         * <param name="formation">{@link string} input.</param>
         * <returns>resolved string.</returns>
         */
        public static string ResolveSh(string formation)
        {
            if (TurkishLanguage.IsVowel(formation[formation.Length - 1]))
            {
                return formation + 'ş';
            }

            if (formation[formation.Length - 1] != 't')
                return formation;
            return formation.Substring(0, formation.Length - 1) + 'd';
        }
    }
}