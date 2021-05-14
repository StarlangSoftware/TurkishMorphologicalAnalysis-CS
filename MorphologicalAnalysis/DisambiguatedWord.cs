using Dictionary.Dictionary;

namespace MorphologicalAnalysis
{
    public class DisambiguatedWord : Word
    {
        private readonly MorphologicalParse _parse;

        /**
         * <summary> The constructor of {@link DisambiguatedWord} class which takes a {@link String} and a {@link MorphologicalParse}
         * as inputs. It creates a new {@link MorphologicalParse} with given MorphologicalParse. It generates a new instance with
         * given {@link String}.</summary>
         *
         * <param name="name"> Instances that will be a DisambiguatedWord.</param>
         * <param name="parse">{@link MorphologicalParse} of the {@link DisambiguatedWord}.</param>
         */
        public DisambiguatedWord(string name, MorphologicalParse parse) : base(name) {
            this._parse = parse;
        }

        /**
         * <summary> Accessor for the {@link MorphologicalParse}.</summary>
         *
         * <returns>MorphologicalParse.</returns>
         */
        public MorphologicalParse GetParse() {
            return _parse;
        }
        
    }
}