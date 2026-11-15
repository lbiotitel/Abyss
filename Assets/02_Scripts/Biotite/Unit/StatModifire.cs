//스탯의 모디파이어를 정의하는 구조체
public struct StatModifire
{
    private float value;
    private EStatModeType statModeType;
    private object resource;


    public StatModifire(float value, EStatModeType statModeType, object resource)
    {
        this.value = value;
        this.statModeType = statModeType;
        this.resource = resource;
    }

    public float Value => value;
    public EStatModeType StatModeType => statModeType;
    public object Resource => resource;
}
