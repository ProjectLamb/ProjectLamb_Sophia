
#Visual

private CancellationTokenSource cts;

public Material materialRef { get; private set; }
public VisualFXObject visualFxRef { get; private set; }


#TickDamage

public float TickDamage { get; private set; }
public float TickDamageRatio { get; private set; }
public float IntervalTime { get; private set; }

#Temporary Modifier
public StatModifier MoveSpeedModifier;                        

#Physics

public float ForceAmount { get; private set; }