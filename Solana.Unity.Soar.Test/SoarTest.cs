using Microsoft.VisualStudio.TestTools.UnitTesting;
using Solana.Unity.Programs;
using Solana.Unity.Rpc.Models;
using Solana.Unity.Soar.Program;
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
        
    }
}