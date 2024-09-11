public class DamageInfo
{
    public DamageType damageType {  get; private set; }
    public float amount { get; private set; }

    public DamageInfo(DamageType damageType, float amount)
    {
        this.damageType = damageType;
        this.amount = amount;
    }

}
