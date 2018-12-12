# State Machine Compiler
State Machine Compiler inspired by [Robert C. Martin](https://twitter.com/unclebobmartin), AKA "Uncle Bob", and his work.

This the State Machine Compiler (SMC) that Robert C. Martin builds in episodes [28](https://cleancoders.com/episode/clean-code-episode-28/show), [29](https://cleancoders.com/episode/clean-code-episode-29/show) and [30](https://cleancoders.com/episode/clean-code-episode-30/show) of [Clean Coders](https://cleancoders.com/), built from scratch in C#. 

It can be used to create Finite State Machines in several programming languages, like Java, C, Python, and C#, of course. In fact, it's built on C# 😄

# Syntax
```
Actions: Turnstile
FSM: TwoCoinTurnstile
Initial: Locked
{
	(Base)  Reset  Locked  Lock
	Locked : Base 
	{
		Pass  Alarming   -
		Coin  FirstCoin  -
	}
                  
	Alarming : Base  >AlarmOn <AlarmOff 
	{
        	-	-	-
	}
                          
	FirstCoin : Base 
	{
        	Pass	Alarming	-
		Coin	Unlocked	Unlock
	}
                          
	Unlocked : Base 
	{
		Pass	Locked  Lock
		Coin	-	ThankYou
	}
}
```

The syntax is quite easy to understand:

- The first 3 lines are the **header**.
- After the header, there is a block enclosed with braces, this block is the definition of the **transitions** of the Finite State Machine (FSM)
- Each each transition consists of 4 values: `current state, event, next state, actions`
  For instance `dry rain wet { getUmbrella, getCoat }. That means: given the "dry" state, when the `rain` event is received, turn to the `wet` state and execute the `getUmbrella` and `getCoat` actions
- However, the transitions of one state can be grouped under a block like this:
	```
	currentState 
	{ 
		event, state, actions
		event, state, actions
	}
	```
- The hyphen (**-**). It's a placeholder to indicate "nothing" or "unchanged". For instance, if a transition has its `next state` to **-**, it means that the next state is remains the same (the state won't change).

There are some cool features to make it easier to define the states:
- **Base states.** A state can derive from a base state, inheriting the transitions from the base state. It's denoted by `State : Base`
- **Entry and exit actions.** You can define which actions happen every time a state enters or exists.  Entry actions are denoted by `>action` and exit actions are denoted by `<action`
- **Abstract states.** They are states that only exists so other states can derive from it. They don't translate to a real state. They are denoted by `(state)` (the name between parentheses).

# Credits

- [Robert C. Martin](https://twitter.com/unclebobmartin) for his wonderful lessons at [Clean Coders](https://cleancoders.com/)
- [Nicholas Blumhardt](https://github.com/nblumhardt) for the great parser combinator library [Superpower](https://github.com/nblumhardt/superpower)