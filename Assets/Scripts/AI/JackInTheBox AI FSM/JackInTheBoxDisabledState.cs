

public class JackInTheBoxDisabledState : JackInTheBoxBaseState
{
    public override void EnterState(JackInTheBoxStateManager jackInTheBox)
    {
        SoundManager.instance.StopSoundFXClip(SoundManager.instance.musicBoxLoopClip);
    }

    public override void UpdateState(JackInTheBoxStateManager jackIntheBox)
    {
        return;
    }

    
}
