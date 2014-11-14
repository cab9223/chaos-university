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
        Menus,
        Playing,
        LevelComp,
        GameOver
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
        Collect
    }
}
