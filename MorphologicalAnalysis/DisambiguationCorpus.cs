using System.Collections.Generic;
using System.IO;
using Corpus;
using DataStructure;
using Dictionary.Dictionary;

namespace MorphologicalAnalysis
{
    public class DisambiguationCorpus : Corpus.Corpus
    {
        /**
         * <summary> Constructor which creates an {@link ArrayList} of sentences and a {@link CounterHashMap} of wordList.</summary>
         */
        public DisambiguationCorpus()
        {
            sentences = new List<Sentence>();
            wordList = new CounterHashMap<Word>();
        }

        /**
         * <summary> Constructor which creates a new empty copy of the {@link DisambiguationCorpus}.</summary>
         *
         * <returns>An empty copy of the {@link DisambiguationCorpus}.</returns>
         */
        public new DisambiguationCorpus EmptyCopy()
        {
            return new DisambiguationCorpus();
        }

        /**
         * <summary> Constructor which takes a file name {@link string} as an input and reads the file line by line. It takes each word of the line,
         * and creates a new {@link DisambiguatedWord} with current word and its {@link MorphologicalParse}. It also creates a new {@link Sentence}
         * when a new sentence starts, and adds each word to this sentence till the end of that sentence.</summary>
         *
         * <param name="fileName">File which will be read and parsed.</param>
         */
        public DisambiguationCorpus(string fileName)
        {
            var i = 1;
            Sentence newSentence = null;
            var streamReader = new StreamReader(fileName);
            var line = streamReader.ReadLine();
            while (line != null)
            {
                var word = line.Substring(0, line.IndexOf("\t"));
                var parse = line.Substring(line.IndexOf("\t") + 1);
                if (word != "" && parse != "")
                {
                    var newWord = new DisambiguatedWord(word, new MorphologicalParse(parse));
                    if (word.Equals("<S>"))
                    {
                        newSentence = new Sentence();
                    }
                    else
                    {
                        if (word.Equals("</S>"))
                        {
                            AddSentence(newSentence);
                        }
                        else
                        {
                            if (word.Equals("<DOC>") || word.Equals("</DOC>") || word.Equals("<TITLE>") ||
                                word.Equals("</TITLE>"))
                            {
                            }
                            else
                            {
                                if (newSentence != null)
                                {
                                    newSentence.AddWord(newWord);
                                }
                            }
                        }
                    }
                }

                i++;
                line = streamReader.ReadLine();
            }
        }

        /**
         * <summary> The writeToFile method takes a {@link string} file name as an input and writes the elements of sentences {@link ArrayList}
         * to this file with proper tags which indicates the beginnings and endings of the document and sentence.</summary>
         *
         * <param name="fileName">File which will be filled with the sentences.</param>
         */
        public new void WriteToFile(string fileName)
        {
            var writer = new StreamWriter(fileName);
            writer.WriteLine("<DOC>\t<DOC>+BDTag");
            foreach (var sentence in sentences)
            {
                writer.WriteLine("<S>\t<S>+BSTag");
                for (var i = 0; i < sentence.WordCount(); i++)
                {
                    var word = (DisambiguatedWord) sentence.GetWord(i);
                    writer.WriteLine(word.GetName() + "\t" + word.GetParse());
                }

                writer.WriteLine("</S>\t</S>+ESTag");
            }

            writer.WriteLine("</DOC>\t</DOC>+EDTag");
            writer.Close();
        }
    }
}