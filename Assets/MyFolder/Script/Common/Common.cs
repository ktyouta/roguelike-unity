using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
	public static class Define
	{
		//�t���A���Ƃ̃I�u�W�F�N�g�ݒu�p�^�[��
		public const int FOREST_DIVISION = 1;
		public const int SOIL_DIVISION = 2;
		public const int STONE_DIVISION = 3;
		public const int DESERT_DIVISION = 4;

		//��悲�Ƃ̃I�u�W�F�N�g�ݒu�p�^�[��
		public const int FIRST_SETTING = 0;
		public const int SECOND_SETTING = 1;

		//�I�u�W�F�N�g�ݒu���̖��x
		public const int NORMAL_SETTING = 0;
		public const int DENCE_SETTING = 1;
		public const int STRETCHING_SETTING = 2;
		public const int RANDOM_SETTING = 3;

		//���C���[�ݒ�
		public const string BLOCKING_LAYER = "BlockingLayer";
		public const string PLAYER_LAYER = "Player";
		public const string ENEMY_LAYER = "Enemy";
		public const string TREASURE_LAYER = "Treasure";
		public const string NPC_LAYER = "Npc";
		public const string NPCFELLOW_LAYER = "NpcFellow";
		public const string ITEM_LAYER = "Item";

		//���Ԃɂł���NPC�̏����
		public const int FELLOWS_MAXNUM = 3;

		//A-star�A���S���Y���̍ő�v�Z(���[�v)��
		public const int ASTAR_LOOPNUM = 3;

		//�G���o������^�[����
		public const int NOMALENEMY_APPEARANCE_MINTURN = 10;
		public const int NOMALENEMY_APPEARANCE_MAXTURN = 20;

		//�s�v�c�̃_���W�����n�}�b�v(�ʏ�)�̃T�C�Y
		//����
		public const int MYSTERYMAP_WHITH = 40;
		//�c��
		public const int MYSTERYMAP_HEIGHT = 35;

		//�s�v�c�̃_���W�����n�}�b�v(�{�X��)�̃T�C�Y
		//����
		public const int MYSTERYBOSSMAP_WHITH = 15;
		//�c��
		public const int MYSTERYBOSSMAP_HEIGHT = 10;

		//�s�v�c�̃_���W�����n�}�b�v�̃^�C�����
		public const int MOVABLE = 1;
		public const int AISLE = 2;
		public const int WALL = 3;

		//�s�v�c�̃_���W�����n�}�b�v�̃G���A������
		public const int AREA_MIN_NUM = 5;
		public const int AREA_MAX_NUM = 8;

		//�s�v�c�̃_���W�����n�}�b�v�̃G���A�������Z�b�g��
		public const int AREA_SPLIT_MAXNUM = 10;

		//�s�v�c�̃_���W�����n�}�b�v�̕����G���A�̍ŏ��ʐ�
		public const int MIN_AREA = 9;

		//�s�v�c�̃_���W�����n�}�b�v�̃^�C���̐؂�ւ����̊K��(MAPTILE_CHANGE_STAIRS�K���ƂɃ}�b�v���؂�ւ��)
		public const int MAPTILE_CHANGE_STAIRS = 1;

		//�s�v�c�̃_���W�����n�}�b�v�̃A�N�Z���g�p�̊O�ǂ̔z�u�m��(%)
		public const int WALL_LOTTERY_PARAM = 10;

		//�s�v�c�̃_���W�����n�}�b�v�̃����X�^�[�n�E�X�̏o���m��(%)
		public const int MONSTERHOUSE_PROBABILITY = 5;
	}

	//�A�C�e���̃^�C�v
	public enum ItemTypeEnum
	{
		Consume,
		Equipment
	}
}