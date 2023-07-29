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

        /// <summary>
        /// ポケモンの名称一覧を取得
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task<PokeNames> GetPokeNames(Empty request, ServerCallContext context)
        {
            var responseData = new PokeNames();

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

            return Task.FromResult(responseData);
        }

        /// <summary>
        /// ポケモンの基本情報を取得
        /// </summary>
        /// <param name="requestData"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task<PokeSimpleInfo> GetPokeSimpleInfo(PokeName requestData, ServerCallContext context)
        {
            var responseData = new PokeSimpleInfo();

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

            return Task.FromResult(responseData);
        }

        /// <summary>
        /// ポケモンの種族値を取得
        /// </summary>
        /// <param name="requestData"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task<PokeBaseStats> GetPokeBaseStats(PokeBaseStatsKey requestData, ServerCallContext context)
        {
            var responseData = new PokeBaseStats();

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
                responseData.H = 0;
                responseData.A = 0; 
                responseData.B = 0;
                responseData.C = 0;
                responseData.D = 0;
                responseData.S = 0;
                responseData.Total = 0;
            }

            return Task.FromResult(responseData);
        }
        
        /// <summary>
        /// ポケモンのタイプ耐性を取得
        /// </summary>
        /// <param name="requestData"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task<PokeTypeEffective> GetPokeTypeEffective(PokeName requestData, ServerCallContext context)
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

            // DB接続先情報
            var sqlConnectionSb = new SQLiteConnectionStringBuilder() { DataSource = "poke.db" };

            // DB接続に必要なインスタンスの生成
            using var connection = new SQLiteConnection(sqlConnectionSb.ToString());

            // 接続開始
            connection.Open();

            // ポケモンのタイプを取得
            using var command1 = connection.CreateCommand();
            command1.CommandText = $"SELECT TYPE_1, TYPE_2 FROM POKE WHERE NAME_JP = '{requestData.Name}'";
            using var reader1 = command1.ExecuteReader();
            string firstType = string.Empty;
            string secondType = string.Empty;
            if (reader1.Read())
            {
                firstType = reader1["TYPE_1"].ToString() ?? string.Empty;
                secondType = reader1["TYPE_2"].ToString() ?? string.Empty;
            }

            // タイプ相性表を取得し、各タイプの耐性を掛け合わせる
            using var command2 = connection.CreateCommand();
            command2.CommandText = "SELECT * FROM TYPE_EFFECTIVE ORDER BY TYPE_NO";
            using var reader2 = command2.ExecuteReader();
            while (reader2.Read())
            {
                switch (reader2["ATTACK_TYPE"].ToString())
                {
                    case "ノーマル":
                        if (!string.IsNullOrEmpty(firstType))
                        {
                            responseData.NormalValue *= (double)reader2[$"{firstType}"];
                        }
                        if (!string.IsNullOrEmpty(secondType))
                        {
                            responseData.NormalValue *= (double)reader2[$"{secondType}"];
                        }
                        break;

                    case "ほのお":
                        if (!string.IsNullOrEmpty(firstType))
                        {
                            responseData.FireValue *= (double)reader2[$"{firstType}"];
                        }
                        if (!string.IsNullOrEmpty(secondType))
                        {
                            responseData.FireValue *= (double)reader2[$"{secondType}"];
                        }
                        break;

                    case "みず":
                        if (!string.IsNullOrEmpty(firstType))
                        {
                            responseData.WaterValue *= (double)reader2[$"{firstType}"];
                        }
                        if (!string.IsNullOrEmpty(secondType))
                        {
                            responseData.WaterValue *= (double)reader2[$"{secondType}"];
                        }
                        break;

                    case "でんき":
                        if (!string.IsNullOrEmpty(firstType))
                        {
                            responseData.ElecticValue *= (double)reader2[$"{firstType}"];
                        }
                        if (!string.IsNullOrEmpty(secondType))
                        {
                            responseData.ElecticValue *= (double)reader2[$"{secondType}"];
                        }
                        break;

                    case "くさ":
                        if (!string.IsNullOrEmpty(firstType))
                        {
                            responseData.GrassValue *= (double)reader2[$"{firstType}"];
                        }
                        if (!string.IsNullOrEmpty(secondType))
                        {
                            responseData.GrassValue *= (double)reader2[$"{secondType}"];
                        }
                        break;

                    case "こおり":
                        if (!string.IsNullOrEmpty(firstType))
                        {
                            responseData.IceValue *= (double)reader2[$"{firstType}"];
                        }
                        if (!string.IsNullOrEmpty(secondType))
                        {
                            responseData.IceValue *= (double)reader2[$"{secondType}"];
                        }
                        break;

                    case "かくとう":
                        if (!string.IsNullOrEmpty(firstType))
                        {
                            responseData.FightingValue *= (double)reader2[$"{firstType}"];
                        }
                        if (!string.IsNullOrEmpty(secondType))
                        {
                            responseData.FightingValue *= (double)reader2[$"{secondType}"];
                        }
                        break;

                    case "どく":
                        if (!string.IsNullOrEmpty(firstType))
                        {
                            responseData.PoisonValue *= (double)reader2[$"{firstType}"];
                        }
                        if (!string.IsNullOrEmpty(secondType))
                        {
                            responseData.PoisonValue *= (double)reader2[$"{secondType}"];
                        }
                        break;

                    case "じめん":
                        if (!string.IsNullOrEmpty(firstType))
                        {
                            responseData.GroundValue *= (double)reader2[$"{firstType}"];
                        }
                        if (!string.IsNullOrEmpty(secondType))
                        {
                            responseData.GroundValue *= (double)reader2[$"{secondType}"];
                        }
                        break;

                    case "ひこう":
                        if (!string.IsNullOrEmpty(firstType))
                        {
                            responseData.FlyingValue *= (double)reader2[$"{firstType}"];
                        }
                        if (!string.IsNullOrEmpty(secondType))
                        {
                            responseData.FlyingValue *= (double)reader2[$"{secondType}"];
                        }
                        break;

                    case "エスパー":
                        if (!string.IsNullOrEmpty(firstType))
                        {
                            responseData.PsychicValue *= (double)reader2[$"{firstType}"];
                        }
                        if (!string.IsNullOrEmpty(secondType))
                        {
                            responseData.PsychicValue *= (double)reader2[$"{secondType}"];
                        }
                        break;

                    case "むし":
                        if (!string.IsNullOrEmpty(firstType))
                        {
                            responseData.BugValue *= (double)reader2[$"{firstType}"];
                        }
                        if (!string.IsNullOrEmpty(secondType))
                        {
                            responseData.BugValue *= (double)reader2[$"{secondType}"];
                        }
                        break;

                    case "いわ":
                        if (!string.IsNullOrEmpty(firstType))
                        {
                            responseData.RockValue *= (double)reader2[$"{firstType}"];
                        }
                        if (!string.IsNullOrEmpty(secondType))
                        {
                            responseData.RockValue *= (double)reader2[$"{secondType}"];
                        }
                        break;

                    case "ゴースト":
                        if (!string.IsNullOrEmpty(firstType))
                        {
                            responseData.GhostValue *= (double)reader2[$"{firstType}"];
                        }
                        if (!string.IsNullOrEmpty(secondType))
                        {
                            responseData.GhostValue *= (double)reader2[$"{secondType}"];
                        }
                        break;

                    case "ドラゴン":
                        if (!string.IsNullOrEmpty(firstType))
                        {
                            responseData.DragonValue *= (double)reader2[$"{firstType}"];
                        }
                        if (!string.IsNullOrEmpty(secondType))
                        {
                            responseData.DragonValue *= (double)reader2[$"{secondType}"];
                        }
                        break;

                    case "あく":
                        if (!string.IsNullOrEmpty(firstType))
                        {
                            responseData.DarkValue *= (double)reader2[$"{firstType}"];
                        }
                        if (!string.IsNullOrEmpty(secondType))
                        {
                            responseData.DarkValue *= (double)reader2[$"{secondType}"];
                        }
                        break;

                    case "はがね":
                        if (!string.IsNullOrEmpty(firstType))
                        {
                            responseData.SteelValue *= (double)reader2[$"{firstType}"];
                        }
                        if (!string.IsNullOrEmpty(secondType))
                        {
                            responseData.SteelValue *= (double)reader2[$"{secondType}"];
                        }
                        break;

                    case "フェアリー":
                        if (!string.IsNullOrEmpty(firstType))
                        {
                            responseData.FairyValue *= (double)reader2[$"{firstType}"];
                        }
                        if (!string.IsNullOrEmpty(secondType))
                        {
                            responseData.FairyValue *= (double)reader2[$"{secondType}"];
                        }
                        break;
                }
            }

            return Task.FromResult(responseData);
        }

        /// <summary>
        /// タイプ相性表を取得
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task<TypeEffectiveTable> GetTypeEffectiveTable(Empty request, ServerCallContext context)
        {
            var responseData = new TypeEffectiveTable();

            // DB接続先情報
            var sqlConnectionSb = new SQLiteConnectionStringBuilder() { DataSource = "poke.db" };

            // DB接続に必要なインスタンスの生成
            using var connection = new SQLiteConnection(sqlConnectionSb.ToString());

            // 接続開始
            connection.Open();

            // SQL実行に必要なインスタンスの生成
            using var command = connection.CreateCommand();

            // SELECT文の実行
            command.CommandText = "SELECT * FROM TYPE_EFFECTIVE ORDER BY TYPE_NO";
            using var reader = command.ExecuteReader();

            // 1行ずつデータを取得
            while (reader.Read())
            {
                var row = new TypeEffectiveRow()
                    {
                        AttackType    = reader["ATTACK_TYPE"].ToString(),
                        TypeEffective = new PokeTypeEffective
                        {
                            NormalValue = (double)reader["ノーマル"],
                            FireValue = (double)reader["ほのお"],
                            WaterValue = (double)reader["みず"],
                            ElecticValue = (double)reader["でんき"],
                            GrassValue = (double)reader["くさ"],
                            IceValue = (double)reader["こおり"],
                            FightingValue = (double)reader["かくとう"],
                            PoisonValue = (double)reader["どく"],
                            GroundValue = (double)reader["じめん"],
                            FlyingValue = (double)reader["ひこう"],
                            PsychicValue = (double)reader["エスパー"],
                            BugValue = (double)reader["むし"],
                            RockValue = (double)reader["いわ"],
                            GhostValue = (double)reader["ゴースト"],
                            DragonValue = (double)reader["ドラゴン"],
                            DarkValue = (double)reader["あく"],
                            SteelValue = (double)reader["はがね"],
                            FairyValue = (double)reader["フェアリー"],
                        }
                    };
                responseData.Items.Add(row);
            }

            return Task.FromResult(responseData);
        }
    }
}