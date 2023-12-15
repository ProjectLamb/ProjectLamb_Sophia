using System;
using System.Collections;
using System.Collections.Generic;

/*********************************************************************************
네임 스페이스란.

변수, 함수, 구조체, 클래스등을 서로 구분하기 위한 식별자에 대한
유효 범위를 제공하는 영역으로

using을 통해서 연결할 네임 스페이스를 명시할 수이싿.

*********************************************************************************/

namespace Feature_NewData
{
    public enum EntityType
    {
        Player = 0, Object, Trap, Enemy, Boss
    }
    public enum ChapterType
    {
        오래된_연구소 = 1, 공장
    }

    public enum StageType
    {
        Start = 0, Combat, Boss, Shop, Hidden
    }
}