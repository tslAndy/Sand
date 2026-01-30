public static class PTypeCombExtensions
{
    public static bool CheckFlag(this PTypeComb comb, PType flag)
    {
        return ((((long)comb) >> (byte)flag) & 1) == 1;
    }
}
