using System.Collections;
using UnityEngine;

public class ClownWanderState : ClownBaseState
{
    public override void EnterState(ClownStateManager clown)
    {
        Debug.Log("The clown is in the wander state");

        //sets the clown to be able to wander
        clown.clownCanWander = true;

      
        SoundManager.instance.PlaySoundFXClipAndAttachToGameObject(SoundManager.instance.soundFXObject, SoundManager.instance.clownApperanceClip, clown.transform, true, 1f, 0.3f, clown.gameObject);

        clown.StartCoroutine(CountdownTimer(clown));
        clown.StartCoroutine(Wander(clown));
    }

    public override void UpdateState(ClownStateManager clown)
    {
        return;
    }


    public IEnumerator CountdownTimer(ClownStateManager clown)
    {
        int timeToWait = Random.Range(5, 20);

        yield return new WaitForSeconds(timeToWait);

        Debug.Log("Stop wandering and chase the player");

        clown.StopCoroutine(Wander(clown));
        clown.clownCanWander = false;

        clown.currentTarget = clown.PlayerRef.gameObject;

        clown.SwitchState(clown.chasePlayerState);

        yield break;



    }


    public IEnumerator Wander(ClownStateManager clown)
    {
        //if the clown is able to wander
        if (!clown.clownCanWander)
        {
            yield break; //exit the coroutine if the clown is not able to wander
        }

        

        // Get a random waypoint from the waypoints array
        int randomIndex = Random.Range(0, clown.waypoints.Length);

        // Set the current target to the random waypoint
        clown.currentTarget = clown.waypoints[randomIndex].gameObject;

        // Move the clown to the random waypoint
        clown.Agent.SetDestination(clown.currentTarget.transform.position);

        // Wait until the clown reaches the destination
        yield return new WaitUntil(() => clown.Agent.remainingDistance <= clown.Agent.stoppingDistance);

        // Wait for a short duration before wandering again
        yield return new WaitForSeconds(Random.Range(2f, 5f));

        //call the Wander coroutine again to continue wandering
        clown.StartCoroutine(Wander(clown));

        yield break;
       
    }
}
