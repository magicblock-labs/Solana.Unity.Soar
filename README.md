<p align="center">
    <img src="https://raw.githubusercontent.com/magicblock-labs/Solana.Unity.Soar/main/assets/icon.png" margin="auto" height="100"/>
</p>

<p align="center">
    <a href="https://github.com/magicblock-labs/Solana.Unity.Soar/actions/workflows/dotnet.yml">
        <img src="https://github.com/magicblock-labs/Solana.Unity.Soar/actions/workflows/dotnet.yml/badge.svg"
            alt="GitHub Workflow Build Status (master)" ></a>
    <a href="">
        <img src="https://img.shields.io/github/license/magicblock-labs/Solana.Unity.Soar?style=flat-square"
            alt="Code License"></a>
    <a href="https://twitter.com/intent/follow?screen_name=magicblock">
        <img src="https://img.shields.io/twitter/follow/magicblock?style=flat-square&logo=twitter"
            alt="Follow on Twitter"></a>
    <a href="https://discord.com/invite/MBkdC3gxcv">
       <img alt="Discord" src="https://img.shields.io/discord/849407317761064961?style=flat-square"
            alt="Join the discussion!"></a>
</p>

# What is Solana.Unity.Soar?

Solana.Unity.Soar is the Unity SDK for integrating with the [Soar](https://github.com/magicblock-labs/SOAR) protocol.

Refer to the [documentation](https://docs.magicblock.gg/Open-source%20programs/SOAR) for more information on how to use the SDK.

Most of the code of Solana.Unity.Soar was auto-generated using the Solana.Unity [Anchor tool](https://github.com/magicblock-labs/Solana.Unity.Anchor) from the Soar [IDL](https://solscan.io/account/SoarNNzwQHMwcfdkdLc6kvbkoMSxcHy89gTHrjhJYkk#anchorProgramIDL).

## Features

## Requirements
- net 2.1

## Dependencies
- Solana.Unity.Programs
- Solana.Unity.Rpc
- Solana.Unity.Wallet

## Examples

Create a player profile on Soar

```csharp
var tx = new Transaction()
{
    FeePayer = Web3.Account,
    Instructions = new List<TransactionInstruction>(),
    RecentBlockHash = await Web3.BlockHash()
};

var accountsInitUser = new InitializeUserAccounts()
{
    Payer = Web3.Account,
    User = userPda,
    SystemProgram = SystemProgram.ProgramIdKey
};
var initUserIx = KamikazeJoeProgram.InitializeUser(accounts: accountsInitUser, _kamikazeJoeProgramId);
tx.Add(initUserIx);
await Web3.Wallet.SignAndSendTransaction(tx);
```
The following example refer to the Kamikaze Joe example, specifically the claim win [instruction](https://github.com/magicblock-labs/Kamikaze-Joe/blob/main/programs/kamikazejoe/src/instructions/claim_prize_soar.rs#L43), which submit the score trough CPI:

```csharp

var game = (await KamikazeJoeClient.GetGameAsync(_gameInstanceId, Commitment.Confirmed)).ParsedResult;
var soar = (await KamikazeJoeClient.GetLeaderboardAsync(FindSoarPda())).ParsedResult;

var tx = new Transaction()
{
    FeePayer = Web3.Account,
    Instructions = new List<TransactionInstruction>(),
    RecentBlockHash = await Web3.BlockHash()
};

var playerAccount = SoarPda.PlayerPda(game.GameState.WonValue.Winner);

var claimPrizeAccounts = new ClaimPrizeSoarAccounts()
{
    Payer = Web3.Account,
    User = FindUserPda(game.GameState.WonValue.Winner),
    Receiver = game.GameState.WonValue.Winner,
    Game = _gameInstanceId,
    Vault = FindVaultPda(),
    LeaderboardInfo = FindSoarPda(),
    SoarGame = soar.Game,
    SoarLeaderboard = soar.LeaderboardField,
    SoarPlayerAccount = playerAccount,
    SoarPlayerScores = SoarPda.PlayerScoresPda(playerAccount, soar.LeaderboardField),
    SoarTopEntries = soar.TopEntries,
    SoarProgram = SoarProgram.ProgramIdKey,
    SystemProgram = SystemProgram.ProgramIdKey
};

var claimPrizeIx = KamikazeJoeProgram.ClaimPrizeSoar(accounts: claimPrizeAccounts, _kamikazeJoeProgramId);
tx.Instructions.Add(claimPrizeIx);
await SignAndSendTransaction(tx);
```

## Contribution

We encourage everyone to contribute, submit issues, PRs, discuss. Every kind of help is welcome.

## License

This project is licensed under the MIT License - see the [LICENSE](https://github.com/magicblock-labs/Solana.Unity.Metaplex/blob/master/LICENSE) file for details



