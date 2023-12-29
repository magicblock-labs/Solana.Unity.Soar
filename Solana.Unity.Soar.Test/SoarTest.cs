using Microsoft.VisualStudio.TestTools.UnitTesting;
using Solana.Unity.Programs;
using Solana.Unity.Rpc.Models;
using Solana.Unity.Soar.Program;
using Solana.Unity.Soar.Types;
using Solana.Unity.Wallet;
using System.Collections.Generic;


namespace Solana.Unity.Soar.Test
{
    // TODO: Add tests for SoarProgram expanding this minimal test cases   

    [TestClass]
    public class SoarTest
    {
        
        [TestMethod]
        public void RetrieverPdas()
        {
            var playerAccount = SoarPda.PlayerPda(new Account().PublicKey);
            Assert.IsNotNull(playerAccount);
            
            var playerScoresPda = SoarPda.PlayerScoresPda(new Account().PublicKey, new Account().PublicKey);
            Assert.IsNotNull(playerScoresPda);
            
            var leaderboardTopEntriesPda = SoarPda.LeaderboardTopEntriesPda(new Account().PublicKey, new Account().PublicKey);
            Assert.IsNotNull(leaderboardTopEntriesPda);
            
            var leaderboardPda = SoarPda.LeaderboardPda(new Account().PublicKey);
            Assert.IsNotNull(leaderboardPda);
            
            var achievementPda = SoarPda.AchievementPda(new Account().PublicKey);
            Assert.IsNotNull(achievementPda);
            
            var playerAchievementPda = SoarPda.PlayerAchievementPda(new Account().PublicKey, new Account().PublicKey);
            Assert.IsNotNull(playerAchievementPda);
            
            var nftClaimPda = SoarPda.NftClaimPda(new Account().PublicKey, new Account().PublicKey);
            Assert.IsNotNull(nftClaimPda);
        }
        
        [TestMethod]
        public void InitializePlayerTxBuilder()
        {
            var user = new Account();
            var playerAccount = SoarPda.PlayerPda(new Account().PublicKey);
            
            var tx = new Transaction()
            {
                FeePayer = user,
                Instructions = new List<TransactionInstruction>(),
            };
            
            var accountsInitPlayer = new InitializePlayerAccounts()
            {
                Payer = user,
                User = user,
                PlayerAccount = playerAccount,
                SystemProgram = SystemProgram.ProgramIdKey
            };
            var initPlayerIx = SoarProgram.InitializePlayer(
                accounts: accountsInitPlayer,
                username: "Test User",
                nftMeta: PublicKey.DefaultPublicKey,
                SoarProgram.ProgramIdKey
            );
            tx.Add(initPlayerIx);
            
            Assert.IsNotNull(tx);
            Assert.IsTrue(tx.Instructions.Count ==  1);
        }
        
        [TestMethod]
        public void CreateLeaderboardTxBuilder()
        {
            var user = new Account();
            var tx = new Transaction()
            {
                FeePayer = user,
                Instructions = new List<TransactionInstruction>(),
            };
            var game = new PublicKey("EFf4gsG44gaWUMd6HsEW7pvcRXXGfLxBC5mMeQD4RGDU");


            var leaderboard = SoarPda.LeaderboardPda(game, 0);
            var topEntries = SoarPda.LeaderboardTopEntriesPda(leaderboard);
            var leaderboardMeta = new RegisterLeaderBoardInput()
            {
                Description = "A new leaderboard",
                NftMeta = new PublicKey("8PyfKjB46ih1NHdNQLGhEGRDNRPTnFf94bwnQxa9Veux"),
                ScoresToRetain = 5,
                IsAscending = false, // From highest to lowest
                AllowMultipleScores = false // Only one score per player in the global leaderboard
            };

            var addLeaderboardAccounts = new AddLeaderboardAccounts()
            {
                Authority = user,
                Payer = user,
                Game = game,
                Leaderboard = leaderboard,
                TopEntries = topEntries,
                SystemProgram = SystemProgram.ProgramIdKey
            };
            var createLeaderboardIx = SoarProgram.AddLeaderboard(
                accounts: addLeaderboardAccounts,
                input: leaderboardMeta,
                SoarProgram.ProgramIdKey
            );
            tx.Add(createLeaderboardIx);
            
            Assert.IsNotNull(tx);
            Assert.IsTrue(tx.Instructions.Count ==  1);
        }
        
    }
}