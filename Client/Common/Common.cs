using PokeBattleSupport.Client.Models;
using System.Runtime.CompilerServices;

namespace PokeBattleSupport.Client.Common
{
    public static class Common
    {
        /// <summary>
        /// 世代
        /// 世代によって種族値が違うポケモンがいる
        /// </summary>
        public const long CurrentGeneration = 9;

        /// <summary>
        /// パーティの所有者
        /// </summary>
        public enum PartyOwner
        {
            I,
            You
        }

        /// <summary>
        /// S種族値
        /// </summary>
        public enum SpeedRank
        {
            Fastest,
            Fast,
            Default,
            Slow,
            Slowest,
            Unknown
        }

        /// <summary>
        /// 持ち物
        /// </summary>
        public enum BattleItem
        {
            ChoiceScarf,
            None
        }

        /// <summary>
        /// タイプの画像のパスを取得
        /// </summary>
        /// <param name="japaneseTypeName">タイプ名（日本語）</param>
        /// <returns></returns>
        public static string GetTypeImageFilePath(string japaneseTypeName)
        {
            string path = string.Empty;

            switch (japaneseTypeName)
            {
                case "ノーマル":
                    path = "images/type_normal.png";
                    break;

                case "ほのお":
                    path = "images/type_fire.png";
                    break;

                case "みず":
                    path = "images/type_water.png";
                    break;

                case "でんき":
                    path = "images/type_electric.png";
                    break;

                case "くさ":
                    path = "images/type_grass.png";
                    break;

                case "こおり":
                    path = "images/type_ice.png";
                    break;

                case "かくとう":
                    path = "images/type_fighting.png";
                    break;

                case "どく":
                    path = "images/type_poison.png";
                    break;

                case "じめん":
                    path = "images/type_ground.png";
                    break;

                case "ひこう":
                    path = "images/type_flying.png";
                    break;

                case "エスパー":
                    path = "images/type_psychic.png";
                    break;

                case "むし":
                    path = "images/type_bug.png";
                    break;

                case "いわ":
                    path = "images/type_rock.png";
                    break;

                case "ゴースト":
                    path = "images/type_ghost.png";
                    break;

                case "ドラゴン":
                    path = "images/type_dragon.png";
                    break;

                case "あく":
                    path = "images/type_dark.png";
                    break;

                case "はがね":
                    path = "images/type_steel.png";
                    break;

                case "フェアリー":
                    path = "images/type_fairy.png";
                    break;

                default:
                    break;
            }

            return path;
        }

        /// <summary>
        /// 攻撃したときのタイプ相性による倍率の数値からマークを取得
        /// </summary>
        /// <param name="TypeEffectiveValue"></param>
        /// <returns></returns>
        public static TypeEffectiveMarkModel GetTypeEffectiveMark(double TypeEffectiveValue)
        {
            TypeEffectiveMarkModel mark = new();

            switch (TypeEffectiveValue)
            {
                case 4:
                    mark.MarkText = "●4";
                    mark.MarkColor = MudBlazor.Color.Secondary;
                    break;

                case 2:
                    mark.MarkText = "●";
                    mark.MarkColor = MudBlazor.Color.Secondary;
                    break;

                case 1:
                    mark.MarkText = "";
                    mark.MarkColor = MudBlazor.Color.Secondary;
                    break;

                case 0.5:
                    mark.MarkText = "▲";
                    mark.MarkColor = MudBlazor.Color.Info;
                    break;

                case 0.25:
                    mark.MarkText = "▲1/4";
                    mark.MarkColor = MudBlazor.Color.Info;
                    break;

                case 0:
                    mark.MarkText = "×";
                    mark.MarkColor = MudBlazor.Color.Default;
                    break;

                default:
                    break;
            }

            return mark;
        }

        /// <summary>
        /// S実数値の計算
        /// </summary>
        /// <param name="baseStat"></param>
        /// <returns></returns>
        public static int CalcSpeedRealNumber(int baseStat, SpeedRank speedRank)
        {
            int realNumber = 0;
            if (baseStat == 0) return 0;

            switch (speedRank)
            {
                case SpeedRank.Fastest:
                    // 最速
                    realNumber = (int)Math.Floor((baseStat + 52) * 1.1);
                    break;

                case SpeedRank.Fast:
                    // 準速
                    realNumber = baseStat + 52;
                    break;

                case SpeedRank.Default:
                    // 無振
                    realNumber = baseStat + 20;
                    break;

                case SpeedRank.Slow:
                    // 下降
                    realNumber = (int)Math.Floor((baseStat + 20) * 0.9);
                    break;

                case SpeedRank.Slowest:
                    // 最遅
                    realNumber = (int)Math.Floor((baseStat + 5) * 0.9);
                    break;
            }

            return realNumber;
        }
    }
}
