public enum PTypeComb : long
{
    Empty = 1L << PType.Empty,
    Wall = 1L << PType.Wall,
    Ice = 1L << PType.Ice,
    Sand = 1L << PType.Sand,
    Water = 1L << PType.Water,
    Oil = 1L << PType.Oil,
    Acid = 1L << PType.Acid,
    Stone = 1L << PType.Stone,
    Wood = 1L << PType.Wood,
    Cloner = 1L << PType.Cloner,
    Gas = 1L << PType.Gas,
    Smoke = 1L << PType.Smoke,
    Lava = 1L << PType.Lava,

    Liquids = Empty | Water | Oil | Acid | Lava | Gas | Smoke,
    DestroyableByAcid = Sand | Water | Oil | Ice | Stone | Wood | Cloner | Lava,
    Cloneable = Ice | Sand | Sand | Water | Oil | Acid | Stone | Wood | Gas | Smoke,
    Ignitable = Gas | Oil | Wood
}
