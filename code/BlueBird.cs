public partial class BlueBird : enemyBase
{
    public override void attacked(float trueDamage)
    {
        base.attacked(trueDamage);
        Hp -= trueDamage;
    }
}
