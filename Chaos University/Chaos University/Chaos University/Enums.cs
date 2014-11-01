using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chaos_University
{
    public enum GameState
    {
        Title,
        Menus,
        PlacingTiles,
        Playing,
        GameOver
    }

    public enum PieceState
    {
        Floor,
        North,
        East,
        South,
        West,
        Wall
    }
}
