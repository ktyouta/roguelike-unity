using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBase : MonoBehaviour
{
    [Header("�}�b�v�I�u�W�F�N�g�ݒu�p���[��")] public int settingFuncRule;
    [Header("�}�b�v�ݒu���̖��x�p�����[�^")] public int denceParam;
    //settingFuncRule��1���Z�b�g�����ۂɗp����p�����[�^
    [Header("�W���̂̍\���v�f���̍ŏ��l")] public int minSettingNum;
    [Header("�W���̂̍\���v�f���̍ő�l")] public int maxSettingNum;
    //settingFuncRule��2���Z�b�g�����ۂɗp����p�����[�^
    [Header("�}�b�v����̍s���Ƃ��̃p�����[�^�̐ς��c�����̒����ɂȂ�(0�`1�͈̔͂Őݒ�)")] public float verticalParam;
    [Header("�}�b�v����̗񐔂Ƃ��̃p�����[�^�̐ς��������̒����ɂȂ�(0�`1�͈̔͂Őݒ�)")] public float horizontalParam;

    [Header("�}�b�v�̂P��敪�̉��������̃p�����[�^�Ŋ������l��(�W���̂�)�ݒu���ɂȂ�")] public int objectSettingNum;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
