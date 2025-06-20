namespace SunnyFarm.code;

public partial class BlueBird : enemyBase
{
    public override void attacked(float trueDamage,baseGun gun)
    {
        base.attacked(trueDamage,gun);
        Hp -= trueDamage;
    }
}