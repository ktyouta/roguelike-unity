using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MoveActionComponentBase : MonoBehaviour
{
    public class NextMovePositionClass
    {
        public int xDir;
        public int yDir;
    }

    /**
     * �L�����N�^�[�̎��̈ړ��_��ԋp
     */
    public abstract List<NextMovePositionClass> retNextPosition(Vector2 targetPosition);
}
