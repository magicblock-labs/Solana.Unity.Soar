using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Solana.Unity.Programs.Abstract;
using Solana.Unity.Programs.Utilities;
using Solana.Unity.Rpc;
using Solana.Unity.Rpc.Core.Sockets;
using Solana.Unity.Rpc.Types;
using Solana.Unity.Wallet;
using Solana.Unity.Soar.Accounts;
using Solana.Unity.Soar.Errors;
using Solana.Unity.Soar.Program;
using Solana.Unity.Soar.Types;

namespace Solana.Unity.Soar
{
    
    namespace Accounts
    {
        public partial class Achievement
        {
            public static ulong ACCOUNT_DISCRIMINATOR => 4486324231916944670UL;
            public static ReadOnlySpan<byte> ACCOUNT_DISCRIMINATOR_BYTES => new byte[]{30, 253, 162, 142, 30, 160, 66, 62};
            public static string ACCOUNT_DISCRIMINATOR_B58 => "6BeewP5F7Am";
            public PublicKey Game { get; set; }

            public ulong Id { get; set; }

            public string Title { get; set; }

            public string Description { get; set; }

            public PublicKey NftMeta { get; set; }

            public PublicKey Reward { get; set; }

            public static Achievement Deserialize(ReadOnlySpan<byte> _data)
            {
                int offset = 0;
                ulong accountHashValue = _data.GetU64(offset);
                offset += 8;
                if (accountHashValue != ACCOUNT_DISCRIMINATOR)
                {
                    return null;
                }

                Achievement result = new Achievement();
                result.Game = _data.GetPubKey(offset);
                offset += 32;
                result.Id = _data.GetU64(offset);
                offset += 8;
                offset += _data.GetBorshString(offset, out var resultTitle);
                result.Title = resultTitle;
                offset += _data.GetBorshString(offset, out var resultDescription);
                result.Description = resultDescription;
                result.NftMeta = _data.GetPubKey(offset);
                offset += 32;
                if (_data.GetBool(offset++))
                {
                    result.Reward = _data.GetPubKey(offset);
                    offset += 32;
                }

                return result;
            }
        }

        public partial class Game
        {
            public static ulong ACCOUNT_DISCRIMINATOR => 1331205435963103771UL;
            public static ReadOnlySpan<byte> ACCOUNT_DISCRIMINATOR_BYTES => new byte[]{27, 90, 166, 125, 74, 100, 121, 18};
            public static string ACCOUNT_DISCRIMINATOR_B58 => "5aNQXizG8jB";
            public GameAttributes Meta { get; set; }

            public ulong LeaderboardCount { get; set; }

            public ulong AchievementCount { get; set; }

            public PublicKey[] Auth { get; set; }

            public static Game Deserialize(ReadOnlySpan<byte> _data)
            {
                int offset = 0;
                ulong accountHashValue = _data.GetU64(offset);
                offset += 8;
                if (accountHashValue != ACCOUNT_DISCRIMINATOR)
                {
                    return null;
                }

                Game result = new Game();
                offset += GameAttributes.Deserialize(_data, offset, out var resultMeta);
                result.Meta = resultMeta;
                result.LeaderboardCount = _data.GetU64(offset);
                offset += 8;
                result.AchievementCount = _data.GetU64(offset);
                offset += 8;
                int resultAuthLength = (int)_data.GetU32(offset);
                offset += 4;
                result.Auth = new PublicKey[resultAuthLength];
                for (uint resultAuthIdx = 0; resultAuthIdx < resultAuthLength; resultAuthIdx++)
                {
                    result.Auth[resultAuthIdx] = _data.GetPubKey(offset);
                    offset += 32;
                }

                return result;
            }
        }

        public partial class LeaderBoard
        {
            public static ulong ACCOUNT_DISCRIMINATOR => 6399363718156250262UL;
            public static ReadOnlySpan<byte> ACCOUNT_DISCRIMINATOR_BYTES => new byte[]{150, 236, 57, 121, 101, 27, 207, 88};
            public static string ACCOUNT_DISCRIMINATOR_B58 => "SF977UKb3w5";
            public ulong Id { get; set; }

            public PublicKey Game { get; set; }

            public string Description { get; set; }

            public PublicKey NftMeta { get; set; }

            public byte Decimals { get; set; }

            public ulong MinScore { get; set; }

            public ulong MaxScore { get; set; }

            public PublicKey TopEntries { get; set; }

            public bool AllowMultipleScores { get; set; }

            public static LeaderBoard Deserialize(ReadOnlySpan<byte> _data)
            {
                int offset = 0;
                ulong accountHashValue = _data.GetU64(offset);
                offset += 8;
                if (accountHashValue != ACCOUNT_DISCRIMINATOR)
                {
                    return null;
                }

                LeaderBoard result = new LeaderBoard();
                result.Id = _data.GetU64(offset);
                offset += 8;
                result.Game = _data.GetPubKey(offset);
                offset += 32;
                offset += _data.GetBorshString(offset, out var resultDescription);
                result.Description = resultDescription;
                result.NftMeta = _data.GetPubKey(offset);
                offset += 32;
                result.Decimals = _data.GetU8(offset);
                offset += 1;
                result.MinScore = _data.GetU64(offset);
                offset += 8;
                result.MaxScore = _data.GetU64(offset);
                offset += 8;
                if (_data.GetBool(offset++))
                {
                    result.TopEntries = _data.GetPubKey(offset);
                    offset += 32;
                }

                result.AllowMultipleScores = _data.GetBool(offset);
                offset += 1;
                return result;
            }
        }

        public partial class Merged
        {
            public static ulong ACCOUNT_DISCRIMINATOR => 4235283110383971565UL;
            public static ReadOnlySpan<byte> ACCOUNT_DISCRIMINATOR_BYTES => new byte[]{237, 236, 165, 165, 140, 191, 198, 58};
            public static string ACCOUNT_DISCRIMINATOR_B58 => "goAZByG2Bn9";
            public PublicKey Initiator { get; set; }

            public MergeApproval[] Approvals { get; set; }

            public bool MergeComplete { get; set; }

            public static Merged Deserialize(ReadOnlySpan<byte> _data)
            {
                int offset = 0;
                ulong accountHashValue = _data.GetU64(offset);
                offset += 8;
                if (accountHashValue != ACCOUNT_DISCRIMINATOR)
                {
                    return null;
                }

                Merged result = new Merged();
                result.Initiator = _data.GetPubKey(offset);
                offset += 32;
                int resultApprovalsLength = (int)_data.GetU32(offset);
                offset += 4;
                result.Approvals = new MergeApproval[resultApprovalsLength];
                for (uint resultApprovalsIdx = 0; resultApprovalsIdx < resultApprovalsLength; resultApprovalsIdx++)
                {
                    offset += MergeApproval.Deserialize(_data, offset, out var resultApprovalsresultApprovalsIdx);
                    result.Approvals[resultApprovalsIdx] = resultApprovalsresultApprovalsIdx;
                }

                result.MergeComplete = _data.GetBool(offset);
                offset += 1;
                return result;
            }
        }

        public partial class PlayerAchievement
        {
            public static ulong ACCOUNT_DISCRIMINATOR => 13307076509943172530UL;
            public static ReadOnlySpan<byte> ACCOUNT_DISCRIMINATOR_BYTES => new byte[]{178, 141, 27, 246, 148, 59, 172, 184};
            public static string ACCOUNT_DISCRIMINATOR_B58 => "WsAtA3cu8sR";
            public PublicKey PlayerAccount { get; set; }

            public PublicKey Achievement { get; set; }

            public long Timestamp { get; set; }

            public bool Unlocked { get; set; }

            public bool Claimed { get; set; }

            public static PlayerAchievement Deserialize(ReadOnlySpan<byte> _data)
            {
                int offset = 0;
                ulong accountHashValue = _data.GetU64(offset);
                offset += 8;
                if (accountHashValue != ACCOUNT_DISCRIMINATOR)
                {
                    return null;
                }

                PlayerAchievement result = new PlayerAchievement();
                result.PlayerAccount = _data.GetPubKey(offset);
                offset += 32;
                result.Achievement = _data.GetPubKey(offset);
                offset += 32;
                result.Timestamp = _data.GetS64(offset);
                offset += 8;
                result.Unlocked = _data.GetBool(offset);
                offset += 1;
                result.Claimed = _data.GetBool(offset);
                offset += 1;
                return result;
            }
        }

        public partial class PlayerScoresList
        {
            public static ulong ACCOUNT_DISCRIMINATOR => 16156031517957510185UL;
            public static ReadOnlySpan<byte> ACCOUNT_DISCRIMINATOR_BYTES => new byte[]{41, 84, 223, 55, 17, 193, 53, 224};
            public static string ACCOUNT_DISCRIMINATOR_B58 => "7uy86AySwGT";
            public PublicKey PlayerAccount { get; set; }

            public PublicKey Leaderboard { get; set; }

            public ushort AllocCount { get; set; }

            public ScoreEntry[] Scores { get; set; }

            public static PlayerScoresList Deserialize(ReadOnlySpan<byte> _data)
            {
                int offset = 0;
                ulong accountHashValue = _data.GetU64(offset);
                offset += 8;
                if (accountHashValue != ACCOUNT_DISCRIMINATOR)
                {
                    return null;
                }

                PlayerScoresList result = new PlayerScoresList();
                result.PlayerAccount = _data.GetPubKey(offset);
                offset += 32;
                result.Leaderboard = _data.GetPubKey(offset);
                offset += 32;
                result.AllocCount = _data.GetU16(offset);
                offset += 2;
                int resultScoresLength = (int)_data.GetU32(offset);
                offset += 4;
                result.Scores = new ScoreEntry[resultScoresLength];
                for (uint resultScoresIdx = 0; resultScoresIdx < resultScoresLength; resultScoresIdx++)
                {
                    offset += ScoreEntry.Deserialize(_data, offset, out var resultScoresresultScoresIdx);
                    result.Scores[resultScoresIdx] = resultScoresresultScoresIdx;
                }

                return result;
            }
        }

        public partial class Player
        {
            public static ulong ACCOUNT_DISCRIMINATOR => 15766710478567431885UL;
            public static ReadOnlySpan<byte> ACCOUNT_DISCRIMINATOR_BYTES => new byte[]{205, 222, 112, 7, 165, 155, 206, 218};
            public static string ACCOUNT_DISCRIMINATOR_B58 => "bSBoKNsSHuj";
            public PublicKey User { get; set; }

            public string Username { get; set; }

            public PublicKey NftMeta { get; set; }

            public static Player Deserialize(ReadOnlySpan<byte> _data)
            {
                int offset = 0;
                ulong accountHashValue = _data.GetU64(offset);
                offset += 8;
                if (accountHashValue != ACCOUNT_DISCRIMINATOR)
                {
                    return null;
                }

                Player result = new Player();
                result.User = _data.GetPubKey(offset);
                offset += 32;
                offset += _data.GetBorshString(offset, out var resultUsername);
                result.Username = resultUsername;
                result.NftMeta = _data.GetPubKey(offset);
                offset += 32;
                return result;
            }
        }

        public partial class NftClaim
        {
            public static ulong ACCOUNT_DISCRIMINATOR => 17538691319540471809UL;
            public static ReadOnlySpan<byte> ACCOUNT_DISCRIMINATOR_BYTES => new byte[]{1, 216, 111, 198, 212, 242, 101, 243};
            public static string ACCOUNT_DISCRIMINATOR_B58 => "JuPUApVJJ6";
            public static NftClaim Deserialize(ReadOnlySpan<byte> _data)
            {
                int offset = 0;
                ulong accountHashValue = _data.GetU64(offset);
                offset += 8;
                if (accountHashValue != ACCOUNT_DISCRIMINATOR)
                {
                    return null;
                }

                NftClaim result = new NftClaim();
                return result;
            }
        }

        public partial class Reward
        {
            public static ulong ACCOUNT_DISCRIMINATOR => 2462645182054171054UL;
            public static ReadOnlySpan<byte> ACCOUNT_DISCRIMINATOR_BYTES => new byte[]{174, 129, 42, 212, 190, 18, 45, 34};
            public static string ACCOUNT_DISCRIMINATOR_B58 => "WBuwjnn8QQh";
            public PublicKey Achievement { get; set; }

            public ulong AvailableSpots { get; set; }

            public RewardKind RewardField { get; set; }

            public static Reward Deserialize(ReadOnlySpan<byte> _data)
            {
                int offset = 0;
                ulong accountHashValue = _data.GetU64(offset);
                offset += 8;
                if (accountHashValue != ACCOUNT_DISCRIMINATOR)
                {
                    return null;
                }

                Reward result = new Reward();
                result.Achievement = _data.GetPubKey(offset);
                offset += 32;
                result.AvailableSpots = _data.GetU64(offset);
                offset += 8;
                offset += RewardKind.Deserialize(_data, offset, out var resultRewardField);
                result.RewardField = resultRewardField;
                return result;
            }
        }

        public partial class LeaderTopEntries
        {
            public static ulong ACCOUNT_DISCRIMINATOR => 30237451010564185UL;
            public static ReadOnlySpan<byte> ACCOUNT_DISCRIMINATOR_BYTES => new byte[]{89, 224, 101, 46, 205, 108, 107, 0};
            public static string ACCOUNT_DISCRIMINATOR_B58 => "G2v5GC9n8ym";
            public bool IsAscending { get; set; }

            public LeaderBoardScore[] TopScores { get; set; }

            public static LeaderTopEntries Deserialize(ReadOnlySpan<byte> _data)
            {
                int offset = 0;
                ulong accountHashValue = _data.GetU64(offset);
                offset += 8;
                if (accountHashValue != ACCOUNT_DISCRIMINATOR)
                {
                    return null;
                }

                LeaderTopEntries result = new LeaderTopEntries();
                result.IsAscending = _data.GetBool(offset);
                offset += 1;
                int resultTopScoresLength = (int)_data.GetU32(offset);
                offset += 4;
                result.TopScores = new LeaderBoardScore[resultTopScoresLength];
                for (uint resultTopScoresIdx = 0; resultTopScoresIdx < resultTopScoresLength; resultTopScoresIdx++)
                {
                    offset += LeaderBoardScore.Deserialize(_data, offset, out var resultTopScoresresultTopScoresIdx);
                    result.TopScores[resultTopScoresIdx] = resultTopScoresresultTopScoresIdx;
                }

                return result;
            }
        }
    }

    namespace Errors
    {
        public enum SoarErrorKind : uint
        {
            InvalidFieldLength = 6000U,
            InvalidAuthority = 6001U,
            MissingSignature = 6002U,
            NoRewardForAchievement = 6003U,
            AccountNotPartOfMerge = 6004U,
            ScoreNotWithinBounds = 6005U,
            MissingExpectedAccount = 6006U,
            InvalidRewardKind = 6007U,
            NoAvailableRewards = 6008U
        }
    }

    namespace Types
    {
        public partial class GameAttributes
        {
            public string Title { get; set; }

            public string Description { get; set; }

            public byte Genre { get; set; }

            public byte GameType { get; set; }

            public PublicKey NftMeta { get; set; }

            public int Serialize(byte[] _data, int initialOffset)
            {
                int offset = initialOffset;
                offset += _data.WriteBorshString(Title, offset);
                offset += _data.WriteBorshString(Description, offset);
                _data.WriteU8(Genre, offset);
                offset += 1;
                _data.WriteU8(GameType, offset);
                offset += 1;
                _data.WritePubKey(NftMeta, offset);
                offset += 32;
                return offset - initialOffset;
            }

            public static int Deserialize(ReadOnlySpan<byte> _data, int initialOffset, out GameAttributes result)
            {
                int offset = initialOffset;
                result = new GameAttributes();
                offset += _data.GetBorshString(offset, out var resultTitle);
                result.Title = resultTitle;
                offset += _data.GetBorshString(offset, out var resultDescription);
                result.Description = resultDescription;
                result.Genre = _data.GetU8(offset);
                offset += 1;
                result.GameType = _data.GetU8(offset);
                offset += 1;
                result.NftMeta = _data.GetPubKey(offset);
                offset += 32;
                return offset - initialOffset;
            }
        }

        public partial class ScoreEntry
        {
            public ulong Score { get; set; }

            public long Timestamp { get; set; }

            public int Serialize(byte[] _data, int initialOffset)
            {
                int offset = initialOffset;
                _data.WriteU64(Score, offset);
                offset += 8;
                _data.WriteS64(Timestamp, offset);
                offset += 8;
                return offset - initialOffset;
            }

            public static int Deserialize(ReadOnlySpan<byte> _data, int initialOffset, out ScoreEntry result)
            {
                int offset = initialOffset;
                result = new ScoreEntry();
                result.Score = _data.GetU64(offset);
                offset += 8;
                result.Timestamp = _data.GetS64(offset);
                offset += 8;
                return offset - initialOffset;
            }
        }

        public partial class MergeApproval
        {
            public PublicKey Key { get; set; }

            public bool Approved { get; set; }

            public int Serialize(byte[] _data, int initialOffset)
            {
                int offset = initialOffset;
                _data.WritePubKey(Key, offset);
                offset += 32;
                _data.WriteBool(Approved, offset);
                offset += 1;
                return offset - initialOffset;
            }

            public static int Deserialize(ReadOnlySpan<byte> _data, int initialOffset, out MergeApproval result)
            {
                int offset = initialOffset;
                result = new MergeApproval();
                result.Key = _data.GetPubKey(offset);
                offset += 32;
                result.Approved = _data.GetBool(offset);
                offset += 1;
                return offset - initialOffset;
            }
        }

        public partial class LeaderBoardScore
        {
            public PublicKey Player { get; set; }

            public ScoreEntry Entry { get; set; }

            public int Serialize(byte[] _data, int initialOffset)
            {
                int offset = initialOffset;
                _data.WritePubKey(Player, offset);
                offset += 32;
                offset += Entry.Serialize(_data, offset);
                return offset - initialOffset;
            }

            public static int Deserialize(ReadOnlySpan<byte> _data, int initialOffset, out LeaderBoardScore result)
            {
                int offset = initialOffset;
                result = new LeaderBoardScore();
                result.Player = _data.GetPubKey(offset);
                offset += 32;
                offset += ScoreEntry.Deserialize(_data, offset, out var resultEntry);
                result.Entry = resultEntry;
                return offset - initialOffset;
            }
        }

        public partial class RegisterLeaderBoardInput
        {
            public string Description { get; set; }

            public PublicKey NftMeta { get; set; }

            public byte? Decimals { get; set; }

            public ulong? MinScore { get; set; }

            public ulong? MaxScore { get; set; }

            public byte ScoresToRetain { get; set; }

            public bool IsAscending { get; set; }

            public bool AllowMultipleScores { get; set; }

            public int Serialize(byte[] _data, int initialOffset)
            {
                int offset = initialOffset;
                offset += _data.WriteBorshString(Description, offset);
                _data.WritePubKey(NftMeta, offset);
                offset += 32;
                if (Decimals != null)
                {
                    _data.WriteU8(1, offset);
                    offset += 1;
                    _data.WriteU8(Decimals.Value, offset);
                    offset += 1;
                }
                else
                {
                    _data.WriteU8(0, offset);
                    offset += 1;
                }

                if (MinScore != null)
                {
                    _data.WriteU8(1, offset);
                    offset += 1;
                    _data.WriteU64(MinScore.Value, offset);
                    offset += 8;
                }
                else
                {
                    _data.WriteU8(0, offset);
                    offset += 1;
                }

                if (MaxScore != null)
                {
                    _data.WriteU8(1, offset);
                    offset += 1;
                    _data.WriteU64(MaxScore.Value, offset);
                    offset += 8;
                }
                else
                {
                    _data.WriteU8(0, offset);
                    offset += 1;
                }

                _data.WriteU8(ScoresToRetain, offset);
                offset += 1;
                _data.WriteBool(IsAscending, offset);
                offset += 1;
                _data.WriteBool(AllowMultipleScores, offset);
                offset += 1;
                return offset - initialOffset;
            }

            public static int Deserialize(ReadOnlySpan<byte> _data, int initialOffset, out RegisterLeaderBoardInput result)
            {
                int offset = initialOffset;
                result = new RegisterLeaderBoardInput();
                offset += _data.GetBorshString(offset, out var resultDescription);
                result.Description = resultDescription;
                result.NftMeta = _data.GetPubKey(offset);
                offset += 32;
                if (_data.GetBool(offset++))
                {
                    result.Decimals = _data.GetU8(offset);
                    offset += 1;
                }

                if (_data.GetBool(offset++))
                {
                    result.MinScore = _data.GetU64(offset);
                    offset += 8;
                }

                if (_data.GetBool(offset++))
                {
                    result.MaxScore = _data.GetU64(offset);
                    offset += 8;
                }

                result.ScoresToRetain = _data.GetU8(offset);
                offset += 1;
                result.IsAscending = _data.GetBool(offset);
                offset += 1;
                result.AllowMultipleScores = _data.GetBool(offset);
                offset += 1;
                return offset - initialOffset;
            }
        }

        public partial class AddNewRewardInput
        {
            public ulong AvailableSpots { get; set; }

            public RewardKindInput Kind { get; set; }

            public int Serialize(byte[] _data, int initialOffset)
            {
                int offset = initialOffset;
                _data.WriteU64(AvailableSpots, offset);
                offset += 8;
                offset += Kind.Serialize(_data, offset);
                return offset - initialOffset;
            }

            public static int Deserialize(ReadOnlySpan<byte> _data, int initialOffset, out AddNewRewardInput result)
            {
                int offset = initialOffset;
                result = new AddNewRewardInput();
                result.AvailableSpots = _data.GetU64(offset);
                offset += 8;
                offset += RewardKindInput.Deserialize(_data, offset, out var resultKind);
                result.Kind = resultKind;
                return offset - initialOffset;
            }
        }

        public enum RewardKindType : byte
        {
            FungibleToken,
            NonFungibleToken
        }

        public partial class FungibleTokenType
        {
            public PublicKey Mint { get; set; }

            public PublicKey Account { get; set; }

            public ulong Amount { get; set; }

            public static int Deserialize(ReadOnlySpan<byte> _data, int initialOffset, out FungibleTokenType result)
            {
                int offset = initialOffset;
                result = new FungibleTokenType();
                result.Mint = _data.GetPubKey(offset);
                offset += 32;
                result.Account = _data.GetPubKey(offset);
                offset += 32;
                result.Amount = _data.GetU64(offset);
                offset += 8;
                return offset - initialOffset;
            }

            public int Serialize(byte[] _data, int initialOffset)
            {
                int offset = initialOffset;
                _data.WritePubKey(Mint, offset);
                offset += 32;
                _data.WritePubKey(Account, offset);
                offset += 32;
                _data.WriteU64(Amount, offset);
                offset += 8;
                return offset - initialOffset;
            }
        }

        public partial class NonFungibleTokenType
        {
            public string Uri { get; set; }

            public string Name { get; set; }

            public string Symbol { get; set; }

            public ulong Minted { get; set; }

            public PublicKey Collection { get; set; }

            public static int Deserialize(ReadOnlySpan<byte> _data, int initialOffset, out NonFungibleTokenType result)
            {
                int offset = initialOffset;
                result = new NonFungibleTokenType();
                offset += _data.GetBorshString(offset, out var resultUri);
                result.Uri = resultUri;
                offset += _data.GetBorshString(offset, out var resultName);
                result.Name = resultName;
                offset += _data.GetBorshString(offset, out var resultSymbol);
                result.Symbol = resultSymbol;
                result.Minted = _data.GetU64(offset);
                offset += 8;
                if (_data.GetBool(offset++))
                {
                    result.Collection = _data.GetPubKey(offset);
                    offset += 32;
                }

                return offset - initialOffset;
            }

            public int Serialize(byte[] _data, int initialOffset)
            {
                int offset = initialOffset;
                offset += _data.WriteBorshString(Uri, offset);
                offset += _data.WriteBorshString(Name, offset);
                offset += _data.WriteBorshString(Symbol, offset);
                _data.WriteU64(Minted, offset);
                offset += 8;
                if (Collection != null)
                {
                    _data.WriteU8(1, offset);
                    offset += 1;
                    _data.WritePubKey(Collection, offset);
                    offset += 32;
                }
                else
                {
                    _data.WriteU8(0, offset);
                    offset += 1;
                }

                return offset - initialOffset;
            }
        }

        public partial class RewardKind
        {
            public FungibleTokenType FungibleTokenValue { get; set; }

            public NonFungibleTokenType NonFungibleTokenValue { get; set; }

            public int Serialize(byte[] _data, int initialOffset)
            {
                int offset = initialOffset;
                _data.WriteU8((byte)Type, offset);
                offset += 1;
                switch (Type)
                {
                    case RewardKindType.FungibleToken:
                        offset += FungibleTokenValue.Serialize(_data, offset);
                        break;
                    case RewardKindType.NonFungibleToken:
                        offset += NonFungibleTokenValue.Serialize(_data, offset);
                        break;
                }

                return offset - initialOffset;
            }

            public RewardKindType Type { get; set; }

            public static int Deserialize(ReadOnlySpan<byte> _data, int initialOffset, out RewardKind result)
            {
                int offset = initialOffset;
                result = new RewardKind();
                result.Type = (RewardKindType)_data.GetU8(offset);
                offset += 1;
                switch (result.Type)
                {
                    case RewardKindType.FungibleToken:
                    {
                        FungibleTokenType tmpFungibleTokenValue = new FungibleTokenType();
                        offset += FungibleTokenType.Deserialize(_data, offset, out tmpFungibleTokenValue);
                        result.FungibleTokenValue = tmpFungibleTokenValue;
                        break;
                    }

                    case RewardKindType.NonFungibleToken:
                    {
                        NonFungibleTokenType tmpNonFungibleTokenValue = new NonFungibleTokenType();
                        offset += NonFungibleTokenType.Deserialize(_data, offset, out tmpNonFungibleTokenValue);
                        result.NonFungibleTokenValue = tmpNonFungibleTokenValue;
                        break;
                    }
                }

                return offset - initialOffset;
            }
        }

        public enum RewardKindInputType : byte
        {
            Ft,
            Nft
        }

        public partial class FtType
        {
            public ulong Deposit { get; set; }

            public ulong Amount { get; set; }

            public static int Deserialize(ReadOnlySpan<byte> _data, int initialOffset, out FtType result)
            {
                int offset = initialOffset;
                result = new FtType();
                result.Deposit = _data.GetU64(offset);
                offset += 8;
                result.Amount = _data.GetU64(offset);
                offset += 8;
                return offset - initialOffset;
            }

            public int Serialize(byte[] _data, int initialOffset)
            {
                int offset = initialOffset;
                _data.WriteU64(Deposit, offset);
                offset += 8;
                _data.WriteU64(Amount, offset);
                offset += 8;
                return offset - initialOffset;
            }
        }

        public partial class NftType
        {
            public string Uri { get; set; }

            public string Name { get; set; }

            public string Symbol { get; set; }

            public static int Deserialize(ReadOnlySpan<byte> _data, int initialOffset, out NftType result)
            {
                int offset = initialOffset;
                result = new NftType();
                offset += _data.GetBorshString(offset, out var resultUri);
                result.Uri = resultUri;
                offset += _data.GetBorshString(offset, out var resultName);
                result.Name = resultName;
                offset += _data.GetBorshString(offset, out var resultSymbol);
                result.Symbol = resultSymbol;
                return offset - initialOffset;
            }

            public int Serialize(byte[] _data, int initialOffset)
            {
                int offset = initialOffset;
                offset += _data.WriteBorshString(Uri, offset);
                offset += _data.WriteBorshString(Name, offset);
                offset += _data.WriteBorshString(Symbol, offset);
                return offset - initialOffset;
            }
        }

        public partial class RewardKindInput
        {
            public FtType FtValue { get; set; }

            public NftType NftValue { get; set; }

            public int Serialize(byte[] _data, int initialOffset)
            {
                int offset = initialOffset;
                _data.WriteU8((byte)Type, offset);
                offset += 1;
                switch (Type)
                {
                    case RewardKindInputType.Ft:
                        offset += FtValue.Serialize(_data, offset);
                        break;
                    case RewardKindInputType.Nft:
                        offset += NftValue.Serialize(_data, offset);
                        break;
                }

                return offset - initialOffset;
            }

            public RewardKindInputType Type { get; set; }

            public static int Deserialize(ReadOnlySpan<byte> _data, int initialOffset, out RewardKindInput result)
            {
                int offset = initialOffset;
                result = new RewardKindInput();
                result.Type = (RewardKindInputType)_data.GetU8(offset);
                offset += 1;
                switch (result.Type)
                {
                    case RewardKindInputType.Ft:
                    {
                        FtType tmpFtValue = new FtType();
                        offset += FtType.Deserialize(_data, offset, out tmpFtValue);
                        result.FtValue = tmpFtValue;
                        break;
                    }

                    case RewardKindInputType.Nft:
                    {
                        NftType tmpNftValue = new NftType();
                        offset += NftType.Deserialize(_data, offset, out tmpNftValue);
                        result.NftValue = tmpNftValue;
                        break;
                    }
                }

                return offset - initialOffset;
            }
        }
    }

    public partial class SoarClient : TransactionalBaseClient<SoarErrorKind>
    {
        public SoarClient(IRpcClient rpcClient, IStreamingRpcClient streamingRpcClient) : base(rpcClient, streamingRpcClient, SoarProgram.ProgramIdKey)
        {
        }
        
        public SoarClient(IRpcClient rpcClient, IStreamingRpcClient streamingRpcClient, PublicKey programId) : base(rpcClient, streamingRpcClient, programId)
        {
        }

        public async Task<Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<Achievement>>> GetAchievementsAsync(string programAddress, Commitment commitment = Commitment.Finalized)
        {
            var list = new List<Solana.Unity.Rpc.Models.MemCmp>{new Solana.Unity.Rpc.Models.MemCmp{Bytes = Achievement.ACCOUNT_DISCRIMINATOR_B58, Offset = 0}};
            var res = await RpcClient.GetProgramAccountsAsync(programAddress, commitment, memCmpList: list);
            if (!res.WasSuccessful || !(res.Result?.Count > 0))
                return new Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<Achievement>>(res);
            List<Achievement> resultingAccounts = new List<Achievement>(res.Result.Count);
            resultingAccounts.AddRange(res.Result.Select(result => Achievement.Deserialize(Convert.FromBase64String(result.Account.Data[0]))));
            return new Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<Achievement>>(res, resultingAccounts);
        }

        public async Task<Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<Game>>> GetGamesAsync(string programAddress, Commitment commitment = Commitment.Finalized)
        {
            var list = new List<Solana.Unity.Rpc.Models.MemCmp>{new Solana.Unity.Rpc.Models.MemCmp{Bytes = Game.ACCOUNT_DISCRIMINATOR_B58, Offset = 0}};
            var res = await RpcClient.GetProgramAccountsAsync(programAddress, commitment, memCmpList: list);
            if (!res.WasSuccessful || !(res.Result?.Count > 0))
                return new Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<Game>>(res);
            List<Game> resultingAccounts = new List<Game>(res.Result.Count);
            resultingAccounts.AddRange(res.Result.Select(result => Game.Deserialize(Convert.FromBase64String(result.Account.Data[0]))));
            return new Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<Game>>(res, resultingAccounts);
        }

        public async Task<Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<LeaderBoard>>> GetLeaderBoardsAsync(string programAddress, Commitment commitment = Commitment.Finalized)
        {
            var list = new List<Solana.Unity.Rpc.Models.MemCmp>{new Solana.Unity.Rpc.Models.MemCmp{Bytes = LeaderBoard.ACCOUNT_DISCRIMINATOR_B58, Offset = 0}};
            var res = await RpcClient.GetProgramAccountsAsync(programAddress, commitment, memCmpList: list);
            if (!res.WasSuccessful || !(res.Result?.Count > 0))
                return new Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<LeaderBoard>>(res);
            List<LeaderBoard> resultingAccounts = new List<LeaderBoard>(res.Result.Count);
            resultingAccounts.AddRange(res.Result.Select(result => LeaderBoard.Deserialize(Convert.FromBase64String(result.Account.Data[0]))));
            return new Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<LeaderBoard>>(res, resultingAccounts);
        }

        public async Task<Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<Merged>>> GetMergedsAsync(string programAddress, Commitment commitment = Commitment.Finalized)
        {
            var list = new List<Solana.Unity.Rpc.Models.MemCmp>{new Solana.Unity.Rpc.Models.MemCmp{Bytes = Merged.ACCOUNT_DISCRIMINATOR_B58, Offset = 0}};
            var res = await RpcClient.GetProgramAccountsAsync(programAddress, commitment, memCmpList: list);
            if (!res.WasSuccessful || !(res.Result?.Count > 0))
                return new Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<Merged>>(res);
            List<Merged> resultingAccounts = new List<Merged>(res.Result.Count);
            resultingAccounts.AddRange(res.Result.Select(result => Merged.Deserialize(Convert.FromBase64String(result.Account.Data[0]))));
            return new Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<Merged>>(res, resultingAccounts);
        }

        public async Task<Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<PlayerAchievement>>> GetPlayerAchievementsAsync(string programAddress, Commitment commitment = Commitment.Finalized)
        {
            var list = new List<Solana.Unity.Rpc.Models.MemCmp>{new Solana.Unity.Rpc.Models.MemCmp{Bytes = PlayerAchievement.ACCOUNT_DISCRIMINATOR_B58, Offset = 0}};
            var res = await RpcClient.GetProgramAccountsAsync(programAddress, commitment, memCmpList: list);
            if (!res.WasSuccessful || !(res.Result?.Count > 0))
                return new Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<PlayerAchievement>>(res);
            List<PlayerAchievement> resultingAccounts = new List<PlayerAchievement>(res.Result.Count);
            resultingAccounts.AddRange(res.Result.Select(result => PlayerAchievement.Deserialize(Convert.FromBase64String(result.Account.Data[0]))));
            return new Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<PlayerAchievement>>(res, resultingAccounts);
        }

        public async Task<Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<PlayerScoresList>>> GetPlayerScoresListsAsync(string programAddress, Commitment commitment = Commitment.Finalized)
        {
            var list = new List<Solana.Unity.Rpc.Models.MemCmp>{new Solana.Unity.Rpc.Models.MemCmp{Bytes = PlayerScoresList.ACCOUNT_DISCRIMINATOR_B58, Offset = 0}};
            var res = await RpcClient.GetProgramAccountsAsync(programAddress, commitment, memCmpList: list);
            if (!res.WasSuccessful || !(res.Result?.Count > 0))
                return new Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<PlayerScoresList>>(res);
            List<PlayerScoresList> resultingAccounts = new List<PlayerScoresList>(res.Result.Count);
            resultingAccounts.AddRange(res.Result.Select(result => PlayerScoresList.Deserialize(Convert.FromBase64String(result.Account.Data[0]))));
            return new Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<PlayerScoresList>>(res, resultingAccounts);
        }

        public async Task<Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<Player>>> GetPlayersAsync(string programAddress, Commitment commitment = Commitment.Finalized)
        {
            var list = new List<Solana.Unity.Rpc.Models.MemCmp>{new Solana.Unity.Rpc.Models.MemCmp{Bytes = Player.ACCOUNT_DISCRIMINATOR_B58, Offset = 0}};
            var res = await RpcClient.GetProgramAccountsAsync(programAddress, commitment, memCmpList: list);
            if (!res.WasSuccessful || !(res.Result?.Count > 0))
                return new Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<Player>>(res);
            List<Player> resultingAccounts = new List<Player>(res.Result.Count);
            resultingAccounts.AddRange(res.Result.Select(result => Player.Deserialize(Convert.FromBase64String(result.Account.Data[0]))));
            return new Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<Player>>(res, resultingAccounts);
        }

        public async Task<Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<NftClaim>>> GetNftClaimsAsync(string programAddress, Commitment commitment = Commitment.Finalized)
        {
            var list = new List<Solana.Unity.Rpc.Models.MemCmp>{new Solana.Unity.Rpc.Models.MemCmp{Bytes = NftClaim.ACCOUNT_DISCRIMINATOR_B58, Offset = 0}};
            var res = await RpcClient.GetProgramAccountsAsync(programAddress, commitment, memCmpList: list);
            if (!res.WasSuccessful || !(res.Result?.Count > 0))
                return new Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<NftClaim>>(res);
            List<NftClaim> resultingAccounts = new List<NftClaim>(res.Result.Count);
            resultingAccounts.AddRange(res.Result.Select(result => NftClaim.Deserialize(Convert.FromBase64String(result.Account.Data[0]))));
            return new Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<NftClaim>>(res, resultingAccounts);
        }

        public async Task<Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<Reward>>> GetRewardsAsync(string programAddress, Commitment commitment = Commitment.Finalized)
        {
            var list = new List<Solana.Unity.Rpc.Models.MemCmp>{new Solana.Unity.Rpc.Models.MemCmp{Bytes = Reward.ACCOUNT_DISCRIMINATOR_B58, Offset = 0}};
            var res = await RpcClient.GetProgramAccountsAsync(programAddress, commitment, memCmpList: list);
            if (!res.WasSuccessful || !(res.Result?.Count > 0))
                return new Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<Reward>>(res);
            List<Reward> resultingAccounts = new List<Reward>(res.Result.Count);
            resultingAccounts.AddRange(res.Result.Select(result => Reward.Deserialize(Convert.FromBase64String(result.Account.Data[0]))));
            return new Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<Reward>>(res, resultingAccounts);
        }

        public async Task<Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<LeaderTopEntries>>> GetLeaderTopEntriessAsync(string programAddress, Commitment commitment = Commitment.Finalized)
        {
            var list = new List<Solana.Unity.Rpc.Models.MemCmp>{new Solana.Unity.Rpc.Models.MemCmp{Bytes = LeaderTopEntries.ACCOUNT_DISCRIMINATOR_B58, Offset = 0}};
            var res = await RpcClient.GetProgramAccountsAsync(programAddress, commitment, memCmpList: list);
            if (!res.WasSuccessful || !(res.Result?.Count > 0))
                return new Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<LeaderTopEntries>>(res);
            List<LeaderTopEntries> resultingAccounts = new List<LeaderTopEntries>(res.Result.Count);
            resultingAccounts.AddRange(res.Result.Select(result => LeaderTopEntries.Deserialize(Convert.FromBase64String(result.Account.Data[0]))));
            return new Solana.Unity.Programs.Models.ProgramAccountsResultWrapper<List<LeaderTopEntries>>(res, resultingAccounts);
        }

        public async Task<Solana.Unity.Programs.Models.AccountResultWrapper<Achievement>> GetAchievementAsync(string accountAddress, Commitment commitment = Commitment.Finalized)
        {
            var res = await RpcClient.GetAccountInfoAsync(accountAddress, commitment);
            if (!res.WasSuccessful)
                return new Solana.Unity.Programs.Models.AccountResultWrapper<Achievement>(res);
            var resultingAccount = Achievement.Deserialize(Convert.FromBase64String(res.Result.Value.Data[0]));
            return new Solana.Unity.Programs.Models.AccountResultWrapper<Achievement>(res, resultingAccount);
        }

        public async Task<Solana.Unity.Programs.Models.AccountResultWrapper<Game>> GetGameAsync(string accountAddress, Commitment commitment = Commitment.Finalized)
        {
            var res = await RpcClient.GetAccountInfoAsync(accountAddress, commitment);
            if (!res.WasSuccessful)
                return new Solana.Unity.Programs.Models.AccountResultWrapper<Game>(res);
            var resultingAccount = Game.Deserialize(Convert.FromBase64String(res.Result.Value.Data[0]));
            return new Solana.Unity.Programs.Models.AccountResultWrapper<Game>(res, resultingAccount);
        }

        public async Task<Solana.Unity.Programs.Models.AccountResultWrapper<LeaderBoard>> GetLeaderBoardAsync(string accountAddress, Commitment commitment = Commitment.Finalized)
        {
            var res = await RpcClient.GetAccountInfoAsync(accountAddress, commitment);
            if (!res.WasSuccessful)
                return new Solana.Unity.Programs.Models.AccountResultWrapper<LeaderBoard>(res);
            var resultingAccount = LeaderBoard.Deserialize(Convert.FromBase64String(res.Result.Value.Data[0]));
            return new Solana.Unity.Programs.Models.AccountResultWrapper<LeaderBoard>(res, resultingAccount);
        }

        public async Task<Solana.Unity.Programs.Models.AccountResultWrapper<Merged>> GetMergedAsync(string accountAddress, Commitment commitment = Commitment.Finalized)
        {
            var res = await RpcClient.GetAccountInfoAsync(accountAddress, commitment);
            if (!res.WasSuccessful)
                return new Solana.Unity.Programs.Models.AccountResultWrapper<Merged>(res);
            var resultingAccount = Merged.Deserialize(Convert.FromBase64String(res.Result.Value.Data[0]));
            return new Solana.Unity.Programs.Models.AccountResultWrapper<Merged>(res, resultingAccount);
        }

        public async Task<Solana.Unity.Programs.Models.AccountResultWrapper<PlayerAchievement>> GetPlayerAchievementAsync(string accountAddress, Commitment commitment = Commitment.Finalized)
        {
            var res = await RpcClient.GetAccountInfoAsync(accountAddress, commitment);
            if (!res.WasSuccessful)
                return new Solana.Unity.Programs.Models.AccountResultWrapper<PlayerAchievement>(res);
            var resultingAccount = PlayerAchievement.Deserialize(Convert.FromBase64String(res.Result.Value.Data[0]));
            return new Solana.Unity.Programs.Models.AccountResultWrapper<PlayerAchievement>(res, resultingAccount);
        }

        public async Task<Solana.Unity.Programs.Models.AccountResultWrapper<PlayerScoresList>> GetPlayerScoresListAsync(string accountAddress, Commitment commitment = Commitment.Finalized)
        {
            var res = await RpcClient.GetAccountInfoAsync(accountAddress, commitment);
            if (!res.WasSuccessful)
                return new Solana.Unity.Programs.Models.AccountResultWrapper<PlayerScoresList>(res);
            var resultingAccount = PlayerScoresList.Deserialize(Convert.FromBase64String(res.Result.Value.Data[0]));
            return new Solana.Unity.Programs.Models.AccountResultWrapper<PlayerScoresList>(res, resultingAccount);
        }

        public async Task<Solana.Unity.Programs.Models.AccountResultWrapper<Player>> GetPlayerAsync(string accountAddress, Commitment commitment = Commitment.Finalized)
        {
            var res = await RpcClient.GetAccountInfoAsync(accountAddress, commitment);
            if (!res.WasSuccessful)
                return new Solana.Unity.Programs.Models.AccountResultWrapper<Player>(res);
            var resultingAccount = Player.Deserialize(Convert.FromBase64String(res.Result.Value.Data[0]));
            return new Solana.Unity.Programs.Models.AccountResultWrapper<Player>(res, resultingAccount);
        }

        public async Task<Solana.Unity.Programs.Models.AccountResultWrapper<NftClaim>> GetNftClaimAsync(string accountAddress, Commitment commitment = Commitment.Finalized)
        {
            var res = await RpcClient.GetAccountInfoAsync(accountAddress, commitment);
            if (!res.WasSuccessful)
                return new Solana.Unity.Programs.Models.AccountResultWrapper<NftClaim>(res);
            var resultingAccount = NftClaim.Deserialize(Convert.FromBase64String(res.Result.Value.Data[0]));
            return new Solana.Unity.Programs.Models.AccountResultWrapper<NftClaim>(res, resultingAccount);
        }

        public async Task<Solana.Unity.Programs.Models.AccountResultWrapper<Reward>> GetRewardAsync(string accountAddress, Commitment commitment = Commitment.Finalized)
        {
            var res = await RpcClient.GetAccountInfoAsync(accountAddress, commitment);
            if (!res.WasSuccessful)
                return new Solana.Unity.Programs.Models.AccountResultWrapper<Reward>(res);
            var resultingAccount = Reward.Deserialize(Convert.FromBase64String(res.Result.Value.Data[0]));
            return new Solana.Unity.Programs.Models.AccountResultWrapper<Reward>(res, resultingAccount);
        }

        public async Task<Solana.Unity.Programs.Models.AccountResultWrapper<LeaderTopEntries>> GetLeaderTopEntriesAsync(string accountAddress, Commitment commitment = Commitment.Finalized)
        {
            var res = await RpcClient.GetAccountInfoAsync(accountAddress, commitment);
            if (!res.WasSuccessful)
                return new Solana.Unity.Programs.Models.AccountResultWrapper<LeaderTopEntries>(res);
            var resultingAccount = LeaderTopEntries.Deserialize(Convert.FromBase64String(res.Result.Value.Data[0]));
            return new Solana.Unity.Programs.Models.AccountResultWrapper<LeaderTopEntries>(res, resultingAccount);
        }

        public async Task<SubscriptionState> SubscribeAchievementAsync(string accountAddress, Action<SubscriptionState, Solana.Unity.Rpc.Messages.ResponseValue<Solana.Unity.Rpc.Models.AccountInfo>, Achievement> callback, Commitment commitment = Commitment.Finalized)
        {
            SubscriptionState res = await StreamingRpcClient.SubscribeAccountInfoAsync(accountAddress, (s, e) =>
            {
                Achievement parsingResult = null;
                if (e.Value?.Data?.Count > 0)
                    parsingResult = Achievement.Deserialize(Convert.FromBase64String(e.Value.Data[0]));
                callback(s, e, parsingResult);
            }, commitment);
            return res;
        }

        public async Task<SubscriptionState> SubscribeGameAsync(string accountAddress, Action<SubscriptionState, Solana.Unity.Rpc.Messages.ResponseValue<Solana.Unity.Rpc.Models.AccountInfo>, Game> callback, Commitment commitment = Commitment.Finalized)
        {
            SubscriptionState res = await StreamingRpcClient.SubscribeAccountInfoAsync(accountAddress, (s, e) =>
            {
                Game parsingResult = null;
                if (e.Value?.Data?.Count > 0)
                    parsingResult = Game.Deserialize(Convert.FromBase64String(e.Value.Data[0]));
                callback(s, e, parsingResult);
            }, commitment);
            return res;
        }

        public async Task<SubscriptionState> SubscribeLeaderBoardAsync(string accountAddress, Action<SubscriptionState, Solana.Unity.Rpc.Messages.ResponseValue<Solana.Unity.Rpc.Models.AccountInfo>, LeaderBoard> callback, Commitment commitment = Commitment.Finalized)
        {
            SubscriptionState res = await StreamingRpcClient.SubscribeAccountInfoAsync(accountAddress, (s, e) =>
            {
                LeaderBoard parsingResult = null;
                if (e.Value?.Data?.Count > 0)
                    parsingResult = LeaderBoard.Deserialize(Convert.FromBase64String(e.Value.Data[0]));
                callback(s, e, parsingResult);
            }, commitment);
            return res;
        }

        public async Task<SubscriptionState> SubscribeMergedAsync(string accountAddress, Action<SubscriptionState, Solana.Unity.Rpc.Messages.ResponseValue<Solana.Unity.Rpc.Models.AccountInfo>, Merged> callback, Commitment commitment = Commitment.Finalized)
        {
            SubscriptionState res = await StreamingRpcClient.SubscribeAccountInfoAsync(accountAddress, (s, e) =>
            {
                Merged parsingResult = null;
                if (e.Value?.Data?.Count > 0)
                    parsingResult = Merged.Deserialize(Convert.FromBase64String(e.Value.Data[0]));
                callback(s, e, parsingResult);
            }, commitment);
            return res;
        }

        public async Task<SubscriptionState> SubscribePlayerAchievementAsync(string accountAddress, Action<SubscriptionState, Solana.Unity.Rpc.Messages.ResponseValue<Solana.Unity.Rpc.Models.AccountInfo>, PlayerAchievement> callback, Commitment commitment = Commitment.Finalized)
        {
            SubscriptionState res = await StreamingRpcClient.SubscribeAccountInfoAsync(accountAddress, (s, e) =>
            {
                PlayerAchievement parsingResult = null;
                if (e.Value?.Data?.Count > 0)
                    parsingResult = PlayerAchievement.Deserialize(Convert.FromBase64String(e.Value.Data[0]));
                callback(s, e, parsingResult);
            }, commitment);
            return res;
        }

        public async Task<SubscriptionState> SubscribePlayerScoresListAsync(string accountAddress, Action<SubscriptionState, Solana.Unity.Rpc.Messages.ResponseValue<Solana.Unity.Rpc.Models.AccountInfo>, PlayerScoresList> callback, Commitment commitment = Commitment.Finalized)
        {
            SubscriptionState res = await StreamingRpcClient.SubscribeAccountInfoAsync(accountAddress, (s, e) =>
            {
                PlayerScoresList parsingResult = null;
                if (e.Value?.Data?.Count > 0)
                    parsingResult = PlayerScoresList.Deserialize(Convert.FromBase64String(e.Value.Data[0]));
                callback(s, e, parsingResult);
            }, commitment);
            return res;
        }

        public async Task<SubscriptionState> SubscribePlayerAsync(string accountAddress, Action<SubscriptionState, Solana.Unity.Rpc.Messages.ResponseValue<Solana.Unity.Rpc.Models.AccountInfo>, Player> callback, Commitment commitment = Commitment.Finalized)
        {
            SubscriptionState res = await StreamingRpcClient.SubscribeAccountInfoAsync(accountAddress, (s, e) =>
            {
                Player parsingResult = null;
                if (e.Value?.Data?.Count > 0)
                    parsingResult = Player.Deserialize(Convert.FromBase64String(e.Value.Data[0]));
                callback(s, e, parsingResult);
            }, commitment);
            return res;
        }

        public async Task<SubscriptionState> SubscribeNftClaimAsync(string accountAddress, Action<SubscriptionState, Solana.Unity.Rpc.Messages.ResponseValue<Solana.Unity.Rpc.Models.AccountInfo>, NftClaim> callback, Commitment commitment = Commitment.Finalized)
        {
            SubscriptionState res = await StreamingRpcClient.SubscribeAccountInfoAsync(accountAddress, (s, e) =>
            {
                NftClaim parsingResult = null;
                if (e.Value?.Data?.Count > 0)
                    parsingResult = NftClaim.Deserialize(Convert.FromBase64String(e.Value.Data[0]));
                callback(s, e, parsingResult);
            }, commitment);
            return res;
        }

        public async Task<SubscriptionState> SubscribeRewardAsync(string accountAddress, Action<SubscriptionState, Solana.Unity.Rpc.Messages.ResponseValue<Solana.Unity.Rpc.Models.AccountInfo>, Reward> callback, Commitment commitment = Commitment.Finalized)
        {
            SubscriptionState res = await StreamingRpcClient.SubscribeAccountInfoAsync(accountAddress, (s, e) =>
            {
                Reward parsingResult = null;
                if (e.Value?.Data?.Count > 0)
                    parsingResult = Reward.Deserialize(Convert.FromBase64String(e.Value.Data[0]));
                callback(s, e, parsingResult);
            }, commitment);
            return res;
        }

        public async Task<SubscriptionState> SubscribeLeaderTopEntriesAsync(string accountAddress, Action<SubscriptionState, Solana.Unity.Rpc.Messages.ResponseValue<Solana.Unity.Rpc.Models.AccountInfo>, LeaderTopEntries> callback, Commitment commitment = Commitment.Finalized)
        {
            SubscriptionState res = await StreamingRpcClient.SubscribeAccountInfoAsync(accountAddress, (s, e) =>
            {
                LeaderTopEntries parsingResult = null;
                if (e.Value?.Data?.Count > 0)
                    parsingResult = LeaderTopEntries.Deserialize(Convert.FromBase64String(e.Value.Data[0]));
                callback(s, e, parsingResult);
            }, commitment);
            return res;
        }

        protected override Dictionary<uint, ProgramError<SoarErrorKind>> BuildErrorsDictionary()
        {
            return new Dictionary<uint, ProgramError<SoarErrorKind>>{{6000U, new ProgramError<SoarErrorKind>(SoarErrorKind.InvalidFieldLength, "Exceeded max length for field.")}, {6001U, new ProgramError<SoarErrorKind>(SoarErrorKind.InvalidAuthority, "Invalid authority for instruction")}, {6002U, new ProgramError<SoarErrorKind>(SoarErrorKind.MissingSignature, "An expected signature isn't present")}, {6003U, new ProgramError<SoarErrorKind>(SoarErrorKind.NoRewardForAchievement, "Reward not specified for this achievement")}, {6004U, new ProgramError<SoarErrorKind>(SoarErrorKind.AccountNotPartOfMerge, "The merge account does not include this player account")}, {6005U, new ProgramError<SoarErrorKind>(SoarErrorKind.ScoreNotWithinBounds, "Tried to input score that is below the minimum or above the maximum")}, {6006U, new ProgramError<SoarErrorKind>(SoarErrorKind.MissingExpectedAccount, "An optional but expected account is missing")}, {6007U, new ProgramError<SoarErrorKind>(SoarErrorKind.InvalidRewardKind, "Invalid reward kind for this instruction")}, {6008U, new ProgramError<SoarErrorKind>(SoarErrorKind.NoAvailableRewards, "No more rewards are being given out for this game")}, };
        }
    }

    namespace Program
    {
        public class InitializeGameAccounts
        {
            public PublicKey Creator { get; set; }

            public PublicKey Game { get; set; }

            public PublicKey SystemProgram { get; set; }
        }

        public class UpdateGameAccounts
        {
            public PublicKey Authority { get; set; }

            public PublicKey Payer { get; set; }

            public PublicKey Game { get; set; }

            public PublicKey SystemProgram { get; set; }
        }

        public class AddAchievementAccounts
        {
            public PublicKey Authority { get; set; }

            public PublicKey Payer { get; set; }

            public PublicKey Game { get; set; }

            public PublicKey NewAchievement { get; set; }

            public PublicKey SystemProgram { get; set; }
        }

        public class UpdateAchievementAccounts
        {
            public PublicKey Authority { get; set; }

            public PublicKey Game { get; set; }

            public PublicKey Achievement { get; set; }
        }

        public class AddLeaderboardAccounts
        {
            public PublicKey Authority { get; set; }

            public PublicKey Payer { get; set; }

            public PublicKey Game { get; set; }

            public PublicKey Leaderboard { get; set; }

            public PublicKey TopEntries { get; set; }

            public PublicKey SystemProgram { get; set; }
        }

        public class UpdateLeaderboardAccounts
        {
            public PublicKey Authority { get; set; }

            public PublicKey Game { get; set; }

            public PublicKey Leaderboard { get; set; }

            public PublicKey TopEntries { get; set; }
        }

        public class InitializePlayerAccounts
        {
            public PublicKey Payer { get; set; }

            public PublicKey User { get; set; }

            public PublicKey PlayerAccount { get; set; }

            public PublicKey SystemProgram { get; set; }
        }

        public class UpdatePlayerAccounts
        {
            public PublicKey User { get; set; }

            public PublicKey PlayerAccount { get; set; }
        }

        public class RegisterPlayerAccounts
        {
            public PublicKey Payer { get; set; }

            public PublicKey User { get; set; }

            public PublicKey PlayerAccount { get; set; }

            public PublicKey Game { get; set; }

            public PublicKey Leaderboard { get; set; }

            public PublicKey NewList { get; set; }

            public PublicKey SystemProgram { get; set; }
        }

        public class SubmitScoreAccounts
        {
            public PublicKey Payer { get; set; }

            public PublicKey Authority { get; set; }

            public PublicKey PlayerAccount { get; set; }

            public PublicKey Game { get; set; }

            public PublicKey Leaderboard { get; set; }

            public PublicKey PlayerScores { get; set; }

            public PublicKey TopEntries { get; set; }

            public PublicKey SystemProgram { get; set; }
        }

        public class InitiateMergeAccounts
        {
            public PublicKey Payer { get; set; }

            public PublicKey User { get; set; }

            public PublicKey PlayerAccount { get; set; }

            public PublicKey MergeAccount { get; set; }

            public PublicKey SystemProgram { get; set; }
        }

        public class ApproveMergeAccounts
        {
            public PublicKey User { get; set; }

            public PublicKey PlayerAccount { get; set; }

            public PublicKey MergeAccount { get; set; }
        }

        public class UnlockPlayerAchievementAccounts
        {
            public PublicKey Authority { get; set; }

            public PublicKey Payer { get; set; }

            public PublicKey PlayerAccount { get; set; }

            public PublicKey Game { get; set; }

            public PublicKey Achievement { get; set; }

            public PublicKey PlayerAchievement { get; set; }

            public PublicKey SystemProgram { get; set; }
        }

        public class AddFtRewardAccounts
        {
            public PublicKey Authority { get; set; }

            public PublicKey Payer { get; set; }

            public PublicKey Game { get; set; }

            public PublicKey Achievement { get; set; }

            public PublicKey NewReward { get; set; }

            public PublicKey RewardTokenMint { get; set; }

            public PublicKey DelegateFromTokenAccount { get; set; }

            public PublicKey TokenAccountOwner { get; set; }

            public PublicKey TokenProgram { get; set; }

            public PublicKey SystemProgram { get; set; }
        }

        public class AddNftRewardAccounts
        {
            public PublicKey Authority { get; set; }

            public PublicKey Payer { get; set; }

            public PublicKey Game { get; set; }

            public PublicKey Achievement { get; set; }

            public PublicKey NewReward { get; set; }

            public PublicKey SystemProgram { get; set; }

            public PublicKey RewardCollectionMint { get; set; }

            public PublicKey CollectionUpdateAuth { get; set; }

            public PublicKey CollectionMetadata { get; set; }

            public PublicKey TokenMetadataProgram { get; set; }
        }

        public class ClaimFtRewardAccounts
        {
            public PublicKey User { get; set; }

            public PublicKey Authority { get; set; }

            public PublicKey Payer { get; set; }

            public PublicKey Game { get; set; }

            public PublicKey Achievement { get; set; }

            public PublicKey Reward { get; set; }

            public PublicKey PlayerAccount { get; set; }

            public PublicKey PlayerAchievement { get; set; }

            public PublicKey SourceTokenAccount { get; set; }

            public PublicKey UserTokenAccount { get; set; }

            public PublicKey TokenProgram { get; set; }

            public PublicKey SystemProgram { get; set; }
        }

        public class ClaimNftRewardAccounts
        {
            public PublicKey User { get; set; }

            public PublicKey Authority { get; set; }

            public PublicKey Game { get; set; }

            public PublicKey Payer { get; set; }

            public PublicKey Achievement { get; set; }

            public PublicKey Reward { get; set; }

            public PublicKey PlayerAccount { get; set; }

            public PublicKey PlayerAchievement { get; set; }

            public PublicKey Claim { get; set; }

            public PublicKey NewMint { get; set; }

            public PublicKey NewMetadata { get; set; }

            public PublicKey NewMasterEdition { get; set; }

            public PublicKey MintTo { get; set; }

            public PublicKey TokenMetadataProgram { get; set; }

            public PublicKey AssociatedTokenProgram { get; set; }

            public PublicKey SystemProgram { get; set; }

            public PublicKey TokenProgram { get; set; }

            public PublicKey Rent { get; set; }
        }

        public class VerifyNftRewardAccounts
        {
            public PublicKey Payer { get; set; }

            public PublicKey Game { get; set; }

            public PublicKey Achievement { get; set; }

            public PublicKey Reward { get; set; }

            public PublicKey User { get; set; }

            public PublicKey PlayerAccount { get; set; }

            public PublicKey Claim { get; set; }

            public PublicKey PlayerAchievement { get; set; }

            public PublicKey Mint { get; set; }

            public PublicKey MetadataToVerify { get; set; }

            public PublicKey CollectionMint { get; set; }

            public PublicKey CollectionMetadata { get; set; }

            public PublicKey CollectionEdition { get; set; }

            public PublicKey TokenMetadataProgram { get; set; }
        }

        public static class SoarSeeds
        {
            public static readonly byte[] Leaderboard = Encoding.UTF8.GetBytes("leaderboard");
            public static readonly byte[] Achievement = Encoding.UTF8.GetBytes("achievement");
            public static readonly byte[] Player = Encoding.UTF8.GetBytes("player");
            public static readonly byte[] PlayerScores = Encoding.UTF8.GetBytes("player-scores-list");
            public static readonly byte[] PlayerAchievement = Encoding.UTF8.GetBytes("player-achievement");
            public static readonly byte[] LeaderTopEntries = Encoding.UTF8.GetBytes("top-scores");
            public static readonly byte[] NftClaim = Encoding.UTF8.GetBytes("nft-claim");
        }

        public static class SoarPda
        {
            public static PublicKey PlayerPda(PublicKey user, PublicKey programId = null)
            {
                programId ??= SoarProgram.ProgramIdKey;
                PublicKey.TryFindProgramAddress(new[]
                {
                    SoarSeeds.Player, user.KeyBytes
                }, programId, out var pda, out _);
                return pda;
            }
            
            public static PublicKey PlayerScoresPda(PublicKey playerAccount, PublicKey leaderboard, PublicKey programId = null)
            {
                programId ??= SoarProgram.ProgramIdKey;
                PublicKey.TryFindProgramAddress(new[]
                {
                    SoarSeeds.PlayerScores, playerAccount.KeyBytes, leaderboard.KeyBytes
                }, programId, out var pda, out _);
                return pda;
            }
            
            public static PublicKey LeaderboardTopEntriesPda(PublicKey leaderboard, PublicKey programId = null)
            {
                programId ??= SoarProgram.ProgramIdKey;
                PublicKey.TryFindProgramAddress(new[]
                {
                    SoarSeeds.LeaderTopEntries, leaderboard.KeyBytes
                }, programId, out var pda, out _);
                return pda;
            }
            
            public static PublicKey LeaderboardPda(PublicKey game, ulong id = 0, PublicKey programId = null)
            {
                programId ??= SoarProgram.ProgramIdKey;
                PublicKey.TryFindProgramAddress(new[]
                {
                    SoarSeeds.Leaderboard, game.KeyBytes, BitConverter.GetBytes(id).ToArray()
                }, programId, out var pda, out _);
                return pda;
            }
            
            public static PublicKey AchievementPda(PublicKey game, ulong id = 0, PublicKey programId = null)
            {
                programId ??= SoarProgram.ProgramIdKey;
                PublicKey.TryFindProgramAddress(new[]
                {
                    SoarSeeds.Achievement, game.KeyBytes, BitConverter.GetBytes(id).ToArray()
                }, programId, out var pda, out _);
                return pda;
            }
            
            public static PublicKey PlayerAchievementPda(PublicKey user, PublicKey achievement, PublicKey programId = null)
            {
                programId ??= SoarProgram.ProgramIdKey;
                PublicKey.TryFindProgramAddress(new[]
                {
                    SoarSeeds.PlayerAchievement, user.KeyBytes, achievement.KeyBytes
                }, programId, out var pda, out _);
                return pda;
            }
            
            public static PublicKey NftClaimPda(PublicKey reward, PublicKey mint, PublicKey programId = null)
            {
                programId ??= SoarProgram.ProgramIdKey;
                PublicKey.TryFindProgramAddress(new[]
                {
                    SoarSeeds.NftClaim, reward.KeyBytes, mint.KeyBytes
                }, programId, out var pda, out _);
                return pda;
            }
        }

        public static class SoarProgram
        {
            // Define a constant for the program id
            public static readonly PublicKey ProgramIdKey = new("SoarNNzwQHMwcfdkdLc6kvbkoMSxcHy89gTHrjhJYkk");
            
            public static Solana.Unity.Rpc.Models.TransactionInstruction InitializeGame(InitializeGameAccounts accounts, GameAttributes gameMeta, PublicKey[] gameAuth, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Creator, true), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Game, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.SystemProgram, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(15529203708862021164UL, offset);
                offset += 8;
                offset += gameMeta.Serialize(_data, offset);
                _data.WriteS32(gameAuth.Length, offset);
                offset += 4;
                foreach (var gameAuthElement in gameAuth)
                {
                    _data.WritePubKey(gameAuthElement, offset);
                    offset += 32;
                }

                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction UpdateGame(UpdateGameAccounts accounts, GameAttributes newMeta, PublicKey[] newAuth, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Authority, true), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Payer, true), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Game, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.SystemProgram, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(15911756259288956319UL, offset);
                offset += 8;
                if (newMeta != null)
                {
                    _data.WriteU8(1, offset);
                    offset += 1;
                    offset += newMeta.Serialize(_data, offset);
                }
                else
                {
                    _data.WriteU8(0, offset);
                    offset += 1;
                }

                if (newAuth != null)
                {
                    _data.WriteU8(1, offset);
                    offset += 1;
                    _data.WriteS32(newAuth.Length, offset);
                    offset += 4;
                    foreach (var newAuthElement in newAuth)
                    {
                        _data.WritePubKey(newAuthElement, offset);
                        offset += 32;
                    }
                }
                else
                {
                    _data.WriteU8(0, offset);
                    offset += 1;
                }

                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction AddAchievement(AddAchievementAccounts accounts, string title, string description, PublicKey nftMeta, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Authority, true), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Payer, true), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Game, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.NewAchievement, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.SystemProgram, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(16394386559700451471UL, offset);
                offset += 8;
                offset += _data.WriteBorshString(title, offset);
                offset += _data.WriteBorshString(description, offset);
                _data.WritePubKey(nftMeta, offset);
                offset += 32;
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction UpdateAchievement(UpdateAchievementAccounts accounts, string newTitle, string newDescription, PublicKey nftMeta, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Authority, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Game, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Achievement, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(13695001829674640816UL, offset);
                offset += 8;
                if (newTitle != null)
                {
                    _data.WriteU8(1, offset);
                    offset += 1;
                    offset += _data.WriteBorshString(newTitle, offset);
                }
                else
                {
                    _data.WriteU8(0, offset);
                    offset += 1;
                }

                if (newDescription != null)
                {
                    _data.WriteU8(1, offset);
                    offset += 1;
                    offset += _data.WriteBorshString(newDescription, offset);
                }
                else
                {
                    _data.WriteU8(0, offset);
                    offset += 1;
                }

                if (nftMeta != null)
                {
                    _data.WriteU8(1, offset);
                    offset += 1;
                    _data.WritePubKey(nftMeta, offset);
                    offset += 32;
                }
                else
                {
                    _data.WriteU8(0, offset);
                    offset += 1;
                }

                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction AddLeaderboard(AddLeaderboardAccounts accounts, RegisterLeaderBoardInput input, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Authority, true), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Payer, true), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Game, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Leaderboard, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.TopEntries == null ? programId : accounts.TopEntries, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.SystemProgram, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(3559684157405148304UL, offset);
                offset += 8;
                offset += input.Serialize(_data, offset);
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction UpdateLeaderboard(UpdateLeaderboardAccounts accounts, string newDescription, PublicKey newNftMeta, ulong? newMinScore, ulong? newMaxScore, bool? newIsAscending, bool? newAllowMultipleScores, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Authority, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Game, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Leaderboard, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.TopEntries == null ? programId : accounts.TopEntries, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(2519656746723991368UL, offset);
                offset += 8;
                if (newDescription != null)
                {
                    _data.WriteU8(1, offset);
                    offset += 1;
                    offset += _data.WriteBorshString(newDescription, offset);
                }
                else
                {
                    _data.WriteU8(0, offset);
                    offset += 1;
                }

                if (newNftMeta != null)
                {
                    _data.WriteU8(1, offset);
                    offset += 1;
                    _data.WritePubKey(newNftMeta, offset);
                    offset += 32;
                }
                else
                {
                    _data.WriteU8(0, offset);
                    offset += 1;
                }

                if (newMinScore != null)
                {
                    _data.WriteU8(1, offset);
                    offset += 1;
                    _data.WriteU64(newMinScore.Value, offset);
                    offset += 8;
                }
                else
                {
                    _data.WriteU8(0, offset);
                    offset += 1;
                }

                if (newMaxScore != null)
                {
                    _data.WriteU8(1, offset);
                    offset += 1;
                    _data.WriteU64(newMaxScore.Value, offset);
                    offset += 8;
                }
                else
                {
                    _data.WriteU8(0, offset);
                    offset += 1;
                }

                if (newIsAscending != null)
                {
                    _data.WriteU8(1, offset);
                    offset += 1;
                    _data.WriteBool(newIsAscending.Value, offset);
                    offset += 1;
                }
                else
                {
                    _data.WriteU8(0, offset);
                    offset += 1;
                }

                if (newAllowMultipleScores != null)
                {
                    _data.WriteU8(1, offset);
                    offset += 1;
                    _data.WriteBool(newAllowMultipleScores.Value, offset);
                    offset += 1;
                }
                else
                {
                    _data.WriteU8(0, offset);
                    offset += 1;
                }

                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction InitializePlayer(InitializePlayerAccounts accounts, string username, PublicKey nftMeta, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Payer, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.User, true), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.PlayerAccount, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.SystemProgram, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(9239203753139697999UL, offset);
                offset += 8;
                offset += _data.WriteBorshString(username, offset);
                _data.WritePubKey(nftMeta, offset);
                offset += 32;
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction UpdatePlayer(UpdatePlayerAccounts accounts, string username, PublicKey nftMeta, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.User, true), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.PlayerAccount, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(9083936632088948156UL, offset);
                offset += 8;
                if (username != null)
                {
                    _data.WriteU8(1, offset);
                    offset += 1;
                    offset += _data.WriteBorshString(username, offset);
                }
                else
                {
                    _data.WriteU8(0, offset);
                    offset += 1;
                }

                if (nftMeta != null)
                {
                    _data.WriteU8(1, offset);
                    offset += 1;
                    _data.WritePubKey(nftMeta, offset);
                    offset += 32;
                }
                else
                {
                    _data.WriteU8(0, offset);
                    offset += 1;
                }

                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction RegisterPlayer(RegisterPlayerAccounts accounts, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Payer, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.User, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.PlayerAccount, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Game, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Leaderboard, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.NewList, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.SystemProgram, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(3090755682429997810UL, offset);
                offset += 8;
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction SubmitScore(SubmitScoreAccounts accounts, ulong score, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Payer, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Authority, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.PlayerAccount, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Game, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Leaderboard, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.PlayerScores, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.TopEntries == null ? programId : accounts.TopEntries, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.SystemProgram, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(16957550613295366356UL, offset);
                offset += 8;
                _data.WriteU64(score, offset);
                offset += 8;
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction InitiateMerge(InitiateMergeAccounts accounts, PublicKey[] adressesKeys, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Payer, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.User, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.PlayerAccount, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.MergeAccount, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.SystemProgram, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(1553774957166952211UL, offset);
                offset += 8;
                _data.WriteS32(adressesKeys.Length, offset);
                offset += 4;
                foreach (var keysElement in adressesKeys)
                {
                    _data.WritePubKey(keysElement, offset);
                    offset += 32;
                }

                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction ApproveMerge(ApproveMergeAccounts accounts, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.User, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.PlayerAccount, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.MergeAccount, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(15040972532094388097UL, offset);
                offset += 8;
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction UnlockPlayerAchievement(UnlockPlayerAchievementAccounts accounts, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Authority, true), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Payer, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.PlayerAccount, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Game, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Achievement, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.PlayerAchievement, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.SystemProgram, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(9372877310797705321UL, offset);
                offset += 8;
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction AddFtReward(AddFtRewardAccounts accounts, AddNewRewardInput input, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Authority, true), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Payer, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Game, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Achievement, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.NewReward, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.RewardTokenMint, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.DelegateFromTokenAccount, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TokenAccountOwner, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TokenProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.SystemProgram, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(11169176224461444512UL, offset);
                offset += 8;
                offset += input.Serialize(_data, offset);
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction AddNftReward(AddNftRewardAccounts accounts, AddNewRewardInput input, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Authority, true), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Payer, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Game, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Achievement, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.NewReward, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.SystemProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.RewardCollectionMint == null ? programId : accounts.RewardCollectionMint, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.CollectionUpdateAuth == null ? programId : accounts.CollectionUpdateAuth, true), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.CollectionMetadata == null ? programId : accounts.CollectionMetadata, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TokenMetadataProgram == null ? programId : accounts.TokenMetadataProgram, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(5449854750952795504UL, offset);
                offset += 8;
                offset += input.Serialize(_data, offset);
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction ClaimFtReward(ClaimFtRewardAccounts accounts, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.User, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Authority, true), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Payer, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Game, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Achievement, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Reward, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.PlayerAccount, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.PlayerAchievement, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.SourceTokenAccount, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.UserTokenAccount, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TokenProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.SystemProgram, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(1958220672705354616UL, offset);
                offset += 8;
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction ClaimNftReward(ClaimNftRewardAccounts accounts, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.User, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Authority, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Game, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Payer, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Achievement, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Reward, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.PlayerAccount, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.PlayerAchievement, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Claim, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.NewMint, true), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.NewMetadata, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.NewMasterEdition, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.MintTo, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TokenMetadataProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.AssociatedTokenProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.SystemProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TokenProgram, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Rent, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(237582963561661816UL, offset);
                offset += 8;
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }

            public static Solana.Unity.Rpc.Models.TransactionInstruction VerifyNftReward(VerifyNftRewardAccounts accounts, PublicKey programId)
            {
                List<Solana.Unity.Rpc.Models.AccountMeta> keys = new()
                {Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.Payer, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Game, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Achievement, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Reward, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.User, true), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.PlayerAccount, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Claim, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.PlayerAchievement, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.Mint, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.MetadataToVerify, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.CollectionMint, false), Solana.Unity.Rpc.Models.AccountMeta.Writable(accounts.CollectionMetadata, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.CollectionEdition, false), Solana.Unity.Rpc.Models.AccountMeta.ReadOnly(accounts.TokenMetadataProgram, false)};
                byte[] _data = new byte[1200];
                int offset = 0;
                _data.WriteU64(13105199909563939123UL, offset);
                offset += 8;
                byte[] resultData = new byte[offset];
                Array.Copy(_data, resultData, offset);
                return new Solana.Unity.Rpc.Models.TransactionInstruction{Keys = keys, ProgramId = programId.KeyBytes, Data = resultData};
            }
        }
    }
}