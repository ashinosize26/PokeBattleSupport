using Grpc.Core;
using GrpcPoke;
using System.Data.SQLite;

namespace Grpc.Services
{
    public class PokeService : PokeSrv.PokeSrvBase
    {
        private readonly ILogger<PokeService> _logger;
        public PokeService(ILogger<PokeService> logger)
        {
            _logger = logger;
        }

        public override Task<PokeNames> GetPokeNames(Empty request, ServerCallContext context)
        {
            var responseData = new PokeNames();

            {
                // DB接続先情報
                var sqlConnectionSb = new SQLiteConnectionStringBuilder(){ DataSource = "poke.db" };

                // DB接続に必要なインスタンスの生成
                using var connection = new SQLiteConnection(sqlConnectionSb.ToString());

                // 接続開始
                connection.Open();

                // SQL実行に必要なインスタンスの生成
                using var command = connection.CreateCommand();

                // SELECT文の実行
                command.CommandText = "SELECT NAME_JP FROM POKE ORDER BY NO";
                using var reader = command.ExecuteReader();

                // 1行ずつデータを取得
                while (reader.Read())
                {
                    responseData.Items.Add(new PokeName() { Name = reader["NAME_JP"].ToString() });
                }
            }

            return Task.FromResult(responseData);
        }
        public override Task<PokeTypes> GetPokeTypes(PokeName requestData, ServerCallContext context)
        {
            var responseData = new PokeTypes();

            {
                // DB接続先情報
                var sqlConnectionSb = new SQLiteConnectionStringBuilder() { DataSource = "poke.db" };

                // DB接続に必要なインスタンスの生成
                using var connection = new SQLiteConnection(sqlConnectionSb.ToString());

                // 接続開始
                connection.Open();

                // SQL実行に必要なインスタンスの生成
                using var command = connection.CreateCommand();

                // SELECT文の実行
                command.CommandText = $"SELECT NAME_JP, NO, TYPE_1, TYPE_2 FROM POKE WHERE NAME_JP = '{requestData.Name}'";
                using var reader = command.ExecuteReader();

                // データを取得
                if (reader.Read())
                {
                    responseData.Name = reader["NAME_JP"].ToString();
                    responseData.No = (long)reader["NO"];
                    responseData.FirstType = reader["TYPE_1"].ToString();
                    responseData.SecondType = reader["TYPE_2"].ToString();
                }
                else
                {
                    responseData.Name = requestData.Name;
                    responseData.No = 0;
                    responseData.FirstType = string.Empty;
                    responseData.SecondType = string.Empty;
                }
            }

            return Task.FromResult(responseData);
        }

        public override Task<PokeBaseStats> GetPokeBaseStats(PokeBaseStatsKey requestData, ServerCallContext context)
        {
            var responseData = new PokeBaseStats();

            {
                // DB接続先情報
                var sqlConnectionSb = new SQLiteConnectionStringBuilder() { DataSource = "poke.db" };

                // DB接続に必要なインスタンスの生成
                using var connection = new SQLiteConnection(sqlConnectionSb.ToString());

                // 接続開始
                connection.Open();

                // SQL実行に必要なインスタンスの生成
                using var command = connection.CreateCommand();

                // SELECT文の実行
                command.CommandText = $"SELECT GENERATION, NAME_JP, NO, H, A, B, C, D, S, TOTAL FROM BASE_STATS WHERE GENERATION = {requestData.Generation} AND NAME_JP = '{requestData.Name}'";
                using var reader = command.ExecuteReader();

                // データを取得
                if (reader.Read())
                {
                    responseData.Generation = (long)reader["GENERATION"];
                    responseData.Name = reader["NAME_JP"].ToString();
                    responseData.No = (long)reader["NO"];
                    responseData.H = (long)reader["H"];
                    responseData.A = (long)reader["A"];
                    responseData.B = (long)reader["B"];
                    responseData.C = (long)reader["C"];
                    responseData.D = (long)reader["D"];
                    responseData.S = (long)reader["S"];
                    responseData.Total = (long)reader["TOTAL"];
                }
                else
                {
                    responseData.Generation = requestData.Generation;
                    responseData.Name = requestData.Name;
                    responseData.No = 0;
                    responseData.H = 0;
                    responseData.A = 0; 
                    responseData.B = 0;
                    responseData.C = 0;
                    responseData.D = 0;
                    responseData.S = 0;
                    responseData.Total = 0;
                }
            }

            return Task.FromResult(responseData);
        }

        public override Task<PokeTypeEffective> GetPokeTypeEffective(PokeTypes requestData, ServerCallContext context)
        {
            var responseData = new PokeTypeEffective()
            {
                NormalValue = 1,
                FireValue = 1,
                WaterValue = 1,
                ElecticValue = 1,
                GrassValue = 1,
                IceValue = 1,
                FightingValue = 1,
                PoisonValue = 1,
                GroundValue = 1,
                FlyingValue = 1,
                PsychicValue = 1,
                BugValue = 1,
                RockValue = 1,
                GhostValue = 1,
                DragonValue = 1,
                DarkValue = 1,
                SteelValue = 1,
                FairyValue = 1,
            };

            {
                // DB接続先情報
                var sqlConnectionSb = new SQLiteConnectionStringBuilder() { DataSource = "poke.db" };

                // DB接続に必要なインスタンスの生成
                using var connection = new SQLiteConnection(sqlConnectionSb.ToString());

                // 接続開始
                connection.Open();

                // SQL実行に必要なインスタンスの生成
                using var command = connection.CreateCommand();

                // SELECT文の実行
                for (int i = 1; i <= 2; i++)
                {
                    if (i == 1 && string.IsNullOrEmpty(requestData.FirstType))
                    {
                        continue;
                    }
                    if (i == 2 && string.IsNullOrEmpty(requestData.SecondType))
                    {
                        continue;
                    }

                    string type = i == 1 ? requestData.FirstType : requestData.SecondType;

                    command.CommandText = $"SELECT ATTACK_TYPE, {type} FROM TYPE_EFFECTIVE";
                    using var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        switch (reader["ATTACK_TYPE"].ToString())
                        {
                            case "ノーマル":
                                responseData.NormalValue = responseData.NormalValue * (double)reader[$"{type}"];
                                break;

                            case "ほのお":
                                responseData.FireValue = responseData.FireValue * (double)reader[$"{type}"];
                                break;

                            case "みず":
                                responseData.WaterValue = responseData.WaterValue * (double)reader[$"{type}"];
                                break;

                            case "でんき":
                                responseData.ElecticValue = responseData.ElecticValue * (double)reader[$"{type}"];
                                break;

                            case "くさ":
                                responseData.GrassValue = responseData.GrassValue * (double)reader[$"{type}"];
                                break;

                            case "こおり":
                                responseData.IceValue = responseData.IceValue * (double)reader[$"{type}"];
                                break;

                            case "かくとう":
                                responseData.FightingValue = responseData.FightingValue * (double)reader[$"{type}"];
                                break;

                            case "どく":
                                responseData.PoisonValue = responseData.PoisonValue * (double)reader[$"{type}"];
                                break;

                            case "じめん":
                                responseData.GroundValue = responseData.GroundValue * (double)reader[$"{type}"];
                                break;

                            case "ひこう":
                                responseData.FlyingValue = responseData.FlyingValue * (double)reader[$"{type}"];
                                break;

                            case "エスパー":
                                responseData.PsychicValue = responseData.PsychicValue * (double)reader[$"{type}"];
                                break;

                            case "むし":
                                responseData.BugValue = responseData.BugValue * (double)reader[$"{type}"];
                                break;

                            case "いわ":
                                responseData.RockValue = responseData.RockValue * (double)reader[$"{type}"];
                                break;

                            case "ゴースト":
                                responseData.GhostValue = responseData.GhostValue * (double)reader[$"{type}"];
                                break;

                            case "ドラゴン":
                                responseData.DragonValue = responseData.DragonValue * (double)reader[$"{type}"];
                                break;

                            case "あく":
                                responseData.DarkValue = responseData.DarkValue * (double)reader[$"{type}"];
                                break;

                            case "はがね":
                                responseData.SteelValue = responseData.SteelValue * (double)reader[$"{type}"];
                                break;

                            case "フェアリー":
                                responseData.FairyValue = responseData.FairyValue * (double)reader[$"{type}"];
                                break;
                        }
                    }
                }
            }

            return Task.FromResult(responseData);
        }

        public override Task<TypeEffectiveTable> GetTypeEffectiveTable(Empty request, ServerCallContext context)
        {
            var responseData = new TypeEffectiveTable();

            {
                // DB接続先情報
                var sqlConnectionSb = new SQLiteConnectionStringBuilder() { DataSource = "poke.db" };

                // DB接続に必要なインスタンスの生成
                using var connection = new SQLiteConnection(sqlConnectionSb.ToString());

                // 接続開始
                connection.Open();

                // SQL実行に必要なインスタンスの生成
                using var command = connection.CreateCommand();

                // SELECT文の実行
                command.CommandText = "SELECT ATTACK_TYPE, ノーマル, ほのお, みず, でんき, くさ, こおり, かくとう, どく, じめん, ひこう, エスパー, むし, いわ, ゴースト, ドラゴン, あく, はがね, フェアリー FROM TYPE_EFFECTIVE ORDER BY TYPE_NO";
                using var reader = command.ExecuteReader();

                // 1行ずつデータを取得
                while (reader.Read())
                {
                    var typeEffective = new TypeEffective()
                        {
                            AttackType    = reader["ATTACK_TYPE"].ToString(),
                            NormalValue   = (double)reader["ノーマル"],
                            FireValue     = (double)reader["ほのお"],
                            WaterValue    = (double)reader["みず"],
                            ElecticValue  = (double)reader["でんき"],
                            GrassValue    = (double)reader["くさ"],
                            IceValue      = (double)reader["こおり"],
                            FightingValue = (double)reader["かくとう"],
                            PoisonValue   = (double)reader["どく"],
                            GroundValue   = (double)reader["じめん"],
                            FlyingValue   = (double)reader["ひこう"],
                            PsychicValue  = (double)reader["エスパー"],
                            BugValue      = (double)reader["むし"],
                            RockValue     = (double)reader["いわ"],
                            GhostValue    = (double)reader["ゴースト"],
                            DragonValue   = (double)reader["ドラゴン"],
                            DarkValue     = (double)reader["あく"],
                            SteelValue    = (double)reader["はがね"],
                            FairyValue    = (double)reader["フェアリー"],
                        };
                    responseData.Items.Add(typeEffective);
                }
            }

            return Task.FromResult(responseData);
        }
    }
}