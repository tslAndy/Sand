public struct Logic
{
    private Point[] field;
    private Extents extents;

    private const int GRAVITY = 4;
    private const int LIQUID_SPREAD = 10;

    public Logic(Point[] field, Extents extents)
    {
        this.field = field;
        this.extents = extents;
    }

    public bool Update(int x, int y)
    {
        Point point = this[x, y];
        if (point.isUpdated)
        {
            extents.Push(x, y);
            return false;
        }
        return point.type switch
        {
            PType.Sand => UpdateSand(x, y),
            PType.Water => UpdateWater(x, y),
            PType.Oil => UpdateOil(x, y),
            PType.Acid => UpdateAcid(x, y),
            PType.Ice => UpdateIce(x, y),
            PType.Stone => UpdateStone(x, y),
            PType.Gas => UpdateGas(x, y),
            PType.Cloner => UpdateCloner(x, y),
            PType.Smoke => UpdateSmoke(x, y),
            PType.Fire => UpdateFire(x, y),
            PType.Ignite => UpdateIgnite(x, y),
            PType.Lava => UpdateLava(x, y),
            _ => false
        };
    }

    private bool UpdateSand(int x, int y)
    {
        Point point = this[x, y];
        point.isUpdated = true;

        int rx = GetRX();
        int fx = x, fy = y;
        for (int i = 0; i < GRAVITY; i++)
        {
            if (PTypeComb.Liquids.CheckFlag(this[fx, fy - 1].type))
            {
                this[fx, fy] = this[fx, fy - 1];
                fy--;
            }
            else if (PTypeComb.Liquids.CheckFlag(this[fx + rx, fy - 1].type) ||
                        PTypeComb.Liquids.CheckFlag(this[fx + (rx = -rx), fy - 1].type))
            {
                this[fx, fy] = this[fx + rx, fy - 1];
                fx += rx;
                fy--;
            }
            else
            {
                break;
            }
        }

        this[fx, fy] = point;
        return fx != x || fy != y;
    }

    private bool UpdateWater(int x, int y)
    {
        Point point = this[x, y];
        point.isUpdated = true;

        bool freeFalling = true;
        int rx = GetRX();
        int fx = x, fy = y;
        for (int i = 0; i < GRAVITY; i++)
        {
            // если то что снизу легче воды
            if (this[fx, fy - 1].type < PType.Water)
            {
                this[fx, fy] = this[fx, fy - 1];
                fy--;
            }
            // если то что снизу по диагонали легче
            else if (this[fx + rx, fy - 1].type < PType.Water || this[fx + (rx = -rx), fy - 1].type < PType.Water)
            {
                this[fx, fy] = this[fx + rx, fy - 1];
                fx += rx;
                fy--;
            }
            else
            {
                freeFalling = false;
                break;
            }
        }

        if (freeFalling)
        {
            this[fx, fy] = point;
            return fx != x || fy != y;
        }

        for (int i = 0; i < LIQUID_SPREAD; i++)
        {
            // если клетка снизу вода или тяжелее, а так же можем сдвинуть клетку со стороны
            if (this[fx, fy - 1].type >= PType.Water &&
                    (this[fx + rx, fy].type < PType.Water || this[fx + (rx = -rx), fy].type < PType.Water))
            {
                this[fx, fy] = this[fx + rx, fy];
                fx += rx;
            }
            else
                break;
        }

        this[fx, fy] = point;
        return fx != x || fy != y;
    }

    private bool UpdateGas(int x, int y)
    {
        Point point = this[x, y];
        point.isUpdated = true;

        bool freeFalling = true;
        int rx = GetRX();
        int fx = x, fy = y;
        for (int i = 0; i < GRAVITY; i++)
        {
            if (this[fx, fy + 1].type < PType.Gas)
            {
                this[fx, fy] = this[fx, fy + 1];
                fy++;
            }
            else if (this[fx + rx, fy + 1].type < PType.Gas || this[fx + (rx = -rx), fy + 1].type < PType.Gas)
            {
                this[fx, fy] = this[fx + rx, fy + 1];
                fx += rx;
                fy++;
            }
            else
            {
                freeFalling = false;
                break;
            }
        }

        if (freeFalling)
        {
            this[fx, fy] = point;
            return fx != x || fy != y;
        }

        for (int i = 0; i < LIQUID_SPREAD; i++)
        {
            if (this[fx, fy + 1].type >= PType.Gas &&
                    (this[fx + rx, fy].type < PType.Gas || this[fx + (rx = -rx), fy].type < PType.Gas))
            {
                this[fx, fy] = this[fx + rx, fy];
                fx += rx;
            }
            else
                break;
        }

        this[fx, fy] = point;
        return fx != x || fy != y;
    }

    private bool UpdateOil(int x, int y)
    {
        Point point = this[x, y];
        point.isUpdated = true;

        bool freeFalling = true;
        int rx = GetRX();
        int fx = x, fy = y;
        for (int i = 0; i < GRAVITY; i++)
        {
            // если то что снизу легче воды
            if (this[fx, fy - 1].type < PType.Oil)
            {
                this[fx, fy] = this[fx, fy - 1];
                fy--;
            }
            // если то что снизу по диагонали легче
            else if (this[fx + rx, fy - 1].type < PType.Oil || this[fx + (rx = -rx), fy - 1].type < PType.Oil)
            {
                this[fx, fy] = this[fx + rx, fy - 1];
                fx += rx;
                fy--;
            }
            else
            {
                freeFalling = false;
                break;
            }
        }

        if (freeFalling)
        {
            this[fx, fy] = point;
            return fx != x || fy != y;
        }

        for (int i = 0; i < LIQUID_SPREAD; i++)
        {
            // если клетка снизу вода или тяжелее, а так же можем сдвинуть клетку со стороны
            if (this[fx, fy - 1].type >= PType.Oil &&
                    (this[fx + rx, fy].type < PType.Oil || this[fx + (rx = -rx), fy].type < PType.Oil))
            {
                this[fx, fy] = this[fx + rx, fy];
                fx += rx;
            }
            else
                break;
        }

        this[fx, fy] = point;
        return fx != x || fy != y;
    }

    private bool UpdateAcid(int x, int y)
    {
        Point point = this[x, y];
        point.isUpdated = true;

        this[x, y] = default;

        bool freeFalling = true;
        int rx = GetRX();
        int fx = x, fy = y;
        for (int i = 0; i < GRAVITY; i++)
        {
            if (this[fx, fy - 1].type == PType.Empty)
            {
                fy--;
            }
            else if (PTypeComb.DestroyableByAcid.CheckFlag(this[fx, fy - 1].type))
            {
                this[fx, fy - 1] = default;
                return true;
            }
            else if (PTypeComb.DestroyableByAcid.CheckFlag(this[fx + rx, fy].type) ||
                    PTypeComb.DestroyableByAcid.CheckFlag(this[fx + (rx = -rx), fy].type))
            {
                this[fx + rx, fy] = default;
                return true;
            }
            else if (this[fx + rx, fy - 1].type == PType.Empty ||
                    this[fx + (rx = -rx), fy - 1].type == PType.Empty)
            {
                fy--;
                fx += rx;
            }
            else
            {
                freeFalling = false;
                break;
            }
        }

        if (freeFalling)
        {
            this[fx, fy] = point;
            return fx != x || fy != y;
        }

        for (int i = 0; i < LIQUID_SPREAD; i++)
        {
            if ((this[fx, fy - 1].type == PType.Wall || this[fx, fy - 1].type == PType.Acid) &&
                (this[fx + rx, fy].type == PType.Empty || this[fx + (rx = -rx), fy].type == PType.Empty))
                fx += rx;
            else
                break;
        }

        this[fx, fy] = point;
        return fx != x || fy != y;
    }

    private bool UpdateIce(int x, int y)
    {
        Point point = this[x, y];
        point.isUpdated = true;
        point.lifetime = Math.Min(point.lifetime + FpsCounter.DT, 4f);
        this[x, y] = point;

        if (point.lifetime < 0.05f)
            return true;

        int dx = GetRX();
        int dy = GetRX();

        if (this[x + dx, y + dy].type == PType.Water)
            this[x + dx, y + dy] = new Point { type = PType.Ice };

        return point.lifetime < 4f;
    }

    private bool UpdateStone(int x, int y)
    {
        Point point = this[x, y];
        point.isUpdated = true;

        int fx = x, fy = y;
        for (int i = 0; i < GRAVITY; i++)
        {
            if (PTypeComb.Liquids.CheckFlag(this[fx, fy - 1].type))
            {
                this[fx, fy] = this[fx, fy - 1];
                fy--;
            }
            else
            {
                break;
            }
        }

        this[fx, fy] = point;
        return fx != x || fy != y;
    }

    private bool UpdateCloner(int x, int y)
    {
        Point point = this[x, y];
        point.isUpdated = true;
        point.lifetime = Math.Min(point.lifetime + FpsCounter.DT, 4f);
        this[x, y] = point;

        if (point.lifetime < 0.1f)
            return true;

        int rx = GetRX();
        int ry = GetRX();

        Point temp = this[x + rx, y + ry];
        if (point.data == 0)
        {
            if (PTypeComb.Cloneable.CheckFlag(temp.type))
            {
                point.data = (byte)temp.type;
                this[x, y] = point;
                return true;
            }
            if (temp.type == PType.Cloner && temp.data != 0)
            {
                point.data = temp.data;
                this[x, y] = point;
                return true;
            }
        }
        else if (temp.type == PType.Empty)
        {
            this[x + rx, y + ry] = new Point { type = (PType)point.data };
            point.lifetime = 0f;
            this[x, y] = point;
            return true;
        }

        return point.lifetime < 4f;
    }

    private bool UpdateSmoke(int x, int y)
    {
        Point point = this[x, y];
        point.isUpdated = true;
        point.lifetime += FpsCounter.DT;
        if (point.lifetime > 0.3f + Random.Shared.NextSingle() * 0.2f)
        {
            this[x, y] = default;
            return true;
        }

        this[x, y] = default;

        bool freeFalling = true;
        int rx = GetRX();
        int fx = x, fy = y;
        for (int i = 0; i < GRAVITY; i++)
        {
            if (Random.Shared.NextSingle() < 0.1f &&
                (this[fx + rx, fy].type == PType.Empty || this[fx + (rx = -rx), fy].type == PType.Empty))
            {
                fx += rx;
                continue;
            }

            if (this[fx, fy + 1].type < PType.Smoke)
            {
                this[fx, fy] = this[fx, fy + 1];
                fy++;
            }
            else if (this[fx + rx, fy + 1].type < PType.Smoke || this[fx + (rx = -rx), fy + 1].type < PType.Smoke)
            {
                this[fx, fy] = this[fx + rx, fy + 1];
                fx += rx;
                fy++;
            }
            else
            {
                freeFalling = false;
                break;
            }
        }

        if (freeFalling)
        {
            this[fx, fy] = point;
            return true;
        }

        for (int i = 0; i < LIQUID_SPREAD; i++)
        {
            if (this[fx, fy + 1].type >= PType.Smoke &&
                    (this[fx + rx, fy].type < PType.Smoke || this[fx + (rx = -rx), fy].type < PType.Smoke))
            {
                this[fx, fy] = this[fx + rx, fy];
                fx += rx;
            }
            else
                break;
        }

        this[fx, fy] = point;
        return true;
    }

    private bool UpdateFire(int x, int y)
    {
        Point point = this[x, y];
        point.isUpdated = true;
        point.lifetime += FpsCounter.DT;
        if (point.lifetime > 0.3f + Random.Shared.NextSingle() * 0.3f)
        {
            this[x, y] = new Point { type = PType.Smoke };
            return true;
        }

        this[x, y] = default;

        int rx = GetRX();
        bool freeFalling = true;
        int fx = x, fy = y;
        for (int i = 0; i < GRAVITY; i++)
        {
            if (PTypeComb.Ignitable.CheckFlag(this[fx, fy + 1].type))
            {
                this[fx, fy + 1] = new Point { type = PType.Ignite };
                return true;
            }
            if (PTypeComb.Ignitable.CheckFlag(this[fx + rx, fy + 1].type) ||
                PTypeComb.Ignitable.CheckFlag(this[fx + (rx = -rx), fy + 1].type))
            {
                this[fx, fy + 1] = new Point { type = PType.Ignite };
                return true;
            }

            if (Random.Shared.NextSingle() < 0.1f && this[fx + rx, fy].type == PType.Empty)
            {
                fx += rx;
                continue;
            }

            if (this[fx, fy + 1].type == PType.Empty ||
                    this[fx, fy + 1].type == PType.Smoke)
            {
                fy++;
            }
            else if (this[fx + rx, fy + 1].type == PType.Empty ||
                    this[fx + rx, fy + 1].type == PType.Smoke ||
                    this[fx + (rx = -rx), fy + 1].type == PType.Empty ||
                    this[fx + (rx = -rx), fy + 1].type == PType.Smoke)
            {
                fy++;
                fx += rx;
            }
            else
            {
                freeFalling = false;
                break;
            }
        }

        if (freeFalling)
        {
            this[fx, fy] = point;
            return true;
        }

        for (int i = 0; i < LIQUID_SPREAD; i++)
        {
            if ((!PTypeComb.Ignitable.CheckFlag(this[fx, fy + 1].type)) &&
                (this[fx + rx, fy].type == PType.Empty || this[fx + (rx = -rx), fy].type == PType.Empty))
            {
                fx += rx;
            }
            else
                break;
        }

        this[fx, fy] = point;
        return true;
    }

    private bool UpdateIgnite(int x, int y)
    {
        Point point = this[x, y];
        point.isUpdated = true;
        point.lifetime += FpsCounter.DT;
        if (point.lifetime > 0.2f)
        {
            this[x, y] = new Point { type = PType.Fire };
            return true;
        }
        this[x, y] = point;

        if (point.lifetime < 0.05f)
            return true;

        int rx = GetRX();
        int ry = GetRX();
        if (PTypeComb.Ignitable.CheckFlag(this[x + rx, y + ry].type))
            this[x + rx, y + ry] = new Point { type = PType.Ignite };
        return true;
    }

    private bool UpdateLava(int x, int y)
    {
        Point point = this[x, y];
        point.isUpdated = true;

        this[x, y] = default;

        bool freeFalling = true;
        int rx = GetRX();
        int fx = x, fy = y;
        for (int i = 0; i < GRAVITY; i++)
        {
            if (PTypeComb.Ignitable.CheckFlag(this[fx, fy - 1].type))
            {
                this[fx, fy - 1] = new Point { type = PType.Ignite };
            }
            else if (PTypeComb.Ignitable.CheckFlag(this[fx + rx, fy].type) ||
                    PTypeComb.Ignitable.CheckFlag(this[fx + (rx = -rx), fy].type))
            {
                this[fx + rx, fy] = new Point { type = PType.Ignite };
            }

            if (this[fx, fy - 1].type == PType.Empty)
            {
                fy--;
            }
            else if (this[fx + rx, fy - 1].type == PType.Empty ||
                    this[fx + (rx = -rx), fy - 1].type == PType.Empty)
            {
                fy--;
                fx += rx;
            }
            else
            {
                freeFalling = false;
                break;
            }
        }

        if (freeFalling)
        {
            this[fx, fy] = point;
            return fx != x || fy != y;
        }

        for (int i = 0; i < LIQUID_SPREAD; i++)
        {
            if ((this[fx, fy - 1].type == PType.Wall || this[fx, fy - 1].type == PType.Lava) &&
                (this[fx + rx, fy].type == PType.Empty || this[fx + (rx = -rx), fy].type == PType.Empty))
                fx += rx;
            else
                break;
        }

        this[fx, fy] = point;
        return fx != x || fy != y;
    }

    private int GetRX() => Random.Shared.Next(0, 2) * 2 - 1;

    public Point this[int x, int y]
    {
        get => field[y * 1024 + x];
        set
        {
            field[y * 1024 + x] = value;
            extents.Push(x, y);
        }
    }
}
