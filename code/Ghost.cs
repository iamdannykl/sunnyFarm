namespace SunnyFarm.code;

public partial class Ghost : enemyBase
{
    public override void attacked(float trueDamage,baseGun gun)
    {
        base.attacked(trueDamage,gun);
        Hp -= trueDamage;
    }
}