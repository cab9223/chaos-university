using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chaos_University
{
    public enum GameState
    {
        Intro,
        Title,
        Playing,
        LevelComp,
        LevelFail,
        GameOver,
        Credits
    }

    public enum PieceState
    {
        Indicator,
        Floor,
        North,
        East,
        South,
        West,
        Wall,
        Goal,
        Collect,
        SpecialNorth,
        SpecialEast,
        SpecialSouth,
        SpecialWest,
        OffNorth,
        OffEast,
        OffSouth,
        OffWest
    }

    public enum CollisionState
    {
        Proceed,
        Fail,
        Goal
    }
}
