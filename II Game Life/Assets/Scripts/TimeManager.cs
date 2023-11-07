using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour, IObservable<Time>
{
    private List<IObserver<Time>> observers;
    // Start is called before the first frame update
    void Start()
    {
        observers = new List<IObserver<Time>>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator Tick()
    {
        yield return new WaitForSeconds(0.5f);
    }

    public void Subscribe(IObserver<Time> observer)
    {
        observers.Add(observer);
    }

    public void UnSubscribe(IObserver<Time> observer)
    {
        observers.Remove(observer);
    }

    public void Notify(Time data)
    {
        foreach (IObserver<Time> observer in observers)
        {
            observer.Update(data);
        }
    }
}
public class Time
{
    private int time;
    Time(int time)
    {
        this.time = time;
    }
}
interface IObservable<TypeDefinition>
{
    void Subscribe(IObserver<TypeDefinition> observer);
    void UnSubscribe(IObserver<TypeDefinition> observer);
    void Notify(TypeDefinition data);
}
