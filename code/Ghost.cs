public partial class Ghost : enemyBase
{
    public override void attacked(float trueDamage)
    {
        base.attacked(trueDamage);
        Hp -= trueDamage;
    }
}