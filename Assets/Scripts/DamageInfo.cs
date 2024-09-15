public class DamageInfo
{
    public DamageType damageType {  get; private set; }
    public DamageSource damageSource { get; private set; }

    public float amount { get; private set; }
 
    public DamageInfo(DamageType damageType,  DamageSource damageSource, float amount)
    {
        this.damageType = damageType;

        this.damageSource = damageSource;

        this.amount = amount;
    }

}
