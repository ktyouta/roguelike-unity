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
		public const int FIRST_SETTING = 1;
		public const int SECOND_SETTING = 2;

		//オブジェクト設置時の密度
		public const int NORMAL_SETTING = 1;
		public const int DENCE_SETTING = 2;
		public const int STRETCHING_SETTING = 3;
		public const int RANDOM_SETTING = 4;
	}
}