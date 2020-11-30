using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IStateControlled
{
    //common
    public Player_Main main;
    public Player_Input input;
    public Rigidbody rid;
    public Player_Variables var;

    private StateMachine state = new StateMachine();
    public List<IPlayer> p_setup = new List<IPlayer>();


    void Awake()
    {
        rid = GetComponent<Rigidbody>(); //gets rigidbody componet
        input = GetComponent<Player_Input>(); //gets input componet
        var = GetComponent<Player_Variables>();
        main = GetComponentInChildren<Player_Main>();
        
        main.GetFinalStats();

        p_setup.AddRange(GetComponents<IPlayer>());
        for (int i = 0; i < p_setup.Count; i++) { p_setup[i].Setup(this); }
        ChangeState("normal");
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < p_setup.Count; i++) { p_setup[i].P_Update(); }

        state.Update();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        for (int i = 0; i < p_setup.Count; i++) { p_setup[i].P_FixedUpdate(); }

        state.FixedUpdate();
    }


    //Sets state of statemachine
    public void ChangeState(IState RequestedState)
    {
        state.SetState(RequestedState);
    }
    public void ChangeState(string RequestedStateKey)
    {
        IState set = null;
        if (state.states.TryGetValue(RequestedStateKey, out set))
        {
            state.SetState(set);
            return;
        }
        Dev.LogError("Set State By Key('String'): Failed");
    }
    //Gets an "IState" from the statemachine
    public IState GetState()
    {
        return state.CurrentState();
    }
    //Adds a state to dictionary of states in state machine
    public void AddState(string stateName, IState state) // state name should be name of the State' class name but only lowercase and no space/underscore
    {
        this.state.AddState(stateName, state);
    }
}




public interface IPlayer
{
    void Setup(Player p);
    void P_Update();

    void P_FixedUpdate();
}


public interface IState // Contract for State
{
    void OnEnter(IState prevState); //Start State
    void OnUpdate(); //Update
    void OnFixedUpdate(); //FixedUpdate
    void OnExit(IState nextState); //Leave State

    /* --- STATE TEMPLATE ---
     public class STATE_State: IState
    {
        Player_Controller player;
        public STATE_State(Player_Controller player)
        {
            this.player = player;
        }

        public void OnEnter(IState lastState) //Start State
        {
            
        }
        public void OnUpdate() //Update
        {

        }
        public void OnFixedUpdate() //FixedUpdate
        {

        }
        public void OnExit(IState nextState) //Leave State
        {

        }
    }
    */
}
public interface IStateControlled
{
    void ChangeState(IState RequestedState);
}
public class StateMachine // State Manager
{
    private IState currentState;

    public Dictionary<string, IState> states = new Dictionary<string, IState>();
    public void SetState(IState nextState)
    {
        if (currentState != null)
            currentState.OnExit(nextState);

        nextState.OnEnter(currentState);
        currentState = nextState;
    }

    public IState CurrentState()
    {
        return currentState;
    }

    public void AddState(string stateName, IState state)
    {
        states.Add(stateName, state);
    }

    public void Update()
    {
        currentState.OnUpdate();

    }

    public void FixedUpdate()
    {
        currentState.OnFixedUpdate();

    }
}
