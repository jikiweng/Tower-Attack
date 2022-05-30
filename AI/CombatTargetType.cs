namespace TowerAttack.AI
{
    //CombatTargetType is used as an identification when attacking.
    //The CombatTargetType attach in CombatTarget is the type of the soldier.
    //The CombatTargetType attach in SoldierPiece is the type of who this soldier should attack.
    public enum CombatTargetType
    {
        Enemy, Friend, Castle
    }
}
