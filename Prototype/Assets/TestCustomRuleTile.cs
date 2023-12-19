using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Custom/TestCustomRuleTile")]
public class TestCustomRuleTile : IsometricRuleTile<TestCustomRuleTile.Neighbor> {
    public class Neighbor : IsometricRuleTile.TilingRule.Neighbor {
        public const int Null = 3;
        public const int NotNull = 4;
        //public const int BrickLeftFront = 5;
        //public const int BrickLeftBack = 6;
        //public const int BrickRightFront = 7;
        //public const int BrickRightBack = 8;
    }

    public override bool RuleMatch(int neighbor, TileBase tile) {
        switch (neighbor) {
            case Neighbor.Null: return tile == null;
            case Neighbor.NotNull: return tile != null;
        }

        return base.RuleMatch(neighbor, tile);
    }

    public override bool RuleMatches(TilingRule rule, Vector3Int position, ITilemap tilemap, ref Matrix4x4 transform)
    {
        return base.RuleMatches(rule, position, tilemap, ref transform);
    }

    //private bool IsBrickLeftFront(TileBase tile)
    //{
    //    return false;
    //}
    //private bool IsBrickLeftBack(TileBase tile)
    //{
    //    return false;
    //}
    //private bool IsBrickRightFront(TileBase tile)
    //{
    //    return false;
    //}
    //private bool IsBrickRightBack(TileBase tile)
    //{
    //    return false;
    //}
}

//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Tilemaps;

//[CreateAssetMenu]
//public class TestCustomRuleTile : RuleTile<TestCustomRuleTile.Neighbor> {
//    public bool customField;

//    public class Neighbor : RuleTile.TilingRule.Neighbor {
//        public const int Null = 3;
//        public const int NotNull = 4;
//    }

//    public override bool RuleMatch(int neighbor, TileBase tile) {
//        switch (neighbor) {
//            case Neighbor.Null: return tile == null;
//            case Neighbor.NotNull: return tile != null;
//        }
//        return base.RuleMatch(neighbor, tile);
//    }
//}