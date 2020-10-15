using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Discord;
using Discord.Commands;
using System.Threading.Tasks;
using Catalyst;
using Mosaik.Core;
using Catalyst.Models;

namespace KawaiiBot2.Modules
{
    public class ButtsBot : ModuleBase<SocketCommandContext>
    {
        Random r = new Random();
        /*[Command("buttsbot", RunMode = RunMode.Async)]*/
        public async Task ButtsBotTransform([Remainder] string sentence)
        {
            //Turn the sentence into a NLP document
            var doc = new Document(sentence, Language.English);
            //some things stolen from example to prepare the NLP thingy
            Storage.Current = new OnlineRepositoryStorage(new DiskStorage("catalyst-models"));
            var nlp = await Pipeline.ForAsync(Language.English);
            //Process it with the NLP thing
            var processed = nlp.ProcessSingle(doc);

            //Get the tokens
            var tl = processed.ToTokenList();


            //Select all nouns (and sometimes proper nouns)
            List<IToken> nounTokens = (from token in processed.ToTokenList()
                                       where token.POS == PartOfSpeech.NOUN || token.POS == PartOfSpeech.PROPN
                                       select token).ToList();

            //strings to print out
            List<string> resultStrings = new List<string>();
            //Index of the string to start taking from: The absolute beginning
            int takeIndex = 0;
            foreach (var noun in nounTokens)
            {
                //If the random wills it, or if we're on the last noun and we don't have any selected yet
                bool ReplaceThis = r.Next(100) > 66 || (noun.Value == nounTokens[^1].Value && resultStrings.Count == 0);

                if (ReplaceThis)
                {
                    //Add everything up to this thing
                    resultStrings.Add(sentence.Substring(takeIndex, noun.Begin - takeIndex));
                    //then add butts
                    resultStrings.Add("butts");
                    //Nouns can have . after: If they do we want that included, if we don't we want +1 in order to go past the length
                    takeIndex = noun.End + (noun.Value.EndsWith('.') ? 0 : 1);
                }
            }
            //Add the rest of the sentence
            resultStrings.Add(sentence.Substring(takeIndex, sentence.Length - takeIndex));

            await ReplyAsync(string.Join("", resultStrings).Clean());
            ;
        }
    }
}
