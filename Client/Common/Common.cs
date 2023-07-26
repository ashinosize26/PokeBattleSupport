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
                    mark.MarkText = "● (4)";
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
                    mark.MarkText = "▲ (0.25)";
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
        /// 最速の計算
        /// </summary>
        /// <param name="baseStat"></param>
        /// <returns></returns>
        public static double CalcSpeedFastest(double baseStat)
        {
            if (baseStat == 0) return 0;
            return Math.Floor((baseStat + 52) * 1.1);
        }

        /// <summary>
        /// 準速の計算
        /// </summary>
        /// <param name="baseStat"></param>
        /// <returns></returns>
        public static double CalcSpeedFast(double baseStat)
        {
            if (baseStat == 0) return 0;
            return baseStat + 52;
        }

        /// <summary>
        /// 無振の計算
        /// </summary>
        /// <param name="baseStat"></param>
        /// <returns></returns>
        public static double CalcSpeedDefault(double baseStat)
        {
            if (baseStat == 0) return 0;
            return baseStat + 20;
        }

        /// <summary>
        /// 下降の計算
        /// </summary>
        /// <param name="baseStat"></param>
        /// <returns></returns>
        public static double CalcSpeedSlow(double baseStat)
        {
            if (baseStat == 0) return 0;
            return Math.Floor((baseStat + 20) * 0.9);
        }

        /// <summary>
        /// 最遅の計算
        /// </summary>
        /// <param name="baseStat"></param>
        /// <returns></returns>
        public static double CalcSpeedSlowest(double baseStat)
        {
            if (baseStat == 0) return 0;
            return Math.Floor((baseStat + 5) * 0.9);
        }
    }
}
