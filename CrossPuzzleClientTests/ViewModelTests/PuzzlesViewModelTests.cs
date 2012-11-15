using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CrossPuzzleClient.DataModel;
using CrossPuzzleClient.GameDataService;
using CrossPuzzleClient.Infrastructure;
using CrossPuzzleClient.ViewModels;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Newtonsoft.Json;
using Windows.UI.Xaml.Controls;

namespace CrossPuzzleClientTests.ViewModelTests
{
    [TestClass]
    public class PuzzlesViewModelTests
    {
         
        [TestMethod]
        public void when_StartPuzzleCommand_is_fired_it_should_send_a_StartPuzzleMessage_with_the_selected_puzzleId_set_in_it()
        {
            var puzzlesViewModel = new PuzzlesViewModel(new FakeNavigationService(), new FakePuzzleRepository());
            puzzlesViewModel.SelectedPuzzleGroupViewModel = new PuzzleViewModel()
                                                                {
                                                                    PuzzleId = 1,
                                                                    Title = "Science",
                                                                    Words = new List<string>()
                                                                };

            int testResult = 0;
            Messenger.Default.Register<StartPuzzleMessage>(this,m=> testResult = m.PuzzleId);


            puzzlesViewModel.StartPuzzleCommand.Execute(null);

            Assert.AreEqual(1,testResult);
        }

        [TestMethod]
        public async Task When_GameCompleteMessage_is_received_should_save_the_game_score_for_the_selected_user_in_the_database()
        {
            var puzzlesViewModel = new PuzzlesViewModel(new FakeNavigationService(), new FakePuzzleRepository());


            var gameCompleteMessage = new GameCompleteMessage()
                               {
                                   GameId = 1,
                                   ScorePercentage = 95,
                                   UserName = "Abdul"
                               };
            var puzzleGroupsTask = GenerateUserGameDataFromPuzzleGroupData("Abdul");
            var puzzlegroupData = await puzzleGroupsTask;
            var puzzleGroups = (from p in puzzlegroupData
                                from pg in p.Puzzles
                                where pg.PuzzleSubGroupId == gameCompleteMessage.GameId
                                select p).ToList();

            puzzlesViewModel.PuzzleGroupData = puzzleGroups;


            //Assert
            Messenger.Default.Send<GameCompleteMessage>(gameCompleteMessage);


            var gameData = (from p in puzzlesViewModel.PuzzleGroupData
                            from game in p.Puzzles
                            where game.PuzzleSubGroupId == 1
                            select game).FirstOrDefault();

            Assert.AreEqual(95, gameData.GameScore);
        }

        private async Task<List<PuzzleGroup>> GenerateUserGameDataFromPuzzleGroupData(string user)
        {
            var puzzleGroupDatas = await GetPuzzleGroupDataFromServiceAsync();
            var puzzleGroupGameDatas = new List<PuzzleGroupGameData>();
            foreach (var puzzleGroupData in puzzleGroupDatas)
            {
                puzzleGroupGameDatas.Add(new PuzzleGroupGameData()
                {
                    Data = puzzleGroupData.Data,
                    PuzzleGroupDataId = puzzleGroupData.PuzzleGroupDataId,
                    GameUserName = user
                });
            }

            return puzzleGroupGameDatas.Select(tabledata => JsonConvert.DeserializeObject<PuzzleGroup>(tabledata.Data)).ToList();
        }

        private async Task<List<PuzzleGroupData>> GetPuzzleGroupDataFromServiceAsync()
        {
            var responseStreamTask = new FakePuzzleWebApiService().GetPuzzleDataFromApi();
            var responseStream = await responseStreamTask;

            List<PuzzleGroup> puzzleGroups = null;
            if (responseStream != null)
            {
                puzzleGroups = CreatePuzzleGroupFromJson(responseStream);
            }

            var index = 1;
            var result = puzzleGroups.Select(puzzleGroupData => CreatePuzzleGroupData(puzzleGroupData, index++)).ToList();
            return result;
        }

        private PuzzleGroupData CreatePuzzleGroupData(PuzzleGroup puzzleGroupData, int i)
        {
            return new PuzzleGroupData()
            {
                PuzzleGroupDataId = i,
                Data = JsonConvert.SerializeObject(puzzleGroupData)
            };
        }

        private List<PuzzleGroup> CreatePuzzleGroupFromJson(Stream responseStream)
        {
            List<PuzzleGroup> resultPuzzleGroup = null;
            using (var reader = new StreamReader(responseStream))
            {
                var res = reader.ReadToEnd();
                var result = JsonConvert.DeserializeObject(res);
                resultPuzzleGroup = JsonConvert.DeserializeObject<List<PuzzleGroup>>(result.ToString());
            }
            return resultPuzzleGroup;
        }

    }

    
    class FakeNavigationService : INavigationService
    {
        public void GoBack()
        {
            throw new NotImplementedException();
        }

        public void GoForward()
        {
            throw new NotImplementedException();
        }

        public bool Navigate<T>(object parameter = null)
        {
            return true;
        }

        public bool Navigate(Type source, object parameter = null)
        {
            return true;
        }
    }
}