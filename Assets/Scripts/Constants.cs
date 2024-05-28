public static class Constants
{
    public const CellType SwapWithSand = CellType.Empty |
                                        CellType.Water |
                                        CellType.Gas |
                                        CellType.Oil;

    public const CellType SwapWithWater = CellType.Empty |
                                        CellType.Oil |
                                        CellType.Gas;


    public const CellType SwapWithStone = CellType.Empty |
                                        CellType.Water |
                                        CellType.Gas |
                                        CellType.Fire |
                                        CellType.Acid |
                                        CellType.Oil;

    public const CellType DestroyableByAcid = CellType.Sand |
                                            CellType.Water |
                                            CellType.Oil |
                                            CellType.Wood;

    public const CellType SwapWithSmoke = CellType.Empty |
                                        CellType.Sand |
                                        CellType.Water |
                                        CellType.Gas |
                                        CellType.Fire |
                                        CellType.Acid |
                                        CellType.Oil;

    public const CellType SwapWithGas = CellType.Empty;
    public const CellType SwapWithOil = CellType.Empty | CellType.Gas;
    public const CellType SwapWithAcid = CellType.Empty | CellType.Gas;

    public const CellType SwapWithFire = CellType.Empty;
    public const CellType CanFire = CellType.Oil | CellType.Wood;

    public const CellType CanDetonate = CellType.Gas;
    public const CellType ThrowableByDetonation = CellType.Gas;

}