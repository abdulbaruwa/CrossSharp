using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace CrossPuzzleClient.GameDataService
{
    public class FakePuzzleWebApiService : IPuzzleWebApiService
    {
        private const string GameJsonData =
            "[{\"PuzzleGroupId\":1,\"Name\":\"English\",\"Puzzles\":[{\"Title\":\"Level One\",\"PuzzleSubGroupId\":1,\"PuzzleGames\":[{\"PuzzleGameId\":1,\"Words\":{\"Confectioner\":\"Maker or seller of sweets.\",\"Detective\":\"Somebody who investigates wrongdoing and gathers evidence.\",\"Dome\":\"Hemispherical roof\",\"Windmill\":\"A building with a set of wind-driven revolving sails or blades to drive a grinding machine\",\"Rhubarb\":\"Plant with green or pink leaf stalks that are edible when cooked\",\"Marquee\":\"Very large tent used for parties, meetings. sales and exhibitions\",\"Stationer\":\"A seller of papers envelopes, pens. pencils and other things used in writing\",\"Hanger\":\"Building housing aircraft\",\"Rheumatism\":\"Stiffness in joints and muscles.\",\"Pylon\":\"Metal tower supporting high-voltage cables.\",\"Journalist\":\"Someone who works as a writer or editor for a newspaper. magazine, or for radio or television\",\"Monastery\":\"A monk�s residence.\"}},{\"PuzzleGameId\":2,\"Words\":{\"Fruitier\":\"More reminiscent of fruit.\",\"Secretary\":\"General clerical and administrative worker.\",\"Accordion\":\"Air-driven musical instrument.\",\"Bungalow\":\"Single-storeyhouse\",\"Jeweller\":\"Someone who makes, sells, or repairs jewellery.\",\"Ironmonger\":\"Dealer in tools and other articles made chiefly of metal.\",\"Tobacconist\":\"Person or shop that specializes in selling cigarettes. tobacco and pipes.\",\"Report\":\"To tell about what happened\",\"Police\":\"Organization for maintaining law and order\",\"Savage\":\"Violent, brutal, or undomesticated.  \",\"Slice\":\"A piece cut from something. \",\"Respond\":\"To react.\"}},{\"PuzzleGameId\":3,\"Words\":{\"Postage\":\"The price paid for mail delivery.\",\"History\":\"What happened in the past.\",\"Glory\":\"The fame, admiration and honour given to somebody who does something  important.\",\"Splice\":\"Join two pieces of rope by interweaving the strands of each into the other.\",\"Export\":\" Send goods abroad.\",\"Remind\":\"To cause a person to remember or think of something or somebody else.\",\"Manage\":\"To achieve something with difficulty.\",\"Record\":\"To make a lasting account of something.\",\"Repent\":\"To recognize the wrong in something you have done and to be sorry about it.\",\"Package\":\"To put things into a container or wrappings.\",\"Notice\":\"An announcement of information\",\"Loss\":\"The amount of money by which a company's expenses exceed income.\",\"Lose\":\"Fail to win.\",\"Import\":\"Bring in from abroad.\"}}]},{\"Title\":\"Level Two\",\"PuzzleSubGroupId\":2,\"PuzzleGames\":[{\"PuzzleGameId\":4,\"Words\":{\"Confectioner\":\"Maker or seller of sweets.\",\"Detective\":\"Somebody who investigates wrongdoing and gathers evidence.\",\"Dome\":\"Hemispherical roof\",\"Windmill\":\"A building with a set of wind-driven revolving sails or blades to drive a grinding machine\",\"Rhubarb\":\"Plant with green or pink leaf stalks that are edible when cooked\",\"Marquee\":\"Very large tent used for parties, meetings. sales and exhibitions\",\"Stationer\":\"A seller of papers envelopes, pens. pencils and other things used in writing\",\"Hanger\":\"Building housing aircraft\",\"Rheumatism\":\"Stiffness in joints and muscles.\",\"Pylon\":\"Metal tower supporting high-voltage cables.\",\"Journalist\":\"Someone who works as a writer or editor for a newspaper. magazine, or for radio or television\",\"Monastery\":\"A monk�s residence.\",\"Fruitier\":\"More reminiscent of fruit.\",\"Secretary\":\"General clerical and administrative worker.\"}},{\"PuzzleGameId\":5,\"Words\":{\"Accordion\":\"Air-driven musical instrument.\",\"Bungalow\":\"Single-storeyhouse\",\"Jeweller\":\"Someone who makes, sells, or repairs jewellery.\",\"Ironmonger\":\"Dealer in tools and other articles made chiefly of metal.\",\"Tobacconist\":\"Person or shop that specializes in selling cigarettes. tobacco and pipes.\",\"Report\":\"To tell about what happened\",\"Police\":\"Organization for maintaining law and order\",\"Savage\":\"Violent, brutal, or undomesticated.  \",\"Slice\":\"A piece cut from something. \",\"Respond\":\"To react.\",\"Postage\":\"The price paid for mail delivery.\",\"History\":\"What happened in the past.\",\"Glory\":\"The fame, admiration and honour given to somebody who does something  important.\",\"Splice\":\"Join two pieces of rope by interweaving the strands of each into the other.\"}},{\"PuzzleGameId\":6,\"Words\":{\"Export\":\" Send goods abroad.\",\"Remind\":\"To cause a person to remember or think of something or somebody else.\",\"Manage\":\"To achieve something with difficulty.\",\"Record\":\"To make a lasting account of something.\",\"Repent\":\"To recognize the wrong in something you have done and to be sorry about it.\",\"Package\":\"To put things into a container or wrappings.\",\"Notice\":\"An announcement of information\",\"Loss\":\"The amount of money by which a company's expenses exceed income.\",\"Lose\":\"Fail to win.\",\"Import\":\"Bring in from abroad.\"}}]},{\"Title\":\"Level Three\",\"PuzzleSubGroupId\":3,\"PuzzleGames\":[]}]},{\"PuzzleGroupId\":2,\"Name\":\"Science\",\"Puzzles\":[{\"Title\":\"KS2 Science\",\"PuzzleSubGroupId\":4,\"PuzzleGames\":[]},{\"Title\":\"KS3 Science\",\"PuzzleSubGroupId\":5,\"PuzzleGames\":[]}]}]";
        //private const string GameJsonData = "[{\"PuzzleGroupId\":1,\"Name\":\"Science\",\"Puzzles\":[{\"Title\":\"KS2 Science\",\"PuzzleSubGroupId\":1,\"PuzzleGames\":[{\"PuzzleGameId\":1,\"Words\":{\"Abolish 1\":\"Stop something 1\",\"Abolish 2\":\"Stop something 2\",\"Abolish 3\":\"Stop something 3\",\"Abolish 4\":\"Stop something 4\",\"Abolish 5\":\"Stop something 5\",\"Abolish 6\":\"Stop something 6\",\"Abolish 7\":\"Stop something 7\",\"Abolish 8\":\"Stop something 8\",\"Abolish 9\":\"Stop something 9\",\"Abolish 10\":\"Stop something 10\"}},{\"PuzzleGameId\":2,\"Words\":{\"Abolish 1\":\"Stop something 1\",\"Abolish 2\":\"Stop something 2\",\"Abolish 3\":\"Stop something 3\",\"Abolish 4\":\"Stop something 4\",\"Abolish 5\":\"Stop something 5\",\"Abolish 6\":\"Stop something 6\",\"Abolish 7\":\"Stop something 7\",\"Abolish 8\":\"Stop something 8\",\"Abolish 9\":\"Stop something 9\",\"Abolish 10\":\"Stop something 10\"}}]},{\"Title\":\"KS3 Science\",\"PuzzleSubGroupId\":2,\"PuzzleGames\":[{\"PuzzleGameId\":3,\"Words\":{\"Abolish 1\":\"Stop something 1\",\"Abolish 2\":\"Stop something 2\",\"Abolish 3\":\"Stop something 3\",\"Abolish 4\":\"Stop something 4\",\"Abolish 5\":\"Stop something 5\",\"Abolish 6\":\"Stop something 6\",\"Abolish 7\":\"Stop something 7\",\"Abolish 8\":\"Stop something 8\",\"Abolish 9\":\"Stop something 9\",\"Abolish 10\":\"Stop something 10\"}}]}]},{\"PuzzleGroupId\":2,\"Name\":\"English\",\"Puzzles\":[]}]";
        public async Task<Stream> GetPuzzleDataFromApi()
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK)
                               {
                                   Content = new StringContent(GameJsonData)
                               };
            return await response.Content.ReadAsStreamAsync();
        }
    }
}