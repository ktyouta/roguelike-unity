using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
	public static class Define
	{
		//フロアごとのオブジェクト設置パターン
		public const int FOREST_DIVISION = 1;
		public const int SOIL_DIVISION = 2;
		public const int STONE_DIVISION = 3;
		public const int DESERT_DIVISION = 4;

		//区画ごとのオブジェクト設置パターン
		public const int FIRST_SETTING = 0;
		public const int SECOND_SETTING = 1;

		//オブジェクト設置時の密度
		public const int NORMAL_SETTING = 0;
		public const int DENCE_SETTING = 1;
		public const int STRETCHING_SETTING = 2;
		public const int RANDOM_SETTING = 3;

		//レイヤー設定
		public const string BLOCKING_LAYER = "BlockingLayer";
		public const string PLAYER_LAYER = "Player";
		public const string ENEMY_LAYER = "Enemy";
		public const string TREASURE_LAYER = "Treasure";
		public const string NPC_LAYER = "Npc";
		public const string NPCFELLOW_LAYER = "NpcFellow";
		public const string ITEM_LAYER = "Item";

		//仲間にできるNPCの上限数
		public const int FELLOWS_MAXNUM = 3;

		//A-starアルゴリズムの最大計算(ループ)回数
		public const int ASTAR_LOOPNUM = 3;

		//敵が出現するターン数
		public const int NOMALENEMY_APPEARANCE_MINTURN = 10;
		public const int NOMALENEMY_APPEARANCE_MAXTURN = 20;

		//不思議のダンジョン系マップ(通常)のサイズ
		//横幅
		public const int MYSTERYMAP_WHITH = 40;
		//縦幅
		public const int MYSTERYMAP_HEIGHT = 35;

		//不思議のダンジョン系マップ(ボス戦)のサイズ
		//横幅
		public const int MYSTERYBOSSMAP_WHITH = 15;
		//縦幅
		public const int MYSTERYBOSSMAP_HEIGHT = 10;

		//不思議のダンジョン系マップのタイル種別
		public const int MOVABLE = 1;
		public const int AISLE = 2;
		public const int WALL = 3;

		//不思議のダンジョン系マップのエリア分割数
		public const int AREA_MIN_NUM = 5;
		public const int AREA_MAX_NUM = 8;

		//不思議のダンジョン系マップのエリア分割リセット回数
		public const int AREA_SPLIT_MAXNUM = 10;

		//不思議のダンジョン系マップの分割エリアの最小面積
		public const int MIN_AREA = 9;

		//不思議のダンジョン系マップのタイルの切り替え時の階数(MAPTILE_CHANGE_STAIRS階ごとにマップが切り替わる)
		public const int MAPTILE_CHANGE_STAIRS = 1;

		//不思議のダンジョン系マップのアクセント用の外壁の配置確率(%)
		public const int WALL_LOTTERY_PARAM = 10;

		//不思議のダンジョン系マップのモンスターハウスの出現確率(%)
		public const int MONSTERHOUSE_PROBABILITY = 5;
	}

	//アイテムのタイプ
	public enum ItemTypeEnum
	{
		Consume,
		Equipment
	}
}