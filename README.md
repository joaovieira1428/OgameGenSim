# What OGameGenSim is?
What OGameGenSim is, a "different" combat simulator for ogame, no, it does not use genetic algorithms (although it was the original ideia), but who knows, it might still go that route.
So, what is so different about this simulator. Well, it runs only on a command line on your computer, so just an executable. You can download the latest version from the releases menu. You also don't have to manually put or adjust any fleet values, just insert your fleet API, the defender report ID, fleet compositions that you might want to simulate and the simulator will give you an answer.
For now, that answer will depend on your chosen options, but the one that is not so obvious is that the simulation will divide your fleet in 5 to see how many ships you actually need to send to get a good profit, it will not choose the one with most profit because that would always be the most fleet sent.

Here's a guide with some print screens to help you get your first simulations and explain how some of the options work.

# Guide
## Navigation
Since it's on a terminal, and I didn't add support to use a mouse (might come in the future), We are to use the keyboard, it will work like it would on a browser, the ***tab*** key will navigate through the menu components and the ***enter*** key will select components. When the textboxes are selected, just write, or paste the text like normal.
That's it.

## Main Menu
<img width="832" height="181" alt="image" src="https://github.com/user-attachments/assets/272f5498-9949-4713-bf5e-a6b9accf8257" />

For now there's only one option available, planning to add the rest of the features in the future.

## Battle Simulator

### Number of players
<img width="821" height="255" alt="image" src="https://github.com/user-attachments/assets/61c07647-0f2b-4388-b67c-8c9ae9747e49" />

Here you choose the number of attackers and defenders you want to simulatte against. 
More than one will be AC-Attack and/or AC-Defend

### API (1) - Main Players
<img width="837" height="228" alt="image" src="https://github.com/user-attachments/assets/686ebf86-23c5-4499-aeb9-97fa2910d80c" />

This menu is to enter the main players. On the attacker it would be the yourself or the AC owner. The defender it would be the planet or moon you want to attack.
What to put on the boxes? The Attacker API, and the Defender Report ID.

<img width="810" height="955" alt="image" src="https://github.com/user-attachments/assets/8e6d14f6-1230-48b1-8edb-e0fa978f47c6" />

### API (2) - ACS
<img width="831" height="224" alt="image" src="https://github.com/user-attachments/assets/6b7ce923-5bda-4654-a8f2-694140648561" />

Almost the same as before, the only difference is that here it's the rest of the players, your mates (AC-attack) and/or your enemy's mates (AC-Defend).
> This page will be ignored if it's just you and your enemy

### Attacker Fleet Composition (1)
<img width="827" height="231" alt="image" src="https://github.com/user-attachments/assets/3e5c75cb-24ea-4d2b-b266-2bf2678573fe" />

Now we are entering on the simulator meat.
In here you will choose your fleet compositions:
- RIPS -> Death Star
- Slow Fleet -> Destroyer and Bomber
- Fast Fleet -> Cruisers, Battleships and Battlecruisers
- Fodder (fighters) -> Light fighters and heavy fighters
- Fodder (probes) -> Just Probes
- Reapers -> Just Reaoers
- Do all combinations -> Will try all combinations of the chosen fleet composition options and choose the best

> Be aware of the Do all combinations option, That one can take a while depending on how many fleet compositions you chose, as with all options enabled, it will run 315 simulations.
 
<img width="849" height="259" alt="image" src="https://github.com/user-attachments/assets/3b8e9fac-a0ee-4aaf-a8c0-2a610f695aa2" />

> When all combinations is enabled, another option will be available, if you choose it it will take into account the speed of the ships in it's decision (I don't know how well it will work in real game experinces).

### Attacker Fleet Composition (2)
<img width="842" height="308" alt="image" src="https://github.com/user-attachments/assets/8766b008-3f7a-4277-80c4-f180e962639d" />

<img width="816" height="482" alt="image" src="https://github.com/user-attachments/assets/40ed08e1-86b4-4ee6-94a1-88957af14da9" />

In this menu you can choose the cargo fleet options, in case of choosing the *Steal Only* optionm it will select only enough ships (+20% for losses) of the chosen type to steal the resources.

You can also just choose to send all your cargos/pathfinders on the options below.

> Be aware of the warnings as they are important.
> Also, the Number of units to send textbox is not working for now. Gonna correct it as soon as I can.

### Defender Fleet Composition
<img width="822" height="157" alt="image" src="https://github.com/user-attachments/assets/84c5fcc0-55b6-41ee-a3b2-aee3bc9e3581" />

When the "Same as the attacker fleet composition" is enabled the simulation will choose the exact same options to run against.
When not, we must choose what the defenders might send to the planet/moon you are attacking.

> As the warning says, it only counts for the ACS-Defend as in the main defender you wan't to catch what you spied.
> Also, the all combinations options does not count for the defenders as we don't know what he might send, we only expect the worse wich is all his fleet (considering the chosen fleet compositions of course).
> This page also gets ignored if AC-Defenders don't exist.

 ### Battle Result
 <img width="1771" height="994" alt="image" src="https://github.com/user-attachments/assets/511d5ac0-8706-481d-9e65-20aab7f4136f" />
Here is the Battle result, not a lot to say, it's all the stats you might want to see.
You can also select between the players and rounds if you want.

From here you go to the Main Menu or to the Next Simulation.
