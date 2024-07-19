

public class JackInTheBoxDisabledState : JackInTheBoxBaseState
{
    public override void EnterState(JackInTheBoxStateManager jackInTheBox)
    {
        SoundFXManager.instance.StopSoundFXClip(SoundFXManager.instance.musicBoxLoopClip);
    }

    public override void UpdateState(JackInTheBoxStateManager jackIntheBox)
    {
        return;
    }

    
}
