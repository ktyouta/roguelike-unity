using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LayerUtil
{
    public static LayerMask playerLayer = LayerMask.GetMask(Define.PLAYER_LAYER);
    public static LayerMask enemyLayer = LayerMask.GetMask(Define.ENEMY_LAYER);
    public static LayerMask treasureLayer = LayerMask.GetMask(Define.TREASURE_LAYER);
    public static LayerMask blockingLayer = LayerMask.GetMask(Define.BLOCKING_LAYER);
    public static LayerMask npcLayer = LayerMask.GetMask(Define.NPC_LAYER);
    public static LayerMask npcFellowLayer = LayerMask.GetMask(Define.NPCFELLOW_LAYER);
    public static LayerMask itemLayer = LayerMask.GetMask(Define.ITEM_LAYER);
}
