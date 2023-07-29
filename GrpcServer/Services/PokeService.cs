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
        /// �|�P�����̖��̈ꗗ���擾
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task<PokeNames> GetPokeNames(Empty request, ServerCallContext context)
        {
            var responseData = new PokeNames();

            // DB�ڑ�����
            var sqlConnectionSb = new SQLiteConnectionStringBuilder(){ DataSource = "poke.db" };

            // DB�ڑ��ɕK�v�ȃC���X�^���X�̐���
            using var connection = new SQLiteConnection(sqlConnectionSb.ToString());

            // �ڑ��J�n
            connection.Open();

            // SQL���s�ɕK�v�ȃC���X�^���X�̐���
            using var command = connection.CreateCommand();

            // SELECT���̎��s
            command.CommandText = "SELECT NAME_JP FROM POKE ORDER BY NO";
            using var reader = command.ExecuteReader();

            // 1�s���f�[�^���擾
            while (reader.Read())
            {
                responseData.Items.Add(new PokeName() { Name = reader["NAME_JP"].ToString() });
            }

            return Task.FromResult(responseData);
        }

        /// <summary>
        /// �|�P�����̊�{�����擾
        /// </summary>
        /// <param name="requestData"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task<PokeSimpleInfo> GetPokeSimpleInfo(PokeName requestData, ServerCallContext context)
        {
            var responseData = new PokeSimpleInfo();

            // DB�ڑ�����
            var sqlConnectionSb = new SQLiteConnectionStringBuilder() { DataSource = "poke.db" };

            // DB�ڑ��ɕK�v�ȃC���X�^���X�̐���
            using var connection = new SQLiteConnection(sqlConnectionSb.ToString());

            // �ڑ��J�n
            connection.Open();

            // SQL���s�ɕK�v�ȃC���X�^���X�̐���
            using var command = connection.CreateCommand();

            // SELECT���̎��s
            command.CommandText = $"SELECT NAME_JP, NO, TYPE_1, TYPE_2 FROM POKE WHERE NAME_JP = '{requestData.Name}'";
            using var reader = command.ExecuteReader();

            // �f�[�^���擾
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
        /// �|�P�����̎푰�l���擾
        /// </summary>
        /// <param name="requestData"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task<PokeBaseStats> GetPokeBaseStats(PokeBaseStatsKey requestData, ServerCallContext context)
        {
            var responseData = new PokeBaseStats();

            // DB�ڑ�����
            var sqlConnectionSb = new SQLiteConnectionStringBuilder() { DataSource = "poke.db" };

            // DB�ڑ��ɕK�v�ȃC���X�^���X�̐���
            using var connection = new SQLiteConnection(sqlConnectionSb.ToString());

            // �ڑ��J�n
            connection.Open();

            // SQL���s�ɕK�v�ȃC���X�^���X�̐���
            using var command = connection.CreateCommand();

            // SELECT���̎��s
            command.CommandText = $"SELECT GENERATION, NAME_JP, NO, H, A, B, C, D, S, TOTAL FROM BASE_STATS WHERE GENERATION = {requestData.Generation} AND NAME_JP = '{requestData.Name}'";
            using var reader = command.ExecuteReader();

            // �f�[�^���擾
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
        /// �|�P�����̃^�C�v�ϐ����擾
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

            // DB�ڑ�����
            var sqlConnectionSb = new SQLiteConnectionStringBuilder() { DataSource = "poke.db" };

            // DB�ڑ��ɕK�v�ȃC���X�^���X�̐���
            using var connection = new SQLiteConnection(sqlConnectionSb.ToString());

            // �ڑ��J�n
            connection.Open();

            // �|�P�����̃^�C�v���擾
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

            // �^�C�v�����\���擾���A�e�^�C�v�̑ϐ����|�����킹��
            using var command2 = connection.CreateCommand();
            command2.CommandText = "SELECT * FROM TYPE_EFFECTIVE ORDER BY TYPE_NO";
            using var reader2 = command2.ExecuteReader();
            while (reader2.Read())
            {
                switch (reader2["ATTACK_TYPE"].ToString())
                {
                    case "�m�[�}��":
                        if (!string.IsNullOrEmpty(firstType))
                        {
                            responseData.NormalValue *= (double)reader2[$"{firstType}"];
                        }
                        if (!string.IsNullOrEmpty(secondType))
                        {
                            responseData.NormalValue *= (double)reader2[$"{secondType}"];
                        }
                        break;

                    case "�ق̂�":
                        if (!string.IsNullOrEmpty(firstType))
                        {
                            responseData.FireValue *= (double)reader2[$"{firstType}"];
                        }
                        if (!string.IsNullOrEmpty(secondType))
                        {
                            responseData.FireValue *= (double)reader2[$"{secondType}"];
                        }
                        break;

                    case "�݂�":
                        if (!string.IsNullOrEmpty(firstType))
                        {
                            responseData.WaterValue *= (double)reader2[$"{firstType}"];
                        }
                        if (!string.IsNullOrEmpty(secondType))
                        {
                            responseData.WaterValue *= (double)reader2[$"{secondType}"];
                        }
                        break;

                    case "�ł�":
                        if (!string.IsNullOrEmpty(firstType))
                        {
                            responseData.ElecticValue *= (double)reader2[$"{firstType}"];
                        }
                        if (!string.IsNullOrEmpty(secondType))
                        {
                            responseData.ElecticValue *= (double)reader2[$"{secondType}"];
                        }
                        break;

                    case "����":
                        if (!string.IsNullOrEmpty(firstType))
                        {
                            responseData.GrassValue *= (double)reader2[$"{firstType}"];
                        }
                        if (!string.IsNullOrEmpty(secondType))
                        {
                            responseData.GrassValue *= (double)reader2[$"{secondType}"];
                        }
                        break;

                    case "������":
                        if (!string.IsNullOrEmpty(firstType))
                        {
                            responseData.IceValue *= (double)reader2[$"{firstType}"];
                        }
                        if (!string.IsNullOrEmpty(secondType))
                        {
                            responseData.IceValue *= (double)reader2[$"{secondType}"];
                        }
                        break;

                    case "�����Ƃ�":
                        if (!string.IsNullOrEmpty(firstType))
                        {
                            responseData.FightingValue *= (double)reader2[$"{firstType}"];
                        }
                        if (!string.IsNullOrEmpty(secondType))
                        {
                            responseData.FightingValue *= (double)reader2[$"{secondType}"];
                        }
                        break;

                    case "�ǂ�":
                        if (!string.IsNullOrEmpty(firstType))
                        {
                            responseData.PoisonValue *= (double)reader2[$"{firstType}"];
                        }
                        if (!string.IsNullOrEmpty(secondType))
                        {
                            responseData.PoisonValue *= (double)reader2[$"{secondType}"];
                        }
                        break;

                    case "���߂�":
                        if (!string.IsNullOrEmpty(firstType))
                        {
                            responseData.GroundValue *= (double)reader2[$"{firstType}"];
                        }
                        if (!string.IsNullOrEmpty(secondType))
                        {
                            responseData.GroundValue *= (double)reader2[$"{secondType}"];
                        }
                        break;

                    case "�Ђ���":
                        if (!string.IsNullOrEmpty(firstType))
                        {
                            responseData.FlyingValue *= (double)reader2[$"{firstType}"];
                        }
                        if (!string.IsNullOrEmpty(secondType))
                        {
                            responseData.FlyingValue *= (double)reader2[$"{secondType}"];
                        }
                        break;

                    case "�G�X�p�[":
                        if (!string.IsNullOrEmpty(firstType))
                        {
                            responseData.PsychicValue *= (double)reader2[$"{firstType}"];
                        }
                        if (!string.IsNullOrEmpty(secondType))
                        {
                            responseData.PsychicValue *= (double)reader2[$"{secondType}"];
                        }
                        break;

                    case "�ނ�":
                        if (!string.IsNullOrEmpty(firstType))
                        {
                            responseData.BugValue *= (double)reader2[$"{firstType}"];
                        }
                        if (!string.IsNullOrEmpty(secondType))
                        {
                            responseData.BugValue *= (double)reader2[$"{secondType}"];
                        }
                        break;

                    case "����":
                        if (!string.IsNullOrEmpty(firstType))
                        {
                            responseData.RockValue *= (double)reader2[$"{firstType}"];
                        }
                        if (!string.IsNullOrEmpty(secondType))
                        {
                            responseData.RockValue *= (double)reader2[$"{secondType}"];
                        }
                        break;

                    case "�S�[�X�g":
                        if (!string.IsNullOrEmpty(firstType))
                        {
                            responseData.GhostValue *= (double)reader2[$"{firstType}"];
                        }
                        if (!string.IsNullOrEmpty(secondType))
                        {
                            responseData.GhostValue *= (double)reader2[$"{secondType}"];
                        }
                        break;

                    case "�h���S��":
                        if (!string.IsNullOrEmpty(firstType))
                        {
                            responseData.DragonValue *= (double)reader2[$"{firstType}"];
                        }
                        if (!string.IsNullOrEmpty(secondType))
                        {
                            responseData.DragonValue *= (double)reader2[$"{secondType}"];
                        }
                        break;

                    case "����":
                        if (!string.IsNullOrEmpty(firstType))
                        {
                            responseData.DarkValue *= (double)reader2[$"{firstType}"];
                        }
                        if (!string.IsNullOrEmpty(secondType))
                        {
                            responseData.DarkValue *= (double)reader2[$"{secondType}"];
                        }
                        break;

                    case "�͂���":
                        if (!string.IsNullOrEmpty(firstType))
                        {
                            responseData.SteelValue *= (double)reader2[$"{firstType}"];
                        }
                        if (!string.IsNullOrEmpty(secondType))
                        {
                            responseData.SteelValue *= (double)reader2[$"{secondType}"];
                        }
                        break;

                    case "�t�F�A���[":
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
        /// �^�C�v�����\���擾
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task<TypeEffectiveTable> GetTypeEffectiveTable(Empty request, ServerCallContext context)
        {
            var responseData = new TypeEffectiveTable();

            // DB�ڑ�����
            var sqlConnectionSb = new SQLiteConnectionStringBuilder() { DataSource = "poke.db" };

            // DB�ڑ��ɕK�v�ȃC���X�^���X�̐���
            using var connection = new SQLiteConnection(sqlConnectionSb.ToString());

            // �ڑ��J�n
            connection.Open();

            // SQL���s�ɕK�v�ȃC���X�^���X�̐���
            using var command = connection.CreateCommand();

            // SELECT���̎��s
            command.CommandText = "SELECT * FROM TYPE_EFFECTIVE ORDER BY TYPE_NO";
            using var reader = command.ExecuteReader();

            // 1�s���f�[�^���擾
            while (reader.Read())
            {
                var row = new TypeEffectiveRow()
                    {
                        AttackType    = reader["ATTACK_TYPE"].ToString(),
                        TypeEffective = new PokeTypeEffective
                        {
                            NormalValue = (double)reader["�m�[�}��"],
                            FireValue = (double)reader["�ق̂�"],
                            WaterValue = (double)reader["�݂�"],
                            ElecticValue = (double)reader["�ł�"],
                            GrassValue = (double)reader["����"],
                            IceValue = (double)reader["������"],
                            FightingValue = (double)reader["�����Ƃ�"],
                            PoisonValue = (double)reader["�ǂ�"],
                            GroundValue = (double)reader["���߂�"],
                            FlyingValue = (double)reader["�Ђ���"],
                            PsychicValue = (double)reader["�G�X�p�["],
                            BugValue = (double)reader["�ނ�"],
                            RockValue = (double)reader["����"],
                            GhostValue = (double)reader["�S�[�X�g"],
                            DragonValue = (double)reader["�h���S��"],
                            DarkValue = (double)reader["����"],
                            SteelValue = (double)reader["�͂���"],
                            FairyValue = (double)reader["�t�F�A���["],
                        }
                    };
                responseData.Items.Add(row);
            }

            return Task.FromResult(responseData);
        }
    }
}