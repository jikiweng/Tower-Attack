namespace TowerAttack.UI
{
    public interface IRaycastable
    {
        CursorType GetCursorType();
        bool HandleRaycast();
    }
}
