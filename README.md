# StateMachineCompiler
State Machine Compiler inspired by Uncle Bob

This is the State Machine Compiler (SMC) that Robert C. Martin builds in episodes 28, 29 and 30 of [Clean Coders](https://cleancoders.com/)

It can be used to create a Finite State Machine in several programming languages, like Java, C, Python, and C#, of course. In fact, it's built on C# 😄

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

Each block 
